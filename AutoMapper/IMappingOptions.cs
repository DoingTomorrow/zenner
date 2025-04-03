// Decompiled with JetBrains decompiler
// Type: AutoMapper.IMappingOptions
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public interface IMappingOptions
  {
    INamingConvention SourceMemberNamingConvention { get; set; }

    INamingConvention DestinationMemberNamingConvention { get; set; }

    IEnumerable<string> Prefixes { get; }

    IEnumerable<string> Postfixes { get; }

    IEnumerable<string> DestinationPrefixes { get; }

    IEnumerable<string> DestinationPostfixes { get; }

    IEnumerable<AliasedMember> Aliases { get; }

    bool ConstructorMappingEnabled { get; }

    bool DataReaderMapperYieldReturnEnabled { get; }

    IEnumerable<MethodInfo> SourceExtensionMethods { get; }
  }
}
