// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.ScalarReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Loader.Custom
{
  public class ScalarReturn : IReturn
  {
    private readonly IType type;
    private readonly string columnAlias;

    public ScalarReturn(IType type, string columnAlias)
    {
      this.type = type;
      this.columnAlias = columnAlias;
    }

    public IType Type => this.type;

    public string ColumnAlias => this.columnAlias;
  }
}
