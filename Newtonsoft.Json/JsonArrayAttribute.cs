// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonArrayAttribute : JsonContainerAttribute
  {
    private bool _allowNullItems;

    public bool AllowNullItems
    {
      get => this._allowNullItems;
      set => this._allowNullItems = value;
    }

    public JsonArrayAttribute()
    {
    }

    public JsonArrayAttribute(bool allowNullItems) => this._allowNullItems = allowNullItems;

    public JsonArrayAttribute(string id)
      : base(id)
    {
    }
  }
}
