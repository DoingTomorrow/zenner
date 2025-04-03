// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DataSets.HardwareTypeTables
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
  [XmlRoot("HardwareTypeTables")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class HardwareTypeTables : DataSet
  {
    private HardwareTypeTables.HardwareTypeDataTable tableHardwareType;
    private HardwareTypeTables.MapBaseDataTable tableMapBase;
    private HardwareTypeTables.ProgFilesDataTable tableProgFiles;
    private HardwareTypeTables.HardwareOverviewDataTable tableHardwareOverview;
    private HardwareTypeTables.HardwareAndFirmwareInfoDataTable tableHardwareAndFirmwareInfo;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public HardwareTypeTables()
    {
      this.BeginInit();
      this.InitClass();
      CollectionChangeEventHandler changeEventHandler = new CollectionChangeEventHandler(this.SchemaChanged);
      base.Tables.CollectionChanged += changeEventHandler;
      base.Relations.CollectionChanged += changeEventHandler;
      this.EndInit();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    protected HardwareTypeTables(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (HardwareType)] != null)
            base.Tables.Add((DataTable) new HardwareTypeTables.HardwareTypeDataTable(dataSet.Tables[nameof (HardwareType)]));
          if (dataSet.Tables[nameof (MapBase)] != null)
            base.Tables.Add((DataTable) new HardwareTypeTables.MapBaseDataTable(dataSet.Tables[nameof (MapBase)]));
          if (dataSet.Tables[nameof (ProgFiles)] != null)
            base.Tables.Add((DataTable) new HardwareTypeTables.ProgFilesDataTable(dataSet.Tables[nameof (ProgFiles)]));
          if (dataSet.Tables[nameof (HardwareOverview)] != null)
            base.Tables.Add((DataTable) new HardwareTypeTables.HardwareOverviewDataTable(dataSet.Tables[nameof (HardwareOverview)]));
          if (dataSet.Tables[nameof (HardwareAndFirmwareInfo)] != null)
            base.Tables.Add((DataTable) new HardwareTypeTables.HardwareAndFirmwareInfoDataTable(dataSet.Tables[nameof (HardwareAndFirmwareInfo)]));
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
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public HardwareTypeTables.HardwareTypeDataTable HardwareType => this.tableHardwareType;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public HardwareTypeTables.MapBaseDataTable MapBase => this.tableMapBase;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public HardwareTypeTables.ProgFilesDataTable ProgFiles => this.tableProgFiles;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public HardwareTypeTables.HardwareOverviewDataTable HardwareOverview
    {
      get => this.tableHardwareOverview;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public HardwareTypeTables.HardwareAndFirmwareInfoDataTable HardwareAndFirmwareInfo
    {
      get => this.tableHardwareAndFirmwareInfo;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override SchemaSerializationMode SchemaSerializationMode
    {
      get => this._schemaSerializationMode;
      set => this._schemaSerializationMode = value;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataTableCollection Tables => base.Tables;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataRelationCollection Relations => base.Relations;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    protected override void InitializeDerivedDataSet()
    {
      this.BeginInit();
      this.InitClass();
      this.EndInit();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public override DataSet Clone()
    {
      HardwareTypeTables hardwareTypeTables = (HardwareTypeTables) base.Clone();
      hardwareTypeTables.InitVars();
      hardwareTypeTables.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) hardwareTypeTables;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    protected override bool ShouldSerializeTables() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    protected override bool ShouldSerializeRelations() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    protected override void ReadXmlSerializable(XmlReader reader)
    {
      if (this.DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
      {
        this.Reset();
        DataSet dataSet = new DataSet();
        int num = (int) dataSet.ReadXml(reader);
        if (dataSet.Tables["HardwareType"] != null)
          base.Tables.Add((DataTable) new HardwareTypeTables.HardwareTypeDataTable(dataSet.Tables["HardwareType"]));
        if (dataSet.Tables["MapBase"] != null)
          base.Tables.Add((DataTable) new HardwareTypeTables.MapBaseDataTable(dataSet.Tables["MapBase"]));
        if (dataSet.Tables["ProgFiles"] != null)
          base.Tables.Add((DataTable) new HardwareTypeTables.ProgFilesDataTable(dataSet.Tables["ProgFiles"]));
        if (dataSet.Tables["HardwareOverview"] != null)
          base.Tables.Add((DataTable) new HardwareTypeTables.HardwareOverviewDataTable(dataSet.Tables["HardwareOverview"]));
        if (dataSet.Tables["HardwareAndFirmwareInfo"] != null)
          base.Tables.Add((DataTable) new HardwareTypeTables.HardwareAndFirmwareInfoDataTable(dataSet.Tables["HardwareAndFirmwareInfo"]));
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
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    protected override XmlSchema GetSchemaSerializable()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.WriteXmlSchema((XmlWriter) new XmlTextWriter((Stream) memoryStream, (Encoding) null));
      memoryStream.Position = 0L;
      return XmlSchema.Read((XmlReader) new XmlTextReader((Stream) memoryStream), (ValidationEventHandler) null);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    internal void InitVars() => this.InitVars(true);

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    internal void InitVars(bool initTable)
    {
      this.tableHardwareType = (HardwareTypeTables.HardwareTypeDataTable) base.Tables["HardwareType"];
      if (initTable && this.tableHardwareType != null)
        this.tableHardwareType.InitVars();
      this.tableMapBase = (HardwareTypeTables.MapBaseDataTable) base.Tables["MapBase"];
      if (initTable && this.tableMapBase != null)
        this.tableMapBase.InitVars();
      this.tableProgFiles = (HardwareTypeTables.ProgFilesDataTable) base.Tables["ProgFiles"];
      if (initTable && this.tableProgFiles != null)
        this.tableProgFiles.InitVars();
      this.tableHardwareOverview = (HardwareTypeTables.HardwareOverviewDataTable) base.Tables["HardwareOverview"];
      if (initTable && this.tableHardwareOverview != null)
        this.tableHardwareOverview.InitVars();
      this.tableHardwareAndFirmwareInfo = (HardwareTypeTables.HardwareAndFirmwareInfoDataTable) base.Tables["HardwareAndFirmwareInfo"];
      if (!initTable || this.tableHardwareAndFirmwareInfo == null)
        return;
      this.tableHardwareAndFirmwareInfo.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (HardwareTypeTables);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/HardwareTypeTables.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableHardwareType = new HardwareTypeTables.HardwareTypeDataTable();
      base.Tables.Add((DataTable) this.tableHardwareType);
      this.tableMapBase = new HardwareTypeTables.MapBaseDataTable();
      base.Tables.Add((DataTable) this.tableMapBase);
      this.tableProgFiles = new HardwareTypeTables.ProgFilesDataTable();
      base.Tables.Add((DataTable) this.tableProgFiles);
      this.tableHardwareOverview = new HardwareTypeTables.HardwareOverviewDataTable();
      base.Tables.Add((DataTable) this.tableHardwareOverview);
      this.tableHardwareAndFirmwareInfo = new HardwareTypeTables.HardwareAndFirmwareInfoDataTable();
      base.Tables.Add((DataTable) this.tableHardwareAndFirmwareInfo);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeHardwareType() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeMapBase() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeProgFiles() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeHardwareOverview() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeHardwareAndFirmwareInfo() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private void SchemaChanged(object sender, CollectionChangeEventArgs e)
    {
      if (e.Action != CollectionChangeAction.Remove)
        return;
      this.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
    {
      HardwareTypeTables hardwareTypeTables = new HardwareTypeTables();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = hardwareTypeTables.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = hardwareTypeTables.GetSchemaSerializable();
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

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class HardwareAndFirmwareInfoDataTable : 
      TypedTableBase<HardwareTypeTables.HardwareAndFirmwareInfoRow>
    {
      private DataColumn columnHardwareVersion;
      private DataColumn columnFirmwareVersion;
      private DataColumn columnHardwareTypeID;
      private DataColumn columnHardwareName;
      private DataColumn columnHardwareResource;
      private DataColumn columnDescription;
      private DataColumn columnTestinfo;
      private DataColumn columnHardwareOptions;
      private DataColumn columnMapID;
      private DataColumn columnOptions;
      private DataColumn columnSourceInfo;
      private DataColumn columnReleasedName;
      private DataColumn columnCompatibleOverwriteGroups;
      private DataColumn columnReleaseComments;
      private DataColumn columnFirmwareDependencies;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareAndFirmwareInfoDataTable()
      {
        this.TableName = "HardwareAndFirmwareInfo";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareAndFirmwareInfoDataTable(DataTable table)
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
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected HardwareAndFirmwareInfoDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareVersionColumn => this.columnHardwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FirmwareVersionColumn => this.columnFirmwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareTypeIDColumn => this.columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareNameColumn => this.columnHardwareName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareResourceColumn => this.columnHardwareResource;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TestinfoColumn => this.columnTestinfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareOptionsColumn => this.columnHardwareOptions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn OptionsColumn => this.columnOptions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SourceInfoColumn => this.columnSourceInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ReleasedNameColumn => this.columnReleasedName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CompatibleOverwriteGroupsColumn => this.columnCompatibleOverwriteGroups;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ReleaseCommentsColumn => this.columnReleaseComments;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FirmwareDependenciesColumn => this.columnFirmwareDependencies;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareAndFirmwareInfoRow this[int index]
      {
        get => (HardwareTypeTables.HardwareAndFirmwareInfoRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEventHandler HardwareAndFirmwareInfoRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEventHandler HardwareAndFirmwareInfoRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEventHandler HardwareAndFirmwareInfoRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEventHandler HardwareAndFirmwareInfoRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddHardwareAndFirmwareInfoRow(HardwareTypeTables.HardwareAndFirmwareInfoRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareAndFirmwareInfoRow AddHardwareAndFirmwareInfoRow(
        int HardwareVersion,
        int FirmwareVersion,
        int HardwareTypeID,
        string HardwareName,
        string HardwareResource,
        string Description,
        string Testinfo,
        string HardwareOptions,
        int MapID,
        string Options,
        string SourceInfo,
        string ReleasedName,
        string CompatibleOverwriteGroups,
        string ReleaseComments,
        string FirmwareDependencies)
      {
        HardwareTypeTables.HardwareAndFirmwareInfoRow row = (HardwareTypeTables.HardwareAndFirmwareInfoRow) this.NewRow();
        object[] objArray = new object[15]
        {
          (object) HardwareVersion,
          (object) FirmwareVersion,
          (object) HardwareTypeID,
          (object) HardwareName,
          (object) HardwareResource,
          (object) Description,
          (object) Testinfo,
          (object) HardwareOptions,
          (object) MapID,
          (object) Options,
          (object) SourceInfo,
          (object) ReleasedName,
          (object) CompatibleOverwriteGroups,
          (object) ReleaseComments,
          (object) FirmwareDependencies
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareAndFirmwareInfoRow FindByHardwareVersionFirmwareVersion(
        int HardwareVersion,
        int FirmwareVersion)
      {
        return (HardwareTypeTables.HardwareAndFirmwareInfoRow) this.Rows.Find(new object[2]
        {
          (object) HardwareVersion,
          (object) FirmwareVersion
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HardwareTypeTables.HardwareAndFirmwareInfoDataTable firmwareInfoDataTable = (HardwareTypeTables.HardwareAndFirmwareInfoDataTable) base.Clone();
        firmwareInfoDataTable.InitVars();
        return (DataTable) firmwareInfoDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HardwareTypeTables.HardwareAndFirmwareInfoDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnHardwareVersion = this.Columns["HardwareVersion"];
        this.columnFirmwareVersion = this.Columns["FirmwareVersion"];
        this.columnHardwareTypeID = this.Columns["HardwareTypeID"];
        this.columnHardwareName = this.Columns["HardwareName"];
        this.columnHardwareResource = this.Columns["HardwareResource"];
        this.columnDescription = this.Columns["Description"];
        this.columnTestinfo = this.Columns["Testinfo"];
        this.columnHardwareOptions = this.Columns["HardwareOptions"];
        this.columnMapID = this.Columns["MapID"];
        this.columnOptions = this.Columns["Options"];
        this.columnSourceInfo = this.Columns["SourceInfo"];
        this.columnReleasedName = this.Columns["ReleasedName"];
        this.columnCompatibleOverwriteGroups = this.Columns["CompatibleOverwriteGroups"];
        this.columnReleaseComments = this.Columns["ReleaseComments"];
        this.columnFirmwareDependencies = this.Columns["FirmwareDependencies"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnHardwareVersion = new DataColumn("HardwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareVersion);
        this.columnFirmwareVersion = new DataColumn("FirmwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareVersion);
        this.columnHardwareTypeID = new DataColumn("HardwareTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareTypeID);
        this.columnHardwareName = new DataColumn("HardwareName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareName);
        this.columnHardwareResource = new DataColumn("HardwareResource", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareResource);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.columnTestinfo = new DataColumn("Testinfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTestinfo);
        this.columnHardwareOptions = new DataColumn("HardwareOptions", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareOptions);
        this.columnMapID = new DataColumn("MapID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMapID);
        this.columnOptions = new DataColumn("Options", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOptions);
        this.columnSourceInfo = new DataColumn("SourceInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSourceInfo);
        this.columnReleasedName = new DataColumn("ReleasedName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnReleasedName);
        this.columnCompatibleOverwriteGroups = new DataColumn("CompatibleOverwriteGroups", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCompatibleOverwriteGroups);
        this.columnReleaseComments = new DataColumn("ReleaseComments", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnReleaseComments);
        this.columnFirmwareDependencies = new DataColumn("FirmwareDependencies", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareDependencies);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnHardwareVersion,
          this.columnFirmwareVersion
        }, true));
        this.columnHardwareVersion.AllowDBNull = false;
        this.columnFirmwareVersion.AllowDBNull = false;
        this.columnHardwareTypeID.AllowDBNull = false;
        this.columnHardwareName.MaxLength = 50;
        this.columnHardwareResource.MaxLength = 536870910;
        this.columnDescription.MaxLength = 536870910;
        this.columnTestinfo.MaxLength = 536870910;
        this.columnHardwareOptions.MaxLength = (int) byte.MaxValue;
        this.columnMapID.AllowDBNull = false;
        this.columnOptions.MaxLength = (int) byte.MaxValue;
        this.columnSourceInfo.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareAndFirmwareInfoRow NewHardwareAndFirmwareInfoRow()
      {
        return (HardwareTypeTables.HardwareAndFirmwareInfoRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HardwareTypeTables.HardwareAndFirmwareInfoRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (HardwareTypeTables.HardwareAndFirmwareInfoRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HardwareAndFirmwareInfoRowChanged == null)
          return;
        this.HardwareAndFirmwareInfoRowChanged((object) this, new HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEvent((HardwareTypeTables.HardwareAndFirmwareInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HardwareAndFirmwareInfoRowChanging == null)
          return;
        this.HardwareAndFirmwareInfoRowChanging((object) this, new HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEvent((HardwareTypeTables.HardwareAndFirmwareInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HardwareAndFirmwareInfoRowDeleted == null)
          return;
        this.HardwareAndFirmwareInfoRowDeleted((object) this, new HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEvent((HardwareTypeTables.HardwareAndFirmwareInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HardwareAndFirmwareInfoRowDeleting == null)
          return;
        this.HardwareAndFirmwareInfoRowDeleting((object) this, new HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEvent((HardwareTypeTables.HardwareAndFirmwareInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveHardwareAndFirmwareInfoRow(HardwareTypeTables.HardwareAndFirmwareInfoRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HardwareTypeTables hardwareTypeTables = new HardwareTypeTables();
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
          FixedValue = hardwareTypeTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HardwareAndFirmwareInfoDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = hardwareTypeTables.GetSchemaSerializable();
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
    public class HardwareOverviewDataTable : TypedTableBase<HardwareTypeTables.HardwareOverviewRow>
    {
      private DataColumn columnHardwareName;
      private DataColumn columnHardwareTypeID;
      private DataColumn columnMapID;
      private DataColumn columnFirmwareVersion;
      private DataColumn columnHardwareVersion;
      private DataColumn columnHardwareResource;
      private DataColumn columnDescription;
      private DataColumn columnTestinfo;
      private DataColumn columnHardwareOptions;
      private DataColumn columnCompatibleFirmwares;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareOverviewDataTable()
      {
        this.TableName = "HardwareOverview";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareOverviewDataTable(DataTable table)
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
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected HardwareOverviewDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareNameColumn => this.columnHardwareName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareTypeIDColumn => this.columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FirmwareVersionColumn => this.columnFirmwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareVersionColumn => this.columnHardwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareResourceColumn => this.columnHardwareResource;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TestinfoColumn => this.columnTestinfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareOptionsColumn => this.columnHardwareOptions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CompatibleFirmwaresColumn => this.columnCompatibleFirmwares;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareOverviewRow this[int index]
      {
        get => (HardwareTypeTables.HardwareOverviewRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareOverviewRowChangeEventHandler HardwareOverviewRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareOverviewRowChangeEventHandler HardwareOverviewRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareOverviewRowChangeEventHandler HardwareOverviewRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareOverviewRowChangeEventHandler HardwareOverviewRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddHardwareOverviewRow(HardwareTypeTables.HardwareOverviewRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareOverviewRow AddHardwareOverviewRow(
        string HardwareName,
        int HardwareTypeID,
        int MapID,
        string FirmwareVersion,
        string HardwareVersion,
        string HardwareResource,
        string Description,
        string Testinfo,
        string HardwareOptions,
        int CompatibleFirmwares)
      {
        HardwareTypeTables.HardwareOverviewRow row = (HardwareTypeTables.HardwareOverviewRow) this.NewRow();
        object[] objArray = new object[10]
        {
          (object) HardwareName,
          (object) HardwareTypeID,
          (object) MapID,
          (object) FirmwareVersion,
          (object) HardwareVersion,
          (object) HardwareResource,
          (object) Description,
          (object) Testinfo,
          (object) HardwareOptions,
          (object) CompatibleFirmwares
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HardwareTypeTables.HardwareOverviewDataTable overviewDataTable = (HardwareTypeTables.HardwareOverviewDataTable) base.Clone();
        overviewDataTable.InitVars();
        return (DataTable) overviewDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HardwareTypeTables.HardwareOverviewDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnHardwareName = this.Columns["HardwareName"];
        this.columnHardwareTypeID = this.Columns["HardwareTypeID"];
        this.columnMapID = this.Columns["MapID"];
        this.columnFirmwareVersion = this.Columns["FirmwareVersion"];
        this.columnHardwareVersion = this.Columns["HardwareVersion"];
        this.columnHardwareResource = this.Columns["HardwareResource"];
        this.columnDescription = this.Columns["Description"];
        this.columnTestinfo = this.Columns["Testinfo"];
        this.columnHardwareOptions = this.Columns["HardwareOptions"];
        this.columnCompatibleFirmwares = this.Columns["CompatibleFirmwares"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnHardwareName = new DataColumn("HardwareName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareName);
        this.columnHardwareTypeID = new DataColumn("HardwareTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareTypeID);
        this.columnMapID = new DataColumn("MapID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMapID);
        this.columnFirmwareVersion = new DataColumn("FirmwareVersion", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareVersion);
        this.columnHardwareVersion = new DataColumn("HardwareVersion", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareVersion);
        this.columnHardwareResource = new DataColumn("HardwareResource", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareResource);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.columnTestinfo = new DataColumn("Testinfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTestinfo);
        this.columnHardwareOptions = new DataColumn("HardwareOptions", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareOptions);
        this.columnCompatibleFirmwares = new DataColumn("CompatibleFirmwares", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCompatibleFirmwares);
        this.columnHardwareName.MaxLength = 50;
        this.columnHardwareTypeID.AllowDBNull = false;
        this.columnHardwareResource.MaxLength = 536870910;
        this.columnDescription.MaxLength = 536870910;
        this.columnTestinfo.MaxLength = 536870910;
        this.columnHardwareOptions.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareOverviewRow NewHardwareOverviewRow()
      {
        return (HardwareTypeTables.HardwareOverviewRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HardwareTypeTables.HardwareOverviewRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (HardwareTypeTables.HardwareOverviewRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HardwareOverviewRowChanged == null)
          return;
        this.HardwareOverviewRowChanged((object) this, new HardwareTypeTables.HardwareOverviewRowChangeEvent((HardwareTypeTables.HardwareOverviewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HardwareOverviewRowChanging == null)
          return;
        this.HardwareOverviewRowChanging((object) this, new HardwareTypeTables.HardwareOverviewRowChangeEvent((HardwareTypeTables.HardwareOverviewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HardwareOverviewRowDeleted == null)
          return;
        this.HardwareOverviewRowDeleted((object) this, new HardwareTypeTables.HardwareOverviewRowChangeEvent((HardwareTypeTables.HardwareOverviewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HardwareOverviewRowDeleting == null)
          return;
        this.HardwareOverviewRowDeleting((object) this, new HardwareTypeTables.HardwareOverviewRowChangeEvent((HardwareTypeTables.HardwareOverviewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveHardwareOverviewRow(HardwareTypeTables.HardwareOverviewRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HardwareTypeTables hardwareTypeTables = new HardwareTypeTables();
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
          FixedValue = hardwareTypeTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HardwareOverviewDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = hardwareTypeTables.GetSchemaSerializable();
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

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void HardwareTypeRowChangeEventHandler(
      object sender,
      HardwareTypeTables.HardwareTypeRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void MapBaseRowChangeEventHandler(
      object sender,
      HardwareTypeTables.MapBaseRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void ProgFilesRowChangeEventHandler(
      object sender,
      HardwareTypeTables.ProgFilesRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void HardwareOverviewRowChangeEventHandler(
      object sender,
      HardwareTypeTables.HardwareOverviewRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void HardwareAndFirmwareInfoRowChangeEventHandler(
      object sender,
      HardwareTypeTables.HardwareAndFirmwareInfoRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class HardwareTypeDataTable : TypedTableBase<HardwareTypeTables.HardwareTypeRow>
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

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeDataTable()
      {
        this.TableName = "HardwareType";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareTypeDataTable(DataTable table)
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
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected HardwareTypeDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareTypeIDColumn => this.columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LinkerTableIDColumn => this.columnLinkerTableID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FirmwareVersionColumn => this.columnFirmwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareNameColumn => this.columnHardwareName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareVersionColumn => this.columnHardwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareResourceColumn => this.columnHardwareResource;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn extEEPSizeColumn => this.columnextEEPSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn maxStackSizeColumn => this.columnmaxStackSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RAMSizeColumn => this.columnRAMSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RAMStartAdrColumn => this.columnRAMStartAdr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn intEEPStartAdrColumn => this.columnintEEPStartAdr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn intEEPSizeColumn => this.columnintEEPSize;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TestinfoColumn => this.columnTestinfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareOptionsColumn => this.columnHardwareOptions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareTypeRow this[int index]
      {
        get => (HardwareTypeTables.HardwareTypeRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareTypeRowChangeEventHandler HardwareTypeRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareTypeRowChangeEventHandler HardwareTypeRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareTypeRowChangeEventHandler HardwareTypeRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.HardwareTypeRowChangeEventHandler HardwareTypeRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddHardwareTypeRow(HardwareTypeTables.HardwareTypeRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareTypeRow AddHardwareTypeRow(
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
        string HardwareOptions)
      {
        HardwareTypeTables.HardwareTypeRow row = (HardwareTypeTables.HardwareTypeRow) this.NewRow();
        object[] objArray = new object[16]
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
          (object) HardwareOptions
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareTypeRow FindByHardwareTypeID(int HardwareTypeID)
      {
        return (HardwareTypeTables.HardwareTypeRow) this.Rows.Find(new object[1]
        {
          (object) HardwareTypeID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable = (HardwareTypeTables.HardwareTypeDataTable) base.Clone();
        hardwareTypeDataTable.InitVars();
        return (DataTable) hardwareTypeDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HardwareTypeTables.HardwareTypeDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
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
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
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
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnHardwareTypeID
        }, true));
        this.columnHardwareTypeID.AllowDBNull = false;
        this.columnHardwareTypeID.Unique = true;
        this.columnHardwareName.MaxLength = 50;
        this.columnHardwareResource.MaxLength = 536870910;
        this.columnDescription.MaxLength = 536870910;
        this.columnTestinfo.MaxLength = 536870910;
        this.columnHardwareOptions.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareTypeRow NewHardwareTypeRow()
      {
        return (HardwareTypeTables.HardwareTypeRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HardwareTypeTables.HardwareTypeRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (HardwareTypeTables.HardwareTypeRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HardwareTypeRowChanged == null)
          return;
        this.HardwareTypeRowChanged((object) this, new HardwareTypeTables.HardwareTypeRowChangeEvent((HardwareTypeTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HardwareTypeRowChanging == null)
          return;
        this.HardwareTypeRowChanging((object) this, new HardwareTypeTables.HardwareTypeRowChangeEvent((HardwareTypeTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HardwareTypeRowDeleted == null)
          return;
        this.HardwareTypeRowDeleted((object) this, new HardwareTypeTables.HardwareTypeRowChangeEvent((HardwareTypeTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HardwareTypeRowDeleting == null)
          return;
        this.HardwareTypeRowDeleting((object) this, new HardwareTypeTables.HardwareTypeRowChangeEvent((HardwareTypeTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveHardwareTypeRow(HardwareTypeTables.HardwareTypeRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HardwareTypeTables hardwareTypeTables = new HardwareTypeTables();
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
          FixedValue = hardwareTypeTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HardwareTypeDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = hardwareTypeTables.GetSchemaSerializable();
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
    public class MapBaseDataTable : TypedTableBase<HardwareTypeTables.MapBaseRow>
    {
      private DataColumn columnMapID;
      private DataColumn columnSystemName;
      private DataColumn columnSystemVersion;
      private DataColumn columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MapBaseDataTable()
      {
        this.TableName = "MapBase";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MapBaseDataTable(DataTable table)
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
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected MapBaseDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SystemNameColumn => this.columnSystemName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SystemVersionColumn => this.columnSystemVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.MapBaseRow this[int index]
      {
        get => (HardwareTypeTables.MapBaseRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.MapBaseRowChangeEventHandler MapBaseRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.MapBaseRowChangeEventHandler MapBaseRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.MapBaseRowChangeEventHandler MapBaseRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.MapBaseRowChangeEventHandler MapBaseRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddMapBaseRow(HardwareTypeTables.MapBaseRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.MapBaseRow AddMapBaseRow(
        short MapID,
        string SystemName,
        string SystemVersion,
        string Description)
      {
        HardwareTypeTables.MapBaseRow row = (HardwareTypeTables.MapBaseRow) this.NewRow();
        object[] objArray = new object[4]
        {
          (object) MapID,
          (object) SystemName,
          (object) SystemVersion,
          (object) Description
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.MapBaseRow FindByMapID(short MapID)
      {
        return (HardwareTypeTables.MapBaseRow) this.Rows.Find(new object[1]
        {
          (object) MapID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HardwareTypeTables.MapBaseDataTable mapBaseDataTable = (HardwareTypeTables.MapBaseDataTable) base.Clone();
        mapBaseDataTable.InitVars();
        return (DataTable) mapBaseDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HardwareTypeTables.MapBaseDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnMapID = this.Columns["MapID"];
        this.columnSystemName = this.Columns["SystemName"];
        this.columnSystemVersion = this.Columns["SystemVersion"];
        this.columnDescription = this.Columns["Description"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnMapID = new DataColumn("MapID", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMapID);
        this.columnSystemName = new DataColumn("SystemName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSystemName);
        this.columnSystemVersion = new DataColumn("SystemVersion", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSystemVersion);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnMapID
        }, true));
        this.columnMapID.AllowDBNull = false;
        this.columnMapID.Unique = true;
        this.columnSystemName.MaxLength = 50;
        this.columnSystemVersion.MaxLength = 50;
        this.columnDescription.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.MapBaseRow NewMapBaseRow()
      {
        return (HardwareTypeTables.MapBaseRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HardwareTypeTables.MapBaseRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (HardwareTypeTables.MapBaseRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MapBaseRowChanged == null)
          return;
        this.MapBaseRowChanged((object) this, new HardwareTypeTables.MapBaseRowChangeEvent((HardwareTypeTables.MapBaseRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MapBaseRowChanging == null)
          return;
        this.MapBaseRowChanging((object) this, new HardwareTypeTables.MapBaseRowChangeEvent((HardwareTypeTables.MapBaseRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MapBaseRowDeleted == null)
          return;
        this.MapBaseRowDeleted((object) this, new HardwareTypeTables.MapBaseRowChangeEvent((HardwareTypeTables.MapBaseRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MapBaseRowDeleting == null)
          return;
        this.MapBaseRowDeleting((object) this, new HardwareTypeTables.MapBaseRowChangeEvent((HardwareTypeTables.MapBaseRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveMapBaseRow(HardwareTypeTables.MapBaseRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HardwareTypeTables hardwareTypeTables = new HardwareTypeTables();
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
          FixedValue = hardwareTypeTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MapBaseDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = hardwareTypeTables.GetSchemaSerializable();
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
    public class ProgFilesDataTable : TypedTableBase<HardwareTypeTables.ProgFilesRow>
    {
      private DataColumn columnMapID;
      private DataColumn columnProgFileName;
      private DataColumn columnOptions;
      private DataColumn columnHexText;
      private DataColumn columnSourceInfo;
      private DataColumn columnHardwareName;
      private DataColumn columnHardwareTypeMapID;
      private DataColumn columnFirmwareVersion;
      private DataColumn columnReleasedName;
      private DataColumn columnCompatibleOverwriteGroups;
      private DataColumn columnReleaseComments;
      private DataColumn columnFirmwareDependencies;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ProgFilesDataTable()
      {
        this.TableName = "ProgFiles";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ProgFilesDataTable(DataTable table)
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
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected ProgFilesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MapIDColumn => this.columnMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ProgFileNameColumn => this.columnProgFileName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn OptionsColumn => this.columnOptions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HexTextColumn => this.columnHexText;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SourceInfoColumn => this.columnSourceInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareNameColumn => this.columnHardwareName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareTypeMapIDColumn => this.columnHardwareTypeMapID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FirmwareVersionColumn => this.columnFirmwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ReleasedNameColumn => this.columnReleasedName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CompatibleOverwriteGroupsColumn => this.columnCompatibleOverwriteGroups;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ReleaseCommentsColumn => this.columnReleaseComments;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FirmwareDependenciesColumn => this.columnFirmwareDependencies;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.ProgFilesRow this[int index]
      {
        get => (HardwareTypeTables.ProgFilesRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.ProgFilesRowChangeEventHandler ProgFilesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.ProgFilesRowChangeEventHandler ProgFilesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.ProgFilesRowChangeEventHandler ProgFilesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HardwareTypeTables.ProgFilesRowChangeEventHandler ProgFilesRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddProgFilesRow(HardwareTypeTables.ProgFilesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.ProgFilesRow AddProgFilesRow(
        int MapID,
        string ProgFileName,
        string Options,
        string HexText,
        string SourceInfo,
        string HardwareName,
        int HardwareTypeMapID,
        int FirmwareVersion,
        string ReleasedName,
        string CompatibleOverwriteGroups,
        string ReleaseComments,
        string FirmwareDependencies)
      {
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) this.NewRow();
        object[] objArray = new object[12]
        {
          (object) MapID,
          (object) ProgFileName,
          (object) Options,
          (object) HexText,
          (object) SourceInfo,
          (object) HardwareName,
          (object) HardwareTypeMapID,
          (object) FirmwareVersion,
          (object) ReleasedName,
          (object) CompatibleOverwriteGroups,
          (object) ReleaseComments,
          (object) FirmwareDependencies
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.ProgFilesRow FindByMapID(int MapID)
      {
        return (HardwareTypeTables.ProgFilesRow) this.Rows.Find(new object[1]
        {
          (object) MapID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = (HardwareTypeTables.ProgFilesDataTable) base.Clone();
        progFilesDataTable.InitVars();
        return (DataTable) progFilesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HardwareTypeTables.ProgFilesDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnMapID = this.Columns["MapID"];
        this.columnProgFileName = this.Columns["ProgFileName"];
        this.columnOptions = this.Columns["Options"];
        this.columnHexText = this.Columns["HexText"];
        this.columnSourceInfo = this.Columns["SourceInfo"];
        this.columnHardwareName = this.Columns["HardwareName"];
        this.columnHardwareTypeMapID = this.Columns["HardwareTypeMapID"];
        this.columnFirmwareVersion = this.Columns["FirmwareVersion"];
        this.columnReleasedName = this.Columns["ReleasedName"];
        this.columnCompatibleOverwriteGroups = this.Columns["CompatibleOverwriteGroups"];
        this.columnReleaseComments = this.Columns["ReleaseComments"];
        this.columnFirmwareDependencies = this.Columns["FirmwareDependencies"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnMapID = new DataColumn("MapID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMapID);
        this.columnProgFileName = new DataColumn("ProgFileName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnProgFileName);
        this.columnOptions = new DataColumn("Options", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOptions);
        this.columnHexText = new DataColumn("HexText", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHexText);
        this.columnSourceInfo = new DataColumn("SourceInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSourceInfo);
        this.columnHardwareName = new DataColumn("HardwareName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareName);
        this.columnHardwareTypeMapID = new DataColumn("HardwareTypeMapID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareTypeMapID);
        this.columnFirmwareVersion = new DataColumn("FirmwareVersion", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareVersion);
        this.columnReleasedName = new DataColumn("ReleasedName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnReleasedName);
        this.columnCompatibleOverwriteGroups = new DataColumn("CompatibleOverwriteGroups", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCompatibleOverwriteGroups);
        this.columnReleaseComments = new DataColumn("ReleaseComments", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnReleaseComments);
        this.columnFirmwareDependencies = new DataColumn("FirmwareDependencies", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFirmwareDependencies);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnMapID
        }, true));
        this.columnMapID.AllowDBNull = false;
        this.columnMapID.Unique = true;
        this.columnProgFileName.MaxLength = 50;
        this.columnOptions.MaxLength = (int) byte.MaxValue;
        this.columnHexText.MaxLength = 536870910;
        this.columnSourceInfo.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.ProgFilesRow NewProgFilesRow()
      {
        return (HardwareTypeTables.ProgFilesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HardwareTypeTables.ProgFilesRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (HardwareTypeTables.ProgFilesRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ProgFilesRowChanged == null)
          return;
        this.ProgFilesRowChanged((object) this, new HardwareTypeTables.ProgFilesRowChangeEvent((HardwareTypeTables.ProgFilesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ProgFilesRowChanging == null)
          return;
        this.ProgFilesRowChanging((object) this, new HardwareTypeTables.ProgFilesRowChangeEvent((HardwareTypeTables.ProgFilesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ProgFilesRowDeleted == null)
          return;
        this.ProgFilesRowDeleted((object) this, new HardwareTypeTables.ProgFilesRowChangeEvent((HardwareTypeTables.ProgFilesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ProgFilesRowDeleting == null)
          return;
        this.ProgFilesRowDeleting((object) this, new HardwareTypeTables.ProgFilesRowChangeEvent((HardwareTypeTables.ProgFilesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveProgFilesRow(HardwareTypeTables.ProgFilesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HardwareTypeTables hardwareTypeTables = new HardwareTypeTables();
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
          FixedValue = hardwareTypeTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ProgFilesDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = hardwareTypeTables.GetSchemaSerializable();
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

    public class HardwareTypeRow : DataRow
    {
      private HardwareTypeTables.HardwareTypeDataTable tableHardwareType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareTypeRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHardwareType = (HardwareTypeTables.HardwareTypeDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareTypeID
      {
        get => (int) this[this.tableHardwareType.HardwareTypeIDColumn];
        set => this[this.tableHardwareType.HardwareTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MapID
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareType.MapIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MapID' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int LinkerTableID
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareType.LinkerTableIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LinkerTableID' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.LinkerTableIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int FirmwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareType.FirmwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareVersion' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.FirmwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareName
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareType.HardwareNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareName' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.HardwareNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareType.HardwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareVersion' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.HardwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareResource
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareType.HardwareResourceColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareResource' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.HardwareResourceColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int extEEPSize
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareType.extEEPSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'extEEPSize' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.extEEPSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short maxStackSize
      {
        get
        {
          try
          {
            return (short) this[this.tableHardwareType.maxStackSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'maxStackSize' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.maxStackSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short RAMSize
      {
        get
        {
          try
          {
            return (short) this[this.tableHardwareType.RAMSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RAMSize' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.RAMSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short RAMStartAdr
      {
        get
        {
          try
          {
            return (short) this[this.tableHardwareType.RAMStartAdrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RAMStartAdr' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.RAMStartAdrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short intEEPStartAdr
      {
        get
        {
          try
          {
            return (short) this[this.tableHardwareType.intEEPStartAdrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'intEEPStartAdr' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.intEEPStartAdrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short intEEPSize
      {
        get
        {
          try
          {
            return (short) this[this.tableHardwareType.intEEPSizeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'intEEPSize' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.intEEPSizeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareType.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Testinfo
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareType.TestinfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Testinfo' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.TestinfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareOptions
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareType.HardwareOptionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareOptions' in table 'HardwareType' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareType.HardwareOptionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMapIDNull() => this.IsNull(this.tableHardwareType.MapIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMapIDNull() => this[this.tableHardwareType.MapIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsLinkerTableIDNull() => this.IsNull(this.tableHardwareType.LinkerTableIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetLinkerTableIDNull()
      {
        this[this.tableHardwareType.LinkerTableIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFirmwareVersionNull()
      {
        return this.IsNull(this.tableHardwareType.FirmwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFirmwareVersionNull()
      {
        this[this.tableHardwareType.FirmwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareNameNull() => this.IsNull(this.tableHardwareType.HardwareNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareNameNull()
      {
        this[this.tableHardwareType.HardwareNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareVersionNull()
      {
        return this.IsNull(this.tableHardwareType.HardwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareVersionNull()
      {
        this[this.tableHardwareType.HardwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareResourceNull()
      {
        return this.IsNull(this.tableHardwareType.HardwareResourceColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareResourceNull()
      {
        this[this.tableHardwareType.HardwareResourceColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsextEEPSizeNull() => this.IsNull(this.tableHardwareType.extEEPSizeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetextEEPSizeNull()
      {
        this[this.tableHardwareType.extEEPSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsmaxStackSizeNull() => this.IsNull(this.tableHardwareType.maxStackSizeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetmaxStackSizeNull()
      {
        this[this.tableHardwareType.maxStackSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsRAMSizeNull() => this.IsNull(this.tableHardwareType.RAMSizeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetRAMSizeNull() => this[this.tableHardwareType.RAMSizeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsRAMStartAdrNull() => this.IsNull(this.tableHardwareType.RAMStartAdrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetRAMStartAdrNull()
      {
        this[this.tableHardwareType.RAMStartAdrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsintEEPStartAdrNull()
      {
        return this.IsNull(this.tableHardwareType.intEEPStartAdrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetintEEPStartAdrNull()
      {
        this[this.tableHardwareType.intEEPStartAdrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsintEEPSizeNull() => this.IsNull(this.tableHardwareType.intEEPSizeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetintEEPSizeNull()
      {
        this[this.tableHardwareType.intEEPSizeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableHardwareType.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableHardwareType.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsTestinfoNull() => this.IsNull(this.tableHardwareType.TestinfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetTestinfoNull() => this[this.tableHardwareType.TestinfoColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareOptionsNull()
      {
        return this.IsNull(this.tableHardwareType.HardwareOptionsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareOptionsNull()
      {
        this[this.tableHardwareType.HardwareOptionsColumn] = Convert.DBNull;
      }
    }

    public class MapBaseRow : DataRow
    {
      private HardwareTypeTables.MapBaseDataTable tableMapBase;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MapBaseRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMapBase = (HardwareTypeTables.MapBaseDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short MapID
      {
        get => (short) this[this.tableMapBase.MapIDColumn];
        set => this[this.tableMapBase.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string SystemName
      {
        get
        {
          try
          {
            return (string) this[this.tableMapBase.SystemNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SystemName' in table 'MapBase' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMapBase.SystemNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string SystemVersion
      {
        get
        {
          try
          {
            return (string) this[this.tableMapBase.SystemVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SystemVersion' in table 'MapBase' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMapBase.SystemVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableMapBase.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'MapBase' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMapBase.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSystemNameNull() => this.IsNull(this.tableMapBase.SystemNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSystemNameNull() => this[this.tableMapBase.SystemNameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSystemVersionNull() => this.IsNull(this.tableMapBase.SystemVersionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSystemVersionNull()
      {
        this[this.tableMapBase.SystemVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableMapBase.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableMapBase.DescriptionColumn] = Convert.DBNull;
      }
    }

    public class ProgFilesRow : DataRow
    {
      private HardwareTypeTables.ProgFilesDataTable tableProgFiles;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ProgFilesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableProgFiles = (HardwareTypeTables.ProgFilesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MapID
      {
        get => (int) this[this.tableProgFiles.MapIDColumn];
        set => this[this.tableProgFiles.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ProgFileName
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.ProgFileNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ProgFileName' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.ProgFileNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Options
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.OptionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Options' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.OptionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HexText
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.HexTextColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HexText' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.HexTextColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string SourceInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.SourceInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SourceInfo' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.SourceInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareName
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.HardwareNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareName' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.HardwareNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareTypeMapID
      {
        get
        {
          try
          {
            return (int) this[this.tableProgFiles.HardwareTypeMapIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareTypeMapID' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.HardwareTypeMapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int FirmwareVersion
      {
        get
        {
          try
          {
            return (int) this[this.tableProgFiles.FirmwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareVersion' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.FirmwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ReleasedName
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.ReleasedNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ReleasedName' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.ReleasedNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string CompatibleOverwriteGroups
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.CompatibleOverwriteGroupsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CompatibleOverwriteGroups' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.CompatibleOverwriteGroupsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ReleaseComments
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.ReleaseCommentsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ReleaseComments' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.ReleaseCommentsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string FirmwareDependencies
      {
        get
        {
          try
          {
            return (string) this[this.tableProgFiles.FirmwareDependenciesColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareDependencies' in table 'ProgFiles' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableProgFiles.FirmwareDependenciesColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsProgFileNameNull() => this.IsNull(this.tableProgFiles.ProgFileNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetProgFileNameNull()
      {
        this[this.tableProgFiles.ProgFileNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsOptionsNull() => this.IsNull(this.tableProgFiles.OptionsColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetOptionsNull() => this[this.tableProgFiles.OptionsColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHexTextNull() => this.IsNull(this.tableProgFiles.HexTextColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHexTextNull() => this[this.tableProgFiles.HexTextColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSourceInfoNull() => this.IsNull(this.tableProgFiles.SourceInfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSourceInfoNull()
      {
        this[this.tableProgFiles.SourceInfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareNameNull() => this.IsNull(this.tableProgFiles.HardwareNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareNameNull()
      {
        this[this.tableProgFiles.HardwareNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareTypeMapIDNull()
      {
        return this.IsNull(this.tableProgFiles.HardwareTypeMapIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareTypeMapIDNull()
      {
        this[this.tableProgFiles.HardwareTypeMapIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFirmwareVersionNull() => this.IsNull(this.tableProgFiles.FirmwareVersionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFirmwareVersionNull()
      {
        this[this.tableProgFiles.FirmwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsReleasedNameNull() => this.IsNull(this.tableProgFiles.ReleasedNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetReleasedNameNull()
      {
        this[this.tableProgFiles.ReleasedNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsCompatibleOverwriteGroupsNull()
      {
        return this.IsNull(this.tableProgFiles.CompatibleOverwriteGroupsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetCompatibleOverwriteGroupsNull()
      {
        this[this.tableProgFiles.CompatibleOverwriteGroupsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsReleaseCommentsNull() => this.IsNull(this.tableProgFiles.ReleaseCommentsColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetReleaseCommentsNull()
      {
        this[this.tableProgFiles.ReleaseCommentsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFirmwareDependenciesNull()
      {
        return this.IsNull(this.tableProgFiles.FirmwareDependenciesColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFirmwareDependenciesNull()
      {
        this[this.tableProgFiles.FirmwareDependenciesColumn] = Convert.DBNull;
      }
    }

    public class HardwareOverviewRow : DataRow
    {
      private HardwareTypeTables.HardwareOverviewDataTable tableHardwareOverview;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareOverviewRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHardwareOverview = (HardwareTypeTables.HardwareOverviewDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareName
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.HardwareNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareName' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.HardwareNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareTypeID
      {
        get => (int) this[this.tableHardwareOverview.HardwareTypeIDColumn];
        set => this[this.tableHardwareOverview.HardwareTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MapID
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareOverview.MapIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MapID' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string FirmwareVersion
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.FirmwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareVersion' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.FirmwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareVersion
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.HardwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareVersion' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.HardwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareResource
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.HardwareResourceColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareResource' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.HardwareResourceColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Testinfo
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.TestinfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Testinfo' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.TestinfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareOptions
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareOverview.HardwareOptionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareOptions' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.HardwareOptionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int CompatibleFirmwares
      {
        get
        {
          try
          {
            return (int) this[this.tableHardwareOverview.CompatibleFirmwaresColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CompatibleFirmwares' in table 'HardwareOverview' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareOverview.CompatibleFirmwaresColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareNameNull()
      {
        return this.IsNull(this.tableHardwareOverview.HardwareNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareNameNull()
      {
        this[this.tableHardwareOverview.HardwareNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMapIDNull() => this.IsNull(this.tableHardwareOverview.MapIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMapIDNull() => this[this.tableHardwareOverview.MapIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFirmwareVersionNull()
      {
        return this.IsNull(this.tableHardwareOverview.FirmwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFirmwareVersionNull()
      {
        this[this.tableHardwareOverview.FirmwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareVersionNull()
      {
        return this.IsNull(this.tableHardwareOverview.HardwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareVersionNull()
      {
        this[this.tableHardwareOverview.HardwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareResourceNull()
      {
        return this.IsNull(this.tableHardwareOverview.HardwareResourceColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareResourceNull()
      {
        this[this.tableHardwareOverview.HardwareResourceColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableHardwareOverview.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableHardwareOverview.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsTestinfoNull() => this.IsNull(this.tableHardwareOverview.TestinfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetTestinfoNull()
      {
        this[this.tableHardwareOverview.TestinfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareOptionsNull()
      {
        return this.IsNull(this.tableHardwareOverview.HardwareOptionsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareOptionsNull()
      {
        this[this.tableHardwareOverview.HardwareOptionsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsCompatibleFirmwaresNull()
      {
        return this.IsNull(this.tableHardwareOverview.CompatibleFirmwaresColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetCompatibleFirmwaresNull()
      {
        this[this.tableHardwareOverview.CompatibleFirmwaresColumn] = Convert.DBNull;
      }
    }

    public class HardwareAndFirmwareInfoRow : DataRow
    {
      private HardwareTypeTables.HardwareAndFirmwareInfoDataTable tableHardwareAndFirmwareInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareAndFirmwareInfoRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHardwareAndFirmwareInfo = (HardwareTypeTables.HardwareAndFirmwareInfoDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareVersion
      {
        get => (int) this[this.tableHardwareAndFirmwareInfo.HardwareVersionColumn];
        set => this[this.tableHardwareAndFirmwareInfo.HardwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int FirmwareVersion
      {
        get => (int) this[this.tableHardwareAndFirmwareInfo.FirmwareVersionColumn];
        set => this[this.tableHardwareAndFirmwareInfo.FirmwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareTypeID
      {
        get => (int) this[this.tableHardwareAndFirmwareInfo.HardwareTypeIDColumn];
        set => this[this.tableHardwareAndFirmwareInfo.HardwareTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareName
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.HardwareNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareName' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.HardwareNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareResource
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.HardwareResourceColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareResource' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.HardwareResourceColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Testinfo
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.TestinfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Testinfo' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.TestinfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HardwareOptions
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.HardwareOptionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareOptions' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.HardwareOptionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MapID
      {
        get => (int) this[this.tableHardwareAndFirmwareInfo.MapIDColumn];
        set => this[this.tableHardwareAndFirmwareInfo.MapIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Options
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.OptionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Options' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.OptionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string SourceInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.SourceInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SourceInfo' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.SourceInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ReleasedName
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.ReleasedNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ReleasedName' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.ReleasedNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string CompatibleOverwriteGroups
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.CompatibleOverwriteGroupsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CompatibleOverwriteGroups' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set
        {
          this[this.tableHardwareAndFirmwareInfo.CompatibleOverwriteGroupsColumn] = (object) value;
        }
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ReleaseComments
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.ReleaseCommentsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ReleaseComments' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.ReleaseCommentsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string FirmwareDependencies
      {
        get
        {
          try
          {
            return (string) this[this.tableHardwareAndFirmwareInfo.FirmwareDependenciesColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FirmwareDependencies' in table 'HardwareAndFirmwareInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHardwareAndFirmwareInfo.FirmwareDependenciesColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareNameNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.HardwareNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareNameNull()
      {
        this[this.tableHardwareAndFirmwareInfo.HardwareNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareResourceNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.HardwareResourceColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareResourceNull()
      {
        this[this.tableHardwareAndFirmwareInfo.HardwareResourceColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.DescriptionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableHardwareAndFirmwareInfo.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsTestinfoNull() => this.IsNull(this.tableHardwareAndFirmwareInfo.TestinfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetTestinfoNull()
      {
        this[this.tableHardwareAndFirmwareInfo.TestinfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareOptionsNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.HardwareOptionsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareOptionsNull()
      {
        this[this.tableHardwareAndFirmwareInfo.HardwareOptionsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsOptionsNull() => this.IsNull(this.tableHardwareAndFirmwareInfo.OptionsColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetOptionsNull()
      {
        this[this.tableHardwareAndFirmwareInfo.OptionsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSourceInfoNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.SourceInfoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSourceInfoNull()
      {
        this[this.tableHardwareAndFirmwareInfo.SourceInfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsReleasedNameNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.ReleasedNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetReleasedNameNull()
      {
        this[this.tableHardwareAndFirmwareInfo.ReleasedNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsCompatibleOverwriteGroupsNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.CompatibleOverwriteGroupsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetCompatibleOverwriteGroupsNull()
      {
        this[this.tableHardwareAndFirmwareInfo.CompatibleOverwriteGroupsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsReleaseCommentsNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.ReleaseCommentsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetReleaseCommentsNull()
      {
        this[this.tableHardwareAndFirmwareInfo.ReleaseCommentsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFirmwareDependenciesNull()
      {
        return this.IsNull(this.tableHardwareAndFirmwareInfo.FirmwareDependenciesColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFirmwareDependenciesNull()
      {
        this[this.tableHardwareAndFirmwareInfo.FirmwareDependenciesColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class HardwareTypeRowChangeEvent : EventArgs
    {
      private HardwareTypeTables.HardwareTypeRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeRowChangeEvent(
        HardwareTypeTables.HardwareTypeRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareTypeRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class MapBaseRowChangeEvent : EventArgs
    {
      private HardwareTypeTables.MapBaseRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MapBaseRowChangeEvent(HardwareTypeTables.MapBaseRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.MapBaseRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class ProgFilesRowChangeEvent : EventArgs
    {
      private HardwareTypeTables.ProgFilesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ProgFilesRowChangeEvent(HardwareTypeTables.ProgFilesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.ProgFilesRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class HardwareOverviewRowChangeEvent : EventArgs
    {
      private HardwareTypeTables.HardwareOverviewRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareOverviewRowChangeEvent(
        HardwareTypeTables.HardwareOverviewRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareOverviewRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class HardwareAndFirmwareInfoRowChangeEvent : EventArgs
    {
      private HardwareTypeTables.HardwareAndFirmwareInfoRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareAndFirmwareInfoRowChangeEvent(
        HardwareTypeTables.HardwareAndFirmwareInfoRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeTables.HardwareAndFirmwareInfoRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
