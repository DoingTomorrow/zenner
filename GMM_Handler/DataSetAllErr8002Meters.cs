// Decompiled with JetBrains decompiler
// Type: GMM_Handler.DataSetAllErr8002Meters
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
  [XmlRoot("DataSetAllErr8002Meters")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class DataSetAllErr8002Meters : DataSet
  {
    private DataSetAllErr8002Meters.Err8002MeterDataTable tableErr8002Meter;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public DataSetAllErr8002Meters()
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
    protected DataSetAllErr8002Meters(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (Err8002Meter)] != null)
            base.Tables.Add((DataTable) new DataSetAllErr8002Meters.Err8002MeterDataTable(dataSet.Tables[nameof (Err8002Meter)]));
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
    public DataSetAllErr8002Meters.Err8002MeterDataTable Err8002Meter => this.tableErr8002Meter;

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
      DataSetAllErr8002Meters allErr8002Meters = (DataSetAllErr8002Meters) base.Clone();
      allErr8002Meters.InitVars();
      allErr8002Meters.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) allErr8002Meters;
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
        if (dataSet.Tables["Err8002Meter"] != null)
          base.Tables.Add((DataTable) new DataSetAllErr8002Meters.Err8002MeterDataTable(dataSet.Tables["Err8002Meter"]));
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
      this.tableErr8002Meter = (DataSetAllErr8002Meters.Err8002MeterDataTable) base.Tables["Err8002Meter"];
      if (!initTable || this.tableErr8002Meter == null)
        return;
      this.tableErr8002Meter.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (DataSetAllErr8002Meters);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/DataSetAllErr8002Meters.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableErr8002Meter = new DataSetAllErr8002Meters.Err8002MeterDataTable();
      base.Tables.Add((DataTable) this.tableErr8002Meter);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeErr8002Meter() => false;

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
      DataSetAllErr8002Meters allErr8002Meters = new DataSetAllErr8002Meters();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = allErr8002Meters.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = allErr8002Meters.GetSchemaSerializable();
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
    public class Err8002MeterDataTable : TypedTableBase<DataSetAllErr8002Meters.Err8002MeterRow>
    {
      private DataColumn columnMeterID;
      private DataColumn columnMeterInfoID;
      private DataColumn columnSerialNr;
      private DataColumn columnKurztext;
      private DataColumn columnProductionDate;
      private DataColumn columnApprovalDate;
      private DataColumn columnLastProgDate;
      private DataColumn columnMeterTypeID;
      private DataColumn columnSAP_Number;
      private DataColumn columnDeviceDataAvailable;
      private DataColumn columnChangeDateTime;
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
      public Err8002MeterDataTable()
      {
        this.TableName = "Err8002Meter";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal Err8002MeterDataTable(DataTable table)
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
      protected Err8002MeterDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterInfoIDColumn => this.columnMeterInfoID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerialNrColumn => this.columnSerialNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn KurztextColumn => this.columnKurztext;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ProductionDateColumn => this.columnProductionDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ApprovalDateColumn => this.columnApprovalDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LastProgDateColumn => this.columnLastProgDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MeterTypeIDColumn => this.columnMeterTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SAP_NumberColumn => this.columnSAP_Number;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DeviceDataAvailableColumn => this.columnDeviceDataAvailable;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeDateTimeColumn => this.columnChangeDateTime;

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
      public DataSetAllErr8002Meters.Err8002MeterRow this[int index]
      {
        get => (DataSetAllErr8002Meters.Err8002MeterRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetAllErr8002Meters.Err8002MeterRowChangeEventHandler Err8002MeterRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetAllErr8002Meters.Err8002MeterRowChangeEventHandler Err8002MeterRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetAllErr8002Meters.Err8002MeterRowChangeEventHandler Err8002MeterRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event DataSetAllErr8002Meters.Err8002MeterRowChangeEventHandler Err8002MeterRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddErr8002MeterRow(DataSetAllErr8002Meters.Err8002MeterRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetAllErr8002Meters.Err8002MeterRow AddErr8002MeterRow(
        int MeterID,
        int MeterInfoID,
        string SerialNr,
        string Kurztext,
        DateTime ProductionDate,
        DateTime ApprovalDate,
        DateTime LastProgDate,
        int MeterTypeID,
        string SAP_Number,
        short DeviceDataAvailable,
        DateTime ChangeDateTime,
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
        DataSetAllErr8002Meters.Err8002MeterRow row = (DataSetAllErr8002Meters.Err8002MeterRow) this.NewRow();
        object[] objArray = new object[54]
        {
          (object) MeterID,
          (object) MeterInfoID,
          (object) SerialNr,
          (object) Kurztext,
          (object) ProductionDate,
          (object) ApprovalDate,
          (object) LastProgDate,
          (object) MeterTypeID,
          (object) SAP_Number,
          (object) DeviceDataAvailable,
          (object) ChangeDateTime,
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
        DataSetAllErr8002Meters.Err8002MeterDataTable err8002MeterDataTable = (DataSetAllErr8002Meters.Err8002MeterDataTable) base.Clone();
        err8002MeterDataTable.InitVars();
        return (DataTable) err8002MeterDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new DataSetAllErr8002Meters.Err8002MeterDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnMeterID = this.Columns["MeterID"];
        this.columnMeterInfoID = this.Columns["MeterInfoID"];
        this.columnSerialNr = this.Columns["SerialNr"];
        this.columnKurztext = this.Columns["Kurztext"];
        this.columnProductionDate = this.Columns["ProductionDate"];
        this.columnApprovalDate = this.Columns["ApprovalDate"];
        this.columnLastProgDate = this.Columns["LastProgDate"];
        this.columnMeterTypeID = this.Columns["MeterTypeID"];
        this.columnSAP_Number = this.Columns["SAP_Number"];
        this.columnDeviceDataAvailable = this.Columns["DeviceDataAvailable"];
        this.columnChangeDateTime = this.Columns["ChangeDateTime"];
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
        this.columnMeterID = new DataColumn("MeterID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnMeterInfoID = new DataColumn("MeterInfoID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterInfoID);
        this.columnSerialNr = new DataColumn("SerialNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerialNr);
        this.columnKurztext = new DataColumn("Kurztext", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnKurztext);
        this.columnProductionDate = new DataColumn("ProductionDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnProductionDate);
        this.columnApprovalDate = new DataColumn("ApprovalDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnApprovalDate);
        this.columnLastProgDate = new DataColumn("LastProgDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLastProgDate);
        this.columnMeterTypeID = new DataColumn("MeterTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterTypeID);
        this.columnSAP_Number = new DataColumn("SAP_Number", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSAP_Number);
        this.columnDeviceDataAvailable = new DataColumn("DeviceDataAvailable", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDeviceDataAvailable);
        this.columnChangeDateTime = new DataColumn("ChangeDateTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeDateTime);
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
        this.columnSerialNr.MaxLength = 50;
        this.columnLastProgDate.Caption = "ApprovalDate";
        this.columnSAP_Number.Caption = "PPSArtikelNr";
        this.columnSAP_Number.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetAllErr8002Meters.Err8002MeterRow NewErr8002MeterRow()
      {
        return (DataSetAllErr8002Meters.Err8002MeterRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new DataSetAllErr8002Meters.Err8002MeterRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (DataSetAllErr8002Meters.Err8002MeterRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.Err8002MeterRowChanged == null)
          return;
        this.Err8002MeterRowChanged((object) this, new DataSetAllErr8002Meters.Err8002MeterRowChangeEvent((DataSetAllErr8002Meters.Err8002MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.Err8002MeterRowChanging == null)
          return;
        this.Err8002MeterRowChanging((object) this, new DataSetAllErr8002Meters.Err8002MeterRowChangeEvent((DataSetAllErr8002Meters.Err8002MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.Err8002MeterRowDeleted == null)
          return;
        this.Err8002MeterRowDeleted((object) this, new DataSetAllErr8002Meters.Err8002MeterRowChangeEvent((DataSetAllErr8002Meters.Err8002MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.Err8002MeterRowDeleting == null)
          return;
        this.Err8002MeterRowDeleting((object) this, new DataSetAllErr8002Meters.Err8002MeterRowChangeEvent((DataSetAllErr8002Meters.Err8002MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveErr8002MeterRow(DataSetAllErr8002Meters.Err8002MeterRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        DataSetAllErr8002Meters allErr8002Meters = new DataSetAllErr8002Meters();
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
          FixedValue = allErr8002Meters.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (Err8002MeterDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = allErr8002Meters.GetSchemaSerializable();
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

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void Err8002MeterRowChangeEventHandler(
      object sender,
      DataSetAllErr8002Meters.Err8002MeterRowChangeEvent e);

    public class Err8002MeterRow : DataRow
    {
      private DataSetAllErr8002Meters.Err8002MeterDataTable tableErr8002Meter;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal Err8002MeterRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableErr8002Meter = (DataSetAllErr8002Meters.Err8002MeterDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterID
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.MeterIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterID' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterInfoID
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.MeterInfoIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterInfoID' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.MeterInfoIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerialNr
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.SerialNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerialNr' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.SerialNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Kurztext
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.KurztextColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kurztext' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.KurztextColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ProductionDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.ProductionDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ProductionDate' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.ProductionDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ApprovalDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.ApprovalDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ApprovalDate' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.ApprovalDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime LastProgDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.LastProgDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LastProgDate' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.LastProgDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MeterTypeID
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.MeterTypeIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterTypeID' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.MeterTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SAP_Number
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.SAP_NumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SAP_Number' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.SAP_NumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short DeviceDataAvailable
      {
        get
        {
          try
          {
            return (short) this[this.tableErr8002Meter.DeviceDataAvailableColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DeviceDataAvailable' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.DeviceDataAvailableColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ChangeDateTime
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.ChangeDateTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChangeDateTime' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.ChangeDateTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Liegenschaft
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.LiegenschaftColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Liegenschaft' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.LiegenschaftColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Nutzer
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.NutzerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Nutzer' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.NutzerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Stichtag
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.StichtagColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Stichtag' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.StichtagColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime AbweichenderStichtag
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.AbweichenderStichtagColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AbweichenderStichtag' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.AbweichenderStichtagColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_Stichtag
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_StichtagColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_Stichtag' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_StichtagColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_Aktuell
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_AktuellColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_Aktuell' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_AktuellColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010710
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010710Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010710' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010710Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010610
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010610Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010610' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010610Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010510
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010510Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010510' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010510Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010410
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010410Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010410' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010410Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010310
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010310Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010310' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010310Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010210
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010210Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010210' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010210Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010110
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010110Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010110' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010110Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_011209
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_011209Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_011209' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_011209Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_011109
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_011109Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_011109' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_011109Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_011009
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_011009Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_011009' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_011009Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010909
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010909Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010909' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010909Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010809
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010809Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010809' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010809Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010709
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010709Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010709' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010709Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010609
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010609Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010609' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010609Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010509
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010509Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010509' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010509Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010409
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010409Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010409' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010409Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010309
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010309Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010309' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010309Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010209
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010209Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010209' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010209Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int kWh_010109
      {
        get
        {
          try
          {
            return (int) this[this.tableErr8002Meter.kWh_010109Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'kWh_010109' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.kWh_010109Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Messbereich
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.MessbereichColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Messbereich' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.MessbereichColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Masseinheit
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.MasseinheitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Masseinheit' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.MasseinheitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbaudatum
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.EinbaudatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbaudatum' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.EinbaudatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Montageauftrag
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.MontageauftragColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Montageauftrag' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.MontageauftragColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbauort
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.EinbauortColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbauort' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.EinbauortColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Ausleser
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.AusleserColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Ausleser' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.AusleserColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Kommentar
      {
        get
        {
          try
          {
            return (string) this[this.tableErr8002Meter.KommentarColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kommentar' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.KommentarColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool FehlerGefunden
      {
        get
        {
          try
          {
            return (bool) this[this.tableErr8002Meter.FehlerGefundenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FehlerGefunden' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.FehlerGefundenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool FehlerBeseitigt
      {
        get
        {
          try
          {
            return (bool) this[this.tableErr8002Meter.FehlerBeseitigtColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FehlerBeseitigt' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.FehlerBeseitigtColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_QmPerHour1_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.Max_QmPerHour1_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour1_Month' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_QmPerHour1_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_QmPerHour1
      {
        get
        {
          try
          {
            return (double) this[this.tableErr8002Meter.Max_QmPerHour1Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour1' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_QmPerHour1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_QmPerHour2_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.Max_QmPerHour2_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour2_Month' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_QmPerHour2_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_QmPerHour2
      {
        get
        {
          try
          {
            return (double) this[this.tableErr8002Meter.Max_QmPerHour2Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_QmPerHour2' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_QmPerHour2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_kW1_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.Max_kW1_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW1_Month' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_kW1_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_kW1
      {
        get
        {
          try
          {
            return (double) this[this.tableErr8002Meter.Max_kW1Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW1' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_kW1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Max_kW2_Month
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableErr8002Meter.Max_kW2_MonthColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW2_Month' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_kW2_MonthColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Max_kW2
      {
        get
        {
          try
          {
            return (double) this[this.tableErr8002Meter.Max_kW2Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Max_kW2' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.Max_kW2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool ManuellGespeichert
      {
        get
        {
          try
          {
            return (bool) this[this.tableErr8002Meter.ManuellGespeichertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ManuellGespeichert' in table 'Err8002Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableErr8002Meter.ManuellGespeichertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterIDNull() => this.IsNull(this.tableErr8002Meter.MeterIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterIDNull() => this[this.tableErr8002Meter.MeterIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterInfoIDNull() => this.IsNull(this.tableErr8002Meter.MeterInfoIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterInfoIDNull()
      {
        this[this.tableErr8002Meter.MeterInfoIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerialNrNull() => this.IsNull(this.tableErr8002Meter.SerialNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerialNrNull() => this[this.tableErr8002Meter.SerialNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKurztextNull() => this.IsNull(this.tableErr8002Meter.KurztextColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKurztextNull() => this[this.tableErr8002Meter.KurztextColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsProductionDateNull()
      {
        return this.IsNull(this.tableErr8002Meter.ProductionDateColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetProductionDateNull()
      {
        this[this.tableErr8002Meter.ProductionDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsApprovalDateNull() => this.IsNull(this.tableErr8002Meter.ApprovalDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetApprovalDateNull()
      {
        this[this.tableErr8002Meter.ApprovalDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLastProgDateNull() => this.IsNull(this.tableErr8002Meter.LastProgDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLastProgDateNull()
      {
        this[this.tableErr8002Meter.LastProgDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMeterTypeIDNull() => this.IsNull(this.tableErr8002Meter.MeterTypeIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMeterTypeIDNull()
      {
        this[this.tableErr8002Meter.MeterTypeIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSAP_NumberNull() => this.IsNull(this.tableErr8002Meter.SAP_NumberColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSAP_NumberNull()
      {
        this[this.tableErr8002Meter.SAP_NumberColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDeviceDataAvailableNull()
      {
        return this.IsNull(this.tableErr8002Meter.DeviceDataAvailableColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDeviceDataAvailableNull()
      {
        this[this.tableErr8002Meter.DeviceDataAvailableColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChangeDateTimeNull()
      {
        return this.IsNull(this.tableErr8002Meter.ChangeDateTimeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChangeDateTimeNull()
      {
        this[this.tableErr8002Meter.ChangeDateTimeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLiegenschaftNull() => this.IsNull(this.tableErr8002Meter.LiegenschaftColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLiegenschaftNull()
      {
        this[this.tableErr8002Meter.LiegenschaftColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsNutzerNull() => this.IsNull(this.tableErr8002Meter.NutzerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNutzerNull() => this[this.tableErr8002Meter.NutzerColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsStichtagNull() => this.IsNull(this.tableErr8002Meter.StichtagColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetStichtagNull() => this[this.tableErr8002Meter.StichtagColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAbweichenderStichtagNull()
      {
        return this.IsNull(this.tableErr8002Meter.AbweichenderStichtagColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAbweichenderStichtagNull()
      {
        this[this.tableErr8002Meter.AbweichenderStichtagColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_StichtagNull() => this.IsNull(this.tableErr8002Meter.kWh_StichtagColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_StichtagNull()
      {
        this[this.tableErr8002Meter.kWh_StichtagColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_AktuellNull() => this.IsNull(this.tableErr8002Meter.kWh_AktuellColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_AktuellNull()
      {
        this[this.tableErr8002Meter.kWh_AktuellColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010710Null() => this.IsNull(this.tableErr8002Meter.kWh_010710Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010710Null()
      {
        this[this.tableErr8002Meter.kWh_010710Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010610Null() => this.IsNull(this.tableErr8002Meter.kWh_010610Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010610Null()
      {
        this[this.tableErr8002Meter.kWh_010610Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010510Null() => this.IsNull(this.tableErr8002Meter.kWh_010510Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010510Null()
      {
        this[this.tableErr8002Meter.kWh_010510Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010410Null() => this.IsNull(this.tableErr8002Meter.kWh_010410Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010410Null()
      {
        this[this.tableErr8002Meter.kWh_010410Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010310Null() => this.IsNull(this.tableErr8002Meter.kWh_010310Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010310Null()
      {
        this[this.tableErr8002Meter.kWh_010310Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010210Null() => this.IsNull(this.tableErr8002Meter.kWh_010210Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010210Null()
      {
        this[this.tableErr8002Meter.kWh_010210Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010110Null() => this.IsNull(this.tableErr8002Meter.kWh_010110Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010110Null()
      {
        this[this.tableErr8002Meter.kWh_010110Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_011209Null() => this.IsNull(this.tableErr8002Meter.kWh_011209Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_011209Null()
      {
        this[this.tableErr8002Meter.kWh_011209Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_011109Null() => this.IsNull(this.tableErr8002Meter.kWh_011109Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_011109Null()
      {
        this[this.tableErr8002Meter.kWh_011109Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_011009Null() => this.IsNull(this.tableErr8002Meter.kWh_011009Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_011009Null()
      {
        this[this.tableErr8002Meter.kWh_011009Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010909Null() => this.IsNull(this.tableErr8002Meter.kWh_010909Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010909Null()
      {
        this[this.tableErr8002Meter.kWh_010909Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010809Null() => this.IsNull(this.tableErr8002Meter.kWh_010809Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010809Null()
      {
        this[this.tableErr8002Meter.kWh_010809Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010709Null() => this.IsNull(this.tableErr8002Meter.kWh_010709Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010709Null()
      {
        this[this.tableErr8002Meter.kWh_010709Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010609Null() => this.IsNull(this.tableErr8002Meter.kWh_010609Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010609Null()
      {
        this[this.tableErr8002Meter.kWh_010609Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010509Null() => this.IsNull(this.tableErr8002Meter.kWh_010509Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010509Null()
      {
        this[this.tableErr8002Meter.kWh_010509Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010409Null() => this.IsNull(this.tableErr8002Meter.kWh_010409Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010409Null()
      {
        this[this.tableErr8002Meter.kWh_010409Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010309Null() => this.IsNull(this.tableErr8002Meter.kWh_010309Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010309Null()
      {
        this[this.tableErr8002Meter.kWh_010309Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010209Null() => this.IsNull(this.tableErr8002Meter.kWh_010209Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010209Null()
      {
        this[this.tableErr8002Meter.kWh_010209Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IskWh_010109Null() => this.IsNull(this.tableErr8002Meter.kWh_010109Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetkWh_010109Null()
      {
        this[this.tableErr8002Meter.kWh_010109Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMessbereichNull() => this.IsNull(this.tableErr8002Meter.MessbereichColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMessbereichNull()
      {
        this[this.tableErr8002Meter.MessbereichColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMasseinheitNull() => this.IsNull(this.tableErr8002Meter.MasseinheitColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMasseinheitNull()
      {
        this[this.tableErr8002Meter.MasseinheitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbaudatumNull() => this.IsNull(this.tableErr8002Meter.EinbaudatumColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbaudatumNull()
      {
        this[this.tableErr8002Meter.EinbaudatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMontageauftragNull()
      {
        return this.IsNull(this.tableErr8002Meter.MontageauftragColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMontageauftragNull()
      {
        this[this.tableErr8002Meter.MontageauftragColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbauortNull() => this.IsNull(this.tableErr8002Meter.EinbauortColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbauortNull()
      {
        this[this.tableErr8002Meter.EinbauortColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAusleserNull() => this.IsNull(this.tableErr8002Meter.AusleserColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAusleserNull() => this[this.tableErr8002Meter.AusleserColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKommentarNull() => this.IsNull(this.tableErr8002Meter.KommentarColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKommentarNull()
      {
        this[this.tableErr8002Meter.KommentarColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFehlerGefundenNull()
      {
        return this.IsNull(this.tableErr8002Meter.FehlerGefundenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFehlerGefundenNull()
      {
        this[this.tableErr8002Meter.FehlerGefundenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFehlerBeseitigtNull()
      {
        return this.IsNull(this.tableErr8002Meter.FehlerBeseitigtColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFehlerBeseitigtNull()
      {
        this[this.tableErr8002Meter.FehlerBeseitigtColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour1_MonthNull()
      {
        return this.IsNull(this.tableErr8002Meter.Max_QmPerHour1_MonthColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour1_MonthNull()
      {
        this[this.tableErr8002Meter.Max_QmPerHour1_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour1Null()
      {
        return this.IsNull(this.tableErr8002Meter.Max_QmPerHour1Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour1Null()
      {
        this[this.tableErr8002Meter.Max_QmPerHour1Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour2_MonthNull()
      {
        return this.IsNull(this.tableErr8002Meter.Max_QmPerHour2_MonthColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour2_MonthNull()
      {
        this[this.tableErr8002Meter.Max_QmPerHour2_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_QmPerHour2Null()
      {
        return this.IsNull(this.tableErr8002Meter.Max_QmPerHour2Column);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_QmPerHour2Null()
      {
        this[this.tableErr8002Meter.Max_QmPerHour2Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW1_MonthNull() => this.IsNull(this.tableErr8002Meter.Max_kW1_MonthColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW1_MonthNull()
      {
        this[this.tableErr8002Meter.Max_kW1_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW1Null() => this.IsNull(this.tableErr8002Meter.Max_kW1Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW1Null() => this[this.tableErr8002Meter.Max_kW1Column] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW2_MonthNull() => this.IsNull(this.tableErr8002Meter.Max_kW2_MonthColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW2_MonthNull()
      {
        this[this.tableErr8002Meter.Max_kW2_MonthColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMax_kW2Null() => this.IsNull(this.tableErr8002Meter.Max_kW2Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMax_kW2Null() => this[this.tableErr8002Meter.Max_kW2Column] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsManuellGespeichertNull()
      {
        return this.IsNull(this.tableErr8002Meter.ManuellGespeichertColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetManuellGespeichertNull()
      {
        this[this.tableErr8002Meter.ManuellGespeichertColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class Err8002MeterRowChangeEvent : EventArgs
    {
      private DataSetAllErr8002Meters.Err8002MeterRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public Err8002MeterRowChangeEvent(
        DataSetAllErr8002Meters.Err8002MeterRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataSetAllErr8002Meters.Err8002MeterRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
