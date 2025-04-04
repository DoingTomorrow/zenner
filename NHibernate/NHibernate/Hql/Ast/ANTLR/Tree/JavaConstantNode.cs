// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.JavaConstantNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class JavaConstantNode(IToken token) : 
    SqlNode(token),
    IExpectedTypeAwareNode,
    ISessionFactoryAwareNode
  {
    private ISessionFactoryImplementor _factory;
    private object _constantValue;
    private IType _heuristicType;
    private IType _expectedType;
    private bool _processedText;

    public IType ExpectedType
    {
      get => this._expectedType;
      set => this._expectedType = value;
    }

    public ISessionFactoryImplementor SessionFactory
    {
      set => this._factory = value;
    }

    public override SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      this.ProcessText();
      return new SqlString(this.ResolveToLiteralString(this._expectedType ?? this._heuristicType));
    }

    private string ResolveToLiteralString(IType type)
    {
      try
      {
        return ((ILiteralType) type).ObjectToSQLString(this._constantValue, this._factory.Dialect);
      }
      catch (Exception ex)
      {
        throw new QueryException("Could not format constant value to SQL literal: " + this.Text, ex);
      }
    }

    private void ProcessText()
    {
      if (this._processedText)
        return;
      this._constantValue = this._factory != null ? ReflectHelper.GetConstantValue(this.Text, this._factory) : throw new InvalidOperationException();
      this._heuristicType = TypeFactory.HeuristicType(this._constantValue.GetType().AssemblyQualifiedName);
      this._processedText = true;
    }
  }
}
