// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.JoinFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;

#nullable disable
namespace NHibernate.SqlCommand
{
  public abstract class JoinFragment
  {
    public abstract void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType);

    public abstract void AddJoin(
      string tableName,
      string alias,
      string[] fkColumns,
      string[] pkColumns,
      JoinType joinType,
      SqlString on);

    public abstract void AddCrossJoin(string tableName, string alias);

    public abstract void AddJoins(SqlString fromFragment, SqlString whereFragment);

    public abstract SqlString ToFromFragmentString { get; }

    public abstract SqlString ToWhereFragmentString { get; }

    public abstract bool AddCondition(string condition);

    public abstract bool AddCondition(SqlString condition);

    public abstract void AddFromFragmentString(SqlString fromFragmentString);

    public virtual void AddFragment(JoinFragment ojf)
    {
      this.AddJoins(ojf.ToFromFragmentString, ojf.ToWhereFragmentString);
    }

    protected bool AddCondition(SqlStringBuilder buffer, string on)
    {
      if (!StringHelper.IsNotEmpty(on))
        return false;
      if (!on.StartsWith(" and"))
        buffer.Add(" and ");
      buffer.Add(on);
      return true;
    }

    protected bool AddCondition(SqlStringBuilder buffer, SqlString on)
    {
      if (!StringHelper.IsNotEmpty(on))
        return false;
      if (!on.StartsWithCaseInsensitive(" and"))
        buffer.Add(" and ");
      buffer.Add(on);
      return true;
    }

    public bool HasFilterCondition { get; set; }

    public bool HasThetaJoins { get; set; }
  }
}
