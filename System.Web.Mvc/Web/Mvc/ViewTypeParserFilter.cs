// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewTypeParserFilter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Web.UI;

#nullable disable
namespace System.Web.Mvc
{
  internal class ViewTypeParserFilter : PageParserFilter
  {
    private static Dictionary<string, Type> _directiveBaseTypeMappings = new Dictionary<string, Type>()
    {
      {
        "page",
        typeof (ViewPage)
      },
      {
        "control",
        typeof (ViewUserControl)
      },
      {
        "master",
        typeof (ViewMasterPage)
      }
    };
    private string _inherits;

    public override bool AllowCode => true;

    public override int NumberOfControlsAllowed => -1;

    public override int NumberOfDirectDependenciesAllowed => -1;

    public override int TotalNumberOfDependenciesAllowed => -1;

    public override void PreprocessDirective(string directiveName, IDictionary attributes)
    {
      base.PreprocessDirective(directiveName, attributes);
      Type type;
      if (!ViewTypeParserFilter._directiveBaseTypeMappings.TryGetValue(directiveName, out type) || !(attributes[(object) "inherits"] is string attribute))
        return;
      if (attribute.IndexOfAny(new char[2]{ '<', '(' }) <= 0)
        return;
      attributes[(object) "inherits"] = (object) type.FullName;
      this._inherits = attribute;
    }

    public override void ParseComplete(ControlBuilder rootBuilder)
    {
      base.ParseComplete(rootBuilder);
      if (!(rootBuilder is IMvcControlBuilder mvcControlBuilder))
        return;
      mvcControlBuilder.Inherits = this._inherits;
    }

    public override bool AllowBaseType(Type baseType) => true;

    public override bool AllowControl(Type controlType, ControlBuilder builder) => true;

    public override bool AllowVirtualReference(
      string referenceVirtualPath,
      VirtualReferenceType referenceType)
    {
      return true;
    }

    public override bool AllowServerSideInclude(string includeVirtualPath) => true;
  }
}
