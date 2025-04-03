// Decompiled with JetBrains decompiler
// Type: ZENNER.ReadSettings
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ZENNER
{
  [Serializable]
  public sealed class ReadSettings
  {
    public DeviceModel System { get; set; }

    public List<long> Filter { get; set; }

    public List<Meter> Meters { get; set; }

    public EquipmentModel EquipmentModel { get; set; }

    public ProfileType ProfileType { get; set; }

    public override string ToString()
    {
      if (this.System != null)
        return this.System.ToString();
      return this.Meters != null && this.Meters.Count != 0 ? this.Meters[0].ToString() : base.ToString();
    }
  }
}
