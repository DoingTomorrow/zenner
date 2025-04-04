// Decompiled with JetBrains decompiler
// Type: StartupLib.InstallationData
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

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
namespace StartupLib
{
  [DesignerCategory("code")]
  [ToolboxItem(true)]
  [XmlSchemaProvider("GetTypedDataSetSchema")]
  [XmlRoot("InstallationData")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class InstallationData : DataSet
  {
    private InstallationData.EQ_EquipmentDataTable tableEQ_Equipment;
    private InstallationData.InstallationsDataTable tableInstallations;
    private InstallationData.InstallationChangeLogDataTable tableInstallationChangeLog;
    private InstallationData.InstallationUsersDataTable tableInstallationUsers;
    private InstallationData.EQ_EquipmentValueDataTable tableEQ_EquipmentValue;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public InstallationData()
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
    protected InstallationData(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (EQ_Equipment)] != null)
            base.Tables.Add((DataTable) new InstallationData.EQ_EquipmentDataTable(dataSet.Tables[nameof (EQ_Equipment)]));
          if (dataSet.Tables[nameof (Installations)] != null)
            base.Tables.Add((DataTable) new InstallationData.InstallationsDataTable(dataSet.Tables[nameof (Installations)]));
          if (dataSet.Tables[nameof (InstallationChangeLog)] != null)
            base.Tables.Add((DataTable) new InstallationData.InstallationChangeLogDataTable(dataSet.Tables[nameof (InstallationChangeLog)]));
          if (dataSet.Tables[nameof (InstallationUsers)] != null)
            base.Tables.Add((DataTable) new InstallationData.InstallationUsersDataTable(dataSet.Tables[nameof (InstallationUsers)]));
          if (dataSet.Tables[nameof (EQ_EquipmentValue)] != null)
            base.Tables.Add((DataTable) new InstallationData.EQ_EquipmentValueDataTable(dataSet.Tables[nameof (EQ_EquipmentValue)]));
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
    public InstallationData.EQ_EquipmentDataTable EQ_Equipment => this.tableEQ_Equipment;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public InstallationData.InstallationsDataTable Installations => this.tableInstallations;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public InstallationData.InstallationChangeLogDataTable InstallationChangeLog
    {
      get => this.tableInstallationChangeLog;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public InstallationData.InstallationUsersDataTable InstallationUsers
    {
      get => this.tableInstallationUsers;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public InstallationData.EQ_EquipmentValueDataTable EQ_EquipmentValue
    {
      get => this.tableEQ_EquipmentValue;
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
      InstallationData installationData = (InstallationData) base.Clone();
      installationData.InitVars();
      installationData.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) installationData;
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
        if (dataSet.Tables["EQ_Equipment"] != null)
          base.Tables.Add((DataTable) new InstallationData.EQ_EquipmentDataTable(dataSet.Tables["EQ_Equipment"]));
        if (dataSet.Tables["Installations"] != null)
          base.Tables.Add((DataTable) new InstallationData.InstallationsDataTable(dataSet.Tables["Installations"]));
        if (dataSet.Tables["InstallationChangeLog"] != null)
          base.Tables.Add((DataTable) new InstallationData.InstallationChangeLogDataTable(dataSet.Tables["InstallationChangeLog"]));
        if (dataSet.Tables["InstallationUsers"] != null)
          base.Tables.Add((DataTable) new InstallationData.InstallationUsersDataTable(dataSet.Tables["InstallationUsers"]));
        if (dataSet.Tables["EQ_EquipmentValue"] != null)
          base.Tables.Add((DataTable) new InstallationData.EQ_EquipmentValueDataTable(dataSet.Tables["EQ_EquipmentValue"]));
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
      this.tableEQ_Equipment = (InstallationData.EQ_EquipmentDataTable) base.Tables["EQ_Equipment"];
      if (initTable && this.tableEQ_Equipment != null)
        this.tableEQ_Equipment.InitVars();
      this.tableInstallations = (InstallationData.InstallationsDataTable) base.Tables["Installations"];
      if (initTable && this.tableInstallations != null)
        this.tableInstallations.InitVars();
      this.tableInstallationChangeLog = (InstallationData.InstallationChangeLogDataTable) base.Tables["InstallationChangeLog"];
      if (initTable && this.tableInstallationChangeLog != null)
        this.tableInstallationChangeLog.InitVars();
      this.tableInstallationUsers = (InstallationData.InstallationUsersDataTable) base.Tables["InstallationUsers"];
      if (initTable && this.tableInstallationUsers != null)
        this.tableInstallationUsers.InitVars();
      this.tableEQ_EquipmentValue = (InstallationData.EQ_EquipmentValueDataTable) base.Tables["EQ_EquipmentValue"];
      if (!initTable || this.tableEQ_EquipmentValue == null)
        return;
      this.tableEQ_EquipmentValue.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (InstallationData);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/InstallationData.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableEQ_Equipment = new InstallationData.EQ_EquipmentDataTable();
      base.Tables.Add((DataTable) this.tableEQ_Equipment);
      this.tableInstallations = new InstallationData.InstallationsDataTable();
      base.Tables.Add((DataTable) this.tableInstallations);
      this.tableInstallationChangeLog = new InstallationData.InstallationChangeLogDataTable();
      base.Tables.Add((DataTable) this.tableInstallationChangeLog);
      this.tableInstallationUsers = new InstallationData.InstallationUsersDataTable();
      base.Tables.Add((DataTable) this.tableInstallationUsers);
      this.tableEQ_EquipmentValue = new InstallationData.EQ_EquipmentValueDataTable();
      base.Tables.Add((DataTable) this.tableEQ_EquipmentValue);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeEQ_Equipment() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeInstallations() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeInstallationChangeLog() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeInstallationUsers() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    private bool ShouldSerializeEQ_EquipmentValue() => false;

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
      InstallationData installationData = new InstallationData();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = installationData.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = installationData.GetSchemaSerializable();
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
    public delegate void EQ_EquipmentRowChangeEventHandler(
      object sender,
      InstallationData.EQ_EquipmentRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void InstallationsRowChangeEventHandler(
      object sender,
      InstallationData.InstallationsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void InstallationChangeLogRowChangeEventHandler(
      object sender,
      InstallationData.InstallationChangeLogRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void InstallationUsersRowChangeEventHandler(
      object sender,
      InstallationData.InstallationUsersRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public delegate void EQ_EquipmentValueRowChangeEventHandler(
      object sender,
      InstallationData.EQ_EquipmentValueRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class EQ_EquipmentDataTable : TypedTableBase<InstallationData.EQ_EquipmentRow>
    {
      private DataColumn columnEquipmentID;
      private DataColumn columnEquipmentGroupID;
      private DataColumn columnEquipmentType;
      private DataColumn columnAccessName;
      private DataColumn columnName;
      private DataColumn columnDescription;
      private DataColumn columnCreationDate;
      private DataColumn columnOutOfOperationDate;
      private DataColumn columnLocationID;
      private DataColumn columnOrderNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EQ_EquipmentDataTable()
      {
        this.TableName = "EQ_Equipment";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EQ_EquipmentDataTable(DataTable table)
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
      protected EQ_EquipmentDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EquipmentIDColumn => this.columnEquipmentID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EquipmentGroupIDColumn => this.columnEquipmentGroupID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EquipmentTypeColumn => this.columnEquipmentType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn AccessNameColumn => this.columnAccessName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn NameColumn => this.columnName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn DescriptionColumn => this.columnDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn CreationDateColumn => this.columnCreationDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn OutOfOperationDateColumn => this.columnOutOfOperationDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn LocationIDColumn => this.columnLocationID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn OrderNumberColumn => this.columnOrderNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentRow this[int index]
      {
        get => (InstallationData.EQ_EquipmentRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentRowChangeEventHandler EQ_EquipmentRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentRowChangeEventHandler EQ_EquipmentRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentRowChangeEventHandler EQ_EquipmentRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentRowChangeEventHandler EQ_EquipmentRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddEQ_EquipmentRow(InstallationData.EQ_EquipmentRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentRow AddEQ_EquipmentRow(
        int EquipmentID,
        int EquipmentGroupID,
        string EquipmentType,
        string AccessName,
        string Name,
        string Description,
        DateTime CreationDate,
        DateTime OutOfOperationDate,
        int LocationID,
        int OrderNumber)
      {
        InstallationData.EQ_EquipmentRow row = (InstallationData.EQ_EquipmentRow) this.NewRow();
        object[] objArray = new object[10]
        {
          (object) EquipmentID,
          (object) EquipmentGroupID,
          (object) EquipmentType,
          (object) AccessName,
          (object) Name,
          (object) Description,
          (object) CreationDate,
          (object) OutOfOperationDate,
          (object) LocationID,
          (object) OrderNumber
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentRow FindByEquipmentIDEquipmentGroupID(
        int EquipmentID,
        int EquipmentGroupID)
      {
        return (InstallationData.EQ_EquipmentRow) this.Rows.Find(new object[2]
        {
          (object) EquipmentID,
          (object) EquipmentGroupID
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        InstallationData.EQ_EquipmentDataTable equipmentDataTable = (InstallationData.EQ_EquipmentDataTable) base.Clone();
        equipmentDataTable.InitVars();
        return (DataTable) equipmentDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new InstallationData.EQ_EquipmentDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnEquipmentID = this.Columns["EquipmentID"];
        this.columnEquipmentGroupID = this.Columns["EquipmentGroupID"];
        this.columnEquipmentType = this.Columns["EquipmentType"];
        this.columnAccessName = this.Columns["AccessName"];
        this.columnName = this.Columns["Name"];
        this.columnDescription = this.Columns["Description"];
        this.columnCreationDate = this.Columns["CreationDate"];
        this.columnOutOfOperationDate = this.Columns["OutOfOperationDate"];
        this.columnLocationID = this.Columns["LocationID"];
        this.columnOrderNumber = this.Columns["OrderNumber"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnEquipmentID = new DataColumn("EquipmentID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEquipmentID);
        this.columnEquipmentGroupID = new DataColumn("EquipmentGroupID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEquipmentGroupID);
        this.columnEquipmentType = new DataColumn("EquipmentType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEquipmentType);
        this.columnAccessName = new DataColumn("AccessName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnAccessName);
        this.columnName = new DataColumn("Name", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnName);
        this.columnDescription = new DataColumn("Description", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnDescription);
        this.columnCreationDate = new DataColumn("CreationDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnCreationDate);
        this.columnOutOfOperationDate = new DataColumn("OutOfOperationDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOutOfOperationDate);
        this.columnLocationID = new DataColumn("LocationID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLocationID);
        this.columnOrderNumber = new DataColumn("OrderNumber", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOrderNumber);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnEquipmentID,
          this.columnEquipmentGroupID
        }, true));
        this.columnEquipmentID.AllowDBNull = false;
        this.columnEquipmentGroupID.AllowDBNull = false;
        this.columnEquipmentType.MaxLength = 50;
        this.columnAccessName.MaxLength = 50;
        this.columnName.MaxLength = 50;
        this.columnDescription.MaxLength = 4000;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentRow NewEQ_EquipmentRow()
      {
        return (InstallationData.EQ_EquipmentRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new InstallationData.EQ_EquipmentRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (InstallationData.EQ_EquipmentRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.EQ_EquipmentRowChanged == null)
          return;
        this.EQ_EquipmentRowChanged((object) this, new InstallationData.EQ_EquipmentRowChangeEvent((InstallationData.EQ_EquipmentRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.EQ_EquipmentRowChanging == null)
          return;
        this.EQ_EquipmentRowChanging((object) this, new InstallationData.EQ_EquipmentRowChangeEvent((InstallationData.EQ_EquipmentRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.EQ_EquipmentRowDeleted == null)
          return;
        this.EQ_EquipmentRowDeleted((object) this, new InstallationData.EQ_EquipmentRowChangeEvent((InstallationData.EQ_EquipmentRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.EQ_EquipmentRowDeleting == null)
          return;
        this.EQ_EquipmentRowDeleting((object) this, new InstallationData.EQ_EquipmentRowChangeEvent((InstallationData.EQ_EquipmentRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveEQ_EquipmentRow(InstallationData.EQ_EquipmentRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        InstallationData installationData = new InstallationData();
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
          FixedValue = installationData.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (EQ_EquipmentDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = installationData.GetSchemaSerializable();
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
    public class InstallationsDataTable : TypedTableBase<InstallationData.InstallationsRow>
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
      public InstallationData.InstallationsRow this[int index]
      {
        get => (InstallationData.InstallationsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationsRowChangeEventHandler InstallationsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationsRowChangeEventHandler InstallationsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationsRowChangeEventHandler InstallationsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationsRowChangeEventHandler InstallationsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddInstallationsRow(InstallationData.InstallationsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.InstallationsRow AddInstallationsRow(
        int InstallationId,
        string PcName,
        string InstallationPath,
        string InstallataionName)
      {
        InstallationData.InstallationsRow row = (InstallationData.InstallationsRow) this.NewRow();
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
      public InstallationData.InstallationsRow FindByInstallationId(int InstallationId)
      {
        return (InstallationData.InstallationsRow) this.Rows.Find(new object[1]
        {
          (object) InstallationId
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        InstallationData.InstallationsDataTable installationsDataTable = (InstallationData.InstallationsDataTable) base.Clone();
        installationsDataTable.InitVars();
        return (DataTable) installationsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new InstallationData.InstallationsDataTable();
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
      public InstallationData.InstallationsRow NewInstallationsRow()
      {
        return (InstallationData.InstallationsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new InstallationData.InstallationsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (InstallationData.InstallationsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.InstallationsRowChanged == null)
          return;
        this.InstallationsRowChanged((object) this, new InstallationData.InstallationsRowChangeEvent((InstallationData.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.InstallationsRowChanging == null)
          return;
        this.InstallationsRowChanging((object) this, new InstallationData.InstallationsRowChangeEvent((InstallationData.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.InstallationsRowDeleted == null)
          return;
        this.InstallationsRowDeleted((object) this, new InstallationData.InstallationsRowChangeEvent((InstallationData.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.InstallationsRowDeleting == null)
          return;
        this.InstallationsRowDeleting((object) this, new InstallationData.InstallationsRowChangeEvent((InstallationData.InstallationsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveInstallationsRow(InstallationData.InstallationsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        InstallationData installationData = new InstallationData();
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
          FixedValue = installationData.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (InstallationsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = installationData.GetSchemaSerializable();
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
      TypedTableBase<InstallationData.InstallationChangeLogRow>
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
      public InstallationData.InstallationChangeLogRow this[int index]
      {
        get => (InstallationData.InstallationChangeLogRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationChangeLogRowChangeEventHandler InstallationChangeLogRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddInstallationChangeLogRow(InstallationData.InstallationChangeLogRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.InstallationChangeLogRow AddInstallationChangeLogRow(
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
        InstallationData.InstallationChangeLogRow row = (InstallationData.InstallationChangeLogRow) this.NewRow();
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
      public InstallationData.InstallationChangeLogRow FindByInstallationIdChangeTime(
        int InstallationId,
        DateTime ChangeTime)
      {
        return (InstallationData.InstallationChangeLogRow) this.Rows.Find(new object[2]
        {
          (object) InstallationId,
          (object) ChangeTime
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        InstallationData.InstallationChangeLogDataTable changeLogDataTable = (InstallationData.InstallationChangeLogDataTable) base.Clone();
        changeLogDataTable.InitVars();
        return (DataTable) changeLogDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new InstallationData.InstallationChangeLogDataTable();
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
      public InstallationData.InstallationChangeLogRow NewInstallationChangeLogRow()
      {
        return (InstallationData.InstallationChangeLogRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new InstallationData.InstallationChangeLogRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (InstallationData.InstallationChangeLogRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.InstallationChangeLogRowChanged == null)
          return;
        this.InstallationChangeLogRowChanged((object) this, new InstallationData.InstallationChangeLogRowChangeEvent((InstallationData.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.InstallationChangeLogRowChanging == null)
          return;
        this.InstallationChangeLogRowChanging((object) this, new InstallationData.InstallationChangeLogRowChangeEvent((InstallationData.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.InstallationChangeLogRowDeleted == null)
          return;
        this.InstallationChangeLogRowDeleted((object) this, new InstallationData.InstallationChangeLogRowChangeEvent((InstallationData.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.InstallationChangeLogRowDeleting == null)
          return;
        this.InstallationChangeLogRowDeleting((object) this, new InstallationData.InstallationChangeLogRowChangeEvent((InstallationData.InstallationChangeLogRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveInstallationChangeLogRow(InstallationData.InstallationChangeLogRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        InstallationData installationData = new InstallationData();
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
          FixedValue = installationData.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (InstallationChangeLogDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = installationData.GetSchemaSerializable();
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
    public class InstallationUsersDataTable : TypedTableBase<InstallationData.InstallationUsersRow>
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
      public InstallationData.InstallationUsersRow this[int index]
      {
        get => (InstallationData.InstallationUsersRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationUsersRowChangeEventHandler InstallationUsersRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationUsersRowChangeEventHandler InstallationUsersRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationUsersRowChangeEventHandler InstallationUsersRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.InstallationUsersRowChangeEventHandler InstallationUsersRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddInstallationUsersRow(InstallationData.InstallationUsersRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.InstallationUsersRow AddInstallationUsersRow(
        int InstallationId,
        DateTime ChangeTime,
        int UserId)
      {
        InstallationData.InstallationUsersRow row = (InstallationData.InstallationUsersRow) this.NewRow();
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
      public InstallationData.InstallationUsersRow FindByInstallationIdChangeTime(
        int InstallationId,
        DateTime ChangeTime)
      {
        return (InstallationData.InstallationUsersRow) this.Rows.Find(new object[2]
        {
          (object) InstallationId,
          (object) ChangeTime
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        InstallationData.InstallationUsersDataTable installationUsersDataTable = (InstallationData.InstallationUsersDataTable) base.Clone();
        installationUsersDataTable.InitVars();
        return (DataTable) installationUsersDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new InstallationData.InstallationUsersDataTable();
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
      public InstallationData.InstallationUsersRow NewInstallationUsersRow()
      {
        return (InstallationData.InstallationUsersRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new InstallationData.InstallationUsersRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (InstallationData.InstallationUsersRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.InstallationUsersRowChanged == null)
          return;
        this.InstallationUsersRowChanged((object) this, new InstallationData.InstallationUsersRowChangeEvent((InstallationData.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.InstallationUsersRowChanging == null)
          return;
        this.InstallationUsersRowChanging((object) this, new InstallationData.InstallationUsersRowChangeEvent((InstallationData.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.InstallationUsersRowDeleted == null)
          return;
        this.InstallationUsersRowDeleted((object) this, new InstallationData.InstallationUsersRowChangeEvent((InstallationData.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.InstallationUsersRowDeleting == null)
          return;
        this.InstallationUsersRowDeleting((object) this, new InstallationData.InstallationUsersRowChangeEvent((InstallationData.InstallationUsersRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveInstallationUsersRow(InstallationData.InstallationUsersRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        InstallationData installationData = new InstallationData();
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
          FixedValue = installationData.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (InstallationUsersDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = installationData.GetSchemaSerializable();
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
    public class EQ_EquipmentValueDataTable : TypedTableBase<InstallationData.EQ_EquipmentValueRow>
    {
      private DataColumn columnValueID;
      private DataColumn columnEquipmentID;
      private DataColumn columnValue;
      private DataColumn columnValueType;
      private DataColumn columnValueUnit;
      private DataColumn columnFromDate;
      private DataColumn columnToDate;
      private DataColumn columnOrderNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EQ_EquipmentValueDataTable()
      {
        this.TableName = "EQ_EquipmentValue";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EQ_EquipmentValueDataTable(DataTable table)
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
      protected EQ_EquipmentValueDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ValueIDColumn => this.columnValueID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn EquipmentIDColumn => this.columnEquipmentID;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ValueColumn => this.columnValue;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ValueTypeColumn => this.columnValueType;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ValueUnitColumn => this.columnValueUnit;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn FromDateColumn => this.columnFromDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn ToDateColumn => this.columnToDate;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataColumn OrderNumberColumn => this.columnOrderNumber;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentValueRow this[int index]
      {
        get => (InstallationData.EQ_EquipmentValueRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentValueRowChangeEventHandler EQ_EquipmentValueRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentValueRowChangeEventHandler EQ_EquipmentValueRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentValueRowChangeEventHandler EQ_EquipmentValueRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public event InstallationData.EQ_EquipmentValueRowChangeEventHandler EQ_EquipmentValueRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void AddEQ_EquipmentValueRow(InstallationData.EQ_EquipmentValueRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentValueRow AddEQ_EquipmentValueRow(
        int ValueID,
        int EquipmentID,
        string Value,
        string ValueType,
        string ValueUnit,
        DateTime FromDate,
        DateTime ToDate,
        int OrderNumber)
      {
        InstallationData.EQ_EquipmentValueRow row = (InstallationData.EQ_EquipmentValueRow) this.NewRow();
        object[] objArray = new object[8]
        {
          (object) ValueID,
          (object) EquipmentID,
          (object) Value,
          (object) ValueType,
          (object) ValueUnit,
          (object) FromDate,
          (object) ToDate,
          (object) OrderNumber
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentValueRow FindByValueIDEquipmentIDFromDate(
        int ValueID,
        int EquipmentID,
        DateTime FromDate)
      {
        return (InstallationData.EQ_EquipmentValueRow) this.Rows.Find(new object[3]
        {
          (object) ValueID,
          (object) EquipmentID,
          (object) FromDate
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public override DataTable Clone()
      {
        InstallationData.EQ_EquipmentValueDataTable equipmentValueDataTable = (InstallationData.EQ_EquipmentValueDataTable) base.Clone();
        equipmentValueDataTable.InitVars();
        return (DataTable) equipmentValueDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new InstallationData.EQ_EquipmentValueDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal void InitVars()
      {
        this.columnValueID = this.Columns["ValueID"];
        this.columnEquipmentID = this.Columns["EquipmentID"];
        this.columnValue = this.Columns["Value"];
        this.columnValueType = this.Columns["ValueType"];
        this.columnValueUnit = this.Columns["ValueUnit"];
        this.columnFromDate = this.Columns["FromDate"];
        this.columnToDate = this.Columns["ToDate"];
        this.columnOrderNumber = this.Columns["OrderNumber"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      private void InitClass()
      {
        this.columnValueID = new DataColumn("ValueID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValueID);
        this.columnEquipmentID = new DataColumn("EquipmentID", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnEquipmentID);
        this.columnValue = new DataColumn("Value", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValue);
        this.columnValueType = new DataColumn("ValueType", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValueType);
        this.columnValueUnit = new DataColumn("ValueUnit", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnValueUnit);
        this.columnFromDate = new DataColumn("FromDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFromDate);
        this.columnToDate = new DataColumn("ToDate", typeof (DateTime), (string) null, MappingType.Element);
        this.Columns.Add(this.columnToDate);
        this.columnOrderNumber = new DataColumn("OrderNumber", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnOrderNumber);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[3]
        {
          this.columnValueID,
          this.columnEquipmentID,
          this.columnFromDate
        }, true));
        this.columnValueID.AllowDBNull = false;
        this.columnEquipmentID.AllowDBNull = false;
        this.columnValue.MaxLength = 100;
        this.columnValueType.MaxLength = 50;
        this.columnValueUnit.MaxLength = 50;
        this.columnFromDate.AllowDBNull = false;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentValueRow NewEQ_EquipmentValueRow()
      {
        return (InstallationData.EQ_EquipmentValueRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new InstallationData.EQ_EquipmentValueRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override Type GetRowType() => typeof (InstallationData.EQ_EquipmentValueRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.EQ_EquipmentValueRowChanged == null)
          return;
        this.EQ_EquipmentValueRowChanged((object) this, new InstallationData.EQ_EquipmentValueRowChangeEvent((InstallationData.EQ_EquipmentValueRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.EQ_EquipmentValueRowChanging == null)
          return;
        this.EQ_EquipmentValueRowChanging((object) this, new InstallationData.EQ_EquipmentValueRowChangeEvent((InstallationData.EQ_EquipmentValueRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.EQ_EquipmentValueRowDeleted == null)
          return;
        this.EQ_EquipmentValueRowDeleted((object) this, new InstallationData.EQ_EquipmentValueRowChangeEvent((InstallationData.EQ_EquipmentValueRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.EQ_EquipmentValueRowDeleting == null)
          return;
        this.EQ_EquipmentValueRowDeleting((object) this, new InstallationData.EQ_EquipmentValueRowChangeEvent((InstallationData.EQ_EquipmentValueRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void RemoveEQ_EquipmentValueRow(InstallationData.EQ_EquipmentValueRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        InstallationData installationData = new InstallationData();
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
          FixedValue = installationData.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (EQ_EquipmentValueDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = installationData.GetSchemaSerializable();
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

    public class EQ_EquipmentRow : DataRow
    {
      private InstallationData.EQ_EquipmentDataTable tableEQ_Equipment;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EQ_EquipmentRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableEQ_Equipment = (InstallationData.EQ_EquipmentDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int EquipmentID
      {
        get => (int) this[this.tableEQ_Equipment.EquipmentIDColumn];
        set => this[this.tableEQ_Equipment.EquipmentIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int EquipmentGroupID
      {
        get => (int) this[this.tableEQ_Equipment.EquipmentGroupIDColumn];
        set => this[this.tableEQ_Equipment.EquipmentGroupIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string EquipmentType
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_Equipment.EquipmentTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'EquipmentType' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.EquipmentTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string AccessName
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_Equipment.AccessNameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'AccessName' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.AccessNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Name
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_Equipment.NameColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Name' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.NameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Description
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_Equipment.DescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Description' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.DescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime CreationDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableEQ_Equipment.CreationDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'CreationDate' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.CreationDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime OutOfOperationDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableEQ_Equipment.OutOfOperationDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'OutOfOperationDate' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.OutOfOperationDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int LocationID
      {
        get
        {
          try
          {
            return (int) this[this.tableEQ_Equipment.LocationIDColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LocationID' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.LocationIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int OrderNumber
      {
        get
        {
          try
          {
            return (int) this[this.tableEQ_Equipment.OrderNumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'OrderNumber' in table 'EQ_Equipment' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_Equipment.OrderNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsEquipmentTypeNull() => this.IsNull(this.tableEQ_Equipment.EquipmentTypeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetEquipmentTypeNull()
      {
        this[this.tableEQ_Equipment.EquipmentTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsAccessNameNull() => this.IsNull(this.tableEQ_Equipment.AccessNameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetAccessNameNull()
      {
        this[this.tableEQ_Equipment.AccessNameColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsNameNull() => this.IsNull(this.tableEQ_Equipment.NameColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetNameNull() => this[this.tableEQ_Equipment.NameColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsDescriptionNull() => this.IsNull(this.tableEQ_Equipment.DescriptionColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetDescriptionNull()
      {
        this[this.tableEQ_Equipment.DescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsCreationDateNull() => this.IsNull(this.tableEQ_Equipment.CreationDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetCreationDateNull()
      {
        this[this.tableEQ_Equipment.CreationDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsOutOfOperationDateNull()
      {
        return this.IsNull(this.tableEQ_Equipment.OutOfOperationDateColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetOutOfOperationDateNull()
      {
        this[this.tableEQ_Equipment.OutOfOperationDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsLocationIDNull() => this.IsNull(this.tableEQ_Equipment.LocationIDColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetLocationIDNull()
      {
        this[this.tableEQ_Equipment.LocationIDColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsOrderNumberNull() => this.IsNull(this.tableEQ_Equipment.OrderNumberColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetOrderNumberNull()
      {
        this[this.tableEQ_Equipment.OrderNumberColumn] = Convert.DBNull;
      }
    }

    public class InstallationsRow : DataRow
    {
      private InstallationData.InstallationsDataTable tableInstallations;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableInstallations = (InstallationData.InstallationsDataTable) this.Table;
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
      private InstallationData.InstallationChangeLogDataTable tableInstallationChangeLog;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationChangeLogRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableInstallationChangeLog = (InstallationData.InstallationChangeLogDataTable) this.Table;
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

    public class InstallationUsersRow : DataRow
    {
      private InstallationData.InstallationUsersDataTable tableInstallationUsers;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal InstallationUsersRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableInstallationUsers = (InstallationData.InstallationUsersDataTable) this.Table;
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

    public class EQ_EquipmentValueRow : DataRow
    {
      private InstallationData.EQ_EquipmentValueDataTable tableEQ_EquipmentValue;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      internal EQ_EquipmentValueRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableEQ_EquipmentValue = (InstallationData.EQ_EquipmentValueDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int ValueID
      {
        get => (int) this[this.tableEQ_EquipmentValue.ValueIDColumn];
        set => this[this.tableEQ_EquipmentValue.ValueIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int EquipmentID
      {
        get => (int) this[this.tableEQ_EquipmentValue.EquipmentIDColumn];
        set => this[this.tableEQ_EquipmentValue.EquipmentIDColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string Value
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_EquipmentValue.ValueColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'Value' in table 'EQ_EquipmentValue' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_EquipmentValue.ValueColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ValueType
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_EquipmentValue.ValueTypeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ValueType' in table 'EQ_EquipmentValue' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_EquipmentValue.ValueTypeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public string ValueUnit
      {
        get
        {
          try
          {
            return (string) this[this.tableEQ_EquipmentValue.ValueUnitColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ValueUnit' in table 'EQ_EquipmentValue' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_EquipmentValue.ValueUnitColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime FromDate
      {
        get => (DateTime) this[this.tableEQ_EquipmentValue.FromDateColumn];
        set => this[this.tableEQ_EquipmentValue.FromDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DateTime ToDate
      {
        get
        {
          try
          {
            return (DateTime) this[this.tableEQ_EquipmentValue.ToDateColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ToDate' in table 'EQ_EquipmentValue' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_EquipmentValue.ToDateColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public int OrderNumber
      {
        get
        {
          try
          {
            return (int) this[this.tableEQ_EquipmentValue.OrderNumberColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'OrderNumber' in table 'EQ_EquipmentValue' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableEQ_EquipmentValue.OrderNumberColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsValueNull() => this.IsNull(this.tableEQ_EquipmentValue.ValueColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetValueNull() => this[this.tableEQ_EquipmentValue.ValueColumn] = Convert.DBNull;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsValueTypeNull() => this.IsNull(this.tableEQ_EquipmentValue.ValueTypeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetValueTypeNull()
      {
        this[this.tableEQ_EquipmentValue.ValueTypeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsValueUnitNull() => this.IsNull(this.tableEQ_EquipmentValue.ValueUnitColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetValueUnitNull()
      {
        this[this.tableEQ_EquipmentValue.ValueUnitColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsToDateNull() => this.IsNull(this.tableEQ_EquipmentValue.ToDateColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetToDateNull()
      {
        this[this.tableEQ_EquipmentValue.ToDateColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public bool IsOrderNumberNull() => this.IsNull(this.tableEQ_EquipmentValue.OrderNumberColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public void SetOrderNumberNull()
      {
        this[this.tableEQ_EquipmentValue.OrderNumberColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class EQ_EquipmentRowChangeEvent : EventArgs
    {
      private InstallationData.EQ_EquipmentRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EQ_EquipmentRowChangeEvent(InstallationData.EQ_EquipmentRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class InstallationsRowChangeEvent : EventArgs
    {
      private InstallationData.InstallationsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationsRowChangeEvent(
        InstallationData.InstallationsRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.InstallationsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class InstallationChangeLogRowChangeEvent : EventArgs
    {
      private InstallationData.InstallationChangeLogRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationChangeLogRowChangeEvent(
        InstallationData.InstallationChangeLogRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.InstallationChangeLogRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class InstallationUsersRowChangeEvent : EventArgs
    {
      private InstallationData.InstallationUsersRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationUsersRowChangeEvent(
        InstallationData.InstallationUsersRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.InstallationUsersRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
    public class EQ_EquipmentValueRowChangeEvent : EventArgs
    {
      private InstallationData.EQ_EquipmentValueRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public EQ_EquipmentValueRowChangeEvent(
        InstallationData.EQ_EquipmentValueRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public InstallationData.EQ_EquipmentValueRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
