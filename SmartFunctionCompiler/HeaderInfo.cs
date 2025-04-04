// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.HeaderInfo
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;

#nullable disable
namespace SmartFunctionCompiler
{
  internal class HeaderInfo
  {
    internal const string FunctionNameToken = "Function name:";
    internal const string RequiredFunctionsToken = "Required functions:";
    internal const string MemberOfGroupsToken = "Member of groups:";
    internal const string FunctionVersionToken = "Function version:";
    internal const string MinimalInterpreterVersionToken = "Interpreter version:";
    internal const string EventToken = "Event:";

    internal static string GetTokenValue(
      string token,
      string[] lines,
      ref int lineNumber,
      bool notRequired = false,
      int maxCharacters = 20,
      bool removeSpaces = false)
    {
      while (lines[lineNumber].Length == 0)
        ++lineNumber;
      if (!lines[lineNumber].StartsWith(token))
      {
        if (notRequired)
          return (string) null;
        throw new Exception("Token not found: " + token);
      }
      string tokenValue = lines[lineNumber++].Substring(token.Length).Trim();
      if (removeSpaces)
        tokenValue = tokenValue.Replace(" ", "");
      else if (tokenValue.Contains(" "))
        throw new Exception("Spaces are not allowed for " + token);
      if (tokenValue.Length > maxCharacters)
        throw new Exception(token + " length exceeds " + maxCharacters.ToString() + " caracters.");
      return tokenValue;
    }
  }
}
