// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewTypeControlBuilder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.CodeDom;
using System.Collections;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  internal sealed class ViewTypeControlBuilder : ControlBuilder
  {
    private string _typeName;

    public override void Init(
      TemplateParser parser,
      ControlBuilder parentBuilder,
      Type type,
      string tagName,
      string id,
      IDictionary attribs)
    {
      base.Init(parser, parentBuilder, type, tagName, id, attribs);
      this._typeName = (string) attribs[(object) "typename"];
    }

    public override void ProcessGeneratedCode(
      CodeCompileUnit codeCompileUnit,
      CodeTypeDeclaration baseType,
      CodeTypeDeclaration derivedType,
      CodeMemberMethod buildMethod,
      CodeMemberMethod dataBindingMethod)
    {
      derivedType.BaseTypes[0] = new CodeTypeReference(this._typeName);
    }
  }
}
