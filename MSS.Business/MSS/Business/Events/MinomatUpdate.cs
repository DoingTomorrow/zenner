// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.MinomatUpdate
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Events
{
  public class MinomatUpdate
  {
    public bool IsUpdate { get; set; }

    public List<Guid> Ids { get; set; }
  }
}
