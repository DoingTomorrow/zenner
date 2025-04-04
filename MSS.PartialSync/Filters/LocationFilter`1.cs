// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.Filters.LocationFilter`1
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.Filters
{
  public class LocationFilter<T> : GenericEntityFilter<T> where T : Location
  {
    private Expression DefineFilters(Guid id) => (Expression) (c => c.Id == id);

    public Expression ApplyReplace(Guid id) => this.VisitTree(this.DefineFilters(id));
  }
}
