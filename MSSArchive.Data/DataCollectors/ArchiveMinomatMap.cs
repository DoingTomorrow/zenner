// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.DataCollectors.ArchiveMinomatMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.DataCollectors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.DataCollectors
{
  public class ArchiveMinomatMap : ClassMap<ArchiveMinomat>
  {
    public ArchiveMinomatMap()
    {
      this.Table("t_Minomat");
      this.Id((Expression<Func<ArchiveMinomat, object>>) (t => (object) t.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (t => (object) t.Id));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.MasterRadioId));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.Status));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.Registered));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.HostAndPort));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.Url));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.CreatedBy));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.CreatedOn));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.LastUpdatedBy));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.LastUpdatedOn));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.Challenge));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.GsmId));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.LockedBy));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.IsDeactivated));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.StartDate));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.EndDate));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.Polling));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.IsMaster));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => (object) c.IsInMasterPool));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.ProviderName));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.SimPin));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.AccessPoint));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.UserId));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.UserPassword));
      this.Map((Expression<Func<ArchiveMinomat, object>>) (c => c.SessionKey));
      this.References<ArchiveInformation>((Expression<Func<ArchiveMinomat, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_Minomat_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
