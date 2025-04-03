// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDelegateFunction
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  public class SQLiteDelegateFunction : SQLiteFunction
  {
    private const string NoCallbackError = "No \"{0}\" callback is set.";
    private const string ResultInt32Error = "\"{0}\" result must be Int32.";
    private Delegate callback1;
    private Delegate callback2;

    public SQLiteDelegateFunction()
      : this((Delegate) null, (Delegate) null)
    {
    }

    public SQLiteDelegateFunction(Delegate callback1, Delegate callback2)
    {
      this.callback1 = callback1;
      this.callback2 = callback2;
    }

    protected virtual object[] GetInvokeArgs(object[] args, bool earlyBound)
    {
      object[] invokeArgs = new object[2]
      {
        (object) "Invoke",
        (object) args
      };
      if (!earlyBound)
        invokeArgs = new object[1]{ (object) invokeArgs };
      return invokeArgs;
    }

    protected virtual object[] GetStepArgs(
      object[] args,
      int stepNumber,
      object contextData,
      bool earlyBound)
    {
      object[] stepArgs = new object[4]
      {
        (object) "Step",
        (object) args,
        (object) stepNumber,
        contextData
      };
      if (!earlyBound)
        stepArgs = new object[1]{ (object) stepArgs };
      return stepArgs;
    }

    protected virtual void UpdateStepArgs(object[] args, ref object contextData, bool earlyBound)
    {
      object[] objArray = !earlyBound ? args[0] as object[] : args;
      if (objArray == null)
        return;
      contextData = objArray[objArray.Length - 1];
    }

    protected virtual object[] GetFinalArgs(object contextData, bool earlyBound)
    {
      object[] finalArgs = new object[2]
      {
        (object) "Final",
        contextData
      };
      if (!earlyBound)
        finalArgs = new object[1]{ (object) finalArgs };
      return finalArgs;
    }

    protected virtual object[] GetCompareArgs(string param1, string param2, bool earlyBound)
    {
      object[] compareArgs = new object[3]
      {
        (object) "Compare",
        (object) param1,
        (object) param2
      };
      if (!earlyBound)
        compareArgs = new object[1]{ (object) compareArgs };
      return compareArgs;
    }

    public virtual Delegate Callback1
    {
      get => this.callback1;
      set => this.callback1 = value;
    }

    public virtual Delegate Callback2
    {
      get => this.callback2;
      set => this.callback2 = value;
    }

    public override object Invoke(object[] args)
    {
      if ((object) this.callback1 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Invoke)));
      return this.callback1 is SQLiteInvokeDelegate callback1 ? callback1(nameof (Invoke), args) : this.callback1.DynamicInvoke(this.GetInvokeArgs(args, false));
    }

    public override void Step(object[] args, int stepNumber, ref object contextData)
    {
      if ((object) this.callback1 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Step)));
      if (this.callback1 is SQLiteStepDelegate callback1)
      {
        callback1(nameof (Step), args, stepNumber, ref contextData);
      }
      else
      {
        object[] stepArgs = this.GetStepArgs(args, stepNumber, contextData, false);
        this.callback1.DynamicInvoke(stepArgs);
        this.UpdateStepArgs(stepArgs, ref contextData, false);
      }
    }

    public override object Final(object contextData)
    {
      if ((object) this.callback2 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Final)));
      return this.callback2 is SQLiteFinalDelegate callback2 ? callback2(nameof (Final), contextData) : this.callback1.DynamicInvoke(this.GetFinalArgs(contextData, false));
    }

    public override int Compare(string param1, string param2)
    {
      if ((object) this.callback1 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Compare)));
      if (this.callback1 is SQLiteCompareDelegate callback1)
        return callback1(nameof (Compare), param1, param2);
      if (this.callback1.DynamicInvoke(this.GetCompareArgs(param1, param2, false)) is int num)
        return num;
      throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "\"{0}\" result must be Int32.", (object) nameof (Compare)));
    }
  }
}
