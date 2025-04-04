// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.IResultOperatorProcessor`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public interface IResultOperatorProcessor<T>
  {
    void Process(T resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree);
  }
}
