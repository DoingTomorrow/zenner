// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ITreeNodeStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal interface ITreeNodeStream : IIntStream
  {
    object Get(int i);

    object TreeSource { get; }

    ITokenStream TokenStream { get; }

    ITreeAdaptor TreeAdaptor { get; }

    bool HasUniqueNavigationNodes { set; }

    object LT(int k);

    string ToString(object start, object stop);

    void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t);
  }
}
