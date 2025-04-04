// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewPageControlBuilder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.CodeDom;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class ViewPageControlBuilder : FileLevelPageControlBuilder, IMvcControlBuilder
  {
    public string Inherits { get; set; }

    public override void ProcessGeneratedCode(
      CodeCompileUnit codeCompileUnit,
      CodeTypeDeclaration baseType,
      CodeTypeDeclaration derivedType,
      CodeMemberMethod buildMethod,
      CodeMemberMethod dataBindingMethod)
    {
      if (string.IsNullOrWhiteSpace(this.Inherits))
        return;
      derivedType.BaseTypes[0] = new CodeTypeReference(this.Inherits);
    }
  }
}
