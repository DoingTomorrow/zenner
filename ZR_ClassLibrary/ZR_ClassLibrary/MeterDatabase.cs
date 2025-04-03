// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterDatabase
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class MeterDatabase : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeterDatabase));
    private IDataReader meterValuesReader;
    private StructureTreeNode nodeValuesReader;
    public const int PHYSICAL_LAYER = 0;
    private static readonly object saveMeterValueLocker = new object();
    private Queue<int> notHandledMeterIDs;
    private DateTime startTimepointOfMeterValues;
    private DateTime endTimepointOfMeterValues;
    private IDbCommand cmdOfMeterValues;
    private const int MAX_ITEM_PER_QUERY = 400;
    private Dictionary<int, DateTime> lastTimepointOfLoadedMeasurementValues;
    private ValueFilter valueFilterAfterLoad;
    public List<long> FilterValueIdentOfMeterValues;

    public long CountOfValues { get; private set; }

    public long CountOfLoadedValues { get; private set; }

    public event EventHandler<MeterDatabase.Progress> OnProgress;

    public static NodeLayer GetLayer(int layerID)
    {
      List<NodeLayer> nodeLayerList = MeterDatabase.LoadNodeLayer();
      if (nodeLayerList == null)
        return (NodeLayer) null;
      foreach (NodeLayer layer in nodeLayerList)
      {
        if (layer.LayerID == layerID)
          return layer;
      }
      return (NodeLayer) null;
    }

    public static List<NodeLayer> LoadNodeLayer() => MeterDatabase.LoadNodeLayer(DbBasis.PrimaryDB);

    public static List<NodeLayer> LoadNodeLayer(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<NodeLayer> nodeLayerList = new List<NodeLayer>();
      nodeLayerList.AddRange((IEnumerable<NodeLayer>) NodeLayer.GetStaticLayers());
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM NodeLayers ORDER BY LayerID ASC;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return nodeLayerList;
          while (dataReader.Read())
            nodeLayerList.Add(new NodeLayer()
            {
              LayerID = Convert.ToInt32(dataReader["LayerID"]),
              Name = Convert.ToString(dataReader["Name"]),
              Description = Convert.ToString(dataReader["Description"])
            });
          return nodeLayerList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<NodeLayer>) null;
      }
    }

    public static List<int> LoadMeterInstallerRootNodes(int layerID)
    {
      return MeterDatabase.LoadMeterInstallerRootNodes(DbBasis.PrimaryDB, layerID);
    }

    public static List<int> LoadMeterInstallerRootNodes(DbBasis db, int layerID)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT NodeID FROM NodeReferences WHERE ParentID = 0 AND LayerID = @LayerID;";
          IDbDataParameter parameter = cmd.CreateParameter();
          parameter.DbType = DbType.Int32;
          parameter.ParameterName = "@LayerID";
          parameter.Value = (object) layerID;
          cmd.Parameters.Add((object) parameter);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<int>) null;
          List<int> intList = new List<int>();
          while (dataReader.Read())
            intList.Add(Convert.ToInt32(dataReader["NodeID"]));
          return intList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<int>) null;
      }
    }

    public static List<StructureTreeNode> LoadMeterInstallerTreesByLayerID(int layerID)
    {
      return MeterDatabase.LoadMeterInstallerTreesByLayerID(DbBasis.PrimaryDB, layerID);
    }

    public static List<StructureTreeNode> LoadMeterInstallerTreesByLayerID(DbBasis db, int layerID)
    {
      List<int> intList = db != null ? MeterDatabase.LoadMeterInstallerRootNodes(db, layerID) : throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (intList == null)
        return (List<StructureTreeNode>) null;
      StructureTreeNodeList structureTreeNodeList = new StructureTreeNodeList();
      foreach (int startNodeID in intList)
      {
        StructureTreeNode node = MeterDatabase.LoadMeterInstallerTree(db, startNodeID, new int?(layerID));
        if (node != null)
          structureTreeNodeList.Add(node);
      }
      return (List<StructureTreeNode>) structureTreeNodeList;
    }

    public static StructureTreeNode LoadMeterInstallerTree(int startNodeID)
    {
      return MeterDatabase.LoadMeterInstallerTree(startNodeID, new int?());
    }

    public static StructureTreeNode LoadMeterInstallerTree(int startNodeID, int? layerID)
    {
      return MeterDatabase.LoadMeterInstallerTree(DbBasis.PrimaryDB, startNodeID, layerID);
    }

    public static StructureTreeNode LoadMeterInstallerTree(
      DbBasis db,
      int startNodeID,
      int? layerID)
    {
      List<NodeReferences> nodeReferences1 = db != null ? MeterDatabase.LoadNodeReferences(db) : throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<NodeList> nodeList1 = MeterDatabase.LoadNodeList(db);
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          cmd = db.DbCommand(dbConnection);
          NodeReferences nodeReferences2 = nodeReferences1.Find((Predicate<NodeReferences>) (e => e.NodeID == startNodeID));
          if (nodeReferences2 == null)
            return (StructureTreeNode) null;
          StructureTreeNode structureTreeNode1 = new StructureTreeNode();
          structureTreeNode1.NodeID = new int?(startNodeID);
          structureTreeNode1.NodeOrder = nodeReferences2.NodeOrder;
          structureTreeNode1.LayerID = new int?(nodeReferences2.LayerID);
          MeterDatabase.AddChildNodes(structureTreeNode1, nodeReferences1);
          foreach (StructureTreeNode structureTreeNode2 in StructureTreeNode.ForEach(structureTreeNode1))
          {
            StructureTreeNode node = structureTreeNode2;
            NodeList nodeList2 = nodeList1.Find((Predicate<NodeList>) (e =>
            {
              int nodeId1 = e.NodeID;
              int? nodeId2 = node.NodeID;
              int valueOrDefault = nodeId2.GetValueOrDefault();
              return nodeId1 == valueOrDefault & nodeId2.HasValue && !e.ValidTo.HasValue;
            }));
            if (nodeList2 == null)
              throw new ArgumentException("MeterInstaller structure in database is corrupt! Invalid table: NodeList. Node reference is available, but not exist the node self!");
            if (!nodeList2.ValidFrom.HasValue)
              throw new ArgumentException("MeterInstaller structure in database is corrupt! Invalid table: NodeList. ValidFrom is missing!");
            StructureNodeType nodeType = StructureTreeNode.TryParseNodeType(nodeList2.NodeTypeID);
            node.ValidFrom = nodeList2.ValidFrom.Value;
            node.ValidTo = nodeList2.ValidTo;
            node.MeterID = nodeList2.MeterID == 0 ? new int?() : new int?(nodeList2.MeterID);
            node.Name = nodeList2.NodeName;
            node.NodeAdditionalInfos = nodeList2.NodeAdditionalInfos;
            node.NodeSettings = nodeList2.NodeSettings;
            node.NodeTyp = nodeType;
            node.NodeDescription = nodeList2.NodeDescription;
            node.LayerID = structureTreeNode1.LayerID;
            MeterDatabase.TryAddReplacedMeter(node, nodeList1);
          }
          return structureTreeNode1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (StructureTreeNode) null;
      }
    }

    private static void TryAddReplacedMeter(StructureTreeNode node, List<NodeList> nodeList)
    {
      if (node == null || node.NodeTyp != StructureNodeType.Meter)
        return;
      List<NodeList> all = nodeList.FindAll((Predicate<NodeList>) (e =>
      {
        int nodeId1 = e.NodeID;
        int? nodeId2 = node.NodeID;
        int valueOrDefault1 = nodeId2.GetValueOrDefault();
        if (nodeId1 == valueOrDefault1 & nodeId2.HasValue)
        {
          int meterId1 = e.MeterID;
          int? meterId2 = node.MeterID;
          int valueOrDefault2 = meterId2.GetValueOrDefault();
          if (!(meterId1 == valueOrDefault2 & meterId2.HasValue))
            return e.ValidTo.HasValue;
        }
        return false;
      }));
      if (all == null)
        return;
      foreach (NodeList nodeList1 in all)
        node.MeterReplacementHistoryList.Add(new StructureTreeNode()
        {
          MeterID = new int?(nodeList1.MeterID),
          Name = nodeList1.NodeName,
          NodeAdditionalInfos = nodeList1.NodeAdditionalInfos,
          NodeSettings = nodeList1.NodeSettings,
          NodeTyp = node.NodeTyp,
          NodeDescription = nodeList1.NodeDescription,
          LayerID = node.LayerID,
          ValidFrom = nodeList1.ValidFrom.Value,
          ValidTo = nodeList1.ValidTo
        });
    }

    private static void AddChildNodes(StructureTreeNode parent, List<NodeReferences> nodeReferences)
    {
      List<NodeReferences> all = nodeReferences.FindAll((Predicate<NodeReferences>) (e =>
      {
        int parentId = e.ParentID;
        int? nodeId = parent.NodeID;
        int valueOrDefault1 = nodeId.GetValueOrDefault();
        if (!(parentId == valueOrDefault1 & nodeId.HasValue))
          return false;
        int layerId1 = e.LayerID;
        int? layerId2 = parent.LayerID;
        int valueOrDefault2 = layerId2.GetValueOrDefault();
        return layerId1 == valueOrDefault2 & layerId2.HasValue;
      }));
      if (all == null)
        return;
      foreach (NodeReferences nodeReferences1 in all)
      {
        StructureTreeNode structureTreeNode = new StructureTreeNode(parent);
        structureTreeNode.NodeID = new int?(nodeReferences1.NodeID);
        structureTreeNode.LayerID = new int?(nodeReferences1.LayerID);
        structureTreeNode.NodeOrder = nodeReferences1.NodeOrder;
        parent.Children.Add(structureTreeNode);
        MeterDatabase.AddChildNodes(structureTreeNode, nodeReferences);
      }
    }

    public static bool ValidateMeterInstallerTreeStructure(StructureTreeNode tree)
    {
      return MeterDatabase.ValidateMeterInstallerTreeStructure(DbBasis.PrimaryDB, tree);
    }

    public static bool ValidateMeterInstallerTreeStructure(DbBasis db, StructureTreeNode tree)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (tree == null)
        throw new ArgumentException(nameof (tree));
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          return MeterDatabase.ValidateMeterInstallerTreeStructure(cmd, tree);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    private static bool ValidateMeterInstallerTreeStructure(IDbCommand cmd, StructureTreeNode tree)
    {
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      if (tree == null)
        throw new ArgumentException(nameof (tree));
      bool flag = true;
      SortedList<string, StructureTreeNode> sortedList = new SortedList<string, StructureTreeNode>();
      foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(tree))
      {
        if (structureTreeNode.NodeTyp != StructureNodeType.Unknown)
        {
          structureTreeNode.NodeErrors.Clear();
          if (string.IsNullOrEmpty(structureTreeNode.Name))
          {
            structureTreeNode.NodeErrors.Add(Ot.GetTranslatedLanguageText(TranslatorKey.MeterInstallerMissingNodeName));
            flag = false;
          }
          if (structureTreeNode.NodeTyp == StructureNodeType.Meter && !structureTreeNode.MeterID.HasValue)
          {
            if (structureTreeNode.Parent != null && structureTreeNode.Parent.NodeTyp == StructureNodeType.Meter && structureTreeNode.Parent.NodeErrors.Count > 0)
            {
              structureTreeNode.NodeErrors.Add(Ot.GetTranslatedLanguageText("MeterInstallerInvalidParentNode"));
              flag = false;
            }
            if (string.IsNullOrEmpty(structureTreeNode.SerialNumber))
            {
              if (structureTreeNode.ReadEnabled)
              {
                structureTreeNode.NodeErrors.Add(Ot.GetTranslatedLanguageText(TranslatorKey.MeterInstallerMissingSerialnumber));
                flag = false;
              }
            }
            else if (MeterDatabase.ExistStructureForMeter(cmd, structureTreeNode.SerialNumber))
            {
              structureTreeNode.NodeErrors.Add(Ot.GetTranslatedLanguageText(TranslatorKey.MeterInstallerMeterAlreadyExists));
              flag = false;
            }
            else if (sortedList.ContainsKey(structureTreeNode.SerialNumber))
            {
              structureTreeNode.NodeErrors.Add(Ot.GetTranslatedLanguageText(TranslatorKey.MeterInstallerMeterAlreadyExists));
              flag = false;
            }
            else
              sortedList.Add(structureTreeNode.SerialNumber, structureTreeNode);
          }
        }
      }
      if (flag)
      {
        foreach (StructureTreeNode root in StructureTreeNode.ForEach(tree))
        {
          if (root.NodeTyp != StructureNodeType.Unknown && root.NodeErrors.Count <= 0)
          {
            foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEachChild(root))
            {
              if (structureTreeNode.NodeErrors.Count > 0)
              {
                root.NodeErrors.Add(Ot.GetTranslatedLanguageText(TranslatorKey.MeterInstallerInvalidSubNodes));
                flag = false;
                break;
              }
            }
          }
        }
      }
      return flag;
    }

    public static bool SaveTreeNode(StructureTreeNode tree)
    {
      return MeterDatabase.SaveTreeNode(DbBasis.PrimaryDB, tree);
    }

    public static bool SaveTreeNode(DbBasis db, StructureTreeNode tree)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (tree == null)
        throw new ArgumentNullException(nameof (tree));
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          int? nextUniqueId1 = MeterDatabase.GetNextUniqueID(cmd, "Meter", "MeterID");
          if (!nextUniqueId1.HasValue)
            return false;
          int? nextUniqueId2 = MeterDatabase.GetNextUniqueID(cmd, "NodeList", "NodeID");
          if (!nextUniqueId2.HasValue)
            return false;
          int num1 = nextUniqueId1.Value;
          int num2 = nextUniqueId2.Value;
          foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(tree))
          {
            if (structureTreeNode.NodeTyp != StructureNodeType.Unknown)
            {
              int? nullable;
              int num3;
              if (structureTreeNode.NodeTyp == StructureNodeType.Meter)
              {
                nullable = structureTreeNode.LayerID;
                int num4 = 0;
                num3 = nullable.GetValueOrDefault() == num4 & nullable.HasValue ? 1 : 0;
              }
              else
                num3 = 0;
              if (num3 != 0)
              {
                if (!string.IsNullOrEmpty(structureTreeNode.SerialNumber) && MeterDatabase.ValidateMeterInstallerTreeStructure(cmd, structureTreeNode))
                {
                  nullable = structureTreeNode.MeterID;
                  if (!nullable.HasValue)
                  {
                    if (!MeterDatabase.AddMeter(cmd, structureTreeNode, num1))
                      return false;
                    ++num1;
                  }
                  else if (!MeterDatabase.UpdateMeter(cmd, structureTreeNode))
                    return false;
                }
                else
                  structureTreeNode.ReadEnabled = false;
              }
              nullable = structureTreeNode.NodeID;
              if (nullable.HasValue)
              {
                if (!MeterDatabase.UpdateTreeNode(cmd, structureTreeNode))
                  return false;
              }
              else if (structureTreeNode.NodeTyp != StructureNodeType.Meter)
              {
                int num5;
                if (structureTreeNode.Parent != null)
                {
                  nullable = structureTreeNode.Parent.NodeID;
                  num5 = !nullable.HasValue ? 1 : 0;
                }
                else
                  num5 = 0;
                if (num5 == 0)
                {
                  if (!MeterDatabase.AddNode(cmd, structureTreeNode, num2))
                    return false;
                  ++num2;
                }
              }
              else if (MeterDatabase.ValidateMeterInstallerTreeStructure(cmd, structureTreeNode))
              {
                if (!MeterDatabase.AddNode(cmd, structureTreeNode, num2))
                  return false;
                ++num2;
              }
            }
          }
          int num6 = num1;
          int? nullable1 = nextUniqueId1;
          int valueOrDefault1 = nullable1.GetValueOrDefault();
          if (num6 > valueOrDefault1 & nullable1.HasValue && !MeterDatabase.SetNextUniqueID(cmd, "Meter", "MeterID", num1))
            return false;
          int num7 = num2;
          nullable1 = nextUniqueId2;
          int valueOrDefault2 = nullable1.GetValueOrDefault();
          if (num7 > valueOrDefault2 & nullable1.HasValue && !MeterDatabase.SetNextUniqueID(cmd, "NodeList", "NodeID", num2))
            return false;
          cmd.Transaction.Commit();
          foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(tree))
            structureTreeNode.OldParent = (StructureTreeNode) null;
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static List<GMM_User> LoadGMM_User() => MeterDatabase.LoadGMM_User(DbBasis.PrimaryDB);

    public static List<GMM_User> LoadGMM_User(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM GMM_User;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<GMM_User>) null;
          List<GMM_User> gmmUserList = new List<GMM_User>();
          while (dataReader.Read())
            gmmUserList.Add(new GMM_User()
            {
              UserName = dataReader["UserName"].ToString(),
              UserPersonalNumber = Convert.ToInt32(dataReader["UserPersonalNumber"]),
              UserRights = dataReader["UserRights"].ToString(),
              UserKey = dataReader["UserKey"].ToString(),
              ChangedUserRights = dataReader["ChangedUserRights"].ToString(),
              ChangedUserKey = dataReader["ChangedUserKey"].ToString()
            });
          return gmmUserList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<GMM_User>) null;
      }
    }

    public static GMM_User AddGMM_User(DbBasis db, GMM_User user)
    {
      return MeterDatabase.AddGMM_User(db, user.UserName, user.UserPersonalNumber, user.UserRights, user.UserKey, user.ChangedUserRights, user.ChangedUserKey);
    }

    public static GMM_User AddGMM_User(
      string userName,
      int userPersonalNumber,
      string userRights,
      string userKey,
      string changedUserRights,
      string changedUserKey)
    {
      return MeterDatabase.AddGMM_User(DbBasis.PrimaryDB, userName, userPersonalNumber, userRights, userKey, changedUserRights, changedUserKey);
    }

    public static GMM_User AddGMM_User(
      DbBasis db,
      string userName,
      int userPersonalNumber,
      string userRights,
      string userKey,
      string changedUserRights,
      string changedUserKey)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (string.IsNullOrEmpty(userName))
        throw new ArgumentNullException("The name of GMM_User can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO GMM_User (UserName, UserPersonalNumber, UserRights, UserKey, ChangedUserRights, ChangedUserKey) VALUES (@UserName, @UserPersonalNumber, @UserRights, @UserKey, @ChangedUserRights, @ChangedUserKey)";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@UserName", userName);
          MeterDatabase.AddParameter(cmd, "@UserPersonalNumber", userPersonalNumber);
          MeterDatabase.AddParameter(cmd, "@UserRights", userRights);
          MeterDatabase.AddParameter(cmd, "@UserKey", userKey);
          MeterDatabase.AddParameter(cmd, "@ChangedUserRights", changedUserRights);
          MeterDatabase.AddParameter(cmd, "@ChangedUserKey", changedUserKey);
          if (cmd.ExecuteNonQuery() != 1)
            return (GMM_User) null;
          return new GMM_User()
          {
            UserName = userName,
            UserPersonalNumber = userPersonalNumber,
            UserRights = userRights,
            UserKey = userKey,
            ChangedUserRights = changedUserRights,
            ChangedUserKey = changedUserKey
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (GMM_User) null;
      }
    }

    public static List<NodeReferences> LoadNodeReferences()
    {
      return MeterDatabase.LoadNodeReferences(DbBasis.PrimaryDB);
    }

    public static List<NodeReferences> LoadNodeReferences(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM NodeReferences ORDER BY NodeOrder;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<NodeReferences>) null;
          List<NodeReferences> nodeReferencesList = new List<NodeReferences>();
          while (dataReader.Read())
          {
            int num1 = 0;
            if (dataReader["NodeOrder"] != DBNull.Value)
              num1 = Convert.ToInt32(dataReader["NodeOrder"]);
            int num2 = 0;
            if (dataReader["LayerID"] != DBNull.Value)
              num2 = Convert.ToInt32(dataReader["LayerID"]);
            nodeReferencesList.Add(new NodeReferences()
            {
              NodeID = Convert.ToInt32(dataReader["NodeID"]),
              ParentID = Convert.ToInt32(dataReader["ParentID"]),
              LayerID = num2,
              NodeOrder = num1
            });
          }
          return nodeReferencesList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<NodeReferences>) null;
      }
    }

    public static MeterInfo GetMeterInfo(uint meterInfoID)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterInfo WHERE MeterInfoID=@MeterInfoID;";
          MeterDatabase.AddParameter(cmd, "@MeterInfoID", (double) meterInfoID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (MeterInfo) null;
          MeterInfo meterInfo = new MeterInfo();
          meterInfo.Description = dataReader["Description"].ToString();
          meterInfo.HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]);
          meterInfo.MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]);
          meterInfo.MeterHardwareID = Convert.ToInt32(dataReader["MeterHardwareID"]);
          meterInfo.MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]);
          meterInfo.PPSArtikelNr = dataReader["PPSArtikelNr"].ToString();
          if (dataReader.Read())
            throw new Exception("INTERNAL ERROR: The function become more as one result from database. SQL: " + cmd.CommandText);
          return meterInfo;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterInfo) null;
      }
    }

    public static List<MeterInfo> LoadMeterInfo(string sapMaterialNumber, string hardwareName)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT mi.Description, mi.HardwareTypeID, mi.MeterInfoID, mi.MeterHardwareID, mi.MeterTypeID, mi.PPSArtikelNr FROM MeterInfo AS mi, HardwareType AS ht WHERE mi.PPSArtikelNr=@PPSArtikelNr AND mi.HardwareTypeID = ht.HardwareTypeID AND ht.HardwareName = @HardwareName;";
          MeterDatabase.AddParameter(cmd, "@PPSArtikelNr", sapMaterialNumber);
          MeterDatabase.AddParameter(cmd, "@HardwareName", hardwareName);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterInfo>) null;
          List<MeterInfo> meterInfoList = new List<MeterInfo>();
          while (dataReader.Read())
            meterInfoList.Add(new MeterInfo()
            {
              Description = dataReader["Description"].ToString(),
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]),
              MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]),
              MeterHardwareID = Convert.ToInt32(dataReader["MeterHardwareID"]),
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              PPSArtikelNr = dataReader["PPSArtikelNr"].ToString()
            });
          return meterInfoList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterInfo>) null;
      }
    }

    public static List<MeterInfo> LoadMeterInfoBySAPAndMeterHardwareId(
      string sapMaterialNumber,
      int MeterHardwareID)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT mi.Description, mi.HardwareTypeID, mi.MeterInfoID, mi.MeterHardwareID, mi.MeterTypeID, mi.PPSArtikelNr FROM MeterInfo AS mi, HardwareType AS ht WHERE mi.PPSArtikelNr=@PPSArtikelNr AND mi.HardwareTypeID = ht.HardwareTypeID AND mi.MeterHardwareID = @MeterHardwareID;";
          MeterDatabase.AddParameter(cmd, "@PPSArtikelNr", sapMaterialNumber);
          MeterDatabase.AddParameter(cmd, "@MeterHardwareID", MeterHardwareID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterInfo>) null;
          List<MeterInfo> meterInfoList = new List<MeterInfo>();
          while (dataReader.Read())
            meterInfoList.Add(new MeterInfo()
            {
              Description = dataReader["Description"].ToString(),
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"]),
              MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]),
              MeterHardwareID = Convert.ToInt32(dataReader[nameof (MeterHardwareID)]),
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              PPSArtikelNr = dataReader["PPSArtikelNr"].ToString()
            });
          return meterInfoList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterInfo>) null;
      }
    }

    public static List<MeterInfo> LoadMeterInfo(bool onlyCustomerTypes)
    {
      return MeterDatabase.LoadMeterInfo(DbBasis.PrimaryDB, new uint?(), new int?(), new int?(), onlyCustomerTypes);
    }

    public static MeterInfo LoadMeterInfo(uint meterInfoID)
    {
      List<MeterInfo> meterInfoList = MeterDatabase.LoadMeterInfo(DbBasis.PrimaryDB, new uint?(meterInfoID), new int?(), new int?(), false);
      return meterInfoList == null || meterInfoList.Count != 1 ? (MeterInfo) null : meterInfoList[0];
    }

    public static List<MeterInfo> LoadMeterInfoByHardwareTypeID(int hardwareTypeID)
    {
      return MeterDatabase.LoadMeterInfo(DbBasis.PrimaryDB, new uint?(), new int?(hardwareTypeID), new int?(), false);
    }

    public static List<MeterInfo> LoadMeterInfoByMeterHardwareID(int meterHardwareID)
    {
      return MeterDatabase.LoadMeterInfo(DbBasis.PrimaryDB, new uint?(), new int?(), new int?(meterHardwareID), false);
    }

    public static List<MeterInfo> LoadMeterInfo(
      DbBasis db,
      uint? meterInfoID,
      int? hardwareTypeID,
      int? meterHardwareID,
      bool onlyCustomerTypes)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterInfo";
          if (onlyCustomerTypes)
            cmd.CommandText += " WHERE MeterInfoID > 99999";
          else
            cmd.CommandText += " WHERE 1=1";
          if (hardwareTypeID.HasValue)
          {
            cmd.CommandText += " AND HardwareTypeID=@HardwareTypeID";
            MeterDatabase.AddParameter(cmd, "@HardwareTypeID", hardwareTypeID.Value);
          }
          if (meterHardwareID.HasValue)
          {
            cmd.CommandText += " AND MeterHardwareID=@MeterHardwareID";
            MeterDatabase.AddParameter(cmd, "@MeterHardwareID", meterHardwareID.Value);
          }
          if (meterInfoID.HasValue)
          {
            cmd.CommandText += " AND MeterInfoID=@MeterInfoID;";
            MeterDatabase.AddParameter(cmd, "@MeterInfoID", (double) meterInfoID.Value);
          }
          else
            cmd.CommandText += ";";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterInfo>) null;
          List<MeterInfo> meterInfoList = new List<MeterInfo>();
          while (dataReader.Read())
            meterInfoList.Add(new MeterInfo()
            {
              MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]),
              MeterHardwareID = Convert.ToInt32(dataReader["MeterHardwareID"]),
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              PPSArtikelNr = dataReader["PPSArtikelNr"].ToString(),
              DefaultFunctionNr = dataReader["DefaultFunctionNr"].ToString(),
              Description = dataReader["Description"].ToString(),
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"])
            });
          return meterInfoList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterInfo>) null;
      }
    }

    public static List<MeterInfo> LoadMeterInfoByHardwareName(string hardwareName)
    {
      if (string.IsNullOrEmpty(hardwareName))
        return (List<MeterInfo>) null;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT m.* FROM MeterInfo AS m, HardwareType AS h WHERE h.HardwareTypeID=m.HardwareTypeID AND h.HardwareName = @HardwareName;";
          MeterDatabase.AddParameter(cmd, "HardwareName", hardwareName);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterInfo>) null;
          List<MeterInfo> meterInfoList = new List<MeterInfo>();
          while (dataReader.Read())
            meterInfoList.Add(new MeterInfo()
            {
              MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]),
              MeterHardwareID = Convert.ToInt32(dataReader["MeterHardwareID"]),
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              PPSArtikelNr = dataReader["PPSArtikelNr"].ToString(),
              DefaultFunctionNr = dataReader["DefaultFunctionNr"].ToString(),
              Description = dataReader["Description"].ToString(),
              HardwareTypeID = Convert.ToInt32(dataReader["HardwareTypeID"])
            });
          return meterInfoList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterInfo>) null;
      }
    }

    public static MeterInfo AddMeterInfo(DbBasis db, MeterInfo meterInfo)
    {
      return MeterDatabase.AddMeterInfo(db, meterInfo.MeterInfoID, meterInfo.MeterHardwareID, meterInfo.MeterTypeID, meterInfo.PPSArtikelNr, meterInfo.DefaultFunctionNr, meterInfo.Description, meterInfo.HardwareTypeID);
    }

    public static MeterInfo AddMeterInfo(
      int meterInfoID,
      int meterHardwareID,
      int meterTypeID,
      string PPSArtikelNr,
      string defaultFunctionNr,
      string description,
      int hardwareTypeID)
    {
      return MeterDatabase.AddMeterInfo(DbBasis.PrimaryDB, meterInfoID, meterHardwareID, meterTypeID, PPSArtikelNr, defaultFunctionNr, description, hardwareTypeID);
    }

    public static MeterInfo AddMeterInfo(
      DbBasis db,
      int meterInfoID,
      int meterHardwareID,
      int meterTypeID,
      string PPSArtikelNr,
      string defaultFunctionNr,
      string description,
      int hardwareTypeID)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (string.IsNullOrEmpty(description))
        throw new ArgumentNullException("The description of MeterInfo can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          return MeterDatabase.AddMeterInfo(cmd, meterInfoID, meterHardwareID, meterTypeID, PPSArtikelNr, defaultFunctionNr, description, hardwareTypeID);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterInfo) null;
      }
    }

    public static MeterInfo AddMeterInfo(
      IDbCommand cmd,
      int meterInfoID,
      int meterHardwareID,
      int meterTypeID,
      string PPSArtikelNr,
      string defaultFunctionNr,
      string description,
      int hardwareTypeID)
    {
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      if (string.IsNullOrEmpty(description))
        throw new ArgumentNullException("The description of MeterInfo can not be null!");
      cmd.CommandText = "INSERT INTO MeterInfo (MeterInfoID, MeterHardwareID, MeterTypeID, PPSArtikelNr, DefaultFunctionNr, Description, HardwareTypeID) VALUES (@MeterInfoID, @MeterHardwareID, @MeterTypeID, @PPSArtikelNr, @DefaultFunctionNr, @Description, @HardwareTypeID)";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@MeterInfoID", meterInfoID);
      MeterDatabase.AddParameter(cmd, "@MeterHardwareID", meterHardwareID);
      MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
      MeterDatabase.AddParameter(cmd, "@PPSArtikelNr", PPSArtikelNr);
      MeterDatabase.AddParameter(cmd, "@DefaultFunctionNr", defaultFunctionNr);
      MeterDatabase.AddParameter(cmd, "@Description", description);
      MeterDatabase.AddParameter(cmd, "@HardwareTypeID", hardwareTypeID);
      if (cmd.ExecuteNonQuery() != 1)
        return (MeterInfo) null;
      return new MeterInfo()
      {
        MeterInfoID = meterInfoID,
        MeterHardwareID = meterHardwareID,
        MeterTypeID = meterTypeID,
        PPSArtikelNr = PPSArtikelNr,
        DefaultFunctionNr = defaultFunctionNr,
        Description = description,
        HardwareTypeID = hardwareTypeID
      };
    }

    public static bool UpdateMeterInfo(MeterInfo type)
    {
      if (type == null)
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE MeterInfo SET MeterHardwareID=@MeterHardwareID, MeterTypeID=@MeterTypeID, PPSArtikelNr=@PPSArtikelNr, DefaultFunctionNr=@DefaultFunctionNr, [Description]=@Description, HardwareTypeID=@HardwareTypeID WHERE MeterInfoID=@MeterInfoID;";
          MeterDatabase.AddParameter(cmd, "@MeterHardwareID", type.MeterHardwareID);
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", type.MeterTypeID);
          MeterDatabase.AddParameter(cmd, "@PPSArtikelNr", type.PPSArtikelNr);
          MeterDatabase.AddParameter(cmd, "@DefaultFunctionNr", type.DefaultFunctionNr);
          MeterDatabase.AddParameter(cmd, "@Description", type.Description);
          MeterDatabase.AddParameter(cmd, "@HardwareTypeID", type.HardwareTypeID);
          MeterDatabase.AddParameter(cmd, "@MeterInfoID", type.MeterInfoID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static List<MeterType> LoadMeterType(string typename)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterType WHERE Typename=@Typename;";
          MeterDatabase.AddParameter(cmd, "@Typename", typename);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterType>) null;
          List<MeterType> meterTypeList = new List<MeterType>();
          while (dataReader.Read())
            meterTypeList.Add(new MeterType()
            {
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              MTypeTableName = dataReader["MTypeTableName"].ToString(),
              Typename = dataReader["Typename"].ToString(),
              GenerateDate = Convert.ToDateTime(dataReader["GenerateDate"]),
              Description = dataReader["Description"].ToString()
            });
          return meterTypeList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterType>) null;
      }
    }

    public static MeterType GetMeterType(int meterTypeID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterType WHERE MeterTypeID=@MeterTypeID;";
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (MeterType) null;
          return new MeterType()
          {
            MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
            MTypeTableName = dataReader["MTypeTableName"].ToString(),
            Typename = dataReader["Typename"].ToString(),
            GenerateDate = Convert.ToDateTime(dataReader["GenerateDate"]),
            Description = dataReader["Description"].ToString()
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterType) null;
      }
    }

    public static List<MeterType> LoadMeterType(bool onlyCustomerTypes)
    {
      return MeterDatabase.LoadMeterType(DbBasis.PrimaryDB, onlyCustomerTypes);
    }

    public static List<MeterType> LoadMeterType(DbBasis db, bool onlyCustomerTypes)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = !onlyCustomerTypes ? "SELECT * FROM MeterType;" : "SELECT * FROM MeterType WHERE MeterTypeID > 99999;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterType>) null;
          List<MeterType> meterTypeList = new List<MeterType>();
          while (dataReader.Read())
            meterTypeList.Add(new MeterType()
            {
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              MTypeTableName = dataReader["MTypeTableName"].ToString(),
              Typename = dataReader["Typename"].ToString(),
              GenerateDate = Convert.ToDateTime(dataReader["GenerateDate"]),
              Description = dataReader["Description"].ToString()
            });
          return meterTypeList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterType>) null;
      }
    }

    public static MeterType AddMeterType(DbBasis db, MeterType meterType)
    {
      return MeterDatabase.AddMeterType(db, meterType.MeterTypeID, meterType.MTypeTableName, meterType.Typename, meterType.GenerateDate, meterType.Description);
    }

    public static MeterType AddMeterType(
      int meterTypeID,
      string MTypeTableName,
      string typename,
      DateTime generateDate,
      string description)
    {
      return MeterDatabase.AddMeterType(DbBasis.PrimaryDB, meterTypeID, MTypeTableName, typename, generateDate, description);
    }

    public static MeterType AddMeterType(
      DbBasis db,
      int meterTypeID,
      string MTypeTableName,
      string typename,
      DateTime generateDate,
      string description)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (string.IsNullOrEmpty(description))
        throw new ArgumentNullException("The description of MeterInfo can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          return MeterDatabase.AddMeterType(cmd, meterTypeID, MTypeTableName, typename, generateDate, description);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterType) null;
      }
    }

    public static MeterType AddMeterType(
      IDbCommand cmd,
      int meterTypeID,
      string MTypeTableName,
      string typename,
      DateTime generateDate,
      string description)
    {
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      if (string.IsNullOrEmpty(description))
        throw new ArgumentNullException("The description of MeterInfo can not be null!");
      cmd.CommandText = "INSERT INTO MeterType (MeterTypeID, MTypeTableName, Typename, GenerateDate, Description) VALUES (@MeterTypeID, @MTypeTableName, @Typename, @GenerateDate, @Description)";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
      MeterDatabase.AddParameter(cmd, "@MTypeTableName", MTypeTableName);
      MeterDatabase.AddParameter(cmd, "@Typename", typename);
      MeterDatabase.AddParameter(cmd, "@GenerateDate", generateDate);
      MeterDatabase.AddParameter(cmd, "@Description", description);
      if (cmd.ExecuteNonQuery() != 1)
        return (MeterType) null;
      return new MeterType()
      {
        MeterTypeID = meterTypeID,
        MTypeTableName = MTypeTableName,
        Typename = typename,
        GenerateDate = generateDate,
        Description = description
      };
    }

    public static List<MTypeZelsius> LoadMTypeZelsius(bool onlyCustomerTypes)
    {
      return MeterDatabase.LoadMTypeZelsius(DbBasis.PrimaryDB, onlyCustomerTypes);
    }

    public static List<MTypeZelsius> LoadMTypeZelsius(DbBasis db, bool onlyCustomerTypes)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = !onlyCustomerTypes ? "SELECT * FROM MTypeZelsius;" : "SELECT * FROM MTypeZelsius WHERE MeterTypeID > 99999;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MTypeZelsius>) null;
          List<MTypeZelsius> mtypeZelsiusList = new List<MTypeZelsius>();
          while (dataReader.Read())
            mtypeZelsiusList.Add(new MTypeZelsius()
            {
              MeterTypeID = Convert.ToInt32(dataReader["MeterTypeID"]),
              EEPdata = dataReader["EEPdata"] == DBNull.Value || dataReader["EEPdata"] == null ? (byte[]) null : (byte[]) dataReader["EEPdata"],
              TypeOverrideString = dataReader["TypeOverrideString"].ToString()
            });
          return mtypeZelsiusList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MTypeZelsius>) null;
      }
    }

    public static MTypeZelsius AddMTypeZelsius(DbBasis db, MTypeZelsius MTypeZelsius)
    {
      return MeterDatabase.AddMTypeZelsius(db, MTypeZelsius.MeterTypeID, MTypeZelsius.EEPdata, MTypeZelsius.TypeOverrideString);
    }

    public static MTypeZelsius AddMTypeZelsius(
      int meterTypeID,
      byte[] EEPdata,
      string typeOverrideString)
    {
      return MeterDatabase.AddMTypeZelsius(DbBasis.PrimaryDB, meterTypeID, EEPdata, typeOverrideString);
    }

    public static MTypeZelsius AddMTypeZelsius(
      DbBasis db,
      int meterTypeID,
      byte[] EEPdata,
      string typeOverrideString)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO MTypeZelsius (MeterTypeID, EEPdata, TypeOverrideString) VALUES (@MeterTypeID, @EEPdata, @TypeOverrideString)";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
          MeterDatabase.AddParameter(cmd, "@EEPdata", EEPdata);
          MeterDatabase.AddParameter(cmd, "@TypeOverrideString", typeOverrideString);
          if (cmd.ExecuteNonQuery() != 1)
            return (MTypeZelsius) null;
          return new MTypeZelsius()
          {
            MeterTypeID = meterTypeID,
            EEPdata = EEPdata,
            TypeOverrideString = typeOverrideString
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MTypeZelsius) null;
      }
    }

    public static Filter AddFilter(DbBasis db, Filter filter)
    {
      return MeterDatabase.AddFilter(db, filter.Name, filter.Description);
    }

    public static Filter AddFilter(string name, string description)
    {
      return MeterDatabase.AddFilter(DbBasis.PrimaryDB, name, description);
    }

    public static Filter AddFilter(DbBasis db, string name, string description)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (string.IsNullOrEmpty(name))
        return (Filter) null;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          int num1 = 0;
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT MAX(FilterId) FROM Filter;";
          object obj = cmd.ExecuteScalar();
          if (obj != null && obj != DBNull.Value)
            num1 = Convert.ToInt32(obj);
          int num2 = num1 + 1;
          cmd.CommandText = "INSERT INTO Filter (FilterId, Name, Description) VALUES (@FilterId, @Name, @Description)";
          MeterDatabase.AddParameter(cmd, "@FilterId", num2);
          MeterDatabase.AddParameter(cmd, "@Name", name);
          MeterDatabase.AddParameter(cmd, "@Description", description);
          if (cmd.ExecuteNonQuery() != 1)
            return (Filter) null;
          return new Filter()
          {
            FilterId = num2,
            Name = name,
            Description = description
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (Filter) null;
      }
    }

    public static List<Filter> LoadFilter() => MeterDatabase.LoadFilter(DbBasis.PrimaryDB);

    public static List<Filter> LoadFilter(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM Filter ORDER BY FilterId ASC;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<Filter>) null;
          List<Filter> filterList = new List<Filter>();
          filterList.Add(new Filter()
          {
            FilterId = 0,
            Name = Ot.GetTranslatedLanguageText("MeterInstaller", "FilterDefault")
          });
          while (dataReader.Read())
            filterList.Add(new Filter()
            {
              FilterId = Convert.ToInt32(dataReader["FilterId"]),
              Name = dataReader["Name"].ToString(),
              Description = dataReader["Description"].ToString()
            });
          return filterList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<Filter>) null;
      }
    }

    public static Filter GetFilter(int filterId)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM Filter WHERE FilterId=@FilterId;";
          MeterDatabase.AddParameter(cmd, "@FilterId", filterId);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (Filter) null;
          return new Filter()
          {
            FilterId = Convert.ToInt32(dataReader["FilterId"]),
            Name = dataReader["Name"].ToString(),
            Description = dataReader["Description"].ToString()
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (Filter) null;
      }
    }

    public static bool DeleteFilter(int filterId)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          cmd.CommandText = "DELETE FROM FilterValue WHERE FilterId = @FilterId;";
          MeterDatabase.AddParameter(cmd, "@FilterId", filterId);
          cmd.ExecuteNonQuery();
          cmd.CommandText = "DELETE FROM Filter WHERE FilterId = @FilterId;";
          cmd.ExecuteNonQuery();
          cmd.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static List<long> LoadFilterValues(int filterId)
    {
      return MeterDatabase.LoadFilterValues(DbBasis.PrimaryDB, filterId);
    }

    public static List<long> LoadFilterValues(DbBasis db, int filterId)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<long> longList = new List<long>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT ValueIdent FROM FilterValue WHERE FilterId = @FilterId;";
          MeterDatabase.AddParameter(cmd, "@FilterId", filterId);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return longList;
          while (dataReader.Read())
            longList.Add(Convert.ToInt64(dataReader["ValueIdent"]));
          return longList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return longList;
      }
    }

    public static bool AddFilterValue(int filterId, long valueIdent)
    {
      return MeterDatabase.AddFilterValue(DbBasis.PrimaryDB, filterId, valueIdent);
    }

    public static bool AddFilterValue(DbBasis db, int filterId, long valueIdent)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (!ValueIdent.IsValid(valueIdent))
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO FilterValue (FilterId, ValueIdent) VALUES (@FilterId, @ValueIdent);";
          MeterDatabase.AddParameter(cmd, "@FilterId", filterId);
          MeterDatabase.AddParameter(cmd, "@ValueIdent", valueIdent.ToString());
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool UpdateFilterValue(int filterId, long oldValueIdent, long newValueIdent)
    {
      if (filterId < 0 || !ValueIdent.IsValid(oldValueIdent) || !ValueIdent.IsValid(newValueIdent))
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE FilterValue SET ValueIdent=@NewValueIdent WHERE FilterId=@FilterId AND ValueIdent=@OldValueIdent;";
          MeterDatabase.AddParameter(cmd, "@NewValueIdent", newValueIdent.ToString());
          MeterDatabase.AddParameter(cmd, "@FilterId", filterId);
          MeterDatabase.AddParameter(cmd, "@OldValueIdent", oldValueIdent.ToString());
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool DeleteFilterValue(int filterId, long valueIdent)
    {
      if (!ValueIdent.IsValid(valueIdent))
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM FilterValue WHERE FilterId = @FilterId AND ValueIdent=@ValueIdent;";
          MeterDatabase.AddParameter(cmd, "@FilterId", filterId);
          MeterDatabase.AddParameter(cmd, "@ValueIdent", valueIdent.ToString());
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    internal static List<long> LoadAllKnownValueIdentsFromTranslationRules()
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT DISTINCT ValueIdent FROM TranslationRules WHERE ValueIdent > 0";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<long>) null;
          List<long> longList = new List<long>();
          while (dataReader.Read())
            longList.Add((long) Convert.ToInt32(dataReader["ValueIdent"]));
          return longList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<long>) null;
      }
    }

    internal static List<long> LoadAllKnownValueIdentsFromMeterValues()
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT DISTINCT ValueIdent FROM MeterValues";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<long>) null;
          List<long> longList = new List<long>();
          while (dataReader.Read())
            longList.Add((long) Convert.ToInt32(dataReader["ValueIdent"]));
          return longList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<long>) null;
      }
    }

    public static List<string> LoadAllKnownNodeSerialnumbers()
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT DISTINCT SerialNr FROM Meter ORDER BY SerialNr ASC;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<string>) null;
          List<string> stringList = new List<string>();
          while (dataReader.Read())
            stringList.Add(dataReader["SerialNr"].ToString());
          return stringList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<string>) null;
      }
    }

    public static List<string> LoadAllKnownNodeNamesOfMeter()
    {
      return MeterDatabase.LoadAllKnownNodeNames("NodeTypeID = 1");
    }

    public static List<string> LoadAllKnownNodeNamesOfNotMeter()
    {
      return MeterDatabase.LoadAllKnownNodeNames("NodeTypeID <> 1");
    }

    private static List<string> LoadAllKnownNodeNames(string whereStatemet)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = string.Format("SELECT DISTINCT NodeName FROM NodeList WHERE (NodeID < 90000000 OR NodeID > 90000203) AND {0} ORDER BY NodeName ASC;", (object) whereStatemet);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<string>) null;
          List<string> stringList = new List<string>();
          while (dataReader.Read())
            stringList.Add(dataReader["NodeName"].ToString());
          return stringList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<string>) null;
      }
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> TrySaveMeterValues(
      ValueIdentSet valueIdentSet)
    {
      if (valueIdentSet == null)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      string serialNumber = valueIdentSet.SerialNumber;
      List<int> intList = MeterDatabase.LoadMeter(serialNumber);
      if (intList != null && intList.Count > 1)
        return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      StructureTreeNode node = (StructureTreeNode) null;
      if (intList != null && intList.Count == 1)
      {
        List<NodeList> nodeListList = MeterDatabase.LoadNodeList(intList[0]);
        if (nodeListList == null || nodeListList.Count > 1)
          return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
        if (nodeListList != null && nodeListList.Count == 1)
          node = MeterDatabase.LoadMeterInstallerTree(nodeListList[0].NodeID, new int?(0));
      }
      if (node == null)
      {
        node = new StructureTreeNode();
        node.NodeTyp = StructureNodeType.Meter;
        node.Name = StructureNodeType.Meter.ToString();
        node.LayerID = new int?(0);
        node.SerialNumber = serialNumber;
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          if (!MeterDatabase.AddMeter(cmd, node))
            return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
          int? nextUniqueId = MeterDatabase.GetNextUniqueID(cmd, "NodeList", "NodeID");
          if (!nextUniqueId.HasValue || !MeterDatabase.AddNode(cmd, node, nextUniqueId.Value) || !MeterDatabase.SetNextUniqueID(cmd, "NodeList", "NodeID", nextUniqueId.Value + 1))
            return (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
          cmd.Transaction.Commit();
        }
      }
      return node == null ? (SortedList<long, SortedList<DateTime, ReadingValue>>) null : MeterDatabase.SaveMeterValues(node.MeterID.Value, node.NodeID.Value, valueIdentSet.AvailableValues);
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> SaveMeterValuesMSS(
      Guid meterId,
      string serialNumber,
      SortedList<long, SortedList<DateTime, ReadingValue>> values)
    {
      lock (MeterDatabase.saveMeterValueLocker)
      {
        SortedList<long, SortedList<DateTime, ReadingValue>> sortedList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
        if (values == null)
          return sortedList;
        try
        {
          using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
          {
            dbConnection.Open();
            IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
            string str1 = "INSERT INTO MeterValuesMSS (MeterId, ValueIdentIndex, TimePoint, [Value], PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation) VALUES (@MeterId, @ValueIdentIndex, @TimePoint, @Value, @PhysicalQuantity, @MeterType, @Calculation, @CalculationStart, @StorageInterval, @Creation);";
            string str2 = "SELECT MeterId FROM MeterValuesMSS WHERE MeterId=@MeterId AND ValueIdentIndex=@ValueIdentIndex AND TimePoint=@TimePoint AND @Value=@Value AND PhysicalQuantity=@PhysicalQuantity AND MeterType=@MeterType AND Calculation=@Calculation AND CalculationStart=@CalculationStart AND StorageInterval=@StorageInterval AND Creation=@Creation;";
            IDbDataParameter parameter1 = cmd.CreateParameter();
            IDbDataParameter parameter2 = cmd.CreateParameter();
            IDbDataParameter parameter3 = cmd.CreateParameter();
            IDbDataParameter parameter4 = cmd.CreateParameter();
            IDbDataParameter parameter5 = cmd.CreateParameter();
            IDbDataParameter parameter6 = cmd.CreateParameter();
            IDbDataParameter parameter7 = cmd.CreateParameter();
            IDbDataParameter parameter8 = cmd.CreateParameter();
            IDbDataParameter parameter9 = cmd.CreateParameter();
            IDbDataParameter parameter10 = cmd.CreateParameter();
            parameter1.ParameterName = "@MeterId";
            parameter1.DbType = DbType.Guid;
            parameter1.Value = (object) meterId;
            cmd.Parameters.Add((object) parameter1);
            parameter2.ParameterName = "@ValueIdentIndex";
            parameter2.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter2);
            parameter3.ParameterName = "@TimePoint";
            parameter3.DbType = DbType.DateTime;
            cmd.Parameters.Add((object) parameter3);
            parameter4.ParameterName = "@Value";
            parameter4.DbType = DbType.Double;
            cmd.Parameters.Add((object) parameter4);
            parameter5.ParameterName = "@PhysicalQuantity";
            parameter5.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter5);
            parameter6.ParameterName = "@MeterType";
            parameter6.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter6);
            parameter7.ParameterName = "@Calculation";
            parameter7.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter7);
            parameter8.ParameterName = "@CalculationStart";
            parameter8.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter8);
            parameter9.ParameterName = "@StorageInterval";
            parameter9.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter9);
            parameter10.ParameterName = "@Creation";
            parameter10.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter10);
            foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in values)
            {
              long key1 = keyValuePair1.Key;
              byte num1 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(key1) / 4096UL);
              byte num2 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(key1) / 65536UL);
              byte num3 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(key1) / 268435456UL);
              byte num4 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(key1) / 64UL);
              ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(key1);
              byte num5 = (byte) ((ulong) physicalQuantity / 1UL);
              byte num6 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(key1) / 4194304UL);
              byte num7 = 0;
              if (physicalQuantity == ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber)
                num7 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentError>(key1) / 2147483648UL);
              parameter2.Value = (object) num7;
              parameter5.Value = (object) num5;
              parameter6.Value = (object) num4;
              parameter7.Value = (object) num1;
              parameter8.Value = (object) num2;
              parameter9.Value = (object) num6;
              parameter10.Value = (object) num3;
              foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
              {
                DateTime key2 = keyValuePair2.Key;
                double num8 = keyValuePair2.Value.value;
                DateTime dateTime = new DateTime(key2.Year, key2.Month, key2.Day, key2.Hour, key2.Minute, key2.Second);
                parameter3.Value = (object) dateTime;
                parameter4.Value = (object) num8;
                try
                {
                  cmd.CommandText = str2;
                  if (cmd.ExecuteScalar() == null)
                  {
                    cmd.CommandText = str1;
                    if (1 == cmd.ExecuteNonQuery())
                    {
                      if (!sortedList.ContainsKey(key1))
                        sortedList.Add(key1, new SortedList<DateTime, ReadingValue>());
                      sortedList[key1].Add(key2, keyValuePair2.Value);
                    }
                  }
                }
                catch (Exception ex)
                {
                  MeterDatabase.LogFailedSQLQuery(cmd);
                  string message = string.Format("Failed save the value to database! \nMeterID: {0},\nTimepoint: {1}, \nValue: {2}, \nValueIdent: {3}, \nValueIdentDescription: {4}, \nError: {5}", (object) meterId, (object) key2, (object) num8, (object) key1, (object) ValueIdent.GetTranslatedValueNameForValueId(key1, false), (object) ex.Message);
                  MeterDatabase.logger.ErrorException(message, ex);
                }
              }
            }
            return sortedList;
          }
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          MeterDatabase.logger.ErrorException(str, ex);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
          return sortedList;
        }
      }
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> SaveMeterValuesMSS(
      DbConnection conn,
      Guid meterId,
      string serialNumber,
      SortedList<long, SortedList<DateTime, ReadingValue>> values)
    {
      lock (MeterDatabase.saveMeterValueLocker)
      {
        SortedList<long, SortedList<DateTime, ReadingValue>> sortedList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
        if (values == null)
          return sortedList;
        try
        {
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand((IDbConnection) conn);
          string str1 = "INSERT INTO MeterValuesMSS (MeterId, ValueIdentIndex, TimePoint, [Value], PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation) VALUES (@MeterId, @ValueIdentIndex, @TimePoint, @Value, @PhysicalQuantity, @MeterType, @Calculation, @CalculationStart, @StorageInterval, @Creation);";
          string str2 = "SELECT MeterId FROM MeterValuesMSS WHERE MeterId=@MeterId AND ValueIdentIndex=@ValueIdentIndex AND TimePoint=@TimePoint AND @Value=@Value AND PhysicalQuantity=@PhysicalQuantity AND MeterType=@MeterType AND Calculation=@Calculation AND CalculationStart=@CalculationStart AND StorageInterval=@StorageInterval AND Creation=@Creation;";
          IDbDataParameter parameter1 = cmd.CreateParameter();
          IDbDataParameter parameter2 = cmd.CreateParameter();
          IDbDataParameter parameter3 = cmd.CreateParameter();
          IDbDataParameter parameter4 = cmd.CreateParameter();
          IDbDataParameter parameter5 = cmd.CreateParameter();
          IDbDataParameter parameter6 = cmd.CreateParameter();
          IDbDataParameter parameter7 = cmd.CreateParameter();
          IDbDataParameter parameter8 = cmd.CreateParameter();
          IDbDataParameter parameter9 = cmd.CreateParameter();
          IDbDataParameter parameter10 = cmd.CreateParameter();
          parameter1.ParameterName = "@MeterId";
          parameter1.DbType = DbType.Guid;
          parameter1.Value = (object) meterId;
          cmd.Parameters.Add((object) parameter1);
          parameter2.ParameterName = "@ValueIdentIndex";
          parameter2.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter2);
          parameter3.ParameterName = "@TimePoint";
          parameter3.DbType = DbType.DateTime;
          cmd.Parameters.Add((object) parameter3);
          parameter4.ParameterName = "@Value";
          parameter4.DbType = DbType.Double;
          cmd.Parameters.Add((object) parameter4);
          parameter5.ParameterName = "@PhysicalQuantity";
          parameter5.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter5);
          parameter6.ParameterName = "@MeterType";
          parameter6.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter6);
          parameter7.ParameterName = "@Calculation";
          parameter7.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter7);
          parameter8.ParameterName = "@CalculationStart";
          parameter8.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter8);
          parameter9.ParameterName = "@StorageInterval";
          parameter9.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter9);
          parameter10.ParameterName = "@Creation";
          parameter10.DbType = DbType.Byte;
          cmd.Parameters.Add((object) parameter10);
          foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in values)
          {
            long key1 = keyValuePair1.Key;
            byte num1 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(key1) / 4096UL);
            byte num2 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(key1) / 65536UL);
            byte num3 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(key1) / 268435456UL);
            byte num4 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(key1) / 64UL);
            ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(key1);
            byte num5 = (byte) ((ulong) physicalQuantity / 1UL);
            byte num6 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(key1) / 4194304UL);
            byte num7 = 0;
            if (physicalQuantity == ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber)
              num7 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentError>(key1) / 2147483648UL);
            parameter2.Value = (object) num7;
            parameter5.Value = (object) num5;
            parameter6.Value = (object) num4;
            parameter7.Value = (object) num1;
            parameter8.Value = (object) num2;
            parameter9.Value = (object) num6;
            parameter10.Value = (object) num3;
            foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
            {
              DateTime key2 = keyValuePair2.Key;
              double num8 = keyValuePair2.Value.value;
              DateTime dateTime = new DateTime(key2.Year, key2.Month, key2.Day, key2.Hour, key2.Minute, key2.Second);
              parameter3.Value = (object) dateTime;
              parameter4.Value = (object) num8;
              try
              {
                cmd.CommandText = str2;
                if (cmd.ExecuteScalar() == null)
                {
                  cmd.CommandText = str1;
                  if (1 == cmd.ExecuteNonQuery())
                  {
                    if (!sortedList.ContainsKey(key1))
                      sortedList.Add(key1, new SortedList<DateTime, ReadingValue>());
                    sortedList[key1].Add(key2, keyValuePair2.Value);
                  }
                }
              }
              catch (Exception ex)
              {
                MeterDatabase.LogFailedSQLQuery(cmd);
                string message = string.Format("Failed save the value to database! \nMeterID: {0},\nTimepoint: {1}, \nValue: {2}, \nValueIdent: {3}, \nValueIdentDescription: {4}, \nError: {5}", (object) meterId, (object) key2, (object) num8, (object) key1, (object) ValueIdent.GetTranslatedValueNameForValueId(key1, false), (object) ex.Message);
                MeterDatabase.logger.ErrorException(message, ex);
              }
            }
          }
          return sortedList;
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          MeterDatabase.logger.ErrorException(str, ex);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
          return sortedList;
        }
      }
    }

    public static SortedList<long, SortedList<DateTime, ReadingValue>> SaveMeterValues(
      int meterId,
      int nodeId,
      SortedList<long, SortedList<DateTime, ReadingValue>> values)
    {
      lock (MeterDatabase.saveMeterValueLocker)
      {
        SortedList<long, SortedList<DateTime, ReadingValue>> sortedList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
        if (meterId < 0 || nodeId < 0)
          return sortedList;
        if (values == null)
          return sortedList;
        try
        {
          using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
          {
            dbConnection.Open();
            IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
            string str1 = "INSERT INTO MeterValues (MeterId, ValueIdentIndex, TimePoint, [Value], PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation, NodeID) VALUES (@MeterId, @ValueIdentIndex, @TimePoint, @Value, @PhysicalQuantity, @MeterType, @Calculation, @CalculationStart, @StorageInterval, @Creation, @NodeID);";
            string str2 = "SELECT MeterId FROM MeterValues WHERE MeterId=@MeterId AND ValueIdentIndex=@ValueIdentIndex AND TimePoint=@TimePoint AND @Value=@Value AND PhysicalQuantity=@PhysicalQuantity AND MeterType=@MeterType AND Calculation=@Calculation AND CalculationStart=@CalculationStart AND StorageInterval=@StorageInterval AND Creation=@Creation AND NodeID=@NodeID;";
            IDbDataParameter parameter1 = cmd.CreateParameter();
            IDbDataParameter parameter2 = cmd.CreateParameter();
            IDbDataParameter parameter3 = cmd.CreateParameter();
            IDbDataParameter parameter4 = cmd.CreateParameter();
            IDbDataParameter parameter5 = cmd.CreateParameter();
            IDbDataParameter parameter6 = cmd.CreateParameter();
            IDbDataParameter parameter7 = cmd.CreateParameter();
            IDbDataParameter parameter8 = cmd.CreateParameter();
            IDbDataParameter parameter9 = cmd.CreateParameter();
            IDbDataParameter parameter10 = cmd.CreateParameter();
            IDbDataParameter parameter11 = cmd.CreateParameter();
            parameter1.ParameterName = "@MeterId";
            parameter1.DbType = DbType.Int32;
            parameter1.Value = (object) meterId;
            cmd.Parameters.Add((object) parameter1);
            parameter2.ParameterName = "@ValueIdentIndex";
            parameter2.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter2);
            parameter3.ParameterName = "@TimePoint";
            parameter3.DbType = DbType.DateTime;
            cmd.Parameters.Add((object) parameter3);
            parameter4.ParameterName = "@Value";
            parameter4.DbType = DbType.Double;
            cmd.Parameters.Add((object) parameter4);
            parameter5.ParameterName = "@PhysicalQuantity";
            parameter5.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter5);
            parameter6.ParameterName = "@MeterType";
            parameter6.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter6);
            parameter7.ParameterName = "@Calculation";
            parameter7.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter7);
            parameter8.ParameterName = "@CalculationStart";
            parameter8.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter8);
            parameter9.ParameterName = "@StorageInterval";
            parameter9.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter9);
            parameter10.ParameterName = "@Creation";
            parameter10.DbType = DbType.Byte;
            cmd.Parameters.Add((object) parameter10);
            parameter11.ParameterName = "@NodeID";
            parameter11.DbType = DbType.Int32;
            parameter11.Value = (object) nodeId;
            cmd.Parameters.Add((object) parameter11);
            foreach (KeyValuePair<long, SortedList<DateTime, ReadingValue>> keyValuePair1 in values)
            {
              long key1 = keyValuePair1.Key;
              byte num1 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(key1) / 4096UL);
              byte num2 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(key1) / 65536UL);
              byte num3 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(key1) / 268435456UL);
              byte num4 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(key1) / 64UL);
              ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(key1);
              byte num5 = (byte) ((ulong) physicalQuantity / 1UL);
              byte num6 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(key1) / 4194304UL);
              byte num7 = 0;
              if (physicalQuantity == ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber)
                num7 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentError>(key1) / 2147483648UL);
              parameter2.Value = (object) num7;
              parameter5.Value = (object) num5;
              parameter6.Value = (object) num4;
              parameter7.Value = (object) num1;
              parameter8.Value = (object) num2;
              parameter9.Value = (object) num6;
              parameter10.Value = (object) num3;
              foreach (KeyValuePair<DateTime, ReadingValue> keyValuePair2 in keyValuePair1.Value)
              {
                DateTime key2 = keyValuePair2.Key;
                double num8 = keyValuePair2.Value.value;
                DateTime dateTime = new DateTime(key2.Year, key2.Month, key2.Day, key2.Hour, key2.Minute, key2.Second);
                parameter3.Value = (object) dateTime;
                parameter4.Value = (object) num8;
                try
                {
                  cmd.CommandText = str2;
                  if (cmd.ExecuteScalar() == null)
                  {
                    cmd.CommandText = str1;
                    if (1 == cmd.ExecuteNonQuery())
                    {
                      if (!sortedList.ContainsKey(key1))
                        sortedList.Add(key1, new SortedList<DateTime, ReadingValue>());
                      sortedList[key1].Add(key2, keyValuePair2.Value);
                    }
                  }
                }
                catch (Exception ex)
                {
                  MeterDatabase.LogFailedSQLQuery(cmd);
                  string message = string.Format("Failed save the value to database! \nMeterID: {0},\nNodeID: {1}, \nTimepoint: {2}, \nValue: {3}, \nValueIdent: {4}, \nValueIdentDescription: {5}, \nError: {6}", (object) meterId, (object) nodeId, (object) key2, (object) num8, (object) key1, (object) ValueIdent.GetTranslatedValueNameForValueId(key1, false), (object) ex.Message);
                  MeterDatabase.logger.ErrorException(message, ex);
                }
              }
            }
            return sortedList;
          }
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          MeterDatabase.logger.ErrorException(str, ex);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
          return sortedList;
        }
      }
    }

    public static void LogFailedSQLQuery(IDbCommand cmd)
    {
      if (!MeterDatabase.logger.IsErrorEnabled || cmd == null)
        return;
      MeterDatabase.logger.Error(cmd.CommandText);
      int num = 1;
      foreach (IDbDataParameter parameter in (IEnumerable) cmd.Parameters)
        MeterDatabase.logger.Error("#{0} Name: {1} DbType: {2}  Value: {3}", new object[4]
        {
          (object) num++.ToString().PadRight(2),
          (object) parameter.ParameterName.PadRight(20),
          (object) parameter.DbType.ToString().PadRight(10),
          parameter.Value
        });
    }

    public static List<int> LoadAllMeterIdOfTree(StructureTreeNode node, bool checkChildNodes)
    {
      if (node == null)
        throw new ArgumentNullException("Input parameter 'node' can not be null!");
      if (!node.NodeID.HasValue)
        throw new ArgumentNullException("Input parameter 'node.NodeID' can not be null!");
      IDbCommand cmd1 = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd1 = DbBasis.PrimaryDB.DbCommand(dbConnection);
          StringBuilder stringBuilder = new StringBuilder();
          List<int> intList = new List<int>();
          int num1 = 0;
          foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(node))
          {
            int? nodeId1 = structureTreeNode.NodeID;
            if (nodeId1.HasValue)
            {
              if (stringBuilder.Length == 0)
                stringBuilder.Append("NodeID=@NodeID").Append(num1);
              else
                stringBuilder.Append(" OR NodeID=@NodeID").Append(num1);
              IDbCommand cmd2 = cmd1;
              string parameterName = "@NodeID" + num1.ToString();
              nodeId1 = structureTreeNode.NodeID;
              int num2 = nodeId1.Value;
              MeterDatabase.AddParameter(cmd2, parameterName, num2);
            }
            if (num1 >= 400)
            {
              cmd1.CommandText = "SELECT MeterID FROM NodeList WHERE " + stringBuilder.ToString();
              IDataReader dataReader = cmd1.ExecuteReader();
              if (dataReader == null)
                return (List<int>) null;
              while (dataReader.Read())
              {
                int int32 = Convert.ToInt32(dataReader["MeterID"]);
                if (int32 != 0 && !intList.Contains(int32))
                  intList.Add(int32);
              }
              dataReader.Close();
              cmd1.Parameters.Clear();
              stringBuilder.Length = 0;
              num1 = 0;
            }
            else
              ++num1;
            int num3;
            if (!checkChildNodes)
            {
              nodeId1 = structureTreeNode.NodeID;
              int? nodeId2 = node.NodeID;
              num3 = nodeId1.GetValueOrDefault() == nodeId2.GetValueOrDefault() & nodeId1.HasValue == nodeId2.HasValue ? 1 : 0;
            }
            else
              num3 = 0;
            if (num3 != 0)
              break;
          }
          if (stringBuilder.Length > 0)
          {
            cmd1.CommandText = "SELECT MeterID FROM NodeList WHERE " + stringBuilder.ToString();
            IDataReader dataReader = cmd1.ExecuteReader();
            if (dataReader == null)
              return (List<int>) null;
            while (dataReader.Read())
            {
              int int32 = Convert.ToInt32(dataReader["MeterID"]);
              if (int32 != 0 && !intList.Contains(int32))
                intList.Add(int32);
            }
          }
          return intList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd1);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<int>) null;
      }
    }

    public static MeterValue[] LoadMeterValues(
      DateTime startTime,
      DateTime endTime,
      List<string> serialNumbers,
      ValueIdent.ValueIdPart_Calculation calculation,
      ValueIdent.ValueIdPart_CalculationStart calculationStart,
      ValueIdent.ValueIdPart_Creation creation,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity,
      ValueIdent.ValueIdPart_StorageInterval storageInterval)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      if (serialNumbers == null || serialNumbers.Count == 0 || serialNumbers[0] == string.Empty)
      {
        stringBuilder1.Append("SELECT * FROM MeterValues LEFT JOIN Meter ON MeterValues.MeterId = Meter.MeterId WHERE MeterValues.TimePoint >= @startDate AND MeterValues.TimePoint <= @endDate");
      }
      else
      {
        stringBuilder1.Append("SELECT * FROM MeterValues LEFT JOIN Meter ON MeterValues.MeterId = Meter.MeterId WHERE MeterValues.TimePoint >= @startDate AND MeterValues.TimePoint <= @endDate AND Meter.SerialNr IN (");
        for (int index = 0; index < serialNumbers.Count; ++index)
          stringBuilder1.Append(serialNumbers[index].ToString() + ",");
        stringBuilder1.Remove(stringBuilder1.Length - 1, 1);
        stringBuilder1.Append(")");
      }
      long num;
      if (calculation != 0)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        num = (long) calculation / 4096L;
        string str = " AND MeterValues.Calculation = " + num.ToString();
        stringBuilder2.Append(str);
      }
      if (calculationStart != 0)
      {
        StringBuilder stringBuilder3 = stringBuilder1;
        num = (long) calculationStart / 65536L;
        string str = " AND MeterValues.CalculationStart = " + num.ToString();
        stringBuilder3.Append(str);
      }
      if (creation != 0)
      {
        StringBuilder stringBuilder4 = stringBuilder1;
        num = (long) creation / 268435456L;
        string str = " AND MeterValues.Creation = " + num.ToString();
        stringBuilder4.Append(str);
      }
      if (meterType != 0)
      {
        StringBuilder stringBuilder5 = stringBuilder1;
        num = (long) meterType / 64L;
        string str = " AND MeterValues.MeterType = " + num.ToString();
        stringBuilder5.Append(str);
      }
      if (physicalQuantity != 0)
      {
        StringBuilder stringBuilder6 = stringBuilder1;
        num = (long) physicalQuantity / 1L;
        string str = " AND MeterValues.PhysicalQuantity = " + num.ToString();
        stringBuilder6.Append(str);
      }
      if (storageInterval != 0)
      {
        StringBuilder stringBuilder7 = stringBuilder1;
        num = (long) storageInterval / 4194304L;
        string str = " AND MeterValues.StorageInterval = " + num.ToString();
        stringBuilder7.Append(str);
      }
      string str1 = stringBuilder1.ToString();
      MeterDatabase.logger.Info("WEBSERVICE: " + str1);
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = str1;
          MeterDatabase.AddParameter(cmd, "@startDate", startTime);
          MeterDatabase.AddParameter(cmd, "@endDate", endTime);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (MeterValue[]) null;
          List<MeterValue> meterValueList = new List<MeterValue>();
          while (dataReader.Read())
          {
            byte calculation1 = Convert.ToByte(dataReader["Calculation"]);
            byte calculationStart1 = Convert.ToByte(dataReader["CalculationStart"]);
            byte creation1 = Convert.ToByte(dataReader["Creation"]);
            byte meterType1 = Convert.ToByte(dataReader["MeterType"]);
            byte physicalQuantity1 = Convert.ToByte(dataReader["PhysicalQuantity"]);
            byte storageInterval1 = Convert.ToByte(dataReader["StorageInterval"]);
            long valueIdent = ValueIdent.GetValueIdent(Convert.ToByte(dataReader["ValueIdentIndex"]), physicalQuantity1, meterType1, calculation1, calculationStart1, storageInterval1, creation1);
            meterValueList.Add(new MeterValue()
            {
              meterId = (long) Convert.ToInt32(dataReader["MeterId"]),
              readoutDate = Convert.ToDateTime(dataReader["TimePoint"]),
              value = Convert.ToDouble(dataReader["Value"]),
              state = ReadingValueState.ok,
              valueId = valueIdent,
              serialNumber = Convert.ToString(dataReader["SerialNr"])
            });
          }
          return meterValueList.ToArray();
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str2 = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str2, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str2);
        return (MeterValue[]) null;
      }
    }

    public void LoadMeterValues(
      StructureTreeNode node,
      DateTime start,
      DateTime end,
      List<long> filterValueIdent,
      ValueFilter valueFilter)
    {
      if (node == null)
        throw new ArgumentNullException("Input parameter 'node' can not be null!");
      this.valueFilterAfterLoad = valueFilter;
      if (this.lastTimepointOfLoadedMeasurementValues == null)
        this.lastTimepointOfLoadedMeasurementValues = new Dictionary<int, DateTime>();
      bool flag = node.NodeTyp == StructureNodeType.Meter && node.Children.Count > 0;
      List<int> intList = MeterDatabase.LoadAllMeterIdOfTree(node, !flag);
      if (intList == null || intList.Count == 0)
        return;
      lock (MeterDatabase.saveMeterValueLocker)
      {
        this.DisposeReader();
        this.nodeValuesReader = node;
        this.startTimepointOfMeterValues = start;
        this.endTimepointOfMeterValues = end;
        this.FilterValueIdentOfMeterValues = filterValueIdent;
        this.CountOfValues = 0L;
        this.CountOfLoadedValues = 0L;
        IDbConnection Connection = (IDbConnection) null;
        IDbCommand cmd = (IDbCommand) null;
        try
        {
          Connection = DbBasis.PrimaryDB.GetDbConnection();
          Connection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(Connection);
          this.cmdOfMeterValues = cmd;
          string filterSqlStatement = MeterDatabase.CreateFilterSQLStatement(filterValueIdent);
          StringBuilder stringBuilder = new StringBuilder();
          int num1 = 0;
          MeterDatabase.AddParameter(cmd, "@start", start);
          MeterDatabase.AddParameter(cmd, "@end", end);
          for (int index = 0; index < intList.Count; ++index)
          {
            if (this.lastTimepointOfLoadedMeasurementValues.ContainsKey(intList[index]))
              this.lastTimepointOfLoadedMeasurementValues[intList[index]] = DateTime.MinValue;
            else
              this.lastTimepointOfLoadedMeasurementValues.Add(intList[index], DateTime.MinValue);
            if (stringBuilder.Length == 0)
              stringBuilder.Append("MeterID=@MeterID").Append(num1);
            else
              stringBuilder.Append(" OR MeterID=@MeterID").Append(num1);
            MeterDatabase.AddParameter(cmd, "@MeterID" + num1.ToString(), intList[index]);
            if (num1 >= 400 || index == intList.Count - 1)
            {
              cmd.CommandText = string.Format("SELECT COUNT(MeterId) FROM MeterValues WHERE TimePoint BETWEEN @start AND @end AND ({0}) {1};", (object) stringBuilder.ToString(), (object) filterSqlStatement);
              object obj = cmd.ExecuteScalar();
              if (obj == null || obj == DBNull.Value)
                return;
              this.CountOfValues += Convert.ToInt64(obj);
              cmd.Parameters.Clear();
              MeterDatabase.AddParameter(cmd, "@start", start);
              MeterDatabase.AddParameter(cmd, "@end", end);
              stringBuilder.Length = 0;
              num1 = 0;
            }
            else
              ++num1;
          }
          stringBuilder.Length = 0;
          int num2 = 0;
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@start", start);
          MeterDatabase.AddParameter(cmd, "@end", end);
          for (int index = 0; index < intList.Count; ++index)
          {
            if (stringBuilder.Length == 0)
              stringBuilder.Append("MeterID=@MeterID").Append(num2);
            else
              stringBuilder.Append(" OR MeterID=@MeterID").Append(num2);
            MeterDatabase.AddParameter(cmd, "@MeterID" + num2.ToString(), intList[index]);
            if (num2 >= 400 || index == intList.Count - 1)
            {
              cmd.CommandText = string.Format("SELECT MeterID, ValueIdentIndex, TimePoint, [Value], PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation, NodeID FROM MeterValues WHERE TimePoint BETWEEN @start AND @end AND ({0}) {1}ORDER BY MeterID, TimePoint DESC;", (object) stringBuilder.ToString(), (object) filterSqlStatement);
              cmd.CommandTimeout = 100;
              this.meterValuesReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
              this.notHandledMeterIDs = new Queue<int>((IEnumerable<int>) intList.GetRange(index, intList.Count - index));
              break;
            }
            ++num2;
          }
        }
        catch (Exception ex)
        {
          MeterDatabase.LogFailedSQLQuery(cmd);
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          MeterDatabase.logger.ErrorException(str, ex);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
          this.DisposeReader();
          Connection?.Close();
        }
      }
    }

    private void DisposeReader()
    {
      if (this.meterValuesReader == null)
        return;
      this.meterValuesReader.Close();
      this.meterValuesReader.Dispose();
      this.meterValuesReader = (IDataReader) null;
      this.valueFilterAfterLoad = ValueFilter.None;
      if (this.lastTimepointOfLoadedMeasurementValues != null)
        this.lastTimepointOfLoadedMeasurementValues.Clear();
    }

    private static string CreateWhereStatementForValueIdent_V2(string enumName, List<byte> values)
    {
      string str = "";
      foreach (byte num in values)
      {
        if (str.Length > 2)
          str += "AND ";
        str += string.Format("{0}={1} ", (object) enumName, (object) num);
      }
      return str.Trim() + " ";
    }

    public List<MeterValueRow> ReadNextMeterValues(int maxRecords)
    {
      if (this.meterValuesReader == null || this.meterValuesReader.IsClosed)
        return (List<MeterValueRow>) null;
      if (this.nodeValuesReader == null)
        throw new ArgumentNullException("Parameter 'nodeValuesReader' can not be null!");
      if (this.cmdOfMeterValues == null)
        throw new ArgumentNullException("Parameter 'cmdOfMeterValues' can not be null!");
      List<MeterValueRow> meterValueRowList = new List<MeterValueRow>();
      while (this.meterValuesReader.Read())
      {
        DateTime timePoint = (DateTime) this.meterValuesReader["TimePoint"];
        int int32 = Convert.ToInt32(this.meterValuesReader["MeterId"]);
        if (this.IsFiltered(timePoint, int32))
        {
          --this.CountOfValues;
        }
        else
        {
          ++this.CountOfLoadedValues;
          MeterValueRow meterValueRow = new MeterValueRow()
          {
            Calculation = Convert.ToByte(this.meterValuesReader["Calculation"]),
            CalculationStart = Convert.ToByte(this.meterValuesReader["CalculationStart"]),
            Creation = Convert.ToByte(this.meterValuesReader["Creation"]),
            MeterId = int32,
            MeterType = Convert.ToByte(this.meterValuesReader["MeterType"]),
            PhysicalQuantity = Convert.ToByte(this.meterValuesReader["PhysicalQuantity"]),
            StorageInterval = Convert.ToByte(this.meterValuesReader["StorageInterval"]),
            TimePoint = timePoint,
            Value = Convert.ToDouble(this.meterValuesReader["Value"]),
            ValueIdentIndex = Convert.ToByte(this.meterValuesReader["ValueIdentIndex"]),
            NodeID = Convert.ToInt32(this.meterValuesReader["NodeID"])
          };
          meterValueRow.NodeName = this.nodeValuesReader.FindNodeName(meterValueRow.MeterId);
          meterValueRow.SerialNr = this.nodeValuesReader.FindSerialnumber(meterValueRow.MeterId);
          meterValueRowList.Add(meterValueRow);
          if (meterValueRowList.Count >= maxRecords)
            return meterValueRowList;
        }
      }
      if (this.notHandledMeterIDs != null && this.notHandledMeterIDs.Count > 0)
      {
        try
        {
          StringBuilder stringBuilder = new StringBuilder();
          int num = 0;
          this.meterValuesReader.Close();
          this.cmdOfMeterValues.Connection.Open();
          this.cmdOfMeterValues.Parameters.Clear();
          MeterDatabase.AddParameter(this.cmdOfMeterValues, "@start", this.startTimepointOfMeterValues);
          MeterDatabase.AddParameter(this.cmdOfMeterValues, "@end", this.endTimepointOfMeterValues);
          string filterSqlStatement = MeterDatabase.CreateFilterSQLStatement(this.FilterValueIdentOfMeterValues);
          while (this.notHandledMeterIDs.Count > 0)
          {
            if (stringBuilder.Length == 0)
              stringBuilder.Append("MeterID=@MeterID").Append(num);
            else
              stringBuilder.Append(" OR MeterID=@MeterID").Append(num);
            MeterDatabase.AddParameter(this.cmdOfMeterValues, "@MeterID" + num.ToString(), this.notHandledMeterIDs.Dequeue());
            if (num >= 400 || num == this.notHandledMeterIDs.Count - 1)
            {
              this.cmdOfMeterValues.CommandText = string.Format("SELECT MeterID, ValueIdentIndex, TimePoint, [Value], PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation, NodeID FROM MeterValues WHERE TimePoint BETWEEN @start AND @end AND ({0}) {1}ORDER BY MeterID, TimePoint DESC;", (object) stringBuilder.ToString(), (object) filterSqlStatement);
              this.cmdOfMeterValues.CommandTimeout = 100;
              this.meterValuesReader = this.cmdOfMeterValues.ExecuteReader(CommandBehavior.CloseConnection);
              return this.ReadNextMeterValues(maxRecords);
            }
            ++num;
          }
        }
        catch (Exception ex)
        {
          MeterDatabase.logger.Error<Exception>(ex);
          return meterValueRowList;
        }
      }
      this.DisposeReader();
      return meterValueRowList;
    }

    private bool IsFiltered(DateTime timePoint, int meterID)
    {
      if (this.valueFilterAfterLoad != 0)
      {
        if (this.lastTimepointOfLoadedMeasurementValues[meterID] == DateTime.MinValue)
        {
          this.lastTimepointOfLoadedMeasurementValues[meterID] = timePoint;
          return false;
        }
        if (this.lastTimepointOfLoadedMeasurementValues[meterID] == timePoint)
          return false;
        if (this.valueFilterAfterLoad == ValueFilter.OnlyOneValueSetPerDay)
        {
          int num;
          if (this.lastTimepointOfLoadedMeasurementValues[meterID].Day == timePoint.Day)
          {
            DateTime measurementValue = this.lastTimepointOfLoadedMeasurementValues[meterID];
            if (measurementValue.Month == timePoint.Month)
            {
              measurementValue = this.lastTimepointOfLoadedMeasurementValues[meterID];
              num = measurementValue.Year == timePoint.Year ? 1 : 0;
              goto label_10;
            }
          }
          num = 0;
label_10:
          if (num != 0)
            return true;
        }
        else if (this.valueFilterAfterLoad == ValueFilter.OnlyOneValueSetPerMonth)
        {
          if (this.lastTimepointOfLoadedMeasurementValues[meterID].Month == timePoint.Month && this.lastTimepointOfLoadedMeasurementValues[meterID].Year == timePoint.Year)
            return true;
        }
        else if (this.valueFilterAfterLoad == ValueFilter.OnlyOneValueSetPerYear && this.lastTimepointOfLoadedMeasurementValues[meterID].Year == timePoint.Year)
          return true;
        this.lastTimepointOfLoadedMeasurementValues[meterID] = timePoint;
      }
      return false;
    }

    public static Dictionary<int, string> GetFullAdditionalInfos(DataTable table)
    {
      if (DbBasis.PrimaryDB == null)
        return (Dictionary<int, string>) null;
      DataTable table1 = table.DefaultView.ToTable(true, "NodeID");
      if (table1 == null || table1.Rows.Count == 0)
        return (Dictionary<int, string>) null;
      string SqlCommand = "SELECT   nl.NodeID, nr.ParentID, nl.NodeAdditionalInfos\r\n\t\t\t\t\t\tFROM     NodeList AS nl INNER JOIN NodeReferences AS nr ON nl.NodeID = nr.NodeID";
      DataTable dataTable = new DataTable();
      using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand, dbConnection).Fill(dataTable);
      Dictionary<int, string> fullAdditionalInfos = new Dictionary<int, string>();
      foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
      {
        int num = (int) row["NodeID"];
        StringBuilder stringBuilder = new StringBuilder();
        while (num > 0)
        {
          DataRow[] dataRowArray = dataTable.Select("NodeID=" + num.ToString());
          if (dataRowArray.Length != 0)
          {
            num = (int) dataRowArray[0]["ParentID"];
            string str = Util.ToString(dataRowArray[0]["NodeAdditionalInfos"]);
            if (!string.IsNullOrEmpty(str))
            {
              if (stringBuilder.Length > 0)
                stringBuilder.Insert(0, ";");
              stringBuilder.Insert(0, str);
            }
          }
          else
            break;
        }
        if (stringBuilder.Length > 0)
          fullAdditionalInfos.Add((int) row["NodeID"], stringBuilder.ToString());
      }
      return fullAdditionalInfos;
    }

    public static long GetDatabaseSize() => DbBasis.PrimaryDB.GetDatabaseSize();

    public void Dispose() => this.DisposeReader();

    public static List<NodeList> LoadNodeList() => MeterDatabase.LoadNodeList(DbBasis.PrimaryDB);

    public static List<NodeList> LoadNodeList(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM NodeList;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<NodeList>) null;
          List<NodeList> nodeListList = new List<NodeList>();
          while (dataReader.Read())
          {
            int num = 0;
            if (dataReader["NodeTypeID"] != DBNull.Value)
              num = Convert.ToInt32(dataReader["NodeTypeID"]);
            string empty1 = string.Empty;
            if (dataReader["NodeName"] != DBNull.Value)
              empty1 = Convert.ToString(dataReader["NodeName"]);
            string empty2 = string.Empty;
            if (dataReader["NodeDescription"] != DBNull.Value)
              empty2 = Convert.ToString(dataReader["NodeDescription"]);
            string empty3 = string.Empty;
            if (dataReader["NodeSettings"] != DBNull.Value)
              empty3 = Convert.ToString(dataReader["NodeSettings"]);
            DateTime? nullable1 = new DateTime?();
            if (dataReader["ValidFrom"] != DBNull.Value)
              nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ValidFrom"]));
            DateTime? nullable2 = new DateTime?();
            if (dataReader["ValidTo"] != DBNull.Value)
              nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ValidTo"]));
            string empty4 = string.Empty;
            if (dataReader["NodeAdditionalInfos"] != DBNull.Value)
              empty4 = Convert.ToString(dataReader["NodeAdditionalInfos"]);
            nodeListList.Add(new NodeList()
            {
              NodeID = Convert.ToInt32(dataReader["NodeID"]),
              MeterID = Convert.ToInt32(dataReader["MeterID"]),
              NodeTypeID = num,
              NodeName = empty1,
              NodeDescription = empty2,
              NodeSettings = empty3,
              ValidFrom = nullable1,
              ValidTo = nullable2,
              NodeAdditionalInfos = empty4
            });
          }
          return nodeListList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<NodeList>) null;
      }
    }

    public static List<NodeList> LoadNodeList(int meterID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM NodeList WHERE MeterID=@MeterID;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<NodeList>) null;
          List<NodeList> nodeListList = new List<NodeList>();
          while (dataReader.Read())
          {
            int num = 0;
            if (dataReader["NodeTypeID"] != DBNull.Value)
              num = Convert.ToInt32(dataReader["NodeTypeID"]);
            string empty1 = string.Empty;
            if (dataReader["NodeName"] != DBNull.Value)
              empty1 = Convert.ToString(dataReader["NodeName"]);
            string empty2 = string.Empty;
            if (dataReader["NodeDescription"] != DBNull.Value)
              empty2 = Convert.ToString(dataReader["NodeDescription"]);
            string empty3 = string.Empty;
            if (dataReader["NodeSettings"] != DBNull.Value)
              empty3 = Convert.ToString(dataReader["NodeSettings"]);
            DateTime? nullable1 = new DateTime?();
            if (dataReader["ValidFrom"] != DBNull.Value)
              nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ValidFrom"]));
            DateTime? nullable2 = new DateTime?();
            if (dataReader["ValidTo"] != DBNull.Value)
              nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ValidTo"]));
            string empty4 = string.Empty;
            if (dataReader["NodeAdditionalInfos"] != DBNull.Value)
              empty4 = Convert.ToString(dataReader["NodeAdditionalInfos"]);
            nodeListList.Add(new NodeList()
            {
              NodeID = Convert.ToInt32(dataReader["NodeID"]),
              MeterID = Convert.ToInt32(dataReader["MeterID"]),
              NodeTypeID = num,
              NodeName = empty1,
              NodeDescription = empty2,
              NodeSettings = empty3,
              ValidFrom = nullable1,
              ValidTo = nullable2,
              NodeAdditionalInfos = empty4
            });
          }
          return nodeListList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<NodeList>) null;
      }
    }

    private bool UpdateNodeList(IDbCommand cmd, int nodeID, int meterID, DateTime validFrom)
    {
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      cmd.CommandText = "UPDATE NodeList SET ValidFrom=@ValidFrom WHERE NodeID=@NodeID AND MeterID=@MeterID;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@ValidFrom", validFrom);
      MeterDatabase.AddParameter(cmd, "@NodeID", nodeID);
      MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
      return cmd.ExecuteNonQuery() == 1;
    }

    public static string TryGetDatabaseIdentificationValue(string key)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT InfoData FROM DatabaseIdentification WHERE InfoName = @InfoName;";
          MeterDatabase.AddParameter(cmd, "@InfoName", key);
          object obj = cmd.ExecuteScalar();
          return obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()) ? obj.ToString() : (string) null;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (string) null;
      }
    }

    public static List<Meter> LoadMeter() => MeterDatabase.LoadMeter(DbBasis.PrimaryDB);

    public static List<Meter> LoadMeter(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM Meter;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<Meter>) null;
          List<Meter> meterList = new List<Meter>();
          while (dataReader.Read())
          {
            DateTime? nullable1 = new DateTime?();
            if (dataReader["ProductionDate"] != DBNull.Value)
              nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ProductionDate"]));
            DateTime? nullable2 = new DateTime?();
            if (dataReader["ApprovalDate"] != DBNull.Value)
              nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ApprovalDate"]));
            meterList.Add(new Meter()
            {
              MeterID = Convert.ToInt32(dataReader["MeterID"]),
              MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]),
              SerialNr = dataReader["SerialNr"].ToString(),
              ProductionDate = nullable1,
              ApprovalDate = nullable2,
              OrderNr = dataReader["OrderNr"].ToString()
            });
          }
          return meterList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<Meter>) null;
      }
    }

    public static List<int> LoadMeter(string serialnumber)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT MeterID FROM Meter WHERE SerialNr=@SerialNr;";
          MeterDatabase.AddParameter(cmd, "@SerialNr", serialnumber);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<int>) null;
          List<int> intList = new List<int>();
          while (dataReader.Read())
            intList.Add(Convert.ToInt32(dataReader["MeterID"]));
          return intList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<int>) null;
      }
    }

    public static Meter AddMeter(DbBasis db, Meter meter)
    {
      return MeterDatabase.AddMeter(db, meter.MeterID, meter.MeterInfoID, meter.SerialNr, meter.ProductionDate, meter.ApprovalDate, meter.OrderNr);
    }

    public static Meter AddMeter(
      int meterID,
      int meterInfoID,
      string serialNr,
      DateTime? productionDate,
      DateTime? approvalDate,
      string orderNr)
    {
      return MeterDatabase.AddMeter(DbBasis.PrimaryDB, meterID, meterInfoID, serialNr, productionDate, approvalDate, orderNr);
    }

    public static Meter AddMeter(
      DbBasis db,
      int meterID,
      int meterInfoID,
      string serialNr,
      DateTime? productionDate,
      DateTime? approvalDate,
      string orderNr)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (string.IsNullOrEmpty(serialNr))
        throw new ArgumentNullException("The serialNr of Meter can not be null or empty!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO Meter (MeterID, MeterInfoID, SerialNr, ProductionDate, ApprovalDate, OrderNr) VALUES (@MeterID, @MeterInfoID, @SerialNr, @ProductionDate, @ApprovalDate, @OrderNr)";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
          MeterDatabase.AddParameter(cmd, "@MeterInfoID", meterInfoID);
          MeterDatabase.AddParameter(cmd, "@SerialNr", serialNr);
          MeterDatabase.AddParameter(cmd, "@ProductionDate", productionDate);
          MeterDatabase.AddParameter(cmd, "@ApprovalDate", approvalDate);
          MeterDatabase.AddParameter(cmd, "@OrderNr", orderNr);
          if (cmd.ExecuteNonQuery() != 1)
            return (Meter) null;
          return new Meter()
          {
            MeterID = meterID,
            MeterInfoID = meterInfoID,
            SerialNr = serialNr,
            ProductionDate = productionDate,
            ApprovalDate = approvalDate,
            OrderNr = orderNr
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (Meter) null;
      }
    }

    private static bool AddMeter(StructureTreeNode node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      if (node.NodeTyp != StructureNodeType.Meter)
        throw new ArgumentException(nameof (node));
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          MeterDatabase.AddMeter(cmd, node);
          cmd.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    private static bool AddMeter(IDbCommand cmd, StructureTreeNode node)
    {
      int? nextUniqueId = MeterDatabase.GetNextUniqueID(cmd, "Meter", "MeterID");
      return nextUniqueId.HasValue && MeterDatabase.AddMeter(cmd, node, nextUniqueId.Value) && MeterDatabase.SetNextUniqueID(cmd, "Meter", "MeterID", nextUniqueId.Value + 1);
    }

    private static bool AddMeter(IDbCommand cmd, StructureTreeNode node, int nextMeterID)
    {
      cmd.CommandText = "SELECT MeterID FROM Meter WHERE SerialNr=@SerialNr;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@SerialNr", node.SerialNumber);
      using (IDataReader dataReader = cmd.ExecuteReader())
      {
        if (dataReader == null)
          return false;
        if (dataReader.RecordsAffected > 0)
          return false;
      }
      cmd.CommandText = "INSERT INTO Meter (MeterID, MeterInfoID, SerialNr, ProductionDate, ApprovalDate, OrderNr) VALUES (@MeterID, @MeterInfoID, @SerialNr, @ProductionDate, @ApprovalDate, @OrderNr);";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@MeterID", nextMeterID);
      MeterDatabase.AddParameter(cmd, "@MeterInfoID", 0);
      MeterDatabase.AddParameter(cmd, "@SerialNr", node.SerialNumber);
      MeterDatabase.AddParameter(cmd, "@ProductionDate", DateTime.Now);
      IDbDataParameter parameter1 = cmd.CreateParameter();
      parameter1.DbType = DbType.Date;
      parameter1.ParameterName = "@ApprovalDate";
      parameter1.Value = (object) DBNull.Value;
      cmd.Parameters.Add((object) parameter1);
      IDbDataParameter parameter2 = cmd.CreateParameter();
      parameter2.DbType = DbType.String;
      parameter2.ParameterName = "@OrderNr";
      parameter2.Value = (object) DBNull.Value;
      cmd.Parameters.Add((object) parameter2);
      if (cmd.ExecuteNonQuery() != 1)
        return false;
      node.MeterID = new int?(nextMeterID);
      return true;
    }

    public static Meter GetMeter(int meterID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM Meter WHERE MeterID=@MeterID;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (Meter) null;
          DateTime? nullable1 = new DateTime?();
          if (dataReader["ProductionDate"] != DBNull.Value)
            nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ProductionDate"]));
          DateTime? nullable2 = new DateTime?();
          if (dataReader["ApprovalDate"] != DBNull.Value)
            nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ApprovalDate"]));
          return new Meter()
          {
            MeterID = Convert.ToInt32(dataReader["MeterID"]),
            MeterInfoID = Convert.ToInt32(dataReader["MeterInfoID"]),
            SerialNr = dataReader["SerialNr"].ToString(),
            ProductionDate = nullable1,
            ApprovalDate = nullable2,
            OrderNr = dataReader["OrderNr"].ToString()
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (Meter) null;
      }
    }

    public static bool UpdateMeter(Meter meter)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          return MeterDatabase.UpdateMeter(cmd, meter);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool UpdateMeter(IDbCommand cmd, Meter meter)
    {
      if (meter == null)
        throw new ArgumentNullException("Input parameter 'meter' can not be null!");
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      cmd.CommandText = "UPDATE Meter SET MeterInfoID=@MeterInfoID, SerialNr=@SerialNr, ProductionDate=@ProductionDate, ApprovalDate=@ApprovalDate, OrderNr=@OrderNr WHERE MeterID=@MeterID;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@MeterInfoID", meter.MeterInfoID);
      MeterDatabase.AddParameter(cmd, "@SerialNr", meter.SerialNr);
      MeterDatabase.AddParameter(cmd, "@ProductionDate", meter.ProductionDate);
      MeterDatabase.AddParameter(cmd, "@ApprovalDate", meter.ApprovalDate);
      MeterDatabase.AddParameter(cmd, "@OrderNr", meter.OrderNr);
      MeterDatabase.AddParameter(cmd, "@MeterID", meter.MeterID);
      return cmd.ExecuteNonQuery() == 1;
    }

    private static bool UpdateMeter(IDbCommand cmd, StructureTreeNode node)
    {
      if (node == null)
        return false;
      int? nullable;
      int num1;
      if (node.NodeTyp == StructureNodeType.Meter)
      {
        if (node.MeterID.HasValue)
        {
          nullable = node.MeterID;
          int num2 = 0;
          num1 = nullable.GetValueOrDefault() == num2 & nullable.HasValue ? 1 : 0;
        }
        else
          num1 = 1;
      }
      else
        num1 = 0;
      if (num1 != 0)
      {
        nullable = node.NodeID;
        string str = "Can't change the data of Meter! The MeterID is invalid! NodeID: " + nullable.ToString();
        MeterDatabase.logger.Error(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
      cmd.CommandText = "UPDATE Meter SET SerialNr=@SerialNr WHERE MeterID=@MeterID;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@SerialNr", node.SerialNumber);
      IDbCommand cmd1 = cmd;
      nullable = node.MeterID;
      int num3 = nullable.Value;
      MeterDatabase.AddParameter(cmd1, "@MeterID", num3);
      return cmd.ExecuteNonQuery() == 1;
    }

    public static string GetDatabaseLocationName()
    {
      return MeterDatabase.GetDatabaseLocationName(DbBasis.PrimaryDB);
    }

    public static string GetDatabaseLocationName(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      using (IDbConnection dbConnection = db.GetDbConnection())
      {
        dbConnection.Open();
        return MeterDatabase.GetDatabaseLocationName(db.DbCommand(dbConnection));
      }
    }

    public static string GetDatabaseLocationName(IDbCommand cmd)
    {
      cmd.CommandText = "SELECT InfoData FROM DatabaseIdentification WHERE InfoName=@InfoName;";
      cmd.Parameters.Clear();
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.DbType = DbType.String;
      parameter.ParameterName = "@InfoName";
      parameter.Value = (object) "DatabaseLocationName";
      cmd.Parameters.Add((object) parameter);
      object obj = cmd.ExecuteScalar();
      return obj != null && obj != DBNull.Value ? Convert.ToString(obj) : (string) null;
    }

    [Obsolete]
    public static int? GetNextUniqueID(string tableName, string fieldName)
    {
      return MeterDatabase.GetNextUniqueID(DbBasis.PrimaryDB, tableName, fieldName);
    }

    [Obsolete]
    public static int? GetNextUniqueID(DbBasis db, string tableName, string fieldName)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      using (IDbConnection dbConnection = db.GetDbConnection())
      {
        dbConnection.Open();
        return MeterDatabase.GetNextUniqueID(db.DbCommand(dbConnection), tableName, fieldName);
      }
    }

    [Obsolete]
    public static int? GetNextUniqueID(IDbCommand cmd, string tableName, string fieldName)
    {
      if (string.IsNullOrEmpty(tableName))
        throw new ArgumentNullException(nameof (tableName));
      if (string.IsNullOrEmpty(fieldName))
        throw new ArgumentNullException(nameof (fieldName));
      string databaseLocationName = MeterDatabase.GetDatabaseLocationName(cmd);
      if (string.IsNullOrEmpty(databaseLocationName))
        throw new ArgumentNullException("databaseLocationName");
      cmd.CommandText = "SELECT ZRNextNr FROM ZRGlobalID WHERE ZRTableName=@ZRTableName AND ZRFieldName=@ZRFieldName AND DatabaseLocationName=@DatabaseLocationName;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@ZRTableName", tableName);
      MeterDatabase.AddParameter(cmd, "@ZRFieldName", fieldName);
      MeterDatabase.AddParameter(cmd, "@DatabaseLocationName", databaseLocationName);
      object obj = cmd.ExecuteScalar();
      return obj != null && obj != DBNull.Value ? new int?(Convert.ToInt32(obj)) : new int?();
    }

    [Obsolete]
    public static bool SetNextUniqueID(string tableName, string fieldName, int nextID)
    {
      return MeterDatabase.SetNextUniqueID(DbBasis.PrimaryDB, tableName, fieldName, nextID);
    }

    [Obsolete]
    public static bool SetNextUniqueID(DbBasis db, string tableName, string fieldName, int nextID)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      using (IDbConnection dbConnection = db.GetDbConnection())
      {
        dbConnection.Open();
        return MeterDatabase.SetNextUniqueID(db.DbCommand(dbConnection), tableName, fieldName, nextID);
      }
    }

    [Obsolete]
    public static bool SetNextUniqueID(
      IDbCommand cmd,
      string tableName,
      string fieldName,
      int nextID)
    {
      string databaseLocationName = MeterDatabase.GetDatabaseLocationName(cmd);
      if (string.IsNullOrEmpty(databaseLocationName))
        throw new ArgumentNullException("databaseLocationName");
      cmd.CommandText = "SELECT ZRNextNr, ZRFirstNr, ZRLastNr FROM ZRGlobalID WHERE ZRTableName=@ZRTableName AND ZRFieldName=@ZRFieldName AND DatabaseLocationName=@DatabaseLocationName;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@ZRTableName", tableName);
      MeterDatabase.AddParameter(cmd, "@ZRFieldName", fieldName);
      MeterDatabase.AddParameter(cmd, "@DatabaseLocationName", databaseLocationName);
      using (IDataReader dataReader = cmd.ExecuteReader())
      {
        if (!dataReader.Read())
          return false;
        int int32_1 = dataReader.GetInt32(0);
        int int32_2 = dataReader.GetInt32(1);
        int int32_3 = dataReader.GetInt32(2);
        if (int32_2 > int32_3)
          throw new ArgumentOutOfRangeException("firstNr > lastNr");
        if (int32_1 < int32_2)
          throw new ArgumentOutOfRangeException("nextNr < firstNr");
        if (int32_3 < int32_1)
          throw new ArgumentOutOfRangeException("lastNr > nextNr");
        if (nextID <= int32_1)
          throw new ArgumentOutOfRangeException("nextID <= nextNr");
        if (nextID > int32_3)
          throw new ArgumentOutOfRangeException("nextID > lastNr");
      }
      return MeterDatabase.UpdateNextUniqueID(cmd, tableName, fieldName, nextID);
    }

    [Obsolete]
    public static bool UpdateNextUniqueID(string tableName, string fieldName, int nextID)
    {
      return MeterDatabase.UpdateNextUniqueID(DbBasis.PrimaryDB, tableName, fieldName, nextID);
    }

    [Obsolete]
    public static bool UpdateNextUniqueID(
      DbBasis db,
      string tableName,
      string fieldName,
      int nextID)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      using (IDbConnection dbConnection = db.GetDbConnection())
      {
        dbConnection.Open();
        return MeterDatabase.UpdateNextUniqueID(db.DbCommand(dbConnection), tableName, fieldName, nextID);
      }
    }

    [Obsolete]
    public static bool UpdateNextUniqueID(
      IDbCommand cmd,
      string tableName,
      string fieldName,
      int nextID)
    {
      string databaseLocationName = MeterDatabase.GetDatabaseLocationName(cmd);
      if (string.IsNullOrEmpty(databaseLocationName))
        throw new ArgumentNullException("databaseLocationName");
      cmd.CommandText = "UPDATE ZRGlobalID SET ZRNextNr=@ZRNextNr WHERE ZRTableName=@ZRTableName AND ZRFieldName=@ZRFieldName AND DatabaseLocationName=@DatabaseLocationName;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@ZRNextNr", nextID);
      MeterDatabase.AddParameter(cmd, "@ZRTableName", tableName);
      MeterDatabase.AddParameter(cmd, "@ZRFieldName", fieldName);
      MeterDatabase.AddParameter(cmd, "@DatabaseLocationName", databaseLocationName);
      return cmd.ExecuteNonQuery() == 1;
    }

    private static bool AddTreeNode(StructureTreeNode node)
    {
      int? nullable = node != null && node.Name != null ? node.NodeID : throw new ArgumentNullException(nameof (node));
      if (nullable.HasValue)
        return false;
      int num;
      if (node.NodeTyp == StructureNodeType.Meter)
      {
        nullable = node.MeterID;
        num = !nullable.HasValue ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        nullable = node.NodeID;
        string str = "Wrong MeterInstaller structur detected! The node exist, but the meter device is not known. NodeID: " + nullable.ToString();
        MeterDatabase.logger.Error(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          int? nextUniqueId = MeterDatabase.GetNextUniqueID(cmd, "NodeList", "NodeID");
          if (!nextUniqueId.HasValue || !MeterDatabase.AddNode(cmd, node, nextUniqueId.Value) || !MeterDatabase.SetNextUniqueID(cmd, "NodeList", "NodeID", nextUniqueId.Value + 1))
            return false;
          cmd.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    private static bool AddNode(IDbCommand cmd, StructureTreeNode node, int nextNodeID)
    {
      cmd.CommandText = "INSERT INTO NodeList (NodeID, MeterID, NodeTypeID, NodeName, NodeDescription, NodeSettings, ValidFrom, ValidTo, NodeAdditionalInfos) VALUES (@NodeID, @MeterID, @NodeTypeID, @NodeName, @NodeDescription, @NodeSettings, @ValidFrom, @ValidTo, @NodeAdditionalInfos);";
      cmd.Parameters.Clear();
      IDbDataParameter parameter1 = cmd.CreateParameter();
      parameter1.DbType = DbType.Int32;
      parameter1.ParameterName = "@NodeID";
      parameter1.Value = (object) nextNodeID;
      cmd.Parameters.Add((object) parameter1);
      IDbDataParameter parameter2 = cmd.CreateParameter();
      parameter2.DbType = DbType.Int32;
      parameter2.ParameterName = "@MeterID";
      int? nullable;
      if (node.MeterID.HasValue)
      {
        IDbDataParameter dbDataParameter = parameter2;
        nullable = node.MeterID;
        // ISSUE: variable of a boxed type
        __Boxed<int> local = (System.ValueType) nullable.Value;
        dbDataParameter.Value = (object) local;
      }
      else
        parameter2.Value = (object) 0;
      cmd.Parameters.Add((object) parameter2);
      IDbDataParameter parameter3 = cmd.CreateParameter();
      parameter3.DbType = DbType.Int32;
      parameter3.ParameterName = "@NodeTypeID";
      parameter3.Value = (object) Convert.ToInt32((object) node.NodeTyp);
      cmd.Parameters.Add((object) parameter3);
      IDbDataParameter parameter4 = cmd.CreateParameter();
      parameter4.DbType = DbType.String;
      parameter4.ParameterName = "@NodeName";
      parameter4.Size = 50;
      parameter4.Value = (object) node.Name;
      cmd.Parameters.Add((object) parameter4);
      IDbDataParameter parameter5 = cmd.CreateParameter();
      parameter5.DbType = DbType.String;
      parameter5.ParameterName = "@NodeDescription";
      parameter5.Value = (object) node.NodeDescription;
      cmd.Parameters.Add((object) parameter5);
      IDbDataParameter parameter6 = cmd.CreateParameter();
      parameter6.DbType = DbType.String;
      parameter6.ParameterName = "@NodeSettings";
      parameter6.Value = (object) node.NodeSettings;
      cmd.Parameters.Add((object) parameter6);
      IDbDataParameter parameter7 = cmd.CreateParameter();
      parameter7.DbType = DbType.DateTime;
      parameter7.ParameterName = "@ValidFrom";
      DateTime now = DateTime.Now;
      parameter7.Value = (object) new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
      cmd.Parameters.Add((object) parameter7);
      IDbDataParameter parameter8 = cmd.CreateParameter();
      parameter8.DbType = DbType.DateTime;
      parameter8.ParameterName = "@ValidTo";
      parameter8.Value = (object) DBNull.Value;
      cmd.Parameters.Add((object) parameter8);
      IDbDataParameter parameter9 = cmd.CreateParameter();
      parameter9.DbType = DbType.String;
      parameter9.ParameterName = "@NodeAdditionalInfos";
      parameter9.Value = (object) node.NodeAdditionalInfos;
      cmd.Parameters.Add((object) parameter9);
      if (cmd.ExecuteNonQuery() == 1)
      {
        cmd.CommandText = "INSERT INTO NodeReferences (NodeID, ParentID, LayerID, NodeOrder) VALUES (@NodeID, @ParentID, @LayerID, @NodeOrder);";
        cmd.Parameters.Clear();
        IDbDataParameter parameter10 = cmd.CreateParameter();
        parameter10.DbType = DbType.Int32;
        parameter10.ParameterName = "@NodeID";
        parameter10.Value = (object) nextNodeID;
        cmd.Parameters.Add((object) parameter10);
        IDbDataParameter parameter11 = cmd.CreateParameter();
        parameter11.DbType = DbType.Int32;
        parameter11.ParameterName = "@ParentID";
        if (node.Parent != null && node.Parent.NodeTyp != 0)
        {
          nullable = node.Parent.NodeID;
          if (nullable.HasValue)
          {
            IDbDataParameter dbDataParameter = parameter11;
            nullable = node.Parent.NodeID;
            // ISSUE: variable of a boxed type
            __Boxed<int> local = (System.ValueType) nullable.Value;
            dbDataParameter.Value = (object) local;
          }
          else
          {
            string str = string.Format("The NodeID of parent can not be null! Node: {0}", (object) node);
            MeterDatabase.logger.Error(str);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
            return false;
          }
        }
        else
          parameter11.Value = (object) 0;
        cmd.Parameters.Add((object) parameter11);
        IDbDataParameter parameter12 = cmd.CreateParameter();
        parameter12.DbType = DbType.Int32;
        parameter12.ParameterName = "@LayerID";
        parameter12.Value = (object) node.LayerID;
        cmd.Parameters.Add((object) parameter12);
        IDbDataParameter parameter13 = cmd.CreateParameter();
        parameter13.DbType = DbType.Int32;
        parameter13.ParameterName = "@NodeOrder";
        parameter13.Value = (object) node.NodeOrder;
        cmd.Parameters.Add((object) parameter13);
        if (cmd.ExecuteNonQuery() == 1)
        {
          node.NodeID = new int?(nextNodeID);
          node.ValidFrom = now;
          return true;
        }
        string str1 = string.Format("Can not add new reference to database! {0}", (object) node);
        MeterDatabase.logger.Error(str1);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str1);
        return false;
      }
      string str2 = string.Format("Can not add new node to database! {0}", (object) node);
      MeterDatabase.logger.Error(str2);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str2);
      return false;
    }

    public static bool UpdateTree(StructureTreeNode root)
    {
      if (root == null)
        throw new ArgumentNullException(nameof (root));
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Transaction = dbConnection.BeginTransaction();
          foreach (StructureTreeNode node in StructureTreeNode.ForEach(root))
          {
            if (node.NodeTyp != StructureNodeType.Unknown && !MeterDatabase.UpdateTreeNode(cmd, node))
              return false;
          }
          cmd.Transaction.Commit();
          foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(root))
            structureTreeNode.OldParent = (StructureTreeNode) null;
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    private static bool UpdateTreeNode(IDbCommand cmd, StructureTreeNode node)
    {
      if (node == null)
        return false;
      if (!node.NodeID.HasValue)
      {
        string str = "Can't modify this node! NodeID is null!";
        MeterDatabase.logger.Error(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
      int? meterId = node.MeterID;
      int? nullable;
      if (node.NodeTyp == StructureNodeType.Meter)
      {
        nullable = node.MeterID;
        int num;
        if (nullable.HasValue)
        {
          nullable = node.MeterID;
          num = nullable.Value == 0 ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0 && !string.IsNullOrEmpty(node.SerialNumber))
          MeterDatabase.AddMeter(cmd, node);
      }
      cmd.CommandText = "UPDATE NodeList SET NodeTypeID=@NodeTypeID, MeterID=@MeterID, NodeName=@NodeName, NodeDescription=@NodeDescription, NodeSettings=@NodeSettings, NodeAdditionalInfos=@NodeAdditionalInfos  WHERE NodeID=@NodeID AND MeterID=@MeterID2;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@NodeTypeID", Convert.ToInt32((object) node.NodeTyp));
      IDbCommand cmd1 = cmd;
      nullable = node.MeterID;
      int num1;
      if (nullable.HasValue)
      {
        nullable = node.MeterID;
        num1 = nullable.Value;
      }
      else
        num1 = 0;
      MeterDatabase.AddParameter(cmd1, "@MeterID", num1);
      MeterDatabase.AddParameter(cmd, "@NodeName", node.Name);
      MeterDatabase.AddParameter(cmd, "@NodeDescription", node.NodeDescription);
      MeterDatabase.AddParameter(cmd, "@NodeSettings", node.NodeSettings);
      MeterDatabase.AddParameter(cmd, "@NodeAdditionalInfos", node.NodeAdditionalInfos);
      IDbCommand cmd2 = cmd;
      nullable = node.NodeID;
      int num2 = nullable.Value;
      MeterDatabase.AddParameter(cmd2, "@NodeID", num2);
      MeterDatabase.AddParameter(cmd, "@MeterID2", !meterId.HasValue ? 0 : meterId.Value);
      if (cmd.ExecuteNonQuery() != 1)
        return false;
      cmd.CommandText = "UPDATE NodeReferences SET ParentID=@ParentID, NodeOrder=@NodeOrder WHERE NodeID=@NodeID AND LayerID=@LayerID AND ParentID=@OldParentID;";
      cmd.Parameters.Clear();
      int num3;
      if (node.Parent != null && node.Parent.NodeTyp != StructureNodeType.Unknown)
      {
        nullable = node.Parent.NodeID;
        num3 = nullable.Value;
      }
      else
        num3 = 0;
      int num4 = num3;
      int num5;
      if (node.OldParent != null)
      {
        nullable = node.OldParent.NodeID;
        num5 = nullable.Value;
      }
      else
        num5 = num4;
      int num6 = num5;
      MeterDatabase.AddParameter(cmd, "@ParentID", num4);
      MeterDatabase.AddParameter(cmd, "@NodeOrder", node.NodeOrder);
      IDbCommand cmd3 = cmd;
      nullable = node.NodeID;
      int num7 = nullable.Value;
      MeterDatabase.AddParameter(cmd3, "@NodeID", num7);
      IDbCommand cmd4 = cmd;
      nullable = node.LayerID;
      int num8 = nullable.Value;
      MeterDatabase.AddParameter(cmd4, "@LayerID", num8);
      MeterDatabase.AddParameter(cmd, "@OldParentID", num6);
      return cmd.ExecuteNonQuery() == 1;
    }

    public static bool AddTreeNodeReference(int nodeId, int parentId, int layerId, int nodeOrder)
    {
      if (nodeId == parentId)
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO NodeReferences (NodeID, ParentID, LayerID, NodeOrder) VALUES (@NodeID, @ParentID, @LayerID, @NodeOrder);";
          cmd.Parameters.Clear();
          MeterDatabase.AddParameter(cmd, "@NodeID", nodeId);
          MeterDatabase.AddParameter(cmd, "@ParentID", parentId);
          MeterDatabase.AddParameter(cmd, "@LayerID", layerId);
          MeterDatabase.AddParameter(cmd, "@NodeOrder", nodeOrder);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool HasTreeNodeReference(int nodeId, int layerId, int parentId)
    {
      return MeterDatabase.LoadNodeReferences().Exists((Predicate<NodeReferences>) (e => e.NodeID == nodeId && e.LayerID == layerId && e.ParentID == parentId));
    }

    public static long GetCountOfInstalledMeters()
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT COUNT(*) AS TOTAL FROM Meter;";
          object obj = cmd.ExecuteScalar();
          return obj == null || obj == DBNull.Value ? 0L : Convert.ToInt64(obj);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return -1;
      }
    }

    public static bool CheckTreeNodeForDelete(
      StructureTreeNode node,
      out StructureTreeNodeList problems)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      problems = new StructureTreeNodeList();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(node))
          {
            if (structureTreeNode.NodeTyp == StructureNodeType.Meter)
            {
              int? nullable = structureTreeNode.NodeID;
              if (nullable.HasValue)
              {
                nullable = structureTreeNode.MeterID;
                if (nullable.HasValue)
                {
                  nullable = structureTreeNode.LayerID;
                  int num = 0;
                  if (nullable.GetValueOrDefault() == num & nullable.HasValue && MeterDatabase.HasMeterValues(cmd, structureTreeNode))
                    problems.Add(structureTreeNode);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
      return problems.Count == 0;
    }

    public static bool HasMeterValues(StructureTreeNode nodeToCheck)
    {
      if (nodeToCheck == null)
        throw new ArgumentNullException(nameof (nodeToCheck));
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          foreach (StructureTreeNode nodeToCheck1 in StructureTreeNode.ForEach(nodeToCheck))
          {
            if (nodeToCheck1.NodeTyp == StructureNodeType.Meter && MeterDatabase.HasMeterValues(cmd, nodeToCheck1))
              return true;
          }
          return false;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return true;
      }
    }

    private static bool HasMeterValues(IDbCommand cmd, StructureTreeNode nodeToCheck)
    {
      if (nodeToCheck == null)
        throw new ArgumentNullException(nameof (nodeToCheck));
      if (!nodeToCheck.MeterID.HasValue || !nodeToCheck.NodeID.HasValue)
        return false;
      IDbCommand cmd1 = cmd;
      int? nullable = nodeToCheck.MeterID;
      int meterId = nullable.Value;
      nullable = nodeToCheck.NodeID;
      int nodeId = nullable.Value;
      return MeterDatabase.HasMeterValues(cmd1, meterId, nodeId);
    }

    private static bool HasMeterValues(IDbCommand cmd, int meterId, int nodeId)
    {
      cmd.CommandText = "SELECT COUNT(MeterId) FROM MeterValues WHERE MeterId=@MeterID AND NodeID=@NodeID;";
      cmd.Parameters.Clear();
      IDbDataParameter parameter1 = cmd.CreateParameter();
      parameter1.DbType = DbType.Int32;
      parameter1.ParameterName = "@MeterID";
      parameter1.Value = (object) meterId;
      cmd.Parameters.Add((object) parameter1);
      IDbDataParameter parameter2 = cmd.CreateParameter();
      parameter2.DbType = DbType.Int32;
      parameter2.ParameterName = "@NodeID";
      parameter2.Value = (object) nodeId;
      cmd.Parameters.Add((object) parameter2);
      return Convert.ToInt64(cmd.ExecuteScalar()) > 0L;
    }

    public static bool DeleteTreeNode(
      StructureTreeNode node,
      out StructureTreeNodeList problems,
      bool verificateNodes)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      if (!node.NodeID.HasValue)
        throw new ArgumentNullException("node.NodeID");
      problems = (StructureTreeNodeList) null;
      if (verificateNodes && !MeterDatabase.CheckTreeNodeForDelete(node, out problems))
        return false;
      IDbCommand cmd1 = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd1 = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd1.Transaction = dbConnection.BeginTransaction();
          foreach (StructureTreeNode node1 in StructureTreeNode.ForEach(node))
          {
            if (node1.NodeTyp != StructureNodeType.Unknown)
            {
              int? nullable = node1.LayerID;
              if (!nullable.HasValue)
                throw new ArgumentNullException("nodeToDelete.LayerID");
              int num1;
              if (node1.NodeTyp == StructureNodeType.Meter)
              {
                nullable = node1.LayerID;
                int num2 = 0;
                if (nullable.GetValueOrDefault() == num2 & nullable.HasValue)
                {
                  nullable = node1.MeterID;
                  num1 = nullable.HasValue ? 1 : 0;
                  goto label_17;
                }
              }
              num1 = 0;
label_17:
              if (num1 != 0)
              {
                cmd1.CommandText = "DELETE FROM MeterValues WHERE MeterID=@MeterID;";
                cmd1.Parameters.Clear();
                IDbDataParameter parameter1 = cmd1.CreateParameter();
                parameter1.DbType = DbType.Int32;
                parameter1.ParameterName = "@MeterID";
                cmd1.Parameters.Add((object) parameter1);
                foreach (int num3 in MeterDatabase.LoadAllMeterIdOfTree(node1, true))
                {
                  parameter1.Value = (object) num3;
                  cmd1.ExecuteNonQuery();
                }
                cmd1.CommandText = "DELETE FROM Meter WHERE MeterID=@MeterID;";
                cmd1.Parameters.Clear();
                MeterDatabase.AddParameter(cmd1, "@MeterID", node1.MeterID.Value);
                cmd1.ExecuteNonQuery();
                cmd1.CommandText = "DELETE FROM NodeReferences WHERE NodeID=@NodeID;";
                cmd1.Parameters.Clear();
                IDbDataParameter parameter2 = cmd1.CreateParameter();
                parameter2.DbType = DbType.Int32;
                parameter2.ParameterName = "@NodeID";
                IDbDataParameter dbDataParameter = parameter2;
                nullable = node1.NodeID;
                // ISSUE: variable of a boxed type
                __Boxed<int> local = (System.ValueType) nullable.Value;
                dbDataParameter.Value = (object) local;
                cmd1.Parameters.Add((object) parameter2);
              }
              else
              {
                cmd1.CommandText = "DELETE FROM NodeReferences WHERE NodeID=@NodeID AND ParentID=@ParentID AND LayerID=@LayerID;";
                cmd1.Parameters.Clear();
                IDbDataParameter parameter3 = cmd1.CreateParameter();
                parameter3.DbType = DbType.Int32;
                parameter3.ParameterName = "@NodeID";
                IDbDataParameter dbDataParameter1 = parameter3;
                nullable = node1.NodeID;
                // ISSUE: variable of a boxed type
                __Boxed<int> local1 = (System.ValueType) nullable.Value;
                dbDataParameter1.Value = (object) local1;
                cmd1.Parameters.Add((object) parameter3);
                IDbDataParameter parameter4 = cmd1.CreateParameter();
                parameter4.DbType = DbType.Int32;
                parameter4.ParameterName = "@ParentID";
                parameter4.Value = (object) (node1.Parent == null || node1.Parent.NodeTyp == StructureNodeType.Unknown ? new int?(0) : node1.Parent.NodeID);
                cmd1.Parameters.Add((object) parameter4);
                IDbDataParameter parameter5 = cmd1.CreateParameter();
                parameter5.DbType = DbType.Int32;
                parameter5.ParameterName = "@LayerID";
                IDbDataParameter dbDataParameter2 = parameter5;
                nullable = node1.LayerID;
                // ISSUE: variable of a boxed type
                __Boxed<int> local2 = (System.ValueType) nullable.Value;
                dbDataParameter2.Value = (object) local2;
                cmd1.Parameters.Add((object) parameter5);
              }
              cmd1.ExecuteNonQuery();
              nullable = node1.LayerID;
              int num4 = 0;
              if (!(nullable.GetValueOrDefault() > num4 & nullable.HasValue) || node1.NodeTyp != StructureNodeType.Meter)
              {
                cmd1.CommandText = "DELETE FROM NodeList WHERE NodeID=@NodeID;";
                cmd1.Parameters.Clear();
                IDbCommand cmd2 = cmd1;
                nullable = node1.NodeID;
                int num5 = nullable.Value;
                MeterDatabase.AddParameter(cmd2, "@NodeID", num5);
                cmd1.ExecuteNonQuery();
              }
            }
          }
          cmd1.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd1);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool ExistStructureForMeter(string serialnumber)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          return MeterDatabase.ExistStructureForMeter(cmd, serialnumber);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return true;
      }
    }

    private static bool ExistStructureForMeter(IDbCommand cmd, string serialnumber)
    {
      cmd.CommandText = "SELECT MeterID FROM Meter WHERE SerialNr=@SerialNr;";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@SerialNr", serialnumber);
      IDataReader dataReader1 = cmd.ExecuteReader();
      if (dataReader1.Read())
      {
        int int32 = dataReader1.GetInt32(0);
        if (dataReader1.Read())
          throw new Exception("Database table 'Meter' has invalid values! Serialnumber: " + serialnumber);
        dataReader1.Close();
        cmd.CommandText = "SELECT MeterID FROM NodeList WHERE MeterID=@MeterID;";
        cmd.Parameters.Clear();
        MeterDatabase.AddParameter(cmd, "@MeterID", int32);
        IDataReader dataReader2 = cmd.ExecuteReader();
        if (dataReader2.Read())
        {
          dataReader2.Close();
          return true;
        }
        dataReader2.Close();
      }
      else
        dataReader1.Close();
      return false;
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, DateTime? value)
    {
      if (value.HasValue)
      {
        MeterDatabase.AddParameter(cmd, parameterName, value.Value);
      }
      else
      {
        IDbDataParameter parameter = cmd.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.DbType = DbType.DateTime;
        parameter.Value = (object) DBNull.Value;
        cmd.Parameters.Add((object) parameter);
      }
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, DateTime value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.DateTime;
      parameter.Value = (object) new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, int value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Int32;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, Guid value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Guid;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, double value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Double;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, double? value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Double;
      if (value.HasValue)
        parameter.Value = (object) value.Value;
      else
        parameter.Value = (object) DBNull.Value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, bool? value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Boolean;
      if (value.HasValue)
        parameter.Value = (object) value.Value;
      else
        parameter.Value = (object) DBNull.Value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, string value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.String;
      if (value == null)
        parameter.Value = (object) DBNull.Value;
      else
        parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, byte[] value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Binary;
      if (value == null)
        parameter.Value = (object) DBNull.Value;
      else
        parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, Type type, object value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = MeterDatabase.TypeToDbType(type);
      if (value == null)
        parameter.Value = (object) DBNull.Value;
      else
        parameter.Value = value;
      cmd.Parameters.Add((object) parameter);
    }

    private static void AddParameter(IDbCommand cmd, string parameterName, Image value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Binary;
      parameter.Value = (object) Util.ImageToByte(value);
      cmd.Parameters.Add((object) parameter);
    }

    public static DbType TypeToDbType(Type t)
    {
      DbType dbType;
      try
      {
        dbType = !(t == typeof (byte[])) ? (DbType) Enum.Parse(typeof (DbType), t.Name, true) : DbType.Binary;
      }
      catch
      {
        dbType = DbType.Object;
      }
      return dbType;
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, bool value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Boolean;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static bool MeterReplacement(StructureTreeNode nodeOld, StructureTreeNode nodeNew)
    {
      if (nodeNew == null || nodeOld == null)
        return false;
      int? nullable = nodeOld.NodeID;
      if (!nullable.HasValue)
        return false;
      nullable = nodeOld.MeterID;
      if (!nullable.HasValue)
        return false;
      nullable = nodeNew.NodeID;
      if (!nullable.HasValue)
        return false;
      nullable = nodeNew.MeterID;
      if (!nullable.HasValue)
        return false;
      DateTime now = DateTime.Now;
      IDbCommand cmd1 = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd1 = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd1.Transaction = dbConnection.BeginTransaction();
          cmd1.CommandText = "UPDATE NodeList SET ValidTo=@ValidTo WHERE NodeID=@NodeID AND MeterID=@MeterID;";
          cmd1.Parameters.Clear();
          MeterDatabase.AddParameter(cmd1, "@ValidTo", now);
          IDbCommand cmd2 = cmd1;
          nullable = nodeOld.NodeID;
          int num1 = nullable.Value;
          MeterDatabase.AddParameter(cmd2, "@NodeID", num1);
          IDbCommand cmd3 = cmd1;
          nullable = nodeOld.MeterID;
          int num2 = nullable.Value;
          MeterDatabase.AddParameter(cmd3, "@MeterID", num2);
          if (cmd1.ExecuteNonQuery() != 1)
            return false;
          cmd1.CommandText = "UPDATE NodeList SET NodeID=@NodeIDNew, ValidTo=@ValidTo, NodeName=@NodeName WHERE NodeID=@NodeID AND MeterID=@MeterID;";
          cmd1.Parameters.Clear();
          IDbCommand cmd4 = cmd1;
          nullable = nodeOld.NodeID;
          int num3 = nullable.Value;
          MeterDatabase.AddParameter(cmd4, "@NodeIDNew", num3);
          IDbDataParameter parameter = cmd1.CreateParameter();
          parameter.ParameterName = "@ValidTo";
          parameter.DbType = DbType.DateTime;
          parameter.Value = (object) DBNull.Value;
          cmd1.Parameters.Add((object) parameter);
          MeterDatabase.AddParameter(cmd1, "@NodeName", nodeNew.Name);
          IDbCommand cmd5 = cmd1;
          nullable = nodeNew.NodeID;
          int num4 = nullable.Value;
          MeterDatabase.AddParameter(cmd5, "@NodeID", num4);
          IDbCommand cmd6 = cmd1;
          nullable = nodeNew.MeterID;
          int num5 = nullable.Value;
          MeterDatabase.AddParameter(cmd6, "@MeterID", num5);
          if (cmd1.ExecuteNonQuery() != 1)
            return false;
          cmd1.CommandText = "DELETE FROM NodeReferences WHERE NodeID=@NodeID AND ParentID=@ParentID AND LayerID=@LayerID;";
          cmd1.Parameters.Clear();
          IDbCommand cmd7 = cmd1;
          nullable = nodeNew.NodeID;
          int num6 = nullable.Value;
          MeterDatabase.AddParameter(cmd7, "@NodeID", num6);
          IDbCommand cmd8 = cmd1;
          nullable = nodeNew.Parent.NodeID;
          int num7 = nullable.Value;
          MeterDatabase.AddParameter(cmd8, "@ParentID", num7);
          IDbCommand cmd9 = cmd1;
          nullable = nodeNew.LayerID;
          int num8 = nullable.Value;
          MeterDatabase.AddParameter(cmd9, "@LayerID", num8);
          if (cmd1.ExecuteNonQuery() != 1)
            return false;
          nodeNew.NodeID = nodeOld.NodeID;
          nodeNew.Name = nodeOld.Name;
          nodeNew.NodeOrder = nodeOld.NodeOrder;
          nodeOld.Parent.Children.Remove(nodeOld);
          cmd1.Transaction.Commit();
          return true;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd1);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static int? GetDatabaseVersion() => MeterDatabase.GetDatabaseVersion(DbBasis.PrimaryDB);

    public static int? GetDatabaseVersion(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT InfoData FROM DatabaseIdentification WHERE InfoName = @InfoName;";
          MeterDatabase.AddParameter(cmd, "@InfoName", "DatabaseVersion");
          object obj = cmd.ExecuteScalar();
          return obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()) ? new int?(Convert.ToInt32(obj)) : new int?(1);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return new int?();
      }
    }

    public static string GetDatabaseVersionDate()
    {
      return MeterDatabase.GetDatabaseVersionDate(DbBasis.PrimaryDB);
    }

    public static string GetDatabaseVersionDate(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT InfoData FROM DatabaseIdentification WHERE InfoName = @InfoName;";
          MeterDatabase.AddParameter(cmd, "@InfoName", "DatabaseVersionDate");
          object obj = cmd.ExecuteScalar();
          return obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()) ? obj.ToString() : "01/01/2011";
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (string) null;
      }
    }

    public static bool ExistTable(DbBasis db, string tableName)
    {
      using (IDbConnection dbConnection = db.GetDbConnection())
      {
        dbConnection.Open();
        IDbCommand command = dbConnection.CreateCommand();
        try
        {
          command.CommandText = "SELECT CASE WHEN EXISTS((SELECT * FROM information_schema.tables WHERE table_name = '" + tableName + "')) THEN 1 ELSE 0 END";
          return (int) command.ExecuteScalar() == 1;
        }
        catch
        {
          try
          {
            command.CommandText = "SELECT 1 FROM " + tableName + " WHERE 1 = 0";
            command.ExecuteNonQuery();
            return true;
          }
          catch
          {
            return false;
          }
        }
      }
    }

    public bool TransferDataToEmptyDatabase(
      DbBasis oldDatabase,
      DbBasis newDatabase,
      string table,
      string filter)
    {
      if (oldDatabase == null)
        throw new ArgumentNullException("Input parameter 'oldDatabase' can not be null!");
      if (newDatabase == null)
        throw new ArgumentNullException("Input parameter 'newDatabase' can not be null!");
      if (string.IsNullOrEmpty(table))
        throw new ArgumentNullException("Input parameter 'table' can not be null!");
      IDbCommand cmd1 = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection1 = oldDatabase.GetDbConnection())
        {
          dbConnection1.Open();
          using (cmd1 = oldDatabase.DbCommand(dbConnection1))
          {
            string str1;
            string str2;
            if (string.IsNullOrEmpty(filter))
            {
              str1 = "SELECT COUNT(*) FROM " + table;
              str2 = "SELECT * FROM " + table;
            }
            else
            {
              str1 = "SELECT COUNT(*) FROM " + table + " WHERE " + filter;
              str2 = "SELECT * FROM " + table + " WHERE " + filter;
            }
            cmd1.CommandText = str1;
            object obj1 = cmd1.ExecuteScalar();
            if (obj1 == null || obj1 == DBNull.Value)
              return false;
            long int64 = Convert.ToInt64(obj1);
            if (int64 == 0L)
              return true;
            cmd1.CommandText = str2;
            IDataReader dataReader = cmd1.ExecuteReader();
            if (dataReader == null)
              return false;
            using (IDbConnection dbConnection2 = newDatabase.GetDbConnection())
            {
              dbConnection2.Open();
              IDbCommand cmd2;
              using (cmd2 = newDatabase.DbCommand(dbConnection2))
              {
                DataTable schemaTable = dataReader.GetSchemaTable();
                StringBuilder stringBuilder = new StringBuilder("INSERT INTO ");
                stringBuilder.Append(table).Append(" (");
                foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
                  stringBuilder.Append("[").Append(row["ColumnName"]).Append("]").Append(", ");
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                stringBuilder.Append(") VALUES (");
                foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
                {
                  stringBuilder.Append("@").Append(row["ColumnName"]).Append(", ");
                  MeterDatabase.AddParameter(cmd2, row["ColumnName"].ToString(), (Type) row["DataType"], (object) null);
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                stringBuilder.Append(");");
                cmd2.CommandText = stringBuilder.ToString();
                long num1 = 0;
                int num2 = 0;
                int num3 = 0;
                MeterDatabase.Progress e = new MeterDatabase.Progress()
                {
                  Count = int64,
                  Successful = num2,
                  Failed = 0,
                  ProgressValue = 0
                };
                DateTime now = DateTime.Now;
                while (dataReader.Read())
                {
                  ++num1;
                  for (int index = 0; index < dataReader.FieldCount; ++index)
                  {
                    object obj2 = dataReader.GetValue(index);
                    ((IDataParameter) cmd2.Parameters[schemaTable.Rows[index]["ColumnName"].ToString()]).Value = obj2;
                  }
                  try
                  {
                    if (cmd2.ExecuteNonQuery() != 1)
                      ++num3;
                    else
                      ++num2;
                  }
                  catch (Exception ex)
                  {
                    MeterDatabase.logger.Error<Exception>(ex);
                    ++num3;
                  }
                  if (this.OnProgress != null)
                  {
                    int int32 = Convert.ToInt32((double) num1 / (double) int64 * 100.0);
                    if (e.ProgressValue != int32 || (DateTime.Now - now).TotalSeconds > 1.0)
                    {
                      e.ProgressValue = int32;
                      e.Successful = num2;
                      e.Failed = num3;
                      this.OnProgress((object) this, e);
                      now = DateTime.Now;
                    }
                  }
                }
                return true;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd1);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public bool TransferTableNodeList_1_to_2(DbBasis oldDatabase, DbBasis newDatabase)
    {
      if (oldDatabase == null)
        throw new ArgumentNullException("Input parameter 'oldDatabase' can not be null!");
      if (newDatabase == null)
        throw new ArgumentNullException("Input parameter 'newDatabase' can not be null!");
      IDbCommand cmd1 = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection1 = oldDatabase.GetDbConnection())
        {
          dbConnection1.Open();
          using (cmd1 = oldDatabase.DbCommand(dbConnection1))
          {
            string str1 = "SELECT COUNT(*) FROM NodeList WHERE NodeID > 99999999;";
            string str2 = "SELECT * FROM NodeList WHERE NodeID > 99999999;";
            cmd1.CommandText = str1;
            object obj = cmd1.ExecuteScalar();
            if (obj == null || obj == DBNull.Value)
              return false;
            long int64 = Convert.ToInt64(obj);
            if (int64 == 0L)
              return true;
            cmd1.CommandText = str2;
            IDataReader dataReader = cmd1.ExecuteReader();
            if (dataReader == null)
              return false;
            using (IDbConnection dbConnection2 = newDatabase.GetDbConnection())
            {
              dbConnection2.Open();
              using (IDbTransaction dbTransaction = dbConnection2.BeginTransaction())
              {
                using (IDbCommand cmd2 = newDatabase.DbCommand(dbConnection2))
                {
                  cmd2.CommandText = "INSERT INTO NodeList (NodeID, MeterID, NodeTypeID, NodeName, NodeDescription, NodeSettings, ValidFrom) VALUES (@NodeID, @MeterID, @NodeTypeID, @NodeName, @NodeDescription, @NodeSettings, @ValidFrom);";
                  MeterDatabase.AddParameter(cmd2, "@NodeID", 0);
                  MeterDatabase.AddParameter(cmd2, "@MeterID", 0);
                  MeterDatabase.AddParameter(cmd2, "@NodeTypeID", 0);
                  MeterDatabase.AddParameter(cmd2, "@NodeName", "");
                  MeterDatabase.AddParameter(cmd2, "@NodeDescription", "");
                  MeterDatabase.AddParameter(cmd2, "@NodeSettings", "");
                  MeterDatabase.AddParameter(cmd2, "@ValidFrom", DateTime.Now);
                  long num1 = 0;
                  int num2 = 0;
                  int num3 = 0;
                  MeterDatabase.Progress e = new MeterDatabase.Progress()
                  {
                    Count = int64,
                    Successful = num2,
                    Failed = 0,
                    ProgressValue = 0
                  };
                  DateTime now = DateTime.Now;
                  while (dataReader.Read())
                  {
                    ++num1;
                    int num4 = Convert.ToInt32(dataReader["NodeTypeID"]);
                    string str3 = dataReader["NodeSettings"].ToString().Replace("VALUE_REQ_ID;1;", string.Empty);
                    MBusDeviceType mbusDeviceType = MBusDeviceType.HEAT_OUTLET;
                    string newValue1 = mbusDeviceType.ToString();
                    string str4 = str3.Replace("HEAT_WARM", newValue1);
                    mbusDeviceType = MBusDeviceType.HEAT_INLET;
                    string newValue2 = mbusDeviceType.ToString();
                    string str5 = str4.Replace("HEAT_COLD", newValue2);
                    mbusDeviceType = MBusDeviceType.COOL_INLET;
                    string newValue3 = mbusDeviceType.ToString();
                    string settings = str5.Replace("COOL_COLD", newValue3);
                    int int32_1 = Convert.ToInt32(dataReader["ReadingType"]);
                    if ((num4 != 2 || settings.IndexOf("AsynchronSeriell") >= 0 || settings.Length <= 10) && num4 == 2 && settings.Length <= 50)
                      num4 = 3;
                    if (int32_1 == 14)
                      num4 = 3;
                    if (int32_1 == 15)
                      num4 = 11;
                    if (num4 == 1)
                      settings = ParameterService.AddOrUpdateParameter(settings, "READOUT_TYPE", "1");
                    ((IDataParameter) cmd2.Parameters["@NodeID"]).Value = dataReader["NodeID"];
                    ((IDataParameter) cmd2.Parameters["@MeterID"]).Value = dataReader["MeterID"];
                    ((IDataParameter) cmd2.Parameters["@NodeTypeID"]).Value = (object) num4;
                    ((IDataParameter) cmd2.Parameters["@NodeName"]).Value = dataReader["NodeName"];
                    ((IDataParameter) cmd2.Parameters["@NodeDescription"]).Value = (object) dataReader["NodeDescription"].ToString().Replace("-", "");
                    ((IDataParameter) cmd2.Parameters["@NodeSettings"]).Value = (object) settings;
                    if (cmd2.ExecuteNonQuery() != 1)
                      ++num3;
                    else
                      ++num2;
                    if (this.OnProgress != null)
                    {
                      int int32_2 = Convert.ToInt32((double) num1 / (double) int64 * 100.0);
                      if (e.ProgressValue != int32_2 || (DateTime.Now - now).TotalSeconds > 1.0)
                      {
                        e.ProgressValue = int32_2;
                        e.Successful = num2;
                        e.Failed = num3;
                        this.OnProgress((object) this, e);
                        now = DateTime.Now;
                      }
                    }
                  }
                }
                dbTransaction.Commit();
                return true;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd1);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public bool TransferTableMeterValues_1_to_2(DbBasis oldDatabase, DbBasis newDatabase)
    {
      if (oldDatabase == null)
        throw new ArgumentNullException("Input parameter 'oldDatabase' can not be null!");
      if (newDatabase == null)
        throw new ArgumentNullException("Input parameter 'newDatabase' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection1 = oldDatabase.GetDbConnection())
        {
          dbConnection1.Open();
          using (cmd = oldDatabase.DbCommand(dbConnection1))
          {
            string str1 = "SELECT COUNT(*) FROM MeterData;";
            string str2 = "SELECT * FROM MeterData;";
            cmd.CommandText = str1;
            object obj = cmd.ExecuteScalar();
            if (obj == null || obj == DBNull.Value)
              return false;
            long int64 = Convert.ToInt64(obj);
            if (int64 == 0L)
              return true;
            cmd.CommandText = str2;
            IDataReader dataReader1 = cmd.ExecuteReader();
            if (dataReader1 == null)
              return false;
            using (IDbConnection dbConnection2 = newDatabase.GetDbConnection())
            {
              dbConnection2.Open();
              using (IDbTransaction dbTransaction = dbConnection2.BeginTransaction())
              {
                using (IDbCommand dbCommand = newDatabase.DbCommand(dbConnection2))
                {
                  dbCommand.CommandText = "SELECT NodeID, MeterID FROM NodeList;";
                  SortedList<int, int> sortedList = new SortedList<int, int>();
                  using (IDataReader dataReader2 = dbCommand.ExecuteReader())
                  {
                    if (dataReader2 == null)
                      return false;
                    while (dataReader2.Read())
                    {
                      int int32_1 = Convert.ToInt32(dataReader2["MeterID"]);
                      int int32_2 = Convert.ToInt32(dataReader2["NodeID"]);
                      if (!sortedList.ContainsKey(int32_1))
                        sortedList.Add(int32_1, int32_2);
                    }
                  }
                  MeterDatabase.InitializeSQLQueryForMeterValues1_to_2(dbCommand);
                  long num1 = 0;
                  int num2 = 0;
                  int num3 = 0;
                  MeterDatabase.Progress e = new MeterDatabase.Progress()
                  {
                    Count = int64,
                    Successful = num2,
                    Failed = 0,
                    ProgressValue = 0
                  };
                  DateTime now = DateTime.Now;
                  while (dataReader1.Read())
                  {
                    ++num1;
                    int int32_3 = Convert.ToInt32(dataReader1["MeterId"]);
                    if (!sortedList.ContainsKey(int32_3))
                    {
                      ++num3;
                    }
                    else
                    {
                      int nodeID = sortedList[int32_3];
                      if (dataReader1["PValueID"] == DBNull.Value || dataReader1["PValueID"] == null)
                      {
                        ++num3;
                      }
                      else
                      {
                        int int32_4 = Convert.ToInt32(dataReader1["PValueID"]);
                        switch (int32_4)
                        {
                          case 1:
                            ++num2;
                            continue;
                          case 1000:
                            DateTime dateTime = Convert.ToDateTime(dataReader1["TimePoint"]);
                            if (this.UpdateNodeList(dbCommand, nodeID, int32_3, dateTime))
                              ++num2;
                            else
                              ++num3;
                            MeterDatabase.InitializeSQLQueryForMeterValues1_to_2(dbCommand);
                            continue;
                          case 1001:
                            ++num2;
                            continue;
                          default:
                            long? valueIdent = this.ConvertPValueToValueIdent(int32_4);
                            if (!valueIdent.HasValue)
                            {
                              MeterDatabase.logger.Error("Can not convert PValueID to ValueIdent! Value: " + int32_4.ToString());
                              ++num3;
                              continue;
                            }
                            long valueId = valueIdent.Value;
                            ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId);
                            byte num4 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId) / 4096UL);
                            byte num5 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId) / 65536UL);
                            byte num6 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId) / 268435456UL);
                            byte num7 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId) / 64UL);
                            byte num8 = (byte) ((ulong) physicalQuantity / 1UL);
                            byte num9 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId) / 4194304UL);
                            byte num10 = (byte) ((ulong) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdentError>(valueId) / 2147483648UL);
                            ((IDataParameter) dbCommand.Parameters["@MeterId"]).Value = (object) int32_3;
                            ((IDataParameter) dbCommand.Parameters["@TimePoint"]).Value = dataReader1["TimePoint"];
                            ((IDataParameter) dbCommand.Parameters["@NodeID"]).Value = (object) nodeID;
                            ((IDataParameter) dbCommand.Parameters["@Calculation"]).Value = (object) num4;
                            ((IDataParameter) dbCommand.Parameters["@CalculationStart"]).Value = (object) num5;
                            ((IDataParameter) dbCommand.Parameters["@Creation"]).Value = (object) num6;
                            ((IDataParameter) dbCommand.Parameters["@MeterType"]).Value = (object) num7;
                            ((IDataParameter) dbCommand.Parameters["@PhysicalQuantity"]).Value = (object) num8;
                            ((IDataParameter) dbCommand.Parameters["@StorageInterval"]).Value = (object) num9;
                            ((IDataParameter) dbCommand.Parameters["@Creation"]).Value = (object) num6;
                            if (physicalQuantity == ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber)
                            {
                              ((IDataParameter) dbCommand.Parameters["@Value"]).Value = (object) 1;
                            }
                            else
                            {
                              double num11 = double.Parse(dataReader1["PValue"].ToString(), (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                              ((IDataParameter) dbCommand.Parameters["@Value"]).Value = (object) num11;
                            }
                            try
                            {
                              if (dbCommand.ExecuteNonQuery() != 1)
                                ++num3;
                              else
                                ++num2;
                            }
                            catch (Exception ex)
                            {
                              MeterDatabase.LogFailedSQLQuery(dbCommand);
                              MeterDatabase.logger.Error(ex.Message);
                              ++num3;
                            }
                            if (this.OnProgress != null)
                            {
                              int int32_5 = Convert.ToInt32((double) num1 / (double) int64 * 100.0);
                              if (e.ProgressValue != int32_5 || (DateTime.Now - now).TotalSeconds > 1.0)
                              {
                                e.ProgressValue = int32_5;
                                e.Successful = num2;
                                e.Failed = num3;
                                this.OnProgress((object) this, e);
                                now = DateTime.Now;
                              }
                            }
                            continue;
                        }
                      }
                    }
                  }
                }
                dbTransaction.Commit();
                return true;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    private static void InitializeSQLQueryForMeterValues1_to_2(IDbCommand cmdNew)
    {
      cmdNew.CommandText = "INSERT INTO MeterValues (MeterId, ValueIdentIndex, TimePoint, [Value], PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation, NodeID) VALUES (@MeterId, @ValueIdentIndex, @TimePoint, @Value, @PhysicalQuantity, @MeterType, @Calculation, @CalculationStart, @StorageInterval, @Creation, @NodeID);";
      cmdNew.Parameters.Clear();
      MeterDatabase.AddParameter(cmdNew, "@MeterId", 0);
      MeterDatabase.AddParameter(cmdNew, "@ValueIdentIndex", 0);
      MeterDatabase.AddParameter(cmdNew, "@TimePoint", DateTime.Now);
      MeterDatabase.AddParameter(cmdNew, "@Value", 0.0);
      MeterDatabase.AddParameter(cmdNew, "@PhysicalQuantity", 0);
      MeterDatabase.AddParameter(cmdNew, "@MeterType", 0);
      MeterDatabase.AddParameter(cmdNew, "@Calculation", 0);
      MeterDatabase.AddParameter(cmdNew, "@CalculationStart", 0);
      MeterDatabase.AddParameter(cmdNew, "@StorageInterval", 0);
      MeterDatabase.AddParameter(cmdNew, "@Creation", 0);
      MeterDatabase.AddParameter(cmdNew, "@NodeID", 0);
    }

    private long? ConvertPValueToValueIdent(int PValueID)
    {
      switch (PValueID)
      {
        case 1:
          return new long?();
        case 1000:
          return new long?();
        case 1001:
          return new long?();
        case 1002:
          return new long?(541204495L);
        case 1010:
          return new long?();
        case 1014:
          return new long?(272769743L);
        case 1100:
        case 1215:
        case 1216:
          return new long?(272699714L);
        case 1101:
        case 1212:
        case 1213:
          return new long?(272699713L);
        case 1102:
          return new long?(541204803L);
        case 1103:
          return new long?(272769356L);
        case 1104:
          return new long?(272769357L);
        case 1105:
          return new long?(272769351L);
        case 1106:
          return new long?(541204804L);
        case 1107:
          return new long?(272699723L);
        case 1131:
          return new long?(272699778L);
        case 1133:
          return new long?(272769412L);
        case 1136:
          return new long?();
        case 1208:
        case 1257:
          return new long?(272699457L);
        case 1209:
        case 1214:
          return new long?(272699585L);
        case 1210:
          return new long?(272699521L);
        case 1211:
          return new long?(272699649L);
        case 1217:
          return new long?(272699586L);
        case 1218:
          return new long?(541204547L);
        case 1219:
          return new long?(541204675L);
        case 1220:
          return new long?(541204611L);
        case 1221:
          return new long?(541204739L);
        case 1223:
          return new long?(541204803L);
        case 1224:
          return new long?(541204803L);
        case 1225:
          return new long?(541204675L);
        case 1226:
          return new long?(541217092L);
        case 1227:
          return new long?(541217092L);
        case 1228:
          return new long?(541217092L);
        case 1229:
          return new long?(541216964L);
        case 1230:
          return new long?(272699841L);
        case 1231:
          return new long?(272769347L);
        case 1232:
          return new long?(272769348L);
        case 1233:
          return new long?(272773443L);
        case 1234:
          return new long?(272773444L);
        case 1235:
          return new long?(281088386L);
        case 1236:
          return new long?(281088322L);
        case 1237:
          return new long?(281088322L);
        case 1238:
          return new long?(281088194L);
        case 1239:
          return new long?(281088449L);
        case 1240:
          return new long?(281088257L);
        case 1241:
          return new long?(281088129L);
        case 1242:
          return new long?(281088321L);
        case 1243:
          return new long?(281088321L);
        case 1244:
          return new long?(281088193L);
        case 1245:
          return new long?(281088065L);
        case 1246:
          return new long?(281088193L);
        case 1247:
          return new long?(272699778L);
        case 1248:
          return new long?(272699714L);
        case 1249:
          return new long?(272699714L);
        case 1250:
          return new long?(272699586L);
        case 1251:
          return new long?(272699841L);
        case 1252:
          return new long?(272699649L);
        case 1253:
          return new long?(272699521L);
        case 1254:
          return new long?(272699713L);
        case 1255:
          return new long?(272699713L);
        case 1256:
          return new long?(272699585L);
        case 1258:
          return new long?(272699585L);
        case 1259:
          return new long?(281088321L);
        case 1260:
          return new long?(272699713L);
        case 1261:
          return new long?(281088322L);
        case 1262:
          return new long?(272699714L);
        case 1263:
          return new long?(272773444L);
        case 1264:
          return new long?(272773444L);
        case 1265:
          return new long?(272773444L);
        case 1266:
          return new long?(272699723L);
        case 1267:
          return new long?(272699723L);
        case 1268:
          return new long?(272699723L);
        case 1269:
          return new long?(272699723L);
        case 1270:
          return new long?(272699723L);
        case 1271:
          return new long?(281088331L);
        case 1272:
          return new long?(281088331L);
        case 1273:
          return new long?(281088331L);
        default:
          return new long?();
      }
    }

    public static SortedList<string, string> LoadOnlineTranslation(string language)
    {
      SortedList<string, string> sortedList = new SortedList<string, string>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        string lower = language.ToLower();
        bool flag = false;
        try
        {
          using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
          {
            newConnection.Open();
            while (true)
            {
              DbCommand command1 = newConnection.CreateCommand();
              command1.CommandText = "SELECT TextKey,LanguageText FROM OnlineTranslations WHERE LanguageCode = '" + lower + "' ORDER BY TextKey ASC";
              DbDataReader dbDataReader1 = command1.ExecuteReader();
              if (dbDataReader1 != null)
              {
                while (dbDataReader1.Read())
                {
                  string key = dbDataReader1["TextKey"].ToString();
                  string str = dbDataReader1["LanguageText"].ToString();
                  sortedList.Add(key, str);
                }
                dbDataReader1.Close();
                DbCommand command2 = newConnection.CreateCommand();
                command2.CommandText = "SELECT TextKey,LanguageText FROM OnlineTranslations WHERE LanguageCode = 'en' ORDER BY TextKey ASC";
                DbDataReader dbDataReader2 = command2.ExecuteReader();
                if (dbDataReader2 != null)
                {
                  while (dbDataReader2.Read())
                  {
                    string key = dbDataReader2["TextKey"].ToString();
                    string str = dbDataReader2["LanguageText"].ToString();
                    if (!sortedList.ContainsKey(key))
                      sortedList.Add(key, str);
                  }
                  dbDataReader2.Close();
                  if (sortedList.Count <= 0)
                  {
                    if (!flag)
                    {
                      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM OnlineTranslations", newConnection, out DbCommandBuilder _);
                      DataTable dataTable = new DataTable();
                      dataAdapter.Fill(dataTable);
                      DbCommand command3 = newConnection.CreateCommand();
                      command3.CommandText = "SELECT * FROM OnlineTranslation";
                      DbDataReader dbDataReader3 = command3.ExecuteReader();
                      if (dbDataReader3 != null)
                      {
                        while (dbDataReader3.Read())
                        {
                          string str1 = dbDataReader3["TextKey"].ToString();
                          string str2 = dbDataReader3["TextEN"].ToString();
                          string str3 = dbDataReader3["TextDE"].ToString();
                          if (!string.IsNullOrEmpty(str3))
                          {
                            DataRow row = dataTable.NewRow();
                            row["TextKey"] = (object) str1;
                            row["LanguageCode"] = (object) "de";
                            row["LanguageText"] = (object) str3;
                            dataTable.Rows.Add(row);
                          }
                          if (!string.IsNullOrEmpty(str2))
                          {
                            DataRow row = dataTable.NewRow();
                            row["TextKey"] = (object) str1;
                            row["LanguageCode"] = (object) "en";
                            row["LanguageText"] = (object) str2;
                            dataTable.Rows.Add(row);
                          }
                        }
                        dbDataReader3.Close();
                        if (dataTable.Rows.Count > 0)
                          dataAdapter.Update(dataTable);
                        flag = true;
                      }
                      else
                        goto label_29;
                    }
                    else
                      goto label_29;
                  }
                  else
                    break;
                }
                else
                  goto label_29;
              }
              else
                goto label_29;
            }
            return sortedList;
          }
        }
        catch
        {
        }
label_29:
        DbBasis primaryDb = DbBasis.PrimaryDB;
        if (primaryDb == null)
          throw new ArgumentNullException("Input parameter 'db' can not be null!");
        MeterDatabase.logger.Debug(nameof (LoadOnlineTranslation));
        string name1 = "TextKey";
        string name2 = "TextEN";
        string name3 = "Text" + language.ToUpper();
        string str4 = name3;
        if (str4 != name2)
          str4 = name2 + "," + name3;
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          cmd = primaryDb.DbCommand(dbConnection);
          cmd.CommandText = "SELECT " + name1 + ", " + str4 + " FROM OnlineTranslation ORDER BY TextKey ASC;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return sortedList;
          while (dataReader.Read())
          {
            string key = dataReader[name1].ToString();
            string str5 = dataReader[name3].ToString();
            if (str5 == "")
            {
              str5 = dataReader[name2].ToString();
              if (str5 == "")
                str5 = key;
            }
            sortedList.Add(key, str5);
          }
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
      }
      return sortedList;
    }

    public static bool DeleteOnlineTranslation(string keyToDelete)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM [OnlineTranslation] WHERE TextKey = @TextKey;";
          MeterDatabase.AddParameter(cmd, "@TextKey", keyToDelete);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static string GetOnlineTranslationText(
      string language,
      string GmmModule,
      string TextKey)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbCommand cmd = (IDbCommand) null;
      MeterDatabase.logger.Debug("LoadOnlineTranslation");
      string str1 = nameof (TextKey);
      string str2 = GmmModule + TextKey;
      string name = "Text" + language.ToUpper();
      string onlineTranslationText = "";
      try
      {
        if (primaryDb == null)
          throw new ArgumentNullException("Input parameter 'db' can not be null!");
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          cmd = primaryDb.DbCommand(dbConnection);
          cmd.CommandText = "SELECT + " + str1 + "," + name + " FROM OnlineTranslation WHERE " + str1 + " = '" + str2 + "'";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader != null)
          {
            if (dataReader.Read())
              onlineTranslationText = dataReader[name].ToString();
          }
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str3 = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str3, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str3);
      }
      return onlineTranslationText;
    }

    public static bool AddOrChangeOnlineTranslation(
      string language,
      string key,
      string newLanguageText)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      if (string.IsNullOrEmpty(key))
        return false;
      IDbCommand cmd = (IDbCommand) null;
      string str = "Text" + language.ToUpper();
      try
      {
        if (primaryDb == null)
          throw new ArgumentNullException("Input parameter 'db' can not be null!");
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          cmd = primaryDb.DbCommand(dbConnection);
          cmd.CommandText = "SELECT TextKey FROM OnlineTranslation WHERE TextKey=@TextKey;";
          MeterDatabase.AddParameter(cmd, "@TextKey", key);
          object obj = cmd.ExecuteScalar();
          cmd.Parameters.Clear();
          if (obj != null && obj != DBNull.Value)
          {
            cmd.CommandText = "UPDATE OnlineTranslation SET " + str + "=@TextLanguage WHERE TextKey=@TextKey;";
            MeterDatabase.AddParameter(cmd, "@TextLanguage", newLanguageText);
            MeterDatabase.AddParameter(cmd, "@TextKey", key);
          }
          else
          {
            cmd.CommandText = "INSERT INTO OnlineTranslation (TextKey, " + str + ") VALUES (@TextKey, @TextLanguage)";
            MeterDatabase.AddParameter(cmd, "@TextKey", key);
            MeterDatabase.AddParameter(cmd, "@TextLanguage", newLanguageText);
          }
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        return false;
      }
    }

    private static string CreateFilterSQLStatement(List<long> filterValueIdent)
    {
      if (filterValueIdent == null || filterValueIdent.Count <= 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (long num in filterValueIdent)
      {
        if (stringBuilder.Length == 0)
          stringBuilder.Append("AND (");
        else
          stringBuilder.Append("OR ");
        List<long> valueIdents = new List<long>();
        valueIdents.Add(num);
        List<byte> values1 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueIdents);
        List<byte> values2 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueIdents);
        List<byte> values3 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueIdents);
        List<byte> values4 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueIdents);
        List<byte> values5 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueIdents);
        List<byte> values6 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueIdents);
        List<byte> values7 = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Index>(valueIdents);
        bool flag1 = values1.Count > 0 && !values1.Contains((byte) 0);
        bool flag2 = values2.Count > 0 && !values2.Contains((byte) 0);
        bool flag3 = values3.Count > 0 && !values3.Contains((byte) 0);
        bool flag4 = values4.Count > 0 && !values4.Contains((byte) 0);
        bool flag5 = values5.Count > 0 && !values5.Contains((byte) 0);
        bool flag6 = values6.Count > 0 && !values6.Contains((byte) 0);
        bool flag7 = values7.Count > 0 && !values7.Contains((byte) 0);
        if (flag1)
        {
          stringBuilder.Append(" (");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("PhysicalQuantity", values1));
        }
        if (flag2)
        {
          if (stringBuilder.Length == 0 | flag1)
            stringBuilder.Append("AND ");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("MeterType", values2));
        }
        if (flag3)
        {
          if (stringBuilder.Length == 0 | flag1 | flag2)
            stringBuilder.Append("AND ");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("Calculation", values3));
        }
        if (flag4)
        {
          if (stringBuilder.Length == 0 | flag1 | flag2 | flag3)
            stringBuilder.Append("AND ");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("CalculationStart", values4));
        }
        if (flag5)
        {
          if (stringBuilder.Length == 0 | flag1 | flag2 | flag3 | flag4)
            stringBuilder.Append("AND ");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("StorageInterval", values5));
        }
        if (flag6)
        {
          if (stringBuilder.Length == 0 | flag1 | flag2 | flag3 | flag4 | flag5)
            stringBuilder.Append("AND ");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("Creation", values6));
        }
        if (flag7)
        {
          if (stringBuilder.Length == 0 | flag1 | flag2 | flag3 | flag4 | flag5 | flag6)
            stringBuilder.Append("AND ");
          stringBuilder.Append(MeterDatabase.CreateWhereStatementForValueIdent_V2("ValueIdentIndex", values7));
        }
        stringBuilder.Append(") ");
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString().Trim();
    }

    public static List<long> LoadAvailableValueIdentsOfNode(
      StructureTreeNode node,
      List<long> filter)
    {
      if (node == null)
        throw new ArgumentNullException("Input parameter 'node' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          StringBuilder stringBuilder = new StringBuilder();
          List<long> longList = new List<long>();
          int num = 0;
          string filterSqlStatement = MeterDatabase.CreateFilterSQLStatement(filter);
          foreach (StructureTreeNode structureTreeNode in StructureTreeNode.ForEach(node))
          {
            if (structureTreeNode.MeterID.HasValue && structureTreeNode.MeterID.Value != 0)
            {
              if (stringBuilder.Length == 0)
                stringBuilder.Append("MeterID=@MeterID").Append(num);
              else
                stringBuilder.Append(" OR MeterID=@MeterID").Append(num);
              MeterDatabase.AddParameter(cmd, "@MeterID" + num.ToString(), structureTreeNode.MeterID.Value);
              if (num >= 400)
              {
                cmd.CommandText = string.Format("SELECT DISTINCT ValueIdentIndex, PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation FROM MeterValues WHERE {0} {1};", (object) stringBuilder.ToString(), (object) filterSqlStatement);
                cmd.CommandTimeout = 3000;
                IDataReader dataReader = cmd.ExecuteReader();
                if (dataReader == null)
                  return (List<long>) null;
                while (dataReader.Read())
                {
                  byte calculation = Convert.ToByte(dataReader["Calculation"]);
                  byte calculationStart = Convert.ToByte(dataReader["CalculationStart"]);
                  byte creation = Convert.ToByte(dataReader["Creation"]);
                  byte meterType = Convert.ToByte(dataReader["MeterType"]);
                  byte physicalQuantity = Convert.ToByte(dataReader["PhysicalQuantity"]);
                  byte storageInterval = Convert.ToByte(dataReader["StorageInterval"]);
                  long valueIdent = ValueIdent.GetValueIdent(Convert.ToByte(dataReader["ValueIdentIndex"]), physicalQuantity, meterType, calculation, calculationStart, storageInterval, creation);
                  if (!longList.Contains(valueIdent))
                    longList.Add(valueIdent);
                }
                dataReader.Close();
                cmd.Parameters.Clear();
                stringBuilder.Length = 0;
                num = 0;
              }
              else
                ++num;
            }
          }
          cmd.CommandText = string.Format("SELECT DISTINCT ValueIdentIndex, PhysicalQuantity, MeterType, Calculation, CalculationStart, StorageInterval, Creation FROM MeterValues WHERE {0} {1};", (object) stringBuilder.ToString(), (object) filterSqlStatement);
          MeterDatabase.logger.Trace(cmd.CommandText);
          cmd.CommandTimeout = 3000;
          IDataReader dataReader1 = cmd.ExecuteReader();
          if (dataReader1 == null)
            return (List<long>) null;
          while (dataReader1.Read())
          {
            byte calculation = Convert.ToByte(dataReader1["Calculation"]);
            byte calculationStart = Convert.ToByte(dataReader1["CalculationStart"]);
            byte creation = Convert.ToByte(dataReader1["Creation"]);
            byte meterType = Convert.ToByte(dataReader1["MeterType"]);
            byte physicalQuantity = Convert.ToByte(dataReader1["PhysicalQuantity"]);
            byte storageInterval = Convert.ToByte(dataReader1["StorageInterval"]);
            long valueIdent = ValueIdent.GetValueIdent(Convert.ToByte(dataReader1["ValueIdentIndex"]), physicalQuantity, meterType, calculation, calculationStart, storageInterval, creation);
            if (!longList.Contains(valueIdent))
              longList.Add(valueIdent);
          }
          return longList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<long>) null;
      }
    }

    public static List<MeterValueRow> LoadConsumptionOfMeterPerMonth(
      StructureTreeNode node,
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity,
      DateTime start,
      DateTime end)
    {
      if (!node.MeterID.HasValue)
        throw new ArgumentNullException("Input parameter 'node.MeterID' can not be null!");
      byte num1 = (byte) ((ulong) physicalQuantity / 1UL);
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT TimePoint, [Value], MeterType FROM MeterValues WHERE TimePoint BETWEEN @start AND @end AND MeterId=@MeterId AND PhysicalQuantity=@PhysicalQuantity AND Calculation=@Calculation AND CalculationStart=@CalculationStart AND ValueIdentIndex=@ValueIdentIndex ORDER BY TimePoint ASC;";
          MeterDatabase.AddParameter(cmd, "@start", start);
          MeterDatabase.AddParameter(cmd, "@end", end);
          MeterDatabase.AddParameter(cmd, "@MeterId", node.MeterID.Value);
          MeterDatabase.AddParameter(cmd, "@PhysicalQuantity", (int) num1);
          MeterDatabase.AddParameter(cmd, "@Calculation", 1);
          MeterDatabase.AddParameter(cmd, "@CalculationStart", 1);
          MeterDatabase.AddParameter(cmd, "@ValueIdentIndex", 0);
          cmd.CommandTimeout = 100;
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterValueRow>) null;
          Dictionary<DateTime, double> dictionary = new Dictionary<DateTime, double>();
          DateTime dateTime1 = DateTime.MinValue;
          double num2 = 0.0;
          bool flag1 = true;
          byte? nullable = new byte?();
          while (dataReader.Read())
          {
            DateTime dateTime2 = Convert.ToDateTime(dataReader["TimePoint"]);
            double num3 = Convert.ToDouble(dataReader["Value"]);
            if (!nullable.HasValue)
              nullable = new byte?(Convert.ToByte(dataReader["MeterType"]));
            if (flag1)
            {
              dateTime1 = dateTime2;
              num2 = num3;
              flag1 = false;
            }
            else if (dateTime2.Year != dateTime1.Year || dateTime2.Month != dateTime1.Month || dateTime2.Day != dateTime1.Day)
            {
              if (dateTime2.Year <= dateTime1.Year && dateTime2.Month <= dateTime1.Month && dateTime2.Day <= dateTime1.Day)
                throw new Exception("Internal error! Not defined state detected!");
              dictionary.Add(new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day), num2);
              dateTime1 = dateTime2;
              num2 = num3;
            }
          }
          List<MeterValueRow> meterValueRowList = new List<MeterValueRow>();
          if (dictionary.Count > 0)
          {
            dictionary.Add(new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day), num2);
            List<MeterDatabase.DayValue> dayValueList = new List<MeterDatabase.DayValue>();
            DateTime dateTime3 = DateTime.MinValue;
            foreach (KeyValuePair<DateTime, double> keyValuePair in dictionary)
            {
              if (dayValueList.Count == 0)
              {
                dateTime3 = keyValuePair.Key;
                dayValueList.Add(new MeterDatabase.DayValue(keyValuePair.Key, keyValuePair.Value, false));
              }
              else
              {
                DateTime dateTime4 = dateTime3.AddDays(1.0);
                DateTime key1 = keyValuePair.Key;
                if (key1.Year > dateTime4.Year || key1.Month > dateTime4.Month || key1.Day > dateTime4.Day)
                {
                  double num4 = dictionary[dateTime3];
                  double num5 = keyValuePair.Value;
                  DateTime key2 = keyValuePair.Key;
                  double num6 = num5 - num4;
                  TimeSpan timeSpan = key2 - dateTime3;
                  double num7 = num6 / timeSpan.TotalDays;
                  for (int index = 0; (double) index < timeSpan.TotalDays - 1.0; ++index)
                  {
                    dateTime3 = dateTime3.AddDays(1.0);
                    num4 += num7;
                    dayValueList.Add(new MeterDatabase.DayValue(dateTime3, num4, true));
                  }
                  dayValueList.Add(new MeterDatabase.DayValue(keyValuePair.Key, keyValuePair.Value, false));
                  dateTime3 = dateTime3.AddDays(1.0);
                }
                else
                {
                  dayValueList.Add(new MeterDatabase.DayValue(keyValuePair.Key, keyValuePair.Value, false));
                  dateTime3 = dateTime3.AddDays(1.0);
                }
              }
            }
            MeterDatabase.DayValue dayValue1 = dayValueList[0];
            MeterDatabase.DayValue dayValue2 = dayValue1;
            for (int index = 0; index < dayValueList.Count; ++index)
            {
              MeterDatabase.DayValue dayValue3 = dayValueList[index];
              if (dayValue3.Timepoint.Year > dayValue1.Timepoint.Year || dayValue3.Timepoint.Month > dayValue1.Timepoint.Month || dayValueList.Count - 1 == index)
              {
                double num8 = Math.Round(dayValue3.Value - dayValue1.Value, 3);
                bool flag2 = dayValue1.IsEstimated || dayValue3.IsEstimated;
                meterValueRowList.Add(new MeterValueRow()
                {
                  Calculation = (byte) 6,
                  CalculationStart = (byte) 6,
                  Creation = flag2 ? (byte) 3 : (byte) 2,
                  StorageInterval = (byte) 5,
                  MeterId = node.MeterID.Value,
                  MeterType = nullable.Value,
                  NodeName = node.Name,
                  PhysicalQuantity = num1,
                  SerialNr = node.SerialNumber,
                  TimePoint = dayValue2.Timepoint,
                  Value = num8
                });
                dayValue1 = dayValue3;
              }
              else
                dayValue2 = dayValue3;
            }
          }
          return meterValueRowList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterValueRow>) null;
      }
    }

    public static List<MeterValueRow> LoadConsumptionOfMeterPerDay(
      StructureTreeNode node,
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity,
      DateTime start,
      DateTime end)
    {
      if (!node.MeterID.HasValue)
        throw new ArgumentNullException("Input parameter 'node.MeterID' can not be null!");
      byte num1 = (byte) ((ulong) physicalQuantity / 1UL);
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT TimePoint, [Value], MeterType FROM MeterValues WHERE TimePoint BETWEEN @start AND @end AND MeterId=@MeterId AND PhysicalQuantity=@PhysicalQuantity AND Calculation=@Calculation AND CalculationStart=@CalculationStart AND ValueIdentIndex=@ValueIdentIndex ORDER BY TimePoint ASC;";
          MeterDatabase.AddParameter(cmd, "@start", start);
          MeterDatabase.AddParameter(cmd, "@end", end);
          MeterDatabase.AddParameter(cmd, "@MeterId", node.MeterID.Value);
          MeterDatabase.AddParameter(cmd, "@PhysicalQuantity", (int) num1);
          MeterDatabase.AddParameter(cmd, "@Calculation", 1);
          MeterDatabase.AddParameter(cmd, "@CalculationStart", 1);
          MeterDatabase.AddParameter(cmd, "@ValueIdentIndex", 0);
          cmd.CommandTimeout = 100;
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterValueRow>) null;
          Dictionary<DateTime, double> dictionary = new Dictionary<DateTime, double>();
          DateTime dateTime1 = DateTime.MinValue;
          double num2 = 0.0;
          bool flag1 = true;
          byte? nullable = new byte?();
          while (dataReader.Read())
          {
            DateTime dateTime2 = Convert.ToDateTime(dataReader["TimePoint"]);
            double num3 = Convert.ToDouble(dataReader["Value"]);
            if (!nullable.HasValue)
              nullable = new byte?(Convert.ToByte(dataReader["MeterType"]));
            if (flag1)
            {
              dateTime1 = dateTime2;
              num2 = num3;
              flag1 = false;
            }
            else if (dateTime2.Year != dateTime1.Year || dateTime2.Month != dateTime1.Month || dateTime2.Day != dateTime1.Day)
            {
              if (dateTime2.Year <= dateTime1.Year && dateTime2.Month <= dateTime1.Month && dateTime2.Day <= dateTime1.Day)
                throw new Exception("Internal error! Not defined state detected!");
              dictionary.Add(new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day), num2);
              dateTime1 = dateTime2;
              num2 = num3;
            }
          }
          List<MeterValueRow> meterValueRowList = new List<MeterValueRow>();
          if (dictionary.Count > 0)
          {
            dictionary.Add(new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day), num2);
            List<MeterDatabase.DayValue> dayValueList = new List<MeterDatabase.DayValue>();
            DateTime dateTime3 = DateTime.MinValue;
            foreach (KeyValuePair<DateTime, double> keyValuePair in dictionary)
            {
              if (dayValueList.Count == 0)
              {
                dateTime3 = keyValuePair.Key;
                dayValueList.Add(new MeterDatabase.DayValue(keyValuePair.Key, keyValuePair.Value, false));
              }
              else
              {
                DateTime dateTime4 = dateTime3.AddDays(1.0);
                DateTime key1 = keyValuePair.Key;
                if (key1.Year > dateTime4.Year || key1.Month > dateTime4.Month || key1.Day > dateTime4.Day)
                {
                  double num4 = dictionary[dateTime3];
                  double num5 = keyValuePair.Value;
                  DateTime key2 = keyValuePair.Key;
                  double num6 = num5 - num4;
                  TimeSpan timeSpan = key2 - dateTime3;
                  double num7 = num6 / timeSpan.TotalDays;
                  for (int index = 0; (double) index < timeSpan.TotalDays - 1.0; ++index)
                  {
                    dateTime3 = dateTime3.AddDays(1.0);
                    num4 += num7;
                    dayValueList.Add(new MeterDatabase.DayValue(dateTime3, num4, true));
                  }
                  dayValueList.Add(new MeterDatabase.DayValue(keyValuePair.Key, keyValuePair.Value, false));
                  dateTime3 = dateTime3.AddDays(1.0);
                }
                else
                {
                  dayValueList.Add(new MeterDatabase.DayValue(keyValuePair.Key, keyValuePair.Value, false));
                  dateTime3 = dateTime3.AddDays(1.0);
                }
              }
            }
            MeterDatabase.DayValue dayValue1 = dayValueList[0];
            for (int index = 1; index < dayValueList.Count; ++index)
            {
              MeterDatabase.DayValue dayValue2 = dayValueList[index];
              double num8 = Math.Round(dayValue2.Value - dayValue1.Value, 3);
              bool flag2 = dayValue1.IsEstimated || dayValue2.IsEstimated;
              meterValueRowList.Add(new MeterValueRow()
              {
                Calculation = (byte) 6,
                CalculationStart = (byte) 9,
                Creation = flag2 ? (byte) 3 : (byte) 2,
                StorageInterval = (byte) 8,
                MeterId = node.MeterID.Value,
                MeterType = nullable.Value,
                NodeName = node.Name,
                PhysicalQuantity = num1,
                SerialNr = node.SerialNumber,
                TimePoint = dayValue1.Timepoint,
                Value = num8
              });
              dayValue1 = dayValue2;
            }
          }
          return meterValueRowList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterValueRow>) null;
      }
    }

    public static bool AddTranslationRule(TranslationRule newRule)
    {
      if (newRule == null)
        return false;
      newRule.Medium = TranslationRule.CorrectMedium(newRule.Medium);
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO TranslationRules (Manufacturer, Medium, VersionMin, VersionMax, MBusZDF, ValueIdent, RuleOrder, Multiplier, SpecialTranslation, SubDeviceAttributeIdentifier, SubDeviceIndex, StorageTimeParam, StorageTimeTranslation) VALUES (@Manufacturer, @Medium, @VersionMin, @VersionMax, @MBusZDF, @ValueIdent, @RuleOrder, @Multiplier, @SpecialTranslation, @SubDeviceAttributeIdentifier, @SubDeviceIndex, @StorageTimeParam, @StorageTimeTranslation);";
          MeterDatabase.AddParameter(cmd, "@Manufacturer", newRule.Manufacturer);
          MeterDatabase.AddParameter(cmd, "@Medium", newRule.Medium);
          MeterDatabase.AddParameter(cmd, "@VersionMin", newRule.VersionMin);
          MeterDatabase.AddParameter(cmd, "@VersionMax", newRule.VersionMax);
          MeterDatabase.AddParameter(cmd, "@MBusZDF", newRule.MBusZDF);
          MeterDatabase.AddParameter(cmd, "@ValueIdent", newRule.ValueIdent.ToString());
          MeterDatabase.AddParameter(cmd, "@RuleOrder", newRule.RuleOrder);
          MeterDatabase.AddParameter(cmd, "@Multiplier", newRule.Multiplier);
          MeterDatabase.AddParameter(cmd, "@SpecialTranslation", Convert.ToInt32((object) newRule.SpecialTranslation));
          MeterDatabase.AddParameter(cmd, "@SubDeviceAttributeIdentifier", newRule.SubDeviceAttributeIdentifier);
          MeterDatabase.AddParameter(cmd, "@SubDeviceIndex", newRule.SubDeviceIndex);
          MeterDatabase.AddParameter(cmd, "@StorageTimeParam", newRule.StorageTimeParam);
          MeterDatabase.AddParameter(cmd, "@StorageTimeTranslation", (int) newRule.StorageTimeTranslation);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static List<TranslationRule> LoadTranslationRules()
    {
      if (DbBasis.PrimaryDB == null)
        return (List<TranslationRule>) null;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM TranslationRules ORDER BY Manufacturer;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<TranslationRule>) null;
          List<TranslationRule> translationRuleList = new List<TranslationRule>();
          while (dataReader.Read())
          {
            double num1 = 1.0;
            if (dataReader["Multiplier"] != DBNull.Value)
              num1 = Convert.ToDouble(dataReader["Multiplier"]);
            SpecialTranslation specialTranslation = SpecialTranslation.None;
            if (dataReader["SpecialTranslation"] != DBNull.Value)
            {
              int int32 = Convert.ToInt32(dataReader["SpecialTranslation"]);
              if (Enum.IsDefined(typeof (SpecialTranslation), (object) int32))
                specialTranslation = (SpecialTranslation) Enum.Parse(typeof (SpecialTranslation), int32.ToString(), true);
            }
            string empty1 = string.Empty;
            if (dataReader["SubDeviceAttributeIdentifier"] != DBNull.Value)
              empty1 = dataReader["SubDeviceAttributeIdentifier"].ToString();
            int num2 = 0;
            if (dataReader["SubDeviceIndex"] != DBNull.Value)
              num2 = Convert.ToInt32(dataReader["SubDeviceIndex"]);
            string empty2 = string.Empty;
            if (dataReader["StorageTimeParam"] != DBNull.Value)
              empty2 = dataReader["StorageTimeParam"].ToString();
            SpecialStorageTimeTranslation storageTimeTranslation = SpecialStorageTimeTranslation.None;
            if (dataReader["StorageTimeTranslation"] != DBNull.Value)
              storageTimeTranslation = (SpecialStorageTimeTranslation) Enum.Parse(typeof (SpecialStorageTimeTranslation), dataReader["StorageTimeTranslation"].ToString(), true);
            translationRuleList.Add(new TranslationRule()
            {
              Manufacturer = dataReader["Manufacturer"].ToString(),
              Medium = dataReader["Medium"].ToString(),
              VersionMin = Convert.ToInt32(dataReader["VersionMin"]),
              VersionMax = Convert.ToInt32(dataReader["VersionMax"]),
              MBusZDF = dataReader["MBusZDF"].ToString(),
              ValueIdent = Convert.ToInt64(dataReader["ValueIdent"]),
              RuleOrder = Convert.ToInt32(dataReader["RuleOrder"]),
              Multiplier = num1,
              SpecialTranslation = specialTranslation,
              SubDeviceAttributeIdentifier = empty1,
              SubDeviceIndex = num2,
              StorageTimeParam = empty2,
              StorageTimeTranslation = storageTimeTranslation
            });
          }
          return translationRuleList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<TranslationRule>) null;
      }
    }

    public static bool UpdateTranslationRule(TranslationRule oldRule, TranslationRule newRule)
    {
      if (oldRule == null)
        throw new ArgumentNullException("Input parameter 'oldRule' can not be null!");
      if (newRule == null)
        throw new ArgumentNullException("Input parameter 'newRule' can not be null!");
      IDbCommand cmd1 = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd1 = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd1.CommandText = "UPDATE TranslationRules SET Manufacturer=@Manufacturer, Medium=@Medium, VersionMin=@VersionMin, VersionMax=@VersionMax, MBusZDF=@MBusZDF, ValueIdent=@ValueIdent, RuleOrder=@RuleOrder, Multiplier=@Multiplier, SpecialTranslation=@SpecialTranslation, SubDeviceAttributeIdentifier=@SubDeviceAttributeIdentifier, SubDeviceIndex=@SubDeviceIndex, StorageTimeParam=@StorageTimeParam, StorageTimeTranslation=@StorageTimeTranslation WHERE Manufacturer=@Manufacturer1 AND Medium=@Medium1 AND VersionMin=@VersionMin1 AND VersionMax=@VersionMax1 AND MBusZDF=@MBusZDF1 AND ValueIdent=@ValueIdent1 AND RuleOrder=@RuleOrder1;";
          MeterDatabase.AddParameter(cmd1, "@Manufacturer", newRule.Manufacturer);
          MeterDatabase.AddParameter(cmd1, "@Medium", newRule.Medium);
          MeterDatabase.AddParameter(cmd1, "@VersionMin", newRule.VersionMin);
          MeterDatabase.AddParameter(cmd1, "@VersionMax", newRule.VersionMax);
          MeterDatabase.AddParameter(cmd1, "@MBusZDF", newRule.MBusZDF);
          IDbCommand cmd2 = cmd1;
          long valueIdent = newRule.ValueIdent;
          string str1 = valueIdent.ToString();
          MeterDatabase.AddParameter(cmd2, "@ValueIdent", str1);
          MeterDatabase.AddParameter(cmd1, "@RuleOrder", newRule.RuleOrder);
          MeterDatabase.AddParameter(cmd1, "@Multiplier", newRule.Multiplier);
          MeterDatabase.AddParameter(cmd1, "@SpecialTranslation", Convert.ToInt32((object) newRule.SpecialTranslation));
          MeterDatabase.AddParameter(cmd1, "@SubDeviceAttributeIdentifier", newRule.SubDeviceAttributeIdentifier);
          MeterDatabase.AddParameter(cmd1, "@SubDeviceIndex", newRule.SubDeviceIndex);
          MeterDatabase.AddParameter(cmd1, "@StorageTimeParam", newRule.StorageTimeParam);
          MeterDatabase.AddParameter(cmd1, "@StorageTimeTranslation", (int) newRule.StorageTimeTranslation);
          MeterDatabase.AddParameter(cmd1, "@Manufacturer1", oldRule.Manufacturer);
          MeterDatabase.AddParameter(cmd1, "@Medium1", oldRule.Medium);
          MeterDatabase.AddParameter(cmd1, "@VersionMin1", oldRule.VersionMin);
          MeterDatabase.AddParameter(cmd1, "@VersionMax1", oldRule.VersionMax);
          MeterDatabase.AddParameter(cmd1, "@MBusZDF1", oldRule.MBusZDF);
          IDbCommand cmd3 = cmd1;
          valueIdent = oldRule.ValueIdent;
          string str2 = valueIdent.ToString();
          MeterDatabase.AddParameter(cmd3, "@ValueIdent1", str2);
          MeterDatabase.AddParameter(cmd1, "@RuleOrder1", oldRule.RuleOrder);
          return cmd1.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd1);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool DeleteTranslationRule(TranslationRule oldRule)
    {
      if (oldRule == null)
        throw new ArgumentNullException("Input parameter 'oldRule' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM TranslationRules WHERE Manufacturer=@Manufacturer AND Medium=@Medium AND VersionMin=@VersionMin AND VersionMax=@VersionMax AND MBusZDF=@MBusZDF AND ValueIdent=@ValueIdent AND RuleOrder=@RuleOrder;";
          MeterDatabase.AddParameter(cmd, "@Manufacturer", oldRule.Manufacturer);
          MeterDatabase.AddParameter(cmd, "@Medium", oldRule.Medium);
          MeterDatabase.AddParameter(cmd, "@VersionMin", oldRule.VersionMin);
          MeterDatabase.AddParameter(cmd, "@VersionMax", oldRule.VersionMax);
          MeterDatabase.AddParameter(cmd, "@MBusZDF", oldRule.MBusZDF);
          MeterDatabase.AddParameter(cmd, "@ValueIdent", oldRule.ValueIdent.ToString());
          MeterDatabase.AddParameter(cmd, "@RuleOrder", oldRule.RuleOrder);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static ReadoutType AddReadoutType(string readoutDeviceType)
    {
      return MeterDatabase.AddReadoutType(readoutDeviceType, 0, string.Empty);
    }

    public static ReadoutType AddReadoutType(
      string readoutDeviceType,
      int readoutSettingsID,
      string imageIdList)
    {
      if (string.IsNullOrEmpty(readoutDeviceType))
        throw new ArgumentNullException("Input parameter 'readoutDeviceType' can not be null or empty!");
      if (!string.IsNullOrEmpty(Ot.GetKey("ReadoutDeviceTypeID", readoutDeviceType.ToString())))
        return (ReadoutType) null;
      IDbCommand cmd = (IDbCommand) null;
      int readoutDeviceTypeID = 1;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT MAX(ReadoutDeviceTypeID) FROM ReadoutType;";
          object obj = cmd.ExecuteScalar();
          if (obj != null && obj != DBNull.Value)
            readoutDeviceTypeID = Convert.ToInt32(obj) + 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ReadoutType) null;
      }
      return MeterDatabase.AddReadoutType(readoutDeviceTypeID, readoutSettingsID, imageIdList);
    }

    public static ReadoutType AddReadoutType(
      int readoutDeviceTypeID,
      int readoutSettingsID,
      string imageIdList)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO ReadoutType (ReadoutDeviceTypeID, ReadoutSettingsID, ImageIdList) VALUES (@ReadoutDeviceTypeID, @ReadoutSettingsID, @ImageIdList);";
          MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", readoutDeviceTypeID);
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          MeterDatabase.AddParameter(cmd, "@ImageIdList", string.IsNullOrEmpty(imageIdList) ? "" : imageIdList);
          if (cmd.ExecuteNonQuery() != 1)
            return (ReadoutType) null;
          return new ReadoutType()
          {
            ReadoutDeviceTypeID = readoutDeviceTypeID,
            ReadoutSettingsID = readoutSettingsID,
            ImageIdList = imageIdList
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ReadoutType) null;
      }
    }

    public static bool DeleteReadoutTypeBySettingsID(int readoutSettingsID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM ReadoutType WHERE ReadoutSettingsID = @ReadoutSettingsID;";
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool DeleteReadoutType(int readoutDeviceTypeID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM ReadoutType WHERE ReadoutDeviceTypeID = @ReadoutDeviceTypeID;";
          MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", readoutDeviceTypeID);
          Ot.Delete("ReadoutDeviceTypeID", readoutDeviceTypeID.ToString());
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool DeleteReadoutType(ReadoutType item)
    {
      if (item == null)
        throw new ArgumentNullException("Input parameter 'item' can not be null!");
      return MeterDatabase.DeleteReadoutType(item.ReadoutDeviceTypeID, item.ReadoutSettingsID);
    }

    public static bool DeleteReadoutType(int readoutDeviceTypeID, int readoutSettingsID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM ReadoutType WHERE ReadoutDeviceTypeID=@ReadoutDeviceTypeID AND ReadoutSettingsID=@ReadoutSettingsID;";
          MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", readoutDeviceTypeID);
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static List<ReadoutType> LoadReadoutType()
    {
      return MeterDatabase.LoadReadoutType(DbBasis.PrimaryDB, new int?());
    }

    public static List<ReadoutType> LoadReadoutType(int? readoutDeviceTypeID)
    {
      return MeterDatabase.LoadReadoutType(DbBasis.PrimaryDB, readoutDeviceTypeID);
    }

    public static List<ReadoutType> LoadReadoutType(DbBasis db, int? readoutDeviceTypeID)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<ReadoutType> readoutTypeList = new List<ReadoutType>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          if (!readoutDeviceTypeID.HasValue)
          {
            cmd.CommandText = "SELECT * FROM ReadoutType;";
          }
          else
          {
            cmd.CommandText = "SELECT * FROM ReadoutType WHERE ReadoutDeviceTypeID=@ReadoutDeviceTypeID;";
            MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", readoutDeviceTypeID.Value);
          }
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return readoutTypeList;
          while (dataReader.Read())
          {
            string empty = string.Empty;
            if (dataReader["ImageIdList"] != DBNull.Value)
              empty = Convert.ToString(dataReader["ImageIdList"]);
            readoutTypeList.Add(new ReadoutType()
            {
              ReadoutDeviceTypeID = Convert.ToInt32(dataReader["ReadoutDeviceTypeID"]),
              ReadoutSettingsID = Convert.ToInt32(dataReader["ReadoutSettingsID"]),
              ImageIdList = empty
            });
          }
          return readoutTypeList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return readoutTypeList;
      }
    }

    public static bool UpdateReadoutType(
      int readoutDeviceTypeID,
      int readoutSettingsID,
      string newImageIdList)
    {
      if (newImageIdList == null)
        throw new ArgumentNullException("Input parameter 'newImageIdList' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE ReadoutType SET ImageIdList=@ImageIdList WHERE ReadoutDeviceTypeID=@ReadoutDeviceTypeID AND ReadoutSettingsID=@ReadoutSettingsID;";
          MeterDatabase.AddParameter(cmd, "@ImageIdList", newImageIdList);
          MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", readoutDeviceTypeID);
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool UpdateReadoutType(ReadoutType oldReadoutType, ReadoutType newReadoutType)
    {
      if (oldReadoutType == null)
        throw new ArgumentNullException("Input parameter 'oldReadoutType' can not be null!");
      if (newReadoutType == null)
        throw new ArgumentNullException("Input parameter 'newReadoutType' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE ReadoutType SET ReadoutDeviceTypeID=@ReadoutDeviceTypeID, ReadoutSettingsID=@ReadoutSettingsID, ImageIdList=@ImageIdList WHERE ReadoutDeviceTypeID=@OldReadoutDeviceTypeID AND ReadoutSettingsID=@OldReadoutSettingsID AND ImageIdList=@OldImageIdList;";
          MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", newReadoutType.ReadoutDeviceTypeID);
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", newReadoutType.ReadoutSettingsID);
          MeterDatabase.AddParameter(cmd, "@ImageIdList", newReadoutType.ImageIdList);
          MeterDatabase.AddParameter(cmd, "@OldReadoutDeviceTypeID", oldReadoutType.ReadoutDeviceTypeID);
          MeterDatabase.AddParameter(cmd, "@OldReadoutSettingsID", oldReadoutType.ReadoutSettingsID);
          MeterDatabase.AddParameter(cmd, "@OldImageIdList", oldReadoutType.ImageIdList);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool UpdateReadoutType(ReadoutType readoutType, string newReadoutDeviceType)
    {
      return readoutType != null;
    }

    public static ReadoutGmmSettings AddReadoutSettings(string readoutSettings)
    {
      return MeterDatabase.AddReadoutSettings(DbBasis.PrimaryDB, readoutSettings);
    }

    public static ReadoutGmmSettings AddReadoutSettings(DbBasis db, string readoutSettings)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (string.IsNullOrEmpty(readoutSettings))
        throw new ArgumentNullException("Input parameter 'readoutSettings' can not be null or empty!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          int num = 1;
          cmd.CommandText = "SELECT MAX(ReadoutSettingsID) FROM ReadoutSettings;";
          object obj = cmd.ExecuteScalar();
          if (obj != null && obj != DBNull.Value)
            num = Convert.ToInt32(obj) + 1;
          cmd.CommandText = "INSERT INTO ReadoutSettings (ReadoutSettingsID, Settings) VALUES (@ReadoutSettingsID, @Settings);";
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", num);
          MeterDatabase.AddParameter(cmd, "@Settings", readoutSettings);
          if (cmd.ExecuteNonQuery() != 1)
            return (ReadoutGmmSettings) null;
          return new ReadoutGmmSettings()
          {
            ReadoutSettingsID = num,
            Settings = readoutSettings
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ReadoutGmmSettings) null;
      }
    }

    public static bool DeleteReadoutSettings(int readoutSettingsID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM ReadoutSettings WHERE ReadoutSettingsID = @ReadoutSettingsID;";
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static List<ReadoutGmmSettings> LoadReadoutSettings(int readoutDeviceTypeID)
    {
      return MeterDatabase.LoadReadoutSettings(DbBasis.PrimaryDB, readoutDeviceTypeID);
    }

    public static List<ReadoutGmmSettings> LoadReadoutSettings(DbBasis db, int readoutDeviceTypeID)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<ReadoutGmmSettings> readoutGmmSettingsList = new List<ReadoutGmmSettings>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT ReadoutSettingsID FROM ReadoutType WHERE ReadoutDeviceTypeID=@ReadoutDeviceTypeID;";
          MeterDatabase.AddParameter(cmd, "@ReadoutDeviceTypeID", readoutDeviceTypeID);
          StringBuilder stringBuilder = new StringBuilder();
          using (IDataReader dataReader = cmd.ExecuteReader())
          {
            if (dataReader == null)
              return readoutGmmSettingsList;
            cmd.Parameters.Clear();
            int num = 0;
            while (dataReader.Read())
            {
              if (Convert.ToInt32(dataReader["ReadoutSettingsID"]) != 0)
              {
                if (stringBuilder.Length > 0)
                  stringBuilder.Append(", ");
                stringBuilder.Append("@").Append(num);
                MeterDatabase.AddParameter(cmd, "@" + num.ToString(), Convert.ToInt32(dataReader["ReadoutSettingsID"]));
                ++num;
              }
            }
          }
          if (stringBuilder.Length == 0)
            return readoutGmmSettingsList;
          cmd.CommandText = "SELECT * FROM ReadoutSettings WHERE ReadoutSettingsID IN (" + stringBuilder.ToString() + ");";
          using (IDataReader dataReader = cmd.ExecuteReader())
          {
            if (dataReader == null)
              return readoutGmmSettingsList;
            while (dataReader.Read())
              readoutGmmSettingsList.Add(new ReadoutGmmSettings()
              {
                ReadoutSettingsID = Convert.ToInt32(dataReader["ReadoutSettingsID"]),
                Settings = Convert.ToString(dataReader["Settings"])
              });
          }
          return readoutGmmSettingsList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return readoutGmmSettingsList;
      }
    }

    public static List<ReadoutGmmSettings> LoadReadoutSettings()
    {
      return MeterDatabase.LoadReadoutSettings(DbBasis.PrimaryDB);
    }

    public static List<ReadoutGmmSettings> LoadReadoutSettings(DbBasis db)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<ReadoutGmmSettings> readoutGmmSettingsList = new List<ReadoutGmmSettings>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM ReadoutSettings;";
          using (IDataReader dataReader = cmd.ExecuteReader())
          {
            if (dataReader == null)
              return readoutGmmSettingsList;
            while (dataReader.Read())
              readoutGmmSettingsList.Add(new ReadoutGmmSettings()
              {
                ReadoutSettingsID = Convert.ToInt32(dataReader["ReadoutSettingsID"]),
                Settings = Convert.ToString(dataReader["Settings"])
              });
          }
          return readoutGmmSettingsList;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return readoutGmmSettingsList;
      }
    }

    public static ReadoutGmmSettings GetReadoutSettings(int readoutSettingsID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM ReadoutSettings WHERE ReadoutSettingsID=@ReadoutSettingsID;";
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (ReadoutGmmSettings) null;
          return new ReadoutGmmSettings()
          {
            ReadoutSettingsID = Convert.ToInt32(dataReader["ReadoutSettingsID"]),
            Settings = Convert.ToString(dataReader["Settings"])
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ReadoutGmmSettings) null;
      }
    }

    public static bool UpdateReadoutSettings(int readoutSettingsID, string readoutSettings)
    {
      if (readoutSettings == null)
        throw new ArgumentNullException("Input parameter 'readoutSettings' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE ReadoutSettings SET Settings=@Settings WHERE ReadoutSettingsID=@ReadoutSettingsID;";
          MeterDatabase.AddParameter(cmd, "@Settings", readoutSettings);
          MeterDatabase.AddParameter(cmd, "@ReadoutSettingsID", readoutSettingsID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static FunctionPrecompiled AddFunctionPrecompiled(
      int functionNumber,
      short recordOrder,
      FunctionRecordType recordType,
      string name,
      int offset,
      byte[] codes)
    {
      return MeterDatabase.AddFunctionPrecompiled(DbBasis.PrimaryDB, functionNumber, recordOrder, recordType, name, offset, codes);
    }

    public static FunctionPrecompiled AddFunctionPrecompiled(
      DbBasis db,
      int functionNumber,
      short recordOrder,
      FunctionRecordType recordType,
      string name,
      int offset,
      byte[] codes)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO ZRFunctionCompiled (FunctionNumber, RecordOrder, RecordType, [Name], [Offset], Codes) VALUES (@FunctionNumber, @RecordOrder, @RecordType, @Name, @Offset, @Codes);";
          MeterDatabase.AddParameter(cmd, "@FunctionNumber", functionNumber);
          MeterDatabase.AddParameter(cmd, "@RecordOrder", (int) recordOrder);
          MeterDatabase.AddParameter(cmd, "@RecordType", (int) Convert.ToInt16((object) recordType));
          MeterDatabase.AddParameter(cmd, "@Name", name);
          MeterDatabase.AddParameter(cmd, "@Offset", offset);
          MeterDatabase.AddParameter(cmd, "@Codes", codes);
          return cmd.ExecuteNonQuery() != 1 ? (FunctionPrecompiled) null : new FunctionPrecompiled(functionNumber, recordOrder, recordType, name, offset, codes);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (FunctionPrecompiled) null;
      }
    }

    public static List<FunctionPrecompiled> LoadFunctionPrecompiled(int functionNumber)
    {
      return MeterDatabase.LoadFunctionPrecompiled(DbBasis.PrimaryDB, functionNumber);
    }

    public static List<FunctionPrecompiled> LoadFunctionPrecompiled(DbBasis db, int functionNumber)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      List<FunctionPrecompiled> functionPrecompiledList = new List<FunctionPrecompiled>();
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          return MeterDatabase.LoadFunctionPrecompiled(cmd, functionNumber);
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return functionPrecompiledList;
      }
    }

    public static List<FunctionPrecompiled> LoadFunctionPrecompiled(
      IDbCommand cmd,
      int functionNumber)
    {
      return MeterDatabase.LoadFunctionPrecompiled(cmd, new List<int>((IEnumerable<int>) new int[1]
      {
        functionNumber
      }));
    }

    public static List<FunctionPrecompiled> LoadFunctionPrecompiled(
      IDbCommand cmd,
      List<int> functionNumbers)
    {
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      List<FunctionPrecompiled> functionPrecompiledList = new List<FunctionPrecompiled>();
      cmd.Parameters.Clear();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < functionNumbers.Count; ++index)
      {
        if (index == 0)
          stringBuilder.Append("FunctionNumber=").Append(functionNumbers[index].ToString());
        else
          stringBuilder.Append(" OR FunctionNumber=").Append(functionNumbers[index].ToString());
      }
      cmd.CommandText = "SELECT * FROM ZRFunctionCompiled WHERE " + stringBuilder.ToString() + " ORDER BY RecordOrder;";
      using (IDataReader dataReader = cmd.ExecuteReader())
      {
        if (dataReader == null)
          return functionPrecompiledList;
        while (dataReader.Read())
        {
          FunctionRecordType RecordType = FunctionRecordType.None;
          if (dataReader["RecordType"] != DBNull.Value)
            RecordType = (FunctionRecordType) Enum.ToObject(typeof (FunctionRecordType), Convert.ToInt16(dataReader["RecordType"]));
          byte[] Codes = (byte[]) null;
          if (dataReader["Codes"] != DBNull.Value)
            Codes = (byte[]) dataReader["Codes"];
          functionPrecompiledList.Add(new FunctionPrecompiled(Convert.ToInt32(dataReader["FunctionNumber"]), Convert.ToInt16(dataReader["RecordOrder"]), RecordType, Convert.ToString(dataReader["Name"]), Convert.ToInt32(dataReader["Offset"]), Codes));
        }
      }
      return functionPrecompiledList;
    }

    public static bool DeleteFunctionPrecompiled(int functionNumber, short recordOrder)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "DELETE FROM ZRFunctionCompiled WHERE FunctionNumber = @FunctionNumber AND RecordOrder = @RecordOrder;";
          MeterDatabase.AddParameter(cmd, "@FunctionNumber", functionNumber);
          MeterDatabase.AddParameter(cmd, "@RecordOrder", (int) recordOrder);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool UpdateFunctionPrecompiled(
      int functionNumber,
      short oldRecordOrder,
      short recordOrder,
      FunctionRecordType recordType,
      string name,
      int offset,
      byte[] codes)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE ZRFunctionCompiled SET RecordOrder=@RecordOrder, RecordType=@RecordType, [Name]=@Name, [Offset]=@Offset, Codes=@Codes WHERE FunctionNumber=@FunctionNumber AND RecordOrder=@OldRecordOrder;";
          MeterDatabase.AddParameter(cmd, "@RecordOrder", (int) recordOrder);
          MeterDatabase.AddParameter(cmd, "@RecordType", (int) Convert.ToInt16((object) recordType));
          MeterDatabase.AddParameter(cmd, "@Name", name);
          MeterDatabase.AddParameter(cmd, "@Offset", offset);
          MeterDatabase.AddParameter(cmd, "@Codes", codes);
          MeterDatabase.AddParameter(cmd, "@FunctionNumber", functionNumber);
          MeterDatabase.AddParameter(cmd, "@OldRecordOrder", (int) oldRecordOrder);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static bool AddMeterData(int meterID, string hardwareTypeId, byte[] buffer)
    {
      return MeterDatabase.AddMeterData(DbBasis.PrimaryDB, meterID, hardwareTypeId, buffer);
    }

    public static bool AddMeterData(DbBasis db, int meterID, string hardwareTypeId, byte[] buffer)
    {
      if (db == null)
        throw new ArgumentNullException("Input parameter 'db' can not be null!");
      if (buffer == null)
        throw new ArgumentNullException("Input parameter 'buffer' can not be null!");
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = db.GetDbConnection())
        {
          dbConnection.Open();
          cmd = db.DbCommand(dbConnection);
          cmd.CommandText = "INSERT INTO MeterData (MeterID, TimePoint, PValueID, PValue, PValueBinary, SyncStatus) VALUES (@MeterID, @TimePoint, @PValueID, @PValue, @PValueBinary, @SyncStatus);";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
          MeterDatabase.AddParameter(cmd, "@TimePoint", DateTime.Now);
          MeterDatabase.AddParameter(cmd, "@PValueID", 1);
          MeterDatabase.AddParameter(cmd, "@PValue", hardwareTypeId);
          MeterDatabase.AddParameter(cmd, "@PValueBinary", buffer);
          MeterDatabase.AddParameter(cmd, "@SyncStatus", 0);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static MeterData GetMeterData(int meterId, DateTime timePoint)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT PValue, PValueBinary, SyncStatus FROM MeterData WHERE MeterID=@MeterID AND TimePoint=@TimePoint;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterId);
          MeterDatabase.AddParameter(cmd, "@TimePoint", timePoint);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (MeterData) null;
          return new MeterData()
          {
            MeterID = meterId,
            TimePoint = new DateTime?(timePoint),
            PValueID = 1,
            PValue = dataReader["PValue"] == DBNull.Value || dataReader["PValue"] == null ? string.Empty : dataReader["PValue"].ToString(),
            PValueBinary = dataReader["PValueBinary"] == DBNull.Value || dataReader["PValueBinary"] == null ? (byte[]) null : (byte[]) dataReader["PValueBinary"],
            SyncStatus = dataReader["SyncStatus"] == DBNull.Value || dataReader["SyncStatus"] == null ? (byte) 0 : Convert.ToByte(dataReader["SyncStatus"])
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterData) null;
      }
    }

    public static MeterData GetMeterDataOfLastBackup(int meterId)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT TOP 1 PValue, PValueBinary, SyncStatus, TimePoint FROM MeterData WHERE MeterID=@MeterID ORDER BY TimePoint DESC;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterId);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (MeterData) null;
          return new MeterData()
          {
            MeterID = meterId,
            TimePoint = new DateTime?(Convert.ToDateTime(dataReader["TimePoint"])),
            PValueID = 1,
            PValue = dataReader["PValue"] == DBNull.Value || dataReader["PValue"] == null ? string.Empty : dataReader["PValue"].ToString(),
            PValueBinary = dataReader["PValueBinary"] == DBNull.Value || dataReader["PValueBinary"] == null ? (byte[]) null : (byte[]) dataReader["PValueBinary"],
            SyncStatus = dataReader["SyncStatus"] == DBNull.Value || dataReader["SyncStatus"] == null ? (byte) 0 : Convert.ToByte(dataReader["SyncStatus"])
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterData) null;
      }
    }

    public static MeterTypeData GetMeterTypeData(string tableName, int meterTypeID)
    {
      if (string.IsNullOrEmpty(tableName))
        return (MeterTypeData) null;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT EEPdata, TypeOverrideString FROM " + tableName + " WHERE MeterTypeID=@MeterTypeID;";
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (MeterTypeData) null;
          return new MeterTypeData()
          {
            MeterTypeID = meterTypeID,
            EEPdata = dataReader["EEPdata"] == DBNull.Value || dataReader["EEPdata"] == null ? (byte[]) null : (byte[]) dataReader["EEPdata"],
            TypeOverrideString = dataReader["TypeOverrideString"].ToString()
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterTypeData) null;
      }
    }

    public static bool UpdateMeterTypeData(
      string tableName,
      int meterTypeID,
      byte[] EEPdata,
      string typeOverrideString)
    {
      if (string.IsNullOrEmpty(tableName))
        return false;
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "UPDATE " + tableName + " SET EEPdata=@EEPdata,TypeOverrideString=@TypeOverrideString WHERE MeterTypeID=@MeterTypeID;";
          MeterDatabase.AddParameter(cmd, "@EEPdata", EEPdata);
          MeterDatabase.AddParameter(cmd, "@TypeOverrideString", typeOverrideString);
          MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
          return cmd.ExecuteNonQuery() == 1;
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return false;
      }
    }

    public static MeterTypeData AddMeterTypeData(
      IDbCommand cmd,
      string tableName,
      int meterTypeID,
      byte[] EEPdata,
      string typeOverrideString)
    {
      if (cmd == null)
        throw new ArgumentNullException("Input parameter 'cmd' can not be null!");
      if (string.IsNullOrEmpty(tableName))
        throw new ArgumentNullException("The 'tableName' can not be null!");
      cmd.CommandText = "INSERT INTO " + tableName + " (MeterTypeID, EEPdata, TypeOverrideString) VALUES (@MeterTypeID, @EEPdata, @TypeOverrideString);";
      cmd.Parameters.Clear();
      MeterDatabase.AddParameter(cmd, "@MeterTypeID", meterTypeID);
      MeterDatabase.AddParameter(cmd, "@EEPdata", EEPdata);
      MeterDatabase.AddParameter(cmd, "@TypeOverrideString", typeOverrideString);
      if (cmd.ExecuteNonQuery() != 1)
        return (MeterTypeData) null;
      return new MeterTypeData()
      {
        MeterTypeID = meterTypeID,
        EEPdata = EEPdata,
        TypeOverrideString = typeOverrideString
      };
    }

    public static ProgFiles GetFirmware(int mapID)
    {
      IDbCommand cmd = (IDbCommand) null;
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM ProgFiles WHERE MapID=@MapID;";
          MeterDatabase.AddParameter(cmd, "@MapID", mapID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (ProgFiles) null;
          return new ProgFiles()
          {
            MapID = mapID,
            ProgFileName = dataReader["ProgFileName"] == DBNull.Value || dataReader["ProgFileName"] == null ? string.Empty : dataReader["ProgFileName"].ToString(),
            Options = dataReader["Options"] == DBNull.Value || dataReader["Options"] == null ? string.Empty : dataReader["Options"].ToString(),
            HexText = dataReader["HexText"] == DBNull.Value || dataReader["HexText"] == null ? string.Empty : dataReader["HexText"].ToString(),
            SourceInfo = dataReader["SourceInfo"] == DBNull.Value || dataReader["SourceInfo"] == null ? string.Empty : dataReader["SourceInfo"].ToString()
          };
        }
      }
      catch (Exception ex)
      {
        MeterDatabase.LogFailedSQLQuery(cmd);
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (ProgFiles) null;
      }
    }

    public static HardwareType GetHardwareType(int hardwareTypeID)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT MapID, FirmwareVersion, HardwareName, HardwareVersion, HardwareResource, Description FROM HardwareType WHERE HardwareTypeID=@HardwareTypeID;";
          MeterDatabase.AddParameter(cmd, "@HardwareTypeID", hardwareTypeID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (HardwareType) null;
          HardwareType hardwareType = new HardwareType();
          hardwareType.HardwareTypeID = hardwareTypeID;
          hardwareType.MapID = Convert.ToInt32(dataReader["MapID"]);
          hardwareType.FirmwareVersion = Convert.ToUInt32(dataReader["FirmwareVersion"]);
          hardwareType.HardwareName = dataReader["HardwareName"].ToString();
          hardwareType.HardwareVersion = Convert.ToInt32(dataReader["HardwareVersion"]);
          hardwareType.HardwareResource = dataReader["HardwareResource"].ToString();
          hardwareType.Description = dataReader["Description"].ToString();
          if (dataReader.Read())
            throw new Exception("INTERNAL ERROR: The function become more as one result from database. SQL: " + cmd.CommandText);
          return hardwareType;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        MeterDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (HardwareType) null;
      }
    }

    public sealed class Progress : EventArgs
    {
      public int Successful;
      public int Failed;
      public long Count;
      public int ProgressValue;
    }

    private sealed class DayValue
    {
      public DateTime Timepoint;
      public double Value;
      public bool IsEstimated;

      public DayValue(DateTime timepoint, double value, bool isEstimated)
      {
        this.Timepoint = timepoint;
        this.Value = value;
        this.IsEstimated = isEstimated;
      }

      public override string ToString()
      {
        return string.Format("{0} {1} {2}", (object) this.Timepoint.ToShortDateString(), (object) this.Value, (object) this.IsEstimated);
      }
    }
  }
}
