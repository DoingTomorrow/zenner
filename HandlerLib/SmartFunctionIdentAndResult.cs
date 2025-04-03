// Decompiled with JetBrains decompiler
// Type: HandlerLib.SmartFunctionIdentAndResult
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public class SmartFunctionIdentAndResult : 
    SmartFunctionRuntimeResult,
    IComparable<SmartFunctionIdentAndResult>
  {
    public string Name { get; protected set; }

    public byte Version { get; protected set; }

    protected SmartFunctionIdentAndResult()
    {
    }

    protected SmartFunctionIdentAndResult(
      string name,
      byte version,
      ushort functionResult,
      ushort errorOffset)
    {
      this.Name = name;
      this.Version = version;
      this.FunctionResult = (SmartFunctionResult) functionResult;
      if (!this.Blocked)
        return;
      this.ErrorOffset = new ushort?(errorOffset);
    }

    public int CompareTo(SmartFunctionIdentAndResult obj)
    {
      if (obj == null)
        return 1;
      int num = this.Name.CompareTo(obj.Name);
      return num != 0 ? num : this.Version.CompareTo(obj.Version);
    }
  }
}
