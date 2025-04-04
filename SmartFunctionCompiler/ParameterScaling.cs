// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.ParameterScaling
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

#nullable disable
namespace SmartFunctionCompiler
{
  public enum ParameterScaling
  {
    none = 0,
    Volume = 16, // 0x00000010
    Flow = 32, // 0x00000020
    Temperature = 48, // 0x00000030
    State = 64, // 0x00000040
    mask = 240, // 0x000000F0
  }
}
