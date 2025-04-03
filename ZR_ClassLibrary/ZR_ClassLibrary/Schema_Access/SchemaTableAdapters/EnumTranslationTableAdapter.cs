// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Schema_Access.SchemaTableAdapters.EnumTranslationTableAdapter
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using ZR_ClassLibrary.Properties;

#nullable disable
namespace ZR_ClassLibrary.Schema_Access.SchemaTableAdapters
{
  [DesignerCategory("code")]
  [ToolboxItem(true)]
  [DataObject(true)]
  [Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
  [HelpKeyword("vs.data.TableAdapter")]
  public class EnumTranslationTableAdapter : Component
  {
    private OleDbDataAdapter _adapter;
    private OleDbConnection _connection;
    private OleDbCommand[] _commandCollection;
    private bool _clearBeforeFill;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public EnumTranslationTableAdapter() => this.ClearBeforeFill = true;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private OleDbDataAdapter Adapter
    {
      get
      {
        if (this._adapter == null)
          this.InitAdapter();
        return this._adapter;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    internal OleDbConnection Connection
    {
      get
      {
        if (this._connection == null)
          this.InitConnection();
        return this._connection;
      }
      set
      {
        this._connection = value;
        if (this.Adapter.InsertCommand != null)
          this.Adapter.InsertCommand.Connection = value;
        if (this.Adapter.DeleteCommand != null)
          this.Adapter.DeleteCommand.Connection = value;
        if (this.Adapter.UpdateCommand != null)
          this.Adapter.UpdateCommand.Connection = value;
        for (int index = 0; index < this.CommandCollection.Length; ++index)
        {
          if (this.CommandCollection[index] != null)
            this.CommandCollection[index].Connection = value;
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected OleDbCommand[] CommandCollection
    {
      get
      {
        if (this._commandCollection == null)
          this.InitCommandCollection();
        return this._commandCollection;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public bool ClearBeforeFill
    {
      get => this._clearBeforeFill;
      set => this._clearBeforeFill = value;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitAdapter()
    {
      this._adapter = new OleDbDataAdapter();
      this._adapter.TableMappings.Add((object) new DataTableMapping()
      {
        SourceTable = "Table",
        DataSetTable = "EnumTranslation",
        ColumnMappings = {
          {
            "EnumName",
            "EnumName"
          },
          {
            "EnumElement",
            "EnumElement"
          },
          {
            "de_Name",
            "de_Name"
          },
          {
            "de_Description",
            "de_Description"
          },
          {
            "en_Name",
            "en_Name"
          },
          {
            "en_Description",
            "en_Description"
          }
        }
      });
      this._adapter.DeleteCommand = new OleDbCommand();
      this._adapter.DeleteCommand.Connection = this.Connection;
      this._adapter.DeleteCommand.CommandText = "DELETE FROM `EnumTranslation` WHERE ((`EnumName` = ?) AND (`EnumElement` = ?) AND ((? = 1 AND `de_Name` IS NULL) OR (`de_Name` = ?)) AND ((? = 1 AND `en_Name` IS NULL) OR (`en_Name` = ?)))";
      this._adapter.DeleteCommand.CommandType = CommandType.Text;
      this._adapter.DeleteCommand.Parameters.Add(new OleDbParameter("Original_EnumName", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumName", DataRowVersion.Original, false, (object) null));
      this._adapter.DeleteCommand.Parameters.Add(new OleDbParameter("Original_EnumElement", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumElement", DataRowVersion.Original, false, (object) null));
      this._adapter.DeleteCommand.Parameters.Add(new OleDbParameter("IsNull_de_Name", OleDbType.Integer, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Name", DataRowVersion.Original, true, (object) null));
      this._adapter.DeleteCommand.Parameters.Add(new OleDbParameter("Original_de_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Name", DataRowVersion.Original, false, (object) null));
      this._adapter.DeleteCommand.Parameters.Add(new OleDbParameter("IsNull_en_Name", OleDbType.Integer, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Name", DataRowVersion.Original, true, (object) null));
      this._adapter.DeleteCommand.Parameters.Add(new OleDbParameter("Original_en_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Name", DataRowVersion.Original, false, (object) null));
      this._adapter.InsertCommand = new OleDbCommand();
      this._adapter.InsertCommand.Connection = this.Connection;
      this._adapter.InsertCommand.CommandText = "INSERT INTO `EnumTranslation` (`EnumName`, `EnumElement`, `de_Name`, `de_Description`, `en_Name`, `en_Description`) VALUES (?, ?, ?, ?, ?, ?)";
      this._adapter.InsertCommand.CommandType = CommandType.Text;
      this._adapter.InsertCommand.Parameters.Add(new OleDbParameter("EnumName", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumName", DataRowVersion.Current, false, (object) null));
      this._adapter.InsertCommand.Parameters.Add(new OleDbParameter("EnumElement", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumElement", DataRowVersion.Current, false, (object) null));
      this._adapter.InsertCommand.Parameters.Add(new OleDbParameter("de_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Name", DataRowVersion.Current, false, (object) null));
      this._adapter.InsertCommand.Parameters.Add(new OleDbParameter("de_Description", OleDbType.LongVarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Description", DataRowVersion.Current, false, (object) null));
      this._adapter.InsertCommand.Parameters.Add(new OleDbParameter("en_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Name", DataRowVersion.Current, false, (object) null));
      this._adapter.InsertCommand.Parameters.Add(new OleDbParameter("en_Description", OleDbType.LongVarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Description", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand = new OleDbCommand();
      this._adapter.UpdateCommand.Connection = this.Connection;
      this._adapter.UpdateCommand.CommandText = "UPDATE `EnumTranslation` SET `EnumName` = ?, `EnumElement` = ?, `de_Name` = ?, `de_Description` = ?, `en_Name` = ?, `en_Description` = ? WHERE ((`EnumName` = ?) AND (`EnumElement` = ?) AND ((? = 1 AND `de_Name` IS NULL) OR (`de_Name` = ?)) AND ((? = 1 AND `en_Name` IS NULL) OR (`en_Name` = ?)))";
      this._adapter.UpdateCommand.CommandType = CommandType.Text;
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("EnumName", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumName", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("EnumElement", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumElement", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("de_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Name", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("de_Description", OleDbType.LongVarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Description", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("en_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Name", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("en_Description", OleDbType.LongVarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Description", DataRowVersion.Current, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("Original_EnumName", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumName", DataRowVersion.Original, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("Original_EnumElement", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "EnumElement", DataRowVersion.Original, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("IsNull_de_Name", OleDbType.Integer, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Name", DataRowVersion.Original, true, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("Original_de_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "de_Name", DataRowVersion.Original, false, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("IsNull_en_Name", OleDbType.Integer, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Name", DataRowVersion.Original, true, (object) null));
      this._adapter.UpdateCommand.Parameters.Add(new OleDbParameter("Original_en_Name", OleDbType.VarWChar, 0, ParameterDirection.Input, (byte) 0, (byte) 0, "en_Name", DataRowVersion.Original, false, (object) null));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitConnection()
    {
      this._connection = new OleDbConnection();
      this._connection.ConnectionString = Settings.Default.MeterDB_NewConnectionString4;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitCommandCollection()
    {
      this._commandCollection = new OleDbCommand[1];
      this._commandCollection[0] = new OleDbCommand();
      this._commandCollection[0].Connection = this.Connection;
      this._commandCollection[0].CommandText = "SELECT EnumName, EnumElement, de_Name, de_Description, en_Name, en_Description FROM EnumTranslation";
      this._commandCollection[0].CommandType = CommandType.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    [DataObjectMethod(DataObjectMethodType.Fill, true)]
    public virtual int Fill(Schema.EnumTranslationDataTable dataTable)
    {
      this.Adapter.SelectCommand = this.CommandCollection[0];
      if (this.ClearBeforeFill)
        dataTable.Clear();
      return this.Adapter.Fill((DataTable) dataTable);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public virtual Schema.EnumTranslationDataTable GetData()
    {
      this.Adapter.SelectCommand = this.CommandCollection[0];
      Schema.EnumTranslationDataTable data = new Schema.EnumTranslationDataTable();
      this.Adapter.Fill((DataTable) data);
      return data;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    public virtual int Update(Schema.EnumTranslationDataTable dataTable)
    {
      return this.Adapter.Update((DataTable) dataTable);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    public virtual int Update(Schema dataSet)
    {
      return this.Adapter.Update((DataSet) dataSet, "EnumTranslation");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    public virtual int Update(DataRow dataRow)
    {
      return this.Adapter.Update(new DataRow[1]{ dataRow });
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    public virtual int Update(DataRow[] dataRows) => this.Adapter.Update(dataRows);

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public virtual int Delete(
      string Original_EnumName,
      string Original_EnumElement,
      string Original_de_Name,
      string Original_en_Name)
    {
      if (Original_EnumName == null)
        this.Adapter.DeleteCommand.Parameters[0].Value = (object) DBNull.Value;
      else
        this.Adapter.DeleteCommand.Parameters[0].Value = (object) Original_EnumName;
      if (Original_EnumElement == null)
        this.Adapter.DeleteCommand.Parameters[1].Value = (object) DBNull.Value;
      else
        this.Adapter.DeleteCommand.Parameters[1].Value = (object) Original_EnumElement;
      if (Original_de_Name == null)
      {
        this.Adapter.DeleteCommand.Parameters[2].Value = (object) 1;
        this.Adapter.DeleteCommand.Parameters[3].Value = (object) DBNull.Value;
      }
      else
      {
        this.Adapter.DeleteCommand.Parameters[2].Value = (object) 0;
        this.Adapter.DeleteCommand.Parameters[3].Value = (object) Original_de_Name;
      }
      if (Original_en_Name == null)
      {
        this.Adapter.DeleteCommand.Parameters[4].Value = (object) 1;
        this.Adapter.DeleteCommand.Parameters[5].Value = (object) DBNull.Value;
      }
      else
      {
        this.Adapter.DeleteCommand.Parameters[4].Value = (object) 0;
        this.Adapter.DeleteCommand.Parameters[5].Value = (object) Original_en_Name;
      }
      ConnectionState state = this.Adapter.DeleteCommand.Connection.State;
      if ((this.Adapter.DeleteCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
        this.Adapter.DeleteCommand.Connection.Open();
      try
      {
        return this.Adapter.DeleteCommand.ExecuteNonQuery();
      }
      finally
      {
        if (state == ConnectionState.Closed)
          this.Adapter.DeleteCommand.Connection.Close();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public virtual int Insert(
      string EnumName,
      string EnumElement,
      string de_Name,
      string de_Description,
      string en_Name,
      string en_Description)
    {
      if (EnumName == null)
        this.Adapter.InsertCommand.Parameters[0].Value = (object) DBNull.Value;
      else
        this.Adapter.InsertCommand.Parameters[0].Value = (object) EnumName;
      if (EnumElement == null)
        this.Adapter.InsertCommand.Parameters[1].Value = (object) DBNull.Value;
      else
        this.Adapter.InsertCommand.Parameters[1].Value = (object) EnumElement;
      if (de_Name == null)
        this.Adapter.InsertCommand.Parameters[2].Value = (object) DBNull.Value;
      else
        this.Adapter.InsertCommand.Parameters[2].Value = (object) de_Name;
      if (de_Description == null)
        this.Adapter.InsertCommand.Parameters[3].Value = (object) DBNull.Value;
      else
        this.Adapter.InsertCommand.Parameters[3].Value = (object) de_Description;
      if (en_Name == null)
        this.Adapter.InsertCommand.Parameters[4].Value = (object) DBNull.Value;
      else
        this.Adapter.InsertCommand.Parameters[4].Value = (object) en_Name;
      if (en_Description == null)
        this.Adapter.InsertCommand.Parameters[5].Value = (object) DBNull.Value;
      else
        this.Adapter.InsertCommand.Parameters[5].Value = (object) en_Description;
      ConnectionState state = this.Adapter.InsertCommand.Connection.State;
      if ((this.Adapter.InsertCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
        this.Adapter.InsertCommand.Connection.Open();
      try
      {
        return this.Adapter.InsertCommand.ExecuteNonQuery();
      }
      finally
      {
        if (state == ConnectionState.Closed)
          this.Adapter.InsertCommand.Connection.Close();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public virtual int Update(
      string EnumName,
      string EnumElement,
      string de_Name,
      string de_Description,
      string en_Name,
      string en_Description,
      string Original_EnumName,
      string Original_EnumElement,
      string Original_de_Name,
      string Original_en_Name)
    {
      if (EnumName == null)
        this.Adapter.UpdateCommand.Parameters[0].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[0].Value = (object) EnumName;
      if (EnumElement == null)
        this.Adapter.UpdateCommand.Parameters[1].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[1].Value = (object) EnumElement;
      if (de_Name == null)
        this.Adapter.UpdateCommand.Parameters[2].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[2].Value = (object) de_Name;
      if (de_Description == null)
        this.Adapter.UpdateCommand.Parameters[3].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[3].Value = (object) de_Description;
      if (en_Name == null)
        this.Adapter.UpdateCommand.Parameters[4].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[4].Value = (object) en_Name;
      if (en_Description == null)
        this.Adapter.UpdateCommand.Parameters[5].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[5].Value = (object) en_Description;
      if (Original_EnumName == null)
        this.Adapter.UpdateCommand.Parameters[6].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[6].Value = (object) Original_EnumName;
      if (Original_EnumElement == null)
        this.Adapter.UpdateCommand.Parameters[7].Value = (object) DBNull.Value;
      else
        this.Adapter.UpdateCommand.Parameters[7].Value = (object) Original_EnumElement;
      if (Original_de_Name == null)
      {
        this.Adapter.UpdateCommand.Parameters[8].Value = (object) 1;
        this.Adapter.UpdateCommand.Parameters[9].Value = (object) DBNull.Value;
      }
      else
      {
        this.Adapter.UpdateCommand.Parameters[8].Value = (object) 0;
        this.Adapter.UpdateCommand.Parameters[9].Value = (object) Original_de_Name;
      }
      if (Original_en_Name == null)
      {
        this.Adapter.UpdateCommand.Parameters[10].Value = (object) 1;
        this.Adapter.UpdateCommand.Parameters[11].Value = (object) DBNull.Value;
      }
      else
      {
        this.Adapter.UpdateCommand.Parameters[10].Value = (object) 0;
        this.Adapter.UpdateCommand.Parameters[11].Value = (object) Original_en_Name;
      }
      ConnectionState state = this.Adapter.UpdateCommand.Connection.State;
      if ((this.Adapter.UpdateCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
        this.Adapter.UpdateCommand.Connection.Open();
      try
      {
        return this.Adapter.UpdateCommand.ExecuteNonQuery();
      }
      finally
      {
        if (state == ConnectionState.Closed)
          this.Adapter.UpdateCommand.Connection.Close();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [HelpKeyword("vs.data.TableAdapter")]
    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public virtual int Update(
      string de_Name,
      string de_Description,
      string en_Name,
      string en_Description,
      string Original_EnumName,
      string Original_EnumElement,
      string Original_de_Name,
      string Original_en_Name)
    {
      return this.Update(Original_EnumName, Original_EnumElement, de_Name, de_Description, en_Name, en_Description, Original_EnumName, Original_EnumElement, Original_de_Name, Original_en_Name);
    }
  }
}
