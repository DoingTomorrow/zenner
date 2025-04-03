// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.GeneratorUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public static class GeneratorUtil
  {
    public static void CopyOutAndRefParameters(
      TypeReference[] dereferencedArguments,
      LocalReference invocation,
      MethodInfo method,
      MethodEmitter emitter)
    {
      ParameterInfo[] parameters = method.GetParameters();
      if (!ArgumentsUtil.IsAnyByRef(parameters))
        return;
      LocalReference invocationArgs = GeneratorUtil.StoreInvocationArgumentsInLocal(emitter, invocation);
      for (int i = 0; i < parameters.Length; ++i)
      {
        if (parameters[i].ParameterType.IsByRef)
          emitter.CodeBuilder.AddStatement((Statement) GeneratorUtil.AssignArgument(dereferencedArguments, i, invocationArgs));
      }
    }

    private static AssignStatement AssignArgument(
      TypeReference[] dereferencedArguments,
      int i,
      LocalReference invocationArgs)
    {
      return new AssignStatement((Reference) dereferencedArguments[i], (Expression) GeneratorUtil.Argument(i, invocationArgs, dereferencedArguments));
    }

    private static ConvertExpression Argument(
      int i,
      LocalReference invocationArgs,
      TypeReference[] arguments)
    {
      return new ConvertExpression(arguments[i].Type, (Expression) new LoadRefArrayElementExpression(i, (Reference) invocationArgs));
    }

    private static LocalReference StoreInvocationArgumentsInLocal(
      MethodEmitter emitter,
      LocalReference invocation)
    {
      LocalReference invocationArgs = emitter.CodeBuilder.DeclareLocal(typeof (object[]));
      emitter.CodeBuilder.AddStatement((Statement) GeneratorUtil.GetArguments(invocationArgs, invocation));
      return invocationArgs;
    }

    private static AssignStatement GetArguments(
      LocalReference invocationArgs,
      LocalReference invocation)
    {
      return new AssignStatement((Reference) invocationArgs, (Expression) new MethodInvocationExpression((Reference) invocation, InvocationMethods.GetArguments, new Expression[0]));
    }
  }
}
