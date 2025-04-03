// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DataSets.BaseTables
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
  [XmlRoot("BaseTables")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class BaseTables : DataSet
  {
    private BaseTables.MeterDataTable tableMeter;
    private BaseTables.MeterChangesDataTable tableMeterChanges;
    private BaseTables.DatabaseLocationDataTable tableDatabaseLocation;
    private BaseTables.DatabaseIdentificationDataTable tableDatabaseIdentification;
    private BaseTables.ZRGlobalIDDataTable tableZRGlobalID;
    private BaseTables.MeterDataDataTable tableMeterData;
    private BaseTables.HardwareTypeDataTable tableHardwareType;
    private BaseTables.OnlineTranslationBaseMassagesDataTable tableOnlineTranslationBaseMassages;
    private BaseTables.OnlineTranslationsDataTable tableOnlineTranslations;
    private BaseTables.MeterInfoDataTable tableMeterInfo;
    private BaseTables.LocationDataTable tableLocation;
    private BaseTables.MeterUniqueIdByARMDataTable tableMeterUniqueIdByARM;
    private BaseTables.ApprovalTypesDataTable tableApprovalTypes;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public BaseTables()
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
    protected BaseTables(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (Meter)] != null)
            base.Tables.Add((DataTable) new BaseTables.MeterDataTable(dataSet.Tables[nameof (Meter)]));
          if (dataSet.Tables[nameof (MeterChanges)] != null)
            base.Tables.Add((DataTable) new BaseTables.MeterChangesDataTable(dataSet.Tables[nameof (MeterChanges)]));
          if (dataSet.Tables[nameof (DatabaseLocation)] != null)
            base.Tables.Add((DataTable) new BaseTables.DatabaseLocationDataTable(dataSet.Tables[nameof (DatabaseLocation)]));
          if (dataSet.Tables[nameof (DatabaseIdentification)] != null)
            base.Tables.Add((DataTable) new BaseTables.DatabaseIdentificationDataTable(dataSet.Tables[nameof (DatabaseIdentification)]));
          if (dataSet.Tables[nameof (ZRGlobalID)] != null)
            base.Tables.Add((DataTable) new BaseTables.ZRGlobalIDDataTable(dataSet.Tables[nameof (ZRGlobalID)]));
          if (dataSet.Tables[nameof (MeterData)] != null)
            base.Tables.Add((DataTable) new BaseTables.MeterDataDataTable(dataSet.Tables[nameof (MeterData)]));
          if (dataSet.Tables[nameof (HardwareType)] != null)
            base.Tables.Add((DataTable) new BaseTables.HardwareTypeDataTable(dataSet.Tables[nameof (HardwareType)]));
          if (dataSet.Tables[nameof (OnlineTranslationBaseMassages)] != null)
            base.Tables.Add((DataTable) new BaseTables.OnlineTranslationBaseMassagesDataTable(dataSet.Tables[nameof (OnlineTranslationBaseMassages)]));
          if (dataSet.Tables[nameof (OnlineTranslations)] != null)
            base.Tables.Add((DataTable) new BaseTables.OnlineTranslationsDataTable(dataSet.Tables[nameof (OnlineTranslations)]));
          if (dataSet.Tables[nameof (MeterInfo)] != null)
            base.Tables.Add((DataTable) new BaseTables.MeterInfoDataTable(dataSet.Tables[nameof (MeterInfo)]));
          if (dataSet.Tables[nameof (Location)] != null)
            base.Tables.Add((DataTable) new BaseTables.LocationDataTable(dataSet.Tables[nameof (Location)]));
          if (dataSet.Tables[nameof (MeterUniqueIdByARM)] != null)
            base.Tables.Add((DataTable) new BaseTables.MeterUniqueIdByARMDataTable(dataSet.Tables[nameof (MeterUniqueIdByARM)]));
          if (dataSet.Tables[nameof (ApprovalTypes)] != null)
            base.Tables.Add((DataTable) new BaseTables.ApprovalTypesDataTable(dataSet.Tables[nameof (ApprovalTypes)]));
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
    public BaseTables.MeterDataTable Meter => this.tableMeter;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.MeterChangesDataTable MeterChanges => this.tableMeterChanges;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.DatabaseLocationDataTable DatabaseLocation => this.tableDatabaseLocation;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.DatabaseIdentificationDataTable DatabaseIdentification
    {
      get => this.tableDatabaseIdentification;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.ZRGlobalIDDataTable ZRGlobalID => this.tableZRGlobalID;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.MeterDataDataTable MeterData => this.tableMeterData;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.HardwareTypeDataTable HardwareType => this.tableHardwareType;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.OnlineTranslationBaseMassagesDataTable OnlineTranslationBaseMassages
    {
      get => this.tableOnlineTranslationBaseMassages;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.OnlineTranslationsDataTable OnlineTranslations
    {
      get => this.tableOnlineTranslations;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.MeterInfoDataTable MeterInfo => this.tableMeterInfo;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.LocationDataTable Location => this.tableLocation;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.MeterUniqueIdByARMDataTable MeterUniqueIdByARM
    {
      get => this.tableMeterUniqueIdByARM;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public BaseTables.ApprovalTypesDataTable ApprovalTypes => this.tableApprovalTypes;

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
      BaseTables baseTables = (BaseTables) base.Clone();
      baseTables.InitVars();
      baseTables.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) baseTables;
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
        if (dataSet.Tables["Meter"] != null)
          base.Tables.Add((DataTable) new BaseTables.MeterDataTable(dataSet.Tables["Meter"]));
        if (dataSet.Tables["MeterChanges"] != null)
          base.Tables.Add((DataTable) new BaseTables.MeterChangesDataTable(dataSet.Tables["MeterChanges"]));
        if (dataSet.Tables["DatabaseLocation"] != null)
          base.Tables.Add((DataTable) new BaseTables.DatabaseLocationDataTable(dataSet.Tables["DatabaseLocation"]));
        if (dataSet.Tables["DatabaseIdentification"] != null)
          base.Tables.Add((DataTable) new BaseTables.DatabaseIdentificationDataTable(dataSet.Tables["DatabaseIdentification"]));
        if (dataSet.Tables["ZRGlobalID"] != null)
          base.Tables.Add((DataTable) new BaseTables.ZRGlobalIDDataTable(dataSet.Tables["ZRGlobalID"]));
        if (dataSet.Tables["MeterData"] != null)
          base.Tables.Add((DataTable) new BaseTables.MeterDataDataTable(dataSet.Tables["MeterData"]));
        if (dataSet.Tables["HardwareType"] != null)
          base.Tables.Add((DataTable) new BaseTables.HardwareTypeDataTable(dataSet.Tables["HardwareType"]));
        if (dataSet.Tables["OnlineTranslationBaseMassages"] != null)
          base.Tables.Add((DataTable) new BaseTables.OnlineTranslationBaseMassagesDataTable(dataSet.Tables["OnlineTranslationBaseMassages"]));
        if (dataSet.Tables["OnlineTranslations"] != null)
          base.Tables.Add((DataTable) new BaseTables.OnlineTranslationsDataTable(dataSet.Tables["OnlineTranslations"]));
        if (dataSet.Tables["MeterInfo"] != null)
          base.Tables.Add((DataTable) new BaseTables.MeterInfoDataTable(dataSet.Tables["MeterInfo"]));
        if (dataSet.Tables["Location"] != null)
          base.Tables.Add((DataTable) new BaseTables.LocationDataTable(dataSet.Tables["Location"]));
        if (dataSet.Tables["MeterUniqueIdByARM"] != null)
          base.Tables.Add((DataTable) new BaseTables.MeterUniqueIdByARMDataTable(dataSet.Tables["MeterUniqueIdByARM"]));
        if (dataSet.Tables["ApprovalTypes"] != null)
          base.Tables.Add((DataTable) new BaseTables.ApprovalTypesDataTable(dataSet.Tables["ApprovalTypes"]));
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
      this.tableMeter = (BaseTables.MeterDataTable) base.Tables["Meter"];
      if (initTable && this.tableMeter != null)
        this.tableMeter.InitVars();
      this.tableMeterChanges = (BaseTables.MeterChangesDataTable) base.Tables["MeterChanges"];
      if (initTable && this.tableMeterChanges != null)
        this.tableMeterChanges.InitVars();
      this.tableDatabaseLocation = (BaseTables.DatabaseLocationDataTable) base.Tables["DatabaseLocation"];
      if (initTable && this.tableDatabaseLocation != null)
        this.tableDatabaseLocation.InitVars();
      this.tableDatabaseIdentification = (BaseTables.DatabaseIdentificationDataTable) base.Tables["DatabaseIdentification"];
      if (initTable && this.tableDatabaseIdentification != null)
        this.tableDatabaseIdentification.InitVars();
      this.tableZRGlobalID = (BaseTables.ZRGlobalIDDataTable) base.Tables["ZRGlobalID"];
      if (initTable && this.tableZRGlobalID != null)
        this.tableZRGlobalID.InitVars();
      this.tableMeterData = (BaseTables.MeterDataDataTable) base.Tables["MeterData"];
      if (initTable && this.tableMeterData != null)
        this.tableMeterData.InitVars();
      this.tableHardwareType = (BaseTables.HardwareTypeDataTable) base.Tables["HardwareType"];
      if (initTable && this.tableHardwareType != null)
        this.tableHardwareType.InitVars();
      this.tableOnlineTranslationBaseMassages = (BaseTables.OnlineTranslationBaseMassagesDataTable) base.Tables["OnlineTranslationBaseMassages"];
      if (initTable && this.tableOnlineTranslationBaseMassages != null)
        this.tableOnlineTranslationBaseMassages.InitVars();
      this.tableOnlineTranslations = (BaseTables.OnlineTranslationsDataTable) base.Tables["OnlineTranslations"];
      if (initTable && this.tableOnlineTranslations != null)
        this.tableOnlineTranslations.InitVars();
      this.tableMeterInfo = (BaseTables.MeterInfoDataTable) base.Tables["MeterInfo"];
      if (initTable && this.tableMeterInfo != null)
        this.tableMeterInfo.InitVars();
      this.tableLocation = (BaseTables.LocationDataTable) base.Tables["Location"];
      if (initTable && this.tableLocation != null)
        this.tableLocation.InitVars();
      this.tableMeterUniqueIdByARM = (BaseTables.MeterUniqueIdByARMDataTable) base.Tables["MeterUniqueIdByARM"];
      if (initTable && this.tableMeterUniqueIdByARM != null)
        this.tableMeterUniqueIdByARM.InitVars();
      this.tableApprovalTypes = (BaseTables.ApprovalTypesDataTable) base.Tables["ApprovalTypes"];
      if (!initTable || this.tableApprovalTypes == null)
        return;
      this.tableApprovalTypes.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (BaseTables);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/BaseTables.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableMeter = new BaseTables.MeterDataTable();
      base.Tables.Add((DataTable) this.tableMeter);
      this.tableMeterChanges = new BaseTables.MeterChangesDataTable();
      base.Tables.Add((DataTable) this.tableMeterChanges);
      this.tableDatabaseLocation = new BaseTables.DatabaseLocationDataTable();
      base.Tables.Add((DataTable) this.tableDatabaseLocation);
      this.tableDatabaseIdentification = new BaseTables.DatabaseIdentificationDataTable();
      base.Tables.Add((DataTable) this.tableDatabaseIdentification);
      this.tableZRGlobalID = new BaseTables.ZRGlobalIDDataTable();
      base.Tables.Add((DataTable) this.tableZRGlobalID);
      this.tableMeterData = new BaseTables.MeterDataDataTable();
      base.Tables.Add((DataTable) this.tableMeterData);
      this.tableHardwareType = new BaseTables.HardwareTypeDataTable();
      base.Tables.Add((DataTable) this.tableHardwareType);
      this.tableOnlineTranslationBaseMassages = new BaseTables.OnlineTranslationBaseMassagesDataTable();
      base.Tables.Add((DataTable) this.tableOnlineTranslationBaseMassages);
      this.tableOnlineTranslations = new BaseTables.OnlineTranslationsDataTable();
      base.Tables.Add((DataTable) this.tableOnlineTranslations);
      this.tableMeterInfo = new BaseTables.MeterInfoDataTable();
      base.Tables.Add((DataTable) this.tableMeterInfo);
      this.tableLocation = new BaseTables.LocationDataTable();
      base.Tables.Add((DataTable) this.tableLocation);
      this.tableMeterUniqueIdByARM = new BaseTables.MeterUniqueIdByARMDataTable();
      base.Tables.Add((DataTable) this.tableMeterUniqueIdByARM);
      this.tableApprovalTypes = new BaseTables.ApprovalTypesDataTable();
      base.Tables.Add((DataTable) this.tableApprovalTypes);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeMeter() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeMeterChanges() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeDatabaseLocation() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeDatabaseIdentification() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeZRGlobalID() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeMeterData() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeHardwareType() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeOnlineTranslationBaseMassages() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeOnlineTranslations() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeMeterInfo() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeLocation() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeMeterUniqueIdByARM() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeApprovalTypes() => false;

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
      BaseTables baseTables = new BaseTables();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = baseTables.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void MeterRowChangeEventHandler(object sender, BaseTables.MeterRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void MeterChangesRowChangeEventHandler(
      object sender,
      BaseTables.MeterChangesRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void DatabaseLocationRowChangeEventHandler(
      object sender,
      BaseTables.DatabaseLocationRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void DatabaseIdentificationRowChangeEventHandler(
      object sender,
      BaseTables.DatabaseIdentificationRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void ZRGlobalIDRowChangeEventHandler(
      object sender,
      BaseTables.ZRGlobalIDRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void MeterDataRowChangeEventHandler(
      object sender,
      BaseTables.MeterDataRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void HardwareTypeRowChangeEventHandler(
      object sender,
      BaseTables.HardwareTypeRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void OnlineTranslationBaseMassagesRowChangeEventHandler(
      object sender,
      BaseTables.OnlineTranslationBaseMassagesRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void OnlineTranslationsRowChangeEventHandler(
      object sender,
      BaseTables.OnlineTranslationsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void MeterInfoRowChangeEventHandler(
      object sender,
      BaseTables.MeterInfoRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void LocationRowChangeEventHandler(
      object sender,
      BaseTables.LocationRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void MeterUniqueIdByARMRowChangeEventHandler(
      object sender,
      BaseTables.MeterUniqueIdByARMRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void ApprovalTypesRowChangeEventHandler(
      object sender,
      BaseTables.ApprovalTypesRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class MeterDataTable : TypedTableBase<BaseTables.MeterRow>
    {
      private DataColumn columnMeterID;
      private DataColumn columnMeterInfoID;
      private DataColumn columnSerialNr;
      private DataColumn columnProductionDate;
      private DataColumn columnApprovalDate;
      private DataColumn columnOrderNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterDataTable()
      {
        this.TableName = "Meter";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterDataTable(DataTable table)
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
      protected MeterDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterInfoIDColumn => this.columnMeterInfoID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SerialNrColumn => this.columnSerialNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ProductionDateColumn => this.columnProductionDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ApprovalDateColumn => this.columnApprovalDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn OrderNrColumn => this.columnOrderNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterRow this[int index] => (BaseTables.MeterRow) this.Rows[index];

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterRowChangeEventHandler MeterRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterRowChangeEventHandler MeterRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterRowChangeEventHandler MeterRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterRowChangeEventHandler MeterRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddMeterRow(BaseTables.MeterRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterRow AddMeterRow(
        int MeterID,
        int MeterInfoID,
        string SerialNr,
        DateTime ProductionDate,
        DateTime ApprovalDate,
        string OrderNr)
      {
        BaseTables.MeterRow row = (BaseTables.MeterRow) this.NewRow();
        object[] objArray = new object[6]
        {
          (object) MeterID,
          (object) MeterInfoID,
          (object) SerialNr,
          (object) ProductionDate,
          (object) ApprovalDate,
          (object) OrderNr
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterRow FindByMeterID(int MeterID)
      {
        return (BaseTables.MeterRow) this.Rows.Find(new object[1]
        {
          (object) MeterID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.MeterDataTable meterDataTable = (BaseTables.MeterDataTable) base.Clone();
        meterDataTable.InitVars();
        return (DataTable) meterDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance() => (DataTable) new BaseTables.MeterDataTable();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnMeterID = this.Columns["MeterID"];
        this.columnMeterInfoID = this.Columns["MeterInfoID"];
        this.columnSerialNr = this.Columns["SerialNr"];
        this.columnProductionDate = this.Columns["ProductionDate"];
        this.columnApprovalDate = this.Columns["ApprovalDate"];
        this.columnOrderNr = this.Columns["OrderNr"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnMeterID = new DataColumn("MeterID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnMeterInfoID = new DataColumn("MeterInfoID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterInfoID);
        this.columnSerialNr = new DataColumn("SerialNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerialNr);
        this.columnProductionDate = new DataColumn("ProductionDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnProductionDate);
        this.columnApprovalDate = new DataColumn("ApprovalDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnApprovalDate);
        this.columnOrderNr = new DataColumn("OrderNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOrderNr);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnMeterID
        }, true));
        this.columnMeterID.AllowDBNull = false;
        this.columnMeterID.Unique = true;
        this.columnSerialNr.MaxLength = 50;
        this.columnProductionDate.DateTimeMode = DataSetDateTime.Utc;
        this.columnApprovalDate.DateTimeMode = DataSetDateTime.Utc;
        this.columnOrderNr.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterRow NewMeterRow() => (BaseTables.MeterRow) this.NewRow();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.MeterRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.MeterRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterRowChanged == null)
          return;
        this.MeterRowChanged((object) this, new BaseTables.MeterRowChangeEvent((BaseTables.MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterRowChanging == null)
          return;
        this.MeterRowChanging((object) this, new BaseTables.MeterRowChangeEvent((BaseTables.MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterRowDeleted == null)
          return;
        this.MeterRowDeleted((object) this, new BaseTables.MeterRowChangeEvent((BaseTables.MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterRowDeleting == null)
          return;
        this.MeterRowDeleting((object) this, new BaseTables.MeterRowChangeEvent((BaseTables.MeterRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveMeterRow(BaseTables.MeterRow row) => this.Rows.Remove((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class MeterChangesDataTable : TypedTableBase<BaseTables.MeterChangesRow>
    {
      private DataColumn columnChangeID;
      private DataColumn columnMeterID;
      private DataColumn columnChangeDate;
      private DataColumn columnMeterInfoID;
      private DataColumn columnSerialNr;
      private DataColumn columnProductionDate;
      private DataColumn columnApprovalDate;
      private DataColumn columnOrderNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterChangesDataTable()
      {
        this.TableName = "MeterChanges";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterChangesDataTable(DataTable table)
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
      protected MeterChangesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ChangeIDColumn => this.columnChangeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ChangeDateColumn => this.columnChangeDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterInfoIDColumn => this.columnMeterInfoID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SerialNrColumn => this.columnSerialNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ProductionDateColumn => this.columnProductionDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ApprovalDateColumn => this.columnApprovalDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn OrderNrColumn => this.columnOrderNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterChangesRow this[int index]
      {
        get => (BaseTables.MeterChangesRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterChangesRowChangeEventHandler MeterChangesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterChangesRowChangeEventHandler MeterChangesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterChangesRowChangeEventHandler MeterChangesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterChangesRowChangeEventHandler MeterChangesRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddMeterChangesRow(BaseTables.MeterChangesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterChangesRow AddMeterChangesRow(
        int ChangeID,
        int MeterID,
        DateTime ChangeDate,
        int MeterInfoID,
        string SerialNr,
        DateTime ProductionDate,
        DateTime ApprovalDate,
        string OrderNr)
      {
        BaseTables.MeterChangesRow row = (BaseTables.MeterChangesRow) this.NewRow();
        object[] objArray = new object[8]
        {
          (object) ChangeID,
          (object) MeterID,
          (object) ChangeDate,
          (object) MeterInfoID,
          (object) SerialNr,
          (object) ProductionDate,
          (object) ApprovalDate,
          (object) OrderNr
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterChangesRow FindByChangeID(int ChangeID)
      {
        return (BaseTables.MeterChangesRow) this.Rows.Find(new object[1]
        {
          (object) ChangeID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.MeterChangesDataTable changesDataTable = (BaseTables.MeterChangesDataTable) base.Clone();
        changesDataTable.InitVars();
        return (DataTable) changesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.MeterChangesDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnChangeID = this.Columns["ChangeID"];
        this.columnMeterID = this.Columns["MeterID"];
        this.columnChangeDate = this.Columns["ChangeDate"];
        this.columnMeterInfoID = this.Columns["MeterInfoID"];
        this.columnSerialNr = this.Columns["SerialNr"];
        this.columnProductionDate = this.Columns["ProductionDate"];
        this.columnApprovalDate = this.Columns["ApprovalDate"];
        this.columnOrderNr = this.Columns["OrderNr"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnChangeID = new DataColumn("ChangeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeID);
        this.columnMeterID = new DataColumn("MeterID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnChangeDate = new DataColumn("ChangeDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeDate);
        this.columnMeterInfoID = new DataColumn("MeterInfoID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterInfoID);
        this.columnSerialNr = new DataColumn("SerialNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSerialNr);
        this.columnProductionDate = new DataColumn("ProductionDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnProductionDate);
        this.columnApprovalDate = new DataColumn("ApprovalDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnApprovalDate);
        this.columnOrderNr = new DataColumn("OrderNr", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOrderNr);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnChangeID
        }, true));
        this.columnChangeID.AllowDBNull = false;
        this.columnChangeID.Unique = true;
        this.columnChangeDate.DateTimeMode = DataSetDateTime.Utc;
        this.columnSerialNr.MaxLength = 50;
        this.columnProductionDate.DateTimeMode = DataSetDateTime.Utc;
        this.columnApprovalDate.DateTimeMode = DataSetDateTime.Utc;
        this.columnOrderNr.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterChangesRow NewMeterChangesRow()
      {
        return (BaseTables.MeterChangesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.MeterChangesRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.MeterChangesRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterChangesRowChanged == null)
          return;
        this.MeterChangesRowChanged((object) this, new BaseTables.MeterChangesRowChangeEvent((BaseTables.MeterChangesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterChangesRowChanging == null)
          return;
        this.MeterChangesRowChanging((object) this, new BaseTables.MeterChangesRowChangeEvent((BaseTables.MeterChangesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterChangesRowDeleted == null)
          return;
        this.MeterChangesRowDeleted((object) this, new BaseTables.MeterChangesRowChangeEvent((BaseTables.MeterChangesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterChangesRowDeleting == null)
          return;
        this.MeterChangesRowDeleting((object) this, new BaseTables.MeterChangesRowChangeEvent((BaseTables.MeterChangesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveMeterChangesRow(BaseTables.MeterChangesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterChangesDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class DatabaseLocationDataTable : TypedTableBase<BaseTables.DatabaseLocationRow>
    {
      private DataColumn columnDatabaseLocationName;
      private DataColumn columnLand;
      private DataColumn columnTown;
      private DataColumn columnCompanyName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DatabaseLocationDataTable()
      {
        this.TableName = "DatabaseLocation";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal DatabaseLocationDataTable(DataTable table)
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
      protected DatabaseLocationDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DatabaseLocationNameColumn => this.columnDatabaseLocationName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LandColumn => this.columnLand;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TownColumn => this.columnTown;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CompanyNameColumn => this.columnCompanyName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseLocationRow this[int index]
      {
        get => (BaseTables.DatabaseLocationRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseLocationRowChangeEventHandler DatabaseLocationRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseLocationRowChangeEventHandler DatabaseLocationRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseLocationRowChangeEventHandler DatabaseLocationRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseLocationRowChangeEventHandler DatabaseLocationRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddDatabaseLocationRow(BaseTables.DatabaseLocationRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseLocationRow AddDatabaseLocationRow(
        string DatabaseLocationName,
        string Land,
        string Town,
        string CompanyName)
      {
        BaseTables.DatabaseLocationRow row = (BaseTables.DatabaseLocationRow) this.NewRow();
        object[] objArray = new object[4]
        {
          (object) DatabaseLocationName,
          (object) Land,
          (object) Town,
          (object) CompanyName
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseLocationRow FindByDatabaseLocationName(string DatabaseLocationName)
      {
        return (BaseTables.DatabaseLocationRow) this.Rows.Find(new object[1]
        {
          (object) DatabaseLocationName
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.DatabaseLocationDataTable locationDataTable = (BaseTables.DatabaseLocationDataTable) base.Clone();
        locationDataTable.InitVars();
        return (DataTable) locationDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.DatabaseLocationDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnDatabaseLocationName = this.Columns["DatabaseLocationName"];
        this.columnLand = this.Columns["Land"];
        this.columnTown = this.Columns["Town"];
        this.columnCompanyName = this.Columns["CompanyName"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnDatabaseLocationName = new DataColumn("DatabaseLocationName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDatabaseLocationName);
        this.columnLand = new DataColumn("Land", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLand);
        this.columnTown = new DataColumn("Town", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTown);
        this.columnCompanyName = new DataColumn("CompanyName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCompanyName);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnDatabaseLocationName
        }, true));
        this.columnDatabaseLocationName.AllowDBNull = false;
        this.columnDatabaseLocationName.Unique = true;
        this.columnDatabaseLocationName.MaxLength = 50;
        this.columnLand.MaxLength = 50;
        this.columnTown.MaxLength = 50;
        this.columnCompanyName.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseLocationRow NewDatabaseLocationRow()
      {
        return (BaseTables.DatabaseLocationRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.DatabaseLocationRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.DatabaseLocationRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.DatabaseLocationRowChanged == null)
          return;
        this.DatabaseLocationRowChanged((object) this, new BaseTables.DatabaseLocationRowChangeEvent((BaseTables.DatabaseLocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.DatabaseLocationRowChanging == null)
          return;
        this.DatabaseLocationRowChanging((object) this, new BaseTables.DatabaseLocationRowChangeEvent((BaseTables.DatabaseLocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.DatabaseLocationRowDeleted == null)
          return;
        this.DatabaseLocationRowDeleted((object) this, new BaseTables.DatabaseLocationRowChangeEvent((BaseTables.DatabaseLocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.DatabaseLocationRowDeleting == null)
          return;
        this.DatabaseLocationRowDeleting((object) this, new BaseTables.DatabaseLocationRowChangeEvent((BaseTables.DatabaseLocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveDatabaseLocationRow(BaseTables.DatabaseLocationRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (DatabaseLocationDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class DatabaseIdentificationDataTable : 
      TypedTableBase<BaseTables.DatabaseIdentificationRow>
    {
      private DataColumn columnInfoName;
      private DataColumn columnInfoData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DatabaseIdentificationDataTable()
      {
        this.TableName = "DatabaseIdentification";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal DatabaseIdentificationDataTable(DataTable table)
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
      protected DatabaseIdentificationDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn InfoNameColumn => this.columnInfoName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn InfoDataColumn => this.columnInfoData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseIdentificationRow this[int index]
      {
        get => (BaseTables.DatabaseIdentificationRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseIdentificationRowChangeEventHandler DatabaseIdentificationRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseIdentificationRowChangeEventHandler DatabaseIdentificationRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseIdentificationRowChangeEventHandler DatabaseIdentificationRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.DatabaseIdentificationRowChangeEventHandler DatabaseIdentificationRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddDatabaseIdentificationRow(BaseTables.DatabaseIdentificationRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseIdentificationRow AddDatabaseIdentificationRow(
        string InfoName,
        string InfoData)
      {
        BaseTables.DatabaseIdentificationRow row = (BaseTables.DatabaseIdentificationRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) InfoName,
          (object) InfoData
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseIdentificationRow FindByInfoName(string InfoName)
      {
        return (BaseTables.DatabaseIdentificationRow) this.Rows.Find(new object[1]
        {
          (object) InfoName
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.DatabaseIdentificationDataTable identificationDataTable = (BaseTables.DatabaseIdentificationDataTable) base.Clone();
        identificationDataTable.InitVars();
        return (DataTable) identificationDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.DatabaseIdentificationDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnInfoName = this.Columns["InfoName"];
        this.columnInfoData = this.Columns["InfoData"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnInfoName = new DataColumn("InfoName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInfoName);
        this.columnInfoData = new DataColumn("InfoData", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInfoData);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnInfoName
        }, true));
        this.columnInfoName.AllowDBNull = false;
        this.columnInfoName.Unique = true;
        this.columnInfoName.MaxLength = 50;
        this.columnInfoData.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseIdentificationRow NewDatabaseIdentificationRow()
      {
        return (BaseTables.DatabaseIdentificationRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.DatabaseIdentificationRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.DatabaseIdentificationRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.DatabaseIdentificationRowChanged == null)
          return;
        this.DatabaseIdentificationRowChanged((object) this, new BaseTables.DatabaseIdentificationRowChangeEvent((BaseTables.DatabaseIdentificationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.DatabaseIdentificationRowChanging == null)
          return;
        this.DatabaseIdentificationRowChanging((object) this, new BaseTables.DatabaseIdentificationRowChangeEvent((BaseTables.DatabaseIdentificationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.DatabaseIdentificationRowDeleted == null)
          return;
        this.DatabaseIdentificationRowDeleted((object) this, new BaseTables.DatabaseIdentificationRowChangeEvent((BaseTables.DatabaseIdentificationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.DatabaseIdentificationRowDeleting == null)
          return;
        this.DatabaseIdentificationRowDeleting((object) this, new BaseTables.DatabaseIdentificationRowChangeEvent((BaseTables.DatabaseIdentificationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveDatabaseIdentificationRow(BaseTables.DatabaseIdentificationRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (DatabaseIdentificationDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class ZRGlobalIDDataTable : TypedTableBase<BaseTables.ZRGlobalIDRow>
    {
      private DataColumn columnZRTableName;
      private DataColumn columnZRFieldName;
      private DataColumn columnZRNextNr;
      private DataColumn columnZRFirstNr;
      private DataColumn columnZRLastNr;
      private DataColumn columnDatabaseLocationName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ZRGlobalIDDataTable()
      {
        this.TableName = "ZRGlobalID";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ZRGlobalIDDataTable(DataTable table)
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
      protected ZRGlobalIDDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ZRTableNameColumn => this.columnZRTableName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ZRFieldNameColumn => this.columnZRFieldName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ZRNextNrColumn => this.columnZRNextNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ZRFirstNrColumn => this.columnZRFirstNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ZRLastNrColumn => this.columnZRLastNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DatabaseLocationNameColumn => this.columnDatabaseLocationName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ZRGlobalIDRow this[int index]
      {
        get => (BaseTables.ZRGlobalIDRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ZRGlobalIDRowChangeEventHandler ZRGlobalIDRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ZRGlobalIDRowChangeEventHandler ZRGlobalIDRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ZRGlobalIDRowChangeEventHandler ZRGlobalIDRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ZRGlobalIDRowChangeEventHandler ZRGlobalIDRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddZRGlobalIDRow(BaseTables.ZRGlobalIDRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ZRGlobalIDRow AddZRGlobalIDRow(
        string ZRTableName,
        string ZRFieldName,
        int ZRNextNr,
        int ZRFirstNr,
        int ZRLastNr,
        string DatabaseLocationName)
      {
        BaseTables.ZRGlobalIDRow row = (BaseTables.ZRGlobalIDRow) this.NewRow();
        object[] objArray = new object[6]
        {
          (object) ZRTableName,
          (object) ZRFieldName,
          (object) ZRNextNr,
          (object) ZRFirstNr,
          (object) ZRLastNr,
          (object) DatabaseLocationName
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ZRGlobalIDRow FindByZRTableNameDatabaseLocationName(
        string ZRTableName,
        string DatabaseLocationName)
      {
        return (BaseTables.ZRGlobalIDRow) this.Rows.Find(new object[2]
        {
          (object) ZRTableName,
          (object) DatabaseLocationName
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.ZRGlobalIDDataTable globalIdDataTable = (BaseTables.ZRGlobalIDDataTable) base.Clone();
        globalIdDataTable.InitVars();
        return (DataTable) globalIdDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.ZRGlobalIDDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnZRTableName = this.Columns["ZRTableName"];
        this.columnZRFieldName = this.Columns["ZRFieldName"];
        this.columnZRNextNr = this.Columns["ZRNextNr"];
        this.columnZRFirstNr = this.Columns["ZRFirstNr"];
        this.columnZRLastNr = this.Columns["ZRLastNr"];
        this.columnDatabaseLocationName = this.Columns["DatabaseLocationName"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnZRTableName = new DataColumn("ZRTableName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZRTableName);
        this.columnZRFieldName = new DataColumn("ZRFieldName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZRFieldName);
        this.columnZRNextNr = new DataColumn("ZRNextNr", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZRNextNr);
        this.columnZRFirstNr = new DataColumn("ZRFirstNr", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZRFirstNr);
        this.columnZRLastNr = new DataColumn("ZRLastNr", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZRLastNr);
        this.columnDatabaseLocationName = new DataColumn("DatabaseLocationName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDatabaseLocationName);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnZRTableName,
          this.columnDatabaseLocationName
        }, true));
        this.columnZRTableName.AllowDBNull = false;
        this.columnZRTableName.MaxLength = 50;
        this.columnZRFieldName.MaxLength = 50;
        this.columnDatabaseLocationName.AllowDBNull = false;
        this.columnDatabaseLocationName.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ZRGlobalIDRow NewZRGlobalIDRow()
      {
        return (BaseTables.ZRGlobalIDRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.ZRGlobalIDRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.ZRGlobalIDRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ZRGlobalIDRowChanged == null)
          return;
        this.ZRGlobalIDRowChanged((object) this, new BaseTables.ZRGlobalIDRowChangeEvent((BaseTables.ZRGlobalIDRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ZRGlobalIDRowChanging == null)
          return;
        this.ZRGlobalIDRowChanging((object) this, new BaseTables.ZRGlobalIDRowChangeEvent((BaseTables.ZRGlobalIDRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ZRGlobalIDRowDeleted == null)
          return;
        this.ZRGlobalIDRowDeleted((object) this, new BaseTables.ZRGlobalIDRowChangeEvent((BaseTables.ZRGlobalIDRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ZRGlobalIDRowDeleting == null)
          return;
        this.ZRGlobalIDRowDeleting((object) this, new BaseTables.ZRGlobalIDRowChangeEvent((BaseTables.ZRGlobalIDRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveZRGlobalIDRow(BaseTables.ZRGlobalIDRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ZRGlobalIDDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class MeterDataDataTable : TypedTableBase<BaseTables.MeterDataRow>
    {
      private DataColumn columnMeterID;
      private DataColumn columnTimePoint;
      private DataColumn columnPValueID;
      private DataColumn columnPValue;
      private DataColumn columnPValueBinary;
      private DataColumn columnSyncStatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterDataDataTable()
      {
        this.TableName = "MeterData";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterDataDataTable(DataTable table)
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
      protected MeterDataDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TimePointColumn => this.columnTimePoint;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn PValueIDColumn => this.columnPValueID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn PValueColumn => this.columnPValue;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn PValueBinaryColumn => this.columnPValueBinary;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn SyncStatusColumn => this.columnSyncStatus;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterDataRow this[int index] => (BaseTables.MeterDataRow) this.Rows[index];

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterDataRowChangeEventHandler MeterDataRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterDataRowChangeEventHandler MeterDataRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterDataRowChangeEventHandler MeterDataRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterDataRowChangeEventHandler MeterDataRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddMeterDataRow(BaseTables.MeterDataRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterDataRow AddMeterDataRow(
        int MeterID,
        DateTime TimePoint,
        int PValueID,
        string PValue,
        byte[] PValueBinary,
        byte SyncStatus)
      {
        BaseTables.MeterDataRow row = (BaseTables.MeterDataRow) this.NewRow();
        object[] objArray = new object[6]
        {
          (object) MeterID,
          (object) TimePoint,
          (object) PValueID,
          (object) PValue,
          (object) PValueBinary,
          (object) SyncStatus
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterDataRow FindByMeterIDTimePointPValueID(
        int MeterID,
        DateTime TimePoint,
        int PValueID)
      {
        return (BaseTables.MeterDataRow) this.Rows.Find(new object[3]
        {
          (object) MeterID,
          (object) TimePoint,
          (object) PValueID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.MeterDataDataTable meterDataDataTable = (BaseTables.MeterDataDataTable) base.Clone();
        meterDataDataTable.InitVars();
        return (DataTable) meterDataDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.MeterDataDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnMeterID = this.Columns["MeterID"];
        this.columnTimePoint = this.Columns["TimePoint"];
        this.columnPValueID = this.Columns["PValueID"];
        this.columnPValue = this.Columns["PValue"];
        this.columnPValueBinary = this.Columns["PValueBinary"];
        this.columnSyncStatus = this.Columns["SyncStatus"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnMeterID = new DataColumn("MeterID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnTimePoint = new DataColumn("TimePoint", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTimePoint);
        this.columnPValueID = new DataColumn("PValueID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPValueID);
        this.columnPValue = new DataColumn("PValue", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPValue);
        this.columnPValueBinary = new DataColumn("PValueBinary", typeof (byte[]), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPValueBinary);
        this.columnSyncStatus = new DataColumn("SyncStatus", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSyncStatus);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[3]
        {
          this.columnMeterID,
          this.columnTimePoint,
          this.columnPValueID
        }, true));
        this.columnMeterID.AllowDBNull = false;
        this.columnTimePoint.AllowDBNull = false;
        this.columnTimePoint.DateTimeMode = DataSetDateTime.Utc;
        this.columnPValueID.AllowDBNull = false;
        this.columnPValue.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterDataRow NewMeterDataRow() => (BaseTables.MeterDataRow) this.NewRow();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.MeterDataRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.MeterDataRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterDataRowChanged == null)
          return;
        this.MeterDataRowChanged((object) this, new BaseTables.MeterDataRowChangeEvent((BaseTables.MeterDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterDataRowChanging == null)
          return;
        this.MeterDataRowChanging((object) this, new BaseTables.MeterDataRowChangeEvent((BaseTables.MeterDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterDataRowDeleted == null)
          return;
        this.MeterDataRowDeleted((object) this, new BaseTables.MeterDataRowChangeEvent((BaseTables.MeterDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterDataRowDeleting == null)
          return;
        this.MeterDataRowDeleting((object) this, new BaseTables.MeterDataRowChangeEvent((BaseTables.MeterDataRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveMeterDataRow(BaseTables.MeterDataRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterDataDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class HardwareTypeDataTable : TypedTableBase<BaseTables.HardwareTypeRow>
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
      public BaseTables.HardwareTypeRow this[int index]
      {
        get => (BaseTables.HardwareTypeRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.HardwareTypeRowChangeEventHandler HardwareTypeRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.HardwareTypeRowChangeEventHandler HardwareTypeRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.HardwareTypeRowChangeEventHandler HardwareTypeRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.HardwareTypeRowChangeEventHandler HardwareTypeRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddHardwareTypeRow(BaseTables.HardwareTypeRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.HardwareTypeRow AddHardwareTypeRow(
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
        BaseTables.HardwareTypeRow row = (BaseTables.HardwareTypeRow) this.NewRow();
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
      public BaseTables.HardwareTypeRow FindByHardwareTypeID(int HardwareTypeID)
      {
        return (BaseTables.HardwareTypeRow) this.Rows.Find(new object[1]
        {
          (object) HardwareTypeID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.HardwareTypeDataTable hardwareTypeDataTable = (BaseTables.HardwareTypeDataTable) base.Clone();
        hardwareTypeDataTable.InitVars();
        return (DataTable) hardwareTypeDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.HardwareTypeDataTable();
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
      public BaseTables.HardwareTypeRow NewHardwareTypeRow()
      {
        return (BaseTables.HardwareTypeRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.HardwareTypeRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.HardwareTypeRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.HardwareTypeRowChanged == null)
          return;
        this.HardwareTypeRowChanged((object) this, new BaseTables.HardwareTypeRowChangeEvent((BaseTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.HardwareTypeRowChanging == null)
          return;
        this.HardwareTypeRowChanging((object) this, new BaseTables.HardwareTypeRowChangeEvent((BaseTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.HardwareTypeRowDeleted == null)
          return;
        this.HardwareTypeRowDeleted((object) this, new BaseTables.HardwareTypeRowChangeEvent((BaseTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.HardwareTypeRowDeleting == null)
          return;
        this.HardwareTypeRowDeleting((object) this, new BaseTables.HardwareTypeRowChangeEvent((BaseTables.HardwareTypeRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveHardwareTypeRow(BaseTables.HardwareTypeRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (HardwareTypeDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class OnlineTranslationBaseMassagesDataTable : 
      TypedTableBase<BaseTables.OnlineTranslationBaseMassagesRow>
    {
      private DataColumn columnMessageTree;
      private DataColumn columnDefaultText;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public OnlineTranslationBaseMassagesDataTable()
      {
        this.TableName = "OnlineTranslationBaseMassages";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal OnlineTranslationBaseMassagesDataTable(DataTable table)
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
      protected OnlineTranslationBaseMassagesDataTable(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MessageTreeColumn => this.columnMessageTree;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DefaultTextColumn => this.columnDefaultText;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationBaseMassagesRow this[int index]
      {
        get => (BaseTables.OnlineTranslationBaseMassagesRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationBaseMassagesRowChangeEventHandler OnlineTranslationBaseMassagesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationBaseMassagesRowChangeEventHandler OnlineTranslationBaseMassagesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationBaseMassagesRowChangeEventHandler OnlineTranslationBaseMassagesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationBaseMassagesRowChangeEventHandler OnlineTranslationBaseMassagesRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddOnlineTranslationBaseMassagesRow(
        BaseTables.OnlineTranslationBaseMassagesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationBaseMassagesRow AddOnlineTranslationBaseMassagesRow(
        string MessageTree,
        string DefaultText)
      {
        BaseTables.OnlineTranslationBaseMassagesRow row = (BaseTables.OnlineTranslationBaseMassagesRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) MessageTree,
          (object) DefaultText
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationBaseMassagesRow FindByMessageTree(string MessageTree)
      {
        return (BaseTables.OnlineTranslationBaseMassagesRow) this.Rows.Find(new object[1]
        {
          (object) MessageTree
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.OnlineTranslationBaseMassagesDataTable massagesDataTable = (BaseTables.OnlineTranslationBaseMassagesDataTable) base.Clone();
        massagesDataTable.InitVars();
        return (DataTable) massagesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.OnlineTranslationBaseMassagesDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnMessageTree = this.Columns["MessageTree"];
        this.columnDefaultText = this.Columns["DefaultText"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnMessageTree = new DataColumn("MessageTree", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMessageTree);
        this.columnDefaultText = new DataColumn("DefaultText", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDefaultText);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnMessageTree
        }, true));
        this.columnMessageTree.AllowDBNull = false;
        this.columnMessageTree.Unique = true;
        this.columnMessageTree.MaxLength = 50;
        this.columnDefaultText.MaxLength = 100;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationBaseMassagesRow NewOnlineTranslationBaseMassagesRow()
      {
        return (BaseTables.OnlineTranslationBaseMassagesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.OnlineTranslationBaseMassagesRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.OnlineTranslationBaseMassagesRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.OnlineTranslationBaseMassagesRowChanged == null)
          return;
        this.OnlineTranslationBaseMassagesRowChanged((object) this, new BaseTables.OnlineTranslationBaseMassagesRowChangeEvent((BaseTables.OnlineTranslationBaseMassagesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.OnlineTranslationBaseMassagesRowChanging == null)
          return;
        this.OnlineTranslationBaseMassagesRowChanging((object) this, new BaseTables.OnlineTranslationBaseMassagesRowChangeEvent((BaseTables.OnlineTranslationBaseMassagesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.OnlineTranslationBaseMassagesRowDeleted == null)
          return;
        this.OnlineTranslationBaseMassagesRowDeleted((object) this, new BaseTables.OnlineTranslationBaseMassagesRowChangeEvent((BaseTables.OnlineTranslationBaseMassagesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.OnlineTranslationBaseMassagesRowDeleting == null)
          return;
        this.OnlineTranslationBaseMassagesRowDeleting((object) this, new BaseTables.OnlineTranslationBaseMassagesRowChangeEvent((BaseTables.OnlineTranslationBaseMassagesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveOnlineTranslationBaseMassagesRow(
        BaseTables.OnlineTranslationBaseMassagesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (OnlineTranslationBaseMassagesDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class OnlineTranslationsDataTable : TypedTableBase<BaseTables.OnlineTranslationsRow>
    {
      private DataColumn columnTranslationGroup;
      private DataColumn columnTextKey;
      private DataColumn columnLanguageCode;
      private DataColumn columnLanguageText;
      private DataColumn columnMessageNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public OnlineTranslationsDataTable()
      {
        this.TableName = "OnlineTranslations";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal OnlineTranslationsDataTable(DataTable table)
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
      protected OnlineTranslationsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TranslationGroupColumn => this.columnTranslationGroup;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn TextKeyColumn => this.columnTextKey;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LanguageCodeColumn => this.columnLanguageCode;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LanguageTextColumn => this.columnLanguageText;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MessageNumberColumn => this.columnMessageNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationsRow this[int index]
      {
        get => (BaseTables.OnlineTranslationsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationsRowChangeEventHandler OnlineTranslationsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationsRowChangeEventHandler OnlineTranslationsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationsRowChangeEventHandler OnlineTranslationsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.OnlineTranslationsRowChangeEventHandler OnlineTranslationsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddOnlineTranslationsRow(BaseTables.OnlineTranslationsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationsRow AddOnlineTranslationsRow(
        int TranslationGroup,
        string TextKey,
        string LanguageCode,
        string LanguageText,
        int MessageNumber)
      {
        BaseTables.OnlineTranslationsRow row = (BaseTables.OnlineTranslationsRow) this.NewRow();
        object[] objArray = new object[5]
        {
          (object) TranslationGroup,
          (object) TextKey,
          (object) LanguageCode,
          (object) LanguageText,
          (object) MessageNumber
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationsRow FindByTranslationGroupTextKeyLanguageCode(
        int TranslationGroup,
        string TextKey,
        string LanguageCode)
      {
        return (BaseTables.OnlineTranslationsRow) this.Rows.Find(new object[3]
        {
          (object) TranslationGroup,
          (object) TextKey,
          (object) LanguageCode
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.OnlineTranslationsDataTable translationsDataTable = (BaseTables.OnlineTranslationsDataTable) base.Clone();
        translationsDataTable.InitVars();
        return (DataTable) translationsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.OnlineTranslationsDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnTranslationGroup = this.Columns["TranslationGroup"];
        this.columnTextKey = this.Columns["TextKey"];
        this.columnLanguageCode = this.Columns["LanguageCode"];
        this.columnLanguageText = this.Columns["LanguageText"];
        this.columnMessageNumber = this.Columns["MessageNumber"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnTranslationGroup = new DataColumn("TranslationGroup", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTranslationGroup);
        this.columnTextKey = new DataColumn("TextKey", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnTextKey);
        this.columnLanguageCode = new DataColumn("LanguageCode", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLanguageCode);
        this.columnLanguageText = new DataColumn("LanguageText", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLanguageText);
        this.columnMessageNumber = new DataColumn("MessageNumber", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMessageNumber);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[3]
        {
          this.columnTranslationGroup,
          this.columnTextKey,
          this.columnLanguageCode
        }, true));
        this.columnTranslationGroup.AllowDBNull = false;
        this.columnTextKey.AllowDBNull = false;
        this.columnTextKey.MaxLength = 100;
        this.columnLanguageCode.AllowDBNull = false;
        this.columnLanguageCode.MaxLength = 10;
        this.columnLanguageText.MaxLength = 536870910;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationsRow NewOnlineTranslationsRow()
      {
        return (BaseTables.OnlineTranslationsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.OnlineTranslationsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.OnlineTranslationsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.OnlineTranslationsRowChanged == null)
          return;
        this.OnlineTranslationsRowChanged((object) this, new BaseTables.OnlineTranslationsRowChangeEvent((BaseTables.OnlineTranslationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.OnlineTranslationsRowChanging == null)
          return;
        this.OnlineTranslationsRowChanging((object) this, new BaseTables.OnlineTranslationsRowChangeEvent((BaseTables.OnlineTranslationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.OnlineTranslationsRowDeleted == null)
          return;
        this.OnlineTranslationsRowDeleted((object) this, new BaseTables.OnlineTranslationsRowChangeEvent((BaseTables.OnlineTranslationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.OnlineTranslationsRowDeleting == null)
          return;
        this.OnlineTranslationsRowDeleting((object) this, new BaseTables.OnlineTranslationsRowChangeEvent((BaseTables.OnlineTranslationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveOnlineTranslationsRow(BaseTables.OnlineTranslationsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (OnlineTranslationsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class MeterInfoDataTable : TypedTableBase<BaseTables.MeterInfoRow>
    {
      private DataColumn columnMeterInfoID;
      private DataColumn columnMeterHardwareID;
      private DataColumn columnMeterTypeID;
      private DataColumn columnPPSArtikelNr;
      private DataColumn columnDefaultFunctionNr;
      private DataColumn columnDescription;
      private DataColumn columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterInfoDataTable()
      {
        this.TableName = "MeterInfo";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterInfoDataTable(DataTable table)
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
      protected MeterInfoDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterInfoIDColumn => this.columnMeterInfoID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterHardwareIDColumn => this.columnMeterHardwareID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterTypeIDColumn => this.columnMeterTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn PPSArtikelNrColumn => this.columnPPSArtikelNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DefaultFunctionNrColumn => this.columnDefaultFunctionNr;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HardwareTypeIDColumn => this.columnHardwareTypeID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterInfoRow this[int index] => (BaseTables.MeterInfoRow) this.Rows[index];

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterInfoRowChangeEventHandler MeterInfoRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterInfoRowChangeEventHandler MeterInfoRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterInfoRowChangeEventHandler MeterInfoRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterInfoRowChangeEventHandler MeterInfoRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddMeterInfoRow(BaseTables.MeterInfoRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterInfoRow AddMeterInfoRow(
        int MeterInfoID,
        int MeterHardwareID,
        int MeterTypeID,
        string PPSArtikelNr,
        string DefaultFunctionNr,
        string Description,
        int HardwareTypeID)
      {
        BaseTables.MeterInfoRow row = (BaseTables.MeterInfoRow) this.NewRow();
        object[] objArray = new object[7]
        {
          (object) MeterInfoID,
          (object) MeterHardwareID,
          (object) MeterTypeID,
          (object) PPSArtikelNr,
          (object) DefaultFunctionNr,
          (object) Description,
          (object) HardwareTypeID
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.MeterInfoDataTable meterInfoDataTable = (BaseTables.MeterInfoDataTable) base.Clone();
        meterInfoDataTable.InitVars();
        return (DataTable) meterInfoDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.MeterInfoDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnMeterInfoID = this.Columns["MeterInfoID"];
        this.columnMeterHardwareID = this.Columns["MeterHardwareID"];
        this.columnMeterTypeID = this.Columns["MeterTypeID"];
        this.columnPPSArtikelNr = this.Columns["PPSArtikelNr"];
        this.columnDefaultFunctionNr = this.Columns["DefaultFunctionNr"];
        this.columnDescription = this.Columns["Description"];
        this.columnHardwareTypeID = this.Columns["HardwareTypeID"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
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
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.columnHardwareTypeID = new DataColumn("HardwareTypeID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHardwareTypeID);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnMeterInfoID
        }, false));
        this.columnMeterInfoID.Unique = true;
        this.columnPPSArtikelNr.MaxLength = 50;
        this.columnDefaultFunctionNr.MaxLength = 50;
        this.columnDescription.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterInfoRow NewMeterInfoRow() => (BaseTables.MeterInfoRow) this.NewRow();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.MeterInfoRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.MeterInfoRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterInfoRowChanged == null)
          return;
        this.MeterInfoRowChanged((object) this, new BaseTables.MeterInfoRowChangeEvent((BaseTables.MeterInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterInfoRowChanging == null)
          return;
        this.MeterInfoRowChanging((object) this, new BaseTables.MeterInfoRowChangeEvent((BaseTables.MeterInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterInfoRowDeleted == null)
          return;
        this.MeterInfoRowDeleted((object) this, new BaseTables.MeterInfoRowChangeEvent((BaseTables.MeterInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterInfoRowDeleting == null)
          return;
        this.MeterInfoRowDeleting((object) this, new BaseTables.MeterInfoRowChangeEvent((BaseTables.MeterInfoRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveMeterInfoRow(BaseTables.MeterInfoRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterInfoDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class LocationDataTable : TypedTableBase<BaseTables.LocationRow>
    {
      private DataColumn columnLocationID;
      private DataColumn columnCountry;
      private DataColumn columnRegion;
      private DataColumn columnCity;
      private DataColumn columnZip;
      private DataColumn columnStreet;
      private DataColumn columnFloor;
      private DataColumn columnHouseNumber;
      private DataColumn columnRoomNumber;
      private DataColumn columnLatitude;
      private DataColumn columnLongitude;
      private DataColumn columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public LocationDataTable()
      {
        this.TableName = "Location";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal LocationDataTable(DataTable table)
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
      protected LocationDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LocationIDColumn => this.columnLocationID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CountryColumn => this.columnCountry;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RegionColumn => this.columnRegion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CityColumn => this.columnCity;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ZipColumn => this.columnZip;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn StreetColumn => this.columnStreet;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FloorColumn => this.columnFloor;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn HouseNumberColumn => this.columnHouseNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RoomNumberColumn => this.columnRoomNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LatitudeColumn => this.columnLatitude;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LongitudeColumn => this.columnLongitude;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.LocationRow this[int index] => (BaseTables.LocationRow) this.Rows[index];

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.LocationRowChangeEventHandler LocationRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.LocationRowChangeEventHandler LocationRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.LocationRowChangeEventHandler LocationRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.LocationRowChangeEventHandler LocationRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddLocationRow(BaseTables.LocationRow row) => this.Rows.Add((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.LocationRow AddLocationRow(
        int LocationID,
        string Country,
        string Region,
        string City,
        string Zip,
        string Street,
        string Floor,
        string HouseNumber,
        string RoomNumber,
        double Latitude,
        double Longitude,
        string Description)
      {
        BaseTables.LocationRow row = (BaseTables.LocationRow) this.NewRow();
        object[] objArray = new object[12]
        {
          (object) LocationID,
          (object) Country,
          (object) Region,
          (object) City,
          (object) Zip,
          (object) Street,
          (object) Floor,
          (object) HouseNumber,
          (object) RoomNumber,
          (object) Latitude,
          (object) Longitude,
          (object) Description
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.LocationRow FindByLocationID(int LocationID)
      {
        return (BaseTables.LocationRow) this.Rows.Find(new object[1]
        {
          (object) LocationID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.LocationDataTable locationDataTable = (BaseTables.LocationDataTable) base.Clone();
        locationDataTable.InitVars();
        return (DataTable) locationDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.LocationDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnLocationID = this.Columns["LocationID"];
        this.columnCountry = this.Columns["Country"];
        this.columnRegion = this.Columns["Region"];
        this.columnCity = this.Columns["City"];
        this.columnZip = this.Columns["Zip"];
        this.columnStreet = this.Columns["Street"];
        this.columnFloor = this.Columns["Floor"];
        this.columnHouseNumber = this.Columns["HouseNumber"];
        this.columnRoomNumber = this.Columns["RoomNumber"];
        this.columnLatitude = this.Columns["Latitude"];
        this.columnLongitude = this.Columns["Longitude"];
        this.columnDescription = this.Columns["Description"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnLocationID = new DataColumn("LocationID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLocationID);
        this.columnCountry = new DataColumn("Country", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCountry);
        this.columnRegion = new DataColumn("Region", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRegion);
        this.columnCity = new DataColumn("City", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCity);
        this.columnZip = new DataColumn("Zip", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnZip);
        this.columnStreet = new DataColumn("Street", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnStreet);
        this.columnFloor = new DataColumn("Floor", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFloor);
        this.columnHouseNumber = new DataColumn("HouseNumber", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnHouseNumber);
        this.columnRoomNumber = new DataColumn("RoomNumber", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRoomNumber);
        this.columnLatitude = new DataColumn("Latitude", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLatitude);
        this.columnLongitude = new DataColumn("Longitude", typeof (double), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLongitude);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnLocationID
        }, true));
        this.columnLocationID.AllowDBNull = false;
        this.columnLocationID.Unique = true;
        this.columnCountry.MaxLength = 50;
        this.columnRegion.MaxLength = 50;
        this.columnCity.MaxLength = 50;
        this.columnZip.MaxLength = 50;
        this.columnStreet.MaxLength = 50;
        this.columnFloor.MaxLength = 50;
        this.columnHouseNumber.MaxLength = 50;
        this.columnRoomNumber.MaxLength = 50;
        this.columnDescription.MaxLength = 200;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.LocationRow NewLocationRow() => (BaseTables.LocationRow) this.NewRow();

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.LocationRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.LocationRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.LocationRowChanged == null)
          return;
        this.LocationRowChanged((object) this, new BaseTables.LocationRowChangeEvent((BaseTables.LocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.LocationRowChanging == null)
          return;
        this.LocationRowChanging((object) this, new BaseTables.LocationRowChangeEvent((BaseTables.LocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.LocationRowDeleted == null)
          return;
        this.LocationRowDeleted((object) this, new BaseTables.LocationRowChangeEvent((BaseTables.LocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.LocationRowDeleting == null)
          return;
        this.LocationRowDeleting((object) this, new BaseTables.LocationRowChangeEvent((BaseTables.LocationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveLocationRow(BaseTables.LocationRow row) => this.Rows.Remove((DataRow) row);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (LocationDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class MeterUniqueIdByARMDataTable : TypedTableBase<BaseTables.MeterUniqueIdByARMRow>
    {
      private DataColumn columnUniqueIdPart1;
      private DataColumn columnUniqueIdPart2;
      private DataColumn columnUniqueIdPart3;
      private DataColumn columnMeterID;
      private DataColumn columnCreateDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterUniqueIdByARMDataTable()
      {
        this.TableName = "MeterUniqueIdByARM";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterUniqueIdByARMDataTable(DataTable table)
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
      protected MeterUniqueIdByARMDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn UniqueIdPart1Column => this.columnUniqueIdPart1;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn UniqueIdPart2Column => this.columnUniqueIdPart2;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn UniqueIdPart3Column => this.columnUniqueIdPart3;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterIDColumn => this.columnMeterID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn CreateDateColumn => this.columnCreateDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterUniqueIdByARMRow this[int index]
      {
        get => (BaseTables.MeterUniqueIdByARMRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterUniqueIdByARMRowChangeEventHandler MeterUniqueIdByARMRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterUniqueIdByARMRowChangeEventHandler MeterUniqueIdByARMRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterUniqueIdByARMRowChangeEventHandler MeterUniqueIdByARMRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.MeterUniqueIdByARMRowChangeEventHandler MeterUniqueIdByARMRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddMeterUniqueIdByARMRow(BaseTables.MeterUniqueIdByARMRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterUniqueIdByARMRow AddMeterUniqueIdByARMRow(
        uint UniqueIdPart1,
        uint UniqueIdPart2,
        uint UniqueIdPart3,
        uint MeterID,
        DateTime CreateDate)
      {
        BaseTables.MeterUniqueIdByARMRow row = (BaseTables.MeterUniqueIdByARMRow) this.NewRow();
        object[] objArray = new object[5]
        {
          (object) UniqueIdPart1,
          (object) UniqueIdPart2,
          (object) UniqueIdPart3,
          (object) MeterID,
          (object) CreateDate
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterUniqueIdByARMRow FindByUniqueIdPart1UniqueIdPart2UniqueIdPart3(
        uint UniqueIdPart1,
        uint UniqueIdPart2,
        uint UniqueIdPart3)
      {
        return (BaseTables.MeterUniqueIdByARMRow) this.Rows.Find(new object[3]
        {
          (object) UniqueIdPart1,
          (object) UniqueIdPart2,
          (object) UniqueIdPart3
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.MeterUniqueIdByARMDataTable idByArmDataTable = (BaseTables.MeterUniqueIdByARMDataTable) base.Clone();
        idByArmDataTable.InitVars();
        return (DataTable) idByArmDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.MeterUniqueIdByARMDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnUniqueIdPart1 = this.Columns["UniqueIdPart1"];
        this.columnUniqueIdPart2 = this.Columns["UniqueIdPart2"];
        this.columnUniqueIdPart3 = this.Columns["UniqueIdPart3"];
        this.columnMeterID = this.Columns["MeterID"];
        this.columnCreateDate = this.Columns["CreateDate"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnUniqueIdPart1 = new DataColumn("UniqueIdPart1", typeof (uint), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUniqueIdPart1);
        this.columnUniqueIdPart2 = new DataColumn("UniqueIdPart2", typeof (uint), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUniqueIdPart2);
        this.columnUniqueIdPart3 = new DataColumn("UniqueIdPart3", typeof (uint), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUniqueIdPart3);
        this.columnMeterID = new DataColumn("MeterID", typeof (uint), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterID);
        this.columnCreateDate = new DataColumn("CreateDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCreateDate);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[3]
        {
          this.columnUniqueIdPart1,
          this.columnUniqueIdPart2,
          this.columnUniqueIdPart3
        }, true));
        this.columnUniqueIdPart1.AllowDBNull = false;
        this.columnUniqueIdPart2.AllowDBNull = false;
        this.columnUniqueIdPart3.AllowDBNull = false;
        this.columnMeterID.AllowDBNull = false;
        this.columnCreateDate.AllowDBNull = false;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterUniqueIdByARMRow NewMeterUniqueIdByARMRow()
      {
        return (BaseTables.MeterUniqueIdByARMRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.MeterUniqueIdByARMRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.MeterUniqueIdByARMRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.MeterUniqueIdByARMRowChanged == null)
          return;
        this.MeterUniqueIdByARMRowChanged((object) this, new BaseTables.MeterUniqueIdByARMRowChangeEvent((BaseTables.MeterUniqueIdByARMRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.MeterUniqueIdByARMRowChanging == null)
          return;
        this.MeterUniqueIdByARMRowChanging((object) this, new BaseTables.MeterUniqueIdByARMRowChangeEvent((BaseTables.MeterUniqueIdByARMRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.MeterUniqueIdByARMRowDeleted == null)
          return;
        this.MeterUniqueIdByARMRowDeleted((object) this, new BaseTables.MeterUniqueIdByARMRowChangeEvent((BaseTables.MeterUniqueIdByARMRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.MeterUniqueIdByARMRowDeleting == null)
          return;
        this.MeterUniqueIdByARMRowDeleting((object) this, new BaseTables.MeterUniqueIdByARMRowChangeEvent((BaseTables.MeterUniqueIdByARMRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveMeterUniqueIdByARMRow(BaseTables.MeterUniqueIdByARMRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (MeterUniqueIdByARMDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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
    public class ApprovalTypesDataTable : TypedTableBase<BaseTables.ApprovalTypesRow>
    {
      private DataColumn columnApprovalID;
      private DataColumn columnApproval;
      private DataColumn columnRevision;
      private DataColumn columnValidFrom;
      private DataColumn columnValidTo;
      private DataColumn columnMeterHardwareID;
      private DataColumn columnRestrictions;
      private DataColumn columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ApprovalTypesDataTable()
      {
        this.TableName = "ApprovalTypes";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ApprovalTypesDataTable(DataTable table)
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
      protected ApprovalTypesDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ApprovalIDColumn => this.columnApprovalID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ApprovalColumn => this.columnApproval;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RevisionColumn => this.columnRevision;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ValidFromColumn => this.columnValidFrom;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ValidToColumn => this.columnValidTo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MeterHardwareIDColumn => this.columnMeterHardwareID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RestrictionsColumn => this.columnRestrictions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ApprovalTypesRow this[int index]
      {
        get => (BaseTables.ApprovalTypesRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ApprovalTypesRowChangeEventHandler ApprovalTypesRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ApprovalTypesRowChangeEventHandler ApprovalTypesRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ApprovalTypesRowChangeEventHandler ApprovalTypesRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event BaseTables.ApprovalTypesRowChangeEventHandler ApprovalTypesRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddApprovalTypesRow(BaseTables.ApprovalTypesRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ApprovalTypesRow AddApprovalTypesRow(
        int ApprovalID,
        string Approval,
        string Revision,
        DateTime ValidFrom,
        DateTime ValidTo,
        int MeterHardwareID,
        string Restrictions,
        string Description)
      {
        BaseTables.ApprovalTypesRow row = (BaseTables.ApprovalTypesRow) this.NewRow();
        object[] objArray = new object[8]
        {
          (object) ApprovalID,
          (object) Approval,
          (object) Revision,
          (object) ValidFrom,
          (object) ValidTo,
          (object) MeterHardwareID,
          (object) Restrictions,
          (object) Description
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ApprovalTypesRow FindByApprovalRevisionRestrictions(
        string Approval,
        string Revision,
        string Restrictions)
      {
        return (BaseTables.ApprovalTypesRow) this.Rows.Find(new object[3]
        {
          (object) Approval,
          (object) Revision,
          (object) Restrictions
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        BaseTables.ApprovalTypesDataTable approvalTypesDataTable = (BaseTables.ApprovalTypesDataTable) base.Clone();
        approvalTypesDataTable.InitVars();
        return (DataTable) approvalTypesDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new BaseTables.ApprovalTypesDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnApprovalID = this.Columns["ApprovalID"];
        this.columnApproval = this.Columns["Approval"];
        this.columnRevision = this.Columns["Revision"];
        this.columnValidFrom = this.Columns["ValidFrom"];
        this.columnValidTo = this.Columns["ValidTo"];
        this.columnMeterHardwareID = this.Columns["MeterHardwareID"];
        this.columnRestrictions = this.Columns["Restrictions"];
        this.columnDescription = this.Columns["Description"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnApprovalID = new DataColumn("ApprovalID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnApprovalID);
        this.columnApproval = new DataColumn("Approval", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnApproval);
        this.columnRevision = new DataColumn("Revision", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRevision);
        this.columnValidFrom = new DataColumn("ValidFrom", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValidFrom);
        this.columnValidTo = new DataColumn("ValidTo", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValidTo);
        this.columnMeterHardwareID = new DataColumn("MeterHardwareID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMeterHardwareID);
        this.columnRestrictions = new DataColumn("Restrictions", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRestrictions);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint2", new DataColumn[3]
        {
          this.columnApproval,
          this.columnRevision,
          this.columnRestrictions
        }, true));
        this.columnApprovalID.AllowDBNull = false;
        this.columnApproval.AllowDBNull = false;
        this.columnApproval.MaxLength = (int) byte.MaxValue;
        this.columnRevision.AllowDBNull = false;
        this.columnRevision.MaxLength = (int) byte.MaxValue;
        this.columnValidFrom.AllowDBNull = false;
        this.columnMeterHardwareID.AllowDBNull = false;
        this.columnRestrictions.AllowDBNull = false;
        this.columnRestrictions.MaxLength = (int) byte.MaxValue;
        this.columnDescription.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ApprovalTypesRow NewApprovalTypesRow()
      {
        return (BaseTables.ApprovalTypesRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new BaseTables.ApprovalTypesRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (BaseTables.ApprovalTypesRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ApprovalTypesRowChanged == null)
          return;
        this.ApprovalTypesRowChanged((object) this, new BaseTables.ApprovalTypesRowChangeEvent((BaseTables.ApprovalTypesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ApprovalTypesRowChanging == null)
          return;
        this.ApprovalTypesRowChanging((object) this, new BaseTables.ApprovalTypesRowChangeEvent((BaseTables.ApprovalTypesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ApprovalTypesRowDeleted == null)
          return;
        this.ApprovalTypesRowDeleted((object) this, new BaseTables.ApprovalTypesRowChangeEvent((BaseTables.ApprovalTypesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ApprovalTypesRowDeleting == null)
          return;
        this.ApprovalTypesRowDeleting((object) this, new BaseTables.ApprovalTypesRowChangeEvent((BaseTables.ApprovalTypesRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveApprovalTypesRow(BaseTables.ApprovalTypesRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        BaseTables baseTables = new BaseTables();
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
          FixedValue = baseTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ApprovalTypesDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = baseTables.GetSchemaSerializable();
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

    public class MeterRow : DataRow
    {
      private BaseTables.MeterDataTable tableMeter;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeter = (BaseTables.MeterDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterID
      {
        get => (int) this[this.tableMeter.MeterIDColumn];
        set => this[this.tableMeter.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterInfoID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeter.MeterInfoIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterInfoID' in table 'Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeter.MeterInfoIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string SerialNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeter.SerialNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerialNr' in table 'Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeter.SerialNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ProductionDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableMeter.ProductionDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ProductionDate' in table 'Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeter.ProductionDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ApprovalDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableMeter.ApprovalDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ApprovalDate' in table 'Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeter.ApprovalDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string OrderNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeter.OrderNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'OrderNr' in table 'Meter' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeter.OrderNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMeterInfoIDNull() => this.IsNull(this.tableMeter.MeterInfoIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMeterInfoIDNull() => this[this.tableMeter.MeterInfoIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSerialNrNull() => this.IsNull(this.tableMeter.SerialNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSerialNrNull() => this[this.tableMeter.SerialNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsProductionDateNull() => this.IsNull(this.tableMeter.ProductionDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetProductionDateNull()
      {
        this[this.tableMeter.ProductionDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsApprovalDateNull() => this.IsNull(this.tableMeter.ApprovalDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetApprovalDateNull()
      {
        this[this.tableMeter.ApprovalDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsOrderNrNull() => this.IsNull(this.tableMeter.OrderNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetOrderNrNull() => this[this.tableMeter.OrderNrColumn] = Convert.DBNull;
    }

    public class MeterChangesRow : DataRow
    {
      private BaseTables.MeterChangesDataTable tableMeterChanges;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterChangesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterChanges = (BaseTables.MeterChangesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int ChangeID
      {
        get => (int) this[this.tableMeterChanges.ChangeIDColumn];
        set => this[this.tableMeterChanges.ChangeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterChanges.MeterIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterID' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ChangeDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableMeterChanges.ChangeDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChangeDate' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.ChangeDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterInfoID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterChanges.MeterInfoIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterInfoID' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.MeterInfoIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string SerialNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterChanges.SerialNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SerialNr' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.SerialNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ProductionDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableMeterChanges.ProductionDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ProductionDate' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.ProductionDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ApprovalDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableMeterChanges.ApprovalDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ApprovalDate' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.ApprovalDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string OrderNr
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterChanges.OrderNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'OrderNr' in table 'MeterChanges' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterChanges.OrderNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMeterIDNull() => this.IsNull(this.tableMeterChanges.MeterIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMeterIDNull() => this[this.tableMeterChanges.MeterIDColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsChangeDateNull() => this.IsNull(this.tableMeterChanges.ChangeDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetChangeDateNull()
      {
        this[this.tableMeterChanges.ChangeDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMeterInfoIDNull() => this.IsNull(this.tableMeterChanges.MeterInfoIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMeterInfoIDNull()
      {
        this[this.tableMeterChanges.MeterInfoIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSerialNrNull() => this.IsNull(this.tableMeterChanges.SerialNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSerialNrNull() => this[this.tableMeterChanges.SerialNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsProductionDateNull()
      {
        return this.IsNull(this.tableMeterChanges.ProductionDateColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetProductionDateNull()
      {
        this[this.tableMeterChanges.ProductionDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsApprovalDateNull() => this.IsNull(this.tableMeterChanges.ApprovalDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetApprovalDateNull()
      {
        this[this.tableMeterChanges.ApprovalDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsOrderNrNull() => this.IsNull(this.tableMeterChanges.OrderNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetOrderNrNull() => this[this.tableMeterChanges.OrderNrColumn] = Convert.DBNull;
    }

    public class DatabaseLocationRow : DataRow
    {
      private BaseTables.DatabaseLocationDataTable tableDatabaseLocation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal DatabaseLocationRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableDatabaseLocation = (BaseTables.DatabaseLocationDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string DatabaseLocationName
      {
        get => (string) this[this.tableDatabaseLocation.DatabaseLocationNameColumn];
        set => this[this.tableDatabaseLocation.DatabaseLocationNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Land
      {
        get
        {
          try
          {
            return (string) this[this.tableDatabaseLocation.LandColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Land' in table 'DatabaseLocation' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDatabaseLocation.LandColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Town
      {
        get
        {
          try
          {
            return (string) this[this.tableDatabaseLocation.TownColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Town' in table 'DatabaseLocation' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDatabaseLocation.TownColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string CompanyName
      {
        get
        {
          try
          {
            return (string) this[this.tableDatabaseLocation.CompanyNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CompanyName' in table 'DatabaseLocation' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDatabaseLocation.CompanyNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsLandNull() => this.IsNull(this.tableDatabaseLocation.LandColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetLandNull() => this[this.tableDatabaseLocation.LandColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsTownNull() => this.IsNull(this.tableDatabaseLocation.TownColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetTownNull() => this[this.tableDatabaseLocation.TownColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsCompanyNameNull() => this.IsNull(this.tableDatabaseLocation.CompanyNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetCompanyNameNull()
      {
        this[this.tableDatabaseLocation.CompanyNameColumn] = Convert.DBNull;
      }
    }

    public class DatabaseIdentificationRow : DataRow
    {
      private BaseTables.DatabaseIdentificationDataTable tableDatabaseIdentification;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal DatabaseIdentificationRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableDatabaseIdentification = (BaseTables.DatabaseIdentificationDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string InfoName
      {
        get => (string) this[this.tableDatabaseIdentification.InfoNameColumn];
        set => this[this.tableDatabaseIdentification.InfoNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string InfoData
      {
        get
        {
          try
          {
            return (string) this[this.tableDatabaseIdentification.InfoDataColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InfoData' in table 'DatabaseIdentification' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableDatabaseIdentification.InfoDataColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsInfoDataNull() => this.IsNull(this.tableDatabaseIdentification.InfoDataColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetInfoDataNull()
      {
        this[this.tableDatabaseIdentification.InfoDataColumn] = Convert.DBNull;
      }
    }

    public class ZRGlobalIDRow : DataRow
    {
      private BaseTables.ZRGlobalIDDataTable tableZRGlobalID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ZRGlobalIDRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableZRGlobalID = (BaseTables.ZRGlobalIDDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ZRTableName
      {
        get => (string) this[this.tableZRGlobalID.ZRTableNameColumn];
        set => this[this.tableZRGlobalID.ZRTableNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ZRFieldName
      {
        get
        {
          try
          {
            return (string) this[this.tableZRGlobalID.ZRFieldNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZRFieldName' in table 'ZRGlobalID' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZRGlobalID.ZRFieldNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int ZRNextNr
      {
        get
        {
          try
          {
            return (int) this[this.tableZRGlobalID.ZRNextNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZRNextNr' in table 'ZRGlobalID' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZRGlobalID.ZRNextNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int ZRFirstNr
      {
        get
        {
          try
          {
            return (int) this[this.tableZRGlobalID.ZRFirstNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZRFirstNr' in table 'ZRGlobalID' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZRGlobalID.ZRFirstNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int ZRLastNr
      {
        get
        {
          try
          {
            return (int) this[this.tableZRGlobalID.ZRLastNrColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ZRLastNr' in table 'ZRGlobalID' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableZRGlobalID.ZRLastNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string DatabaseLocationName
      {
        get => (string) this[this.tableZRGlobalID.DatabaseLocationNameColumn];
        set => this[this.tableZRGlobalID.DatabaseLocationNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsZRFieldNameNull() => this.IsNull(this.tableZRGlobalID.ZRFieldNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetZRFieldNameNull()
      {
        this[this.tableZRGlobalID.ZRFieldNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsZRNextNrNull() => this.IsNull(this.tableZRGlobalID.ZRNextNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetZRNextNrNull() => this[this.tableZRGlobalID.ZRNextNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsZRFirstNrNull() => this.IsNull(this.tableZRGlobalID.ZRFirstNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetZRFirstNrNull() => this[this.tableZRGlobalID.ZRFirstNrColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsZRLastNrNull() => this.IsNull(this.tableZRGlobalID.ZRLastNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetZRLastNrNull() => this[this.tableZRGlobalID.ZRLastNrColumn] = Convert.DBNull;
    }

    public class MeterDataRow : DataRow
    {
      private BaseTables.MeterDataDataTable tableMeterData;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterDataRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterData = (BaseTables.MeterDataDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterID
      {
        get => (int) this[this.tableMeterData.MeterIDColumn];
        set => this[this.tableMeterData.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime TimePoint
      {
        get => (DateTime) this[this.tableMeterData.TimePointColumn];
        set => this[this.tableMeterData.TimePointColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int PValueID
      {
        get => (int) this[this.tableMeterData.PValueIDColumn];
        set => this[this.tableMeterData.PValueIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string PValue
      {
        get
        {
          try
          {
            return (string) this[this.tableMeterData.PValueColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PValue' in table 'MeterData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterData.PValueColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public byte[] PValueBinary
      {
        get
        {
          try
          {
            return (byte[]) this[this.tableMeterData.PValueBinaryColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PValueBinary' in table 'MeterData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterData.PValueBinaryColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public byte SyncStatus
      {
        get
        {
          try
          {
            return (byte) this[this.tableMeterData.SyncStatusColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SyncStatus' in table 'MeterData' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterData.SyncStatusColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsPValueNull() => this.IsNull(this.tableMeterData.PValueColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetPValueNull() => this[this.tableMeterData.PValueColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsPValueBinaryNull() => this.IsNull(this.tableMeterData.PValueBinaryColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetPValueBinaryNull()
      {
        this[this.tableMeterData.PValueBinaryColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsSyncStatusNull() => this.IsNull(this.tableMeterData.SyncStatusColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetSyncStatusNull()
      {
        this[this.tableMeterData.SyncStatusColumn] = Convert.DBNull;
      }
    }

    public class HardwareTypeRow : DataRow
    {
      private BaseTables.HardwareTypeDataTable tableHardwareType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal HardwareTypeRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableHardwareType = (BaseTables.HardwareTypeDataTable) this.Table;
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

    public class OnlineTranslationBaseMassagesRow : DataRow
    {
      private BaseTables.OnlineTranslationBaseMassagesDataTable tableOnlineTranslationBaseMassages;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal OnlineTranslationBaseMassagesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableOnlineTranslationBaseMassages = (BaseTables.OnlineTranslationBaseMassagesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string MessageTree
      {
        get => (string) this[this.tableOnlineTranslationBaseMassages.MessageTreeColumn];
        set => this[this.tableOnlineTranslationBaseMassages.MessageTreeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string DefaultText
      {
        get
        {
          try
          {
            return (string) this[this.tableOnlineTranslationBaseMassages.DefaultTextColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'DefaultText' in table 'OnlineTranslationBaseMassages' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableOnlineTranslationBaseMassages.DefaultTextColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDefaultTextNull()
      {
        return this.IsNull(this.tableOnlineTranslationBaseMassages.DefaultTextColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDefaultTextNull()
      {
        this[this.tableOnlineTranslationBaseMassages.DefaultTextColumn] = Convert.DBNull;
      }
    }

    public class OnlineTranslationsRow : DataRow
    {
      private BaseTables.OnlineTranslationsDataTable tableOnlineTranslations;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal OnlineTranslationsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableOnlineTranslations = (BaseTables.OnlineTranslationsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int TranslationGroup
      {
        get => (int) this[this.tableOnlineTranslations.TranslationGroupColumn];
        set => this[this.tableOnlineTranslations.TranslationGroupColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string TextKey
      {
        get => (string) this[this.tableOnlineTranslations.TextKeyColumn];
        set => this[this.tableOnlineTranslations.TextKeyColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string LanguageCode
      {
        get => (string) this[this.tableOnlineTranslations.LanguageCodeColumn];
        set => this[this.tableOnlineTranslations.LanguageCodeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string LanguageText
      {
        get
        {
          try
          {
            return (string) this[this.tableOnlineTranslations.LanguageTextColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LanguageText' in table 'OnlineTranslations' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableOnlineTranslations.LanguageTextColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MessageNumber
      {
        get
        {
          try
          {
            return (int) this[this.tableOnlineTranslations.MessageNumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MessageNumber' in table 'OnlineTranslations' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableOnlineTranslations.MessageNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsLanguageTextNull()
      {
        return this.IsNull(this.tableOnlineTranslations.LanguageTextColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetLanguageTextNull()
      {
        this[this.tableOnlineTranslations.LanguageTextColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMessageNumberNull()
      {
        return this.IsNull(this.tableOnlineTranslations.MessageNumberColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMessageNumberNull()
      {
        this[this.tableOnlineTranslations.MessageNumberColumn] = Convert.DBNull;
      }
    }

    public class MeterInfoRow : DataRow
    {
      private BaseTables.MeterInfoDataTable tableMeterInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterInfoRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterInfo = (BaseTables.MeterInfoDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterInfoID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfo.MeterInfoIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterInfoID' in table 'MeterInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfo.MeterInfoIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterHardwareID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfo.MeterHardwareIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterHardwareID' in table 'MeterInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfo.MeterHardwareIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterTypeID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfo.MeterTypeIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MeterTypeID' in table 'MeterInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfo.MeterTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string PPSArtikelNr
      {
        get
        {
          return this.IsPPSArtikelNrNull() ? (string) null : (string) this[this.tableMeterInfo.PPSArtikelNrColumn];
        }
        set => this[this.tableMeterInfo.PPSArtikelNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string DefaultFunctionNr
      {
        get
        {
          return this.IsDefaultFunctionNrNull() ? (string) null : (string) this[this.tableMeterInfo.DefaultFunctionNrColumn];
        }
        set => this[this.tableMeterInfo.DefaultFunctionNrColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          return this.IsDescriptionNull() ? (string) null : (string) this[this.tableMeterInfo.DescriptionColumn];
        }
        set => this[this.tableMeterInfo.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int HardwareTypeID
      {
        get
        {
          try
          {
            return (int) this[this.tableMeterInfo.HardwareTypeIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HardwareTypeID' in table 'MeterInfo' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableMeterInfo.HardwareTypeIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMeterInfoIDNull() => this.IsNull(this.tableMeterInfo.MeterInfoIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMeterInfoIDNull()
      {
        this[this.tableMeterInfo.MeterInfoIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMeterHardwareIDNull() => this.IsNull(this.tableMeterInfo.MeterHardwareIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMeterHardwareIDNull()
      {
        this[this.tableMeterInfo.MeterHardwareIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMeterTypeIDNull() => this.IsNull(this.tableMeterInfo.MeterTypeIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMeterTypeIDNull()
      {
        this[this.tableMeterInfo.MeterTypeIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsPPSArtikelNrNull() => this.IsNull(this.tableMeterInfo.PPSArtikelNrColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetPPSArtikelNrNull()
      {
        this[this.tableMeterInfo.PPSArtikelNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDefaultFunctionNrNull()
      {
        return this.IsNull(this.tableMeterInfo.DefaultFunctionNrColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDefaultFunctionNrNull()
      {
        this[this.tableMeterInfo.DefaultFunctionNrColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableMeterInfo.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableMeterInfo.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHardwareTypeIDNull() => this.IsNull(this.tableMeterInfo.HardwareTypeIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHardwareTypeIDNull()
      {
        this[this.tableMeterInfo.HardwareTypeIDColumn] = Convert.DBNull;
      }
    }

    public class LocationRow : DataRow
    {
      private BaseTables.LocationDataTable tableLocation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal LocationRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableLocation = (BaseTables.LocationDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int LocationID
      {
        get => (int) this[this.tableLocation.LocationIDColumn];
        set => this[this.tableLocation.LocationIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Country
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.CountryColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Country' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.CountryColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Region
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.RegionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Region' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.RegionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string City
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.CityColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'City' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.CityColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Zip
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.ZipColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Zip' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.ZipColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Street
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.StreetColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Street' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.StreetColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Floor
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.FloorColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Floor' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.FloorColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string HouseNumber
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.HouseNumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'HouseNumber' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.HouseNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string RoomNumber
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.RoomNumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RoomNumber' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.RoomNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public double Latitude
      {
        get
        {
          try
          {
            return (double) this[this.tableLocation.LatitudeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Latitude' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.LatitudeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public double Longitude
      {
        get
        {
          try
          {
            return (double) this[this.tableLocation.LongitudeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Longitude' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.LongitudeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableLocation.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'Location' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableLocation.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsCountryNull() => this.IsNull(this.tableLocation.CountryColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetCountryNull() => this[this.tableLocation.CountryColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsRegionNull() => this.IsNull(this.tableLocation.RegionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetRegionNull() => this[this.tableLocation.RegionColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsCityNull() => this.IsNull(this.tableLocation.CityColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetCityNull() => this[this.tableLocation.CityColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsZipNull() => this.IsNull(this.tableLocation.ZipColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetZipNull() => this[this.tableLocation.ZipColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsStreetNull() => this.IsNull(this.tableLocation.StreetColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetStreetNull() => this[this.tableLocation.StreetColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFloorNull() => this.IsNull(this.tableLocation.FloorColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFloorNull() => this[this.tableLocation.FloorColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsHouseNumberNull() => this.IsNull(this.tableLocation.HouseNumberColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetHouseNumberNull()
      {
        this[this.tableLocation.HouseNumberColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsRoomNumberNull() => this.IsNull(this.tableLocation.RoomNumberColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetRoomNumberNull() => this[this.tableLocation.RoomNumberColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsLatitudeNull() => this.IsNull(this.tableLocation.LatitudeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetLatitudeNull() => this[this.tableLocation.LatitudeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsLongitudeNull() => this.IsNull(this.tableLocation.LongitudeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetLongitudeNull() => this[this.tableLocation.LongitudeColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableLocation.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableLocation.DescriptionColumn] = Convert.DBNull;
      }
    }

    public class MeterUniqueIdByARMRow : DataRow
    {
      private BaseTables.MeterUniqueIdByARMDataTable tableMeterUniqueIdByARM;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal MeterUniqueIdByARMRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableMeterUniqueIdByARM = (BaseTables.MeterUniqueIdByARMDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public uint UniqueIdPart1
      {
        get => (uint) this[this.tableMeterUniqueIdByARM.UniqueIdPart1Column];
        set => this[this.tableMeterUniqueIdByARM.UniqueIdPart1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public uint UniqueIdPart2
      {
        get => (uint) this[this.tableMeterUniqueIdByARM.UniqueIdPart2Column];
        set => this[this.tableMeterUniqueIdByARM.UniqueIdPart2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public uint UniqueIdPart3
      {
        get => (uint) this[this.tableMeterUniqueIdByARM.UniqueIdPart3Column];
        set => this[this.tableMeterUniqueIdByARM.UniqueIdPart3Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public uint MeterID
      {
        get => (uint) this[this.tableMeterUniqueIdByARM.MeterIDColumn];
        set => this[this.tableMeterUniqueIdByARM.MeterIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime CreateDate
      {
        get => (DateTime) this[this.tableMeterUniqueIdByARM.CreateDateColumn];
        set => this[this.tableMeterUniqueIdByARM.CreateDateColumn] = (object) value;
      }
    }

    public class ApprovalTypesRow : DataRow
    {
      private BaseTables.ApprovalTypesDataTable tableApprovalTypes;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ApprovalTypesRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableApprovalTypes = (BaseTables.ApprovalTypesDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int ApprovalID
      {
        get => (int) this[this.tableApprovalTypes.ApprovalIDColumn];
        set => this[this.tableApprovalTypes.ApprovalIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Approval
      {
        get => (string) this[this.tableApprovalTypes.ApprovalColumn];
        set => this[this.tableApprovalTypes.ApprovalColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Revision
      {
        get => (string) this[this.tableApprovalTypes.RevisionColumn];
        set => this[this.tableApprovalTypes.RevisionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ValidFrom
      {
        get => (DateTime) this[this.tableApprovalTypes.ValidFromColumn];
        set => this[this.tableApprovalTypes.ValidFromColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DateTime ValidTo
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableApprovalTypes.ValidToColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ValidTo' in table 'ApprovalTypes' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableApprovalTypes.ValidToColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int MeterHardwareID
      {
        get => (int) this[this.tableApprovalTypes.MeterHardwareIDColumn];
        set => this[this.tableApprovalTypes.MeterHardwareIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Restrictions
      {
        get => (string) this[this.tableApprovalTypes.RestrictionsColumn];
        set => this[this.tableApprovalTypes.RestrictionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableApprovalTypes.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'ApprovalTypes' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableApprovalTypes.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsValidToNull() => this.IsNull(this.tableApprovalTypes.ValidToColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetValidToNull() => this[this.tableApprovalTypes.ValidToColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableApprovalTypes.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableApprovalTypes.DescriptionColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class MeterRowChangeEvent : EventArgs
    {
      private BaseTables.MeterRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterRowChangeEvent(BaseTables.MeterRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class MeterChangesRowChangeEvent : EventArgs
    {
      private BaseTables.MeterChangesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterChangesRowChangeEvent(BaseTables.MeterChangesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterChangesRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class DatabaseLocationRowChangeEvent : EventArgs
    {
      private BaseTables.DatabaseLocationRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DatabaseLocationRowChangeEvent(
        BaseTables.DatabaseLocationRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseLocationRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class DatabaseIdentificationRowChangeEvent : EventArgs
    {
      private BaseTables.DatabaseIdentificationRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DatabaseIdentificationRowChangeEvent(
        BaseTables.DatabaseIdentificationRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.DatabaseIdentificationRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class ZRGlobalIDRowChangeEvent : EventArgs
    {
      private BaseTables.ZRGlobalIDRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ZRGlobalIDRowChangeEvent(BaseTables.ZRGlobalIDRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ZRGlobalIDRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class MeterDataRowChangeEvent : EventArgs
    {
      private BaseTables.MeterDataRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterDataRowChangeEvent(BaseTables.MeterDataRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterDataRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class HardwareTypeRowChangeEvent : EventArgs
    {
      private BaseTables.HardwareTypeRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HardwareTypeRowChangeEvent(BaseTables.HardwareTypeRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.HardwareTypeRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class OnlineTranslationBaseMassagesRowChangeEvent : EventArgs
    {
      private BaseTables.OnlineTranslationBaseMassagesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public OnlineTranslationBaseMassagesRowChangeEvent(
        BaseTables.OnlineTranslationBaseMassagesRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationBaseMassagesRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class OnlineTranslationsRowChangeEvent : EventArgs
    {
      private BaseTables.OnlineTranslationsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public OnlineTranslationsRowChangeEvent(
        BaseTables.OnlineTranslationsRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.OnlineTranslationsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class MeterInfoRowChangeEvent : EventArgs
    {
      private BaseTables.MeterInfoRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterInfoRowChangeEvent(BaseTables.MeterInfoRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterInfoRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class LocationRowChangeEvent : EventArgs
    {
      private BaseTables.LocationRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public LocationRowChangeEvent(BaseTables.LocationRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.LocationRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class MeterUniqueIdByARMRowChangeEvent : EventArgs
    {
      private BaseTables.MeterUniqueIdByARMRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public MeterUniqueIdByARMRowChangeEvent(
        BaseTables.MeterUniqueIdByARMRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.MeterUniqueIdByARMRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class ApprovalTypesRowChangeEvent : EventArgs
    {
      private BaseTables.ApprovalTypesRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ApprovalTypesRowChangeEvent(BaseTables.ApprovalTypesRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public BaseTables.ApprovalTypesRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
