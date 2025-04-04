// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.RemoteDebugEventSocketListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class RemoteDebugEventSocketListener
  {
    internal const int MAX_EVENT_ELEMENTS = 8;
    internal IDebugEventListener listener;
    internal string hostName;
    internal int port;
    internal TcpClient channel;
    internal StreamWriter writer;
    internal StreamReader reader;
    internal string eventLabel;
    public string version;
    public string grammarFileName;
    private int previousTokenIndex = -1;
    private bool tokenIndexesInvalid;

    public RemoteDebugEventSocketListener(IDebugEventListener listener, string hostName, int port)
    {
      this.listener = listener;
      this.hostName = hostName;
      this.port = port;
      if (!this.OpenConnection())
        throw new Exception();
    }

    protected virtual void EventHandler()
    {
      try
      {
        this.Handshake();
        for (this.eventLabel = this.reader.ReadLine(); this.eventLabel != null; this.eventLabel = this.reader.ReadLine())
        {
          this.Dispatch(this.eventLabel);
          this.Ack();
        }
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine((object) ex);
        Console.Error.WriteLine(ex.StackTrace);
      }
      finally
      {
        this.CloseConnection();
      }
    }

    protected virtual bool OpenConnection()
    {
      bool flag = false;
      try
      {
        this.channel = new TcpClient(this.hostName, this.port);
        this.channel.NoDelay = true;
        this.writer = new StreamWriter((Stream) this.channel.GetStream(), Encoding.UTF8);
        this.reader = new StreamReader((Stream) this.channel.GetStream(), Encoding.UTF8);
        flag = true;
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine((object) ex);
      }
      return flag;
    }

    protected virtual void CloseConnection()
    {
      try
      {
        this.reader.Close();
        this.reader = (StreamReader) null;
        this.writer.Close();
        this.writer = (StreamWriter) null;
        this.channel.Close();
        this.channel = (TcpClient) null;
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine((object) ex);
        Console.Error.WriteLine(ex.StackTrace);
      }
      finally
      {
        if (this.reader != null)
        {
          try
          {
            this.reader.Close();
          }
          catch (IOException ex)
          {
            Console.Error.WriteLine((object) ex);
          }
        }
        if (this.writer != null)
          this.writer.Close();
        if (this.channel != null)
        {
          try
          {
            this.channel.Close();
          }
          catch (IOException ex)
          {
            Console.Error.WriteLine((object) ex);
          }
        }
      }
    }

    protected virtual void Handshake()
    {
      this.version = this.GetEventElements(this.reader.ReadLine())[1];
      this.grammarFileName = this.GetEventElements(this.reader.ReadLine())[1];
      this.Ack();
      this.listener.Commence();
    }

    protected virtual void Ack()
    {
      this.writer.WriteLine("ack");
      this.writer.Flush();
    }

    protected virtual void Dispatch(string line)
    {
      string[] eventElements = this.GetEventElements(line);
      if (eventElements == null || eventElements[0] == null)
        Console.Error.WriteLine("unknown debug event: " + line);
      else if (eventElements[0].Equals("enterRule"))
        this.listener.EnterRule(eventElements[1], eventElements[2]);
      else if (eventElements[0].Equals("exitRule"))
        this.listener.ExitRule(eventElements[1], eventElements[2]);
      else if (eventElements[0].Equals("enterAlt"))
        this.listener.EnterAlt(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("enterSubRule"))
        this.listener.EnterSubRule(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("exitSubRule"))
        this.listener.ExitSubRule(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("enterDecision"))
        this.listener.EnterDecision(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("exitDecision"))
        this.listener.ExitDecision(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("location"))
        this.listener.Location(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("consumeToken"))
      {
        RemoteDebugEventSocketListener.ProxyToken t = this.DeserializeToken(eventElements, 1);
        if (t.TokenIndex == this.previousTokenIndex)
          this.tokenIndexesInvalid = true;
        this.previousTokenIndex = t.TokenIndex;
        this.listener.ConsumeToken((IToken) t);
      }
      else if (eventElements[0].Equals("consumeHiddenToken"))
      {
        RemoteDebugEventSocketListener.ProxyToken t = this.DeserializeToken(eventElements, 1);
        if (t.TokenIndex == this.previousTokenIndex)
          this.tokenIndexesInvalid = true;
        this.previousTokenIndex = t.TokenIndex;
        this.listener.ConsumeHiddenToken((IToken) t);
      }
      else if (eventElements[0].Equals("LT"))
      {
        IToken t = (IToken) this.DeserializeToken(eventElements, 2);
        this.listener.LT(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture), t);
      }
      else if (eventElements[0].Equals("mark"))
        this.listener.Mark(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("rewind"))
      {
        if (eventElements[1] != null)
          this.listener.Rewind(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
        else
          this.listener.Rewind();
      }
      else if (eventElements[0].Equals("beginBacktrack"))
        this.listener.BeginBacktrack(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture));
      else if (eventElements[0].Equals("endBacktrack"))
        this.listener.EndBacktrack(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture) == 1);
      else if (eventElements[0].Equals("exception"))
      {
        string typeName = eventElements[1];
        string s1 = eventElements[2];
        string s2 = eventElements[3];
        string s3 = eventElements[4];
        try
        {
          RecognitionException instance = (RecognitionException) Activator.CreateInstance(Type.GetType(typeName));
          instance.Index = int.Parse(s1, (IFormatProvider) CultureInfo.InvariantCulture);
          instance.Line = int.Parse(s2, (IFormatProvider) CultureInfo.InvariantCulture);
          instance.CharPositionInLine = int.Parse(s3, (IFormatProvider) CultureInfo.InvariantCulture);
          this.listener.RecognitionException(instance);
        }
        catch (UnauthorizedAccessException ex)
        {
          Console.Error.WriteLine("can't access class " + (object) ex);
          Console.Error.WriteLine(ex.StackTrace);
        }
      }
      else if (eventElements[0].Equals("beginResync"))
        this.listener.BeginResync();
      else if (eventElements[0].Equals("endResync"))
        this.listener.EndResync();
      else if (eventElements[0].Equals("terminate"))
        this.listener.Terminate();
      else if (eventElements[0].Equals("semanticPredicate"))
        this.listener.SemanticPredicate(bool.Parse(eventElements[1]), this.UnEscapeNewlines(eventElements[2]));
      else if (eventElements[0].Equals("consumeNode"))
        this.listener.ConsumeNode((object) this.DeserializeNode(eventElements, 1));
      else if (eventElements[0].Equals("LN"))
        this.listener.LT(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture), (object) this.DeserializeNode(eventElements, 2));
      else if (eventElements[0].Equals("createNodeFromTokenElements"))
        this.listener.CreateNode((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture), -1, -1, -1, this.UnEscapeNewlines(eventElements[3])));
      else if (eventElements[0].Equals("createNode"))
        this.listener.CreateNode((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture)), (IToken) new RemoteDebugEventSocketListener.ProxyToken(int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture)));
      else if (eventElements[0].Equals("nilNode"))
        this.listener.GetNilNode((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture)));
      else if (eventElements[0].Equals("errorNode"))
        this.listener.ErrorNode((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture), -1, -1, -1, this.UnEscapeNewlines(eventElements[3])));
      else if (eventElements[0].Equals("becomeRoot"))
        this.listener.BecomeRoot((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture)), (object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture)));
      else if (eventElements[0].Equals("addChild"))
        this.listener.AddChild((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture)), (object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture)));
      else if (eventElements[0].Equals("setTokenBoundaries"))
        this.listener.SetTokenBoundaries((object) new RemoteDebugEventSocketListener.ProxyTree(int.Parse(eventElements[1], (IFormatProvider) CultureInfo.InvariantCulture)), int.Parse(eventElements[2], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(eventElements[3], (IFormatProvider) CultureInfo.InvariantCulture));
      else
        Console.Error.WriteLine("unknown debug event: " + line);
    }

    protected internal RemoteDebugEventSocketListener.ProxyTree DeserializeNode(
      string[] elements,
      int offset)
    {
      return new RemoteDebugEventSocketListener.ProxyTree(int.Parse(elements[offset], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(elements[offset + 1], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(elements[offset + 2], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(elements[offset + 3], (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(elements[offset + 4], (IFormatProvider) CultureInfo.InvariantCulture), this.UnEscapeNewlines(elements[offset + 5]));
    }

    protected internal virtual RemoteDebugEventSocketListener.ProxyToken DeserializeToken(
      string[] elements,
      int offset)
    {
      string element1 = elements[offset];
      string element2 = elements[offset + 1];
      string element3 = elements[offset + 2];
      string element4 = elements[offset + 3];
      string element5 = elements[offset + 4];
      string text = this.UnEscapeNewlines(elements[offset + 5]);
      return new RemoteDebugEventSocketListener.ProxyToken(int.Parse(element1, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(element2, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(element3, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(element4, (IFormatProvider) CultureInfo.InvariantCulture), int.Parse(element5, (IFormatProvider) CultureInfo.InvariantCulture), text);
    }

    public virtual void start() => new Thread(new ThreadStart(this.Run)).Start();

    public virtual void Run() => this.EventHandler();

    public virtual string[] GetEventElements(string eventLabel)
    {
      if (eventLabel == null)
        return (string[]) null;
      string[] eventElements = new string[8];
      string str1 = (string) null;
      try
      {
        int length = eventLabel.IndexOf('"');
        if (length >= 0)
        {
          string str2 = eventLabel.Substring(0, length);
          str1 = eventLabel.Substring(length + 1, eventLabel.Length - (length + 1));
          eventLabel = str2;
        }
        string[] strArray = eventLabel.Split('\t');
        int index;
        for (index = 0; index < strArray.Length; ++index)
        {
          if (index >= 8)
            return eventElements;
          eventElements[index] = strArray[index];
        }
        if (str1 != null)
          eventElements[index] = str1;
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.StackTrace);
      }
      return eventElements;
    }

    protected string UnEscapeNewlines(string txt)
    {
      txt = txt.Replace("%0A", "\n");
      txt = txt.Replace("%0D", "\r");
      txt = txt.Replace("%25", "%");
      return txt;
    }

    public bool TokenIndexesAreInvalid => false;

    public class ProxyToken : IToken
    {
      internal int index;
      internal int type;
      internal int channel;
      internal int line;
      internal int charPos;
      internal string text;

      public ProxyToken(int index) => this.index = index;

      public ProxyToken(int index, int type, int channel, int line, int charPos, string text)
      {
        this.index = index;
        this.type = type;
        this.channel = channel;
        this.line = line;
        this.charPos = charPos;
        this.text = text;
      }

      public int Type
      {
        get => this.type;
        set => this.type = value;
      }

      public int Line
      {
        get => this.line;
        set => this.line = value;
      }

      public int CharPositionInLine
      {
        get => this.charPos;
        set => this.charPos = value;
      }

      public int Channel
      {
        get => this.channel;
        set => this.channel = value;
      }

      public int TokenIndex
      {
        get => this.index;
        set => this.index = value;
      }

      public string Text
      {
        get => this.text;
        set => this.text = value;
      }

      public ICharStream InputStream
      {
        get => (ICharStream) null;
        set
        {
        }
      }

      public override string ToString()
      {
        string str = string.Empty;
        if (this.channel != 0)
          str = ",channel=" + (object) this.channel;
        return "[" + this.Text + "/<" + (object) this.type + ">" + str + "," + (object) this.line + ":" + (object) this.CharPositionInLine + ",@" + (object) this.index + "]";
      }
    }

    public class ProxyTree : BaseTree
    {
      public int ID;
      public int type;
      public int line;
      public int charPos = -1;
      public int tokenIndex = -1;
      public string text;

      public ProxyTree(int ID) => this.ID = ID;

      public ProxyTree(int ID, int type, int line, int charPos, int tokenIndex, string text)
      {
        this.ID = ID;
        this.type = type;
        this.line = line;
        this.charPos = charPos;
        this.tokenIndex = tokenIndex;
        this.text = text;
      }

      public override int TokenStartIndex
      {
        get => this.tokenIndex;
        set
        {
        }
      }

      public override int TokenStopIndex
      {
        get => 0;
        set
        {
        }
      }

      public override ITree DupNode() => (ITree) null;

      public override int Type => this.type;

      public override string Text => this.text;

      public override string ToString() => "fix this";
    }
  }
}
