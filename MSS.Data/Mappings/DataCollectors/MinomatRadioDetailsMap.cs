// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.DataCollectors.MinomatRadioDetailsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.DataCollectors
{
  public class MinomatRadioDetailsMap : ClassMap<MinomatRadioDetails>
  {
    public MinomatRadioDetailsMap()
    {
      this.Table("t_MinomatRadioDetails");
      this.Id((Expression<Func<MinomatRadioDetails, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.NodeId));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.Description));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.Location));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.Entrance));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.IsConfigured));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.DueDate));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.CanRegiesterMoreThanOne));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.ReservedPlaces));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.InstalledOn));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.LastConnection));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.NetId));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.Channel));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.NrOfReceivedDevices));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.NrOfAssignedDevices));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => c.NrOfRegisteredDevices));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.NetExtensionMode));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.StatusDevices));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.StatusNetwork));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.LastStartOn));
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.LastChangedOn)).Nullable();
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.LastRegisteredOn)).Nullable();
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.GSMStatus)).Nullable();
      this.Map((Expression<Func<MinomatRadioDetails, object>>) (c => (object) c.GSMStatusDate)).Nullable();
      this.References<Minomat>((Expression<Func<MinomatRadioDetails, Minomat>>) (c => c.Minomat), "MinomatId").Not.Nullable().Unique();
    }
  }
}
