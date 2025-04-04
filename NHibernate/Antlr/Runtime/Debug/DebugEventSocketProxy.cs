// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugEventSocketProxy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugEventSocketProxy : BlankDebugEventListener
  {
    public const int DEFAULT_DEBUGGER_PORT = 49100;
    protected int port = 49100;
    protected TcpListener serverSocket;
    protected TcpClient socket;
    protected string grammarFileName;
    protected StreamWriter writer;
    protected StreamReader reader;
    protected BaseRecognizer recognizer;
    protected ITreeAdaptor adaptor;

    public DebugEventSocketProxy(BaseRecognizer recognizer, ITreeAdaptor adaptor)
      : this(recognizer, 49100, adaptor)
    {
    }

    public DebugEventSocketProxy(BaseRecognizer recognizer, int port, ITreeAdaptor adaptor)
    {
      this.grammarFileName = recognizer.GrammarFileName;
      this.port = port;
      this.adaptor = adaptor;
    }

    public virtual void Handshake()
    {
      if (this.serverSocket != null)
        return;
      this.serverSocket = new TcpListener(this.port);
      this.serverSocket.Start();
      this.socket = this.serverSocket.AcceptTcpClient();
      this.socket.NoDelay = true;
      this.reader = new StreamReader((Stream) this.socket.GetStream(), Encoding.UTF8);
      this.writer = new StreamWriter((Stream) this.socket.GetStream(), Encoding.UTF8);
      this.writer.WriteLine("ANTLR " + Constants.DEBUG_PROTOCOL_VERSION);
      this.writer.WriteLine("grammar \"" + this.grammarFileName);
      this.writer.Flush();
      this.Ack();
    }

    public override void Commence()
    {
    }

    public override void Terminate()
    {
      this.Transmit("terminate");
      this.writer.Close();
      try
      {
        this.socket.Close();
      }
      catch (IOException ex)
      {
        Console.Error.WriteLine(ex.StackTrace);
      }
    }

    protected internal virtual void Ack()
    {
      try
      {
        this.reader.ReadLine();
      }
      catch (IOException ex)
      {
        Console.Error.WriteLine(ex.StackTrace);
      }
    }

    protected internal virtual void Transmit(string eventLabel)
    {
      this.writer.WriteLine(eventLabel);
      this.writer.Flush();
      this.Ack();
    }

    public override void EnterRule(string grammarFileName, string ruleName)
    {
      this.Transmit("enterRule\t" + grammarFileName + "\t" + ruleName);
    }

    public override void EnterAlt(int alt) => this.Transmit("enterAlt\t" + (object) alt);

    public override void ExitRule(string grammarFileName, string ruleName)
    {
      this.Transmit("exitRule\t" + grammarFileName + "\t" + ruleName);
    }

    public override void EnterSubRule(int decisionNumber)
    {
      this.Transmit("enterSubRule\t" + (object) decisionNumber);
    }

    public override void ExitSubRule(int decisionNumber)
    {
      this.Transmit("exitSubRule\t" + (object) decisionNumber);
    }

    public override void EnterDecision(int decisionNumber)
    {
      this.Transmit("enterDecision\t" + (object) decisionNumber);
    }

    public override void ExitDecision(int decisionNumber)
    {
      this.Transmit("exitDecision\t" + (object) decisionNumber);
    }

    public override void ConsumeToken(IToken t)
    {
      this.Transmit("consumeToken\t" + this.SerializeToken(t));
    }

    public override void ConsumeHiddenToken(IToken t)
    {
      this.Transmit("consumeHiddenToken\t" + this.SerializeToken(t));
    }

    public override void LT(int i, IToken t)
    {
      if (t == null)
        return;
      this.Transmit("LT\t" + (object) i + "\t" + this.SerializeToken(t));
    }

    public override void Mark(int i) => this.Transmit("mark\t" + (object) i);

    public override void Rewind(int i) => this.Transmit("rewind\t" + (object) i);

    public override void Rewind() => this.Transmit("rewind");

    public override void BeginBacktrack(int level)
    {
      this.Transmit("beginBacktrack\t" + (object) level);
    }

    public override void EndBacktrack(int level, bool successful)
    {
      this.Transmit("endBacktrack\t" + (object) level + "\t" + (!successful ? (object) false.ToString() : (object) true.ToString()));
    }

    public override void Location(int line, int pos)
    {
      this.Transmit("location\t" + (object) line + "\t" + (object) pos);
    }

    public override void RecognitionException(Antlr.Runtime.RecognitionException e)
    {
      StringBuilder stringBuilder = new StringBuilder(50);
      stringBuilder.Append("exception\t");
      stringBuilder.Append(e.GetType().FullName);
      stringBuilder.Append("\t");
      stringBuilder.Append(e.Index);
      stringBuilder.Append("\t");
      stringBuilder.Append(e.Line);
      stringBuilder.Append("\t");
      stringBuilder.Append(e.CharPositionInLine);
      this.Transmit(stringBuilder.ToString());
    }

    public override void BeginResync() => this.Transmit("beginResync");

    public override void EndResync() => this.Transmit("endResync");

    public override void SemanticPredicate(bool result, string predicate)
    {
      StringBuilder buf = new StringBuilder(50);
      buf.Append("semanticPredicate\t");
      buf.Append(result);
      this.SerializeText(buf, predicate);
      this.Transmit(buf.ToString());
    }

    public override void ConsumeNode(object t)
    {
      StringBuilder buf = new StringBuilder(50);
      buf.Append("consumeNode\t");
      this.SerializeNode(buf, t);
      this.Transmit(buf.ToString());
    }

    public override void LT(int i, object t)
    {
      this.adaptor.GetUniqueID(t);
      this.adaptor.GetNodeText(t);
      this.adaptor.GetNodeType(t);
      StringBuilder buf = new StringBuilder(50);
      buf.Append("LN\t");
      buf.Append(i);
      this.SerializeNode(buf, t);
      this.Transmit(buf.ToString());
    }

    public override void GetNilNode(object t)
    {
      this.Transmit("nilNode\t" + (object) this.adaptor.GetUniqueID(t));
    }

    public override void ErrorNode(object t)
    {
      int uniqueId = this.adaptor.GetUniqueID(t);
      string text = t.ToString();
      StringBuilder buf = new StringBuilder(50);
      buf.Append("errorNode\t");
      buf.Append(uniqueId);
      buf.Append("\t");
      buf.Append(0);
      this.SerializeText(buf, text);
      this.Transmit(buf.ToString());
    }

    public override void CreateNode(object t)
    {
      int uniqueId = this.adaptor.GetUniqueID(t);
      string nodeText = this.adaptor.GetNodeText(t);
      int nodeType = this.adaptor.GetNodeType(t);
      StringBuilder buf = new StringBuilder(50);
      buf.Append("createNodeFromTokenElements\t");
      buf.Append(uniqueId);
      buf.Append("\t");
      buf.Append(nodeType);
      this.SerializeText(buf, nodeText);
      this.Transmit(buf.ToString());
    }

    public override void CreateNode(object node, IToken token)
    {
      this.Transmit("createNode\t" + (object) this.adaptor.GetUniqueID(node) + "\t" + (object) token.TokenIndex);
    }

    public override void BecomeRoot(object newRoot, object oldRoot)
    {
      this.Transmit("becomeRoot\t" + (object) this.adaptor.GetUniqueID(newRoot) + "\t" + (object) this.adaptor.GetUniqueID(oldRoot));
    }

    public override void AddChild(object root, object child)
    {
      this.Transmit("addChild\t" + (object) this.adaptor.GetUniqueID(root) + "\t" + (object) this.adaptor.GetUniqueID(child));
    }

    public override void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex)
    {
      this.Transmit("setTokenBoundaries\t" + (object) this.adaptor.GetUniqueID(t) + "\t" + (object) tokenStartIndex + "\t" + (object) tokenStopIndex);
    }

    public ITreeAdaptor TreeAdaptor
    {
      set => this.adaptor = value;
      get => this.adaptor;
    }

    protected internal virtual string SerializeToken(IToken t)
    {
      StringBuilder buf = new StringBuilder(50);
      buf.Append(t.TokenIndex);
      buf.Append('\t');
      buf.Append(t.Type);
      buf.Append('\t');
      buf.Append(t.Channel);
      buf.Append('\t');
      buf.Append(t.Line);
      buf.Append('\t');
      buf.Append(t.CharPositionInLine);
      this.SerializeText(buf, t.Text);
      return buf.ToString();
    }

    protected internal virtual string EscapeNewlines(string txt)
    {
      txt = txt.Replace("%", "%25");
      txt = txt.Replace("\n", "%0A");
      txt = txt.Replace("\r", "%0D");
      return txt;
    }

    protected internal void SerializeNode(StringBuilder buf, object t)
    {
      int uniqueId = this.adaptor.GetUniqueID(t);
      string nodeText = this.adaptor.GetNodeText(t);
      int nodeType = this.adaptor.GetNodeType(t);
      buf.Append("\t");
      buf.Append(uniqueId);
      buf.Append("\t");
      buf.Append(nodeType);
      IToken token = this.adaptor.GetToken(t);
      int num1 = -1;
      int num2 = -1;
      if (token != null)
      {
        num1 = token.Line;
        num2 = token.CharPositionInLine;
      }
      buf.Append("\t");
      buf.Append(num1);
      buf.Append("\t");
      buf.Append(num2);
      int tokenStartIndex = this.adaptor.GetTokenStartIndex(t);
      buf.Append("\t");
      buf.Append(tokenStartIndex);
      this.SerializeText(buf, nodeText);
    }

    protected void SerializeText(StringBuilder buf, string text)
    {
      buf.Append("\t\"");
      if (text == null)
        text = string.Empty;
      text = this.EscapeNewlines(text);
      buf.Append(text);
    }
  }
}
