// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.SyntacticAnalyzer
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class SyntacticAnalyzer : ISyntacticAnalyzer
  {
    public void Analyze(IEnumerable<Token> tokens)
    {
      SyntacticAnalyzer.AnalyzingContext context = new SyntacticAnalyzer.AnalyzingContext();
      foreach (Token token in tokens)
      {
        if (token.TokenType == TokenType.Unrecognized)
          throw new UnrecognizedTokenException(token);
        this.EnsureParenthesesAreWellFormed(token, context);
        this.EnsureStringsAreWellFormed(token, context);
      }
      SyntacticAnalyzer.Validate(context);
    }

    private static void Validate(SyntacticAnalyzer.AnalyzingContext context)
    {
      if (context.NumberOfOpenedParentheses != context.NumberOfClosedParentheses)
        throw new FormatException("Number of opened and closed parentheses does not match");
      if (context.OpenedStrings != context.ClosedStrings)
        throw new FormatException("Unterminated string");
    }

    private void EnsureParenthesesAreWellFormed(
      Token token,
      SyntacticAnalyzer.AnalyzingContext context)
    {
      if (token.TokenType == TokenType.OpeningParenthesis)
      {
        ++context.NumberOfOpenedParentheses;
      }
      else
      {
        if (token.TokenType != TokenType.ClosingParenthesis)
          return;
        ++context.NumberOfClosedParentheses;
      }
    }

    private void EnsureStringsAreWellFormed(Token token, SyntacticAnalyzer.AnalyzingContext context)
    {
      if (!context.IsInString && token.TokenType == TokenType.String)
      {
        context.IsInString = true;
        ++context.OpenedStrings;
      }
      else
      {
        if (!context.IsInString || token.TokenType != TokenType.String)
          return;
        context.IsInString = false;
        ++context.ClosedStrings;
      }
    }

    private class AnalyzingContext
    {
      public int NumberOfOpenedParentheses { get; set; }

      public int NumberOfClosedParentheses { get; set; }

      public int OpenedStrings { get; set; }

      public int ClosedStrings { get; set; }

      public bool IsInString { get; set; }
    }
  }
}
