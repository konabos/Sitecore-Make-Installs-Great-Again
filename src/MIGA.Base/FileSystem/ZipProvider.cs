using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;
using Sitecore.Diagnostics.Base;
using JetBrains.Annotations;

namespace MIGA.FileSystem
{
  using Sitecore.Diagnostics.Logging;
  using MIGA.Extensions;

  public class ZipProvider
  {
    #region Constants

    private const long Kb = 1024;
    private const long Mb = 1024 * Kb;

    #endregion

    #region Fields

    private FileSystem FileSystem { get; }

    #endregion

    #region Constructors

    public ZipProvider(FileSystem fileSystem)
    {
      FileSystem = fileSystem;
    }

    #endregion

    #region Public methods

    public virtual void CheckZip([NotNull] string packagePath)
    {
      Assert.ArgumentNotNullOrEmpty(packagePath, nameof(packagePath));

      if (System.IO.File.Exists(packagePath))
      {
        try
        {
          using (ZipFile zip = new ZipFile(packagePath))
          {
            bool data = zip.Entries.FirstOrDefault(e => e.IsDirectory && e.FileName.Contains("/Data/")) != null;
            bool databases = zip.Entries.FirstOrDefault(e => e.IsDirectory && e.FileName.Contains("/Databases/")) !=
                             null;
            bool website = zip.Entries.FirstOrDefault(e => e.IsDirectory && e.FileName.Contains("/Website/")) != null;
            if (!(data && databases && website))
            {
              throw new InvalidOperationException(
                $"The \"{packagePath}\" archive isn't a Sitecore installation package.");
            }
          }
        }
        catch (ZipException)
        {
          throw new InvalidOperationException($"The \"{packagePath}\" installation package seems to be corrupted.");
        }
      }
    }

    public virtual void CreateZip(string path, string zipFileName, string ignore = null, int compressionLevel = 0)
    {
      CompressionLevel zipCompressionLevel;
      if (typeof(CompressionLevel).IsEnumDefined(compressionLevel))
      {
        zipCompressionLevel = (CompressionLevel)compressionLevel;
      }
      else
      {
        zipCompressionLevel = CompressionLevel.None;
      }
      var zip = new ZipFile
      {
        CompressionLevel = zipCompressionLevel,
        UseZip64WhenSaving = Zip64Option.AsNecessary
      };
      if (ignore == null)
      {
        zip.AddDirectory(path); // , relativePath);
      }
      else
      {
        Assert.IsTrue(!ignore.Contains('\\') && !ignore.Contains('/'), "Multi-level ignore is not supported for archiving");
        foreach (var directory in Directory.GetDirectories(path))
        {
          var directoryName = new DirectoryInfo(directory).Name;
          if (!directoryName.EqualsIgnoreCase(ignore))
          {
            zip.AddDirectory(directory, directoryName);
          }
        }

        foreach (var file in Directory.GetFiles(path))
        {
          zip.AddFile(file, "/");
        }
      }

      // zip.SaveProgress += this.OnSaveProgress;
      zip.Save(zipFileName);
    }

    public virtual string GetFirstRootFolder(string packagePath)
    {
      if (System.IO.File.Exists(packagePath))
      {
        try
        {
          using (ZipFile zip = new ZipFile(packagePath))
          {
            var first = zip.EntryFileNames.First();
            return first.Substring(0, first.IndexOf('/'));
          }
        }
        catch (ZipException)
        {
          throw new InvalidOperationException($"The \"{packagePath}\" installation package seems to be corrupted.");
        }
      }

      return null;
    }

    public virtual void UnpackZip([NotNull] string packagePath, [NotNull] string path, 
      [CanBeNull] string entriesPattern = null, int stepsCount = 1, 
      [CanBeNull] Action incrementProgress = null, bool skipErrors = false)
    {
      Assert.ArgumentNotNull(packagePath, nameof(packagePath));
      Assert.ArgumentNotNull(path, nameof(path));

      // TODO: comment this line when the progress bar is adjusted
      incrementProgress = null;

      bool hasResponse = incrementProgress != null;
      if (System.IO.File.Exists(packagePath))
      {
        try
        {
          if (entriesPattern != null)
          {
            Log.Info(string.Format("Unzipping the {2} entries of the '{0}' archive to the '{1}' folder", packagePath, path, entriesPattern));
          }
          else 
          {
            Log.Info($"Unzipping the '{packagePath}' archive to the '{path}' folder");
          }

          using (ZipFile zip = new ZipFile(packagePath))
          {
            var q = Math.Max(zip.Entries.Count / stepsCount, 1);

            FileSystem.Directory.Ensure(path);
            var i = 0;
            ICollection<ZipEntry> entries = entriesPattern != null ? zip.SelectEntries(entriesPattern) : zip.Entries;
            foreach (ZipEntry entry in entries)
            {
              try
              {
                entry.Extract(path, ExtractExistingFileAction.OverwriteSilently);
              }
              catch (IOException ex)
              {
                if (skipErrors)
                {
                  Log.Error(ex, "Unpacking caused exception");
                  continue;
                }

                bool b = false;
                foreach (string postFix in new[]
                {
                  ".tmp", ".PendingOverwrite"
                })
                {
                  var errorPath = Path.Combine(path, entry.FileName) + postFix;
                  if (System.IO.File.Exists(errorPath))
                  {
                    System.IO.File.Delete(errorPath);
                    b = true;
                  }
                }

                if (!b)
                {
                  throw;
                }

                entry.Extract(path, ExtractExistingFileAction.OverwriteSilently);
              }
              catch (Exception ex)
              {
                if (skipErrors)
                {
                  Log.Error(ex, "Unpacking caused exception");
                  continue;
                }
              }

              if (hasResponse)
              {
                if (++i == q)
                {
                  incrementProgress();
                  i = 0;
                }
              }
            }
          }
        }
        catch (ZipException)
        {
          throw new InvalidOperationException($"The \"{packagePath}\" package seems to be corrupted.");
        }
      }
    }

