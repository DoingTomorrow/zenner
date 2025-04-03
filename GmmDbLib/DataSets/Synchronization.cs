// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DataSets.Synchronization
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace GmmDbLib.DataSets
{
  [DesignerCategory("code")]
  [ToolboxItem(true)]
  [XmlSchemaProvider("GetTypedDataSetSchema")]
  [XmlRoot("Synchronization")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class Synchronization : DataSet
  {
    private Synchronization.SynchronizationJobsDataTable tableSynchronizationJobs;
    private Synchronization.SynchronizationTableInfoDataTable tableSynchronizationTableInfo;
    private Synchronization.SynchronizationJobTablesDataTable tableSynchronizationJobTables;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public Synchronization()
    {
      this.BeginInit();
      this.InitClass();
      CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
      base.Tables.CollectionChanged += changeEventHandler;
      base.Relations.CollectionChanged += changeEventHandler;
      this.EndInit();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected Synchronization(SerializationInfo info, StreamingContext context)
      : base(info, context, false)
    {
      if (this.IsBinarySerialized(info, context))
      {
        this.InitVars(false);
        CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
        this.Tables.CollectionChanged += changeEventHandler;
        this.Relations.CollectionChanged += changeEventHandler;
      }
      else
      {
        string s = (string) info.GetValue("XmlSchema", typeof (string));
        if (this.DetermineSchemaSerializationMode(info, context) == SchemaSerializationMode.IncludeSchema)
        {
          DataSet dataSet = new DataSet();
          dataSet.ReadXmlSchema((XmlReader) new XmlTextReader((TextReader) new StringReader(s)));
          if (dataSet.Tables[nameof (SynchronizationJobs)] != null)
            base.Tables.Add((DataTable) new Synchronization.SynchronizationJobsDataTable(dataSet.Tables[nameof (SynchronizationJobs)]));
          if (dataSet.Tables[nameof (SynchronizationTableInfo)] != null)
            base.Tables.Add((DataTable) new Synchronization.SynchronizationTableInfoDataTable(dataSet.Tables[nameof (SynchronizationTableInfo)]));
          if (dataSet.Tables[nameof (SynchronizationJobTables)] != null)
            base.Tables.Add((DataTable) new Synchronization.SynchronizationJobTablesDataTable(dataSet.Tables[nameof (SynchronizationJobTables)]));
          this.DataSetName = dataSet.DataSetName;
          this.Prefix = dataSet.Prefix;
          this.Namespace = dataSet.Namespace;
          this.Locale = dataSet.Locale;
          this.CaseSensitive = dataSet.CaseSensitive;
          this.EnforceConstraints = dataSet.EnforceConstraints;
          this.Merge(dataSet, false, MissingSchemaAction.Add);
          this.InitVars();
        }
        else
          this.ReadXmlSchema((XmlReader) new XmlTextReader((TextReader) new StringReader(s)));
        this.GetSerializationData(info, context);
        CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
        base.Tables.CollectionChanged += changeEventHandler;
        this.Relations.CollectionChanged += changeEventHandler;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Synchronization.SynchronizationJobsDataTable SynchronizationJobs
    {
      get => this.tableSynchronizationJobs;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Synchronization.SynchronizationTableInfoDataTable SynchronizationTableInfo
    {
      get => this.tableSynchronizationTableInfo;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public Synchronization.SynchronizationJobTablesDataTable SynchronizationJobTables
    {
      get => this.tableSynchronizationJobTables;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override SchemaSerializationMode SchemaSerializationMode
    {
      get => this._schemaSerializationMode;
      set => this._schemaSerializationMode = value;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataTableCollection Tables => base.Tables;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataRelationCollection Relations => base.Relations;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override void InitializeDerivedDataSet()
    {
      this.BeginInit();
      this.InitClass();
      this.EndInit();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public override DataSet Clone()
    {
      Synchronization synchronization = (Synchronization) base.Clone();
      synchronization.InitVars();
      synchronization.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) synchronization;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override bool ShouldSerializeTables() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override bool ShouldSerializeRelations() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override void ReadXmlSerializable(XmlReader reader)
    {
      if (this.DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
      {
        this.Reset();
        DataSet dataSet = new DataSet();
        int num = (int) dataSet.ReadXml(reader);
        if (dataSet.Tables["SynchronizationJobs"] != null)
          base.Tables.Add((DataTable) new Synchronization.SynchronizationJobsDataTable(dataSet.Tables["SynchronizationJobs"]));
        if (dataSet.Tables["SynchronizationTableInfo"] != null)
          base.Tables.Add((DataTable) new Synchronization.SynchronizationTableInfoDataTable(dataSet.Tables["SynchronizationTableInfo"]));
        if (dataSet.Tables["SynchronizationJobTables"] != null)
          base.Tables.Add((DataTable) new Synchronization.SynchronizationJobTablesDataTable(dataSet.Tables["SynchronizationJobTables"]));
        this.DataSetName = dataSet.DataSetName;
        this.Prefix = dataSet.Prefix;
        this.Namespace = dataSet.Namespace;
        this.Locale = dataSet.Locale;
        this.CaseSensitive = dataSet.CaseSensitive;
        this.EnforceConstraints = dataSet.EnforceConstraints;
        this.Merge(dataSet, false, MissingSchemaAction.Add);
        this.InitVars();
      }
      else
      {
        int num = (int) this.ReadXml(reader);
        this.InitVars();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    protected override XmlSchema GetSchemaSerializable()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.WriteXmlSchema((XmlWriter) new XmlTextWriter((Stream) memoryStream, (Encoding) null));
      memoryStream.Position = 0L;
      return XmlSchema.Read((XmlReader) new XmlTextReader((Stream) memoryStream), (ValidationEventHandler) null);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    internal void InitVars() => this.InitVars(true);

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    internal void InitVars(bool initTable)
    {
      this.tableSynchronizationJobs = (Synchronization.SynchronizationJobsDataTable) base.Tables["SynchronizationJobs"];
      if (initTable && this.tableSynchronizationJobs != null)
        this.tableSynchronizationJobs.InitVars();
      this.tableSynchronizationTableInfo = (Synchronization.SynchronizationTableInfoDataTable) base.Tables["SynchronizationTableInfo"];
      if (initTable && this.tableSynchronizationTableInfo != null)
        this.tableSynchronizationTableInfo.InitVars();
      this.tableSynchronizationJobTables = (Synchronization.SynchronizationJobTablesDataTable) base.Tables["SynchronizationJobTables"];
      if (!initTable || this.tableSynchronizationJobTables == null)
        return;
      this.tableSynchronizationJobTables.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (Synchronization);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/Synchronization.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableSynchronizationJobs = new Synchronization.SynchronizationJobsDataTable();
      base.Tables.Add((DataTable) this.tableSynchronizationJobs);
      this.tableSynchronizationTableInfo = new Synchronization.SynchronizationTableInfoDataTable();
      base.Tables.Add((DataTable) this.tableSynchronizationTableInfo);
      this.tableSynchronizationJobTables = new Synchronization.SynchronizationJobTablesDataTable();
      base.Tables.Add((DataTable) this.tableSynchronizationJobTables);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeSynchronizationJobs() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeSynchronizationTableInfo() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeSynchronizationJobTables() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void SchemaChanged(object sender, CollectionChangeEventArgs e)
    {
      if (e.Action != CollectionChangeAction.Remove)
        return;
      this.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
    {
      Synchronization synchronization = new Synchronization();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = synchronization.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = synchronization.GetSchemaSerializable();
      if (xs.Contains(schemaSerializable.TargetNamespace))
      {
        MemoryStream memoryStream1 = new MemoryStream();
        MemoryStream memoryStream2 = new MemoryStream();
        try
        {
          schemaSerializable.Write((Stream) memoryStream1);
          IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
          while (enumerator.MoveNext())
          {
            XmlSchema current = (XmlSchema) enumerator.Current;
            memoryStream2.SetLength(0L);
            current.Write((Stream) memoryStream2);
            if (memoryStream1.Length == memoryStream2.Length)
            {
              memoryStream1.Position = 0L;
              memoryStream2.Position = 0L;
              do
                ;
              while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
              if (memoryStream1.Position == memoryStream1.Length)
                return typedDataSetSchema;
            }
          }
        }
        finally
        {
          memoryStream1?.Close();
          memoryStream2?.Close();
        }
      }
      xs.Add(schemaSerializable);
      return typedDataSetSchema;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void SynchronizationJobsRowChangeEventHandler(
      object sender,
      Synchronization.SynchronizationJobsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void SynchronizationTableInfoRowChangeEventHandler(
      object sender,
      Synchronization.SynchronizationTableInfoRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void SynchronizationJobTablesRowChangeEventHandler(
      object sender,
      Synchronization.SynchronizationJobTablesRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class SynchronizationJobsDataTable : 
      TypedTableBase<Synchronization.SynchronizationJobsRow>
    {
      private DataColumn columnSyncJobID;
      private DataColumn columnJobGroup;
      private DataColumn columnJobName;
      private DataColumn columnFirstStartTime;
      private DataColumn columnCycleTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SynchronizationJobsDataTable()
      {
        this.TableName = "SynchronizationJobs";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SynchronizationJobsDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected SynchronizationJobsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SyncJobIDColumn => this.columnSyncJobID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn JobGroupColumn => this.columnJobGroup;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn JobNameColumn => this.columnJobName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FirstStartTimeColumn => this.columnFirstStartTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CycleTimeColumn => this.columnCycleTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobsRow this[int index]
      {
        get => (Synchronization.SynchronizationJobsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobsRowChangeEventHandler SynchronizationJobsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobsRowChangeEventHandler SynchronizationJobsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobsRowChangeEventHandler SynchronizationJobsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobsRowChangeEventHandler SynchronizationJobsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddSynchronizationJobsRow(Synchronization.SynchronizationJobsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobsRow AddSynchronizationJobsRow(
        int SyncJobID,
        string JobGroup,
        string JobName,
        DateTime FirstStartTime,
        DateTime CycleTime)
      {
        Synchronization.SynchronizationJobsRow row = (Synchronization.SynchronizationJobsRow) this.NewRow();
        object[] objArray = new object[5]
        {
          (object) SyncJobID,
          (object) JobGroup,
          (object) JobName,
          (object) FirstStartTime,
          (object) CycleTime
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        Synchronization.SynchronizationJobsDataTable synchronizationJobsDataTable = (Synchronization.SynchronizationJobsDataTable) base.Clone();
        synchronizationJobsDataTable.InitVars();
        return (DataTable) synchronizationJobsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new Synchronization.SynchronizationJobsDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnSyncJobID = this.Columns["SyncJobID"];
        this.columnJobGroup = this.Columns["JobGroup"];
        this.columnJobName = this.Columns["JobName"];
        this.columnFirstStartTime = this.Columns["FirstStartTime"];
        this.columnCycleTime = this.Columns["CycleTime"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnSyncJobID = new DataColumn("SyncJobID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSyncJobID);
        this.columnJobGroup = new DataColumn("JobGroup", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnJobGroup);
        this.columnJobName = new DataColumn("JobName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnJobName);
        this.columnFirstStartTime = new DataColumn("FirstStartTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirstStartTime);
        this.columnCycleTime = new DataColumn("CycleTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCycleTime);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnSyncJobID
        }, false));
        this.columnSyncJobID.Unique = true;
        this.columnJobGroup.MaxLength = (int) byte.MaxValue;
        this.columnJobName.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobsRow NewSynchronizationJobsRow()
      {
        return (Synchronization.SynchronizationJobsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new Synchronization.SynchronizationJobsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (Synchronization.SynchronizationJobsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.SynchronizationJobsRowChanged == null)
          return;
        this.SynchronizationJobsRowChanged((object) this, new Synchronization.SynchronizationJobsRowChangeEvent((Synchronization.SynchronizationJobsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.SynchronizationJobsRowChanging == null)
          return;
        this.SynchronizationJobsRowChanging((object) this, new Synchronization.SynchronizationJobsRowChangeEvent((Synchronization.SynchronizationJobsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.SynchronizationJobsRowDeleted == null)
          return;
        this.SynchronizationJobsRowDeleted((object) this, new Synchronization.SynchronizationJobsRowChangeEvent((Synchronization.SynchronizationJobsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.SynchronizationJobsRowDeleting == null)
          return;
        this.SynchronizationJobsRowDeleting((object) this, new Synchronization.SynchronizationJobsRowChangeEvent((Synchronization.SynchronizationJobsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveSynchronizationJobsRow(Synchronization.SynchronizationJobsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        Synchronization synchronization = new Synchronization();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = 0M;
        xmlSchemaAny1.MaxOccurs = Decimal.MaxValue;
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = 1M;
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = synchronization.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (SynchronizationJobsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = synchronization.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
            while (enumerator.MoveNext())
            {
              XmlSchema current = (XmlSchema) enumerator.Current;
              memoryStream2.SetLength(0L);
              current.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return typedTableSchema;
              }
            }
          }
          finally
          {
            memoryStream1?.Close();
            memoryStream2?.Close();
          }
        }
        xs.Add(schemaSerializable);
        return typedTableSchema;
      }
    }

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class SynchronizationTableInfoDataTable : 
      TypedTableBase<Synchronization.SynchronizationTableInfoRow>
    {
      private DataColumn columnTableName;
      private DataColumn columnTableUsing;
      private DataColumn columnID_Generation;
      private DataColumn columnIdColumnName;
      private DataColumn columnSyncType;
      private DataColumn columnRecordsNeverChanged;
      private DataColumn columnPartOfDatabaseSynchronisation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SynchronizationTableInfoDataTable()
      {
        this.TableName = "SynchronizationTableInfo";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SynchronizationTableInfoDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected SynchronizationTableInfoDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TableNameColumn => this.columnTableName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TableUsingColumn => this.columnTableUsing;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ID_GenerationColumn => this.columnID_Generation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn IdColumnNameColumn => this.columnIdColumnName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SyncTypeColumn => this.columnSyncType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecordsNeverChangedColumn => this.columnRecordsNeverChanged;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PartOfDatabaseSynchronisationColumn
      {
        get => this.columnPartOfDatabaseSynchronisation;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationTableInfoRow this[int index]
      {
        get => (Synchronization.SynchronizationTableInfoRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationTableInfoRowChangeEventHandler SynchronizationTableInfoRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationTableInfoRowChangeEventHandler SynchronizationTableInfoRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationTableInfoRowChangeEventHandler SynchronizationTableInfoRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationTableInfoRowChangeEventHandler SynchronizationTableInfoRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddSynchronizationTableInfoRow(Synchronization.SynchronizationTableInfoRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationTableInfoRow AddSynchronizationTableInfoRow(
        string TableName,
        string TableUsing,
        string ID_Generation,
        string IdColumnName,
        string SyncType,
        bool RecordsNeverChanged,
        bool PartOfDatabaseSynchronisation)
      {
        Synchronization.SynchronizationTableInfoRow row = (Synchronization.SynchronizationTableInfoRow) this.NewRow();
        object[] objArray = new object[7]
        {
          (object) TableName,
          (object) TableUsing,
          (object) ID_Generation,
          (object) IdColumnName,
          (object) SyncType,
          (object) RecordsNeverChanged,
          (object) PartOfDatabaseSynchronisation
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationTableInfoRow FindByTableName(string TableName)
      {
        return (Synchronization.SynchronizationTableInfoRow) this.Rows.Find(new object[1]
        {
          (object) TableName
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        Synchronization.SynchronizationTableInfoDataTable tableInfoDataTable = (Synchronization.SynchronizationTableInfoDataTable) base.Clone();
        tableInfoDataTable.InitVars();
        return (DataTable) tableInfoDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new Synchronization.SynchronizationTableInfoDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnTableName = this.Columns["TableName"];
        this.columnTableUsing = this.Columns["TableUsing"];
        this.columnID_Generation = this.Columns["ID_Generation"];
        this.columnIdColumnName = this.Columns["IdColumnName"];
        this.columnSyncType = this.Columns["SyncType"];
        this.columnRecordsNeverChanged = this.Columns["RecordsNeverChanged"];
        this.columnPartOfDatabaseSynchronisation = this.Columns["PartOfDatabaseSynchronisation"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnTableName = new DataColumn("TableName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTableName);
        this.columnTableUsing = new DataColumn("TableUsing", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTableUsing);
        this.columnID_Generation = new DataColumn("ID_Generation", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnID_Generation);
        this.columnIdColumnName = new DataColumn("IdColumnName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnIdColumnName);
        this.columnSyncType = new DataColumn("SyncType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSyncType);
        this.columnRecordsNeverChanged = new DataColumn("RecordsNeverChanged", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecordsNeverChanged);
        this.columnPartOfDatabaseSynchronisation = new DataColumn("PartOfDatabaseSynchronisation", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPartOfDatabaseSynchronisation);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnTableName
        }, true));
        this.columnTableName.AllowDBNull = false;
        this.columnTableName.Unique = true;
        this.columnTableName.MaxLength = 50;
        this.columnTableUsing.MaxLength = 536870910;
        this.columnID_Generation.MaxLength = 536870910;
        this.columnIdColumnName.MaxLength = 50;
        this.columnSyncType.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationTableInfoRow NewSynchronizationTableInfoRow()
      {
        return (Synchronization.SynchronizationTableInfoRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new Synchronization.SynchronizationTableInfoRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (Synchronization.SynchronizationTableInfoRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.SynchronizationTableInfoRowChanged == null)
          return;
        this.SynchronizationTableInfoRowChanged((object) this, new Synchronization.SynchronizationTableInfoRowChangeEvent((Synchronization.SynchronizationTableInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.SynchronizationTableInfoRowChanging == null)
          return;
        this.SynchronizationTableInfoRowChanging((object) this, new Synchronization.SynchronizationTableInfoRowChangeEvent((Synchronization.SynchronizationTableInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.SynchronizationTableInfoRowDeleted == null)
          return;
        this.SynchronizationTableInfoRowDeleted((object) this, new Synchronization.SynchronizationTableInfoRowChangeEvent((Synchronization.SynchronizationTableInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.SynchronizationTableInfoRowDeleting == null)
          return;
        this.SynchronizationTableInfoRowDeleting((object) this, new Synchronization.SynchronizationTableInfoRowChangeEvent((Synchronization.SynchronizationTableInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveSynchronizationTableInfoRow(Synchronization.SynchronizationTableInfoRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        Synchronization synchronization = new Synchronization();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = 0M;
        xmlSchemaAny1.MaxOccurs = Decimal.MaxValue;
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = 1M;
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = synchronization.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (SynchronizationTableInfoDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = synchronization.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
            while (enumerator.MoveNext())
            {
              XmlSchema current = (XmlSchema) enumerator.Current;
              memoryStream2.SetLength(0L);
              current.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return typedTableSchema;
              }
            }
          }
          finally
          {
            memoryStream1?.Close();
            memoryStream2?.Close();
          }
        }
        xs.Add(schemaSerializable);
        return typedTableSchema;
      }
    }

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class SynchronizationJobTablesDataTable : 
      TypedTableBase<Synchronization.SynchronizationJobTablesRow>
    {
      private DataColumn columnSyncJobID;
      private DataColumn columnTableOrder;
      private DataColumn columnTableName;
      private DataColumn columnSyncBack;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SynchronizationJobTablesDataTable()
      {
        this.TableName = "SynchronizationJobTables";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SynchronizationJobTablesDataTable(DataTable table)
      {
        this.TableName = table.TableName;
        if (table.CaseSensitive != table.DataSet.CaseSensitive)
          this.CaseSensitive = table.CaseSensitive;
        if (table.Locale.ToString() != table.DataSet.Locale.ToString())
          this.Locale = table.Locale;
        if (table.Namespace != table.DataSet.Namespace)
          this.Namespace = table.Namespace;
        this.Prefix = table.Prefix;
        this.MinimumCapacity = table.MinimumCapacity;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected SynchronizationJobTablesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SyncJobIDColumn => this.columnSyncJobID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TableOrderColumn => this.columnTableOrder;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TableNameColumn => this.columnTableName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SyncBackColumn => this.columnSyncBack;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobTablesRow this[int index]
      {
        get => (Synchronization.SynchronizationJobTablesRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobTablesRowChangeEventHandler SynchronizationJobTablesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobTablesRowChangeEventHandler SynchronizationJobTablesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobTablesRowChangeEventHandler SynchronizationJobTablesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event Synchronization.SynchronizationJobTablesRowChangeEventHandler SynchronizationJobTablesRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddSynchronizationJobTablesRow(Synchronization.SynchronizationJobTablesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobTablesRow AddSynchronizationJobTablesRow(
        int SyncJobID,
        int TableOrder,
        string TableName,
        bool SyncBack)
      {
        Synchronization.SynchronizationJobTablesRow row = (Synchronization.SynchronizationJobTablesRow) this.NewRow();
        object[] objArray = new object[4]
        {
          (object) SyncJobID,
          (object) TableOrder,
          (object) TableName,
          (object) SyncBack
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobTablesRow FindBySyncJobID(int SyncJobID)
      {
        return (Synchronization.SynchronizationJobTablesRow) this.Rows.Find(new object[1]
        {
          (object) SyncJobID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        Synchronization.SynchronizationJobTablesDataTable jobTablesDataTable = (Synchronization.SynchronizationJobTablesDataTable) base.Clone();
        jobTablesDataTable.InitVars();
        return (DataTable) jobTablesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new Synchronization.SynchronizationJobTablesDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnSyncJobID = this.Columns["SyncJobID"];
        this.columnTableOrder = this.Columns["TableOrder"];
        this.columnTableName = this.Columns["TableName"];
        this.columnSyncBack = this.Columns["SyncBack"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnSyncJobID = new DataColumn("SyncJobID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSyncJobID);
        this.columnTableOrder = new DataColumn("TableOrder", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTableOrder);
        this.columnTableName = new DataColumn("TableName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTableName);
        this.columnSyncBack = new DataColumn("SyncBack", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSyncBack);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnSyncJobID
        }, true));
        this.columnSyncJobID.AllowDBNull = false;
        this.columnSyncJobID.Unique = true;
        this.columnTableName.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobTablesRow NewSynchronizationJobTablesRow()
      {
        return (Synchronization.SynchronizationJobTablesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new Synchronization.SynchronizationJobTablesRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (Synchronization.SynchronizationJobTablesRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.SynchronizationJobTablesRowChanged == null)
          return;
        this.SynchronizationJobTablesRowChanged((object) this, new Synchronization.SynchronizationJobTablesRowChangeEvent((Synchronization.SynchronizationJobTablesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.SynchronizationJobTablesRowChanging == null)
          return;
        this.SynchronizationJobTablesRowChanging((object) this, new Synchronization.SynchronizationJobTablesRowChangeEvent((Synchronization.SynchronizationJobTablesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.SynchronizationJobTablesRowDeleted == null)
          return;
        this.SynchronizationJobTablesRowDeleted((object) this, new Synchronization.SynchronizationJobTablesRowChangeEvent((Synchronization.SynchronizationJobTablesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.SynchronizationJobTablesRowDeleting == null)
          return;
        this.SynchronizationJobTablesRowDeleting((object) this, new Synchronization.SynchronizationJobTablesRowChangeEvent((Synchronization.SynchronizationJobTablesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveSynchronizationJobTablesRow(Synchronization.SynchronizationJobTablesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        Synchronization synchronization = new Synchronization();
        XmlSchemaAny xmlSchemaAny1 = new XmlSchemaAny();
        xmlSchemaAny1.Namespace = "http://www.w3.org/2001/XMLSchema";
        xmlSchemaAny1.MinOccurs = 0M;
        xmlSchemaAny1.MaxOccurs = Decimal.MaxValue;
        xmlSchemaAny1.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny1);
        XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
        xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
        xmlSchemaAny2.MinOccurs = 1M;
        xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
        xmlSchemaSequence.Items.Add((XmlSchemaObject) xmlSchemaAny2);
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "namespace",
          FixedValue = synchronization.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (SynchronizationJobTablesDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = synchronization.GetSchemaSerializable();
        if (xs.Contains(schemaSerializable.TargetNamespace))
        {
          MemoryStream memoryStream1 = new MemoryStream();
          MemoryStream memoryStream2 = new MemoryStream();
          try
          {
            schemaSerializable.Write((Stream) memoryStream1);
            IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
            while (enumerator.MoveNext())
            {
              XmlSchema current = (XmlSchema) enumerator.Current;
              memoryStream2.SetLength(0L);
              current.Write((Stream) memoryStream2);
              if (memoryStream1.Length == memoryStream2.Length)
              {
                memoryStream1.Position = 0L;
                memoryStream2.Position = 0L;
                do
                  ;
                while (memoryStream1.Position != memoryStream1.Length && memoryStream1.ReadByte() == memoryStream2.ReadByte());
                if (memoryStream1.Position == memoryStream1.Length)
                  return typedTableSchema;
              }
            }
          }
          finally
          {
            memoryStream1?.Close();
            memoryStream2?.Close();
          }
        }
        xs.Add(schemaSerializable);
        return typedTableSchema;
      }
    }

    public class SynchronizationJobsRow : DataRow
    {
      private Synchronization.SynchronizationJobsDataTable tableSynchronizationJobs;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SynchronizationJobsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableSynchronizationJobs = (Synchronization.SynchronizationJobsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int SyncJobID
      {
        get
        {
          try
          {
            return (int) this[this.tableSynchronizationJobs.SyncJobIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SyncJobID' in table 'SynchronizationJobs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobs.SyncJobIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string JobGroup
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationJobs.JobGroupColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'JobGroup' in table 'SynchronizationJobs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobs.JobGroupColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string JobName
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationJobs.JobNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'JobName' in table 'SynchronizationJobs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobs.JobNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime FirstStartTime
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableSynchronizationJobs.FirstStartTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirstStartTime' in table 'SynchronizationJobs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobs.FirstStartTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime CycleTime
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableSynchronizationJobs.CycleTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CycleTime' in table 'SynchronizationJobs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobs.CycleTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSyncJobIDNull() => this.IsNull(this.tableSynchronizationJobs.SyncJobIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSyncJobIDNull()
      {
        this[this.tableSynchronizationJobs.SyncJobIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsJobGroupNull() => this.IsNull(this.tableSynchronizationJobs.JobGroupColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetJobGroupNull()
      {
        this[this.tableSynchronizationJobs.JobGroupColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsJobNameNull() => this.IsNull(this.tableSynchronizationJobs.JobNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetJobNameNull()
      {
        this[this.tableSynchronizationJobs.JobNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFirstStartTimeNull()
      {
        return this.IsNull(this.tableSynchronizationJobs.FirstStartTimeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFirstStartTimeNull()
      {
        this[this.tableSynchronizationJobs.FirstStartTimeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCycleTimeNull() => this.IsNull(this.tableSynchronizationJobs.CycleTimeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCycleTimeNull()
      {
        this[this.tableSynchronizationJobs.CycleTimeColumn] = Convert.DBNull;
      }
    }

    public class SynchronizationTableInfoRow : DataRow
    {
      private Synchronization.SynchronizationTableInfoDataTable tableSynchronizationTableInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SynchronizationTableInfoRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableSynchronizationTableInfo = (Synchronization.SynchronizationTableInfoDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string TableName
      {
        get => (string) this[this.tableSynchronizationTableInfo.TableNameColumn];
        set => this[this.tableSynchronizationTableInfo.TableNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string TableUsing
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationTableInfo.TableUsingColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TableUsing' in table 'SynchronizationTableInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationTableInfo.TableUsingColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ID_Generation
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationTableInfo.ID_GenerationColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ID_Generation' in table 'SynchronizationTableInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationTableInfo.ID_GenerationColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string IdColumnName
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationTableInfo.IdColumnNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'IdColumnName' in table 'SynchronizationTableInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationTableInfo.IdColumnNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SyncType
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationTableInfo.SyncTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SyncType' in table 'SynchronizationTableInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationTableInfo.SyncTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool RecordsNeverChanged
      {
        get
        {
          try
          {
            return (bool) this[this.tableSynchronizationTableInfo.RecordsNeverChangedColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecordsNeverChanged' in table 'SynchronizationTableInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationTableInfo.RecordsNeverChangedColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool PartOfDatabaseSynchronisation
      {
        get
        {
          try
          {
            return (bool) this[this.tableSynchronizationTableInfo.PartOfDatabaseSynchronisationColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PartOfDatabaseSynchronisation' in table 'SynchronizationTableInfo' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableSynchronizationTableInfo.PartOfDatabaseSynchronisationColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTableUsingNull()
      {
        return this.IsNull(this.tableSynchronizationTableInfo.TableUsingColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTableUsingNull()
      {
        this[this.tableSynchronizationTableInfo.TableUsingColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsID_GenerationNull()
      {
        return this.IsNull(this.tableSynchronizationTableInfo.ID_GenerationColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetID_GenerationNull()
      {
        this[this.tableSynchronizationTableInfo.ID_GenerationColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsIdColumnNameNull()
      {
        return this.IsNull(this.tableSynchronizationTableInfo.IdColumnNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetIdColumnNameNull()
      {
        this[this.tableSynchronizationTableInfo.IdColumnNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSyncTypeNull()
      {
        return this.IsNull(this.tableSynchronizationTableInfo.SyncTypeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSyncTypeNull()
      {
        this[this.tableSynchronizationTableInfo.SyncTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecordsNeverChangedNull()
      {
        return this.IsNull(this.tableSynchronizationTableInfo.RecordsNeverChangedColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecordsNeverChangedNull()
      {
        this[this.tableSynchronizationTableInfo.RecordsNeverChangedColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPartOfDatabaseSynchronisationNull()
      {
        return this.IsNull(this.tableSynchronizationTableInfo.PartOfDatabaseSynchronisationColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPartOfDatabaseSynchronisationNull()
      {
        this[this.tableSynchronizationTableInfo.PartOfDatabaseSynchronisationColumn] = Convert.DBNull;
      }
    }

    public class SynchronizationJobTablesRow : DataRow
    {
      private Synchronization.SynchronizationJobTablesDataTable tableSynchronizationJobTables;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SynchronizationJobTablesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableSynchronizationJobTables = (Synchronization.SynchronizationJobTablesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int SyncJobID
      {
        get => (int) this[this.tableSynchronizationJobTables.SyncJobIDColumn];
        set => this[this.tableSynchronizationJobTables.SyncJobIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int TableOrder
      {
        get
        {
          try
          {
            return (int) this[this.tableSynchronizationJobTables.TableOrderColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TableOrder' in table 'SynchronizationJobTables' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobTables.TableOrderColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string TableName
      {
        get
        {
          try
          {
            return (string) this[this.tableSynchronizationJobTables.TableNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TableName' in table 'SynchronizationJobTables' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobTables.TableNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool SyncBack
      {
        get
        {
          try
          {
            return (bool) this[this.tableSynchronizationJobTables.SyncBackColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SyncBack' in table 'SynchronizationJobTables' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSynchronizationJobTables.SyncBackColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTableOrderNull()
      {
        return this.IsNull(this.tableSynchronizationJobTables.TableOrderColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTableOrderNull()
      {
        this[this.tableSynchronizationJobTables.TableOrderColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTableNameNull()
      {
        return this.IsNull(this.tableSynchronizationJobTables.TableNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTableNameNull()
      {
        this[this.tableSynchronizationJobTables.TableNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSyncBackNull()
      {
        return this.IsNull(this.tableSynchronizationJobTables.SyncBackColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSyncBackNull()
      {
        this[this.tableSynchronizationJobTables.SyncBackColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class SynchronizationJobsRowChangeEvent : EventArgs
    {
      private Synchronization.SynchronizationJobsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SynchronizationJobsRowChangeEvent(
        Synchronization.SynchronizationJobsRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class SynchronizationTableInfoRowChangeEvent : EventArgs
    {
      private Synchronization.SynchronizationTableInfoRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SynchronizationTableInfoRowChangeEvent(
        Synchronization.SynchronizationTableInfoRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationTableInfoRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class SynchronizationJobTablesRowChangeEvent : EventArgs
    {
      private Synchronization.SynchronizationJobTablesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SynchronizationJobTablesRowChangeEvent(
        Synchronization.SynchronizationJobTablesRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Synchronization.SynchronizationJobTablesRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
