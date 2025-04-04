// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.StructureNodeSorter
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections;

#nullable disable
namespace ZR_ClassLibrary
{
  public class StructureNodeSorter : IComparer
  {
    private StructureSortMode mode;
    private StructureSortOrder order;

    public StructureNodeSorter(StructureSortMode mode, StructureSortOrder order)
    {
      this.mode = mode;
      this.order = order;
    }

    public virtual int Compare(object x, object y)
    {
      ISortable sortable1 = x as ISortable;
      ISortable sortable2 = y as ISortable;
      int num = 0;
      if (sortable1 == null || sortable2 == null)
        return num;
      if (this.mode != StructureSortMode.NodeOrder)
        throw new NotImplementedException();
      if (sortable1.NodeOrder > sortable2.NodeOrder)
        num = -1;
      else if (sortable1.NodeOrder < sortable2.NodeOrder)
        num = 1;
      return this.order == StructureSortOrder.Ascending ? -num : num;
    }
  }
}
