// Decompiled with JetBrains decompiler
// Type: S3_Handler.Function
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public sealed class Function
  {
    internal List<string> ExclusiveNeedResources;
    internal List<string> NeedResources;
    internal List<string> SetResources;

    public int FunctionNumber { get; set; }

    public string FunctionName { get; set; }

    public string FunctionGroup { get; set; }

    public string FullName { get; set; }

    public string MeterUnit { get; set; }

    public int FirmwareVersionMin { get; set; }

    public int FirmwareVersionMax { get; set; }

    public int FunctionType { get; set; }

    public string FunctionShortInfo { get; set; }

    public string FunctionDescription { get; set; }

    public int FunctionVersion { get; set; }

    public int AccessRight { get; set; }

    public int UserGroup { get; set; }

    public string Symbolname { get; set; }

    public int LoggerType { get; set; }

    public string HardwareResource
    {
      set => this.AddResources(value, ref this.NeedResources);
    }

    public string AccessRights { get; set; }

    public string SoftwareResource
    {
      set => this.AddResources(value, ref this.SetResources);
    }

    public List<FunctionPrecompiled> Precompiled { get; set; }

    public Function() => this.Precompiled = new List<FunctionPrecompiled>();

    public override string ToString() => this.FunctionNumber.ToString() + " " + this.FunctionName;

    public List<FunctionPrecompiled> PointerList
    {
      get
      {
        return this.Precompiled.FindAll((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Pointer));
      }
    }

    public List<FunctionPrecompiled> EventList
    {
      get
      {
        return this.Precompiled.FindAll((Predicate<FunctionPrecompiled>) (e => e.RecordType == FunctionRecordType.Event_Click || e.RecordType == FunctionRecordType.Event_Hold || e.RecordType == FunctionRecordType.Event_None || e.RecordType == FunctionRecordType.Event_Press || e.RecordType == FunctionRecordType.Event_Timeout));
      }
    }

    private void AddResources(string resList, ref List<string> defaultResourcesArray)
    {
      if (resList == null)
        return;
      string[] strArray1 = Util.RemoveEmptyEntries(resList.Split(';'));
      if (strArray1.Length < 1)
        return;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (strArray1[index].Contains(":"))
        {
          string[] strArray2 = Util.RemoveEmptyEntries(strArray1[index].Split(':'));
          if (strArray2.Length != 2)
            ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "illegal resource:" + strArray1[index]);
          if (strArray2[0] == "excl")
          {
            if (this.ExclusiveNeedResources == null)
              this.ExclusiveNeedResources = new List<string>();
            if (this.NeedResources == null)
              this.NeedResources = new List<string>();
            this.ExclusiveNeedResources.Add(strArray2[1]);
            this.NeedResources.Add(strArray2[1]);
          }
          else
          {
            ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "illegal resource type:" + strArray2[0]);
            break;
          }
        }
        else
        {
          if (defaultResourcesArray == null)
            defaultResourcesArray = new List<string>();
          defaultResourcesArray.Add(strArray1[index]);
        }
      }
    }
  }
}
