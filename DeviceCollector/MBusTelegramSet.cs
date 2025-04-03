// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusTelegramSet
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System.Collections.Generic;

#nullable disable
namespace DeviceCollector
{
  internal sealed class MBusTelegramSet
  {
    public long SerialNumber { get; set; }

    public byte PrimaryAddress { get; set; }

    public List<MbusTelegram> MBusTelegrams { get; set; }

    public MBusTelegramSet(byte primaryAddress, long serialNumber)
    {
      this.MBusTelegrams = new List<MbusTelegram>();
      this.PrimaryAddress = primaryAddress;
      this.SerialNumber = serialNumber;
    }

    public MBusTelegramSet(byte primaryAddress, long serialNumber, MbusTelegram telegram)
      : this(primaryAddress, serialNumber)
    {
      this.MBusTelegrams.Add(telegram);
    }

    public override string ToString()
    {
      return string.Format("{0}+{1}", (object) this.SerialNumber, (object) this.PrimaryAddress);
    }
  }
}
