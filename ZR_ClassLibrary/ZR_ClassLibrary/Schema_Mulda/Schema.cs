// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Schema_Mulda.Schema
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

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
namespace ZR_ClassLibrary.Schema_Mulda
{
  [DesignerCategory("code")]
  [ToolboxItem(true)]
  [XmlSchemaProvider("GetTypedDataSetSchema")]
  [XmlRoot("Schema")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class Schema : DataSet
  {
    private ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable tableaccess;
    private ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable tableDichte;
    private ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable tableEdrbsql;
    private ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable tableFormulare;
    private ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable tablegroups;
    private ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable tablegrpcoll;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable tableHydraulikTyp;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable tableHydAblaufsteuerung;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable tableHydAblaufsteuerung1;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable tableHydAGew;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable tableHydBel_2008;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable tableHydDN;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable tableHydHerst;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable tableHydPos_2008;
    private ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable tableHydSNrListe;
    private ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable tableMIDKalibrierDaten;
    private ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable tableRechenwerkTyp;
    private ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable tableRechenwerkHerst;
    private ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable tableRecBel_2008;
    private ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable tableRecPos_2008;
    private ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable tableTypNamenReport;
    private ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable tableuseracc;
    private ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable tableusers;
    private ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable tableVersionsUpdate;
    private ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable tableZaehlerTyp;
    private ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable tableZaehlertypherst;
    private ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable tableZaeBel_2008;
    private ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable tableZaePos_2008;
    private ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable tableDateiliste;
    private ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable tableS_Version;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public Schema()
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
    protected Schema(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (access)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable(dataSet.Tables[nameof (access)]));
          if (dataSet.Tables[nameof (Dichte)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable(dataSet.Tables[nameof (Dichte)]));
          if (dataSet.Tables[nameof (Edrbsql)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable(dataSet.Tables[nameof (Edrbsql)]));
          if (dataSet.Tables[nameof (Formulare)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable(dataSet.Tables[nameof (Formulare)]));
          if (dataSet.Tables[nameof (groups)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable(dataSet.Tables[nameof (groups)]));
          if (dataSet.Tables[nameof (grpcoll)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable(dataSet.Tables[nameof (grpcoll)]));
          if (dataSet.Tables[nameof (HydraulikTyp)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable(dataSet.Tables[nameof (HydraulikTyp)]));
          if (dataSet.Tables[nameof (HydAblaufsteuerung)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable(dataSet.Tables[nameof (HydAblaufsteuerung)]));
          if (dataSet.Tables[nameof (HydAblaufsteuerung1)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable(dataSet.Tables[nameof (HydAblaufsteuerung1)]));
          if (dataSet.Tables[nameof (HydAGew)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable(dataSet.Tables[nameof (HydAGew)]));
          if (dataSet.Tables[nameof (HydBel_2008)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable(dataSet.Tables[nameof (HydBel_2008)]));
          if (dataSet.Tables[nameof (HydDN)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable(dataSet.Tables[nameof (HydDN)]));
          if (dataSet.Tables[nameof (HydHerst)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable(dataSet.Tables[nameof (HydHerst)]));
          if (dataSet.Tables[nameof (HydPos_2008)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable(dataSet.Tables[nameof (HydPos_2008)]));
          if (dataSet.Tables[nameof (HydSNrListe)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable(dataSet.Tables[nameof (HydSNrListe)]));
          if (dataSet.Tables[nameof (MIDKalibrierDaten)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable(dataSet.Tables[nameof (MIDKalibrierDaten)]));
          if (dataSet.Tables[nameof (RechenwerkTyp)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable(dataSet.Tables[nameof (RechenwerkTyp)]));
          if (dataSet.Tables[nameof (RechenwerkHerst)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable(dataSet.Tables[nameof (RechenwerkHerst)]));
          if (dataSet.Tables[nameof (RecBel_2008)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable(dataSet.Tables[nameof (RecBel_2008)]));
          if (dataSet.Tables[nameof (RecPos_2008)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable(dataSet.Tables[nameof (RecPos_2008)]));
          if (dataSet.Tables[nameof (TypNamenReport)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable(dataSet.Tables[nameof (TypNamenReport)]));
          if (dataSet.Tables[nameof (useracc)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable(dataSet.Tables[nameof (useracc)]));
          if (dataSet.Tables[nameof (users)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable(dataSet.Tables[nameof (users)]));
          if (dataSet.Tables[nameof (VersionsUpdate)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable(dataSet.Tables[nameof (VersionsUpdate)]));
          if (dataSet.Tables[nameof (ZaehlerTyp)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable(dataSet.Tables[nameof (ZaehlerTyp)]));
          if (dataSet.Tables[nameof (Zaehlertypherst)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable(dataSet.Tables[nameof (Zaehlertypherst)]));
          if (dataSet.Tables[nameof (ZaeBel_2008)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable(dataSet.Tables[nameof (ZaeBel_2008)]));
          if (dataSet.Tables[nameof (ZaePos_2008)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable(dataSet.Tables[nameof (ZaePos_2008)]));
          if (dataSet.Tables[nameof (Dateiliste)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable(dataSet.Tables[nameof (Dateiliste)]));
          if (dataSet.Tables[nameof (S_Version)] != null)
            base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable(dataSet.Tables[nameof (S_Version)]));
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
    public ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable access => this.tableaccess;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable Dichte => this.tableDichte;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable Edrbsql => this.tableEdrbsql;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable Formulare => this.tableFormulare;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable groups => this.tablegroups;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable grpcoll => this.tablegrpcoll;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable HydraulikTyp
    {
      get => this.tableHydraulikTyp;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable HydAblaufsteuerung
    {
      get => this.tableHydAblaufsteuerung;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable HydAblaufsteuerung1
    {
      get => this.tableHydAblaufsteuerung1;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable HydAGew => this.tableHydAGew;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable HydBel_2008
    {
      get => this.tableHydBel_2008;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable HydDN => this.tableHydDN;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable HydHerst => this.tableHydHerst;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable HydPos_2008
    {
      get => this.tableHydPos_2008;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable HydSNrListe
    {
      get => this.tableHydSNrListe;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable MIDKalibrierDaten
    {
      get => this.tableMIDKalibrierDaten;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable RechenwerkTyp
    {
      get => this.tableRechenwerkTyp;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable RechenwerkHerst
    {
      get => this.tableRechenwerkHerst;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable RecBel_2008
    {
      get => this.tableRecBel_2008;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable RecPos_2008
    {
      get => this.tableRecPos_2008;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable TypNamenReport
    {
      get => this.tableTypNamenReport;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable useracc => this.tableuseracc;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable users => this.tableusers;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable VersionsUpdate
    {
      get => this.tableVersionsUpdate;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable ZaehlerTyp
    {
      get => this.tableZaehlerTyp;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable Zaehlertypherst
    {
      get => this.tableZaehlertypherst;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable ZaeBel_2008
    {
      get => this.tableZaeBel_2008;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable ZaePos_2008
    {
      get => this.tableZaePos_2008;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable Dateiliste
    {
      get => this.tableDateiliste;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable S_Version => this.tableS_Version;

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
      ZR_ClassLibrary.Schema_Mulda.Schema schema = (ZR_ClassLibrary.Schema_Mulda.Schema) base.Clone();
      schema.InitVars();
      schema.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) schema;
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
        if (dataSet.Tables["access"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable(dataSet.Tables["access"]));
        if (dataSet.Tables["Dichte"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable(dataSet.Tables["Dichte"]));
        if (dataSet.Tables["Edrbsql"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable(dataSet.Tables["Edrbsql"]));
        if (dataSet.Tables["Formulare"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable(dataSet.Tables["Formulare"]));
        if (dataSet.Tables["groups"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable(dataSet.Tables["groups"]));
        if (dataSet.Tables["grpcoll"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable(dataSet.Tables["grpcoll"]));
        if (dataSet.Tables["HydraulikTyp"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable(dataSet.Tables["HydraulikTyp"]));
        if (dataSet.Tables["HydAblaufsteuerung"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable(dataSet.Tables["HydAblaufsteuerung"]));
        if (dataSet.Tables["HydAblaufsteuerung1"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable(dataSet.Tables["HydAblaufsteuerung1"]));
        if (dataSet.Tables["HydAGew"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable(dataSet.Tables["HydAGew"]));
        if (dataSet.Tables["HydBel_2008"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable(dataSet.Tables["HydBel_2008"]));
        if (dataSet.Tables["HydDN"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable(dataSet.Tables["HydDN"]));
        if (dataSet.Tables["HydHerst"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable(dataSet.Tables["HydHerst"]));
        if (dataSet.Tables["HydPos_2008"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable(dataSet.Tables["HydPos_2008"]));
        if (dataSet.Tables["HydSNrListe"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable(dataSet.Tables["HydSNrListe"]));
        if (dataSet.Tables["MIDKalibrierDaten"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable(dataSet.Tables["MIDKalibrierDaten"]));
        if (dataSet.Tables["RechenwerkTyp"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable(dataSet.Tables["RechenwerkTyp"]));
        if (dataSet.Tables["RechenwerkHerst"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable(dataSet.Tables["RechenwerkHerst"]));
        if (dataSet.Tables["RecBel_2008"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable(dataSet.Tables["RecBel_2008"]));
        if (dataSet.Tables["RecPos_2008"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable(dataSet.Tables["RecPos_2008"]));
        if (dataSet.Tables["TypNamenReport"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable(dataSet.Tables["TypNamenReport"]));
        if (dataSet.Tables["useracc"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable(dataSet.Tables["useracc"]));
        if (dataSet.Tables["users"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable(dataSet.Tables["users"]));
        if (dataSet.Tables["VersionsUpdate"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable(dataSet.Tables["VersionsUpdate"]));
        if (dataSet.Tables["ZaehlerTyp"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable(dataSet.Tables["ZaehlerTyp"]));
        if (dataSet.Tables["Zaehlertypherst"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable(dataSet.Tables["Zaehlertypherst"]));
        if (dataSet.Tables["ZaeBel_2008"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable(dataSet.Tables["ZaeBel_2008"]));
        if (dataSet.Tables["ZaePos_2008"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable(dataSet.Tables["ZaePos_2008"]));
        if (dataSet.Tables["Dateiliste"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable(dataSet.Tables["Dateiliste"]));
        if (dataSet.Tables["S_Version"] != null)
          base.Tables.Add((DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable(dataSet.Tables["S_Version"]));
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
      this.tableaccess = (ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable) base.Tables["access"];
      if (initTable && this.tableaccess != null)
        this.tableaccess.InitVars();
      this.tableDichte = (ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable) base.Tables["Dichte"];
      if (initTable && this.tableDichte != null)
        this.tableDichte.InitVars();
      this.tableEdrbsql = (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable) base.Tables["Edrbsql"];
      if (initTable && this.tableEdrbsql != null)
        this.tableEdrbsql.InitVars();
      this.tableFormulare = (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable) base.Tables["Formulare"];
      if (initTable && this.tableFormulare != null)
        this.tableFormulare.InitVars();
      this.tablegroups = (ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable) base.Tables["groups"];
      if (initTable && this.tablegroups != null)
        this.tablegroups.InitVars();
      this.tablegrpcoll = (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable) base.Tables["grpcoll"];
      if (initTable && this.tablegrpcoll != null)
        this.tablegrpcoll.InitVars();
      this.tableHydraulikTyp = (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable) base.Tables["HydraulikTyp"];
      if (initTable && this.tableHydraulikTyp != null)
        this.tableHydraulikTyp.InitVars();
      this.tableHydAblaufsteuerung = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable) base.Tables["HydAblaufsteuerung"];
      if (initTable && this.tableHydAblaufsteuerung != null)
        this.tableHydAblaufsteuerung.InitVars();
      this.tableHydAblaufsteuerung1 = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable) base.Tables["HydAblaufsteuerung1"];
      if (initTable && this.tableHydAblaufsteuerung1 != null)
        this.tableHydAblaufsteuerung1.InitVars();
      this.tableHydAGew = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable) base.Tables["HydAGew"];
      if (initTable && this.tableHydAGew != null)
        this.tableHydAGew.InitVars();
      this.tableHydBel_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable) base.Tables["HydBel_2008"];
      if (initTable && this.tableHydBel_2008 != null)
        this.tableHydBel_2008.InitVars();
      this.tableHydDN = (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable) base.Tables["HydDN"];
      if (initTable && this.tableHydDN != null)
        this.tableHydDN.InitVars();
      this.tableHydHerst = (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable) base.Tables["HydHerst"];
      if (initTable && this.tableHydHerst != null)
        this.tableHydHerst.InitVars();
      this.tableHydPos_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable) base.Tables["HydPos_2008"];
      if (initTable && this.tableHydPos_2008 != null)
        this.tableHydPos_2008.InitVars();
      this.tableHydSNrListe = (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable) base.Tables["HydSNrListe"];
      if (initTable && this.tableHydSNrListe != null)
        this.tableHydSNrListe.InitVars();
      this.tableMIDKalibrierDaten = (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable) base.Tables["MIDKalibrierDaten"];
      if (initTable && this.tableMIDKalibrierDaten != null)
        this.tableMIDKalibrierDaten.InitVars();
      this.tableRechenwerkTyp = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable) base.Tables["RechenwerkTyp"];
      if (initTable && this.tableRechenwerkTyp != null)
        this.tableRechenwerkTyp.InitVars();
      this.tableRechenwerkHerst = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable) base.Tables["RechenwerkHerst"];
      if (initTable && this.tableRechenwerkHerst != null)
        this.tableRechenwerkHerst.InitVars();
      this.tableRecBel_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable) base.Tables["RecBel_2008"];
      if (initTable && this.tableRecBel_2008 != null)
        this.tableRecBel_2008.InitVars();
      this.tableRecPos_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable) base.Tables["RecPos_2008"];
      if (initTable && this.tableRecPos_2008 != null)
        this.tableRecPos_2008.InitVars();
      this.tableTypNamenReport = (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable) base.Tables["TypNamenReport"];
      if (initTable && this.tableTypNamenReport != null)
        this.tableTypNamenReport.InitVars();
      this.tableuseracc = (ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable) base.Tables["useracc"];
      if (initTable && this.tableuseracc != null)
        this.tableuseracc.InitVars();
      this.tableusers = (ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable) base.Tables["users"];
      if (initTable && this.tableusers != null)
        this.tableusers.InitVars();
      this.tableVersionsUpdate = (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable) base.Tables["VersionsUpdate"];
      if (initTable && this.tableVersionsUpdate != null)
        this.tableVersionsUpdate.InitVars();
      this.tableZaehlerTyp = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable) base.Tables["ZaehlerTyp"];
      if (initTable && this.tableZaehlerTyp != null)
        this.tableZaehlerTyp.InitVars();
      this.tableZaehlertypherst = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable) base.Tables["Zaehlertypherst"];
      if (initTable && this.tableZaehlertypherst != null)
        this.tableZaehlertypherst.InitVars();
      this.tableZaeBel_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable) base.Tables["ZaeBel_2008"];
      if (initTable && this.tableZaeBel_2008 != null)
        this.tableZaeBel_2008.InitVars();
      this.tableZaePos_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable) base.Tables["ZaePos_2008"];
      if (initTable && this.tableZaePos_2008 != null)
        this.tableZaePos_2008.InitVars();
      this.tableDateiliste = (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable) base.Tables["Dateiliste"];
      if (initTable && this.tableDateiliste != null)
        this.tableDateiliste.InitVars();
      this.tableS_Version = (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable) base.Tables["S_Version"];
      if (!initTable || this.tableS_Version == null)
        return;
      this.tableS_Version.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (Schema);
      this.Prefix = "";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableaccess = new ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable();
      base.Tables.Add((DataTable) this.tableaccess);
      this.tableDichte = new ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable();
      base.Tables.Add((DataTable) this.tableDichte);
      this.tableEdrbsql = new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable();
      base.Tables.Add((DataTable) this.tableEdrbsql);
      this.tableFormulare = new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable();
      base.Tables.Add((DataTable) this.tableFormulare);
      this.tablegroups = new ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable();
      base.Tables.Add((DataTable) this.tablegroups);
      this.tablegrpcoll = new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable();
      base.Tables.Add((DataTable) this.tablegrpcoll);
      this.tableHydraulikTyp = new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable();
      base.Tables.Add((DataTable) this.tableHydraulikTyp);
      this.tableHydAblaufsteuerung = new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable();
      base.Tables.Add((DataTable) this.tableHydAblaufsteuerung);
      this.tableHydAblaufsteuerung1 = new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable();
      base.Tables.Add((DataTable) this.tableHydAblaufsteuerung1);
      this.tableHydAGew = new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable();
      base.Tables.Add((DataTable) this.tableHydAGew);
      this.tableHydBel_2008 = new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable();
      base.Tables.Add((DataTable) this.tableHydBel_2008);
      this.tableHydDN = new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable();
      base.Tables.Add((DataTable) this.tableHydDN);
      this.tableHydHerst = new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable();
      base.Tables.Add((DataTable) this.tableHydHerst);
      this.tableHydPos_2008 = new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable();
      base.Tables.Add((DataTable) this.tableHydPos_2008);
      this.tableHydSNrListe = new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable();
      base.Tables.Add((DataTable) this.tableHydSNrListe);
      this.tableMIDKalibrierDaten = new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable();
      base.Tables.Add((DataTable) this.tableMIDKalibrierDaten);
      this.tableRechenwerkTyp = new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable();
      base.Tables.Add((DataTable) this.tableRechenwerkTyp);
      this.tableRechenwerkHerst = new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable();
      base.Tables.Add((DataTable) this.tableRechenwerkHerst);
      this.tableRecBel_2008 = new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable();
      base.Tables.Add((DataTable) this.tableRecBel_2008);
      this.tableRecPos_2008 = new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable();
      base.Tables.Add((DataTable) this.tableRecPos_2008);
      this.tableTypNamenReport = new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable();
      base.Tables.Add((DataTable) this.tableTypNamenReport);
      this.tableuseracc = new ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable();
      base.Tables.Add((DataTable) this.tableuseracc);
      this.tableusers = new ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable();
      base.Tables.Add((DataTable) this.tableusers);
      this.tableVersionsUpdate = new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable();
      base.Tables.Add((DataTable) this.tableVersionsUpdate);
      this.tableZaehlerTyp = new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable();
      base.Tables.Add((DataTable) this.tableZaehlerTyp);
      this.tableZaehlertypherst = new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable();
      base.Tables.Add((DataTable) this.tableZaehlertypherst);
      this.tableZaeBel_2008 = new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable();
      base.Tables.Add((DataTable) this.tableZaeBel_2008);
      this.tableZaePos_2008 = new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable();
      base.Tables.Add((DataTable) this.tableZaePos_2008);
      this.tableDateiliste = new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable();
      base.Tables.Add((DataTable) this.tableDateiliste);
      this.tableS_Version = new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable();
      base.Tables.Add((DataTable) this.tableS_Version);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeaccess() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeDichte() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeEdrbsql() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeFormulare() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializegroups() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializegrpcoll() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydraulikTyp() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydAblaufsteuerung() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydAblaufsteuerung1() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydAGew() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydBel_2008() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydDN() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydHerst() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydPos_2008() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeHydSNrListe() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeMIDKalibrierDaten() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeRechenwerkTyp() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeRechenwerkHerst() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeRecBel_2008() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeRecPos_2008() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeTypNamenReport() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeuseracc() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeusers() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeVersionsUpdate() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeZaehlerTyp() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeZaehlertypherst() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeZaeBel_2008() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeZaePos_2008() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeDateiliste() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeS_Version() => false;

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
      ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = schema.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public delegate void accessRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void DichteRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void EdrbsqlRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void FormulareRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void groupsRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void grpcollRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydraulikTypRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydAblaufsteuerungRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydAblaufsteuerung1RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydAGewRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydBel_2008RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydDNRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydHerstRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydPos_2008RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void HydSNrListeRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void MIDKalibrierDatenRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void RechenwerkTypRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void RechenwerkHerstRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void RecBel_2008RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void RecPos_2008RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void TypNamenReportRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void useraccRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void usersRowChangeEventHandler(object sender, ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void VersionsUpdateRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void ZaehlerTypRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void ZaehlertypherstRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void ZaeBel_2008RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void ZaePos_2008RowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void DateilisteRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void S_VersionRowChangeEventHandler(
      object sender,
      ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class accessDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.accessRow>
    {
      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public accessDataTable()
      {
        this.TableName = "access";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal accessDataTable(DataTable table)
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
      protected accessDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.accessRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEventHandler accessRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEventHandler accessRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEventHandler accessRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEventHandler accessRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddaccessRow(ZR_ClassLibrary.Schema_Mulda.Schema.accessRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.accessRow AddaccessRow()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.accessRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) this.NewRow();
        object[] objArray = new object[0];
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable accessDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable) base.Clone();
        accessDataTable.InitVars();
        return (DataTable) accessDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.accessRow NewaccessRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.accessRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.accessRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.accessRowChanged == null)
          return;
        this.accessRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.accessRowChanging == null)
          return;
        this.accessRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.accessRowDeleted == null)
          return;
        this.accessRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.accessRowDeleting == null)
          return;
        this.accessRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.accessRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.accessRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveaccessRow(ZR_ClassLibrary.Schema_Mulda.Schema.accessRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (accessDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class DichteDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnDichte;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DichteDataTable()
      {
        this.TableName = "Dichte";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal DichteDataTable(DataTable table)
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
      protected DichteDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DichteColumn => this.columnDichte;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEventHandler DichteRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEventHandler DichteRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEventHandler DichteRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEventHandler DichteRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddDichteRow(ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow AddDichteRow(int RecID, double Dichte)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) RecID,
          (object) Dichte
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable dichteDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable) base.Clone();
        dichteDataTable.InitVars();
        return (DataTable) dichteDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnDichte = this.Columns["Dichte"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnDichte = new DataColumn("Dichte", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDichte);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow NewDichteRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.DichteRowChanged == null)
          return;
        this.DichteRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.DichteRowChanging == null)
          return;
        this.DichteRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.DichteRowDeleted == null)
          return;
        this.DichteRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.DichteRowDeleting == null)
          return;
        this.DichteRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DichteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveDichteRow(ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (DichteDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class EdrbsqlDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow>
    {
      private DataColumn columnID;
      private DataColumn columnBeschreibung;
      private DataColumn columnSQL;
      private DataColumn columnParams;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EdrbsqlDataTable()
      {
        this.TableName = "Edrbsql";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EdrbsqlDataTable(DataTable table)
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
      protected EdrbsqlDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn IDColumn => this.columnID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BeschreibungColumn => this.columnBeschreibung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SQLColumn => this.columnSQL;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ParamsColumn => this.columnParams;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEventHandler EdrbsqlRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEventHandler EdrbsqlRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEventHandler EdrbsqlRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEventHandler EdrbsqlRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddEdrbsqlRow(ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow AddEdrbsqlRow(
        short ID,
        string Beschreibung,
        string SQL,
        string Params,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) this.NewRow();
        object[] objArray = new object[5]
        {
          (object) ID,
          (object) Beschreibung,
          (object) SQL,
          (object) Params,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable edrbsqlDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable) base.Clone();
        edrbsqlDataTable.InitVars();
        return (DataTable) edrbsqlDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnID = this.Columns["ID"];
        this.columnBeschreibung = this.Columns["Beschreibung"];
        this.columnSQL = this.Columns["SQL"];
        this.columnParams = this.Columns["Params"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnID = new DataColumn("ID", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnID);
        this.columnBeschreibung = new DataColumn("Beschreibung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBeschreibung);
        this.columnSQL = new DataColumn("SQL", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSQL);
        this.columnParams = new DataColumn("Params", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnParams);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.columnBeschreibung.MaxLength = 30;
        this.columnSQL.MaxLength = int.MaxValue;
        this.columnParams.MaxLength = int.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow NewEdrbsqlRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.EdrbsqlRowChanged == null)
          return;
        this.EdrbsqlRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.EdrbsqlRowChanging == null)
          return;
        this.EdrbsqlRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.EdrbsqlRowDeleted == null)
          return;
        this.EdrbsqlRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.EdrbsqlRowDeleting == null)
          return;
        this.EdrbsqlRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveEdrbsqlRow(ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (EdrbsqlDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class FormulareDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow>
    {
      private DataColumn columnSatzID;
      private DataColumn columnFormulartyp;
      private DataColumn columnBezeichnung;
      private DataColumn columnDruckerbemerkungen;
      private DataColumn columnFormularID;
      private DataColumn columnFormular;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public FormulareDataTable()
      {
        this.TableName = "Formulare";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal FormulareDataTable(DataTable table)
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
      protected FormulareDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SatzIDColumn => this.columnSatzID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FormulartypColumn => this.columnFormulartyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BezeichnungColumn => this.columnBezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DruckerbemerkungenColumn => this.columnDruckerbemerkungen;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FormularIDColumn => this.columnFormularID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FormularColumn => this.columnFormular;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEventHandler FormulareRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEventHandler FormulareRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEventHandler FormulareRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEventHandler FormulareRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddFormulareRow(ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow AddFormulareRow(
        string Formulartyp,
        string Bezeichnung,
        string Druckerbemerkungen,
        short FormularID,
        byte[] Formular)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) this.NewRow();
        object[] objArray = new object[6]
        {
          null,
          (object) Formulartyp,
          (object) Bezeichnung,
          (object) Druckerbemerkungen,
          (object) FormularID,
          (object) Formular
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable formulareDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable) base.Clone();
        formulareDataTable.InitVars();
        return (DataTable) formulareDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnSatzID = this.Columns["SatzID"];
        this.columnFormulartyp = this.Columns["Formulartyp"];
        this.columnBezeichnung = this.Columns["Bezeichnung"];
        this.columnDruckerbemerkungen = this.Columns["Druckerbemerkungen"];
        this.columnFormularID = this.Columns["FormularID"];
        this.columnFormular = this.Columns["Formular"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnSatzID = new DataColumn("SatzID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSatzID);
        this.columnFormulartyp = new DataColumn("Formulartyp", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFormulartyp);
        this.columnBezeichnung = new DataColumn("Bezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBezeichnung);
        this.columnDruckerbemerkungen = new DataColumn("Druckerbemerkungen", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDruckerbemerkungen);
        this.columnFormularID = new DataColumn("FormularID", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFormularID);
        this.columnFormular = new DataColumn("Formular", typeof (byte[]), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFormular);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnSatzID
        }, false));
        this.columnSatzID.AutoIncrement = true;
        this.columnSatzID.Unique = true;
        this.columnFormulartyp.MaxLength = 12;
        this.columnBezeichnung.MaxLength = 40;
        this.columnDruckerbemerkungen.MaxLength = 40;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow NewFormulareRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.FormulareRowChanged == null)
          return;
        this.FormulareRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.FormulareRowChanging == null)
          return;
        this.FormulareRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.FormulareRowDeleted == null)
          return;
        this.FormulareRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.FormulareRowDeleting == null)
          return;
        this.FormulareRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveFormulareRow(ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (FormulareDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class groupsDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow>
    {
      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public groupsDataTable()
      {
        this.TableName = "groups";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal groupsDataTable(DataTable table)
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
      protected groupsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEventHandler groupsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEventHandler groupsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEventHandler groupsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEventHandler groupsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddgroupsRow(ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow AddgroupsRow()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) this.NewRow();
        object[] objArray = new object[0];
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable groupsDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable) base.Clone();
        groupsDataTable.InitVars();
        return (DataTable) groupsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow NewgroupsRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.groupsRowChanged == null)
          return;
        this.groupsRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.groupsRowChanging == null)
          return;
        this.groupsRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.groupsRowDeleted == null)
          return;
        this.groupsRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.groupsRowDeleting == null)
          return;
        this.groupsRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.groupsRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemovegroupsRow(ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (groupsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class grpcollDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow>
    {
      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public grpcollDataTable()
      {
        this.TableName = "grpcoll";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal grpcollDataTable(DataTable table)
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
      protected grpcollDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEventHandler grpcollRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEventHandler grpcollRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEventHandler grpcollRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEventHandler grpcollRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddgrpcollRow(ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow AddgrpcollRow()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) this.NewRow();
        object[] objArray = new object[0];
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable grpcollDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable) base.Clone();
        grpcollDataTable.InitVars();
        return (DataTable) grpcollDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow NewgrpcollRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.grpcollRowChanged == null)
          return;
        this.grpcollRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.grpcollRowChanging == null)
          return;
        this.grpcollRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.grpcollRowDeleted == null)
          return;
        this.grpcollRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.grpcollRowDeleting == null)
          return;
        this.grpcollRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemovegrpcollRow(ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (grpcollDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydraulikTypDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow>
    {
      private DataColumn columnTypID;
      private DataColumn columnHersteller;
      private DataColumn columnTyp;
      private DataColumn columnArtikelnummer;
      private DataColumn columnZaehlerPruefTyp;
      private DataColumn columnZulassungsnr;
      private DataColumn columnEinbaulage;
      private DataColumn columnMetrologischeKlasse;
      private DataColumn columnQNenn;
      private DataColumn columnEichwert;
      private DataColumn columnStandardimpulswertigkeit;
      private DataColumn columnQMIN_UG;
      private DataColumn columnQMIN_OG;
      private DataColumn columnQTRENN_UG;
      private DataColumn columnQTRENN_OG;
      private DataColumn columnQNENN_UG;
      private DataColumn columnQNENN_OG;
      private DataColumn columnAnschlusstyp;
      private DataColumn columnQtMessZeit;
      private DataColumn columnQmMessZeit;
      private DataColumn columnQnMessZeit;
      private DataColumn columnQnMessmenge;
      private DataColumn columnQtMessmenge;
      private DataColumn columnQmMessmenge;
      private DataColumn columnGeraetepass;
      private DataColumn columnZaehlertyp_Pass;
      private DataColumn columnNennweite;
      private DataColumn columnEinbaulaenge;
      private DataColumn columnGewinde;
      private DataColumn columnQnennP;
      private DataColumn columnQnennN;
      private DataColumn columnQtrennP;
      private DataColumn columnQtrennN;
      private DataColumn columnQminP;
      private DataColumn columnQminN;
      private DataColumn columnMidQnenn;
      private DataColumn columnMidQtrenn;
      private DataColumn columnMidQmin;
      private DataColumn columnAnzPruefdurchflussQnenn;
      private DataColumn columnAnzPruefdurchflussQtrenn;
      private DataColumn columnAnzPruefdurchflussQmin;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydraulikTypDataTable()
      {
        this.TableName = "HydraulikTyp";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydraulikTypDataTable(DataTable table)
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
      protected HydraulikTypDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TypIDColumn => this.columnTypID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HerstellerColumn => this.columnHersteller;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TypColumn => this.columnTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ArtikelnummerColumn => this.columnArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ZaehlerPruefTypColumn => this.columnZaehlerPruefTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ZulassungsnrColumn => this.columnZulassungsnr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EinbaulageColumn => this.columnEinbaulage;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MetrologischeKlasseColumn => this.columnMetrologischeKlasse;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QNennColumn => this.columnQNenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EichwertColumn => this.columnEichwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn StandardimpulswertigkeitColumn => this.columnStandardimpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QMIN_UGColumn => this.columnQMIN_UG;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QMIN_OGColumn => this.columnQMIN_OG;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QTRENN_UGColumn => this.columnQTRENN_UG;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QTRENN_OGColumn => this.columnQTRENN_OG;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QNENN_UGColumn => this.columnQNENN_UG;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QNENN_OGColumn => this.columnQNENN_OG;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AnschlusstypColumn => this.columnAnschlusstyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtMessZeitColumn => this.columnQtMessZeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QmMessZeitColumn => this.columnQmMessZeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnMessZeitColumn => this.columnQnMessZeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnMessmengeColumn => this.columnQnMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtMessmengeColumn => this.columnQtMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QmMessmengeColumn => this.columnQmMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GeraetepassColumn => this.columnGeraetepass;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Zaehlertyp_PassColumn => this.columnZaehlertyp_Pass;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn NennweiteColumn => this.columnNennweite;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EinbaulaengeColumn => this.columnEinbaulaenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GewindeColumn => this.columnGewinde;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennPColumn => this.columnQnennP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennNColumn => this.columnQnennN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennPColumn => this.columnQtrennP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennNColumn => this.columnQtrennN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminPColumn => this.columnQminP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminNColumn => this.columnQminN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MidQnennColumn => this.columnMidQnenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MidQtrennColumn => this.columnMidQtrenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MidQminColumn => this.columnMidQmin;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AnzPruefdurchflussQnennColumn => this.columnAnzPruefdurchflussQnenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AnzPruefdurchflussQtrennColumn => this.columnAnzPruefdurchflussQtrenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AnzPruefdurchflussQminColumn => this.columnAnzPruefdurchflussQmin;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEventHandler HydraulikTypRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEventHandler HydraulikTypRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEventHandler HydraulikTypRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEventHandler HydraulikTypRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydraulikTypRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow AddHydraulikTypRow(
        int TypID,
        string Hersteller,
        string Typ,
        string Artikelnummer,
        short ZaehlerPruefTyp,
        string Zulassungsnr,
        string Einbaulage,
        string MetrologischeKlasse,
        double QNenn,
        double Eichwert,
        double Standardimpulswertigkeit,
        double QMIN_UG,
        double QMIN_OG,
        double QTRENN_UG,
        double QTRENN_OG,
        double QNENN_UG,
        double QNENN_OG,
        string Anschlusstyp,
        short QtMessZeit,
        short QmMessZeit,
        short QnMessZeit,
        double QnMessmenge,
        double QtMessmenge,
        double QmMessmenge,
        bool Geraetepass,
        string Zaehlertyp_Pass,
        string Nennweite,
        string Einbaulaenge,
        string Gewinde,
        double QnennP,
        double QnennN,
        double QtrennP,
        double QtrennN,
        double QminP,
        double QminN,
        short MidQnenn,
        short MidQtrenn,
        short MidQmin,
        string AnzPruefdurchflussQnenn,
        string AnzPruefdurchflussQtrenn,
        string AnzPruefdurchflussQmin,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) this.NewRow();
        object[] objArray = new object[42]
        {
          (object) TypID,
          (object) Hersteller,
          (object) Typ,
          (object) Artikelnummer,
          (object) ZaehlerPruefTyp,
          (object) Zulassungsnr,
          (object) Einbaulage,
          (object) MetrologischeKlasse,
          (object) QNenn,
          (object) Eichwert,
          (object) Standardimpulswertigkeit,
          (object) QMIN_UG,
          (object) QMIN_OG,
          (object) QTRENN_UG,
          (object) QTRENN_OG,
          (object) QNENN_UG,
          (object) QNENN_OG,
          (object) Anschlusstyp,
          (object) QtMessZeit,
          (object) QmMessZeit,
          (object) QnMessZeit,
          (object) QnMessmenge,
          (object) QtMessmenge,
          (object) QmMessmenge,
          (object) Geraetepass,
          (object) Zaehlertyp_Pass,
          (object) Nennweite,
          (object) Einbaulaenge,
          (object) Gewinde,
          (object) QnennP,
          (object) QnennN,
          (object) QtrennP,
          (object) QtrennN,
          (object) QminP,
          (object) QminN,
          (object) MidQnenn,
          (object) MidQtrenn,
          (object) MidQmin,
          (object) AnzPruefdurchflussQnenn,
          (object) AnzPruefdurchflussQtrenn,
          (object) AnzPruefdurchflussQmin,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable hydraulikTypDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable) base.Clone();
        hydraulikTypDataTable.InitVars();
        return (DataTable) hydraulikTypDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnTypID = this.Columns["TypID"];
        this.columnHersteller = this.Columns["Hersteller"];
        this.columnTyp = this.Columns["Typ"];
        this.columnArtikelnummer = this.Columns["Artikelnummer"];
        this.columnZaehlerPruefTyp = this.Columns["ZaehlerPruefTyp"];
        this.columnZulassungsnr = this.Columns["Zulassungsnr"];
        this.columnEinbaulage = this.Columns["Einbaulage"];
        this.columnMetrologischeKlasse = this.Columns["MetrologischeKlasse"];
        this.columnQNenn = this.Columns["QNenn"];
        this.columnEichwert = this.Columns["Eichwert"];
        this.columnStandardimpulswertigkeit = this.Columns["Standardimpulswertigkeit"];
        this.columnQMIN_UG = this.Columns["QMIN_UG"];
        this.columnQMIN_OG = this.Columns["QMIN_OG"];
        this.columnQTRENN_UG = this.Columns["QTRENN_UG"];
        this.columnQTRENN_OG = this.Columns["QTRENN_OG"];
        this.columnQNENN_UG = this.Columns["QNENN_UG"];
        this.columnQNENN_OG = this.Columns["QNENN_OG"];
        this.columnAnschlusstyp = this.Columns["Anschlusstyp"];
        this.columnQtMessZeit = this.Columns["QtMessZeit"];
        this.columnQmMessZeit = this.Columns["QmMessZeit"];
        this.columnQnMessZeit = this.Columns["QnMessZeit"];
        this.columnQnMessmenge = this.Columns["QnMessmenge"];
        this.columnQtMessmenge = this.Columns["QtMessmenge"];
        this.columnQmMessmenge = this.Columns["QmMessmenge"];
        this.columnGeraetepass = this.Columns["Geraetepass"];
        this.columnZaehlertyp_Pass = this.Columns["Zaehlertyp_Pass"];
        this.columnNennweite = this.Columns["Nennweite"];
        this.columnEinbaulaenge = this.Columns["Einbaulaenge"];
        this.columnGewinde = this.Columns["Gewinde"];
        this.columnQnennP = this.Columns["QnennP"];
        this.columnQnennN = this.Columns["QnennN"];
        this.columnQtrennP = this.Columns["QtrennP"];
        this.columnQtrennN = this.Columns["QtrennN"];
        this.columnQminP = this.Columns["QminP"];
        this.columnQminN = this.Columns["QminN"];
        this.columnMidQnenn = this.Columns["MidQnenn"];
        this.columnMidQtrenn = this.Columns["MidQtrenn"];
        this.columnMidQmin = this.Columns["MidQmin"];
        this.columnAnzPruefdurchflussQnenn = this.Columns["AnzPruefdurchflussQnenn"];
        this.columnAnzPruefdurchflussQtrenn = this.Columns["AnzPruefdurchflussQtrenn"];
        this.columnAnzPruefdurchflussQmin = this.Columns["AnzPruefdurchflussQmin"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnTypID = new DataColumn("TypID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTypID);
        this.columnHersteller = new DataColumn("Hersteller", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHersteller);
        this.columnTyp = new DataColumn("Typ", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTyp);
        this.columnArtikelnummer = new DataColumn("Artikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnArtikelnummer);
        this.columnZaehlerPruefTyp = new DataColumn("ZaehlerPruefTyp", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZaehlerPruefTyp);
        this.columnZulassungsnr = new DataColumn("Zulassungsnr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZulassungsnr);
        this.columnEinbaulage = new DataColumn("Einbaulage", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEinbaulage);
        this.columnMetrologischeKlasse = new DataColumn("MetrologischeKlasse", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMetrologischeKlasse);
        this.columnQNenn = new DataColumn("QNenn", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQNenn);
        this.columnEichwert = new DataColumn("Eichwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEichwert);
        this.columnStandardimpulswertigkeit = new DataColumn("Standardimpulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnStandardimpulswertigkeit);
        this.columnQMIN_UG = new DataColumn("QMIN_UG", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQMIN_UG);
        this.columnQMIN_OG = new DataColumn("QMIN_OG", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQMIN_OG);
        this.columnQTRENN_UG = new DataColumn("QTRENN_UG", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQTRENN_UG);
        this.columnQTRENN_OG = new DataColumn("QTRENN_OG", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQTRENN_OG);
        this.columnQNENN_UG = new DataColumn("QNENN_UG", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQNENN_UG);
        this.columnQNENN_OG = new DataColumn("QNENN_OG", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQNENN_OG);
        this.columnAnschlusstyp = new DataColumn("Anschlusstyp", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAnschlusstyp);
        this.columnQtMessZeit = new DataColumn("QtMessZeit", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtMessZeit);
        this.columnQmMessZeit = new DataColumn("QmMessZeit", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQmMessZeit);
        this.columnQnMessZeit = new DataColumn("QnMessZeit", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnMessZeit);
        this.columnQnMessmenge = new DataColumn("QnMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnMessmenge);
        this.columnQtMessmenge = new DataColumn("QtMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtMessmenge);
        this.columnQmMessmenge = new DataColumn("QmMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQmMessmenge);
        this.columnGeraetepass = new DataColumn("Geraetepass", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGeraetepass);
        this.columnZaehlertyp_Pass = new DataColumn("Zaehlertyp_Pass", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZaehlertyp_Pass);
        this.columnNennweite = new DataColumn("Nennweite", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnNennweite);
        this.columnEinbaulaenge = new DataColumn("Einbaulaenge", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEinbaulaenge);
        this.columnGewinde = new DataColumn("Gewinde", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGewinde);
        this.columnQnennP = new DataColumn("QnennP", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennP);
        this.columnQnennN = new DataColumn("QnennN", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennN);
        this.columnQtrennP = new DataColumn("QtrennP", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennP);
        this.columnQtrennN = new DataColumn("QtrennN", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennN);
        this.columnQminP = new DataColumn("QminP", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminP);
        this.columnQminN = new DataColumn("QminN", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminN);
        this.columnMidQnenn = new DataColumn("MidQnenn", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMidQnenn);
        this.columnMidQtrenn = new DataColumn("MidQtrenn", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMidQtrenn);
        this.columnMidQmin = new DataColumn("MidQmin", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMidQmin);
        this.columnAnzPruefdurchflussQnenn = new DataColumn("AnzPruefdurchflussQnenn", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAnzPruefdurchflussQnenn);
        this.columnAnzPruefdurchflussQtrenn = new DataColumn("AnzPruefdurchflussQtrenn", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAnzPruefdurchflussQtrenn);
        this.columnAnzPruefdurchflussQmin = new DataColumn("AnzPruefdurchflussQmin", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAnzPruefdurchflussQmin);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnTypID
        }, false));
        this.columnTypID.Unique = true;
        this.columnHersteller.MaxLength = 30;
        this.columnTyp.MaxLength = 50;
        this.columnArtikelnummer.MaxLength = 20;
        this.columnZulassungsnr.MaxLength = 50;
        this.columnEinbaulage.MaxLength = 5;
        this.columnMetrologischeKlasse.MaxLength = 5;
        this.columnAnschlusstyp.MaxLength = 50;
        this.columnZaehlertyp_Pass.MaxLength = 10;
        this.columnNennweite.MaxLength = 10;
        this.columnEinbaulaenge.MaxLength = 20;
        this.columnGewinde.MaxLength = 20;
        this.columnAnzPruefdurchflussQnenn.MaxLength = int.MaxValue;
        this.columnAnzPruefdurchflussQtrenn.MaxLength = int.MaxValue;
        this.columnAnzPruefdurchflussQmin.MaxLength = int.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow NewHydraulikTypRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydraulikTypRowChanged == null)
          return;
        this.HydraulikTypRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydraulikTypRowChanging == null)
          return;
        this.HydraulikTypRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydraulikTypRowDeleted == null)
          return;
        this.HydraulikTypRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydraulikTypRowDeleting == null)
          return;
        this.HydraulikTypRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydraulikTypRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydraulikTypDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydAblaufsteuerungDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow>
    {
      private DataColumn columnAblaufTyp;
      private DataColumn columnAblaufID;
      private DataColumn columnAblaufSchritt;
      private DataColumn columnAblaufKommando;
      private DataColumn columnAblaufHinweis;
      private DataColumn columnDauer;
      private DataColumn columnKommando;
      private DataColumn columnSignal;
      private DataColumn columnAnzPruefdurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydAblaufsteuerungDataTable()
      {
        this.TableName = "HydAblaufsteuerung";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydAblaufsteuerungDataTable(DataTable table)
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
      protected HydAblaufsteuerungDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufTypColumn => this.columnAblaufTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufIDColumn => this.columnAblaufID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufSchrittColumn => this.columnAblaufSchritt;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufKommandoColumn => this.columnAblaufKommando;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufHinweisColumn => this.columnAblaufHinweis;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DauerColumn => this.columnDauer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn KommandoColumn => this.columnKommando;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SignalColumn => this.columnSignal;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AnzPruefdurchflussColumn => this.columnAnzPruefdurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEventHandler HydAblaufsteuerungRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEventHandler HydAblaufsteuerungRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEventHandler HydAblaufsteuerungRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEventHandler HydAblaufsteuerungRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydAblaufsteuerungRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow AddHydAblaufsteuerungRow(
        string AblaufTyp,
        int AblaufID,
        string AblaufSchritt,
        string AblaufKommando,
        string AblaufHinweis,
        int Dauer,
        short Kommando,
        bool Signal,
        short AnzPruefdurchfluss)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) this.NewRow();
        object[] objArray = new object[9]
        {
          (object) AblaufTyp,
          (object) AblaufID,
          (object) AblaufSchritt,
          (object) AblaufKommando,
          (object) AblaufHinweis,
          (object) Dauer,
          (object) Kommando,
          (object) Signal,
          (object) AnzPruefdurchfluss
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable ablaufsteuerungDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable) base.Clone();
        ablaufsteuerungDataTable.InitVars();
        return (DataTable) ablaufsteuerungDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnAblaufTyp = this.Columns["AblaufTyp"];
        this.columnAblaufID = this.Columns["AblaufID"];
        this.columnAblaufSchritt = this.Columns["AblaufSchritt"];
        this.columnAblaufKommando = this.Columns["AblaufKommando"];
        this.columnAblaufHinweis = this.Columns["AblaufHinweis"];
        this.columnDauer = this.Columns["Dauer"];
        this.columnKommando = this.Columns["Kommando"];
        this.columnSignal = this.Columns["Signal"];
        this.columnAnzPruefdurchfluss = this.Columns["AnzPruefdurchfluss"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnAblaufTyp = new DataColumn("AblaufTyp", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufTyp);
        this.columnAblaufID = new DataColumn("AblaufID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufID);
        this.columnAblaufSchritt = new DataColumn("AblaufSchritt", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufSchritt);
        this.columnAblaufKommando = new DataColumn("AblaufKommando", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufKommando);
        this.columnAblaufHinweis = new DataColumn("AblaufHinweis", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufHinweis);
        this.columnDauer = new DataColumn("Dauer", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDauer);
        this.columnKommando = new DataColumn("Kommando", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnKommando);
        this.columnSignal = new DataColumn("Signal", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSignal);
        this.columnAnzPruefdurchfluss = new DataColumn("AnzPruefdurchfluss", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAnzPruefdurchfluss);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnAblaufTyp
        }, false));
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[1]
        {
          this.columnAblaufID
        }, false));
        this.columnAblaufTyp.Unique = true;
        this.columnAblaufTyp.MaxLength = 3;
        this.columnAblaufID.Unique = true;
        this.columnAblaufSchritt.MaxLength = 50;
        this.columnAblaufKommando.MaxLength = int.MaxValue;
        this.columnAblaufHinweis.MaxLength = int.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow NewHydAblaufsteuerungRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydAblaufsteuerungRowChanged == null)
          return;
        this.HydAblaufsteuerungRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydAblaufsteuerungRowChanging == null)
          return;
        this.HydAblaufsteuerungRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydAblaufsteuerungRowDeleted == null)
          return;
        this.HydAblaufsteuerungRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydAblaufsteuerungRowDeleting == null)
          return;
        this.HydAblaufsteuerungRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydAblaufsteuerungRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydAblaufsteuerungDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydAblaufsteuerung1DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row>
    {
      private DataColumn columnAblaufTyp;
      private DataColumn columnAblaufID;
      private DataColumn columnAblaufSchritt;
      private DataColumn columnAblaufKommando;
      private DataColumn columnAblaufHinweis;
      private DataColumn columnDauer;
      private DataColumn columnKommando;
      private DataColumn columnSignal;
      private DataColumn columnAnzPruefdurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydAblaufsteuerung1DataTable()
      {
        this.TableName = "HydAblaufsteuerung1";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydAblaufsteuerung1DataTable(DataTable table)
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
      protected HydAblaufsteuerung1DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufTypColumn => this.columnAblaufTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufIDColumn => this.columnAblaufID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufSchrittColumn => this.columnAblaufSchritt;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufKommandoColumn => this.columnAblaufKommando;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AblaufHinweisColumn => this.columnAblaufHinweis;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DauerColumn => this.columnDauer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn KommandoColumn => this.columnKommando;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SignalColumn => this.columnSignal;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AnzPruefdurchflussColumn => this.columnAnzPruefdurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEventHandler HydAblaufsteuerung1RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEventHandler HydAblaufsteuerung1RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEventHandler HydAblaufsteuerung1RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEventHandler HydAblaufsteuerung1RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydAblaufsteuerung1Row(ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row AddHydAblaufsteuerung1Row(
        string AblaufTyp,
        int AblaufID,
        string AblaufSchritt,
        string AblaufKommando,
        string AblaufHinweis,
        int Dauer,
        short Kommando,
        bool Signal,
        short AnzPruefdurchfluss)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) this.NewRow();
        object[] objArray = new object[9]
        {
          (object) AblaufTyp,
          (object) AblaufID,
          (object) AblaufSchritt,
          (object) AblaufKommando,
          (object) AblaufHinweis,
          (object) Dauer,
          (object) Kommando,
          (object) Signal,
          (object) AnzPruefdurchfluss
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable ablaufsteuerung1DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable) base.Clone();
        ablaufsteuerung1DataTable.InitVars();
        return (DataTable) ablaufsteuerung1DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnAblaufTyp = this.Columns["AblaufTyp"];
        this.columnAblaufID = this.Columns["AblaufID"];
        this.columnAblaufSchritt = this.Columns["AblaufSchritt"];
        this.columnAblaufKommando = this.Columns["AblaufKommando"];
        this.columnAblaufHinweis = this.Columns["AblaufHinweis"];
        this.columnDauer = this.Columns["Dauer"];
        this.columnKommando = this.Columns["Kommando"];
        this.columnSignal = this.Columns["Signal"];
        this.columnAnzPruefdurchfluss = this.Columns["AnzPruefdurchfluss"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnAblaufTyp = new DataColumn("AblaufTyp", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufTyp);
        this.columnAblaufID = new DataColumn("AblaufID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufID);
        this.columnAblaufSchritt = new DataColumn("AblaufSchritt", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufSchritt);
        this.columnAblaufKommando = new DataColumn("AblaufKommando", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufKommando);
        this.columnAblaufHinweis = new DataColumn("AblaufHinweis", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAblaufHinweis);
        this.columnDauer = new DataColumn("Dauer", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDauer);
        this.columnKommando = new DataColumn("Kommando", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnKommando);
        this.columnSignal = new DataColumn("Signal", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSignal);
        this.columnAnzPruefdurchfluss = new DataColumn("AnzPruefdurchfluss", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAnzPruefdurchfluss);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnAblaufTyp
        }, false));
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[1]
        {
          this.columnAblaufID
        }, false));
        this.columnAblaufTyp.Unique = true;
        this.columnAblaufTyp.MaxLength = 3;
        this.columnAblaufID.Unique = true;
        this.columnAblaufSchritt.MaxLength = 50;
        this.columnAblaufKommando.MaxLength = int.MaxValue;
        this.columnAblaufHinweis.MaxLength = int.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row NewHydAblaufsteuerung1Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydAblaufsteuerung1RowChanged == null)
          return;
        this.HydAblaufsteuerung1RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydAblaufsteuerung1RowChanging == null)
          return;
        this.HydAblaufsteuerung1RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydAblaufsteuerung1RowDeleted == null)
          return;
        this.HydAblaufsteuerung1RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydAblaufsteuerung1RowDeleting == null)
          return;
        this.HydAblaufsteuerung1RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydAblaufsteuerung1Row(ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydAblaufsteuerung1DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydAGewDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnBezeichnung;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydAGewDataTable()
      {
        this.TableName = "HydAGew";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydAGewDataTable(DataTable table)
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
      protected HydAGewDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BezeichnungColumn => this.columnBezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEventHandler HydAGewRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEventHandler HydAGewRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEventHandler HydAGewRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEventHandler HydAGewRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydAGewRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow AddHydAGewRow(
        int RecID,
        string Bezeichnung,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) this.NewRow();
        object[] objArray = new object[3]
        {
          (object) RecID,
          (object) Bezeichnung,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable hydAgewDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable) base.Clone();
        hydAgewDataTable.InitVars();
        return (DataTable) hydAgewDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnBezeichnung = this.Columns["Bezeichnung"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnBezeichnung = new DataColumn("Bezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBezeichnung);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
        this.columnBezeichnung.MaxLength = 30;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow NewHydAGewRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydAGewRowChanged == null)
          return;
        this.HydAGewRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydAGewRowChanging == null)
          return;
        this.HydAGewRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydAGewRowDeleted == null)
          return;
        this.HydAGewRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydAGewRowDeleting == null)
          return;
        this.HydAGewRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydAGewRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydAGewDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydBel_2008DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row>
    {
      private DataColumn columnChargenNr;
      private DataColumn columnZaehlerTyp;
      private DataColumn columnZaehlerPruefTyp;
      private DataColumn columnArtikelnummer;
      private DataColumn columnBelegstatus;
      private DataColumn columnImpulswertigkeit;
      private DataColumn columnHerstelljahr;
      private DataColumn columnPruefstand;
      private DataColumn columnPruefer;
      private DataColumn columnMIDKalibID;
      private DataColumn columnMessdatum;
      private DataColumn columnQtrennTemp;
      private DataColumn columnQtrennDurchfluss;
      private DataColumn columnQtrennKalibriervolumen;
      private DataColumn columnQtrennMessmenge;
      private DataColumn columnQtrennTime;
      private DataColumn columnQminTemp;
      private DataColumn columnQminDurchfluss;
      private DataColumn columnQminKalibriervolumen;
      private DataColumn columnQminMessmenge;
      private DataColumn columnQminTime;
      private DataColumn columnQnennTemp;
      private DataColumn columnQnennDurchfluss;
      private DataColumn columnQnennKalibriervolumen;
      private DataColumn columnQnennMessmenge;
      private DataColumn columnQnennTime;
      private DataColumn columnGeraetepass;
      private DataColumn columnBemerkung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydBel_2008DataTable()
      {
        this.TableName = "HydBel_2008";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydBel_2008DataTable(DataTable table)
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
      protected HydBel_2008DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNrColumn => this.columnChargenNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ZaehlerTypColumn => this.columnZaehlerTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ZaehlerPruefTypColumn => this.columnZaehlerPruefTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ArtikelnummerColumn => this.columnArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BelegstatusColumn => this.columnBelegstatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ImpulswertigkeitColumn => this.columnImpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HerstelljahrColumn => this.columnHerstelljahr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PruefstandColumn => this.columnPruefstand;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PrueferColumn => this.columnPruefer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MIDKalibIDColumn => this.columnMIDKalibID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MessdatumColumn => this.columnMessdatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennTempColumn => this.columnQtrennTemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennDurchflussColumn => this.columnQtrennDurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennKalibriervolumenColumn => this.columnQtrennKalibriervolumen;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennMessmengeColumn => this.columnQtrennMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennTimeColumn => this.columnQtrennTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminTempColumn => this.columnQminTemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminDurchflussColumn => this.columnQminDurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminKalibriervolumenColumn => this.columnQminKalibriervolumen;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminMessmengeColumn => this.columnQminMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminTimeColumn => this.columnQminTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennTempColumn => this.columnQnennTemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennDurchflussColumn => this.columnQnennDurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennKalibriervolumenColumn => this.columnQnennKalibriervolumen;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennMessmengeColumn => this.columnQnennMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennTimeColumn => this.columnQnennTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GeraetepassColumn => this.columnGeraetepass;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BemerkungColumn => this.columnBemerkung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEventHandler HydBel_2008RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEventHandler HydBel_2008RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEventHandler HydBel_2008RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEventHandler HydBel_2008RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydBel_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row AddHydBel_2008Row(
        string ChargenNr,
        int ZaehlerTyp,
        short ZaehlerPruefTyp,
        string Artikelnummer,
        short Belegstatus,
        double Impulswertigkeit,
        string Herstelljahr,
        string Pruefstand,
        string Pruefer,
        int MIDKalibID,
        DateTime Messdatum,
        double QtrennTemp,
        double QtrennDurchfluss,
        double QtrennKalibriervolumen,
        double QtrennMessmenge,
        short QtrennTime,
        double QminTemp,
        double QminDurchfluss,
        double QminKalibriervolumen,
        double QminMessmenge,
        short QminTime,
        double QnennTemp,
        double QnennDurchfluss,
        double QnennKalibriervolumen,
        double QnennMessmenge,
        short QnennTime,
        bool Geraetepass,
        string Bemerkung)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) this.NewRow();
        object[] objArray = new object[28]
        {
          (object) ChargenNr,
          (object) ZaehlerTyp,
          (object) ZaehlerPruefTyp,
          (object) Artikelnummer,
          (object) Belegstatus,
          (object) Impulswertigkeit,
          (object) Herstelljahr,
          (object) Pruefstand,
          (object) Pruefer,
          (object) MIDKalibID,
          (object) Messdatum,
          (object) QtrennTemp,
          (object) QtrennDurchfluss,
          (object) QtrennKalibriervolumen,
          (object) QtrennMessmenge,
          (object) QtrennTime,
          (object) QminTemp,
          (object) QminDurchfluss,
          (object) QminKalibriervolumen,
          (object) QminMessmenge,
          (object) QminTime,
          (object) QnennTemp,
          (object) QnennDurchfluss,
          (object) QnennKalibriervolumen,
          (object) QnennMessmenge,
          (object) QnennTime,
          (object) Geraetepass,
          (object) Bemerkung
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable bel2008DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable) base.Clone();
        bel2008DataTable.InitVars();
        return (DataTable) bel2008DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnChargenNr = this.Columns["ChargenNr"];
        this.columnZaehlerTyp = this.Columns["ZaehlerTyp"];
        this.columnZaehlerPruefTyp = this.Columns["ZaehlerPruefTyp"];
        this.columnArtikelnummer = this.Columns["Artikelnummer"];
        this.columnBelegstatus = this.Columns["Belegstatus"];
        this.columnImpulswertigkeit = this.Columns["Impulswertigkeit"];
        this.columnHerstelljahr = this.Columns["Herstelljahr"];
        this.columnPruefstand = this.Columns["Pruefstand"];
        this.columnPruefer = this.Columns["Pruefer"];
        this.columnMIDKalibID = this.Columns["MIDKalibID"];
        this.columnMessdatum = this.Columns["Messdatum"];
        this.columnQtrennTemp = this.Columns["QtrennTemp"];
        this.columnQtrennDurchfluss = this.Columns["QtrennDurchfluss"];
        this.columnQtrennKalibriervolumen = this.Columns["QtrennKalibriervolumen"];
        this.columnQtrennMessmenge = this.Columns["QtrennMessmenge"];
        this.columnQtrennTime = this.Columns["QtrennTime"];
        this.columnQminTemp = this.Columns["QminTemp"];
        this.columnQminDurchfluss = this.Columns["QminDurchfluss"];
        this.columnQminKalibriervolumen = this.Columns["QminKalibriervolumen"];
        this.columnQminMessmenge = this.Columns["QminMessmenge"];
        this.columnQminTime = this.Columns["QminTime"];
        this.columnQnennTemp = this.Columns["QnennTemp"];
        this.columnQnennDurchfluss = this.Columns["QnennDurchfluss"];
        this.columnQnennKalibriervolumen = this.Columns["QnennKalibriervolumen"];
        this.columnQnennMessmenge = this.Columns["QnennMessmenge"];
        this.columnQnennTime = this.Columns["QnennTime"];
        this.columnGeraetepass = this.Columns["Geraetepass"];
        this.columnBemerkung = this.Columns["Bemerkung"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnChargenNr = new DataColumn("ChargenNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr);
        this.columnZaehlerTyp = new DataColumn("ZaehlerTyp", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZaehlerTyp);
        this.columnZaehlerPruefTyp = new DataColumn("ZaehlerPruefTyp", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZaehlerPruefTyp);
        this.columnArtikelnummer = new DataColumn("Artikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnArtikelnummer);
        this.columnBelegstatus = new DataColumn("Belegstatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBelegstatus);
        this.columnImpulswertigkeit = new DataColumn("Impulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnImpulswertigkeit);
        this.columnHerstelljahr = new DataColumn("Herstelljahr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHerstelljahr);
        this.columnPruefstand = new DataColumn("Pruefstand", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefstand);
        this.columnPruefer = new DataColumn("Pruefer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefer);
        this.columnMIDKalibID = new DataColumn("MIDKalibID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMIDKalibID);
        this.columnMessdatum = new DataColumn("Messdatum", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMessdatum);
        this.columnQtrennTemp = new DataColumn("QtrennTemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennTemp);
        this.columnQtrennDurchfluss = new DataColumn("QtrennDurchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennDurchfluss);
        this.columnQtrennKalibriervolumen = new DataColumn("QtrennKalibriervolumen", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennKalibriervolumen);
        this.columnQtrennMessmenge = new DataColumn("QtrennMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennMessmenge);
        this.columnQtrennTime = new DataColumn("QtrennTime", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennTime);
        this.columnQminTemp = new DataColumn("QminTemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminTemp);
        this.columnQminDurchfluss = new DataColumn("QminDurchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminDurchfluss);
        this.columnQminKalibriervolumen = new DataColumn("QminKalibriervolumen", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminKalibriervolumen);
        this.columnQminMessmenge = new DataColumn("QminMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminMessmenge);
        this.columnQminTime = new DataColumn("QminTime", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminTime);
        this.columnQnennTemp = new DataColumn("QnennTemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennTemp);
        this.columnQnennDurchfluss = new DataColumn("QnennDurchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennDurchfluss);
        this.columnQnennKalibriervolumen = new DataColumn("QnennKalibriervolumen", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennKalibriervolumen);
        this.columnQnennMessmenge = new DataColumn("QnennMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennMessmenge);
        this.columnQnennTime = new DataColumn("QnennTime", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennTime);
        this.columnGeraetepass = new DataColumn("Geraetepass", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGeraetepass);
        this.columnBemerkung = new DataColumn("Bemerkung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBemerkung);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnChargenNr
        }, false));
        this.columnChargenNr.Unique = true;
        this.columnChargenNr.MaxLength = 10;
        this.columnArtikelnummer.MaxLength = 20;
        this.columnHerstelljahr.MaxLength = 4;
        this.columnPruefstand.MaxLength = 6;
        this.columnPruefer.MaxLength = 20;
        this.columnBemerkung.MaxLength = 5;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row NewHydBel_2008Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydBel_2008RowChanged == null)
          return;
        this.HydBel_2008RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydBel_2008RowChanging == null)
          return;
        this.HydBel_2008RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydBel_2008RowDeleted == null)
          return;
        this.HydBel_2008RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydBel_2008RowDeleting == null)
          return;
        this.HydBel_2008RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydBel_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydBel_2008DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydDNDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnBezeichnung;
      private DataColumn columnQnMessmenge;
      private DataColumn columnQtMessmenge;
      private DataColumn columnQmMessmenge;
      private DataColumn columnQnennP;
      private DataColumn columnQnennN;
      private DataColumn columnQtrennP;
      private DataColumn columnQtrennN;
      private DataColumn columnQminP;
      private DataColumn columnQminN;
      private DataColumn columnMidQnenn;
      private DataColumn columnMidQtrenn;
      private DataColumn columnMidQmin;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydDNDataTable()
      {
        this.TableName = "HydDN";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydDNDataTable(DataTable table)
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
      protected HydDNDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BezeichnungColumn => this.columnBezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnMessmengeColumn => this.columnQnMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtMessmengeColumn => this.columnQtMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QmMessmengeColumn => this.columnQmMessmenge;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennPColumn => this.columnQnennP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennNColumn => this.columnQnennN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennPColumn => this.columnQtrennP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennNColumn => this.columnQtrennN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminPColumn => this.columnQminP;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminNColumn => this.columnQminN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MidQnennColumn => this.columnMidQnenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MidQtrennColumn => this.columnMidQtrenn;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MidQminColumn => this.columnMidQmin;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEventHandler HydDNRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEventHandler HydDNRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEventHandler HydDNRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEventHandler HydDNRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydDNRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow AddHydDNRow(
        int RecID,
        string Bezeichnung,
        double QnMessmenge,
        double QtMessmenge,
        double QmMessmenge,
        double QnennP,
        double QnennN,
        double QtrennP,
        double QtrennN,
        double QminP,
        double QminN,
        short MidQnenn,
        short MidQtrenn,
        short MidQmin,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) this.NewRow();
        object[] objArray = new object[15]
        {
          (object) RecID,
          (object) Bezeichnung,
          (object) QnMessmenge,
          (object) QtMessmenge,
          (object) QmMessmenge,
          (object) QnennP,
          (object) QnennN,
          (object) QtrennP,
          (object) QtrennN,
          (object) QminP,
          (object) QminN,
          (object) MidQnenn,
          (object) MidQtrenn,
          (object) MidQmin,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable hydDnDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable) base.Clone();
        hydDnDataTable.InitVars();
        return (DataTable) hydDnDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnBezeichnung = this.Columns["Bezeichnung"];
        this.columnQnMessmenge = this.Columns["QnMessmenge"];
        this.columnQtMessmenge = this.Columns["QtMessmenge"];
        this.columnQmMessmenge = this.Columns["QmMessmenge"];
        this.columnQnennP = this.Columns["QnennP"];
        this.columnQnennN = this.Columns["QnennN"];
        this.columnQtrennP = this.Columns["QtrennP"];
        this.columnQtrennN = this.Columns["QtrennN"];
        this.columnQminP = this.Columns["QminP"];
        this.columnQminN = this.Columns["QminN"];
        this.columnMidQnenn = this.Columns["MidQnenn"];
        this.columnMidQtrenn = this.Columns["MidQtrenn"];
        this.columnMidQmin = this.Columns["MidQmin"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnBezeichnung = new DataColumn("Bezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBezeichnung);
        this.columnQnMessmenge = new DataColumn("QnMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnMessmenge);
        this.columnQtMessmenge = new DataColumn("QtMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtMessmenge);
        this.columnQmMessmenge = new DataColumn("QmMessmenge", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQmMessmenge);
        this.columnQnennP = new DataColumn("QnennP", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennP);
        this.columnQnennN = new DataColumn("QnennN", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennN);
        this.columnQtrennP = new DataColumn("QtrennP", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennP);
        this.columnQtrennN = new DataColumn("QtrennN", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennN);
        this.columnQminP = new DataColumn("QminP", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminP);
        this.columnQminN = new DataColumn("QminN", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminN);
        this.columnMidQnenn = new DataColumn("MidQnenn", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMidQnenn);
        this.columnMidQtrenn = new DataColumn("MidQtrenn", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMidQtrenn);
        this.columnMidQmin = new DataColumn("MidQmin", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMidQmin);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
        this.columnBezeichnung.MaxLength = 10;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow NewHydDNRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydDNRowChanged == null)
          return;
        this.HydDNRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydDNRowChanging == null)
          return;
        this.HydDNRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydDNRowDeleted == null)
          return;
        this.HydDNRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydDNRowDeleting == null)
          return;
        this.HydDNRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydDNRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydDNDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydHerstDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnBezeichnung;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydHerstDataTable()
      {
        this.TableName = "HydHerst";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydHerstDataTable(DataTable table)
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
      protected HydHerstDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BezeichnungColumn => this.columnBezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEventHandler HydHerstRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEventHandler HydHerstRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEventHandler HydHerstRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEventHandler HydHerstRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydHerstRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow AddHydHerstRow(
        int RecID,
        string Bezeichnung,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) this.NewRow();
        object[] objArray = new object[3]
        {
          (object) RecID,
          (object) Bezeichnung,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable hydHerstDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable) base.Clone();
        hydHerstDataTable.InitVars();
        return (DataTable) hydHerstDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnBezeichnung = this.Columns["Bezeichnung"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnBezeichnung = new DataColumn("Bezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBezeichnung);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
        this.columnBezeichnung.MaxLength = 30;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow NewHydHerstRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydHerstRowChanged == null)
          return;
        this.HydHerstRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydHerstRowChanging == null)
          return;
        this.HydHerstRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydHerstRowDeleted == null)
          return;
        this.HydHerstRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydHerstRowDeleting == null)
          return;
        this.HydHerstRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydHerstRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydHerstDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydPos_2008DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row>
    {
      private DataColumn columnChargenNr;
      private DataColumn columnChargenIndex;
      private DataColumn columnSerienNr;
      private DataColumn columnSerienNrZaehler;
      private DataColumn columnLSNr;
      private DataColumn columnTermin;
      private DataColumn columnGeraetestatus;
      private DataColumn columnBasisImpulswertigkeit;
      private DataColumn columnQnennMessfehler;
      private DataColumn columnQtrennMessfehler;
      private DataColumn columnQminMessfehler;
      private DataColumn columnImpulswertigkeit;
      private DataColumn columnQnennFehler;
      private DataColumn columnQtrennFehler;
      private DataColumn columnQminFehler;
      private DataColumn columnInZaehlerNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydPos_2008DataTable()
      {
        this.TableName = "HydPos_2008";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydPos_2008DataTable(DataTable table)
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
      protected HydPos_2008DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNrColumn => this.columnChargenNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenIndexColumn => this.columnChargenIndex;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNrColumn => this.columnSerienNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNrZaehlerColumn => this.columnSerienNrZaehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LSNrColumn => this.columnLSNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TerminColumn => this.columnTermin;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GeraetestatusColumn => this.columnGeraetestatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BasisImpulswertigkeitColumn => this.columnBasisImpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennMessfehlerColumn => this.columnQnennMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennMessfehlerColumn => this.columnQtrennMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminMessfehlerColumn => this.columnQminMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ImpulswertigkeitColumn => this.columnImpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennFehlerColumn => this.columnQnennFehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennFehlerColumn => this.columnQtrennFehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminFehlerColumn => this.columnQminFehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InZaehlerNrColumn => this.columnInZaehlerNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEventHandler HydPos_2008RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEventHandler HydPos_2008RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEventHandler HydPos_2008RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEventHandler HydPos_2008RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydPos_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row AddHydPos_2008Row(
        string ChargenNr,
        short ChargenIndex,
        string SerienNr,
        short SerienNrZaehler,
        short LSNr,
        DateTime Termin,
        short Geraetestatus,
        double BasisImpulswertigkeit,
        double QnennMessfehler,
        double QtrennMessfehler,
        double QminMessfehler,
        double Impulswertigkeit,
        double QnennFehler,
        double QtrennFehler,
        double QminFehler,
        string InZaehlerNr)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) this.NewRow();
        object[] objArray = new object[16]
        {
          (object) ChargenNr,
          (object) ChargenIndex,
          (object) SerienNr,
          (object) SerienNrZaehler,
          (object) LSNr,
          (object) Termin,
          (object) Geraetestatus,
          (object) BasisImpulswertigkeit,
          (object) QnennMessfehler,
          (object) QtrennMessfehler,
          (object) QminMessfehler,
          (object) Impulswertigkeit,
          (object) QnennFehler,
          (object) QtrennFehler,
          (object) QminFehler,
          (object) InZaehlerNr
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable pos2008DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable) base.Clone();
        pos2008DataTable.InitVars();
        return (DataTable) pos2008DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnChargenNr = this.Columns["ChargenNr"];
        this.columnChargenIndex = this.Columns["ChargenIndex"];
        this.columnSerienNr = this.Columns["SerienNr"];
        this.columnSerienNrZaehler = this.Columns["SerienNrZaehler"];
        this.columnLSNr = this.Columns["LSNr"];
        this.columnTermin = this.Columns["Termin"];
        this.columnGeraetestatus = this.Columns["Geraetestatus"];
        this.columnBasisImpulswertigkeit = this.Columns["BasisImpulswertigkeit"];
        this.columnQnennMessfehler = this.Columns["QnennMessfehler"];
        this.columnQtrennMessfehler = this.Columns["QtrennMessfehler"];
        this.columnQminMessfehler = this.Columns["QminMessfehler"];
        this.columnImpulswertigkeit = this.Columns["Impulswertigkeit"];
        this.columnQnennFehler = this.Columns["QnennFehler"];
        this.columnQtrennFehler = this.Columns["QtrennFehler"];
        this.columnQminFehler = this.Columns["QminFehler"];
        this.columnInZaehlerNr = this.Columns["InZaehlerNr"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnChargenNr = new DataColumn("ChargenNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr);
        this.columnChargenIndex = new DataColumn("ChargenIndex", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenIndex);
        this.columnSerienNr = new DataColumn("SerienNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNr);
        this.columnSerienNrZaehler = new DataColumn("SerienNrZaehler", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNrZaehler);
        this.columnLSNr = new DataColumn("LSNr", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLSNr);
        this.columnTermin = new DataColumn("Termin", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTermin);
        this.columnGeraetestatus = new DataColumn("Geraetestatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGeraetestatus);
        this.columnBasisImpulswertigkeit = new DataColumn("BasisImpulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBasisImpulswertigkeit);
        this.columnQnennMessfehler = new DataColumn("QnennMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennMessfehler);
        this.columnQtrennMessfehler = new DataColumn("QtrennMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennMessfehler);
        this.columnQminMessfehler = new DataColumn("QminMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminMessfehler);
        this.columnImpulswertigkeit = new DataColumn("Impulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnImpulswertigkeit);
        this.columnQnennFehler = new DataColumn("QnennFehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennFehler);
        this.columnQtrennFehler = new DataColumn("QtrennFehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennFehler);
        this.columnQminFehler = new DataColumn("QminFehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminFehler);
        this.columnInZaehlerNr = new DataColumn("InZaehlerNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInZaehlerNr);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnChargenNr
        }, false));
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[1]
        {
          this.columnChargenIndex
        }, false));
        this.columnChargenNr.Unique = true;
        this.columnChargenNr.MaxLength = 10;
        this.columnChargenIndex.Unique = true;
        this.columnSerienNr.MaxLength = 9;
        this.columnInZaehlerNr.MaxLength = 8;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row NewHydPos_2008Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydPos_2008RowChanged == null)
          return;
        this.HydPos_2008RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydPos_2008RowChanging == null)
          return;
        this.HydPos_2008RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydPos_2008RowDeleted == null)
          return;
        this.HydPos_2008RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydPos_2008RowDeleting == null)
          return;
        this.HydPos_2008RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydPos_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydPos_2008DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class HydSNrListeDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow>
    {
      private DataColumn columnSerienNr;
      private DataColumn columnSerienNrZaehler;
      private DataColumn columnChargenNr;
      private DataColumn columnChargenIndex;
      private DataColumn columnTermin;
      private DataColumn columnGeraetestatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydSNrListeDataTable()
      {
        this.TableName = "HydSNrListe";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydSNrListeDataTable(DataTable table)
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
      protected HydSNrListeDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNrColumn => this.columnSerienNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNrZaehlerColumn => this.columnSerienNrZaehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNrColumn => this.columnChargenNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenIndexColumn => this.columnChargenIndex;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TerminColumn => this.columnTermin;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GeraetestatusColumn => this.columnGeraetestatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEventHandler HydSNrListeRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEventHandler HydSNrListeRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEventHandler HydSNrListeRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEventHandler HydSNrListeRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddHydSNrListeRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow AddHydSNrListeRow(
        string SerienNr,
        short SerienNrZaehler,
        string ChargenNr,
        short ChargenIndex,
        DateTime Termin,
        short Geraetestatus)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) this.NewRow();
        object[] objArray = new object[6]
        {
          (object) SerienNr,
          (object) SerienNrZaehler,
          (object) ChargenNr,
          (object) ChargenIndex,
          (object) Termin,
          (object) Geraetestatus
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable snrListeDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable) base.Clone();
        snrListeDataTable.InitVars();
        return (DataTable) snrListeDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnSerienNr = this.Columns["SerienNr"];
        this.columnSerienNrZaehler = this.Columns["SerienNrZaehler"];
        this.columnChargenNr = this.Columns["ChargenNr"];
        this.columnChargenIndex = this.Columns["ChargenIndex"];
        this.columnTermin = this.Columns["Termin"];
        this.columnGeraetestatus = this.Columns["Geraetestatus"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnSerienNr = new DataColumn("SerienNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNr);
        this.columnSerienNrZaehler = new DataColumn("SerienNrZaehler", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNrZaehler);
        this.columnChargenNr = new DataColumn("ChargenNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr);
        this.columnChargenIndex = new DataColumn("ChargenIndex", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenIndex);
        this.columnTermin = new DataColumn("Termin", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTermin);
        this.columnGeraetestatus = new DataColumn("Geraetestatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGeraetestatus);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnSerienNr
        }, false));
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[1]
        {
          this.columnSerienNrZaehler
        }, false));
        this.columnSerienNr.Unique = true;
        this.columnSerienNr.MaxLength = 9;
        this.columnSerienNrZaehler.Unique = true;
        this.columnChargenNr.MaxLength = 10;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow NewHydSNrListeRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HydSNrListeRowChanged == null)
          return;
        this.HydSNrListeRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HydSNrListeRowChanging == null)
          return;
        this.HydSNrListeRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HydSNrListeRowDeleted == null)
          return;
        this.HydSNrListeRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HydSNrListeRowDeleting == null)
          return;
        this.HydSNrListeRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveHydSNrListeRow(ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HydSNrListeDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class MIDKalibrierDatenDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnPruefstand;
      private DataColumn columnPruefer;
      private DataColumn columnKalibrierdatum;
      private DataColumn columnTemperatur;
      private DataColumn columnMID1_Durchfluss;
      private DataColumn columnMID1_Impulse;
      private DataColumn columnMID1_Temperatur;
      private DataColumn columnMID1_Gewicht;
      private DataColumn columnMID2_Durchfluss;
      private DataColumn columnMID2_Impulse;
      private DataColumn columnMID2_Temperatur;
      private DataColumn columnMID2_Gewicht;
      private DataColumn columnMID3_Durchfluss;
      private DataColumn columnMID3_Impulse;
      private DataColumn columnMID3_Temperatur;
      private DataColumn columnMID3_Gewicht;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MIDKalibrierDatenDataTable()
      {
        this.TableName = "MIDKalibrierDaten";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MIDKalibrierDatenDataTable(DataTable table)
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
      protected MIDKalibrierDatenDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PruefstandColumn => this.columnPruefstand;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PrueferColumn => this.columnPruefer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn KalibrierdatumColumn => this.columnKalibrierdatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TemperaturColumn => this.columnTemperatur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID1_DurchflussColumn => this.columnMID1_Durchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID1_ImpulseColumn => this.columnMID1_Impulse;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID1_TemperaturColumn => this.columnMID1_Temperatur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID1_GewichtColumn => this.columnMID1_Gewicht;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID2_DurchflussColumn => this.columnMID2_Durchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID2_ImpulseColumn => this.columnMID2_Impulse;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID2_TemperaturColumn => this.columnMID2_Temperatur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID2_GewichtColumn => this.columnMID2_Gewicht;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID3_DurchflussColumn => this.columnMID3_Durchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID3_ImpulseColumn => this.columnMID3_Impulse;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID3_TemperaturColumn => this.columnMID3_Temperatur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MID3_GewichtColumn => this.columnMID3_Gewicht;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEventHandler MIDKalibrierDatenRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEventHandler MIDKalibrierDatenRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEventHandler MIDKalibrierDatenRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEventHandler MIDKalibrierDatenRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddMIDKalibrierDatenRow(ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow AddMIDKalibrierDatenRow(
        int RecID,
        string Pruefstand,
        string Pruefer,
        DateTime Kalibrierdatum,
        double Temperatur,
        double MID1_Durchfluss,
        double MID1_Impulse,
        double MID1_Temperatur,
        double MID1_Gewicht,
        double MID2_Durchfluss,
        double MID2_Impulse,
        double MID2_Temperatur,
        double MID2_Gewicht,
        double MID3_Durchfluss,
        double MID3_Impulse,
        double MID3_Temperatur,
        double MID3_Gewicht)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) this.NewRow();
        object[] objArray = new object[17]
        {
          (object) RecID,
          (object) Pruefstand,
          (object) Pruefer,
          (object) Kalibrierdatum,
          (object) Temperatur,
          (object) MID1_Durchfluss,
          (object) MID1_Impulse,
          (object) MID1_Temperatur,
          (object) MID1_Gewicht,
          (object) MID2_Durchfluss,
          (object) MID2_Impulse,
          (object) MID2_Temperatur,
          (object) MID2_Gewicht,
          (object) MID3_Durchfluss,
          (object) MID3_Impulse,
          (object) MID3_Temperatur,
          (object) MID3_Gewicht
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable kalibrierDatenDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable) base.Clone();
        kalibrierDatenDataTable.InitVars();
        return (DataTable) kalibrierDatenDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnPruefstand = this.Columns["Pruefstand"];
        this.columnPruefer = this.Columns["Pruefer"];
        this.columnKalibrierdatum = this.Columns["Kalibrierdatum"];
        this.columnTemperatur = this.Columns["Temperatur"];
        this.columnMID1_Durchfluss = this.Columns["MID1_Durchfluss"];
        this.columnMID1_Impulse = this.Columns["MID1_Impulse"];
        this.columnMID1_Temperatur = this.Columns["MID1_Temperatur"];
        this.columnMID1_Gewicht = this.Columns["MID1_Gewicht"];
        this.columnMID2_Durchfluss = this.Columns["MID2_Durchfluss"];
        this.columnMID2_Impulse = this.Columns["MID2_Impulse"];
        this.columnMID2_Temperatur = this.Columns["MID2_Temperatur"];
        this.columnMID2_Gewicht = this.Columns["MID2_Gewicht"];
        this.columnMID3_Durchfluss = this.Columns["MID3_Durchfluss"];
        this.columnMID3_Impulse = this.Columns["MID3_Impulse"];
        this.columnMID3_Temperatur = this.Columns["MID3_Temperatur"];
        this.columnMID3_Gewicht = this.Columns["MID3_Gewicht"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnPruefstand = new DataColumn("Pruefstand", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefstand);
        this.columnPruefer = new DataColumn("Pruefer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefer);
        this.columnKalibrierdatum = new DataColumn("Kalibrierdatum", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnKalibrierdatum);
        this.columnTemperatur = new DataColumn("Temperatur", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTemperatur);
        this.columnMID1_Durchfluss = new DataColumn("MID1_Durchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID1_Durchfluss);
        this.columnMID1_Impulse = new DataColumn("MID1_Impulse", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID1_Impulse);
        this.columnMID1_Temperatur = new DataColumn("MID1_Temperatur", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID1_Temperatur);
        this.columnMID1_Gewicht = new DataColumn("MID1_Gewicht", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID1_Gewicht);
        this.columnMID2_Durchfluss = new DataColumn("MID2_Durchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID2_Durchfluss);
        this.columnMID2_Impulse = new DataColumn("MID2_Impulse", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID2_Impulse);
        this.columnMID2_Temperatur = new DataColumn("MID2_Temperatur", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID2_Temperatur);
        this.columnMID2_Gewicht = new DataColumn("MID2_Gewicht", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID2_Gewicht);
        this.columnMID3_Durchfluss = new DataColumn("MID3_Durchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID3_Durchfluss);
        this.columnMID3_Impulse = new DataColumn("MID3_Impulse", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID3_Impulse);
        this.columnMID3_Temperatur = new DataColumn("MID3_Temperatur", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID3_Temperatur);
        this.columnMID3_Gewicht = new DataColumn("MID3_Gewicht", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMID3_Gewicht);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
        this.columnPruefstand.MaxLength = 10;
        this.columnPruefer.MaxLength = 10;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow NewMIDKalibrierDatenRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MIDKalibrierDatenRowChanged == null)
          return;
        this.MIDKalibrierDatenRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MIDKalibrierDatenRowChanging == null)
          return;
        this.MIDKalibrierDatenRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MIDKalibrierDatenRowDeleted == null)
          return;
        this.MIDKalibrierDatenRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MIDKalibrierDatenRowDeleting == null)
          return;
        this.MIDKalibrierDatenRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveMIDKalibrierDatenRow(ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MIDKalibrierDatenDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class RechenwerkTypDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow>
    {
      private DataColumn columnTypID;
      private DataColumn columnHersteller;
      private DataColumn columnArtikelnummer;
      private DataColumn columnSAPName;
      private DataColumn columnEngelmannTypName;
      private DataColumn columnEngelmannArtikelnummer;
      private DataColumn columnBauartenzulassung;
      private DataColumn columnEinbaulage;
      private DataColumn columnMetrologischeKlasse;
      private DataColumn columnNenndurchfluss;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RechenwerkTypDataTable()
      {
        this.TableName = "RechenwerkTyp";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RechenwerkTypDataTable(DataTable table)
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
      protected RechenwerkTypDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TypIDColumn => this.columnTypID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HerstellerColumn => this.columnHersteller;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ArtikelnummerColumn => this.columnArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SAPNameColumn => this.columnSAPName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EngelmannTypNameColumn => this.columnEngelmannTypName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EngelmannArtikelnummerColumn => this.columnEngelmannArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BauartenzulassungColumn => this.columnBauartenzulassung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EinbaulageColumn => this.columnEinbaulage;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MetrologischeKlasseColumn => this.columnMetrologischeKlasse;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn NenndurchflussColumn => this.columnNenndurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEventHandler RechenwerkTypRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEventHandler RechenwerkTypRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEventHandler RechenwerkTypRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEventHandler RechenwerkTypRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddRechenwerkTypRow(ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow AddRechenwerkTypRow(
        int TypID,
        string Hersteller,
        string Artikelnummer,
        string SAPName,
        string EngelmannTypName,
        string EngelmannArtikelnummer,
        string Bauartenzulassung,
        string Einbaulage,
        string MetrologischeKlasse,
        double Nenndurchfluss,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) this.NewRow();
        object[] objArray = new object[11]
        {
          (object) TypID,
          (object) Hersteller,
          (object) Artikelnummer,
          (object) SAPName,
          (object) EngelmannTypName,
          (object) EngelmannArtikelnummer,
          (object) Bauartenzulassung,
          (object) Einbaulage,
          (object) MetrologischeKlasse,
          (object) Nenndurchfluss,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable rechenwerkTypDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable) base.Clone();
        rechenwerkTypDataTable.InitVars();
        return (DataTable) rechenwerkTypDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnTypID = this.Columns["TypID"];
        this.columnHersteller = this.Columns["Hersteller"];
        this.columnArtikelnummer = this.Columns["Artikelnummer"];
        this.columnSAPName = this.Columns["SAPName"];
        this.columnEngelmannTypName = this.Columns["EngelmannTypName"];
        this.columnEngelmannArtikelnummer = this.Columns["EngelmannArtikelnummer"];
        this.columnBauartenzulassung = this.Columns["Bauartenzulassung"];
        this.columnEinbaulage = this.Columns["Einbaulage"];
        this.columnMetrologischeKlasse = this.Columns["MetrologischeKlasse"];
        this.columnNenndurchfluss = this.Columns["Nenndurchfluss"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnTypID = new DataColumn("TypID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTypID);
        this.columnHersteller = new DataColumn("Hersteller", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHersteller);
        this.columnArtikelnummer = new DataColumn("Artikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnArtikelnummer);
        this.columnSAPName = new DataColumn("SAPName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSAPName);
        this.columnEngelmannTypName = new DataColumn("EngelmannTypName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEngelmannTypName);
        this.columnEngelmannArtikelnummer = new DataColumn("EngelmannArtikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEngelmannArtikelnummer);
        this.columnBauartenzulassung = new DataColumn("Bauartenzulassung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBauartenzulassung);
        this.columnEinbaulage = new DataColumn("Einbaulage", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEinbaulage);
        this.columnMetrologischeKlasse = new DataColumn("MetrologischeKlasse", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMetrologischeKlasse);
        this.columnNenndurchfluss = new DataColumn("Nenndurchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnNenndurchfluss);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnTypID
        }, false));
        this.columnTypID.Unique = true;
        this.columnHersteller.MaxLength = 30;
        this.columnArtikelnummer.MaxLength = 20;
        this.columnSAPName.MaxLength = 50;
        this.columnEngelmannTypName.MaxLength = 20;
        this.columnEngelmannArtikelnummer.MaxLength = 20;
        this.columnBauartenzulassung.MaxLength = 50;
        this.columnEinbaulage.MaxLength = 5;
        this.columnMetrologischeKlasse.MaxLength = 5;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow NewRechenwerkTypRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.RechenwerkTypRowChanged == null)
          return;
        this.RechenwerkTypRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.RechenwerkTypRowChanging == null)
          return;
        this.RechenwerkTypRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.RechenwerkTypRowDeleted == null)
          return;
        this.RechenwerkTypRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.RechenwerkTypRowDeleting == null)
          return;
        this.RechenwerkTypRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveRechenwerkTypRow(ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (RechenwerkTypDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class RechenwerkHerstDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnBezeichnung;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RechenwerkHerstDataTable()
      {
        this.TableName = "RechenwerkHerst";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RechenwerkHerstDataTable(DataTable table)
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
      protected RechenwerkHerstDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BezeichnungColumn => this.columnBezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEventHandler RechenwerkHerstRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEventHandler RechenwerkHerstRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEventHandler RechenwerkHerstRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEventHandler RechenwerkHerstRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddRechenwerkHerstRow(ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow AddRechenwerkHerstRow(
        int RecID,
        string Bezeichnung,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) this.NewRow();
        object[] objArray = new object[3]
        {
          (object) RecID,
          (object) Bezeichnung,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable rechenwerkHerstDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable) base.Clone();
        rechenwerkHerstDataTable.InitVars();
        return (DataTable) rechenwerkHerstDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnBezeichnung = this.Columns["Bezeichnung"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnBezeichnung = new DataColumn("Bezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBezeichnung);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
        this.columnBezeichnung.MaxLength = 30;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow NewRechenwerkHerstRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.RechenwerkHerstRowChanged == null)
          return;
        this.RechenwerkHerstRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.RechenwerkHerstRowChanging == null)
          return;
        this.RechenwerkHerstRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.RechenwerkHerstRowDeleted == null)
          return;
        this.RechenwerkHerstRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.RechenwerkHerstRowDeleting == null)
          return;
        this.RechenwerkHerstRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveRechenwerkHerstRow(ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (RechenwerkHerstDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class RecBel_2008DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row>
    {
      private DataColumn columnChargenNr;
      private DataColumn columnZaehlerTyp;
      private DataColumn columnArtikelnummer;
      private DataColumn columnSAPName;
      private DataColumn columnBelegstatus;
      private DataColumn columnAuftragsnummer;
      private DataColumn columnBauartenzulassung;
      private DataColumn columnMetrologischeKlasse;
      private DataColumn columnPrueftemperatur;
      private DataColumn columnNenndurchfluss;
      private DataColumn columnEngelmannTypName;
      private DataColumn columnEngelmannArtikelnummer;
      private DataColumn columnPruefer;
      private DataColumn columnPruefstellenleitung;
      private DataColumn columnBemerkung;
      private DataColumn columnPruefdatum;
      private DataColumn columnDatenuebernahmedatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RecBel_2008DataTable()
      {
        this.TableName = "RecBel_2008";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RecBel_2008DataTable(DataTable table)
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
      protected RecBel_2008DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNrColumn => this.columnChargenNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ZaehlerTypColumn => this.columnZaehlerTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ArtikelnummerColumn => this.columnArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SAPNameColumn => this.columnSAPName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BelegstatusColumn => this.columnBelegstatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AuftragsnummerColumn => this.columnAuftragsnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BauartenzulassungColumn => this.columnBauartenzulassung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MetrologischeKlasseColumn => this.columnMetrologischeKlasse;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PrueftemperaturColumn => this.columnPrueftemperatur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn NenndurchflussColumn => this.columnNenndurchfluss;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EngelmannTypNameColumn => this.columnEngelmannTypName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EngelmannArtikelnummerColumn => this.columnEngelmannArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PrueferColumn => this.columnPruefer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PruefstellenleitungColumn => this.columnPruefstellenleitung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BemerkungColumn => this.columnBemerkung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PruefdatumColumn => this.columnPruefdatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DatenuebernahmedatumColumn => this.columnDatenuebernahmedatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEventHandler RecBel_2008RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEventHandler RecBel_2008RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEventHandler RecBel_2008RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEventHandler RecBel_2008RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddRecBel_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row AddRecBel_2008Row(
        string ChargenNr,
        int ZaehlerTyp,
        string Artikelnummer,
        string SAPName,
        short Belegstatus,
        string Auftragsnummer,
        string Bauartenzulassung,
        string MetrologischeKlasse,
        double Prueftemperatur,
        double Nenndurchfluss,
        string EngelmannTypName,
        string EngelmannArtikelnummer,
        string Pruefer,
        string Pruefstellenleitung,
        string Bemerkung,
        DateTime Pruefdatum,
        DateTime Datenuebernahmedatum)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) this.NewRow();
        object[] objArray = new object[17]
        {
          (object) ChargenNr,
          (object) ZaehlerTyp,
          (object) Artikelnummer,
          (object) SAPName,
          (object) Belegstatus,
          (object) Auftragsnummer,
          (object) Bauartenzulassung,
          (object) MetrologischeKlasse,
          (object) Prueftemperatur,
          (object) Nenndurchfluss,
          (object) EngelmannTypName,
          (object) EngelmannArtikelnummer,
          (object) Pruefer,
          (object) Pruefstellenleitung,
          (object) Bemerkung,
          (object) Pruefdatum,
          (object) Datenuebernahmedatum
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable bel2008DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable) base.Clone();
        bel2008DataTable.InitVars();
        return (DataTable) bel2008DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnChargenNr = this.Columns["ChargenNr"];
        this.columnZaehlerTyp = this.Columns["ZaehlerTyp"];
        this.columnArtikelnummer = this.Columns["Artikelnummer"];
        this.columnSAPName = this.Columns["SAPName"];
        this.columnBelegstatus = this.Columns["Belegstatus"];
        this.columnAuftragsnummer = this.Columns["Auftragsnummer"];
        this.columnBauartenzulassung = this.Columns["Bauartenzulassung"];
        this.columnMetrologischeKlasse = this.Columns["MetrologischeKlasse"];
        this.columnPrueftemperatur = this.Columns["Prueftemperatur"];
        this.columnNenndurchfluss = this.Columns["Nenndurchfluss"];
        this.columnEngelmannTypName = this.Columns["EngelmannTypName"];
        this.columnEngelmannArtikelnummer = this.Columns["EngelmannArtikelnummer"];
        this.columnPruefer = this.Columns["Pruefer"];
        this.columnPruefstellenleitung = this.Columns["Pruefstellenleitung"];
        this.columnBemerkung = this.Columns["Bemerkung"];
        this.columnPruefdatum = this.Columns["Pruefdatum"];
        this.columnDatenuebernahmedatum = this.Columns["Datenuebernahmedatum"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnChargenNr = new DataColumn("ChargenNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr);
        this.columnZaehlerTyp = new DataColumn("ZaehlerTyp", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZaehlerTyp);
        this.columnArtikelnummer = new DataColumn("Artikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnArtikelnummer);
        this.columnSAPName = new DataColumn("SAPName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSAPName);
        this.columnBelegstatus = new DataColumn("Belegstatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBelegstatus);
        this.columnAuftragsnummer = new DataColumn("Auftragsnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAuftragsnummer);
        this.columnBauartenzulassung = new DataColumn("Bauartenzulassung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBauartenzulassung);
        this.columnMetrologischeKlasse = new DataColumn("MetrologischeKlasse", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMetrologischeKlasse);
        this.columnPrueftemperatur = new DataColumn("Prueftemperatur", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPrueftemperatur);
        this.columnNenndurchfluss = new DataColumn("Nenndurchfluss", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnNenndurchfluss);
        this.columnEngelmannTypName = new DataColumn("EngelmannTypName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEngelmannTypName);
        this.columnEngelmannArtikelnummer = new DataColumn("EngelmannArtikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEngelmannArtikelnummer);
        this.columnPruefer = new DataColumn("Pruefer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefer);
        this.columnPruefstellenleitung = new DataColumn("Pruefstellenleitung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefstellenleitung);
        this.columnBemerkung = new DataColumn("Bemerkung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBemerkung);
        this.columnPruefdatum = new DataColumn("Pruefdatum", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefdatum);
        this.columnDatenuebernahmedatum = new DataColumn("Datenuebernahmedatum", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDatenuebernahmedatum);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnChargenNr
        }, false));
        this.columnChargenNr.Unique = true;
        this.columnChargenNr.MaxLength = 10;
        this.columnArtikelnummer.MaxLength = 20;
        this.columnSAPName.MaxLength = 50;
        this.columnAuftragsnummer.MaxLength = 10;
        this.columnBauartenzulassung.MaxLength = 20;
        this.columnMetrologischeKlasse.MaxLength = 1;
        this.columnEngelmannTypName.MaxLength = 20;
        this.columnEngelmannArtikelnummer.MaxLength = 20;
        this.columnPruefer.MaxLength = 20;
        this.columnPruefstellenleitung.MaxLength = 20;
        this.columnBemerkung.MaxLength = 5;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row NewRecBel_2008Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.RecBel_2008RowChanged == null)
          return;
        this.RecBel_2008RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.RecBel_2008RowChanging == null)
          return;
        this.RecBel_2008RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.RecBel_2008RowDeleted == null)
          return;
        this.RecBel_2008RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.RecBel_2008RowDeleting == null)
          return;
        this.RecBel_2008RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveRecBel_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (RecBel_2008DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class RecPos_2008DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row>
    {
      private DataColumn columnChargenNr;
      private DataColumn columnChargenIndex;
      private DataColumn columnSerienNr;
      private DataColumn columnGeraetestatus;
      private DataColumn columnQdTminVorlaufbadtemp;
      private DataColumn columnQdTminRuecklaufbadtemp;
      private DataColumn columnQdTminKFaktor;
      private DataColumn columnQdTminSollwert;
      private DataColumn columnQdTminIstwert;
      private DataColumn columnQdTminMessfehler;
      private DataColumn columnQdTnennVorlaufbadtemp;
      private DataColumn columnQdTnennRuecklaufbadtemp;
      private DataColumn columnQdTnennKFaktor;
      private DataColumn columnQdTnennSollwert;
      private DataColumn columnQdTnennIstwert;
      private DataColumn columnQdTnennMessfehler;
      private DataColumn columnQdTmaxVorlaufbadtemp;
      private DataColumn columnQdTmaxRuecklaufbadtemp;
      private DataColumn columnQdTmaxKFaktor;
      private DataColumn columnQdTmaxSollwert;
      private DataColumn columnQdTmaxIstwert;
      private DataColumn columnQdTmaxMessfehler;
      private DataColumn columnQdRZWVorlaufbadtemp;
      private DataColumn columnQdRZWRuecklaufbadtemp;
      private DataColumn columnQdRZWKFaktor;
      private DataColumn columnQdRZWSollwert;
      private DataColumn columnQdRZWIstwert;
      private DataColumn columnQdRZWMessfehler;
      private DataColumn columnInZaehlerNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RecPos_2008DataTable()
      {
        this.TableName = "RecPos_2008";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RecPos_2008DataTable(DataTable table)
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
      protected RecPos_2008DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNrColumn => this.columnChargenNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenIndexColumn => this.columnChargenIndex;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNrColumn => this.columnSerienNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GeraetestatusColumn => this.columnGeraetestatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminVorlaufbadtempColumn => this.columnQdTminVorlaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminRuecklaufbadtempColumn => this.columnQdTminRuecklaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminKFaktorColumn => this.columnQdTminKFaktor;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminSollwertColumn => this.columnQdTminSollwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminIstwertColumn => this.columnQdTminIstwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminMessfehlerColumn => this.columnQdTminMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennVorlaufbadtempColumn => this.columnQdTnennVorlaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennRuecklaufbadtempColumn => this.columnQdTnennRuecklaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennKFaktorColumn => this.columnQdTnennKFaktor;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennSollwertColumn => this.columnQdTnennSollwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennIstwertColumn => this.columnQdTnennIstwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennMessfehlerColumn => this.columnQdTnennMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxVorlaufbadtempColumn => this.columnQdTmaxVorlaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxRuecklaufbadtempColumn => this.columnQdTmaxRuecklaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxKFaktorColumn => this.columnQdTmaxKFaktor;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxSollwertColumn => this.columnQdTmaxSollwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxIstwertColumn => this.columnQdTmaxIstwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxMessfehlerColumn => this.columnQdTmaxMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdRZWVorlaufbadtempColumn => this.columnQdRZWVorlaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdRZWRuecklaufbadtempColumn => this.columnQdRZWRuecklaufbadtemp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdRZWKFaktorColumn => this.columnQdRZWKFaktor;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdRZWSollwertColumn => this.columnQdRZWSollwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdRZWIstwertColumn => this.columnQdRZWIstwert;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdRZWMessfehlerColumn => this.columnQdRZWMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InZaehlerNrColumn => this.columnInZaehlerNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEventHandler RecPos_2008RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEventHandler RecPos_2008RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEventHandler RecPos_2008RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEventHandler RecPos_2008RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddRecPos_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row AddRecPos_2008Row(
        string ChargenNr,
        short ChargenIndex,
        string SerienNr,
        short Geraetestatus,
        double QdTminVorlaufbadtemp,
        double QdTminRuecklaufbadtemp,
        double QdTminKFaktor,
        double QdTminSollwert,
        double QdTminIstwert,
        double QdTminMessfehler,
        double QdTnennVorlaufbadtemp,
        double QdTnennRuecklaufbadtemp,
        double QdTnennKFaktor,
        double QdTnennSollwert,
        double QdTnennIstwert,
        double QdTnennMessfehler,
        double QdTmaxVorlaufbadtemp,
        double QdTmaxRuecklaufbadtemp,
        double QdTmaxKFaktor,
        double QdTmaxSollwert,
        double QdTmaxIstwert,
        double QdTmaxMessfehler,
        double QdRZWVorlaufbadtemp,
        double QdRZWRuecklaufbadtemp,
        double QdRZWKFaktor,
        double QdRZWSollwert,
        double QdRZWIstwert,
        double QdRZWMessfehler,
        string InZaehlerNr)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) this.NewRow();
        object[] objArray = new object[29]
        {
          (object) ChargenNr,
          (object) ChargenIndex,
          (object) SerienNr,
          (object) Geraetestatus,
          (object) QdTminVorlaufbadtemp,
          (object) QdTminRuecklaufbadtemp,
          (object) QdTminKFaktor,
          (object) QdTminSollwert,
          (object) QdTminIstwert,
          (object) QdTminMessfehler,
          (object) QdTnennVorlaufbadtemp,
          (object) QdTnennRuecklaufbadtemp,
          (object) QdTnennKFaktor,
          (object) QdTnennSollwert,
          (object) QdTnennIstwert,
          (object) QdTnennMessfehler,
          (object) QdTmaxVorlaufbadtemp,
          (object) QdTmaxRuecklaufbadtemp,
          (object) QdTmaxKFaktor,
          (object) QdTmaxSollwert,
          (object) QdTmaxIstwert,
          (object) QdTmaxMessfehler,
          (object) QdRZWVorlaufbadtemp,
          (object) QdRZWRuecklaufbadtemp,
          (object) QdRZWKFaktor,
          (object) QdRZWSollwert,
          (object) QdRZWIstwert,
          (object) QdRZWMessfehler,
          (object) InZaehlerNr
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable pos2008DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable) base.Clone();
        pos2008DataTable.InitVars();
        return (DataTable) pos2008DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnChargenNr = this.Columns["ChargenNr"];
        this.columnChargenIndex = this.Columns["ChargenIndex"];
        this.columnSerienNr = this.Columns["SerienNr"];
        this.columnGeraetestatus = this.Columns["Geraetestatus"];
        this.columnQdTminVorlaufbadtemp = this.Columns["QdTminVorlaufbadtemp"];
        this.columnQdTminRuecklaufbadtemp = this.Columns["QdTminRuecklaufbadtemp"];
        this.columnQdTminKFaktor = this.Columns["QdTminKFaktor"];
        this.columnQdTminSollwert = this.Columns["QdTminSollwert"];
        this.columnQdTminIstwert = this.Columns["QdTminIstwert"];
        this.columnQdTminMessfehler = this.Columns["QdTminMessfehler"];
        this.columnQdTnennVorlaufbadtemp = this.Columns["QdTnennVorlaufbadtemp"];
        this.columnQdTnennRuecklaufbadtemp = this.Columns["QdTnennRuecklaufbadtemp"];
        this.columnQdTnennKFaktor = this.Columns["QdTnennKFaktor"];
        this.columnQdTnennSollwert = this.Columns["QdTnennSollwert"];
        this.columnQdTnennIstwert = this.Columns["QdTnennIstwert"];
        this.columnQdTnennMessfehler = this.Columns["QdTnennMessfehler"];
        this.columnQdTmaxVorlaufbadtemp = this.Columns["QdTmaxVorlaufbadtemp"];
        this.columnQdTmaxRuecklaufbadtemp = this.Columns["QdTmaxRuecklaufbadtemp"];
        this.columnQdTmaxKFaktor = this.Columns["QdTmaxKFaktor"];
        this.columnQdTmaxSollwert = this.Columns["QdTmaxSollwert"];
        this.columnQdTmaxIstwert = this.Columns["QdTmaxIstwert"];
        this.columnQdTmaxMessfehler = this.Columns["QdTmaxMessfehler"];
        this.columnQdRZWVorlaufbadtemp = this.Columns["QdRZWVorlaufbadtemp"];
        this.columnQdRZWRuecklaufbadtemp = this.Columns["QdRZWRuecklaufbadtemp"];
        this.columnQdRZWKFaktor = this.Columns["QdRZWKFaktor"];
        this.columnQdRZWSollwert = this.Columns["QdRZWSollwert"];
        this.columnQdRZWIstwert = this.Columns["QdRZWIstwert"];
        this.columnQdRZWMessfehler = this.Columns["QdRZWMessfehler"];
        this.columnInZaehlerNr = this.Columns["InZaehlerNr"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnChargenNr = new DataColumn("ChargenNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr);
        this.columnChargenIndex = new DataColumn("ChargenIndex", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenIndex);
        this.columnSerienNr = new DataColumn("SerienNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNr);
        this.columnGeraetestatus = new DataColumn("Geraetestatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGeraetestatus);
        this.columnQdTminVorlaufbadtemp = new DataColumn("QdTminVorlaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminVorlaufbadtemp);
        this.columnQdTminRuecklaufbadtemp = new DataColumn("QdTminRuecklaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminRuecklaufbadtemp);
        this.columnQdTminKFaktor = new DataColumn("QdTminKFaktor", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminKFaktor);
        this.columnQdTminSollwert = new DataColumn("QdTminSollwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminSollwert);
        this.columnQdTminIstwert = new DataColumn("QdTminIstwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminIstwert);
        this.columnQdTminMessfehler = new DataColumn("QdTminMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminMessfehler);
        this.columnQdTnennVorlaufbadtemp = new DataColumn("QdTnennVorlaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennVorlaufbadtemp);
        this.columnQdTnennRuecklaufbadtemp = new DataColumn("QdTnennRuecklaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennRuecklaufbadtemp);
        this.columnQdTnennKFaktor = new DataColumn("QdTnennKFaktor", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennKFaktor);
        this.columnQdTnennSollwert = new DataColumn("QdTnennSollwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennSollwert);
        this.columnQdTnennIstwert = new DataColumn("QdTnennIstwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennIstwert);
        this.columnQdTnennMessfehler = new DataColumn("QdTnennMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennMessfehler);
        this.columnQdTmaxVorlaufbadtemp = new DataColumn("QdTmaxVorlaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxVorlaufbadtemp);
        this.columnQdTmaxRuecklaufbadtemp = new DataColumn("QdTmaxRuecklaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxRuecklaufbadtemp);
        this.columnQdTmaxKFaktor = new DataColumn("QdTmaxKFaktor", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxKFaktor);
        this.columnQdTmaxSollwert = new DataColumn("QdTmaxSollwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxSollwert);
        this.columnQdTmaxIstwert = new DataColumn("QdTmaxIstwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxIstwert);
        this.columnQdTmaxMessfehler = new DataColumn("QdTmaxMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxMessfehler);
        this.columnQdRZWVorlaufbadtemp = new DataColumn("QdRZWVorlaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdRZWVorlaufbadtemp);
        this.columnQdRZWRuecklaufbadtemp = new DataColumn("QdRZWRuecklaufbadtemp", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdRZWRuecklaufbadtemp);
        this.columnQdRZWKFaktor = new DataColumn("QdRZWKFaktor", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdRZWKFaktor);
        this.columnQdRZWSollwert = new DataColumn("QdRZWSollwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdRZWSollwert);
        this.columnQdRZWIstwert = new DataColumn("QdRZWIstwert", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdRZWIstwert);
        this.columnQdRZWMessfehler = new DataColumn("QdRZWMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdRZWMessfehler);
        this.columnInZaehlerNr = new DataColumn("InZaehlerNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInZaehlerNr);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnChargenNr
        }, false));
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[1]
        {
          this.columnChargenIndex
        }, false));
        this.columnChargenNr.Unique = true;
        this.columnChargenNr.MaxLength = 10;
        this.columnChargenIndex.Unique = true;
        this.columnSerienNr.MaxLength = 8;
        this.columnInZaehlerNr.MaxLength = 8;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row NewRecPos_2008Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.RecPos_2008RowChanged == null)
          return;
        this.RecPos_2008RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.RecPos_2008RowChanging == null)
          return;
        this.RecPos_2008RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.RecPos_2008RowDeleted == null)
          return;
        this.RecPos_2008RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.RecPos_2008RowDeleting == null)
          return;
        this.RecPos_2008RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveRecPos_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (RecPos_2008DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class TypNamenReportDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow>
    {
      private DataColumn columnTYP_ID;
      private DataColumn columnBerichtName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TypNamenReportDataTable()
      {
        this.TableName = "TypNamenReport";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal TypNamenReportDataTable(DataTable table)
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
      protected TypNamenReportDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TYP_IDColumn => this.columnTYP_ID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BerichtNameColumn => this.columnBerichtName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEventHandler TypNamenReportRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEventHandler TypNamenReportRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEventHandler TypNamenReportRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEventHandler TypNamenReportRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddTypNamenReportRow(ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow AddTypNamenReportRow(
        short TYP_ID,
        string BerichtName)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) TYP_ID,
          (object) BerichtName
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable namenReportDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable) base.Clone();
        namenReportDataTable.InitVars();
        return (DataTable) namenReportDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnTYP_ID = this.Columns["TYP_ID"];
        this.columnBerichtName = this.Columns["BerichtName"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnTYP_ID = new DataColumn("TYP_ID", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTYP_ID);
        this.columnBerichtName = new DataColumn("BerichtName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBerichtName);
        this.columnBerichtName.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow NewTypNamenReportRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.TypNamenReportRowChanged == null)
          return;
        this.TypNamenReportRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.TypNamenReportRowChanging == null)
          return;
        this.TypNamenReportRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.TypNamenReportRowDeleted == null)
          return;
        this.TypNamenReportRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.TypNamenReportRowDeleting == null)
          return;
        this.TypNamenReportRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveTypNamenReportRow(ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (TypNamenReportDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class useraccDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow>
    {
      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public useraccDataTable()
      {
        this.TableName = "useracc";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal useraccDataTable(DataTable table)
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
      protected useraccDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEventHandler useraccRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEventHandler useraccRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEventHandler useraccRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEventHandler useraccRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AdduseraccRow(ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow AdduseraccRow()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) this.NewRow();
        object[] objArray = new object[0];
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable useraccDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable) base.Clone();
        useraccDataTable.InitVars();
        return (DataTable) useraccDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow NewuseraccRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.useraccRowChanged == null)
          return;
        this.useraccRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.useraccRowChanging == null)
          return;
        this.useraccRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.useraccRowDeleted == null)
          return;
        this.useraccRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.useraccRowDeleting == null)
          return;
        this.useraccRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.useraccRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveuseraccRow(ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (useraccDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class usersDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.usersRow>
    {
      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public usersDataTable()
      {
        this.TableName = "users";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal usersDataTable(DataTable table)
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
      protected usersDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.usersRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEventHandler usersRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEventHandler usersRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEventHandler usersRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEventHandler usersRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddusersRow(ZR_ClassLibrary.Schema_Mulda.Schema.usersRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.usersRow AddusersRow()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.usersRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) this.NewRow();
        object[] objArray = new object[0];
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable usersDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable) base.Clone();
        usersDataTable.InitVars();
        return (DataTable) usersDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.usersRow NewusersRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.usersRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.usersRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.usersRowChanged == null)
          return;
        this.usersRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.usersRowChanging == null)
          return;
        this.usersRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.usersRowDeleted == null)
          return;
        this.usersRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.usersRowDeleting == null)
          return;
        this.usersRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.usersRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.usersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveusersRow(ZR_ClassLibrary.Schema_Mulda.Schema.usersRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (usersDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class VersionsUpdateDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow>
    {
      private DataColumn columnFileName;
      private DataColumn columnTimeStamp;
      private DataColumn columnFile;
      private DataColumn columnKz_Neu;
      private DataColumn columnCheckSumme;
      private DataColumn columnAppName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public VersionsUpdateDataTable()
      {
        this.TableName = "VersionsUpdate";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal VersionsUpdateDataTable(DataTable table)
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
      protected VersionsUpdateDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FileNameColumn => this.columnFileName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FileColumn => this.columnFile;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Kz_NeuColumn => this.columnKz_Neu;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CheckSummeColumn => this.columnCheckSumme;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AppNameColumn => this.columnAppName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEventHandler VersionsUpdateRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEventHandler VersionsUpdateRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEventHandler VersionsUpdateRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEventHandler VersionsUpdateRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddVersionsUpdateRow(ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow AddVersionsUpdateRow(
        string FileName,
        DateTime TimeStamp,
        byte[] File,
        bool Kz_Neu,
        long CheckSumme,
        string AppName)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) this.NewRow();
        object[] objArray = new object[6]
        {
          (object) FileName,
          (object) TimeStamp,
          (object) File,
          (object) Kz_Neu,
          (object) CheckSumme,
          (object) AppName
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable versionsUpdateDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable) base.Clone();
        versionsUpdateDataTable.InitVars();
        return (DataTable) versionsUpdateDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnFileName = this.Columns["FileName"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
        this.columnFile = this.Columns["File"];
        this.columnKz_Neu = this.Columns["Kz_Neu"];
        this.columnCheckSumme = this.Columns["CheckSumme"];
        this.columnAppName = this.Columns["AppName"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnFileName = new DataColumn("FileName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFileName);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.columnFile = new DataColumn("File", typeof (byte[]), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFile);
        this.columnKz_Neu = new DataColumn("Kz_Neu", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnKz_Neu);
        this.columnCheckSumme = new DataColumn("CheckSumme", typeof (long), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCheckSumme);
        this.columnAppName = new DataColumn("AppName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAppName);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnFileName
        }, false));
        this.columnFileName.Unique = true;
        this.columnFileName.MaxLength = 30;
        this.columnAppName.MaxLength = 30;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow NewVersionsUpdateRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.VersionsUpdateRowChanged == null)
          return;
        this.VersionsUpdateRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.VersionsUpdateRowChanging == null)
          return;
        this.VersionsUpdateRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.VersionsUpdateRowDeleted == null)
          return;
        this.VersionsUpdateRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.VersionsUpdateRowDeleting == null)
          return;
        this.VersionsUpdateRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveVersionsUpdateRow(ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (VersionsUpdateDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class ZaehlerTypDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow>
    {
      private DataColumn columnTypID;
      private DataColumn columnTyp;
      private DataColumn columnArtikelnummer;
      private DataColumn columnEAN_Nr;
      private DataColumn columnHersteller;
      private DataColumn columnEtikettenbezeichnung;
      private DataColumn columnFuehlerlaenge_VL;
      private DataColumn columnFuehlerlaenge_RL;
      private DataColumn columnHyd_TypID;
      private DataColumn columnRechenwerk_TypID;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaehlerTypDataTable()
      {
        this.TableName = "ZaehlerTyp";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaehlerTypDataTable(DataTable table)
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
      protected ZaehlerTypDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TypIDColumn => this.columnTypID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TypColumn => this.columnTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ArtikelnummerColumn => this.columnArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EAN_NrColumn => this.columnEAN_Nr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HerstellerColumn => this.columnHersteller;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EtikettenbezeichnungColumn => this.columnEtikettenbezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Fuehlerlaenge_VLColumn => this.columnFuehlerlaenge_VL;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Fuehlerlaenge_RLColumn => this.columnFuehlerlaenge_RL;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Hyd_TypIDColumn => this.columnHyd_TypID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Rechenwerk_TypIDColumn => this.columnRechenwerk_TypID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEventHandler ZaehlerTypRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEventHandler ZaehlerTypRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEventHandler ZaehlerTypRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEventHandler ZaehlerTypRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddZaehlerTypRow(ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow AddZaehlerTypRow(
        int TypID,
        string Typ,
        string Artikelnummer,
        string EAN_Nr,
        string Hersteller,
        string Etikettenbezeichnung,
        double Fuehlerlaenge_VL,
        double Fuehlerlaenge_RL,
        int Hyd_TypID,
        int Rechenwerk_TypID,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) this.NewRow();
        object[] objArray = new object[11]
        {
          (object) TypID,
          (object) Typ,
          (object) Artikelnummer,
          (object) EAN_Nr,
          (object) Hersteller,
          (object) Etikettenbezeichnung,
          (object) Fuehlerlaenge_VL,
          (object) Fuehlerlaenge_RL,
          (object) Hyd_TypID,
          (object) Rechenwerk_TypID,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable zaehlerTypDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable) base.Clone();
        zaehlerTypDataTable.InitVars();
        return (DataTable) zaehlerTypDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnTypID = this.Columns["TypID"];
        this.columnTyp = this.Columns["Typ"];
        this.columnArtikelnummer = this.Columns["Artikelnummer"];
        this.columnEAN_Nr = this.Columns["EAN_Nr"];
        this.columnHersteller = this.Columns["Hersteller"];
        this.columnEtikettenbezeichnung = this.Columns["Etikettenbezeichnung"];
        this.columnFuehlerlaenge_VL = this.Columns["Fuehlerlaenge_VL"];
        this.columnFuehlerlaenge_RL = this.Columns["Fuehlerlaenge_RL"];
        this.columnHyd_TypID = this.Columns["Hyd_TypID"];
        this.columnRechenwerk_TypID = this.Columns["Rechenwerk_TypID"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnTypID = new DataColumn("TypID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTypID);
        this.columnTyp = new DataColumn("Typ", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTyp);
        this.columnArtikelnummer = new DataColumn("Artikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnArtikelnummer);
        this.columnEAN_Nr = new DataColumn("EAN_Nr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEAN_Nr);
        this.columnHersteller = new DataColumn("Hersteller", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHersteller);
        this.columnEtikettenbezeichnung = new DataColumn("Etikettenbezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEtikettenbezeichnung);
        this.columnFuehlerlaenge_VL = new DataColumn("Fuehlerlaenge_VL", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFuehlerlaenge_VL);
        this.columnFuehlerlaenge_RL = new DataColumn("Fuehlerlaenge_RL", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFuehlerlaenge_RL);
        this.columnHyd_TypID = new DataColumn("Hyd_TypID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHyd_TypID);
        this.columnRechenwerk_TypID = new DataColumn("Rechenwerk_TypID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRechenwerk_TypID);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnTypID
        }, false));
        this.columnTypID.Unique = true;
        this.columnTyp.MaxLength = 50;
        this.columnArtikelnummer.MaxLength = 20;
        this.columnEAN_Nr.MaxLength = 13;
        this.columnHersteller.MaxLength = 30;
        this.columnEtikettenbezeichnung.MaxLength = 40;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow NewZaehlerTypRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ZaehlerTypRowChanged == null)
          return;
        this.ZaehlerTypRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ZaehlerTypRowChanging == null)
          return;
        this.ZaehlerTypRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ZaehlerTypRowDeleted == null)
          return;
        this.ZaehlerTypRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ZaehlerTypRowDeleting == null)
          return;
        this.ZaehlerTypRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveZaehlerTypRow(ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ZaehlerTypDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class ZaehlertypherstDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow>
    {
      private DataColumn columnRecID;
      private DataColumn columnBezeichnung;
      private DataColumn columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaehlertypherstDataTable()
      {
        this.TableName = "Zaehlertypherst";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaehlertypherstDataTable(DataTable table)
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
      protected ZaehlertypherstDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RecIDColumn => this.columnRecID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BezeichnungColumn => this.columnBezeichnung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn TimeStampColumn => this.columnTimeStamp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEventHandler ZaehlertypherstRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEventHandler ZaehlertypherstRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEventHandler ZaehlertypherstRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEventHandler ZaehlertypherstRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddZaehlertypherstRow(ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow AddZaehlertypherstRow(
        int RecID,
        string Bezeichnung,
        DateTime TimeStamp)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) this.NewRow();
        object[] objArray = new object[3]
        {
          (object) RecID,
          (object) Bezeichnung,
          (object) TimeStamp
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable zaehlertypherstDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable) base.Clone();
        zaehlertypherstDataTable.InitVars();
        return (DataTable) zaehlertypherstDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnRecID = this.Columns["RecID"];
        this.columnBezeichnung = this.Columns["Bezeichnung"];
        this.columnTimeStamp = this.Columns["TimeStamp"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnRecID = new DataColumn("RecID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRecID);
        this.columnBezeichnung = new DataColumn("Bezeichnung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBezeichnung);
        this.columnTimeStamp = new DataColumn("TimeStamp", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimeStamp);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnRecID
        }, false));
        this.columnRecID.Unique = true;
        this.columnBezeichnung.MaxLength = 30;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow NewZaehlertypherstRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ZaehlertypherstRowChanged == null)
          return;
        this.ZaehlertypherstRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ZaehlertypherstRowChanging == null)
          return;
        this.ZaehlertypherstRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ZaehlertypherstRowDeleted == null)
          return;
        this.ZaehlertypherstRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ZaehlertypherstRowDeleting == null)
          return;
        this.ZaehlertypherstRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveZaehlertypherstRow(ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ZaehlertypherstDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class ZaeBel_2008DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row>
    {
      private DataColumn columnAuftragsNr;
      private DataColumn columnZaehlerTyp;
      private DataColumn columnArtikelnummer;
      private DataColumn columnEAN_Nr;
      private DataColumn columnBelegstatus;
      private DataColumn columnMontagedatum;
      private DataColumn columnImpulswertigkeit;
      private DataColumn columnHerstelljahr;
      private DataColumn columnPruefstand;
      private DataColumn columnPruefer;
      private DataColumn columnBemerkung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaeBel_2008DataTable()
      {
        this.TableName = "ZaeBel_2008";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaeBel_2008DataTable(DataTable table)
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
      protected ZaeBel_2008DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AuftragsNrColumn => this.columnAuftragsNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ZaehlerTypColumn => this.columnZaehlerTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ArtikelnummerColumn => this.columnArtikelnummer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EAN_NrColumn => this.columnEAN_Nr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BelegstatusColumn => this.columnBelegstatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MontagedatumColumn => this.columnMontagedatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ImpulswertigkeitColumn => this.columnImpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HerstelljahrColumn => this.columnHerstelljahr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PruefstandColumn => this.columnPruefstand;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PrueferColumn => this.columnPruefer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BemerkungColumn => this.columnBemerkung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEventHandler ZaeBel_2008RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEventHandler ZaeBel_2008RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEventHandler ZaeBel_2008RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEventHandler ZaeBel_2008RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddZaeBel_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row AddZaeBel_2008Row(
        string AuftragsNr,
        int ZaehlerTyp,
        string Artikelnummer,
        string EAN_Nr,
        short Belegstatus,
        DateTime Montagedatum,
        double Impulswertigkeit,
        string Herstelljahr,
        string Pruefstand,
        string Pruefer,
        string Bemerkung)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) this.NewRow();
        object[] objArray = new object[11]
        {
          (object) AuftragsNr,
          (object) ZaehlerTyp,
          (object) Artikelnummer,
          (object) EAN_Nr,
          (object) Belegstatus,
          (object) Montagedatum,
          (object) Impulswertigkeit,
          (object) Herstelljahr,
          (object) Pruefstand,
          (object) Pruefer,
          (object) Bemerkung
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable bel2008DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable) base.Clone();
        bel2008DataTable.InitVars();
        return (DataTable) bel2008DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnAuftragsNr = this.Columns["AuftragsNr"];
        this.columnZaehlerTyp = this.Columns["ZaehlerTyp"];
        this.columnArtikelnummer = this.Columns["Artikelnummer"];
        this.columnEAN_Nr = this.Columns["EAN_Nr"];
        this.columnBelegstatus = this.Columns["Belegstatus"];
        this.columnMontagedatum = this.Columns["Montagedatum"];
        this.columnImpulswertigkeit = this.Columns["Impulswertigkeit"];
        this.columnHerstelljahr = this.Columns["Herstelljahr"];
        this.columnPruefstand = this.Columns["Pruefstand"];
        this.columnPruefer = this.Columns["Pruefer"];
        this.columnBemerkung = this.Columns["Bemerkung"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnAuftragsNr = new DataColumn("AuftragsNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAuftragsNr);
        this.columnZaehlerTyp = new DataColumn("ZaehlerTyp", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZaehlerTyp);
        this.columnArtikelnummer = new DataColumn("Artikelnummer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnArtikelnummer);
        this.columnEAN_Nr = new DataColumn("EAN_Nr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEAN_Nr);
        this.columnBelegstatus = new DataColumn("Belegstatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBelegstatus);
        this.columnMontagedatum = new DataColumn("Montagedatum", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMontagedatum);
        this.columnImpulswertigkeit = new DataColumn("Impulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnImpulswertigkeit);
        this.columnHerstelljahr = new DataColumn("Herstelljahr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHerstelljahr);
        this.columnPruefstand = new DataColumn("Pruefstand", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefstand);
        this.columnPruefer = new DataColumn("Pruefer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPruefer);
        this.columnBemerkung = new DataColumn("Bemerkung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBemerkung);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnAuftragsNr
        }, false));
        this.columnAuftragsNr.Unique = true;
        this.columnAuftragsNr.MaxLength = 15;
        this.columnArtikelnummer.MaxLength = 20;
        this.columnEAN_Nr.MaxLength = 13;
        this.columnHerstelljahr.MaxLength = 4;
        this.columnPruefstand.MaxLength = 6;
        this.columnPruefer.MaxLength = 20;
        this.columnBemerkung.MaxLength = 5;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row NewZaeBel_2008Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ZaeBel_2008RowChanged == null)
          return;
        this.ZaeBel_2008RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ZaeBel_2008RowChanging == null)
          return;
        this.ZaeBel_2008RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ZaeBel_2008RowDeleted == null)
          return;
        this.ZaeBel_2008RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ZaeBel_2008RowDeleting == null)
          return;
        this.ZaeBel_2008RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveZaeBel_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ZaeBel_2008DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class ZaePos_2008DataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row>
    {
      private DataColumn columnAuftragsNr;
      private DataColumn columnAuftragsIndex;
      private DataColumn columnSerienNr;
      private DataColumn columnMontagedatum;
      private DataColumn columnGeraetestatus;
      private DataColumn columnSerienNr_Hydraulik;
      private DataColumn columnHydraulikDatenVorhanden;
      private DataColumn columnSerienNr_Rechenwerk;
      private DataColumn columnRechenwerkDatenVorhanden;
      private DataColumn columnBasisImpulswertigkeit;
      private DataColumn columnQtrennMessfehler;
      private DataColumn columnQminMessfehler;
      private DataColumn columnQnennMessfehler;
      private DataColumn columnImpulswertigkeit;
      private DataColumn columnQtrennFehler;
      private DataColumn columnQminFehler;
      private DataColumn columnQnennFehler;
      private DataColumn columnQdTminFehler_Rec;
      private DataColumn columnQdTnennFehler_Rec;
      private DataColumn columnQdTmaxFehler_Rec;
      private DataColumn columnChargenNr_Hyd;
      private DataColumn columnChargenNr_Rec;
      private DataColumn columnGedruckt;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaePos_2008DataTable()
      {
        this.TableName = "ZaePos_2008";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaePos_2008DataTable(DataTable table)
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
      protected ZaePos_2008DataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AuftragsNrColumn => this.columnAuftragsNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AuftragsIndexColumn => this.columnAuftragsIndex;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNrColumn => this.columnSerienNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MontagedatumColumn => this.columnMontagedatum;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GeraetestatusColumn => this.columnGeraetestatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNr_HydraulikColumn => this.columnSerienNr_Hydraulik;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn HydraulikDatenVorhandenColumn => this.columnHydraulikDatenVorhanden;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SerienNr_RechenwerkColumn => this.columnSerienNr_Rechenwerk;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn RechenwerkDatenVorhandenColumn => this.columnRechenwerkDatenVorhanden;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BasisImpulswertigkeitColumn => this.columnBasisImpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennMessfehlerColumn => this.columnQtrennMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminMessfehlerColumn => this.columnQminMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennMessfehlerColumn => this.columnQnennMessfehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ImpulswertigkeitColumn => this.columnImpulswertigkeit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QtrennFehlerColumn => this.columnQtrennFehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QminFehlerColumn => this.columnQminFehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QnennFehlerColumn => this.columnQnennFehler;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTminFehler_RecColumn => this.columnQdTminFehler_Rec;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTnennFehler_RecColumn => this.columnQdTnennFehler_Rec;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn QdTmaxFehler_RecColumn => this.columnQdTmaxFehler_Rec;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNr_HydColumn => this.columnChargenNr_Hyd;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChargenNr_RecColumn => this.columnChargenNr_Rec;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn GedrucktColumn => this.columnGedruckt;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEventHandler ZaePos_2008RowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEventHandler ZaePos_2008RowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEventHandler ZaePos_2008RowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEventHandler ZaePos_2008RowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddZaePos_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row AddZaePos_2008Row(
        string AuftragsNr,
        short AuftragsIndex,
        string SerienNr,
        DateTime Montagedatum,
        short Geraetestatus,
        string SerienNr_Hydraulik,
        bool HydraulikDatenVorhanden,
        string SerienNr_Rechenwerk,
        bool RechenwerkDatenVorhanden,
        double BasisImpulswertigkeit,
        double QtrennMessfehler,
        double QminMessfehler,
        double QnennMessfehler,
        double Impulswertigkeit,
        double QtrennFehler,
        double QminFehler,
        double QnennFehler,
        double QdTminFehler_Rec,
        double QdTnennFehler_Rec,
        double QdTmaxFehler_Rec,
        string ChargenNr_Hyd,
        string ChargenNr_Rec,
        bool Gedruckt)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row row = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) this.NewRow();
        object[] objArray = new object[23]
        {
          (object) AuftragsNr,
          (object) AuftragsIndex,
          (object) SerienNr,
          (object) Montagedatum,
          (object) Geraetestatus,
          (object) SerienNr_Hydraulik,
          (object) HydraulikDatenVorhanden,
          (object) SerienNr_Rechenwerk,
          (object) RechenwerkDatenVorhanden,
          (object) BasisImpulswertigkeit,
          (object) QtrennMessfehler,
          (object) QminMessfehler,
          (object) QnennMessfehler,
          (object) Impulswertigkeit,
          (object) QtrennFehler,
          (object) QminFehler,
          (object) QnennFehler,
          (object) QdTminFehler_Rec,
          (object) QdTnennFehler_Rec,
          (object) QdTmaxFehler_Rec,
          (object) ChargenNr_Hyd,
          (object) ChargenNr_Rec,
          (object) Gedruckt
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable pos2008DataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable) base.Clone();
        pos2008DataTable.InitVars();
        return (DataTable) pos2008DataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnAuftragsNr = this.Columns["AuftragsNr"];
        this.columnAuftragsIndex = this.Columns["AuftragsIndex"];
        this.columnSerienNr = this.Columns["SerienNr"];
        this.columnMontagedatum = this.Columns["Montagedatum"];
        this.columnGeraetestatus = this.Columns["Geraetestatus"];
        this.columnSerienNr_Hydraulik = this.Columns["SerienNr_Hydraulik"];
        this.columnHydraulikDatenVorhanden = this.Columns["HydraulikDatenVorhanden"];
        this.columnSerienNr_Rechenwerk = this.Columns["SerienNr_Rechenwerk"];
        this.columnRechenwerkDatenVorhanden = this.Columns["RechenwerkDatenVorhanden"];
        this.columnBasisImpulswertigkeit = this.Columns["BasisImpulswertigkeit"];
        this.columnQtrennMessfehler = this.Columns["QtrennMessfehler"];
        this.columnQminMessfehler = this.Columns["QminMessfehler"];
        this.columnQnennMessfehler = this.Columns["QnennMessfehler"];
        this.columnImpulswertigkeit = this.Columns["Impulswertigkeit"];
        this.columnQtrennFehler = this.Columns["QtrennFehler"];
        this.columnQminFehler = this.Columns["QminFehler"];
        this.columnQnennFehler = this.Columns["QnennFehler"];
        this.columnQdTminFehler_Rec = this.Columns["QdTminFehler_Rec"];
        this.columnQdTnennFehler_Rec = this.Columns["QdTnennFehler_Rec"];
        this.columnQdTmaxFehler_Rec = this.Columns["QdTmaxFehler_Rec"];
        this.columnChargenNr_Hyd = this.Columns["ChargenNr_Hyd"];
        this.columnChargenNr_Rec = this.Columns["ChargenNr_Rec"];
        this.columnGedruckt = this.Columns["Gedruckt"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnAuftragsNr = new DataColumn("AuftragsNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAuftragsNr);
        this.columnAuftragsIndex = new DataColumn("AuftragsIndex", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAuftragsIndex);
        this.columnSerienNr = new DataColumn("SerienNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNr);
        this.columnMontagedatum = new DataColumn("Montagedatum", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMontagedatum);
        this.columnGeraetestatus = new DataColumn("Geraetestatus", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGeraetestatus);
        this.columnSerienNr_Hydraulik = new DataColumn("SerienNr_Hydraulik", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNr_Hydraulik);
        this.columnHydraulikDatenVorhanden = new DataColumn("HydraulikDatenVorhanden", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHydraulikDatenVorhanden);
        this.columnSerienNr_Rechenwerk = new DataColumn("SerienNr_Rechenwerk", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerienNr_Rechenwerk);
        this.columnRechenwerkDatenVorhanden = new DataColumn("RechenwerkDatenVorhanden", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRechenwerkDatenVorhanden);
        this.columnBasisImpulswertigkeit = new DataColumn("BasisImpulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBasisImpulswertigkeit);
        this.columnQtrennMessfehler = new DataColumn("QtrennMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennMessfehler);
        this.columnQminMessfehler = new DataColumn("QminMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminMessfehler);
        this.columnQnennMessfehler = new DataColumn("QnennMessfehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennMessfehler);
        this.columnImpulswertigkeit = new DataColumn("Impulswertigkeit", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnImpulswertigkeit);
        this.columnQtrennFehler = new DataColumn("QtrennFehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQtrennFehler);
        this.columnQminFehler = new DataColumn("QminFehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQminFehler);
        this.columnQnennFehler = new DataColumn("QnennFehler", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQnennFehler);
        this.columnQdTminFehler_Rec = new DataColumn("QdTminFehler_Rec", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTminFehler_Rec);
        this.columnQdTnennFehler_Rec = new DataColumn("QdTnennFehler_Rec", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTnennFehler_Rec);
        this.columnQdTmaxFehler_Rec = new DataColumn("QdTmaxFehler_Rec", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnQdTmaxFehler_Rec);
        this.columnChargenNr_Hyd = new DataColumn("ChargenNr_Hyd", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr_Hyd);
        this.columnChargenNr_Rec = new DataColumn("ChargenNr_Rec", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChargenNr_Rec);
        this.columnGedruckt = new DataColumn("Gedruckt", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGedruckt);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnAuftragsNr
        }, false));
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[1]
        {
          this.columnAuftragsIndex
        }, false));
        this.columnAuftragsNr.Unique = true;
        this.columnAuftragsNr.MaxLength = 15;
        this.columnAuftragsIndex.Unique = true;
        this.columnSerienNr.MaxLength = 8;
        this.columnSerienNr_Hydraulik.MaxLength = 9;
        this.columnSerienNr_Rechenwerk.MaxLength = 8;
        this.columnChargenNr_Hyd.MaxLength = 10;
        this.columnChargenNr_Rec.MaxLength = 10;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row NewZaePos_2008Row()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ZaePos_2008RowChanged == null)
          return;
        this.ZaePos_2008RowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ZaePos_2008RowChanging == null)
          return;
        this.ZaePos_2008RowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ZaePos_2008RowDeleted == null)
          return;
        this.ZaePos_2008RowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ZaePos_2008RowDeleting == null)
          return;
        this.ZaePos_2008RowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008RowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveZaePos_2008Row(ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ZaePos_2008DataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class DateilisteDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow>
    {
      private DataColumn columnDateiname;
      private DataColumn columnBemerkung;
      private DataColumn columnUserMajorVersion;
      private DataColumn columnUserMinorVersion;
      private DataColumn columnDBStruktur;
      private DataColumn columnDBZwischenstruktur;
      private DataColumn columnAktiviereZwischenschritt;
      private DataColumn columnAutocreate;
      private DataColumn columnLetzte_Strukturaenderung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateilisteDataTable()
      {
        this.TableName = "Dateiliste";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal DateilisteDataTable(DataTable table)
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
      protected DateilisteDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DateinameColumn => this.columnDateiname;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BemerkungColumn => this.columnBemerkung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn UserMajorVersionColumn => this.columnUserMajorVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn UserMinorVersionColumn => this.columnUserMinorVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DBStrukturColumn => this.columnDBStruktur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DBZwischenstrukturColumn => this.columnDBZwischenstruktur;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AktiviereZwischenschrittColumn => this.columnAktiviereZwischenschritt;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AutocreateColumn => this.columnAutocreate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn Letzte_StrukturaenderungColumn => this.columnLetzte_Strukturaenderung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEventHandler DateilisteRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEventHandler DateilisteRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEventHandler DateilisteRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEventHandler DateilisteRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddDateilisteRow(ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow AddDateilisteRow(
        string Dateiname,
        string Bemerkung,
        ushort UserMajorVersion,
        ushort UserMinorVersion,
        string DBStruktur,
        string DBZwischenstruktur,
        bool AktiviereZwischenschritt,
        bool Autocreate,
        DateTime Letzte_Strukturaenderung)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) this.NewRow();
        object[] objArray = new object[9]
        {
          (object) Dateiname,
          (object) Bemerkung,
          (object) UserMajorVersion,
          (object) UserMinorVersion,
          (object) DBStruktur,
          (object) DBZwischenstruktur,
          (object) AktiviereZwischenschritt,
          (object) Autocreate,
          (object) Letzte_Strukturaenderung
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable dateilisteDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable) base.Clone();
        dateilisteDataTable.InitVars();
        return (DataTable) dateilisteDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnDateiname = this.Columns["Dateiname"];
        this.columnBemerkung = this.Columns["Bemerkung"];
        this.columnUserMajorVersion = this.Columns["UserMajorVersion"];
        this.columnUserMinorVersion = this.Columns["UserMinorVersion"];
        this.columnDBStruktur = this.Columns["DBStruktur"];
        this.columnDBZwischenstruktur = this.Columns["DBZwischenstruktur"];
        this.columnAktiviereZwischenschritt = this.Columns["AktiviereZwischenschritt"];
        this.columnAutocreate = this.Columns["Autocreate"];
        this.columnLetzte_Strukturaenderung = this.Columns["Letzte_Strukturaenderung"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnDateiname = new DataColumn("Dateiname", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDateiname);
        this.columnBemerkung = new DataColumn("Bemerkung", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBemerkung);
        this.columnUserMajorVersion = new DataColumn("UserMajorVersion", typeof (ushort), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUserMajorVersion);
        this.columnUserMinorVersion = new DataColumn("UserMinorVersion", typeof (ushort), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUserMinorVersion);
        this.columnDBStruktur = new DataColumn("DBStruktur", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDBStruktur);
        this.columnDBZwischenstruktur = new DataColumn("DBZwischenstruktur", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDBZwischenstruktur);
        this.columnAktiviereZwischenschritt = new DataColumn("AktiviereZwischenschritt", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAktiviereZwischenschritt);
        this.columnAutocreate = new DataColumn("Autocreate", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAutocreate);
        this.columnLetzte_Strukturaenderung = new DataColumn("Letzte_Strukturaenderung", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLetzte_Strukturaenderung);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnDateiname
        }, false));
        this.columnDateiname.Unique = true;
        this.columnDateiname.MaxLength = 25;
        this.columnBemerkung.MaxLength = 50;
        this.columnDBStruktur.MaxLength = int.MaxValue;
        this.columnDBZwischenstruktur.MaxLength = int.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow NewDateilisteRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.DateilisteRowChanged == null)
          return;
        this.DateilisteRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.DateilisteRowChanging == null)
          return;
        this.DateilisteRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.DateilisteRowDeleted == null)
          return;
        this.DateilisteRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.DateilisteRowDeleting == null)
          return;
        this.DateilisteRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveDateilisteRow(ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (DateilisteDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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
    public class S_VersionDataTable : TypedTableBase<ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow>
    {
      private DataColumn columnVersion;
      private DataColumn columnSQL;
      private DataColumn columnExecSQL;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public S_VersionDataTable()
      {
        this.TableName = "S_Version";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal S_VersionDataTable(DataTable table)
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
      protected S_VersionDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn VersionColumn => this.columnVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SQLColumn => this.columnSQL;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ExecSQLColumn => this.columnExecSQL;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow this[int index]
      {
        get => (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEventHandler S_VersionRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEventHandler S_VersionRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEventHandler S_VersionRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEventHandler S_VersionRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddS_VersionRow(ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow AddS_VersionRow(
        string Version,
        string SQL,
        bool ExecSQL)
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow row = (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) this.NewRow();
        object[] objArray = new object[3]
        {
          (object) Version,
          (object) SQL,
          (object) ExecSQL
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable versionDataTable = (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable) base.Clone();
        versionDataTable.InitVars();
        return (DataTable) versionDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnVersion = this.Columns["Version"];
        this.columnSQL = this.Columns["SQL"];
        this.columnExecSQL = this.Columns["ExecSQL"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnVersion = new DataColumn("Version", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnVersion);
        this.columnSQL = new DataColumn("SQL", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSQL);
        this.columnExecSQL = new DataColumn("ExecSQL", typeof (bool), (string) null, MappingType.Element);
        this.Columns.Add(this.columnExecSQL);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnVersion
        }, false));
        this.columnVersion.Unique = true;
        this.columnVersion.MaxLength = 8;
        this.columnSQL.MaxLength = int.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow NewS_VersionRow()
      {
        return (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.S_VersionRowChanged == null)
          return;
        this.S_VersionRowChanged((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.S_VersionRowChanging == null)
          return;
        this.S_VersionRowChanging((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.S_VersionRowDeleted == null)
          return;
        this.S_VersionRowDeleted((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.S_VersionRowDeleting == null)
          return;
        this.S_VersionRowDeleting((object) this, new ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRowChangeEvent((ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveS_VersionRow(ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        ZR_ClassLibrary.Schema_Mulda.Schema schema = new ZR_ClassLibrary.Schema_Mulda.Schema();
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
          FixedValue = schema.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (S_VersionDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = schema.GetSchemaSerializable();
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

    public class accessRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable tableaccess;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal accessRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableaccess = (ZR_ClassLibrary.Schema_Mulda.Schema.accessDataTable) this.Table;
      }
    }

    public class DichteRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable tableDichte;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal DichteRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableDichte = (ZR_ClassLibrary.Schema_Mulda.Schema.DichteDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableDichte.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'Dichte' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDichte.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Dichte
      {
        get
        {
          try
          {
            return (double) this[this.tableDichte.DichteColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Dichte' in table 'Dichte' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDichte.DichteColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableDichte.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableDichte.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDichteNull() => this.IsNull(this.tableDichte.DichteColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDichteNull() => this[this.tableDichte.DichteColumn] = Convert.DBNull;
    }

    public class EdrbsqlRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable tableEdrbsql;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EdrbsqlRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableEdrbsql = (ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short ID
      {
        get
        {
          try
          {
            return (short) this[this.tableEdrbsql.IDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ID' in table 'Edrbsql' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEdrbsql.IDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Beschreibung
      {
        get
        {
          try
          {
            return (string) this[this.tableEdrbsql.BeschreibungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Beschreibung' in table 'Edrbsql' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEdrbsql.BeschreibungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SQL
      {
        get
        {
          try
          {
            return (string) this[this.tableEdrbsql.SQLColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SQL' in table 'Edrbsql' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEdrbsql.SQLColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Params
      {
        get
        {
          try
          {
            return (string) this[this.tableEdrbsql.ParamsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Params' in table 'Edrbsql' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEdrbsql.ParamsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableEdrbsql.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'Edrbsql' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEdrbsql.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsIDNull() => this.IsNull(this.tableEdrbsql.IDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetIDNull() => this[this.tableEdrbsql.IDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBeschreibungNull() => this.IsNull(this.tableEdrbsql.BeschreibungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBeschreibungNull()
      {
        this[this.tableEdrbsql.BeschreibungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSQLNull() => this.IsNull(this.tableEdrbsql.SQLColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSQLNull() => this[this.tableEdrbsql.SQLColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsParamsNull() => this.IsNull(this.tableEdrbsql.ParamsColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetParamsNull() => this[this.tableEdrbsql.ParamsColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableEdrbsql.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull() => this[this.tableEdrbsql.TimeStampColumn] = Convert.DBNull;
    }

    public class FormulareRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable tableFormulare;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal FormulareRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableFormulare = (ZR_ClassLibrary.Schema_Mulda.Schema.FormulareDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int SatzID
      {
        get
        {
          try
          {
            return (int) this[this.tableFormulare.SatzIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SatzID' in table 'Formulare' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableFormulare.SatzIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Formulartyp
      {
        get
        {
          try
          {
            return (string) this[this.tableFormulare.FormulartypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Formulartyp' in table 'Formulare' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableFormulare.FormulartypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableFormulare.BezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bezeichnung' in table 'Formulare' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableFormulare.BezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Druckerbemerkungen
      {
        get
        {
          try
          {
            return (string) this[this.tableFormulare.DruckerbemerkungenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Druckerbemerkungen' in table 'Formulare' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableFormulare.DruckerbemerkungenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short FormularID
      {
        get
        {
          try
          {
            return (short) this[this.tableFormulare.FormularIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FormularID' in table 'Formulare' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableFormulare.FormularIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte[] Formular
      {
        get
        {
          try
          {
            return (byte[]) this[this.tableFormulare.FormularColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Formular' in table 'Formulare' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableFormulare.FormularColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSatzIDNull() => this.IsNull(this.tableFormulare.SatzIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSatzIDNull() => this[this.tableFormulare.SatzIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFormulartypNull() => this.IsNull(this.tableFormulare.FormulartypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFormulartypNull()
      {
        this[this.tableFormulare.FormulartypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBezeichnungNull() => this.IsNull(this.tableFormulare.BezeichnungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBezeichnungNull()
      {
        this[this.tableFormulare.BezeichnungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDruckerbemerkungenNull()
      {
        return this.IsNull(this.tableFormulare.DruckerbemerkungenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDruckerbemerkungenNull()
      {
        this[this.tableFormulare.DruckerbemerkungenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFormularIDNull() => this.IsNull(this.tableFormulare.FormularIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFormularIDNull()
      {
        this[this.tableFormulare.FormularIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFormularNull() => this.IsNull(this.tableFormulare.FormularColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFormularNull() => this[this.tableFormulare.FormularColumn] = Convert.DBNull;
    }

    public class groupsRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable tablegroups;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal groupsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tablegroups = (ZR_ClassLibrary.Schema_Mulda.Schema.groupsDataTable) this.Table;
      }
    }

    public class grpcollRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable tablegrpcoll;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal grpcollRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tablegrpcoll = (ZR_ClassLibrary.Schema_Mulda.Schema.grpcollDataTable) this.Table;
      }
    }

    public class HydraulikTypRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable tableHydraulikTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydraulikTypRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydraulikTyp = (ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int TypID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydraulikTyp.TypIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TypID' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.TypIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Hersteller
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.HerstellerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Hersteller' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.HerstellerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Typ
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.TypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Typ' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.TypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Artikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.ArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Artikelnummer' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.ArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short ZaehlerPruefTyp
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.ZaehlerPruefTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZaehlerPruefTyp' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.ZaehlerPruefTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Zulassungsnr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.ZulassungsnrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Zulassungsnr' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.ZulassungsnrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbaulage
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.EinbaulageColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbaulage' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.EinbaulageColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string MetrologischeKlasse
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.MetrologischeKlasseColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MetrologischeKlasse' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.MetrologischeKlasseColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QNenn
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QNennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QNenn' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QNennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Eichwert
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.EichwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Eichwert' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.EichwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Standardimpulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.StandardimpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Standardimpulswertigkeit' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.StandardimpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QMIN_UG
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QMIN_UGColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QMIN_UG' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QMIN_UGColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QMIN_OG
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QMIN_OGColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QMIN_OG' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QMIN_OGColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QTRENN_UG
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QTRENN_UGColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QTRENN_UG' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QTRENN_UGColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QTRENN_OG
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QTRENN_OGColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QTRENN_OG' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QTRENN_OGColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QNENN_UG
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QNENN_UGColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QNENN_UG' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QNENN_UGColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QNENN_OG
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QNENN_OGColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QNENN_OG' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QNENN_OGColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Anschlusstyp
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.AnschlusstypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Anschlusstyp' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.AnschlusstypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short QtMessZeit
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.QtMessZeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtMessZeit' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QtMessZeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short QmMessZeit
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.QmMessZeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QmMessZeit' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QmMessZeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short QnMessZeit
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.QnMessZeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnMessZeit' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QnMessZeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QnMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnMessmenge' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QnMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QtMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtMessmenge' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QtMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QmMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QmMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QmMessmenge' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QmMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Geraetepass
      {
        get
        {
          try
          {
            return (bool) this[this.tableHydraulikTyp.GeraetepassColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Geraetepass' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.GeraetepassColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Zaehlertyp_Pass
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.Zaehlertyp_PassColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Zaehlertyp_Pass' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.Zaehlertyp_PassColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Nennweite
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.NennweiteColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Nennweite' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.NennweiteColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbaulaenge
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.EinbaulaengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbaulaenge' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.EinbaulaengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Gewinde
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.GewindeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Gewinde' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.GewindeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennP
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QnennPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennP' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QnennPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennN
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QnennNColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennN' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QnennNColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennP
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QtrennPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennP' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QtrennPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennN
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QtrennNColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennN' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QtrennNColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminP
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QminPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminP' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QminPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminN
      {
        get
        {
          try
          {
            return (double) this[this.tableHydraulikTyp.QminNColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminN' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.QminNColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short MidQnenn
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.MidQnennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MidQnenn' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.MidQnennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short MidQtrenn
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.MidQtrennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MidQtrenn' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.MidQtrennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short MidQmin
      {
        get
        {
          try
          {
            return (short) this[this.tableHydraulikTyp.MidQminColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MidQmin' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.MidQminColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AnzPruefdurchflussQnenn
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.AnzPruefdurchflussQnennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AnzPruefdurchflussQnenn' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.AnzPruefdurchflussQnennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AnzPruefdurchflussQtrenn
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.AnzPruefdurchflussQtrennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AnzPruefdurchflussQtrenn' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.AnzPruefdurchflussQtrennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AnzPruefdurchflussQmin
      {
        get
        {
          try
          {
            return (string) this[this.tableHydraulikTyp.AnzPruefdurchflussQminColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AnzPruefdurchflussQmin' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.AnzPruefdurchflussQminColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydraulikTyp.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'HydraulikTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydraulikTyp.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTypIDNull() => this.IsNull(this.tableHydraulikTyp.TypIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTypIDNull() => this[this.tableHydraulikTyp.TypIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHerstellerNull() => this.IsNull(this.tableHydraulikTyp.HerstellerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHerstellerNull()
      {
        this[this.tableHydraulikTyp.HerstellerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTypNull() => this.IsNull(this.tableHydraulikTyp.TypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTypNull() => this[this.tableHydraulikTyp.TypColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsArtikelnummerNull() => this.IsNull(this.tableHydraulikTyp.ArtikelnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetArtikelnummerNull()
      {
        this[this.tableHydraulikTyp.ArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZaehlerPruefTypNull()
      {
        return this.IsNull(this.tableHydraulikTyp.ZaehlerPruefTypColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZaehlerPruefTypNull()
      {
        this[this.tableHydraulikTyp.ZaehlerPruefTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZulassungsnrNull() => this.IsNull(this.tableHydraulikTyp.ZulassungsnrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZulassungsnrNull()
      {
        this[this.tableHydraulikTyp.ZulassungsnrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbaulageNull() => this.IsNull(this.tableHydraulikTyp.EinbaulageColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbaulageNull()
      {
        this[this.tableHydraulikTyp.EinbaulageColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMetrologischeKlasseNull()
      {
        return this.IsNull(this.tableHydraulikTyp.MetrologischeKlasseColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMetrologischeKlasseNull()
      {
        this[this.tableHydraulikTyp.MetrologischeKlasseColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQNennNull() => this.IsNull(this.tableHydraulikTyp.QNennColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQNennNull() => this[this.tableHydraulikTyp.QNennColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEichwertNull() => this.IsNull(this.tableHydraulikTyp.EichwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEichwertNull() => this[this.tableHydraulikTyp.EichwertColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsStandardimpulswertigkeitNull()
      {
        return this.IsNull(this.tableHydraulikTyp.StandardimpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetStandardimpulswertigkeitNull()
      {
        this[this.tableHydraulikTyp.StandardimpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQMIN_UGNull() => this.IsNull(this.tableHydraulikTyp.QMIN_UGColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQMIN_UGNull() => this[this.tableHydraulikTyp.QMIN_UGColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQMIN_OGNull() => this.IsNull(this.tableHydraulikTyp.QMIN_OGColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQMIN_OGNull() => this[this.tableHydraulikTyp.QMIN_OGColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQTRENN_UGNull() => this.IsNull(this.tableHydraulikTyp.QTRENN_UGColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQTRENN_UGNull()
      {
        this[this.tableHydraulikTyp.QTRENN_UGColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQTRENN_OGNull() => this.IsNull(this.tableHydraulikTyp.QTRENN_OGColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQTRENN_OGNull()
      {
        this[this.tableHydraulikTyp.QTRENN_OGColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQNENN_UGNull() => this.IsNull(this.tableHydraulikTyp.QNENN_UGColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQNENN_UGNull() => this[this.tableHydraulikTyp.QNENN_UGColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQNENN_OGNull() => this.IsNull(this.tableHydraulikTyp.QNENN_OGColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQNENN_OGNull() => this[this.tableHydraulikTyp.QNENN_OGColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAnschlusstypNull() => this.IsNull(this.tableHydraulikTyp.AnschlusstypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAnschlusstypNull()
      {
        this[this.tableHydraulikTyp.AnschlusstypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtMessZeitNull() => this.IsNull(this.tableHydraulikTyp.QtMessZeitColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtMessZeitNull()
      {
        this[this.tableHydraulikTyp.QtMessZeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQmMessZeitNull() => this.IsNull(this.tableHydraulikTyp.QmMessZeitColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQmMessZeitNull()
      {
        this[this.tableHydraulikTyp.QmMessZeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnMessZeitNull() => this.IsNull(this.tableHydraulikTyp.QnMessZeitColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnMessZeitNull()
      {
        this[this.tableHydraulikTyp.QnMessZeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnMessmengeNull() => this.IsNull(this.tableHydraulikTyp.QnMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnMessmengeNull()
      {
        this[this.tableHydraulikTyp.QnMessmengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtMessmengeNull() => this.IsNull(this.tableHydraulikTyp.QtMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtMessmengeNull()
      {
        this[this.tableHydraulikTyp.QtMessmengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQmMessmengeNull() => this.IsNull(this.tableHydraulikTyp.QmMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQmMessmengeNull()
      {
        this[this.tableHydraulikTyp.QmMessmengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGeraetepassNull() => this.IsNull(this.tableHydraulikTyp.GeraetepassColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGeraetepassNull()
      {
        this[this.tableHydraulikTyp.GeraetepassColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZaehlertyp_PassNull()
      {
        return this.IsNull(this.tableHydraulikTyp.Zaehlertyp_PassColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZaehlertyp_PassNull()
      {
        this[this.tableHydraulikTyp.Zaehlertyp_PassColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsNennweiteNull() => this.IsNull(this.tableHydraulikTyp.NennweiteColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNennweiteNull()
      {
        this[this.tableHydraulikTyp.NennweiteColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbaulaengeNull() => this.IsNull(this.tableHydraulikTyp.EinbaulaengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbaulaengeNull()
      {
        this[this.tableHydraulikTyp.EinbaulaengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGewindeNull() => this.IsNull(this.tableHydraulikTyp.GewindeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGewindeNull() => this[this.tableHydraulikTyp.GewindeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennPNull() => this.IsNull(this.tableHydraulikTyp.QnennPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennPNull() => this[this.tableHydraulikTyp.QnennPColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennNNull() => this.IsNull(this.tableHydraulikTyp.QnennNColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennNNull() => this[this.tableHydraulikTyp.QnennNColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennPNull() => this.IsNull(this.tableHydraulikTyp.QtrennPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennPNull() => this[this.tableHydraulikTyp.QtrennPColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennNNull() => this.IsNull(this.tableHydraulikTyp.QtrennNColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennNNull() => this[this.tableHydraulikTyp.QtrennNColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminPNull() => this.IsNull(this.tableHydraulikTyp.QminPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminPNull() => this[this.tableHydraulikTyp.QminPColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminNNull() => this.IsNull(this.tableHydraulikTyp.QminNColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminNNull() => this[this.tableHydraulikTyp.QminNColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMidQnennNull() => this.IsNull(this.tableHydraulikTyp.MidQnennColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMidQnennNull() => this[this.tableHydraulikTyp.MidQnennColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMidQtrennNull() => this.IsNull(this.tableHydraulikTyp.MidQtrennColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMidQtrennNull()
      {
        this[this.tableHydraulikTyp.MidQtrennColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMidQminNull() => this.IsNull(this.tableHydraulikTyp.MidQminColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMidQminNull() => this[this.tableHydraulikTyp.MidQminColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAnzPruefdurchflussQnennNull()
      {
        return this.IsNull(this.tableHydraulikTyp.AnzPruefdurchflussQnennColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAnzPruefdurchflussQnennNull()
      {
        this[this.tableHydraulikTyp.AnzPruefdurchflussQnennColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAnzPruefdurchflussQtrennNull()
      {
        return this.IsNull(this.tableHydraulikTyp.AnzPruefdurchflussQtrennColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAnzPruefdurchflussQtrennNull()
      {
        this[this.tableHydraulikTyp.AnzPruefdurchflussQtrennColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAnzPruefdurchflussQminNull()
      {
        return this.IsNull(this.tableHydraulikTyp.AnzPruefdurchflussQminColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAnzPruefdurchflussQminNull()
      {
        this[this.tableHydraulikTyp.AnzPruefdurchflussQminColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableHydraulikTyp.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull()
      {
        this[this.tableHydraulikTyp.TimeStampColumn] = Convert.DBNull;
      }
    }

    public class HydAblaufsteuerungRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable tableHydAblaufsteuerung;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydAblaufsteuerungRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydAblaufsteuerung = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufTyp
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung.AblaufTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufTyp' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.AblaufTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int AblaufID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydAblaufsteuerung.AblaufIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufID' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.AblaufIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufSchritt
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung.AblaufSchrittColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufSchritt' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.AblaufSchrittColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufKommando
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung.AblaufKommandoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufKommando' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.AblaufKommandoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufHinweis
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung.AblaufHinweisColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufHinweis' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.AblaufHinweisColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Dauer
      {
        get
        {
          try
          {
            return (int) this[this.tableHydAblaufsteuerung.DauerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Dauer' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.DauerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Kommando
      {
        get
        {
          try
          {
            return (short) this[this.tableHydAblaufsteuerung.KommandoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kommando' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.KommandoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Signal
      {
        get
        {
          try
          {
            return (bool) this[this.tableHydAblaufsteuerung.SignalColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Signal' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.SignalColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short AnzPruefdurchfluss
      {
        get
        {
          try
          {
            return (short) this[this.tableHydAblaufsteuerung.AnzPruefdurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AnzPruefdurchfluss' in table 'HydAblaufsteuerung' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung.AnzPruefdurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufTypNull() => this.IsNull(this.tableHydAblaufsteuerung.AblaufTypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufTypNull()
      {
        this[this.tableHydAblaufsteuerung.AblaufTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufIDNull() => this.IsNull(this.tableHydAblaufsteuerung.AblaufIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufIDNull()
      {
        this[this.tableHydAblaufsteuerung.AblaufIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufSchrittNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung.AblaufSchrittColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufSchrittNull()
      {
        this[this.tableHydAblaufsteuerung.AblaufSchrittColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufKommandoNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung.AblaufKommandoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufKommandoNull()
      {
        this[this.tableHydAblaufsteuerung.AblaufKommandoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufHinweisNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung.AblaufHinweisColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufHinweisNull()
      {
        this[this.tableHydAblaufsteuerung.AblaufHinweisColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDauerNull() => this.IsNull(this.tableHydAblaufsteuerung.DauerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDauerNull() => this[this.tableHydAblaufsteuerung.DauerColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKommandoNull() => this.IsNull(this.tableHydAblaufsteuerung.KommandoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKommandoNull()
      {
        this[this.tableHydAblaufsteuerung.KommandoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSignalNull() => this.IsNull(this.tableHydAblaufsteuerung.SignalColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSignalNull()
      {
        this[this.tableHydAblaufsteuerung.SignalColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAnzPruefdurchflussNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung.AnzPruefdurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAnzPruefdurchflussNull()
      {
        this[this.tableHydAblaufsteuerung.AnzPruefdurchflussColumn] = Convert.DBNull;
      }
    }

    public class HydAblaufsteuerung1Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable tableHydAblaufsteuerung1;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydAblaufsteuerung1Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydAblaufsteuerung1 = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufTyp
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung1.AblaufTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufTyp' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.AblaufTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int AblaufID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydAblaufsteuerung1.AblaufIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufID' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.AblaufIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufSchritt
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung1.AblaufSchrittColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufSchritt' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.AblaufSchrittColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufKommando
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung1.AblaufKommandoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufKommando' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.AblaufKommandoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AblaufHinweis
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAblaufsteuerung1.AblaufHinweisColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AblaufHinweis' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.AblaufHinweisColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Dauer
      {
        get
        {
          try
          {
            return (int) this[this.tableHydAblaufsteuerung1.DauerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Dauer' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.DauerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Kommando
      {
        get
        {
          try
          {
            return (short) this[this.tableHydAblaufsteuerung1.KommandoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kommando' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.KommandoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Signal
      {
        get
        {
          try
          {
            return (bool) this[this.tableHydAblaufsteuerung1.SignalColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Signal' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.SignalColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short AnzPruefdurchfluss
      {
        get
        {
          try
          {
            return (short) this[this.tableHydAblaufsteuerung1.AnzPruefdurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AnzPruefdurchfluss' in table 'HydAblaufsteuerung1' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAblaufsteuerung1.AnzPruefdurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufTypNull() => this.IsNull(this.tableHydAblaufsteuerung1.AblaufTypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufTypNull()
      {
        this[this.tableHydAblaufsteuerung1.AblaufTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufIDNull() => this.IsNull(this.tableHydAblaufsteuerung1.AblaufIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufIDNull()
      {
        this[this.tableHydAblaufsteuerung1.AblaufIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufSchrittNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung1.AblaufSchrittColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufSchrittNull()
      {
        this[this.tableHydAblaufsteuerung1.AblaufSchrittColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufKommandoNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung1.AblaufKommandoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufKommandoNull()
      {
        this[this.tableHydAblaufsteuerung1.AblaufKommandoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAblaufHinweisNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung1.AblaufHinweisColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAblaufHinweisNull()
      {
        this[this.tableHydAblaufsteuerung1.AblaufHinweisColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDauerNull() => this.IsNull(this.tableHydAblaufsteuerung1.DauerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDauerNull()
      {
        this[this.tableHydAblaufsteuerung1.DauerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKommandoNull() => this.IsNull(this.tableHydAblaufsteuerung1.KommandoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKommandoNull()
      {
        this[this.tableHydAblaufsteuerung1.KommandoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSignalNull() => this.IsNull(this.tableHydAblaufsteuerung1.SignalColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSignalNull()
      {
        this[this.tableHydAblaufsteuerung1.SignalColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAnzPruefdurchflussNull()
      {
        return this.IsNull(this.tableHydAblaufsteuerung1.AnzPruefdurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAnzPruefdurchflussNull()
      {
        this[this.tableHydAblaufsteuerung1.AnzPruefdurchflussColumn] = Convert.DBNull;
      }
    }

    public class HydAGewRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable tableHydAGew;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydAGewRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydAGew = (ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydAGew.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'HydAGew' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAGew.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableHydAGew.BezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bezeichnung' in table 'HydAGew' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAGew.BezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydAGew.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'HydAGew' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydAGew.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableHydAGew.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableHydAGew.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBezeichnungNull() => this.IsNull(this.tableHydAGew.BezeichnungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBezeichnungNull()
      {
        this[this.tableHydAGew.BezeichnungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableHydAGew.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull() => this[this.tableHydAGew.TimeStampColumn] = Convert.DBNull;
    }

    public class HydBel_2008Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable tableHydBel_2008;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydBel_2008Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydBel_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydBel_2008.ChargenNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.ChargenNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int ZaehlerTyp
      {
        get
        {
          try
          {
            return (int) this[this.tableHydBel_2008.ZaehlerTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZaehlerTyp' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.ZaehlerTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short ZaehlerPruefTyp
      {
        get
        {
          try
          {
            return (short) this[this.tableHydBel_2008.ZaehlerPruefTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZaehlerPruefTyp' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.ZaehlerPruefTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Artikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableHydBel_2008.ArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Artikelnummer' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.ArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Belegstatus
      {
        get
        {
          try
          {
            return (short) this[this.tableHydBel_2008.BelegstatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Belegstatus' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.BelegstatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Impulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.ImpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Impulswertigkeit' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.ImpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Herstelljahr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydBel_2008.HerstelljahrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Herstelljahr' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.HerstelljahrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefstand
      {
        get
        {
          try
          {
            return (string) this[this.tableHydBel_2008.PruefstandColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefstand' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.PruefstandColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefer
      {
        get
        {
          try
          {
            return (string) this[this.tableHydBel_2008.PrueferColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefer' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.PrueferColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MIDKalibID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydBel_2008.MIDKalibIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MIDKalibID' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.MIDKalibIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Messdatum
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydBel_2008.MessdatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Messdatum' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.MessdatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennTemp
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QtrennTempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennTemp' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QtrennTempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennDurchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QtrennDurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennDurchfluss' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QtrennDurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennKalibriervolumen
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QtrennKalibriervolumenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennKalibriervolumen' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QtrennKalibriervolumenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QtrennMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennMessmenge' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QtrennMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short QtrennTime
      {
        get
        {
          try
          {
            return (short) this[this.tableHydBel_2008.QtrennTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennTime' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QtrennTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminTemp
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QminTempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminTemp' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QminTempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminDurchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QminDurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminDurchfluss' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QminDurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminKalibriervolumen
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QminKalibriervolumenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminKalibriervolumen' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QminKalibriervolumenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QminMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminMessmenge' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QminMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short QminTime
      {
        get
        {
          try
          {
            return (short) this[this.tableHydBel_2008.QminTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminTime' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QminTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennTemp
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QnennTempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennTemp' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QnennTempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennDurchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QnennDurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennDurchfluss' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QnennDurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennKalibriervolumen
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QnennKalibriervolumenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennKalibriervolumen' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QnennKalibriervolumenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydBel_2008.QnennMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennMessmenge' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QnennMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short QnennTime
      {
        get
        {
          try
          {
            return (short) this[this.tableHydBel_2008.QnennTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennTime' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.QnennTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Geraetepass
      {
        get
        {
          try
          {
            return (bool) this[this.tableHydBel_2008.GeraetepassColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Geraetepass' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.GeraetepassColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bemerkung
      {
        get
        {
          try
          {
            return (string) this[this.tableHydBel_2008.BemerkungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bemerkung' in table 'HydBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydBel_2008.BemerkungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNrNull() => this.IsNull(this.tableHydBel_2008.ChargenNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNrNull()
      {
        this[this.tableHydBel_2008.ChargenNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZaehlerTypNull() => this.IsNull(this.tableHydBel_2008.ZaehlerTypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZaehlerTypNull()
      {
        this[this.tableHydBel_2008.ZaehlerTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZaehlerPruefTypNull()
      {
        return this.IsNull(this.tableHydBel_2008.ZaehlerPruefTypColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZaehlerPruefTypNull()
      {
        this[this.tableHydBel_2008.ZaehlerPruefTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsArtikelnummerNull() => this.IsNull(this.tableHydBel_2008.ArtikelnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetArtikelnummerNull()
      {
        this[this.tableHydBel_2008.ArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBelegstatusNull() => this.IsNull(this.tableHydBel_2008.BelegstatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBelegstatusNull()
      {
        this[this.tableHydBel_2008.BelegstatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsImpulswertigkeitNull()
      {
        return this.IsNull(this.tableHydBel_2008.ImpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetImpulswertigkeitNull()
      {
        this[this.tableHydBel_2008.ImpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHerstelljahrNull() => this.IsNull(this.tableHydBel_2008.HerstelljahrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHerstelljahrNull()
      {
        this[this.tableHydBel_2008.HerstelljahrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPruefstandNull() => this.IsNull(this.tableHydBel_2008.PruefstandColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPruefstandNull()
      {
        this[this.tableHydBel_2008.PruefstandColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPrueferNull() => this.IsNull(this.tableHydBel_2008.PrueferColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPrueferNull() => this[this.tableHydBel_2008.PrueferColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMIDKalibIDNull() => this.IsNull(this.tableHydBel_2008.MIDKalibIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMIDKalibIDNull()
      {
        this[this.tableHydBel_2008.MIDKalibIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMessdatumNull() => this.IsNull(this.tableHydBel_2008.MessdatumColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMessdatumNull()
      {
        this[this.tableHydBel_2008.MessdatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennTempNull() => this.IsNull(this.tableHydBel_2008.QtrennTempColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennTempNull()
      {
        this[this.tableHydBel_2008.QtrennTempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennDurchflussNull()
      {
        return this.IsNull(this.tableHydBel_2008.QtrennDurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennDurchflussNull()
      {
        this[this.tableHydBel_2008.QtrennDurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennKalibriervolumenNull()
      {
        return this.IsNull(this.tableHydBel_2008.QtrennKalibriervolumenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennKalibriervolumenNull()
      {
        this[this.tableHydBel_2008.QtrennKalibriervolumenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennMessmengeNull()
      {
        return this.IsNull(this.tableHydBel_2008.QtrennMessmengeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennMessmengeNull()
      {
        this[this.tableHydBel_2008.QtrennMessmengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennTimeNull() => this.IsNull(this.tableHydBel_2008.QtrennTimeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennTimeNull()
      {
        this[this.tableHydBel_2008.QtrennTimeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminTempNull() => this.IsNull(this.tableHydBel_2008.QminTempColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminTempNull() => this[this.tableHydBel_2008.QminTempColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminDurchflussNull() => this.IsNull(this.tableHydBel_2008.QminDurchflussColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminDurchflussNull()
      {
        this[this.tableHydBel_2008.QminDurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminKalibriervolumenNull()
      {
        return this.IsNull(this.tableHydBel_2008.QminKalibriervolumenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminKalibriervolumenNull()
      {
        this[this.tableHydBel_2008.QminKalibriervolumenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminMessmengeNull() => this.IsNull(this.tableHydBel_2008.QminMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminMessmengeNull()
      {
        this[this.tableHydBel_2008.QminMessmengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminTimeNull() => this.IsNull(this.tableHydBel_2008.QminTimeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminTimeNull() => this[this.tableHydBel_2008.QminTimeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennTempNull() => this.IsNull(this.tableHydBel_2008.QnennTempColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennTempNull()
      {
        this[this.tableHydBel_2008.QnennTempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennDurchflussNull()
      {
        return this.IsNull(this.tableHydBel_2008.QnennDurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennDurchflussNull()
      {
        this[this.tableHydBel_2008.QnennDurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennKalibriervolumenNull()
      {
        return this.IsNull(this.tableHydBel_2008.QnennKalibriervolumenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennKalibriervolumenNull()
      {
        this[this.tableHydBel_2008.QnennKalibriervolumenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennMessmengeNull() => this.IsNull(this.tableHydBel_2008.QnennMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennMessmengeNull()
      {
        this[this.tableHydBel_2008.QnennMessmengeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennTimeNull() => this.IsNull(this.tableHydBel_2008.QnennTimeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennTimeNull()
      {
        this[this.tableHydBel_2008.QnennTimeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGeraetepassNull() => this.IsNull(this.tableHydBel_2008.GeraetepassColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGeraetepassNull()
      {
        this[this.tableHydBel_2008.GeraetepassColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBemerkungNull() => this.IsNull(this.tableHydBel_2008.BemerkungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBemerkungNull()
      {
        this[this.tableHydBel_2008.BemerkungColumn] = Convert.DBNull;
      }
    }

    public class HydDNRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable tableHydDN;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydDNRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydDN = (ZR_ClassLibrary.Schema_Mulda.Schema.HydDNDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydDN.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableHydDN.BezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bezeichnung' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.BezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QnMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnMessmenge' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QnMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QtMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtMessmenge' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QtMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QmMessmenge
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QmMessmengeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QmMessmenge' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QmMessmengeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennP
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QnennPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennP' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QnennPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennN
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QnennNColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennN' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QnennNColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennP
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QtrennPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennP' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QtrennPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennN
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QtrennNColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennN' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QtrennNColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminP
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QminPColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminP' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QminPColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminN
      {
        get
        {
          try
          {
            return (double) this[this.tableHydDN.QminNColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminN' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.QminNColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short MidQnenn
      {
        get
        {
          try
          {
            return (short) this[this.tableHydDN.MidQnennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MidQnenn' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.MidQnennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short MidQtrenn
      {
        get
        {
          try
          {
            return (short) this[this.tableHydDN.MidQtrennColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MidQtrenn' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.MidQtrennColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short MidQmin
      {
        get
        {
          try
          {
            return (short) this[this.tableHydDN.MidQminColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MidQmin' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.MidQminColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydDN.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'HydDN' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydDN.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableHydDN.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableHydDN.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBezeichnungNull() => this.IsNull(this.tableHydDN.BezeichnungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBezeichnungNull() => this[this.tableHydDN.BezeichnungColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnMessmengeNull() => this.IsNull(this.tableHydDN.QnMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnMessmengeNull() => this[this.tableHydDN.QnMessmengeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtMessmengeNull() => this.IsNull(this.tableHydDN.QtMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtMessmengeNull() => this[this.tableHydDN.QtMessmengeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQmMessmengeNull() => this.IsNull(this.tableHydDN.QmMessmengeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQmMessmengeNull() => this[this.tableHydDN.QmMessmengeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennPNull() => this.IsNull(this.tableHydDN.QnennPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennPNull() => this[this.tableHydDN.QnennPColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennNNull() => this.IsNull(this.tableHydDN.QnennNColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennNNull() => this[this.tableHydDN.QnennNColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennPNull() => this.IsNull(this.tableHydDN.QtrennPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennPNull() => this[this.tableHydDN.QtrennPColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennNNull() => this.IsNull(this.tableHydDN.QtrennNColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennNNull() => this[this.tableHydDN.QtrennNColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminPNull() => this.IsNull(this.tableHydDN.QminPColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminPNull() => this[this.tableHydDN.QminPColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminNNull() => this.IsNull(this.tableHydDN.QminNColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminNNull() => this[this.tableHydDN.QminNColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMidQnennNull() => this.IsNull(this.tableHydDN.MidQnennColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMidQnennNull() => this[this.tableHydDN.MidQnennColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMidQtrennNull() => this.IsNull(this.tableHydDN.MidQtrennColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMidQtrennNull() => this[this.tableHydDN.MidQtrennColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMidQminNull() => this.IsNull(this.tableHydDN.MidQminColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMidQminNull() => this[this.tableHydDN.MidQminColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableHydDN.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull() => this[this.tableHydDN.TimeStampColumn] = Convert.DBNull;
    }

    public class HydHerstRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable tableHydHerst;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydHerstRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydHerst = (ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableHydHerst.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'HydHerst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydHerst.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableHydHerst.BezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bezeichnung' in table 'HydHerst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydHerst.BezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydHerst.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'HydHerst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydHerst.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableHydHerst.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableHydHerst.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBezeichnungNull() => this.IsNull(this.tableHydHerst.BezeichnungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBezeichnungNull()
      {
        this[this.tableHydHerst.BezeichnungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableHydHerst.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull() => this[this.tableHydHerst.TimeStampColumn] = Convert.DBNull;
    }

    public class HydPos_2008Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable tableHydPos_2008;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydPos_2008Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydPos_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydPos_2008.ChargenNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.ChargenNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short ChargenIndex
      {
        get
        {
          try
          {
            return (short) this[this.tableHydPos_2008.ChargenIndexColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenIndex' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.ChargenIndexColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerienNr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydPos_2008.SerienNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNr' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.SerienNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short SerienNrZaehler
      {
        get
        {
          try
          {
            return (short) this[this.tableHydPos_2008.SerienNrZaehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNrZaehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.SerienNrZaehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short LSNr
      {
        get
        {
          try
          {
            return (short) this[this.tableHydPos_2008.LSNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LSNr' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.LSNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Termin
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydPos_2008.TerminColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Termin' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.TerminColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Geraetestatus
      {
        get
        {
          try
          {
            return (short) this[this.tableHydPos_2008.GeraetestatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Geraetestatus' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.GeraetestatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double BasisImpulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.BasisImpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'BasisImpulswertigkeit' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.BasisImpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.QnennMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennMessfehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.QnennMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.QtrennMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennMessfehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.QtrennMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.QminMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminMessfehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.QminMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Impulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.ImpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Impulswertigkeit' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.ImpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennFehler
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.QnennFehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennFehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.QnennFehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennFehler
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.QtrennFehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennFehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.QtrennFehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminFehler
      {
        get
        {
          try
          {
            return (double) this[this.tableHydPos_2008.QminFehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminFehler' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.QminFehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string InZaehlerNr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydPos_2008.InZaehlerNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InZaehlerNr' in table 'HydPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydPos_2008.InZaehlerNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNrNull() => this.IsNull(this.tableHydPos_2008.ChargenNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNrNull()
      {
        this[this.tableHydPos_2008.ChargenNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenIndexNull() => this.IsNull(this.tableHydPos_2008.ChargenIndexColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenIndexNull()
      {
        this[this.tableHydPos_2008.ChargenIndexColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNrNull() => this.IsNull(this.tableHydPos_2008.SerienNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNrNull() => this[this.tableHydPos_2008.SerienNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNrZaehlerNull()
      {
        return this.IsNull(this.tableHydPos_2008.SerienNrZaehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNrZaehlerNull()
      {
        this[this.tableHydPos_2008.SerienNrZaehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLSNrNull() => this.IsNull(this.tableHydPos_2008.LSNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLSNrNull() => this[this.tableHydPos_2008.LSNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTerminNull() => this.IsNull(this.tableHydPos_2008.TerminColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTerminNull() => this[this.tableHydPos_2008.TerminColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGeraetestatusNull() => this.IsNull(this.tableHydPos_2008.GeraetestatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGeraetestatusNull()
      {
        this[this.tableHydPos_2008.GeraetestatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBasisImpulswertigkeitNull()
      {
        return this.IsNull(this.tableHydPos_2008.BasisImpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBasisImpulswertigkeitNull()
      {
        this[this.tableHydPos_2008.BasisImpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennMessfehlerNull()
      {
        return this.IsNull(this.tableHydPos_2008.QnennMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennMessfehlerNull()
      {
        this[this.tableHydPos_2008.QnennMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennMessfehlerNull()
      {
        return this.IsNull(this.tableHydPos_2008.QtrennMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennMessfehlerNull()
      {
        this[this.tableHydPos_2008.QtrennMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminMessfehlerNull() => this.IsNull(this.tableHydPos_2008.QminMessfehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminMessfehlerNull()
      {
        this[this.tableHydPos_2008.QminMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsImpulswertigkeitNull()
      {
        return this.IsNull(this.tableHydPos_2008.ImpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetImpulswertigkeitNull()
      {
        this[this.tableHydPos_2008.ImpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennFehlerNull() => this.IsNull(this.tableHydPos_2008.QnennFehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennFehlerNull()
      {
        this[this.tableHydPos_2008.QnennFehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennFehlerNull() => this.IsNull(this.tableHydPos_2008.QtrennFehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennFehlerNull()
      {
        this[this.tableHydPos_2008.QtrennFehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminFehlerNull() => this.IsNull(this.tableHydPos_2008.QminFehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminFehlerNull()
      {
        this[this.tableHydPos_2008.QminFehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsInZaehlerNrNull() => this.IsNull(this.tableHydPos_2008.InZaehlerNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetInZaehlerNrNull()
      {
        this[this.tableHydPos_2008.InZaehlerNrColumn] = Convert.DBNull;
      }
    }

    public class HydSNrListeRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable tableHydSNrListe;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal HydSNrListeRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHydSNrListe = (ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerienNr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydSNrListe.SerienNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNr' in table 'HydSNrListe' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydSNrListe.SerienNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short SerienNrZaehler
      {
        get
        {
          try
          {
            return (short) this[this.tableHydSNrListe.SerienNrZaehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNrZaehler' in table 'HydSNrListe' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydSNrListe.SerienNrZaehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr
      {
        get
        {
          try
          {
            return (string) this[this.tableHydSNrListe.ChargenNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr' in table 'HydSNrListe' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydSNrListe.ChargenNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short ChargenIndex
      {
        get
        {
          try
          {
            return (short) this[this.tableHydSNrListe.ChargenIndexColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenIndex' in table 'HydSNrListe' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydSNrListe.ChargenIndexColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Termin
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableHydSNrListe.TerminColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Termin' in table 'HydSNrListe' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydSNrListe.TerminColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Geraetestatus
      {
        get
        {
          try
          {
            return (short) this[this.tableHydSNrListe.GeraetestatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Geraetestatus' in table 'HydSNrListe' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableHydSNrListe.GeraetestatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNrNull() => this.IsNull(this.tableHydSNrListe.SerienNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNrNull() => this[this.tableHydSNrListe.SerienNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNrZaehlerNull()
      {
        return this.IsNull(this.tableHydSNrListe.SerienNrZaehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNrZaehlerNull()
      {
        this[this.tableHydSNrListe.SerienNrZaehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNrNull() => this.IsNull(this.tableHydSNrListe.ChargenNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNrNull()
      {
        this[this.tableHydSNrListe.ChargenNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenIndexNull() => this.IsNull(this.tableHydSNrListe.ChargenIndexColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenIndexNull()
      {
        this[this.tableHydSNrListe.ChargenIndexColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTerminNull() => this.IsNull(this.tableHydSNrListe.TerminColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTerminNull() => this[this.tableHydSNrListe.TerminColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGeraetestatusNull() => this.IsNull(this.tableHydSNrListe.GeraetestatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGeraetestatusNull()
      {
        this[this.tableHydSNrListe.GeraetestatusColumn] = Convert.DBNull;
      }
    }

    public class MIDKalibrierDatenRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable tableMIDKalibrierDaten;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal MIDKalibrierDatenRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMIDKalibrierDaten = (ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableMIDKalibrierDaten.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefstand
      {
        get
        {
          try
          {
            return (string) this[this.tableMIDKalibrierDaten.PruefstandColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefstand' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.PruefstandColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefer
      {
        get
        {
          try
          {
            return (string) this[this.tableMIDKalibrierDaten.PrueferColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefer' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.PrueferColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Kalibrierdatum
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableMIDKalibrierDaten.KalibrierdatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kalibrierdatum' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.KalibrierdatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Temperatur
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.TemperaturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Temperatur' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.TemperaturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID1_Durchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID1_DurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID1_Durchfluss' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID1_DurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID1_Impulse
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID1_ImpulseColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID1_Impulse' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID1_ImpulseColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID1_Temperatur
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID1_TemperaturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID1_Temperatur' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID1_TemperaturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID1_Gewicht
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID1_GewichtColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID1_Gewicht' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID1_GewichtColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID2_Durchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID2_DurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID2_Durchfluss' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID2_DurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID2_Impulse
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID2_ImpulseColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID2_Impulse' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID2_ImpulseColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID2_Temperatur
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID2_TemperaturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID2_Temperatur' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID2_TemperaturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID2_Gewicht
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID2_GewichtColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID2_Gewicht' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID2_GewichtColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID3_Durchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID3_DurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID3_Durchfluss' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID3_DurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID3_Impulse
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID3_ImpulseColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID3_Impulse' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID3_ImpulseColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID3_Temperatur
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID3_TemperaturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID3_Temperatur' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID3_TemperaturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double MID3_Gewicht
      {
        get
        {
          try
          {
            return (double) this[this.tableMIDKalibrierDaten.MID3_GewichtColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MID3_Gewicht' in table 'MIDKalibrierDaten' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMIDKalibrierDaten.MID3_GewichtColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableMIDKalibrierDaten.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableMIDKalibrierDaten.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPruefstandNull() => this.IsNull(this.tableMIDKalibrierDaten.PruefstandColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPruefstandNull()
      {
        this[this.tableMIDKalibrierDaten.PruefstandColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPrueferNull() => this.IsNull(this.tableMIDKalibrierDaten.PrueferColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPrueferNull()
      {
        this[this.tableMIDKalibrierDaten.PrueferColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKalibrierdatumNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.KalibrierdatumColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKalibrierdatumNull()
      {
        this[this.tableMIDKalibrierDaten.KalibrierdatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTemperaturNull() => this.IsNull(this.tableMIDKalibrierDaten.TemperaturColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTemperaturNull()
      {
        this[this.tableMIDKalibrierDaten.TemperaturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID1_DurchflussNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID1_DurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID1_DurchflussNull()
      {
        this[this.tableMIDKalibrierDaten.MID1_DurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID1_ImpulseNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID1_ImpulseColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID1_ImpulseNull()
      {
        this[this.tableMIDKalibrierDaten.MID1_ImpulseColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID1_TemperaturNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID1_TemperaturColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID1_TemperaturNull()
      {
        this[this.tableMIDKalibrierDaten.MID1_TemperaturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID1_GewichtNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID1_GewichtColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID1_GewichtNull()
      {
        this[this.tableMIDKalibrierDaten.MID1_GewichtColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID2_DurchflussNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID2_DurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID2_DurchflussNull()
      {
        this[this.tableMIDKalibrierDaten.MID2_DurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID2_ImpulseNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID2_ImpulseColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID2_ImpulseNull()
      {
        this[this.tableMIDKalibrierDaten.MID2_ImpulseColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID2_TemperaturNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID2_TemperaturColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID2_TemperaturNull()
      {
        this[this.tableMIDKalibrierDaten.MID2_TemperaturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID2_GewichtNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID2_GewichtColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID2_GewichtNull()
      {
        this[this.tableMIDKalibrierDaten.MID2_GewichtColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID3_DurchflussNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID3_DurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID3_DurchflussNull()
      {
        this[this.tableMIDKalibrierDaten.MID3_DurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID3_ImpulseNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID3_ImpulseColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID3_ImpulseNull()
      {
        this[this.tableMIDKalibrierDaten.MID3_ImpulseColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID3_TemperaturNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID3_TemperaturColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID3_TemperaturNull()
      {
        this[this.tableMIDKalibrierDaten.MID3_TemperaturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMID3_GewichtNull()
      {
        return this.IsNull(this.tableMIDKalibrierDaten.MID3_GewichtColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMID3_GewichtNull()
      {
        this[this.tableMIDKalibrierDaten.MID3_GewichtColumn] = Convert.DBNull;
      }
    }

    public class RechenwerkTypRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable tableRechenwerkTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RechenwerkTypRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableRechenwerkTyp = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int TypID
      {
        get
        {
          try
          {
            return (int) this[this.tableRechenwerkTyp.TypIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TypID' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.TypIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Hersteller
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.HerstellerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Hersteller' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.HerstellerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Artikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.ArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Artikelnummer' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.ArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SAPName
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.SAPNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SAPName' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.SAPNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EngelmannTypName
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.EngelmannTypNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EngelmannTypName' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.EngelmannTypNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EngelmannArtikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.EngelmannArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EngelmannArtikelnummer' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.EngelmannArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bauartenzulassung
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.BauartenzulassungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bauartenzulassung' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.BauartenzulassungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Einbaulage
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.EinbaulageColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Einbaulage' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.EinbaulageColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string MetrologischeKlasse
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkTyp.MetrologischeKlasseColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MetrologischeKlasse' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.MetrologischeKlasseColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Nenndurchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableRechenwerkTyp.NenndurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Nenndurchfluss' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.NenndurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableRechenwerkTyp.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'RechenwerkTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkTyp.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTypIDNull() => this.IsNull(this.tableRechenwerkTyp.TypIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTypIDNull() => this[this.tableRechenwerkTyp.TypIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHerstellerNull() => this.IsNull(this.tableRechenwerkTyp.HerstellerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHerstellerNull()
      {
        this[this.tableRechenwerkTyp.HerstellerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsArtikelnummerNull() => this.IsNull(this.tableRechenwerkTyp.ArtikelnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetArtikelnummerNull()
      {
        this[this.tableRechenwerkTyp.ArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSAPNameNull() => this.IsNull(this.tableRechenwerkTyp.SAPNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSAPNameNull() => this[this.tableRechenwerkTyp.SAPNameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEngelmannTypNameNull()
      {
        return this.IsNull(this.tableRechenwerkTyp.EngelmannTypNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEngelmannTypNameNull()
      {
        this[this.tableRechenwerkTyp.EngelmannTypNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEngelmannArtikelnummerNull()
      {
        return this.IsNull(this.tableRechenwerkTyp.EngelmannArtikelnummerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEngelmannArtikelnummerNull()
      {
        this[this.tableRechenwerkTyp.EngelmannArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBauartenzulassungNull()
      {
        return this.IsNull(this.tableRechenwerkTyp.BauartenzulassungColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBauartenzulassungNull()
      {
        this[this.tableRechenwerkTyp.BauartenzulassungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEinbaulageNull() => this.IsNull(this.tableRechenwerkTyp.EinbaulageColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEinbaulageNull()
      {
        this[this.tableRechenwerkTyp.EinbaulageColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMetrologischeKlasseNull()
      {
        return this.IsNull(this.tableRechenwerkTyp.MetrologischeKlasseColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMetrologischeKlasseNull()
      {
        this[this.tableRechenwerkTyp.MetrologischeKlasseColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsNenndurchflussNull()
      {
        return this.IsNull(this.tableRechenwerkTyp.NenndurchflussColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNenndurchflussNull()
      {
        this[this.tableRechenwerkTyp.NenndurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableRechenwerkTyp.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull()
      {
        this[this.tableRechenwerkTyp.TimeStampColumn] = Convert.DBNull;
      }
    }

    public class RechenwerkHerstRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable tableRechenwerkHerst;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RechenwerkHerstRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableRechenwerkHerst = (ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableRechenwerkHerst.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'RechenwerkHerst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkHerst.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableRechenwerkHerst.BezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bezeichnung' in table 'RechenwerkHerst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkHerst.BezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableRechenwerkHerst.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'RechenwerkHerst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRechenwerkHerst.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableRechenwerkHerst.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableRechenwerkHerst.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBezeichnungNull() => this.IsNull(this.tableRechenwerkHerst.BezeichnungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBezeichnungNull()
      {
        this[this.tableRechenwerkHerst.BezeichnungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableRechenwerkHerst.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull()
      {
        this[this.tableRechenwerkHerst.TimeStampColumn] = Convert.DBNull;
      }
    }

    public class RecBel_2008Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable tableRecBel_2008;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RecBel_2008Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableRecBel_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.ChargenNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.ChargenNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int ZaehlerTyp
      {
        get
        {
          try
          {
            return (int) this[this.tableRecBel_2008.ZaehlerTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZaehlerTyp' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.ZaehlerTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Artikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.ArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Artikelnummer' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.ArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SAPName
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.SAPNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SAPName' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.SAPNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Belegstatus
      {
        get
        {
          try
          {
            return (short) this[this.tableRecBel_2008.BelegstatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Belegstatus' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.BelegstatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Auftragsnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.AuftragsnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Auftragsnummer' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.AuftragsnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bauartenzulassung
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.BauartenzulassungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bauartenzulassung' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.BauartenzulassungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string MetrologischeKlasse
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.MetrologischeKlasseColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MetrologischeKlasse' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.MetrologischeKlasseColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Prueftemperatur
      {
        get
        {
          try
          {
            return (double) this[this.tableRecBel_2008.PrueftemperaturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Prueftemperatur' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.PrueftemperaturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Nenndurchfluss
      {
        get
        {
          try
          {
            return (double) this[this.tableRecBel_2008.NenndurchflussColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Nenndurchfluss' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.NenndurchflussColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EngelmannTypName
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.EngelmannTypNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EngelmannTypName' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.EngelmannTypNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EngelmannArtikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.EngelmannArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EngelmannArtikelnummer' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.EngelmannArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefer
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.PrueferColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefer' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.PrueferColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefstellenleitung
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.PruefstellenleitungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefstellenleitung' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.PruefstellenleitungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bemerkung
      {
        get
        {
          try
          {
            return (string) this[this.tableRecBel_2008.BemerkungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bemerkung' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.BemerkungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Pruefdatum
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableRecBel_2008.PruefdatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefdatum' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.PruefdatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Datenuebernahmedatum
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableRecBel_2008.DatenuebernahmedatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Datenuebernahmedatum' in table 'RecBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecBel_2008.DatenuebernahmedatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNrNull() => this.IsNull(this.tableRecBel_2008.ChargenNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNrNull()
      {
        this[this.tableRecBel_2008.ChargenNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZaehlerTypNull() => this.IsNull(this.tableRecBel_2008.ZaehlerTypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZaehlerTypNull()
      {
        this[this.tableRecBel_2008.ZaehlerTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsArtikelnummerNull() => this.IsNull(this.tableRecBel_2008.ArtikelnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetArtikelnummerNull()
      {
        this[this.tableRecBel_2008.ArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSAPNameNull() => this.IsNull(this.tableRecBel_2008.SAPNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSAPNameNull() => this[this.tableRecBel_2008.SAPNameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBelegstatusNull() => this.IsNull(this.tableRecBel_2008.BelegstatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBelegstatusNull()
      {
        this[this.tableRecBel_2008.BelegstatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAuftragsnummerNull() => this.IsNull(this.tableRecBel_2008.AuftragsnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAuftragsnummerNull()
      {
        this[this.tableRecBel_2008.AuftragsnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBauartenzulassungNull()
      {
        return this.IsNull(this.tableRecBel_2008.BauartenzulassungColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBauartenzulassungNull()
      {
        this[this.tableRecBel_2008.BauartenzulassungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMetrologischeKlasseNull()
      {
        return this.IsNull(this.tableRecBel_2008.MetrologischeKlasseColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMetrologischeKlasseNull()
      {
        this[this.tableRecBel_2008.MetrologischeKlasseColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPrueftemperaturNull()
      {
        return this.IsNull(this.tableRecBel_2008.PrueftemperaturColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPrueftemperaturNull()
      {
        this[this.tableRecBel_2008.PrueftemperaturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsNenndurchflussNull() => this.IsNull(this.tableRecBel_2008.NenndurchflussColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNenndurchflussNull()
      {
        this[this.tableRecBel_2008.NenndurchflussColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEngelmannTypNameNull()
      {
        return this.IsNull(this.tableRecBel_2008.EngelmannTypNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEngelmannTypNameNull()
      {
        this[this.tableRecBel_2008.EngelmannTypNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEngelmannArtikelnummerNull()
      {
        return this.IsNull(this.tableRecBel_2008.EngelmannArtikelnummerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEngelmannArtikelnummerNull()
      {
        this[this.tableRecBel_2008.EngelmannArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPrueferNull() => this.IsNull(this.tableRecBel_2008.PrueferColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPrueferNull() => this[this.tableRecBel_2008.PrueferColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPruefstellenleitungNull()
      {
        return this.IsNull(this.tableRecBel_2008.PruefstellenleitungColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPruefstellenleitungNull()
      {
        this[this.tableRecBel_2008.PruefstellenleitungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBemerkungNull() => this.IsNull(this.tableRecBel_2008.BemerkungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBemerkungNull()
      {
        this[this.tableRecBel_2008.BemerkungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPruefdatumNull() => this.IsNull(this.tableRecBel_2008.PruefdatumColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPruefdatumNull()
      {
        this[this.tableRecBel_2008.PruefdatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDatenuebernahmedatumNull()
      {
        return this.IsNull(this.tableRecBel_2008.DatenuebernahmedatumColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDatenuebernahmedatumNull()
      {
        this[this.tableRecBel_2008.DatenuebernahmedatumColumn] = Convert.DBNull;
      }
    }

    public class RecPos_2008Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable tableRecPos_2008;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal RecPos_2008Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableRecPos_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr
      {
        get
        {
          try
          {
            return (string) this[this.tableRecPos_2008.ChargenNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.ChargenNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short ChargenIndex
      {
        get
        {
          try
          {
            return (short) this[this.tableRecPos_2008.ChargenIndexColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenIndex' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.ChargenIndexColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerienNr
      {
        get
        {
          try
          {
            return (string) this[this.tableRecPos_2008.SerienNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNr' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.SerienNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Geraetestatus
      {
        get
        {
          try
          {
            return (short) this[this.tableRecPos_2008.GeraetestatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Geraetestatus' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.GeraetestatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminVorlaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTminVorlaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminVorlaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTminVorlaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminRuecklaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTminRuecklaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminRuecklaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTminRuecklaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminKFaktor
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTminKFaktorColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminKFaktor' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTminKFaktorColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminSollwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTminSollwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminSollwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTminSollwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminIstwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTminIstwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminIstwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTminIstwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTminMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminMessfehler' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTminMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennVorlaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTnennVorlaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennVorlaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTnennVorlaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennRuecklaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTnennRuecklaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennRuecklaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTnennRuecklaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennKFaktor
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTnennKFaktorColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennKFaktor' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTnennKFaktorColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennSollwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTnennSollwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennSollwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTnennSollwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennIstwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTnennIstwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennIstwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTnennIstwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTnennMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennMessfehler' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTnennMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxVorlaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTmaxVorlaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxVorlaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTmaxVorlaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxRuecklaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTmaxRuecklaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxRuecklaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTmaxRuecklaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxKFaktor
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTmaxKFaktorColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxKFaktor' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTmaxKFaktorColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxSollwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTmaxSollwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxSollwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTmaxSollwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxIstwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTmaxIstwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxIstwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTmaxIstwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdTmaxMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxMessfehler' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdTmaxMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdRZWVorlaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdRZWVorlaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdRZWVorlaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdRZWVorlaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdRZWRuecklaufbadtemp
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdRZWRuecklaufbadtempColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdRZWRuecklaufbadtemp' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdRZWRuecklaufbadtempColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdRZWKFaktor
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdRZWKFaktorColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdRZWKFaktor' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdRZWKFaktorColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdRZWSollwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdRZWSollwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdRZWSollwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdRZWSollwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdRZWIstwert
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdRZWIstwertColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdRZWIstwert' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdRZWIstwertColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdRZWMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableRecPos_2008.QdRZWMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdRZWMessfehler' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.QdRZWMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string InZaehlerNr
      {
        get
        {
          try
          {
            return (string) this[this.tableRecPos_2008.InZaehlerNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InZaehlerNr' in table 'RecPos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableRecPos_2008.InZaehlerNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNrNull() => this.IsNull(this.tableRecPos_2008.ChargenNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNrNull()
      {
        this[this.tableRecPos_2008.ChargenNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenIndexNull() => this.IsNull(this.tableRecPos_2008.ChargenIndexColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenIndexNull()
      {
        this[this.tableRecPos_2008.ChargenIndexColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNrNull() => this.IsNull(this.tableRecPos_2008.SerienNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNrNull() => this[this.tableRecPos_2008.SerienNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGeraetestatusNull() => this.IsNull(this.tableRecPos_2008.GeraetestatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGeraetestatusNull()
      {
        this[this.tableRecPos_2008.GeraetestatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminVorlaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTminVorlaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminVorlaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdTminVorlaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminRuecklaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTminRuecklaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminRuecklaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdTminRuecklaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminKFaktorNull() => this.IsNull(this.tableRecPos_2008.QdTminKFaktorColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminKFaktorNull()
      {
        this[this.tableRecPos_2008.QdTminKFaktorColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminSollwertNull() => this.IsNull(this.tableRecPos_2008.QdTminSollwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminSollwertNull()
      {
        this[this.tableRecPos_2008.QdTminSollwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminIstwertNull() => this.IsNull(this.tableRecPos_2008.QdTminIstwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminIstwertNull()
      {
        this[this.tableRecPos_2008.QdTminIstwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminMessfehlerNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTminMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminMessfehlerNull()
      {
        this[this.tableRecPos_2008.QdTminMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennVorlaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTnennVorlaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennVorlaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdTnennVorlaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennRuecklaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTnennRuecklaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennRuecklaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdTnennRuecklaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennKFaktorNull() => this.IsNull(this.tableRecPos_2008.QdTnennKFaktorColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennKFaktorNull()
      {
        this[this.tableRecPos_2008.QdTnennKFaktorColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennSollwertNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTnennSollwertColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennSollwertNull()
      {
        this[this.tableRecPos_2008.QdTnennSollwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennIstwertNull() => this.IsNull(this.tableRecPos_2008.QdTnennIstwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennIstwertNull()
      {
        this[this.tableRecPos_2008.QdTnennIstwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennMessfehlerNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTnennMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennMessfehlerNull()
      {
        this[this.tableRecPos_2008.QdTnennMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxVorlaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTmaxVorlaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxVorlaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdTmaxVorlaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxRuecklaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTmaxRuecklaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxRuecklaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdTmaxRuecklaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxKFaktorNull() => this.IsNull(this.tableRecPos_2008.QdTmaxKFaktorColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxKFaktorNull()
      {
        this[this.tableRecPos_2008.QdTmaxKFaktorColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxSollwertNull() => this.IsNull(this.tableRecPos_2008.QdTmaxSollwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxSollwertNull()
      {
        this[this.tableRecPos_2008.QdTmaxSollwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxIstwertNull() => this.IsNull(this.tableRecPos_2008.QdTmaxIstwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxIstwertNull()
      {
        this[this.tableRecPos_2008.QdTmaxIstwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxMessfehlerNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdTmaxMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxMessfehlerNull()
      {
        this[this.tableRecPos_2008.QdTmaxMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdRZWVorlaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdRZWVorlaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdRZWVorlaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdRZWVorlaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdRZWRuecklaufbadtempNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdRZWRuecklaufbadtempColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdRZWRuecklaufbadtempNull()
      {
        this[this.tableRecPos_2008.QdRZWRuecklaufbadtempColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdRZWKFaktorNull() => this.IsNull(this.tableRecPos_2008.QdRZWKFaktorColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdRZWKFaktorNull()
      {
        this[this.tableRecPos_2008.QdRZWKFaktorColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdRZWSollwertNull() => this.IsNull(this.tableRecPos_2008.QdRZWSollwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdRZWSollwertNull()
      {
        this[this.tableRecPos_2008.QdRZWSollwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdRZWIstwertNull() => this.IsNull(this.tableRecPos_2008.QdRZWIstwertColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdRZWIstwertNull()
      {
        this[this.tableRecPos_2008.QdRZWIstwertColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdRZWMessfehlerNull()
      {
        return this.IsNull(this.tableRecPos_2008.QdRZWMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdRZWMessfehlerNull()
      {
        this[this.tableRecPos_2008.QdRZWMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsInZaehlerNrNull() => this.IsNull(this.tableRecPos_2008.InZaehlerNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetInZaehlerNrNull()
      {
        this[this.tableRecPos_2008.InZaehlerNrColumn] = Convert.DBNull;
      }
    }

    public class TypNamenReportRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable tableTypNamenReport;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal TypNamenReportRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableTypNamenReport = (ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short TYP_ID
      {
        get
        {
          try
          {
            return (short) this[this.tableTypNamenReport.TYP_IDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TYP_ID' in table 'TypNamenReport' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableTypNamenReport.TYP_IDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string BerichtName
      {
        get
        {
          try
          {
            return (string) this[this.tableTypNamenReport.BerichtNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'BerichtName' in table 'TypNamenReport' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableTypNamenReport.BerichtNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTYP_IDNull() => this.IsNull(this.tableTypNamenReport.TYP_IDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTYP_IDNull() => this[this.tableTypNamenReport.TYP_IDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBerichtNameNull() => this.IsNull(this.tableTypNamenReport.BerichtNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBerichtNameNull()
      {
        this[this.tableTypNamenReport.BerichtNameColumn] = Convert.DBNull;
      }
    }

    public class useraccRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable tableuseracc;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal useraccRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableuseracc = (ZR_ClassLibrary.Schema_Mulda.Schema.useraccDataTable) this.Table;
      }
    }

    public class usersRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable tableusers;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal usersRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableusers = (ZR_ClassLibrary.Schema_Mulda.Schema.usersDataTable) this.Table;
      }
    }

    public class VersionsUpdateRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable tableVersionsUpdate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal VersionsUpdateRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableVersionsUpdate = (ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string FileName
      {
        get
        {
          try
          {
            return (string) this[this.tableVersionsUpdate.FileNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FileName' in table 'VersionsUpdate' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableVersionsUpdate.FileNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableVersionsUpdate.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'VersionsUpdate' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableVersionsUpdate.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public byte[] File
      {
        get
        {
          try
          {
            return (byte[]) this[this.tableVersionsUpdate.FileColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'File' in table 'VersionsUpdate' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableVersionsUpdate.FileColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Kz_Neu
      {
        get
        {
          try
          {
            return (bool) this[this.tableVersionsUpdate.Kz_NeuColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Kz_Neu' in table 'VersionsUpdate' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableVersionsUpdate.Kz_NeuColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public long CheckSumme
      {
        get
        {
          try
          {
            return (long) this[this.tableVersionsUpdate.CheckSummeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CheckSumme' in table 'VersionsUpdate' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableVersionsUpdate.CheckSummeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AppName
      {
        get
        {
          try
          {
            return (string) this[this.tableVersionsUpdate.AppNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AppName' in table 'VersionsUpdate' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableVersionsUpdate.AppNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFileNameNull() => this.IsNull(this.tableVersionsUpdate.FileNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFileNameNull()
      {
        this[this.tableVersionsUpdate.FileNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableVersionsUpdate.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull()
      {
        this[this.tableVersionsUpdate.TimeStampColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFileNull() => this.IsNull(this.tableVersionsUpdate.FileColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFileNull() => this[this.tableVersionsUpdate.FileColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsKz_NeuNull() => this.IsNull(this.tableVersionsUpdate.Kz_NeuColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetKz_NeuNull() => this[this.tableVersionsUpdate.Kz_NeuColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCheckSummeNull() => this.IsNull(this.tableVersionsUpdate.CheckSummeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCheckSummeNull()
      {
        this[this.tableVersionsUpdate.CheckSummeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAppNameNull() => this.IsNull(this.tableVersionsUpdate.AppNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAppNameNull() => this[this.tableVersionsUpdate.AppNameColumn] = Convert.DBNull;
    }

    public class ZaehlerTypRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable tableZaehlerTyp;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaehlerTypRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableZaehlerTyp = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int TypID
      {
        get
        {
          try
          {
            return (int) this[this.tableZaehlerTyp.TypIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TypID' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.TypIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Typ
      {
        get
        {
          try
          {
            return (string) this[this.tableZaehlerTyp.TypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Typ' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.TypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Artikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableZaehlerTyp.ArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Artikelnummer' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.ArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EAN_Nr
      {
        get
        {
          try
          {
            return (string) this[this.tableZaehlerTyp.EAN_NrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EAN_Nr' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.EAN_NrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Hersteller
      {
        get
        {
          try
          {
            return (string) this[this.tableZaehlerTyp.HerstellerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Hersteller' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.HerstellerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Etikettenbezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableZaehlerTyp.EtikettenbezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Etikettenbezeichnung' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.EtikettenbezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Fuehlerlaenge_VL
      {
        get
        {
          try
          {
            return (double) this[this.tableZaehlerTyp.Fuehlerlaenge_VLColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Fuehlerlaenge_VL' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.Fuehlerlaenge_VLColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Fuehlerlaenge_RL
      {
        get
        {
          try
          {
            return (double) this[this.tableZaehlerTyp.Fuehlerlaenge_RLColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Fuehlerlaenge_RL' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.Fuehlerlaenge_RLColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Hyd_TypID
      {
        get
        {
          try
          {
            return (int) this[this.tableZaehlerTyp.Hyd_TypIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Hyd_TypID' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.Hyd_TypIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int Rechenwerk_TypID
      {
        get
        {
          try
          {
            return (int) this[this.tableZaehlerTyp.Rechenwerk_TypIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Rechenwerk_TypID' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.Rechenwerk_TypIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableZaehlerTyp.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'ZaehlerTyp' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlerTyp.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTypIDNull() => this.IsNull(this.tableZaehlerTyp.TypIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTypIDNull() => this[this.tableZaehlerTyp.TypIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTypNull() => this.IsNull(this.tableZaehlerTyp.TypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTypNull() => this[this.tableZaehlerTyp.TypColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsArtikelnummerNull() => this.IsNull(this.tableZaehlerTyp.ArtikelnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetArtikelnummerNull()
      {
        this[this.tableZaehlerTyp.ArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEAN_NrNull() => this.IsNull(this.tableZaehlerTyp.EAN_NrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEAN_NrNull() => this[this.tableZaehlerTyp.EAN_NrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHerstellerNull() => this.IsNull(this.tableZaehlerTyp.HerstellerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHerstellerNull()
      {
        this[this.tableZaehlerTyp.HerstellerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEtikettenbezeichnungNull()
      {
        return this.IsNull(this.tableZaehlerTyp.EtikettenbezeichnungColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEtikettenbezeichnungNull()
      {
        this[this.tableZaehlerTyp.EtikettenbezeichnungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFuehlerlaenge_VLNull()
      {
        return this.IsNull(this.tableZaehlerTyp.Fuehlerlaenge_VLColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFuehlerlaenge_VLNull()
      {
        this[this.tableZaehlerTyp.Fuehlerlaenge_VLColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsFuehlerlaenge_RLNull()
      {
        return this.IsNull(this.tableZaehlerTyp.Fuehlerlaenge_RLColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetFuehlerlaenge_RLNull()
      {
        this[this.tableZaehlerTyp.Fuehlerlaenge_RLColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHyd_TypIDNull() => this.IsNull(this.tableZaehlerTyp.Hyd_TypIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHyd_TypIDNull() => this[this.tableZaehlerTyp.Hyd_TypIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRechenwerk_TypIDNull()
      {
        return this.IsNull(this.tableZaehlerTyp.Rechenwerk_TypIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRechenwerk_TypIDNull()
      {
        this[this.tableZaehlerTyp.Rechenwerk_TypIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableZaehlerTyp.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull() => this[this.tableZaehlerTyp.TimeStampColumn] = Convert.DBNull;
    }

    public class ZaehlertypherstRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable tableZaehlertypherst;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaehlertypherstRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableZaehlertypherst = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int RecID
      {
        get
        {
          try
          {
            return (int) this[this.tableZaehlertypherst.RecIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RecID' in table 'Zaehlertypherst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlertypherst.RecIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bezeichnung
      {
        get
        {
          try
          {
            return (string) this[this.tableZaehlertypherst.BezeichnungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bezeichnung' in table 'Zaehlertypherst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlertypherst.BezeichnungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime TimeStamp
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableZaehlertypherst.TimeStampColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'TimeStamp' in table 'Zaehlertypherst' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaehlertypherst.TimeStampColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRecIDNull() => this.IsNull(this.tableZaehlertypherst.RecIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRecIDNull() => this[this.tableZaehlertypherst.RecIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBezeichnungNull() => this.IsNull(this.tableZaehlertypherst.BezeichnungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBezeichnungNull()
      {
        this[this.tableZaehlertypherst.BezeichnungColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsTimeStampNull() => this.IsNull(this.tableZaehlertypherst.TimeStampColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetTimeStampNull()
      {
        this[this.tableZaehlertypherst.TimeStampColumn] = Convert.DBNull;
      }
    }

    public class ZaeBel_2008Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable tableZaeBel_2008;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaeBel_2008Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableZaeBel_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AuftragsNr
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.AuftragsNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AuftragsNr' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.AuftragsNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int ZaehlerTyp
      {
        get
        {
          try
          {
            return (int) this[this.tableZaeBel_2008.ZaehlerTypColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZaehlerTyp' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.ZaehlerTypColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Artikelnummer
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.ArtikelnummerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Artikelnummer' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.ArtikelnummerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EAN_Nr
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.EAN_NrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EAN_Nr' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.EAN_NrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Belegstatus
      {
        get
        {
          try
          {
            return (short) this[this.tableZaeBel_2008.BelegstatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Belegstatus' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.BelegstatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Montagedatum
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableZaeBel_2008.MontagedatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Montagedatum' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.MontagedatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Impulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableZaeBel_2008.ImpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Impulswertigkeit' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.ImpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Herstelljahr
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.HerstelljahrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Herstelljahr' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.HerstelljahrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefstand
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.PruefstandColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefstand' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.PruefstandColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Pruefer
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.PrueferColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Pruefer' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.PrueferColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bemerkung
      {
        get
        {
          try
          {
            return (string) this[this.tableZaeBel_2008.BemerkungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bemerkung' in table 'ZaeBel_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaeBel_2008.BemerkungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAuftragsNrNull() => this.IsNull(this.tableZaeBel_2008.AuftragsNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAuftragsNrNull()
      {
        this[this.tableZaeBel_2008.AuftragsNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsZaehlerTypNull() => this.IsNull(this.tableZaeBel_2008.ZaehlerTypColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetZaehlerTypNull()
      {
        this[this.tableZaeBel_2008.ZaehlerTypColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsArtikelnummerNull() => this.IsNull(this.tableZaeBel_2008.ArtikelnummerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetArtikelnummerNull()
      {
        this[this.tableZaeBel_2008.ArtikelnummerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEAN_NrNull() => this.IsNull(this.tableZaeBel_2008.EAN_NrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEAN_NrNull() => this[this.tableZaeBel_2008.EAN_NrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBelegstatusNull() => this.IsNull(this.tableZaeBel_2008.BelegstatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBelegstatusNull()
      {
        this[this.tableZaeBel_2008.BelegstatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMontagedatumNull() => this.IsNull(this.tableZaeBel_2008.MontagedatumColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMontagedatumNull()
      {
        this[this.tableZaeBel_2008.MontagedatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsImpulswertigkeitNull()
      {
        return this.IsNull(this.tableZaeBel_2008.ImpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetImpulswertigkeitNull()
      {
        this[this.tableZaeBel_2008.ImpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHerstelljahrNull() => this.IsNull(this.tableZaeBel_2008.HerstelljahrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHerstelljahrNull()
      {
        this[this.tableZaeBel_2008.HerstelljahrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPruefstandNull() => this.IsNull(this.tableZaeBel_2008.PruefstandColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPruefstandNull()
      {
        this[this.tableZaeBel_2008.PruefstandColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPrueferNull() => this.IsNull(this.tableZaeBel_2008.PrueferColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPrueferNull() => this[this.tableZaeBel_2008.PrueferColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBemerkungNull() => this.IsNull(this.tableZaeBel_2008.BemerkungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBemerkungNull()
      {
        this[this.tableZaeBel_2008.BemerkungColumn] = Convert.DBNull;
      }
    }

    public class ZaePos_2008Row : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable tableZaePos_2008;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal ZaePos_2008Row(DataRowBuilder rb)
        : base(rb)
      {
        this.tableZaePos_2008 = (ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008DataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AuftragsNr
      {
        get
        {
          try
          {
            return (string) this[this.tableZaePos_2008.AuftragsNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AuftragsNr' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.AuftragsNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short AuftragsIndex
      {
        get
        {
          try
          {
            return (short) this[this.tableZaePos_2008.AuftragsIndexColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AuftragsIndex' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.AuftragsIndexColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerienNr
      {
        get
        {
          try
          {
            return (string) this[this.tableZaePos_2008.SerienNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNr' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.SerienNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Montagedatum
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableZaePos_2008.MontagedatumColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Montagedatum' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.MontagedatumColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public short Geraetestatus
      {
        get
        {
          try
          {
            return (short) this[this.tableZaePos_2008.GeraetestatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Geraetestatus' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.GeraetestatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerienNr_Hydraulik
      {
        get
        {
          try
          {
            return (string) this[this.tableZaePos_2008.SerienNr_HydraulikColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNr_Hydraulik' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.SerienNr_HydraulikColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool HydraulikDatenVorhanden
      {
        get
        {
          try
          {
            return (bool) this[this.tableZaePos_2008.HydraulikDatenVorhandenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HydraulikDatenVorhanden' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.HydraulikDatenVorhandenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SerienNr_Rechenwerk
      {
        get
        {
          try
          {
            return (string) this[this.tableZaePos_2008.SerienNr_RechenwerkColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerienNr_Rechenwerk' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.SerienNr_RechenwerkColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool RechenwerkDatenVorhanden
      {
        get
        {
          try
          {
            return (bool) this[this.tableZaePos_2008.RechenwerkDatenVorhandenColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RechenwerkDatenVorhanden' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.RechenwerkDatenVorhandenColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double BasisImpulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.BasisImpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'BasisImpulswertigkeit' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.BasisImpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QtrennMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennMessfehler' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QtrennMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QminMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminMessfehler' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QminMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennMessfehler
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QnennMessfehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennMessfehler' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QnennMessfehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double Impulswertigkeit
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.ImpulswertigkeitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Impulswertigkeit' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.ImpulswertigkeitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QtrennFehler
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QtrennFehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QtrennFehler' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QtrennFehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QminFehler
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QminFehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QminFehler' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QminFehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QnennFehler
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QnennFehlerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QnennFehler' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QnennFehlerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTminFehler_Rec
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QdTminFehler_RecColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTminFehler_Rec' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QdTminFehler_RecColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTnennFehler_Rec
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QdTnennFehler_RecColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTnennFehler_Rec' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QdTnennFehler_RecColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public double QdTmaxFehler_Rec
      {
        get
        {
          try
          {
            return (double) this[this.tableZaePos_2008.QdTmaxFehler_RecColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'QdTmaxFehler_Rec' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.QdTmaxFehler_RecColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr_Hyd
      {
        get
        {
          try
          {
            return (string) this[this.tableZaePos_2008.ChargenNr_HydColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr_Hyd' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.ChargenNr_HydColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChargenNr_Rec
      {
        get
        {
          try
          {
            return (string) this[this.tableZaePos_2008.ChargenNr_RecColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChargenNr_Rec' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.ChargenNr_RecColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Gedruckt
      {
        get
        {
          try
          {
            return (bool) this[this.tableZaePos_2008.GedrucktColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Gedruckt' in table 'ZaePos_2008' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZaePos_2008.GedrucktColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAuftragsNrNull() => this.IsNull(this.tableZaePos_2008.AuftragsNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAuftragsNrNull()
      {
        this[this.tableZaePos_2008.AuftragsNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAuftragsIndexNull() => this.IsNull(this.tableZaePos_2008.AuftragsIndexColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAuftragsIndexNull()
      {
        this[this.tableZaePos_2008.AuftragsIndexColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNrNull() => this.IsNull(this.tableZaePos_2008.SerienNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNrNull() => this[this.tableZaePos_2008.SerienNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMontagedatumNull() => this.IsNull(this.tableZaePos_2008.MontagedatumColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMontagedatumNull()
      {
        this[this.tableZaePos_2008.MontagedatumColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGeraetestatusNull() => this.IsNull(this.tableZaePos_2008.GeraetestatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGeraetestatusNull()
      {
        this[this.tableZaePos_2008.GeraetestatusColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNr_HydraulikNull()
      {
        return this.IsNull(this.tableZaePos_2008.SerienNr_HydraulikColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNr_HydraulikNull()
      {
        this[this.tableZaePos_2008.SerienNr_HydraulikColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsHydraulikDatenVorhandenNull()
      {
        return this.IsNull(this.tableZaePos_2008.HydraulikDatenVorhandenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetHydraulikDatenVorhandenNull()
      {
        this[this.tableZaePos_2008.HydraulikDatenVorhandenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSerienNr_RechenwerkNull()
      {
        return this.IsNull(this.tableZaePos_2008.SerienNr_RechenwerkColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSerienNr_RechenwerkNull()
      {
        this[this.tableZaePos_2008.SerienNr_RechenwerkColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsRechenwerkDatenVorhandenNull()
      {
        return this.IsNull(this.tableZaePos_2008.RechenwerkDatenVorhandenColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetRechenwerkDatenVorhandenNull()
      {
        this[this.tableZaePos_2008.RechenwerkDatenVorhandenColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBasisImpulswertigkeitNull()
      {
        return this.IsNull(this.tableZaePos_2008.BasisImpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBasisImpulswertigkeitNull()
      {
        this[this.tableZaePos_2008.BasisImpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennMessfehlerNull()
      {
        return this.IsNull(this.tableZaePos_2008.QtrennMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennMessfehlerNull()
      {
        this[this.tableZaePos_2008.QtrennMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminMessfehlerNull() => this.IsNull(this.tableZaePos_2008.QminMessfehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminMessfehlerNull()
      {
        this[this.tableZaePos_2008.QminMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennMessfehlerNull()
      {
        return this.IsNull(this.tableZaePos_2008.QnennMessfehlerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennMessfehlerNull()
      {
        this[this.tableZaePos_2008.QnennMessfehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsImpulswertigkeitNull()
      {
        return this.IsNull(this.tableZaePos_2008.ImpulswertigkeitColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetImpulswertigkeitNull()
      {
        this[this.tableZaePos_2008.ImpulswertigkeitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQtrennFehlerNull() => this.IsNull(this.tableZaePos_2008.QtrennFehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQtrennFehlerNull()
      {
        this[this.tableZaePos_2008.QtrennFehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQminFehlerNull() => this.IsNull(this.tableZaePos_2008.QminFehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQminFehlerNull()
      {
        this[this.tableZaePos_2008.QminFehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQnennFehlerNull() => this.IsNull(this.tableZaePos_2008.QnennFehlerColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQnennFehlerNull()
      {
        this[this.tableZaePos_2008.QnennFehlerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTminFehler_RecNull()
      {
        return this.IsNull(this.tableZaePos_2008.QdTminFehler_RecColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTminFehler_RecNull()
      {
        this[this.tableZaePos_2008.QdTminFehler_RecColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTnennFehler_RecNull()
      {
        return this.IsNull(this.tableZaePos_2008.QdTnennFehler_RecColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTnennFehler_RecNull()
      {
        this[this.tableZaePos_2008.QdTnennFehler_RecColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsQdTmaxFehler_RecNull()
      {
        return this.IsNull(this.tableZaePos_2008.QdTmaxFehler_RecColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetQdTmaxFehler_RecNull()
      {
        this[this.tableZaePos_2008.QdTmaxFehler_RecColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNr_HydNull() => this.IsNull(this.tableZaePos_2008.ChargenNr_HydColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNr_HydNull()
      {
        this[this.tableZaePos_2008.ChargenNr_HydColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChargenNr_RecNull() => this.IsNull(this.tableZaePos_2008.ChargenNr_RecColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChargenNr_RecNull()
      {
        this[this.tableZaePos_2008.ChargenNr_RecColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsGedrucktNull() => this.IsNull(this.tableZaePos_2008.GedrucktColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetGedrucktNull() => this[this.tableZaePos_2008.GedrucktColumn] = Convert.DBNull;
    }

    public class DateilisteRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable tableDateiliste;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal DateilisteRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableDateiliste = (ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Dateiname
      {
        get
        {
          try
          {
            return (string) this[this.tableDateiliste.DateinameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Dateiname' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.DateinameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Bemerkung
      {
        get
        {
          try
          {
            return (string) this[this.tableDateiliste.BemerkungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Bemerkung' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.BemerkungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ushort UserMajorVersion
      {
        get
        {
          try
          {
            return (ushort) this[this.tableDateiliste.UserMajorVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'UserMajorVersion' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.UserMajorVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ushort UserMinorVersion
      {
        get
        {
          try
          {
            return (ushort) this[this.tableDateiliste.UserMinorVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'UserMinorVersion' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.UserMinorVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string DBStruktur
      {
        get
        {
          try
          {
            return (string) this[this.tableDateiliste.DBStrukturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DBStruktur' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.DBStrukturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string DBZwischenstruktur
      {
        get
        {
          try
          {
            return (string) this[this.tableDateiliste.DBZwischenstrukturColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DBZwischenstruktur' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.DBZwischenstrukturColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool AktiviereZwischenschritt
      {
        get
        {
          try
          {
            return (bool) this[this.tableDateiliste.AktiviereZwischenschrittColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AktiviereZwischenschritt' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.AktiviereZwischenschrittColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool Autocreate
      {
        get
        {
          try
          {
            return (bool) this[this.tableDateiliste.AutocreateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Autocreate' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.AutocreateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime Letzte_Strukturaenderung
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableDateiliste.Letzte_StrukturaenderungColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Letzte_Strukturaenderung' in table 'Dateiliste' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDateiliste.Letzte_StrukturaenderungColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDateinameNull() => this.IsNull(this.tableDateiliste.DateinameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDateinameNull() => this[this.tableDateiliste.DateinameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBemerkungNull() => this.IsNull(this.tableDateiliste.BemerkungColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBemerkungNull() => this[this.tableDateiliste.BemerkungColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsUserMajorVersionNull()
      {
        return this.IsNull(this.tableDateiliste.UserMajorVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetUserMajorVersionNull()
      {
        this[this.tableDateiliste.UserMajorVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsUserMinorVersionNull()
      {
        return this.IsNull(this.tableDateiliste.UserMinorVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetUserMinorVersionNull()
      {
        this[this.tableDateiliste.UserMinorVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDBStrukturNull() => this.IsNull(this.tableDateiliste.DBStrukturColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDBStrukturNull()
      {
        this[this.tableDateiliste.DBStrukturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDBZwischenstrukturNull()
      {
        return this.IsNull(this.tableDateiliste.DBZwischenstrukturColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDBZwischenstrukturNull()
      {
        this[this.tableDateiliste.DBZwischenstrukturColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAktiviereZwischenschrittNull()
      {
        return this.IsNull(this.tableDateiliste.AktiviereZwischenschrittColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAktiviereZwischenschrittNull()
      {
        this[this.tableDateiliste.AktiviereZwischenschrittColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAutocreateNull() => this.IsNull(this.tableDateiliste.AutocreateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAutocreateNull()
      {
        this[this.tableDateiliste.AutocreateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLetzte_StrukturaenderungNull()
      {
        return this.IsNull(this.tableDateiliste.Letzte_StrukturaenderungColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLetzte_StrukturaenderungNull()
      {
        this[this.tableDateiliste.Letzte_StrukturaenderungColumn] = Convert.DBNull;
      }
    }

    public class S_VersionRow : DataRow
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable tableS_Version;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal S_VersionRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableS_Version = (ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Version
      {
        get
        {
          try
          {
            return (string) this[this.tableS_Version.VersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Version' in table 'S_Version' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableS_Version.VersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SQL
      {
        get
        {
          try
          {
            return (string) this[this.tableS_Version.SQLColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SQL' in table 'S_Version' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableS_Version.SQLColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool ExecSQL
      {
        get
        {
          try
          {
            return (bool) this[this.tableS_Version.ExecSQLColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ExecSQL' in table 'S_Version' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableS_Version.ExecSQLColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsVersionNull() => this.IsNull(this.tableS_Version.VersionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetVersionNull() => this[this.tableS_Version.VersionColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSQLNull() => this.IsNull(this.tableS_Version.SQLColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSQLNull() => this[this.tableS_Version.SQLColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsExecSQLNull() => this.IsNull(this.tableS_Version.ExecSQLColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetExecSQLNull() => this[this.tableS_Version.ExecSQLColumn] = Convert.DBNull;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class accessRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.accessRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public accessRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.accessRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.accessRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class DichteRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DichteRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DichteRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class EdrbsqlRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EdrbsqlRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.EdrbsqlRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class FormulareRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public FormulareRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.FormulareRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class groupsRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public groupsRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.groupsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class grpcollRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public grpcollRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.grpcollRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydraulikTypRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydraulikTypRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydraulikTypRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydAblaufsteuerungRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydAblaufsteuerungRowChangeEvent(
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerungRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydAblaufsteuerung1RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydAblaufsteuerung1RowChangeEvent(
        ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAblaufsteuerung1Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydAGewRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydAGewRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydAGewRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydBel_2008RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydBel_2008RowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydBel_2008Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydDNRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydDNRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydDNRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydHerstRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydHerstRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydHerstRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydPos_2008RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydPos_2008RowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydPos_2008Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class HydSNrListeRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public HydSNrListeRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.HydSNrListeRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class MIDKalibrierDatenRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public MIDKalibrierDatenRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.MIDKalibrierDatenRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class RechenwerkTypRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RechenwerkTypRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkTypRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class RechenwerkHerstRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RechenwerkHerstRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RechenwerkHerstRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class RecBel_2008RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RecBel_2008RowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecBel_2008Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class RecPos_2008RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public RecPos_2008RowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.RecPos_2008Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class TypNamenReportRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TypNamenReportRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.TypNamenReportRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class useraccRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public useraccRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.useraccRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class usersRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.usersRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public usersRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.usersRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.usersRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class VersionsUpdateRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public VersionsUpdateRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.VersionsUpdateRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class ZaehlerTypRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaehlerTypRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlerTypRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class ZaehlertypherstRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaehlertypherstRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaehlertypherstRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class ZaeBel_2008RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaeBel_2008RowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaeBel_2008Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class ZaePos_2008RowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZaePos_2008RowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.ZaePos_2008Row Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class DateilisteRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateilisteRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.DateilisteRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class S_VersionRowChangeEvent : EventArgs
    {
      private ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public S_VersionRowChangeEvent(ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public ZR_ClassLibrary.Schema_Mulda.Schema.S_VersionRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
