// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.Vml.ExcelVmlDrawingPictureCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing.Vml
{
  public class ExcelVmlDrawingPictureCollection : ExcelVmlDrawingBaseCollection, IEnumerable
  {
    internal List<ExcelVmlDrawingPicture> _images;
    private ExcelPackage _pck;
    private ExcelWorksheet _ws;
    private int _nextID;

    internal ExcelVmlDrawingPictureCollection(ExcelPackage pck, ExcelWorksheet ws, Uri uri)
      : base(pck, ws, uri)
    {
      this._pck = pck;
      this._ws = ws;
      if (uri == (Uri) null)
      {
        this.VmlDrawingXml.LoadXml(this.CreateVmlDrawings());
        this._images = new List<ExcelVmlDrawingPicture>();
      }
      else
        this.AddDrawingsFromXml();
    }

    private void AddDrawingsFromXml()
    {
      XmlNodeList xmlNodeList = this.VmlDrawingXml.SelectNodes("//v:shape", this.NameSpaceManager);
      this._images = new List<ExcelVmlDrawingPicture>();
      foreach (XmlNode topNode in xmlNodeList)
      {
        ExcelVmlDrawingPicture vmlDrawingPicture = new ExcelVmlDrawingPicture(topNode, this.NameSpaceManager, this._ws);
        ZipPackageRelationship relationship = this.Part.GetRelationship(vmlDrawingPicture.RelId);
        vmlDrawingPicture.ImageUri = UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri);
        this._images.Add(vmlDrawingPicture);
      }
    }

    private string CreateVmlDrawings()
    {
      return string.Format("<xml xmlns:v=\"{0}\" xmlns:o=\"{1}\" xmlns:x=\"{2}\">", (object) "urn:schemas-microsoft-com:vml", (object) "urn:schemas-microsoft-com:office:office", (object) "urn:schemas-microsoft-com:office:excel") + "<o:shapelayout v:ext=\"edit\">" + "<o:idmap v:ext=\"edit\" data=\"1\"/>" + "</o:shapelayout>" + "<v:shapetype id=\"_x0000_t202\" coordsize=\"21600,21600\" o:spt=\"202\" path=\"m,l,21600r21600,l21600,xe\">" + "<v:stroke joinstyle=\"miter\" />" + "<v:path gradientshapeok=\"t\" o:connecttype=\"rect\" />" + "</v:shapetype>" + "</xml>";
    }

    internal ExcelVmlDrawingPicture Add(
      string id,
      Uri uri,
      string name,
      double width,
      double height)
    {
      ExcelVmlDrawingPicture vmlDrawingPicture = new ExcelVmlDrawingPicture(this.AddImage(id, uri, name, width, height), this.NameSpaceManager, this._ws);
      vmlDrawingPicture.ImageUri = uri;
      this._images.Add(vmlDrawingPicture);
      return vmlDrawingPicture;
    }

    private XmlNode AddImage(string id, Uri targeUri, string Name, double width, double height)
    {
      XmlElement element = this.VmlDrawingXml.CreateElement("v", "shape", "urn:schemas-microsoft-com:vml");
      this.VmlDrawingXml.DocumentElement.AppendChild((XmlNode) element);
      element.SetAttribute(nameof (id), id);
      element.SetAttribute("o:type", "#_x0000_t75");
      element.SetAttribute("style", string.Format("position:absolute;margin-left:0;margin-top:0;width:{0}pt;height:{1}pt;z-index:1", (object) width.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) height.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      element.InnerXml = string.Format("<v:imagedata o:relid=\"\" o:title=\"{0}\"/><o:lock v:ext=\"edit\" rotation=\"t\"/>", (object) Name);
      return (XmlNode) element;
    }

    public ExcelVmlDrawingPicture this[int Index] => this._images[Index];

    public int Count => this._images.Count;

    internal string GetNewId()
    {
      if (this._nextID == 0)
      {
        foreach (ExcelVmlDrawingComment vmlDrawingComment in (IEnumerable) this)
        {
          int result;
          if (vmlDrawingComment.Id.Length > 3 && vmlDrawingComment.Id.StartsWith("vml") && int.TryParse(vmlDrawingComment.Id.Substring(3, vmlDrawingComment.Id.Length - 3), out result) && result > this._nextID)
            this._nextID = result;
        }
      }
      ++this._nextID;
      return "vml" + this._nextID.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._images.GetEnumerator();
  }
}
