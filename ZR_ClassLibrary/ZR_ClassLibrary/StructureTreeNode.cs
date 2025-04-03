// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.StructureTreeNode
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using ZR_ClassLibrary.Properties;

#nullable disable
namespace ZR_ClassLibrary
{
  public class StructureTreeNode : ISortable
  {
    public const string MODUL_NAME = "MeterInstaller";
    private const string TAG_FILTER = "FILTER";
    private const string TAG_SERIALNUMBER = "SID";
    private const string TAG_MANUFACTURER = "MAN";
    private const string TAG_MEDIUM = "MED";
    private const string TAG_MEDIUM_SUB_DEVICE = "TYPE[0][{0}]";
    private const string TAG_SERIALNUMBER_SUB_DEVICE = "CID[0][{0}]";
    private const string TAG_ADDRESS = "RADR";
    private const string TAG_RSSI = "RSSI_dBm";
    private const string TAG_INTERVAL = "INTERVAL";
    private const string TAG_SUB_DEVICE_IDX = "SUB_DEVICE_IDX";
    private const string TAG_READ_ENABLED = "READ_ENABLED";
    private const string TAG_READOUT_TYPE = "READOUT_TYPE";
    private string nodeSettings;
    private string _name;

    public StructureTreeNode()
    {
      this.Parent = (StructureTreeNode) null;
      this.Children = new StructureTreeNodeList(this);
      this.NodeTyp = StructureNodeType.Unknown;
      this.NodeErrors = new List<string>();
      this.NodeSettings = string.Empty;
      this.NodeDescription = string.Empty;
      this.NodeAdditionalInfos = string.Empty;
      this.MeterReplacementHistoryList = new StructureTreeNodeList();
      this.ReadEnabled = false;
    }

    public StructureTreeNode(StructureTreeNode parent)
      : this()
    {
      this.Parent = parent;
    }

    public StructureTreeNode(StructureTreeNodeList children)
      : this()
    {
      this.Children = children;
    }

    public StructureTreeNode(StructureTreeNode parent, StructureTreeNodeList children)
      : this()
    {
      this.Parent = parent;
      this.Children = children;
      this.Children.Parent = this;
    }

    public StructureTreeNode(string name, StructureNodeType type)
      : this()
    {
      this.Name = name;
      this.NodeTyp = type;
    }

    public StructureTreeNode Parent { get; set; }

    public StructureTreeNode OldParent { get; set; }

    public StructureTreeNodeList Children { get; set; }

    public int Depth
    {
      get
      {
        int depth = 0;
        StructureTreeNode structureTreeNode = this;
        while (structureTreeNode.Parent != null)
        {
          structureTreeNode = structureTreeNode.Parent;
          ++depth;
        }
        return depth;
      }
    }

    public string NamePath
    {
      get
      {
        StructureTreeNode structureTreeNode = this;
        string namePath = structureTreeNode.Name;
        while (structureTreeNode.Parent != null)
        {
          structureTreeNode = structureTreeNode.Parent;
          namePath = structureTreeNode.Name + "->" + namePath;
        }
        return namePath;
      }
    }

    public int? NodeID { get; set; }

