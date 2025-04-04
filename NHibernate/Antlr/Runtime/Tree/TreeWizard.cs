// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeWizard
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class TreeWizard
  {
    protected ITreeAdaptor adaptor;
    protected IDictionary tokenNameToTypeMap;

    public TreeWizard(ITreeAdaptor adaptor) => this.adaptor = adaptor;

    public TreeWizard(ITreeAdaptor adaptor, IDictionary tokenNameToTypeMap)
    {
      this.adaptor = adaptor;
      this.tokenNameToTypeMap = tokenNameToTypeMap;
    }

    public TreeWizard(ITreeAdaptor adaptor, string[] tokenNames)
    {
      this.adaptor = adaptor;
      this.tokenNameToTypeMap = this.ComputeTokenTypes(tokenNames);
    }

    public TreeWizard(string[] tokenNames)
      : this((ITreeAdaptor) null, tokenNames)
    {
    }

    public IDictionary ComputeTokenTypes(string[] tokenNames)
    {
      IDictionary tokenTypes = (IDictionary) new Hashtable();
      if (tokenNames == null)
        return tokenTypes;
      for (int minTokenType = Token.MIN_TOKEN_TYPE; minTokenType < tokenNames.Length; ++minTokenType)
      {
        string tokenName = tokenNames[minTokenType];
        tokenTypes.Add((object) tokenName, (object) minTokenType);
      }
      return tokenTypes;
    }

    public int GetTokenType(string tokenName)
    {
      if (this.tokenNameToTypeMap == null)
        return 0;
      object tokenNameToType = this.tokenNameToTypeMap[(object) tokenName];
      return tokenNameToType != null ? (int) tokenNameToType : 0;
    }

    public IDictionary Index(object t)
    {
      IDictionary m = (IDictionary) new Hashtable();
      this._Index(t, m);
      return m;
    }

    protected void _Index(object t, IDictionary m)
    {
      if (t == null)
        return;
      int nodeType = this.adaptor.GetNodeType(t);
      if (!(m[(object) nodeType] is IList list))
      {
        list = (IList) new ArrayList();
        m[(object) nodeType] = (object) list;
      }
      list.Add(t);
      int childCount = this.adaptor.GetChildCount(t);
      for (int i = 0; i < childCount; ++i)
        this._Index(this.adaptor.GetChild(t, i), m);
    }

    public IList Find(object t, int ttype)
    {
      IList list = (IList) new ArrayList();
      this.Visit(t, ttype, (TreeWizard.ContextVisitor) new TreeWizard.RecordAllElementsVisitor(list));
      return list;
    }

    public IList Find(object t, string pattern)
    {
      IList list = (IList) new ArrayList();
      TreeWizard.TreePattern pattern1 = (TreeWizard.TreePattern) new TreePatternParser(new TreePatternLexer(pattern), this, (ITreeAdaptor) new TreeWizard.TreePatternTreeAdaptor()).Pattern();
      if (pattern1 == null || pattern1.IsNil || pattern1.GetType() == typeof (TreeWizard.WildcardTreePattern))
        return (IList) null;
      int type = pattern1.Type;
      this.Visit(t, type, (TreeWizard.ContextVisitor) new TreeWizard.PatternMatchingContextVisitor(this, pattern1, list));
      return list;
    }

    public object FindFirst(object t, int ttype) => (object) null;

    public object FindFirst(object t, string pattern) => (object) null;

    public void Visit(object t, int ttype, TreeWizard.ContextVisitor visitor)
    {
      this._Visit(t, (object) null, 0, ttype, visitor);
    }

    protected void _Visit(
      object t,
      object parent,
      int childIndex,
      int ttype,
      TreeWizard.ContextVisitor visitor)
    {
      if (t == null)
        return;
      if (this.adaptor.GetNodeType(t) == ttype)
        visitor.Visit(t, parent, childIndex, (IDictionary) null);
      int childCount = this.adaptor.GetChildCount(t);
      for (int index = 0; index < childCount; ++index)
        this._Visit(this.adaptor.GetChild(t, index), t, index, ttype, visitor);
    }

    public void Visit(object t, string pattern, TreeWizard.ContextVisitor visitor)
    {
      TreeWizard.TreePattern pattern1 = (TreeWizard.TreePattern) new TreePatternParser(new TreePatternLexer(pattern), this, (ITreeAdaptor) new TreeWizard.TreePatternTreeAdaptor()).Pattern();
      if (pattern1 == null || pattern1.IsNil || pattern1.GetType() == typeof (TreeWizard.WildcardTreePattern))
        return;
      int type = pattern1.Type;
      this.Visit(t, type, (TreeWizard.ContextVisitor) new TreeWizard.InvokeVisitorOnPatternMatchContextVisitor(this, pattern1, visitor));
    }

    public bool Parse(object t, string pattern, IDictionary labels)
    {
      TreeWizard.TreePattern t2 = (TreeWizard.TreePattern) new TreePatternParser(new TreePatternLexer(pattern), this, (ITreeAdaptor) new TreeWizard.TreePatternTreeAdaptor()).Pattern();
      return this._Parse(t, t2, labels);
    }

    public bool Parse(object t, string pattern) => this.Parse(t, pattern, (IDictionary) null);

    protected bool _Parse(object t1, TreeWizard.TreePattern t2, IDictionary labels)
    {
      if (t1 == null || t2 == null || t2.GetType() != typeof (TreeWizard.WildcardTreePattern) && (this.adaptor.GetNodeType(t1) != t2.Type || t2.hasTextArg && !this.adaptor.GetNodeText(t1).Equals(t2.Text)))
        return false;
      if (t2.label != null && labels != null)
        labels[(object) t2.label] = t1;
      int childCount1 = this.adaptor.GetChildCount(t1);
      int childCount2 = t2.ChildCount;
      if (childCount1 != childCount2)
        return false;
      for (int i = 0; i < childCount1; ++i)
      {
        if (!this._Parse(this.adaptor.GetChild(t1, i), (TreeWizard.TreePattern) t2.GetChild(i), labels))
          return false;
      }
      return true;
    }

    public object Create(string pattern)
    {
      return new TreePatternParser(new TreePatternLexer(pattern), this, this.adaptor).Pattern();
    }

    public static bool Equals(object t1, object t2, ITreeAdaptor adaptor)
    {
      return TreeWizard._Equals(t1, t2, adaptor);
    }

    public bool Equals(object t1, object t2) => TreeWizard._Equals(t1, t2, this.adaptor);

    protected static bool _Equals(object t1, object t2, ITreeAdaptor adaptor)
    {
      if (t1 == null || t2 == null || adaptor.GetNodeType(t1) != adaptor.GetNodeType(t2) || !adaptor.GetNodeText(t1).Equals(adaptor.GetNodeText(t2)))
        return false;
      int childCount1 = adaptor.GetChildCount(t1);
      int childCount2 = adaptor.GetChildCount(t2);
      if (childCount1 != childCount2)
        return false;
      for (int i = 0; i < childCount1; ++i)
      {
        if (!TreeWizard._Equals(adaptor.GetChild(t1, i), adaptor.GetChild(t2, i), adaptor))
          return false;
      }
      return true;
    }

    public interface ContextVisitor
    {
      void Visit(object t, object parent, int childIndex, IDictionary labels);
    }

    public abstract class Visitor : TreeWizard.ContextVisitor
    {
      public void Visit(object t, object parent, int childIndex, IDictionary labels)
      {
        this.Visit(t);
      }

      public abstract void Visit(object t);
    }

    private sealed class RecordAllElementsVisitor : TreeWizard.Visitor
    {
      private IList list;

      public RecordAllElementsVisitor(IList list) => this.list = list;

      public override void Visit(object t) => this.list.Add(t);
    }

    private sealed class PatternMatchingContextVisitor : TreeWizard.ContextVisitor
    {
      private TreeWizard owner;
      private TreeWizard.TreePattern pattern;
      private IList list;

      public PatternMatchingContextVisitor(
        TreeWizard owner,
        TreeWizard.TreePattern pattern,
        IList list)
      {
        this.owner = owner;
        this.pattern = pattern;
        this.list = list;
      }

      public void Visit(object t, object parent, int childIndex, IDictionary labels)
      {
        if (!this.owner._Parse(t, this.pattern, (IDictionary) null))
          return;
        this.list.Add(t);
      }
    }

    private sealed class InvokeVisitorOnPatternMatchContextVisitor : TreeWizard.ContextVisitor
    {
      private TreeWizard owner;
      private TreeWizard.TreePattern pattern;
      private TreeWizard.ContextVisitor visitor;
      private Hashtable labels = new Hashtable();

      public InvokeVisitorOnPatternMatchContextVisitor(
        TreeWizard owner,
        TreeWizard.TreePattern pattern,
        TreeWizard.ContextVisitor visitor)
      {
        this.owner = owner;
        this.pattern = pattern;
        this.visitor = visitor;
      }

      public void Visit(object t, object parent, int childIndex, IDictionary unusedlabels)
      {
        this.labels.Clear();
        if (!this.owner._Parse(t, this.pattern, (IDictionary) this.labels))
          return;
        this.visitor.Visit(t, parent, childIndex, (IDictionary) this.labels);
      }
    }

    public class TreePattern(IToken payload) : CommonTree(payload)
    {
      public string label;
      public bool hasTextArg;

      public override string ToString()
      {
        return this.label != null ? "%" + this.label + ":" + base.ToString() : base.ToString();
      }
    }

    public class WildcardTreePattern(IToken payload) : TreeWizard.TreePattern(payload)
    {
    }

    public class TreePatternTreeAdaptor : CommonTreeAdaptor
    {
      public override object Create(IToken payload) => (object) new TreeWizard.TreePattern(payload);
    }
  }
}
