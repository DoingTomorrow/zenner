// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MbusTelegramCollection
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace DeviceCollector
{
  internal sealed class MbusTelegramCollection : List<MBusTelegramSet>
  {
    internal void Add(
      byte primaryAddress,
      long serialNumber,
      DateTime timePoint,
      byte[] mbusTelegram)
    {
      MbusTelegram telegram = new MbusTelegram(timePoint, mbusTelegram);
      if (this.ContainsKey(primaryAddress, serialNumber))
        this[primaryAddress, serialNumber].MBusTelegrams.Add(telegram);
      else
        this.Add(new MBusTelegramSet(primaryAddress, serialNumber, telegram));
    }

    public MBusTelegramSet this[byte primaryAddress, long serialNumber]
    {
      get
      {
        return this.Find((Predicate<MBusTelegramSet>) (device => (int) device.PrimaryAddress == (int) primaryAddress && device.SerialNumber == serialNumber));
      }
    }

    internal bool ContainsKey(byte primaryAddress, long serialNumber)
    {
      return this.Exists((Predicate<MBusTelegramSet>) (device => (int) device.PrimaryAddress == (int) primaryAddress && device.SerialNumber == serialNumber));
    }

    internal MBusTelegramSet GetTelegramsOfMBusDevice(byte primaryAddress, long serialNumber)
    {
      return this.Find((Predicate<MBusTelegramSet>) (device => (int) device.PrimaryAddress == (int) primaryAddress && device.SerialNumber == serialNumber));
    }
  }
}
