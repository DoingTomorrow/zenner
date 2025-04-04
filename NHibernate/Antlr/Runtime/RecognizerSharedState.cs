// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.RecognizerSharedState
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace Antlr.Runtime
{
  internal class RecognizerSharedState
  {
    public BitSet[] following = new BitSet[100];
    public int followingStackPointer = -1;
    public bool errorRecovery;
    public int lastErrorIndex = -1;
    public bool failed;
    public int syntaxErrors;
    public int backtracking;
    public IDictionary[] ruleMemo;
    public IToken token;
    public int tokenStartCharIndex = -1;
    public int tokenStartLine;
    public int tokenStartCharPositionInLine;
    public int channel;
    public int type;
    public string text;
  }
}
