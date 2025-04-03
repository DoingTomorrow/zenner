// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeRuleReturnScope`1
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class TreeRuleReturnScope<TTree> : IRuleReturnScope<TTree>, IRuleReturnScope
  {
    private TTree _start;

    public TTree Start
    {
      get => this._start;
      set => this._start = value;
    }

    object IRuleReturnScope.Start => (object) this.Start;

    TTree IRuleReturnScope<TTree>.Stop => default (TTree);

    object IRuleReturnScope.Stop => (object) default (TTree);
  }
}
