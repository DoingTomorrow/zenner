// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.LicenseManagement.HardwareKeyGenerator
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using System;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace MSS.Business.Modules.LicenseManagement
{
  public class HardwareKeyGenerator
  {
    public static string GetHardwareUniqueKey()
    {
      string motherboardId = HardwareKeyGenerator.GetMotherboardId();
      string hardDiskId = HardwareKeyGenerator.GetHardDiskId();
      string hash = HardwareKeyGenerator.GetHash("BASE >> " + motherboardId + "\nHDD >> " + hardDiskId);
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append("GetHardwareUniqueKey() = GetHash(BASE >> " + motherboardId + "\\nHDD >> " + hardDiskId + ") = ");
      stringBuilder.Append(hash);
      stringBuilder.Append(Environment.NewLine);
      MessageHandler.LogDebug(stringBuilder.ToString());
      return hash;
    }

    public static string GetHardwareUniqueKeyBackwardsCompatible()
    {
      string cpuId = HardwareKeyGenerator.GetCpuId();
      string biosId = HardwareKeyGenerator.GetBiosId();
      string motherboardId = HardwareKeyGenerator.GetMotherboardId();
      string hash = HardwareKeyGenerator.GetHash("CPU >> " + cpuId + "\nBIOS >> " + biosId + "\nBASE >> " + motherboardId);
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append("GetHardwareUniqueKeyBackwardsCompatible() = GetHash(CPU >> " + cpuId + "\\nBIOS >> " + biosId + "\\nBASE >> " + motherboardId + ") = ");
      stringBuilder.Append(hash);
      stringBuilder.Append(Environment.NewLine);
      MessageHandler.LogDebug(stringBuilder.ToString());
      return hash;
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

    private static string Identifier(string wmiClass, string wmiProperty, bool isHardDiskCheck = false)
    {
      string str = "";
      foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances())
      {
        if (!isHardDiskCheck || instance["InterfaceType"] == null || !(instance["InterfaceType"].ToString() == "USB"))
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
      }
      return str;
    }

    private static string GetCpuId()
    {
      StringBuilder stringBuilder = new StringBuilder("CPU ID:   ");
      string cpuId = HardwareKeyGenerator.Identifier("Win32_Processor", "UniqueId");
      if (cpuId == "")
      {
        cpuId = HardwareKeyGenerator.Identifier("Win32_Processor", "ProcessorId");
        if (cpuId == "")
        {
          string str1 = HardwareKeyGenerator.Identifier("Win32_Processor", "Name");
          if (str1 == "")
          {
            str1 = HardwareKeyGenerator.Identifier("Win32_Processor", "Manufacturer");
            stringBuilder.Append("ID(Win32_Processor, Manufacturer) = " + str1 + "   ");
          }
          else
            stringBuilder.Append("ID(Win32_Processor, Name) = " + str1 + "   ");
          string str2 = HardwareKeyGenerator.Identifier("Win32_Processor", "MaxClockSpeed");
          cpuId = str1 + str2;
          stringBuilder.Append("ID(Win32_Processor, MaxClockSpeed) = " + str2 + "   ");
        }
        else
          stringBuilder.Append("ID(Win32_Processor, ProcessorId) = " + cpuId + "   ");
      }
      else
        stringBuilder.Append("ID(Win32_Processor, UniqueId) = " + cpuId + "   ");
      stringBuilder.Append(Environment.NewLine);
      MessageHandler.LogDebug(stringBuilder.ToString());
      return cpuId;
    }

    private static string GetBiosId()
    {
      string str1 = HardwareKeyGenerator.Identifier("Win32_BIOS", "Manufacturer");
      string str2 = HardwareKeyGenerator.Identifier("Win32_BIOS", "SMBIOSBIOSVersion");
      string str3 = HardwareKeyGenerator.Identifier("Win32_BIOS", "IdentificationCode");
      string str4 = HardwareKeyGenerator.Identifier("Win32_BIOS", "SerialNumber");
      string str5 = HardwareKeyGenerator.Identifier("Win32_BIOS", "ReleaseDate");
      string str6 = HardwareKeyGenerator.Identifier("Win32_BIOS", "Version");
      string biosId = str1 + str2 + str3 + str4 + str5 + str6;
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append("BIOS ID:   ID(Win32_BIOS, Manufacturer) = " + str1);
      stringBuilder.Append(" + ID(Win32_BIOS, SMBIOSBIOSVersion) = " + str2);
      stringBuilder.Append(" + ID(Win32_BIOS, IdentificationCode) = " + str3);
      stringBuilder.Append(" + ID(Win32_BIOS, SerialNumber) = " + str4);
      stringBuilder.Append(" + ID(Win32_BIOS, ReleaseDate) = " + str5);
      stringBuilder.Append(" + ID(Win32_BIOS, Version) = " + str6);
      stringBuilder.Append(Environment.NewLine);
      MessageHandler.LogDebug(stringBuilder.ToString());
      return biosId;
    }

    private static string GetMotherboardId()
    {
      string str1 = HardwareKeyGenerator.Identifier("Win32_BaseBoard", "Model");
      string str2 = HardwareKeyGenerator.Identifier("Win32_BaseBoard", "Manufacturer");
      string str3 = HardwareKeyGenerator.Identifier("Win32_BaseBoard", "Name");
      string str4 = HardwareKeyGenerator.Identifier("Win32_BaseBoard", "SerialNumber");
      string motherboardId = str1 + str2 + str3 + str4;
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append("MotherboardID:   ID(Win32_BaseBoard, Model) = " + str1);
      stringBuilder.Append(" + ID(Win32_BaseBoard, Manufacturer) = " + str2);
      stringBuilder.Append(" + ID(Win32_BaseBoard, Name) = " + str3);
      stringBuilder.Append(" + ID(Win32_BaseBoard, SerialNumber) = " + str4);
      stringBuilder.Append(Environment.NewLine);
      MessageHandler.LogDebug(stringBuilder.ToString());
      return motherboardId;
    }

    private static string GetHardDiskId()
    {
      string str1 = HardwareKeyGenerator.Identifier("Win32_DiskDrive", "Model", true);
      string str2 = HardwareKeyGenerator.Identifier("Win32_DiskDrive", "Manufacturer", true);
      string str3 = HardwareKeyGenerator.Identifier("Win32_DiskDrive", "Signature", true);
      string str4 = HardwareKeyGenerator.Identifier("Win32_DiskDrive", "TotalHeads", true);
      string hardDiskId = str1 + str2 + str3 + str4;
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append("HDD ID:   ID(Win32_DiskDrive, Model) = " + str1);
      stringBuilder.Append(" + ID(Win32_DiskDrive, Manufacturer) = " + str2);
      stringBuilder.Append(" + ID(Win32_DiskDrive, Signature) = " + str3);
      stringBuilder.Append(" + ID(Win32_DiskDrive, TotalHeads) = " + str4);
      stringBuilder.Append(Environment.NewLine);
      MessageHandler.LogDebug(stringBuilder.ToString());
      return hardDiskId;
    }
  }
}
