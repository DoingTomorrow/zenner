// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.ArrayLookupNavigator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class ArrayLookupNavigator : LookupNavigator
  {
    private readonly FunctionArgument[] _arrayData;
    private int _index;
    private object _currentValue;

    public ArrayLookupNavigator(
      LookupDirection direction,
      LookupArguments arguments,
      ParsingContext parsingContext)
      : base(direction, arguments, parsingContext)
    {
      Require.That<LookupArguments>(arguments).Named(nameof (arguments)).IsNotNull<LookupArguments>();
      Require.That<IEnumerable<FunctionArgument>>(arguments.DataArray).Named("arguments.DataArray").IsNotNull<IEnumerable<FunctionArgument>>();
      this._arrayData = arguments.DataArray.ToArray<FunctionArgument>();
      this.Initialize();
    }

    private void Initialize()
    {
      if (this.Arguments.LookupIndex >= this._arrayData.Length)
        throw new ExcelErrorValueException(eErrorType.Ref);
      this.SetCurrentValue();
    }

    public override int Index => this._index;

    private void SetCurrentValue() => this._currentValue = (object) this._arrayData[this._index];

    private bool HasNext()
    {
      return this.Direction == LookupDirection.Vertical && this._index < this._arrayData.Length - 1;
    }

    public override bool MoveNext()
    {
      if (!this.HasNext())
        return false;
      if (this.Direction == LookupDirection.Vertical)
        ++this._index;
      this.SetCurrentValue();
      return true;
    }

    public override object CurrentValue => this._arrayData[this._index].Value;

    public override object GetLookupValue() => this._arrayData[this._index].Value;
  }
}
