// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonConverterAttribute : Attribute
  {
    private readonly Type _converterType;

    public Type ConverterType => this._converterType;

    public object[] ConverterParameters { get; private set; }

    public JsonConverterAttribute(Type converterType)
    {
      this._converterType = !(converterType == (Type) null) ? converterType : throw new ArgumentNullException(nameof (converterType));
    }

    public JsonConverterAttribute(Type converterType, params object[] converterParameters)
      : this(converterType)
    {
      this.ConverterParameters = converterParameters;
    }
  }
}
