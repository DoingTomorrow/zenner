// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.NoteTypeMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public class NoteTypeMap : ClassMap<NoteType>
  {
    public NoteTypeMap()
    {
      this.Table("t_NoteType");
      this.Id((Expression<Func<NoteType, object>>) (_ => (object) _.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<NoteType, object>>) (_ => _.Description)).Nullable();
    }
  }
}
