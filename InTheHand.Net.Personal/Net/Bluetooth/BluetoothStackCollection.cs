// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothStackCollection
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Configuration;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  [ConfigurationCollection(typeof (BluetoothStackConfigElement))]
  internal sealed class BluetoothStackCollection : ConfigurationElementCollection
  {
    private BluetoothStackCollection()
    {
      foreach (string knownStack in BluetoothFactoryConfig.s_knownStacks)
        this.BaseAdd((ConfigurationElement) new BluetoothStackConfigElement(knownStack));
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return (ConfigurationElement) new BluetoothStackConfigElement();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return (object) ((BluetoothStackConfigElement) element).Name;
    }
  }
}
