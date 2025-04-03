// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.NestedClassEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public class NestedClassEmitter : AbstractTypeEmitter
  {
    public NestedClassEmitter(
      AbstractTypeEmitter maintype,
      string name,
      Type baseType,
      Type[] interfaces)
      : this(maintype, NestedClassEmitter.CreateTypeBuilder(maintype, name, TypeAttributes.NestedPublic | TypeAttributes.Sealed, baseType, interfaces))
    {
    }

    public NestedClassEmitter(
      AbstractTypeEmitter maintype,
      string name,
      TypeAttributes attributes,
      Type baseType,
      Type[] interfaces)
      : this(maintype, NestedClassEmitter.CreateTypeBuilder(maintype, name, attributes, baseType, interfaces))
    {
    }

    private static TypeBuilder CreateTypeBuilder(
      AbstractTypeEmitter maintype,
      string name,
      TypeAttributes attributes,
      Type baseType,
      Type[] interfaces)
    {
      return maintype.TypeBuilder.DefineNestedType(name, attributes, baseType, interfaces);
    }

    public NestedClassEmitter(AbstractTypeEmitter maintype, TypeBuilder typeBuilder)
      : base(typeBuilder)
    {
      maintype.Nested.Add(this);
    }
  }
}
