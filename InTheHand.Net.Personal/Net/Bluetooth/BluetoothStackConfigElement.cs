// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothStackConfigElement
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Configuration;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal sealed class BluetoothStackConfigElement : ConfigurationElement
  {
    internal BluetoothStackConfigElement()
    {
    }

    internal BluetoothStackConfigElement(string name) => this[nameof (name)] = (object) name;

    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    [ApplicationScopedSetting]
    public string Name
    {
      get
      {
        string fullName = (string) this["name"];
        if (fullName.ToUpperInvariant() == "WIDCOMM")
          fullName = BluetoothFactoryConfig.WidcommFactoryType.FullName;
        else if (fullName.ToUpperInvariant() == "MSFT")
          fullName = BluetoothFactoryConfig.MsftFactoryType.FullName;
        return fullName;
      }
    }
  }
}
