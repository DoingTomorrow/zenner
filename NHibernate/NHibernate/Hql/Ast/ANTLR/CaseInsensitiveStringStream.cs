// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.CaseInsensitiveStringStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class CaseInsensitiveStringStream(string input) : ANTLRStringStream(input)
  {
    public override int LA(int i)
    {
      if (i == 0)
        return 0;
      if (i < 0)
        ++i;
      return this.p + i - 1 >= this.n ? -1 : (int) char.ToLowerInvariant(this.data[this.p + i - 1]);
    }
  }
}
