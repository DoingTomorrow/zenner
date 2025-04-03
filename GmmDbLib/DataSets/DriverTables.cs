// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DataSets.DriverTables
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
  [XmlRoot("DriverTables")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class DriverTables : DataSet
  {
    private DriverTables.MeterMSSDataTable tableMeterMSS;
    private DriverTables.MeterValuesMSSDataTable tableMeterValuesMSS;
    private DriverTables.ServiceTaskResultDataTable tableServiceTaskResult;
    private DriverTables.MinomatListDataTable tableMinomatList;
    private DriverTables.MinomatDataLogsDataTable tableMinomatDataLogs;
    private DriverTables.MinomatConnectionLogsDataTable tableMinomatConnectionLogs;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public DriverTables()
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
    protected DriverTables(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (MeterMSS)] != null)
            base.Tables.Add((DataTable) new DriverTables.MeterMSSDataTable(dataSet.Tables[nameof (MeterMSS)]));
          if (dataSet.Tables[nameof (MeterValuesMSS)] != null)
            base.Tables.Add((DataTable) new DriverTables.MeterValuesMSSDataTable(dataSet.Tables[nameof (MeterValuesMSS)]));
          if (dataSet.Tables[nameof (ServiceTaskResult)] != null)
            base.Tables.Add((DataTable) new DriverTables.ServiceTaskResultDataTable(dataSet.Tables[nameof (ServiceTaskResult)]));
          if (dataSet.Tables[nameof (MinomatList)] != null)
            base.Tables.Add((DataTable) new DriverTables.MinomatListDataTable(dataSet.Tables[nameof (MinomatList)]));
          if (dataSet.Tables[nameof (MinomatDataLogs)] != null)
            base.Tables.Add((DataTable) new DriverTables.MinomatDataLogsDataTable(dataSet.Tables[nameof (MinomatDataLogs)]));
          if (dataSet.Tables[nameof (MinomatConnectionLogs)] != null)
            base.Tables.Add((DataTable) new DriverTables.MinomatConnectionLogsDataTable(dataSet.Tables[nameof (MinomatConnectionLogs)]));
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
    public DriverTables.MeterMSSDataTable MeterMSS => this.tableMeterMSS;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DriverTables.MeterValuesMSSDataTable MeterValuesMSS => this.tableMeterValuesMSS;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DriverTables.ServiceTaskResultDataTable ServiceTaskResult => this.tableServiceTaskResult;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DriverTables.MinomatListDataTable MinomatList => this.tableMinomatList;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DriverTables.MinomatDataLogsDataTable MinomatDataLogs => this.tableMinomatDataLogs;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DriverTables.MinomatConnectionLogsDataTable MinomatConnectionLogs
    {
      get => this.tableMinomatConnectionLogs;
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
      DriverTables driverTables = (DriverTables) base.Clone();
      driverTables.InitVars();
      driverTables.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) driverTables;
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
        if (dataSet.Tables["MeterMSS"] != null)
          base.Tables.Add((DataTable) new DriverTables.MeterMSSDataTable(dataSet.Tables["MeterMSS"]));
        if (dataSet.Tables["MeterValuesMSS"] != null)
          base.Tables.Add((DataTable) new DriverTables.MeterValuesMSSDataTable(dataSet.Tables["MeterValuesMSS"]));
        if (dataSet.Tables["ServiceTaskResult"] != null)
          base.Tables.Add((DataTable) new DriverTables.ServiceTaskResultDataTable(dataSet.Tables["ServiceTaskResult"]));
        if (dataSet.Tables["MinomatList"] != null)
          base.Tables.Add((DataTable) new DriverTables.MinomatListDataTable(dataSet.Tables["MinomatList"]));
        if (dataSet.Tables["MinomatDataLogs"] != null)
          base.Tables.Add((DataTable) new DriverTables.MinomatDataLogsDataTable(dataSet.Tables["MinomatDataLogs"]));
        if (dataSet.Tables["MinomatConnectionLogs"] != null)
          base.Tables.Add((DataTable) new DriverTables.MinomatConnectionLogsDataTable(dataSet.Tables["MinomatConnectionLogs"]));
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
      this.tableMeterMSS = (DriverTables.MeterMSSDataTable) base.Tables["MeterMSS"];
      if (initTable && this.tableMeterMSS != null)
        this.tableMeterMSS.InitVars();
      this.tableMeterValuesMSS = (DriverTables.MeterValuesMSSDataTable) base.Tables["MeterValuesMSS"];
      if (initTable && this.tableMeterValuesMSS != null)
        this.tableMeterValuesMSS.InitVars();
      this.tableServiceTaskResult = (DriverTables.ServiceTaskResultDataTable) base.Tables["ServiceTaskResult"];
      if (initTable && this.tableServiceTaskResult != null)
        this.tableServiceTaskResult.InitVars();
      this.tableMinomatList = (DriverTables.MinomatListDataTable) base.Tables["MinomatList"];
      if (initTable && this.tableMinomatList != null)
        this.tableMinomatList.InitVars();
      this.tableMinomatDataLogs = (DriverTables.MinomatDataLogsDataTable) base.Tables["MinomatDataLogs"];
      if (initTable && this.tableMinomatDataLogs != null)
        this.tableMinomatDataLogs.InitVars();
      this.tableMinomatConnectionLogs = (DriverTables.MinomatConnectionLogsDataTable) base.Tables["MinomatConnectionLogs"];
      if (!initTable || this.tableMinomatConnectionLogs == null)
        return;
      this.tableMinomatConnectionLogs.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (DriverTables);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/DriverTables.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableMeterMSS = new DriverTables.MeterMSSDataTable();
      base.Tables.Add((DataTable) this.tableMeterMSS);
      this.tableMeterValuesMSS = new DriverTables.MeterValuesMSSDataTable();
      base.Tables.Add((DataTable) this.tableMeterValuesMSS);
      this.tableServiceTaskResult = new DriverTables.ServiceTaskResultDataTable();
      base.Tables.Add((DataTable) this.tableServiceTaskResult);
      this.tableMinomatList = new DriverTables.MinomatListDataTable();
      base.Tables.Add((DataTable) this.tableMinomatList);
      this.tableMinomatDataLogs = new DriverTables.MinomatDataLogsDataTable();
      base.Tables.Add((DataTable) this.tableMinomatDataLogs);
      this.tableMinomatConnectionLogs = new DriverTables.MinomatConnectionLogsDataTable();
      base.Tables.Add((DataTable) this.tableMinomatConnectionLogs);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMeterMSS() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMeterValuesMSS() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeServiceTaskResult() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMinomatList() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMinomatDataLogs() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMinomatConnectionLogs() => false;

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
      DriverTables driverTables = new DriverTables();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = driverTables.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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
    public delegate void MeterMSSRowChangeEventHandler(
      object sender,
      DriverTables.MeterMSSRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void MeterValuesMSSRowChangeEventHandler(
      object sender,
      DriverTables.MeterValuesMSSRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void ServiceTaskResultRowChangeEventHandler(
      object sender,
      DriverTables.ServiceTaskResultRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void MinomatListRowChangeEventHandler(
      object sender,
      DriverTables.MinomatListRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void MinomatDataLogsRowChangeEventHandler(
      object sender,
      DriverTables.MinomatDataLogsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void MinomatConnectionLogsRowChangeEventHandler(
      object sender,
      DriverTables.MinomatConnectionLogsRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class MeterMSSDataTable : TypedTableBase<DriverTables.MeterMSSRow>
    {
      private DataColumn columnMeterID;
      private DataColumn columnSerialNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterMSSDataTable()
      {
        this.TableName = "MeterMSS";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterMSSDataTable(DataTable table)
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
      protected MeterMSSDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerialNumberColumn => this.columnSerialNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterMSSRow this[int index]
      {
        get => (DriverTables.MeterMSSRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterMSSRowChangeEventHandler MeterMSSRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterMSSRowChangeEventHandler MeterMSSRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterMSSRowChangeEventHandler MeterMSSRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterMSSRowChangeEventHandler MeterMSSRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMeterMSSRow(DriverTables.MeterMSSRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterMSSRow AddMeterMSSRow(Guid MeterID, string SerialNumber)
      {
        DriverTables.MeterMSSRow row = (DriverTables.MeterMSSRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) MeterID,
          (object) SerialNumber
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterMSSRow FindByMeterIDSerialNumber(Guid MeterID, string SerialNumber)
      {
        return (DriverTables.MeterMSSRow) this.Rows.Find(new object[2]
        {
          (object) MeterID,
          (object) SerialNumber
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DriverTables.MeterMSSDataTable meterMssDataTable = (DriverTables.MeterMSSDataTable) base.Clone();
        meterMssDataTable.InitVars();
        return (DataTable) meterMssDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DriverTables.MeterMSSDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnMeterID = this.Columns["MeterID"];
        this.columnSerialNumber = this.Columns["SerialNumber"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnMeterID = new DataColumn("MeterID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnSerialNumber = new DataColumn("SerialNumber", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerialNumber);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnMeterID,
          this.columnSerialNumber
        }, true));
        this.columnMeterID.AllowDBNull = false;
        this.columnSerialNumber.AllowDBNull = false;
        this.columnSerialNumber.MaxLength = 100;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterMSSRow NewMeterMSSRow() => (DriverTables.MeterMSSRow) this.NewRow();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DriverTables.MeterMSSRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DriverTables.MeterMSSRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterMSSRowChanged == null)
          return;
        this.MeterMSSRowChanged((object) this, new DriverTables.MeterMSSRowChangeEvent((DriverTables.MeterMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterMSSRowChanging == null)
          return;
        this.MeterMSSRowChanging((object) this, new DriverTables.MeterMSSRowChangeEvent((DriverTables.MeterMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterMSSRowDeleted == null)
          return;
        this.MeterMSSRowDeleted((object) this, new DriverTables.MeterMSSRowChangeEvent((DriverTables.MeterMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterMSSRowDeleting == null)
          return;
        this.MeterMSSRowDeleting((object) this, new DriverTables.MeterMSSRowChangeEvent((DriverTables.MeterMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMeterMSSRow(DriverTables.MeterMSSRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DriverTables driverTables = new DriverTables();
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
          FixedValue = driverTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterMSSDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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
    public class MeterValuesMSSDataTable : TypedTableBase<DriverTables.MeterValuesMSSRow>
    {
      private DataColumn columnMeterID;
      private DataColumn columnValueIdentIndex;
      private DataColumn columnTimePoint;
      private DataColumn columnValue;
      private DataColumn columnPhysicalQuantity;
      private DataColumn columnMeterType;
      private DataColumn columnCalculation;
      private DataColumn columnCalculationStart;
      private DataColumn columnStorageInterval;
      private DataColumn columnCreation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterValuesMSSDataTable()
      {
        this.TableName = "MeterValuesMSS";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterValuesMSSDataTable(DataTable table)
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
      protected MeterValuesMSSDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ValueIdentIndexColumn => this.columnValueIdentIndex;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimePointColumn => this.columnTimePoint;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ValueColumn => this.columnValue;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PhysicalQuantityColumn => this.columnPhysicalQuantity;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterTypeColumn => this.columnMeterType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CalculationColumn => this.columnCalculation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CalculationStartColumn => this.columnCalculationStart;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn StorageIntervalColumn => this.columnStorageInterval;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CreationColumn => this.columnCreation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterValuesMSSRow this[int index]
      {
        get => (DriverTables.MeterValuesMSSRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterValuesMSSRowChangeEventHandler MeterValuesMSSRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterValuesMSSRowChangeEventHandler MeterValuesMSSRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterValuesMSSRowChangeEventHandler MeterValuesMSSRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MeterValuesMSSRowChangeEventHandler MeterValuesMSSRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMeterValuesMSSRow(DriverTables.MeterValuesMSSRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterValuesMSSRow AddMeterValuesMSSRow(
        Guid MeterID,
        byte ValueIdentIndex,
        DateTime TimePoint,
        double Value,
        byte PhysicalQuantity,
        byte MeterType,
        byte Calculation,
        byte CalculationStart,
        byte StorageInterval,
        byte Creation)
      {
        DriverTables.MeterValuesMSSRow row = (DriverTables.MeterValuesMSSRow) this.NewRow();
        object[] objArray = new object[10]
        {
          (object) MeterID,
          (object) ValueIdentIndex,
          (object) TimePoint,
          (object) Value,
          (object) PhysicalQuantity,
          (object) MeterType,
          (object) Calculation,
          (object) CalculationStart,
          (object) StorageInterval,
          (object) Creation
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterValuesMSSRow FindByMeterIDValueIdentIndexTimePointPhysicalQuantityMeterTypeCalculationCalculationStartStorageIntervalCreation(
        Guid MeterID,
        byte ValueIdentIndex,
        DateTime TimePoint,
        byte PhysicalQuantity,
        byte MeterType,
        byte Calculation,
        byte CalculationStart,
        byte StorageInterval,
        byte Creation)
      {
        return (DriverTables.MeterValuesMSSRow) this.Rows.Find(new object[9]
        {
          (object) MeterID,
          (object) ValueIdentIndex,
          (object) TimePoint,
          (object) PhysicalQuantity,
          (object) MeterType,
          (object) Calculation,
          (object) CalculationStart,
          (object) StorageInterval,
          (object) Creation
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DriverTables.MeterValuesMSSDataTable valuesMssDataTable = (DriverTables.MeterValuesMSSDataTable) base.Clone();
        valuesMssDataTable.InitVars();
        return (DataTable) valuesMssDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DriverTables.MeterValuesMSSDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnMeterID = this.Columns["MeterID"];
        this.columnValueIdentIndex = this.Columns["ValueIdentIndex"];
        this.columnTimePoint = this.Columns["TimePoint"];
        this.columnValue = this.Columns["Value"];
        this.columnPhysicalQuantity = this.Columns["PhysicalQuantity"];
        this.columnMeterType = this.Columns["MeterType"];
        this.columnCalculation = this.Columns["Calculation"];
        this.columnCalculationStart = this.Columns["CalculationStart"];
        this.columnStorageInterval = this.Columns["StorageInterval"];
        this.columnCreation = this.Columns["Creation"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnMeterID = new DataColumn("MeterID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnValueIdentIndex = new DataColumn("ValueIdentIndex", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValueIdentIndex);
        this.columnTimePoint = new DataColumn("TimePoint", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimePoint);
        this.columnValue = new DataColumn("Value", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValue);
        this.columnPhysicalQuantity = new DataColumn("PhysicalQuantity", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPhysicalQuantity);
        this.columnMeterType = new DataColumn("MeterType", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterType);
        this.columnCalculation = new DataColumn("Calculation", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCalculation);
        this.columnCalculationStart = new DataColumn("CalculationStart", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCalculationStart);
        this.columnStorageInterval = new DataColumn("StorageInterval", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnStorageInterval);
        this.columnCreation = new DataColumn("Creation", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCreation);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[9]
        {
          this.columnMeterID,
          this.columnValueIdentIndex,
          this.columnTimePoint,
          this.columnPhysicalQuantity,
          this.columnMeterType,
          this.columnCalculation,
          this.columnCalculationStart,
          this.columnStorageInterval,
          this.columnCreation
        }, true));
        this.columnMeterID.AllowDBNull = false;
        this.columnValueIdentIndex.AllowDBNull = false;
        this.columnTimePoint.AllowDBNull = false;
        this.columnPhysicalQuantity.AllowDBNull = false;
        this.columnMeterType.AllowDBNull = false;
        this.columnCalculation.AllowDBNull = false;
        this.columnCalculationStart.AllowDBNull = false;
        this.columnStorageInterval.AllowDBNull = false;
        this.columnCreation.AllowDBNull = false;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterValuesMSSRow NewMeterValuesMSSRow()
      {
        return (DriverTables.MeterValuesMSSRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DriverTables.MeterValuesMSSRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DriverTables.MeterValuesMSSRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterValuesMSSRowChanged == null)
          return;
        this.MeterValuesMSSRowChanged((object) this, new DriverTables.MeterValuesMSSRowChangeEvent((DriverTables.MeterValuesMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterValuesMSSRowChanging == null)
          return;
        this.MeterValuesMSSRowChanging((object) this, new DriverTables.MeterValuesMSSRowChangeEvent((DriverTables.MeterValuesMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterValuesMSSRowDeleted == null)
          return;
        this.MeterValuesMSSRowDeleted((object) this, new DriverTables.MeterValuesMSSRowChangeEvent((DriverTables.MeterValuesMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterValuesMSSRowDeleting == null)
          return;
        this.MeterValuesMSSRowDeleting((object) this, new DriverTables.MeterValuesMSSRowChangeEvent((DriverTables.MeterValuesMSSRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMeterValuesMSSRow(DriverTables.MeterValuesMSSRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DriverTables driverTables = new DriverTables();
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
          FixedValue = driverTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterValuesMSSDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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
    public class ServiceTaskResultDataTable : TypedTableBase<DriverTables.ServiceTaskResultRow>
    {
      private DataColumn columnTimePoint;
      private DataColumn columnSerialNumber;
      private DataColumn columnJobID;
      private DataColumn columnMeterID;
      private DataColumn columnMethodName;
      private DataColumn columnResultType;
      private DataColumn columnResultObject;
      private DataColumn columnRawData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ServiceTaskResultDataTable()
      {
        this.TableName = "ServiceTaskResult";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ServiceTaskResultDataTable(DataTable table)
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
      protected ServiceTaskResultDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimePointColumn => this.columnTimePoint;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerialNumberColumn => this.columnSerialNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn JobIDColumn => this.columnJobID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MethodNameColumn => this.columnMethodName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ResultTypeColumn => this.columnResultType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ResultObjectColumn => this.columnResultObject;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RawDataColumn => this.columnRawData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.ServiceTaskResultRow this[int index]
      {
        get => (DriverTables.ServiceTaskResultRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.ServiceTaskResultRowChangeEventHandler ServiceTaskResultRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.ServiceTaskResultRowChangeEventHandler ServiceTaskResultRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.ServiceTaskResultRowChangeEventHandler ServiceTaskResultRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.ServiceTaskResultRowChangeEventHandler ServiceTaskResultRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddServiceTaskResultRow(DriverTables.ServiceTaskResultRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.ServiceTaskResultRow AddServiceTaskResultRow(
        DateTime TimePoint,
        string SerialNumber,
        Guid JobID,
        Guid MeterID,
        string MethodName,
        string ResultType,
        string ResultObject,
        string RawData)
      {
        DriverTables.ServiceTaskResultRow row = (DriverTables.ServiceTaskResultRow) this.NewRow();
        object[] objArray = new object[8]
        {
          (object) TimePoint,
          (object) SerialNumber,
          (object) JobID,
          (object) MeterID,
          (object) MethodName,
          (object) ResultType,
          (object) ResultObject,
          (object) RawData
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.ServiceTaskResultRow FindByTimePointSerialNumber(
        DateTime TimePoint,
        string SerialNumber)
      {
        return (DriverTables.ServiceTaskResultRow) this.Rows.Find(new object[2]
        {
          (object) TimePoint,
          (object) SerialNumber
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DriverTables.ServiceTaskResultDataTable taskResultDataTable = (DriverTables.ServiceTaskResultDataTable) base.Clone();
        taskResultDataTable.InitVars();
        return (DataTable) taskResultDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DriverTables.ServiceTaskResultDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnTimePoint = this.Columns["TimePoint"];
        this.columnSerialNumber = this.Columns["SerialNumber"];
        this.columnJobID = this.Columns["JobID"];
        this.columnMeterID = this.Columns["MeterID"];
        this.columnMethodName = this.Columns["MethodName"];
        this.columnResultType = this.Columns["ResultType"];
        this.columnResultObject = this.Columns["ResultObject"];
        this.columnRawData = this.Columns["RawData"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnTimePoint = new DataColumn("TimePoint", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimePoint);
        this.columnSerialNumber = new DataColumn("SerialNumber", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerialNumber);
        this.columnJobID = new DataColumn("JobID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnJobID);
        this.columnMeterID = new DataColumn("MeterID", typeof (Guid), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnMethodName = new DataColumn("MethodName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMethodName);
        this.columnResultType = new DataColumn("ResultType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnResultType);
        this.columnResultObject = new DataColumn("ResultObject", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnResultObject);
        this.columnRawData = new DataColumn("RawData", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRawData);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnTimePoint,
          this.columnSerialNumber
        }, true));
        this.columnTimePoint.AllowDBNull = false;
        this.columnSerialNumber.AllowDBNull = false;
        this.columnSerialNumber.MaxLength = 100;
        this.columnMethodName.MaxLength = (int) byte.MaxValue;
        this.columnResultType.MaxLength = (int) byte.MaxValue;
        this.columnResultObject.MaxLength = 536870910;
        this.columnRawData.MaxLength = 536870910;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.ServiceTaskResultRow NewServiceTaskResultRow()
      {
        return (DriverTables.ServiceTaskResultRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DriverTables.ServiceTaskResultRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DriverTables.ServiceTaskResultRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ServiceTaskResultRowChanged == null)
          return;
        this.ServiceTaskResultRowChanged((object) this, new DriverTables.ServiceTaskResultRowChangeEvent((DriverTables.ServiceTaskResultRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ServiceTaskResultRowChanging == null)
          return;
        this.ServiceTaskResultRowChanging((object) this, new DriverTables.ServiceTaskResultRowChangeEvent((DriverTables.ServiceTaskResultRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ServiceTaskResultRowDeleted == null)
          return;
        this.ServiceTaskResultRowDeleted((object) this, new DriverTables.ServiceTaskResultRowChangeEvent((DriverTables.ServiceTaskResultRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ServiceTaskResultRowDeleting == null)
          return;
        this.ServiceTaskResultRowDeleting((object) this, new DriverTables.ServiceTaskResultRowChangeEvent((DriverTables.ServiceTaskResultRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveServiceTaskResultRow(DriverTables.ServiceTaskResultRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DriverTables driverTables = new DriverTables();
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
          FixedValue = driverTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ServiceTaskResultDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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
    public class MinomatListDataTable : TypedTableBase<DriverTables.MinomatListRow>
    {
      private DataColumn columnGsmID;
      private DataColumn columnMinolID;
      private DataColumn columnChallengeKey;
      private DataColumn columnSessionKey;
      private DataColumn columnChallengeKeyOld;
      private DataColumn columnSessionKeyOld;
      private DataColumn columnConnectionDate;
      private DataColumn columnConnectionLog;
      private DataColumn columnGsmIDEncoded;
      private DataColumn columnChallengeKeyEncoded;
      private DataColumn columnGsmIDEncodedOld;
      private DataColumn columnChallengeKeyEncodedOld;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MinomatListDataTable()
      {
        this.TableName = "MinomatList";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MinomatListDataTable(DataTable table)
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
      protected MinomatListDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GsmIDColumn => this.columnGsmID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MinolIDColumn => this.columnMinolID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChallengeKeyColumn => this.columnChallengeKey;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SessionKeyColumn => this.columnSessionKey;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChallengeKeyOldColumn => this.columnChallengeKeyOld;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SessionKeyOldColumn => this.columnSessionKeyOld;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ConnectionDateColumn => this.columnConnectionDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ConnectionLogColumn => this.columnConnectionLog;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GsmIDEncodedColumn => this.columnGsmIDEncoded;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChallengeKeyEncodedColumn => this.columnChallengeKeyEncoded;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GsmIDEncodedOldColumn => this.columnGsmIDEncodedOld;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChallengeKeyEncodedOldColumn => this.columnChallengeKeyEncodedOld;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatListRow this[int index]
      {
        get => (DriverTables.MinomatListRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatListRowChangeEventHandler MinomatListRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatListRowChangeEventHandler MinomatListRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatListRowChangeEventHandler MinomatListRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatListRowChangeEventHandler MinomatListRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMinomatListRow(DriverTables.MinomatListRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatListRow AddMinomatListRow(
        string GsmID,
        string MinolID,
        string ChallengeKey,
        string SessionKey,
        string ChallengeKeyOld,
        string SessionKeyOld,
        string ConnectionDate,
        string ConnectionLog,
        string GsmIDEncoded,
        string ChallengeKeyEncoded,
        string GsmIDEncodedOld,
        string ChallengeKeyEncodedOld)
      {
        DriverTables.MinomatListRow row = (DriverTables.MinomatListRow) this.NewRow();
        object[] objArray = new object[12]
        {
          (object) GsmID,
          (object) MinolID,
          (object) ChallengeKey,
          (object) SessionKey,
          (object) ChallengeKeyOld,
          (object) SessionKeyOld,
          (object) ConnectionDate,
          (object) ConnectionLog,
          (object) GsmIDEncoded,
          (object) ChallengeKeyEncoded,
          (object) GsmIDEncodedOld,
          (object) ChallengeKeyEncodedOld
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatListRow FindByGsmID(string GsmID)
      {
        return (DriverTables.MinomatListRow) this.Rows.Find(new object[1]
        {
          (object) GsmID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DriverTables.MinomatListDataTable minomatListDataTable = (DriverTables.MinomatListDataTable) base.Clone();
        minomatListDataTable.InitVars();
        return (DataTable) minomatListDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DriverTables.MinomatListDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnGsmID = this.Columns["GsmID"];
        this.columnMinolID = this.Columns["MinolID"];
        this.columnChallengeKey = this.Columns["ChallengeKey"];
        this.columnSessionKey = this.Columns["SessionKey"];
        this.columnChallengeKeyOld = this.Columns["ChallengeKeyOld"];
        this.columnSessionKeyOld = this.Columns["SessionKeyOld"];
        this.columnConnectionDate = this.Columns["ConnectionDate"];
        this.columnConnectionLog = this.Columns["ConnectionLog"];
        this.columnGsmIDEncoded = this.Columns["GsmIDEncoded"];
        this.columnChallengeKeyEncoded = this.Columns["ChallengeKeyEncoded"];
        this.columnGsmIDEncodedOld = this.Columns["GsmIDEncodedOld"];
        this.columnChallengeKeyEncodedOld = this.Columns["ChallengeKeyEncodedOld"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnGsmID = new DataColumn("GsmID", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGsmID);
        this.columnMinolID = new DataColumn("MinolID", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMinolID);
        this.columnChallengeKey = new DataColumn("ChallengeKey", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChallengeKey);
        this.columnSessionKey = new DataColumn("SessionKey", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSessionKey);
        this.columnChallengeKeyOld = new DataColumn("ChallengeKeyOld", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChallengeKeyOld);
        this.columnSessionKeyOld = new DataColumn("SessionKeyOld", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSessionKeyOld);
        this.columnConnectionDate = new DataColumn("ConnectionDate", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnConnectionDate);
        this.columnConnectionLog = new DataColumn("ConnectionLog", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnConnectionLog);
        this.columnGsmIDEncoded = new DataColumn("GsmIDEncoded", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGsmIDEncoded);
        this.columnChallengeKeyEncoded = new DataColumn("ChallengeKeyEncoded", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChallengeKeyEncoded);
        this.columnGsmIDEncodedOld = new DataColumn("GsmIDEncodedOld", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGsmIDEncodedOld);
        this.columnChallengeKeyEncodedOld = new DataColumn("ChallengeKeyEncodedOld", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChallengeKeyEncodedOld);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnGsmID
        }, true));
        this.columnGsmID.AllowDBNull = false;
        this.columnGsmID.Unique = true;
        this.columnGsmID.MaxLength = 8;
        this.columnMinolID.MaxLength = 8;
        this.columnChallengeKey.MaxLength = 8;
        this.columnSessionKey.MaxLength = 16;
        this.columnChallengeKeyOld.MaxLength = 8;
        this.columnSessionKeyOld.MaxLength = 16;
        this.columnConnectionDate.MaxLength = 14;
        this.columnConnectionLog.MaxLength = 150;
        this.columnGsmIDEncoded.MaxLength = 8;
        this.columnChallengeKeyEncoded.MaxLength = 8;
        this.columnGsmIDEncodedOld.MaxLength = 8;
        this.columnChallengeKeyEncodedOld.MaxLength = 8;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatListRow NewMinomatListRow()
      {
        return (DriverTables.MinomatListRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DriverTables.MinomatListRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DriverTables.MinomatListRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MinomatListRowChanged == null)
          return;
        this.MinomatListRowChanged((object) this, new DriverTables.MinomatListRowChangeEvent((DriverTables.MinomatListRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MinomatListRowChanging == null)
          return;
        this.MinomatListRowChanging((object) this, new DriverTables.MinomatListRowChangeEvent((DriverTables.MinomatListRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MinomatListRowDeleted == null)
          return;
        this.MinomatListRowDeleted((object) this, new DriverTables.MinomatListRowChangeEvent((DriverTables.MinomatListRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MinomatListRowDeleting == null)
          return;
        this.MinomatListRowDeleting((object) this, new DriverTables.MinomatListRowChangeEvent((DriverTables.MinomatListRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMinomatListRow(DriverTables.MinomatListRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DriverTables driverTables = new DriverTables();
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
          FixedValue = driverTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MinomatListDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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
    public class MinomatDataLogsDataTable : TypedTableBase<DriverTables.MinomatDataLogsRow>
    {
      private DataColumn columnMinomatDataLogID;
      private DataColumn columnConnectionID;
      private DataColumn columnTimePoint;
      private DataColumn columnRawData;
      private DataColumn columnChallengeKey;
      private DataColumn columnSessionKey;
      private DataColumn columnIsIncoming;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MinomatDataLogsDataTable()
      {
        this.TableName = "MinomatDataLogs";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MinomatDataLogsDataTable(DataTable table)
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
      protected MinomatDataLogsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MinomatDataLogIDColumn => this.columnMinomatDataLogID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ConnectionIDColumn => this.columnConnectionID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimePointColumn => this.columnTimePoint;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RawDataColumn => this.columnRawData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChallengeKeyColumn => this.columnChallengeKey;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SessionKeyColumn => this.columnSessionKey;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn IsIncomingColumn => this.columnIsIncoming;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatDataLogsRow this[int index]
      {
        get => (DriverTables.MinomatDataLogsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatDataLogsRowChangeEventHandler MinomatDataLogsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatDataLogsRowChangeEventHandler MinomatDataLogsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatDataLogsRowChangeEventHandler MinomatDataLogsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatDataLogsRowChangeEventHandler MinomatDataLogsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMinomatDataLogsRow(DriverTables.MinomatDataLogsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatDataLogsRow AddMinomatDataLogsRow(
        string MinomatDataLogID,
        string ConnectionID,
        string TimePoint,
        string RawData,
        string ChallengeKey,
        string SessionKey,
        bool IsIncoming)
      {
        DriverTables.MinomatDataLogsRow row = (DriverTables.MinomatDataLogsRow) this.NewRow();
        object[] objArray = new object[7]
        {
          (object) MinomatDataLogID,
          (object) ConnectionID,
          (object) TimePoint,
          (object) RawData,
          (object) ChallengeKey,
          (object) SessionKey,
          (object) IsIncoming
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatDataLogsRow FindByMinomatDataLogID(string MinomatDataLogID)
      {
        return (DriverTables.MinomatDataLogsRow) this.Rows.Find(new object[1]
        {
          (object) MinomatDataLogID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DriverTables.MinomatDataLogsDataTable dataLogsDataTable = (DriverTables.MinomatDataLogsDataTable) base.Clone();
        dataLogsDataTable.InitVars();
        return (DataTable) dataLogsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DriverTables.MinomatDataLogsDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnMinomatDataLogID = this.Columns["MinomatDataLogID"];
        this.columnConnectionID = this.Columns["ConnectionID"];
        this.columnTimePoint = this.Columns["TimePoint"];
        this.columnRawData = this.Columns["RawData"];
        this.columnChallengeKey = this.Columns["ChallengeKey"];
        this.columnSessionKey = this.Columns["SessionKey"];
        this.columnIsIncoming = this.Columns["IsIncoming"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnMinomatDataLogID = new DataColumn("MinomatDataLogID", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMinomatDataLogID);
        this.columnConnectionID = new DataColumn("ConnectionID", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnConnectionID);
        this.columnTimePoint = new DataColumn("TimePoint", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimePoint);
        this.columnRawData = new DataColumn("RawData", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRawData);
        this.columnChallengeKey = new DataColumn("ChallengeKey", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChallengeKey);
        this.columnSessionKey = new DataColumn("SessionKey", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSessionKey);
        this.columnIsIncoming = new DataColumn("IsIncoming", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnIsIncoming);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnMinomatDataLogID
        }, true));
        this.columnMinomatDataLogID.AllowDBNull = false;
        this.columnMinomatDataLogID.Unique = true;
        this.columnMinomatDataLogID.MaxLength = 36;
        this.columnConnectionID.MaxLength = 36;
        this.columnTimePoint.MaxLength = 14;
        this.columnRawData.MaxLength = 536870910;
        this.columnChallengeKey.MaxLength = 8;
        this.columnSessionKey.MaxLength = 16;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatDataLogsRow NewMinomatDataLogsRow()
      {
        return (DriverTables.MinomatDataLogsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DriverTables.MinomatDataLogsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DriverTables.MinomatDataLogsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MinomatDataLogsRowChanged == null)
          return;
        this.MinomatDataLogsRowChanged((object) this, new DriverTables.MinomatDataLogsRowChangeEvent((DriverTables.MinomatDataLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MinomatDataLogsRowChanging == null)
          return;
        this.MinomatDataLogsRowChanging((object) this, new DriverTables.MinomatDataLogsRowChangeEvent((DriverTables.MinomatDataLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MinomatDataLogsRowDeleted == null)
          return;
        this.MinomatDataLogsRowDeleted((object) this, new DriverTables.MinomatDataLogsRowChangeEvent((DriverTables.MinomatDataLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MinomatDataLogsRowDeleting == null)
          return;
        this.MinomatDataLogsRowDeleting((object) this, new DriverTables.MinomatDataLogsRowChangeEvent((DriverTables.MinomatDataLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMinomatDataLogsRow(DriverTables.MinomatDataLogsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DriverTables driverTables = new DriverTables();
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
          FixedValue = driverTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MinomatDataLogsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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
    public class MinomatConnectionLogsDataTable : 
      TypedTableBase<DriverTables.MinomatConnectionLogsRow>
    {
      private DataColumn columnConnectionID;
      private DataColumn columnTimePoint;
      private DataColumn columnClientIP;
      private DataColumn columnGsmID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MinomatConnectionLogsDataTable()
      {
        this.TableName = "MinomatConnectionLogs";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MinomatConnectionLogsDataTable(DataTable table)
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
      protected MinomatConnectionLogsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ConnectionIDColumn => this.columnConnectionID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimePointColumn => this.columnTimePoint;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ClientIPColumn => this.columnClientIP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GsmIDColumn => this.columnGsmID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatConnectionLogsRow this[int index]
      {
        get => (DriverTables.MinomatConnectionLogsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatConnectionLogsRowChangeEventHandler MinomatConnectionLogsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatConnectionLogsRowChangeEventHandler MinomatConnectionLogsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatConnectionLogsRowChangeEventHandler MinomatConnectionLogsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DriverTables.MinomatConnectionLogsRowChangeEventHandler MinomatConnectionLogsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMinomatConnectionLogsRow(DriverTables.MinomatConnectionLogsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatConnectionLogsRow AddMinomatConnectionLogsRow(
        string ConnectionID,
        string TimePoint,
        string ClientIP,
        string GsmID)
      {
        DriverTables.MinomatConnectionLogsRow row = (DriverTables.MinomatConnectionLogsRow) this.NewRow();
        object[] objArray = new object[4]
        {
          (object) ConnectionID,
          (object) TimePoint,
          (object) ClientIP,
          (object) GsmID
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatConnectionLogsRow FindByConnectionID(string ConnectionID)
      {
        return (DriverTables.MinomatConnectionLogsRow) this.Rows.Find(new object[1]
        {
          (object) ConnectionID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DriverTables.MinomatConnectionLogsDataTable connectionLogsDataTable = (DriverTables.MinomatConnectionLogsDataTable) base.Clone();
        connectionLogsDataTable.InitVars();
        return (DataTable) connectionLogsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DriverTables.MinomatConnectionLogsDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnConnectionID = this.Columns["ConnectionID"];
        this.columnTimePoint = this.Columns["TimePoint"];
        this.columnClientIP = this.Columns["ClientIP"];
        this.columnGsmID = this.Columns["GsmID"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnConnectionID = new DataColumn("ConnectionID", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnConnectionID);
        this.columnTimePoint = new DataColumn("TimePoint", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimePoint);
        this.columnClientIP = new DataColumn("ClientIP", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnClientIP);
        this.columnGsmID = new DataColumn("GsmID", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGsmID);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnConnectionID
        }, true));
        this.columnConnectionID.AllowDBNull = false;
        this.columnConnectionID.Unique = true;
        this.columnConnectionID.MaxLength = 36;
        this.columnTimePoint.MaxLength = 14;
        this.columnClientIP.MaxLength = 46;
        this.columnGsmID.MaxLength = 8;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatConnectionLogsRow NewMinomatConnectionLogsRow()
      {
        return (DriverTables.MinomatConnectionLogsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DriverTables.MinomatConnectionLogsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DriverTables.MinomatConnectionLogsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MinomatConnectionLogsRowChanged == null)
          return;
        this.MinomatConnectionLogsRowChanged((object) this, new DriverTables.MinomatConnectionLogsRowChangeEvent((DriverTables.MinomatConnectionLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MinomatConnectionLogsRowChanging == null)
          return;
        this.MinomatConnectionLogsRowChanging((object) this, new DriverTables.MinomatConnectionLogsRowChangeEvent((DriverTables.MinomatConnectionLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MinomatConnectionLogsRowDeleted == null)
          return;
        this.MinomatConnectionLogsRowDeleted((object) this, new DriverTables.MinomatConnectionLogsRowChangeEvent((DriverTables.MinomatConnectionLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MinomatConnectionLogsRowDeleting == null)
          return;
        this.MinomatConnectionLogsRowDeleting((object) this, new DriverTables.MinomatConnectionLogsRowChangeEvent((DriverTables.MinomatConnectionLogsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMinomatConnectionLogsRow(DriverTables.MinomatConnectionLogsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DriverTables driverTables = new DriverTables();
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
          FixedValue = driverTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MinomatConnectionLogsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = driverTables.GetSchemaSerializable();
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

    public class MeterMSSRow : DataRow
    {
      private DriverTables.MeterMSSDataTable tableMeterMSS;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterMSSRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterMSS = (DriverTables.MeterMSSDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Guid MeterID
      {
        get => (Guid) this[this.tableMeterMSS.MeterIDColumn];
        set => this[this.tableMeterMSS.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerialNumber
      {
        get => (string) this[this.tableMeterMSS.SerialNumberColumn];
        set => this[this.tableMeterMSS.SerialNumberColumn] = (object) value;
      }
    }

    public class MeterValuesMSSRow : DataRow
    {
      private DriverTables.MeterValuesMSSDataTable tableMeterValuesMSS;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterValuesMSSRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterValuesMSS = (DriverTables.MeterValuesMSSDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Guid MeterID
      {
        get => (Guid) this[this.tableMeterValuesMSS.MeterIDColumn];
        set => this[this.tableMeterValuesMSS.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte ValueIdentIndex
      {
        get => (byte) this[this.tableMeterValuesMSS.ValueIdentIndexColumn];
        set => this[this.tableMeterValuesMSS.ValueIdentIndexColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimePoint
      {
        get => (DateTime) this[this.tableMeterValuesMSS.TimePointColumn];
        set => this[this.tableMeterValuesMSS.TimePointColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Value
      {
        get
        {
          try
          {
            return (double) this[this.tableMeterValuesMSS.ValueColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Value' in table 'MeterValuesMSS' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterValuesMSS.ValueColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte PhysicalQuantity
      {
        get => (byte) this[this.tableMeterValuesMSS.PhysicalQuantityColumn];
        set => this[this.tableMeterValuesMSS.PhysicalQuantityColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte MeterType
      {
        get => (byte) this[this.tableMeterValuesMSS.MeterTypeColumn];
        set => this[this.tableMeterValuesMSS.MeterTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte Calculation
      {
        get => (byte) this[this.tableMeterValuesMSS.CalculationColumn];
        set => this[this.tableMeterValuesMSS.CalculationColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte CalculationStart
      {
        get => (byte) this[this.tableMeterValuesMSS.CalculationStartColumn];
        set => this[this.tableMeterValuesMSS.CalculationStartColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte StorageInterval
      {
        get => (byte) this[this.tableMeterValuesMSS.StorageIntervalColumn];
        set => this[this.tableMeterValuesMSS.StorageIntervalColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte Creation
      {
        get => (byte) this[this.tableMeterValuesMSS.CreationColumn];
        set => this[this.tableMeterValuesMSS.CreationColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsValueNull() => this.IsNull(this.tableMeterValuesMSS.ValueColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetValueNull() => this[this.tableMeterValuesMSS.ValueColumn] = Convert.DBNull;
    }

    public class ServiceTaskResultRow : DataRow
    {
      private DriverTables.ServiceTaskResultDataTable tableServiceTaskResult;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ServiceTaskResultRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableServiceTaskResult = (DriverTables.ServiceTaskResultDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimePoint
      {
        get => (DateTime) this[this.tableServiceTaskResult.TimePointColumn];
        set => this[this.tableServiceTaskResult.TimePointColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerialNumber
      {
        get => (string) this[this.tableServiceTaskResult.SerialNumberColumn];
        set => this[this.tableServiceTaskResult.SerialNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Guid JobID
      {
        get
        {
          try
          {
            return (Guid) this[this.tableServiceTaskResult.JobIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'JobID' in table 'ServiceTaskResult' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableServiceTaskResult.JobIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Guid MeterID
      {
        get
        {
          try
          {
            return (Guid) this[this.tableServiceTaskResult.MeterIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterID' in table 'ServiceTaskResult' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableServiceTaskResult.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string MethodName
      {
        get
        {
          try
          {
            return (string) this[this.tableServiceTaskResult.MethodNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MethodName' in table 'ServiceTaskResult' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableServiceTaskResult.MethodNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ResultType
      {
        get
        {
          try
          {
            return (string) this[this.tableServiceTaskResult.ResultTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ResultType' in table 'ServiceTaskResult' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableServiceTaskResult.ResultTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ResultObject
      {
        get
        {
          try
          {
            return (string) this[this.tableServiceTaskResult.ResultObjectColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ResultObject' in table 'ServiceTaskResult' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableServiceTaskResult.ResultObjectColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string RawData
      {
        get
        {
          try
          {
            return (string) this[this.tableServiceTaskResult.RawDataColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RawData' in table 'ServiceTaskResult' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableServiceTaskResult.RawDataColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsJobIDNull() => this.IsNull(this.tableServiceTaskResult.JobIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetJobIDNull() => this[this.tableServiceTaskResult.JobIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterIDNull() => this.IsNull(this.tableServiceTaskResult.MeterIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterIDNull()
      {
        this[this.tableServiceTaskResult.MeterIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMethodNameNull() => this.IsNull(this.tableServiceTaskResult.MethodNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMethodNameNull()
      {
        this[this.tableServiceTaskResult.MethodNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsResultTypeNull() => this.IsNull(this.tableServiceTaskResult.ResultTypeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetResultTypeNull()
      {
        this[this.tableServiceTaskResult.ResultTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsResultObjectNull()
      {
        return this.IsNull(this.tableServiceTaskResult.ResultObjectColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetResultObjectNull()
      {
        this[this.tableServiceTaskResult.ResultObjectColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRawDataNull() => this.IsNull(this.tableServiceTaskResult.RawDataColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRawDataNull()
      {
        this[this.tableServiceTaskResult.RawDataColumn] = Convert.DBNull;
      }
    }

    public class MinomatListRow : DataRow
    {
      private DriverTables.MinomatListDataTable tableMinomatList;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MinomatListRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMinomatList = (DriverTables.MinomatListDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string GsmID
      {
        get => (string) this[this.tableMinomatList.GsmIDColumn];
        set => this[this.tableMinomatList.GsmIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string MinolID
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.MinolIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MinolID' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.MinolIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChallengeKey
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.ChallengeKeyColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChallengeKey' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.ChallengeKeyColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SessionKey
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.SessionKeyColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SessionKey' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.SessionKeyColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChallengeKeyOld
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.ChallengeKeyOldColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChallengeKeyOld' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.ChallengeKeyOldColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SessionKeyOld
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.SessionKeyOldColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SessionKeyOld' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.SessionKeyOldColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ConnectionDate
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.ConnectionDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ConnectionDate' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.ConnectionDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ConnectionLog
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.ConnectionLogColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ConnectionLog' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.ConnectionLogColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string GsmIDEncoded
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.GsmIDEncodedColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'GsmIDEncoded' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.GsmIDEncodedColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChallengeKeyEncoded
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.ChallengeKeyEncodedColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChallengeKeyEncoded' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.ChallengeKeyEncodedColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string GsmIDEncodedOld
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.GsmIDEncodedOldColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'GsmIDEncodedOld' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.GsmIDEncodedOldColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChallengeKeyEncodedOld
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatList.ChallengeKeyEncodedOldColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChallengeKeyEncodedOld' in table 'MinomatList' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatList.ChallengeKeyEncodedOldColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMinolIDNull() => this.IsNull(this.tableMinomatList.MinolIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMinolIDNull() => this[this.tableMinomatList.MinolIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChallengeKeyNull() => this.IsNull(this.tableMinomatList.ChallengeKeyColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChallengeKeyNull()
      {
        this[this.tableMinomatList.ChallengeKeyColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSessionKeyNull() => this.IsNull(this.tableMinomatList.SessionKeyColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSessionKeyNull()
      {
        this[this.tableMinomatList.SessionKeyColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChallengeKeyOldNull()
      {
        return this.IsNull(this.tableMinomatList.ChallengeKeyOldColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChallengeKeyOldNull()
      {
        this[this.tableMinomatList.ChallengeKeyOldColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSessionKeyOldNull() => this.IsNull(this.tableMinomatList.SessionKeyOldColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSessionKeyOldNull()
      {
        this[this.tableMinomatList.SessionKeyOldColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsConnectionDateNull() => this.IsNull(this.tableMinomatList.ConnectionDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetConnectionDateNull()
      {
        this[this.tableMinomatList.ConnectionDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsConnectionLogNull() => this.IsNull(this.tableMinomatList.ConnectionLogColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetConnectionLogNull()
      {
        this[this.tableMinomatList.ConnectionLogColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGsmIDEncodedNull() => this.IsNull(this.tableMinomatList.GsmIDEncodedColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGsmIDEncodedNull()
      {
        this[this.tableMinomatList.GsmIDEncodedColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChallengeKeyEncodedNull()
      {
        return this.IsNull(this.tableMinomatList.ChallengeKeyEncodedColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChallengeKeyEncodedNull()
      {
        this[this.tableMinomatList.ChallengeKeyEncodedColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGsmIDEncodedOldNull()
      {
        return this.IsNull(this.tableMinomatList.GsmIDEncodedOldColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGsmIDEncodedOldNull()
      {
        this[this.tableMinomatList.GsmIDEncodedOldColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChallengeKeyEncodedOldNull()
      {
        return this.IsNull(this.tableMinomatList.ChallengeKeyEncodedOldColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChallengeKeyEncodedOldNull()
      {
        this[this.tableMinomatList.ChallengeKeyEncodedOldColumn] = Convert.DBNull;
      }
    }

    public class MinomatDataLogsRow : DataRow
    {
      private DriverTables.MinomatDataLogsDataTable tableMinomatDataLogs;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MinomatDataLogsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMinomatDataLogs = (DriverTables.MinomatDataLogsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string MinomatDataLogID
      {
        get => (string) this[this.tableMinomatDataLogs.MinomatDataLogIDColumn];
        set => this[this.tableMinomatDataLogs.MinomatDataLogIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ConnectionID
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatDataLogs.ConnectionIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ConnectionID' in table 'MinomatDataLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatDataLogs.ConnectionIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string TimePoint
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatDataLogs.TimePointColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimePoint' in table 'MinomatDataLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatDataLogs.TimePointColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string RawData
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatDataLogs.RawDataColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RawData' in table 'MinomatDataLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatDataLogs.RawDataColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChallengeKey
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatDataLogs.ChallengeKeyColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChallengeKey' in table 'MinomatDataLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatDataLogs.ChallengeKeyColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SessionKey
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatDataLogs.SessionKeyColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SessionKey' in table 'MinomatDataLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatDataLogs.SessionKeyColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsIncoming
      {
        get
        {
          try
          {
            return (bool) this[this.tableMinomatDataLogs.IsIncomingColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'IsIncoming' in table 'MinomatDataLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatDataLogs.IsIncomingColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsConnectionIDNull() => this.IsNull(this.tableMinomatDataLogs.ConnectionIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetConnectionIDNull()
      {
        this[this.tableMinomatDataLogs.ConnectionIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimePointNull() => this.IsNull(this.tableMinomatDataLogs.TimePointColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimePointNull()
      {
        this[this.tableMinomatDataLogs.TimePointColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRawDataNull() => this.IsNull(this.tableMinomatDataLogs.RawDataColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRawDataNull()
      {
        this[this.tableMinomatDataLogs.RawDataColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChallengeKeyNull() => this.IsNull(this.tableMinomatDataLogs.ChallengeKeyColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChallengeKeyNull()
      {
        this[this.tableMinomatDataLogs.ChallengeKeyColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSessionKeyNull() => this.IsNull(this.tableMinomatDataLogs.SessionKeyColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSessionKeyNull()
      {
        this[this.tableMinomatDataLogs.SessionKeyColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsIsIncomingNull() => this.IsNull(this.tableMinomatDataLogs.IsIncomingColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetIsIncomingNull()
      {
        this[this.tableMinomatDataLogs.IsIncomingColumn] = Convert.DBNull;
      }
    }

    public class MinomatConnectionLogsRow : DataRow
    {
      private DriverTables.MinomatConnectionLogsDataTable tableMinomatConnectionLogs;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MinomatConnectionLogsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMinomatConnectionLogs = (DriverTables.MinomatConnectionLogsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ConnectionID
      {
        get => (string) this[this.tableMinomatConnectionLogs.ConnectionIDColumn];
        set => this[this.tableMinomatConnectionLogs.ConnectionIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string TimePoint
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatConnectionLogs.TimePointColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimePoint' in table 'MinomatConnectionLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatConnectionLogs.TimePointColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ClientIP
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatConnectionLogs.ClientIPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ClientIP' in table 'MinomatConnectionLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatConnectionLogs.ClientIPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string GsmID
      {
        get
        {
          try
          {
            return (string) this[this.tableMinomatConnectionLogs.GsmIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'GsmID' in table 'MinomatConnectionLogs' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMinomatConnectionLogs.GsmIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimePointNull() => this.IsNull(this.tableMinomatConnectionLogs.TimePointColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimePointNull()
      {
        this[this.tableMinomatConnectionLogs.TimePointColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsClientIPNull() => this.IsNull(this.tableMinomatConnectionLogs.ClientIPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetClientIPNull()
      {
        this[this.tableMinomatConnectionLogs.ClientIPColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGsmIDNull() => this.IsNull(this.tableMinomatConnectionLogs.GsmIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGsmIDNull()
      {
        this[this.tableMinomatConnectionLogs.GsmIDColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MeterMSSRowChangeEvent : EventArgs
    {
      private DriverTables.MeterMSSRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterMSSRowChangeEvent(DriverTables.MeterMSSRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterMSSRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MeterValuesMSSRowChangeEvent : EventArgs
    {
      private DriverTables.MeterValuesMSSRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterValuesMSSRowChangeEvent(DriverTables.MeterValuesMSSRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MeterValuesMSSRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class ServiceTaskResultRowChangeEvent : EventArgs
    {
      private DriverTables.ServiceTaskResultRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ServiceTaskResultRowChangeEvent(
        DriverTables.ServiceTaskResultRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.ServiceTaskResultRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MinomatListRowChangeEvent : EventArgs
    {
      private DriverTables.MinomatListRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MinomatListRowChangeEvent(DriverTables.MinomatListRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatListRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MinomatDataLogsRowChangeEvent : EventArgs
    {
      private DriverTables.MinomatDataLogsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MinomatDataLogsRowChangeEvent(
        DriverTables.MinomatDataLogsRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatDataLogsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MinomatConnectionLogsRowChangeEvent : EventArgs
    {
      private DriverTables.MinomatConnectionLogsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MinomatConnectionLogsRowChangeEvent(
        DriverTables.MinomatConnectionLogsRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DriverTables.MinomatConnectionLogsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
