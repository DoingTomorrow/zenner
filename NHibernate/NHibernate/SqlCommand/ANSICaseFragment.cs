// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.ANSICaseFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class ANSICaseFragment(NHibernate.Dialect.Dialect dialect) : CaseFragment(dialect)
  {
    public override string ToSqlStringFragment()
    {
      StringBuilder stringBuilder = new StringBuilder(this.cases.Count * 15 + 10).Append("case");
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.cases)
        stringBuilder.Append(" when ").Append(keyValuePair.Key).Append(" is not null then ").Append(keyValuePair.Value);
      stringBuilder.Append(" end");
      if (this.returnColumnName != null)
        stringBuilder.Append(" as ").Append(this.returnColumnName);
      return stringBuilder.ToString();
    }
  }
}