    public int? MeterID { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime? InstallationDate
    {
      get => this.ValidFrom == DateTime.MinValue ? new DateTime?() : new DateTime?(this.ValidFrom);
    }

    public DateTime? ValidTo { get; set; }

    public string SerialNumber
    {
      get
      {
        if (this.NodeTyp != StructureNodeType.Meter)
          return string.Empty;
        string key = "SID";
        if (this.SubDeviceIndex > 0)
        {
          key = TranslationRulesManager.TryGetSpecialTranslationKeyOfSubDevice(this.nodeSettings, this.SubDeviceIndex, SpecialTranslation.Serialnumber);
          if (string.IsNullOrEmpty(key))
            key = string.Format("CID[0][{0}]", (object) this.SubDeviceIndex);
        }
        if (string.IsNullOrEmpty(key))
          return string.Empty;
        string nodeSettingsValue = this.GetNodeSettingsValue(key);
        if (string.IsNullOrEmpty(nodeSettingsValue))
          return string.Empty;
        string str = nodeSettingsValue.Trim();
        return new List<char>((IEnumerable<char>) str.ToCharArray()).FindAll((Predicate<char>) (e => e == '0')).Count == str.Length ? string.Empty : str;
      }
      set
      {
        string key = "SID";
        if (this.SubDeviceIndex > 0)
        {
          key = TranslationRulesManager.TryGetSpecialTranslationKeyOfSubDevice(this.nodeSettings, this.SubDeviceIndex, SpecialTranslation.Serialnumber);
          if (string.IsNullOrEmpty(key))
            key = string.Format("CID[0][{0}]", (object) this.SubDeviceIndex);
        }
        if (string.IsNullOrEmpty(key))
          return;
        this.SetNodeSettingsValue(key, value);
      }
    }

    public string Manufacturer
    {
      get
      {
        return this.NodeTyp != StructureNodeType.Meter ? string.Empty : this.GetNodeSettingsValue("MAN");
      }
    }

    public string MediumString
    {
      get => this.Medium != MBusDeviceType.UNKNOWN ? this.Medium.ToString() : string.Empty;
    }

    public MBusDeviceType Medium
    {
      get
      {
        if (this.NodeTyp != StructureNodeType.Meter)
          return MBusDeviceType.UNKNOWN;
        string key = "MED";
        if (this.SubDeviceIndex > 0)
        {
          key = TranslationRulesManager.TryGetSpecialTranslationKeyOfSubDevice(this.nodeSettings, this.SubDeviceIndex, SpecialTranslation.MeterType);
          if (string.IsNullOrEmpty(key))
            key = string.Format("TYPE[0][{0}]", (object) this.SubDeviceIndex);
        }
        if (string.IsNullOrEmpty(key))
          return MBusDeviceType.UNKNOWN;
        string nodeSettingsValue = this.GetNodeSettingsValue(key);
        return Enum.IsDefined(typeof (MBusDeviceType), (object) nodeSettingsValue) ? (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), nodeSettingsValue, true) : MBusDeviceType.UNKNOWN;
      }
      set
      {
        if (this.NodeTyp != StructureNodeType.Meter)
          return;
        string key = "MED";
        if (this.SubDeviceIndex > 0)
        {
          key = TranslationRulesManager.TryGetSpecialTranslationKeyOfSubDevice(this.nodeSettings, this.SubDeviceIndex, SpecialTranslation.MeterType);
          if (string.IsNullOrEmpty(key))
            key = string.Format("TYPE[0][{0}]", (object) this.SubDeviceIndex);
        }
        if (string.IsNullOrEmpty(key))
          return;
        this.SetNodeSettingsValue(key, value.ToString());
      }
    }

    public int SubDeviceIndex
    {
      get
      {
        if (string.IsNullOrEmpty(this.NodeSettings))
          return 0;
        string nodeSettingsValue = this.GetNodeSettingsValue("SUB_DEVICE_IDX");
        return string.IsNullOrEmpty(nodeSettingsValue) ? 0 : Convert.ToInt32(nodeSettingsValue);
      }
      set => this.UpdateNodeSettingsValue("SUB_DEVICE_IDX", (object) value);
    }

    public bool ReadEnabled
    {
      get
      {
        if (string.IsNullOrEmpty(this.NodeSettings))
          return true;
        string nodeSettingsValue = this.GetNodeSettingsValue("READ_ENABLED");
        return string.IsNullOrEmpty(nodeSettingsValue) || Convert.ToBoolean(nodeSettingsValue);
      }
      set => this.UpdateNodeSettingsValue("READ_ENABLED", (object) value);
    }

    public StructureNodeType NodeTyp { get; set; }

    public int? LayerID { get; set; }

    public string Name
    {
      get => this._name;
      set
      {
        this._name = value == null || value.Length <= 50 ? value : throw new ArgumentException("The name should be less as 50 chars! Actually is: " + value.Length.ToString());
      }
    }

    public List<string> NodeErrors { get; set; }

    public string NodeSettings
    {
      get => this.nodeSettings;
      set
      {
        if (!(this.nodeSettings != value))
          return;
        this.nodeSettings = value;
      }
    }

    public string DeviceInfo => StructureTreeNode.GenerateDeviceInfo(this.NodeSettings);

    public string NodeDescription { get; set; }

