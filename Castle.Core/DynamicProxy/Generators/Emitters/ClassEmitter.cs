// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.ClassEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public class ClassEmitter : AbstractTypeEmitter
  {
    private const TypeAttributes DefaultAttributes = TypeAttributes.Public | TypeAttributes.Serializable;
    private readonly ModuleScope moduleScope;

    public ClassEmitter(
      ModuleScope modulescope,
      string name,
      Type baseType,
      IEnumerable<Type> interfaces)
      : this(modulescope, name, baseType, interfaces, TypeAttributes.Public | TypeAttributes.Serializable, ClassEmitter.ShouldForceUnsigned())
    {
    }

    public ClassEmitter(
      ModuleScope modulescope,
      string name,
      Type baseType,
      IEnumerable<Type> interfaces,
      TypeAttributes flags)
      : this(modulescope, name, baseType, interfaces, flags, ClassEmitter.ShouldForceUnsigned())
    {
    }

    public ClassEmitter(
      ModuleScope modulescope,
      string name,
      Type baseType,
      IEnumerable<Type> interfaces,
      TypeAttributes flags,
      bool forceUnsigned)
      : this(ClassEmitter.CreateTypeBuilder(modulescope, name, baseType, interfaces, flags, forceUnsigned))
    {
      interfaces = this.InitializeGenericArgumentsFromBases(ref baseType, interfaces);
      if (interfaces != null)
      {
        foreach (Type interfaceType in interfaces)
          this.TypeBuilder.AddInterfaceImplementation(interfaceType);
      }
      this.TypeBuilder.SetParent(baseType);
      this.moduleScope = modulescope;
    }

    public ModuleScope ModuleScope => this.moduleScope;

    private static TypeBuilder CreateTypeBuilder(
      ModuleScope modulescope,
      string name,
      Type baseType,
      IEnumerable<Type> interfaces,
      TypeAttributes flags,
      bool forceUnsigned)
    {
      bool inSignedModulePreferably = !forceUnsigned && !StrongNameUtil.IsAnyTypeFromUnsignedAssembly(baseType, interfaces);
      return modulescope.DefineType(inSignedModulePreferably, name, flags);
    }

    private static bool ShouldForceUnsigned() => !StrongNameUtil.CanStrongNameAssembly;

    public ClassEmitter(TypeBuilder typeBuilder)
      : base(typeBuilder)
    {
    }

    protected virtual IEnumerable<Type> InitializeGenericArgumentsFromBases(
      ref Type baseType,
      IEnumerable<Type> interfaces)
    {
      if (baseType != null && baseType.IsGenericTypeDefinition)
        throw new NotSupportedException("ClassEmitter does not support open generic base types. Type: " + baseType.FullName);
      if (interfaces == null)
        return interfaces;
      foreach (Type type in interfaces)
      {
        if (type.IsGenericTypeDefinition)
          throw new NotSupportedException("ClassEmitter does not support open generic interfaces. Type: " + type.FullName);
      }
      return interfaces;
    }
  }
}
