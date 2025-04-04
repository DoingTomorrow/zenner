// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionParser
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace NLog.Conditions
{
  public class ConditionParser
  {
    private readonly ConditionTokenizer _tokenizer;
    private readonly ConfigurationItemFactory _configurationItemFactory;

    private ConditionParser(
      SimpleStringReader stringReader,
      ConfigurationItemFactory configurationItemFactory)
    {
      this._configurationItemFactory = configurationItemFactory;
      this._tokenizer = new ConditionTokenizer(stringReader);
    }

    public static ConditionExpression ParseExpression(string expressionText)
    {
      return ConditionParser.ParseExpression(expressionText, ConfigurationItemFactory.Default);
    }

    public static ConditionExpression ParseExpression(
      string expressionText,
      ConfigurationItemFactory configurationItemFactories)
    {
      if (expressionText == null)
        return (ConditionExpression) null;
      ConditionParser conditionParser = new ConditionParser(new SimpleStringReader(expressionText), configurationItemFactories);
      ConditionExpression expression = conditionParser.ParseExpression();
      if (conditionParser._tokenizer.IsEOF())
        return expression;
      throw new ConditionParseException(string.Format("Unexpected token: {0}", (object) conditionParser._tokenizer.TokenValue));
    }

    internal static ConditionExpression ParseExpression(
      SimpleStringReader stringReader,
      ConfigurationItemFactory configurationItemFactories)
    {
      return new ConditionParser(stringReader, configurationItemFactories).ParseExpression();
    }

    private ConditionMethodExpression ParsePredicate(string functionName)
    {
      List<ConditionExpression> methodParameters = new List<ConditionExpression>();
      while (!this._tokenizer.IsEOF() && this._tokenizer.TokenType != ConditionTokenType.RightParen)
      {
        methodParameters.Add(this.ParseExpression());
        if (this._tokenizer.TokenType == ConditionTokenType.Comma)
          this._tokenizer.GetNextToken();
        else
          break;
      }
      this._tokenizer.Expect(ConditionTokenType.RightParen);
      try
      {
        MethodInfo instance = this._configurationItemFactory.ConditionMethods.CreateInstance(functionName);
        return new ConditionMethodExpression(functionName, instance, (IEnumerable<ConditionExpression>) methodParameters);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Cannot resolve function '{0}'", (object) functionName);
        if (!ex.MustBeRethrownImmediately())
          throw new ConditionParseException(string.Format("Cannot resolve function '{0}'", (object) functionName), ex);
        throw;
      }
    }

    private ConditionExpression ParseLiteralExpression()
    {
      if (this._tokenizer.IsToken(ConditionTokenType.LeftParen))
      {
        this._tokenizer.GetNextToken();
        ConditionExpression expression = this.ParseExpression();
        this._tokenizer.Expect(ConditionTokenType.RightParen);
        return expression;
      }
      if (this._tokenizer.IsToken(ConditionTokenType.Minus))
      {
        this._tokenizer.GetNextToken();
        string s = this._tokenizer.IsNumber() ? this._tokenizer.TokenValue : throw new ConditionParseException(string.Format("Number expected, got {0}", (object) this._tokenizer.TokenType));
        this._tokenizer.GetNextToken();
        return s.IndexOf('.') >= 0 ? (ConditionExpression) new ConditionLiteralExpression((object) -double.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture)) : (ConditionExpression) new ConditionLiteralExpression((object) -int.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture));
      }
      if (this._tokenizer.IsNumber())
      {
        string tokenValue = this._tokenizer.TokenValue;
        this._tokenizer.GetNextToken();
        return tokenValue.IndexOf('.') >= 0 ? (ConditionExpression) new ConditionLiteralExpression((object) double.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture)) : (ConditionExpression) new ConditionLiteralExpression((object) int.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture));
      }
      if (this._tokenizer.TokenType == ConditionTokenType.String)
      {
        ConditionLayoutExpression literalExpression = new ConditionLayoutExpression(Layout.FromString(this._tokenizer.StringTokenValue, this._configurationItemFactory));
        this._tokenizer.GetNextToken();
        return (ConditionExpression) literalExpression;
      }
      string str = this._tokenizer.TokenType == ConditionTokenType.Keyword ? this._tokenizer.EatKeyword() : throw new ConditionParseException("Unexpected token: " + this._tokenizer.TokenValue);
      if (string.Compare(str, "level", StringComparison.OrdinalIgnoreCase) == 0)
        return (ConditionExpression) new ConditionLevelExpression();
      if (string.Compare(str, "logger", StringComparison.OrdinalIgnoreCase) == 0)
        return (ConditionExpression) new ConditionLoggerNameExpression();
      if (string.Compare(str, "message", StringComparison.OrdinalIgnoreCase) == 0)
        return (ConditionExpression) new ConditionMessageExpression();
      if (string.Compare(str, "loglevel", StringComparison.OrdinalIgnoreCase) == 0)
      {
        this._tokenizer.Expect(ConditionTokenType.Dot);
        return (ConditionExpression) new ConditionLiteralExpression((object) NLog.LogLevel.FromString(this._tokenizer.EatKeyword()));
      }
      if (string.Compare(str, "true", StringComparison.OrdinalIgnoreCase) == 0)
        return (ConditionExpression) new ConditionLiteralExpression((object) true);
      if (string.Compare(str, "false", StringComparison.OrdinalIgnoreCase) == 0)
        return (ConditionExpression) new ConditionLiteralExpression((object) false);
      if (string.Compare(str, "null", StringComparison.OrdinalIgnoreCase) == 0)
        return (ConditionExpression) new ConditionLiteralExpression((object) null);
      if (this._tokenizer.TokenType == ConditionTokenType.LeftParen)
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) this.ParsePredicate(str);
      }
    }

    private ConditionExpression ParseBooleanRelation()
    {
      ConditionExpression literalExpression = this.ParseLiteralExpression();
      if (this._tokenizer.IsToken(ConditionTokenType.EqualTo))
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.Equal);
      }
      if (this._tokenizer.IsToken(ConditionTokenType.NotEqual))
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.NotEqual);
      }
      if (this._tokenizer.IsToken(ConditionTokenType.LessThan))
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.Less);
      }
      if (this._tokenizer.IsToken(ConditionTokenType.GreaterThan))
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.Greater);
      }
      if (this._tokenizer.IsToken(ConditionTokenType.LessThanOrEqualTo))
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.LessOrEqual);
      }
      if (!this._tokenizer.IsToken(ConditionTokenType.GreaterThanOrEqualTo))
        return literalExpression;
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.GreaterOrEqual);
    }

    private ConditionExpression ParseBooleanPredicate()
    {
      if (!this._tokenizer.IsKeyword("not") && !this._tokenizer.IsToken(ConditionTokenType.Not))
        return this.ParseBooleanRelation();
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionNotExpression(this.ParseBooleanPredicate());
    }

    private ConditionExpression ParseBooleanAnd()
    {
      ConditionExpression left = this.ParseBooleanPredicate();
      while (this._tokenizer.IsKeyword("and") || this._tokenizer.IsToken(ConditionTokenType.And))
      {
        this._tokenizer.GetNextToken();
        left = (ConditionExpression) new ConditionAndExpression(left, this.ParseBooleanPredicate());
      }
      return left;
    }

    private ConditionExpression ParseBooleanOr()
    {
      ConditionExpression left = this.ParseBooleanAnd();
      while (this._tokenizer.IsKeyword("or") || this._tokenizer.IsToken(ConditionTokenType.Or))
      {
        this._tokenizer.GetNextToken();
        left = (ConditionExpression) new ConditionOrExpression(left, this.ParseBooleanAnd());
      }
      return left;
    }

    private ConditionExpression ParseBooleanExpression() => this.ParseBooleanOr();

    private ConditionExpression ParseExpression() => this.ParseBooleanExpression();
  }
}
