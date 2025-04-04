// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.CsvLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  [Layout("CsvLayout")]
  [NLog.Config.ThreadAgnostic]
  [NLog.Config.ThreadSafe]
  [AppDomainFixedOutput]
  public class CsvLayout : LayoutWithHeaderAndFooter
  {
    private string _actualColumnDelimiter;
    private string _doubleQuoteChar;
    private char[] _quotableCharacters;

    public CsvLayout()
    {
      this.Columns = (IList<CsvColumn>) new List<CsvColumn>();
      this.WithHeader = true;
      this.Delimiter = CsvColumnDelimiterMode.Auto;
      this.Quoting = CsvQuotingMode.Auto;
      this.QuoteChar = "\"";
      this.Layout = (Layout) this;
      this.Header = (Layout) new CsvLayout.CsvHeaderLayout(this);
      this.Footer = (Layout) null;
    }

    [ArrayParameter(typeof (CsvColumn), "column")]
    public IList<CsvColumn> Columns { get; private set; }

    public bool WithHeader { get; set; }

    [DefaultValue("Auto")]
    public CsvColumnDelimiterMode Delimiter { get; set; }

    [DefaultValue("Auto")]
    public CsvQuotingMode Quoting { get; set; }

    [DefaultValue("\"")]
    public string QuoteChar { get; set; }

    public string CustomColumnDelimiter { get; set; }

    protected override void InitializeLayout()
    {
      base.InitializeLayout();
      if (!this.WithHeader)
        this.Header = (Layout) null;
      switch (this.Delimiter)
      {
        case CsvColumnDelimiterMode.Auto:
          this._actualColumnDelimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
          break;
        case CsvColumnDelimiterMode.Comma:
          this._actualColumnDelimiter = ",";
          break;
        case CsvColumnDelimiterMode.Semicolon:
          this._actualColumnDelimiter = ";";
          break;
        case CsvColumnDelimiterMode.Tab:
          this._actualColumnDelimiter = "\t";
          break;
        case CsvColumnDelimiterMode.Pipe:
          this._actualColumnDelimiter = "|";
          break;
        case CsvColumnDelimiterMode.Space:
          this._actualColumnDelimiter = " ";
          break;
        case CsvColumnDelimiterMode.Custom:
          this._actualColumnDelimiter = this.CustomColumnDelimiter;
          break;
      }
      this._quotableCharacters = (this.QuoteChar + "\r\n" + this._actualColumnDelimiter).ToCharArray();
      this._doubleQuoteChar = this.QuoteChar + this.QuoteChar;
    }

    internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
      if (this.ThreadAgnostic)
        return;
      this.RenderAppendBuilder(logEvent, target, true);
    }

    protected override string GetFormattedMessage(LogEventInfo logEvent)
    {
      return this.RenderAllocateBuilder(logEvent);
    }

    private void RenderAllColumns(LogEventInfo logEvent, StringBuilder sb)
    {
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        string columnValue = this.Columns[index].Layout.Render(logEvent);
        this.RenderCol(sb, index, columnValue);
      }
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      this.RenderAllColumns(logEvent, target);
    }

    private void RenderHeader(StringBuilder sb)
    {
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        string name = this.Columns[index].Name;
        this.RenderCol(sb, index, name);
      }
    }

    private void RenderCol(StringBuilder sb, int columnIndex, string columnValue)
    {
      if (columnIndex != 0)
        sb.Append(this._actualColumnDelimiter);
      bool flag;
      switch (this.Quoting)
      {
        case CsvQuotingMode.All:
          flag = true;
          break;
        case CsvQuotingMode.Nothing:
          flag = false;
          break;
        default:
          flag = columnValue.IndexOfAny(this._quotableCharacters) >= 0;
          break;
      }
      if (flag)
        sb.Append(this.QuoteChar);
      if (flag)
        sb.Append(columnValue.Replace(this.QuoteChar, this._doubleQuoteChar));
      else
        sb.Append(columnValue);
      if (!flag)
        return;
      sb.Append(this.QuoteChar);
    }

    public override string ToString()
    {
      return this.ToStringWithNestedItems<CsvColumn>(this.Columns, (Func<CsvColumn, string>) (c => c.Name));
    }

    [NLog.Config.ThreadAgnostic]
    private class CsvHeaderLayout : Layout
    {
      private CsvLayout _parent;

      public CsvHeaderLayout(CsvLayout parent) => this._parent = parent;

      internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
      {
        if (this.ThreadAgnostic)
          return;
        this.RenderAppendBuilder(logEvent, target, true);
      }

      protected override string GetFormattedMessage(LogEventInfo logEvent)
      {
        return this.RenderAllocateBuilder(logEvent);
      }

      protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
      {
        this._parent.RenderHeader(target);
      }
    }
  }
}
