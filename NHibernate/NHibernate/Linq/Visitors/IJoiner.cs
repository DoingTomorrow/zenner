// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.IJoiner
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public interface IJoiner
  {
    Expression AddJoin(Expression expression, string key);

    bool CanAddJoin(Expression expression);

    void MakeInnerIfJoined(string key);
  }
}
