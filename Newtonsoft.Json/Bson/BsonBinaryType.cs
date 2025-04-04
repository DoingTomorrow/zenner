// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonBinaryType
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Bson
{
  internal enum BsonBinaryType : byte
  {
    Binary = 0,
    Function = 1,
    [Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")] BinaryOld = 2,
    [Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")] UuidOld = 3,
    Uuid = 4,
    Md5 = 5,
    UserDefined = 128, // 0x80
  }
}
