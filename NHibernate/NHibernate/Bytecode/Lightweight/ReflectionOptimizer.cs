// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.Lightweight.ReflectionOptimizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using NHibernate.Util;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace NHibernate.Bytecode.Lightweight
{
  public class ReflectionOptimizer : IReflectionOptimizer, IInstantiationOptimizer
  {
    private readonly IAccessOptimizer accessOptimizer;
    private readonly CreateInstanceInvoker createInstanceMethod;
    protected readonly Type mappedType;
    private readonly Type typeOfThis;

    public IAccessOptimizer AccessOptimizer => this.accessOptimizer;

    public IInstantiationOptimizer InstantiationOptimizer => (IInstantiationOptimizer) this;

    public virtual object CreateInstance()
    {
      return this.createInstanceMethod == null ? (object) null : this.createInstanceMethod();
    }

    public ReflectionOptimizer(Type mappedType, IGetter[] getters, ISetter[] setters)
    {
      this.mappedType = mappedType;
      this.typeOfThis = mappedType.IsValueType ? mappedType.MakeByRefType() : mappedType;
      this.accessOptimizer = (IAccessOptimizer) new NHibernate.Bytecode.Lightweight.AccessOptimizer(this.GenerateGetPropertyValuesMethod(getters), this.GenerateSetPropertyValuesMethod(getters, setters), getters, setters);
      this.createInstanceMethod = this.CreateCreateInstanceMethod(mappedType);
    }

    protected virtual CreateInstanceInvoker CreateCreateInstanceMethod(Type type)
    {
      if (type.IsInterface || type.IsAbstract)
        return (CreateInstanceInvoker) null;
      DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), (Type[]) null, type, true);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      if (type.IsValueType)
      {
        LocalBuilder local = ilGenerator.DeclareLocal(type);
        ilGenerator.Emit(OpCodes.Ldloca, local);
        ilGenerator.Emit(OpCodes.Initobj, type);
        ilGenerator.Emit(OpCodes.Ldloc, local);
        ilGenerator.Emit(OpCodes.Box, type);
        ilGenerator.Emit(OpCodes.Ret);
        return (CreateInstanceInvoker) dynamicMethod.CreateDelegate(typeof (CreateInstanceInvoker));
      }
      ConstructorInfo defaultConstructor = ReflectHelper.GetDefaultConstructor(type);
      if (defaultConstructor != null)
      {
        ilGenerator.Emit(OpCodes.Newobj, defaultConstructor);
        ilGenerator.Emit(OpCodes.Ret);
        return (CreateInstanceInvoker) dynamicMethod.CreateDelegate(typeof (CreateInstanceInvoker));
      }
      this.ThrowExceptionForNoDefaultCtor(type);
      return (CreateInstanceInvoker) null;
    }

    protected virtual void ThrowExceptionForNoDefaultCtor(Type type)
    {
      throw new InstantiationException("Object class " + (object) type + " must declare a default (no-argument) constructor", type);
    }

    protected DynamicMethod CreateDynamicMethod(Type returnType, Type[] argumentTypes)
    {
      Type owner = this.mappedType.IsInterface ? typeof (object) : this.mappedType;
      bool skipVisibility = SecurityManager.IsGranted((IPermission) new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
      return new DynamicMethod(string.Empty, returnType, argumentTypes, owner, skipVisibility);
    }

    private static void EmitCastToReference(ILGenerator il, Type type)
    {
      if (type.IsValueType)
        il.Emit(OpCodes.Unbox, type);
      else
        il.Emit(OpCodes.Castclass, type);
    }

    private GetPropertyValuesInvoker GenerateGetPropertyValuesMethod(IGetter[] getters)
    {
      DynamicMethod dynamicMethod = this.CreateDynamicMethod(typeof (object[]), new Type[2]
      {
        typeof (object),
        typeof (GetterCallback)
      });
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      LocalBuilder local1 = ilGenerator.DeclareLocal(this.typeOfThis);
      LocalBuilder local2 = ilGenerator.DeclareLocal(typeof (object[]));
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ReflectionOptimizer.EmitCastToReference(ilGenerator, this.mappedType);
      ilGenerator.Emit(OpCodes.Stloc, local1);
      ilGenerator.Emit(OpCodes.Ldc_I4, getters.Length);
      ilGenerator.Emit(OpCodes.Newarr, typeof (object));
      ilGenerator.Emit(OpCodes.Stloc, local2);
      for (int index = 0; index < getters.Length; ++index)
      {
        IGetter getter = getters[index];
        ilGenerator.Emit(OpCodes.Ldloc, local2);
        ilGenerator.Emit(OpCodes.Ldc_I4, index);
        if (getter is IOptimizableGetter optimizableGetter)
        {
          ilGenerator.Emit(OpCodes.Ldloc, local1);
          optimizableGetter.Emit(ilGenerator);
          EmitUtil.EmitBoxIfNeeded(ilGenerator, getter.ReturnType);
        }
        else
        {
          MethodInfo method = typeof (GetterCallback).GetMethod("Invoke", new Type[2]
          {
            typeof (object),
            typeof (int)
          });
          ilGenerator.Emit(OpCodes.Ldarg_1);
          ilGenerator.Emit(OpCodes.Ldarg_0);
          ilGenerator.Emit(OpCodes.Ldc_I4, index);
          ilGenerator.Emit(OpCodes.Callvirt, method);
        }
        ilGenerator.Emit(OpCodes.Stelem_Ref);
      }
      ilGenerator.Emit(OpCodes.Ldloc, local2.LocalIndex);
      ilGenerator.Emit(OpCodes.Ret);
      return (GetPropertyValuesInvoker) dynamicMethod.CreateDelegate(typeof (GetPropertyValuesInvoker));
    }

    private SetPropertyValuesInvoker GenerateSetPropertyValuesMethod(
      IGetter[] getters,
      ISetter[] setters)
    {
      DynamicMethod dynamicMethod = this.CreateDynamicMethod((Type) null, new Type[3]
      {
        typeof (object),
        typeof (object[]),
        typeof (SetterCallback)
      });
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      LocalBuilder local = ilGenerator.DeclareLocal(this.typeOfThis);
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ReflectionOptimizer.EmitCastToReference(ilGenerator, this.mappedType);
      ilGenerator.Emit(OpCodes.Stloc, local.LocalIndex);
      for (int index = 0; index < setters.Length; ++index)
      {
        ISetter setter = setters[index];
        Type returnType = getters[index].ReturnType;
        if (setter is IOptimizableSetter optimizableSetter)
        {
          ilGenerator.Emit(OpCodes.Ldloc, local);
          ilGenerator.Emit(OpCodes.Ldarg_1);
          ilGenerator.Emit(OpCodes.Ldc_I4, index);
          ilGenerator.Emit(OpCodes.Ldelem_Ref);
          EmitUtil.PreparePropertyForSet(ilGenerator, returnType);
          optimizableSetter.Emit(ilGenerator);
        }
        else
        {
          MethodInfo method = typeof (SetterCallback).GetMethod("Invoke", new Type[3]
          {
            typeof (object),
            typeof (int),
            typeof (object)
          });
          ilGenerator.Emit(OpCodes.Ldarg_2);
          ilGenerator.Emit(OpCodes.Ldarg_0);
          ilGenerator.Emit(OpCodes.Ldc_I4, index);
          ilGenerator.Emit(OpCodes.Ldarg_1);
          ilGenerator.Emit(OpCodes.Ldc_I4, index);
          ilGenerator.Emit(OpCodes.Ldelem_Ref);
          ilGenerator.Emit(OpCodes.Callvirt, method);
        }
      }
      ilGenerator.Emit(OpCodes.Ret);
      return (SetPropertyValuesInvoker) dynamicMethod.CreateDelegate(typeof (SetPropertyValuesInvoker));
    }
  }
}
