// Decompiled with JetBrains decompiler
// Type: AutoMapper.IDelegateFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace AutoMapper
{
  public interface IDelegateFactory
  {
    LateBoundMethod CreateGet(MethodInfo method);

    LateBoundPropertyGet CreateGet(PropertyInfo property);

    LateBoundFieldGet CreateGet(FieldInfo field);

    LateBoundFieldSet CreateSet(FieldInfo field);

    LateBoundPropertySet CreateSet(PropertyInfo property);

    LateBoundCtor CreateCtor(Type type);

    LateBoundParamsCtor CreateCtor(
      ConstructorInfo constructorInfo,
      IEnumerable<ConstructorParameterMap> ctorParams);
  }
}
