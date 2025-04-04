// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.DummyPropertyInfo
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  [Serializable]
  public sealed class DummyPropertyInfo : PropertyInfo
  {
    private readonly string name;
    private readonly Type type;

    public DummyPropertyInfo(string name, Type type)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      this.name = name;
      this.type = type;
    }

    public override Module Module => (Module) null;

    public override int MetadataToken => this.name.GetHashCode();

    public override object[] GetCustomAttributes(bool inherit) => new object[0];

    public override bool IsDefined(Type attributeType, bool inherit) => false;

    public override object GetValue(
      object obj,
      BindingFlags invokeAttr,
      Binder binder,
      object[] index,
      CultureInfo culture)
    {
      return obj;
    }

    public override void SetValue(
      object obj,
      object value,
      BindingFlags invokeAttr,
      Binder binder,
      object[] index,
      CultureInfo culture)
    {
    }

    public override MethodInfo[] GetAccessors(bool nonPublic) => new MethodInfo[0];

    public override MethodInfo GetGetMethod(bool nonPublic) => (MethodInfo) null;

    public override MethodInfo GetSetMethod(bool nonPublic) => (MethodInfo) null;

    public override ParameterInfo[] GetIndexParameters() => new ParameterInfo[0];

    public override string Name => this.name;

    public override Type DeclaringType => this.type;

    public override Type ReflectedType => (Type) null;

    public override Type PropertyType => this.type;

    public override PropertyAttributes Attributes => PropertyAttributes.None;

    public override bool CanRead => false;

    public override bool CanWrite => false;

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[0];
  }
}
