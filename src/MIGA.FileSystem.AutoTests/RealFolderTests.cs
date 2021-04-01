﻿using MIGA.IO;
using MIGA.IO.Real;

namespace MIGA.FileSystem.AutoTests
{
  using System;
  using System.IO;
  using System.Linq;
  using MIGA.IO;
  using MIGA.IO.Real;
  using Xunit;

  public class RealFolderTests : IDisposable
  {
    private DirectoryInfo Root { get; } = new DirectoryInfo($"C:\\TMP\\{Guid.NewGuid():N}");

    private RealFileSystem FileSystem { get; } = new RealFileSystem();

    public void Dispose()
    {
      Root.Delete(true);
    }

    private DirectoryInfo CreateUniqueDir(DirectoryInfo dir = null)
    {
      return (dir ?? Root).CreateSubdirectory(Guid.NewGuid().ToString("N"));
    }

    private FileInfo CreateUniqueFile(DirectoryInfo dir = null)
    {
      return CreateFile(dir ?? Root, Guid.NewGuid().ToString("N"));
    }

    private FileInfo CreateFile(DirectoryInfo dir, string fileName)
    {
      var path = Path.Combine(dir.FullName, fileName);
      File.WriteAllText(path, "");

      return new FileInfo(path);
    }

    [Fact]
    public void Ctor_NoStateChange_Exists()
    {
      // arrange
      var dir = CreateUniqueDir();
      dir.Create();

      var before = dir.Exists;

      // act
      FileSystem.ParseFolder(dir);

      // assert
      var after = dir.Exists;
      Assert.True(before == after);
    }

    [Fact]
    public void Ctor_NoStateChange_NotExists()
    {
      // arrange
      var dir = CreateUniqueDir();
      dir.Delete(true);

      var before = dir.Exists;

      // act
      FileSystem.ParseFolder(dir);

      // assert
      var after = dir.Exists;
      Assert.True(before == after);
    }

    [Fact]
    public void FullName()
    {
      // arrange
      var dir = CreateUniqueDir();
      var expected = dir.FullName;

      var sut = FileSystem.ParseFolder(dir);

      // act
      var name = sut.FullName;

      // assert
      Assert.Equal(expected, name);
    }

    [Fact]
    public void Name()
    {
      // arrange
      var dir = CreateUniqueDir();

      var sut = FileSystem.ParseFolder(dir);

      // act
      var name = sut.Name;

      // assert
      Assert.Equal(dir.Name, name);
    }

    [Fact]
    public void Equals_Same()
    {
      // arrange
      var dir = CreateUniqueDir();
      var sutA = FileSystem.ParseFolder(dir);
      var sutB = FileSystem.ParseFolder(dir);

      // act
      var result = sutA.Equals(sutB);

      // assert
      Assert.True(result);
    }

    [Fact]
    public void Equals_Different()
    {
      // arrange
      var dir = CreateUniqueDir();
      var folder0 = CreateUniqueDir(dir);
      var folder1 = CreateUniqueDir(dir);
      var sut1 = FileSystem.ParseFolder(folder0);
      var sut2 = FileSystem.ParseFolder(folder1);

      // act
      var result = sut1.Equals(sut2);

      // assert
      Assert.True(!result);
    }

    [Fact]
    public void Equals_Object()
    {
      // arrange
      var dir = CreateUniqueDir();
      var folder0 = CreateUniqueDir(dir);
      var sut1 = FileSystem.ParseFolder(folder0);
      
      // act
      Assert.Throws<InvalidCastException>(() => sut1.Equals(new object()));
    }

    [Fact]
    public void Exists_Yes()
    {
      // arrange
      var dir = CreateUniqueDir();
      dir.Create();
      
      var sut = FileSystem.ParseFolder(dir);

      // act
      var exists = sut.Exists;

      // assert
      Assert.True(exists);
    }

    [Fact]
    public void Exists_No()
    {
      // arrange
      var dir = CreateUniqueDir();
      dir.Delete(true);
      
      var sut = FileSystem.ParseFolder(dir);

      // act
      var exists = sut.Exists;

      // assert
      Assert.True(!exists);
    }

    [Fact]
    public void Create()
    {
      // arrange
      var dir = CreateUniqueDir();
      dir.Delete(true);

      var sut = FileSystem.ParseFolder(dir);

      // act
      sut.Create();

      // assert
      var exists = dir.Exists;
      Assert.True(exists);
    }

    [Fact]
    public void GetFolders()
    {
      // arrange
      var dir = CreateUniqueDir();
      var folder0 = CreateUniqueDir(dir);
      var folder1 = CreateUniqueDir(dir);
      var sut = FileSystem.ParseFolder(dir);

      // act
      var folders = sut.GetFolders();

      // assert
      Assert.Equal(2, folders.Count);
      Assert.True(
        (folder0.FullName == folders[0].FullName && folder1.FullName == folders[1].FullName) || 
        (folder0.FullName == folders[1].FullName && folder1.FullName == folders[0].FullName));
    }

    [Fact]
    public void GetFiles()
    {
      // arrange
      var dir = CreateUniqueDir();
      var file0 = CreateUniqueFile(dir);
      var file1 = CreateUniqueFile(dir);
      var sut = FileSystem.ParseFolder(dir);

      // act
      var files = sut.GetFiles();

      // assert
      Assert.Equal(2, files.Count);
      Assert.True(
        (file0.FullName == files[0].FullName && file1.FullName == files[1].FullName) ||
        (file0.FullName == files[1].FullName && file1.FullName == files[0].FullName));
    }

