// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonTreeAdaptor
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class CommonTreeAdaptor : BaseTreeAdaptor
  {
    public override object Create(IToken payload) => (object) new CommonTree(payload);

    public override IToken CreateToken(int tokenType, string text)
    {
      return (IToken) new CommonToken(tokenType, text);
    }

    public override IToken CreateToken(IToken fromToken) => (IToken) new CommonToken(fromToken);

    public override IToken GetToken(object t)
    {
      return t is CommonTree ? ((CommonTree) t).Token : (IToken) null;
    }
  }
}
