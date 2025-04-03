// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.ArchiveJobManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.Archiving;
using MSS.Core.Model.Reporting;
using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public class ArchiveJobManager
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public ArchiveJobManager(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }

    public List<ArchiveJob> GetActiveJobs()
    {
      return this._repositoryFactory.GetRepository<ArchiveJob>().GetAll().Where<ArchiveJob>(new Func<ArchiveJob, bool>(this.IsDueForExecution)).ToList<ArchiveJob>();
    }

    public ArchiveDetailsADO GetArchiveDetails(ArchiveJob archiveJob)
    {
      DateTime dateTime = archiveJob.LastExecutionDate.HasValue ? archiveJob.LastExecutionDate.Value : new DateTime(2000, 1, 1);
      return new ArchiveDetailsADO()
      {
        StartTime = dateTime,
        EndTime = DateTime.Now,
        ArchivedEntities = ArchivingHelper.DeserializeArchivedEntities(archiveJob.ArchivedEntities),
        ArchiveName = archiveJob.ArchiveName + "_" + dateTime.ToString("dd-MMM-yy HH:mm") + "_" + DateTime.Now.ToString("dd-MMM-yy HH:mm")
      };
    }

    private bool IsDueForExecution(ArchiveJob jobToCheck)
    {
      DateTime dateTime1;
      if (jobToCheck.LastExecutionDate.HasValue)
      {
        DateTime dateTime2 = jobToCheck.LastExecutionDate.Value;
        DateTime startDate = jobToCheck.StartDate;
        dateTime1 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, startDate.Hour, startDate.Minute, 0);
      }
      else
        dateTime1 = jobToCheck.StartDate;
      DateTime dateTime3;
      switch (jobToCheck.Periodicity)
      {
        case AutomatedExportJobPeriodicityEnum.Hourly:
          dateTime3 = dateTime1.AddHours(1.0);
          break;
        case AutomatedExportJobPeriodicityEnum.Daily:
          dateTime3 = dateTime1.AddDays(1.0);
          break;
        case AutomatedExportJobPeriodicityEnum.Weekly:
          dateTime3 = dateTime1.AddDays(7.0);
          break;
        case AutomatedExportJobPeriodicityEnum.Monthly:
          dateTime3 = dateTime1.AddMonths(1);
          break;
        case AutomatedExportJobPeriodicityEnum.Annually:
          dateTime3 = dateTime1.AddYears(1);
          break;
        default:
          return false;
      }
      return dateTime3 < DateTime.Now;
    }
  }
}
