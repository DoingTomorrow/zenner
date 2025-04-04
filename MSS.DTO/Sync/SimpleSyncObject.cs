// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Sync.SimpleSyncObject
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace MSS.DTO.Sync
{
  [DataContract]
  public class SimpleSyncObject
  {
    [DataMember]
    public Guid Id { get; set; }

    public class Comparer<T> : IEqualityComparer<T> where T : SimpleSyncObject
    {
      public bool Equals(T x, T y) => x.Id == y.Id;

      public int GetHashCode(T obj) => obj.Id.GetHashCode();
    }
  }
}
