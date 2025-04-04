// Decompiled with JetBrains decompiler
// Type: RestSharp.IJsonSerializerStrategy
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp
{
  internal interface IJsonSerializerStrategy
  {
    bool SerializeNonPrimitiveObject(object input, out object output);

    object DeserializeObject(object value, Type type);
  }
}
