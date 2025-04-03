// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteParameterCollection
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

#nullable disable
namespace System.Data.SQLite
{
  [ListBindable(false)]
  [Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
  public sealed class SQLiteParameterCollection : DbParameterCollection
  {
    private SQLiteCommand _command;
    private List<SQLiteParameter> _parameterList;
    private bool _unboundFlag;

    internal SQLiteParameterCollection(SQLiteCommand cmd)
    {
      this._command = cmd;
      this._parameterList = new List<SQLiteParameter>();
      this._unboundFlag = true;
    }

    public override bool IsSynchronized => false;

    public override bool IsFixedSize => false;

    public override bool IsReadOnly => false;

    public override object SyncRoot => (object) null;

    public override IEnumerator GetEnumerator()
    {
      return (IEnumerator) this._parameterList.GetEnumerator();
    }

    public SQLiteParameter Add(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      string sourceColumn)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, parameterType, parameterSize, sourceColumn);
      this.Add(parameter);
      return parameter;
    }

    public SQLiteParameter Add(string parameterName, DbType parameterType, int parameterSize)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, parameterType, parameterSize);
      this.Add(parameter);
      return parameter;
    }

    public SQLiteParameter Add(string parameterName, DbType parameterType)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, parameterType);
      this.Add(parameter);
      return parameter;
    }

    public int Add(SQLiteParameter parameter)
    {
      int index = -1;
      if (!string.IsNullOrEmpty(parameter.ParameterName))
        index = this.IndexOf(parameter.ParameterName);
      if (index == -1)
      {
        index = this._parameterList.Count;
        this._parameterList.Add(parameter);
      }
      this.SetParameter(index, (DbParameter) parameter);
      return index;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int Add(object value) => this.Add((SQLiteParameter) value);

    public SQLiteParameter AddWithValue(string parameterName, object value)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, value);
      this.Add(parameter);
      return parameter;
    }

    public void AddRange(SQLiteParameter[] values)
    {
      int length = values.Length;
      for (int index = 0; index < length; ++index)
        this.Add(values[index]);
    }

    public override void AddRange(Array values)
    {
      int length = values.Length;
      for (int index = 0; index < length; ++index)
        this.Add((SQLiteParameter) values.GetValue(index));
    }

    public override void Clear()
    {
      this._unboundFlag = true;
      this._parameterList.Clear();
    }

    public override bool Contains(string parameterName) => this.IndexOf(parameterName) != -1;

    public override bool Contains(object value)
    {
      return this._parameterList.Contains((SQLiteParameter) value);
    }

    public override void CopyTo(Array array, int index) => throw new NotImplementedException();

    public override int Count => this._parameterList.Count;

    public SQLiteParameter this[string parameterName]
    {
      get => (SQLiteParameter) this.GetParameter(parameterName);
      set => this.SetParameter(parameterName, (DbParameter) value);
    }

    public SQLiteParameter this[int index]
    {
      get => (SQLiteParameter) this.GetParameter(index);
      set => this.SetParameter(index, (DbParameter) value);
    }

    protected override DbParameter GetParameter(string parameterName)
    {
      return this.GetParameter(this.IndexOf(parameterName));
    }

    protected override DbParameter GetParameter(int index)
    {
      return (DbParameter) this._parameterList[index];
    }

    public override int IndexOf(string parameterName)
    {
      int count = this._parameterList.Count;
      for (int index = 0; index < count; ++index)
      {
        if (string.Compare(parameterName, this._parameterList[index].ParameterName, StringComparison.OrdinalIgnoreCase) == 0)
          return index;
      }
      return -1;
    }

    public override int IndexOf(object value)
    {
      return this._parameterList.IndexOf((SQLiteParameter) value);
    }

    public override void Insert(int index, object value)
    {
      this._unboundFlag = true;
      this._parameterList.Insert(index, (SQLiteParameter) value);
    }

    public override void Remove(object value)
    {
      this._unboundFlag = true;
      this._parameterList.Remove((SQLiteParameter) value);
    }

    public override void RemoveAt(string parameterName)
    {
      this.RemoveAt(this.IndexOf(parameterName));
    }

    public override void RemoveAt(int index)
    {
      this._unboundFlag = true;
      this._parameterList.RemoveAt(index);
    }

    protected override void SetParameter(string parameterName, DbParameter value)
    {
      this.SetParameter(this.IndexOf(parameterName), value);
    }

    protected override void SetParameter(int index, DbParameter value)
    {
      this._unboundFlag = true;
      this._parameterList[index] = (SQLiteParameter) value;
    }

    internal void Unbind() => this._unboundFlag = true;

    internal void MapParameters(SQLiteStatement activeStatement)
    {
      if (!this._unboundFlag || this._parameterList.Count == 0 || this._command._statementList == null)
        return;
      int num1 = 0;
      int num2 = -1;
      foreach (SQLiteParameter parameter in this._parameterList)
      {
        ++num2;
        string s1 = parameter.ParameterName;
        if (s1 == null)
        {
          s1 = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, ";{0}", (object) num1);
          ++num1;
        }
        bool flag = false;
        int num3 = activeStatement != null ? 1 : this._command._statementList.Count;
        SQLiteStatement sqLiteStatement1 = activeStatement;
        for (int index = 0; index < num3; ++index)
        {
          flag = false;
          if (sqLiteStatement1 == null)
            sqLiteStatement1 = this._command._statementList[index];
          if (sqLiteStatement1._paramNames != null && sqLiteStatement1.MapParameter(s1, parameter))
            flag = true;
          sqLiteStatement1 = (SQLiteStatement) null;
        }
        if (!flag)
        {
          string s2 = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, ";{0}", (object) num2);
          SQLiteStatement sqLiteStatement2 = activeStatement;
          for (int index = 0; index < num3; ++index)
          {
            if (sqLiteStatement2 == null)
              sqLiteStatement2 = this._command._statementList[index];
            if (sqLiteStatement2._paramNames != null && sqLiteStatement2.MapParameter(s2, parameter))
              ;
            sqLiteStatement2 = (SQLiteStatement) null;
          }
        }
      }
      if (activeStatement != null)
        return;
      this._unboundFlag = false;
    }
  }
}
