﻿using MIGA.Adapters.WebServer;

namespace MIGA.Instances
{
  #region

  using System.IO;
  using System.Linq;
  using System.Xml;
  using MIGA.Adapters.WebServer;
  using Sitecore.Diagnostics.Base;
  using JetBrains.Annotations;
  using MIGA.Extensions;

  #endregion

  public sealed class InstanceConfiguration
  {
    #region Fields

    [NotNull]
    private Instance Instance { get; }

    #endregion

    #region Constructors

    public InstanceConfiguration([NotNull] Instance instance)
    {
      Assert.ArgumentNotNull(instance, nameof(instance));

      Instance = instance;
    }

    #endregion

    #region Properties

    #region Public properties

    [NotNull]
    public ConnectionStringCollection ConnectionStrings
    {
      get
      {
        XmlElementEx connectionStringsNode = GetConnectionStringsElement();
        return GetConnectionStringCollection(connectionStringsNode);
      }
    }

    #endregion

    #region Private methods

    private static ConnectionStringCollection GetConnectionStringCollection(XmlElementEx connectionStringsNode)
    {
      ConnectionStringCollection connectionStrings = new ConnectionStringCollection(connectionStringsNode);
      XmlNodeList addNodes = connectionStringsNode.Element.ChildNodes;
      connectionStrings.AddRange(
        addNodes.OfType<XmlElement>().Select(element => new ConnectionString(element, connectionStringsNode.Document)));

      return connectionStrings;
    }

    #endregion

    #endregion

    #region Methods

    private static XmlElementEx GetConnectionStringsElement(XmlDocumentEx webConfig)
    {
      var webRootPath = Path.GetDirectoryName(webConfig.FilePath);
      XmlElement configurationNode = webConfig.SelectSingleNode(WebConfig.ConfigurationXPath) as XmlElement;
      Assert.IsNotNull(configurationNode, 
        "The {0} element is missing in the {1} file".FormatWith("/configuration", webConfig.FilePath));
      XmlElement webConfigConnectionStrings = configurationNode.SelectSingleNode("connectionStrings") as XmlElement;
      Assert.IsNotNull(webConfigConnectionStrings, 
        "The web.config file doesn't contain the /configuration/connectionStrings node");
      XmlAttribute configSourceAttribute = webConfigConnectionStrings.Attributes[WebConfig.ConfigSourceAttributeName];
      if (configSourceAttribute != null)
      {
        var configSourceValue = configSourceAttribute.Value;
        if (!string.IsNullOrEmpty(configSourceValue) && !string.IsNullOrEmpty(webRootPath))
        {
          var filePath = Path.Combine(webRootPath, configSourceValue);
          if (FileSystem.FileSystem.Local.File.Exists(filePath))
          {
            XmlDocumentEx connectionStringsConfig = XmlDocumentEx.LoadFile(filePath);
            XmlElement connectionStrings = connectionStringsConfig.SelectSingleNode("/connectionStrings") as XmlElement;
            if (connectionStrings != null)
            {
              return new XmlElementEx(connectionStrings, connectionStringsConfig);
            }
          }
        }
      }

      return new XmlElementEx(webConfigConnectionStrings, webConfig);
    }

    [NotNull]
    private XmlElementEx GetConnectionStringsElement()
    {
      XmlDocumentEx webConfig = Instance.GetWebConfig();
      Assert.IsNotNull(webConfig, nameof(webConfig));

      return GetConnectionStringsElement(webConfig);
    }

    #endregion

    #region Public methods

    #endregion
  }
}