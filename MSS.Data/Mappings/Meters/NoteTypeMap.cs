// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.NoteTypeMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public class NoteTypeMap : ClassMap<NoteType>
  {
    public NoteTypeMap()
    {
      this.Table("t_NoteType");
      this.Id((Expression<Func<NoteType, object>>) (_ => (object) _.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<NoteType, object>>) (_ => _.Description)).Nullable();
    }
  }
}
