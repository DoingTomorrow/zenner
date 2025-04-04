// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.DecodeCaseFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class DecodeCaseFragment(NHibernate.Dialect.Dialect dialect) : CaseFragment(dialect)
  {
    public override string ToSqlStringFragment()
    {
      StringBuilder stringBuilder = new StringBuilder(this.cases.Count * 15 + 10).Append("decode(");
      int num = 0;
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.cases)
      {
        if (num < this.cases.Count - 1)
          stringBuilder.Append(", ").Append(keyValuePair.Key).Append(", ").Append(keyValuePair.Value);
        else
          stringBuilder.Insert(7, keyValuePair.Key).Append(", ").Append(keyValuePair.Value);
        ++num;
      }
      stringBuilder.Append(")");
      if (!string.IsNullOrEmpty(this.returnColumnName))
        stringBuilder.Append(" as ").Append(this.returnColumnName);
      return stringBuilder.ToString();
    }
  }
}
