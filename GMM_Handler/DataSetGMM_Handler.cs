// Decompiled with JetBrains decompiler
// Type: GMM_Handler.DataSetGMM_Handler
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

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
namespace GMM_Handler
{
  [DesignerCategory("code")]
  [ToolboxItem(true)]
  [XmlSchemaProvider("GetTypedDataSetSchema")]
  [XmlRoot("DataSetGMM_Handler")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class DataSetGMM_Handler : DataSet
  {
    private DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable tableMeterInfoHardwareTypeJoined;
    private DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable tableMeterInfoHardwareTypeMTypeZelsiusJoined;
    private DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable tableCodeRuntimeCodeJoined;
    private DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable tableCodeDisplayCodeJoined;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public DataSetGMM_Handler()
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
    protected DataSetGMM_Handler(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (MeterInfoHardwareTypeJoined)] != null)
            base.Tables.Add((DataTable) new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable(dataSet.Tables[nameof (MeterInfoHardwareTypeJoined)]));
          if (dataSet.Tables[nameof (MeterInfoHardwareTypeMTypeZelsiusJoined)] != null)
            base.Tables.Add((DataTable) new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable(dataSet.Tables[nameof (MeterInfoHardwareTypeMTypeZelsiusJoined)]));
          if (dataSet.Tables[nameof (CodeRuntimeCodeJoined)] != null)
            base.Tables.Add((DataTable) new DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable(dataSet.Tables[nameof (CodeRuntimeCodeJoined)]));
          if (dataSet.Tables[nameof (CodeDisplayCodeJoined)] != null)
            base.Tables.Add((DataTable) new DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable(dataSet.Tables[nameof (CodeDisplayCodeJoined)]));
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
    public DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable MeterInfoHardwareTypeJoined
    {
      get => this.tableMeterInfoHardwareTypeJoined;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable MeterInfoHardwareTypeMTypeZelsiusJoined
    {
      get => this.tableMeterInfoHardwareTypeMTypeZelsiusJoined;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable CodeRuntimeCodeJoined
    {
      get => this.tableCodeRuntimeCodeJoined;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable CodeDisplayCodeJoined
    {
      get => this.tableCodeDisplayCodeJoined;
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
      DataSetGMM_Handler dataSetGmmHandler = (DataSetGMM_Handler) base.Clone();
      dataSetGmmHandler.InitVars();
      dataSetGmmHandler.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) dataSetGmmHandler;
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
        if (dataSet.Tables["MeterInfoHardwareTypeJoined"] != null)
          base.Tables.Add((DataTable) new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable(dataSet.Tables["MeterInfoHardwareTypeJoined"]));
        if (dataSet.Tables["MeterInfoHardwareTypeMTypeZelsiusJoined"] != null)
          base.Tables.Add((DataTable) new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable(dataSet.Tables["MeterInfoHardwareTypeMTypeZelsiusJoined"]));
        if (dataSet.Tables["CodeRuntimeCodeJoined"] != null)
          base.Tables.Add((DataTable) new DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable(dataSet.Tables["CodeRuntimeCodeJoined"]));
        if (dataSet.Tables["CodeDisplayCodeJoined"] != null)
          base.Tables.Add((DataTable) new DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable(dataSet.Tables["CodeDisplayCodeJoined"]));
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
      this.tableMeterInfoHardwareTypeJoined = (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable) base.Tables["MeterInfoHardwareTypeJoined"];
      if (initTable && this.tableMeterInfoHardwareTypeJoined != null)
        this.tableMeterInfoHardwareTypeJoined.InitVars();
      this.tableMeterInfoHardwareTypeMTypeZelsiusJoined = (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable) base.Tables["MeterInfoHardwareTypeMTypeZelsiusJoined"];
      if (initTable && this.tableMeterInfoHardwareTypeMTypeZelsiusJoined != null)
        this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.InitVars();
      this.tableCodeRuntimeCodeJoined = (DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable) base.Tables["CodeRuntimeCodeJoined"];
      if (initTable && this.tableCodeRuntimeCodeJoined != null)
        this.tableCodeRuntimeCodeJoined.InitVars();
      this.tableCodeDisplayCodeJoined = (DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable) base.Tables["CodeDisplayCodeJoined"];
      if (!initTable || this.tableCodeDisplayCodeJoined == null)
        return;
      this.tableCodeDisplayCodeJoined.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (DataSetGMM_Handler);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/DataSet1.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableMeterInfoHardwareTypeJoined = new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable();
      base.Tables.Add((DataTable) this.tableMeterInfoHardwareTypeJoined);
      this.tableMeterInfoHardwareTypeMTypeZelsiusJoined = new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable();
      base.Tables.Add((DataTable) this.tableMeterInfoHardwareTypeMTypeZelsiusJoined);
      this.tableCodeRuntimeCodeJoined = new DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable();
      base.Tables.Add((DataTable) this.tableCodeRuntimeCodeJoined);
      this.tableCodeDisplayCodeJoined = new DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable();
      base.Tables.Add((DataTable) this.tableCodeDisplayCodeJoined);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMeterInfoHardwareTypeJoined() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMeterInfoHardwareTypeMTypeZelsiusJoined() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeCodeRuntimeCodeJoined() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeCodeDisplayCodeJoined() => false;

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
      DataSetGMM_Handler dataSetGmmHandler = new DataSetGMM_Handler();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = dataSetGmmHandler.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = dataSetGmmHandler.GetSchemaSerializable();
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
    public delegate void MeterInfoHardwareTypeJoinedRowChangeEventHandler(
      object sender,
      DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEventHandler(
      object sender,
      DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void CodeRuntimeCodeJoinedRowChangeEventHandler(
      object sender,
      DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void CodeDisplayCodeJoinedRowChangeEventHandler(
      object sender,
      DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class MeterInfoHardwareTypeJoinedDataTable : 
      TypedTableBase<DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow>
    {
      private DataColumn columnHardwareTypeID;
      private DataColumn columnMapID;
      private DataColumn columnLinkerTableID;
      private DataColumn columnFirmwareVersion;
      private DataColumn columnHardwareName;
      private DataColumn columnHardwareVersion;
      private DataColumn columnHardwareResource;
      private DataColumn columnextEEPSize;
      private DataColumn columnmaxStackSize;
      private DataColumn columnRAMSize;
      private DataColumn columnRAMStartAdr;
      private DataColumn columnintEEPStartAdr;
      private DataColumn columnintEEPSize;
      private DataColumn columnDescription;
      private DataColumn columnTestinfo;
      private DataColumn columnHardwareOptions;
      private DataColumn columnMeterInfoID;
      private DataColumn columnMeterHardwareID;
      private DataColumn columnMeterTypeID;
      private DataColumn columnPPSArtikelNr;
      private DataColumn columnDefaultFunctionNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterInfoHardwareTypeJoinedDataTable()
      {
        this.TableName = "MeterInfoHardwareTypeJoined";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterInfoHardwareTypeJoinedDataTable(DataTable table)
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
      protected MeterInfoHardwareTypeJoinedDataTable(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareTypeIDColumn => this.columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LinkerTableIDColumn => this.columnLinkerTableID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FirmwareVersionColumn => this.columnFirmwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareNameColumn => this.columnHardwareName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareVersionColumn => this.columnHardwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareResourceColumn => this.columnHardwareResource;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn extEEPSizeColumn => this.columnextEEPSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn maxStackSizeColumn => this.columnmaxStackSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RAMSizeColumn => this.columnRAMSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RAMStartAdrColumn => this.columnRAMStartAdr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn intEEPStartAdrColumn => this.columnintEEPStartAdr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn intEEPSizeColumn => this.columnintEEPSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TestinfoColumn => this.columnTestinfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareOptionsColumn => this.columnHardwareOptions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterInfoIDColumn => this.columnMeterInfoID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterHardwareIDColumn => this.columnMeterHardwareID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterTypeIDColumn => this.columnMeterTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PPSArtikelNrColumn => this.columnPPSArtikelNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DefaultFunctionNrColumn => this.columnDefaultFunctionNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow this[int index]
      {
        get => (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEventHandler MeterInfoHardwareTypeJoinedRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEventHandler MeterInfoHardwareTypeJoinedRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEventHandler MeterInfoHardwareTypeJoinedRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEventHandler MeterInfoHardwareTypeJoinedRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMeterInfoHardwareTypeJoinedRow(
        DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow AddMeterInfoHardwareTypeJoinedRow(
        int HardwareTypeID,
        int MapID,
        int LinkerTableID,
        int FirmwareVersion,
        string HardwareName,
        int HardwareVersion,
        string HardwareResource,
        int extEEPSize,
        short maxStackSize,
        short RAMSize,
        short RAMStartAdr,
        short intEEPStartAdr,
        short intEEPSize,
        string Description,
        string Testinfo,
        string HardwareOptions,
        int MeterInfoID,
        int MeterHardwareID,
        int MeterTypeID,
        string PPSArtikelNr,
        string DefaultFunctionNr)
      {
        DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow row = (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) this.NewRow();
        object[] objArray = new object[21]
        {
          (object) HardwareTypeID,
          (object) MapID,
          (object) LinkerTableID,
          (object) FirmwareVersion,
          (object) HardwareName,
          (object) HardwareVersion,
          (object) HardwareResource,
          (object) extEEPSize,
          (object) maxStackSize,
          (object) RAMSize,
          (object) RAMStartAdr,
          (object) intEEPStartAdr,
          (object) intEEPSize,
          (object) Description,
          (object) Testinfo,
          (object) HardwareOptions,
          (object) MeterInfoID,
          (object) MeterHardwareID,
          (object) MeterTypeID,
          (object) PPSArtikelNr,
          (object) DefaultFunctionNr
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow FindByMeterInfoIDHardwareTypeID(
        int MeterInfoID,
        int HardwareTypeID)
      {
        return (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) this.Rows.Find(new object[2]
        {
          (object) MeterInfoID,
          (object) HardwareTypeID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable typeJoinedDataTable = (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable) base.Clone();
        typeJoinedDataTable.InitVars();
        return (DataTable) typeJoinedDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnHardwareTypeID = this.Columns["HardwareTypeID"];
        this.columnMapID = this.Columns["MapID"];
        this.columnLinkerTableID = this.Columns["LinkerTableID"];
        this.columnFirmwareVersion = this.Columns["FirmwareVersion"];
        this.columnHardwareName = this.Columns["HardwareName"];
        this.columnHardwareVersion = this.Columns["HardwareVersion"];
        this.columnHardwareResource = this.Columns["HardwareResource"];
        this.columnextEEPSize = this.Columns["extEEPSize"];
        this.columnmaxStackSize = this.Columns["maxStackSize"];
        this.columnRAMSize = this.Columns["RAMSize"];
        this.columnRAMStartAdr = this.Columns["RAMStartAdr"];
        this.columnintEEPStartAdr = this.Columns["intEEPStartAdr"];
        this.columnintEEPSize = this.Columns["intEEPSize"];
        this.columnDescription = this.Columns["Description"];
        this.columnTestinfo = this.Columns["Testinfo"];
        this.columnHardwareOptions = this.Columns["HardwareOptions"];
        this.columnMeterInfoID = this.Columns["MeterInfoID"];
        this.columnMeterHardwareID = this.Columns["MeterHardwareID"];
        this.columnMeterTypeID = this.Columns["MeterTypeID"];
        this.columnPPSArtikelNr = this.Columns["PPSArtikelNr"];
        this.columnDefaultFunctionNr = this.Columns["DefaultFunctionNr"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnHardwareTypeID = new DataColumn("HardwareTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareTypeID);
        this.columnMapID = new DataColumn("MapID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMapID);
        this.columnLinkerTableID = new DataColumn("LinkerTableID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLinkerTableID);
        this.columnFirmwareVersion = new DataColumn("FirmwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareVersion);
        this.columnHardwareName = new DataColumn("HardwareName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareName);
        this.columnHardwareVersion = new DataColumn("HardwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareVersion);
        this.columnHardwareResource = new DataColumn("HardwareResource", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareResource);
        this.columnextEEPSize = new DataColumn("extEEPSize", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnextEEPSize);
        this.columnmaxStackSize = new DataColumn("maxStackSize", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnmaxStackSize);
        this.columnRAMSize = new DataColumn("RAMSize", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRAMSize);
        this.columnRAMStartAdr = new DataColumn("RAMStartAdr", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRAMStartAdr);
        this.columnintEEPStartAdr = new DataColumn("intEEPStartAdr", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnintEEPStartAdr);
        this.columnintEEPSize = new DataColumn("intEEPSize", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnintEEPSize);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.columnTestinfo = new DataColumn("Testinfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTestinfo);
        this.columnHardwareOptions = new DataColumn("HardwareOptions", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareOptions);
        this.columnMeterInfoID = new DataColumn("MeterInfoID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterInfoID);
        this.columnMeterHardwareID = new DataColumn("MeterHardwareID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterHardwareID);
        this.columnMeterTypeID = new DataColumn("MeterTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterTypeID);
        this.columnPPSArtikelNr = new DataColumn("PPSArtikelNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPPSArtikelNr);
        this.columnDefaultFunctionNr = new DataColumn("DefaultFunctionNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDefaultFunctionNr);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnMeterInfoID,
          this.columnHardwareTypeID
        }, true));
        this.columnHardwareTypeID.AllowDBNull = false;
        this.columnHardwareName.MaxLength = 50;
        this.columnHardwareResource.MaxLength = 536870910;
        this.columnDescription.MaxLength = 536870910;
        this.columnTestinfo.MaxLength = 536870910;
        this.columnHardwareOptions.MaxLength = (int) byte.MaxValue;
        this.columnMeterInfoID.AllowDBNull = false;
        this.columnPPSArtikelNr.MaxLength = 50;
        this.columnDefaultFunctionNr.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow NewMeterInfoHardwareTypeJoinedRow()
      {
        return (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterInfoHardwareTypeJoinedRowChanged == null)
          return;
        this.MeterInfoHardwareTypeJoinedRowChanged((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterInfoHardwareTypeJoinedRowChanging == null)
          return;
        this.MeterInfoHardwareTypeJoinedRowChanging((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterInfoHardwareTypeJoinedRowDeleted == null)
          return;
        this.MeterInfoHardwareTypeJoinedRowDeleted((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterInfoHardwareTypeJoinedRowDeleting == null)
          return;
        this.MeterInfoHardwareTypeJoinedRowDeleting((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMeterInfoHardwareTypeJoinedRow(
        DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DataSetGMM_Handler dataSetGmmHandler = new DataSetGMM_Handler();
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
          FixedValue = dataSetGmmHandler.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterInfoHardwareTypeJoinedDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dataSetGmmHandler.GetSchemaSerializable();
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
    public class MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable : 
      TypedTableBase<DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow>
    {
      private DataColumn columnHardwareTypeID;
      private DataColumn columnMapID;
      private DataColumn columnLinkerTableID;
      private DataColumn columnFirmwareVersion;
      private DataColumn columnHardwareName;
      private DataColumn columnHardwareVersion;
      private DataColumn columnextEEPSize;
      private DataColumn columnDescription;
      private DataColumn columnMeterHardwareID;
      private DataColumn columnMeterTypeID;
      private DataColumn columnPPSArtikelNr;
      private DataColumn columnDefaultFunctionNr;
      private DataColumn columnEEPdata;
      private DataColumn columnTypeOverrideString;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable()
      {
        this.TableName = "MeterInfoHardwareTypeMTypeZelsiusJoined";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable(DataTable table)
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
      protected MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareTypeIDColumn => this.columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LinkerTableIDColumn => this.columnLinkerTableID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FirmwareVersionColumn => this.columnFirmwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareNameColumn => this.columnHardwareName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HardwareVersionColumn => this.columnHardwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn extEEPSizeColumn => this.columnextEEPSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterHardwareIDColumn => this.columnMeterHardwareID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterTypeIDColumn => this.columnMeterTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PPSArtikelNrColumn => this.columnPPSArtikelNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DefaultFunctionNrColumn => this.columnDefaultFunctionNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EEPdataColumn => this.columnEEPdata;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TypeOverrideStringColumn => this.columnTypeOverrideString;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow this[int index]
      {
        get => (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEventHandler MeterInfoHardwareTypeMTypeZelsiusJoinedRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEventHandler MeterInfoHardwareTypeMTypeZelsiusJoinedRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEventHandler MeterInfoHardwareTypeMTypeZelsiusJoinedRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEventHandler MeterInfoHardwareTypeMTypeZelsiusJoinedRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMeterInfoHardwareTypeMTypeZelsiusJoinedRow(
        DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow AddMeterInfoHardwareTypeMTypeZelsiusJoinedRow(
        int HardwareTypeID,
        int MapID,
        int LinkerTableID,
        int FirmwareVersion,
        string HardwareName,
        int HardwareVersion,
        int extEEPSize,
        string Description,
        int MeterHardwareID,
        int MeterTypeID,
        string PPSArtikelNr,
        string DefaultFunctionNr,
        byte[] EEPdata,
        string TypeOverrideString)
      {
        DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow row = (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) this.NewRow();
        object[] objArray = new object[14]
        {
          (object) HardwareTypeID,
          (object) MapID,
          (object) LinkerTableID,
          (object) FirmwareVersion,
          (object) HardwareName,
          (object) HardwareVersion,
          (object) extEEPSize,
          (object) Description,
          (object) MeterHardwareID,
          (object) MeterTypeID,
          (object) PPSArtikelNr,
          (object) DefaultFunctionNr,
          (object) EEPdata,
          (object) TypeOverrideString
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable zelsiusJoinedDataTable = (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable) base.Clone();
        zelsiusJoinedDataTable.InitVars();
        return (DataTable) zelsiusJoinedDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnHardwareTypeID = this.Columns["HardwareTypeID"];
        this.columnMapID = this.Columns["MapID"];
        this.columnLinkerTableID = this.Columns["LinkerTableID"];
        this.columnFirmwareVersion = this.Columns["FirmwareVersion"];
        this.columnHardwareName = this.Columns["HardwareName"];
        this.columnHardwareVersion = this.Columns["HardwareVersion"];
        this.columnextEEPSize = this.Columns["extEEPSize"];
        this.columnDescription = this.Columns["Description"];
        this.columnMeterHardwareID = this.Columns["MeterHardwareID"];
        this.columnMeterTypeID = this.Columns["MeterTypeID"];
        this.columnPPSArtikelNr = this.Columns["PPSArtikelNr"];
        this.columnDefaultFunctionNr = this.Columns["DefaultFunctionNr"];
        this.columnEEPdata = this.Columns["EEPdata"];
        this.columnTypeOverrideString = this.Columns["TypeOverrideString"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnHardwareTypeID = new DataColumn("HardwareTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareTypeID);
        this.columnMapID = new DataColumn("MapID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMapID);
        this.columnLinkerTableID = new DataColumn("LinkerTableID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLinkerTableID);
        this.columnFirmwareVersion = new DataColumn("FirmwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareVersion);
        this.columnHardwareName = new DataColumn("HardwareName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareName);
        this.columnHardwareVersion = new DataColumn("HardwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareVersion);
        this.columnextEEPSize = new DataColumn("extEEPSize", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnextEEPSize);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.columnMeterHardwareID = new DataColumn("MeterHardwareID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterHardwareID);
        this.columnMeterTypeID = new DataColumn("MeterTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterTypeID);
        this.columnPPSArtikelNr = new DataColumn("PPSArtikelNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPPSArtikelNr);
        this.columnDefaultFunctionNr = new DataColumn("DefaultFunctionNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDefaultFunctionNr);
        this.columnEEPdata = new DataColumn("EEPdata", typeof (byte[]), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEEPdata);
        this.columnTypeOverrideString = new DataColumn("TypeOverrideString", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTypeOverrideString);
        this.columnHardwareTypeID.AllowDBNull = false;
        this.columnHardwareName.MaxLength = 50;
        this.columnDescription.MaxLength = 536870910;
        this.columnPPSArtikelNr.MaxLength = 50;
        this.columnDefaultFunctionNr.MaxLength = 50;
        this.columnTypeOverrideString.DefaultValue = (object) "";
        this.columnTypeOverrideString.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow NewMeterInfoHardwareTypeMTypeZelsiusJoinedRow()
      {
        return (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChanged == null)
          return;
        this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChanged((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChanging == null)
          return;
        this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChanging((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowDeleted == null)
          return;
        this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowDeleted((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowDeleting == null)
          return;
        this.MeterInfoHardwareTypeMTypeZelsiusJoinedRowDeleting((object) this, new DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent((DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMeterInfoHardwareTypeMTypeZelsiusJoinedRow(
        DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DataSetGMM_Handler dataSetGmmHandler = new DataSetGMM_Handler();
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
          FixedValue = dataSetGmmHandler.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dataSetGmmHandler.GetSchemaSerializable();
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
    public class CodeRuntimeCodeJoinedDataTable : 
      TypedTableBase<DataSetGMM_Handler.CodeRuntimeCodeJoinedRow>
    {
      private DataColumn columnFunctionNumber;
      private DataColumn columnCodeID;
      private DataColumn columnCodeSequenceType;
      private DataColumn columnCodeSequenceInfo;
      private DataColumn columnCodeSequenceName;
      private DataColumn columnLineNr;
      private DataColumn columnCodeType;
      private DataColumn columnCodeValue;
      private DataColumn columnLineInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public CodeRuntimeCodeJoinedDataTable()
      {
        this.TableName = "CodeRuntimeCodeJoined";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal CodeRuntimeCodeJoinedDataTable(DataTable table)
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
      protected CodeRuntimeCodeJoinedDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FunctionNumberColumn => this.columnFunctionNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeIDColumn => this.columnCodeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeSequenceTypeColumn => this.columnCodeSequenceType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeSequenceInfoColumn => this.columnCodeSequenceInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeSequenceNameColumn => this.columnCodeSequenceName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LineNrColumn => this.columnLineNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeTypeColumn => this.columnCodeType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeValueColumn => this.columnCodeValue;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LineInfoColumn => this.columnLineInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeRuntimeCodeJoinedRow this[int index]
      {
        get => (DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEventHandler CodeRuntimeCodeJoinedRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEventHandler CodeRuntimeCodeJoinedRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEventHandler CodeRuntimeCodeJoinedRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEventHandler CodeRuntimeCodeJoinedRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddCodeRuntimeCodeJoinedRow(DataSetGMM_Handler.CodeRuntimeCodeJoinedRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeRuntimeCodeJoinedRow AddCodeRuntimeCodeJoinedRow(
        int FunctionNumber,
        int CodeID,
        string CodeSequenceType,
        string CodeSequenceInfo,
        string CodeSequenceName,
        int LineNr,
        string CodeType,
        string CodeValue,
        string LineInfo)
      {
        DataSetGMM_Handler.CodeRuntimeCodeJoinedRow row = (DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) this.NewRow();
        object[] objArray = new object[9]
        {
          (object) FunctionNumber,
          (object) CodeID,
          (object) CodeSequenceType,
          (object) CodeSequenceInfo,
          (object) CodeSequenceName,
          (object) LineNr,
          (object) CodeType,
          (object) CodeValue,
          (object) LineInfo
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeRuntimeCodeJoinedRow FindByFunctionNumberCodeIDLineNr(
        int FunctionNumber,
        int CodeID,
        int LineNr)
      {
        return (DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) this.Rows.Find(new object[3]
        {
          (object) FunctionNumber,
          (object) CodeID,
          (object) LineNr
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable codeJoinedDataTable = (DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable) base.Clone();
        codeJoinedDataTable.InitVars();
        return (DataTable) codeJoinedDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnFunctionNumber = this.Columns["FunctionNumber"];
        this.columnCodeID = this.Columns["CodeID"];
        this.columnCodeSequenceType = this.Columns["CodeSequenceType"];
        this.columnCodeSequenceInfo = this.Columns["CodeSequenceInfo"];
        this.columnCodeSequenceName = this.Columns["CodeSequenceName"];
        this.columnLineNr = this.Columns["LineNr"];
        this.columnCodeType = this.Columns["CodeType"];
        this.columnCodeValue = this.Columns["CodeValue"];
        this.columnLineInfo = this.Columns["LineInfo"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnFunctionNumber = new DataColumn("FunctionNumber", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFunctionNumber);
        this.columnCodeID = new DataColumn("CodeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeID);
        this.columnCodeSequenceType = new DataColumn("CodeSequenceType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeSequenceType);
        this.columnCodeSequenceInfo = new DataColumn("CodeSequenceInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeSequenceInfo);
        this.columnCodeSequenceName = new DataColumn("CodeSequenceName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeSequenceName);
        this.columnLineNr = new DataColumn("LineNr", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLineNr);
        this.columnCodeType = new DataColumn("CodeType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeType);
        this.columnCodeValue = new DataColumn("CodeValue", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeValue);
        this.columnLineInfo = new DataColumn("LineInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLineInfo);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[3]
        {
          this.columnFunctionNumber,
          this.columnCodeID,
          this.columnLineNr
        }, true));
        this.columnFunctionNumber.AllowDBNull = false;
        this.columnCodeID.AllowDBNull = false;
        this.columnCodeSequenceType.MaxLength = 50;
        this.columnCodeSequenceInfo.MaxLength = 50;
        this.columnCodeSequenceName.MaxLength = 50;
        this.columnLineNr.AllowDBNull = false;
        this.columnCodeType.MaxLength = 50;
        this.columnCodeValue.MaxLength = 536870910;
        this.columnLineInfo.MaxLength = 250;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeRuntimeCodeJoinedRow NewCodeRuntimeCodeJoinedRow()
      {
        return (DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DataSetGMM_Handler.CodeRuntimeCodeJoinedRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DataSetGMM_Handler.CodeRuntimeCodeJoinedRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.CodeRuntimeCodeJoinedRowChanged == null)
          return;
        this.CodeRuntimeCodeJoinedRowChanged((object) this, new DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.CodeRuntimeCodeJoinedRowChanging == null)
          return;
        this.CodeRuntimeCodeJoinedRowChanging((object) this, new DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.CodeRuntimeCodeJoinedRowDeleted == null)
          return;
        this.CodeRuntimeCodeJoinedRowDeleted((object) this, new DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.CodeRuntimeCodeJoinedRowDeleting == null)
          return;
        this.CodeRuntimeCodeJoinedRowDeleting((object) this, new DataSetGMM_Handler.CodeRuntimeCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeRuntimeCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveCodeRuntimeCodeJoinedRow(DataSetGMM_Handler.CodeRuntimeCodeJoinedRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DataSetGMM_Handler dataSetGmmHandler = new DataSetGMM_Handler();
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
          FixedValue = dataSetGmmHandler.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (CodeRuntimeCodeJoinedDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dataSetGmmHandler.GetSchemaSerializable();
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
    public class CodeDisplayCodeJoinedDataTable : 
      TypedTableBase<DataSetGMM_Handler.CodeDisplayCodeJoinedRow>
    {
      private DataColumn columnCodeID;
      private DataColumn columnLineNr;
      private DataColumn columnCodeType;
      private DataColumn columnCodeValue;
      private DataColumn columnLineInfo;
      private DataColumn columnInterpreterCode;
      private DataColumn columnSequenceNr;
      private DataColumn columnCodeSequenceType;
      private DataColumn columnCodeSequenceInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public CodeDisplayCodeJoinedDataTable()
      {
        this.TableName = "CodeDisplayCodeJoined";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal CodeDisplayCodeJoinedDataTable(DataTable table)
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
      protected CodeDisplayCodeJoinedDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeIDColumn => this.columnCodeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LineNrColumn => this.columnLineNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeTypeColumn => this.columnCodeType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeValueColumn => this.columnCodeValue;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LineInfoColumn => this.columnLineInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InterpreterCodeColumn => this.columnInterpreterCode;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SequenceNrColumn => this.columnSequenceNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeSequenceTypeColumn => this.columnCodeSequenceType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CodeSequenceInfoColumn => this.columnCodeSequenceInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeDisplayCodeJoinedRow this[int index]
      {
        get => (DataSetGMM_Handler.CodeDisplayCodeJoinedRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEventHandler CodeDisplayCodeJoinedRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEventHandler CodeDisplayCodeJoinedRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEventHandler CodeDisplayCodeJoinedRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEventHandler CodeDisplayCodeJoinedRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddCodeDisplayCodeJoinedRow(DataSetGMM_Handler.CodeDisplayCodeJoinedRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeDisplayCodeJoinedRow AddCodeDisplayCodeJoinedRow(
        int CodeID,
        int LineNr,
        string CodeType,
        string CodeValue,
        string LineInfo,
        int InterpreterCode,
        int SequenceNr,
        string CodeSequenceType,
        string CodeSequenceInfo)
      {
        DataSetGMM_Handler.CodeDisplayCodeJoinedRow row = (DataSetGMM_Handler.CodeDisplayCodeJoinedRow) this.NewRow();
        object[] objArray = new object[9]
        {
          (object) CodeID,
          (object) LineNr,
          (object) CodeType,
          (object) CodeValue,
          (object) LineInfo,
          (object) InterpreterCode,
          (object) SequenceNr,
          (object) CodeSequenceType,
          (object) CodeSequenceInfo
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeDisplayCodeJoinedRow FindByInterpreterCodeSequenceNrLineNrCodeID(
        int InterpreterCode,
        int SequenceNr,
        int LineNr,
        int CodeID)
      {
        return (DataSetGMM_Handler.CodeDisplayCodeJoinedRow) this.Rows.Find(new object[4]
        {
          (object) InterpreterCode,
          (object) SequenceNr,
          (object) LineNr,
          (object) CodeID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable codeJoinedDataTable = (DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable) base.Clone();
        codeJoinedDataTable.InitVars();
        return (DataTable) codeJoinedDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnCodeID = this.Columns["CodeID"];
        this.columnLineNr = this.Columns["LineNr"];
        this.columnCodeType = this.Columns["CodeType"];
        this.columnCodeValue = this.Columns["CodeValue"];
        this.columnLineInfo = this.Columns["LineInfo"];
        this.columnInterpreterCode = this.Columns["InterpreterCode"];
        this.columnSequenceNr = this.Columns["SequenceNr"];
        this.columnCodeSequenceType = this.Columns["CodeSequenceType"];
        this.columnCodeSequenceInfo = this.Columns["CodeSequenceInfo"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnCodeID = new DataColumn("CodeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeID);
        this.columnLineNr = new DataColumn("LineNr", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLineNr);
        this.columnCodeType = new DataColumn("CodeType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeType);
        this.columnCodeValue = new DataColumn("CodeValue", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeValue);
        this.columnLineInfo = new DataColumn("LineInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLineInfo);
        this.columnInterpreterCode = new DataColumn("InterpreterCode", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInterpreterCode);
        this.columnSequenceNr = new DataColumn("SequenceNr", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSequenceNr);
        this.columnCodeSequenceType = new DataColumn("CodeSequenceType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeSequenceType);
        this.columnCodeSequenceInfo = new DataColumn("CodeSequenceInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCodeSequenceInfo);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[4]
        {
          this.columnInterpreterCode,
          this.columnSequenceNr,
          this.columnLineNr,
          this.columnCodeID
        }, true));
        this.columnCodeID.AllowDBNull = false;
        this.columnLineNr.AllowDBNull = false;
        this.columnCodeType.MaxLength = 50;
        this.columnCodeValue.MaxLength = 536870910;
        this.columnLineInfo.MaxLength = 250;
        this.columnInterpreterCode.AllowDBNull = false;
        this.columnSequenceNr.AllowDBNull = false;
        this.columnCodeSequenceType.MaxLength = 50;
        this.columnCodeSequenceInfo.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeDisplayCodeJoinedRow NewCodeDisplayCodeJoinedRow()
      {
        return (DataSetGMM_Handler.CodeDisplayCodeJoinedRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DataSetGMM_Handler.CodeDisplayCodeJoinedRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DataSetGMM_Handler.CodeDisplayCodeJoinedRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.CodeDisplayCodeJoinedRowChanged == null)
          return;
        this.CodeDisplayCodeJoinedRowChanged((object) this, new DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeDisplayCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.CodeDisplayCodeJoinedRowChanging == null)
          return;
        this.CodeDisplayCodeJoinedRowChanging((object) this, new DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeDisplayCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.CodeDisplayCodeJoinedRowDeleted == null)
          return;
        this.CodeDisplayCodeJoinedRowDeleted((object) this, new DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeDisplayCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.CodeDisplayCodeJoinedRowDeleting == null)
          return;
        this.CodeDisplayCodeJoinedRowDeleting((object) this, new DataSetGMM_Handler.CodeDisplayCodeJoinedRowChangeEvent((DataSetGMM_Handler.CodeDisplayCodeJoinedRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveCodeDisplayCodeJoinedRow(DataSetGMM_Handler.CodeDisplayCodeJoinedRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DataSetGMM_Handler dataSetGmmHandler = new DataSetGMM_Handler();
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
          FixedValue = dataSetGmmHandler.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (CodeDisplayCodeJoinedDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dataSetGmmHandler.GetSchemaSerializable();
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

    public class MeterInfoHardwareTypeJoinedRow : DataRow
    {
      private DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable tableMeterInfoHardwareTypeJoined;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterInfoHardwareTypeJoinedRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterInfoHardwareTypeJoined = (DataSetGMM_Handler.MeterInfoHardwareTypeJoinedDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int HardwareTypeID
      {
        get => (int) this[this.tableMeterInfoHardwareTypeJoined.HardwareTypeIDColumn];
        set => this[this.tableMeterInfoHardwareTypeJoined.HardwareTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MapID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.MapIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MapID' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LinkerTableID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.LinkerTableIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LinkerTableID' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.LinkerTableIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int FirmwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.FirmwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareVersion' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.FirmwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string HardwareName
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.HardwareNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareName' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.HardwareNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int HardwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.HardwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareVersion' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.HardwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string HardwareResource
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.HardwareResourceColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareResource' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.HardwareResourceColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int extEEPSize
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.extEEPSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'extEEPSize' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.extEEPSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short maxStackSize
      {
        get
        {
          try
          {
            return (short) this[this.tableMeterInfoHardwareTypeJoined.maxStackSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'maxStackSize' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.maxStackSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short RAMSize
      {
        get
        {
          try
          {
            return (short) this[this.tableMeterInfoHardwareTypeJoined.RAMSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RAMSize' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.RAMSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short RAMStartAdr
      {
        get
        {
          try
          {
            return (short) this[this.tableMeterInfoHardwareTypeJoined.RAMStartAdrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RAMStartAdr' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.RAMStartAdrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short intEEPStartAdr
      {
        get
        {
          try
          {
            return (short) this[this.tableMeterInfoHardwareTypeJoined.intEEPStartAdrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'intEEPStartAdr' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.intEEPStartAdrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short intEEPSize
      {
        get
        {
          try
          {
            return (short) this[this.tableMeterInfoHardwareTypeJoined.intEEPSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'intEEPSize' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.intEEPSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Testinfo
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.TestinfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Testinfo' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.TestinfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string HardwareOptions
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.HardwareOptionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareOptions' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.HardwareOptionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterInfoID
      {
        get => (int) this[this.tableMeterInfoHardwareTypeJoined.MeterInfoIDColumn];
        set => this[this.tableMeterInfoHardwareTypeJoined.MeterInfoIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterHardwareID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.MeterHardwareIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterHardwareID' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.MeterHardwareIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterTypeID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeJoined.MeterTypeIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterTypeID' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.MeterTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string PPSArtikelNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.PPSArtikelNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PPSArtikelNr' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.PPSArtikelNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string DefaultFunctionNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeJoined.DefaultFunctionNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DefaultFunctionNr' in table 'MeterInfoHardwareTypeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeJoined.DefaultFunctionNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMapIDNull() => this.IsNull(this.tableMeterInfoHardwareTypeJoined.MapIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMapIDNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.MapIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLinkerTableIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.LinkerTableIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLinkerTableIDNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.LinkerTableIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFirmwareVersionNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.FirmwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFirmwareVersionNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.FirmwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHardwareNameNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.HardwareNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHardwareNameNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.HardwareNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHardwareVersionNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.HardwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHardwareVersionNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.HardwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHardwareResourceNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.HardwareResourceColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHardwareResourceNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.HardwareResourceColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsextEEPSizeNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.extEEPSizeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetextEEPSizeNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.extEEPSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsmaxStackSizeNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.maxStackSizeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetmaxStackSizeNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.maxStackSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRAMSizeNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.RAMSizeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRAMSizeNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.RAMSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRAMStartAdrNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.RAMStartAdrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRAMStartAdrNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.RAMStartAdrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsintEEPStartAdrNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.intEEPStartAdrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetintEEPStartAdrNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.intEEPStartAdrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsintEEPSizeNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.intEEPSizeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetintEEPSizeNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.intEEPSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDescriptionNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.DescriptionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTestinfoNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.TestinfoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTestinfoNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.TestinfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHardwareOptionsNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.HardwareOptionsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHardwareOptionsNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.HardwareOptionsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterHardwareIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.MeterHardwareIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterHardwareIDNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.MeterHardwareIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterTypeIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.MeterTypeIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterTypeIDNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.MeterTypeIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPPSArtikelNrNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.PPSArtikelNrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPPSArtikelNrNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.PPSArtikelNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDefaultFunctionNrNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeJoined.DefaultFunctionNrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDefaultFunctionNrNull()
      {
        this[this.tableMeterInfoHardwareTypeJoined.DefaultFunctionNrColumn] = Convert.DBNull;
      }
    }

    public class MeterInfoHardwareTypeMTypeZelsiusJoinedRow : DataRow
    {
      private DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable tableMeterInfoHardwareTypeMTypeZelsiusJoined;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MeterInfoHardwareTypeMTypeZelsiusJoinedRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterInfoHardwareTypeMTypeZelsiusJoined = (DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int HardwareTypeID
      {
        get => (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareTypeIDColumn];
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareTypeIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MapID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MapIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MapID' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LinkerTableID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.LinkerTableIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LinkerTableID' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.LinkerTableIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int FirmwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.FirmwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareVersion' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.FirmwareVersionColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string HardwareName
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareName' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareNameColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int HardwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareVersion' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareVersionColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int extEEPSize
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.extEEPSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'extEEPSize' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.extEEPSizeColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DescriptionColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterHardwareID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterHardwareIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterHardwareID' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterHardwareIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterTypeID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterTypeIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterTypeID' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterTypeIDColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string PPSArtikelNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.PPSArtikelNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PPSArtikelNr' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.PPSArtikelNrColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string DefaultFunctionNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DefaultFunctionNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DefaultFunctionNr' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DefaultFunctionNrColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte[] EEPdata
      {
        get
        {
          try
          {
            return (byte[]) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.EEPdataColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EEPdata' in table 'MeterInfoHardwareTypeMTypeZelsiusJoined' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.EEPdataColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string TypeOverrideString
      {
        get
        {
          return this.IsTypeOverrideStringNull() ? string.Empty : (string) this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.TypeOverrideStringColumn];
        }
        set
        {
          this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.TypeOverrideStringColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMapIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MapIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMapIDNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MapIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLinkerTableIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.LinkerTableIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLinkerTableIDNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.LinkerTableIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFirmwareVersionNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.FirmwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFirmwareVersionNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.FirmwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHardwareNameNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHardwareNameNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHardwareVersionNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHardwareVersionNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.HardwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsextEEPSizeNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.extEEPSizeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetextEEPSizeNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.extEEPSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDescriptionNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DescriptionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterHardwareIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterHardwareIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterHardwareIDNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterHardwareIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterTypeIDNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterTypeIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterTypeIDNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.MeterTypeIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPPSArtikelNrNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.PPSArtikelNrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPPSArtikelNrNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.PPSArtikelNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDefaultFunctionNrNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DefaultFunctionNrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDefaultFunctionNrNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.DefaultFunctionNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEEPdataNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.EEPdataColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEEPdataNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.EEPdataColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTypeOverrideStringNull()
      {
        return this.IsNull(this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.TypeOverrideStringColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTypeOverrideStringNull()
      {
        this[this.tableMeterInfoHardwareTypeMTypeZelsiusJoined.TypeOverrideStringColumn] = Convert.DBNull;
      }
    }

    public class CodeRuntimeCodeJoinedRow : DataRow
    {
      private DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable tableCodeRuntimeCodeJoined;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal CodeRuntimeCodeJoinedRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableCodeRuntimeCodeJoined = (DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int FunctionNumber
      {
        get => (int) this[this.tableCodeRuntimeCodeJoined.FunctionNumberColumn];
        set => this[this.tableCodeRuntimeCodeJoined.FunctionNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int CodeID
      {
        get => (int) this[this.tableCodeRuntimeCodeJoined.CodeIDColumn];
        set => this[this.tableCodeRuntimeCodeJoined.CodeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeSequenceType
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeRuntimeCodeJoined.CodeSequenceTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeSequenceType' in table 'CodeRuntimeCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeRuntimeCodeJoined.CodeSequenceTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeSequenceInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeRuntimeCodeJoined.CodeSequenceInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeSequenceInfo' in table 'CodeRuntimeCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeRuntimeCodeJoined.CodeSequenceInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeSequenceName
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeRuntimeCodeJoined.CodeSequenceNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeSequenceName' in table 'CodeRuntimeCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeRuntimeCodeJoined.CodeSequenceNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LineNr
      {
        get => (int) this[this.tableCodeRuntimeCodeJoined.LineNrColumn];
        set => this[this.tableCodeRuntimeCodeJoined.LineNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeType
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeRuntimeCodeJoined.CodeTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeType' in table 'CodeRuntimeCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeRuntimeCodeJoined.CodeTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeValue
      {
        get
        {
          return this.IsCodeValueNull() ? (string) null : (string) this[this.tableCodeRuntimeCodeJoined.CodeValueColumn];
        }
        set => this[this.tableCodeRuntimeCodeJoined.CodeValueColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LineInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeRuntimeCodeJoined.LineInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LineInfo' in table 'CodeRuntimeCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeRuntimeCodeJoined.LineInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeSequenceTypeNull()
      {
        return this.IsNull(this.tableCodeRuntimeCodeJoined.CodeSequenceTypeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeSequenceTypeNull()
      {
        this[this.tableCodeRuntimeCodeJoined.CodeSequenceTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeSequenceInfoNull()
      {
        return this.IsNull(this.tableCodeRuntimeCodeJoined.CodeSequenceInfoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeSequenceInfoNull()
      {
        this[this.tableCodeRuntimeCodeJoined.CodeSequenceInfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeSequenceNameNull()
      {
        return this.IsNull(this.tableCodeRuntimeCodeJoined.CodeSequenceNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeSequenceNameNull()
      {
        this[this.tableCodeRuntimeCodeJoined.CodeSequenceNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeTypeNull() => this.IsNull(this.tableCodeRuntimeCodeJoined.CodeTypeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeTypeNull()
      {
        this[this.tableCodeRuntimeCodeJoined.CodeTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeValueNull() => this.IsNull(this.tableCodeRuntimeCodeJoined.CodeValueColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeValueNull()
      {
        this[this.tableCodeRuntimeCodeJoined.CodeValueColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLineInfoNull() => this.IsNull(this.tableCodeRuntimeCodeJoined.LineInfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLineInfoNull()
      {
        this[this.tableCodeRuntimeCodeJoined.LineInfoColumn] = Convert.DBNull;
      }
    }

    public class CodeDisplayCodeJoinedRow : DataRow
    {
      private DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable tableCodeDisplayCodeJoined;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal CodeDisplayCodeJoinedRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableCodeDisplayCodeJoined = (DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int CodeID
      {
        get => (int) this[this.tableCodeDisplayCodeJoined.CodeIDColumn];
        set => this[this.tableCodeDisplayCodeJoined.CodeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LineNr
      {
        get => (int) this[this.tableCodeDisplayCodeJoined.LineNrColumn];
        set => this[this.tableCodeDisplayCodeJoined.LineNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeType
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeDisplayCodeJoined.CodeTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeType' in table 'CodeDisplayCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeDisplayCodeJoined.CodeTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeValue
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeDisplayCodeJoined.CodeValueColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeValue' in table 'CodeDisplayCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeDisplayCodeJoined.CodeValueColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LineInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeDisplayCodeJoined.LineInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LineInfo' in table 'CodeDisplayCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeDisplayCodeJoined.LineInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int InterpreterCode
      {
        get => (int) this[this.tableCodeDisplayCodeJoined.InterpreterCodeColumn];
        set => this[this.tableCodeDisplayCodeJoined.InterpreterCodeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int SequenceNr
      {
        get => (int) this[this.tableCodeDisplayCodeJoined.SequenceNrColumn];
        set => this[this.tableCodeDisplayCodeJoined.SequenceNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeSequenceType
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeDisplayCodeJoined.CodeSequenceTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeSequenceType' in table 'CodeDisplayCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeDisplayCodeJoined.CodeSequenceTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string CodeSequenceInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableCodeDisplayCodeJoined.CodeSequenceInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CodeSequenceInfo' in table 'CodeDisplayCodeJoined' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableCodeDisplayCodeJoined.CodeSequenceInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeTypeNull() => this.IsNull(this.tableCodeDisplayCodeJoined.CodeTypeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeTypeNull()
      {
        this[this.tableCodeDisplayCodeJoined.CodeTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeValueNull() => this.IsNull(this.tableCodeDisplayCodeJoined.CodeValueColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeValueNull()
      {
        this[this.tableCodeDisplayCodeJoined.CodeValueColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLineInfoNull() => this.IsNull(this.tableCodeDisplayCodeJoined.LineInfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLineInfoNull()
      {
        this[this.tableCodeDisplayCodeJoined.LineInfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeSequenceTypeNull()
      {
        return this.IsNull(this.tableCodeDisplayCodeJoined.CodeSequenceTypeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeSequenceTypeNull()
      {
        this[this.tableCodeDisplayCodeJoined.CodeSequenceTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCodeSequenceInfoNull()
      {
        return this.IsNull(this.tableCodeDisplayCodeJoined.CodeSequenceInfoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCodeSequenceInfoNull()
      {
        this[this.tableCodeDisplayCodeJoined.CodeSequenceInfoColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MeterInfoHardwareTypeJoinedRowChangeEvent : EventArgs
    {
      private DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterInfoHardwareTypeJoinedRowChangeEvent(
        DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeJoinedRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent : EventArgs
    {
      private DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MeterInfoHardwareTypeMTypeZelsiusJoinedRowChangeEvent(
        DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.MeterInfoHardwareTypeMTypeZelsiusJoinedRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class CodeRuntimeCodeJoinedRowChangeEvent : EventArgs
    {
      private DataSetGMM_Handler.CodeRuntimeCodeJoinedRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public CodeRuntimeCodeJoinedRowChangeEvent(
        DataSetGMM_Handler.CodeRuntimeCodeJoinedRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeRuntimeCodeJoinedRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class CodeDisplayCodeJoinedRowChangeEvent : EventArgs
    {
      private DataSetGMM_Handler.CodeDisplayCodeJoinedRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public CodeDisplayCodeJoinedRowChangeEvent(
        DataSetGMM_Handler.CodeDisplayCodeJoinedRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetGMM_Handler.CodeDisplayCodeJoinedRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
