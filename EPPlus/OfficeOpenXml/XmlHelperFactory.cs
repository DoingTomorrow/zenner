// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.XmlHelperFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  internal static class XmlHelperFactory
  {
    internal static XmlHelper Create(XmlNamespaceManager namespaceManager)
    {
      return (XmlHelper) new XmlHelperInstance(namespaceManager);
    }

    internal static XmlHelper Create(XmlNamespaceManager namespaceManager, XmlNode topNode)
    {
      return (XmlHelper) new XmlHelperInstance(namespaceManager, topNode);
    }
  }
}
