// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableField
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableField : XmlHelper
  {
    internal ExcelPivotTable _table;
    internal ExcelPivotTablePageFieldSettings _pageFieldSettings;
    private ExcelPivotTableFieldGroup _grouping;
    internal XmlHelperInstance _cacheFieldHelper;
    internal ExcelPivotTableFieldCollectionBase<ExcelPivotTableFieldItem> _items;

    internal ExcelPivotTableField(
      XmlNamespaceManager ns,
      XmlNode topNode,
      ExcelPivotTable table,
      int index,
      int baseIndex)
      : base(ns, topNode)
    {
      this.Index = index;
      this.BaseIndex = baseIndex;
      this._table = table;
    }

    public int Index { get; set; }

    internal int BaseIndex { get; set; }

    public string Name
    {
      get
      {
        return this.GetXmlNodeString("@name") == "" ? this._cacheFieldHelper.GetXmlNodeString("@name") : this.GetXmlNodeString("@name");
      }
      set => this.SetXmlNodeString("@name", value);
    }

    public bool Compact
    {
      get => this.GetXmlNodeBool("@compact");
      set => this.SetXmlNodeBool("@compact", value);
    }

    public bool Outline
    {
      get => this.GetXmlNodeBool("@outline");
      set => this.SetXmlNodeBool("@outline", value);
    }

    public bool SubtotalTop
    {
      get => this.GetXmlNodeBool("@subtotalTop");
      set => this.SetXmlNodeBool("@subtotalTop", value);
    }

    public bool ShowAll
    {
      get => this.GetXmlNodeBool("@showAll");
      set => this.SetXmlNodeBool("@showAll", value);
    }

    public eSortType Sort
    {
      get
      {
        string xmlNodeString = this.GetXmlNodeString("@sortType");
        return !(xmlNodeString == "") ? (eSortType) Enum.Parse(typeof (eSortType), xmlNodeString, true) : eSortType.None;
      }
      set
      {
        if (value == eSortType.None)
          this.DeleteNode("@sortType");
        else
          this.SetXmlNodeString("@sortType", value.ToString().ToLower());
      }
    }

    public bool IncludeNewItemsInFilter
    {
      get => this.GetXmlNodeBool("@includeNewItemsInFilter");
      set => this.SetXmlNodeBool("@includeNewItemsInFilter", value);
    }

    public eSubTotalFunctions SubTotalFunctions
    {
      get
      {
        eSubTotalFunctions subTotalFunctions = (eSubTotalFunctions) 0;
        XmlNodeList xmlNodeList = this.TopNode.SelectNodes("d:items/d:item/@t", this.NameSpaceManager);
        if (xmlNodeList.Count == 0)
          return eSubTotalFunctions.None;
        foreach (XmlAttribute xmlAttribute in xmlNodeList)
        {
          try
          {
            subTotalFunctions |= (eSubTotalFunctions) Enum.Parse(typeof (eSubTotalFunctions), xmlAttribute.Value, true);
          }
          catch (ArgumentException ex)
          {
            throw new ArgumentException("Unable to parse value of " + xmlAttribute.Value + " to a valid pivot table subtotal function", (Exception) ex);
          }
        }
        return subTotalFunctions;
      }
      set
      {
        if ((value & eSubTotalFunctions.None) == eSubTotalFunctions.None && value != eSubTotalFunctions.None)
          throw new ArgumentException("Value None can not be combined with other values.");
        if ((value & eSubTotalFunctions.Default) == eSubTotalFunctions.Default && value != eSubTotalFunctions.Default)
          throw new ArgumentException("Value Default can not be combined with other values.");
        XmlNodeList xmlNodeList = this.TopNode.SelectNodes("d:items/d:item/@t", this.NameSpaceManager);
        if (xmlNodeList.Count > 0)
        {
          foreach (XmlAttribute xmlAttribute in xmlNodeList)
          {
            this.DeleteNode("@" + xmlAttribute.Value + "Subtotal");
            xmlAttribute.OwnerElement.ParentNode.RemoveChild((XmlNode) xmlAttribute.OwnerElement);
          }
        }
        if (value == eSubTotalFunctions.None)
        {
          this.SetXmlNodeBool("@defaultSubtotal", false);
          this.TopNode.InnerXml = "";
        }
        else
        {
          string str1 = "";
          int num = 0;
          foreach (eSubTotalFunctions subTotalFunctions in Enum.GetValues(typeof (eSubTotalFunctions)))
          {
            if ((value & subTotalFunctions) == subTotalFunctions)
            {
              string str2 = subTotalFunctions.ToString();
              string str3 = char.ToLower(str2[0]).ToString() + str2.Substring(1);
              this.SetXmlNodeBool("@" + str3 + "Subtotal", true);
              str1 = str1 + "<item t=\"" + str3 + "\" />";
              ++num;
            }
          }
          this.TopNode.InnerXml = string.Format("<items count=\"{0}\">{1}</items>", (object) num, (object) str1);
        }
      }
    }

    public ePivotFieldAxis Axis
    {
      get
      {
        switch (this.GetXmlNodeString("@axis"))
        {
          case "axisRow":
            return ePivotFieldAxis.Row;
          case "axisCol":
            return ePivotFieldAxis.Column;
          case "axisPage":
            return ePivotFieldAxis.Page;
          case "axisValues":
            return ePivotFieldAxis.Values;
          default:
            return ePivotFieldAxis.None;
        }
      }
      internal set
      {
        switch (value)
        {
          case ePivotFieldAxis.Column:
            this.SetXmlNodeString("@axis", "axisCol");
            break;
          case ePivotFieldAxis.Page:
            this.SetXmlNodeString("@axis", "axisPage");
            break;
          case ePivotFieldAxis.Row:
            this.SetXmlNodeString("@axis", "axisRow");
            break;
          case ePivotFieldAxis.Values:
            this.SetXmlNodeString("@axis", "axisValues");
            break;
          default:
            this.DeleteNode("@axis");
            break;
        }
      }
    }

    public bool IsRowField
    {
      get
      {
        return this.TopNode.SelectSingleNode(string.Format("../../d:rowFields/d:field[@x={0}]", (object) this.Index), this.NameSpaceManager) != null;
      }
      internal set
      {
        if (value)
        {
          if (this.TopNode.SelectSingleNode("../../d:rowFields", this.NameSpaceManager) == null)
            this._table.CreateNode("d:rowFields");
          this.AppendField(this.TopNode.SelectSingleNode("../../d:rowFields", this.NameSpaceManager), this.Index, "field", "x");
          if (this.BaseIndex == this.Index)
            this.TopNode.InnerXml = "<items count=\"1\"><item t=\"default\" /></items>";
          else
            this.TopNode.InnerXml = "<items count=\"0\"></items>";
        }
        else
        {
          if (!(this.TopNode.SelectSingleNode(string.Format("../../d:rowFields/d:field[@x={0}]", (object) this.Index), this.NameSpaceManager) is XmlElement oldChild))
            return;
          oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
        }
      }
    }

    public bool IsColumnField
    {
      get
      {
        return this.TopNode.SelectSingleNode(string.Format("../../d:colFields/d:field[@x={0}]", (object) this.Index), this.NameSpaceManager) != null;
      }
      internal set
      {
        if (value)
        {
          if (this.TopNode.SelectSingleNode("../../d:colFields", this.NameSpaceManager) == null)
            this._table.CreateNode("d:colFields");
          this.AppendField(this.TopNode.SelectSingleNode("../../d:colFields", this.NameSpaceManager), this.Index, "field", "x");
          if (this.BaseIndex == this.Index)
            this.TopNode.InnerXml = "<items count=\"1\"><item t=\"default\" /></items>";
          else
            this.TopNode.InnerXml = "<items count=\"0\"></items>";
        }
        else
        {
          if (!(this.TopNode.SelectSingleNode(string.Format("../../d:colFields/d:field[@x={0}]", (object) this.Index), this.NameSpaceManager) is XmlElement oldChild))
            return;
          oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
        }
      }
    }

    public bool IsDataField => this.GetXmlNodeBool("@dataField", false);

    public bool IsPageField
    {
      get => this.Axis == ePivotFieldAxis.Page;
      internal set
      {
        if (value)
        {
          XmlNode rowsNode = this.TopNode.SelectSingleNode("../../d:pageFields", this.NameSpaceManager);
          if (rowsNode == null)
          {
            this._table.CreateNode("d:pageFields");
            rowsNode = this.TopNode.SelectSingleNode("../../d:pageFields", this.NameSpaceManager);
          }
          this.TopNode.InnerXml = "<items count=\"1\"><item t=\"default\" /></items>";
          this._pageFieldSettings = new ExcelPivotTablePageFieldSettings(this.NameSpaceManager, (XmlNode) this.AppendField(rowsNode, this.Index, "pageField", "fld"), this, this.Index);
        }
        else
        {
          this._pageFieldSettings = (ExcelPivotTablePageFieldSettings) null;
          if (!(this.TopNode.SelectSingleNode(string.Format("../../d:pageFields/d:pageField[@fld={0}]", (object) this.Index), this.NameSpaceManager) is XmlElement oldChild))
            return;
          oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
        }
      }
    }

    public ExcelPivotTablePageFieldSettings PageFieldSettings => this._pageFieldSettings;

    internal eDateGroupBy DateGrouping { get; set; }

    public ExcelPivotTableFieldGroup Grouping => this._grouping;

    internal XmlElement AppendField(
      XmlNode rowsNode,
      int index,
      string fieldNodeText,
      string indexAttrText)
    {
      XmlElement refChild = (XmlElement) null;
      foreach (XmlElement childNode in rowsNode.ChildNodes)
      {
        int result;
        if (int.TryParse(childNode.GetAttribute(indexAttrText), out result))
        {
          if (result == index)
            return childNode;
          if (result > index)
          {
            XmlElement element = rowsNode.OwnerDocument.CreateElement(fieldNodeText, "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            element.SetAttribute(indexAttrText, index.ToString());
            rowsNode.InsertAfter((XmlNode) element, (XmlNode) childNode);
          }
        }
        refChild = childNode;
      }
      XmlElement element1 = rowsNode.OwnerDocument.CreateElement(fieldNodeText, "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element1.SetAttribute(indexAttrText, index.ToString());
      rowsNode.InsertAfter((XmlNode) element1, (XmlNode) refChild);
      return element1;
    }

    internal void SetCacheFieldNode(XmlNode cacheField)
    {
      this._cacheFieldHelper = new XmlHelperInstance(this.NameSpaceManager, cacheField);
      XmlNode topNode = cacheField.SelectSingleNode("d:fieldGroup", this.NameSpaceManager);
      if (topNode == null)
        return;
      XmlNode xmlNode = topNode.SelectSingleNode("d:rangePr/@groupBy", this.NameSpaceManager);
      if (xmlNode == null)
      {
        this._grouping = (ExcelPivotTableFieldGroup) new ExcelPivotTableFieldNumericGroup(this.NameSpaceManager, cacheField);
      }
      else
      {
        this.DateGrouping = (eDateGroupBy) Enum.Parse(typeof (eDateGroupBy), xmlNode.Value, true);
        this._grouping = (ExcelPivotTableFieldGroup) new ExcelPivotTableFieldDateGroup(this.NameSpaceManager, topNode);
      }
    }

    internal ExcelPivotTableFieldDateGroup SetDateGroup(
      eDateGroupBy GroupBy,
      DateTime StartDate,
      DateTime EndDate,
      int interval)
    {
      ExcelPivotTableFieldDateGroup group = new ExcelPivotTableFieldDateGroup(this.NameSpaceManager, this._cacheFieldHelper.TopNode);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsDate", true);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsNonDate", false);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsSemiMixedTypes", false);
      group.TopNode.InnerXml += string.Format("<fieldGroup base=\"{0}\"><rangePr groupBy=\"{1}\" /><groupItems /></fieldGroup>", (object) this.BaseIndex, (object) GroupBy.ToString().ToLower());
      if (StartDate.Year < 1900)
      {
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@startDate", "1900-01-01T00:00:00");
      }
      else
      {
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@startDate", StartDate.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture));
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@autoStart", "0");
      }
      if (EndDate == DateTime.MaxValue)
      {
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@endDate", "9999-12-31T00:00:00");
      }
      else
      {
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@endDate", EndDate.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture));
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@autoEnd", "0");
      }
      this.AddFieldItems(this.AddDateGroupItems((ExcelPivotTableFieldGroup) group, GroupBy, StartDate, EndDate, interval));
      this._grouping = (ExcelPivotTableFieldGroup) group;
      return group;
    }

    internal ExcelPivotTableFieldNumericGroup SetNumericGroup(
      double start,
      double end,
      double interval)
    {
      ExcelPivotTableFieldNumericGroup group = new ExcelPivotTableFieldNumericGroup(this.NameSpaceManager, this._cacheFieldHelper.TopNode);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsNumber", true);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsInteger", true);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsSemiMixedTypes", false);
      this._cacheFieldHelper.SetXmlNodeBool("d:sharedItems/@containsString", false);
      group.TopNode.InnerXml += string.Format("<fieldGroup base=\"{0}\"><rangePr autoStart=\"0\" autoEnd=\"0\" startNum=\"{1}\" endNum=\"{2}\" groupInterval=\"{3}\"/><groupItems /></fieldGroup>", (object) this.BaseIndex, (object) start.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) end.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) interval.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.AddFieldItems(this.AddNumericGroupItems(group, start, end, interval));
      this._grouping = (ExcelPivotTableFieldGroup) group;
      return group;
    }

    private int AddNumericGroupItems(
      ExcelPivotTableFieldNumericGroup group,
      double start,
      double end,
      double interval)
    {
      if (interval < 0.0)
        throw new Exception("The interval must be a positiv");
      if (start > end)
        throw new Exception("Then End number must be larger than the Start number");
      XmlElement groupItems = group.TopNode.SelectSingleNode("d:fieldGroup/d:groupItems", group.NameSpaceManager) as XmlElement;
      int num1 = 2;
      double num2 = start;
      double num3 = start + interval;
      this.AddGroupItem(groupItems, "<" + start.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      while (num2 < end)
      {
        this.AddGroupItem(groupItems, string.Format("{0}-{1}", (object) num2.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) num3.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
        num2 = num3;
        num3 += interval;
        ++num1;
      }
      this.AddGroupItem(groupItems, ">" + num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return num1;
    }

    private void AddFieldItems(int items)
    {
      XmlElement refChild = (XmlElement) null;
      XmlElement xmlElement = this.TopNode.SelectSingleNode("d:items", this.NameSpaceManager) as XmlElement;
      for (int index = 0; index < items; ++index)
      {
        XmlElement element = xmlElement.OwnerDocument.CreateElement("item", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element.SetAttribute("x", index.ToString());
        if (refChild == null)
          xmlElement.PrependChild((XmlNode) element);
        else
          xmlElement.InsertAfter((XmlNode) element, (XmlNode) refChild);
        refChild = element;
      }
      xmlElement.SetAttribute("count", (items + 1).ToString());
    }

    private int AddDateGroupItems(
      ExcelPivotTableFieldGroup group,
      eDateGroupBy GroupBy,
      DateTime StartDate,
      DateTime EndDate,
      int interval)
    {
      XmlElement groupItems = group.TopNode.SelectSingleNode("d:fieldGroup/d:groupItems", group.NameSpaceManager) as XmlElement;
      int num = 2;
      this.AddGroupItem(groupItems, "<" + StartDate.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture).Substring(0, 10));
      switch (GroupBy)
      {
        case eDateGroupBy.Years:
          if (StartDate.Year >= 1900 && EndDate != DateTime.MaxValue)
          {
            for (int year = StartDate.Year; year <= EndDate.Year; ++year)
              this.AddGroupItem(groupItems, year.ToString());
            num += EndDate.Year - StartDate.Year + 1;
            break;
          }
          break;
        case eDateGroupBy.Quarters:
          this.AddGroupItem(groupItems, "Qtr1");
          this.AddGroupItem(groupItems, "Qtr2");
          this.AddGroupItem(groupItems, "Qtr3");
          this.AddGroupItem(groupItems, "Qtr4");
          num += 4;
          break;
        case eDateGroupBy.Months:
          this.AddGroupItem(groupItems, "jan");
          this.AddGroupItem(groupItems, "feb");
          this.AddGroupItem(groupItems, "mar");
          this.AddGroupItem(groupItems, "apr");
          this.AddGroupItem(groupItems, "may");
          this.AddGroupItem(groupItems, "jun");
          this.AddGroupItem(groupItems, "jul");
          this.AddGroupItem(groupItems, "aug");
          this.AddGroupItem(groupItems, "sep");
          this.AddGroupItem(groupItems, "oct");
          this.AddGroupItem(groupItems, "nov");
          this.AddGroupItem(groupItems, "dec");
          num += 12;
          break;
        case eDateGroupBy.Days:
          if (interval == 1)
          {
            for (DateTime dateTime = new DateTime(2008, 1, 1); dateTime.Year == 2008; dateTime = dateTime.AddDays(1.0))
              this.AddGroupItem(groupItems, dateTime.ToString("dd-MMM"));
            num += 366;
            break;
          }
          DateTime dateTime1 = StartDate;
          num = 0;
          while (dateTime1 < EndDate)
          {
            this.AddGroupItem(groupItems, dateTime1.ToString("dd-MMM"));
            dateTime1 = dateTime1.AddDays((double) interval);
            ++num;
          }
          break;
        case eDateGroupBy.Hours:
          this.AddTimeSerie(24, groupItems);
          num += 24;
          break;
        case eDateGroupBy.Minutes:
        case eDateGroupBy.Seconds:
          this.AddTimeSerie(60, groupItems);
          num += 60;
          break;
        default:
          throw new Exception("unsupported grouping");
      }
      this.AddGroupItem(groupItems, ">" + EndDate.ToString("s", (IFormatProvider) CultureInfo.InvariantCulture).Substring(0, 10));
      return num;
    }

    private void AddTimeSerie(int count, XmlElement groupItems)
    {
      for (int index = 0; index < count; ++index)
        this.AddGroupItem(groupItems, string.Format("{0:00}", (object) index));
    }

    private void AddGroupItem(XmlElement groupItems, string value)
    {
      XmlElement element = groupItems.OwnerDocument.CreateElement("s", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element.SetAttribute("v", value);
      groupItems.AppendChild((XmlNode) element);
    }

    public ExcelPivotTableFieldCollectionBase<ExcelPivotTableFieldItem> Items
    {
      get
      {
        if (this._items == null)
        {
          this._items = new ExcelPivotTableFieldCollectionBase<ExcelPivotTableFieldItem>(this._table);
          foreach (XmlNode selectNode in this.TopNode.SelectNodes("d:items//d:item", this.NameSpaceManager))
          {
            ExcelPivotTableFieldItem field = new ExcelPivotTableFieldItem(this.NameSpaceManager, selectNode, this);
            if (field.T == "")
              this._items.AddInternal(field);
          }
        }
        return this._items;
      }
    }

    public void AddNumericGrouping(double Start, double End, double Interval)
    {
      this.ValidateGrouping();
      this.SetNumericGroup(Start, End, Interval);
    }

    public void AddDateGrouping(eDateGroupBy groupBy)
    {
      this.AddDateGrouping(groupBy, DateTime.MinValue, DateTime.MaxValue, 1);
    }

    public void AddDateGrouping(eDateGroupBy groupBy, DateTime startDate, DateTime endDate)
    {
      this.AddDateGrouping(groupBy, startDate, endDate, 1);
    }

    public void AddDateGrouping(int days, DateTime startDate, DateTime endDate)
    {
      this.AddDateGrouping(eDateGroupBy.Days, startDate, endDate, days);
    }

    private void AddDateGrouping(
      eDateGroupBy groupBy,
      DateTime startDate,
      DateTime endDate,
      int groupInterval)
    {
      if (groupInterval < 1 || groupInterval >= (int) short.MaxValue)
        throw new ArgumentOutOfRangeException("Group interval is out of range");
      if (groupInterval > 1 && groupBy != eDateGroupBy.Days)
        throw new ArgumentException("Group interval is can only be used when groupBy is Days");
      this.ValidateGrouping();
      bool firstField = true;
      List<ExcelPivotTableField> excelPivotTableFieldList = new List<ExcelPivotTableField>();
      if ((groupBy & eDateGroupBy.Seconds) == eDateGroupBy.Seconds)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Seconds, startDate, endDate, ref firstField));
      if ((groupBy & eDateGroupBy.Minutes) == eDateGroupBy.Minutes)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Minutes, startDate, endDate, ref firstField));
      if ((groupBy & eDateGroupBy.Hours) == eDateGroupBy.Hours)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Hours, startDate, endDate, ref firstField));
      if ((groupBy & eDateGroupBy.Days) == eDateGroupBy.Days)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Days, startDate, endDate, ref firstField, groupInterval));
      if ((groupBy & eDateGroupBy.Months) == eDateGroupBy.Months)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Months, startDate, endDate, ref firstField));
      if ((groupBy & eDateGroupBy.Quarters) == eDateGroupBy.Quarters)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Quarters, startDate, endDate, ref firstField));
      if ((groupBy & eDateGroupBy.Years) == eDateGroupBy.Years)
        excelPivotTableFieldList.Add(this.AddField(eDateGroupBy.Years, startDate, endDate, ref firstField));
      if (excelPivotTableFieldList.Count > 1)
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/@par", (this._table.Fields.Count - 1).ToString());
      if (groupInterval != 1)
        this._cacheFieldHelper.SetXmlNodeString("d:fieldGroup/d:rangePr/@groupInterval", groupInterval.ToString());
      else
        this._cacheFieldHelper.DeleteNode("d:fieldGroup/d:rangePr/@groupInterval");
      this._items = (ExcelPivotTableFieldCollectionBase<ExcelPivotTableFieldItem>) null;
    }

    private void ValidateGrouping()
    {
      if (!this.IsColumnField && !this.IsRowField)
        throw new Exception("Field must be a row or column field");
      foreach (ExcelPivotTableField field in (ExcelPivotTableFieldCollectionBase<ExcelPivotTableField>) this._table.Fields)
      {
        if (field.Grouping != null)
          throw new Exception("Grouping already exists");
      }
    }

    private ExcelPivotTableField AddField(
      eDateGroupBy groupBy,
      DateTime startDate,
      DateTime endDate,
      ref bool firstField)
    {
      return this.AddField(groupBy, startDate, endDate, ref firstField, 1);
    }

    private ExcelPivotTableField AddField(
      eDateGroupBy groupBy,
      DateTime startDate,
      DateTime endDate,
      ref bool firstField,
      int interval)
    {
      if (!firstField)
      {
        XmlNode xmlNode1 = this._table.PivotTableXml.SelectSingleNode("//d:pivotFields", this._table.NameSpaceManager);
        XmlElement element1 = this._table.PivotTableXml.CreateElement("pivotField", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        element1.SetAttribute("compact", "0");
        element1.SetAttribute("outline", "0");
        element1.SetAttribute("showAll", "0");
        element1.SetAttribute("defaultSubtotal", "0");
        xmlNode1.AppendChild((XmlNode) element1);
        ExcelPivotTableField excelPivotTableField = new ExcelPivotTableField(this._table.NameSpaceManager, (XmlNode) element1, this._table, this._table.Fields.Count, this.Index);
        excelPivotTableField.DateGrouping = groupBy;
        XmlNode xmlNode2 = !this.IsRowField ? this.TopNode.SelectSingleNode("../../d:colFields", this.NameSpaceManager) : this.TopNode.SelectSingleNode("../../d:rowFields", this.NameSpaceManager);
        int Index = 0;
        foreach (XmlElement childNode in xmlNode2.ChildNodes)
        {
          int result;
          if (int.TryParse(childNode.GetAttribute("x"), out result) && this._table.Fields[result].BaseIndex == this.BaseIndex)
          {
            XmlElement element2 = xmlNode2.OwnerDocument.CreateElement("field", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            element2.SetAttribute("x", excelPivotTableField.Index.ToString());
            xmlNode2.InsertBefore((XmlNode) element2, (XmlNode) childNode);
            break;
          }
          ++Index;
        }
        if (this.IsRowField)
          this._table.RowFields.Insert(excelPivotTableField, Index);
        else
          this._table.ColumnFields.Insert(excelPivotTableField, Index);
        this._table.Fields.AddInternal(excelPivotTableField);
        this.AddCacheField(excelPivotTableField, startDate, endDate, interval);
        return excelPivotTableField;
      }
      firstField = false;
      this.DateGrouping = groupBy;
      this.Compact = false;
      this.SetDateGroup(groupBy, startDate, endDate, interval);
      return this;
    }

    private void AddCacheField(
      ExcelPivotTableField field,
      DateTime startDate,
      DateTime endDate,
      int interval)
    {
      XmlNode xmlNode = this._table.CacheDefinition.CacheDefinitionXml.SelectSingleNode("//d:cacheFields", this._table.NameSpaceManager);
      XmlElement element = this._table.CacheDefinition.CacheDefinitionXml.CreateElement("cacheField", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      element.SetAttribute("name", field.DateGrouping.ToString());
      element.SetAttribute("databaseField", "0");
      xmlNode.AppendChild((XmlNode) element);
      field.SetCacheFieldNode((XmlNode) element);
      field.SetDateGroup(field.DateGrouping, startDate, endDate, interval);
    }
  }
}
