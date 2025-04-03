// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DataSets.HandlerTables
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
  [XmlRoot("HandlerTables")]
  [HelpKeyword("vs.data.DataSet")]
  [Serializable]
  public class HandlerTables : DataSet
  {
    private HandlerTables.SmartFunctionsDataTable tableSmartFunctions;
    private HandlerTables.ScenarioDefinitionDataTable tableScenarioDefinition;
    private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public HandlerTables()
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
    protected HandlerTables(SerializationInfo info, StreamingContext context)
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
          if (dataSet.Tables[nameof (SmartFunctions)] != null)
            base.Tables.Add((DataTable) new HandlerTables.SmartFunctionsDataTable(dataSet.Tables[nameof (SmartFunctions)]));
          if (dataSet.Tables[nameof (ScenarioDefinition)] != null)
            base.Tables.Add((DataTable) new HandlerTables.ScenarioDefinitionDataTable(dataSet.Tables[nameof (ScenarioDefinition)]));
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
    public HandlerTables.SmartFunctionsDataTable SmartFunctions => this.tableSmartFunctions;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public HandlerTables.ScenarioDefinitionDataTable ScenarioDefinition
    {
      get => this.tableScenarioDefinition;
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
      HandlerTables handlerTables = (HandlerTables) base.Clone();
      handlerTables.InitVars();
      handlerTables.SchemaSerializationMode = this.SchemaSerializationMode;
      return (DataSet) handlerTables;
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
        if (dataSet.Tables["SmartFunctions"] != null)
          base.Tables.Add((DataTable) new HandlerTables.SmartFunctionsDataTable(dataSet.Tables["SmartFunctions"]));
        if (dataSet.Tables["ScenarioDefinition"] != null)
          base.Tables.Add((DataTable) new HandlerTables.ScenarioDefinitionDataTable(dataSet.Tables["ScenarioDefinition"]));
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
      this.tableSmartFunctions = (HandlerTables.SmartFunctionsDataTable) base.Tables["SmartFunctions"];
      if (initTable && this.tableSmartFunctions != null)
        this.tableSmartFunctions.InitVars();
      this.tableScenarioDefinition = (HandlerTables.ScenarioDefinitionDataTable) base.Tables["ScenarioDefinition"];
      if (!initTable || this.tableScenarioDefinition == null)
        return;
      this.tableScenarioDefinition.InitVars();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private void InitClass()
    {
      this.DataSetName = nameof (HandlerTables);
      this.Prefix = "";
      this.Namespace = "http://tempuri.org/HandlerTables.xsd";
      this.EnforceConstraints = true;
      this.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
      this.tableSmartFunctions = new HandlerTables.SmartFunctionsDataTable();
      base.Tables.Add((DataTable) this.tableSmartFunctions);
      this.tableScenarioDefinition = new HandlerTables.ScenarioDefinitionDataTable();
      base.Tables.Add((DataTable) this.tableScenarioDefinition);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeSmartFunctions() => false;

    [DebuggerNonUserCode]
    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    private bool ShouldSerializeScenarioDefinition() => false;

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
      HandlerTables handlerTables = new HandlerTables();
      XmlSchemaComplexType typedDataSetSchema = new XmlSchemaComplexType();
      XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
      xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
      {
        Namespace = handlerTables.Namespace
      });
      typedDataSetSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
      XmlSchema schemaSerializable = handlerTables.GetSchemaSerializable();
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
    public delegate void SmartFunctionsRowChangeEventHandler(
      object sender,
      HandlerTables.SmartFunctionsRowChangeEvent e);

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public delegate void ScenarioDefinitionRowChangeEventHandler(
      object sender,
      HandlerTables.ScenarioDefinitionRowChangeEvent e);

    [XmlSchemaProvider("GetTypedTableSchema")]
    [Serializable]
    public class SmartFunctionsDataTable : TypedTableBase<HandlerTables.SmartFunctionsRow>
    {
      private DataColumn columnFunctionName;
      private DataColumn columnFunctionVersion;
      private DataColumn columnLoadOrder;
      private DataColumn columnInterpreterVersion;
      private DataColumn columnFunctionEvent;
      private DataColumn columnFunctionCode;
      private DataColumn columnFunctionDescription;
      private DataColumn columnRequiredFunctions;
      private DataColumn columnMemberOfGroups;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public SmartFunctionsDataTable()
      {
        this.TableName = "SmartFunctions";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal SmartFunctionsDataTable(DataTable table)
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
      protected SmartFunctionsDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FunctionNameColumn => this.columnFunctionName;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FunctionVersionColumn => this.columnFunctionVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LoadOrderColumn => this.columnLoadOrder;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn InterpreterVersionColumn => this.columnInterpreterVersion;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FunctionEventColumn => this.columnFunctionEvent;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FunctionCodeColumn => this.columnFunctionCode;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn FunctionDescriptionColumn => this.columnFunctionDescription;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn RequiredFunctionsColumn => this.columnRequiredFunctions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn MemberOfGroupsColumn => this.columnMemberOfGroups;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.SmartFunctionsRow this[int index]
      {
        get => (HandlerTables.SmartFunctionsRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.SmartFunctionsRowChangeEventHandler SmartFunctionsRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.SmartFunctionsRowChangeEventHandler SmartFunctionsRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.SmartFunctionsRowChangeEventHandler SmartFunctionsRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.SmartFunctionsRowChangeEventHandler SmartFunctionsRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddSmartFunctionsRow(HandlerTables.SmartFunctionsRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.SmartFunctionsRow AddSmartFunctionsRow(
        string FunctionName,
        byte FunctionVersion,
        short LoadOrder,
        byte InterpreterVersion,
        string FunctionEvent,
        byte[] FunctionCode,
        string FunctionDescription,
        string RequiredFunctions,
        string MemberOfGroups)
      {
        HandlerTables.SmartFunctionsRow row = (HandlerTables.SmartFunctionsRow) this.NewRow();
        object[] objArray = new object[9]
        {
          (object) FunctionName,
          (object) FunctionVersion,
          (object) LoadOrder,
          (object) InterpreterVersion,
          (object) FunctionEvent,
          (object) FunctionCode,
          (object) FunctionDescription,
          (object) RequiredFunctions,
          (object) MemberOfGroups
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.SmartFunctionsRow FindByFunctionNameFunctionVersion(
        string FunctionName,
        byte FunctionVersion)
      {
        return (HandlerTables.SmartFunctionsRow) this.Rows.Find(new object[2]
        {
          (object) FunctionName,
          (object) FunctionVersion
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HandlerTables.SmartFunctionsDataTable functionsDataTable = (HandlerTables.SmartFunctionsDataTable) base.Clone();
        functionsDataTable.InitVars();
        return (DataTable) functionsDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HandlerTables.SmartFunctionsDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnFunctionName = this.Columns["FunctionName"];
        this.columnFunctionVersion = this.Columns["FunctionVersion"];
        this.columnLoadOrder = this.Columns["LoadOrder"];
        this.columnInterpreterVersion = this.Columns["InterpreterVersion"];
        this.columnFunctionEvent = this.Columns["FunctionEvent"];
        this.columnFunctionCode = this.Columns["FunctionCode"];
        this.columnFunctionDescription = this.Columns["FunctionDescription"];
        this.columnRequiredFunctions = this.Columns["RequiredFunctions"];
        this.columnMemberOfGroups = this.Columns["MemberOfGroups"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnFunctionName = new DataColumn("FunctionName", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFunctionName);
        this.columnFunctionVersion = new DataColumn("FunctionVersion", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFunctionVersion);
        this.columnLoadOrder = new DataColumn("LoadOrder", typeof (short), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLoadOrder);
        this.columnInterpreterVersion = new DataColumn("InterpreterVersion", typeof (byte), (string) null, MappingType.Element);
        this.Columns.Add(this.columnInterpreterVersion);
        this.columnFunctionEvent = new DataColumn("FunctionEvent", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFunctionEvent);
        this.columnFunctionCode = new DataColumn("FunctionCode", typeof (byte[]), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFunctionCode);
        this.columnFunctionDescription = new DataColumn("FunctionDescription", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnFunctionDescription);
        this.columnRequiredFunctions = new DataColumn("RequiredFunctions", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnRequiredFunctions);
        this.columnMemberOfGroups = new DataColumn("MemberOfGroups", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnMemberOfGroups);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[2]
        {
          this.columnFunctionName,
          this.columnFunctionVersion
        }, true));
        this.columnFunctionName.AllowDBNull = false;
        this.columnFunctionName.MaxLength = 64;
        this.columnFunctionVersion.AllowDBNull = false;
        this.columnLoadOrder.AllowDBNull = false;
        this.columnLoadOrder.DefaultValue = (object) (short) 0;
        this.columnFunctionEvent.MaxLength = 64;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.SmartFunctionsRow NewSmartFunctionsRow()
      {
        return (HandlerTables.SmartFunctionsRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HandlerTables.SmartFunctionsRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (HandlerTables.SmartFunctionsRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.SmartFunctionsRowChanged == null)
          return;
        this.SmartFunctionsRowChanged((object) this, new HandlerTables.SmartFunctionsRowChangeEvent((HandlerTables.SmartFunctionsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.SmartFunctionsRowChanging == null)
          return;
        this.SmartFunctionsRowChanging((object) this, new HandlerTables.SmartFunctionsRowChangeEvent((HandlerTables.SmartFunctionsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.SmartFunctionsRowDeleted == null)
          return;
        this.SmartFunctionsRowDeleted((object) this, new HandlerTables.SmartFunctionsRowChangeEvent((HandlerTables.SmartFunctionsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.SmartFunctionsRowDeleting == null)
          return;
        this.SmartFunctionsRowDeleting((object) this, new HandlerTables.SmartFunctionsRowChangeEvent((HandlerTables.SmartFunctionsRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveSmartFunctionsRow(HandlerTables.SmartFunctionsRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HandlerTables handlerTables = new HandlerTables();
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
          FixedValue = handlerTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (SmartFunctionsDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = handlerTables.GetSchemaSerializable();
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
    public class ScenarioDefinitionDataTable : TypedTableBase<HandlerTables.ScenarioDefinitionRow>
    {
      private DataColumn columnScenario;
      private DataColumn columnGroupNames;
      private DataColumn columnScenarioSource;
      private DataColumn columnLoadDefines;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ScenarioDefinitionDataTable()
      {
        this.TableName = "ScenarioDefinition";
        this.BeginInit();
        this.InitClass();
        this.EndInit();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ScenarioDefinitionDataTable(DataTable table)
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
      protected ScenarioDefinitionDataTable(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
        this.InitVars();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ScenarioColumn => this.columnScenario;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn GroupNamesColumn => this.columnGroupNames;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn ScenarioSourceColumn => this.columnScenarioSource;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataColumn LoadDefinesColumn => this.columnLoadDefines;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      [Browsable(false)]
      public int Count => this.Rows.Count;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.ScenarioDefinitionRow this[int index]
      {
        get => (HandlerTables.ScenarioDefinitionRow) this.Rows[index];
      }

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.ScenarioDefinitionRowChangeEventHandler ScenarioDefinitionRowChanging;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.ScenarioDefinitionRowChangeEventHandler ScenarioDefinitionRowChanged;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.ScenarioDefinitionRowChangeEventHandler ScenarioDefinitionRowDeleting;

      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public event HandlerTables.ScenarioDefinitionRowChangeEventHandler ScenarioDefinitionRowDeleted;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void AddScenarioDefinitionRow(HandlerTables.ScenarioDefinitionRow row)
      {
        this.Rows.Add((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.ScenarioDefinitionRow AddScenarioDefinitionRow(
        int Scenario,
        string GroupNames,
        string ScenarioSource,
        string LoadDefines)
      {
        HandlerTables.ScenarioDefinitionRow row = (HandlerTables.ScenarioDefinitionRow) this.NewRow();
        object[] objArray = new object[4]
        {
          (object) Scenario,
          (object) GroupNames,
          (object) ScenarioSource,
          (object) LoadDefines
        };
        row.ItemArray = objArray;
        this.Rows.Add((DataRow) row);
        return row;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.ScenarioDefinitionRow FindByScenario(int Scenario)
      {
        return (HandlerTables.ScenarioDefinitionRow) this.Rows.Find(new object[1]
        {
          (object) Scenario
        });
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public override DataTable Clone()
      {
        HandlerTables.ScenarioDefinitionDataTable definitionDataTable = (HandlerTables.ScenarioDefinitionDataTable) base.Clone();
        definitionDataTable.InitVars();
        return (DataTable) definitionDataTable;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataTable CreateInstance()
      {
        return (DataTable) new HandlerTables.ScenarioDefinitionDataTable();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal void InitVars()
      {
        this.columnScenario = this.Columns["Scenario"];
        this.columnGroupNames = this.Columns["GroupNames"];
        this.columnScenarioSource = this.Columns["ScenarioSource"];
        this.columnLoadDefines = this.Columns["LoadDefines"];
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      private void InitClass()
      {
        this.columnScenario = new DataColumn("Scenario", typeof (int), (string) null, MappingType.Element);
        this.Columns.Add(this.columnScenario);
        this.columnGroupNames = new DataColumn("GroupNames", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnGroupNames);
        this.columnScenarioSource = new DataColumn("ScenarioSource", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnScenarioSource);
        this.columnLoadDefines = new DataColumn("LoadDefines", typeof (string), (string) null, MappingType.Element);
        this.Columns.Add(this.columnLoadDefines);
        this.Constraints.Add((Constraint) new UniqueConstraint("Constraint1", new DataColumn[1]
        {
          this.columnScenario
        }, true));
        this.columnScenario.AllowDBNull = false;
        this.columnScenario.Unique = true;
        this.columnGroupNames.MaxLength = (int) byte.MaxValue;
        this.columnScenarioSource.MaxLength = 536870910;
        this.columnLoadDefines.MaxLength = (int) byte.MaxValue;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.ScenarioDefinitionRow NewScenarioDefinitionRow()
      {
        return (HandlerTables.ScenarioDefinitionRow) this.NewRow();
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
      {
        return (DataRow) new HandlerTables.ScenarioDefinitionRow(builder);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override Type GetRowType() => typeof (HandlerTables.ScenarioDefinitionRow);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanged(DataRowChangeEventArgs e)
      {
        base.OnRowChanged(e);
        if (this.ScenarioDefinitionRowChanged == null)
          return;
        this.ScenarioDefinitionRowChanged((object) this, new HandlerTables.ScenarioDefinitionRowChangeEvent((HandlerTables.ScenarioDefinitionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowChanging(DataRowChangeEventArgs e)
      {
        base.OnRowChanging(e);
        if (this.ScenarioDefinitionRowChanging == null)
          return;
        this.ScenarioDefinitionRowChanging((object) this, new HandlerTables.ScenarioDefinitionRowChangeEvent((HandlerTables.ScenarioDefinitionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleted(DataRowChangeEventArgs e)
      {
        base.OnRowDeleted(e);
        if (this.ScenarioDefinitionRowDeleted == null)
          return;
        this.ScenarioDefinitionRowDeleted((object) this, new HandlerTables.ScenarioDefinitionRowChangeEvent((HandlerTables.ScenarioDefinitionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      protected override void OnRowDeleting(DataRowChangeEventArgs e)
      {
        base.OnRowDeleting(e);
        if (this.ScenarioDefinitionRowDeleting == null)
          return;
        this.ScenarioDefinitionRowDeleting((object) this, new HandlerTables.ScenarioDefinitionRowChangeEvent((HandlerTables.ScenarioDefinitionRow) e.Row, e.Action));
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void RemoveScenarioDefinitionRow(HandlerTables.ScenarioDefinitionRow row)
      {
        this.Rows.Remove((DataRow) row);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
      {
        XmlSchemaComplexType typedTableSchema = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        HandlerTables handlerTables = new HandlerTables();
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
          FixedValue = handlerTables.Namespace
        });
        typedTableSchema.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
        {
          Name = "tableTypeName",
          FixedValue = nameof (ScenarioDefinitionDataTable)
        });
        typedTableSchema.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        XmlSchema schemaSerializable = handlerTables.GetSchemaSerializable();
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

    public class SmartFunctionsRow : DataRow
    {
      private HandlerTables.SmartFunctionsDataTable tableSmartFunctions;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal SmartFunctionsRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableSmartFunctions = (HandlerTables.SmartFunctionsDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string FunctionName
      {
        get => (string) this[this.tableSmartFunctions.FunctionNameColumn];
        set => this[this.tableSmartFunctions.FunctionNameColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public byte FunctionVersion
      {
        get => (byte) this[this.tableSmartFunctions.FunctionVersionColumn];
        set => this[this.tableSmartFunctions.FunctionVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public short LoadOrder
      {
        get => (short) this[this.tableSmartFunctions.LoadOrderColumn];
        set => this[this.tableSmartFunctions.LoadOrderColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public byte InterpreterVersion
      {
        get
        {
          try
          {
            return (byte) this[this.tableSmartFunctions.InterpreterVersionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'InterpreterVersion' in table 'SmartFunctions' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSmartFunctions.InterpreterVersionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string FunctionEvent
      {
        get
        {
          try
          {
            return (string) this[this.tableSmartFunctions.FunctionEventColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FunctionEvent' in table 'SmartFunctions' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSmartFunctions.FunctionEventColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public byte[] FunctionCode
      {
        get
        {
          try
          {
            return (byte[]) this[this.tableSmartFunctions.FunctionCodeColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FunctionCode' in table 'SmartFunctions' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSmartFunctions.FunctionCodeColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string FunctionDescription
      {
        get
        {
          try
          {
            return (string) this[this.tableSmartFunctions.FunctionDescriptionColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'FunctionDescription' in table 'SmartFunctions' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSmartFunctions.FunctionDescriptionColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string RequiredFunctions
      {
        get
        {
          try
          {
            return (string) this[this.tableSmartFunctions.RequiredFunctionsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'RequiredFunctions' in table 'SmartFunctions' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSmartFunctions.RequiredFunctionsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string MemberOfGroups
      {
        get
        {
          try
          {
            return (string) this[this.tableSmartFunctions.MemberOfGroupsColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'MemberOfGroups' in table 'SmartFunctions' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableSmartFunctions.MemberOfGroupsColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsInterpreterVersionNull()
      {
        return this.IsNull(this.tableSmartFunctions.InterpreterVersionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetInterpreterVersionNull()
      {
        this[this.tableSmartFunctions.InterpreterVersionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFunctionEventNull()
      {
        return this.IsNull(this.tableSmartFunctions.FunctionEventColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFunctionEventNull()
      {
        this[this.tableSmartFunctions.FunctionEventColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFunctionCodeNull() => this.IsNull(this.tableSmartFunctions.FunctionCodeColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFunctionCodeNull()
      {
        this[this.tableSmartFunctions.FunctionCodeColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsFunctionDescriptionNull()
      {
        return this.IsNull(this.tableSmartFunctions.FunctionDescriptionColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetFunctionDescriptionNull()
      {
        this[this.tableSmartFunctions.FunctionDescriptionColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsRequiredFunctionsNull()
      {
        return this.IsNull(this.tableSmartFunctions.RequiredFunctionsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetRequiredFunctionsNull()
      {
        this[this.tableSmartFunctions.RequiredFunctionsColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsMemberOfGroupsNull()
      {
        return this.IsNull(this.tableSmartFunctions.MemberOfGroupsColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetMemberOfGroupsNull()
      {
        this[this.tableSmartFunctions.MemberOfGroupsColumn] = Convert.DBNull;
      }
    }

    public class ScenarioDefinitionRow : DataRow
    {
      private HandlerTables.ScenarioDefinitionDataTable tableScenarioDefinition;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      internal ScenarioDefinitionRow(DataRowBuilder rb)
        : base(rb)
      {
        this.tableScenarioDefinition = (HandlerTables.ScenarioDefinitionDataTable) this.Table;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public int Scenario
      {
        get => (int) this[this.tableScenarioDefinition.ScenarioColumn];
        set => this[this.tableScenarioDefinition.ScenarioColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string GroupNames
      {
        get
        {
          try
          {
            return (string) this[this.tableScenarioDefinition.GroupNamesColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'GroupNames' in table 'ScenarioDefinition' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableScenarioDefinition.GroupNamesColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string ScenarioSource
      {
        get
        {
          try
          {
            return (string) this[this.tableScenarioDefinition.ScenarioSourceColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'ScenarioSource' in table 'ScenarioDefinition' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableScenarioDefinition.ScenarioSourceColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public string LoadDefines
      {
        get
        {
          try
          {
            return (string) this[this.tableScenarioDefinition.LoadDefinesColumn];
          }
          catch (InvalidCastException ex)
          {
            throw new StrongTypingException("The value for column 'LoadDefines' in table 'ScenarioDefinition' is DBNull.", (Exception) ex);
          }
        }
        set => this[this.tableScenarioDefinition.LoadDefinesColumn] = (object) value;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsGroupNamesNull() => this.IsNull(this.tableScenarioDefinition.GroupNamesColumn);

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetGroupNamesNull()
      {
        this[this.tableScenarioDefinition.GroupNamesColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsScenarioSourceNull()
      {
        return this.IsNull(this.tableScenarioDefinition.ScenarioSourceColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetScenarioSourceNull()
      {
        this[this.tableScenarioDefinition.ScenarioSourceColumn] = Convert.DBNull;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public bool IsLoadDefinesNull()
      {
        return this.IsNull(this.tableScenarioDefinition.LoadDefinesColumn);
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public void SetLoadDefinesNull()
      {
        this[this.tableScenarioDefinition.LoadDefinesColumn] = Convert.DBNull;
      }
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class SmartFunctionsRowChangeEvent : EventArgs
    {
      private HandlerTables.SmartFunctionsRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public SmartFunctionsRowChangeEvent(HandlerTables.SmartFunctionsRow row, DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.SmartFunctionsRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }

    [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
    public class ScenarioDefinitionRowChangeEvent : EventArgs
    {
      private HandlerTables.ScenarioDefinitionRow eventRow;
      private DataRowAction eventAction;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public ScenarioDefinitionRowChangeEvent(
        HandlerTables.ScenarioDefinitionRow row,
        DataRowAction action)
      {
        this.eventRow = row;
        this.eventAction = action;
      }

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public HandlerTables.ScenarioDefinitionRow Row => this.eventRow;

      [DebuggerNonUserCode]
      [GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
      public DataRowAction Action => this.eventAction;
    }
  }
}
