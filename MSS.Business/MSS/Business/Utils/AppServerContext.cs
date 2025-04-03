// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.AppServerContext
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Core.Model.TechnicalParameters;
using MSS.DTO.Sync;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Utils
{
  public class AppServerContext
  {
    private static AppServerContext _instance;

    public static AppServerContext Current
    {
      get
      {
        lock (typeof (AppServerContext))
          return AppServerContext._instance ?? (AppServerContext._instance = new AppServerContext());
      }
    }

    public List<Guid> ClientState { get; set; }

    public Dictionary<Guid, SimpleMetadata> TemporaryMetadataDictionary { get; set; }

    public TechnicalParameter TechnicalParameters { get; set; }

    public string HardwareKey { get; set; }
  }
}