    public int? Interval
    {
      get
      {
        if (string.IsNullOrEmpty(this.NodeSettings))
          return new int?();
        string nodeSettingsValue = this.GetNodeSettingsValue("INTERVAL");
        return string.IsNullOrEmpty(nodeSettingsValue) || !Util.IsNumeric((object) nodeSettingsValue) ? new int?() : new int?(Convert.ToInt32(nodeSettingsValue));
      }
      set => this.UpdateNodeSettingsValue("INTERVAL", (object) value);
    }

    public GMMSettings CommunicationSettings
    {
      get
      {
        if (this.NodeTyp != StructureNodeType.Meter || !this.MeterID.HasValue || !this.ReadoutDeviceTypeID.HasValue)
          return (GMMSettings) null;
        ReadoutSettingsManager.ReadoutDeviceSettings readoutDeviceSettings = ReadoutSettingsManager.Instance.Settings.Find((Predicate<ReadoutSettingsManager.ReadoutDeviceSettings>) (e => e.MeterID == this.MeterID.Value));
        if (readoutDeviceSettings == null || string.IsNullOrEmpty(readoutDeviceSettings.Settings))
          return (GMMSettings) null;
        GMMSettings communicationSettings = new GMMSettings();
        communicationSettings.ReadoutType = new ReadoutType()
        {
          ReadoutSettingsID = readoutDeviceSettings.ReadoutSettingsID,
          ReadoutDeviceTypeID = this.ReadoutDeviceTypeID.Value
        };
        communicationSettings.SetSettings(readoutDeviceSettings.Settings);
        return communicationSettings;
      }
    }

    public int NodeOrder { get; set; }

    public string NodeAdditionalInfos { get; set; }

    public int? ValueIdentFilter
    {
      get
      {
        if (string.IsNullOrEmpty(this.NodeSettings))
          return new int?();
        string nodeSettingsValue = this.GetNodeSettingsValue("FILTER");
        if (string.IsNullOrEmpty(nodeSettingsValue))
          return new int?();
        return !Util.IsNumeric((object) nodeSettingsValue) ? new int?() : new int?(Convert.ToInt32(nodeSettingsValue));
      }
      set => this.UpdateNodeSettingsValue("FILTER", (object) value);
    }

    public static List<Filter> GecachedFilterList { get; set; }

    public string FilterName
    {
      get
      {
        if (!this.ValueIdentFilter.HasValue)
          return string.Empty;
        if (StructureTreeNode.GecachedFilterList == null)
          StructureTreeNode.GecachedFilterList = MeterDatabase.LoadFilter();
        if (StructureTreeNode.GecachedFilterList == null)
          return string.Empty;
        Filter filter = StructureTreeNode.GecachedFilterList.Find((Predicate<Filter>) (e => e.FilterId == this.ValueIdentFilter.Value));
        return filter == null ? string.Empty : filter.Name;
      }
    }

    public int? ReadoutDeviceTypeID
    {
      get
      {
        if (string.IsNullOrEmpty(this.NodeSettings))
          return new int?();
        if (this.NodeSettings.IndexOf("READOUT_TYPE") < 0)
          return new int?();
        try
        {
          return new int?(int.Parse(this.GetNodeSettingsValue("READOUT_TYPE")));
        }
        catch
        {
          return new int?();
        }
      }
      set => this.UpdateNodeSettingsValue("READOUT_TYPE", (object) value);
    }

    public string ReadoutDeviceType
    {
      get
      {
        string translatedLanguageText = StructureTreeNode.GetTranslatedLanguageText("ReadoutDeviceTypeID", this.ReadoutDeviceTypeID.ToString());
        return translatedLanguageText != null && translatedLanguageText != "ReadoutDeviceTypeID" ? translatedLanguageText : string.Empty;
      }
    }

    public bool IsLeaf => this.Children.Count == 0;

    public Image StateImage
    {
      get
      {
        return this.NodeErrors.Count > 0 ? (Image) Resources.AlertIcon : (Image) Resources.SuccessIconTransporent;
      }
    }

