// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MethodCallExpressionNodeFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class MethodCallExpressionNodeFactory
  {
    public static IExpressionNode CreateExpressionNode(
      Type nodeType,
      MethodCallExpressionParseInfo parseInfo,
      object[] additionalConstructorParameters)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (nodeType), nodeType);
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (nodeType), nodeType, typeof (IExpressionNode));
      ArgumentUtility.CheckNotNull<object[]>(nameof (additionalConstructorParameters), additionalConstructorParameters);
      ConstructorInfo[] constructors = nodeType.GetConstructors();
      if (constructors.Length > 1)
        throw new ArgumentException(string.Format("Expression node type '{0}' contains too many constructors. It must only contain a single constructor, allowing null to be passed for any optional arguments.", (object) nodeType.FullName), nameof (nodeType));
      object[] parameterArray = MethodCallExpressionNodeFactory.GetParameterArray(constructors[0], parseInfo, additionalConstructorParameters);
      try
      {
        return (IExpressionNode) constructors[0].Invoke(parameterArray);
      }
      catch (ArgumentException ex)
      {
        throw new ExpressionNodeInstantiationException(MethodCallExpressionNodeFactory.GetArgumentMismatchMessage(ex), (Exception) ex);
      }
    }

    private static string GetArgumentMismatchMessage(ArgumentException ex)
    {
      return ex.Message.Contains(typeof (LambdaExpression).Name) && ex.Message.Contains(typeof (ConstantExpression).Name) ? string.Format("{0} If you tried to pass a delegate instead of a LambdaExpression, this is not supported because delegates are not parsable expressions.", (object) ex.Message) : string.Format("The given arguments did not match the expected arguments: {0}", (object) ex.Message);
    }

    private static object[] GetParameterArray(
      ConstructorInfo nodeTypeConstructor,
      MethodCallExpressionParseInfo parseInfo,
      object[] additionalConstructorParameters)
    {
      ParameterInfo[] parameters = nodeTypeConstructor.GetParameters();
      if (additionalConstructorParameters.Length > parameters.Length - 1)
        throw new ExpressionNodeInstantiationException(string.Format("The constructor of expression node type '{0}' only takes {1} parameters, but you specified {2} (including the parse info parameter).", (object) nodeTypeConstructor.DeclaringType.FullName, (object) parameters.Length, (object) (additionalConstructorParameters.Length + 1)));
      object[] parameterArray = new object[parameters.Length];
      parameterArray[0] = (object) parseInfo;
      additionalConstructorParameters.CopyTo((Array) parameterArray, 1);
      return parameterArray;
    }
  }
}
