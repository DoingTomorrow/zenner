// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelWorkbookView
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelWorkbookView : XmlHelper
  {
    private const string LEFT_PATH = "d:bookViews/d:workbookView/@xWindow";
    private const string TOP_PATH = "d:bookViews/d:workbookView/@yWindow";
    private const string WIDTH_PATH = "d:bookViews/d:workbookView/@windowWidth";
    private const string HEIGHT_PATH = "d:bookViews/d:workbookView/@windowHeight";
    private const string MINIMIZED_PATH = "d:bookViews/d:workbookView/@minimized";
    private const string SHOWVERTICALSCROLL_PATH = "d:bookViews/d:workbookView/@showVerticalScroll";
    private const string SHOWHORIZONTALSCR_PATH = "d:bookViews/d:workbookView/@showHorizontalScroll";
    private const string SHOWSHEETTABS_PATH = "d:bookViews/d:workbookView/@showSheetTabs";
    private const string ACTIVETAB_PATH = "d:bookViews/d:workbookView/@activeTab";

    internal ExcelWorkbookView(XmlNamespaceManager ns, XmlNode node, ExcelWorkbook wb)
      : base(ns, node)
    {
      this.SchemaNodeOrder = wb.SchemaNodeOrder;
    }

    public int Left
    {
      get => this.GetXmlNodeInt("d:bookViews/d:workbookView/@xWindow");
      internal set
      {
        this.SetXmlNodeString("d:bookViews/d:workbookView/@xWindow", value.ToString());
      }
    }

    public int Top
    {
      get => this.GetXmlNodeInt("d:bookViews/d:workbookView/@yWindow");
      internal set
      {
        this.SetXmlNodeString("d:bookViews/d:workbookView/@yWindow", value.ToString());
      }
    }

    public int Width
    {
      get => this.GetXmlNodeInt("d:bookViews/d:workbookView/@windowWidth");
      internal set
      {
        this.SetXmlNodeString("d:bookViews/d:workbookView/@windowWidth", value.ToString());
      }
    }

    public int Height
    {
      get => this.GetXmlNodeInt("d:bookViews/d:workbookView/@windowHeight");
      internal set
      {
        this.SetXmlNodeString("d:bookViews/d:workbookView/@windowHeight", value.ToString());
      }
    }

    public bool Minimized
    {
      get => this.GetXmlNodeBool("d:bookViews/d:workbookView/@minimized");
      set => this.SetXmlNodeString("d:bookViews/d:workbookView/@minimized", value.ToString());
    }

    public bool ShowVerticalScrollBar
    {
      get => this.GetXmlNodeBool("d:bookViews/d:workbookView/@showVerticalScroll", true);
      set => this.SetXmlNodeBool("d:bookViews/d:workbookView/@showVerticalScroll", value, true);
    }

    public bool ShowHorizontalScrollBar
    {
      get => this.GetXmlNodeBool("d:bookViews/d:workbookView/@showHorizontalScroll", true);
      set => this.SetXmlNodeBool("d:bookViews/d:workbookView/@showHorizontalScroll", value, true);
    }

    public bool ShowSheetTabs
    {
      get => this.GetXmlNodeBool("d:bookViews/d:workbookView/@showSheetTabs", true);
      set => this.SetXmlNodeBool("d:bookViews/d:workbookView/@showSheetTabs", value, true);
    }

    public void SetWindowSize(int left, int top, int width, int height)
    {
      this.Left = left;
      this.Top = top;
      this.Width = width;
      this.Height = height;
    }

    public int ActiveTab
    {
      get
      {
        int xmlNodeInt = this.GetXmlNodeInt("d:bookViews/d:workbookView/@activeTab");
        return xmlNodeInt < 0 ? 0 : xmlNodeInt;
      }
      set
      {
        this.SetXmlNodeString("d:bookViews/d:workbookView/@activeTab", value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
    }
  }
}
