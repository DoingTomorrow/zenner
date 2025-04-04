// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.SmartFunctionIdentAndFlashParams
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;
using System.Collections.Generic;

#nullable disable
namespace SmartFunctionCompiler
{
  public class SmartFunctionIdentAndFlashParams : 
    SmartFunctionIdent,
    IComparable<SmartFunctionIdentAndFlashParams>
  {
    public SortedList<string, string> FlashParameters;
    public SmartFunctionResult FunctionResult;

    public SmartFunctionIdentAndFlashParams(
      string name,
      byte version,
      SmartFunctionResult functionResult)
      : base(name, version)
    {
      this.FunctionResult = functionResult;
      this.FlashParameters = new SortedList<string, string>();
    }

    public int CompareTo(SmartFunctionIdentAndFlashParams obj)
    {
      return this.CompareTo((SmartFunctionIdent) obj);
    }
  }
}
