// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Razor.MvcCSharpRazorCodeGenerator
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.CodeDom;
using System.Web.Razor;
using System.Web.Razor.Generator;

#nullable disable
namespace System.Web.Mvc.Razor
{
  internal class MvcCSharpRazorCodeGenerator : CSharpRazorCodeGenerator
  {
    private const string DefaultModelTypeName = "dynamic";

    public MvcCSharpRazorCodeGenerator(
      string className,
      string rootNamespaceName,
      string sourceFileName,
      RazorEngineHost host)
      : base(className, rootNamespaceName, sourceFileName, host)
    {
      if (!(host is MvcWebPageRazorHost webPageRazorHost) || webPageRazorHost.IsSpecialPage)
        return;
      this.SetBaseType("dynamic");
    }

    private void SetBaseType(string modelTypeName)
    {
      CodeTypeReference codeTypeReference = new CodeTypeReference(((RazorCodeGenerator) this).Context.Host.DefaultBaseClass + "<" + modelTypeName + ">");
      ((RazorCodeGenerator) this).Context.GeneratedClass.BaseTypes.Clear();
      ((RazorCodeGenerator) this).Context.GeneratedClass.BaseTypes.Add(codeTypeReference);
    }
  }
}
