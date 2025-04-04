// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessClientSideSelect2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.GroupBy;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  internal class ProcessClientSideSelect2 : IResultOperatorProcessor<ClientSideSelect2>
  {
    public void Process(
      ClientSideSelect2 resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      tree.AddListTransformer(resultOperator.SelectClause);
    }
  }
}
