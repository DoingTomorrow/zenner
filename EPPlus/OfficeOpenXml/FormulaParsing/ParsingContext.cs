// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ParsingContext
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class ParsingContext : IParsingLifetimeEventHandler
  {
    private ParsingContext()
    {
    }

    public FormulaParser Parser { get; set; }

    public ExcelDataProvider ExcelDataProvider { get; set; }

    public RangeAddressFactory RangeAddressFactory { get; set; }

    public INameValueProvider NameValueProvider { get; set; }

    public ParsingConfiguration Configuration { get; set; }

    public ParsingScopes Scopes { get; private set; }

    public static ParsingContext Create()
    {
      ParsingContext parsingContext = new ParsingContext()
      {
        Configuration = ParsingConfiguration.Create()
      };
      parsingContext.Scopes = new ParsingScopes((IParsingLifetimeEventHandler) parsingContext);
      return parsingContext;
    }

    void IParsingLifetimeEventHandler.ParsingCompleted()
    {
    }
  }
}
