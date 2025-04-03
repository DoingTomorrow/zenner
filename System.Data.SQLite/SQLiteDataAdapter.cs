// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDataAdapter
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.ComponentModel;
using System.Data.Common;

#nullable disable
namespace System.Data.SQLite
{
  [Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
  [ToolboxItem("SQLite.Designer.SQLiteDataAdapterToolboxItem, SQLite.Designer, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
  [DefaultEvent("RowUpdated")]
  public sealed class SQLiteDataAdapter : DbDataAdapter
  {
    private bool disposeSelect = true;
    private static object _updatingEventPH = new object();
    private static object _updatedEventPH = new object();
    private bool disposed;

    public SQLiteDataAdapter()
    {
    }

    public SQLiteDataAdapter(SQLiteCommand cmd)
    {
      this.SelectCommand = cmd;
      this.disposeSelect = false;
    }

    public SQLiteDataAdapter(string commandText, SQLiteConnection connection)
    {
      this.SelectCommand = new SQLiteCommand(commandText, connection);
    }

    public SQLiteDataAdapter(string commandText, string connectionString)
      : this(commandText, connectionString, false)
    {
    }

    public SQLiteDataAdapter(string commandText, string connectionString, bool parseViaFramework)
    {
      SQLiteConnection connection = new SQLiteConnection(connectionString, parseViaFramework);
      this.SelectCommand = new SQLiteCommand(commandText, connection);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteDataAdapter).Name);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing)
          return;
        if (this.disposeSelect && this.SelectCommand != null)
        {
          this.SelectCommand.Dispose();
          this.SelectCommand = (SQLiteCommand) null;
        }
        if (this.InsertCommand != null)
        {
          this.InsertCommand.Dispose();
          this.InsertCommand = (SQLiteCommand) null;
        }
        if (this.UpdateCommand != null)
        {
          this.UpdateCommand.Dispose();
          this.UpdateCommand = (SQLiteCommand) null;
        }
        if (this.DeleteCommand == null)
          return;
        this.DeleteCommand.Dispose();
        this.DeleteCommand = (SQLiteCommand) null;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    public event EventHandler<RowUpdatingEventArgs> RowUpdating
    {
      add
      {
        this.CheckDisposed();
        EventHandler<RowUpdatingEventArgs> mcd = (EventHandler<RowUpdatingEventArgs>) this.Events[SQLiteDataAdapter._updatingEventPH];
        if (mcd != null && value.Target is DbCommandBuilder)
        {
          EventHandler<RowUpdatingEventArgs> builder = (EventHandler<RowUpdatingEventArgs>) SQLiteDataAdapter.FindBuilder((MulticastDelegate) mcd);
          if (builder != null)
            this.Events.RemoveHandler(SQLiteDataAdapter._updatingEventPH, (Delegate) builder);
        }
        this.Events.AddHandler(SQLiteDataAdapter._updatingEventPH, (Delegate) value);
      }
      remove
      {
        this.CheckDisposed();
        this.Events.RemoveHandler(SQLiteDataAdapter._updatingEventPH, (Delegate) value);
      }
    }

    internal static Delegate FindBuilder(MulticastDelegate mcd)
    {
      if ((object) mcd != null)
      {
        Delegate[] invocationList = mcd.GetInvocationList();
        for (int index = 0; index < invocationList.Length; ++index)
        {
          if (invocationList[index].Target is DbCommandBuilder)
            return invocationList[index];
        }
      }
      return (Delegate) null;
    }

    public event EventHandler<RowUpdatedEventArgs> RowUpdated
    {
      add
      {
        this.CheckDisposed();
        this.Events.AddHandler(SQLiteDataAdapter._updatedEventPH, (Delegate) value);
      }
      remove
      {
        this.CheckDisposed();
        this.Events.RemoveHandler(SQLiteDataAdapter._updatedEventPH, (Delegate) value);
      }
    }

    protected override void OnRowUpdating(RowUpdatingEventArgs value)
    {
      if (!(this.Events[SQLiteDataAdapter._updatingEventPH] is EventHandler<RowUpdatingEventArgs> eventHandler))
        return;
      eventHandler((object) this, value);
    }

    protected override void OnRowUpdated(RowUpdatedEventArgs value)
    {
      if (!(this.Events[SQLiteDataAdapter._updatedEventPH] is EventHandler<RowUpdatedEventArgs> eventHandler))
        return;
      eventHandler((object) this, value);
    }

    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DefaultValue(null)]
    public SQLiteCommand SelectCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.SelectCommand;
      }
      set
      {
        this.CheckDisposed();
        this.SelectCommand = (DbCommand) value;
      }
    }

    [DefaultValue(null)]
    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public SQLiteCommand InsertCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.InsertCommand;
      }
      set
      {
        this.CheckDisposed();
        this.InsertCommand = (DbCommand) value;
      }
    }

    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DefaultValue(null)]
    public SQLiteCommand UpdateCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.UpdateCommand;
      }
      set
      {
        this.CheckDisposed();
        this.UpdateCommand = (DbCommand) value;
      }
    }

    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DefaultValue(null)]
    public SQLiteCommand DeleteCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.DeleteCommand;
      }
      set
      {
        this.CheckDisposed();
        this.DeleteCommand = (DbCommand) value;
      }
    }
  }
}
