// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.DataCollectors.ArchiveMinomatRadioDetailsMap
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
  public class ArchiveMinomatRadioDetailsMap : ClassMap<ArchiveMinomatRadioDetails>
  {
    public ArchiveMinomatRadioDetailsMap()
    {
      this.Table("t_MinomatRadioDetails");
      this.Id((Expression<Func<ArchiveMinomatRadioDetails, object>>) (t => (object) t.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (t => (object) t.Id));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.Deleted));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.Conflict));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.NoteId));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.MinomatType));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.RadioIdMaster));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.RegistrationStatus));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.Name));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.Location));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.Entrance));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.MinomatStatus));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.IsConfigured));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.ParameterSet));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.DueDate));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.CanRegiesterMoreThanOne));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.ReservedPlaces));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.InstalledOn));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.IsSlave));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.LastConnection));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.StatusOfTheLastConnection));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.NetzId));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.Channel));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.NrOfReceivedDevices));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.NrOfAssignedDevices));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.NrOfRegisteredDevices));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.NetExtensionMode));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => c.InstallatioStatus));
      this.Map((Expression<Func<ArchiveMinomatRadioDetails, object>>) (c => (object) c.MinomatId));
      this.References<ArchiveInformation>((Expression<Func<ArchiveMinomatRadioDetails, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_MinomatRadioDetails_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
