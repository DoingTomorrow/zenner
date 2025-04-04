// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Repository.JobsRepository
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Repository
{
  public class JobsRepository(ISession session) : 
    MSS.PartialSyncData.Repository.Repository<MssReadingJob>(session),
    IJobRepository,
    IRepository<MssReadingJob>
  {
    public List<MssReadingJob> GetJobs()
    {
      return this._session.Query<MssReadingJob>().Fetch<MssReadingJob, JobDefinition>((Expression<Func<MssReadingJob, JobDefinition>>) (x => x.JobDefinition)).Where<MssReadingJob>((Expression<Func<MssReadingJob, bool>>) (x => x.IsDeactivated == false)).ToList<MssReadingJob>();
    }

    public List<Minomat> GetMinomatsWithMissingJobs()
    {
      Minomat minomatAlias = (Minomat) null;
      Scenario scenarioAlias = (Scenario) null;
      ScenarioJobDefinition scenarioJobDefinitionAlias = (ScenarioJobDefinition) null;
      JobDefinition jobDefinitionAlias = (JobDefinition) null;
      QueryOver<MssReadingJob, MssReadingJob> detachedQuery = QueryOver.Of<MssReadingJob>().Where((Expression<Func<MssReadingJob, bool>>) (j => j.Minomat.Id == minomatAlias.Id && j.JobDefinition.Id == jobDefinitionAlias.Id && !j.IsDeactivated && j.EndDate == new DateTime?())).Select((Expression<Func<MssReadingJob, object>>) (j => (object) j.Id));
      return this._session.QueryOver<Minomat>((Expression<Func<Minomat>>) (() => minomatAlias)).JoinAlias((Expression<Func<Minomat, object>>) (m => minomatAlias.Scenario), (Expression<Func<object>>) (() => scenarioAlias), JoinType.InnerJoin).JoinAlias((Expression<Func<Minomat, object>>) (m => scenarioAlias.ScenarioJobDefinitions), (Expression<Func<object>>) (() => scenarioJobDefinitionAlias), JoinType.InnerJoin).JoinAlias((Expression<Func<Minomat, object>>) (m => scenarioJobDefinitionAlias.JobDefinition), (Expression<Func<object>>) (() => jobDefinitionAlias), JoinType.InnerJoin).WithSubquery.WhereNotExists<MssReadingJob>((QueryOver<MssReadingJob>) detachedQuery).Where((Expression<Func<Minomat, bool>>) (m => !minomatAlias.IsDeactivated && minomatAlias.IsMaster && minomatAlias.GsmId != default (string))).TransformUsing(Transformers.DistinctRootEntity).List<Minomat>().ToList<Minomat>();
    }
  }
}
