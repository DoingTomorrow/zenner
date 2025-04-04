// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.DEV_VER_INFO
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct DEV_VER_INFO
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    internal byte[] bd_addr;
    internal readonly byte hci_version;
    internal readonly ushort hci_revision;
    internal readonly byte lmp_version;
    internal readonly ushort manufacturer;
    internal readonly ushort lmp_sub_version;

    internal DEV_VER_INFO(HciVersion fake)
      : this()
    {
      DEV_VER_INFO.SetManufacturerAndVersionsToUnknown(out this.manufacturer, out this.hci_version, out this.lmp_version);
    }

    internal static void SetManufacturerAndVersionsToUnknown(
      out ushort manufacturer,
      out byte hci_version,
      out byte lmp_version)
    {
      manufacturer = ushort.MaxValue;
      hci_version = byte.MaxValue;
      lmp_version = byte.MaxValue;
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[DEV_VER_INFO: {0}{1}{2}, {3}, {4}, {5}, {6}, {7}]", (object) '{', (object) '}', (object) BitConverter.ToString(this.bd_addr), (object) this.hci_version, (object) this.hci_revision, (object) this.lmp_version, (object) this.manufacturer, (object) this.lmp_sub_version);
    }
  }
}
