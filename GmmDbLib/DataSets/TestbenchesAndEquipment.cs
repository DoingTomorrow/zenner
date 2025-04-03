// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DataSets.TestbenchesAndEquipment
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
  [XmlRoot("TestbenchesAndEquipment")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class TestbenchesAndEquipment : DataSet
  {
    private TestbenchesAndEquipment.InstallationsDataTable tableInstallations;
    private TestbenchesAndEquipment.InstallationChangeLogDataTable tableInstallationChangeLog;
    private TestbenchesAndEquipment.EquipmentPartlistCreationDataTable tableEquipmentPartlistCreation;
    private TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable tableEquipmentTypeDescription;
    private TestbenchesAndEquipment.InstallationUsersDataTable tableInstallationUsers;
    private TestbenchesAndEquipment.SoftwareUsersDataTable tableSoftwareUsers;
    private TestbenchesAndEquipment.EquipmentChangeLogDataTable tableEquipmentChangeLog;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public TestbenchesAndEquipment()
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
    protected TestbenchesAndEquipment(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (Installations)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.InstallationsDataTable(dataSet.Tables[nameof (Installations)]));
          if (dataSet.Tables[nameof (InstallationChangeLog)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.InstallationChangeLogDataTable(dataSet.Tables[nameof (InstallationChangeLog)]));
          if (dataSet.Tables[nameof (EquipmentPartlistCreation)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.EquipmentPartlistCreationDataTable(dataSet.Tables[nameof (EquipmentPartlistCreation)]));
          if (dataSet.Tables[nameof (EquipmentTypeDescription)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable(dataSet.Tables[nameof (EquipmentTypeDescription)]));
          if (dataSet.Tables[nameof (InstallationUsers)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.InstallationUsersDataTable(dataSet.Tables[nameof (InstallationUsers)]));
          if (dataSet.Tables[nameof (SoftwareUsers)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.SoftwareUsersDataTable(dataSet.Tables[nameof (SoftwareUsers)]));
          if (dataSet.Tables[nameof (EquipmentChangeLog)] != null)
            base.Tables.Add((DataTable) new TestbenchesAndEquipment.EquipmentChangeLogDataTable(dataSet.Tables[nameof (EquipmentChangeLog)]));
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
    public TestbenchesAndEquipment.InstallationsDataTable Installations => this.tableInstallations;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TestbenchesAndEquipment.InstallationChangeLogDataTable InstallationChangeLog
    {
      get => this.tableInstallationChangeLog;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TestbenchesAndEquipment.EquipmentPartlistCreationDataTable EquipmentPartlistCreation
    {
      get => this.tableEquipmentPartlistCreation;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable EquipmentTypeDescription
    {
      get => this.tableEquipmentTypeDescription;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TestbenchesAndEquipment.InstallationUsersDataTable InstallationUsers
    {
      get => this.tableInstallationUsers;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TestbenchesAndEquipment.SoftwareUsersDataTable SoftwareUsers => this.tableSoftwareUsers;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public TestbenchesAndEquipment.EquipmentChangeLogDataTable EquipmentChangeLog
    {
      get => this.tableEquipmentChangeLog;
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
      TestbenchesAndEquipment testbenchesAndEquipment = (TestbenchesAndEquipment) base.Clone();
      testbenchesAndEquipment.InitVars();
      testbenchesAndEquipment.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) testbenchesAndEquipment;
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
        if (dataSet.Tables["Installations"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.InstallationsDataTable(dataSet.Tables["Installations"]));
        if (dataSet.Tables["InstallationChangeLog"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.InstallationChangeLogDataTable(dataSet.Tables["InstallationChangeLog"]));
        if (dataSet.Tables["EquipmentPartlistCreation"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.EquipmentPartlistCreationDataTable(dataSet.Tables["EquipmentPartlistCreation"]));
        if (dataSet.Tables["EquipmentTypeDescription"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable(dataSet.Tables["EquipmentTypeDescription"]));
        if (dataSet.Tables["InstallationUsers"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.InstallationUsersDataTable(dataSet.Tables["InstallationUsers"]));
        if (dataSet.Tables["SoftwareUsers"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.SoftwareUsersDataTable(dataSet.Tables["SoftwareUsers"]));
        if (dataSet.Tables["EquipmentChangeLog"] != null)
          base.Tables.Add((DataTable) new TestbenchesAndEquipment.EquipmentChangeLogDataTable(dataSet.Tables["EquipmentChangeLog"]));
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
      this.tableInstallations = (TestbenchesAndEquipment.InstallationsDataTable) base.Tables["Installations"];
      if (initTable && this.tableInstallations != null)
        this.tableInstallations.InitVars();
      this.tableInstallationChangeLog = (TestbenchesAndEquipment.InstallationChangeLogDataTable) base.Tables["InstallationChangeLog"];
      if (initTable && this.tableInstallationChangeLog != null)
        this.tableInstallationChangeLog.InitVars();
      this.tableEquipmentPartlistCreation = (TestbenchesAndEquipment.EquipmentPartlistCreationDataTable) base.Tables["EquipmentPartlistCreation"];
      if (initTable && this.tableEquipmentPartlistCreation != null)
        this.tableEquipmentPartlistCreation.InitVars();
      this.tableEquipmentTypeDescription = (TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable) base.Tables["EquipmentTypeDescription"];
      if (initTable && this.tableEquipmentTypeDescription != null)
        this.tableEquipmentTypeDescription.InitVars();
      this.tableInstallationUsers = (TestbenchesAndEquipment.InstallationUsersDataTable) base.Tables["InstallationUsers"];
      if (initTable && this.tableInstallationUsers != null)
        this.tableInstallationUsers.InitVars();
      this.tableSoftwareUsers = (TestbenchesAndEquipment.SoftwareUsersDataTable) base.Tables["SoftwareUsers"];
      if (initTable && this.tableSoftwareUsers != null)
        this.tableSoftwareUsers.InitVars();
      this.tableEquipmentChangeLog = (TestbenchesAndEquipment.EquipmentChangeLogDataTable) base.Tables["EquipmentChangeLog"];
      if (!initTable || this.tableEquipmentChangeLog == null)
        return;
      this.tableEquipmentChangeLog.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (TestbenchesAndEquipment);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/TestbenchesAndEquipment.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableInstallations = new TestbenchesAndEquipment.InstallationsDataTable();
      base.Tables.Add((DataTable) this.tableInstallations);
      this.tableInstallationChangeLog = new TestbenchesAndEquipment.InstallationChangeLogDataTable();
      base.Tables.Add((DataTable) this.tableInstallationChangeLog);
      this.tableEquipmentPartlistCreation = new TestbenchesAndEquipment.EquipmentPartlistCreationDataTable();
      base.Tables.Add((DataTable) this.tableEquipmentPartlistCreation);
      this.tableEquipmentTypeDescription = new TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable();
      base.Tables.Add((DataTable) this.tableEquipmentTypeDescription);
      this.tableInstallationUsers = new TestbenchesAndEquipment.InstallationUsersDataTable();
      base.Tables.Add((DataTable) this.tableInstallationUsers);
      this.tableSoftwareUsers = new TestbenchesAndEquipment.SoftwareUsersDataTable();
      base.Tables.Add((DataTable) this.tableSoftwareUsers);
      this.tableEquipmentChangeLog = new TestbenchesAndEquipment.EquipmentChangeLogDataTable();
      base.Tables.Add((DataTable) this.tableEquipmentChangeLog);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeInstallations() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeInstallationChangeLog() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeEquipmentPartlistCreation() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeEquipmentTypeDescription() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeInstallationUsers() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeSoftwareUsers() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeEquipmentChangeLog() => false;

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
      TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = testbenchesAndEquipment.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public class EquipmentChangeLogDataTable : 
      TypedTableBase<TestbenchesAndEquipment.EquipmentChangeLogRow>
    {
      private DataColumn columnInstallationId;
      private DataColumn columnChangeTime;
      private DataColumn columnPcName;
      private DataColumn columnInstallationPath;
      private DataColumn columnSoftwareVersion;
      private DataColumn columnLicenseName;
      private DataColumn columnLicenseCustomer;
      private DataColumn columnLicenseGeneratorID;
      private DataColumn columnLicenseGenerationTime;
      private DataColumn columnBasicState;
      private DataColumn columnState1;
      private DataColumn columnState2;
      private DataColumn columnChangeInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EquipmentChangeLogDataTable()
      {
        this.TableName = "EquipmentChangeLog";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EquipmentChangeLogDataTable(DataTable table)
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
      protected EquipmentChangeLogDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallationIdColumn => this.columnInstallationId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeTimeColumn => this.columnChangeTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PcNameColumn => this.columnPcName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallationPathColumn => this.columnInstallationPath;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SoftwareVersionColumn => this.columnSoftwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseNameColumn => this.columnLicenseName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseCustomerColumn => this.columnLicenseCustomer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseGeneratorIDColumn => this.columnLicenseGeneratorID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseGenerationTimeColumn => this.columnLicenseGenerationTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BasicStateColumn => this.columnBasicState;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn State1Column => this.columnState1;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn State2Column => this.columnState2;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeInfoColumn => this.columnChangeInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentChangeLogRow this[int index]
      {
        get => (TestbenchesAndEquipment.EquipmentChangeLogRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentChangeLogRowChangeEventHandler EquipmentChangeLogRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentChangeLogRowChangeEventHandler EquipmentChangeLogRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentChangeLogRowChangeEventHandler EquipmentChangeLogRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentChangeLogRowChangeEventHandler EquipmentChangeLogRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddEquipmentChangeLogRow(TestbenchesAndEquipment.EquipmentChangeLogRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentChangeLogRow AddEquipmentChangeLogRow(
        int InstallationId,
        DateTime ChangeTime,
        string PcName,
        string InstallationPath,
        string SoftwareVersion,
        string LicenseName,
        string LicenseCustomer,
        int LicenseGeneratorID,
        DateTime LicenseGenerationTime,
        string BasicState,
        string State1,
        string State2,
        string ChangeInfo)
      {
        TestbenchesAndEquipment.EquipmentChangeLogRow row = (TestbenchesAndEquipment.EquipmentChangeLogRow) this.NewRow();
        object[] objArray = new object[13]
        {
          (object) InstallationId,
          (object) ChangeTime,
          (object) PcName,
          (object) InstallationPath,
          (object) SoftwareVersion,
          (object) LicenseName,
          (object) LicenseCustomer,
          (object) LicenseGeneratorID,
          (object) LicenseGenerationTime,
          (object) BasicState,
          (object) State1,
          (object) State2,
          (object) ChangeInfo
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentChangeLogRow FindByInstallationIdChangeTime(
        int InstallationId,
        DateTime ChangeTime)
      {
        return (TestbenchesAndEquipment.EquipmentChangeLogRow) this.Rows.Find(new object[2]
        {
          (object) InstallationId,
          (object) ChangeTime
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.EquipmentChangeLogDataTable changeLogDataTable = (TestbenchesAndEquipment.EquipmentChangeLogDataTable) base.Clone();
        changeLogDataTable.InitVars();
        return (DataTable) changeLogDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.EquipmentChangeLogDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnInstallationId = this.Columns["InstallationId"];
        this.columnChangeTime = this.Columns["ChangeTime"];
        this.columnPcName = this.Columns["PcName"];
        this.columnInstallationPath = this.Columns["InstallationPath"];
        this.columnSoftwareVersion = this.Columns["SoftwareVersion"];
        this.columnLicenseName = this.Columns["LicenseName"];
        this.columnLicenseCustomer = this.Columns["LicenseCustomer"];
        this.columnLicenseGeneratorID = this.Columns["LicenseGeneratorID"];
        this.columnLicenseGenerationTime = this.Columns["LicenseGenerationTime"];
        this.columnBasicState = this.Columns["BasicState"];
        this.columnState1 = this.Columns["State1"];
        this.columnState2 = this.Columns["State2"];
        this.columnChangeInfo = this.Columns["ChangeInfo"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnInstallationId = new DataColumn("InstallationId", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallationId);
        this.columnChangeTime = new DataColumn("ChangeTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeTime);
        this.columnPcName = new DataColumn("PcName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPcName);
        this.columnInstallationPath = new DataColumn("InstallationPath", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallationPath);
        this.columnSoftwareVersion = new DataColumn("SoftwareVersion", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSoftwareVersion);
        this.columnLicenseName = new DataColumn("LicenseName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseName);
        this.columnLicenseCustomer = new DataColumn("LicenseCustomer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseCustomer);
        this.columnLicenseGeneratorID = new DataColumn("LicenseGeneratorID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseGeneratorID);
        this.columnLicenseGenerationTime = new DataColumn("LicenseGenerationTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseGenerationTime);
        this.columnBasicState = new DataColumn("BasicState", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBasicState);
        this.columnState1 = new DataColumn("State1", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnState1);
        this.columnState2 = new DataColumn("State2", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnState2);
        this.columnChangeInfo = new DataColumn("ChangeInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeInfo);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnInstallationId,
          this.columnChangeTime
        }, true));
        this.columnInstallationId.AllowDBNull = false;
        this.columnChangeTime.AllowDBNull = false;
        this.columnPcName.MaxLength = 100;
        this.columnInstallationPath.MaxLength = 536870910;
        this.columnSoftwareVersion.MaxLength = 50;
        this.columnLicenseName.MaxLength = (int) byte.MaxValue;
        this.columnLicenseCustomer.MaxLength = (int) byte.MaxValue;
        this.columnBasicState.MaxLength = 50;
        this.columnState1.MaxLength = 50;
        this.columnState2.MaxLength = 50;
        this.columnChangeInfo.MaxLength = 536870910;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentChangeLogRow NewEquipmentChangeLogRow()
      {
        return (TestbenchesAndEquipment.EquipmentChangeLogRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.EquipmentChangeLogRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (TestbenchesAndEquipment.EquipmentChangeLogRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.EquipmentChangeLogRowChanged == null)
          return;
        this.EquipmentChangeLogRowChanged((object) this, new TestbenchesAndEquipment.EquipmentChangeLogRowChangeEvent((TestbenchesAndEquipment.EquipmentChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.EquipmentChangeLogRowChanging == null)
          return;
        this.EquipmentChangeLogRowChanging((object) this, new TestbenchesAndEquipment.EquipmentChangeLogRowChangeEvent((TestbenchesAndEquipment.EquipmentChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.EquipmentChangeLogRowDeleted == null)
          return;
        this.EquipmentChangeLogRowDeleted((object) this, new TestbenchesAndEquipment.EquipmentChangeLogRowChangeEvent((TestbenchesAndEquipment.EquipmentChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.EquipmentChangeLogRowDeleting == null)
          return;
        this.EquipmentChangeLogRowDeleting((object) this, new TestbenchesAndEquipment.EquipmentChangeLogRowChangeEvent((TestbenchesAndEquipment.EquipmentChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveEquipmentChangeLogRow(TestbenchesAndEquipment.EquipmentChangeLogRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (EquipmentChangeLogDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public delegate void InstallationsRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.InstallationsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void InstallationChangeLogRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.InstallationChangeLogRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void EquipmentPartlistCreationRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void EquipmentTypeDescriptionRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void InstallationUsersRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.InstallationUsersRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void SoftwareUsersRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.SoftwareUsersRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void EquipmentChangeLogRowChangeEventHandler(
      object sender,
      TestbenchesAndEquipment.EquipmentChangeLogRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class InstallationsDataTable : TypedTableBase<TestbenchesAndEquipment.InstallationsRow>
    {
      private DataColumn columnInstallationId;
      private DataColumn columnPcName;
      private DataColumn columnInstallationPath;
      private DataColumn columnInstallataionName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationsDataTable()
      {
        this.TableName = "Installations";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationsDataTable(DataTable table)
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
      protected InstallationsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallationIdColumn => this.columnInstallationId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PcNameColumn => this.columnPcName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallationPathColumn => this.columnInstallationPath;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallataionNameColumn => this.columnInstallataionName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationsRow this[int index]
      {
        get => (TestbenchesAndEquipment.InstallationsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationsRowChangeEventHandler InstallationsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationsRowChangeEventHandler InstallationsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationsRowChangeEventHandler InstallationsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationsRowChangeEventHandler InstallationsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddInstallationsRow(TestbenchesAndEquipment.InstallationsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationsRow AddInstallationsRow(
        int InstallationId,
        string PcName,
        string InstallationPath,
        string InstallataionName)
      {
        TestbenchesAndEquipment.InstallationsRow row = (TestbenchesAndEquipment.InstallationsRow) this.NewRow();
        object[] objArray = new object[4]
        {
          (object) InstallationId,
          (object) PcName,
          (object) InstallationPath,
          (object) InstallataionName
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationsRow FindByInstallationId(int InstallationId)
      {
        return (TestbenchesAndEquipment.InstallationsRow) this.Rows.Find(new object[1]
        {
          (object) InstallationId
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.InstallationsDataTable installationsDataTable = (TestbenchesAndEquipment.InstallationsDataTable) base.Clone();
        installationsDataTable.InitVars();
        return (DataTable) installationsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.InstallationsDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnInstallationId = this.Columns["InstallationId"];
        this.columnPcName = this.Columns["PcName"];
        this.columnInstallationPath = this.Columns["InstallationPath"];
        this.columnInstallataionName = this.Columns["InstallataionName"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnInstallationId = new DataColumn("InstallationId", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallationId);
        this.columnPcName = new DataColumn("PcName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPcName);
        this.columnInstallationPath = new DataColumn("InstallationPath", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallationPath);
        this.columnInstallataionName = new DataColumn("InstallataionName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallataionName);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnInstallationId
        }, true));
        this.columnInstallationId.AllowDBNull = false;
        this.columnInstallationId.Unique = true;
        this.columnPcName.MaxLength = 100;
        this.columnInstallationPath.MaxLength = 536870910;
        this.columnInstallataionName.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationsRow NewInstallationsRow()
      {
        return (TestbenchesAndEquipment.InstallationsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.InstallationsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (TestbenchesAndEquipment.InstallationsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.InstallationsRowChanged == null)
          return;
        this.InstallationsRowChanged((object) this, new TestbenchesAndEquipment.InstallationsRowChangeEvent((TestbenchesAndEquipment.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.InstallationsRowChanging == null)
          return;
        this.InstallationsRowChanging((object) this, new TestbenchesAndEquipment.InstallationsRowChangeEvent((TestbenchesAndEquipment.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.InstallationsRowDeleted == null)
          return;
        this.InstallationsRowDeleted((object) this, new TestbenchesAndEquipment.InstallationsRowChangeEvent((TestbenchesAndEquipment.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.InstallationsRowDeleting == null)
          return;
        this.InstallationsRowDeleting((object) this, new TestbenchesAndEquipment.InstallationsRowChangeEvent((TestbenchesAndEquipment.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveInstallationsRow(TestbenchesAndEquipment.InstallationsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (InstallationsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public class InstallationChangeLogDataTable : 
      TypedTableBase<TestbenchesAndEquipment.InstallationChangeLogRow>
    {
      private DataColumn columnInstallationId;
      private DataColumn columnChangeTime;
      private DataColumn columnSoftwareVersion;
      private DataColumn columnLicenseName;
      private DataColumn columnLicenseCustomer;
      private DataColumn columnLicenseGeneratorID;
      private DataColumn columnLicenseGenerationTime;
      private DataColumn columnBasicState;
      private DataColumn columnState1;
      private DataColumn columnState2;
      private DataColumn columnChangeInfo;
      private DataColumn columnMainEquipmentID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationChangeLogDataTable()
      {
        this.TableName = "InstallationChangeLog";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationChangeLogDataTable(DataTable table)
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
      protected InstallationChangeLogDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallationIdColumn => this.columnInstallationId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeTimeColumn => this.columnChangeTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn SoftwareVersionColumn => this.columnSoftwareVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseNameColumn => this.columnLicenseName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseCustomerColumn => this.columnLicenseCustomer;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseGeneratorIDColumn => this.columnLicenseGeneratorID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LicenseGenerationTimeColumn => this.columnLicenseGenerationTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn BasicStateColumn => this.columnBasicState;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn State1Column => this.columnState1;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn State2Column => this.columnState2;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeInfoColumn => this.columnChangeInfo;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn MainEquipmentIDColumn => this.columnMainEquipmentID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationChangeLogRow this[int index]
      {
        get => (TestbenchesAndEquipment.InstallationChangeLogRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddInstallationChangeLogRow(
        TestbenchesAndEquipment.InstallationChangeLogRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationChangeLogRow AddInstallationChangeLogRow(
        int InstallationId,
        DateTime ChangeTime,
        string SoftwareVersion,
        string LicenseName,
        string LicenseCustomer,
        int LicenseGeneratorID,
        DateTime LicenseGenerationTime,
        string BasicState,
        string State1,
        string State2,
        string ChangeInfo,
        int MainEquipmentID)
      {
        TestbenchesAndEquipment.InstallationChangeLogRow row = (TestbenchesAndEquipment.InstallationChangeLogRow) this.NewRow();
        object[] objArray = new object[12]
        {
          (object) InstallationId,
          (object) ChangeTime,
          (object) SoftwareVersion,
          (object) LicenseName,
          (object) LicenseCustomer,
          (object) LicenseGeneratorID,
          (object) LicenseGenerationTime,
          (object) BasicState,
          (object) State1,
          (object) State2,
          (object) ChangeInfo,
          (object) MainEquipmentID
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationChangeLogRow FindByInstallationIdChangeTime(
        int InstallationId,
        DateTime ChangeTime)
      {
        return (TestbenchesAndEquipment.InstallationChangeLogRow) this.Rows.Find(new object[2]
        {
          (object) InstallationId,
          (object) ChangeTime
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.InstallationChangeLogDataTable changeLogDataTable = (TestbenchesAndEquipment.InstallationChangeLogDataTable) base.Clone();
        changeLogDataTable.InitVars();
        return (DataTable) changeLogDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.InstallationChangeLogDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnInstallationId = this.Columns["InstallationId"];
        this.columnChangeTime = this.Columns["ChangeTime"];
        this.columnSoftwareVersion = this.Columns["SoftwareVersion"];
        this.columnLicenseName = this.Columns["LicenseName"];
        this.columnLicenseCustomer = this.Columns["LicenseCustomer"];
        this.columnLicenseGeneratorID = this.Columns["LicenseGeneratorID"];
        this.columnLicenseGenerationTime = this.Columns["LicenseGenerationTime"];
        this.columnBasicState = this.Columns["BasicState"];
        this.columnState1 = this.Columns["State1"];
        this.columnState2 = this.Columns["State2"];
        this.columnChangeInfo = this.Columns["ChangeInfo"];
        this.columnMainEquipmentID = this.Columns["MainEquipmentID"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnInstallationId = new DataColumn("InstallationId", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallationId);
        this.columnChangeTime = new DataColumn("ChangeTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeTime);
        this.columnSoftwareVersion = new DataColumn("SoftwareVersion", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnSoftwareVersion);
        this.columnLicenseName = new DataColumn("LicenseName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseName);
        this.columnLicenseCustomer = new DataColumn("LicenseCustomer", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseCustomer);
        this.columnLicenseGeneratorID = new DataColumn("LicenseGeneratorID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseGeneratorID);
        this.columnLicenseGenerationTime = new DataColumn("LicenseGenerationTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLicenseGenerationTime);
        this.columnBasicState = new DataColumn("BasicState", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnBasicState);
        this.columnState1 = new DataColumn("State1", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnState1);
        this.columnState2 = new DataColumn("State2", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnState2);
        this.columnChangeInfo = new DataColumn("ChangeInfo", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeInfo);
        this.columnMainEquipmentID = new DataColumn("MainEquipmentID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMainEquipmentID);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnInstallationId,
          this.columnChangeTime
        }, true));
        this.columnInstallationId.AllowDBNull = false;
        this.columnChangeTime.AllowDBNull = false;
        this.columnChangeTime.DateTimeMode = DataSetDateTime.Utc;
        this.columnSoftwareVersion.MaxLength = 50;
        this.columnLicenseName.MaxLength = (int) byte.MaxValue;
        this.columnLicenseCustomer.MaxLength = (int) byte.MaxValue;
        this.columnBasicState.MaxLength = 50;
        this.columnState1.MaxLength = 50;
        this.columnState2.MaxLength = 50;
        this.columnChangeInfo.MaxLength = 536870910;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationChangeLogRow NewInstallationChangeLogRow()
      {
        return (TestbenchesAndEquipment.InstallationChangeLogRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.InstallationChangeLogRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (TestbenchesAndEquipment.InstallationChangeLogRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.InstallationChangeLogRowChanged == null)
          return;
        this.InstallationChangeLogRowChanged((object) this, new TestbenchesAndEquipment.InstallationChangeLogRowChangeEvent((TestbenchesAndEquipment.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.InstallationChangeLogRowChanging == null)
          return;
        this.InstallationChangeLogRowChanging((object) this, new TestbenchesAndEquipment.InstallationChangeLogRowChangeEvent((TestbenchesAndEquipment.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.InstallationChangeLogRowDeleted == null)
          return;
        this.InstallationChangeLogRowDeleted((object) this, new TestbenchesAndEquipment.InstallationChangeLogRowChangeEvent((TestbenchesAndEquipment.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.InstallationChangeLogRowDeleting == null)
          return;
        this.InstallationChangeLogRowDeleting((object) this, new TestbenchesAndEquipment.InstallationChangeLogRowChangeEvent((TestbenchesAndEquipment.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveInstallationChangeLogRow(
        TestbenchesAndEquipment.InstallationChangeLogRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (InstallationChangeLogDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public class EquipmentPartlistCreationDataTable : 
      TypedTableBase<TestbenchesAndEquipment.EquipmentPartlistCreationRow>
    {
      private DataColumn columnPartlistID;
      private DataColumn columnCreationDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EquipmentPartlistCreationDataTable()
      {
        this.TableName = "EquipmentPartlistCreation";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EquipmentPartlistCreationDataTable(DataTable table)
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
      protected EquipmentPartlistCreationDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PartlistIDColumn => this.columnPartlistID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CreationDateColumn => this.columnCreationDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentPartlistCreationRow this[int index]
      {
        get => (TestbenchesAndEquipment.EquipmentPartlistCreationRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEventHandler EquipmentPartlistCreationRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEventHandler EquipmentPartlistCreationRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEventHandler EquipmentPartlistCreationRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEventHandler EquipmentPartlistCreationRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddEquipmentPartlistCreationRow(
        TestbenchesAndEquipment.EquipmentPartlistCreationRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentPartlistCreationRow AddEquipmentPartlistCreationRow(
        int PartlistID,
        DateTime CreationDate)
      {
        TestbenchesAndEquipment.EquipmentPartlistCreationRow row = (TestbenchesAndEquipment.EquipmentPartlistCreationRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) PartlistID,
          (object) CreationDate
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentPartlistCreationRow FindByPartlistID(int PartlistID)
      {
        return (TestbenchesAndEquipment.EquipmentPartlistCreationRow) this.Rows.Find(new object[1]
        {
          (object) PartlistID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.EquipmentPartlistCreationDataTable creationDataTable = (TestbenchesAndEquipment.EquipmentPartlistCreationDataTable) base.Clone();
        creationDataTable.InitVars();
        return (DataTable) creationDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.EquipmentPartlistCreationDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnPartlistID = this.Columns["PartlistID"];
        this.columnCreationDate = this.Columns["CreationDate"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnPartlistID = new DataColumn("PartlistID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPartlistID);
        this.columnCreationDate = new DataColumn("CreationDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCreationDate);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnPartlistID
        }, true));
        this.columnPartlistID.AllowDBNull = false;
        this.columnPartlistID.Unique = true;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentPartlistCreationRow NewEquipmentPartlistCreationRow()
      {
        return (TestbenchesAndEquipment.EquipmentPartlistCreationRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.EquipmentPartlistCreationRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (TestbenchesAndEquipment.EquipmentPartlistCreationRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.EquipmentPartlistCreationRowChanged == null)
          return;
        this.EquipmentPartlistCreationRowChanged((object) this, new TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEvent((TestbenchesAndEquipment.EquipmentPartlistCreationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.EquipmentPartlistCreationRowChanging == null)
          return;
        this.EquipmentPartlistCreationRowChanging((object) this, new TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEvent((TestbenchesAndEquipment.EquipmentPartlistCreationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.EquipmentPartlistCreationRowDeleted == null)
          return;
        this.EquipmentPartlistCreationRowDeleted((object) this, new TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEvent((TestbenchesAndEquipment.EquipmentPartlistCreationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.EquipmentPartlistCreationRowDeleting == null)
          return;
        this.EquipmentPartlistCreationRowDeleting((object) this, new TestbenchesAndEquipment.EquipmentPartlistCreationRowChangeEvent((TestbenchesAndEquipment.EquipmentPartlistCreationRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveEquipmentPartlistCreationRow(
        TestbenchesAndEquipment.EquipmentPartlistCreationRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (EquipmentPartlistCreationDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public class EquipmentTypeDescriptionDataTable : 
      TypedTableBase<TestbenchesAndEquipment.EquipmentTypeDescriptionRow>
    {
      private DataColumn columnEquipmentType;
      private DataColumn columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EquipmentTypeDescriptionDataTable()
      {
        this.TableName = "EquipmentTypeDescription";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EquipmentTypeDescriptionDataTable(DataTable table)
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
      protected EquipmentTypeDescriptionDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EquipmentTypeColumn => this.columnEquipmentType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentTypeDescriptionRow this[int index]
      {
        get => (TestbenchesAndEquipment.EquipmentTypeDescriptionRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEventHandler EquipmentTypeDescriptionRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEventHandler EquipmentTypeDescriptionRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEventHandler EquipmentTypeDescriptionRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEventHandler EquipmentTypeDescriptionRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddEquipmentTypeDescriptionRow(
        TestbenchesAndEquipment.EquipmentTypeDescriptionRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentTypeDescriptionRow AddEquipmentTypeDescriptionRow(
        int EquipmentType,
        string Description)
      {
        TestbenchesAndEquipment.EquipmentTypeDescriptionRow row = (TestbenchesAndEquipment.EquipmentTypeDescriptionRow) this.NewRow();
        object[] objArray = new object[2]
        {
          (object) EquipmentType,
          (object) Description
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentTypeDescriptionRow FindByEquipmentType(
        int EquipmentType)
      {
        return (TestbenchesAndEquipment.EquipmentTypeDescriptionRow) this.Rows.Find(new object[1]
        {
          (object) EquipmentType
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable descriptionDataTable = (TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable) base.Clone();
        descriptionDataTable.InitVars();
        return (DataTable) descriptionDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnEquipmentType = this.Columns["EquipmentType"];
        this.columnDescription = this.Columns["Description"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnEquipmentType = new DataColumn("EquipmentType", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEquipmentType);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnEquipmentType
        }, true));
        this.columnEquipmentType.AllowDBNull = false;
        this.columnEquipmentType.Unique = true;
        this.columnDescription.MaxLength = 50;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentTypeDescriptionRow NewEquipmentTypeDescriptionRow()
      {
        return (TestbenchesAndEquipment.EquipmentTypeDescriptionRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.EquipmentTypeDescriptionRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType()
      {
        return typeof (TestbenchesAndEquipment.EquipmentTypeDescriptionRow);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.EquipmentTypeDescriptionRowChanged == null)
          return;
        this.EquipmentTypeDescriptionRowChanged((object) this, new TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEvent((TestbenchesAndEquipment.EquipmentTypeDescriptionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.EquipmentTypeDescriptionRowChanging == null)
          return;
        this.EquipmentTypeDescriptionRowChanging((object) this, new TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEvent((TestbenchesAndEquipment.EquipmentTypeDescriptionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.EquipmentTypeDescriptionRowDeleted == null)
          return;
        this.EquipmentTypeDescriptionRowDeleted((object) this, new TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEvent((TestbenchesAndEquipment.EquipmentTypeDescriptionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.EquipmentTypeDescriptionRowDeleting == null)
          return;
        this.EquipmentTypeDescriptionRowDeleting((object) this, new TestbenchesAndEquipment.EquipmentTypeDescriptionRowChangeEvent((TestbenchesAndEquipment.EquipmentTypeDescriptionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveEquipmentTypeDescriptionRow(
        TestbenchesAndEquipment.EquipmentTypeDescriptionRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (EquipmentTypeDescriptionDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public class InstallationUsersDataTable : 
      TypedTableBase<TestbenchesAndEquipment.InstallationUsersRow>
    {
      private DataColumn columnInstallationId;
      private DataColumn columnChangeTime;
      private DataColumn columnUserId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationUsersDataTable()
      {
        this.TableName = "InstallationUsers";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationUsersDataTable(DataTable table)
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
      protected InstallationUsersDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn InstallationIdColumn => this.columnInstallationId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ChangeTimeColumn => this.columnChangeTime;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn UserIdColumn => this.columnUserId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationUsersRow this[int index]
      {
        get => (TestbenchesAndEquipment.InstallationUsersRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationUsersRowChangeEventHandler InstallationUsersRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationUsersRowChangeEventHandler InstallationUsersRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationUsersRowChangeEventHandler InstallationUsersRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.InstallationUsersRowChangeEventHandler InstallationUsersRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddInstallationUsersRow(TestbenchesAndEquipment.InstallationUsersRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationUsersRow AddInstallationUsersRow(
        int InstallationId,
        DateTime ChangeTime,
        int UserId)
      {
        TestbenchesAndEquipment.InstallationUsersRow row = (TestbenchesAndEquipment.InstallationUsersRow) this.NewRow();
        object[] objArray = new object[3]
        {
          (object) InstallationId,
          (object) ChangeTime,
          (object) UserId
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationUsersRow FindByInstallationIdChangeTime(
        int InstallationId,
        DateTime ChangeTime)
      {
        return (TestbenchesAndEquipment.InstallationUsersRow) this.Rows.Find(new object[2]
        {
          (object) InstallationId,
          (object) ChangeTime
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.InstallationUsersDataTable installationUsersDataTable = (TestbenchesAndEquipment.InstallationUsersDataTable) base.Clone();
        installationUsersDataTable.InitVars();
        return (DataTable) installationUsersDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.InstallationUsersDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnInstallationId = this.Columns["InstallationId"];
        this.columnChangeTime = this.Columns["ChangeTime"];
        this.columnUserId = this.Columns["UserId"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnInstallationId = new DataColumn("InstallationId", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInstallationId);
        this.columnChangeTime = new DataColumn("ChangeTime", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnChangeTime);
        this.columnUserId = new DataColumn("UserId", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUserId);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnInstallationId,
          this.columnChangeTime
        }, true));
        this.columnInstallationId.AllowDBNull = false;
        this.columnChangeTime.AllowDBNull = false;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationUsersRow NewInstallationUsersRow()
      {
        return (TestbenchesAndEquipment.InstallationUsersRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.InstallationUsersRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (TestbenchesAndEquipment.InstallationUsersRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.InstallationUsersRowChanged == null)
          return;
        this.InstallationUsersRowChanged((object) this, new TestbenchesAndEquipment.InstallationUsersRowChangeEvent((TestbenchesAndEquipment.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.InstallationUsersRowChanging == null)
          return;
        this.InstallationUsersRowChanging((object) this, new TestbenchesAndEquipment.InstallationUsersRowChangeEvent((TestbenchesAndEquipment.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.InstallationUsersRowDeleted == null)
          return;
        this.InstallationUsersRowDeleted((object) this, new TestbenchesAndEquipment.InstallationUsersRowChangeEvent((TestbenchesAndEquipment.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.InstallationUsersRowDeleting == null)
          return;
        this.InstallationUsersRowDeleting((object) this, new TestbenchesAndEquipment.InstallationUsersRowChangeEvent((TestbenchesAndEquipment.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveInstallationUsersRow(TestbenchesAndEquipment.InstallationUsersRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (InstallationUsersDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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
    public class SoftwareUsersDataTable : TypedTableBase<TestbenchesAndEquipment.SoftwareUsersRow>
    {
      private DataColumn columnUserId;
      private DataColumn columnName;
      private DataColumn columnPersonalNumber;
      private DataColumn columnPassword;
      private DataColumn columnLanguageSetting;
      private DataColumn columnControlKey;
      private DataColumn columnUserRole;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SoftwareUsersDataTable()
      {
        this.TableName = "SoftwareUsers";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SoftwareUsersDataTable(DataTable table)
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
      protected SoftwareUsersDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn UserIdColumn => this.columnUserId;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn NameColumn => this.columnName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PersonalNumberColumn => this.columnPersonalNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn PasswordColumn => this.columnPassword;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LanguageSettingColumn => this.columnLanguageSetting;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ControlKeyColumn => this.columnControlKey;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn UserRoleColumn => this.columnUserRole;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.SoftwareUsersRow this[int index]
      {
        get => (TestbenchesAndEquipment.SoftwareUsersRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.SoftwareUsersRowChangeEventHandler SoftwareUsersRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.SoftwareUsersRowChangeEventHandler SoftwareUsersRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.SoftwareUsersRowChangeEventHandler SoftwareUsersRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event TestbenchesAndEquipment.SoftwareUsersRowChangeEventHandler SoftwareUsersRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddSoftwareUsersRow(TestbenchesAndEquipment.SoftwareUsersRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.SoftwareUsersRow AddSoftwareUsersRow(
        int UserId,
        string Name,
        int PersonalNumber,
        string Password,
        string LanguageSetting,
        string ControlKey,
        int UserRole)
      {
        TestbenchesAndEquipment.SoftwareUsersRow row = (TestbenchesAndEquipment.SoftwareUsersRow) this.NewRow();
        object[] objArray = new object[7]
        {
          (object) UserId,
          (object) Name,
          (object) PersonalNumber,
          (object) Password,
          (object) LanguageSetting,
          (object) ControlKey,
          (object) UserRole
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.SoftwareUsersRow FindByUserId(int UserId)
      {
        return (TestbenchesAndEquipment.SoftwareUsersRow) this.Rows.Find(new object[1]
        {
          (object) UserId
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        TestbenchesAndEquipment.SoftwareUsersDataTable softwareUsersDataTable = (TestbenchesAndEquipment.SoftwareUsersDataTable) base.Clone();
        softwareUsersDataTable.InitVars();
        return (DataTable) softwareUsersDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new TestbenchesAndEquipment.SoftwareUsersDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnUserId = this.Columns["UserId"];
        this.columnName = this.Columns["Name"];
        this.columnPersonalNumber = this.Columns["PersonalNumber"];
        this.columnPassword = this.Columns["Password"];
        this.columnLanguageSetting = this.Columns["LanguageSetting"];
        this.columnControlKey = this.Columns["ControlKey"];
        this.columnUserRole = this.Columns["UserRole"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnUserId = new DataColumn("UserId", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUserId);
        this.columnName = new DataColumn("Name", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnName);
        this.columnPersonalNumber = new DataColumn("PersonalNumber", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPersonalNumber);
        this.columnPassword = new DataColumn("Password", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnPassword);
        this.columnLanguageSetting = new DataColumn("LanguageSetting", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLanguageSetting);
        this.columnControlKey = new DataColumn("ControlKey", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnControlKey);
        this.columnUserRole = new DataColumn("UserRole", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnUserRole);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnUserId
        }, true));
        this.columnUserId.AllowDBNull = false;
        this.columnUserId.Unique = true;
        this.columnName.MaxLength = 50;
        this.columnPassword.MaxLength = 100;
        this.columnLanguageSetting.MaxLength = 20;
        this.columnControlKey.MaxLength = 100;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.SoftwareUsersRow NewSoftwareUsersRow()
      {
        return (TestbenchesAndEquipment.SoftwareUsersRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new TestbenchesAndEquipment.SoftwareUsersRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (TestbenchesAndEquipment.SoftwareUsersRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.SoftwareUsersRowChanged == null)
          return;
        this.SoftwareUsersRowChanged((object) this, new TestbenchesAndEquipment.SoftwareUsersRowChangeEvent((TestbenchesAndEquipment.SoftwareUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.SoftwareUsersRowChanging == null)
          return;
        this.SoftwareUsersRowChanging((object) this, new TestbenchesAndEquipment.SoftwareUsersRowChangeEvent((TestbenchesAndEquipment.SoftwareUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.SoftwareUsersRowDeleted == null)
          return;
        this.SoftwareUsersRowDeleted((object) this, new TestbenchesAndEquipment.SoftwareUsersRowChangeEvent((TestbenchesAndEquipment.SoftwareUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.SoftwareUsersRowDeleting == null)
          return;
        this.SoftwareUsersRowDeleting((object) this, new TestbenchesAndEquipment.SoftwareUsersRowChangeEvent((TestbenchesAndEquipment.SoftwareUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveSoftwareUsersRow(TestbenchesAndEquipment.SoftwareUsersRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        TestbenchesAndEquipment testbenchesAndEquipment = new TestbenchesAndEquipment();
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
          FixedValue = testbenchesAndEquipment.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (SoftwareUsersDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = testbenchesAndEquipment.GetSchemaSerializable();
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

    public class InstallationsRow : DataRow
    {
      private TestbenchesAndEquipment.InstallationsDataTable tableInstallations;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableInstallations = (TestbenchesAndEquipment.InstallationsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int InstallationId
      {
        get => (int) this[this.tableInstallations.InstallationIdColumn];
        set => this[this.tableInstallations.InstallationIdColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string PcName
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallations.PcNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PcName' in table 'Installations' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallations.PcNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string InstallationPath
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallations.InstallationPathColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InstallationPath' in table 'Installations' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallations.InstallationPathColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string InstallataionName
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallations.InstallataionNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InstallataionName' in table 'Installations' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallations.InstallataionNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPcNameNull() => this.IsNull(this.tableInstallations.PcNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPcNameNull() => this[this.tableInstallations.PcNameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsInstallationPathNull()
      {
        return this.IsNull(this.tableInstallations.InstallationPathColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetInstallationPathNull()
      {
        this[this.tableInstallations.InstallationPathColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsInstallataionNameNull()
      {
        return this.IsNull(this.tableInstallations.InstallataionNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetInstallataionNameNull()
      {
        this[this.tableInstallations.InstallataionNameColumn] = Convert.DBNull;
      }
    }

    public class InstallationChangeLogRow : DataRow
    {
      private TestbenchesAndEquipment.InstallationChangeLogDataTable tableInstallationChangeLog;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationChangeLogRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableInstallationChangeLog = (TestbenchesAndEquipment.InstallationChangeLogDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int InstallationId
      {
        get => (int) this[this.tableInstallationChangeLog.InstallationIdColumn];
        set => this[this.tableInstallationChangeLog.InstallationIdColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ChangeTime
      {
        get => (DateTime) this[this.tableInstallationChangeLog.ChangeTimeColumn];
        set => this[this.tableInstallationChangeLog.ChangeTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SoftwareVersion
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.SoftwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SoftwareVersion' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.SoftwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LicenseName
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.LicenseNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseName' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.LicenseNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LicenseCustomer
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.LicenseCustomerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseCustomer' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.LicenseCustomerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LicenseGeneratorID
      {
        get
        {
          try
          {
            return (int) this[this.tableInstallationChangeLog.LicenseGeneratorIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseGeneratorID' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.LicenseGeneratorIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime LicenseGenerationTime
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableInstallationChangeLog.LicenseGenerationTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseGenerationTime' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.LicenseGenerationTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string BasicState
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.BasicStateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'BasicState' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.BasicStateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string State1
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.State1Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'State1' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.State1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string State2
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.State2Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'State2' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.State2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChangeInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableInstallationChangeLog.ChangeInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChangeInfo' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.ChangeInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int MainEquipmentID
      {
        get
        {
          try
          {
            return (int) this[this.tableInstallationChangeLog.MainEquipmentIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MainEquipmentID' in table 'InstallationChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationChangeLog.MainEquipmentIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSoftwareVersionNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.SoftwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSoftwareVersionNull()
      {
        this[this.tableInstallationChangeLog.SoftwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseNameNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.LicenseNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseNameNull()
      {
        this[this.tableInstallationChangeLog.LicenseNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseCustomerNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.LicenseCustomerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseCustomerNull()
      {
        this[this.tableInstallationChangeLog.LicenseCustomerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseGeneratorIDNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.LicenseGeneratorIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseGeneratorIDNull()
      {
        this[this.tableInstallationChangeLog.LicenseGeneratorIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseGenerationTimeNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.LicenseGenerationTimeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseGenerationTimeNull()
      {
        this[this.tableInstallationChangeLog.LicenseGenerationTimeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBasicStateNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.BasicStateColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBasicStateNull()
      {
        this[this.tableInstallationChangeLog.BasicStateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsState1Null() => this.IsNull(this.tableInstallationChangeLog.State1Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetState1Null()
      {
        this[this.tableInstallationChangeLog.State1Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsState2Null() => this.IsNull(this.tableInstallationChangeLog.State2Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetState2Null()
      {
        this[this.tableInstallationChangeLog.State2Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChangeInfoNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.ChangeInfoColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChangeInfoNull()
      {
        this[this.tableInstallationChangeLog.ChangeInfoColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsMainEquipmentIDNull()
      {
        return this.IsNull(this.tableInstallationChangeLog.MainEquipmentIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetMainEquipmentIDNull()
      {
        this[this.tableInstallationChangeLog.MainEquipmentIDColumn] = Convert.DBNull;
      }
    }

    public class EquipmentPartlistCreationRow : DataRow
    {
      private TestbenchesAndEquipment.EquipmentPartlistCreationDataTable tableEquipmentPartlistCreation;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EquipmentPartlistCreationRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableEquipmentPartlistCreation = (TestbenchesAndEquipment.EquipmentPartlistCreationDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int PartlistID
      {
        get => (int) this[this.tableEquipmentPartlistCreation.PartlistIDColumn];
        set => this[this.tableEquipmentPartlistCreation.PartlistIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime CreationDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableEquipmentPartlistCreation.CreationDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CreationDate' in table 'EquipmentPartlistCreation' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentPartlistCreation.CreationDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCreationDateNull()
      {
        return this.IsNull(this.tableEquipmentPartlistCreation.CreationDateColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCreationDateNull()
      {
        this[this.tableEquipmentPartlistCreation.CreationDateColumn] = Convert.DBNull;
      }
    }

    public class EquipmentTypeDescriptionRow : DataRow
    {
      private TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable tableEquipmentTypeDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EquipmentTypeDescriptionRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableEquipmentTypeDescription = (TestbenchesAndEquipment.EquipmentTypeDescriptionDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int EquipmentType
      {
        get => (int) this[this.tableEquipmentTypeDescription.EquipmentTypeColumn];
        set => this[this.tableEquipmentTypeDescription.EquipmentTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentTypeDescription.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'EquipmentTypeDescription' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentTypeDescription.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDescriptionNull()
      {
        return this.IsNull(this.tableEquipmentTypeDescription.DescriptionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableEquipmentTypeDescription.DescriptionColumn] = Convert.DBNull;
      }
    }

    public class InstallationUsersRow : DataRow
    {
      private TestbenchesAndEquipment.InstallationUsersDataTable tableInstallationUsers;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationUsersRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableInstallationUsers = (TestbenchesAndEquipment.InstallationUsersDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int InstallationId
      {
        get => (int) this[this.tableInstallationUsers.InstallationIdColumn];
        set => this[this.tableInstallationUsers.InstallationIdColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ChangeTime
      {
        get => (DateTime) this[this.tableInstallationUsers.ChangeTimeColumn];
        set => this[this.tableInstallationUsers.ChangeTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int UserId
      {
        get
        {
          try
          {
            return (int) this[this.tableInstallationUsers.UserIdColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'UserId' in table 'InstallationUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableInstallationUsers.UserIdColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsUserIdNull() => this.IsNull(this.tableInstallationUsers.UserIdColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetUserIdNull()
      {
        this[this.tableInstallationUsers.UserIdColumn] = Convert.DBNull;
      }
    }

    public class SoftwareUsersRow : DataRow
    {
      private TestbenchesAndEquipment.SoftwareUsersDataTable tableSoftwareUsers;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal SoftwareUsersRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableSoftwareUsers = (TestbenchesAndEquipment.SoftwareUsersDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int UserId
      {
        get => (int) this[this.tableSoftwareUsers.UserIdColumn];
        set => this[this.tableSoftwareUsers.UserIdColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Name
      {
        get
        {
          try
          {
            return (string) this[this.tableSoftwareUsers.NameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Name' in table 'SoftwareUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSoftwareUsers.NameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int PersonalNumber
      {
        get
        {
          try
          {
            return (int) this[this.tableSoftwareUsers.PersonalNumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PersonalNumber' in table 'SoftwareUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSoftwareUsers.PersonalNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Password
      {
        get
        {
          try
          {
            return (string) this[this.tableSoftwareUsers.PasswordColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Password' in table 'SoftwareUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSoftwareUsers.PasswordColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LanguageSetting
      {
        get
        {
          try
          {
            return (string) this[this.tableSoftwareUsers.LanguageSettingColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LanguageSetting' in table 'SoftwareUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSoftwareUsers.LanguageSettingColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ControlKey
      {
        get
        {
          try
          {
            return (string) this[this.tableSoftwareUsers.ControlKeyColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ControlKey' in table 'SoftwareUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSoftwareUsers.ControlKeyColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int UserRole
      {
        get
        {
          try
          {
            return (int) this[this.tableSoftwareUsers.UserRoleColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'UserRole' in table 'SoftwareUsers' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSoftwareUsers.UserRoleColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsNameNull() => this.IsNull(this.tableSoftwareUsers.NameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNameNull() => this[this.tableSoftwareUsers.NameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPersonalNumberNull()
      {
        return this.IsNull(this.tableSoftwareUsers.PersonalNumberColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPersonalNumberNull()
      {
        this[this.tableSoftwareUsers.PersonalNumberColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPasswordNull() => this.IsNull(this.tableSoftwareUsers.PasswordColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPasswordNull()
      {
        this[this.tableSoftwareUsers.PasswordColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLanguageSettingNull()
      {
        return this.IsNull(this.tableSoftwareUsers.LanguageSettingColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLanguageSettingNull()
      {
        this[this.tableSoftwareUsers.LanguageSettingColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsControlKeyNull() => this.IsNull(this.tableSoftwareUsers.ControlKeyColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetControlKeyNull()
      {
        this[this.tableSoftwareUsers.ControlKeyColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsUserRoleNull() => this.IsNull(this.tableSoftwareUsers.UserRoleColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetUserRoleNull()
      {
        this[this.tableSoftwareUsers.UserRoleColumn] = Convert.DBNull;
      }
    }

    public class EquipmentChangeLogRow : DataRow
    {
      private TestbenchesAndEquipment.EquipmentChangeLogDataTable tableEquipmentChangeLog;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EquipmentChangeLogRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableEquipmentChangeLog = (TestbenchesAndEquipment.EquipmentChangeLogDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int InstallationId
      {
        get => (int) this[this.tableEquipmentChangeLog.InstallationIdColumn];
        set => this[this.tableEquipmentChangeLog.InstallationIdColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ChangeTime
      {
        get => (DateTime) this[this.tableEquipmentChangeLog.ChangeTimeColumn];
        set => this[this.tableEquipmentChangeLog.ChangeTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string PcName
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.PcNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'PcName' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.PcNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string InstallationPath
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.InstallationPathColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InstallationPath' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.InstallationPathColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string SoftwareVersion
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.SoftwareVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'SoftwareVersion' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.SoftwareVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LicenseName
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.LicenseNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseName' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.LicenseNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string LicenseCustomer
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.LicenseCustomerColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseCustomer' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.LicenseCustomerColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LicenseGeneratorID
      {
        get
        {
          try
          {
            return (int) this[this.tableEquipmentChangeLog.LicenseGeneratorIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseGeneratorID' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.LicenseGeneratorIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime LicenseGenerationTime
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableEquipmentChangeLog.LicenseGenerationTimeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LicenseGenerationTime' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.LicenseGenerationTimeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string BasicState
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.BasicStateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'BasicState' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.BasicStateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string State1
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.State1Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'State1' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.State1Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string State2
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.State2Column];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'State2' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.State2Column] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ChangeInfo
      {
        get
        {
          try
          {
            return (string) this[this.tableEquipmentChangeLog.ChangeInfoColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ChangeInfo' in table 'EquipmentChangeLog' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEquipmentChangeLog.ChangeInfoColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsPcNameNull() => this.IsNull(this.tableEquipmentChangeLog.PcNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetPcNameNull()
      {
        this[this.tableEquipmentChangeLog.PcNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsInstallationPathNull()
      {
        return this.IsNull(this.tableEquipmentChangeLog.InstallationPathColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetInstallationPathNull()
      {
        this[this.tableEquipmentChangeLog.InstallationPathColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsSoftwareVersionNull()
      {
        return this.IsNull(this.tableEquipmentChangeLog.SoftwareVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetSoftwareVersionNull()
      {
        this[this.tableEquipmentChangeLog.SoftwareVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseNameNull()
      {
        return this.IsNull(this.tableEquipmentChangeLog.LicenseNameColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseNameNull()
      {
        this[this.tableEquipmentChangeLog.LicenseNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseCustomerNull()
      {
        return this.IsNull(this.tableEquipmentChangeLog.LicenseCustomerColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseCustomerNull()
      {
        this[this.tableEquipmentChangeLog.LicenseCustomerColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseGeneratorIDNull()
      {
        return this.IsNull(this.tableEquipmentChangeLog.LicenseGeneratorIDColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseGeneratorIDNull()
      {
        this[this.tableEquipmentChangeLog.LicenseGeneratorIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLicenseGenerationTimeNull()
      {
        return this.IsNull(this.tableEquipmentChangeLog.LicenseGenerationTimeColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLicenseGenerationTimeNull()
      {
        this[this.tableEquipmentChangeLog.LicenseGenerationTimeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsBasicStateNull() => this.IsNull(this.tableEquipmentChangeLog.BasicStateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetBasicStateNull()
      {
        this[this.tableEquipmentChangeLog.BasicStateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsState1Null() => this.IsNull(this.tableEquipmentChangeLog.State1Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetState1Null()
      {
        this[this.tableEquipmentChangeLog.State1Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsState2Null() => this.IsNull(this.tableEquipmentChangeLog.State2Column);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetState2Null()
      {
        this[this.tableEquipmentChangeLog.State2Column] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsChangeInfoNull() => this.IsNull(this.tableEquipmentChangeLog.ChangeInfoColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetChangeInfoNull()
      {
        this[this.tableEquipmentChangeLog.ChangeInfoColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class InstallationsRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.InstallationsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationsRowChangeEvent(
        TestbenchesAndEquipment.InstallationsRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class InstallationChangeLogRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.InstallationChangeLogRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationChangeLogRowChangeEvent(
        TestbenchesAndEquipment.InstallationChangeLogRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationChangeLogRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class EquipmentPartlistCreationRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.EquipmentPartlistCreationRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EquipmentPartlistCreationRowChangeEvent(
        TestbenchesAndEquipment.EquipmentPartlistCreationRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentPartlistCreationRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class EquipmentTypeDescriptionRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.EquipmentTypeDescriptionRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EquipmentTypeDescriptionRowChangeEvent(
        TestbenchesAndEquipment.EquipmentTypeDescriptionRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentTypeDescriptionRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class InstallationUsersRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.InstallationUsersRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationUsersRowChangeEvent(
        TestbenchesAndEquipment.InstallationUsersRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.InstallationUsersRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class SoftwareUsersRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.SoftwareUsersRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public SoftwareUsersRowChangeEvent(
        TestbenchesAndEquipment.SoftwareUsersRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.SoftwareUsersRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class EquipmentChangeLogRowChangeEvent : EventArgs
    {
      private TestbenchesAndEquipment.EquipmentChangeLogRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EquipmentChangeLogRowChangeEvent(
        TestbenchesAndEquipment.EquipmentChangeLogRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public TestbenchesAndEquipment.EquipmentChangeLogRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
