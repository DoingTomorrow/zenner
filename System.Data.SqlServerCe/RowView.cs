// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.RowView
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.ComponentModel;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class RowView : IEditableObject, IDataErrorInfo, IDisposable
  {
    private int hBookmark = -1;
    private ResultSetView parent;
    private WeakReference weakRefRecord;
    private SqlCeUpdatableRecord record;
    private string error;
    private bool isBeingEdited;
    private bool isNewRow;
    private bool isDeleted;

    internal RowView(RowView value)
    {
      this.hBookmark = value.hBookmark;
      this.parent = value.parent;
      this.weakRefRecord = value.weakRefRecord;
      this.error = value.error;
      this.isBeingEdited = value.isBeingEdited;
      this.isNewRow = value.isNewRow;
      this.isDeleted = value.isDeleted;
    }

    internal RowView(int hBookmark)
    {
      this.hBookmark = hBookmark;
      this.isNewRow = false;
    }

    internal RowView(ResultSetView parent, int hBookmark)
      : this(hBookmark)
    {
      this.parent = parent;
    }

    internal RowView(ResultSetView parent, int hBookmark, SqlCeUpdatableRecord record)
      : this(parent, hBookmark)
    {
      if (-1 == hBookmark)
      {
        this.isNewRow = true;
        this.record = record;
      }
      this.weakRefRecord = new WeakReference((object) record);
    }

    internal bool IsNew
    {
      set => this.isNewRow = value;
      get => this.isNewRow;
    }

    internal ResultSetView Parent
    {
      get => this.parent;
      set => this.parent = value;
    }

    public override int GetHashCode() => this.hBookmark;

    public override bool Equals(object other)
    {
      return other is RowView && ((RowView) other).Bookmark == this.hBookmark;
    }

    internal int Bookmark => this.hBookmark;

    public SqlCeUpdatableRecord UpdatableRecord
    {
      get
      {
        if (this.record != null)
          return this.record;
        if (ADP.IsAlive(this.weakRefRecord))
          return (SqlCeUpdatableRecord) this.weakRefRecord.Target;
        if (this.isDeleted)
          return (SqlCeUpdatableRecord) null;
        this.record = (SqlCeUpdatableRecord) null;
        try
        {
          SqlCeResultSet sqlCeResultSet = this.parent.SqlCeResultSet;
          if (!sqlCeResultSet.GotoRow(this.hBookmark))
          {
            this.error = Res.GetString("SQLCE_DeletedRow");
            this.isDeleted = true;
            return (SqlCeUpdatableRecord) null;
          }
          SqlCeUpdatableRecord currentRecord = sqlCeResultSet.GetCurrentRecord();
          this.weakRefRecord = new WeakReference((object) currentRecord);
          return currentRecord;
        }
        catch (SqlCeException ex)
        {
          if (-2147217906 == ex.HResult || -2147217885 == ex.HResult)
          {
            this.error = Res.GetString("SQLCE_DeletedRow");
            this.isDeleted = true;
            return (SqlCeUpdatableRecord) null;
          }
          this.error = ex.Message;
          throw ex;
        }
        catch (Exception ex)
        {
          this.error = ADP.IsCatchableExceptionType(ex) ? ex.Message : throw ex;
          throw ex;
        }
      }
    }

    void IDisposable.Dispose()
    {
      this.hBookmark = -1;
      this.parent = (ResultSetView) null;
      this.record = (SqlCeUpdatableRecord) null;
      this.weakRefRecord = (WeakReference) null;
    }

    void IEditableObject.BeginEdit()
    {
      if (this.isBeingEdited)
        return;
      if (this.isDeleted)
        return;
      try
      {
        this.isBeingEdited = true;
        if (this.isNewRow)
        {
          for (int ordinal = 0; ordinal < this.record.FieldCount; ++ordinal)
          {
            this.record.SetValue(ordinal, (object) DBNull.Value);
            this.record.ColumnsUpdatedStatus[ordinal] = ColumnUpdatedStatus.None;
          }
        }
        else
        {
          SqlCeResultSet sqlCeResultSet = this.parent.SqlCeResultSet;
          if (!sqlCeResultSet.GotoRow(this.hBookmark))
          {
            this.error = Res.GetString("SQLCE_DeletedRow");
            this.isDeleted = true;
          }
          else
          {
            this.record = sqlCeResultSet.GetCurrentRecord();
            this.weakRefRecord = new WeakReference((object) this.record);
          }
        }
      }
      catch (SqlCeException ex)
      {
        this.isBeingEdited = false;
        if (-2147217906 == ex.HResult || -2147217885 == ex.HResult)
        {
          this.error = Res.GetString("SQLCE_DeletedRow");
          this.isDeleted = true;
        }
        else
          this.error = ex.Message;
      }
      catch (Exception ex)
      {
        if (!ADP.IsCatchableExceptionType(ex))
          throw ex;
        this.isBeingEdited = false;
        this.error = ex.Message;
      }
    }

    void IEditableObject.CancelEdit()
    {
      if (this.isNewRow)
      {
        ((IList) this.parent).Remove((object) this);
        this.record = (SqlCeUpdatableRecord) null;
        this.weakRefRecord = (WeakReference) null;
      }
      else
      {
        if (this.isDeleted)
          return;
        this.Refresh();
      }
    }

    void IEditableObject.EndEdit()
    {
      if (this.isDeleted)
        return;
      SqlCeResultSet sqlCeResultSet = this.parent.SqlCeResultSet;
      this.error = (string) null;
      if (!this.isBeingEdited)
        return;
      bool flag1 = false;
      try
      {
        if (this.isNewRow)
        {
          int num = sqlCeResultSet.InternalInsert(false, (object) this.parent, this.record);
          ((IList) this.parent).Remove((object) this);
          this.isNewRow = false;
          this.hBookmark = num;
          for (int index = 0; index < this.record.FieldCount; ++index)
            this.record.ColumnsUpdatedStatus[index] = ColumnUpdatedStatus.None;
          this.record = (SqlCeUpdatableRecord) null;
        }
        else
        {
          bool flag2 = false;
          for (int index = 0; index < this.record.FieldCount; ++index)
          {
            if (this.record.ColumnsUpdatedStatus[index] != ColumnUpdatedStatus.None)
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
            this.record = (SqlCeUpdatableRecord) null;
          else if (!sqlCeResultSet.GotoRow(this.hBookmark))
          {
            this.error = Res.GetString("SQLCE_DeletedRow");
            this.isDeleted = true;
          }
          else
          {
            for (int ordinal = 0; ordinal < this.record.FieldCount; ++ordinal)
            {
              if (this.record.ColumnsUpdatedStatus[ordinal] != ColumnUpdatedStatus.None)
                sqlCeResultSet.SetValue(ordinal, this.record.GetValue(ordinal));
            }
            sqlCeResultSet.InternalUpdate((object) this.parent);
            for (int index = 0; index < this.record.FieldCount; ++index)
              this.record.ColumnsUpdatedStatus[index] = ColumnUpdatedStatus.None;
            this.record = (SqlCeUpdatableRecord) null;
          }
        }
      }
      catch (SqlCeException ex)
      {
        flag1 = true;
        if (-2147217906 != ex.HResult && -2147217885 != ex.HResult)
          throw ex;
        this.error = Res.GetString("SQLCE_DeletedRow");
        this.isDeleted = true;
      }
      finally
      {
        if (!flag1)
          this.isBeingEdited = false;
      }
    }

    internal void Refresh()
    {
      SqlCeUpdatableRecord record = this.record;
      WeakReference weakRefRecord = this.weakRefRecord;
      this.record = (SqlCeUpdatableRecord) null;
      this.weakRefRecord = (WeakReference) null;
      this.record = this.UpdatableRecord;
      if (!this.isDeleted)
        return;
      this.record = record;
      this.weakRefRecord = weakRefRecord;
    }

    string IDataErrorInfo.this[string colName] => string.Empty;

    string IDataErrorInfo.Error => this.error;

    internal string Error
    {
      set => this.error = value;
    }
  }
}
