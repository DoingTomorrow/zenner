// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.FingerprintingExpressionVisitor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal sealed class FingerprintingExpressionVisitor : ExpressionVisitor
  {
    private readonly List<object> _seenConstants = new List<object>();
    private readonly List<ParameterExpression> _seenParameters = new List<ParameterExpression>();
    private readonly ExpressionFingerprintChain _currentChain = new ExpressionFingerprintChain();
    private bool _gaveUp;

    private FingerprintingExpressionVisitor()
    {
    }

    private T GiveUp<T>(T node)
    {
      this._gaveUp = true;
      return node;
    }

    public static ExpressionFingerprintChain GetFingerprintChain(
      Expression expr,
      out List<object> capturedConstants)
    {
      FingerprintingExpressionVisitor expressionVisitor = new FingerprintingExpressionVisitor();
      expressionVisitor.Visit(expr);
      if (expressionVisitor._gaveUp)
      {
        capturedConstants = (List<object>) null;
        return (ExpressionFingerprintChain) null;
      }
      capturedConstants = expressionVisitor._seenConstants;
      return expressionVisitor._currentChain;
    }

    public override Expression Visit(Expression node)
    {
      if (node != null)
        return base.Visit(node);
      this._currentChain.Elements.Add((ExpressionFingerprint) null);
      return (Expression) null;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new BinaryExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitBinary(node);
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
      return (Expression) this.GiveUp<BlockExpression>(node);
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node) => this.GiveUp<CatchBlock>(node);

    protected override Expression VisitConditional(ConditionalExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new ConditionalExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitConditional(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._seenConstants.Add(node.Value);
      this._currentChain.Elements.Add((ExpressionFingerprint) new ConstantExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitConstant(node);
    }

    protected override Expression VisitDebugInfo(DebugInfoExpression node)
    {
      return (Expression) this.GiveUp<DebugInfoExpression>(node);
    }

    protected override Expression VisitDefault(DefaultExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new DefaultExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitDefault(node);
    }

    protected override Expression VisitDynamic(DynamicExpression node)
    {
      return (Expression) this.GiveUp<DynamicExpression>(node);
    }

    protected override ElementInit VisitElementInit(ElementInit node)
    {
      return this.GiveUp<ElementInit>(node);
    }

    protected override Expression VisitExtension(Expression node) => this.GiveUp<Expression>(node);

    protected override Expression VisitGoto(GotoExpression node)
    {
      return (Expression) this.GiveUp<GotoExpression>(node);
    }

    protected override Expression VisitIndex(IndexExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new IndexExpressionFingerprint(node.NodeType, node.Type, node.Indexer));
      return base.VisitIndex(node);
    }

    protected override Expression VisitInvocation(InvocationExpression node)
    {
      return (Expression) this.GiveUp<InvocationExpression>(node);
    }

    protected override Expression VisitLabel(LabelExpression node)
    {
      return (Expression) this.GiveUp<LabelExpression>(node);
    }

    protected override LabelTarget VisitLabelTarget(LabelTarget node)
    {
      return this.GiveUp<LabelTarget>(node);
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new LambdaExpressionFingerprint(node.NodeType, node.Type));
      return base.VisitLambda<T>(node);
    }

    protected override Expression VisitListInit(ListInitExpression node)
    {
      return (Expression) this.GiveUp<ListInitExpression>(node);
    }

    protected override Expression VisitLoop(LoopExpression node)
    {
      return (Expression) this.GiveUp<LoopExpression>(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new MemberExpressionFingerprint(node.NodeType, node.Type, node.Member));
      return base.VisitMember(node);
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
      return this.GiveUp<MemberAssignment>(node);
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding node)
    {
      return this.GiveUp<MemberBinding>(node);
    }

    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
      return (Expression) this.GiveUp<MemberInitExpression>(node);
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
      return this.GiveUp<MemberListBinding>(node);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
    {
      return this.GiveUp<MemberMemberBinding>(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new MethodCallExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
      return (Expression) this.GiveUp<NewExpression>(node);
    }

    protected override Expression VisitNewArray(NewArrayExpression node)
    {
      return (Expression) this.GiveUp<NewArrayExpression>(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      int parameterIndex = this._seenParameters.IndexOf(node);
      if (parameterIndex < 0)
      {
        parameterIndex = this._seenParameters.Count;
        this._seenParameters.Add(node);
      }
      this._currentChain.Elements.Add((ExpressionFingerprint) new ParameterExpressionFingerprint(node.NodeType, node.Type, parameterIndex));
      return base.VisitParameter(node);
    }

    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
    {
      return (Expression) this.GiveUp<RuntimeVariablesExpression>(node);
    }

    protected override Expression VisitSwitch(SwitchExpression node)
    {
      return (Expression) this.GiveUp<SwitchExpression>(node);
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase node) => this.GiveUp<SwitchCase>(node);

    protected override Expression VisitTry(TryExpression node)
    {
      return (Expression) this.GiveUp<TryExpression>(node);
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new TypeBinaryExpressionFingerprint(node.NodeType, node.Type, node.TypeOperand));
      return base.VisitTypeBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
      if (this._gaveUp)
        return (Expression) node;
      this._currentChain.Elements.Add((ExpressionFingerprint) new UnaryExpressionFingerprint(node.NodeType, node.Type, node.Method));
      return base.VisitUnary(node);
    }
  }
}
