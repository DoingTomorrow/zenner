// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ShowRoutingTableViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MinomatHandler;
using MSS.Business.DTO;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class ShowRoutingTableViewModel : ViewModelBase
  {
    private ObservableCollection<MinomatMasterInfo> _minomatMastersInfoCollection;
    private Dictionary<Guid, RoutingTable> _minomatMasterRoutingTables;
    private IWindowFactory _windowFactory;
    private Dictionary<Guid, string> _minomatMastersWithRoutingTables_SerialNumbers;
    private KeyValuePair<Guid, string> _selectedMinomatMaster;
    private ObservableCollection<StructureNodeDTO> _minomatTreeCollection;
    private StructureNodeDTO _minomatTree_SelectedMinomat;
    private bool _areMinomatDetailsVisible;
    private ObservableCollection<StructureNodeDTO> _unregisteredSlaves;

    [Inject]
    public ShowRoutingTableViewModel(
      ObservableCollection<MinomatMasterInfo> minomatMastersInfoCollection,
      Guid selectedMinomatMasterGuid,
      Dictionary<Guid, RoutingTable> minomatMasterRoutingTables,
      IWindowFactory windowFactory)
    {
      this._windowFactory = windowFactory;
      this._minomatMastersInfoCollection = minomatMastersInfoCollection;
      this._minomatMasterRoutingTables = minomatMasterRoutingTables;
      if (selectedMinomatMasterGuid != Guid.Empty)
        this.SelectedMinomatMaster = this.MinomatMastersWithRoutingTables_SerialNumbers.FirstOrDefault<KeyValuePair<Guid, string>>((Func<KeyValuePair<Guid, string>, bool>) (item => item.Key == selectedMinomatMasterGuid));
      this.AreMinomatDetailsVisible = false;
    }

    private void CreateRegisteredMinomatTree()
    {
      MinomatMasterInfo minomatMaster = this._minomatMastersInfoCollection.FirstOrDefault<MinomatMasterInfo>((Func<MinomatMasterInfo, bool>) (item => item.MinomatMaster.Id == this._selectedMinomatMaster.Key));
      RoutingTable routingTable = (RoutingTable) null;
      if (minomatMaster != null)
        this._minomatMasterRoutingTables.TryGetValue(minomatMaster.MinomatMaster.Id, out routingTable);
      if (minomatMaster == null || routingTable == null)
        return;
      Dictionary<string, MinomatSlaveInfo> withMinomatSlaves = this.GetNodeIdsWithMinomatSlaves(minomatMaster);
      StructureNodeDTO minomatTree = new StructureNodeDTO();
      minomatTree.Entity = (object) new MinomatTreeInfoDTO()
      {
        SerialNumber = minomatMaster.MinomatMaster.GsmId,
        NodeID = 1,
        Letter = "M",
        ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-green.png"
      };
      int count = routingTable.Count;
      bool[] source = new bool[count];
      for (int index = 0; index < count; ++index)
        source[index] = false;
      bool[] flagArray = (bool[]) null;
      List<string> list = this.GetAllSlaves().Select<MinomatSlaveInfo, string>((Func<MinomatSlaveInfo, string>) (item => item.NodeId)).ToList<string>();
      if (((IEnumerable<bool>) source).Any<bool>())
      {
        while (((IEnumerable<bool>) source).Any<bool>((Func<bool, bool>) (item => !item)))
        {
          foreach (RoutingRow routingRow in (List<RoutingRow>) routingTable)
          {
            RoutingRow routingTableEntry = routingRow;
            int index = routingTable.FindIndex((Predicate<RoutingRow>) (item => (int) item.NodeId == (int) routingTableEntry.NodeId));
            if (!source[index])
            {
              StructureNodeDTO parent = this.GetParent(routingTableEntry, minomatTree);
              if (parent != null)
              {
                if (parent.SubNodes == null)
                  parent.SubNodes = new ObservableCollection<StructureNodeDTO>();
                MinomatSlaveInfo minomatSlaveInfo;
                withMinomatSlaves.TryGetValue(((int) routingTableEntry.NodeId).ToString(), out minomatSlaveInfo);
                MinomatTreeInfoDTO minomatTreeInfoDto1 = new MinomatTreeInfoDTO();
                minomatTreeInfoDto1.SerialNumber = !string.IsNullOrEmpty(minomatSlaveInfo?.MinomatSlave.GsmId) ? minomatSlaveInfo?.MinomatSlave.GsmId : Resources.MSS_Client_UnknownSlave;
                minomatTreeInfoDto1.Letter = "S";
                minomatTreeInfoDto1.NodeID = (int) routingTableEntry.NodeId;
                MinomatTreeInfoDTO minomatTreeInfoDto2 = minomatTreeInfoDto1;
                List<string> stringList = list;
                int num = minomatTreeInfoDto1.NodeID;
                string str1 = num.ToString();
                string str2 = stringList.Contains(str1) ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "";
                minomatTreeInfoDto2.ImageLocation = str2;
                minomatTreeInfoDto1.ParentID = (int) routingTableEntry.ParentNodeId;
                MinomatTreeInfoDTO minomatTreeInfoDto3 = minomatTreeInfoDto1;
                num = routingTableEntry.RSSI_dBm;
                string str3 = num.ToString() + " dBm";
                minomatTreeInfoDto3.RSSI = str3;
                MinomatTreeInfoDTO minomatTreeInfoDto4 = minomatTreeInfoDto1;
                num = (int) routingTableEntry.HopCount;
                string str4 = num.ToString();
                minomatTreeInfoDto4.HopCount = str4;
                minomatTreeInfoDto1.Floor = minomatSlaveInfo?.Floor;
                minomatTreeInfoDto1.Address = minomatSlaveInfo?.Address;
                parent.SubNodes.Add(new StructureNodeDTO()
                {
                  Entity = (object) minomatTreeInfoDto1
                });
              }
              source[index] = true;
            }
          }
          flagArray = source;
        }
      }
      this.MinomatTreeCollection = new ObservableCollection<StructureNodeDTO>();
      this.MinomatTreeCollection.Add(minomatTree);
    }

    private bool AreArraysEqual(bool[] firstArray, bool[] secondArray)
    {
      if (firstArray == null && secondArray == null)
        return true;
      if (firstArray != null && secondArray != null && firstArray.Length != secondArray.Length || firstArray == null && secondArray != null || firstArray != null && secondArray == null || firstArray == null || secondArray == null || firstArray.Length != secondArray.Length)
        return false;
      for (int index = 0; index < firstArray.Length; ++index)
      {
        if (firstArray[index] != secondArray[index])
          return false;
      }
      return true;
    }

    private StructureNodeDTO GetParent(RoutingRow routingTableEntry, StructureNodeDTO minomatTree)
    {
      StructureNodeDTO parent = (StructureNodeDTO) null;
      int? nullable = minomatTree.Entity is MinomatTreeInfoDTO entity ? new int?(entity.NodeID) : new int?();
      int parentNodeId = (int) routingTableEntry.ParentNodeId;
      if (nullable.GetValueOrDefault() == parentNodeId && nullable.HasValue)
        parent = minomatTree;
      else
        this.WalkMinomatTreeInfoAndGetParent(routingTableEntry.ParentNodeId, minomatTree, ref parent);
      return parent;
    }

    private void WalkMinomatTreeInfoAndGetParent(
      ushort parentNodeId,
      StructureNodeDTO minomatTree,
      ref StructureNodeDTO parent)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) minomatTree.SubNodes)
      {
        if (subNode.Entity is MinomatTreeInfoDTO entity && entity.NodeID == (int) parentNodeId)
          parent = subNode;
        else
          this.WalkMinomatTreeInfoAndGetParent(parentNodeId, subNode, ref parent);
      }
    }

    private Dictionary<string, MinomatSlaveInfo> GetNodeIdsWithMinomatSlaves(
      MinomatMasterInfo minomatMaster)
    {
      Dictionary<string, MinomatSlaveInfo> nodeIdsWithSlaves = new Dictionary<string, MinomatSlaveInfo>();
      foreach (MinomatSlaveInfo minomatSlaves in minomatMaster.MinomatSlavesList)
      {
        nodeIdsWithSlaves.Add(minomatSlaves.NodeId, minomatSlaves);
        this.WalkMinomatSlaves(minomatSlaves, ref nodeIdsWithSlaves);
      }
      return nodeIdsWithSlaves;
    }

    private void WalkMinomatSlaves(
      MinomatSlaveInfo slave,
      ref Dictionary<string, MinomatSlaveInfo> nodeIdsWithSlaves)
    {
      if (slave.MinomatSlavesList == null)
        return;
      foreach (MinomatSlaveInfo minomatSlaves in slave.MinomatSlavesList)
      {
        nodeIdsWithSlaves.Add(minomatSlaves.NodeId, minomatSlaves);
        this.WalkMinomatSlaves(minomatSlaves, ref nodeIdsWithSlaves);
      }
    }

    private void GetUnregisteredSlaves()
    {
      List<StructureNodeDTO> registeredSlaves = this.GetRegisteredSlaves();
      List<MinomatSlaveInfo> allSlaves = this.GetAllSlaves();
      this._unregisteredSlaves = new ObservableCollection<StructureNodeDTO>();
      foreach (MinomatSlaveInfo minomatSlaveInfo in allSlaves)
      {
        string nodeIdToSearchFor = minomatSlaveInfo.NodeId;
        if (!registeredSlaves.Exists((Predicate<StructureNodeDTO>) (item => (item?.Entity is MinomatTreeInfoDTO entity ? entity.NodeID.ToString() : (string) null) == nodeIdToSearchFor)))
        {
          MinomatTreeInfoDTO minomatTreeInfoDto = new MinomatTreeInfoDTO()
          {
            SerialNumber = minomatSlaveInfo.MinomatSlave.GsmId,
            ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-red.png",
            Letter = "S"
          };
          this._unregisteredSlaves.Add(new StructureNodeDTO()
          {
            Entity = (object) minomatTreeInfoDto
          });
        }
      }
      this.OnPropertyChanged("UnregisteredSlaves");
    }

    private List<StructureNodeDTO> GetRegisteredSlaves()
    {
      List<StructureNodeDTO> registeredSlaves = new List<StructureNodeDTO>();
      this.WalkCollectionAndGetNodes(this.MinomatTreeCollection.First<StructureNodeDTO>(), ref registeredSlaves);
      return registeredSlaves;
    }

    private List<MinomatSlaveInfo> GetAllSlaves()
    {
      MinomatMasterInfo minomatMasterInfo = this._minomatMastersInfoCollection.FirstOrDefault<MinomatMasterInfo>((Func<MinomatMasterInfo, bool>) (item => item.MinomatMaster.Id == this.SelectedMinomatMaster.Key));
      if (minomatMasterInfo == null)
        return (List<MinomatSlaveInfo>) null;
      List<MinomatSlaveInfo> foundSlaves = new List<MinomatSlaveInfo>();
      if (minomatMasterInfo.MinomatSlavesList != null && minomatMasterInfo.MinomatSlavesList.Any<MinomatSlaveInfo>())
        this.GetSlavesFromList(minomatMasterInfo.MinomatSlavesList, ref foundSlaves);
      return minomatMasterInfo.MinomatSlavesList;
    }

    private void GetSlavesFromList(
      List<MinomatSlaveInfo> slavesToSearch,
      ref List<MinomatSlaveInfo> foundSlaves)
    {
      foreach (MinomatSlaveInfo minomatSlaveInfo in slavesToSearch)
      {
        foundSlaves.Add(minomatSlaveInfo);
        if (minomatSlaveInfo.MinomatSlavesList != null && minomatSlaveInfo.MinomatSlavesList.Any<MinomatSlaveInfo>())
          this.GetSlavesFromList(minomatSlaveInfo.MinomatSlavesList, ref foundSlaves);
      }
    }

    private void WalkCollectionAndGetNodes(
      StructureNodeDTO rootNode,
      ref List<StructureNodeDTO> registeredSlaves)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) rootNode.SubNodes)
      {
        registeredSlaves.Add(subNode);
        this.WalkCollectionAndGetNodes(subNode, ref registeredSlaves);
      }
    }

    public Dictionary<Guid, string> MinomatMastersWithRoutingTables_SerialNumbers
    {
      get
      {
        this._minomatMastersWithRoutingTables_SerialNumbers = new Dictionary<Guid, string>();
        foreach (MinomatMasterInfo minomatMastersInfo in (Collection<MinomatMasterInfo>) this._minomatMastersInfoCollection)
        {
          if (this._minomatMasterRoutingTables.ContainsKey(minomatMastersInfo.MinomatMaster.Id))
            this._minomatMastersWithRoutingTables_SerialNumbers.Add(minomatMastersInfo.MinomatMaster.Id, minomatMastersInfo.MinomatMaster.RadioId);
        }
        return this._minomatMastersWithRoutingTables_SerialNumbers;
      }
    }

    public KeyValuePair<Guid, string> SelectedMinomatMaster
    {
      get => this._selectedMinomatMaster;
      set
      {
        this._selectedMinomatMaster = value;
        this.OnPropertyChanged(nameof (SelectedMinomatMaster));
        this.CreateRegisteredMinomatTree();
        this.GetUnregisteredSlaves();
      }
    }

    public ObservableCollection<StructureNodeDTO> MinomatTreeCollection
    {
      get => this._minomatTreeCollection;
      set
      {
        this._minomatTreeCollection = value;
        this.OnPropertyChanged(nameof (MinomatTreeCollection));
      }
    }

    public StructureNodeDTO MinomatTree_SelectedMinomat
    {
      get => this._minomatTree_SelectedMinomat;
      set
      {
        this._minomatTree_SelectedMinomat = value;
        this.AreMinomatDetailsVisible = value != null;
        this.OnPropertyChanged(nameof (MinomatTree_SelectedMinomat));
      }
    }

    public bool AreMinomatDetailsVisible
    {
      get => this._areMinomatDetailsVisible;
      set
      {
        this._areMinomatDetailsVisible = value;
        this.OnPropertyChanged(nameof (AreMinomatDetailsVisible));
      }
    }

    public ObservableCollection<StructureNodeDTO> UnregisteredSlaves
    {
      get => this._unregisteredSlaves;
      set
      {
        this._unregisteredSlaves = value;
        this.OnPropertyChanged(nameof (UnregisteredSlaves));
      }
    }
  }
}
