// Decompiled with JetBrains decompiler
// Type: GMM_Handler.DataSetLogData
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
  [XmlRoot("DataSetLogData")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class DataSetLogData : DataSet
  {
    private DataSetLogData.GMM_Serie2DeviceLogDataDataTable tableGMM_Serie2DeviceLogData;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public DataSetLogData()
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
    protected DataSetLogData(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (GMM_Serie2DeviceLogData)] != null)
            base.Tables.Add((DataTable) new DataSetLogData.GMM_Serie2DeviceLogDataDataTable(dataSet.Tables[nameof (GMM_Serie2DeviceLogData)]));
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
    public DataSetLogData.GMM_Serie2DeviceLogDataDataTable GMM_Serie2DeviceLogData
    {
      get => this.tableGMM_Serie2DeviceLogData;
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
      DataSetLogData dataSetLogData = (DataSetLogData) base.Clone();
      dataSetLogData.InitVars();
      dataSetLogData.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) dataSetLogData;
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
        if (dataSet.Tables["GMM_Serie2DeviceLogData"] != null)
          base.Tables.Add((DataTable) new DataSetLogData.GMM_Serie2DeviceLogDataDataTable(dataSet.Tables["GMM_Serie2DeviceLogData"]));
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
      this.tableGMM_Serie2DeviceLogData = (DataSetLogData.GMM_Serie2DeviceLogDataDataTable) base.Tables["GMM_Serie2DeviceLogData"];
      if (!initTable || this.tableGMM_Serie2DeviceLogData == null)
        return;
      this.tableGMM_Serie2DeviceLogData.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (DataSetLogData);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/DataSetLogData.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableGMM_Serie2DeviceLogData = new DataSetLogData.GMM_Serie2DeviceLogDataDataTable();
      base.Tables.Add((DataTable) this.tableGMM_Serie2DeviceLogData);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeGMM_Serie2DeviceLogData() => false;

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
      DataSetLogData dataSetLogData = new DataSetLogData();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = dataSetLogData.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = dataSetLogData.GetSchemaSerializable();
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
    public delegate void GMM_Serie2DeviceLogDataRowChangeEventHandler(
      object sender,
      DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class GMM_Serie2DeviceLogDataDataTable : 
      TypedTableBase<DataSetLogData.GMM_Serie2DeviceLogDataRow>
    {
      private DataColumn columnChangeDateTime;
      private DataColumn columnDeviceNumber;
      private DataColumn columnLiegenschaft;
      private DataColumn columnNutzer;
      private DataColumn columnStichtag;
      private DataColumn columnAbweichenderStichtag;
      private DataColumn columnkWh_Stichtag;
      private DataColumn columnkWh_Aktuell;
      private DataColumn columnkWh_010710;
      private DataColumn columnkWh_010610;
      private DataColumn columnkWh_010510;
      private DataColumn columnkWh_010410;
      private DataColumn columnkWh_010310;
      private DataColumn columnkWh_010210;
      private DataColumn columnkWh_010110;
      private DataColumn columnkWh_011209;
      private DataColumn columnkWh_011109;
      private DataColumn columnkWh_011009;
      private DataColumn columnkWh_010909;
      private DataColumn columnkWh_010809;
      private DataColumn columnkWh_010709;
      private DataColumn columnkWh_010609;
      private DataColumn columnkWh_010509;
      private DataColumn columnkWh_010409;
      private DataColumn columnkWh_010309;
      private DataColumn columnkWh_010209;
      private DataColumn columnkWh_010109;
      private DataColumn columnMessbereich;
      private DataColumn columnMasseinheit;
      private DataColumn columnEinbaudatum;
      private DataColumn columnMontageauftrag;
      private DataColumn columnEinbauort;
      private DataColumn columnAusleser;
      private DataColumn columnKommentar;
      private DataColumn columnFehlerGefunden;
      private DataColumn columnFehlerBeseitigt;
      private DataColumn columnMax_QmPerHour1_Month;
      private DataColumn columnMax_QmPerHour1;
      private DataColumn columnMax_QmPerHour2_Month;
      private DataColumn columnMax_QmPerHour2;
      private DataColumn columnMax_kW1_Month;
      private DataColumn columnMax_kW1;
      private DataColumn columnMax_kW2_Month;
      private DataColumn columnMax_kW2;
      private DataColumn columnManuellGespeichert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public GMM_Serie2DeviceLogDataDataTable()
      {
        this.TableName = "GMM_Serie2DeviceLogData";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal GMM_Serie2DeviceLogDataDataTable(DataTable table)
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
      protected GMM_Serie2DeviceLogDataDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeDateTimeColumn => this.columnChangeDateTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DeviceNumberColumn => this.columnDeviceNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LiegenschaftColumn => this.columnLiegenschaft;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn NutzerColumn => this.columnNutzer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn StichtagColumn => this.columnStichtag;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AbweichenderStichtagColumn => this.columnAbweichenderStichtag;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_StichtagColumn => this.columnkWh_Stichtag;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_AktuellColumn => this.columnkWh_Aktuell;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010710Column => this.columnkWh_010710;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010610Column => this.columnkWh_010610;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010510Column => this.columnkWh_010510;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010410Column => this.columnkWh_010410;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010310Column => this.columnkWh_010310;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010210Column => this.columnkWh_010210;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010110Column => this.columnkWh_010110;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_011209Column => this.columnkWh_011209;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_011109Column => this.columnkWh_011109;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_011009Column => this.columnkWh_011009;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010909Column => this.columnkWh_010909;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010809Column => this.columnkWh_010809;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010709Column => this.columnkWh_010709;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010609Column => this.columnkWh_010609;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010509Column => this.columnkWh_010509;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010409Column => this.columnkWh_010409;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010309Column => this.columnkWh_010309;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010209Column => this.columnkWh_010209;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn kWh_010109Column => this.columnkWh_010109;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MessbereichColumn => this.columnMessbereich;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MasseinheitColumn => this.columnMasseinheit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EinbaudatumColumn => this.columnEinbaudatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MontageauftragColumn => this.columnMontageauftrag;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EinbauortColumn => this.columnEinbauort;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AusleserColumn => this.columnAusleser;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn KommentarColumn => this.columnKommentar;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FehlerGefundenColumn => this.columnFehlerGefunden;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FehlerBeseitigtColumn => this.columnFehlerBeseitigt;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_QmPerHour1_MonthColumn => this.columnMax_QmPerHour1_Month;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_QmPerHour1Column => this.columnMax_QmPerHour1;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_QmPerHour2_MonthColumn => this.columnMax_QmPerHour2_Month;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_QmPerHour2Column => this.columnMax_QmPerHour2;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_kW1_MonthColumn => this.columnMax_kW1_Month;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_kW1Column => this.columnMax_kW1;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_kW2_MonthColumn => this.columnMax_kW2_Month;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Max_kW2Column => this.columnMax_kW2;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ManuellGespeichertColumn => this.columnManuellGespeichert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetLogData.GMM_Serie2DeviceLogDataRow this[int index]
      {
        get => (DataSetLogData.GMM_Serie2DeviceLogDataRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEventHandler GMM_Serie2DeviceLogDataRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEventHandler GMM_Serie2DeviceLogDataRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEventHandler GMM_Serie2DeviceLogDataRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEventHandler GMM_Serie2DeviceLogDataRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddGMM_Serie2DeviceLogDataRow(DataSetLogData.GMM_Serie2DeviceLogDataRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetLogData.GMM_Serie2DeviceLogDataRow AddGMM_Serie2DeviceLogDataRow(
        DateTime ChangeDateTime,
        int DeviceNumber,
        string Liegenschaft,
        int Nutzer,
        DateTime Stichtag,
        DateTime AbweichenderStichtag,
        int kWh_Stichtag,
        int kWh_Aktuell,
        int kWh_010710,
        int kWh_010610,
        int kWh_010510,
        int kWh_010410,
        int kWh_010310,
        int kWh_010210,
        int kWh_010110,
        int kWh_011209,
        int kWh_011109,
        int kWh_011009,
        int kWh_010909,
        int kWh_010809,
        int kWh_010709,
        int kWh_010609,
        int kWh_010509,
        int kWh_010409,
        int kWh_010309,
        int kWh_010209,
        int kWh_010109,
        string Messbereich,
        string Masseinheit,
        string Einbaudatum,
        string Montageauftrag,
        string Einbauort,
        string Ausleser,
        string Kommentar,
        bool FehlerGefunden,
        bool FehlerBeseitigt,
        DateTime Max_QmPerHour1_Month,
        double Max_QmPerHour1,
        DateTime Max_QmPerHour2_Month,
        double Max_QmPerHour2,
        DateTime Max_kW1_Month,
        double Max_kW1,
        DateTime Max_kW2_Month,
        double Max_kW2,
        bool ManuellGespeichert)
      {
        DataSetLogData.GMM_Serie2DeviceLogDataRow row = (DataSetLogData.GMM_Serie2DeviceLogDataRow) this.NewRow();
        object[] objArray = new object[45]
        {
          (object) ChangeDateTime,
          (object) DeviceNumber,
          (object) Liegenschaft,
          (object) Nutzer,
          (object) Stichtag,
          (object) AbweichenderStichtag,
          (object) kWh_Stichtag,
          (object) kWh_Aktuell,
          (object) kWh_010710,
          (object) kWh_010610,
          (object) kWh_010510,
          (object) kWh_010410,
          (object) kWh_010310,
          (object) kWh_010210,
          (object) kWh_010110,
          (object) kWh_011209,
          (object) kWh_011109,
          (object) kWh_011009,
          (object) kWh_010909,
          (object) kWh_010809,
          (object) kWh_010709,
          (object) kWh_010609,
          (object) kWh_010509,
          (object) kWh_010409,
          (object) kWh_010309,
          (object) kWh_010209,
          (object) kWh_010109,
          (object) Messbereich,
          (object) Masseinheit,
          (object) Einbaudatum,
          (object) Montageauftrag,
          (object) Einbauort,
          (object) Ausleser,
          (object) Kommentar,
          (object) FehlerGefunden,
          (object) FehlerBeseitigt,
          (object) Max_QmPerHour1_Month,
          (object) Max_QmPerHour1,
          (object) Max_QmPerHour2_Month,
          (object) Max_QmPerHour2,
          (object) Max_kW1_Month,
          (object) Max_kW1,
          (object) Max_kW2_Month,
          (object) Max_kW2,
          (object) ManuellGespeichert
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        DataSetLogData.GMM_Serie2DeviceLogDataDataTable logDataDataTable = (DataSetLogData.GMM_Serie2DeviceLogDataDataTable) base.Clone();
        logDataDataTable.InitVars();
        return (DataTable) logDataDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DataSetLogData.GMM_Serie2DeviceLogDataDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnChangeDateTime = this.Columns["ChangeDateTime"];
        this.columnDeviceNumber = this.Columns["DeviceNumber"];
        this.columnLiegenschaft = this.Columns["Liegenschaft"];
        this.columnNutzer = this.Columns["Nutzer"];
        this.columnStichtag = this.Columns["Stichtag"];
        this.columnAbweichenderStichtag = this.Columns["AbweichenderStichtag"];
        this.columnkWh_Stichtag = this.Columns["kWh_Stichtag"];
        this.columnkWh_Aktuell = this.Columns["kWh_Aktuell"];
        this.columnkWh_010710 = this.Columns["kWh_010710"];
        this.columnkWh_010610 = this.Columns["kWh_010610"];
        this.columnkWh_010510 = this.Columns["kWh_010510"];
        this.columnkWh_010410 = this.Columns["kWh_010410"];
        this.columnkWh_010310 = this.Columns["kWh_010310"];
        this.columnkWh_010210 = this.Columns["kWh_010210"];
        this.columnkWh_010110 = this.Columns["kWh_010110"];
        this.columnkWh_011209 = this.Columns["kWh_011209"];
        this.columnkWh_011109 = this.Columns["kWh_011109"];
        this.columnkWh_011009 = this.Columns["kWh_011009"];
        this.columnkWh_010909 = this.Columns["kWh_010909"];
        this.columnkWh_010809 = this.Columns["kWh_010809"];
        this.columnkWh_010709 = this.Columns["kWh_010709"];
        this.columnkWh_010609 = this.Columns["kWh_010609"];
        this.columnkWh_010509 = this.Columns["kWh_010509"];
        this.columnkWh_010409 = this.Columns["kWh_010409"];
        this.columnkWh_010309 = this.Columns["kWh_010309"];
        this.columnkWh_010209 = this.Columns["kWh_010209"];
        this.columnkWh_010109 = this.Columns["kWh_010109"];
        this.columnMessbereich = this.Columns["Messbereich"];
        this.columnMasseinheit = this.Columns["Masseinheit"];
        this.columnEinbaudatum = this.Columns["Einbaudatum"];
        this.columnMontageauftrag = this.Columns["Montageauftrag"];
        this.columnEinbauort = this.Columns["Einbauort"];
        this.columnAusleser = this.Columns["Ausleser"];
        this.columnKommentar = this.Columns["Kommentar"];
        this.columnFehlerGefunden = this.Columns["FehlerGefunden"];
        this.columnFehlerBeseitigt = this.Columns["FehlerBeseitigt"];
        this.columnMax_QmPerHour1_Month = this.Columns["Max_QmPerHour1_Month"];
        this.columnMax_QmPerHour1 = this.Columns["Max_QmPerHour1"];
        this.columnMax_QmPerHour2_Month = this.Columns["Max_QmPerHour2_Month"];
        this.columnMax_QmPerHour2 = this.Columns["Max_QmPerHour2"];
        this.columnMax_kW1_Month = this.Columns["Max_kW1_Month"];
        this.columnMax_kW1 = this.Columns["Max_kW1"];
        this.columnMax_kW2_Month = this.Columns["Max_kW2_Month"];
        this.columnMax_kW2 = this.Columns["Max_kW2"];
        this.columnManuellGespeichert = this.Columns["ManuellGespeichert"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnChangeDateTime = new DataColumn("ChangeDateTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeDateTime);
        this.columnDeviceNumber = new DataColumn("DeviceNumber", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDeviceNumber);
        this.columnLiegenschaft = new DataColumn("Liegenschaft", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLiegenschaft);
        this.columnNutzer = new DataColumn("Nutzer", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnNutzer);
        this.columnStichtag = new DataColumn("Stichtag", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnStichtag);
        this.columnAbweichenderStichtag = new DataColumn("AbweichenderStichtag", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAbweichenderStichtag);
        this.columnkWh_Stichtag = new DataColumn("kWh_Stichtag", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_Stichtag);
        this.columnkWh_Aktuell = new DataColumn("kWh_Aktuell", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_Aktuell);
        this.columnkWh_010710 = new DataColumn("kWh_010710", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010710);
        this.columnkWh_010610 = new DataColumn("kWh_010610", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010610);
        this.columnkWh_010510 = new DataColumn("kWh_010510", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010510);
        this.columnkWh_010410 = new DataColumn("kWh_010410", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010410);
        this.columnkWh_010310 = new DataColumn("kWh_010310", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010310);
        this.columnkWh_010210 = new DataColumn("kWh_010210", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010210);
        this.columnkWh_010110 = new DataColumn("kWh_010110", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010110);
        this.columnkWh_011209 = new DataColumn("kWh_011209", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_011209);
        this.columnkWh_011109 = new DataColumn("kWh_011109", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_011109);
        this.columnkWh_011009 = new DataColumn("kWh_011009", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_011009);
        this.columnkWh_010909 = new DataColumn("kWh_010909", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010909);
        this.columnkWh_010809 = new DataColumn("kWh_010809", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010809);
        this.columnkWh_010709 = new DataColumn("kWh_010709", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010709);
        this.columnkWh_010609 = new DataColumn("kWh_010609", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010609);
        this.columnkWh_010509 = new DataColumn("kWh_010509", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010509);
        this.columnkWh_010409 = new DataColumn("kWh_010409", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010409);
        this.columnkWh_010309 = new DataColumn("kWh_010309", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010309);
        this.columnkWh_010209 = new DataColumn("kWh_010209", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010209);
        this.columnkWh_010109 = new DataColumn("kWh_010109", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnkWh_010109);
        this.columnMessbereich = new DataColumn("Messbereich", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMessbereich);
        this.columnMasseinheit = new DataColumn("Masseinheit", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMasseinheit);
        this.columnEinbaudatum = new DataColumn("Einbaudatum", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEinbaudatum);
        this.columnMontageauftrag = new DataColumn("Montageauftrag", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMontageauftrag);
        this.columnEinbauort = new DataColumn("Einbauort", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEinbauort);
        this.columnAusleser = new DataColumn("Ausleser", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAusleser);
        this.columnKommentar = new DataColumn("Kommentar", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnKommentar);
        this.columnFehlerGefunden = new DataColumn("FehlerGefunden", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFehlerGefunden);
        this.columnFehlerBeseitigt = new DataColumn("FehlerBeseitigt", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFehlerBeseitigt);
        this.columnMax_QmPerHour1_Month = new DataColumn("Max_QmPerHour1_Month", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_QmPerHour1_Month);
        this.columnMax_QmPerHour1 = new DataColumn("Max_QmPerHour1", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_QmPerHour1);
        this.columnMax_QmPerHour2_Month = new DataColumn("Max_QmPerHour2_Month", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_QmPerHour2_Month);
        this.columnMax_QmPerHour2 = new DataColumn("Max_QmPerHour2", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_QmPerHour2);
        this.columnMax_kW1_Month = new DataColumn("Max_kW1_Month", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_kW1_Month);
        this.columnMax_kW1 = new DataColumn("Max_kW1", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_kW1);
        this.columnMax_kW2_Month = new DataColumn("Max_kW2_Month", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_kW2_Month);
        this.columnMax_kW2 = new DataColumn("Max_kW2", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMax_kW2);
        this.columnManuellGespeichert = new DataColumn("ManuellGespeichert", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnManuellGespeichert);
        this.columnChangeDateTime.AllowDBNull = false;
        this.columnDeviceNumber.AllowDBNull = false;
        this.columnLiegenschaft.AllowDBNull = false;
        this.columnNutzer.AllowDBNull = false;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetLogData.GMM_Serie2DeviceLogDataRow NewGMM_Serie2DeviceLogDataRow()
      {
        return (DataSetLogData.GMM_Serie2DeviceLogDataRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DataSetLogData.GMM_Serie2DeviceLogDataRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DataSetLogData.GMM_Serie2DeviceLogDataRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.GMM_Serie2DeviceLogDataRowChanged == null)
          return;
        this.GMM_Serie2DeviceLogDataRowChanged((object) this, new DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEvent((DataSetLogData.GMM_Serie2DeviceLogDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.GMM_Serie2DeviceLogDataRowChanging == null)
          return;
        this.GMM_Serie2DeviceLogDataRowChanging((object) this, new DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEvent((DataSetLogData.GMM_Serie2DeviceLogDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.GMM_Serie2DeviceLogDataRowDeleted == null)
          return;
        this.GMM_Serie2DeviceLogDataRowDeleted((object) this, new DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEvent((DataSetLogData.GMM_Serie2DeviceLogDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.GMM_Serie2DeviceLogDataRowDeleting == null)
          return;
        this.GMM_Serie2DeviceLogDataRowDeleting((object) this, new DataSetLogData.GMM_Serie2DeviceLogDataRowChangeEvent((DataSetLogData.GMM_Serie2DeviceLogDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveGMM_Serie2DeviceLogDataRow(DataSetLogData.GMM_Serie2DeviceLogDataRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DataSetLogData dataSetLogData = new DataSetLogData();
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
          FixedValue = dataSetLogData.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (GMM_Serie2DeviceLogDataDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = dataSetLogData.GetSchemaSerializable();
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

    public class GMM_Serie2DeviceLogDataRow : DataRow
    {
      private DataSetLogData.GMM_Serie2DeviceLogDataDataTable tableGMM_Serie2DeviceLogData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal GMM_Serie2DeviceLogDataRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableGMM_Serie2DeviceLogData = (DataSetLogData.GMM_Serie2DeviceLogDataDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ChangeDateTime
      {
        get => (DateTime) this[this.tableGMM_Serie2DeviceLogData.ChangeDateTimeColumn];
        set => this[this.tableGMM_Serie2DeviceLogData.ChangeDateTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int DeviceNumber
      {
        get => (int) this[this.tableGMM_Serie2DeviceLogData.DeviceNumberColumn];
        set => this[this.tableGMM_Serie2DeviceLogData.DeviceNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Liegenschaft
      {
        get => (string) this[this.tableGMM_Serie2DeviceLogData.LiegenschaftColumn];
        set => this[this.tableGMM_Serie2DeviceLogData.LiegenschaftColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Nutzer
      {
        get => (int) this[this.tableGMM_Serie2DeviceLogData.NutzerColumn];
        set => this[this.tableGMM_Serie2DeviceLogData.NutzerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Stichtag
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableGMM_Serie2DeviceLogData.StichtagColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Stichtag' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.StichtagColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime AbweichenderStichtag
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableGMM_Serie2DeviceLogData.AbweichenderStichtagColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AbweichenderStichtag' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.AbweichenderStichtagColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_Stichtag
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_StichtagColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_Stichtag' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_StichtagColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_Aktuell
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_AktuellColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_Aktuell' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_AktuellColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010710
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010710Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010710' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010710Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010610
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010610Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010610' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010610Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010510
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010510Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010510' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010510Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010410
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010410Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010410' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010410Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010310
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010310Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010310' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010310Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010210
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010210Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010210' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010210Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010110
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010110Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010110' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010110Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_011209
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_011209Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_011209' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_011209Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_011109
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_011109Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_011109' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_011109Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_011009
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_011009Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_011009' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_011009Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010909
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010909Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010909' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010909Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010809
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010809Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010809' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010809Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010709
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010709Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010709' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010709Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010609
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010609Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010609' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010609Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010509
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010509Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010509' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010509Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010409
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010409Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010409' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010409Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010309
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010309Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010309' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010309Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010209
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010209Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010209' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010209Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010109
      {
        get
        {
          try
          {
            return (int) this[this.tableGMM_Serie2DeviceLogData.kWh_010109Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010109' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.kWh_010109Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Messbereich
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.MessbereichColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Messbereich' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.MessbereichColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Masseinheit
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.MasseinheitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Masseinheit' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.MasseinheitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbaudatum
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.EinbaudatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbaudatum' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.EinbaudatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Montageauftrag
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.MontageauftragColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Montageauftrag' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.MontageauftragColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbauort
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.EinbauortColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbauort' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.EinbauortColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Ausleser
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.AusleserColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Ausleser' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.AusleserColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Kommentar
      {
        get
        {
          try
          {
            return (string) this[this.tableGMM_Serie2DeviceLogData.KommentarColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kommentar' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.KommentarColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool FehlerGefunden
      {
        get
        {
          try
          {
            return (bool) this[this.tableGMM_Serie2DeviceLogData.FehlerGefundenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FehlerGefunden' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.FehlerGefundenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool FehlerBeseitigt
      {
        get
        {
          try
          {
            return (bool) this[this.tableGMM_Serie2DeviceLogData.FehlerBeseitigtColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FehlerBeseitigt' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.FehlerBeseitigtColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_QmPerHour1_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour1_Month' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_QmPerHour1
      {
        get
        {
          try
          {
            return (double) this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour1' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_QmPerHour2_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour2_Month' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_QmPerHour2
      {
        get
        {
          try
          {
            return (double) this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour2' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_kW1_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableGMM_Serie2DeviceLogData.Max_kW1_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW1_Month' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_kW1_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_kW1
      {
        get
        {
          try
          {
            return (double) this[this.tableGMM_Serie2DeviceLogData.Max_kW1Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW1' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_kW1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_kW2_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableGMM_Serie2DeviceLogData.Max_kW2_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW2_Month' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_kW2_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_kW2
      {
        get
        {
          try
          {
            return (double) this[this.tableGMM_Serie2DeviceLogData.Max_kW2Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW2' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.Max_kW2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool ManuellGespeichert
      {
        get
        {
          try
          {
            return (bool) this[this.tableGMM_Serie2DeviceLogData.ManuellGespeichertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ManuellGespeichert' in table 'GMM_Serie2DeviceLogData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableGMM_Serie2DeviceLogData.ManuellGespeichertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsStichtagNull() => this.IsNull(this.tableGMM_Serie2DeviceLogData.StichtagColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetStichtagNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.StichtagColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAbweichenderStichtagNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.AbweichenderStichtagColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAbweichenderStichtagNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.AbweichenderStichtagColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_StichtagNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_StichtagColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_StichtagNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_StichtagColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_AktuellNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_AktuellColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_AktuellNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_AktuellColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010710Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010710Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010710Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010710Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010610Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010610Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010610Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010610Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010510Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010510Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010510Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010510Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010410Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010410Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010410Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010410Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010310Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010310Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010310Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010310Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010210Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010210Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010210Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010210Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010110Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010110Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010110Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010110Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_011209Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_011209Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_011209Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_011209Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_011109Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_011109Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_011109Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_011109Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_011009Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_011009Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_011009Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_011009Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010909Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010909Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010909Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010909Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010809Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010809Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010809Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010809Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010709Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010709Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010709Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010709Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010609Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010609Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010609Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010609Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010509Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010509Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010509Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010509Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010409Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010409Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010409Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010409Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010309Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010309Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010309Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010309Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010209Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010209Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010209Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010209Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010109Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.kWh_010109Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010109Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.kWh_010109Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMessbereichNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.MessbereichColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMessbereichNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.MessbereichColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMasseinheitNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.MasseinheitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMasseinheitNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.MasseinheitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbaudatumNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.EinbaudatumColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbaudatumNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.EinbaudatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMontageauftragNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.MontageauftragColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMontageauftragNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.MontageauftragColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbauortNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.EinbauortColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbauortNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.EinbauortColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAusleserNull() => this.IsNull(this.tableGMM_Serie2DeviceLogData.AusleserColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAusleserNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.AusleserColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKommentarNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.KommentarColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKommentarNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.KommentarColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFehlerGefundenNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.FehlerGefundenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFehlerGefundenNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.FehlerGefundenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFehlerBeseitigtNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.FehlerBeseitigtColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFehlerBeseitigtNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.FehlerBeseitigtColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour1_MonthNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1_MonthColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour1_MonthNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour1Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour1Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour1Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour2_MonthNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2_MonthColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour2_MonthNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour2Null()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour2Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_QmPerHour2Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW1_MonthNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_kW1_MonthColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW1_MonthNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_kW1_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW1Null() => this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_kW1Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW1Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_kW1Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW2_MonthNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_kW2_MonthColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW2_MonthNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_kW2_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW2Null() => this.IsNull(this.tableGMM_Serie2DeviceLogData.Max_kW2Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW2Null()
      {
        this[this.tableGMM_Serie2DeviceLogData.Max_kW2Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsManuellGespeichertNull()
      {
        return this.IsNull(this.tableGMM_Serie2DeviceLogData.ManuellGespeichertColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetManuellGespeichertNull()
      {
        this[this.tableGMM_Serie2DeviceLogData.ManuellGespeichertColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class GMM_Serie2DeviceLogDataRowChangeEvent : EventArgs
    {
      private DataSetLogData.GMM_Serie2DeviceLogDataRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public GMM_Serie2DeviceLogDataRowChangeEvent(
        DataSetLogData.GMM_Serie2DeviceLogDataRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetLogData.GMM_Serie2DeviceLogDataRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
