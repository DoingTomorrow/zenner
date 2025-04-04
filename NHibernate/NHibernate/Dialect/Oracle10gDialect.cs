// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Oracle10gDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;

#nullable disable
namespace NHibernate.Dialect
{
  public class Oracle10gDialect : Oracle9iDialect
  {
    public override JoinFragment CreateOuterJoinFragment() => (JoinFragment) new ANSIJoinFragment();
  }
}
