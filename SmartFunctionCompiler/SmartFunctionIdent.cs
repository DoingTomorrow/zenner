// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunctionIdent
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SmartFunctionIdent : IComparable<SmartFunctionIdent>
  {
    public string FunctionDescription;
    public string RequiredFunctions;
    public string MemberOfGroups;

    public string Name { get; private set; }

    public byte Version { get; private set; }

    public byte InterpreterVersion { get; set; }

    public SmartFunctionIdent(string name, byte version)
    {
      this.Name = name;
      this.Version = version;
    }

    public int CompareTo(SmartFunctionIdent obj)
    {
      if (obj == null)
        return 1;
      int num = this.Name.CompareTo(obj.Name);
      return num != 0 ? num : this.Version.CompareTo(obj.Version);
    }

    public override string ToString() => this.Name + "; V:" + this.Version.ToString();
  }
}
