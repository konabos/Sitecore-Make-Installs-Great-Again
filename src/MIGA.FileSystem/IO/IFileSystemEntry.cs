﻿namespace MIGA.IO
{
  using JetBrains.Annotations;

  public interface IFileSystemEntry
  {             
    [NotNull]
    IFileSystem FileSystem { get; }

    [NotNull]
    string FullName { get; }

    [NotNull]
    string Name { get; }

    void TryDelete();

    void Create();

    bool Exists { get; }

    [NotNull]
    IFolder Parent { get; }
  }
}