﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaType
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Schema
{
  [Flags]
  [Obsolete("JSON Schema validation has been moved to its own package. See http://www.newtonsoft.com/jsonschema for more details.")]
  public enum JsonSchemaType
  {
    None = 0,
    String = 1,
    Float = 2,
    Integer = 4,
    Boolean = 8,
    Object = 16, // 0x00000010
    Array = 32, // 0x00000020
    Null = 64, // 0x00000040
    Any = Null | Array | Object | Boolean | Integer | Float | String, // 0x0000007F
  }
}
