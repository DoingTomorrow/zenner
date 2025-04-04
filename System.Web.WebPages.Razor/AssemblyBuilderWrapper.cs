// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Razor.AssemblyBuilderWrapper
// Assembly: System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: C1ADC2BA-BB9B-44AC-BD15-1738CDE0D481
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Razor.dll

using System.CodeDom;
using System.Web.Compilation;

#nullable disable
namespace System.Web.WebPages.Razor
{
  internal sealed class AssemblyBuilderWrapper : IAssemblyBuilder
  {
    public AssemblyBuilderWrapper(AssemblyBuilder builder)
    {
      this.InnerBuilder = builder != null ? builder : throw new ArgumentNullException(nameof (builder));
    }

    internal AssemblyBuilder InnerBuilder { get; set; }

    public void AddCodeCompileUnit(BuildProvider buildProvider, CodeCompileUnit compileUnit)
    {
      this.InnerBuilder.AddCodeCompileUnit(buildProvider, compileUnit);
    }

    public void GenerateTypeFactory(string typeName)
    {
      this.InnerBuilder.GenerateTypeFactory(typeName);
    }
  }
}
