// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlToken
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class HqlToken : CommonToken
  {
    private int _previousTokenType;

    public HqlToken(ICharStream input, int type, int channel, int start, int stop)
      : base(input, type, channel, start, stop)
    {
      this.charPositionInLine = input.CharPositionInLine - (stop - start + 1);
    }

    public HqlToken(IToken other)
      : base(other)
    {
      if (!(other is HqlToken hqlToken))
        return;
      this._previousTokenType = hqlToken._previousTokenType;
    }

    public bool PossibleId => HqlParser.possibleIds[this.Type];

    public override int Type
    {
      get => base.Type;
      set
      {
        this._previousTokenType = this.Type;
        base.Type = value;
      }
    }

    private int PreviousType => this._previousTokenType;

    public override string ToString()
    {
      return "[\"" + this.Text + "\",<" + (object) this.Type + "> previously: <" + (object) this.PreviousType + ">,line=" + (object) this.Line + ",col=" + (object) this.CharPositionInLine + ",possibleID=" + (object) this.PossibleId + "]";
    }
  }
}
