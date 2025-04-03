// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.ProxyGeneratorFactoryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using AutoMapper.Impl;

#nullable disable
namespace AutoMapper.Internal
{
  public class ProxyGeneratorFactoryOverride : IProxyGeneratorFactory
  {
    public IProxyGenerator Create() => (IProxyGenerator) new ProxyGenerator();
  }
}
