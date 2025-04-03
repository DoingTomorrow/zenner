// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapExceptionClass
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib.MapManagement
{
  [Serializable]
  public class MapExceptionClass : Exception
  {
    private MAP_EXCEPTION_HANDLE localHandle;

    public MapExceptionClass()
    {
    }

    public MapExceptionClass(string message)
      : base(message)
    {
    }

    public MapExceptionClass(MAP_EXCEPTION_HANDLE handle, string message)
      : base(message)
    {
      this.localHandle = handle;
    }

    public MapExceptionClass(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MapExceptionClass(MAP_EXCEPTION_HANDLE handle, Exception innerException)
      : base(handle.ToString(), innerException)
    {
      this.localHandle = handle;
    }

    public MapExceptionClass(MAP_EXCEPTION_HANDLE handle, string message, Exception innerException)
      : base(message, innerException)
    {
      this.localHandle = handle;
    }

    public override string ToString() => base.ToString();
  }
}
