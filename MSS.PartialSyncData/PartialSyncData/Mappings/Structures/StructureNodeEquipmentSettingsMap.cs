// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Structures.StructureNodeEquipmentSettingsMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Structures
{
  public class StructureNodeEquipmentSettingsMap : ClassMap<StructureNodeEquipmentSettings>
  {
    public StructureNodeEquipmentSettingsMap()
    {
      this.Table("t_StructureNodeEquipmentSettings");
      this.Id((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => (object) _.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.EquipmentName));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.EquipmentParams));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.SystemName));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.ScanMode));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.ScanParams));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.ReadingProfileName));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.ReadingProfileParams));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => _.DeviceModelReadingParams));
      this.Map((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => (object) _.LastChangedOn)).Nullable();
      this.References<StructureNode>((Expression<Func<StructureNodeEquipmentSettings, StructureNode>>) (_ => _.StructureNode), "StructureNodeId").Class<StructureNode>();
    }
  }
}