    public Image NodeImage
    {
      get
      {
        switch (this.NodeTyp)
        {
          case StructureNodeType.Unknown:
            return (Image) null;
          case StructureNodeType.Meter:
            return Images.pics.Device_16x16.Image;
          case StructureNodeType.MobileDevice:
            return Images.pics.BaseNodeMobileDevice_16x16.Image;
          case StructureNodeType.Country:
            return (Image) Resources.CountryNode;
          case StructureNodeType.City:
            return (Image) Resources.CityNode;
          case StructureNodeType.CityArea:
            return Images.pics.AddressTypeCityArea_16x16.Image;
          case StructureNodeType.Street:
            return (Image) Resources.StreetNode;
          case StructureNodeType.House:
          case StructureNodeType.ESatzNodeHaus:
            return Images.pics.AddressTypeHouse_16x16.Image;
          case StructureNodeType.Floor:
            return Images.pics.AddressTypeFloor_16x16.Image;
          case StructureNodeType.DeviceGroup:
            return (Image) Resources.DeviceGroup;
          case StructureNodeType.Flat:
            return (Image) Resources.FlatNode;
          case StructureNodeType.WaveFlowNode:
            return Images.pics.WaveFlowDevice_16x16.Image;
          case StructureNodeType.ESatzNodeLiegenschaft:
            return Images.pics.Liegenschaft_16x16.Image;
          case StructureNodeType.NodeUserGroupe:
            return Images.pics.Nutzergruppe_16x16.Image;
          case StructureNodeType.NodeUser:
            return Images.pics.Nutzer_16x16.Image;
          case StructureNodeType.ESatzNodeWohnung:
            return Images.pics.Wohnung_16x16.Image;
          case StructureNodeType.ESatzNodeRechenzentrum:
            return Images.pics.Rechenzentrum_16x16.Image;
          case StructureNodeType.BerechnungsNodeNutzungsgrad:
            return Images.pics.Nutzungsgrad_16x16.Image;
          case StructureNodeType.BerechnungsNodeSumIN:
            return Images.pics.Berechnung_Input_16x16.Image;
          case StructureNodeType.BerechnungsNodeSumOUT:
            return Images.pics.Berechnung_Output_16x16.Image;
          case StructureNodeType.Repeater:
            return (Image) Resources.RepeaterNode;
          case StructureNodeType.Manifold:
            return (Image) Resources.ManifoldNode;
          case StructureNodeType.COMserver:
            return (Image) Resources.COMserver;
          case StructureNodeType.Converter:
            return (Image) Resources.ConverterNode;
          default:
            throw new Exception("Unknown MeterInstaller node type detected! Name: " + this.NodeTyp.ToString());
        }
      }
    }

    public StructureTreeNodeList MeterReplacementHistoryList { get; set; }

    public string MeterReplacementHistoryListCount
    {
      get
      {
        return this.MeterReplacementHistoryList == null || this.MeterReplacementHistoryList.Count == 0 ? string.Empty : this.MeterReplacementHistoryList.Count.ToString();
      }
    }

    public override string ToString()
    {
      string str = string.Format("Name={0}, Serial={1}, NodeID={2}, Depth={3}, Children={4}, MeterID={5}, LayerID={6}, Order={7}, Typ={8}", (object) this.Name, (object) this.SerialNumber, (object) this.NodeID, (object) this.Depth, (object) this.Children.Count, (object) this.MeterID, (object) this.LayerID, (object) this.NodeOrder, (object) this.NodeTyp);
      if (this.Parent == null)
        str = str.Insert(0, "(Root) ");
      return str;
    }

    public List<StructureTreeNode> GetDownNodes(StructureNodeType type)
    {
      List<StructureTreeNode> downNodes = new List<StructureTreeNode>();
      foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(this, type))
        downNodes.Add(structureTreeNode);
      return downNodes;
    }

    public bool ContainsNode(long nodeId) => this.ContainsNode(nodeId, this.Children);

    private bool ContainsNode(long nodeId, StructureTreeNodeList children)
    {
      foreach (StructureTreeNode child in (List<StructureTreeNode>) children)
      {
        int? nodeId1 = child.NodeID;
        long? nullable = nodeId1.HasValue ? new long?((long) nodeId1.GetValueOrDefault()) : new long?();
        long num = nodeId;
        if (nullable.GetValueOrDefault() == num & nullable.HasValue || this.ContainsNode(nodeId, child.Children))
          return true;
      }
      return false;
    }

