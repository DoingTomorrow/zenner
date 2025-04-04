// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.OpcodeLoadParameter
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

#nullable disable
namespace SmartFunctionCompiler
{
  internal enum OpcodeLoadParameter : byte
  {
    Load = 35, // 0x23
    CompareEQ = 36, // 0x24
    CompareNE = 37, // 0x25
    CompareGE = 38, // 0x26
    CompareGT = 39, // 0x27
    CompareLE = 40, // 0x28
    CompareLT = 41, // 0x29
    Add = 42, // 0x2A
    Sub = 43, // 0x2B
    Mul = 44, // 0x2C
    Div = 45, // 0x2D
    Mod = 46, // 0x2E
    And = 47, // 0x2F
    Or = 48, // 0x30
    XOr = 49, // 0x31
  }
}
