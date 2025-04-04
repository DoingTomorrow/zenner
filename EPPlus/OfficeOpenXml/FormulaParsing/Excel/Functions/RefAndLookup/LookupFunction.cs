// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.LookupFunction
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public abstract class LookupFunction : ExcelFunction
  {
    private readonly ValueMatcher _valueMatcher;
    private readonly CompileResultFactory _compileResultFactory;

    public LookupFunction()
      : this((ValueMatcher) new LookupValueMatcher(), new CompileResultFactory())
    {
    }

    public LookupFunction(ValueMatcher valueMatcher, CompileResultFactory compileResultFactory)
    {
      this._valueMatcher = valueMatcher;
      this._compileResultFactory = compileResultFactory;
    }

    public override bool IsLookupFuction => true;

    protected int IsMatch(object o1, object o2) => this._valueMatcher.IsMatch(o1, o2);

    protected LookupDirection GetLookupDirection(RangeAddress rangeAddress)
    {
      int num = rangeAddress.ToRow - rangeAddress.FromRow;
      return rangeAddress.ToCol - rangeAddress.FromCol <= num ? LookupDirection.Vertical : LookupDirection.Horizontal;
    }

    protected CompileResult Lookup(LookupNavigator navigator, LookupArguments lookupArgs)
    {
      object obj1 = (object) null;
      object obj2 = (object) null;
      int? nullable1 = new int?();
      if (lookupArgs.SearchedValue == null)
        throw new ExcelErrorValueException("Lookupfunction failed to lookup value", ExcelErrorValue.Create(eErrorType.NA));
      do
      {
        int num = this.IsMatch(navigator.CurrentValue, lookupArgs.SearchedValue);
        if (num == 0)
          return this._compileResultFactory.Create(navigator.GetLookupValue());
        if (obj1 == null || navigator.CurrentValue != null)
        {
          if (lookupArgs.RangeLookup)
          {
            if (obj1 == null && num > 0)
              this.ThrowExcelErrorValueException(eErrorType.NA);
            if (obj1 != null && num > 0)
            {
              int? nullable2 = nullable1;
              if ((nullable2.GetValueOrDefault() >= 0 ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                return this._compileResultFactory.Create(obj2);
            }
            nullable1 = new int?(num);
            obj1 = navigator.CurrentValue;
            obj2 = navigator.GetLookupValue();
          }
        }
        else
          break;
      }
      while (navigator.MoveNext());
      if (lookupArgs.RangeLookup)
        return this._compileResultFactory.Create(obj2);
      throw new ExcelErrorValueException("Lookupfunction failed to lookup value", ExcelErrorValue.Create(eErrorType.NA));
    }
  }
}
