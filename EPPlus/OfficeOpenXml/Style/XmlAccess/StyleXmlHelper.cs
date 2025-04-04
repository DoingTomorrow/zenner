// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.StyleXmlHelper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public abstract class StyleXmlHelper : XmlHelper
  {
    internal long useCnt;
    internal int newID = int.MinValue;

    internal StyleXmlHelper(XmlNamespaceManager nameSpaceManager)
      : base(nameSpaceManager)
    {
    }

    internal StyleXmlHelper(XmlNamespaceManager nameSpaceManager, XmlNode topNode)
      : base(nameSpaceManager, topNode)
    {
    }

    internal abstract XmlNode CreateXmlNode(XmlNode top);

    internal abstract string Id { get; }
  }
}
