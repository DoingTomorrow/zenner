// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Style.XmlAccess.ExcelXfs
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Drawing;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Style.XmlAccess
{
  public sealed class ExcelXfs : StyleXmlHelper
  {
    private const string horizontalAlignPath = "d:alignment/@horizontal";
    private const string verticalAlignPath = "d:alignment/@vertical";
    private const string wrapTextPath = "d:alignment/@wrapText";
    private const string lockedPath = "d:protection/@locked";
    private const string hiddenPath = "d:protection/@hidden";
    private const string readingOrderPath = "d:alignment/@readingOrder";
    private const string shrinkToFitPath = "d:alignment/@shrinkToFit";
    private const string indentPath = "d:alignment/@indent";
    private ExcelStyles _styles;
    private int _xfID;
    private int _numFmtId;
    private int _fontId;
    private int _fillId;
    private int _borderId;
    private ExcelHorizontalAlignment _horizontalAlignment;
    private ExcelVerticalAlignment _verticalAlignment = ExcelVerticalAlignment.Bottom;
    private bool _wrapText;
    private string textRotationPath = "d:alignment/@textRotation";
    private int _textRotation;
    private bool _locked = true;
    private bool _hidden;
    private ExcelReadingOrder _readingOrder;
    private bool _shrinkToFit;
    private int _indent;

    internal ExcelXfs(XmlNamespaceManager nameSpaceManager, ExcelStyles styles)
      : base(nameSpaceManager)
    {
      this._styles = styles;
      this.isBuildIn = false;
    }

    internal ExcelXfs(XmlNamespaceManager nsm, XmlNode topNode, ExcelStyles styles)
      : base(nsm, topNode)
    {
      this._styles = styles;
      this._xfID = this.GetXmlNodeInt("@xfId");
      if (this._xfID == 0)
        this.isBuildIn = true;
      this._numFmtId = this.GetXmlNodeInt("@numFmtId");
      this._fontId = this.GetXmlNodeInt("@fontId");
      this._fillId = this.GetXmlNodeInt("@fillId");
      this._borderId = this.GetXmlNodeInt("@borderId");
      this._readingOrder = this.GetReadingOrder(this.GetXmlNodeString("d:alignment/@readingOrder"));
      this._indent = this.GetXmlNodeInt("d:alignment/@indent");
      this._shrinkToFit = this.GetXmlNodeString("d:alignment/@shrinkToFit") == "1";
      this._verticalAlignment = this.GetVerticalAlign(this.GetXmlNodeString("d:alignment/@vertical"));
      this._horizontalAlignment = this.GetHorizontalAlign(this.GetXmlNodeString("d:alignment/@horizontal"));
      this._wrapText = this.GetXmlNodeString("d:alignment/@wrapText") == "1";
      this._textRotation = this.GetXmlNodeInt(this.textRotationPath);
      this._hidden = this.GetXmlNodeBool("d:protection/@hidden");
      this._locked = this.GetXmlNodeBool("d:protection/@locked", true);
    }

    private ExcelReadingOrder GetReadingOrder(string value)
    {
      switch (value)
      {
        case "1":
          return ExcelReadingOrder.LeftToRight;
        case "2":
          return ExcelReadingOrder.RightToLeft;
        default:
          return ExcelReadingOrder.ContextDependent;
      }
    }

    private ExcelHorizontalAlignment GetHorizontalAlign(string align)
    {
      if (align == "")
        return ExcelHorizontalAlignment.General;
      align = align.Substring(0, 1).ToUpper() + align.Substring(1, align.Length - 1);
      try
      {
        return (ExcelHorizontalAlignment) Enum.Parse(typeof (ExcelHorizontalAlignment), align);
      }
      catch
      {
        return ExcelHorizontalAlignment.General;
      }
    }

    private ExcelVerticalAlignment GetVerticalAlign(string align)
    {
      if (align == "")
        return ExcelVerticalAlignment.Bottom;
      align = align.Substring(0, 1).ToUpper() + align.Substring(1, align.Length - 1);
      try
      {
        return (ExcelVerticalAlignment) Enum.Parse(typeof (ExcelVerticalAlignment), align);
      }
      catch
      {
        return ExcelVerticalAlignment.Bottom;
      }
    }

    internal void Xf_ChangedEvent(object sender, EventArgs e)
    {
    }

    public int XfId
    {
      get => this._xfID;
      set => this._xfID = value;
    }

    internal int NumberFormatId
    {
      get => this._numFmtId;
      set
      {
        this._numFmtId = value;
        this.ApplyNumberFormat = value > 0;
      }
    }

    internal int FontId
    {
      get => this._fontId;
      set => this._fontId = value;
    }

    internal int FillId
    {
      get => this._fillId;
      set => this._fillId = value;
    }

    internal int BorderId
    {
      get => this._borderId;
      set => this._borderId = value;
    }

    private bool isBuildIn { get; set; }

    internal bool ApplyNumberFormat { get; set; }

    internal bool ApplyFont { get; set; }

    internal bool ApplyFill { get; set; }

    internal bool ApplyBorder { get; set; }

    internal bool ApplyAlignment { get; set; }

    internal bool ApplyProtection { get; set; }

    public ExcelStyles Styles { get; private set; }

    public ExcelNumberFormatXml Numberformat
    {
      get => this._styles.NumberFormats[this._numFmtId < 0 ? 0 : this._numFmtId];
    }

    public ExcelFontXml Font => this._styles.Fonts[this._fontId < 0 ? 0 : this._fontId];

    public ExcelFillXml Fill => this._styles.Fills[this._fillId < 0 ? 0 : this._fillId];

    public ExcelBorderXml Border => this._styles.Borders[this._borderId < 0 ? 0 : this._borderId];

    public ExcelHorizontalAlignment HorizontalAlignment
    {
      get => this._horizontalAlignment;
      set => this._horizontalAlignment = value;
    }

    public ExcelVerticalAlignment VerticalAlignment
    {
      get => this._verticalAlignment;
      set => this._verticalAlignment = value;
    }

    public bool WrapText
    {
      get => this._wrapText;
      set => this._wrapText = value;
    }

    public int TextRotation
    {
      get => this._textRotation;
      set => this._textRotation = value;
    }

    public bool Locked
    {
      get => this._locked;
      set => this._locked = value;
    }

    public bool Hidden
    {
      get => this._hidden;
      set => this._hidden = value;
    }

    public ExcelReadingOrder ReadingOrder
    {
      get => this._readingOrder;
      set => this._readingOrder = value;
    }

    public bool ShrinkToFit
    {
      get => this._shrinkToFit;
      set => this._shrinkToFit = value;
    }

    public int Indent
    {
      get => this._indent;
      set => this._indent = value;
    }

    internal void RegisterEvent(ExcelXfs xf)
    {
    }

    internal override string Id
    {
      get
      {
        return this.XfId.ToString() + "|" + this.NumberFormatId.ToString() + "|" + this.FontId.ToString() + "|" + this.FillId.ToString() + "|" + this.BorderId.ToString() + this.VerticalAlignment.ToString() + "|" + this.HorizontalAlignment.ToString() + "|" + this.WrapText.ToString() + "|" + this.ReadingOrder.ToString() + "|" + this.isBuildIn.ToString() + this.TextRotation.ToString() + this.Locked.ToString() + this.Hidden.ToString() + this.ShrinkToFit.ToString() + this.Indent.ToString();
      }
    }

    internal ExcelXfs Copy() => this.Copy(this._styles);

    internal ExcelXfs Copy(ExcelStyles styles)
    {
      return new ExcelXfs(this.NameSpaceManager, styles)
      {
        NumberFormatId = this._numFmtId,
        FontId = this._fontId,
        FillId = this._fillId,
        BorderId = this._borderId,
        XfId = this._xfID,
        ReadingOrder = this._readingOrder,
        HorizontalAlignment = this._horizontalAlignment,
        VerticalAlignment = this._verticalAlignment,
        WrapText = this._wrapText,
        ShrinkToFit = this._shrinkToFit,
        Indent = this._indent,
        TextRotation = this._textRotation,
        Locked = this._locked,
        Hidden = this._hidden
      };
    }

    internal int GetNewID(
      ExcelStyleCollection<ExcelXfs> xfsCol,
      StyleBase styleObject,
      eStyleClass styleClass,
      eStyleProperty styleProperty,
      object value)
    {
      ExcelXfs excelXfs = this.Copy();
      switch (styleClass)
      {
        case eStyleClass.Numberformat:
          excelXfs.NumberFormatId = this.GetIdNumberFormat(styleProperty, value);
          styleObject.SetIndex(excelXfs.NumberFormatId);
          break;
        case eStyleClass.Font:
          excelXfs.FontId = this.GetIdFont(styleProperty, value);
          styleObject.SetIndex(excelXfs.FontId);
          break;
        case eStyleClass.Border:
        case eStyleClass.BorderTop:
        case eStyleClass.BorderLeft:
        case eStyleClass.BorderBottom:
        case eStyleClass.BorderRight:
        case eStyleClass.BorderDiagonal:
          excelXfs.BorderId = this.GetIdBorder(styleClass, styleProperty, value);
          styleObject.SetIndex(excelXfs.BorderId);
          break;
        case eStyleClass.Fill:
        case eStyleClass.FillBackgroundColor:
        case eStyleClass.FillPatternColor:
          excelXfs.FillId = this.GetIdFill(styleClass, styleProperty, value);
          styleObject.SetIndex(excelXfs.FillId);
          break;
        case eStyleClass.GradientFill:
        case eStyleClass.FillGradientColor1:
        case eStyleClass.FillGradientColor2:
          excelXfs.FillId = this.GetIdGradientFill(styleClass, styleProperty, value);
          styleObject.SetIndex(excelXfs.FillId);
          break;
        case eStyleClass.Style:
          switch (styleProperty)
          {
            case eStyleProperty.HorizontalAlign:
              excelXfs.HorizontalAlignment = (ExcelHorizontalAlignment) value;
              break;
            case eStyleProperty.VerticalAlign:
              excelXfs.VerticalAlignment = (ExcelVerticalAlignment) value;
              break;
            case eStyleProperty.ReadingOrder:
              excelXfs.ReadingOrder = (ExcelReadingOrder) value;
              break;
            case eStyleProperty.WrapText:
              excelXfs.WrapText = (bool) value;
              break;
            case eStyleProperty.TextRotation:
              excelXfs.TextRotation = (int) value;
              break;
            case eStyleProperty.Locked:
              excelXfs.Locked = (bool) value;
              break;
            case eStyleProperty.Hidden:
              excelXfs.Hidden = (bool) value;
              break;
            case eStyleProperty.ShrinkToFit:
              excelXfs.ShrinkToFit = (bool) value;
              break;
            case eStyleProperty.XfId:
              excelXfs.XfId = (int) value;
              break;
            case eStyleProperty.Indent:
              excelXfs.Indent = (int) value;
              break;
            default:
              throw new Exception("Invalid property for class style.");
          }
          break;
      }
      int indexById = xfsCol.FindIndexByID(excelXfs.Id);
      return indexById < 0 ? xfsCol.Add(excelXfs.Id, excelXfs) : indexById;
    }

    private int GetIdBorder(eStyleClass styleClass, eStyleProperty styleProperty, object value)
    {
      ExcelBorderXml excelBorderXml = this.Border.Copy();
      switch (styleClass)
      {
        case eStyleClass.Border:
          if (styleProperty == eStyleProperty.BorderDiagonalUp)
          {
            excelBorderXml.DiagonalUp = (bool) value;
            break;
          }
          if (styleProperty != eStyleProperty.BorderDiagonalDown)
            throw new Exception("Invalid property for class Border.");
          excelBorderXml.DiagonalDown = (bool) value;
          break;
        case eStyleClass.BorderTop:
          this.SetBorderItem(excelBorderXml.Top, styleProperty, value);
          break;
        case eStyleClass.BorderLeft:
          this.SetBorderItem(excelBorderXml.Left, styleProperty, value);
          break;
        case eStyleClass.BorderBottom:
          this.SetBorderItem(excelBorderXml.Bottom, styleProperty, value);
          break;
        case eStyleClass.BorderRight:
          this.SetBorderItem(excelBorderXml.Right, styleProperty, value);
          break;
        case eStyleClass.BorderDiagonal:
          this.SetBorderItem(excelBorderXml.Diagonal, styleProperty, value);
          break;
        default:
          throw new Exception("Invalid class/property for class Border.");
      }
      string id = excelBorderXml.Id;
      int indexById = this._styles.Borders.FindIndexByID(id);
      return indexById == int.MinValue ? this._styles.Borders.Add(id, excelBorderXml) : indexById;
    }

    private void SetBorderItem(
      ExcelBorderItemXml excelBorderItem,
      eStyleProperty styleProperty,
      object value)
    {
      switch (styleProperty)
      {
        case eStyleProperty.Color:
        case eStyleProperty.Tint:
        case eStyleProperty.IndexedColor:
          if (excelBorderItem.Style == ExcelBorderStyle.None)
            throw new Exception("Can't set bordercolor when style is not set.");
          excelBorderItem.Color.Rgb = value.ToString();
          break;
        case eStyleProperty.Style:
          excelBorderItem.Style = (ExcelBorderStyle) value;
          break;
      }
    }

    private int GetIdFill(eStyleClass styleClass, eStyleProperty styleProperty, object value)
    {
      ExcelFillXml excelFillXml = this.Fill.Copy();
      switch (styleProperty)
      {
        case eStyleProperty.Color:
        case eStyleProperty.Tint:
        case eStyleProperty.IndexedColor:
        case eStyleProperty.AutoColor:
          if (excelFillXml is ExcelGradientFillXml)
            excelFillXml = new ExcelFillXml(this.NameSpaceManager);
          if (excelFillXml.PatternType == ExcelFillStyle.None)
            throw new ArgumentException("Can't set color when patterntype is not set.");
          ExcelColorXml excelColorXml = styleClass != eStyleClass.FillPatternColor ? excelFillXml.BackgroundColor : excelFillXml.PatternColor;
          switch (styleProperty)
          {
            case eStyleProperty.Color:
              excelColorXml.Rgb = value.ToString();
              break;
            case eStyleProperty.Tint:
              excelColorXml.Tint = (Decimal) value;
              break;
            case eStyleProperty.IndexedColor:
              excelColorXml.Indexed = (int) value;
              break;
            default:
              excelColorXml.Auto = (bool) value;
              break;
          }
          break;
        case eStyleProperty.PatternType:
          if (excelFillXml is ExcelGradientFillXml)
            excelFillXml = new ExcelFillXml(this.NameSpaceManager);
          excelFillXml.PatternType = (ExcelFillStyle) value;
          break;
        default:
          throw new ArgumentException("Invalid class/property for class Fill.");
      }
      string id = excelFillXml.Id;
      int indexById = this._styles.Fills.FindIndexByID(id);
      return indexById == int.MinValue ? this._styles.Fills.Add(id, excelFillXml) : indexById;
    }

    private int GetIdGradientFill(
      eStyleClass styleClass,
      eStyleProperty styleProperty,
      object value)
    {
      ExcelGradientFillXml excelGradientFillXml;
      if (this.Fill is ExcelGradientFillXml)
      {
        excelGradientFillXml = (ExcelGradientFillXml) this.Fill.Copy();
      }
      else
      {
        excelGradientFillXml = new ExcelGradientFillXml(this.Fill.NameSpaceManager);
        excelGradientFillXml.GradientColor1.SetColor(Color.White);
        excelGradientFillXml.GradientColor2.SetColor(Color.FromArgb(79, 129, 189));
        excelGradientFillXml.Type = ExcelFillGradientType.Linear;
        excelGradientFillXml.Degree = 90.0;
        excelGradientFillXml.Top = double.NaN;
        excelGradientFillXml.Bottom = double.NaN;
        excelGradientFillXml.Left = double.NaN;
        excelGradientFillXml.Right = double.NaN;
      }
      switch (styleProperty)
      {
        case eStyleProperty.Color:
        case eStyleProperty.Tint:
        case eStyleProperty.IndexedColor:
        case eStyleProperty.AutoColor:
          ExcelColorXml excelColorXml = styleClass != eStyleClass.FillGradientColor1 ? excelGradientFillXml.GradientColor2 : excelGradientFillXml.GradientColor1;
          switch (styleProperty)
          {
            case eStyleProperty.Color:
              excelColorXml.Rgb = value.ToString();
              break;
            case eStyleProperty.Tint:
              excelColorXml.Tint = (Decimal) value;
              break;
            case eStyleProperty.IndexedColor:
              excelColorXml.Indexed = (int) value;
              break;
            default:
              excelColorXml.Auto = (bool) value;
              break;
          }
          break;
        case eStyleProperty.GradientDegree:
          excelGradientFillXml.Degree = (double) value;
          break;
        case eStyleProperty.GradientType:
          excelGradientFillXml.Type = (ExcelFillGradientType) value;
          break;
        case eStyleProperty.GradientTop:
          excelGradientFillXml.Top = (double) value;
          break;
        case eStyleProperty.GradientBottom:
          excelGradientFillXml.Bottom = (double) value;
          break;
        case eStyleProperty.GradientLeft:
          excelGradientFillXml.Left = (double) value;
          break;
        case eStyleProperty.GradientRight:
          excelGradientFillXml.Right = (double) value;
          break;
        default:
          throw new ArgumentException("Invalid class/property for class Fill.");
      }
      string id = excelGradientFillXml.Id;
      int indexById = this._styles.Fills.FindIndexByID(id);
      return indexById == int.MinValue ? this._styles.Fills.Add(id, (ExcelFillXml) excelGradientFillXml) : indexById;
    }

    private int GetIdNumberFormat(eStyleProperty styleProperty, object value)
    {
      if (styleProperty != eStyleProperty.Format)
        throw new Exception("Invalid property for class Numberformat");
      ExcelNumberFormatXml excelNumberFormatXml = (ExcelNumberFormatXml) null;
      if (!this._styles.NumberFormats.FindByID(value.ToString(), ref excelNumberFormatXml))
      {
        excelNumberFormatXml = new ExcelNumberFormatXml(this.NameSpaceManager)
        {
          Format = value.ToString(),
          NumFmtId = this._styles.NumberFormats.NextId++
        };
        this._styles.NumberFormats.Add(value.ToString(), excelNumberFormatXml);
      }
      return excelNumberFormatXml.NumFmtId;
    }

    private int GetIdFont(eStyleProperty styleProperty, object value)
    {
      ExcelFontXml excelFontXml = this.Font.Copy();
      switch (styleProperty)
      {
        case eStyleProperty.Name:
          excelFontXml.Name = value.ToString();
          break;
        case eStyleProperty.Size:
          excelFontXml.Size = (float) value;
          break;
        case eStyleProperty.Bold:
          excelFontXml.Bold = (bool) value;
          break;
        case eStyleProperty.Italic:
          excelFontXml.Italic = (bool) value;
          break;
        case eStyleProperty.Strike:
          excelFontXml.Strike = (bool) value;
          break;
        case eStyleProperty.Color:
          excelFontXml.Color.Rgb = value.ToString();
          break;
        case eStyleProperty.Family:
          excelFontXml.Family = (int) value;
          break;
        case eStyleProperty.UnderlineType:
          excelFontXml.UnderLineType = (ExcelUnderLineType) value;
          break;
        case eStyleProperty.VerticalAlign:
          excelFontXml.VerticalAlign = (ExcelVerticalAlignmentFont) value == ExcelVerticalAlignmentFont.None ? "" : value.ToString().ToLower();
          break;
        default:
          throw new Exception("Invalid property for class Font");
      }
      string id = excelFontXml.Id;
      int indexById = this._styles.Fonts.FindIndexByID(id);
      return indexById == int.MinValue ? this._styles.Fonts.Add(id, excelFontXml) : indexById;
    }

    internal override XmlNode CreateXmlNode(XmlNode topNode) => this.CreateXmlNode(topNode, false);

    internal XmlNode CreateXmlNode(XmlNode topNode, bool isCellStyleXsf)
    {
      this.TopNode = topNode;
      if (this._numFmtId >= 0)
      {
        this.SetXmlNodeString("@numFmtId", this._numFmtId.ToString());
        this.SetXmlNodeString("@applyNumberFormat", "1");
      }
      if (this._fontId >= 0)
      {
        this.SetXmlNodeString("@fontId", this._styles.Fonts[this._fontId].newID.ToString());
        this.SetXmlNodeString("@applyFont", "1");
      }
      if (this._fillId >= 0)
      {
        this.SetXmlNodeString("@fillId", this._styles.Fills[this._fillId].newID.ToString());
        this.SetXmlNodeString("@applyFill", "1");
      }
      if (this._borderId >= 0)
      {
        this.SetXmlNodeString("@borderId", this._styles.Borders[this._borderId].newID.ToString());
        this.SetXmlNodeString("@applyBorder", "1");
      }
      if (this._horizontalAlignment != ExcelHorizontalAlignment.General)
        this.SetXmlNodeString("d:alignment/@horizontal", this.SetAlignString((Enum) this._horizontalAlignment));
      if (!isCellStyleXsf && this._xfID > int.MinValue && this._styles.CellStyleXfs.Count > 0)
        this.SetXmlNodeString("@xfId", this._styles.CellStyleXfs[this._xfID].newID.ToString());
      if (this._verticalAlignment != ExcelVerticalAlignment.Bottom)
        this.SetXmlNodeString("d:alignment/@vertical", this.SetAlignString((Enum) this._verticalAlignment));
      if (this._wrapText)
        this.SetXmlNodeString("d:alignment/@wrapText", "1");
      if (this._readingOrder != ExcelReadingOrder.ContextDependent)
        this.SetXmlNodeString("d:alignment/@readingOrder", ((int) this._readingOrder).ToString());
      if (this._shrinkToFit)
        this.SetXmlNodeString("d:alignment/@shrinkToFit", "1");
      if (this._indent > 0)
        this.SetXmlNodeString("d:alignment/@indent", this._indent.ToString());
      if (this._textRotation > 0)
        this.SetXmlNodeString(this.textRotationPath, this._textRotation.ToString());
      if (!this._locked)
        this.SetXmlNodeString("d:protection/@locked", "0");
      if (this._hidden)
        this.SetXmlNodeString("d:protection/@hidden", "1");
      return this.TopNode;
    }

    private string SetAlignString(Enum align)
    {
      string name = Enum.GetName(align.GetType(), (object) align);
      return name.Substring(0, 1).ToLower() + name.Substring(1, name.Length - 1);
    }
  }
}
