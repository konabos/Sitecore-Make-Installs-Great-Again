using System.Collections.Generic;

namespace MIGA.IO
{
  using JetBrains.Annotations;

  public interface IZipFileEntries
  {
    bool Contains([NotNull] string entryPath);

    IEnumerable<IZipFileEntry> GetEntries();
  }
}