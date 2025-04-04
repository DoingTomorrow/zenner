// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelBackgroundImage
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelBackgroundImage : XmlHelper
  {
    private const string BACKGROUNDPIC_PATH = "d:picture/@r:id";
    private ExcelWorksheet _workSheet;

    internal ExcelBackgroundImage(
      XmlNamespaceManager nsm,
      XmlNode topNode,
      ExcelWorksheet workSheet)
      : base(nsm, topNode)
    {
      this._workSheet = workSheet;
    }

    public Image Image
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("d:picture/@r:id");
        if (string.IsNullOrEmpty(xmlNodeString))
          return (Image) null;
        ZipPackageRelationship relationship = this._workSheet.Part.GetRelationship(xmlNodeString);
        return Image.FromStream((Stream) this._workSheet.Part.Package.GetPart(UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri)).GetStream());
      }
      set
      {
        this.DeletePrevImage();
        if (value == null)
          this.DeleteAllNode("d:picture/@r:id");
        else
          this.SetXmlNodeString("d:picture/@r:id", this._workSheet.Part.CreateRelationship(this._workSheet.Workbook._package.AddImage((byte[]) new ImageConverter().ConvertTo((object) value, typeof (byte[]))).Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image").Id);
      }
    }

    public void SetFromFile(FileInfo PictureFile)
    {
      this.DeletePrevImage();
      Image image;
      try
      {
        image = Image.FromFile(PictureFile.FullName);
      }
      catch (Exception ex)
      {
        throw new InvalidDataException("File is not a supported image-file or is corrupt", ex);
      }
      ImageConverter imageConverter = new ImageConverter();
      string contentType = ExcelPicture.GetContentType(PictureFile.Extension);
      Uri newUri = XmlHelper.GetNewUri(this._workSheet._package.Package, "/xl/media/" + PictureFile.Name.Substring(0, PictureFile.Name.Length - PictureFile.Extension.Length) + "{0}" + PictureFile.Extension);
      byte[] numArray = (byte[]) imageConverter.ConvertTo((object) image, typeof (byte[]));
      ExcelPackage.ImageInfo imageInfo = this._workSheet.Workbook._package.AddImage(numArray, newUri, contentType);
      if (this._workSheet.Part.Package.PartExists(newUri) && imageInfo.RefCount == 1)
        this._workSheet.Part.Package.DeletePart(newUri);
      this._workSheet.Part.Package.CreatePart(newUri, contentType, CompressionLevel.Level0).GetStream(FileMode.Create, FileAccess.Write).Write(numArray, 0, numArray.Length);
      this.SetXmlNodeString("d:picture/@r:id", this._workSheet.Part.CreateRelationship(newUri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image").Id);
    }

    private void DeletePrevImage()
    {
      string xmlNodeString = this.GetXmlNodeString("d:picture/@r:id");
      if (!(xmlNodeString != ""))
        return;
      ExcelPackage.ImageInfo imageInfo = this._workSheet.Workbook._package.GetImageInfo((byte[]) new ImageConverter().ConvertTo((object) this.Image, typeof (byte[])));
      this._workSheet.Part.DeleteRelationship(xmlNodeString);
      if (imageInfo == null || imageInfo.RefCount != 1 || !this._workSheet.Part.Package.PartExists(imageInfo.Uri))
        return;
      this._workSheet.Part.Package.DeletePart(imageInfo.Uri);
    }
  }
}
