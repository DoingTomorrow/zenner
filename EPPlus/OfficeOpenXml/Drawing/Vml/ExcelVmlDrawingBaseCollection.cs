// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingBaseCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  public class ExcelVmlDrawingBaseCollection
  {
    internal ExcelVmlDrawingBaseCollection(ExcelPackage pck, ExcelWorksheet ws, Uri uri)
    {
      this.VmlDrawingXml = new XmlDocument();
      this.VmlDrawingXml.PreserveWhitespace = false;
      this.NameSpaceManager = new XmlNamespaceManager((XmlNameTable) new NameTable());
      this.NameSpaceManager.AddNamespace("v", "urn:schemas-microsoft-com:vml");
      this.NameSpaceManager.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
      this.NameSpaceManager.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
      this.Uri = uri;
      if (uri == (Uri) null)
      {
        this.Part = (ZipPackagePart) null;
      }
      else
      {
        this.Part = pck.Package.GetPart(uri);
        XmlHelper.LoadXmlSafe(this.VmlDrawingXml, (Stream) this.Part.GetStream());
      }
    }

    internal XmlDocument VmlDrawingXml { get; set; }

    internal Uri Uri { get; set; }

    internal string RelId { get; set; }

    internal ZipPackagePart Part { get; set; }

    internal XmlNamespaceManager NameSpaceManager { get; set; }
  }
}
