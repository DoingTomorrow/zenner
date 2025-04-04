// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.DefaultyProxyMethodBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  internal class DefaultyProxyMethodBuilder : IProxyMethodBuilder
  {
    public DefaultyProxyMethodBuilder()
      : this((IMethodBodyEmitter) new DefaultMethodEmitter())
    {
    }

    public DefaultyProxyMethodBuilder(IMethodBodyEmitter emitter)
    {
      this.MethodBodyEmitter = emitter != null ? emitter : throw new ArgumentNullException(nameof (emitter));
    }

    public IMethodBodyEmitter MethodBodyEmitter { get; private set; }

    public void CreateProxiedMethod(FieldInfo field, MethodInfo method, TypeBuilder typeBuilder)
    {
      ParameterInfo[] parameters = method.GetParameters();
      MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, CallingConventions.HasThis, method.ReturnType, ((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (param => param.ParameterType)).ToArray<Type>());
      Type[] genericArguments = method.GetGenericArguments();
      if (genericArguments.Length > 0)
      {
        List<string> stringList = new List<string>();
        for (int index = 0; index < genericArguments.Length; ++index)
          stringList.Add(string.Format("T{0}", (object) index));
        GenericTypeParameterBuilder[] parameterBuilderArray = methodBuilder.DefineGenericParameters(stringList.ToArray());
        for (int index = 0; index < genericArguments.Length; ++index)
          parameterBuilderArray[index].SetInterfaceConstraints(genericArguments[index].GetGenericParameterConstraints());
      }
      this.MethodBodyEmitter.EmitMethodBody(methodBuilder.GetILGenerator(), method, field);
    }
  }
}
