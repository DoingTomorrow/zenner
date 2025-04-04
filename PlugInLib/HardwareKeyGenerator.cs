// Decompiled with JetBrains decompiler
// Type: PlugInLib.HardwareKeyGenerator
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using System;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace PlugInLib
{
  public class HardwareKeyGenerator
  {
    public static string GetHardwareUniqueKey()
    {
      return HardwareKeyGenerator.GetHash("BASE >> " + HardwareKeyGenerator.GetMotherboardId() + "\nHDD >> " + HardwareKeyGenerator.GetHardDiskId());
    }

    public static string GetHardwareUniqueKeyBackwardsCompatible()
    {
      return HardwareKeyGenerator.GetHash("CPU >> " + HardwareKeyGenerator.GetCpuId() + "\nBIOS >> " + HardwareKeyGenerator.GetBiosId() + "\nBASE >> " + HardwareKeyGenerator.GetMotherboardId());
    }

    private static string GetHash(string s)
    {
      return HardwareKeyGenerator.GetHexString(new MD5CryptoServiceProvider().ComputeHash(new ASCIIEncoding().GetBytes(s)));
    }

    private static string GetHexString(byte[] bt)
    {
      string hexString = string.Empty;
      for (int index = 0; index < bt.Length; ++index)
      {
        int num1 = (int) bt[index];
        int num2 = num1 & 15;
        int num3 = num1 >> 4 & 15;
        string str = num3 <= 9 ? hexString + num3.ToString((IFormatProvider) CultureInfo.InvariantCulture) : hexString + ((char) (num3 - 10 + 65)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        hexString = num2 <= 9 ? str + num2.ToString((IFormatProvider) CultureInfo.InvariantCulture) : str + ((char) (num2 - 10 + 65)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        if (index + 1 != bt.Length && (index + 1) % 2 == 0)
          hexString += "-";
      }
      return hexString;
    }

    private static string Identifier(string wmiClass, string wmiProperty)
    {
      string str = "";
      foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances())
      {
        if (str == "")
        {
          try
          {
            if (instance[wmiProperty] != null)
            {
              str = instance[wmiProperty].ToString();
              break;
            }
            break;
          }
          catch
          {
          }
        }
      }
      return str;
    }

    private static ManagementObjectCollection GetIdentifierCollection(string wmiClass)
    {
      return new ManagementClass(wmiClass).GetInstances();
    }

    private static string GetFirstIdentifier(ManagementObjectCollection moc, string wmiProperty)
    {
      string firstIdentifier = "";
      foreach (ManagementObject managementObject in moc)
      {
        if (firstIdentifier == "")
        {
          try
          {
            if (managementObject[wmiProperty] != null)
            {
              firstIdentifier = managementObject[wmiProperty].ToString();
              break;
            }
            break;
          }
          catch
          {
          }
        }
      }
      return firstIdentifier;
    }

    private static string GetDiskIdentifier(ManagementObjectCollection moc, string wmiProperty)
    {
      string diskIdentifier = "";
      foreach (ManagementObject managementObject in moc)
      {
        if ((managementObject["InterfaceType"] == null || !(managementObject["InterfaceType"].ToString() == "USB")) && (managementObject["MediaType"] == null || !(managementObject["MediaType"].ToString() == "Removable Media")))
        {
          if (diskIdentifier == "")
          {
            try
            {
              if (managementObject[wmiProperty] != null)
              {
                diskIdentifier = managementObject[wmiProperty].ToString();
                break;
              }
              break;
            }
            catch
            {
            }
          }
        }
      }
      return diskIdentifier;
    }

    private static string GetCpuId()
    {
      string cpuId = HardwareKeyGenerator.Identifier("Win32_Processor", "UniqueId");
      if (cpuId == "")
      {
        cpuId = HardwareKeyGenerator.Identifier("Win32_Processor", "ProcessorId");
        if (cpuId == "")
        {
          string str1 = HardwareKeyGenerator.Identifier("Win32_Processor", "Name");
          if (str1 == "")
            str1 = HardwareKeyGenerator.Identifier("Win32_Processor", "Manufacturer");
          string str2 = HardwareKeyGenerator.Identifier("Win32_Processor", "MaxClockSpeed");
          cpuId = str1 + str2;
        }
      }
      return cpuId;
    }

    private static string GetBiosId()
    {
      return HardwareKeyGenerator.Identifier("Win32_BIOS", "Manufacturer") + HardwareKeyGenerator.Identifier("Win32_BIOS", "SMBIOSBIOSVersion") + HardwareKeyGenerator.Identifier("Win32_BIOS", "IdentificationCode") + HardwareKeyGenerator.Identifier("Win32_BIOS", "SerialNumber") + HardwareKeyGenerator.Identifier("Win32_BIOS", "ReleaseDate") + HardwareKeyGenerator.Identifier("Win32_BIOS", "Version");
    }

    private static string GetMotherboardId()
    {
      ManagementObjectCollection identifierCollection = HardwareKeyGenerator.GetIdentifierCollection("Win32_BaseBoard");
      return HardwareKeyGenerator.GetFirstIdentifier(identifierCollection, "Model") + HardwareKeyGenerator.GetFirstIdentifier(identifierCollection, "Manufacturer") + HardwareKeyGenerator.GetFirstIdentifier(identifierCollection, "Name") + HardwareKeyGenerator.GetFirstIdentifier(identifierCollection, "SerialNumber");
    }

    private static string GetHardDiskId()
    {
      ManagementObjectCollection identifierCollection = HardwareKeyGenerator.GetIdentifierCollection("Win32_DiskDrive");
      return HardwareKeyGenerator.GetDiskIdentifier(identifierCollection, "Model") + HardwareKeyGenerator.GetDiskIdentifier(identifierCollection, "Manufacturer") + HardwareKeyGenerator.GetDiskIdentifier(identifierCollection, "Signature") + HardwareKeyGenerator.GetDiskIdentifier(identifierCollection, "TotalHeads");
    }
  }
}
