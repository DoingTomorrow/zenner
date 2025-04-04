// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.RecognitionException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class RecognitionException : Exception
  {
    [NonSerialized]
    protected IIntStream input;
    protected int index;
    protected IToken token;
    protected object node;
    protected int c;
    protected int line;
    protected int charPositionInLine;
    public bool approximateLineInfo;

    public RecognitionException()
      : this((string) null, (Exception) null, (IIntStream) null)
    {
    }

    public RecognitionException(string message)
      : this(message, (Exception) null, (IIntStream) null)
    {
    }

    public RecognitionException(string message, Exception inner)
      : this(message, inner, (IIntStream) null)
    {
    }

    public RecognitionException(IIntStream input)
      : this((string) null, (Exception) null, input)
    {
    }

    public RecognitionException(string message, IIntStream input)
      : this(message, (Exception) null, input)
    {
    }

    public RecognitionException(string message, Exception inner, IIntStream input)
      : base(message, inner)
    {
      this.input = input;
      this.index = input.Index();
      if (input is ITokenStream)
      {
        this.token = ((ITokenStream) input).LT(1);
        this.line = this.token.Line;
        this.charPositionInLine = this.token.CharPositionInLine;
      }
      if (input is ITreeNodeStream)
        this.ExtractInformationFromTreeNodeStream(input);
      else if (input is ICharStream)
      {
        this.c = input.LA(1);
        this.line = ((ICharStream) input).Line;
        this.charPositionInLine = ((ICharStream) input).CharPositionInLine;
      }
      else
        this.c = input.LA(1);
    }

    public IIntStream Input
    {
      get => this.input;
      set => this.input = value;
    }

    public int Index
    {
      get => this.index;
      set => this.index = value;
    }

    public IToken Token
    {
      get => this.token;
      set => this.token = value;
    }

    public object Node
    {
      get => this.node;
      set => this.node = value;
    }

    public int Char
    {
      get => this.c;
      set => this.c = value;
    }

    public int CharPositionInLine
    {
      get => this.charPositionInLine;
      set => this.charPositionInLine = value;
    }

    public int Line
    {
      get => this.line;
      set => this.line = value;
    }

    public virtual int UnexpectedType
    {
      get
      {
        if (this.input is ITokenStream)
          return this.token.Type;
        return this.input is ITreeNodeStream ? ((ITreeNodeStream) this.input).TreeAdaptor.GetNodeType(this.node) : this.c;
      }
    }

    protected void ExtractInformationFromTreeNodeStream(IIntStream input)
    {
      ITreeNodeStream treeNodeStream = (ITreeNodeStream) input;
      this.node = treeNodeStream.LT(1);
      ITreeAdaptor treeAdaptor = treeNodeStream.TreeAdaptor;
      IToken token1 = treeAdaptor.GetToken(this.node);
      if (token1 != null)
      {
        this.token = token1;
        if (token1.Line <= 0)
        {
          int k = -1;
          for (object treeNode = treeNodeStream.LT(k); treeNode != null; treeNode = treeNodeStream.LT(k))
          {
            IToken token2 = treeAdaptor.GetToken(treeNode);
            if (token2 != null && token2.Line > 0)
            {
              this.line = token2.Line;
              this.charPositionInLine = token2.CharPositionInLine;
              this.approximateLineInfo = true;
              break;
            }
            --k;
          }
        }
        else
        {
          this.line = token1.Line;
          this.charPositionInLine = token1.CharPositionInLine;
        }
      }
      else if (this.node is ITree)
      {
        this.line = ((ITree) this.node).Line;
        this.charPositionInLine = ((ITree) this.node).CharPositionInLine;
        if (!(this.node is CommonTree))
          return;
        this.token = ((CommonTree) this.node).Token;
      }
      else
        this.token = (IToken) new CommonToken(treeAdaptor.GetNodeType(this.node), treeAdaptor.GetNodeText(this.node));
    }
  }
}
