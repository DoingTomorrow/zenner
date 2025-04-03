// Decompiled with JetBrains decompiler
// Type: AutoMapper.IMappingExpression
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper
{
  public interface IMappingExpression
  {
    void ConvertUsing<TTypeConverter>();

    void ConvertUsing(Type typeConverterType);

    void As(Type typeOverride);

    IMappingExpression WithProfile(string profileName);

    IMappingExpression ForMember(
      string name,
      Action<IMemberConfigurationExpression> memberOptions);

    IMappingExpression ForSourceMember(
      string sourceMemberName,
      Action<ISourceMemberConfigurationExpression> memberOptions);
  }
}
