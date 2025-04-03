// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_ParameterStatics
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_ParameterStatics
  {
    internal S3_VariableTypes S3_VarType;
    internal double DefaultValue;
    internal double MinValue;
    internal double MaxValue;
    internal int MBusParameterLen;
    internal MBusParameterConverter MBusParamConverter;
    internal byte[] DefaultDifVif;
    internal S3_ParameterUnits ParameterUnit;
    internal Type ParameterStorageType;
    internal double ParameterUnitFactor;
    internal string ParameterInfo;
    internal string IsResource;
    internal string NeedResource;
    internal bool IsDynamicRAM_Parameter;
    internal string TranslatedName;
    internal string Description;
    internal byte VirtualDeviceNumber;

    internal S3_ParameterStatics(S3_VariableTypes S3_VarType) => this.S3_VarType = S3_VarType;
  }
}
