// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.AliasGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  public class AliasGenerator
  {
    private int next;

    private int nextCount() => this.next++;

    public string CreateName(string name) => StringHelper.GenerateAlias(name, this.nextCount());
  }
}
