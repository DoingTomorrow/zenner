// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFunction
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace System.Data.SQLite
{
  public abstract class SQLiteFunction : IDisposable
  {
    internal SQLiteBase _base;
    private Dictionary<IntPtr, SQLiteFunction.AggregateData> _contextDataList;
    private SQLiteConnectionFlags _flags;
    private SQLiteCallback _InvokeFunc;
    private SQLiteCallback _StepFunc;
    private SQLiteFinalCallback _FinalFunc;
    private SQLiteCollation _CompareFunc;
    private SQLiteCollation _CompareFunc16;
    internal IntPtr _context;
    private static IDictionary<SQLiteFunctionAttribute, object> _registeredFunctions = (IDictionary<SQLiteFunctionAttribute, object>) new Dictionary<SQLiteFunctionAttribute, object>();
    private bool disposed;

    protected SQLiteFunction()
    {
      this._contextDataList = new Dictionary<IntPtr, SQLiteFunction.AggregateData>();
    }

    protected SQLiteFunction(
      SQLiteDateFormats format,
      DateTimeKind kind,
      string formatString,
      bool utf16)
      : this()
    {
      if (utf16)
        this._base = (SQLiteBase) new SQLite3_UTF16(format, kind, formatString, IntPtr.Zero, (string) null, false);
      else
        this._base = (SQLiteBase) new SQLite3(format, kind, formatString, IntPtr.Zero, (string) null, false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteFunction).Name);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        foreach (KeyValuePair<IntPtr, SQLiteFunction.AggregateData> contextData in this._contextDataList)
        {
          if (contextData.Value._data is IDisposable data)
            data.Dispose();
        }
        this._contextDataList.Clear();
        this._contextDataList = (Dictionary<IntPtr, SQLiteFunction.AggregateData>) null;
        this._flags = SQLiteConnectionFlags.None;
        this._InvokeFunc = (SQLiteCallback) null;
        this._StepFunc = (SQLiteCallback) null;
        this._FinalFunc = (SQLiteFinalCallback) null;
        this._CompareFunc = (SQLiteCollation) null;
        this._base = (SQLiteBase) null;
      }
      this.disposed = true;
    }

    ~SQLiteFunction() => this.Dispose(false);

    public SQLiteConvert SQLiteConvert
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteConvert) this._base;
      }
    }

    public virtual object Invoke(object[] args)
    {
      this.CheckDisposed();
      return (object) null;
    }

    public virtual void Step(object[] args, int stepNumber, ref object contextData)
    {
      this.CheckDisposed();
    }

    public virtual object Final(object contextData)
    {
      this.CheckDisposed();
      return (object) null;
    }

    public virtual int Compare(string param1, string param2)
    {
      this.CheckDisposed();
      return 0;
    }

    internal object[] ConvertParams(int nArgs, IntPtr argsptr)
    {
      object[] objArray = new object[nArgs];
      IntPtr[] destination = new IntPtr[nArgs];
      Marshal.Copy(argsptr, destination, 0, nArgs);
      for (int index = 0; index < nArgs; ++index)
      {
        switch (this._base.GetParamValueType(destination[index]))
        {
          case TypeAffinity.Int64:
            objArray[index] = (object) this._base.GetParamValueInt64(destination[index]);
            break;
          case TypeAffinity.Double:
            objArray[index] = (object) this._base.GetParamValueDouble(destination[index]);
            break;
          case TypeAffinity.Text:
            objArray[index] = (object) this._base.GetParamValueText(destination[index]);
            break;
          case TypeAffinity.Blob:
            int paramValueBytes = (int) this._base.GetParamValueBytes(destination[index], 0, (byte[]) null, 0, 0);
            byte[] bDest = new byte[paramValueBytes];
            this._base.GetParamValueBytes(destination[index], 0, bDest, 0, paramValueBytes);
            objArray[index] = (object) bDest;
            break;
          case TypeAffinity.Null:
            objArray[index] = (object) DBNull.Value;
            break;
          case TypeAffinity.DateTime:
            objArray[index] = (object) this._base.ToDateTime(this._base.GetParamValueText(destination[index]));
            break;
        }
      }
      return objArray;
    }

    private void SetReturnValue(IntPtr context, object returnValue)
    {
      if (returnValue == null || returnValue == DBNull.Value)
      {
        this._base.ReturnNull(context);
      }
      else
      {
        Type type = returnValue.GetType();
        if (type == typeof (DateTime))
          this._base.ReturnText(context, this._base.ToString((DateTime) returnValue));
        else if (returnValue is Exception exception)
        {
          this._base.ReturnError(context, exception.Message);
        }
        else
        {
          switch (SQLiteConvert.TypeToAffinity(type))
          {
            case TypeAffinity.Int64:
              this._base.ReturnInt64(context, Convert.ToInt64(returnValue, (IFormatProvider) CultureInfo.CurrentCulture));
              break;
            case TypeAffinity.Double:
              this._base.ReturnDouble(context, Convert.ToDouble(returnValue, (IFormatProvider) CultureInfo.CurrentCulture));
              break;
            case TypeAffinity.Text:
              this._base.ReturnText(context, returnValue.ToString());
              break;
            case TypeAffinity.Blob:
              this._base.ReturnBlob(context, (byte[]) returnValue);
              break;
            case TypeAffinity.Null:
              this._base.ReturnNull(context);
              break;
          }
        }
      }
    }

    internal void ScalarCallback(IntPtr context, int nArgs, IntPtr argsptr)
    {
      try
      {
        this._context = context;
        this.SetReturnValue(context, this.Invoke(this.ConvertParams(nArgs, argsptr)));
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) != SQLiteConnectionFlags.LogCallbackException)
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Invoke\" method: {0}", (object) ex));
        }
        catch
        {
        }
      }
    }

    internal int CompareCallback(IntPtr ptr, int len1, IntPtr ptr1, int len2, IntPtr ptr2)
    {
      try
      {
        return this.Compare(SQLiteConvert.UTF8ToString(ptr1, len1), SQLiteConvert.UTF8ToString(ptr2, len2));
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) == SQLiteConnectionFlags.LogCallbackException)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Compare\" (UTF8) method: {0}", (object) ex));
        }
        catch
        {
        }
      }
      if (this._base != null && this._base.IsOpen())
        this._base.Cancel();
      return 0;
    }

    internal int CompareCallback16(IntPtr ptr, int len1, IntPtr ptr1, int len2, IntPtr ptr2)
    {
      try
      {
        return this.Compare(SQLite3_UTF16.UTF16ToString(ptr1, len1), SQLite3_UTF16.UTF16ToString(ptr2, len2));
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) == SQLiteConnectionFlags.LogCallbackException)
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Compare\" (UTF16) method: {0}", (object) ex));
        }
        catch
        {
        }
      }
      if (this._base != null && this._base.IsOpen())
        this._base.Cancel();
      return 0;
    }

    internal void StepCallback(IntPtr context, int nArgs, IntPtr argsptr)
    {
      try
      {
        SQLiteFunction.AggregateData aggregateData = (SQLiteFunction.AggregateData) null;
        if (this._base != null)
        {
          IntPtr key = this._base.AggregateContext(context);
          if (this._contextDataList != null && !this._contextDataList.TryGetValue(key, out aggregateData))
          {
            aggregateData = new SQLiteFunction.AggregateData();
            this._contextDataList[key] = aggregateData;
          }
        }
        if (aggregateData == null)
          aggregateData = new SQLiteFunction.AggregateData();
        try
        {
          this._context = context;
          this.Step(this.ConvertParams(nArgs, argsptr), aggregateData._count, ref aggregateData._data);
        }
        finally
        {
          ++aggregateData._count;
        }
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) != SQLiteConnectionFlags.LogCallbackException)
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Step\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
    }

    internal void FinalCallback(IntPtr context)
    {
      try
      {
        object contextData = (object) null;
        if (this._base != null)
        {
          IntPtr key = this._base.AggregateContext(context);
          if (this._contextDataList != null)
          {
            SQLiteFunction.AggregateData aggregateData;
            if (this._contextDataList.TryGetValue(key, out aggregateData))
            {
              contextData = aggregateData._data;
              this._contextDataList.Remove(key);
            }
          }
        }
        try
        {
          this._context = context;
          this.SetReturnValue(context, this.Final(contextData));
        }
        finally
        {
          if (contextData is IDisposable disposable)
            disposable.Dispose();
        }
      }
      catch (Exception ex)
      {
        try
        {
          if ((this._flags & SQLiteConnectionFlags.LogCallbackException) != SQLiteConnectionFlags.LogCallbackException)
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"Final\" method: {1}", (object) ex));
        }
        catch
        {
        }
      }
    }

    [FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
    static SQLiteFunction()
    {
      try
      {
        if (UnsafeNativeMethods.GetSettingValue("No_SQLiteFunctions", (string) null) != null)
          return;
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        int length1 = assemblies.Length;
        AssemblyName name = Assembly.GetExecutingAssembly().GetName();
        for (int index1 = 0; index1 < length1; ++index1)
        {
          bool flag = false;
          Type[] types;
          try
          {
            AssemblyName[] referencedAssemblies = assemblies[index1].GetReferencedAssemblies();
            int length2 = referencedAssemblies.Length;
            for (int index2 = 0; index2 < length2; ++index2)
            {
              if (referencedAssemblies[index2].Name == name.Name)
              {
                flag = true;
                break;
              }
            }
            if (flag)
              types = assemblies[index1].GetTypes();
            else
              continue;
          }
          catch (ReflectionTypeLoadException ex)
          {
            types = ex.Types;
          }
          int length3 = types.Length;
          for (int index3 = 0; index3 < length3; ++index3)
          {
            if (!(types[index3] == (Type) null))
            {
              object[] customAttributes = types[index3].GetCustomAttributes(typeof (SQLiteFunctionAttribute), false);
              int length4 = customAttributes.Length;
              for (int index4 = 0; index4 < length4; ++index4)
              {
                if (customAttributes[index4] is SQLiteFunctionAttribute at)
                {
                  at.InstanceType = types[index3];
                  SQLiteFunction.ReplaceFunction(at, (object) null);
                }
              }
            }
          }
        }
      }
      catch
      {
      }
    }

    public static void RegisterFunction(Type typ)
    {
      foreach (object customAttribute in typ.GetCustomAttributes(typeof (SQLiteFunctionAttribute), false))
      {
        if (customAttribute is SQLiteFunctionAttribute functionAttribute)
          SQLiteFunction.RegisterFunction(functionAttribute.Name, functionAttribute.Arguments, functionAttribute.FuncType, typ, functionAttribute.Callback1, functionAttribute.Callback2);
      }
    }

    public static void RegisterFunction(
      string name,
      int argumentCount,
      FunctionType functionType,
      Type instanceType,
      Delegate callback1,
      Delegate callback2)
    {
      SQLiteFunction.ReplaceFunction(new SQLiteFunctionAttribute(name, argumentCount, functionType)
      {
        InstanceType = instanceType,
        Callback1 = callback1,
        Callback2 = callback2
      }, (object) null);
    }

    private static bool ReplaceFunction(SQLiteFunctionAttribute at, object newValue)
    {
      object obj;
      if (SQLiteFunction._registeredFunctions.TryGetValue(at, out obj))
      {
        if (obj is IDisposable disposable)
          disposable.Dispose();
        SQLiteFunction._registeredFunctions[at] = newValue;
        return true;
      }
      SQLiteFunction._registeredFunctions.Add(at, newValue);
      return false;
    }

    private static bool CreateFunction(
      SQLiteFunctionAttribute functionAttribute,
      out SQLiteFunction function)
    {
      if (functionAttribute == null)
      {
        function = (SQLiteFunction) null;
        return false;
      }
      if ((object) functionAttribute.Callback1 != null || (object) functionAttribute.Callback2 != null)
      {
        function = (SQLiteFunction) new SQLiteDelegateFunction(functionAttribute.Callback1, functionAttribute.Callback2);
        return true;
      }
      if (functionAttribute.InstanceType != (Type) null)
      {
        function = (SQLiteFunction) Activator.CreateInstance(functionAttribute.InstanceType);
        return true;
      }
      function = (SQLiteFunction) null;
      return false;
    }

    internal static IDictionary<SQLiteFunctionAttribute, SQLiteFunction> BindFunctions(
      SQLiteBase sqlbase,
      SQLiteConnectionFlags flags)
    {
      IDictionary<SQLiteFunctionAttribute, SQLiteFunction> dictionary = (IDictionary<SQLiteFunctionAttribute, SQLiteFunction>) new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
      foreach (KeyValuePair<SQLiteFunctionAttribute, object> registeredFunction in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, object>>) SQLiteFunction._registeredFunctions)
      {
        SQLiteFunctionAttribute key = registeredFunction.Key;
        if (key != null)
        {
          SQLiteFunction function;
          if (SQLiteFunction.CreateFunction(key, out function))
          {
            SQLiteFunction.BindFunction(sqlbase, key, function, flags);
            dictionary[key] = function;
          }
          else
            dictionary[key] = (SQLiteFunction) null;
        }
      }
      return dictionary;
    }

    internal static bool UnbindAllFunctions(
      SQLiteBase sqlbase,
      SQLiteConnectionFlags flags,
      bool registered)
    {
      if (sqlbase == null)
        return false;
      IDictionary<SQLiteFunctionAttribute, SQLiteFunction> functions = sqlbase.Functions;
      if (functions == null)
        return false;
      bool flag = true;
      if (registered)
      {
        foreach (KeyValuePair<SQLiteFunctionAttribute, object> registeredFunction in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, object>>) SQLiteFunction._registeredFunctions)
        {
          SQLiteFunctionAttribute key = registeredFunction.Key;
          SQLiteFunction function;
          if (key != null && (!functions.TryGetValue(key, out function) || function == null || !SQLiteFunction.UnbindFunction(sqlbase, key, function, flags)))
            flag = false;
        }
      }
      else
      {
        foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> keyValuePair in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction>>) new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>(functions))
        {
          SQLiteFunctionAttribute key = keyValuePair.Key;
          if (key != null)
          {
            SQLiteFunction function = keyValuePair.Value;
            if (function == null || !SQLiteFunction.UnbindFunction(sqlbase, key, function, flags))
              flag = false;
          }
        }
      }
      return flag;
    }

    internal static void BindFunction(
      SQLiteBase sqliteBase,
      SQLiteFunctionAttribute functionAttribute,
      SQLiteFunction function,
      SQLiteConnectionFlags flags)
    {
      if (sqliteBase == null)
        throw new ArgumentNullException(nameof (sqliteBase));
      if (functionAttribute == null)
        throw new ArgumentNullException(nameof (functionAttribute));
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      FunctionType funcType = functionAttribute.FuncType;
      function._base = sqliteBase;
      function._flags = flags;
      function._InvokeFunc = funcType == FunctionType.Scalar ? new SQLiteCallback(function.ScalarCallback) : (SQLiteCallback) null;
      function._StepFunc = funcType == FunctionType.Aggregate ? new SQLiteCallback(function.StepCallback) : (SQLiteCallback) null;
      function._FinalFunc = funcType == FunctionType.Aggregate ? new SQLiteFinalCallback(function.FinalCallback) : (SQLiteFinalCallback) null;
      function._CompareFunc = funcType == FunctionType.Collation ? new SQLiteCollation(function.CompareCallback) : (SQLiteCollation) null;
      function._CompareFunc16 = funcType == FunctionType.Collation ? new SQLiteCollation(function.CompareCallback16) : (SQLiteCollation) null;
      string name = functionAttribute.Name;
      if (funcType != FunctionType.Collation)
      {
        bool needCollSeq = function is SQLiteFunctionEx;
        int function1 = (int) sqliteBase.CreateFunction(name, functionAttribute.Arguments, needCollSeq, function._InvokeFunc, function._StepFunc, function._FinalFunc, true);
      }
      else
      {
        int collation = (int) sqliteBase.CreateCollation(name, function._CompareFunc, function._CompareFunc16, true);
      }
    }

    internal static bool UnbindFunction(
      SQLiteBase sqliteBase,
      SQLiteFunctionAttribute functionAttribute,
      SQLiteFunction function,
      SQLiteConnectionFlags flags)
    {
      if (sqliteBase == null)
        throw new ArgumentNullException(nameof (sqliteBase));
      if (functionAttribute == null)
        throw new ArgumentNullException(nameof (functionAttribute));
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      FunctionType funcType = functionAttribute.FuncType;
      string name = functionAttribute.Name;
      if (funcType == FunctionType.Collation)
        return sqliteBase.CreateCollation(name, (SQLiteCollation) null, (SQLiteCollation) null, false) == SQLiteErrorCode.Ok;
      bool needCollSeq = function is SQLiteFunctionEx;
      return sqliteBase.CreateFunction(name, functionAttribute.Arguments, needCollSeq, (SQLiteCallback) null, (SQLiteCallback) null, (SQLiteFinalCallback) null, false) == SQLiteErrorCode.Ok;
    }

    private class AggregateData
    {
      internal int _count = 1;
      internal object _data;
    }
  }
}
