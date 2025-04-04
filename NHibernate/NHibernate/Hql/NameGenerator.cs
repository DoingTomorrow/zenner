// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.NameGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System.Text;

#nullable disable
namespace NHibernate.Hql
{
  public class NameGenerator
  {
    public static string[][] GenerateColumnNames(IType[] types, ISessionFactoryImplementor f)
    {
      string[][] columnNames = new string[types.Length][];
      for (int x = 0; x < types.Length; ++x)
      {
        int length = types[x] != null ? types[x].GetColumnSpan((IMapping) f) : 1;
        columnNames[x] = new string[length];
        for (int y = 0; y < length; ++y)
          columnNames[x][y] = NameGenerator.ScalarName(x, y);
      }
      return columnNames;
    }

    public static string ScalarName(int x, int y)
    {
      return new StringBuilder(16).Append("col_").Append(x).Append('_').Append(y).Append('_').ToString();
    }
  }
}
