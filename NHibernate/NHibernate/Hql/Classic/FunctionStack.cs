// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.FunctionStack
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class FunctionStack
  {
    private readonly Stack<FunctionStack.FunctionHolder> stack = new Stack<FunctionStack.FunctionHolder>(5);
    private readonly IMapping mapping;

    public FunctionStack(IMapping mapping)
    {
      this.mapping = mapping != null ? mapping : throw new ArgumentNullException(nameof (mapping));
    }

    public void Push(ISQLFunction sqlFunction)
    {
      if (sqlFunction == null)
        throw new ArgumentNullException(nameof (sqlFunction));
      this.stack.Push(new FunctionStack.FunctionHolder(sqlFunction));
    }

    private FunctionStack.FunctionHolder Peek() => this.stack.Peek();

    public void Pop()
    {
      IType type;
      try
      {
        FunctionStack.FunctionHolder functionHolder = this.Peek();
        type = functionHolder.SqlFunction.ReturnType(functionHolder.FirstValidColumnType, this.mapping);
        this.stack.Pop();
      }
      catch (InvalidOperationException ex)
      {
        throw new QueryException("Parsing HQL: Pop on empty functions stack.", (Exception) ex);
      }
      if (this.stack.Count <= 0)
        return;
      this.Peek().FirstValidColumnType = type;
    }

    public bool HasFunctions => this.stack.Count > 0;

    public int NestedFunctionCount => this.stack.Count;

    public PathExpressionParser PathExpressionParser => this.Peek().PathExpressionParser;

    public ISQLFunction SqlFunction => this.Peek().SqlFunction;

    public IFunctionGrammar FunctionGrammar => this.Peek().FunctionGrammar;

    public IType GetReturnType()
    {
      FunctionStack.FunctionHolder functionHolder = this.Peek();
      IType type;
      try
      {
        type = functionHolder.SqlFunction.ReturnType(functionHolder.FirstValidColumnType, this.mapping);
      }
      catch (ArgumentNullException ex)
      {
        type = (IType) null;
      }
      return type != null ? type : throw new QueryException(string.Format("Can't extract the type of one parameter of a HQL function: expression->{{{0}}}; check aliases.", (object) functionHolder.PathExpressionParser.ProcessedPath));
    }

    private class FunctionHolder
    {
      private readonly PathExpressionParser pathExpressionParser = new PathExpressionParser();
      private readonly ISQLFunction sqlFunction;
      private readonly IFunctionGrammar functionGrammar;
      private IType firstValidColumnReturnType;

      public PathExpressionParser PathExpressionParser
      {
        get
        {
          this.FirstValidColumnType = this.pathExpressionParser.WhereColumnType;
          return this.pathExpressionParser;
        }
      }

      public ISQLFunction SqlFunction => this.sqlFunction;

      public IFunctionGrammar FunctionGrammar => this.functionGrammar;

      public FunctionHolder(ISQLFunction sqlFunction)
      {
        this.pathExpressionParser.UseThetaStyleJoin = true;
        this.sqlFunction = sqlFunction;
        this.functionGrammar = sqlFunction as IFunctionGrammar;
        if (this.functionGrammar != null)
          return;
        this.functionGrammar = (IFunctionGrammar) new CommonGrammar();
      }

      public IType FirstValidColumnType
      {
        get => this.firstValidColumnReturnType;
        set
        {
          if (this.firstValidColumnReturnType != null)
            return;
          this.firstValidColumnReturnType = value;
        }
      }
    }
  }
}
