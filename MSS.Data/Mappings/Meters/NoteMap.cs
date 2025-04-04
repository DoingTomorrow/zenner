// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.NoteMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public sealed class NoteMap : ClassMap<Note>
  {
    public NoteMap()
    {
      this.Table("t_Note");
      this.Id((Expression<Func<Note, object>>) (_ => (object) _.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Note, object>>) (_ => _.NoteDescription));
      this.Map((Expression<Func<Note, object>>) (_ => (object) _.LastChangedOn), "LastChangedOn");
      this.References<StructureNode>((Expression<Func<Note, StructureNode>>) (_ => _.StructureNode), "StructureNodeId");
      this.References<NoteType>((Expression<Func<Note, NoteType>>) (_ => _.NoteType), "NoteTypeId");
    }
  }
}
