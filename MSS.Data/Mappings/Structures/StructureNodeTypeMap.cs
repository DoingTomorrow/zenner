// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Structures.StructureNodeTypeMap
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
  public sealed class StructureNodeTypeMap : ClassMap<StructureNodeType>
  {
    public StructureNodeTypeMap()
    {
      this.Table("t_StructureNodeType");
      this.Id((Expression<Func<StructureNodeType, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<StructureNodeType, object>>) (n => n.Name)).Length(200);
      this.Map((Expression<Func<StructureNodeType, object>>) (n => n.IconPath)).Length(200);
      this.Map((Expression<Func<StructureNodeType, object>>) (n => (object) n.IsFixed));
      this.Map((Expression<Func<StructureNodeType, object>>) (n => (object) n.IsLogical));
      this.Map((Expression<Func<StructureNodeType, object>>) (n => (object) n.IsPhysical));
    }
  }
}
