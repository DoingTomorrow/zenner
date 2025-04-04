// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.BluetoothFactoryConfig
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.BlueSoleil;
using InTheHand.Net.Bluetooth.Widcomm;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  internal static class BluetoothFactoryConfig
  {
    internal const bool Default_oneStackOnly = true;
    internal const bool Default_reportAllErrors = false;
    internal static readonly Type MsftFactoryType = typeof (SocketsBluetoothFactory);
    internal static readonly Type WidcommFactoryType = typeof (WidcommBluetoothFactory);
    internal static readonly Type BlueSoleilFactoryType = typeof (BluesoleilFactory);
    internal static readonly string[] s_knownStacks = new string[3]
    {
      BluetoothFactoryConfig.MsftFactoryType.FullName,
      BluetoothFactoryConfig.WidcommFactoryType.FullName,
      BluetoothFactoryConfig.BlueSoleilFactoryType.FullName
    };
    private static readonly string[] ElementNames = new string[3]
    {
      "configuration",
      "InTheHand.Net.Personal",
      "BluetoothFactory"
    };

    internal static string[] KnownStacks => BluetoothFactorySection.GetInstance().StackList2;

    internal static bool ReportAllErrors => BluetoothFactorySection.GetInstance().ReportAllErrors;

    internal static bool OneStackOnly => BluetoothFactorySection.GetInstance().OneStackOnly;

    internal static bool WidcommICheckIgnorePlatform
    {
      get => BluetoothFactorySection.GetInstance().WidcommICheckIgnorePlatform;
    }

    internal static string GetEntryAssemblyPath()
    {
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      return entryAssembly == null ? (string) null : new Uri(entryAssembly.CodeBase).LocalPath;
    }

    internal static bool LoadManually(TextReader src, BluetoothFactoryConfig.Values v)
    {
      XmlDocument xd = new XmlDocument();
      xd.Load(src);
      return BluetoothFactoryConfig.LoadManually(xd, v);
    }

    private static bool LoadManually(XmlDocument xd, BluetoothFactoryConfig.Values v)
    {
      XmlNode xmlNode = (XmlNode) xd;
      for (int index = 0; index < BluetoothFactoryConfig.ElementNames.Length; ++index)
      {
        xmlNode = (XmlNode) xmlNode[BluetoothFactoryConfig.ElementNames[index]];
        if (xmlNode == null)
          return false;
      }
      XmlElement elem = (XmlElement) xmlNode;
      return BluetoothFactoryConfig.GetBoolOptionalAttribute(elem, "oneStackOnly", ref v.oneStackOnly) | BluetoothFactoryConfig.GetBoolOptionalAttribute(elem, "reportAllErrors", ref v.reportAllErrors);
    }

    private static bool GetBoolOptionalAttribute(XmlElement elem, string name, ref bool? var)
    {
      string attribute = elem.GetAttribute(name);
      if (StringUtilities.IsNullOrEmpty(attribute))
        return false;
      var = new bool?(XmlConvert.ToBoolean(attribute));
      return true;
    }

    internal sealed class Values
    {
      public bool? oneStackOnly;
      public bool? reportAllErrors;
    }
  }
}
