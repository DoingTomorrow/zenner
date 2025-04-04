// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.FunctionRepository
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class FunctionRepository : IFunctionNameProvider
  {
    private Dictionary<string, ExcelFunction> _functions = new Dictionary<string, ExcelFunction>();

    private FunctionRepository()
    {
    }

    public static FunctionRepository Create()
    {
      FunctionRepository functionRepository = new FunctionRepository();
      functionRepository.LoadModule((IFunctionModule) new BuiltInFunctions());
      return functionRepository;
    }

    public virtual void LoadModule(IFunctionModule module)
    {
      foreach (string key in (IEnumerable<string>) module.Functions.Keys)
        this._functions[key.ToLower()] = module.Functions[key];
    }

    public virtual ExcelFunction GetFunction(string name)
    {
      return this._functions.ContainsKey(name.ToLower()) ? this._functions[name.ToLower()] : throw new ExcelErrorValueException("Non supported function: " + name, ExcelErrorValue.Create(eErrorType.Name));
    }

    public virtual void Clear() => this._functions.Clear();

    public bool IsFunctionName(string name) => this._functions.ContainsKey(name.ToLower());

    public IEnumerable<string> FunctionNames => (IEnumerable<string>) this._functions.Keys;

    public void AddOrReplaceFunction(string functionName, ExcelFunction functionImpl)
    {
      Require.That<string>(functionName).Named(nameof (functionName)).IsNotNullOrEmpty();
      Require.That<ExcelFunction>(functionImpl).Named(nameof (functionImpl)).IsNotNull<ExcelFunction>();
      string lower = functionName.ToLower();
      if (this._functions.ContainsKey(lower))
        this._functions.Remove(lower);
      this._functions[lower] = functionImpl;
    }
  }
}
