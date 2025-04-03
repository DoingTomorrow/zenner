// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.Meter
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class Meter
  {
    public Guid ID { get; set; }

    public string SerialNumber { get; set; }

    public DeviceModel DeviceModel { get; set; }

    public ConnectionAdjuster ConnectionAdjuster { get; set; }

    public Scheduler.TriggerItem Interval { get; set; }

    public List<long> Filter { get; set; }

    public Dictionary<AdditionalInfoKey, string> AdditionalInfo { get; set; }

    public string AdditionalInfoString
    {
      get
      {
        if (this.AdditionalInfo == null)
          return string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<AdditionalInfoKey, string> keyValuePair in this.AdditionalInfo)
          stringBuilder.Append(keyValuePair.Key.ToString()).Append(": ").AppendLine(keyValuePair.Value);
        return stringBuilder.ToString();
      }
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.SerialNumber) && this.DeviceModel == null)
        return base.ToString();
      if (!string.IsNullOrEmpty(this.SerialNumber) && this.DeviceModel == null)
        return this.SerialNumber;
      return string.IsNullOrEmpty(this.SerialNumber) && this.DeviceModel != null ? this.DeviceModel.Name : this.SerialNumber + " " + this.DeviceModel.Name;
    }

    public Meter Create(string type)
    {
      if (!(type == "M8"))
        return (Meter) null;
      return new Meter()
      {
        DeviceModel = new DeviceModel()
        {
          DeviceModelID = 76
        }
      };
    }
  }
}
