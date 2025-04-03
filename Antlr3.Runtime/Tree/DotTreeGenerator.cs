// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.DotTreeGenerator
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class DotTreeGenerator
  {
    private const string Footer = "}";
    private const string NodeFormat = "  {0} [label=\"{1}\"];";
    private const string EdgeFormat = "  {0} -> {1} // \"{2}\" -> \"{3}\"";
    private readonly string[] HeaderLines = new string[8]
    {
      "digraph {",
      "",
      "\tordering=out;",
      "\tranksep=.4;",
      "\tbgcolor=\"lightgrey\"; node [shape=box, fixedsize=false, fontsize=12, fontname=\"Helvetica-bold\", fontcolor=\"blue\"",
      "\t\twidth=.25, height=.25, color=\"black\", fillcolor=\"white\", style=\"filled, solid, bold\"];",
      "\tedge [arrowsize=.5, color=\"black\", style=\"bold\"]",
      ""
    };
    private Dictionary<object, int> nodeToNumberMap = new Dictionary<object, int>();
    private int nodeNumber;

    public virtual string ToDot(object tree, ITreeAdaptor adaptor)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string headerLine in this.HeaderLines)
        stringBuilder.AppendLine(headerLine);
      this.nodeNumber = 0;
      IEnumerable<string> strings1 = this.DefineNodes(tree, adaptor);
      this.nodeNumber = 0;
      IEnumerable<string> strings2 = this.DefineEdges(tree, adaptor);
      foreach (string str in strings1)
        stringBuilder.AppendLine(str);
      stringBuilder.AppendLine();
      foreach (string str in strings2)
        stringBuilder.AppendLine(str);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("}");
      return stringBuilder.ToString();
    }

    public virtual string ToDot(ITree tree)
    {
      return this.ToDot((object) tree, (ITreeAdaptor) new CommonTreeAdaptor());
    }

    protected virtual IEnumerable<string> DefineNodes(object tree, ITreeAdaptor adaptor)
    {
      if (tree != null)
      {
        int n = adaptor.GetChildCount(tree);
        if (n != 0)
        {
          yield return this.GetNodeText(adaptor, tree);
          for (int i = 0; i < n; ++i)
          {
            object child = adaptor.GetChild(tree, i);
            yield return this.GetNodeText(adaptor, child);
            foreach (string t in this.DefineNodes(child, adaptor))
              yield return t;
          }
        }
      }
    }

    protected virtual IEnumerable<string> DefineEdges(object tree, ITreeAdaptor adaptor)
    {
      if (tree != null)
      {
        int n = adaptor.GetChildCount(tree);
        if (n != 0)
        {
          string parentName = "n" + (object) this.GetNodeNumber(tree);
          string parentText = adaptor.GetText(tree);
          for (int i = 0; i < n; ++i)
          {
            object child = adaptor.GetChild(tree, i);
            string childText = adaptor.GetText(child);
            string childName = "n" + (object) this.GetNodeNumber(child);
            yield return string.Format("  {0} -> {1} // \"{2}\" -> \"{3}\"", (object) parentName, (object) childName, (object) this.FixString(parentText), (object) this.FixString(childText));
            foreach (string t in this.DefineEdges(child, adaptor))
              yield return t;
          }
        }
      }
    }

    protected virtual string GetNodeText(ITreeAdaptor adaptor, object t)
    {
      string text = adaptor.GetText(t);
      return string.Format("  {0} [label=\"{1}\"];", (object) ("n" + (object) this.GetNodeNumber(t)), (object) this.FixString(text));
    }

    protected virtual int GetNodeNumber(object t)
    {
      int nodeNumber;
      if (this.nodeToNumberMap.TryGetValue(t, out nodeNumber))
        return nodeNumber;
      this.nodeToNumberMap[t] = this.nodeNumber;
      ++this.nodeNumber;
      return this.nodeNumber - 1;
    }

    protected virtual string FixString(string text)
    {
      if (text != null)
      {
        text = Regex.Replace(text, "\"", "\\\\\"");
        text = Regex.Replace(text, "\\t", "    ");
        text = Regex.Replace(text, "\\n", "\\\\n");
        text = Regex.Replace(text, "\\r", "\\\\r");
        if (text.Length > 20)
          text = text.Substring(0, 8) + "..." + text.Substring(text.Length - 8);
      }
      return text;
    }
  }
}
