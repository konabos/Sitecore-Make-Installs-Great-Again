﻿using Sitecore.Diagnostics.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MIGA.Tool.Windows.Dialogs
{
  class GridEditorContext
  {
    public ObservableCollection<object> GridItems { get; }
    public string Title { get; }
    public string Description { get; }
    public Type ElementType { get; }

    public GridEditorContext(Type elementType, IEnumerable<object> itemsSource, string title, string description)
    {
      Assert.ArgumentNotNull(itemsSource, nameof(itemsSource));
      Assert.ArgumentNotNullOrEmpty(description, nameof(description));
      this.GridItems = new ObservableCollection<object>(itemsSource);
      this.Title = title;
      this.Description = description;
      this.ElementType = elementType;
    }
  }
}
