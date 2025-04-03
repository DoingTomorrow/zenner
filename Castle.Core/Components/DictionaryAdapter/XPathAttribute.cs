// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
  public class XPathAttribute : Attribute
  {
    private string expression;

    public XPathAttribute(string expression) => this.Expression = expression;

    public string Expression
    {
      get => this.expression;
      private set
      {
        this.expression = value;
        this.CompiledExpression = XPathExpression.Compile(this.expression);
      }
    }

    public XPathExpression CompiledExpression { get; private set; }
  }
}
