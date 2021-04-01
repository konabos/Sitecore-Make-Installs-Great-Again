﻿namespace MIGA
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Xml;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;

  #region

  #endregion

  [Serializable]
  public class DataObject : DataObjectBase
  {
    #region Fields

    private readonly Dictionary<string, object> _Values = new Dictionary<string, object>();

    #endregion

    #region Public Methods

    public void Load([NotNull] XmlNode[] nodes)
    {
      Assert.ArgumentNotNull(nodes, nameof(nodes));

      foreach (XmlElement element in nodes.OfType<XmlElement>())
      {
        _Values.Add(element.Name, element.InnerXml);
      }
    }

    public void Save([NotNull] XmlElement root)
    {
      Assert.ArgumentNotNull(root, nameof(root));

      XmlDocument document = root.OwnerDocument;
      Assert.IsNotNull(document, "Root element must have a document");
      foreach (string key in _Values.Keys)
      {
        XmlElement element = document.CreateElement(key);
        element.InnerXml = _Values[key].ToString();
        root.AppendChild(element);
      }
    }

    public virtual void Validate()
    {
      Type type = GetType();
      const string ValidatePrefix = "Validate";
      IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(pr => pr.Name.StartsWith(ValidatePrefix) && pr.Name.Length != ValidatePrefix.Length);
      foreach (MethodInfo method in methods)
      {
        method.Invoke(this, new object[0]);
      }
    }

    #endregion

    #region Methods

    [CanBeNull]
    protected string GetString([NotNull] string name)
    {
      Assert.ArgumentNotNull(name, nameof(name));

      return GetValue(name) as string;
    }

    [CanBeNull]
    protected object GetValue([NotNull] string name)
    {
      Assert.ArgumentNotNull(name, nameof(name));

      return _Values.ContainsKey(name) ? _Values[name] : null;
    }

    protected virtual void SetValue([NotNull] string name, [CanBeNull] object value)
    {
      Assert.ArgumentNotNull(name, nameof(name));

      _Values[name] = value;
      NotifyPropertyChanged(name);
    }

    protected void SetValue([NotNull] string name, [CanBeNull] string value)
    {
      Assert.ArgumentNotNull(name, nameof(name));

      SetValue(name, value as object);
    }

    #endregion
  }
}