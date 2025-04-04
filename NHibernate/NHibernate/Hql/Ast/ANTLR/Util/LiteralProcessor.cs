// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.LiteralProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Globalization;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class LiteralProcessor
  {
    public const string ErrorCannotFetchWithIterate = "fetch may not be used with scroll() or iterate()";
    public const string ErrorNamedParameterDoesNotAppear = "Named parameter does not appear in Query: ";
    public const string ErrorCannotDetermineType = "Could not determine type of: ";
    public const string ErrorCannotFormatLiteral = "Could not format constant value to SQL literal: ";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (LiteralProcessor));
    private readonly HqlSqlWalker _walker;
    private static readonly LiteralProcessor.IDecimalFormatter[] _formatters = new LiteralProcessor.IDecimalFormatter[2]
    {
      (LiteralProcessor.IDecimalFormatter) new LiteralProcessor.ExactDecimalFormatter(),
      (LiteralProcessor.IDecimalFormatter) new LiteralProcessor.ApproximateDecimalFormatter()
    };
    public static readonly int EXACT = 0;
    public static readonly int APPROXIMATE = 1;
    public static readonly int DECIMAL_LITERAL_FORMAT = LiteralProcessor.EXACT;

    public LiteralProcessor(HqlSqlWalker walker) => this._walker = walker;

    public void LookupConstant(DotNode node)
    {
      string pathText = ASTUtil.GetPathText((IASTNode) node);
      IQueryable queryableUsingImports = this._walker.SessionFactoryHelper.FindQueryableUsingImports(pathText);
      if (queryableUsingImports != null)
      {
        string discriminatorSqlValue = queryableUsingImports.DiscriminatorSQLValue;
        node.DataType = queryableUsingImports.DiscriminatorType;
        if (InFragment.Null == discriminatorSqlValue || InFragment.NotNull == discriminatorSqlValue)
          throw new InvalidPathException("subclass test not allowed for null or not null discriminator: '" + pathText + "'");
        this.SetSQLValue(node, pathText, discriminatorSqlValue);
      }
      else
        this.SetConstantValue(node, pathText, ReflectHelper.GetConstantValue(pathText) ?? throw new InvalidPathException("Invalid path: '" + pathText + "'"));
    }

    public void ProcessNumericLiteral(SqlNode literal)
    {
      if (literal.Type == 95 || literal.Type == 99)
        literal.Text = this.DetermineIntegerRepresentation(literal.Text, literal.Type);
      else if (literal.Type == 98 || literal.Type == 96 || literal.Type == 97)
        literal.Text = LiteralProcessor.DetermineDecimalRepresentation(literal.Text, literal.Type);
      else
        LiteralProcessor.log.Warn((object) ("Unexpected literal token type [" + (object) literal.Type + "] passed for numeric processing"));
    }

    private bool IsAlias(string alias)
    {
      FromClause fromClause;
      for (fromClause = this._walker.CurrentFromClause; fromClause.IsSubQuery; fromClause = fromClause.ParentFromClause)
      {
        if (fromClause.ContainsClassAlias(alias))
          return true;
      }
      return fromClause.ContainsClassAlias(alias);
    }

    public void ProcessBoolean(IASTNode constant)
    {
      string str;
      this._walker.TokenReplacements.TryGetValue(constant.Text, out str);
      if (str != null)
      {
        constant.Text = str;
      }
      else
      {
        bool flag = "true" == constant.Text.ToLowerInvariant();
        NHibernate.Dialect.Dialect dialect = this._walker.SessionFactoryHelper.Factory.Dialect;
        constant.Text = dialect.ToBooleanValueString(flag);
      }
    }

    public void ProcessConstant(SqlNode constant, bool resolveIdent)
    {
      bool flag = constant.Type == 125 || constant.Type == 93;
      if (resolveIdent && flag && this.IsAlias(constant.Text))
      {
        ((FromReferenceNode) constant).Resolve(false, true);
      }
      else
      {
        IQueryable queryableUsingImports = this._walker.SessionFactoryHelper.FindQueryableUsingImports(constant.Text);
        if (flag && queryableUsingImports != null)
          constant.Text = queryableUsingImports.DiscriminatorSQLValue;
        else
          this.ProcessLiteral(constant);
      }
    }

    private static string DetermineDecimalRepresentation(string text, int type)
    {
      string s = text;
      switch (type)
      {
        case 96:
          if (!s.EndsWith("d"))
          {
            if (!s.EndsWith("D"))
              break;
          }
          s = s.Substring(0, s.Length - 1);
          break;
        case 97:
          if (!s.EndsWith("m"))
          {
            if (!s.EndsWith("M"))
              break;
          }
          s = s.Substring(0, s.Length - 1);
          break;
        case 98:
          if (!s.EndsWith("f"))
          {
            if (!s.EndsWith("F"))
              break;
          }
          s = s.Substring(0, s.Length - 1);
          break;
      }
      Decimal number;
      try
      {
        number = Decimal.Parse(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not parse literal [" + text + "] as System.Decimal.", ex);
      }
      return LiteralProcessor._formatters[LiteralProcessor.DECIMAL_LITERAL_FORMAT].Format(number);
    }

    private string DetermineIntegerRepresentation(string text, int type)
    {
      string s = text;
      bool flag = s.EndsWith("l") || s.EndsWith("L");
      if (flag)
        s = s.Substring(0, s.Length - 1);
      try
      {
        if (type == 95)
        {
          if (s.Length <= 10)
            goto label_5;
        }
        if (!flag)
          goto label_8;
label_5:
        try
        {
          return int.Parse(s).ToString();
        }
        catch (FormatException ex)
        {
          LiteralProcessor.log.Info((object) ("could not format incoming text [" + text + "] as a NUM_INT; assuming numeric overflow and attempting as NUM_LONG"));
        }
        catch (OverflowException ex)
        {
          LiteralProcessor.log.Info((object) ("could not format incoming text [" + text + "] as a NUM_INT; assuming numeric overflow and attempting as NUM_LONG"));
        }
label_8:
        return long.Parse(s).ToString();
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not parse literal [" + text + "] as integer", ex);
      }
    }

    private void ProcessLiteral(SqlNode constant)
    {
      string str;
      if (!this._walker.TokenReplacements.TryGetValue(constant.Text, out str) || str == null)
        return;
      if (LiteralProcessor.log.IsDebugEnabled)
        LiteralProcessor.log.Debug((object) ("processConstant() : Replacing '" + constant.Text + "' with '" + str + "'"));
      constant.Text = str;
    }

    private void SetSQLValue(DotNode node, string text, string value)
    {
      if (LiteralProcessor.log.IsDebugEnabled)
        LiteralProcessor.log.Debug((object) ("setSQLValue() " + text + " -> " + value));
      node.ClearChildren();
      node.Type = 143;
      node.Text = value;
      node.SetResolvedConstant(text);
    }

    private void SetConstantValue(DotNode node, string text, object value)
    {
      if (LiteralProcessor.log.IsDebugEnabled)
        LiteralProcessor.log.Debug((object) ("setConstantValue() " + text + " -> " + value + " " + value.GetType().Name));
      node.ClearChildren();
      switch (value)
      {
        case string _:
          node.Type = 124;
          break;
        case char _:
          node.Type = 124;
          break;
        case byte _:
          node.Type = 95;
          break;
        case short _:
          node.Type = 95;
          break;
        case int _:
          node.Type = 95;
          break;
        case long _:
          node.Type = 99;
          break;
        case double _:
          node.Type = 96;
          break;
        case Decimal _:
          node.Type = 97;
          break;
        case float _:
          node.Type = 98;
          break;
        default:
          node.Type = 94;
          break;
      }
      IType type;
      try
      {
        type = TypeFactory.HeuristicType(value.GetType().Name);
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
      if (type == null)
        throw new QueryException("Could not determine type of: " + node.Text);
      try
      {
        ILiteralType literalType = (ILiteralType) type;
        NHibernate.Dialect.Dialect dialect = this._walker.SessionFactoryHelper.Factory.Dialect;
        node.Text = literalType.ObjectToSQLString(value, dialect);
      }
      catch (Exception ex)
      {
        throw new QueryException("Could not format constant value to SQL literal: " + node.Text, ex);
      }
      node.DataType = type;
      node.SetResolvedConstant(text);
    }

    private interface IDecimalFormatter
    {
      string Format(Decimal number);
    }

    private class ExactDecimalFormatter : LiteralProcessor.IDecimalFormatter
    {
      public string Format(Decimal number)
      {
        return number.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo);
      }
    }

    private class ApproximateDecimalFormatter : LiteralProcessor.IDecimalFormatter
    {
      private static readonly string FORMAT_STRING = "#0.0E0";

      public string Format(Decimal number)
      {
        try
        {
          return number.ToString(LiteralProcessor.ApproximateDecimalFormatter.FORMAT_STRING);
        }
        catch (Exception ex)
        {
          throw new HibernateException("Unable to format decimal literal in approximate format [" + number.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + "]", ex);
        }
      }
    }
  }
}