    public virtual void UnpackZipWithActualWebRootName([NotNull] string packagePath, [NotNull] string path, string webRootName, [CanBeNull] string entriesPattern = null, int stepsCount = 1, [CanBeNull] Action incrementProgress = null)
    {
      Assert.ArgumentNotNull(packagePath, nameof(packagePath));
      Assert.ArgumentNotNull(path, nameof(path));

      // TODO: comment this line when the progress bar is adjusted
      incrementProgress = null;

      var hasResponse = incrementProgress != null;
      if (!System.IO.File.Exists(packagePath))
      {
        return;
      }

      try
      {
        if (entriesPattern != null)
        {
          Log.Info(string.Format("Unzipping the {2} entries of the '{0}' archive to the '{1}' folder", packagePath, path, entriesPattern));
        }
        else
        {
          Log.Info($"Unzipping the '{packagePath}' archive to the '{path}' folder");
        }

        using (var zip = new ZipFile(packagePath))
        {
          var q = Math.Max(zip.Entries.Count / stepsCount, 1);

          FileSystem.Directory.Ensure(path);
          var i = 0;
          var entries = entriesPattern != null ? zip.SelectEntries(entriesPattern) : zip.Entries;

          for (var j = 0; j < entries.Count(); j++)
          {
            try
            {
              if (entries.ElementAt(j).FileName.StartsWith("Website", true, CultureInfo.InvariantCulture))
              {
                entries.ElementAt(j).FileName = webRootName + entries.ElementAt(j).FileName.Substring(7);
              }

              entries.ElementAt(j).Extract(path, ExtractExistingFileAction.OverwriteSilently);
            }
            catch (IOException)
            {
              var b = false;
              foreach (var postFix in new[]
              {
                ".tmp", ".PendingOverwrite"
              })
              {
                var errorPath = Path.Combine(path, entries.ElementAt(j).FileName) + postFix;
                if (!System.IO.File.Exists(errorPath))
                {
                  continue;
                }

                System.IO.File.Delete(errorPath);
                b = true;
              }

              if (!b)
              {
                throw;
              }

              entries.ElementAt(j).Extract(path, ExtractExistingFileAction.OverwriteSilently);
            }

            if (!hasResponse)
            {
              continue;
            }

            if (++i != q)
            {
              continue;
            }

            incrementProgress();
            i = 0;
          }
        }
      }
      catch (ZipException)
      {
        throw new InvalidOperationException($"The \"{packagePath}\" package seems to be corrupted.");
      }
    }
    
    public virtual bool ZipContainsSingleFile(string packagePath, string innerFileName)
    {
      var fileInfo = new FileInfo(packagePath);
      Assert.IsTrue(fileInfo.Exists, $"The {packagePath} file does not exist");

      if (fileInfo.Length <= 22)
      {
        return false;
      }

      using (ZipFile zip = new ZipFile(packagePath))
      {
        if (zip.Entries.Count == 1 && zip.Entries.First().FileName == innerFileName)
        {
          return true;
        }
      }

      return false;
    }

    public virtual string ZipUnpackFile(string pathToZip, string pathToUnpack, string fileName)
    {
      var zipToUnpack = pathToZip;
      var unpackDirectory = pathToUnpack;
      using (ZipFile zip = ZipFile.Read(zipToUnpack))
      {
        foreach (ZipEntry entry in zip)
        {
          string[] splittedFileName = entry.FileName.Split('/');
          if (splittedFileName[splittedFileName.Length - 1] == fileName)
          {
            entry.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
            return Path.Combine(pathToUnpack, fileName);
          }
        }
      }

      return string.Empty;
    }

    public virtual string ZipUnpackFolder(string pathToZip, string pathToUnpack, string folderName)
    {
      using (ZipFile zip1 = ZipFile.Read(pathToZip))
      {
        var selection = from e in zip1.Entries
          where (e.FileName).StartsWith(folderName + "/") || (e.FileName).StartsWith(folderName + "\\")          
          select e;


        Directory.CreateDirectory(pathToUnpack);

        foreach (var e in selection)
        {
          if (e.UncompressedSize > 0)
          {
            e.Extract(pathToUnpack, ExtractExistingFileAction.OverwriteSilently);
          }
          else
          {
            new DirectoryInfo(Path.Combine(pathToUnpack, e.FileName.Trim("\\/".ToCharArray()))).Create();
          }
        }
      }

      return pathToUnpack.PathCombine(folderName);
    }

    #endregion
  }
}