// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.ProxyBase
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.ComponentModel;

#nullable disable
namespace AutoMapper.Impl
{
  public abstract class ProxyBase
  {
    protected void NotifyPropertyChanged(PropertyChangedEventHandler handler, string method)
    {
      if (handler == null)
        return;
      handler((object) this, new PropertyChangedEventArgs(method));
    }
  }
}
