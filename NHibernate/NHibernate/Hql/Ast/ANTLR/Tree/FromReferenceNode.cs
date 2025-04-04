// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.FromReferenceNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public abstract class FromReferenceNode(IToken token) : 
    AbstractSelectExpression(token),
    IResolvableNode,
    IDisplayableNode,
    IPathNode
  {
    public const int RootLevel = 0;
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (FromReferenceNode));
    private FromElement _fromElement;
    private bool _resolved;

    public override bool IsReturnableEntity => !this.IsScalar && this._fromElement.IsEntity;

    public override FromElement FromElement
    {
      get => this._fromElement;
      set => this._fromElement = value;
    }

    public bool IsResolved
    {
      get => this._resolved;
      set
      {
        this._resolved = true;
        if (!FromReferenceNode.Log.IsDebugEnabled)
          return;
        FromReferenceNode.Log.Debug((object) ("Resolved :  " + this.Path + " -> " + this.Text));
      }
    }

    public string GetDisplayText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("{").Append(this._fromElement == null ? "no fromElement" : this._fromElement.GetDisplayText());
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public abstract void Resolve(
      bool generateJoin,
      bool implicitJoin,
      string classAlias,
      IASTNode parent);

    public void Resolve(bool generateJoin, bool implicitJoin, string classAlias)
    {
      this.Resolve(generateJoin, implicitJoin, classAlias, (IASTNode) null);
    }

    public void Resolve(bool generateJoin, bool implicitJoin)
    {
      this.Resolve(generateJoin, implicitJoin, (string) null);
    }

    public virtual void ResolveInFunctionCall(bool generateJoin, bool implicitJoin)
    {
      this.Resolve(generateJoin, implicitJoin, (string) null);
    }

    public abstract void ResolveIndex(IASTNode parent);

    public virtual string Path => this.OriginalText;

    public void RecursiveResolve(
      int level,
      bool impliedAtRoot,
      string classAlias,
      IASTNode parent)
    {
      ((FromReferenceNode) this.GetFirstChild())?.RecursiveResolve(level + 1, impliedAtRoot, (string) null, (IASTNode) this);
      this.ResolveFirstChild();
      bool implicitJoin = true;
      if (level == 0 && !impliedAtRoot)
        implicitJoin = false;
      this.Resolve(true, implicitJoin, classAlias, parent);
    }

    public virtual FromElement GetImpliedJoin() => (FromElement) null;

    public virtual void PrepareForDot(string propertyName)
    {
    }

    public virtual void ResolveFirstChild()
    {
    }
  }
}
