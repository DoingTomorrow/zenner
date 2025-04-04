// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Razor.MvcCSharpRazorCodeParser
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Mvc.Properties;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.Razor.Text;
using System.Web.Razor.Tokenizer;
using System.Web.Razor.Tokenizer.Symbols;

#nullable disable
namespace System.Web.Mvc.Razor
{
  public class MvcCSharpRazorCodeParser : CSharpCodeParser
  {
    private const string ModelKeyword = "model";
    private const string GenericTypeFormatString = "{0}<{1}>";
    private SourceLocation? _endInheritsLocation;
    private bool _modelStatementFound;

    public MvcCSharpRazorCodeParser()
    {
      this.MapDirectives(new Action(this.ModelDirective), new string[1]
      {
        "model"
      });
    }

    protected virtual void InheritsDirective()
    {
      ((TokenizerBackedParser<CSharpTokenizer, CSharpSymbol, CSharpSymbolType>) this).AcceptAndMoveNext();
      this._endInheritsLocation = new SourceLocation?(((TokenizerBackedParser<CSharpTokenizer, CSharpSymbol, CSharpSymbolType>) this).CurrentLocation);
      this.InheritsDirectiveCore();
      this.CheckForInheritsAndModelStatements();
    }

    private void CheckForInheritsAndModelStatements()
    {
      if (!this._modelStatementFound || !this._endInheritsLocation.HasValue)
        return;
      ((ParserBase) this).Context.OnError(this._endInheritsLocation.Value, string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.MvcRazorCodeParser_CannotHaveModelAndInheritsKeyword, new object[1]
      {
        (object) "model"
      }));
    }

    protected virtual void ModelDirective()
    {
      ((TokenizerBackedParser<CSharpTokenizer, CSharpSymbol, CSharpSymbolType>) this).AcceptAndMoveNext();
      SourceLocation currentLocation = ((TokenizerBackedParser<CSharpTokenizer, CSharpSymbol, CSharpSymbolType>) this).CurrentLocation;
      this.BaseTypeDirective(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.MvcRazorCodeParser_ModelKeywordMustBeFollowedByTypeName, new object[1]
      {
        (object) "model"
      }), new Func<string, SpanCodeGenerator>(this.CreateModelCodeGenerator));
      if (this._modelStatementFound)
        ((ParserBase) this).Context.OnError(currentLocation, string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.MvcRazorCodeParser_OnlyOneModelStatementIsAllowed, new object[1]
        {
          (object) "model"
        }));
      this._modelStatementFound = true;
      this.CheckForInheritsAndModelStatements();
    }

    private SpanCodeGenerator CreateModelCodeGenerator(string model)
    {
      return (SpanCodeGenerator) new SetModelTypeCodeGenerator(model, "{0}<{1}>");
    }
  }
}
