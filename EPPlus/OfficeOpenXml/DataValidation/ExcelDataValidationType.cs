// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.DataValidation.ExcelDataValidationType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.DataValidation
{
  public class ExcelDataValidationType
  {
    private static ExcelDataValidationType _whole;
    private static ExcelDataValidationType _list;
    private static ExcelDataValidationType _decimal;
    private static ExcelDataValidationType _textLength;
    private static ExcelDataValidationType _dateTime;
    private static ExcelDataValidationType _time;
    private static ExcelDataValidationType _custom;

    private ExcelDataValidationType(
      eDataValidationType validationType,
      bool allowOperator,
      string schemaName)
    {
      this.Type = validationType;
      this.AllowOperator = allowOperator;
      this.SchemaName = schemaName;
    }

    public eDataValidationType Type { get; private set; }

    internal string SchemaName { get; private set; }

    internal bool AllowOperator { get; private set; }

    internal static ExcelDataValidationType GetByValidationType(eDataValidationType type)
    {
      switch (type)
      {
        case eDataValidationType.Whole:
          return ExcelDataValidationType.Whole;
        case eDataValidationType.Decimal:
          return ExcelDataValidationType.Decimal;
        case eDataValidationType.List:
          return ExcelDataValidationType.List;
        case eDataValidationType.TextLength:
          return ExcelDataValidationType.TextLength;
        case eDataValidationType.DateTime:
          return ExcelDataValidationType.DateTime;
        case eDataValidationType.Time:
          return ExcelDataValidationType.Time;
        case eDataValidationType.Custom:
          return ExcelDataValidationType.Custom;
        default:
          throw new InvalidOperationException("Non supported Validationtype : " + type.ToString());
      }
    }

    internal static ExcelDataValidationType GetBySchemaName(string schemaName)
    {
      switch (schemaName)
      {
        case "whole":
          return ExcelDataValidationType.Whole;
        case "decimal":
          return ExcelDataValidationType.Decimal;
        case "list":
          return ExcelDataValidationType.List;
        case "textLength":
          return ExcelDataValidationType.TextLength;
        case "date":
          return ExcelDataValidationType.DateTime;
        case "time":
          return ExcelDataValidationType.Time;
        case "custom":
          return ExcelDataValidationType.Custom;
        default:
          throw new ArgumentException("Invalid schemaname: " + schemaName);
      }
    }

    public override bool Equals(object obj)
    {
      return obj is ExcelDataValidationType && ((ExcelDataValidationType) obj).Type == this.Type;
    }

    public override int GetHashCode() => base.GetHashCode();

    public static ExcelDataValidationType Whole
    {
      get
      {
        if (ExcelDataValidationType._whole == null)
          ExcelDataValidationType._whole = new ExcelDataValidationType(eDataValidationType.Whole, true, "whole");
        return ExcelDataValidationType._whole;
      }
    }

    public static ExcelDataValidationType List
    {
      get
      {
        if (ExcelDataValidationType._list == null)
          ExcelDataValidationType._list = new ExcelDataValidationType(eDataValidationType.List, false, "list");
        return ExcelDataValidationType._list;
      }
    }

    public static ExcelDataValidationType Decimal
    {
      get
      {
        if (ExcelDataValidationType._decimal == null)
          ExcelDataValidationType._decimal = new ExcelDataValidationType(eDataValidationType.Decimal, true, "decimal");
        return ExcelDataValidationType._decimal;
      }
    }

    public static ExcelDataValidationType TextLength
    {
      get
      {
        if (ExcelDataValidationType._textLength == null)
          ExcelDataValidationType._textLength = new ExcelDataValidationType(eDataValidationType.TextLength, true, "textLength");
        return ExcelDataValidationType._textLength;
      }
    }

    public static ExcelDataValidationType DateTime
    {
      get
      {
        if (ExcelDataValidationType._dateTime == null)
          ExcelDataValidationType._dateTime = new ExcelDataValidationType(eDataValidationType.DateTime, true, "date");
        return ExcelDataValidationType._dateTime;
      }
    }

    public static ExcelDataValidationType Time
    {
      get
      {
        if (ExcelDataValidationType._time == null)
          ExcelDataValidationType._time = new ExcelDataValidationType(eDataValidationType.Time, true, "time");
        return ExcelDataValidationType._time;
      }
    }

    public static ExcelDataValidationType Custom
    {
      get
      {
        if (ExcelDataValidationType._custom == null)
          ExcelDataValidationType._custom = new ExcelDataValidationType(eDataValidationType.Custom, true, "custom");
        return ExcelDataValidationType._custom;
      }
    }
  }
}
