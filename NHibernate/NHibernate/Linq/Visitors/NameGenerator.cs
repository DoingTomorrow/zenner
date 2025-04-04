// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.NameGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class NameGenerator
  {
    private readonly QueryModel _model;

    public NameGenerator(QueryModel model) => this._model = model;

    public string GetNewName() => this._model.GetNewName("_");
  }
}
