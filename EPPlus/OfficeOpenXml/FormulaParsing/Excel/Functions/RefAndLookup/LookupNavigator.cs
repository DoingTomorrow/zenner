// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.LookupNavigator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Utilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public abstract class LookupNavigator
  {
    protected readonly LookupDirection Direction;
    protected readonly LookupArguments Arguments;
    protected readonly ParsingContext ParsingContext;

    public LookupNavigator(
      LookupDirection direction,
      LookupArguments arguments,
      ParsingContext parsingContext)
    {
      Require.That<LookupArguments>(arguments).Named(nameof (arguments)).IsNotNull<LookupArguments>();
      Require.That<ParsingContext>(parsingContext).Named(nameof (parsingContext)).IsNotNull<ParsingContext>();
      Require.That<ExcelDataProvider>(parsingContext.ExcelDataProvider).Named("parsingContext.ExcelDataProvider").IsNotNull<ExcelDataProvider>();
      this.Direction = direction;
      this.Arguments = arguments;
      this.ParsingContext = parsingContext;
    }

    public abstract int Index { get; }

    public abstract bool MoveNext();

    public abstract object CurrentValue { get; }

    public abstract object GetLookupValue();
  }
}
