// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Razor.MvcVBRazorCodeParser
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Linq;
using System.Web.Mvc.Properties;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Razor.Text;
using System.Web.Razor.Tokenizer;
using System.Web.Razor.Tokenizer.Symbols;

#nullable disable
namespace System.Web.Mvc.Razor
{
  public class MvcVBRazorCodeParser : VBCodeParser
  {
    internal const string ModelTypeKeyword = "ModelType";
    private const string GenericTypeFormatString = "{0}(Of {1})";
    private SourceLocation? _endInheritsLocation;
    private bool _modelStatementFound;

    public MvcVBRazorCodeParser()
    {
      this.MapDirective("ModelType", new Func<bool>(this.ModelTypeDirective));
    }

    protected virtual bool InheritsStatement()
    {
      VBSymbol currentSymbol = ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).CurrentSymbol;
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).NextToken();
      this._endInheritsLocation = new SourceLocation?(((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).CurrentLocation);
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).PutCurrentBack();
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).PutBack(currentSymbol);
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).EnsureCurrent();
      bool flag = base.InheritsStatement();
      this.CheckForInheritsAndModelStatements();
      return flag;
    }

    private void CheckForInheritsAndModelStatements()
    {
      if (!this._modelStatementFound || !this._endInheritsLocation.HasValue)
        return;
      ((ParserBase) this).Context.OnError(this._endInheritsLocation.Value, string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.MvcRazorCodeParser_CannotHaveModelAndInheritsKeyword, new object[1]
      {
        (object) "ModelType"
      }));
    }

    protected virtual bool ModelTypeDirective()
    {
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Span.CodeGenerator = SpanCodeGenerator.Null;
      ((ParserBase) this).Context.CurrentBlock.Type = new BlockType?((BlockType) 1);
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).AcceptAndMoveNext();
      SourceLocation currentLocation = ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).CurrentLocation;
      if (((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).At((VBSymbolType) 1))
        ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Span.EditHandler.AcceptedCharacters = (AcceptedCharacters) 0;
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).AcceptWhile((VBSymbolType) 1);
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Output((SpanKind) 1);
      if (this._modelStatementFound)
        ((ParserBase) this).Context.OnError(currentLocation, string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.MvcRazorCodeParser_OnlyOneModelStatementIsAllowed, new object[1]
        {
          (object) "ModelType"
        }));
      this._modelStatementFound = true;
      if (((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).EndOfFile || ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).At((VBSymbolType) 1) || ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).At((VBSymbolType) 2))
        ((ParserBase) this).Context.OnError(currentLocation, MvcResources.MvcRazorCodeParser_ModelKeywordMustBeFollowedByTypeName, new object[1]
        {
          (object) "ModelType"
        });
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).AcceptUntil((VBSymbolType) 2);
      if (!((ParserBase) this).Context.DesignTimeMode)
        ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Optional((VBSymbolType) 2);
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Span.CodeGenerator = (ISpanCodeGenerator) new SetModelTypeCodeGenerator(string.Concat(((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Span.Symbols.Select<ISymbol, string>((Func<ISymbol, string>) (s => s.Content))).Trim(), "{0}(Of {1})");
      this.CheckForInheritsAndModelStatements();
      ((TokenizerBackedParser<VBTokenizer, VBSymbol, VBSymbolType>) this).Output((SpanKind) 3);
      return false;
    }
  }
}
