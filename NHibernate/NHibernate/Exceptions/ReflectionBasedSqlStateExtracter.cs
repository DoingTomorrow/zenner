// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.ReflectionBasedSqlStateExtracter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Data.Common;
using System.Reflection;

#nullable disable
namespace NHibernate.Exceptions
{
  internal class ReflectionBasedSqlStateExtracter : SqlStateExtracter
  {
    public override int ExtractSingleErrorCode(DbException sqle)
    {
      PropertyInfo property1 = sqle.GetType().GetProperty("Errors");
      if (property1 == null)
        return 0;
      int singleErrorCode = 0;
      foreach (object obj in property1.GetValue((object) sqle, (object[]) null) as IEnumerable)
      {
        PropertyInfo property2 = obj.GetType().GetProperty("NativeError");
        if (property2 == null)
          return 0;
        singleErrorCode = (int) property2.GetValue(obj, (object[]) null);
        if (singleErrorCode != 0)
          break;
      }
      return singleErrorCode;
    }

    public override string ExtractSingleSqlState(DbException sqle)
    {
      PropertyInfo property1 = sqle.GetType().GetProperty("Errors");
      if (property1 == null)
        return (string) null;
      string singleSqlState = "";
      foreach (object obj in property1.GetValue((object) sqle, (object[]) null) as IEnumerable)
      {
        PropertyInfo property2 = obj.GetType().GetProperty("SQLState");
        if (property2 == null)
          return (string) null;
        singleSqlState = (string) property2.GetValue(obj, (object[]) null);
        if (singleSqlState.Length != 0)
          break;
      }
      return singleSqlState;
    }
  }
}