    [Fact]
    public void GetChildren()
    {
      // arrange
      var dir = CreateUniqueDir();
      var folder0 = CreateUniqueDir(dir);
      var folder1 = CreateUniqueDir(dir);
      var file0 = CreateUniqueFile(dir);
      var file1 = CreateUniqueFile(dir);
      var sut = FileSystem.ParseFolder(dir);

      // act
      var children = sut.GetChildren();
      var folders = children.OfType<IFolder>().ToArray();
      var files = children.OfType<IFile>().ToArray();

      // assert
      Assert.Equal(2, folders.Length);
      Assert.True(
        (folder0.FullName == folders[0].FullName && folder1.FullName == folders[1].FullName) ||
        (folder0.FullName == folders[1].FullName && folder1.FullName == folders[0].FullName));

      Assert.Equal(2, files.Length);
      Assert.True(
        (file0.FullName == files[0].FullName && file1.FullName == files[1].FullName) ||
        (file0.FullName == files[1].FullName && file1.FullName == files[0].FullName));
    }

    [Fact]
    public void MoveTo_Exists_Plain()
    {
      // arrange
      var dir = CreateUniqueDir();
      var folderA = CreateUniqueDir(dir);
      var folderB = CreateUniqueDir(dir);

      var sutA = FileSystem.ParseFolder(folderA);
      var sutB = FileSystem.ParseFolder(folderB);

      // act
      var moved = sutA.MoveTo(sutB);

      // assert
      Assert.True(moved.Exists);
      Assert.True(!sutA.Exists);
    }

    [Fact]
    public void MoveTo_FullName_Plain()
    {
      // arrange
      var dir = CreateUniqueDir();
      var folderA = CreateUniqueDir(dir);
      var folderB = CreateUniqueDir(dir);
      
      var sutA = FileSystem.ParseFolder(folderA);
      var sutB = FileSystem.ParseFolder(folderB);

      // act
      var moved = sutA.MoveTo(sutB);

      // assert
      Assert.Equal($"{dir.FullName}\\{folderB.Name}\\{folderA.Name}", moved.FullName);
    }

    [Fact]
    public void MoveTo_FullName_File()
    {
      // arrange
      var dir = CreateUniqueDir();
      var outerA = CreateUniqueDir(dir);
      var innerA = CreateUniqueDir(outerA);
      var outerB = CreateUniqueDir(dir);

      // outerA/innerA/fileA
      var fileA = CreateUniqueFile(innerA);
      var sutA = FileSystem.ParseFolder(outerA);
      var sutB = FileSystem.ParseFolder(outerB);

      // act
      sutA.MoveTo(sutB);

      // assert
      Assert.True(File.Exists($"{dir.FullName}\\{outerB.Name}\\{outerA.Name}\\{innerA.Name}\\{fileA.Name}"));
    }

    [Fact]
    public void MoveTo_Conflict()
    {
      // arrange
      var dir = CreateUniqueDir();
      var outerA = CreateUniqueDir(dir);
      var fileA = CreateUniqueFile(outerA);

      var outerB = CreateUniqueDir(dir);
      var innerB = outerB.CreateSubdirectory(outerA.Name);
      new FileInfo(Path.Combine(innerB.FullName, fileA.Name)).Create().Close();

      var sutA = FileSystem.ParseFolder(outerA);
      var sutB = FileSystem.ParseFolder(outerB);

      // act
      sutA.MoveTo(sutB);
      
      // assert
      Assert.True(File.Exists($"{dir.FullName}\\{outerB.Name}\\{outerA.Name}\\{fileA.Name}"));
    }

    [Fact]
    public void CopyTo_FullName_Plain()
    {
      // arrange
      var dir = CreateUniqueDir();

      var a = FileSystem.ParseFolder(CreateUniqueDir(dir));
      var b = FileSystem.ParseFolder(CreateUniqueDir(dir));

      // act
      var result = a.CopyTo(b);

      // assert
      Assert.Equal($"{dir.FullName}\\{b.Name}\\{a.Name}", result.FullName);
    }

    [Fact]
    public void CopyTo_FullName_Deep()
    {
      // arrange
      var dir = CreateUniqueDir();

      var a = FileSystem.ParseFolder(CreateUniqueDir(dir));
      var b = a.GetChildFolder(Guid.NewGuid().ToString("N"));
      b.Create();

      var c = FileSystem.ParseFolder(CreateUniqueDir(dir));

      // act
      var result = a.CopyTo(c);

      // assert
      var d = result.GetChildFolder(b.Name);
      Assert.Equal($"{dir.FullName}\\{c.Name}\\{a.Name}\\{b.Name}", d.FullName);
    }

    [Fact]
    public void CopyTo_Exists_Deep()
    {
      // arrange
      var dir = CreateUniqueDir();

      var a = FileSystem.ParseFolder(CreateUniqueDir(dir));
      var b = a.GetChildFolder(Guid.NewGuid().ToString("N"));
      b.Create();

      var c = FileSystem.ParseFolder(CreateUniqueDir(dir));

      // act
      var result = a.CopyTo(c);

      // assert
      var d = result.GetChildFolder(b.Name);
      Assert.True(d.Exists);
    }
  }
}
