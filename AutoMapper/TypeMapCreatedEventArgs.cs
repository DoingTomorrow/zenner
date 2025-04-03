// Decompiled with JetBrains decompiler
// Type: AutoMapper.TypeMapCreatedEventArgs
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public class TypeMapCreatedEventArgs : EventArgs
  {
    public TypeMap TypeMap { get; private set; }

    public TypeMapCreatedEventArgs(TypeMap typeMap) => this.TypeMap = typeMap;
  }
}
