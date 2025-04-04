// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ASTErrorNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ASTErrorNode : ASTNode
  {
    public ASTErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e)
      : base(start)
    {
      this.Input = input;
      this.Stop = stop;
      this.RecognitionException = e;
    }

    public ITokenStream Input { get; private set; }

    public IToken Stop { get; private set; }

    public RecognitionException RecognitionException { get; private set; }
  }
}
