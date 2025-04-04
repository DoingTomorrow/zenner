// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing
{
  public abstract class ExpressionTreeVisitor
  {
    public static bool IsSupportedStandardExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      switch (expression.NodeType)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.ArrayLength:
        case ExpressionType.ArrayIndex:
        case ExpressionType.Call:
        case ExpressionType.Coalesce:
        case ExpressionType.Conditional:
        case ExpressionType.Constant:
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Divide:
        case ExpressionType.Equal:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.Invoke:
        case ExpressionType.Lambda:
        case ExpressionType.LeftShift:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.ListInit:
        case ExpressionType.MemberAccess:
        case ExpressionType.MemberInit:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.NegateChecked:
        case ExpressionType.New:
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
        case ExpressionType.Not:
        case ExpressionType.NotEqual:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.Parameter:
        case ExpressionType.Power:
        case ExpressionType.Quote:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
        case ExpressionType.TypeAs:
        case ExpressionType.TypeIs:
          return true;
        default:
          return false;
      }
    }

    public static bool IsRelinqExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      switch (expression.NodeType)
      {
        case (ExpressionType) 100001:
        case (ExpressionType) 100002:
          return true;
        default:
          return false;
      }
    }

    public static bool IsExtensionExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return expression is ExtensionExpression;
    }

    public static bool IsUnknownNonExtensionExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return !ExpressionTreeVisitor.IsSupportedStandardExpression(expression) && !ExpressionTreeVisitor.IsExtensionExpression(expression) && !ExpressionTreeVisitor.IsRelinqExpression(expression);
    }

    public static IEnumerable<Expression> AdjustArgumentsForNewExpression(
      IList<Expression> arguments,
      IList<MemberInfo> members)
    {
      ArgumentUtility.CheckNotNull<IList<Expression>>(nameof (arguments), arguments);
      ArgumentUtility.CheckNotNull<IList<MemberInfo>>(nameof (members), members);
      Trace.Assert(arguments.Count == members.Count);
      for (int i = 0; i < arguments.Count; ++i)
      {
        Type memberReturnType = ReflectionUtility.GetMemberReturnType(members[i]);
        if (arguments[i].Type == memberReturnType)
          yield return arguments[i];
        else
          yield return (Expression) Expression.Convert(arguments[i], memberReturnType);
      }
    }

    public virtual Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      if (expression is ExtensionExpression extensionExpression)
        return extensionExpression.Accept(this);
      switch (expression.NodeType)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.ArrayIndex:
        case ExpressionType.Coalesce:
        case ExpressionType.Divide:
        case ExpressionType.Equal:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LeftShift:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.NotEqual:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.Power:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return this.VisitBinaryExpression((BinaryExpression) expression);
        case ExpressionType.ArrayLength:
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
          return this.VisitUnaryExpression((UnaryExpression) expression);
        case ExpressionType.Call:
          return this.VisitMethodCallExpression((MethodCallExpression) expression);
        case ExpressionType.Conditional:
          return this.VisitConditionalExpression((ConditionalExpression) expression);
        case ExpressionType.Constant:
          return this.VisitConstantExpression((ConstantExpression) expression);
        case ExpressionType.Invoke:
          return this.VisitInvocationExpression((InvocationExpression) expression);
        case ExpressionType.Lambda:
          return this.VisitLambdaExpression((LambdaExpression) expression);
        case ExpressionType.ListInit:
          return this.VisitListInitExpression((ListInitExpression) expression);
        case ExpressionType.MemberAccess:
          return this.VisitMemberExpression((MemberExpression) expression);
        case ExpressionType.MemberInit:
          return this.VisitMemberInitExpression((MemberInitExpression) expression);
        case ExpressionType.New:
          return this.VisitNewExpression((NewExpression) expression);
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
          return this.VisitNewArrayExpression((NewArrayExpression) expression);
        case ExpressionType.Parameter:
          return this.VisitParameterExpression((ParameterExpression) expression);
        case ExpressionType.TypeIs:
          return this.VisitTypeBinaryExpression((TypeBinaryExpression) expression);
        case (ExpressionType) 100001:
          return this.VisitQuerySourceReferenceExpression((QuerySourceReferenceExpression) expression);
        case (ExpressionType) 100002:
          return this.VisitSubQueryExpression((SubQueryExpression) expression);
        default:
          return this.VisitUnknownNonExtensionExpression(expression);
      }
    }

    public virtual T VisitAndConvert<T>(T expression, string methodName) where T : Expression
    {
      ArgumentUtility.CheckNotNull<string>(nameof (methodName), methodName);
      if ((object) expression == null)
        return default (T);
      return this.VisitExpression((Expression) expression) is T obj ? obj : throw new InvalidOperationException(string.Format("When called from '{0}', expressions of type '{1}' can only be replaced with other non-null expressions of type '{2}'.", (object) methodName, (object) typeof (T).Name, (object) typeof (T).Name));
    }

    public virtual ReadOnlyCollection<T> VisitAndConvert<T>(
      ReadOnlyCollection<T> expressions,
      string callerName)
      where T : Expression
    {
      ArgumentUtility.CheckNotNull<ReadOnlyCollection<T>>(nameof (expressions), expressions);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (callerName), callerName);
      return this.VisitList<T>(expressions, (Func<T, T>) (expression => this.VisitAndConvert<T>(expression, callerName)));
    }

    public ReadOnlyCollection<T> VisitList<T>(ReadOnlyCollection<T> list, Func<T, T> visitMethod) where T : class
    {
      ArgumentUtility.CheckNotNull<ReadOnlyCollection<T>>(nameof (list), list);
      ArgumentUtility.CheckNotNull<Func<T, T>>(nameof (visitMethod), visitMethod);
      List<T> objList = (List<T>) null;
      for (int index = 0; index < list.Count; ++index)
      {
        T obj1 = list[index];
        T obj2 = visitMethod(obj1);
        if ((object) obj2 == null)
          throw new NotSupportedException("The current list only supports objects of type '" + typeof (T).Name + "' as its elements.");
        if ((object) obj1 != (object) obj2)
        {
          if (objList == null)
            objList = new List<T>((IEnumerable<T>) list);
          objList[index] = obj2;
        }
      }
      return objList != null ? objList.AsReadOnly() : list;
    }

    protected internal virtual Expression VisitExtensionExpression(ExtensionExpression expression)
    {
      ArgumentUtility.CheckNotNull<ExtensionExpression>(nameof (expression), expression);
      return expression.VisitChildren(this);
    }

    protected internal virtual Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      throw new NotSupportedException(string.Format("Expression type '{0}' is not supported by this {1}.", (object) expression.GetType().Name, (object) this.GetType().Name));
    }

    protected virtual Expression VisitUnaryExpression(UnaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<UnaryExpression>(nameof (expression), expression);
      Expression expression1 = this.VisitExpression(expression.Operand);
      if (expression1 == expression.Operand)
        return (Expression) expression;
      return expression.NodeType == ExpressionType.UnaryPlus ? (Expression) Expression.UnaryPlus(expression1, expression.Method) : (Expression) Expression.MakeUnary(expression.NodeType, expression1, expression.Type, expression.Method);
    }

    protected virtual Expression VisitBinaryExpression(BinaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<BinaryExpression>(nameof (expression), expression);
      Expression left = this.VisitExpression(expression.Left);
      Expression right = this.VisitExpression(expression.Right);
      LambdaExpression conversion = (LambdaExpression) this.VisitExpression((Expression) expression.Conversion);
      return left != expression.Left || right != expression.Right || conversion != expression.Conversion ? (Expression) Expression.MakeBinary(expression.NodeType, left, right, expression.IsLiftedToNull, expression.Method, conversion) : (Expression) expression;
    }

    protected virtual Expression VisitTypeBinaryExpression(TypeBinaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<TypeBinaryExpression>(nameof (expression), expression);
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 != expression.Expression ? (Expression) Expression.TypeIs(expression1, expression.TypeOperand) : (Expression) expression;
    }

    protected virtual Expression VisitConstantExpression(ConstantExpression expression)
    {
      ArgumentUtility.CheckNotNull<ConstantExpression>(nameof (expression), expression);
      return (Expression) expression;
    }

    protected virtual Expression VisitConditionalExpression(ConditionalExpression expression)
    {
      ArgumentUtility.CheckNotNull<ConditionalExpression>(nameof (expression), expression);
      Expression test = this.VisitExpression(expression.Test);
      Expression ifFalse = this.VisitExpression(expression.IfFalse);
      Expression ifTrue = this.VisitExpression(expression.IfTrue);
      return test != expression.Test || ifFalse != expression.IfFalse || ifTrue != expression.IfTrue ? (Expression) Expression.Condition(test, ifTrue, ifFalse) : (Expression) expression;
    }

    protected virtual Expression VisitParameterExpression(ParameterExpression expression)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (expression), expression);
      return (Expression) expression;
    }

    protected virtual Expression VisitLambdaExpression(LambdaExpression expression)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (expression), expression);
      ReadOnlyCollection<ParameterExpression> parameters = this.VisitAndConvert<ParameterExpression>(expression.Parameters, nameof (VisitLambdaExpression));
      Expression body = this.VisitExpression(expression.Body);
      return body != expression.Body || parameters != expression.Parameters ? (Expression) Expression.Lambda(expression.Type, body, (IEnumerable<ParameterExpression>) parameters) : (Expression) expression;
    }

    protected virtual Expression VisitMethodCallExpression(MethodCallExpression expression)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expression), expression);
      Expression instance = this.VisitExpression(expression.Object);
      ReadOnlyCollection<Expression> arguments = this.VisitAndConvert<Expression>(expression.Arguments, nameof (VisitMethodCallExpression));
      return instance != expression.Object || arguments != expression.Arguments ? (Expression) Expression.Call(instance, expression.Method, (IEnumerable<Expression>) arguments) : (Expression) expression;
    }

    protected virtual Expression VisitInvocationExpression(InvocationExpression expression)
    {
      ArgumentUtility.CheckNotNull<InvocationExpression>(nameof (expression), expression);
      Expression expression1 = this.VisitExpression(expression.Expression);
      ReadOnlyCollection<Expression> arguments = this.VisitAndConvert<Expression>(expression.Arguments, nameof (VisitInvocationExpression));
      return expression1 != expression.Expression || arguments != expression.Arguments ? (Expression) Expression.Invoke(expression1, (IEnumerable<Expression>) arguments) : (Expression) expression;
    }

    protected virtual Expression VisitMemberExpression(MemberExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberExpression>(nameof (expression), expression);
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 != expression.Expression ? (Expression) Expression.MakeMemberAccess(expression1, expression.Member) : (Expression) expression;
    }

    protected virtual Expression VisitNewExpression(NewExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (expression), expression);
      ReadOnlyCollection<Expression> arguments = this.VisitAndConvert<Expression>(expression.Arguments, nameof (VisitNewExpression));
      if (arguments == expression.Arguments)
        return (Expression) expression;
      return expression.Members == null ? (Expression) Expression.New(expression.Constructor, (IEnumerable<Expression>) arguments) : (Expression) Expression.New(expression.Constructor, ExpressionTreeVisitor.AdjustArgumentsForNewExpression((IList<Expression>) arguments, (IList<MemberInfo>) expression.Members), (IEnumerable<MemberInfo>) expression.Members);
    }

    protected virtual Expression VisitNewArrayExpression(NewArrayExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewArrayExpression>(nameof (expression), expression);
      ReadOnlyCollection<Expression> readOnlyCollection = this.VisitAndConvert<Expression>(expression.Expressions, nameof (VisitNewArrayExpression));
      if (readOnlyCollection == expression.Expressions)
        return (Expression) expression;
      Type elementType = expression.Type.GetElementType();
      return expression.NodeType == ExpressionType.NewArrayInit ? (Expression) Expression.NewArrayInit(elementType, (IEnumerable<Expression>) readOnlyCollection) : (Expression) Expression.NewArrayBounds(elementType, (IEnumerable<Expression>) readOnlyCollection);
    }

    protected virtual Expression VisitMemberInitExpression(MemberInitExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberInitExpression>(nameof (expression), expression);
      if (!(this.VisitExpression((Expression) expression.NewExpression) is NewExpression newExpression))
        throw new NotSupportedException("MemberInitExpressions only support non-null instances of type 'NewExpression' as their NewExpression member.");
      ReadOnlyCollection<MemberBinding> bindings = this.VisitMemberBindingList(expression.Bindings);
      return newExpression != expression.NewExpression || bindings != expression.Bindings ? (Expression) Expression.MemberInit(newExpression, (IEnumerable<MemberBinding>) bindings) : (Expression) expression;
    }

    protected virtual Expression VisitListInitExpression(ListInitExpression expression)
    {
      ArgumentUtility.CheckNotNull<ListInitExpression>(nameof (expression), expression);
      if (!(this.VisitExpression((Expression) expression.NewExpression) is NewExpression newExpression))
        throw new NotSupportedException("ListInitExpressions only support non-null instances of type 'NewExpression' as their NewExpression member.");
      ReadOnlyCollection<ElementInit> initializers = this.VisitElementInitList(expression.Initializers);
      return newExpression != expression.NewExpression || initializers != expression.Initializers ? (Expression) Expression.ListInit(newExpression, (IEnumerable<ElementInit>) initializers) : (Expression) expression;
    }

    protected virtual ElementInit VisitElementInit(ElementInit elementInit)
    {
      ArgumentUtility.CheckNotNull<ElementInit>(nameof (elementInit), elementInit);
      ReadOnlyCollection<Expression> arguments = this.VisitAndConvert<Expression>(elementInit.Arguments, nameof (VisitElementInit));
      return arguments != elementInit.Arguments ? Expression.ElementInit(elementInit.AddMethod, (IEnumerable<Expression>) arguments) : elementInit;
    }

    protected virtual MemberBinding VisitMemberBinding(MemberBinding memberBinding)
    {
      ArgumentUtility.CheckNotNull<MemberBinding>(nameof (memberBinding), memberBinding);
      switch (memberBinding.BindingType)
      {
        case MemberBindingType.Assignment:
          return this.VisitMemberAssignment((MemberAssignment) memberBinding);
        case MemberBindingType.MemberBinding:
          return this.VisitMemberMemberBinding((MemberMemberBinding) memberBinding);
        default:
          return this.VisitMemberListBinding((MemberListBinding) memberBinding);
      }
    }

    protected virtual MemberBinding VisitMemberAssignment(MemberAssignment memberAssigment)
    {
      ArgumentUtility.CheckNotNull<MemberAssignment>(nameof (memberAssigment), memberAssigment);
      Expression expression = this.VisitExpression(memberAssigment.Expression);
      return expression != memberAssigment.Expression ? (MemberBinding) Expression.Bind(memberAssigment.Member, expression) : (MemberBinding) memberAssigment;
    }

    protected virtual MemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      ArgumentUtility.CheckNotNull<MemberMemberBinding>(nameof (binding), binding);
      ReadOnlyCollection<MemberBinding> bindings = this.VisitMemberBindingList(binding.Bindings);
      return bindings != binding.Bindings ? (MemberBinding) Expression.MemberBind(binding.Member, (IEnumerable<MemberBinding>) bindings) : (MemberBinding) binding;
    }

    protected virtual MemberBinding VisitMemberListBinding(MemberListBinding listBinding)
    {
      ArgumentUtility.CheckNotNull<MemberListBinding>(nameof (listBinding), listBinding);
      ReadOnlyCollection<ElementInit> initializers = this.VisitElementInitList(listBinding.Initializers);
      return initializers != listBinding.Initializers ? (MemberBinding) Expression.ListBind(listBinding.Member, (IEnumerable<ElementInit>) initializers) : (MemberBinding) listBinding;
    }

    protected virtual ReadOnlyCollection<MemberBinding> VisitMemberBindingList(
      ReadOnlyCollection<MemberBinding> expressions)
    {
      return this.VisitList<MemberBinding>(expressions, new Func<MemberBinding, MemberBinding>(this.VisitMemberBinding));
    }

    protected virtual ReadOnlyCollection<ElementInit> VisitElementInitList(
      ReadOnlyCollection<ElementInit> expressions)
    {
      return this.VisitList<ElementInit>(expressions, new Func<ElementInit, ElementInit>(this.VisitElementInit));
    }

    protected virtual Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      return (Expression) expression;
    }

    protected virtual Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return (Expression) expression;
    }

    [Obsolete("This method has been split. Use VisitExtensionExpression or VisitUnknownNonExtensionExpression instead. 1.13.75")]
    protected internal virtual Expression VisitUnknownExpression(Expression expression)
    {
      throw new NotImplementedException();
    }
  }
}
