// Decompiled with JetBrains decompiler
// Type: MSS.Business.Documents.GeneratePdfFromData
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.Configuration;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Documents
{
  public static class GeneratePdfFromData
  {
    public static RadDocument GeneratePdfFromMeterReadingValues(
      string meterSerialNumber,
      List<string[]> readingValues)
    {
      RadDocument meterReadingValues = new RadDocument();
      DateTime now = DateTime.Now;
      string spanData = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(now.DayOfWeek) + ", " + (object) now.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(now.Month) + " " + (object) now.Year + " " + string.Format("{0:D2}", (object) now.Hour) + ":" + string.Format("{0:D2}", (object) now.Minute);
      Section section = new Section();
      meterReadingValues.Sections.Add(section);
      Table table1 = new Table(1, 2);
      GeneratePdfFromData.AddCellDataSpan(table1.Rows.First.Cells.First, "SN: " + meterSerialNumber, Colors.White, Colors.Black, RadTextAlignment.Left, false, new TableWidthUnit(TableWidthUnitType.Percent, 35.0), FontWeights.Normal);
      GeneratePdfFromData.AddCellDataSpan(table1.Rows.First.Cells.Last, spanData, Colors.White, Colors.Black, RadTextAlignment.Right, false, new TableWidthUnit(TableWidthUnitType.Percent, 65.0), FontWeights.Normal);
      section.Blocks.Add((Block) table1);
      Table table2 = new Table(1, 4);
      TableCell cell = table2.Rows.First.Cells.First;
      for (int columnIndex = 0; columnIndex < readingValues[0].Length; ++columnIndex)
      {
        GeneratePdfFromData.AddCellDataSpan(cell, readingValues[0][columnIndex], Colors.Gray, Colors.Black, RadTextAlignment.Center, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) GeneratePdfFromData.GetColumnWidthPercentage(columnIndex)), FontWeights.Normal);
        cell = (TableCell) cell.NextSibling;
      }
      for (int index1 = 1; index1 < readingValues.Count; ++index1)
      {
        TableRow tableRow = table2.AddRow();
        for (int index2 = 0; index2 < tableRow.Cells.Count; ++index2)
        {
          RadTextAlignment cellTextAligment = RadTextAlignment.Center;
          switch (index2 % 4)
          {
            case 1:
              cellTextAligment = RadTextAlignment.Right;
              break;
            case 3:
              cellTextAligment = RadTextAlignment.Left;
              break;
          }
          GeneratePdfFromData.AddCellDataSpan(tableRow.Cells.ElementAt<TableCell>(index2), readingValues[index1][index2], Colors.White, Colors.Black, cellTextAligment, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) GeneratePdfFromData.GetColumnWidthPercentage(index2)), FontWeights.Normal);
        }
      }
      section.Blocks.Add((Block) table2);
      meterReadingValues.LayoutMode = DocumentLayoutMode.Paged;
      meterReadingValues.Measure(RadDocument.MAX_DOCUMENT_SIZE);
      meterReadingValues.Arrange(new RectangleF(PointF.Empty, meterReadingValues.DesiredSize));
      return meterReadingValues;
    }

    private static void AddCellDataSpan(
      TableCell cell,
      string spanData,
      Color cellBackgroundColor,
      Color textColor,
      RadTextAlignment cellTextAligment,
      bool hasBorder,
      TableWidthUnit cellWidth,
      FontWeight fontWeight,
      Padding? padding = null,
      double? fontSize = null)
    {
      Paragraph paragraph = new Paragraph();
      paragraph.TextAlignment = cellTextAligment;
      cell.VerticalAlignment = RadVerticalAlignment.Center;
      cell.Blocks.Add((Block) paragraph);
      cell.Background = cellBackgroundColor;
      if (hasBorder)
      {
        Border border = new Border(1f, BorderStyle.Single, Colors.Black);
        cell.Borders = new TableCellBorders(border, border, border, border);
      }
      cell.PreferredWidth = cellWidth;
      if (padding.HasValue && !string.IsNullOrEmpty(spanData) && !string.IsNullOrWhiteSpace(spanData.Trim()))
        cell.Padding = padding.Value;
      else if (padding.HasValue)
      {
        TableCell tableCell = cell;
        Padding padding1 = padding.Value;
        int left = padding1.Left;
        padding1 = padding.Value;
        int top = padding1.Top;
        padding1 = padding.Value;
        int right = padding1.Right;
        padding1 = padding.Value;
        int bottom = padding1.Bottom - 12;
        Padding padding2 = new Padding(left, top, right, bottom);
        tableCell.Padding = padding2;
      }
      Span span = new Span()
      {
        Text = !string.IsNullOrEmpty(spanData) ? spanData : " ",
        ForeColor = textColor,
        FontWeight = fontWeight
      };
      if (fontSize.HasValue)
        span.FontSize = fontSize.Value;
      paragraph.Inlines.Add((Inline) span);
    }

    private static int GetColumnWidthPercentage(int columnIndex)
    {
      switch (columnIndex)
      {
        case 0:
        case 1:
        case 2:
          return 20;
        case 3:
          return 40;
        default:
          return 20;
      }
    }

    public static RadDocument GeneratePdfFromConfigurationParameters(
      List<Config> dynamicGridTag,
      List<Config> channel1DynamicGridTag,
      List<Config> channel2DynamicGridTag,
      List<Config> channel3DynamicGridTag,
      string deviceModel)
    {
      RadDocument configurationParameters1 = new RadDocument();
      DateTime now = DateTime.Now;
      string spanData = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(now.DayOfWeek) + ", " + (object) now.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(now.Month) + " " + (object) now.Year + " " + string.Format("{0:D2}", (object) now.Hour) + ":" + string.Format("{0:D2}", (object) now.Minute);
      Section section = new Section();
      configurationParameters1.Sections.Add(section);
      Table table = new Table(1, 2);
      GeneratePdfFromData.AddCellDataSpan(table.Rows.First.Cells.First, deviceModel, Colors.White, Colors.Black, RadTextAlignment.Left, false, new TableWidthUnit(TableWidthUnitType.Percent, 35.0), FontWeights.Normal);
      GeneratePdfFromData.AddCellDataSpan(table.Rows.First.Cells.Last, spanData, Colors.White, Colors.Black, RadTextAlignment.Right, false, new TableWidthUnit(TableWidthUnitType.Percent, 65.0), FontWeights.Normal);
      section.Blocks.Add((Block) table);
      Table configurationParameters2 = GeneratePdfFromData.CreateTableFromConfigurationParameters(dynamicGridTag);
      Table configurationParameters3 = GeneratePdfFromData.CreateTableFromConfigurationParameters(channel1DynamicGridTag);
      Table configurationParameters4 = GeneratePdfFromData.CreateTableFromConfigurationParameters(channel2DynamicGridTag);
      Table configurationParameters5 = GeneratePdfFromData.CreateTableFromConfigurationParameters(channel3DynamicGridTag);
      if (configurationParameters2 != null)
      {
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(" "));
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(Resources.MSS_Client_ConfigurationParametersTableTitle));
        section.Blocks.Add((Block) configurationParameters2);
      }
      if (configurationParameters3 != null)
      {
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(" "));
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(Resources.MSS_Client_Channel1TableTitle));
        section.Blocks.Add((Block) configurationParameters3);
      }
      if (configurationParameters4 != null)
      {
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(" "));
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(Resources.MSS_Client_Channel2TableTitle));
        section.Blocks.Add((Block) configurationParameters4);
      }
      if (configurationParameters5 != null)
      {
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(" "));
        section.Blocks.Add((Block) GeneratePdfFromData.CreateTableFromTitle(Resources.MSS_Client_Channel3TableTitle));
        section.Blocks.Add((Block) configurationParameters5);
      }
      configurationParameters1.LayoutMode = DocumentLayoutMode.Paged;
      configurationParameters1.Measure(RadDocument.MAX_DOCUMENT_SIZE);
      configurationParameters1.Arrange(new RectangleF(PointF.Empty, configurationParameters1.DesiredSize));
      return configurationParameters1;
    }

    private static Table CreateTableFromTitle(string title)
    {
      Table tableFromTitle = new Table(1, 1);
      GeneratePdfFromData.AddCellDataSpan(tableFromTitle.Rows.First.Cells.First, title, Colors.White, Colors.Black, RadTextAlignment.Left, false, new TableWidthUnit(TableWidthUnitType.Percent, 100.0), FontWeights.Bold, new Padding?(new Padding(0, 0, 0, -12)), new double?(10.0));
      return tableFromTitle;
    }

    private static Table CreateTableFromConfigurationParameters(List<Config> configurationParameters)
    {
      int num1 = 30;
      int num2 = 60;
      int num3 = 10;
      if (configurationParameters == null || configurationParameters.Count <= 0)
        return (Table) null;
      Table configurationParameters1 = new Table(1, 3);
      TableCell first = configurationParameters1.Rows.First.Cells.First;
      GeneratePdfFromData.AddCellDataSpan(first, Resources.MSS_Client_MeterReadingSettings_Parameter, Colors.Gray, Colors.Black, RadTextAlignment.Center, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) num1), FontWeights.Bold, new Padding?(new Padding(4, 1, 4, -13)), new double?(8.0));
      TableCell nextSibling = (TableCell) first.NextSibling;
      GeneratePdfFromData.AddCellDataSpan(nextSibling, Resources.MSS_Client_MeterReadingSettings_Value, Colors.Gray, Colors.Black, RadTextAlignment.Center, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) num2), FontWeights.Bold, new Padding?(new Padding(4, 1, 4, -13)), new double?(8.0));
      GeneratePdfFromData.AddCellDataSpan((TableCell) nextSibling.NextSibling, Resources.MSS_Client_MeterReadingSettings_Unit, Colors.Gray, Colors.Black, RadTextAlignment.Center, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) num3), FontWeights.Bold, new Padding?(new Padding(4, 1, 4, -13)), new double?(8.0));
      for (int index = 0; index < configurationParameters.Count; ++index)
      {
        TableRow tableRow = configurationParameters1.AddRow();
        GeneratePdfFromData.AddCellDataSpan(tableRow.Cells.ElementAt<TableCell>(0), configurationParameters[index].PropertyName, Colors.White, Colors.Black, RadTextAlignment.Right, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) num1), FontWeights.Normal, new Padding?(new Padding(4, 1, 4, -13)), new double?(8.0));
        GeneratePdfFromData.AddCellDataSpan(tableRow.Cells.ElementAt<TableCell>(1), configurationParameters[index].PropertyValue, Colors.White, Colors.Black, RadTextAlignment.Left, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) num2), FontWeights.Normal, new Padding?(new Padding(4, 1, 4, -13)), new double?(8.0));
        KeyValuePair<OverrideID, ConfigurationParameter> parameter = (KeyValuePair<OverrideID, ConfigurationParameter>) configurationParameters[index].Parameter;
        GeneratePdfFromData.AddCellDataSpan(tableRow.Cells.ElementAt<TableCell>(2), parameter.Value.Unit, Colors.White, Colors.Black, RadTextAlignment.Center, true, new TableWidthUnit(TableWidthUnitType.Percent, (double) num3), FontWeights.Normal, new Padding?(new Padding(4, 1, 4, -13)), new double?(8.0));
      }
      return configurationParameters1;
    }
  }
}
