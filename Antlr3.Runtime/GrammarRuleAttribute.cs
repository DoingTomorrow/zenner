// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.GrammarRuleAttribute
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public sealed class GrammarRuleAttribute : Attribute
  {
    private readonly string _name;

    public GrammarRuleAttribute(string name) => this._name = name;

    public string Name => this._name;
  }
}
