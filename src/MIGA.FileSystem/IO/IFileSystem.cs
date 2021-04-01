﻿namespace MIGA.IO
{
  using JetBrains.Annotations;

  public interface IFileSystem
  { 
    [NotNull]
    IFolder ParseFolder([NotNull] string path);

    [NotNull]
    IFile ParseFile([NotNull] string path);

    [NotNull]
    ITempFile ParseTempFile();
  }
}