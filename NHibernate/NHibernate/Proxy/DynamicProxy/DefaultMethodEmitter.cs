// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.DefaultMethodEmitter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  internal class DefaultMethodEmitter : IMethodBodyEmitter
  {
    private static readonly MethodInfo getInterceptor;
    private static readonly MethodInfo getGenericMethodFromHandle = typeof (MethodBase).GetMethod("GetMethodFromHandle", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[2]
    {
      typeof (RuntimeMethodHandle),
      typeof (RuntimeTypeHandle)
    }, (ParameterModifier[]) null);
    private static readonly MethodInfo getMethodFromHandle = typeof (MethodBase).GetMethod("GetMethodFromHandle", new Type[1]
    {
      typeof (RuntimeMethodHandle)
    });
    private static readonly MethodInfo getTypeFromHandle = typeof (Type).GetMethod("GetTypeFromHandle");
    private static readonly MethodInfo handlerMethod = typeof (IInterceptor).GetMethod("Intercept");
    private static readonly ConstructorInfo infoConstructor;
    private static readonly PropertyInfo interceptorProperty = typeof (IProxy).GetProperty("Interceptor");
    private static readonly ConstructorInfo notImplementedConstructor = typeof (NotImplementedException).GetConstructor(new Type[0]);
    private readonly IArgumentHandler _argumentHandler;

    static DefaultMethodEmitter()
    {
      DefaultMethodEmitter.getInterceptor = DefaultMethodEmitter.interceptorProperty.GetGetMethod();
      DefaultMethodEmitter.infoConstructor = typeof (InvocationInfo).GetConstructor(new Type[5]
      {
        typeof (object),
        typeof (MethodInfo),
        typeof (StackTrace),
        typeof (Type[]),
        typeof (object[])
      });
    }

    public DefaultMethodEmitter()
      : this((IArgumentHandler) new DefaultArgumentHandler())
    {
    }

    public DefaultMethodEmitter(IArgumentHandler argumentHandler)
    {
      this._argumentHandler = argumentHandler;
    }

    public void EmitMethodBody(ILGenerator IL, MethodInfo method, FieldInfo field)
    {
      ParameterInfo[] parameters = method.GetParameters();
      IL.DeclareLocal(typeof (object[]));
      IL.DeclareLocal(typeof (InvocationInfo));
      IL.DeclareLocal(typeof (Type[]));
      IL.Emit(OpCodes.Ldarg_0);
      IL.Emit(OpCodes.Callvirt, DefaultMethodEmitter.getInterceptor);
      Label label = IL.DefineLabel();
      IL.Emit(OpCodes.Ldnull);
      IL.Emit(OpCodes.Bne_Un, label);
      IL.Emit(OpCodes.Ldarg_0);
      for (int index = 0; index < method.GetParameters().Length; ++index)
        IL.Emit(OpCodes.Ldarg_S, (sbyte) (index + 1));
      IL.Emit(OpCodes.Call, method);
      IL.Emit(OpCodes.Ret);
      IL.MarkLabel(label);
      IL.Emit(OpCodes.Ldarg_0);
      Type declaringType = method.DeclaringType;
      IL.Emit(OpCodes.Ldtoken, method);
      if (declaringType.IsGenericType)
      {
        IL.Emit(OpCodes.Ldtoken, declaringType);
        IL.Emit(OpCodes.Call, DefaultMethodEmitter.getGenericMethodFromHandle);
      }
      else
        IL.Emit(OpCodes.Call, DefaultMethodEmitter.getMethodFromHandle);
      IL.Emit(OpCodes.Castclass, typeof (MethodInfo));
      this.PushStackTrace(IL);
      this.PushGenericArguments(method, IL);
      this._argumentHandler.PushArguments(parameters, IL, false);
      IL.Emit(OpCodes.Newobj, DefaultMethodEmitter.infoConstructor);
      IL.Emit(OpCodes.Stloc_1);
      IL.Emit(OpCodes.Ldarg_0);
      IL.Emit(OpCodes.Callvirt, DefaultMethodEmitter.getInterceptor);
      IL.Emit(OpCodes.Ldloc_1);
      IL.Emit(OpCodes.Callvirt, DefaultMethodEmitter.handlerMethod);
      this.PackageReturnType(method, IL);
      DefaultMethodEmitter.SaveRefArguments(IL, parameters);
      IL.Emit(OpCodes.Ret);
    }

    private static void SaveRefArguments(ILGenerator IL, ParameterInfo[] parameters)
    {
      MethodInfo method = typeof (InvocationInfo).GetMethod("get_Arguments");
      IL.Emit(OpCodes.Ldloc_1);
      IL.Emit(OpCodes.Call, method);
      IL.Emit(OpCodes.Stloc_0);
      foreach (ParameterInfo parameter in parameters)
      {
        string name = parameter.ParameterType.Name;
        if (parameter.ParameterType.IsByRef && name.EndsWith("&"))
        {
          IL.Emit(OpCodes.Ldarg, parameter.Position + 1);
          IL.Emit(OpCodes.Ldloc_0);
          IL.Emit(OpCodes.Ldc_I4, parameter.Position);
          IL.Emit(OpCodes.Ldelem_Ref);
          Type elementType = parameter.ParameterType.GetElementType();
          IL.Emit(OpCodes.Unbox_Any, elementType);
          OpCode stindInstruction = DefaultMethodEmitter.GetStindInstruction(parameter.ParameterType);
          IL.Emit(stindInstruction);
        }
      }
    }

    private static OpCode GetStindInstruction(Type parameterType)
    {
      OpCode opCode;
      return parameterType.IsByRef && OpCodesMap.TryGetStindOpCode(parameterType.GetElementType(), out opCode) ? opCode : OpCodes.Stind_Ref;
    }

    private void PushStackTrace(ILGenerator IL) => IL.Emit(OpCodes.Ldnull);

    private void PushGenericArguments(MethodInfo method, ILGenerator IL)
    {
      Type[] genericArguments = method.GetGenericArguments();
      int length = genericArguments.Length;
      IL.Emit(OpCodes.Ldc_I4, length);
      IL.Emit(OpCodes.Newarr, typeof (Type));
      if (length == 0)
        return;
      for (int index = 0; index < length; ++index)
      {
        Type cls = genericArguments[index];
        IL.Emit(OpCodes.Dup);
        IL.Emit(OpCodes.Ldc_I4, index);
        IL.Emit(OpCodes.Ldtoken, cls);
        IL.Emit(OpCodes.Call, DefaultMethodEmitter.getTypeFromHandle);
        IL.Emit(OpCodes.Stelem_Ref);
      }
    }

    private void PackageReturnType(MethodInfo method, ILGenerator IL)
    {
      Type returnType = method.ReturnType;
      if (returnType == typeof (void))
        IL.Emit(OpCodes.Pop);
      else
        IL.Emit(OpCodes.Unbox_Any, returnType);
    }
  }
}
