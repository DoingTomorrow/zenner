// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_ParameterLoader
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib.MapManagement;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_ParameterLoader
  {
    private static SortedList<string, S3_Parameter> S3ParameterCache = new SortedList<string, S3_Parameter>();

    static S3_ParameterLoader()
    {
      S3_ParameterDefineClass parameterDefineClass = new S3_ParameterDefineClass();
      int readIndex = 0;
      if (LoadDataFromMapClass.GetByteForced(parameterDefineClass.fullByteList, ref readIndex) > (byte) 0)
        throw new FormatException("Illegal parameter config class format");
      string str1 = "";
      while (readIndex < parameterDefineClass.fullByteList.Length)
      {
        try
        {
          str1 = LoadDataFromMapClass.GetStringForced(parameterDefineClass.fullByteList, ref readIndex);
          S3_VariableTypes byteForced1 = (S3_VariableTypes) LoadDataFromMapClass.GetByteForced(parameterDefineClass.fullByteList, ref readIndex);
          S3_MemorySegment byteForced2 = (S3_MemorySegment) LoadDataFromMapClass.GetByteForced(parameterDefineClass.fullByteList, ref readIndex);
          S3_Parameter s3Parameter = new S3_Parameter(str1, byteForced1, byteForced2);
          double? nullable1 = LoadDataFromMapClass.GetDouble(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.DefaultValue = nullable1.HasValue ? nullable1.Value : 0.0;
          double? nullable2 = LoadDataFromMapClass.GetDouble(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.MinValue = nullable2.HasValue ? nullable2.Value : 0.0;
          double? nullable3 = LoadDataFromMapClass.GetDouble(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.MaxValue = nullable3.HasValue ? nullable3.Value : double.MaxValue;
          s3Parameter.Statics.DefaultDifVif = LoadDataFromMapClass.GetBytes(parameterDefineClass.fullByteList, ref readIndex);
          int? nullable4 = LoadDataFromMapClass.GetInt(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.MBusParameterLen = nullable4.HasValue ? nullable4.Value : 0;
          string str2 = LoadDataFromMapClass.GetString(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.MBusParamConverter = str2 != null ? (MBusParameterConverter) Enum.Parse(typeof (MBusParameterConverter), str2, true) : MBusParameterConverter.None;
          s3Parameter.Statics.IsResource = LoadDataFromMapClass.GetString(parameterDefineClass.fullByteList, ref readIndex);
          string str3 = LoadDataFromMapClass.GetString(parameterDefineClass.fullByteList, ref readIndex);
          if (str3 == null)
          {
            s3Parameter.Statics.NeedResource = (string) null;
          }
          else
          {
            s3Parameter.Statics.NeedResource = str3;
            if (s3Parameter.Statics.NeedResource.Contains("_Input"))
            {
              if (s3Parameter.Statics.NeedResource.Contains("IO_1_Input"))
                s3Parameter.Statics.VirtualDeviceNumber = (byte) 1;
              else if (s3Parameter.Statics.NeedResource.Contains("IO_2_Input"))
                s3Parameter.Statics.VirtualDeviceNumber = (byte) 2;
              else if (s3Parameter.Statics.NeedResource.Contains("IO_3_Input"))
                s3Parameter.Statics.VirtualDeviceNumber = (byte) 3;
            }
          }
          string typeName = LoadDataFromMapClass.GetString(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.ParameterStorageType = typeName != null ? Type.GetType(typeName) : typeof (int);
          string str4 = LoadDataFromMapClass.GetString(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.ParameterUnit = str4 != null ? (S3_ParameterUnits) Enum.Parse(typeof (S3_ParameterUnits), str4, true) : S3_ParameterUnits.None;
          double? nullable5 = LoadDataFromMapClass.GetDouble(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.ParameterUnitFactor = nullable5.HasValue ? nullable5.Value : 0.0;
          string str5 = LoadDataFromMapClass.GetString(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.ParameterInfo = str5 ?? string.Empty;
          bool? nullable6 = LoadDataFromMapClass.GetBool(parameterDefineClass.fullByteList, ref readIndex);
          s3Parameter.Statics.IsDynamicRAM_Parameter = nullable6.HasValue && nullable6.Value;
          switch (s3Parameter.Statics.S3_VarType)
          {
            case S3_VariableTypes.INT8:
            case S3_VariableTypes.UINT8:
              s3Parameter.Alignment = 1;
              break;
            default:
              s3Parameter.Alignment = 2;
              break;
          }
          S3_ParameterLoader.S3ParameterCache.Add(str1, s3Parameter);
        }
        catch (Exception ex)
        {
          throw new FormatException("Illegal parameter config class. Error near parameter:" + str1 + Environment.NewLine + ex.Message);
        }
      }
    }

    internal static S3_Parameter GetS3Parameter(S3_Meter TheMeter, string ParameterName)
    {
      try
      {
        return S3_ParameterLoader.S3ParameterCache.IndexOfKey(ParameterName) >= 0 ? S3_ParameterLoader.S3ParameterCache[ParameterName].Clone(TheMeter) : (S3_Parameter) null;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "S3_Parameter '" + ParameterName + "' not found");
        throw ex;
      }
    }

    internal static AddressRange GetParameterAddressRange(
      SortedList<string, S3_Parameter> parameterList)
    {
      AddressRange parameterAddressRange = (AddressRange) null;
      foreach (S3_Parameter s3Parameter in (IEnumerable<S3_Parameter>) parameterList.Values)
      {
        if (parameterAddressRange == null)
        {
          parameterAddressRange = new AddressRange((uint) s3Parameter.BlockStartAddress, (uint) s3Parameter.ByteSize);
        }
        else
        {
          if ((long) s3Parameter.BlockStartAddress < (long) parameterAddressRange.StartAddress)
          {
            uint endAddress = parameterAddressRange.EndAddress;
            parameterAddressRange.StartAddress = (uint) s3Parameter.BlockStartAddress;
            parameterAddressRange.EndAddress = endAddress;
          }
          uint num = (uint) (s3Parameter.BlockStartAddress + s3Parameter.ByteSize - 1);
          if (parameterAddressRange.EndAddress < num)
            parameterAddressRange.EndAddress = num;
        }
      }
      return parameterAddressRange;
    }
  }
}
