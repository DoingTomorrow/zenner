// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.ServiceDiscoveryParams
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  internal sealed class ServiceDiscoveryParams
  {
    internal readonly BluetoothAddress address;
    internal readonly Guid serviceGuid;
    internal readonly SdpSearchScope searchScope;

    internal ServiceDiscoveryParams(
      BluetoothAddress address,
      Guid serviceGuid,
      SdpSearchScope searchScope)
    {
      this.address = address;
      this.serviceGuid = serviceGuid;
      this.searchScope = searchScope == SdpSearchScope.Anywhere || searchScope == SdpSearchScope.ServiceClassOnly ? searchScope : throw new ArgumentException("Unrecognized value for SdpSearchScope enum.", nameof (searchScope));
    }
  }
}
