// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Insert.InsertSelectIdentityInsert
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;

#nullable disable
namespace NHibernate.Id.Insert
{
  public class InsertSelectIdentityInsert(ISessionFactoryImplementor factory) : 
    IdentifierGeneratingInsert(factory)
  {
    public override SqlString ToSqlString()
    {
      return this.Dialect.AppendIdentitySelectToInsert(base.ToSqlString());
    }
  }
}
