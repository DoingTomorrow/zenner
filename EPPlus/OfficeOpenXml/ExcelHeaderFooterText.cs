// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelHeaderFooterText
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Vml;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelHeaderFooterText
  {
    private ExcelWorksheet _ws;
    private string _hf;
    public string LeftAlignedText;
    public string CenteredText;
    public string RightAlignedText;

    internal ExcelHeaderFooterText(XmlNode TextNode, ExcelWorksheet ws, string hf)
    {
      this._ws = ws;
      this._hf = hf;
      if (TextNode == null || string.IsNullOrEmpty(TextNode.InnerText))
        return;
      string innerText = TextNode.InnerText;
      string code = innerText.Substring(0, 2);
      int startIndex1 = 2;
      for (int startIndex2 = startIndex1; startIndex2 < innerText.Length - 2; ++startIndex2)
      {
        string str = innerText.Substring(startIndex2, 2);
        if (str == "&C" || str == "&R")
        {
          this.SetText(code, innerText.Substring(startIndex1, startIndex2 - startIndex1));
          startIndex1 = startIndex2 + 2;
          startIndex2 = startIndex1;
          code = str;
        }
      }
      this.SetText(code, innerText.Substring(startIndex1, innerText.Length - startIndex1));
    }

    private void SetText(string code, string text)
    {
      switch (code)
      {
        case "&L":
          this.LeftAlignedText = text;
          break;
        case "&C":
          this.CenteredText = text;
          break;
        default:
          this.RightAlignedText = text;
          break;
      }
    }

    public ExcelVmlDrawingPicture InsertPicture(Image Picture, PictureAlignment Alignment)
    {
      string id = this.ValidateImage(Alignment);
      ExcelPackage.ImageInfo ii = this._ws.Workbook._package.AddImage((byte[]) new ImageConverter().ConvertTo((object) Picture, typeof (byte[])));
      return this.AddImage(Picture, id, ii);
    }

    public ExcelVmlDrawingPicture InsertPicture(FileInfo PictureFile, PictureAlignment Alignment)
    {
      string id = this.ValidateImage(Alignment);
      Image Picture;
      try
      {
        Picture = PictureFile.Exists ? Image.FromFile(PictureFile.FullName) : throw new FileNotFoundException(string.Format("{0} is missing", (object) PictureFile.FullName));
      }
      catch (Exception ex)
      {
        throw new InvalidDataException("File is not a supported image-file or is corrupt", ex);
      }
      ImageConverter imageConverter = new ImageConverter();
      string contentType = ExcelPicture.GetContentType(PictureFile.Extension);
      Uri newUri = XmlHelper.GetNewUri(this._ws._package.Package, "/xl/media/" + PictureFile.Name.Substring(0, PictureFile.Name.Length - PictureFile.Extension.Length) + "{0}" + PictureFile.Extension);
      ExcelPackage.ImageInfo ii = this._ws.Workbook._package.AddImage((byte[]) imageConverter.ConvertTo((object) Picture, typeof (byte[])), newUri, contentType);
      return this.AddImage(Picture, id, ii);
    }

    private ExcelVmlDrawingPicture AddImage(Image Picture, string id, ExcelPackage.ImageInfo ii)
    {
      double width = (double) (Picture.Width * 72) / (double) Picture.HorizontalResolution;
      double height = (double) (Picture.Height * 72) / (double) Picture.VerticalResolution;
      return this._ws.HeaderFooter.Pictures.Add(id, ii.Uri, "", width, height);
    }

    private string ValidateImage(PictureAlignment Alignment)
    {
      string str = Alignment.ToString()[0].ToString() + this._hf;
      foreach (ExcelVmlDrawingBase picture in (IEnumerable) this._ws.HeaderFooter.Pictures)
      {
        if (picture.Id == str)
          throw new InvalidOperationException("A picture already exists in this section");
      }
      switch (Alignment)
      {
        case PictureAlignment.Left:
          this.LeftAlignedText += "&G";
          break;
        case PictureAlignment.Centered:
          this.CenteredText += "&G";
          break;
        default:
          this.RightAlignedText += "&G";
          break;
      }
      return str;
    }
  }
}
