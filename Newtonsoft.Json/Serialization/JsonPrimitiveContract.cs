﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonPrimitiveContract
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class JsonPrimitiveContract : JsonContract
  {
    private static readonly Dictionary<Type, ReadType> ReadTypeMap = new Dictionary<Type, ReadType>()
    {
      [typeof (byte[])] = ReadType.ReadAsBytes,
      [typeof (byte)] = ReadType.ReadAsInt32,
      [typeof (short)] = ReadType.ReadAsInt32,
      [typeof (int)] = ReadType.ReadAsInt32,
      [typeof (Decimal)] = ReadType.ReadAsDecimal,
      [typeof (bool)] = ReadType.ReadAsBoolean,
      [typeof (string)] = ReadType.ReadAsString,
      [typeof (DateTime)] = ReadType.ReadAsDateTime,
      [typeof (DateTimeOffset)] = ReadType.ReadAsDateTimeOffset,
      [typeof (float)] = ReadType.ReadAsDouble,
      [typeof (double)] = ReadType.ReadAsDouble
    };

    internal PrimitiveTypeCode TypeCode { get; set; }

    public JsonPrimitiveContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Primitive;
      this.TypeCode = ConvertUtils.GetTypeCode(underlyingType);
      this.IsReadOnlyOrFixedSize = true;
      ReadType readType;
      if (!JsonPrimitiveContract.ReadTypeMap.TryGetValue(this.NonNullableUnderlyingType, out readType))
        return;
      this.InternalReadType = readType;
    }
  }
}
