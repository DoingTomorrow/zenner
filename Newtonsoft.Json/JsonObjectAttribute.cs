// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonObjectAttribute
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonObjectAttribute : JsonContainerAttribute
  {
    private MemberSerialization _memberSerialization;
    internal Required? _itemRequired;

    public MemberSerialization MemberSerialization
    {
      get => this._memberSerialization;
      set => this._memberSerialization = value;
    }

    public Required ItemRequired
    {
      get => this._itemRequired ?? Required.Default;
      set => this._itemRequired = new Required?(value);
    }

    public JsonObjectAttribute()
    {
    }

    public JsonObjectAttribute(MemberSerialization memberSerialization)
    {
      this.MemberSerialization = memberSerialization;
    }

    public JsonObjectAttribute(string id)
      : base(id)
    {
    }
  }
}