    public void UpdateNodeSettingsValue(string key, object value)
    {
      if (value == null || string.IsNullOrEmpty(key) || this.NodeSettings == null)
        return;
      this.NodeSettings = ParameterService.AddOrUpdateParameter(this.NodeSettings, key, value.ToString());
    }

    public string GetNodeSettingsValue(string key)
    {
      return string.IsNullOrEmpty(key) || string.IsNullOrEmpty(this.NodeSettings) ? string.Empty : ParameterService.GetParameter(this.NodeSettings, key);
    }

    private void SetNodeSettingsValue(string key, string value)
    {
      if (string.IsNullOrEmpty(key))
        return;
      this.NodeSettings = ParameterService.AddOrUpdateParameter(this.NodeSettings, key, value);
    }

    public static IEnumerable<StructureTreeNode> ForEachChild(StructureTreeNode root)
    {
      if (root != null)
      {
        foreach (StructureTreeNode node in (List<StructureTreeNode>) root.Children)
        {
          yield return node;
          foreach (StructureTreeNode child in StructureTreeNode.ForEachChild(node))
            yield return child;
        }
      }
    }

    public static IEnumerable<StructureTreeNode> ForEach(StructureTreeNode root)
    {
      if (root != null)
      {
        yield return root;
        foreach (StructureTreeNode child in StructureTreeNode.ForEachChild(root))
          yield return child;
      }
    }

    public static IEnumerable<StructureTreeNode> ForEach(
      StructureTreeNode root,
      StructureNodeType type)
    {
      foreach (StructureTreeNode node in StructureTreeNode.ForEach(root))
      {
        if (node.NodeTyp == type)
          yield return node;
      }
    }

    public static StructureNodeType TryParseNodeType(int nodeTypeID)
    {
      return (StructureNodeType) Enum.ToObject(typeof (StructureNodeType), nodeTypeID);
    }

    public static StructureTreeNodeList GetTemplateNodes(NodeLayer layer)
    {
      if (layer.IsPhysicalLayer)
      {
        StructureTreeNodeList list = new StructureTreeNodeList();
        StructureTreeNode.AddNode(list, "SettingsNode");
        StructureTreeNode.AddNode(list[0].Children, StructureNodeType.Meter);
        StructureTreeNode.AddNode(list, "GenaralNodes");
        StructureTreeNodeList children = list[1].Children;
        StructureTreeNode.AddNode(children, StructureNodeType.Country);
        StructureTreeNode.AddNode(children, StructureNodeType.City);
        StructureTreeNode.AddNode(children, StructureNodeType.CityArea);
        StructureTreeNode.AddNode(children, StructureNodeType.Street);
        StructureTreeNode.AddNode(children, StructureNodeType.House);
        StructureTreeNode.AddNode(children, StructureNodeType.Floor);
        StructureTreeNode.AddNode(children, StructureNodeType.DeviceGroup);
        StructureTreeNode.AddNode(children, StructureNodeType.Flat);
        StructureTreeNode.AddNode(children, StructureNodeType.NodeUser);
        StructureTreeNode.AddNode(children, StructureNodeType.NodeUserGroupe);
        StructureTreeNode.AddNode(children, StructureNodeType.COMserver);
        StructureTreeNode.AddNode(children, StructureNodeType.Converter);
        StructureTreeNode.AddNode(children, StructureNodeType.Manifold);
        StructureTreeNode.AddNode(children, StructureNodeType.Repeater);
        return list;
      }
      StructureTreeNodeList list1 = new StructureTreeNodeList();
      StructureTreeNode.AddNode(list1, "GenaralNodes");
      StructureTreeNodeList children1 = list1[0].Children;
      StructureTreeNode.AddNode(children1, StructureNodeType.Country);
      StructureTreeNode.AddNode(children1, StructureNodeType.City);
      StructureTreeNode.AddNode(children1, StructureNodeType.CityArea);
      StructureTreeNode.AddNode(children1, StructureNodeType.Street);
      StructureTreeNode.AddNode(children1, StructureNodeType.House);
      StructureTreeNode.AddNode(children1, StructureNodeType.Floor);
      StructureTreeNode.AddNode(children1, StructureNodeType.DeviceGroup);
      StructureTreeNode.AddNode(children1, StructureNodeType.Flat);
      StructureTreeNode.AddNode(children1, StructureNodeType.NodeUser);
      StructureTreeNode.AddNode(children1, StructureNodeType.NodeUserGroupe);
      StructureTreeNode.AddNode(list1, "DeviceNodes");
      list1[1].Children.AddRange((IEnumerable<StructureTreeNode>) MeterDatabase.LoadMeterInstallerTreesByLayerID(0));
      return list1;
    }

