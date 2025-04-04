// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionMethodDispatcher
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class ActionMethodDispatcher
  {
    private ActionMethodDispatcher.ActionExecutor _executor;

    public ActionMethodDispatcher(MethodInfo methodInfo)
    {
      this._executor = ActionMethodDispatcher.GetExecutor(methodInfo);
      this.MethodInfo = methodInfo;
    }

    public MethodInfo MethodInfo { get; private set; }

    public object Execute(ControllerBase controller, object[] parameters)
    {
      return this._executor(controller, parameters);
    }

    private static ActionMethodDispatcher.ActionExecutor GetExecutor(MethodInfo methodInfo)
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (ControllerBase), "controller");
      ParameterExpression array = Expression.Parameter(typeof (object[]), "parameters");
      List<Expression> arguments = new List<Expression>();
      ParameterInfo[] parameters = methodInfo.GetParameters();
      for (int index = 0; index < parameters.Length; ++index)
      {
        ParameterInfo parameterInfo = parameters[index];
        UnaryExpression unaryExpression = Expression.Convert((Expression) Expression.ArrayIndex((Expression) array, (Expression) Expression.Constant((object) index)), parameterInfo.ParameterType);
        arguments.Add((Expression) unaryExpression);
      }
      MethodCallExpression body = Expression.Call((Expression) (!methodInfo.IsStatic ? Expression.Convert((Expression) parameterExpression, methodInfo.ReflectedType) : (UnaryExpression) null), methodInfo, (IEnumerable<Expression>) arguments);
      return body.Type == typeof (void) ? ActionMethodDispatcher.WrapVoidAction(Expression.Lambda<ActionMethodDispatcher.VoidActionExecutor>((Expression) body, parameterExpression, array).Compile()) : Expression.Lambda<ActionMethodDispatcher.ActionExecutor>((Expression) Expression.Convert((Expression) body, typeof (object)), parameterExpression, array).Compile();
    }

    private static ActionMethodDispatcher.ActionExecutor WrapVoidAction(
      ActionMethodDispatcher.VoidActionExecutor executor)
    {
      return (ActionMethodDispatcher.ActionExecutor) ((controller, parameters) =>
      {
        executor(controller, parameters);
        return (object) null;
      });
    }

    private delegate object ActionExecutor(ControllerBase controller, object[] parameters);

    private delegate void VoidActionExecutor(ControllerBase controller, object[] parameters);
  }
}
