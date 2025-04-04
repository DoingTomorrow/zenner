// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.DCUConnectionRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class DCUConnectionRecord
  {
    public string DCU_ID { get; set; }

    public DateTime Load_Dttm { get; set; }

    public int GSMConnection { get; set; }

    public int HttpRequest { get; set; }

    public int HttpIn { get; set; }

    public int HttpOut { get; set; }
  }
}
