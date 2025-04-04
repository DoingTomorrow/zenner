// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.TraceDebugEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class TraceDebugEventListener : BlankDebugEventListener
  {
    private ITreeAdaptor adaptor;

    public TraceDebugEventListener(ITreeAdaptor adaptor) => this.adaptor = adaptor;

    public override void EnterRule(string grammarFileName, string ruleName)
    {
      Console.Out.WriteLine("EnterRule " + grammarFileName + " " + ruleName);
    }

    public override void ExitRule(string grammarFileName, string ruleName)
    {
      Console.Out.WriteLine("ExitRule " + grammarFileName + " " + ruleName);
    }

    public override void EnterSubRule(int decisionNumber)
    {
      Console.Out.WriteLine(nameof (EnterSubRule));
    }

    public override void ExitSubRule(int decisionNumber)
    {
      Console.Out.WriteLine(nameof (ExitSubRule));
    }

    public override void Location(int line, int pos)
    {
      Console.Out.WriteLine("Location " + (object) line + ":" + (object) pos);
    }

    public override void ConsumeNode(object t)
    {
      Console.Out.WriteLine("ConsumeNode " + (object) this.adaptor.GetUniqueID(t) + " " + this.adaptor.GetNodeText(t) + " " + (object) this.adaptor.GetNodeType(t));
    }

    public override void LT(int i, object t)
    {
      int uniqueId = this.adaptor.GetUniqueID(t);
      string nodeText = this.adaptor.GetNodeText(t);
      int nodeType = this.adaptor.GetNodeType(t);
      Console.Out.WriteLine("LT " + (object) i + " " + (object) uniqueId + " " + nodeText + " " + (object) nodeType);
    }

    public override void GetNilNode(object t)
    {
      Console.Out.WriteLine("GetNilNode " + (object) this.adaptor.GetUniqueID(t));
    }

    public override void CreateNode(object t)
    {
      Console.Out.WriteLine("Create " + (object) this.adaptor.GetUniqueID(t) + ": " + this.adaptor.GetNodeText(t) + ", " + (object) this.adaptor.GetNodeType(t));
    }

    public override void CreateNode(object t, IToken token)
    {
      Console.Out.WriteLine("Create " + (object) this.adaptor.GetUniqueID(t) + ": " + (object) token.TokenIndex);
    }

    public override void BecomeRoot(object newRoot, object oldRoot)
    {
      Console.Out.WriteLine("BecomeRoot " + (object) this.adaptor.GetUniqueID(newRoot) + ", " + (object) this.adaptor.GetUniqueID(oldRoot));
    }

    public override void AddChild(object root, object child)
    {
      Console.Out.WriteLine("AddChild " + (object) this.adaptor.GetUniqueID(root) + ", " + (object) this.adaptor.GetUniqueID(child));
    }

    public override void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex)
    {
      Console.Out.WriteLine("SetTokenBoundaries " + (object) this.adaptor.GetUniqueID(t) + ", " + (object) tokenStartIndex + ", " + (object) tokenStopIndex);
    }
  }
}
