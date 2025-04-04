// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Insert.ReturningIdentifierInsert
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;

#nullable disable
namespace NHibernate.Id.Insert
{
  public class ReturningIdentifierInsert : NoCommentsInsert
  {
    private readonly string identifierColumnName;
    private readonly string returnParameterName;

    public ReturningIdentifierInsert(
      ISessionFactoryImplementor factory,
      string identifierColumnName,
      string returnParameterName)
      : base(factory)
    {
      this.returnParameterName = returnParameterName;
      this.identifierColumnName = identifierColumnName;
    }

    public override SqlString ToSqlString()
    {
      return this.Dialect.AddIdentifierOutParameterToInsert(base.ToSqlString(), this.identifierColumnName, this.returnParameterName);
    }
  }
}
