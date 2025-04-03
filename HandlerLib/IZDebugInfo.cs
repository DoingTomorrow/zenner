// Decompiled with JetBrains decompiler
// Type: HandlerLib.IZDebugInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public interface IZDebugInfo
  {
    void setDebugInfo(string txt);

    void setDebugInfo(string txt, bool clear, int clearRows);

    void setErrorInfo(string txt);

    bool? ignoreError();
  }
}
