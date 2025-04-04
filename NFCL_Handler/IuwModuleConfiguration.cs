// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.IuwModuleConfiguration
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using System;

#nullable disable
namespace NFCL_Handler
{
  [Serializable]
  public class IuwModuleConfiguration
  {
    public ushort CommunicationScenario { get; private set; }

    public IuwModuleConfiguration.NdcModule Module { get; private set; }

    public byte Version { get; private set; }

    public IuwModuleConfiguration.NdcModuleType[] Types { get; private set; }

    public byte Size { get; private set; }

    public byte[] DevEUI { get; private set; }

    public byte[] JoinEUI { get; private set; }

    public byte[] AppKey { get; private set; }

    public byte? MinolDeviceType { get; private set; }

    public string Mode { get; private set; }

    public char? FrameFormat { get; private set; }

    public char? EncryptionMode { get; private set; }

    public uint? Frequency { get; private set; }

    public char? FrequencyBand { get; private set; }

    public byte[] AesKey { get; private set; }

    public uint? SendInterval { get; private set; }

    public char? HeaderFormat { get; private set; }

    public uint? NfcCycleSeconds { get; private set; }

    public static IuwModuleConfiguration Parse(byte[] bytes)
    {
      IuwModuleConfiguration moduleConfiguration1 = new IuwModuleConfiguration();
      moduleConfiguration1.CommunicationScenario = BitConverter.ToUInt16(bytes, 0);
      moduleConfiguration1.Module = (IuwModuleConfiguration.NdcModule) bytes[2];
      moduleConfiguration1.Version = bytes[3];
      moduleConfiguration1.Types = new IuwModuleConfiguration.NdcModuleType[(int) bytes[4]];
      for (int index = 0; index < moduleConfiguration1.Types.Length; ++index)
        moduleConfiguration1.Types[index] = (IuwModuleConfiguration.NdcModuleType) bytes[index + 5];
      int num1 = 5 + moduleConfiguration1.Types.Length;
      IuwModuleConfiguration moduleConfiguration2 = moduleConfiguration1;
      byte[] numArray1 = bytes;
      int index1 = num1;
      int index2 = index1 + 1;
      int num2 = (int) numArray1[index1];
      moduleConfiguration2.Size = (byte) num2;
      if (moduleConfiguration1.Size > (byte) 0)
      {
        if (moduleConfiguration1.Module == IuwModuleConfiguration.NdcModule.LoRa)
        {
          if (moduleConfiguration1.Version > (byte) 0)
            throw new Exception("LoRa module configuration v" + moduleConfiguration1.Version.ToString() + " is not supported!");
          moduleConfiguration1.DevEUI = bytes.SubArray<byte>(index2, 8);
          int index3 = index2 + 8;
          moduleConfiguration1.JoinEUI = bytes.SubArray<byte>(index3, 8);
          int index4 = index3 + 8;
          moduleConfiguration1.AppKey = bytes.SubArray<byte>(index4, 16);
          int num3 = index4 + 16;
          IuwModuleConfiguration moduleConfiguration3 = moduleConfiguration1;
          byte[] numArray2 = bytes;
          int index5 = num3;
          int num4 = index5 + 1;
          byte? nullable = new byte?(numArray2[index5]);
          moduleConfiguration3.MinolDeviceType = nullable;
        }
        else
        {
          if (moduleConfiguration1.Module != IuwModuleConfiguration.NdcModule.wMBus)
            throw new NotImplementedException("NDC Module " + moduleConfiguration1.Module.ToString() + " is not implemented!");
          IuwModuleConfiguration moduleConfiguration4 = moduleConfiguration1.Version >= (byte) 2 ? moduleConfiguration1 : throw new Exception("wM-Bus module configuration v" + moduleConfiguration1.Version.ToString() + " is not supported!");
          byte[] numArray3 = bytes;
          int index6 = index2;
          int num5 = index6 + 1;
          string str1 = ((char) numArray3[index6]).ToString();
          byte[] numArray4 = bytes;
          int index7 = num5;
          int num6 = index7 + 1;
          string str2 = ((char) numArray4[index7]).ToString();
          string str3 = str1 + str2;
          moduleConfiguration4.Mode = str3;
          IuwModuleConfiguration moduleConfiguration5 = moduleConfiguration1;
          byte[] numArray5 = bytes;
          int index8 = num6;
          int num7 = index8 + 1;
          char? nullable1 = new char?((char) numArray5[index8]);
          moduleConfiguration5.FrameFormat = nullable1;
          IuwModuleConfiguration moduleConfiguration6 = moduleConfiguration1;
          byte[] numArray6 = bytes;
          int index9 = num7;
          int startIndex1 = index9 + 1;
          char? nullable2 = new char?((char) numArray6[index9]);
          moduleConfiguration6.EncryptionMode = nullable2;
          moduleConfiguration1.Frequency = new uint?(BitConverter.ToUInt32(bytes, startIndex1));
          int num8 = startIndex1 + 4;
          IuwModuleConfiguration moduleConfiguration7 = moduleConfiguration1;
          byte[] numArray7 = bytes;
          int index10 = num8;
          int index11 = index10 + 1;
          char? nullable3 = new char?((char) numArray7[index10]);
          moduleConfiguration7.FrequencyBand = nullable3;
          moduleConfiguration1.AesKey = bytes.SubArray<byte>(index11, 16);
          int startIndex2 = index11 + 16;
          moduleConfiguration1.SendInterval = new uint?(BitConverter.ToUInt32(bytes, startIndex2));
          int num9 = startIndex2 + 4;
          IuwModuleConfiguration moduleConfiguration8 = moduleConfiguration1;
          byte[] numArray8 = bytes;
          int index12 = num9;
          int num10 = index12 + 1;
          char? nullable4 = new char?((char) numArray8[index12]);
          moduleConfiguration8.HeaderFormat = nullable4;
          int startIndex3 = num10 + 7;
          if (moduleConfiguration1.Version > (byte) 2)
            moduleConfiguration1.NfcCycleSeconds = new uint?(BitConverter.ToUInt32(bytes, startIndex3));
        }
      }
      return moduleConfiguration1;
    }

    public enum NdcModule : byte
    {
      LoRa = 1,
      wMBus = 2,
    }

    public enum NdcModuleType : byte
    {
      Any,
      LoRa,
      wMBus,
      Impulse,
      NB_IoT,
      LoRa_wMBus_Bat_AA,
      LoRa_wMBus_Bat_C,
    }
  }
}
