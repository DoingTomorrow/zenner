// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.ClassOfDevice
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [Serializable]
  public sealed class ClassOfDevice : IEquatable<ClassOfDevice>
  {
    private readonly uint cod;

    [CLSCompliant(false)]
    public ClassOfDevice(uint cod) => this.cod = cod;

    public ClassOfDevice(DeviceClass device, ServiceClass service)
    {
      uint num = (uint) service << 13;
      this.cod = (uint) (device | (DeviceClass) num);
    }

    public DeviceClass Device => (DeviceClass) ((int) this.cod & 8188);

    public DeviceClass MajorDevice => (DeviceClass) ((int) this.cod & 7936);

    public ServiceClass Service => (ServiceClass) (this.cod >> 13);

    [CLSCompliant(false)]
    public uint Value => this.cod;

    public int ValueAsInt32 => (int) this.cod;

    public override int GetHashCode() => Convert.ToInt32(this.cod);

    public override string ToString() => this.cod.ToString("X");

    public override bool Equals(object obj) => this.Equals(obj as ClassOfDevice);

    public bool Equals(ClassOfDevice other) => other != null && (int) this.cod == (int) other.cod;
  }
}
