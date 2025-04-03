// Decompiled with JetBrains decompiler
// Type: Castle.Core.Configuration.Xml.XmlConfigurationDeserializer
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace Castle.Core.Configuration.Xml
{
  public class XmlConfigurationDeserializer
  {
    public IConfiguration Deserialize(XmlNode node) => this.Deserialize(node);

    public static IConfiguration GetDeserializedNode(XmlNode node)
    {
      ConfigurationCollection collection = new ConfigurationCollection();
      StringBuilder stringBuilder = new StringBuilder();
      if (node.HasChildNodes)
      {
        foreach (XmlNode childNode in node.ChildNodes)
        {
          if (XmlConfigurationDeserializer.IsTextNode(childNode))
            stringBuilder.Append(childNode.Value);
          else if (childNode.NodeType == XmlNodeType.Element)
            collection.Add(XmlConfigurationDeserializer.GetDeserializedNode(childNode));
        }
      }
      MutableConfiguration deserializedNode = new MutableConfiguration(node.Name, XmlConfigurationDeserializer.GetConfigValue(stringBuilder.ToString()));
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) node.Attributes)
        deserializedNode.Attributes.Add(attribute.Name, attribute.Value);
      deserializedNode.Children.AddRange((IEnumerable<IConfiguration>) collection);
      return (IConfiguration) deserializedNode;
    }

    public static string GetConfigValue(string value)
    {
      return !string.IsNullOrEmpty(value) ? value.Trim() : (string) null;
    }

    public static bool IsTextNode(XmlNode node)
    {
      return node.NodeType == XmlNodeType.Text || node.NodeType == XmlNodeType.CDATA;
    }
  }
}
