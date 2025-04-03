// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFunctionAttribute
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

#nullable disable
namespace System.Data.SQLite
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
  public sealed class SQLiteFunctionAttribute : Attribute
  {
    private string _name;
    private int _argumentCount;
    private FunctionType _functionType;
    private Type _instanceType;
    private Delegate _callback1;
    private Delegate _callback2;

    public SQLiteFunctionAttribute()
      : this((string) null, -1, FunctionType.Scalar)
    {
    }

    public SQLiteFunctionAttribute(string name, int argumentCount, FunctionType functionType)
    {
      this._name = name;
      this._argumentCount = argumentCount;
      this._functionType = functionType;
      this._instanceType = (Type) null;
      this._callback1 = (Delegate) null;
      this._callback2 = (Delegate) null;
    }

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public int Arguments
    {
      get => this._argumentCount;
      set => this._argumentCount = value;
    }

    public FunctionType FuncType
    {
      get => this._functionType;
      set => this._functionType = value;
    }

    internal Type InstanceType
    {
      get => this._instanceType;
      set => this._instanceType = value;
    }

    internal Delegate Callback1
    {
      get => this._callback1;
      set => this._callback1 = value;
    }

    internal Delegate Callback2
    {
      get => this._callback2;
      set => this._callback2 = value;
    }
  }
}
