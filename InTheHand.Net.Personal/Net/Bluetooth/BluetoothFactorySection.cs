// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothFactorySection
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal sealed class BluetoothFactorySection : ConfigurationSection
  {
    internal const string GlobalSectionGroupName = "InTheHand.Net.Personal/";
    internal const string SectionName = "BluetoothFactory";
    private static BluetoothFactorySection s_instance;

    internal static BluetoothFactorySection GetInstance()
    {
      if (BluetoothFactorySection.s_instance == null)
      {
        BluetoothFactorySection.s_instance = (BluetoothFactorySection) ConfigurationManager.GetSection("InTheHand.Net.Personal/BluetoothFactory");
        if (BluetoothFactorySection.s_instance == null)
          BluetoothFactorySection.s_instance = new BluetoothFactorySection();
      }
      return BluetoothFactorySection.s_instance;
    }

    private BluetoothFactorySection()
    {
    }

    public override string ToString()
    {
      BluetoothStackConfigElement[] array = new BluetoothStackConfigElement[this.StackList.Count];
      this.StackList.CopyTo((ConfigurationElement[]) array, 0);
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[BluetoothFactorySection: OneStackOnly: {0}, ReportAllErrors: {1}, StackList: {2}, FooBar1: {3}]", (object) this.OneStackOnly, (object) this.ReportAllErrors, (object) ("{" + string.Join(", ", Array.ConvertAll<BluetoothStackConfigElement, string>(array, (Converter<BluetoothStackConfigElement, string>) (inp => inp != null ? inp.Name : "(null)"))) + "}"), (object) this.FooBar1);
    }

    [ConfigurationProperty("reportAllErrors", DefaultValue = false)]
    [ApplicationScopedSetting]
    public bool ReportAllErrors => (bool) this["reportAllErrors"];

    [ConfigurationProperty("oneStackOnly", DefaultValue = true)]
    [ApplicationScopedSetting]
    public bool OneStackOnly => (bool) this["oneStackOnly"];

    [ApplicationScopedSetting]
    [ConfigurationProperty("firstStack", DefaultValue = "defaultValue2")]
    public string FooBar1 => (string) this["firstStack"];

    [ApplicationScopedSetting]
    [ConfigurationProperty("widcommICheckIgnorePlatform", DefaultValue = false)]
    public bool WidcommICheckIgnorePlatform => (bool) this["widcommICheckIgnorePlatform"];

    [ConfigurationProperty("stackList")]
    [ApplicationScopedSetting]
    public BluetoothStackCollection StackList => (BluetoothStackCollection) this["stackList"];

    public string[] StackList2
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (BluetoothStackConfigElement stack in (ConfigurationElementCollection) this.StackList)
          stringList.Add(stack.Name);
        return stringList.ToArray();
      }
    }
  }
}
