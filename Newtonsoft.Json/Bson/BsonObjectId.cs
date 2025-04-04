// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObjectId
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

#nullable disable
namespace Newtonsoft.Json.Bson
{
  public class BsonObjectId
  {
    public byte[] Value { get; private set; }

    public BsonObjectId(byte[] value)
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      this.Value = value.Length == 12 ? value : throw new ArgumentException("An ObjectId must be 12 bytes", nameof (value));
    }
  }
}
