// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.DCUInfoRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class DCUInfoRecord
  {
    public string Project_ID { get; set; }

    public string DCU_ID { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string SWVersion { get; set; }

    public string ModemVersion { get; set; }

    public string Location { get; set; }

    public string Entrance { get; set; }

    public string Reg_MasterID { get; set; }

    public string DistReg { get; set; }

    public DateTime LastCall_In { get; set; }

    public DateTime NextCall_In { get; set; }

    public string PrimaryIP { get; set; }

    public string SecondIP { get; set; }

    public int Net_ID { get; set; }

    public int Node_ID { get; set; }

    public int Channel { get; set; }

    public int NumRegDevices { get; set; }

    public int NumRecDevices { get; set; }

    public int NumAssignDevices { get; set; }

    public int NumReservDevices { get; set; }

    public int Polling { get; set; }

    public long DDConfig { get; set; }

    public bool IsActive { get; set; }
  }
}
