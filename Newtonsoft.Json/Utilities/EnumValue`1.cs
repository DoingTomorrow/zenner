// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumValue`1
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

#nullable disable
namespace Newtonsoft.Json.Utilities
{
  internal class EnumValue<T> where T : struct
  {
    private readonly string _name;
    private readonly T _value;

    public string Name => this._name;

    public T Value => this._value;

    public EnumValue(string name, T value)
    {
      this._name = name;
      this._value = value;
    }
  }
}
