﻿// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.TokenRewriteStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  [DebuggerDisplay("TODO: TokenRewriteStream debugger display")]
  [Serializable]
  public class TokenRewriteStream : CommonTokenStream
  {
    public const string DEFAULT_PROGRAM_NAME = "default";
    public const int PROGRAM_INIT_SIZE = 100;
    public const int MIN_TOKEN_INDEX = 0;
    protected IDictionary<string, IList<TokenRewriteStream.RewriteOperation>> programs;
    protected IDictionary<string, int> lastRewriteTokenIndexes;

    public TokenRewriteStream() => this.Init();

    protected void Init()
    {
      this.programs = (IDictionary<string, IList<TokenRewriteStream.RewriteOperation>>) new Dictionary<string, IList<TokenRewriteStream.RewriteOperation>>();
      this.programs["default"] = (IList<TokenRewriteStream.RewriteOperation>) new List<TokenRewriteStream.RewriteOperation>(100);
      this.lastRewriteTokenIndexes = (IDictionary<string, int>) new Dictionary<string, int>();
    }

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

    public virtual void Rollback(int instructionIndex)
    {
      this.Rollback("default", instructionIndex);
    }

    public virtual void Rollback(string programName, int instructionIndex)
    {
      IList<TokenRewriteStream.RewriteOperation> rewriteOperationList1;
      if (!this.programs.TryGetValue(programName, out rewriteOperationList1) || rewriteOperationList1 == null)
        return;
      List<TokenRewriteStream.RewriteOperation> rewriteOperationList2 = new List<TokenRewriteStream.RewriteOperation>();
      for (int index = 0; index <= instructionIndex; ++index)
        rewriteOperationList2.Add(rewriteOperationList1[index]);
      this.programs[programName] = (IList<TokenRewriteStream.RewriteOperation>) rewriteOperationList2;
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
      TokenRewriteStream.RewriteOperation rewriteOperation = (TokenRewriteStream.RewriteOperation) new TokenRewriteStream.InsertBeforeOp(this, index, text);
      IList<TokenRewriteStream.RewriteOperation> program = this.GetProgram(programName);
      rewriteOperation.instructionIndex = program.Count;
      program.Add(rewriteOperation);
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
      if (from > to || from < 0 || to < 0 || to >= this._tokens.Count)
        throw new ArgumentException("replace: range invalid: " + (object) from + ".." + (object) to + "(size=" + (object) this._tokens.Count + ")");
      TokenRewriteStream.RewriteOperation rewriteOperation = (TokenRewriteStream.RewriteOperation) new TokenRewriteStream.ReplaceOp(this, from, to, text);
      IList<TokenRewriteStream.RewriteOperation> program = this.GetProgram(programName);
      rewriteOperation.instructionIndex = program.Count;
      program.Add(rewriteOperation);
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
      int num;
      return this.lastRewriteTokenIndexes.TryGetValue(programName, out num) ? num : -1;
    }

    protected virtual void SetLastRewriteTokenIndex(string programName, int i)
    {
      this.lastRewriteTokenIndexes[programName] = i;
    }

    protected virtual IList<TokenRewriteStream.RewriteOperation> GetProgram(string name)
    {
      IList<TokenRewriteStream.RewriteOperation> program;
      if (!this.programs.TryGetValue(name, out program) || program == null)
        program = this.InitializeProgram(name);
      return program;
    }

    private IList<TokenRewriteStream.RewriteOperation> InitializeProgram(string name)
    {
      IList<TokenRewriteStream.RewriteOperation> rewriteOperationList = (IList<TokenRewriteStream.RewriteOperation>) new List<TokenRewriteStream.RewriteOperation>(100);
      this.programs[name] = rewriteOperationList;
      return rewriteOperationList;
    }

    public virtual string ToOriginalString()
    {
      this.Fill();
      return this.ToOriginalString(0, this.Count - 1);
    }

    public virtual string ToOriginalString(int start, int end)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int i = start; i >= 0 && i <= end && i < this._tokens.Count; ++i)
      {
        if (this.Get(i).Type != -1)
          stringBuilder.Append(this.Get(i).Text);
      }
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      this.Fill();
      return this.ToString(0, this.Count - 1);
    }

    public virtual string ToString(string programName)
    {
      this.Fill();
      return this.ToString(programName, 0, this.Count - 1);
    }

    public override string ToString(int start, int end) => this.ToString("default", start, end);

    public virtual string ToString(string programName, int start, int end)
    {
      IList<TokenRewriteStream.RewriteOperation> rewrites;
      if (!this.programs.TryGetValue(programName, out rewrites))
        rewrites = (IList<TokenRewriteStream.RewriteOperation>) null;
      if (end > this._tokens.Count - 1)
        end = this._tokens.Count - 1;
      if (start < 0)
        start = 0;
      if (rewrites == null || rewrites.Count == 0)
        return this.ToOriginalString(start, end);
      StringBuilder buf = new StringBuilder();
      IDictionary<int, TokenRewriteStream.RewriteOperation> operationPerIndex = this.ReduceToSingleOperationPerIndex(rewrites);
      int num = start;
      while (num <= end && num < this._tokens.Count)
      {
        TokenRewriteStream.RewriteOperation rewriteOperation;
        bool flag = operationPerIndex.TryGetValue(num, out rewriteOperation);
        if (flag)
          operationPerIndex.Remove(num);
        if (!flag || rewriteOperation == null)
        {
          IToken token = this._tokens[num];
          if (token.Type != -1)
            buf.Append(token.Text);
          ++num;
        }
        else
          num = rewriteOperation.Execute(buf);
      }
      if (end == this._tokens.Count - 1)
      {
        foreach (TokenRewriteStream.RewriteOperation rewriteOperation in (IEnumerable<TokenRewriteStream.RewriteOperation>) operationPerIndex.Values)
        {
          if (rewriteOperation.index >= this._tokens.Count - 1)
            buf.Append(rewriteOperation.text);
        }
      }
      return buf.ToString();
    }

    protected virtual IDictionary<int, TokenRewriteStream.RewriteOperation> ReduceToSingleOperationPerIndex(
      IList<TokenRewriteStream.RewriteOperation> rewrites)
    {
      for (int index1 = 0; index1 < rewrites.Count; ++index1)
      {
        TokenRewriteStream.RewriteOperation rewrite1 = rewrites[index1];
        if (rewrite1 != null && rewrite1 is TokenRewriteStream.ReplaceOp)
        {
          TokenRewriteStream.ReplaceOp rewrite2 = (TokenRewriteStream.ReplaceOp) rewrites[index1];
          IList<TokenRewriteStream.RewriteOperation> kindOfOps1 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.InsertBeforeOp), index1);
          for (int index2 = 0; index2 < kindOfOps1.Count; ++index2)
          {
            TokenRewriteStream.InsertBeforeOp insertBeforeOp = (TokenRewriteStream.InsertBeforeOp) kindOfOps1[index2];
            if (insertBeforeOp.index == rewrite2.index)
            {
              rewrites[insertBeforeOp.instructionIndex] = (TokenRewriteStream.RewriteOperation) null;
              rewrite2.text = (object) (insertBeforeOp.text.ToString() + (rewrite2.text != null ? rewrite2.text.ToString() : string.Empty));
            }
            else if (insertBeforeOp.index > rewrite2.index && insertBeforeOp.index <= rewrite2.lastIndex)
              rewrites[insertBeforeOp.instructionIndex] = (TokenRewriteStream.RewriteOperation) null;
          }
          IList<TokenRewriteStream.RewriteOperation> kindOfOps2 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.ReplaceOp), index1);
          for (int index3 = 0; index3 < kindOfOps2.Count; ++index3)
          {
            TokenRewriteStream.ReplaceOp replaceOp = (TokenRewriteStream.ReplaceOp) kindOfOps2[index3];
            if (replaceOp.index >= rewrite2.index && replaceOp.lastIndex <= rewrite2.lastIndex)
            {
              rewrites[replaceOp.instructionIndex] = (TokenRewriteStream.RewriteOperation) null;
            }
            else
            {
              bool flag1 = replaceOp.lastIndex < rewrite2.index || replaceOp.index > rewrite2.lastIndex;
              bool flag2 = replaceOp.index == rewrite2.index && replaceOp.lastIndex == rewrite2.lastIndex;
              if (replaceOp.text == null && rewrite2.text == null && !flag1)
              {
                rewrites[replaceOp.instructionIndex] = (TokenRewriteStream.RewriteOperation) null;
                rewrite2.index = Math.Min(replaceOp.index, rewrite2.index);
                rewrite2.lastIndex = Math.Max(replaceOp.lastIndex, rewrite2.lastIndex);
                Console.WriteLine("new rop " + (object) rewrite2);
              }
              else if (!flag1 && !flag2)
                throw new ArgumentException("replace op boundaries of " + (object) rewrite2 + " overlap with previous " + (object) replaceOp);
            }
          }
        }
      }
      for (int index4 = 0; index4 < rewrites.Count; ++index4)
      {
        TokenRewriteStream.RewriteOperation rewrite3 = rewrites[index4];
        if (rewrite3 != null && rewrite3 is TokenRewriteStream.InsertBeforeOp)
        {
          TokenRewriteStream.InsertBeforeOp rewrite4 = (TokenRewriteStream.InsertBeforeOp) rewrites[index4];
          IList<TokenRewriteStream.RewriteOperation> kindOfOps3 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.InsertBeforeOp), index4);
          for (int index5 = 0; index5 < kindOfOps3.Count; ++index5)
          {
            TokenRewriteStream.InsertBeforeOp insertBeforeOp = (TokenRewriteStream.InsertBeforeOp) kindOfOps3[index5];
            if (insertBeforeOp.index == rewrite4.index)
            {
              rewrite4.text = (object) this.CatOpText(rewrite4.text, insertBeforeOp.text);
              rewrites[insertBeforeOp.instructionIndex] = (TokenRewriteStream.RewriteOperation) null;
            }
          }
          IList<TokenRewriteStream.RewriteOperation> kindOfOps4 = this.GetKindOfOps(rewrites, typeof (TokenRewriteStream.ReplaceOp), index4);
          for (int index6 = 0; index6 < kindOfOps4.Count; ++index6)
          {
            TokenRewriteStream.ReplaceOp replaceOp = (TokenRewriteStream.ReplaceOp) kindOfOps4[index6];
            if (rewrite4.index == replaceOp.index)
            {
              replaceOp.text = (object) this.CatOpText(rewrite4.text, replaceOp.text);
              rewrites[index4] = (TokenRewriteStream.RewriteOperation) null;
            }
            else if (rewrite4.index >= replaceOp.index && rewrite4.index <= replaceOp.lastIndex)
              throw new ArgumentException("insert op " + (object) rewrite4 + " within boundaries of previous " + (object) replaceOp);
          }
        }
      }
      IDictionary<int, TokenRewriteStream.RewriteOperation> operationPerIndex = (IDictionary<int, TokenRewriteStream.RewriteOperation>) new Dictionary<int, TokenRewriteStream.RewriteOperation>();
      for (int index = 0; index < rewrites.Count; ++index)
      {
        TokenRewriteStream.RewriteOperation rewrite = rewrites[index];
        if (rewrite != null)
        {
          TokenRewriteStream.RewriteOperation rewriteOperation;
          if (operationPerIndex.TryGetValue(rewrite.index, out rewriteOperation) && rewriteOperation != null)
            throw new Exception("should only be one op per index");
          operationPerIndex[rewrite.index] = rewrite;
        }
      }
      return operationPerIndex;
    }

    protected virtual string CatOpText(object a, object b) => a.ToString() + b;

    protected virtual IList<TokenRewriteStream.RewriteOperation> GetKindOfOps(
      IList<TokenRewriteStream.RewriteOperation> rewrites,
      Type kind)
    {
      return this.GetKindOfOps(rewrites, kind, rewrites.Count);
    }

    protected virtual IList<TokenRewriteStream.RewriteOperation> GetKindOfOps(
      IList<TokenRewriteStream.RewriteOperation> rewrites,
      Type kind,
      int before)
    {
      IList<TokenRewriteStream.RewriteOperation> kindOfOps = (IList<TokenRewriteStream.RewriteOperation>) new List<TokenRewriteStream.RewriteOperation>();
      for (int index = 0; index < before && index < rewrites.Count; ++index)
      {
        TokenRewriteStream.RewriteOperation rewrite = rewrites[index];
        if (rewrite != null && rewrite.GetType() == kind)
          kindOfOps.Add(rewrite);
      }
      return kindOfOps;
    }

    public virtual string ToDebugString() => this.ToDebugString(0, this.Count - 1);

    public virtual string ToDebugString(int start, int end)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int i = start; i >= 0 && i <= end && i < this._tokens.Count; ++i)
        stringBuilder.Append((object) this.Get(i));
      return stringBuilder.ToString();
    }

    protected class RewriteOperation
    {
      public int instructionIndex;
      public int index;
      public object text;
      protected TokenRewriteStream stream;

      protected RewriteOperation(TokenRewriteStream stream, int index)
      {
        this.stream = stream;
        this.index = index;
      }

      protected RewriteOperation(TokenRewriteStream stream, int index, object text)
      {
        this.index = index;
        this.text = text;
        this.stream = stream;
      }

      public virtual int Execute(StringBuilder buf) => this.index;

      public override string ToString()
      {
        string name = this.GetType().Name;
        int num = name.IndexOf('$');
        return string.Format("<{0}@{1}:\"{2}\">", (object) name.Substring(num + 1), (object) this.stream._tokens[this.index], this.text);
      }
    }

    private class InsertBeforeOp(TokenRewriteStream stream, int index, object text) : 
      TokenRewriteStream.RewriteOperation(stream, index, text)
    {
      public override int Execute(StringBuilder buf)
      {
        buf.Append(this.text);
        if (this.stream._tokens[this.index].Type != -1)
          buf.Append(this.stream._tokens[this.index].Text);
        return this.index + 1;
      }
    }

    private class ReplaceOp : TokenRewriteStream.RewriteOperation
    {
      public int lastIndex;

      public ReplaceOp(TokenRewriteStream stream, int from, int to, object text)
        : base(stream, from, text)
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
        return this.text == null ? string.Format("<DeleteOp@{0}..{1}>", (object) this.stream._tokens[this.index], (object) this.stream._tokens[this.lastIndex]) : string.Format("<ReplaceOp@{0}..{1}:\"{2}\">", (object) this.stream._tokens[this.index], (object) this.stream._tokens[this.lastIndex], this.text);
      }
    }
  }
}
