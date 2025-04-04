// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunctionIdentAndCode
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SmartFunctionIdentAndCode : SmartFunctionIdent, IComparable<SmartFunctionIdentAndCode>
  {
    public string FunctionEvent { get; private set; }

    public byte[] Code { get; private set; }

    public SmartFunctionIdentAndCode(
      string name,
      byte version,
      byte interpreterVersion,
      string functionEvent,
      byte[] functionCode)
      : base(name, version)
    {
      this.InterpreterVersion = interpreterVersion;
      this.FunctionEvent = functionEvent;
      this.Code = functionCode;
    }

    public int CompareTo(SmartFunctionIdentAndCode obj) => this.CompareTo((SmartFunctionIdent) obj);
  }
}
