// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.DisjunctionFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class DisjunctionFragment
  {
    private SqlStringBuilder buffer = new SqlStringBuilder();

    public DisjunctionFragment()
    {
    }

    public DisjunctionFragment(IEnumerable<ConditionalFragment> fragments)
    {
      foreach (ConditionalFragment fragment in fragments)
        this.AddCondition(fragment);
    }

    public DisjunctionFragment AddCondition(ConditionalFragment fragment)
    {
      if (this.buffer.Count > 0)
        this.buffer.Add(" or ");
      this.buffer.Add("(").Add(fragment.ToSqlStringFragment()).Add(")");
      return this;
    }

    public SqlString ToFragmentString() => this.buffer.ToSqlString();
  }
}
