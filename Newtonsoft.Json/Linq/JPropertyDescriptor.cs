// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JPropertyDescriptor
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JPropertyDescriptor(string name) : PropertyDescriptor(name, (Attribute[]) null)
  {
    private static JObject CastInstance(object instance) => (JObject) instance;

    public override bool CanResetValue(object component) => false;

    public override object GetValue(object component)
    {
      return (object) JPropertyDescriptor.CastInstance(component)[this.Name];
    }

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value)
    {
      JToken jtoken = value is JToken ? (JToken) value : (JToken) new JValue(value);
      JPropertyDescriptor.CastInstance(component)[this.Name] = jtoken;
    }

    public override bool ShouldSerializeValue(object component) => false;

    public override Type ComponentType => typeof (JObject);

    public override bool IsReadOnly => false;

    public override Type PropertyType => typeof (object);

    protected override int NameHashCode => base.NameHashCode;
  }
}
