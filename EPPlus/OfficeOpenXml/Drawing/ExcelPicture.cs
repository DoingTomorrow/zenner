// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Drawing.ExcelPicture
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Drawing
{
  public sealed class ExcelPicture : ExcelDrawing
  {
    private Image _image;
    private ImageFormat _imageFormat = ImageFormat.Jpeg;
    internal ZipPackagePart Part;
    private ExcelDrawingFill _fill;
    private ExcelDrawingBorder _border;
    private Uri _hyperlink;

    internal ExcelPicture(ExcelDrawings drawings, XmlNode node)
      : base(drawings, node, "xdr:pic/xdr:nvPicPr/xdr:cNvPr/@name")
    {
      XmlNode xmlNode = node.SelectSingleNode("xdr:pic/xdr:blipFill/a:blip", drawings.NameSpaceManager);
      if (xmlNode == null)
        return;
      this.RelPic = drawings.Part.GetRelationship(xmlNode.Attributes["r:embed"].Value);
      this.UriPic = UriHelper.ResolvePartUri(drawings.UriDrawing, this.RelPic.TargetUri);
      this.Part = drawings.Part.Package.GetPart(this.UriPic);
      this.ContentType = ExcelPicture.GetContentType(new FileInfo(this.UriPic.OriginalString).Extension);
      this._image = Image.FromStream((Stream) this.Part.GetStream());
      this.ImageHash = this._drawings._package.LoadImage((byte[]) new ImageConverter().ConvertTo((object) this._image, typeof (byte[])), this.UriPic, this.Part).Hash;
      string xmlNodeString = this.GetXmlNodeString("xdr:pic/xdr:nvPicPr/xdr:cNvPr/a:hlinkClick/@r:id");
      if (string.IsNullOrEmpty(xmlNodeString))
        return;
      this.HypRel = drawings.Part.GetRelationship(xmlNodeString);
      this._hyperlink = !this.HypRel.TargetUri.IsAbsoluteUri ? (Uri) new ExcelHyperLink(this.HypRel.TargetUri.OriginalString, UriKind.Relative) : (Uri) new ExcelHyperLink(this.HypRel.TargetUri.AbsoluteUri);
      ((ExcelHyperLink) this._hyperlink).ToolTip = this.GetXmlNodeString("xdr:pic/xdr:nvPicPr/xdr:cNvPr/a:hlinkClick/@tooltip");
    }

    internal ExcelPicture(ExcelDrawings drawings, XmlNode node, Image image)
      : this(drawings, node, image, (Uri) null)
    {
    }

    internal ExcelPicture(ExcelDrawings drawings, XmlNode node, Image image, Uri hyperlink)
      : base(drawings, node, "xdr:pic/xdr:nvPicPr/xdr:cNvPr/@name")
    {
      XmlElement element = node.OwnerDocument.CreateElement("xdr", "pic", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      node.InsertAfter((XmlNode) element, node.SelectSingleNode("xdr:to", this.NameSpaceManager));
      this._hyperlink = hyperlink;
      element.InnerXml = this.PicStartXml();
      node.InsertAfter((XmlNode) node.OwnerDocument.CreateElement("xdr", "clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing"), (XmlNode) element);
      ZipPackage package = drawings.Worksheet._package.Package;
      this._image = image;
      string str = this.SavePicture(image);
      node.SelectSingleNode("xdr:pic/xdr:blipFill/a:blip/@r:embed", this.NameSpaceManager).Value = str;
      this.SetPosDefaults(image);
      package.Flush();
    }

    internal ExcelPicture(ExcelDrawings drawings, XmlNode node, FileInfo imageFile)
      : this(drawings, node, imageFile, (Uri) null)
    {
    }

    internal ExcelPicture(ExcelDrawings drawings, XmlNode node, FileInfo imageFile, Uri hyperlink)
      : base(drawings, node, "xdr:pic/xdr:nvPicPr/xdr:cNvPr/@name")
    {
      XmlElement element = node.OwnerDocument.CreateElement("xdr", "pic", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      node.InsertAfter((XmlNode) element, node.SelectSingleNode("xdr:to", this.NameSpaceManager));
      this._hyperlink = hyperlink;
      element.InnerXml = this.PicStartXml();
      node.InsertAfter((XmlNode) node.OwnerDocument.CreateElement("xdr", "clientData", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing"), (XmlNode) element);
      ZipPackage package = drawings.Worksheet._package.Package;
      this.ContentType = ExcelPicture.GetContentType(imageFile.Extension);
      FileStream fileStream = new FileStream(imageFile.FullName, FileMode.Open, FileAccess.Read);
      this._image = Image.FromStream((Stream) fileStream);
      byte[] numArray = (byte[]) new ImageConverter().ConvertTo((object) this._image, typeof (byte[]));
      fileStream.Close();
      this.UriPic = XmlHelper.GetNewUri(package, "/xl/media/{0}" + imageFile.Name);
      ExcelPackage.ImageInfo imageInfo = this._drawings._package.AddImage(numArray, this.UriPic, this.ContentType);
      string str;
      if (!drawings._hashes.ContainsKey(imageInfo.Hash))
      {
        this.Part = imageInfo.Part;
        this.RelPic = drawings.Part.CreateRelationship(UriHelper.GetRelativeUri(drawings.UriDrawing, imageInfo.Uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
        str = this.RelPic.Id;
        this._drawings._hashes.Add(imageInfo.Hash, str);
        this.AddNewPicture(numArray, str);
      }
      else
      {
        str = drawings._hashes[imageInfo.Hash];
        ZipPackageRelationship relationship = this._drawings.Part.GetRelationship(str);
        this.UriPic = UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri);
      }
      this.SetPosDefaults(this.Image);
      node.SelectSingleNode("xdr:pic/xdr:blipFill/a:blip/@r:embed", this.NameSpaceManager).Value = str;
      package.Flush();
    }

    internal static string GetContentType(string extension)
    {
      switch (extension.ToLower())
      {
        case ".bmp":
          return "image/bmp";
        case ".jpg":
        case ".jpeg":
          return "image/jpeg";
        case ".gif":
          return "image/gif";
        case ".png":
          return "image/png";
        case ".cgm":
          return "image/cgm";
        case ".emf":
          return "image/x-emf";
        case ".eps":
          return "image/x-eps";
        case ".pcx":
          return "image/x-pcx";
        case ".tga":
          return "image/x-tga";
        case ".tif":
        case ".tiff":
          return "image/x-tiff";
        case ".wmf":
          return "image/x-wmf";
        default:
          return "image/jpeg";
      }
    }

    private void AddNewPicture(byte[] img, string relID)
    {
      ExcelDrawings.ImageCompare imageCompare = new ExcelDrawings.ImageCompare()
      {
        image = img,
        relID = relID
      };
    }

    private string SavePicture(Image image)
    {
      ExcelPackage.ImageInfo imageInfo = this._drawings._package.AddImage((byte[]) new ImageConverter().ConvertTo((object) image, typeof (byte[])));
      if (this._drawings._hashes.ContainsKey(imageInfo.Hash))
      {
        string hash = this._drawings._hashes[imageInfo.Hash];
        ZipPackageRelationship relationship = this._drawings.Part.GetRelationship(hash);
        this.UriPic = UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri);
        return hash;
      }
      this.UriPic = imageInfo.Uri;
      this.RelPic = this._drawings.Part.CreateRelationship(UriHelper.GetRelativeUri(this._drawings.UriDrawing, this.UriPic), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
      this._drawings._hashes.Add(imageInfo.Hash, this.RelPic.Id);
      this.ImageHash = imageInfo.Hash;
      return this.RelPic.Id;
    }

    private void SetPosDefaults(Image image)
    {
      this.EditAs = eEditAs.OneCell;
      this.SetPixelWidth(image.Width, image.HorizontalResolution);
      this.SetPixelHeight(image.Height, image.VerticalResolution);
    }

    private string PicStartXml()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("<xdr:nvPicPr>");
      if (this._hyperlink == (Uri) null)
      {
        stringBuilder.AppendFormat("<xdr:cNvPr id=\"{0}\" descr=\"\" />", (object) this._id);
      }
      else
      {
        this.HypRel = this._drawings.Part.CreateRelationship(this._hyperlink, TargetMode.External, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink");
        stringBuilder.AppendFormat("<xdr:cNvPr id=\"{0}\" descr=\"\">", (object) this._id);
        if (this.HypRel != null)
        {
          if (this._hyperlink is ExcelHyperLink)
            stringBuilder.AppendFormat("<a:hlinkClick xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" r:id=\"{0}\" tooltip=\"{1}\"/>", (object) this.HypRel.Id, (object) ((ExcelHyperLink) this._hyperlink).ToolTip);
          else
            stringBuilder.AppendFormat("<a:hlinkClick xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" r:id=\"{0}\" />", (object) this.HypRel.Id);
        }
        stringBuilder.Append("</xdr:cNvPr>");
      }
      stringBuilder.Append("<xdr:cNvPicPr><a:picLocks noChangeAspect=\"1\" /></xdr:cNvPicPr></xdr:nvPicPr><xdr:blipFill><a:blip xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" r:embed=\"\" cstate=\"print\" /><a:stretch><a:fillRect /> </a:stretch> </xdr:blipFill> <xdr:spPr> <a:xfrm> <a:off x=\"0\" y=\"0\" />  <a:ext cx=\"0\" cy=\"0\" /> </a:xfrm> <a:prstGeom prst=\"rect\"> <a:avLst /> </a:prstGeom> </xdr:spPr>");
      return stringBuilder.ToString();
    }

    internal string ImageHash { get; set; }

    public Image Image
    {
      get => this._image;
      set
      {
        if (value == null)
          return;
        this._image = value;
        try
        {
          string str = this.SavePicture(value);
          this.TopNode.SelectSingleNode("xdr:pic/xdr:blipFill/a:blip/@r:embed", this.NameSpaceManager).Value = str;
        }
        catch (Exception ex)
        {
          throw new Exception("Can't save image - " + ex.Message, ex);
        }
      }
    }

    public ImageFormat ImageFormat
    {
      get => this._imageFormat;
      internal set => this._imageFormat = value;
    }

    internal string ContentType { get; set; }

    public override void SetSize(int Percent)
    {
      if (this.Image == null)
      {
        base.SetSize(Percent);
      }
      else
      {
        int width = this.Image.Width;
        int height = this.Image.Height;
        int pixels1 = (int) ((Decimal) width * ((Decimal) Percent / 100M));
        int pixels2 = (int) ((Decimal) height * ((Decimal) Percent / 100M));
        this.SetPixelWidth(pixels1, this.Image.HorizontalResolution);
        this.SetPixelHeight(pixels2, this.Image.VerticalResolution);
      }
    }

    internal Uri UriPic { get; set; }

    internal ZipPackageRelationship RelPic { get; set; }

    internal ZipPackageRelationship HypRel { get; set; }

    internal new string Id => this.Name;

    public ExcelDrawingFill Fill
    {
      get
      {
        if (this._fill == null)
          this._fill = new ExcelDrawingFill(this.NameSpaceManager, this.TopNode, "xdr:pic/xdr:spPr");
        return this._fill;
      }
    }

    public ExcelDrawingBorder Border
    {
      get
      {
        if (this._border == null)
          this._border = new ExcelDrawingBorder(this.NameSpaceManager, this.TopNode, "xdr:pic/xdr:spPr/a:ln");
        return this._border;
      }
    }

    public Uri Hyperlink => this._hyperlink;

    internal override void DeleteMe()
    {
      this._drawings._package.RemoveImage(this.ImageHash);
      base.DeleteMe();
    }

    public override void Dispose()
    {
      base.Dispose();
      this._hyperlink = (Uri) null;
      this._image.Dispose();
      this._image = (Image) null;
    }
  }
}
