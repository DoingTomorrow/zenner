// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.ValueMapper
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Globalization;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class ValueMapper
  {
    private static ValueMapper.ValueMapperDelegate _typeMapperInternal = Type.GetType("System.DateTimeOffset") == null ? new ValueMapper.ValueMapperDelegate(ValueMapper.GetMappedValueNET20RTM) : new ValueMapper.ValueMapperDelegate(ValueMapper.GetMappedValueNET20SP1);

    public static object GetMappedValue(SqlDbType paramType, object value)
    {
      return ValueMapper._typeMapperInternal(paramType, value);
    }

    private static object GetMappedValueNET20RTM(SqlDbType paramType, object value)
    {
      switch (value)
      {
        case null:
          return (object) null;
        case DateTime _:
          return ValueMapper.DateTimeMapper(paramType, value);
        case TimeSpan _:
          return ValueMapper.TimeSpanMapper(paramType, value);
        default:
          return value;
      }
    }

    private static object GetMappedValueNET20SP1(SqlDbType paramType, object value)
    {
      switch (value)
      {
        case null:
          return (object) null;
        case DateTime _:
          return ValueMapper.DateTimeMapper(paramType, value);
        case DateTimeOffset _:
          return ValueMapper.DateTimeOffsetMapper(paramType, value);
        case TimeSpan _:
          return ValueMapper.TimeSpanMapper(paramType, value);
        default:
          return value;
      }
    }

    private static object DateTimeMapper(SqlDbType paramType, object value)
    {
      return paramType == SqlDbType.NChar || paramType == SqlDbType.NVarChar ? (object) ((DateTime) value).ToString("yyyy-MM-dd HH:mm:ss.fffffff", (IFormatProvider) CultureInfo.InvariantCulture) : value;
    }

    private static object TimeSpanMapper(SqlDbType paramType, object value)
    {
      if (paramType != SqlDbType.NChar && paramType != SqlDbType.NVarChar)
        return value;
      TimeSpan timeSpan = (TimeSpan) value;
      return (object) DateTime.MinValue.Add(timeSpan).ToString("HH:mm:ss.fffffff");
    }

    private static object DateTimeOffsetMapper(SqlDbType paramType, object value)
    {
      return paramType == SqlDbType.NChar || paramType == SqlDbType.NVarChar ? (object) ((DateTimeOffset) value).ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz", (IFormatProvider) CultureInfo.InvariantCulture) : value;
    }

    private delegate object ValueMapperDelegate(SqlDbType paramType, object value);
  }
}
