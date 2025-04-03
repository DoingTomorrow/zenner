// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.DefaultXmlSerializer
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class DefaultXmlSerializer : IXPathSerializer
  {
    public static readonly DefaultXmlSerializer Instance = new DefaultXmlSerializer();

    private DefaultXmlSerializer()
    {
    }

    public bool WriteObject(XPathResult result, XPathNavigator node, object value)
    {
      XmlRootAttribute root = new XmlRootAttribute(node.LocalName)
      {
        Namespace = node.NamespaceURI
      };
      StringBuilder output = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        OmitXmlDeclaration = true,
        Indent = false
      };
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add(string.Empty, string.Empty);
      if (!string.IsNullOrEmpty(node.NamespaceURI))
      {
        string prefix = result.Context.AddNamespace(node.NamespaceURI);
        namespaces.Add(prefix, node.NamespaceURI);
      }
      using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
      {
        new XmlSerializer(result.Type, root).Serialize(xmlWriter, value, namespaces);
        xmlWriter.Flush();
      }
      node.ReplaceSelf(output.ToString());
      return true;
    }

    public bool ReadObject(XPathResult result, XPathNavigator node, out object value)
    {
      XmlRootAttribute root = new XmlRootAttribute(node.LocalName)
      {
        Namespace = node.NamespaceURI
      };
      XmlSerializer xmlSerializer = new XmlSerializer(result.Type, root);
      using (XmlReader xmlReader = node.ReadSubtree())
      {
        int content = (int) xmlReader.MoveToContent();
        if (xmlSerializer.CanDeserialize(xmlReader))
        {
          value = xmlSerializer.Deserialize(xmlReader);
          return true;
        }
      }
      value = (object) null;
      return false;
    }
  }
}
