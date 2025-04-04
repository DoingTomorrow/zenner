// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynamicProxy.DefaultArgumentHandler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace NHibernate.Proxy.DynamicProxy
{
  internal class DefaultArgumentHandler : IArgumentHandler
  {
    public void PushArguments(ParameterInfo[] methodParameters, ILGenerator IL, bool isStatic)
    {
      ParameterInfo[] parameterInfoArray = methodParameters ?? new ParameterInfo[0];
      int length = parameterInfoArray.Length;
      IL.Emit(OpCodes.Ldc_I4, length);
      IL.Emit(OpCodes.Newarr, typeof (object));
      IL.Emit(OpCodes.Stloc_S, 0);
      if (length == 0)
      {
        IL.Emit(OpCodes.Ldloc_S, 0);
      }
      else
      {
        int num1 = 0;
        int num2 = 1;
        foreach (ParameterInfo parameterInfo in parameterInfoArray)
        {
          Type cls = parameterInfo.ParameterType.IsByRef ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType;
          IL.Emit(OpCodes.Ldloc_S, 0);
          IL.Emit(OpCodes.Ldc_I4, num1);
          if (parameterInfo.IsOut)
          {
            IL.Emit(OpCodes.Ldnull);
            IL.Emit(OpCodes.Stelem_Ref);
            ++num2;
            ++num1;
          }
          else
          {
            IL.Emit(OpCodes.Ldarg, num2);
            if (parameterInfo.ParameterType.IsByRef)
            {
              OpCode opCode;
              if (!OpCodesMap.TryGetLdindOpCode(parameterInfo.ParameterType.GetElementType(), out opCode))
                opCode = OpCodes.Ldind_Ref;
              IL.Emit(opCode);
            }
            if (cls.IsValueType || parameterInfo.ParameterType.IsByRef || cls.IsGenericParameter)
              IL.Emit(OpCodes.Box, cls);
            IL.Emit(OpCodes.Stelem_Ref);
            ++num1;
            ++num2;
          }
        }
        IL.Emit(OpCodes.Ldloc_S, 0);
      }
    }
  }
}
