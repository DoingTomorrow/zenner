// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.ArgumentsUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public abstract class ArgumentsUtil
  {
    public static void EmitLoadOwnerAndReference(Reference reference, ILGenerator il)
    {
      if (reference == null)
        return;
      ArgumentsUtil.EmitLoadOwnerAndReference(reference.OwnerReference, il);
      reference.LoadReference(il);
    }

    public static void InitializeArgumentsByPosition(ArgumentReference[] args, bool isStatic)
    {
      int num = isStatic ? 0 : 1;
      for (int index = 0; index < args.Length; ++index)
        args[index].Position = index + num;
    }

    public static Type[] InitializeAndConvert(ArgumentReference[] args)
    {
      Type[] typeArray = new Type[args.Length];
      for (int index = 0; index < args.Length; ++index)
      {
        args[index].Position = index + 1;
        typeArray[index] = args[index].Type;
      }
      return typeArray;
    }

    public static ArgumentReference[] ConvertToArgumentReference(Type[] args)
    {
      ArgumentReference[] argumentReference = new ArgumentReference[args.Length];
      for (int index = 0; index < args.Length; ++index)
        argumentReference[index] = new ArgumentReference(args[index]);
      return argumentReference;
    }

    public static bool IsAnyByRef(ParameterInfo[] parameters)
    {
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (parameters[index].ParameterType.IsByRef)
          return true;
      }
      return false;
    }

    public static ArgumentReference[] ConvertToArgumentReference(ParameterInfo[] args)
    {
      ArgumentReference[] argumentReference = new ArgumentReference[args.Length];
      for (int index = 0; index < args.Length; ++index)
        argumentReference[index] = new ArgumentReference(args[index].ParameterType);
      return argumentReference;
    }

    public static ReferenceExpression[] ConvertToArgumentReferenceExpression(ParameterInfo[] args)
    {
      ReferenceExpression[] referenceExpression = new ReferenceExpression[args.Length];
      for (int index = 0; index < args.Length; ++index)
        referenceExpression[index] = new ReferenceExpression((Reference) new ArgumentReference(args[index].ParameterType, index + 1));
      return referenceExpression;
    }

    public static Expression[] ConvertArgumentReferenceToExpression(ArgumentReference[] args)
    {
      Expression[] expression = new Expression[args.Length];
      for (int index = 0; index < args.Length; ++index)
        expression[index] = args[index].ToExpression();
      return expression;
    }

    public static Type[] GetTypes(ParameterInfo[] parameters)
    {
      Type[] types = new Type[parameters.Length];
      for (int index = 0; index < parameters.Length; ++index)
        types[index] = parameters[index].ParameterType;
      return types;
    }
  }
}
