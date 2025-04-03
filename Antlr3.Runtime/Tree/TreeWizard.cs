// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeWizard
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class TreeWizard
  {
    protected ITreeAdaptor adaptor;
    protected IDictionary<string, int> tokenNameToTypeMap;

    public TreeWizard(ITreeAdaptor adaptor) => this.adaptor = adaptor;

    public TreeWizard(ITreeAdaptor adaptor, IDictionary<string, int> tokenNameToTypeMap)
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
      : this((ITreeAdaptor) new CommonTreeAdaptor(), tokenNames)
    {
    }

    public virtual IDictionary<string, int> ComputeTokenTypes(string[] tokenNames)
    {
      IDictionary<string, int> tokenTypes = (IDictionary<string, int>) new Dictionary<string, int>();
      if (tokenNames == null)
        return tokenTypes;
      for (int index = 4; index < tokenNames.Length; ++index)
      {
        string tokenName = tokenNames[index];
        tokenTypes[tokenName] = index;
      }
      return tokenTypes;
    }

    public virtual int GetTokenType(string tokenName)
    {
      int num;
      return this.tokenNameToTypeMap == null || !this.tokenNameToTypeMap.TryGetValue(tokenName, out num) ? 0 : num;
    }

    public IDictionary<int, IList> Index(object t)
    {
      IDictionary<int, IList> m = (IDictionary<int, IList>) new Dictionary<int, IList>();
      this.IndexCore(t, m);
      return m;
    }

    protected virtual void IndexCore(object t, IDictionary<int, IList> m)
    {
      if (t == null)
        return;
      int type = this.adaptor.GetType(t);
      IList list;
      if (!m.TryGetValue(type, out list) || list == null)
      {
        list = (IList) new List<object>();
        m[type] = list;
      }
      list.Add(t);
      int childCount = this.adaptor.GetChildCount(t);
      for (int i = 0; i < childCount; ++i)
        this.IndexCore(this.adaptor.GetChild(t, i), m);
    }

    public virtual IList Find(object t, int ttype)
    {
      IList nodes = (IList) new List<object>();
      this.Visit(t, ttype, (TreeWizard.IContextVisitor) new TreeWizard.FindTreeWizardVisitor(nodes));
      return nodes;
    }

    public virtual IList Find(object t, string pattern)
    {
      IList subtrees = (IList) new List<object>();
      TreeWizard.TreePattern tpattern = (TreeWizard.TreePattern) new TreePatternParser(new TreePatternLexer(pattern), this, (ITreeAdaptor) new TreeWizard.TreePatternTreeAdaptor()).Pattern();
      if (tpattern == null || tpattern.IsNil || tpattern.GetType() == typeof (TreeWizard.WildcardTreePattern))
        return (IList) null;
      int type = tpattern.Type;
      this.Visit(t, type, (TreeWizard.IContextVisitor) new TreeWizard.FindTreeWizardContextVisitor(this, tpattern, subtrees));
      return subtrees;
    }

    public virtual object FindFirst(object t, int ttype) => (object) null;

    public virtual object FindFirst(object t, string pattern) => (object) null;

    public void Visit(object t, int ttype, TreeWizard.IContextVisitor visitor)
    {
      this.VisitCore(t, (object) null, 0, ttype, visitor);
    }

    public void Visit(object t, int ttype, Action<object> action)
    {
      this.Visit(t, ttype, (TreeWizard.IContextVisitor) new TreeWizard.ActionVisitor(action));
    }

    protected virtual void VisitCore(
      object t,
      object parent,
      int childIndex,
      int ttype,
      TreeWizard.IContextVisitor visitor)
    {
      if (t == null)
        return;
      if (this.adaptor.GetType(t) == ttype)
        visitor.Visit(t, parent, childIndex, (IDictionary<string, object>) null);
      int childCount = this.adaptor.GetChildCount(t);
      for (int index = 0; index < childCount; ++index)
        this.VisitCore(this.adaptor.GetChild(t, index), t, index, ttype, visitor);
    }

    public void Visit(object t, string pattern, TreeWizard.IContextVisitor visitor)
    {
      TreeWizard.TreePattern tpattern = (TreeWizard.TreePattern) new TreePatternParser(new TreePatternLexer(pattern), this, (ITreeAdaptor) new TreeWizard.TreePatternTreeAdaptor()).Pattern();
      if (tpattern == null || tpattern.IsNil || tpattern.GetType() == typeof (TreeWizard.WildcardTreePattern))
        return;
      IDictionary<string, object> labels = (IDictionary<string, object>) new Dictionary<string, object>();
      int type = tpattern.Type;
      this.Visit(t, type, (TreeWizard.IContextVisitor) new TreeWizard.VisitTreeWizardContextVisitor(this, visitor, labels, tpattern));
    }

    public bool Parse(object t, string pattern, IDictionary<string, object> labels)
    {
      TreeWizard.TreePattern tpattern = (TreeWizard.TreePattern) new TreePatternParser(new TreePatternLexer(pattern), this, (ITreeAdaptor) new TreeWizard.TreePatternTreeAdaptor()).Pattern();
      return this.ParseCore(t, tpattern, labels);
    }

    public bool Parse(object t, string pattern)
    {
      return this.Parse(t, pattern, (IDictionary<string, object>) null);
    }

    protected virtual bool ParseCore(
      object t1,
      TreeWizard.TreePattern tpattern,
      IDictionary<string, object> labels)
    {
      if (t1 == null || tpattern == null || tpattern.GetType() != typeof (TreeWizard.WildcardTreePattern) && (this.adaptor.GetType(t1) != tpattern.Type || tpattern.hasTextArg && !this.adaptor.GetText(t1).Equals(tpattern.Text)))
        return false;
      if (tpattern.label != null && labels != null)
        labels[tpattern.label] = t1;
      int childCount1 = this.adaptor.GetChildCount(t1);
      int childCount2 = tpattern.ChildCount;
      if (childCount1 != childCount2)
        return false;
      for (int i = 0; i < childCount1; ++i)
      {
        if (!this.ParseCore(this.adaptor.GetChild(t1, i), (TreeWizard.TreePattern) tpattern.GetChild(i), labels))
          return false;
      }
      return true;
    }

    public virtual object Create(string pattern)
    {
      return new TreePatternParser(new TreePatternLexer(pattern), this, this.adaptor).Pattern();
    }

    public static bool Equals(object t1, object t2, ITreeAdaptor adaptor)
    {
      return TreeWizard.EqualsCore(t1, t2, adaptor);
    }

    public bool Equals(object t1, object t2) => TreeWizard.EqualsCore(t1, t2, this.adaptor);

    protected static bool EqualsCore(object t1, object t2, ITreeAdaptor adaptor)
    {
      if (t1 == null || t2 == null || adaptor.GetType(t1) != adaptor.GetType(t2) || !adaptor.GetText(t1).Equals(adaptor.GetText(t2)))
        return false;
      int childCount1 = adaptor.GetChildCount(t1);
      int childCount2 = adaptor.GetChildCount(t2);
      if (childCount1 != childCount2)
        return false;
      for (int i = 0; i < childCount1; ++i)
      {
        if (!TreeWizard.EqualsCore(adaptor.GetChild(t1, i), adaptor.GetChild(t2, i), adaptor))
          return false;
      }
      return true;
    }

    public interface IContextVisitor
    {
      void Visit(object t, object parent, int childIndex, IDictionary<string, object> labels);
    }

    public abstract class Visitor : TreeWizard.IContextVisitor
    {
      public virtual void Visit(
        object t,
        object parent,
        int childIndex,
        IDictionary<string, object> labels)
      {
        this.Visit(t);
      }

      public abstract void Visit(object t);
    }

    private class ActionVisitor : TreeWizard.Visitor
    {
      private Action<object> _action;

      public ActionVisitor(Action<object> action) => this._action = action;

      public override void Visit(object t) => this._action(t);
    }

    public class TreePattern(IToken payload) : CommonTree(payload)
    {
      public string label;
      public bool hasTextArg;

      public override string ToString()
      {
        return this.label != null ? "%" + this.label + ":" : base.ToString();
      }
    }

    public class WildcardTreePattern(IToken payload) : TreeWizard.TreePattern(payload)
    {
    }

    public class TreePatternTreeAdaptor : CommonTreeAdaptor
    {
      public override object Create(IToken payload) => (object) new TreeWizard.TreePattern(payload);
    }

    private class FindTreeWizardVisitor : TreeWizard.Visitor
    {
      private IList _nodes;

      public FindTreeWizardVisitor(IList nodes) => this._nodes = nodes;

      public override void Visit(object t) => this._nodes.Add(t);
    }

    private class FindTreeWizardContextVisitor : TreeWizard.IContextVisitor
    {
      private TreeWizard _outer;
      private TreeWizard.TreePattern _tpattern;
      private IList _subtrees;

      public FindTreeWizardContextVisitor(
        TreeWizard outer,
        TreeWizard.TreePattern tpattern,
        IList subtrees)
      {
        this._outer = outer;
        this._tpattern = tpattern;
        this._subtrees = subtrees;
      }

      public void Visit(
        object t,
        object parent,
        int childIndex,
        IDictionary<string, object> labels)
      {
        if (!this._outer.ParseCore(t, this._tpattern, (IDictionary<string, object>) null))
          return;
        this._subtrees.Add(t);
      }
    }

    private class VisitTreeWizardContextVisitor : TreeWizard.IContextVisitor
    {
      private TreeWizard _outer;
      private TreeWizard.IContextVisitor _visitor;
      private IDictionary<string, object> _labels;
      private TreeWizard.TreePattern _tpattern;

      public VisitTreeWizardContextVisitor(
        TreeWizard outer,
        TreeWizard.IContextVisitor visitor,
        IDictionary<string, object> labels,
        TreeWizard.TreePattern tpattern)
      {
        this._outer = outer;
        this._visitor = visitor;
        this._labels = labels;
        this._tpattern = tpattern;
      }

      public void Visit(
        object t,
        object parent,
        int childIndex,
        IDictionary<string, object> unusedlabels)
      {
        this._labels.Clear();
        if (!this._outer.ParseCore(t, this._tpattern, this._labels))
          return;
        this._visitor.Visit(t, parent, childIndex, this._labels);
      }
    }
  }
}