    private static StructureTreeNode AddNode(StructureTreeNodeList list, string name)
    {
      return list.Add(new StructureTreeNode()
      {
        Name = StructureTreeNode.GetTranslatedLanguageText("MeterInstaller", name),
        NodeOrder = list.Count
      });
    }

    private static StructureTreeNode AddNode(StructureTreeNodeList list, StructureNodeType t)
    {
      return list.Add(new StructureTreeNode()
      {
        Name = StructureTreeNode.GetTranslatedLanguageText("MeterInstaller", t.ToString()),
        NodeTyp = t,
        NodeOrder = list.Count
      });
    }

    public static string GenerateDeviceInfo(string zdf)
    {
      if (string.IsNullOrEmpty(zdf) || string.IsNullOrEmpty(ParameterService.GetParameter(zdf, "SID")))
        return string.Empty;
      string parameter1 = ParameterService.GetParameter(zdf, "RADR");
      if (!string.IsNullOrEmpty(parameter1))
        return string.Format("RADR: {0,2}", (object) parameter1);
      string parameter2 = ParameterService.GetParameter(zdf, "RSSI_dBm");
      return !string.IsNullOrEmpty(parameter2) ? string.Format("{0,4} dBm", (object) parameter2) : string.Empty;
    }

    public bool SaveToXML(string fileName)
    {
      XmlDocument xml = new XmlDocument();
      XmlComment comment = xml.CreateComment("Global Meter Manager www.zenner.de " + DateTime.Now.ToString("u"));
      XmlNode refChild = xml.AppendChild((XmlNode) xml.CreateElement("L" + this.LayerID.ToString()));
      xml.InsertBefore((XmlNode) comment, refChild);
      XmlNode xmlNode = xml.DocumentElement.AppendChild(this.CreateXmlNode(xml, this));
      this.CreateXML(xml, xmlNode, this);
      xml.Save(fileName);
      return true;
    }

    private void CreateXML(XmlDocument xml, XmlNode xmlNode, StructureTreeNode node)
    {
      foreach (StructureTreeNode child in (List<StructureTreeNode>) node.Children)
      {
        XmlNode xmlNode1 = this.CreateXmlNode(xml, child);
        xmlNode?.AppendChild(xmlNode1);
        if (child.Children.Count > 0)
          this.CreateXML(xml, xmlNode1, child);
      }
    }

    private XmlNode CreateXmlNode(XmlDocument xml, StructureTreeNode node)
    {
      XmlElement element = xml.CreateElement(node.NodeTyp.ToString());
      element.Attributes.Append(xml.CreateAttribute("N"));
      element.Attributes["N"].Value = node.Name;
      element.Attributes.Append(xml.CreateAttribute("S"));
      element.Attributes["S"].Value = node.NodeSettings;
      return (XmlNode) element;
    }

    public static StructureTreeNode LoadStructureFromXML(string fileName)
    {
      XmlDocument xml = new XmlDocument();
      xml.Load(fileName);
      string name = xml.DocumentElement.Name;
      if (name.StartsWith("L"))
        return StructureTreeNode.CreateStructure(xml);
      return name == "ROOT" ? StructureTreeNode.CreateOldStructure(xml) : (StructureTreeNode) null;
    }

    private static StructureTreeNode CreateStructure(XmlDocument xml)
    {
      int int32 = Convert.ToInt32(xml.DocumentElement.Name.Substring(1));
      StructureTreeNode parent = new StructureTreeNode();
      parent.LayerID = new int?(int32);
      StructureTreeNode.CreateStructure(parent, xml.ChildNodes[1].ChildNodes);
      return parent;
    }

