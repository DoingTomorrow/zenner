// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.NoteMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public sealed class NoteMap : ClassMap<Note>
  {
    public NoteMap()
    {
      this.Table("t_Note");
      this.Id((Expression<Func<Note, object>>) (_ => (object) _.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<Note, object>>) (_ => _.NoteDescription));
      this.Map((Expression<Func<Note, object>>) (_ => (object) _.LastChangedOn), "LastChangedOn");
      this.References<StructureNode>((Expression<Func<Note, StructureNode>>) (_ => _.StructureNode), "StructureNodeId");
      this.References<NoteType>((Expression<Func<Note, NoteType>>) (_ => _.NoteType), "NoteTypeId");
    }
  }
}
