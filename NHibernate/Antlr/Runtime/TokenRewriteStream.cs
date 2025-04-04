// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.TokenRewriteStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  internal class TokenRewriteStream : CommonTokenStream
  {
    public const string DEFAULT_PROGRAM_NAME = "default";
    public const int PROGRAM_INIT_SIZE = 100;
    public const int MIN_TOKEN_INDEX = 0;
    protected IDictionary programs;
    protected IDictionary lastRewriteTokenIndexes;

    public TokenRewriteStream() => this.Init();

    public TokenRewriteStream(ITokenSource tokenSource)
      : base(tokenSource)
    {
      this.Init();
    }

    public TokenRewriteStream(ITokenSource tokenSource, int channel)
      : base(tokenSource, channel)
    {
      this.Init();
    }

    protected internal virtual void Init()
    {
      this.programs = (IDictionary) new Hashtable();
      this.programs[(object) "default"] = (object) new ArrayList(100);
      this.lastRewriteTokenIndexes = (IDictionary) new Hashtable();
    }

    public virtual void Rollback(int instructionIndex)
    {
      this.Rollback("default", instructionIndex);
    }

    public virtual void Rollback(string programName, int instructionIndex)
    {
      IList program = (IList) this.programs[(object) programName];
      if (program == null)
        return;
      this.programs[(object) programName] = (object) ((ArrayList) program).GetRange(0, instructionIndex);
    }

    public virtual void DeleteProgram() => this.DeleteProgram("default");

    public virtual void DeleteProgram(string programName) => this.Rollback(programName, 0);

    public virtual void InsertAfter(IToken t, object text) => this.InsertAfter("default", t, text);

    public virtual void InsertAfter(int index, object text)
    {
      this.InsertAfter("default", index, text);
    }

    public virtual void InsertAfter(string programName, IToken t, object text)
    {
      this.InsertAfter(programName, t.TokenIndex, text);
    }

    public virtual void InsertAfter(string programName, int index, object text)
    {
      this.InsertBefore(programName, index + 1, text);
    }

    public virtual void InsertBefore(IToken t, object text)
    {
      this.InsertBefore("default", t, text);
    }

    public virtual void InsertBefore(int index, object text)
    {
      this.InsertBefore("default", index, text);
    }

    public virtual void InsertBefore(string programName, IToken t, object text)
    {
      this.InsertBefore(programName, t.TokenIndex, text);
    }

    public virtual void InsertBefore(string programName, int index, object text)
    {
      TokenRewriteStream.RewriteOperation rewriteOperation = (TokenRewriteStream.RewriteOperation) new TokenRewriteStream.InsertBeforeOp(index, text, this);
      this.GetProgram(programName).Add((object) rewriteOperation);
    }

    public virtual void Replace(int index, object text)
    {
      this.Replace("default", index, index, text);
    }

    public virtual void Replace(int from, int to, object text)
    {
      this.Replace("default", from, to, text);
    }

    public virtual void Replace(IToken indexT, object text)
    {
      this.Replace("default", indexT, indexT, text);
    }

    public virtual void Replace(IToken from, IToken to, object text)
    {
      this.Replace("default", from, to, text);
    }

    public virtual void Replace(string programName, int from, int to, object text)
    {
      if (from > to || from < 0 || to < 0 || to >= this.tokens.Count)
        throw new ArgumentOutOfRangeException("replace: range invalid: " + (object) from + ".." + (object) to + "(size=" + (object) this.tokens.Count + ")");
      TokenRewriteStream.RewriteOperation rewriteOperation = (TokenRewriteStream.RewriteOperation) new TokenRewriteStream.ReplaceOp(from, to, text, this);
      IList program = this.GetProgram(programName);
      rewriteOperation.instructionIndex = program.Count;
      program.Add((object) rewriteOperation);
    }

    public virtual void Replace(string programName, IToken from, IToken to, object text)
    {
      this.Replace(programName, from.TokenIndex, to.TokenIndex, text);
    }

    public virtual void Delete(int index) => this.Delete("default", index, index);

    public virtual void Delete(int from, int to) => this.Delete("default", from, to);

    public virtual void Delete(IToken indexT) => this.Delete("default", indexT, indexT);

    public virtual void Delete(IToken from, IToken to) => this.Delete("default", from, to);

    public virtual void Delete(string programName, int from, int to)
    {
      this.Replace(programName, from, to, (object) null);
    }

    public virtual void Delete(string programName, IToken from, IToken to)
    {
      this.Replace(programName, from, to, (object) null);
    }

    public virtual int GetLastRewriteTokenIndex() => this.GetLastRewriteTokenIndex("default");

    protected virtual int GetLastRewriteTokenIndex(string programName)
    {
      object rewriteTokenIndex = this.lastRewriteTokenIndexes[(object) programName];
      return rewriteTokenIndex == null ? -1 : (int) rewriteTokenIndex;
    }

    protected virtual void SetLastRewriteTokenIndex(string programName, int i)
    {
      this.lastRewriteTokenIndexes[(object) programName] = (object) i;
    }

    protected virtual IList GetProgram(string name)
    {
      return (IList) this.programs[(object) name] ?? this.InitializeProgram(name);
    }

    private IList InitializeProgram(string name)
    {
      IList list = (IList) new ArrayList(100);
      this.programs[(object) name] = (object) list;
      return list;
    }

    public virtual string ToOriginalString() => this.ToOriginalString(0, this.Count - 1);

    public virtual string ToOriginalString(int start, int end)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int i = start; i >= 0 && i <= end && i < this.tokens.Count; ++i)
        stringBuilder.Append(this.Get(i).Text);
      return stringBuilder.ToString();
    }

    public override string ToString() => this.ToString(0, this.Count - 1);

    public virtual string ToString(string programName)
    {
      return this.ToString(programName, 0, this.Count - 1);
    }

    public override string ToString(int start, int end) => this.ToString("default", start, end);

    public virtual string ToString(string programName, int start, int end)
    {
      IList program = (IList) this.programs[(object) programName];
      if (end > this.tokens.Count - 1)
        end = this.tokens.Count - 1;
      if (start < 0)
        start = 0;
      if (program == null || program.Count == 0)
        return this.ToOriginalString(start, end);
      StringBuilder buf = new StringBuilder();
      IDictionary operationPerIndex = this.ReduceToSingleOperationPerIndex(program);
      int num = start;
      while (num <= end && num < this.tokens.Count)
      {
        TokenRewriteStream.RewriteOperation rewriteOperation = (TokenRewriteStream.RewriteOperation) operationPerIndex[(object) num];
        operationPerIndex.Remove((object) num);
        IToken token = (IToken) this.tokens[num];
        if (rewriteOperation == null)
        {
          buf.Append(token.Text);
          ++num;
        }
        else
          num = rewriteOperation.Execute(buf);
      }
      if (end == this.tokens.Count - 1)
      {
        foreach (TokenRewriteStream.InsertBeforeOp insertBeforeOp in (IEnumerable) operationPerIndex.Values)
        {
          if (insertBeforeOp.index >= this.tokens.Count - 1)
            buf.Append(insertBeforeOp.text);
        }
      }
      return buf.ToString();
    }

    protected IDictionary ReduceToSingleOperationPerIndex(IList rewrites)
    {
      for (int index1 = 0; index1 < rewrites.Count; ++index1)
      {
        TokenRewriteStream.RewriteOperation rewrite1 = (TokenRewriteStream.RewriteOperation) rewrites[index1];
        if (rewrite1 != null && rewrite1 is TokenRewriteStream.ReplaceOp)
        {
          TokenRewriteStream.ReplaceOp rewrite2 = (TokenRewriteStream.ReplaceOp) rewrites[index1];
          IList kindOfOps1 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.InsertBeforeOp), index1);
          for (int index2 = 0; index2 < kindOfOps1.Count; ++index2)
          {
            TokenRewriteStream.InsertBeforeOp insertBeforeOp = (TokenRewriteStream.InsertBeforeOp) kindOfOps1[index2];
            if (insertBeforeOp.index >= rewrite2.index && insertBeforeOp.index <= rewrite2.lastIndex)
              rewrites[insertBeforeOp.instructionIndex] = (object) null;
          }
          IList kindOfOps2 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.ReplaceOp), index1);
          for (int index3 = 0; index3 < kindOfOps2.Count; ++index3)
          {
            TokenRewriteStream.ReplaceOp replaceOp = (TokenRewriteStream.ReplaceOp) kindOfOps2[index3];
            if (replaceOp.index >= rewrite2.index && replaceOp.lastIndex <= rewrite2.lastIndex)
            {
              rewrites[replaceOp.instructionIndex] = (object) null;
            }
            else
            {
              bool flag1 = replaceOp.lastIndex < rewrite2.index || replaceOp.index > rewrite2.lastIndex;
              bool flag2 = replaceOp.index == rewrite2.index && replaceOp.lastIndex == rewrite2.lastIndex;
              if (!flag1 && !flag2)
                throw new ArgumentOutOfRangeException("replace op boundaries of " + (object) rewrite2 + " overlap with previous " + (object) replaceOp);
            }
          }
        }
      }
      for (int index4 = 0; index4 < rewrites.Count; ++index4)
      {
        TokenRewriteStream.RewriteOperation rewrite3 = (TokenRewriteStream.RewriteOperation) rewrites[index4];
        if (rewrite3 != null && rewrite3 is TokenRewriteStream.InsertBeforeOp)
        {
          TokenRewriteStream.InsertBeforeOp rewrite4 = (TokenRewriteStream.InsertBeforeOp) rewrites[index4];
          IList kindOfOps3 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.InsertBeforeOp), index4);
          for (int index5 = 0; index5 < kindOfOps3.Count; ++index5)
          {
            TokenRewriteStream.InsertBeforeOp insertBeforeOp = (TokenRewriteStream.InsertBeforeOp) kindOfOps3[index5];
            if (insertBeforeOp.index == rewrite4.index)
            {
              rewrite4.text = (object) this.CatOpText(rewrite4.text, insertBeforeOp.text);
              rewrites[insertBeforeOp.instructionIndex] = (object) null;
            }
          }
          IList kindOfOps4 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.ReplaceOp), index4);
          for (int index6 = 0; index6 < kindOfOps4.Count; ++index6)
          {
            TokenRewriteStream.ReplaceOp replaceOp = (TokenRewriteStream.ReplaceOp) kindOfOps4[index6];
            if (rewrite4.index == replaceOp.index)
            {
              replaceOp.text = (object) this.CatOpText(rewrite4.text, replaceOp.text);
              rewrites[index4] = (object) null;
            }
            else if (rewrite4.index >= replaceOp.index && rewrite4.index <= replaceOp.lastIndex)
              throw new ArgumentOutOfRangeException("insert op " + (object) rewrite4 + " within boundaries of previous " + (object) replaceOp);
          }
        }
      }
      IDictionary operationPerIndex = (IDictionary) new Hashtable();
      for (int index = 0; index < rewrites.Count; ++index)
      {
        TokenRewriteStream.RewriteOperation rewrite = (TokenRewriteStream.RewriteOperation) rewrites[index];
        if (rewrite != null)
        {
          if (operationPerIndex[(object) rewrite.index] != null)
            throw new Exception("should only be one op per index");
          operationPerIndex[(object) rewrite.index] = (object) rewrite;
        }
      }
      return operationPerIndex;
    }

    protected string CatOpText(object a, object b)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (a != null)
        empty1 = a.ToString();
      if (b != null)
        empty2 = b.ToString();
      return empty1 + empty2;
    }

    protected IList GetKindOfOps(IList rewrites, Type kind)
    {
      return this.GetKindOfOps(rewrites, kind, rewrites.Count);
    }

    protected IList GetKindOfOps(IList rewrites, Type kind, int before)
    {
      IList kindOfOps = (IList) new ArrayList();
      for (int index = 0; index < before && index < rewrites.Count; ++index)
      {
        TokenRewriteStream.RewriteOperation rewrite = (TokenRewriteStream.RewriteOperation) rewrites[index];
        if (rewrite != null && rewrite.GetType() == kind)
          kindOfOps.Add((object) rewrite);
      }
      return kindOfOps;
    }

    public virtual string ToDebugString() => this.ToDebugString(0, this.Count - 1);

    public virtual string ToDebugString(int start, int end)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int i = start; i >= 0 && i <= end && i < this.tokens.Count; ++i)
        stringBuilder.Append((object) this.Get(i));
      return stringBuilder.ToString();
    }

    private class RewriteOpComparer : IComparer
    {
      public virtual int Compare(object o1, object o2)
      {
        TokenRewriteStream.RewriteOperation rewriteOperation1 = (TokenRewriteStream.RewriteOperation) o1;
        TokenRewriteStream.RewriteOperation rewriteOperation2 = (TokenRewriteStream.RewriteOperation) o2;
        if (rewriteOperation1.index < rewriteOperation2.index)
          return -1;
        return rewriteOperation1.index > rewriteOperation2.index ? 1 : 0;
      }
    }

    protected internal class RewriteOperation
    {
      protected internal int instructionIndex;
      protected internal int index;
      protected internal object text;
      protected internal TokenRewriteStream parent;

      protected internal RewriteOperation(int index, object text, TokenRewriteStream parent)
      {
        this.index = index;
        this.text = text;
        this.parent = parent;
      }

      public virtual int Execute(StringBuilder buf) => this.index;

      public override string ToString()
      {
        string fullName = this.GetType().FullName;
        int num = fullName.IndexOf('$');
        return "<" + fullName.Substring(num + 1, fullName.Length - (num + 1)) + "@" + (object) this.index + ":\"" + this.text + "\">";
      }
    }

    protected internal class InsertBeforeOp(int index, object text, TokenRewriteStream parent) : 
      TokenRewriteStream.RewriteOperation(index, text, parent)
    {
      public override int Execute(StringBuilder buf)
      {
        buf.Append(this.text);
        buf.Append(this.parent.Get(this.index).Text);
        return this.index + 1;
      }
    }

    protected internal class ReplaceOp : TokenRewriteStream.RewriteOperation
    {
      protected internal int lastIndex;

      public ReplaceOp(int from, int to, object text, TokenRewriteStream parent)
        : base(from, text, parent)
      {
        this.lastIndex = to;
      }

      public override int Execute(StringBuilder buf)
      {
        if (this.text != null)
          buf.Append(this.text);
        return this.lastIndex + 1;
      }

      public override string ToString()
      {
        return "<ReplaceOp@" + (object) this.index + ".." + (object) this.lastIndex + ":\"" + this.text + "\">";
      }
    }

    protected internal class DeleteOp(int from, int to, TokenRewriteStream parent) : 
      TokenRewriteStream.ReplaceOp(from, to, (object) null, parent)
    {
      public override string ToString()
      {
        return "<DeleteOp@" + (object) this.index + ".." + (object) this.lastIndex + ">";
      }
    }
  }
}