    private static void CreateStructure(StructureTreeNode parent, XmlNodeList xmlNodeList)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        string name = xmlNode.Name;
        string str1 = xmlNode.Attributes["N"].Value;
        string str2 = xmlNode.Attributes["S"].Value;
        if (Enum.IsDefined(typeof (StructureNodeType), (object) name))
        {
          StructureTreeNode structureTreeNode = new StructureTreeNode();
          structureTreeNode.Name = str1;
          structureTreeNode.NodeTyp = (StructureNodeType) Enum.Parse(typeof (StructureNodeType), name, true);
          structureTreeNode.NodeSettings = str2;
          structureTreeNode.LayerID = parent.LayerID;
          structureTreeNode.Parent = parent;
          structureTreeNode.NodeOrder = parent.Children.Count + 1;
          parent.Children.Add(structureTreeNode);
          StructureTreeNode.CreateStructure(structureTreeNode, xmlNode.ChildNodes);
        }
      }
    }

    private static StructureTreeNode CreateOldStructure(XmlDocument xml)
    {
      StructureTreeNode parent = new StructureTreeNode();
      parent.LayerID = new int?(0);
      StructureTreeNode.CreateOldStructure(parent, xml.ChildNodes[0].ChildNodes);
      return parent;
    }

    private static void CreateOldStructure(StructureTreeNode parent, XmlNodeList xmlNodeList)
    {
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        StructureNodeType structureNodeType = StructureTreeNode.GetNodeType(xmlNode.Name);
        string str1 = xmlNode.Attributes["NodeName"].Value;
        string str2 = xmlNode.Attributes["ParameterList"].Value;
        string str3 = xmlNode.Attributes["ReadingType"].Value;
        string str4 = str2.Replace("VALUE_REQ_ID;1;", "");
        if (structureNodeType != StructureNodeType.Unknown)
        {
          if (xmlNode.Name == "GroupNode")
          {
            if (str4.IndexOf("Type") == -1)
              structureNodeType = StructureNodeType.Country;
            else if (str4.IndexOf(DeviceCollectorSettings.BusMode.ToString()) == -1 && (str3 == "MBus" || str3 == "MBusCom"))
              str4 += ";BusMode;MBus;FromTime;01.01.2000 01:01:01;ToTime;01.01.2030 01:01:01;MaxRequestRepeat;2;ScanStartAddress=;0;ScanStartSerialnumber;fffffff0;OrganizeStartAddress;1;CycleTime;5;OnlySecondaryAddressing;True;FastSecondaryAddressing;False;BeepSignalOnReadResult;False;LogToFileEnabled;False;LogFilePath;";
          }
          StructureTreeNode structureTreeNode = new StructureTreeNode();
          structureTreeNode.Name = str1;
          structureTreeNode.NodeTyp = structureNodeType;
          structureTreeNode.NodeSettings = str4;
          structureTreeNode.LayerID = parent.LayerID;
          structureTreeNode.Parent = parent;
          structureTreeNode.NodeOrder = parent.Children.Count + 1;
          parent.Children.Add(structureTreeNode);
          StructureTreeNode.CreateOldStructure(structureTreeNode, xmlNode.ChildNodes);
        }
      }
    }

    private static StructureNodeType GetNodeType(string oldNameOfNodeType)
    {
      string str = oldNameOfNodeType;
      if (str != null)
      {
        switch (str.Length)
        {
          case 9:
            switch (str[0])
            {
              case 'G':
                if (str == "GroupNode")
                  return StructureNodeType.Country;
                break;
              case 'M':
                if (str == "MeterNode")
                  return StructureNodeType.Meter;
                break;
            }
            break;
          case 12:
            if (str == "WaveFlowNode")
              return StructureNodeType.WaveFlowNode;
            break;
          case 13:
            if (str == "ESatzNodeHaus")
              return StructureNodeType.House;
            break;
          case 15:
            switch (str[0])
            {
              case 'A':
                if (str == "AddressNodeCity")
                  return StructureNodeType.City;
                break;
              case 'E':
                if (str == "ESatzNodeNutzer")
                  return StructureNodeType.NodeUser;
                break;
            }
            break;
          case 16:
            switch (str[11])
            {
              case 'F':
                if (str == "AddressNodeFloor")
                  return StructureNodeType.Floor;
                break;
              case 'H':
                if (str == "AddressNodeHouse")
                  return StructureNodeType.House;
                break;
              case 'h':
                if (str == "ESatzNodeWohnung")
                  return StructureNodeType.Flat;
                break;
            }
            break;
          case 17:
            if (str == "AddressNodeStreet")
              return StructureNodeType.Street;
            break;
          case 18:
            if (str == "AddressNodeCountry")
              return StructureNodeType.Country;
            break;
          case 19:
            switch (str[12])
            {
              case 'i':
                if (str == "AddressNodeCityArea")
                  return StructureNodeType.CityArea;
                break;
              case 'o':
                if (str == "AddressNodeCorridor")
                  return StructureNodeType.DeviceGroup;
                break;
            }
            break;
          case 20:
            switch (str[0])
            {
              case 'B':
                if (str == "BerechnungsNodeSumIN")
                  return StructureNodeType.BerechnungsNodeSumIN;
                break;
              case 'M':
                if (str == "MobileDeviceBaseNode")
                  return StructureNodeType.MobileDevice;
                break;
            }
            break;
          case 21:
            switch (str[9])
            {
              case 'L':
                if (str == "ESatzNodeLiegenschaft")
                  return StructureNodeType.ESatzNodeLiegenschaft;
                break;
              case 'N':
                if (str == "ESatzNodeNutzergruppe")
                  return StructureNodeType.NodeUserGroupe;
                break;
              case 'd':
                if (str == "AddressNodeRoomNumber")
                  return StructureNodeType.NodeUser;
                break;
              case 'g':
                if (str == "BerechnungsNodeSumOUT")
                  return StructureNodeType.BerechnungsNodeSumOUT;
                break;
            }
            break;
          case 22:
            if (str == "ESatzNodeRechenzentrum")
              return StructureNodeType.ESatzNodeRechenzentrum;
            break;
          case 27:
            if (str == "BerechnungsNodeWirkungsgrad")
              return StructureNodeType.BerechnungsNodeNutzungsgrad;
            break;
        }
      }
      return StructureNodeType.Unknown;
    }

    public string FindNodeName(int meterID)
    {
      foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(this))
      {
        int? meterId = structureTreeNode.MeterID;
        int num1;
        if (meterId.HasValue)
        {
          meterId = structureTreeNode.MeterID;
          int num2 = meterID;
          num1 = meterId.GetValueOrDefault() == num2 & meterId.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
          return structureTreeNode.Name;
      }
      return string.Empty;
    }

    public List<StructureTreeNode> FindBySerialNumber(string serialNumber)
    {
      List<StructureTreeNode> bySerialNumber = new List<StructureTreeNode>();
      foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(this))
      {
        if (structureTreeNode.SerialNumber == serialNumber)
          bySerialNumber.Add(structureTreeNode);
      }
      return bySerialNumber;
    }

    public string FindSerialnumber(int meterID)
    {
      foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(this))
      {
        int? meterId = structureTreeNode.MeterID;
        int num1;
        if (meterId.HasValue)
        {
          meterId = structureTreeNode.MeterID;
          int num2 = meterID;
          num1 = meterId.GetValueOrDefault() == num2 & meterId.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
        if (num1 != 0)
          return structureTreeNode.SerialNumber;
      }
      return string.Empty;
    }

    public void UpdateNodeSettingsValues(string newNodeSettings)
    {
      if (this.SubDeviceIndex > 0)
        return;
      List<string> keys = ParameterService.GetKeys(newNodeSettings);
      if (keys == null)
        return;
      foreach (string str in keys)
      {
        string parameter = ParameterService.GetParameter(newNodeSettings, str);
        this.UpdateNodeSettingsValue(str, (object) parameter);
      }
    }

    public void UpdateChildren(StructureTreeNodeList unknownChildren)
    {
      if (unknownChildren == null || unknownChildren.Count == 0)
        return;
      foreach (StructureTreeNode unknownChild in (List<StructureTreeNode>) unknownChildren)
      {
        bool flag = false;
        foreach (StructureTreeNode child in (List<StructureTreeNode>) this.Children)
        {
          if (child.SubDeviceIndex == unknownChild.SubDeviceIndex)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          unknownChild.Parent = this;
          this.Children.Add(unknownChild);
        }
      }
    }

    private static string GetTranslatedLanguageText(string GmmModule, string TextKey)
    {
      string str = GmmModule + TextKey;
      return Ot.Gtt(Tg.Common, str, str);
    }
  }
}
