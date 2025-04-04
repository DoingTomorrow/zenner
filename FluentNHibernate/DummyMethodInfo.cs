// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.DummyMethodInfo
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  internal sealed class DummyMethodInfo : MethodInfo
  {
    private readonly string name;
    private readonly Type type;

    public DummyMethodInfo(string name, Type type)
    {
      this.name = name;
      this.type = type;
    }

    public override Type ReturnType => this.type;

    public override object[] GetCustomAttributes(bool inherit) => new object[0];

    public override bool IsDefined(Type attributeType, bool inherit) => false;

    public override ParameterInfo[] GetParameters() => new ParameterInfo[0];

    public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.IL;

    public override object Invoke(
      object obj,
      BindingFlags invokeAttr,
      Binder binder,
      object[] parameters,
      CultureInfo culture)
    {
      return obj;
    }

    public override MethodInfo GetBaseDefinition() => (MethodInfo) null;

    public override ICustomAttributeProvider ReturnTypeCustomAttributes
    {
      get => (ICustomAttributeProvider) null;
    }

    public override string Name => this.name;

    public override Type DeclaringType => (Type) null;

    public override Type ReflectedType => (Type) null;

    public override RuntimeMethodHandle MethodHandle => new RuntimeMethodHandle();

    public override MethodAttributes Attributes => MethodAttributes.Public;

    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => new object[0];
  }
}
