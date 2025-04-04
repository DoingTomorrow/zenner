// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Structures.StructureNodeEquipmentSettingsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Structures
{
  public class StructureNodeEquipmentSettingsMap : ClassMap<StructureNodeEquipmentSettings>
  {
    public StructureNodeEquipmentSettingsMap()
    {
      this.Table("t_StructureNodeEquipmentSettings");
      this.Id((Expression<Func<StructureNodeEquipmentSettings, object>>) (_ => (object) _.Id)).GeneratedBy.GuidComb();
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
