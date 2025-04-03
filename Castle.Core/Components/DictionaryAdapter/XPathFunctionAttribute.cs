// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathFunctionAttribute
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Xml.Xsl;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
  public class XPathFunctionAttribute : Attribute
  {
    protected XPathFunctionAttribute(string name)
    {
      this.Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentException("Name cannot be empty", nameof (name));
    }

    public XPathFunctionAttribute(string name, Type functionType)
      : this(name)
    {
      if (!typeof (IXsltContextFunction).IsAssignableFrom(functionType))
        throw new ArgumentException("The functionType does not implement IXsltContextFunction");
      this.Function = functionType.GetConstructor(Type.EmptyTypes) != null ? (IXsltContextFunction) Activator.CreateInstance(functionType) : throw new ArgumentException("The functionType does not have a parameterless constructor");
    }

    public string Name { get; private set; }

    public IXsltContextFunction Function { get; protected set; }

    public string Prefix { get; set; }
  }
}
