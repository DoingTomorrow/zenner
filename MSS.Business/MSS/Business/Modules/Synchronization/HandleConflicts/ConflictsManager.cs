// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.HandleConflicts.ConflictsManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace MSS.Business.Modules.Synchronization.HandleConflicts
{
  public class ConflictsManager
  {
    private readonly Dictionary<Type, DataTable> Conflicts;

    public ConflictsManager(Dictionary<Type, DataTable> conflicts) => this.Conflicts = conflicts;

    public DataTable GetEntityConflicts(Type entityType)
    {
      return this.Conflicts.ContainsKey(entityType) ? this.Conflicts[entityType] : (DataTable) null;
    }
  }
}
