// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelHeaderFooter
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Drawing.Vml;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.IO;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public sealed class ExcelHeaderFooter : XmlHelper
  {
    public const string PageNumber = "&P";
    public const string NumberOfPages = "&N";
    public const string FontColor = "&K";
    public const string SheetName = "&A";
    public const string FilePath = "&Z";
    public const string FileName = "&F";
    public const string CurrentDate = "&D";
    public const string CurrentTime = "&T";
    public const string Image = "&G";
    public const string OutlineStyle = "&O";
    public const string ShadowStyle = "&H";
    private const string alignWithMarginsPath = "@alignWithMargins";
    private const string differentOddEvenPath = "@differentOddEven";
    private const string differentFirstPath = "@differentFirst";
    internal ExcelHeaderFooterText _oddHeader;
    internal ExcelHeaderFooterText _oddFooter;
    internal ExcelHeaderFooterText _evenHeader;
    internal ExcelHeaderFooterText _evenFooter;
    internal ExcelHeaderFooterText _firstHeader;
    internal ExcelHeaderFooterText _firstFooter;
    private ExcelWorksheet _ws;
    private ExcelVmlDrawingPictureCollection _vmlDrawingsHF;

    internal ExcelHeaderFooter(
      XmlNamespaceManager nameSpaceManager,
      XmlNode topNode,
      ExcelWorksheet ws)
      : base(nameSpaceManager, topNode)
    {
      this._ws = ws;
      this.SchemaNodeOrder = new string[7]
      {
        "headerFooter",
        "oddHeader",
        "oddFooter",
        "evenHeader",
        "evenFooter",
        "firstHeader",
        "firstFooter"
      };
    }

    public bool AlignWithMargins
    {
      get => this.GetXmlNodeBool("@alignWithMargins");
      set => this.SetXmlNodeString("@alignWithMargins", value ? "1" : "0");
    }

    public bool differentOddEven
    {
      get => this.GetXmlNodeBool("@differentOddEven");
      set => this.SetXmlNodeString("@differentOddEven", value ? "1" : "0");
    }

    public bool differentFirst
    {
      get => this.GetXmlNodeBool("@differentFirst");
      set => this.SetXmlNodeString("@differentFirst", value ? "1" : "0");
    }

    public ExcelHeaderFooterText OddHeader
    {
      get
      {
        if (this._oddHeader == null)
          this._oddHeader = new ExcelHeaderFooterText(this.TopNode.SelectSingleNode("d:oddHeader", this.NameSpaceManager), this._ws, "H");
        return this._oddHeader;
      }
    }

    public ExcelHeaderFooterText OddFooter
    {
      get
      {
        if (this._oddFooter == null)
          this._oddFooter = new ExcelHeaderFooterText(this.TopNode.SelectSingleNode("d:oddFooter", this.NameSpaceManager), this._ws, "F");
        return this._oddFooter;
      }
    }

    public ExcelHeaderFooterText EvenHeader
    {
      get
      {
        if (this._evenHeader == null)
        {
          this._evenHeader = new ExcelHeaderFooterText(this.TopNode.SelectSingleNode("d:evenHeader", this.NameSpaceManager), this._ws, "HEVEN");
          this.differentOddEven = true;
        }
        return this._evenHeader;
      }
    }

    public ExcelHeaderFooterText EvenFooter
    {
      get
      {
        if (this._evenFooter == null)
        {
          this._evenFooter = new ExcelHeaderFooterText(this.TopNode.SelectSingleNode("d:evenFooter", this.NameSpaceManager), this._ws, "FEVEN");
          this.differentOddEven = true;
        }
        return this._evenFooter;
      }
    }

    public ExcelHeaderFooterText FirstHeader
    {
      get
      {
        if (this._firstHeader == null)
        {
          this._firstHeader = new ExcelHeaderFooterText(this.TopNode.SelectSingleNode("d:firstHeader", this.NameSpaceManager), this._ws, "HFIRST");
          this.differentFirst = true;
        }
        return this._firstHeader;
      }
    }

    public ExcelHeaderFooterText FirstFooter
    {
      get
      {
        if (this._firstFooter == null)
        {
          this._firstFooter = new ExcelHeaderFooterText(this.TopNode.SelectSingleNode("d:firstFooter", this.NameSpaceManager), this._ws, "FFIRST");
          this.differentFirst = true;
        }
        return this._firstFooter;
      }
    }

    public ExcelVmlDrawingPictureCollection Pictures
    {
      get
      {
        if (this._vmlDrawingsHF == null)
        {
          XmlNode xmlNode = this._ws.WorksheetXml.SelectSingleNode("d:worksheet/d:legacyDrawingHF/@r:id", this.NameSpaceManager);
          if (xmlNode == null)
            this._vmlDrawingsHF = new ExcelVmlDrawingPictureCollection(this._ws._package, this._ws, (Uri) null);
          else if (this._ws.Part.RelationshipExists(xmlNode.Value))
          {
            ZipPackageRelationship relationship = this._ws.Part.GetRelationship(xmlNode.Value);
            this._vmlDrawingsHF = new ExcelVmlDrawingPictureCollection(this._ws._package, this._ws, UriHelper.ResolvePartUri(relationship.SourceUri, relationship.TargetUri));
            this._vmlDrawingsHF.RelId = relationship.Id;
          }
        }
        return this._vmlDrawingsHF;
      }
    }

    internal void Save()
    {
      if (this._oddHeader != null)
        this.SetXmlNodeString("d:oddHeader", this.GetText(this.OddHeader));
      if (this._oddFooter != null)
        this.SetXmlNodeString("d:oddFooter", this.GetText(this.OddFooter));
      if (this.differentOddEven)
      {
        if (this._evenHeader != null)
          this.SetXmlNodeString("d:evenHeader", this.GetText(this.EvenHeader));
        if (this._evenFooter != null)
          this.SetXmlNodeString("d:evenFooter", this.GetText(this.EvenFooter));
      }
      if (!this.differentFirst)
        return;
      if (this._firstHeader != null)
        this.SetXmlNodeString("d:firstHeader", this.GetText(this.FirstHeader));
      if (this._firstFooter == null)
        return;
      this.SetXmlNodeString("d:firstFooter", this.GetText(this.FirstFooter));
    }

    internal void SaveHeaderFooterImages()
    {
      if (this._vmlDrawingsHF == null)
        return;
      if (this._vmlDrawingsHF.Count == 0)
      {
        if (!(this._vmlDrawingsHF.Uri != (Uri) null))
          return;
        this._ws.Part.DeleteRelationship(this._vmlDrawingsHF.RelId);
        this._ws._package.Package.DeletePart(this._vmlDrawingsHF.Uri);
      }
      else
      {
        if (this._vmlDrawingsHF.Uri == (Uri) null)
          this._vmlDrawingsHF.Uri = XmlHelper.GetNewUri(this._ws._package.Package, "/xl/drawings/vmlDrawing{0}.vml");
        if (this._vmlDrawingsHF.Part == null)
        {
          this._vmlDrawingsHF.Part = this._ws._package.Package.CreatePart(this._vmlDrawingsHF.Uri, "application/vnd.openxmlformats-officedocument.vmlDrawing", this._ws._package.Compression);
          ZipPackageRelationship relationship1 = this._ws.Part.CreateRelationship(UriHelper.GetRelativeUri(this._ws.WorksheetUri, this._vmlDrawingsHF.Uri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing");
          this._ws.SetHFLegacyDrawingRel(relationship1.Id);
          this._vmlDrawingsHF.RelId = relationship1.Id;
          foreach (ExcelVmlDrawingPicture vmlDrawingPicture in (IEnumerable) this._vmlDrawingsHF)
          {
            ZipPackageRelationship relationship2 = this._vmlDrawingsHF.Part.CreateRelationship(UriHelper.GetRelativeUri(this._vmlDrawingsHF.Uri, vmlDrawingPicture.ImageUri), TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
            vmlDrawingPicture.RelId = relationship2.Id;
          }
        }
        this._vmlDrawingsHF.VmlDrawingXml.Save((Stream) this._vmlDrawingsHF.Part.GetStream());
      }
    }

    private string GetText(ExcelHeaderFooterText headerFooter)
    {
      string text = "";
      if (headerFooter.LeftAlignedText != null)
        text = text + "&L" + headerFooter.LeftAlignedText;
      if (headerFooter.CenteredText != null)
        text = text + "&C" + headerFooter.CenteredText;
      if (headerFooter.RightAlignedText != null)
        text = text + "&R" + headerFooter.RightAlignedText;
      return text;
    }
  }
}
