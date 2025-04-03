// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ITreeNodeStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

#nullable disable
namespace Antlr.Runtime.Tree
{
  public interface ITreeNodeStream : IIntStream
  {
    object this[int i] { get; }

    object LT(int k);

    object TreeSource { get; }

    ITokenStream TokenStream { get; }

    ITreeAdaptor TreeAdaptor { get; }

    bool UniqueNavigationNodes { get; set; }

    string ToString(object start, object stop);

    void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t);
  }
}
