// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ConfigDatabaseAccess
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using CommonWPF;
using GmmDbLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ReadoutConfiguration
{
  internal class ConfigDatabaseAccess
  {
    private static Logger ReadoutDatabaseAccess = LogManager.GetLogger(nameof (ReadoutDatabaseAccess));
    internal IDbConnection MyDbConnection;
    internal bool DailyAutosave = true;
    private StringBuilder SQL = new StringBuilder(2000);
    private Schema.DatabaseIdentificationDataTable DatabaseIdentTable;
    private Schema.ConnectionItemsDataTable connectionItemsDataTable;
    private ZRDataAdapter ConnectionItemsAdapter;
    private Schema.ConnectionItemParametersDataTable connectionItemParametersDataTable;
    private ZRDataAdapter ConnectionItemsParametersAdapter;
    private Schema.ConnectionProfilesDataTable connectionProfilesDataTable;
    private ZRDataAdapter ConnectionProfilesAdapter;
    private Schema.ConnectionProfileParametersDataTable connectionProfileParametersDataTable;
    private ZRDataAdapter ConnectionProfilesParametersAdapter;
    private Schema.ConnectionSettingsDataTable connectionSettingsDataTable;
    private ZRDataAdapter ConnectionSettingsAdapter;
    private Schema.ChangeableParametersDataTable changeableParametersDataTable;
    private ZRDataAdapter ChangeableParametersAdapter;
    private Schema.ConnectionProfileFiltersDataTable connectionFiltersDataTable;
    private ZRDataAdapter ConnectionFiltersAdapter;
    private Schema.GmmImagesDataTable _gmmImageDataTable;
    private ZRDataAdapter GmmImageAdapter;
    internal List<DeviceGroup> CachedDeviceGroups;
    internal List<DeviceModel> CachedDeviceModels;
    internal List<EquipmentGroup> CachedEquipmentGroups;
    internal List<EquipmentModel> CachedEquipmentModels;
    internal List<ProfileTypeGroup> CachedProfileTypeGroups;
    internal List<ProfileType> CachedProfileTypes;
    internal SortedList<int, int> SettingsID_FromProfileID;
    internal SortedList<int, ConnectionSettings> CachedConnectionSettingsById;
    internal SortedList<int, ChangeableParameter> ChangableParameterByID;
    internal SortedList<string, ChangeableParameter> ChangableParameterByName;
    internal SortedList<int, BitmapImage> CachedBitmapImages;
    internal List<ConnectionProfile> CachedPartiallyConnectionProfiles;
    private SortedList<string, ConnectionProfileFilter> profileFilters;
    private SortedList<int, Schema.ConnectionItemsRow> ProfileTypesById = (SortedList<int, Schema.ConnectionItemsRow>) null;
    private ObservableCollection<ConnectionListRow> currentConnectionProfilesList;
    private ConnectionListRow selectedProfileRow;
    private const string profileIdTag = "ProfileID:";

    private Schema.GmmImagesDataTable gmmImageDataTable
    {
      get
      {
        if (this._gmmImageDataTable != null)
          return this._gmmImageDataTable;
        this.LoadImages();
        return this._gmmImageDataTable;
      }
    }

    internal SortedList<string, ConnectionProfileFilter> CachedProfileFilters
    {
      get
      {
        if (this.profileFilters == null)
          this.LoadProfileFilters();
        return this.profileFilters;
      }
    }

    public ConfigDatabaseAccess()
    {
      this.MyDbConnection = DbBasis.PrimaryDB.GetDbConnection();
      string databaseOption = this.GetDatabaseOption("DatabaseSaveOption");
      if (databaseOption != null && databaseOption == "DailyAutosaveOff")
        this.DailyAutosave = false;
      this.LoadAllConnectionTables();
    }

    internal string GetDatabaseOption(string option)
    {
      if (this.DatabaseIdentTable == null)
      {
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM DatabaseIdentification", this.MyDbConnection);
        this.DatabaseIdentTable = new Schema.DatabaseIdentificationDataTable();
        zrDataAdapter.Fill((DataTable) this.DatabaseIdentTable);
      }
      Schema.DatabaseIdentificationRow[] identificationRowArray = (Schema.DatabaseIdentificationRow[]) this.DatabaseIdentTable.Select("InfoName = '" + option + "'");
      return identificationRowArray.Length == 1 ? identificationRowArray[0].InfoData : (string) null;
    }

    internal void LoadAllConnectionTables()
    {
      this.CachedDeviceGroups = (List<DeviceGroup>) null;
      this.CachedDeviceModels = (List<DeviceModel>) null;
      this.CachedEquipmentGroups = (List<EquipmentGroup>) null;
      this.CachedEquipmentModels = (List<EquipmentModel>) null;
      this.CachedProfileTypes = (List<ProfileType>) null;
      this.CachedProfileTypeGroups = (List<ProfileTypeGroup>) null;
      this.CachedConnectionSettingsById = (SortedList<int, ConnectionSettings>) null;
      this.ChangableParameterByID = (SortedList<int, ChangeableParameter>) null;
      this.CachedPartiallyConnectionProfiles = (List<ConnectionProfile>) null;
      this.CachedBitmapImages = (SortedList<int, BitmapImage>) null;
      this.LoadChangeableParameters();
      this.LoadConnectionSettings();
      this.LoadConnectionProfiles();
      this.LoadConnectionItems();
      this.GetReadoutSettings();
      this.GetDeviceGroups();
      this.GetDeviceModels();
      this.GetEquipmentGroups();
      this.GetEquipmentModels();
      this.GetProfileTypeGroups();
      this.GetProfileTypes();
      this.GetPartiallyConnectionProfiles();
    }

    internal void LoadConnectionItems()
    {
      this.ConnectionItemsAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ConnectionItems", this.MyDbConnection);
      this.connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
      this.ConnectionItemsAdapter.Fill((DataTable) this.connectionItemsDataTable);
      try
      {
        this.ConnectionItemsParametersAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ConnectionItemParameters", this.MyDbConnection);
        this.connectionItemParametersDataTable = new Schema.ConnectionItemParametersDataTable();
        this.ConnectionItemsParametersAdapter.Fill((DataTable) this.connectionItemParametersDataTable);
      }
      catch
      {
        this.connectionItemParametersDataTable = (Schema.ConnectionItemParametersDataTable) null;
      }
    }

    internal void LoadProfileFilters()
    {
      try
      {
        this.profileFilters = new SortedList<string, ConnectionProfileFilter>();
        this.ConnectionFiltersAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ConnectionProfileFilters ORDER BY ConnectionFilterID,ParameterOrder", this.MyDbConnection);
        this.connectionFiltersDataTable = new Schema.ConnectionProfileFiltersDataTable();
        this.ConnectionFiltersAdapter.Fill((DataTable) this.connectionFiltersDataTable);
        ConnectionProfileFilter connectionProfileFilter = (ConnectionProfileFilter) null;
        ConnectionParameterGroup connectionParameterGroup1 = (ConnectionParameterGroup) null;
        foreach (Schema.ConnectionProfileFiltersRow profileFiltersRow in (TypedTableBase<Schema.ConnectionProfileFiltersRow>) this.connectionFiltersDataTable)
        {
          if (connectionProfileFilter == null || profileFiltersRow.ConnectionFilterID != connectionProfileFilter.FilterID)
          {
            connectionProfileFilter = new ConnectionProfileFilter(profileFiltersRow.ConnectionFilterID, profileFiltersRow.ParameterValue);
            this.profileFilters.Add(connectionProfileFilter.Name, connectionProfileFilter);
            connectionParameterGroup1 = (ConnectionParameterGroup) null;
          }
          else
          {
            if (connectionParameterGroup1 == null)
            {
              connectionParameterGroup1 = new ConnectionParameterGroup(profileFiltersRow.FilterGroupNumber);
              connectionParameterGroup1.GroupFunction = (ConnectionProfileFilterGroupFunctions) profileFiltersRow.GroupFunction;
              connectionProfileFilter.FilterGroups.Add(connectionParameterGroup1);
            }
            else if (connectionParameterGroup1.GroupNumber != profileFiltersRow.FilterGroupNumber)
            {
              if (connectionProfileFilter.SubGroups != null && connectionProfileFilter.SubGroups.ContainsKey(profileFiltersRow.FilterGroupNumber))
              {
                connectionParameterGroup1 = connectionProfileFilter.SubGroups[profileFiltersRow.FilterGroupNumber];
              }
              else
              {
                connectionParameterGroup1 = new ConnectionParameterGroup(profileFiltersRow.FilterGroupNumber);
                connectionProfileFilter.FilterGroups.Add(connectionParameterGroup1);
              }
              connectionParameterGroup1.GroupFunction = (ConnectionProfileFilterGroupFunctions) profileFiltersRow.GroupFunction;
            }
            if (profileFiltersRow.ConnectionProfileParameter == 0)
            {
              int num = int.Parse(profileFiltersRow.ParameterValue);
              if (connectionProfileFilter.SubGroups == null)
                connectionProfileFilter.SubGroups = new SortedList<int, ConnectionParameterGroup>();
              if (connectionParameterGroup1.SubGroups == null)
                connectionParameterGroup1.SubGroups = new List<ConnectionParameterGroup>();
              if (connectionProfileFilter.SubGroups.ContainsKey(num))
              {
                connectionParameterGroup1.SubGroups.Add(connectionProfileFilter.SubGroups[num]);
              }
              else
              {
                ConnectionParameterGroup connectionParameterGroup2 = new ConnectionParameterGroup(num);
                connectionProfileFilter.SubGroups.Add(connectionParameterGroup2.GroupNumber, connectionParameterGroup2);
                connectionParameterGroup1.SubGroups.Add(connectionParameterGroup2);
              }
            }
            else
            {
              string parameterValue = (string) null;
              if (!profileFiltersRow.IsParameterValueNull())
                parameterValue = profileFiltersRow.ParameterValue;
              connectionParameterGroup1.Parameters.Add(new ConnectionProfileParameterPair((ConnectionProfileParameter) profileFiltersRow.ConnectionProfileParameter, parameterValue));
            }
          }
        }
      }
      catch
      {
      }
    }

    public List<string> GetFilterList(
      SortedList<ConnectionProfileParameter, string> filtersFor = null)
    {
      if (filtersFor == null)
        return this.CachedProfileFilters.Keys.ToList<string>();
      List<string> filterList = new List<string>();
      foreach (ConnectionProfileFilter connectionProfileFilter in (IEnumerable<ConnectionProfileFilter>) this.CachedProfileFilters.Values)
      {
        if (connectionProfileFilter.IsFilterExpliciteDesignedFor(filtersFor))
          filterList.Add(connectionProfileFilter.Name);
      }
      return filterList;
    }

    internal void LoadConnectionProfiles()
    {
      this.ConnectionProfilesAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ConnectionProfiles", this.MyDbConnection);
      this.connectionProfilesDataTable = new Schema.ConnectionProfilesDataTable();
      this.ConnectionProfilesAdapter.Fill((DataTable) this.connectionProfilesDataTable);
      try
      {
        this.ConnectionProfilesParametersAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ConnectionProfileParameters", this.MyDbConnection);
        this.connectionProfileParametersDataTable = new Schema.ConnectionProfileParametersDataTable();
        this.ConnectionProfilesParametersAdapter.Fill((DataTable) this.connectionProfileParametersDataTable);
      }
      catch
      {
        this.connectionProfileParametersDataTable = (Schema.ConnectionProfileParametersDataTable) null;
      }
    }

    internal void LoadConnectionSettings()
    {
      this.ConnectionSettingsAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ConnectionSettings", this.MyDbConnection);
      this.connectionSettingsDataTable = new Schema.ConnectionSettingsDataTable();
      this.ConnectionSettingsAdapter.Fill((DataTable) this.connectionSettingsDataTable);
    }

    internal void LoadChangeableParameters()
    {
      this.ChangeableParametersAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM ChangeableParameters", this.MyDbConnection);
      this.changeableParametersDataTable = new Schema.ChangeableParametersDataTable();
      this.ChangeableParametersAdapter.Fill((DataTable) this.changeableParametersDataTable);
    }

    internal void LoadImages()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      lock (this.MyDbConnection)
      {
        this.GmmImageAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM GmmImages", this.MyDbConnection);
        this._gmmImageDataTable = new Schema.GmmImagesDataTable();
        this.GmmImageAdapter.Fill((DataTable) this._gmmImageDataTable);
      }
      stopwatch.Stop();
      ConfigDatabaseAccess.ReadoutDatabaseAccess.Trace("PictureLoadTime:" + stopwatch.Elapsed.TotalMilliseconds.ToString() + "ms");
    }

    internal void PreloadImages(List<int> theImageIDs)
    {
      List<int> intList;
      if (this.CachedBitmapImages == null)
      {
        this.CachedBitmapImages = new SortedList<int, BitmapImage>();
        intList = theImageIDs;
      }
      else
      {
        intList = new List<int>();
        foreach (int theImageId in theImageIDs)
        {
          if (!this.CachedBitmapImages.ContainsKey(theImageId))
            intList.Add(theImageId);
        }
      }
      if (intList.Count == 0)
        return;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder("SELECT * FROM GmmImages");
          stringBuilder.Append(" WHERE ImageID = " + intList[0].ToString());
          for (int index = 1; index < intList.Count; ++index)
            stringBuilder.Append(" OR ImageID = " + intList[index].ToString());
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          Schema.GmmImagesDataTable gmmImagesDataTable = new Schema.GmmImagesDataTable();
          dataAdapter.Fill((DataTable) gmmImagesDataTable);
          foreach (Schema.GmmImagesRow gmmImagesRow in (TypedTableBase<Schema.GmmImagesRow>) gmmImagesDataTable)
            this.CachedBitmapImages.Add(gmmImagesRow.ImageID, this.GetImage(gmmImagesRow.ImageData));
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error on preload images", ex);
      }
    }

    private BitmapImage GetBitmapImage(int imageId)
    {
      if (this.CachedBitmapImages != null && this.CachedBitmapImages.ContainsKey(imageId))
        return this.CachedBitmapImages[imageId];
      if (imageId == 0)
        return (BitmapImage) null;
      Schema.GmmImagesRow[] gmmImagesRowArray = (Schema.GmmImagesRow[]) this.gmmImageDataTable.Select("ImageID = " + imageId.ToString());
      return gmmImagesRowArray.Length != 1 ? (BitmapImage) null : this.GetImage(gmmImagesRowArray[0].ImageData);
    }

    private BitmapImage GetImage(byte[] imageData)
    {
      MemoryStream memoryStream = new MemoryStream(imageData);
      BitmapImage image = new BitmapImage();
      image.BeginInit();
      image.StreamSource = (Stream) memoryStream;
      image.EndInit();
      return image;
    }

    private string GetProfileTypeName(int ProfileTypeId)
    {
      lock (this.connectionItemsDataTable)
      {
        if (this.ProfileTypesById == null)
        {
          this.ProfileTypesById = new SortedList<int, Schema.ConnectionItemsRow>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.ProfileType.ToString() + "'"))
            this.ProfileTypesById.Add(connectionItemsRow.ConnectionItemID, connectionItemsRow);
        }
      }
      return this.ProfileTypesById[ProfileTypeId].Name;
    }

    internal List<DeviceGroup> GetDeviceGroups()
    {
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedDeviceGroups == null)
        {
          this.CachedDeviceGroups = new List<DeviceGroup>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.DeviceGroup.ToString() + "'"))
          {
            DeviceGroup deviceGroup = new DeviceGroup();
            deviceGroup.DeviceGroupID = connectionItemsRow.ConnectionItemID;
            deviceGroup.Name = connectionItemsRow.Name;
            deviceGroup.ImageID = connectionItemsRow.ImageID;
            deviceGroup.PreLoadImage = new System.Func<int, BitmapImage>(this.GetBitmapImage);
            if (!connectionItemsRow.IsDescriptionNull())
              deviceGroup.Description = connectionItemsRow.Description;
            this.CachedDeviceGroups.Add(deviceGroup);
          }
        }
      }
      return this.CachedDeviceGroups;
    }

    internal List<DeviceModel> GetDeviceModels()
    {
      if (this.CachedDeviceGroups == null)
        this.GetDeviceGroups();
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedDeviceModels == null)
        {
          this.CachedDeviceModels = new List<DeviceModel>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.DeviceModel.ToString() + "'"))
          {
            Schema.ConnectionItemsRow theRow = connectionItemsRow;
            DeviceModel deviceModel = new DeviceModel();
            deviceModel.DeviceModelID = theRow.ConnectionItemID;
            deviceModel.Name = theRow.Name;
            deviceModel.ImageID = theRow.ImageID;
            deviceModel.PreLoadImage = new System.Func<int, BitmapImage>(this.GetBitmapImage);
            if (this.connectionItemParametersDataTable != null)
            {
              Schema.ConnectionItemParametersRow[] itemParametersRowArray = (Schema.ConnectionItemParametersRow[]) this.connectionItemParametersDataTable.Select("ConnectionItemID = " + deviceModel.DeviceModelID.ToString());
              if (itemParametersRowArray != null && itemParametersRowArray.Length != 0)
              {
                deviceModel.Parameters = new SortedList<ConnectionProfileParameter, string>();
                foreach (Schema.ConnectionItemParametersRow itemParametersRow in itemParametersRowArray)
                {
                  ConnectionProfileParameter connectionItemParameter = (ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter;
                  if (!deviceModel.Parameters.ContainsKey(connectionItemParameter))
                  {
                    if (itemParametersRow.IsParameterValueNull())
                      deviceModel.Parameters.Add(connectionItemParameter, (string) null);
                    else
                      deviceModel.Parameters.Add(connectionItemParameter, itemParametersRow.ParameterValue);
                  }
                }
              }
            }
            if (!theRow.IsDescriptionNull())
              deviceModel.Description = theRow.Description;
            if (!theRow.IsManufacturerNull())
              deviceModel.Manufacturer = theRow.Manufacturer;
            if (!theRow.IsMediumNull())
              deviceModel.Medium = theRow.Medium;
            if (!theRow.IsGenerationNull())
              deviceModel.Generation = theRow.Generation;
            if (theRow.ItemGroupID > 0)
            {
              deviceModel.DeviceGroup = this.CachedDeviceGroups.Find((Predicate<DeviceGroup>) (x => x.DeviceGroupID == theRow.ItemGroupID));
              if (deviceModel.DeviceGroup != null)
              {
                deviceModel.ChangeableParameters = new List<ChangeableParameter>();
                foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) this.connectionProfilesDataTable)
                {
                  if (connectionProfilesRow.DeviceModelID == deviceModel.DeviceModelID && this.CachedConnectionSettingsById.ContainsKey(connectionProfilesRow.ConnectionSettingsID))
                  {
                    ConnectionSettings connectionSettings = this.CachedConnectionSettingsById[connectionProfilesRow.ConnectionSettingsID];
                    if (connectionSettings.ChangableDeviceParameters != null)
                    {
                      using (List<string>.Enumerator enumerator = connectionSettings.ChangableDeviceParameters.GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          string parameterName = enumerator.Current;
                          if (deviceModel.ChangeableParameters.FirstOrDefault<ChangeableParameter>((System.Func<ChangeableParameter, bool>) (x => x.Key == parameterName)) == null)
                          {
                            ChangeableParameter changeableParameter = this.ChangableParameterByName[parameterName].DeepCopy();
                            changeableParameter.Value = connectionSettings.AllChangableParameters[parameterName];
                            deviceModel.ChangeableParameters.Add(changeableParameter);
                          }
                        }
                        break;
                      }
                    }
                  }
                }
                this.CachedDeviceModels.Add(deviceModel);
              }
            }
          }
        }
      }
      return this.CachedDeviceModels;
    }

    internal List<EquipmentGroup> GetEquipmentGroups()
    {
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedEquipmentGroups == null)
        {
          this.CachedEquipmentGroups = new List<EquipmentGroup>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.EquipmentGroup.ToString() + "'"))
          {
            EquipmentGroup equipmentGroup = new EquipmentGroup();
            equipmentGroup.EquipmentGroupID = connectionItemsRow.ConnectionItemID;
            equipmentGroup.Name = connectionItemsRow.Name;
            equipmentGroup.ImageID = connectionItemsRow.ImageID;
            equipmentGroup.PreLoadImage = new System.Func<int, BitmapImage>(this.GetBitmapImage);
            if (!connectionItemsRow.IsDescriptionNull())
              equipmentGroup.Description = connectionItemsRow.Description;
            this.CachedEquipmentGroups.Add(equipmentGroup);
          }
        }
      }
      return this.CachedEquipmentGroups;
    }

    internal List<EquipmentModel> GetEquipmentModels()
    {
      if (this.CachedEquipmentGroups == null)
        this.GetEquipmentGroups();
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedEquipmentModels == null)
        {
          this.CachedEquipmentModels = new List<EquipmentModel>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.EquipmentModel.ToString() + "'"))
          {
            Schema.ConnectionItemsRow theRow = connectionItemsRow;
            EquipmentModel equipmentModel = new EquipmentModel();
            equipmentModel.EquipmentModelID = theRow.ConnectionItemID;
            equipmentModel.Name = theRow.Name;
            equipmentModel.ImageID = theRow.ImageID;
            equipmentModel.PreLoadImage = new System.Func<int, BitmapImage>(this.GetBitmapImage);
            if (this.connectionItemParametersDataTable != null)
            {
              Schema.ConnectionItemParametersRow[] itemParametersRowArray = (Schema.ConnectionItemParametersRow[]) this.connectionItemParametersDataTable.Select("ConnectionItemID = " + equipmentModel.EquipmentModelID.ToString());
              if (itemParametersRowArray != null && itemParametersRowArray.Length != 0)
              {
                equipmentModel.Parameters = new SortedList<ConnectionProfileParameter, string>();
                foreach (Schema.ConnectionItemParametersRow itemParametersRow in itemParametersRowArray)
                {
                  if (itemParametersRow.IsParameterValueNull())
                    equipmentModel.Parameters.Add((ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter, (string) null);
                  else
                    equipmentModel.Parameters.Add((ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter, itemParametersRow.ParameterValue);
                }
              }
            }
            if (!theRow.IsDescriptionNull())
              equipmentModel.Description = theRow.Description;
            if (theRow.ItemGroupID > 0)
            {
              equipmentModel.EquipmentGroup = this.CachedEquipmentGroups.Find((Predicate<EquipmentGroup>) (x => x.EquipmentGroupID == theRow.ItemGroupID));
              if (equipmentModel.EquipmentGroup != null)
              {
                equipmentModel.ChangeableParameters = new List<ChangeableParameter>();
                foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) this.connectionProfilesDataTable)
                {
                  if (connectionProfilesRow.EquipmentModelID == equipmentModel.EquipmentModelID && this.CachedConnectionSettingsById.ContainsKey(connectionProfilesRow.ConnectionSettingsID))
                  {
                    ConnectionSettings connectionSettings = this.CachedConnectionSettingsById[connectionProfilesRow.ConnectionSettingsID];
                    if (connectionSettings.ChangableEquipmentParameters != null)
                    {
                      using (List<string>.Enumerator enumerator = connectionSettings.ChangableEquipmentParameters.GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          string parameterName = enumerator.Current;
                          if (equipmentModel.ChangeableParameters.FirstOrDefault<ChangeableParameter>((System.Func<ChangeableParameter, bool>) (x => x.Key == parameterName)) == null)
                          {
                            ChangeableParameter changeableParameter = this.ChangableParameterByName[parameterName].DeepCopy();
                            changeableParameter.Value = connectionSettings.AllChangableParameters[parameterName];
                            equipmentModel.ChangeableParameters.Add(changeableParameter);
                          }
                        }
                        break;
                      }
                    }
                  }
                }
                this.CachedEquipmentModels.Add(equipmentModel);
              }
            }
          }
        }
      }
      return this.CachedEquipmentModels;
    }

    internal List<ProfileTypeGroup> GetProfileTypeGroups()
    {
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedProfileTypeGroups == null)
        {
          this.CachedProfileTypeGroups = new List<ProfileTypeGroup>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.ProfileTypeGroup.ToString() + "'"))
          {
            ProfileTypeGroup profileTypeGroup = new ProfileTypeGroup();
            profileTypeGroup.ProfileTypeGroupID = connectionItemsRow.ConnectionItemID;
            profileTypeGroup.Name = connectionItemsRow.Name;
            profileTypeGroup.ImageID = connectionItemsRow.ImageID;
            profileTypeGroup.PreLoadImage = new System.Func<int, BitmapImage>(this.GetBitmapImage);
            if (!connectionItemsRow.IsDescriptionNull())
              profileTypeGroup.Description = connectionItemsRow.Description;
            this.CachedProfileTypeGroups.Add(profileTypeGroup);
          }
        }
      }
      return this.CachedProfileTypeGroups;
    }

    internal List<ProfileType> GetProfileTypes()
    {
      if (this.CachedProfileTypeGroups == null)
        this.GetProfileTypeGroups();
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedProfileTypes == null)
        {
          this.CachedProfileTypes = new List<ProfileType>();
          foreach (Schema.ConnectionItemsRow connectionItemsRow in (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + ConnectionItemTypes.ProfileType.ToString() + "'"))
          {
            Schema.ConnectionItemsRow theRow = connectionItemsRow;
            ProfileType profileType = new ProfileType();
            profileType.ProfileTypeID = theRow.ConnectionItemID;
            profileType.Name = theRow.Name;
            profileType.ImageID = theRow.ImageID;
            profileType.PreLoadImage = new System.Func<int, BitmapImage>(this.GetBitmapImage);
            if (this.connectionItemParametersDataTable != null)
            {
              Schema.ConnectionItemParametersRow[] itemParametersRowArray = (Schema.ConnectionItemParametersRow[]) this.connectionItemParametersDataTable.Select("ConnectionItemID = " + profileType.ProfileTypeID.ToString());
              if (itemParametersRowArray != null && itemParametersRowArray.Length != 0)
              {
                profileType.Parameters = new SortedList<ConnectionProfileParameter, string>();
                foreach (Schema.ConnectionItemParametersRow itemParametersRow in itemParametersRowArray)
                {
                  if (itemParametersRow.IsParameterValueNull())
                    profileType.Parameters.Add((ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter, (string) null);
                  else
                    profileType.Parameters.Add((ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter, itemParametersRow.ParameterValue);
                }
              }
            }
            if (!theRow.IsDescriptionNull())
              profileType.Description = theRow.Description;
            if (theRow.ItemGroupID > 0)
            {
              profileType.ProfileTypeGroup = this.CachedProfileTypeGroups.Find((Predicate<ProfileTypeGroup>) (x => x.ProfileTypeGroupID == theRow.ItemGroupID));
              if (profileType.ProfileTypeGroup != null)
              {
                profileType.ChangeableParameters = new List<ChangeableParameter>();
                foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) this.connectionProfilesDataTable)
                {
                  if (connectionProfilesRow.ProfileTypeID == profileType.ProfileTypeID && this.CachedConnectionSettingsById.ContainsKey(connectionProfilesRow.ConnectionSettingsID))
                  {
                    ConnectionSettings connectionSettings = this.CachedConnectionSettingsById[connectionProfilesRow.ConnectionSettingsID];
                    if (connectionSettings.ChangableProfileTypeParameters != null)
                    {
                      using (List<string>.Enumerator enumerator = connectionSettings.ChangableProfileTypeParameters.GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          string parameterName = enumerator.Current;
                          if (profileType.ChangeableParameters.FirstOrDefault<ChangeableParameter>((System.Func<ChangeableParameter, bool>) (x => x.Key == parameterName)) == null)
                          {
                            ChangeableParameter changeableParameter = this.ChangableParameterByName[parameterName].DeepCopy();
                            changeableParameter.Value = connectionSettings.AllChangableParameters[parameterName];
                            profileType.ChangeableParameters.Add(changeableParameter);
                          }
                        }
                        break;
                      }
                    }
                  }
                }
                this.CachedProfileTypes.Add(profileType);
              }
            }
          }
        }
      }
      return this.CachedProfileTypes;
    }

    internal List<ConnectionSettings> GetReadoutSettings()
    {
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedConnectionSettingsById == null)
        {
          try
          {
            this.ChangableParameterByID = new SortedList<int, ChangeableParameter>();
            this.ChangableParameterByName = new SortedList<string, ChangeableParameter>();
            foreach (Schema.ChangeableParametersRow changeableParametersRow in (TypedTableBase<Schema.ChangeableParametersRow>) this.changeableParametersDataTable)
            {
              HashSet<ConfigurationParameterEnvironment> parameterEnvironmentSet = (HashSet<ConfigurationParameterEnvironment>) null;
              if (!changeableParametersRow.IsParameterEnvironmentNull())
              {
                string parameterEnvironment = changeableParametersRow.ParameterEnvironment;
                char[] separator = new char[1]{ ';' };
                foreach (string str in parameterEnvironment.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                  ConfigurationParameterEnvironment result;
                  if (Enum.TryParse<ConfigurationParameterEnvironment>(str.Trim(), out result))
                  {
                    if (parameterEnvironmentSet == null)
                      parameterEnvironmentSet = new HashSet<ConfigurationParameterEnvironment>();
                    parameterEnvironmentSet.Add(result);
                  }
                }
              }
              ChangeableParameter changeableParameter = new ChangeableParameter();
              if (parameterEnvironmentSet != null)
              {
                if (!parameterEnvironmentSet.Contains(ConfigurationParameterEnvironment.Obsolete))
                  changeableParameter.ParameterEnvironment = parameterEnvironmentSet;
                else
                  continue;
              }
              changeableParameter.Key = changeableParametersRow.Name;
              changeableParameter.Type = System.Type.GetType(changeableParametersRow.ParameterType);
              if (!changeableParametersRow.IsMaxValueNull())
                changeableParameter.ValueMax = (object) changeableParametersRow.MaxValue;
              else if (changeableParameter.Type != (System.Type) null)
              {
                FieldInfo field = changeableParameter.Type.GetField("MaxValue", BindingFlags.Static | BindingFlags.Public);
                if (field != (FieldInfo) null)
                  changeableParameter.ValueMax = Convert.ChangeType(field.GetValue((object) null), changeableParameter.Type);
              }
              if (!changeableParametersRow.IsMinValueNull())
                changeableParameter.ValueMin = (object) changeableParametersRow.MinValue;
              else if (changeableParameter.Type != (System.Type) null)
              {
                FieldInfo field = changeableParameter.Type.GetField("MinValue", BindingFlags.Static | BindingFlags.Public);
                if (field != (FieldInfo) null)
                  changeableParameter.ValueMin = Convert.ChangeType(field.GetValue((object) null), changeableParameter.Type);
              }
              if (!changeableParametersRow.IsValueListNull())
              {
                changeableParameter.AvailableValues = new List<ValueItem>();
                string valueList = changeableParametersRow.ValueList;
                char[] separator = new char[1]{ ';' };
                foreach (string str in ((IEnumerable<string>) valueList.Split(separator, StringSplitOptions.RemoveEmptyEntries)).ToList<string>())
                  changeableParameter.AvailableValues.Add(new ValueItem(str));
              }
              if (changeableParameter.Key == "Port")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableComPorts);
              else if (changeableParameter.Key == "Baudrate")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableBaudrates);
              else if (changeableParameter.Key == "COMserver")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableCOMserver);
              else if (changeableParameter.Key == "Parity")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableParity);
              else if (changeableParameter.Key == "TestEcho")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableTestEcho);
              else if (changeableParameter.Key == "Wakeup")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableWakeup);
              else if (changeableParameter.Key == "IrDaSelection")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableIrDaSelection);
              else if (changeableParameter.Key == "TransceiverDevice")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableTransceiverDevice);
              else if (changeableParameter.Key == "Type")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableAsyncComConnectionType);
              else if (changeableParameter.Key == "BusMode")
                changeableParameter.UpdateAvailableValuesHandler = new ChangeableParameter.UpdateAvailableValuesDelegate(Constants.GetAvailableBusMode);
              this.ChangableParameterByID.Add(changeableParametersRow.ParameterID, changeableParameter);
              this.ChangableParameterByName.Add(changeableParameter.Key, changeableParameter);
            }
            this.CachedConnectionSettingsById = new SortedList<int, ConnectionSettings>();
            foreach (Schema.ConnectionSettingsRow connectionSettingsRow in (TypedTableBase<Schema.ConnectionSettingsRow>) this.connectionSettingsDataTable)
            {
              if (this.ChangableParameterByID.ContainsKey(connectionSettingsRow.ParameterID))
              {
                ConnectionSettings connectionSettings;
                if (this.CachedConnectionSettingsById.ContainsKey(connectionSettingsRow.ConnectionSettingsID))
                {
                  connectionSettings = this.CachedConnectionSettingsById[connectionSettingsRow.ConnectionSettingsID];
                }
                else
                {
                  connectionSettings = new ConnectionSettings();
                  connectionSettings.ConnectionSettingsID = connectionSettingsRow.ConnectionSettingsID;
                  connectionSettings.SetupParameterList = new SortedList<string, string>();
                  this.CachedConnectionSettingsById.Add(connectionSettings.ConnectionSettingsID, connectionSettings);
                }
                string key = this.ChangableParameterByID[connectionSettingsRow.ParameterID].Key;
                if (!connectionSettingsRow.IsParameterValueNull())
                  connectionSettings.SetupParameterList.Add(key, connectionSettingsRow.ParameterValue);
                else
                  connectionSettings.SetupParameterList.Add(key, string.Empty);
                if (!connectionSettingsRow.IsParameterUsingNull())
                {
                  string parameterUsing1 = connectionSettingsRow.ParameterUsing;
                  ConnectionSettingsParameterUsing settingsParameterUsing = ConnectionSettingsParameterUsing.changableByDevice;
                  string str1 = settingsParameterUsing.ToString();
                  if (parameterUsing1 == str1)
                  {
                    if (connectionSettings.ChangableDeviceParameters == null)
                      connectionSettings.ChangableDeviceParameters = new List<string>();
                    connectionSettings.ChangableDeviceParameters.Add(key);
                  }
                  else
                  {
                    string parameterUsing2 = connectionSettingsRow.ParameterUsing;
                    settingsParameterUsing = ConnectionSettingsParameterUsing.changableByEquipment;
                    string str2 = settingsParameterUsing.ToString();
                    if (parameterUsing2 == str2)
                    {
                      if (connectionSettings.ChangableEquipmentParameters == null)
                        connectionSettings.ChangableEquipmentParameters = new List<string>();
                      connectionSettings.ChangableEquipmentParameters.Add(key);
                    }
                    else
                    {
                      string parameterUsing3 = connectionSettingsRow.ParameterUsing;
                      settingsParameterUsing = ConnectionSettingsParameterUsing.changableByProfileType;
                      string str3 = settingsParameterUsing.ToString();
                      if (parameterUsing3 == str3)
                      {
                        if (connectionSettings.ChangableProfileTypeParameters == null)
                          connectionSettings.ChangableProfileTypeParameters = new List<string>();
                        connectionSettings.ChangableProfileTypeParameters.Add(key);
                      }
                    }
                  }
                }
              }
            }
            foreach (ConnectionSettings connectionSettings in (IEnumerable<ConnectionSettings>) this.CachedConnectionSettingsById.Values)
            {
              int index1 = connectionSettings.SetupParameterList.IndexOfKey("TransceiverType");
              if (index1 >= 0)
              {
                connectionSettings.TransceiverType = (TransceiverType) Enum.Parse(typeof (TransceiverType), connectionSettings.SetupParameterList.Values[index1]);
                connectionSettings.SetupParameterList.RemoveAt(index1);
              }
              int index2 = connectionSettings.SetupParameterList.IndexOfKey("ConnectionSettingsName");
              if (index2 >= 0)
                connectionSettings.SetupParameterList.RemoveAt(index2);
              ConfigList configList = new ConfigList(connectionSettings.SetupParameterList);
              connectionSettings.Name = configList.Name;
            }
          }
          catch (Exception ex)
          {
            throw new Exception("Error by loading connection settings", ex);
          }
        }
      }
      return this.CachedConnectionSettingsById.Values.ToList<ConnectionSettings>();
    }

    internal List<ConnectionProfile> GetPartiallyConnectionProfiles(
      ConnectionProfileFilter profileFilter = null)
    {
      if (this.CachedEquipmentModels == null)
        this.GetEquipmentModels();
      if (this.CachedDeviceModels == null)
        this.GetDeviceModels();
      if (this.CachedConnectionSettingsById == null)
        this.GetReadoutSettings();
      lock (this.connectionItemsDataTable)
      {
        if (this.CachedPartiallyConnectionProfiles == null)
        {
          this.SettingsID_FromProfileID = new SortedList<int, int>();
          this.CachedPartiallyConnectionProfiles = new List<ConnectionProfile>();
          foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) this.connectionProfilesDataTable)
          {
            Schema.ConnectionProfilesRow profileRow = connectionProfilesRow;
            if (this.CachedConnectionSettingsById.ContainsKey(profileRow.ConnectionSettingsID))
            {
              ConnectionProfile theProfile = new ConnectionProfile();
              theProfile.ConnectionProfileID = profileRow.ConnectionProfileID;
              theProfile.EquipmentModel = this.CachedEquipmentModels.Find((Predicate<EquipmentModel>) (x => x.EquipmentModelID == profileRow.EquipmentModelID));
              theProfile.DeviceModel = this.CachedDeviceModels.Find((Predicate<DeviceModel>) (x => x.DeviceModelID == profileRow.DeviceModelID));
              theProfile.ProfileType = this.CachedProfileTypes.Find((Predicate<ProfileType>) (x => x.ProfileTypeID == profileRow.ProfileTypeID));
              theProfile.ConnectionSettings = this.CachedConnectionSettingsById[profileRow.ConnectionSettingsID];
              if (theProfile.EquipmentModel != null && theProfile.DeviceModel != null && theProfile.ProfileType != null && theProfile.ConnectionSettings != null)
              {
                this.SettingsID_FromProfileID.Add(profileRow.ConnectionProfileID, profileRow.ConnectionSettingsID);
                if (this.connectionProfileParametersDataTable != null)
                {
                  Schema.ConnectionProfileParametersRow[] profileParametersRowArray = (Schema.ConnectionProfileParametersRow[]) this.connectionProfileParametersDataTable.Select("ConnectionProfileID = " + theProfile.ConnectionProfileID.ToString());
                  if (profileParametersRowArray != null && profileParametersRowArray.Length != 0)
                  {
                    theProfile.Parameters = new SortedList<ConnectionProfileParameter, string>();
                    foreach (Schema.ConnectionProfileParametersRow profileParametersRow in profileParametersRowArray)
                    {
                      ConnectionProfileParameter profileParameter = (ConnectionProfileParameter) profileParametersRow.ConnectionProfileParameter;
                      if (profileParametersRow.IsParameterValueNull())
                        theProfile.Parameters.Add(profileParameter, (string) null);
                      else
                        theProfile.Parameters.Add(profileParameter, profileParametersRow.ParameterValue);
                    }
                  }
                }
                this.GarantSelectedDeviceMBusTypeFromHandler(theProfile);
                this.CachedPartiallyConnectionProfiles.Add(theProfile);
              }
            }
          }
        }
      }
      if (profileFilter == null)
        return this.CachedPartiallyConnectionProfiles;
      List<ConnectionProfile> connectionProfiles = new List<ConnectionProfile>();
      foreach (ConnectionProfile connectionProfile in this.CachedPartiallyConnectionProfiles)
      {
        if (profileFilter.IsSelectedByFilter(connectionProfile.CombinedParameters, connectionProfile.ConnectionSettings.SetupParameterList))
          connectionProfiles.Add(connectionProfile);
      }
      return connectionProfiles;
    }

    private void GarantSelectedDeviceMBusTypeFromHandler(ConnectionProfile theProfile)
    {
      if (theProfile.ConnectionSettings.SetupParameterList.IndexOfKey(ParameterKey.BusMode.ToString()) < 0 || theProfile.ConnectionSettings.SetupParameterList[ParameterKey.BusMode.ToString()] != ZENNER.CommonLibrary.BusMode.MBusPointToPoint.ToString())
        return;
      bool flag = false;
      string str = PointToPointDevices.MBus.ToString();
      if (theProfile.CombinedParameters.ContainsKey(ConnectionProfileParameter.Handler))
      {
        flag = true;
        switch (theProfile.CombinedParameters[ConnectionProfileParameter.Handler])
        {
          case "EDC_Handler":
            str = PointToPointDevices.EDC.ToString();
            break;
          case "TH_Handler":
            str = PointToPointDevices.HumiditySensor.ToString();
            break;
          case "MinolHandler":
            str = PointToPointDevices.Minol_Device.ToString();
            break;
          case "PDC_Handler":
            str = PointToPointDevices.PDC.ToString();
            break;
          case "GMM_Handler":
            str = PointToPointDevices.ZR_Serie2.ToString();
            break;
          case "S3_Handler":
            str = PointToPointDevices.ZR_Serie3.ToString();
            break;
          default:
            flag = false;
            break;
        }
      }
      if (!flag)
        return;
      if (theProfile.ConnectionSettings.SetupParameterList.IndexOfKey(ParameterKey.SelectedDeviceMBusType.ToString()) >= 0)
      {
        string setupParameter = theProfile.ConnectionSettings.SetupParameterList[ParameterKey.SelectedDeviceMBusType.ToString()];
        if (setupParameter == str)
          return;
        theProfile.ConnectionSettings = theProfile.ConnectionSettings.Clone();
        theProfile.ConnectionSettings.SetupParameterList[ParameterKey.SelectedDeviceMBusType.ToString()] = str;
        theProfile.ConnectionSettings.Name = new ConfigList(theProfile.ConnectionSettings.SetupParameterList).Name;
        ConfigDatabaseAccess.ReadoutDatabaseAccess.Info("MBusType in Profile " + theProfile.ConnectionProfileID.ToString() + " changed from " + setupParameter + " to " + str);
      }
      else
      {
        theProfile.ConnectionSettings = theProfile.ConnectionSettings.Clone();
        theProfile.ConnectionSettings.SetupParameterList.Add(ParameterKey.SelectedDeviceMBusType.ToString(), str);
        theProfile.ConnectionSettings.Name = new ConfigList(theProfile.ConnectionSettings.SetupParameterList).Name;
        ConfigDatabaseAccess.ReadoutDatabaseAccess.Info("MBusType added to Profile " + theProfile.ConnectionProfileID.ToString() + ": " + str);
      }
    }

    internal List<ConnectionProfile> GetConnectionProfiles(
      ConnectionProfileFilter profileFilter,
      bool fullConfiguration)
    {
      List<ConnectionProfile> connectionProfiles1 = this.GetPartiallyConnectionProfiles(profileFilter);
      List<ConnectionProfile> connectionProfiles2 = new List<ConnectionProfile>();
      foreach (ConnectionProfile connectionProfile1 in connectionProfiles1)
      {
        if (UserManager.IsDeviceModelAllowed(connectionProfile1.DeviceModel.Name))
        {
          ConnectionProfile connectionProfile2 = connectionProfile1.DeepCopy();
          connectionProfile2.DeviceModel.ChangeableParameters = new List<ChangeableParameter>();
          if (connectionProfile2.ConnectionSettings.ChangableDeviceParameters != null)
          {
            foreach (string changableDeviceParameter in connectionProfile2.ConnectionSettings.ChangableDeviceParameters)
            {
              ChangeableParameter changeableParameter = this.ChangableParameterByName[changableDeviceParameter].DeepCopy();
              changeableParameter.ParameterUsing = ChangeableParameterUsings.changableByDevice;
              changeableParameter.Value = connectionProfile2.ConnectionSettings.SetupParameterList[changableDeviceParameter];
              connectionProfile2.DeviceModel.ChangeableParameters.Add(changeableParameter);
            }
          }
          connectionProfile2.EquipmentModel.ChangeableParameters = new List<ChangeableParameter>();
          if (connectionProfile2.ConnectionSettings.ChangableEquipmentParameters != null)
          {
            foreach (string equipmentParameter in connectionProfile2.ConnectionSettings.ChangableEquipmentParameters)
            {
              ChangeableParameter changeableParameter = this.ChangableParameterByName[equipmentParameter].DeepCopy();
              changeableParameter.ParameterUsing = ChangeableParameterUsings.changableByEquipment;
              changeableParameter.Value = connectionProfile2.ConnectionSettings.SetupParameterList[equipmentParameter];
              connectionProfile2.EquipmentModel.ChangeableParameters.Add(changeableParameter);
            }
          }
          connectionProfile2.ProfileType.ChangeableParameters = new List<ChangeableParameter>();
          if (connectionProfile2.ConnectionSettings.ChangableProfileTypeParameters != null)
          {
            foreach (string profileTypeParameter in connectionProfile2.ConnectionSettings.ChangableProfileTypeParameters)
            {
              ChangeableParameter changeableParameter = this.ChangableParameterByName[profileTypeParameter].DeepCopy();
              changeableParameter.ParameterUsing = ChangeableParameterUsings.changableByProfileType;
              changeableParameter.Value = connectionProfile2.ConnectionSettings.SetupParameterList[profileTypeParameter];
              connectionProfile2.ProfileType.ChangeableParameters.Add(changeableParameter);
            }
          }
          if (fullConfiguration)
          {
            foreach (KeyValuePair<string, string> setupParameter in connectionProfile2.ConnectionSettings.SetupParameterList)
            {
              KeyValuePair<string, string> p = setupParameter;
              if ((connectionProfile2.EquipmentModel.ChangeableParameters == null || !connectionProfile2.EquipmentModel.ChangeableParameters.Exists((Predicate<ChangeableParameter>) (x => x.Key == p.Key))) && (connectionProfile2.DeviceModel.ChangeableParameters == null || !connectionProfile2.DeviceModel.ChangeableParameters.Exists((Predicate<ChangeableParameter>) (x => x.Key == p.Key))) && (connectionProfile2.ProfileType.ChangeableParameters == null || !connectionProfile2.ProfileType.ChangeableParameters.Exists((Predicate<ChangeableParameter>) (x => x.Key == p.Key))) && (!(p.Key == "BusMode") || UserManager.CheckPermission("Developer")))
              {
                if (connectionProfile2.ProfileType.ChangeableParameters == null)
                  connectionProfile2.ProfileType.ChangeableParameters = new List<ChangeableParameter>();
                ChangeableParameter changeableParameter = this.ChangableParameterByName[p.Key].DeepCopy();
                changeableParameter.ParameterUsing = ChangeableParameterUsings.standard;
                changeableParameter.Value = p.Value;
                connectionProfile2.ProfileType.ChangeableParameters.Add(changeableParameter);
              }
            }
          }
          connectionProfiles2.Add(connectionProfile2);
        }
      }
      return connectionProfiles2;
    }

    internal ObservableCollection<ConnectionListRow> GetConnectionProfilesList(
      int startProfileID,
      int selectedProfileID,
      ConnectionProfileFilter profileFilter = null)
    {
      this.currentConnectionProfilesList = new ObservableCollection<ConnectionListRow>();
      this.selectedProfileRow = (ConnectionListRow) null;
      foreach (ConnectionProfile connectionProfile in this.GetPartiallyConnectionProfiles(profileFilter))
      {
        ConnectionListRow connectionListRow = new ConnectionListRow();
        connectionListRow.ID = connectionProfile.ConnectionProfileID;
        if (connectionProfile.EquipmentModel != null)
        {
          if (connectionProfile.EquipmentModel.EquipmentGroup != null)
          {
            connectionListRow.EquipmentGroup = connectionProfile.EquipmentModel.EquipmentGroup.Name;
            connectionListRow.EquipmentGroupID = connectionProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID;
          }
          connectionListRow.EquipmentModel = connectionProfile.EquipmentModel.Name;
          connectionListRow.EquipmentModelID = connectionProfile.EquipmentModel.EquipmentModelID;
        }
        if (connectionProfile.DeviceModel != null)
        {
          if (connectionProfile.DeviceModel.DeviceGroup != null)
          {
            connectionListRow.DeviceGroup = connectionProfile.DeviceModel.DeviceGroup.Name;
            connectionListRow.DeviceGroupID = connectionProfile.DeviceModel.DeviceGroup.DeviceGroupID;
          }
          connectionListRow.DeviceModel = connectionProfile.DeviceModel.Name;
          connectionListRow.DeviceModelID = connectionProfile.DeviceModel.DeviceModelID;
        }
        if (connectionProfile.ProfileType != null)
        {
          if (connectionProfile.ProfileType.ProfileTypeGroup != null)
          {
            connectionListRow.ProfileTypeGroup = connectionProfile.ProfileType.ProfileTypeGroup.Name;
            connectionListRow.ProfileTypeGroupID = connectionProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID;
          }
          connectionListRow.ProfileType = connectionProfile.ProfileType.Name;
          connectionListRow.ProfileTypeID = connectionProfile.ProfileType.ProfileTypeID;
        }
        connectionListRow.SettingsId = connectionProfile.ConnectionSettings.ConnectionSettingsID;
        connectionListRow.SettingsName = connectionProfile.ConnectionSettings.Name;
        StringBuilder stringBuilder = new StringBuilder();
        if (connectionProfile.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in connectionProfile.Parameters)
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Append(';');
            stringBuilder.Append(parameter.Key.ToString());
            if (parameter.Value != null)
              stringBuilder.Append("=" + parameter.Value);
          }
        }
        if (connectionProfile.DeviceModel.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in connectionProfile.DeviceModel.Parameters)
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Append(';');
            stringBuilder.Append("D:" + parameter.Key.ToString());
            if (parameter.Value != null)
              stringBuilder.Append("=" + parameter.Value);
          }
        }
        if (connectionProfile.EquipmentModel.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in connectionProfile.EquipmentModel.Parameters)
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Append(';');
            stringBuilder.Append("E:" + parameter.Key.ToString());
            if (parameter.Value != null)
              stringBuilder.Append("=" + parameter.Value);
          }
        }
        if (connectionProfile.ProfileType.Parameters != null)
        {
          foreach (KeyValuePair<ConnectionProfileParameter, string> parameter in connectionProfile.ProfileType.Parameters)
          {
            if (stringBuilder.Length > 0)
              stringBuilder.Append(';');
            stringBuilder.Append("T:" + parameter.Key.ToString());
            if (parameter.Value != null)
              stringBuilder.Append("=" + parameter.Value);
          }
        }
        connectionListRow.Parameters = stringBuilder.ToString();
        this.currentConnectionProfilesList.Add(connectionListRow);
      }
      int num1 = 0;
      int num2 = 0;
      for (int index1 = 0; index1 < this.currentConnectionProfilesList.Count; ++index1)
      {
        if (num2 > 0)
        {
          num2 = 0;
          ++num1;
        }
        ConnectionListRow connectionProfiles1 = this.currentConnectionProfilesList[index1];
        for (int index2 = index1 + 1; index2 < this.currentConnectionProfilesList.Count; ++index2)
        {
          ConnectionListRow connectionProfiles2 = this.currentConnectionProfilesList[index2];
          if (connectionProfiles1.DeviceGroup == connectionProfiles2.DeviceGroup && connectionProfiles1.DeviceModel == connectionProfiles2.DeviceModel && connectionProfiles1.EquipmentGroup == connectionProfiles2.EquipmentGroup && connectionProfiles1.EquipmentModel == connectionProfiles2.EquipmentModel && connectionProfiles1.ProfileTypeGroup == connectionProfiles2.ProfileTypeGroup && connectionProfiles1.ProfileType == connectionProfiles2.ProfileType)
          {
            ++num2;
            connectionProfiles1.Mark = num1.ToString();
            connectionProfiles2.Mark = num1.ToString();
          }
        }
        if (connectionProfiles1.ID == startProfileID)
          connectionProfiles1.Mark = connectionProfiles1.Mark != null ? connectionProfiles1.Mark + "***" : "***";
        if (connectionProfiles1.ID == selectedProfileID)
        {
          this.selectedProfileRow = connectionProfiles1;
          connectionProfiles1.Mark = connectionProfiles1.Mark != null ? connectionProfiles1.Mark + "!!!" : "!!!";
        }
      }
      return this.currentConnectionProfilesList;
    }

    internal void MarkSelectedProfile(int selectedProfileID)
    {
      if (this.selectedProfileRow != null)
      {
        if (this.selectedProfileRow.Mark != null)
        {
          string str = this.selectedProfileRow.Mark.Replace("!", "").Trim();
          this.selectedProfileRow.Mark = str.Length <= 0 ? (string) null : str;
        }
        this.selectedProfileRow = (ConnectionListRow) null;
      }
      ConnectionListRow connectionListRow = this.currentConnectionProfilesList.First<ConnectionListRow>((System.Func<ConnectionListRow, bool>) (item => item.ID == selectedProfileID));
      if (connectionListRow == null)
        return;
      if (connectionListRow.Mark == null)
      {
        connectionListRow.Mark = "!!!";
      }
      else
      {
        string mark = connectionListRow.Mark;
        string str = "";
        for (int index = 0; index < mark.Length && char.IsDigit(mark[index]); ++index)
          str += mark[index].ToString();
        connectionListRow.Mark = str + "!!!";
        this.selectedProfileRow = connectionListRow;
      }
    }

    internal void CloneSettings(List<int> profileIdList)
    {
      if (profileIdList == null || profileIdList.Count == 0)
        return;
      int? nullable = new int?();
      foreach (int profileId1 in profileIdList)
      {
        int profileId = profileId1;
        ConnectionProfile connectionProfile = ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles().Find((Predicate<ConnectionProfile>) (x => x.ConnectionProfileID == profileId));
        if (!nullable.HasValue)
          nullable = new int?(connectionProfile.ConnectionSettings.ConnectionSettingsID);
        else if (nullable.Value != connectionProfile.ConnectionSettings.ConnectionSettingsID)
        {
          int num = (int) System.Windows.MessageBox.Show("More the one ConnectionSettingsID selected! This is not supported.");
          return;
        }
      }
      int newId = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionSettings", "ConnectionSettingsID");
      Schema.ConnectionSettingsRow[] connectionSettingsRowArray = (Schema.ConnectionSettingsRow[]) this.connectionSettingsDataTable.Select("ConnectionSettingsID=" + nullable.Value.ToString());
      for (int index = 0; index < connectionSettingsRowArray.Length; ++index)
      {
        Schema.ConnectionSettingsRow row = this.connectionSettingsDataTable.NewConnectionSettingsRow();
        row.ConnectionSettingsID = newId;
        row.ParameterID = connectionSettingsRowArray[index].ParameterID;
        row.ParameterValue = connectionSettingsRowArray[index].ParameterValue;
        row.ParameterUsing = connectionSettingsRowArray[index].ParameterUsing;
        this.connectionSettingsDataTable.AddConnectionSettingsRow(row);
      }
      this.ConnectionSettingsAdapter.Update((DataTable) this.connectionSettingsDataTable);
      foreach (int profileId in profileIdList)
        ((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("ConnectionProfileID=" + profileId.ToString()))[0].ConnectionSettingsID = newId;
      this.ConnectionProfilesAdapter.Update((DataTable) this.connectionProfilesDataTable);
      this.LoadAllConnectionTables();
    }

    internal void ChangeSettings(int ConnectionSettingsID, SortedList<string, string> newSettings)
    {
      bool flag1 = false;
      bool flag2 = false;
      foreach (KeyValuePair<string, string> newSetting in newSettings)
      {
        Schema.ChangeableParametersRow[] changeableParametersRowArray = (Schema.ChangeableParametersRow[]) this.changeableParametersDataTable.Select("Name = '" + newSetting.Key + "'");
        int num;
        if (changeableParametersRowArray == null || changeableParametersRowArray.Length == 0)
        {
          num = (int) Datenbankverbindung.MainDBAccess.GetNewId("ChangeableParameters", "ParameterID");
          Schema.ChangeableParametersRow row = this.changeableParametersDataTable.NewChangeableParametersRow();
          row.ParameterID = num;
          row.Name = newSetting.Key;
          row.ParameterType = typeof (string).ToString();
          this.changeableParametersDataTable.AddChangeableParametersRow(row);
          flag1 = true;
        }
        else
          num = changeableParametersRowArray[0].ParameterID;
        Schema.ConnectionSettingsRow[] connectionSettingsRowArray = (Schema.ConnectionSettingsRow[]) this.connectionSettingsDataTable.Select("ConnectionSettingsID=" + ConnectionSettingsID.ToString() + "AND ParameterID = " + num.ToString());
        if (connectionSettingsRowArray == null || connectionSettingsRowArray.Length == 0)
        {
          Schema.ConnectionSettingsRow row = this.connectionSettingsDataTable.NewConnectionSettingsRow();
          row.ConnectionSettingsID = ConnectionSettingsID;
          row.ParameterID = num;
          row.ParameterValue = newSetting.Value;
          row.ParameterUsing = ConnectionSettingsParameterUsing.standard.ToString();
          this.connectionSettingsDataTable.AddConnectionSettingsRow(row);
          flag2 = true;
        }
        else if (connectionSettingsRowArray[0].ParameterValue != newSetting.Value)
        {
          connectionSettingsRowArray[0].ParameterValue = newSetting.Value;
          flag2 = true;
        }
      }
      if (flag1)
        this.ChangeableParametersAdapter.Update((DataTable) this.changeableParametersDataTable);
      if (!flag2)
        return;
      this.ConnectionSettingsAdapter.Update((DataTable) this.connectionSettingsDataTable);
      this.LoadAllConnectionTables();
    }

    internal ObservableCollection<SettingsParameterData> GetSettingList(int ConnectionSettingsID)
    {
      Schema.ConnectionSettingsRow[] connectionSettingsRowArray = (Schema.ConnectionSettingsRow[]) this.connectionSettingsDataTable.Select("ConnectionSettingsID=" + ConnectionSettingsID.ToString());
      ObservableCollection<SettingsParameterData> source = new ObservableCollection<SettingsParameterData>();
      for (int index = 0; index < connectionSettingsRowArray.Length; ++index)
      {
        if (this.ChangableParameterByID.ContainsKey(connectionSettingsRowArray[index].ParameterID))
          source.Add(new SettingsParameterData()
          {
            Name = this.ChangableParameterByID[connectionSettingsRowArray[index].ParameterID].Key,
            Value = connectionSettingsRowArray[index].ParameterValue,
            Using = connectionSettingsRowArray[index].ParameterUsing
          });
      }
      return new ObservableCollection<SettingsParameterData>((IEnumerable<SettingsParameterData>) source.OrderBy<SettingsParameterData, string>((System.Func<SettingsParameterData, string>) (x => x.Name)));
    }

    internal bool ChangeSettingList(
      int ConnectionSettingsID,
      ObservableCollection<SettingsParameterData> newSettingsList)
    {
      bool flag1 = false;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (SettingsParameterData newSettings in (Collection<SettingsParameterData>) newSettingsList)
      {
        if (!(newSettings.Name == "ConnectionSettingsName"))
        {
          if (!Enum.TryParse<ParameterKey>(newSettings.Name, out ParameterKey _))
            stringBuilder.AppendLine("Illegal parameter name: " + newSettings.Name);
          else if (!Enum.TryParse<ConnectionSettingsParameterUsing>(newSettings.Using, out ConnectionSettingsParameterUsing _))
          {
            stringBuilder.AppendLine("Illegal parameter using: " + newSettings.Using);
          }
          else
          {
            Schema.ChangeableParametersRow[] changeableParametersRowArray = (Schema.ChangeableParametersRow[]) this.changeableParametersDataTable.Select("Name = '" + newSettings.Name + "'");
            if (changeableParametersRowArray == null || changeableParametersRowArray.Length == 0)
            {
              int newId = (int) Datenbankverbindung.MainDBAccess.GetNewId("ChangeableParameters", "ParameterID");
              Schema.ChangeableParametersRow row = this.changeableParametersDataTable.NewChangeableParametersRow();
              row.ParameterID = newId;
              row.Name = newSettings.Name;
              row.ParameterType = typeof (string).ToString();
              this.changeableParametersDataTable.AddChangeableParametersRow(row);
              flag1 = true;
            }
            else
            {
              int parameterId = changeableParametersRowArray[0].ParameterID;
            }
          }
        }
      }
      if (stringBuilder.Length > 0)
        throw new Exception(stringBuilder.ToString());
      if (flag1)
      {
        this.ChangeableParametersAdapter.Update((DataTable) this.changeableParametersDataTable);
        this.LoadChangeableParameters();
      }
      Schema.ConnectionSettingsRow[] SettingRows = (Schema.ConnectionSettingsRow[]) this.connectionSettingsDataTable.Select("ConnectionSettingsID=" + ConnectionSettingsID.ToString());
      List<SettingsParameterData> settingsParameterDataList = new List<SettingsParameterData>();
      bool flag2 = false;
      for (int i = 0; i < SettingRows.Length; i++)
      {
        if (!this.ChangableParameterByID.ContainsKey(SettingRows[i].ParameterID))
        {
          SettingRows[i].Delete();
          flag2 = true;
        }
        else
        {
          SettingsParameterData settingsParameterData = newSettingsList.FirstOrDefault<SettingsParameterData>((System.Func<SettingsParameterData, bool>) (x => x.Name == this.ChangableParameterByID[SettingRows[i].ParameterID].Key));
          if (settingsParameterData != null)
          {
            settingsParameterDataList.Add(settingsParameterData);
            if (settingsParameterData.Value != SettingRows[i].ParameterValue)
            {
              SettingRows[i].ParameterValue = settingsParameterData.Value;
              flag2 = true;
            }
            if (settingsParameterData.Using != SettingRows[i].ParameterUsing)
            {
              SettingRows[i].ParameterUsing = settingsParameterData.Using;
              flag2 = true;
            }
          }
          else
          {
            SettingRows[i].Delete();
            flag2 = true;
          }
        }
      }
      foreach (SettingsParameterData newSettings in (Collection<SettingsParameterData>) newSettingsList)
      {
        if (!settingsParameterDataList.Contains(newSettings))
        {
          Schema.ChangeableParametersRow[] changeableParametersRowArray = (Schema.ChangeableParametersRow[]) this.changeableParametersDataTable.Select("Name = '" + newSettings.Name + "'");
          Schema.ConnectionSettingsRow row = this.connectionSettingsDataTable.NewConnectionSettingsRow();
          row.ConnectionSettingsID = ConnectionSettingsID;
          row.ParameterID = changeableParametersRowArray[0].ParameterID;
          row.ParameterValue = newSettings.Value;
          row.ParameterUsing = newSettings.Using;
          this.connectionSettingsDataTable.AddConnectionSettingsRow(row);
          flag2 = true;
        }
      }
      if (flag2)
      {
        this.ConnectionSettingsAdapter.Update((DataTable) this.connectionSettingsDataTable);
        this.LoadAllConnectionTables();
      }
      return flag2;
    }

    internal bool ChangeNameAndDescription(int ConnectionItemID)
    {
      try
      {
        Schema.ConnectionItemsRow[] connectionItemsRowArray = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
        ReadoutConfiguration.ChangeNameAndDescription nameAndDescription = new ReadoutConfiguration.ChangeNameAndDescription();
        nameAndDescription.ConfigItemName = connectionItemsRowArray[0].Name;
        nameAndDescription.ConfigItemType = connectionItemsRowArray[0].ItemType;
        if (!connectionItemsRowArray[0].IsTypeClassificationNull())
          nameAndDescription.ConfigItemTypeClassification = connectionItemsRowArray[0].TypeClassification;
        if (!connectionItemsRowArray[0].IsDescriptionNull())
          nameAndDescription.ConfigItemDescription = connectionItemsRowArray[0].Description;
        bool? nullable = nameAndDescription.ShowDialog();
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        {
          connectionItemsRowArray[0].Name = nameAndDescription.ConfigItemName;
          if (string.IsNullOrEmpty(nameAndDescription.ConfigItemDescription))
            connectionItemsRowArray[0].SetDescriptionNull();
          else
            connectionItemsRowArray[0].Description = nameAndDescription.ConfigItemDescription;
          this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        }
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Change picture error", ex.ToString());
      }
      return false;
    }

    internal bool ChooseNewGroup(int ConnectionItemID)
    {
      try
      {
        Schema.ConnectionItemsRow[] connectionItemsRowArray1 = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
        if (connectionItemsRowArray1 == null || connectionItemsRowArray1.Length != 1)
          throw new Exception("ConnectionItem not found");
        Schema.ConnectionItemsRow[] connectionItemsRowArray2 = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + connectionItemsRowArray1[0].ItemGroupID.ToString());
        Schema.ConnectionItemsRow connectionItemsRow = connectionItemsRowArray2 != null && connectionItemsRowArray2.Length == 1 ? connectionItemsRowArray2[0] : throw new Exception("Original group not found");
        Schema.ConnectionItemsRow[] connectionItemsRowArray3 = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ItemType = '" + connectionItemsRow.ItemType + "'");
        string[] selectList = connectionItemsRowArray3 != null && connectionItemsRowArray3.Length >= 1 ? new string[connectionItemsRowArray3.Length] : throw new Exception("Group list not found");
        for (int index = 0; index < selectList.Length; ++index)
          selectList[index] = connectionItemsRowArray3[index].Name;
        string selectedValue = SelectOneValue.GetSelectedValue("Select group", "Select the new group", selectList, connectionItemsRow.Name);
        if (selectedValue != null)
        {
          Schema.ConnectionItemsRow[] connectionItemsRowArray4 = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("Name = '" + selectedValue + "'");
          if (connectionItemsRowArray4 == null || connectionItemsRowArray4.Length != 1)
            throw new Exception("Group not found");
          this.ChangeGroup(ConnectionItemID, connectionItemsRowArray4[0].ConnectionItemID);
          this.LoadAllConnectionTables();
        }
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Change picture error", ex.ToString());
      }
      return false;
    }

    internal bool ChangeGroup(int ConnectionItemID, int GroupID)
    {
      try
      {
        Schema.ConnectionItemsRow[] connectionItemsRowArray = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
        if (connectionItemsRowArray.Length != 1)
          throw new Exception("Connection imtem not found");
        connectionItemsRowArray[0].ItemGroupID = GroupID;
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("ChangeGroup error", ex.ToString());
      }
      return false;
    }

    internal int GetGroupID(int ConnectionItemID)
    {
      try
      {
        Schema.ConnectionItemsRow[] connectionItemsRowArray = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
        return connectionItemsRowArray.Length == 1 ? connectionItemsRowArray[0].ItemGroupID : throw new Exception("Connection item not found");
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("ChangeGroup error", ex.ToString());
      }
      return -1;
    }

    internal bool ChangePicture(int ConnectionItemID)
    {
      try
      {
        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
        openFileDialog.DefaultExt = ".png";
        openFileDialog.Filter = "Image documents (.png)|*.png";
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        {
          BitmapImage source = new BitmapImage();
          using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
          {
            source.BeginInit();
            source.StreamSource = (Stream) fileStream;
            source.EndInit();
            using (MemoryStream memoryStream = new MemoryStream())
            {
              PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
              pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
              pngBitmapEncoder.Save((Stream) memoryStream);
              Schema.ConnectionItemsRow[] connectionItemsRowArray = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
              if (connectionItemsRowArray[0].ImageID > 0)
              {
                ((Schema.GmmImagesRow[]) this.gmmImageDataTable.Select("ImageID = " + connectionItemsRowArray[0].ImageID.ToString()))[0].ImageData = memoryStream.ToArray();
                this.GmmImageAdapter.Update((DataTable) this.gmmImageDataTable);
              }
              else
              {
                int newId = (int) Datenbankverbindung.MainDBAccess.GetNewId("GmmImages", "ImageID");
                Schema.GmmImagesRow row = this.gmmImageDataTable.NewGmmImagesRow();
                row.ImageID = newId;
                row.ImageData = memoryStream.ToArray();
                this.gmmImageDataTable.AddGmmImagesRow(row);
                this.GmmImageAdapter.Update((DataTable) this.gmmImageDataTable);
                connectionItemsRowArray[0].ImageID = newId;
                this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
              }
              this.LoadAllConnectionTables();
              return true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Change picture from file error");
      }
      return false;
    }

    internal bool PastPicture(int ConnectionItemID)
    {
      try
      {
        System.Windows.DataObject dataObject = System.Windows.Clipboard.GetDataObject() as System.Windows.DataObject;
        if (!dataObject.GetDataPresent("PNG"))
        {
          int num = (int) System.Windows.MessageBox.Show("No PNG data found");
          return false;
        }
        if (!(dataObject.GetData("PNG") is MemoryStream data))
        {
          int num = (int) System.Windows.MessageBox.Show("Cannot load PNG data");
          return false;
        }
        Schema.ConnectionItemsRow[] connectionItemsRowArray = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
        if (connectionItemsRowArray[0].ImageID > 0)
        {
          ((Schema.GmmImagesRow[]) this.gmmImageDataTable.Select("ImageID = " + connectionItemsRowArray[0].ImageID.ToString()))[0].ImageData = data.ToArray();
          this.GmmImageAdapter.Update((DataTable) this.gmmImageDataTable);
        }
        else
        {
          int newId = (int) Datenbankverbindung.MainDBAccess.GetNewId("GmmImages", "ImageID");
          Schema.GmmImagesRow row = this.gmmImageDataTable.NewGmmImagesRow();
          row.ImageID = newId;
          row.ImageData = data.ToArray();
          this.gmmImageDataTable.AddGmmImagesRow(row);
          this.GmmImageAdapter.Update((DataTable) this.gmmImageDataTable);
          connectionItemsRowArray[0].ImageID = newId;
          this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        }
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Past picture error");
      }
      return false;
    }

    internal bool CopyPicture(int ConnectionItemID)
    {
      try
      {
        Schema.ConnectionItemsRow[] connectionItemsRowArray = (Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + ConnectionItemID.ToString());
        if (connectionItemsRowArray[0].ImageID > 0)
        {
          Schema.GmmImagesRow[] gmmImagesRowArray = (Schema.GmmImagesRow[]) this.gmmImageDataTable.Select("ImageID = " + connectionItemsRowArray[0].ImageID.ToString());
          System.Windows.Clipboard.Clear();
          System.Windows.DataObject data1 = new System.Windows.DataObject();
          using (MemoryStream data2 = new MemoryStream(gmmImagesRowArray[0].ImageData))
          {
            data1.SetData("PNG", (object) data2, false);
            using (MemoryStream bitmapStream = new MemoryStream(gmmImagesRowArray[0].ImageData))
            {
              PngBitmapDecoder pngBitmapDecoder = new PngBitmapDecoder((Stream) bitmapStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
              data1.SetData(System.Windows.DataFormats.Bitmap, (object) pngBitmapDecoder.Frames[0], true);
              System.Windows.Clipboard.SetDataObject((object) data1, true);
            }
          }
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Copy picture error");
      }
      return false;
    }

    internal bool AddDeviceModel(ConnectionProfile activeProfile)
    {
      try
      {
        if (((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("DeviceModelID = " + activeProfile.DeviceModel.DeviceModelID.ToString())).Length < 2)
          return this.ShowNeadAdditionalProfileErrorMessage("device model");
        Schema.ConnectionItemsRow row = this.connectionItemsDataTable.NewConnectionItemsRow();
        row.ConnectionItemID = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionItems", "ConnectionItemID");
        row.ItemGroupID = activeProfile.DeviceModel.DeviceGroup.DeviceGroupID;
        row.Name = this.GetUniqueConnectionItemName("New device model");
        row.ImageID = 0;
        row.ItemType = ConnectionItemTypes.DeviceModel.ToString();
        ((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("ConnectionProfileID = " + activeProfile.ConnectionProfileID.ToString()))[0].DeviceModelID = row.ConnectionItemID;
        this.connectionItemsDataTable.AddConnectionItemsRow(row);
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.ConnectionProfilesAdapter.Update((DataTable) this.connectionProfilesDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("AddDeviceModel error", ex.ToString());
      }
      return false;
    }

    internal bool AddDeviceGroupForDeviceModel(ConnectionProfile activeProfile)
    {
      try
      {
        Schema.ConnectionItemsRow row = this.connectionItemsDataTable.NewConnectionItemsRow();
        row.ConnectionItemID = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionItems", "ConnectionItemID");
        row.ItemGroupID = 0;
        row.Name = this.GetUniqueConnectionItemName("New profile type group");
        row.ImageID = 0;
        row.ItemType = ConnectionItemTypes.DeviceGroup.ToString();
        ((Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + activeProfile.DeviceModel.DeviceGroup.DeviceGroupID.ToString()))[0].ItemGroupID = row.ConnectionItemID;
        this.connectionItemsDataTable.AddConnectionItemsRow(row);
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("ProfileTypeGroup error", ex.ToString());
      }
      return false;
    }

    internal bool AddEquipmentModel(ConnectionProfile activeProfile)
    {
      try
      {
        if (((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("EquipmentModelID = " + activeProfile.EquipmentModel.EquipmentModelID.ToString())).Length < 2)
          return this.ShowNeadAdditionalProfileErrorMessage("equipment model");
        Schema.ConnectionItemsRow row = this.connectionItemsDataTable.NewConnectionItemsRow();
        row.ConnectionItemID = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionItems", "ConnectionItemID");
        row.ItemGroupID = activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID;
        row.Name = this.GetUniqueConnectionItemName("New Equipment model");
        row.ImageID = 0;
        row.ItemType = ConnectionItemTypes.EquipmentModel.ToString();
        ((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("ConnectionProfileID = " + activeProfile.ConnectionProfileID.ToString()))[0].EquipmentModelID = row.ConnectionItemID;
        this.connectionItemsDataTable.AddConnectionItemsRow(row);
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.ConnectionProfilesAdapter.Update((DataTable) this.connectionProfilesDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("AddEquipmentModel error", ex.ToString());
      }
      return false;
    }

    internal bool AddEquipmentGroupForModel(ConnectionProfile activeProfile)
    {
      try
      {
        Schema.ConnectionItemsRow row = this.connectionItemsDataTable.NewConnectionItemsRow();
        row.ConnectionItemID = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionItems", "ConnectionItemID");
        row.ItemGroupID = 0;
        row.Name = this.GetUniqueConnectionItemName("New equipment group");
        row.ImageID = 0;
        row.ItemType = ConnectionItemTypes.EquipmentGroup.ToString();
        ((Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + activeProfile.EquipmentModel.EquipmentModelID.ToString()))[0].ItemGroupID = row.ConnectionItemID;
        this.connectionItemsDataTable.AddConnectionItemsRow(row);
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("AddEquipmentModel error", ex.ToString());
      }
      return false;
    }

    internal bool AddProfileType(ConnectionProfile activeProfile)
    {
      try
      {
        if (((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("ProfileTypeID = " + activeProfile.ProfileType.ProfileTypeID.ToString())).Length < 2)
          return this.ShowNeadAdditionalProfileErrorMessage("profile type");
        Schema.ConnectionItemsRow row = this.connectionItemsDataTable.NewConnectionItemsRow();
        row.ConnectionItemID = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionItems", "ConnectionItemID");
        row.ItemGroupID = activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID;
        row.Name = this.GetUniqueConnectionItemName("New ProfileType model");
        row.ImageID = 0;
        row.ItemType = ConnectionItemTypes.ProfileType.ToString();
        ((Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("ConnectionProfileID = " + activeProfile.ConnectionProfileID.ToString()))[0].ProfileTypeID = row.ConnectionItemID;
        this.connectionItemsDataTable.AddConnectionItemsRow(row);
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.ConnectionProfilesAdapter.Update((DataTable) this.connectionProfilesDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("AddProfileType error", ex.ToString());
      }
      return false;
    }

    internal bool AddProfileTypeGroupForProfileType(ConnectionProfile activeProfile)
    {
      try
      {
        Schema.ConnectionItemsRow row = this.connectionItemsDataTable.NewConnectionItemsRow();
        row.ConnectionItemID = (int) Datenbankverbindung.MainDBAccess.GetNewId("ConnectionItems", "ConnectionItemID");
        row.ItemGroupID = 0;
        row.Name = this.GetUniqueConnectionItemName("New profile type group");
        row.ImageID = 0;
        row.ItemType = ConnectionItemTypes.ProfileTypeGroup.ToString();
        ((Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("ConnectionItemID = " + activeProfile.ProfileType.ProfileTypeID.ToString()))[0].ItemGroupID = row.ConnectionItemID;
        this.connectionItemsDataTable.AddConnectionItemsRow(row);
        this.ConnectionItemsAdapter.Update((DataTable) this.connectionItemsDataTable);
        this.LoadAllConnectionTables();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("ProfileTypeGroup error", ex.ToString());
      }
      return false;
    }

    internal string DeleteProfiles(List<int> connectionProfileIDsList)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Deleted profiles");
      stringBuilder1.AppendLine("------------------");
      stringBuilder1.AppendLine();
      try
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("SELECT * FROM ConnectionProfiles");
        int num1;
        for (int index = 0; index < connectionProfileIDsList.Count; ++index)
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          num1 = connectionProfileIDsList[index];
          string str1 = num1.ToString();
          stringBuilder3.AppendLine(str1);
          if (index == 0)
          {
            StringBuilder stringBuilder4 = stringBuilder2;
            num1 = connectionProfileIDsList[index];
            string str2 = " WHERE ConnectionProfileID = " + num1.ToString();
            stringBuilder4.Append(str2);
          }
          else
          {
            StringBuilder stringBuilder5 = stringBuilder2;
            num1 = connectionProfileIDsList[index];
            string str3 = " OR ConnectionProfileID = " + num1.ToString();
            stringBuilder5.Append(str3);
          }
        }
        string selectSql = stringBuilder2.ToString();
        DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
        DbCommandBuilder commandBuilder;
        DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out commandBuilder);
        Schema.ConnectionProfilesDataTable source1 = new Schema.ConnectionProfilesDataTable();
        dataAdapter1.Fill((DataTable) source1);
        DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionProfiles", newConnection);
        Schema.ConnectionProfilesDataTable source2 = new Schema.ConnectionProfilesDataTable();
        dataAdapter2.Fill((DataTable) source2);
        SortedList<int, int> sortedList1 = new SortedList<int, int>();
        SortedList<int, int> sortedList2 = new SortedList<int, int>();
        SortedList<int, int> sortedList3 = new SortedList<int, int>();
        SortedList<int, int> sortedList4 = new SortedList<int, int>();
        SortedList<int, int> sortedList5 = new SortedList<int, int>();
        StringBuilder stringBuilder6 = new StringBuilder();
        foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) source1)
        {
          Schema.ConnectionProfilesRow theRow = connectionProfilesRow;
          if (!sortedList1.ContainsKey(theRow.DeviceModelID))
          {
            if (source1.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.DeviceModelID == theRow.DeviceModelID)) >= source2.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.DeviceModelID == theRow.DeviceModelID)))
            {
              StringBuilder stringBuilder7 = stringBuilder6;
              num1 = theRow.DeviceModelID;
              string str = "DeviceModelID '" + num1.ToString() + "' is only available inside this range and will be deleted!";
              stringBuilder7.AppendLine(str);
              sortedList4.Add(theRow.DeviceModelID, 0);
            }
            sortedList1.Add(theRow.DeviceModelID, 0);
          }
          if (!sortedList2.ContainsKey(theRow.EquipmentModelID))
          {
            if (source1.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.EquipmentModelID == theRow.EquipmentModelID)) >= source2.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.EquipmentModelID == theRow.EquipmentModelID)))
            {
              StringBuilder stringBuilder8 = stringBuilder6;
              num1 = theRow.EquipmentModelID;
              string str = "EquipmentModelID '" + num1.ToString() + "' is only available inside this range and will be deleted!";
              stringBuilder8.AppendLine(str);
              sortedList4.Add(theRow.EquipmentModelID, 0);
            }
            sortedList2.Add(theRow.EquipmentModelID, 0);
          }
          if (!sortedList3.ContainsKey(theRow.ProfileTypeID))
          {
            if (source1.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.ProfileTypeID == theRow.ProfileTypeID)) >= source2.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.ProfileTypeID == theRow.ProfileTypeID)))
            {
              StringBuilder stringBuilder9 = stringBuilder6;
              num1 = theRow.ProfileTypeID;
              string str = "ProfileTypeID '" + num1.ToString() + "' is only available inside this range and will be deleted!";
              stringBuilder9.AppendLine(str);
              sortedList4.Add(theRow.ProfileTypeID, 0);
            }
            sortedList3.Add(theRow.ProfileTypeID, 0);
          }
          if (!sortedList5.ContainsKey(theRow.ConnectionSettingsID))
          {
            if (source1.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.ConnectionSettingsID == theRow.ConnectionSettingsID)) >= source2.Count<Schema.ConnectionProfilesRow>((System.Func<Schema.ConnectionProfilesRow, bool>) (item => item.ConnectionSettingsID == theRow.ConnectionSettingsID)))
            {
              StringBuilder stringBuilder10 = stringBuilder6;
              num1 = theRow.ConnectionSettingsID;
              string str = "ConnectionSettingsID '" + num1.ToString() + "' is only available inside this range and will be deleted!";
              stringBuilder10.AppendLine(str);
            }
            sortedList5.Add(theRow.ConnectionSettingsID, 0);
          }
        }
        if (stringBuilder6.Length > 0)
        {
          StringBuilder stringBuilder11 = new StringBuilder();
          stringBuilder11.AppendLine("****** Warning ******");
          stringBuilder11.AppendLine("Would you like to continue?");
          stringBuilder11.AppendLine("---------------------------------------");
          stringBuilder11.AppendLine();
          stringBuilder6.Insert(0, stringBuilder11.ToString());
          if (GMM_MessageBox.ShowMessage("Delete profiles", stringBuilder6.ToString(), MessageBoxButtons.OKCancel) != DialogResult.OK)
            return "Delete canceled. Nothing done.";
        }
        foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionProfilesRow>) source1)
          dataRow.Delete();
        int num2 = dataAdapter1.Update((DataTable) source1);
        stringBuilder1.Append("Number of deleted profiles: " + num2.ToString());
        if (sortedList4.Count > 0)
        {
          StringBuilder stringBuilder12 = new StringBuilder();
          stringBuilder12.Append("SELECT * FROM ConnectionItems");
          bool flag = true;
          foreach (int key in (IEnumerable<int>) sortedList4.Keys)
          {
            if (flag)
              stringBuilder12.Append(" WHERE ");
            else
              stringBuilder12.Append(" OR ");
          }
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder12.ToString(), newConnection, out commandBuilder);
          Schema.ConnectionItemsDataTable connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
        }
        this.LoadAllConnectionTables();
      }
      catch (Exception ex)
      {
        stringBuilder1.Clear();
        stringBuilder1.AppendLine("Delete error");
        stringBuilder1.AppendLine("Exception");
        stringBuilder1.AppendLine(ex.ToString());
        return stringBuilder1.ToString();
      }
      return stringBuilder1.ToString();
    }

    internal List<CommonEditValues> GetCommonProfileEditValues(List<int> connectionProfileIDsList)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Deleted profiles");
      stringBuilder1.AppendLine("------------------");
      stringBuilder1.AppendLine();
      List<CommonEditValues> profileEditValues = (List<CommonEditValues>) null;
      try
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("SELECT * FROM ConnectionProfiles");
        for (int index = 0; index < connectionProfileIDsList.Count; ++index)
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          int connectionProfileIds = connectionProfileIDsList[index];
          string str1 = connectionProfileIds.ToString();
          stringBuilder3.AppendLine(str1);
          if (index == 0)
          {
            StringBuilder stringBuilder4 = stringBuilder2;
            connectionProfileIds = connectionProfileIDsList[index];
            string str2 = " WHERE ConnectionProfileID = " + connectionProfileIds.ToString();
            stringBuilder4.Append(str2);
          }
          else
          {
            StringBuilder stringBuilder5 = stringBuilder2;
            connectionProfileIds = connectionProfileIDsList[index];
            string str3 = " OR ConnectionProfileID = " + connectionProfileIds.ToString();
            stringBuilder5.Append(str3);
          }
        }
        string selectSql = stringBuilder2.ToString();
        DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
        DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out DbCommandBuilder _).Fill((DataTable) new Schema.ConnectionProfilesDataTable());
      }
      catch (Exception ex)
      {
        stringBuilder1.Clear();
        stringBuilder1.AppendLine("Delete error");
        stringBuilder1.AppendLine("Exception");
        stringBuilder1.AppendLine(ex.ToString());
        return (List<CommonEditValues>) null;
      }
      return profileEditValues;
    }

    internal List<CommonEditValues> GetCommonSettingsEditValues(
      List<int> connectionProfileIDsList,
      out List<int> settingIds)
    {
      settingIds = (List<int>) null;
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Deleted profiles");
      stringBuilder1.AppendLine("------------------");
      stringBuilder1.AppendLine();
      List<CommonEditValues> settingsEditValues;
      try
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("SELECT * FROM ConnectionProfiles");
        for (int index = 0; index < connectionProfileIDsList.Count; ++index)
        {
          stringBuilder1.AppendLine(connectionProfileIDsList[index].ToString());
          if (index == 0)
            stringBuilder2.Append(" WHERE ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
          else
            stringBuilder2.Append(" OR ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
        }
        string selectSql1 = stringBuilder2.ToString();
        DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
        DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection);
        Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
        dataAdapter1.Fill((DataTable) profilesDataTable);
        settingIds = new List<int>();
        foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) profilesDataTable)
        {
          if (!settingIds.Contains(connectionProfilesRow.ConnectionSettingsID))
            settingIds.Add(connectionProfilesRow.ConnectionSettingsID);
        }
        stringBuilder2.Clear();
        stringBuilder2.Append("SELECT * FROM ConnectionSettings");
        for (int index = 0; index < settingIds.Count; ++index)
        {
          stringBuilder1.AppendLine(settingIds[index].ToString());
          if (index == 0)
            stringBuilder2.Append(" WHERE ConnectionSettingsID = " + settingIds[index].ToString());
          else
            stringBuilder2.Append(" OR ConnectionSettingsID = " + settingIds[index].ToString());
        }
        string selectSql2 = stringBuilder2.ToString();
        DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, out DbCommandBuilder _);
        Schema.ConnectionSettingsDataTable settingsDataTable = new Schema.ConnectionSettingsDataTable();
        dataAdapter2.Fill((DataTable) settingsDataTable);
        settingsEditValues = new List<CommonEditValues>();
        foreach (Schema.ConnectionSettingsRow connectionSettingsRow in (TypedTableBase<Schema.ConnectionSettingsRow>) settingsDataTable)
        {
          if (connectionSettingsRow.ParameterID != 1)
          {
            ChangeableParameter cParam = this.ChangableParameterByID[connectionSettingsRow.ParameterID];
            CommonEditValues commonEditValues = settingsEditValues.Find((Predicate<CommonEditValues>) (item => item.ValueName == cParam.Key));
            if (commonEditValues == null)
            {
              commonEditValues = new CommonEditValues(cParam.Key);
              settingsEditValues.Add(commonEditValues);
            }
            commonEditValues.AddValue(connectionSettingsRow.ParameterValue, connectionSettingsRow.ConnectionSettingsID);
            commonEditValues.AddEditBy(connectionSettingsRow.ParameterUsing, connectionSettingsRow.ConnectionSettingsID);
          }
        }
        foreach (CommonEditValues commonEditValues in settingsEditValues)
          commonEditValues.EscapeEdit(settingIds);
      }
      catch (Exception ex)
      {
        stringBuilder1.Clear();
        stringBuilder1.AppendLine("Delete error");
        stringBuilder1.AppendLine("Exception");
        stringBuilder1.AppendLine(ex.ToString());
        return (List<CommonEditValues>) null;
      }
      return settingsEditValues;
    }

    internal void SetCommonSettingsValues(
      string parameterName,
      string newValue,
      string newUsing,
      List<int> settingIds)
    {
      int key = this.ChangableParameterByID.Keys[this.ChangableParameterByID.IndexOfValue(this.ChangableParameterByName[parameterName])];
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SELECT * FROM ConnectionSettings");
      stringBuilder.Append(" WHERE ParameterID = " + key.ToString());
      for (int index = 0; index < settingIds.Count; ++index)
      {
        if (index == 0)
          stringBuilder.Append(" AND (ConnectionSettingsID = " + settingIds[index].ToString());
        else
          stringBuilder.Append(" OR ConnectionSettingsID = " + settingIds[index].ToString());
      }
      stringBuilder.Append(")");
      DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
      Schema.ConnectionSettingsDataTable settingsDataTable = new Schema.ConnectionSettingsDataTable();
      dataAdapter.Fill((DataTable) settingsDataTable);
      foreach (int settingId in settingIds)
      {
        Schema.ConnectionSettingsRow[] connectionSettingsRowArray = (Schema.ConnectionSettingsRow[]) settingsDataTable.Select("ConnectionSettingsID = " + settingId.ToString());
        if (connectionSettingsRowArray.Length == 1)
        {
          connectionSettingsRowArray[0].ParameterValue = newValue;
          connectionSettingsRowArray[0].ParameterUsing = newUsing;
        }
        else
        {
          if (connectionSettingsRowArray.Length != 0)
            throw new Exception("Illegal settings count");
          Schema.ConnectionSettingsRow row = settingsDataTable.NewConnectionSettingsRow();
          row.ConnectionSettingsID = settingId;
          row.ParameterID = key;
          row.ParameterValue = newValue;
          row.ParameterUsing = newUsing;
          settingsDataTable.AddConnectionSettingsRow(row);
        }
      }
      dataAdapter.Update((DataTable) settingsDataTable);
    }

    internal string CreateCopiedProfiles(
      List<int> connectionProfileIDsList,
      string newName,
      int groupID,
      ConnectionItemTypes itemType)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Copy profiles");
      stringBuilder1.AppendLine("------------------");
      stringBuilder1.AppendLine();
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          DbTransaction transaction = newConnection.BeginTransaction();
          string selectSql1 = "SELECT * FROM ConnectionItems WHERE Name = '" + newName + "'";
          DbCommandBuilder commandBuilder;
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection, transaction, out commandBuilder);
          Schema.ConnectionItemsDataTable connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
          dataAdapter1.Fill((DataTable) connectionItemsDataTable);
          Schema.ConnectionItemsRow row1 = connectionItemsDataTable.Count <= 0 ? connectionItemsDataTable.NewConnectionItemsRow() : throw new Exception("The name exists!");
          row1.ConnectionItemID = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("ConnectionItems");
          row1.ItemGroupID = groupID;
          row1.ItemType = itemType.ToString();
          row1.ImageID = 0;
          row1.ItemOrder = 0;
          row1.Name = newName;
          connectionItemsDataTable.AddConnectionItemsRow(row1);
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append("SELECT * FROM ConnectionProfiles");
          int num1;
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            StringBuilder stringBuilder3 = stringBuilder1;
            num1 = connectionProfileIDsList[index];
            string str1 = num1.ToString();
            stringBuilder3.AppendLine(str1);
            if (index == 0)
            {
              StringBuilder stringBuilder4 = stringBuilder2;
              num1 = connectionProfileIDsList[index];
              string str2 = " WHERE ConnectionProfileID = " + num1.ToString();
              stringBuilder4.Append(str2);
            }
            else
            {
              StringBuilder stringBuilder5 = stringBuilder2;
              num1 = connectionProfileIDsList[index];
              string str3 = " OR ConnectionProfileID = " + num1.ToString();
              stringBuilder5.Append(str3);
            }
          }
          string selectSql2 = stringBuilder2.ToString();
          DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, transaction, out commandBuilder);
          Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
          dataAdapter2.Fill((DataTable) profilesDataTable);
          if (profilesDataTable.Count != connectionProfileIDsList.Count)
            throw new Exception("Illegal number of profiles found");
          stringBuilder2.Clear();
          stringBuilder2.Append("SELECT * FROM ConnectionProfileParameters");
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            StringBuilder stringBuilder6 = stringBuilder1;
            num1 = connectionProfileIDsList[index];
            string str4 = num1.ToString();
            stringBuilder6.AppendLine(str4);
            if (index == 0)
            {
              StringBuilder stringBuilder7 = stringBuilder2;
              num1 = connectionProfileIDsList[index];
              string str5 = " WHERE ConnectionProfileID = " + num1.ToString();
              stringBuilder7.Append(str5);
            }
            else
            {
              StringBuilder stringBuilder8 = stringBuilder2;
              num1 = connectionProfileIDsList[index];
              string str6 = " OR ConnectionProfileID = " + num1.ToString();
              stringBuilder8.Append(str6);
            }
          }
          string selectSql3 = stringBuilder2.ToString();
          DbDataAdapter dataAdapter3 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql3, newConnection, transaction, out DbCommandBuilder _);
          Schema.ConnectionProfileParametersDataTable parametersDataTable1 = new Schema.ConnectionProfileParametersDataTable();
          dataAdapter3.Fill((DataTable) parametersDataTable1);
          IdContainer newIds = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("ConnectionProfiles", profilesDataTable.Count);
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            Schema.ConnectionProfilesRow connectionProfilesRow = profilesDataTable[index];
            Schema.ConnectionProfilesRow row2 = profilesDataTable.NewConnectionProfilesRow();
            row2.ConnectionProfileID = newIds.GetNextID();
            row2.DeviceModelID = connectionProfilesRow.DeviceModelID;
            row2.EquipmentModelID = connectionProfilesRow.EquipmentModelID;
            row2.ProfileTypeID = connectionProfilesRow.ProfileTypeID;
            row2.ConnectionSettingsID = connectionProfilesRow.ConnectionSettingsID;
            switch (itemType)
            {
              case ConnectionItemTypes.EquipmentModel:
                row2.EquipmentModelID = row1.ConnectionItemID;
                break;
              case ConnectionItemTypes.DeviceModel:
                row2.DeviceModelID = row1.ConnectionItemID;
                break;
              case ConnectionItemTypes.ProfileType:
                row2.ProfileTypeID = row1.ConnectionItemID;
                break;
              default:
                throw new Exception("Illegal item type");
            }
            profilesDataTable.AddConnectionProfilesRow(row2);
            Schema.ConnectionProfileParametersDataTable parametersDataTable2 = parametersDataTable1;
            num1 = connectionProfilesRow.ConnectionProfileID;
            string filterExpression = "ConnectionProfileID = " + num1.ToString();
            foreach (Schema.ConnectionProfileParametersRow profileParametersRow in (Schema.ConnectionProfileParametersRow[]) parametersDataTable2.Select(filterExpression))
            {
              Schema.ConnectionProfileParametersRow row3 = parametersDataTable1.NewConnectionProfileParametersRow();
              row3.ConnectionProfileID = row2.ConnectionProfileID;
              row3.ConnectionProfileParameter = profileParametersRow.ConnectionProfileParameter;
              row3.ParameterOrder = profileParametersRow.ParameterOrder;
              if (!profileParametersRow.IsParameterValueNull())
                row3.ParameterValue = profileParametersRow.ParameterValue;
              parametersDataTable1.AddConnectionProfileParametersRow(row3);
            }
          }
          dataAdapter1.Update((DataTable) connectionItemsDataTable);
          int num2 = dataAdapter2.Update((DataTable) profilesDataTable);
          dataAdapter3.Update((DataTable) parametersDataTable1);
          stringBuilder1.Append("Number of created profiles: " + num2.ToString());
          transaction.Commit();
          this.LoadAllConnectionTables();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on copy profiles");
        return (string) null;
      }
      return stringBuilder1.ToString();
    }

    internal string CreateCopiedProfiles(
      List<int> connectionProfileIDsList,
      ConnectionItemTypes itemType)
    {
      ConnectionProfile profileFromClipboard = this.GetProfileFromClipboard();
      int num1;
      switch (itemType)
      {
        case ConnectionItemTypes.EquipmentModel:
          num1 = profileFromClipboard.EquipmentModel.EquipmentModelID;
          break;
        case ConnectionItemTypes.DeviceModel:
          num1 = profileFromClipboard.DeviceModel.DeviceModelID;
          break;
        case ConnectionItemTypes.ProfileType:
          num1 = profileFromClipboard.ProfileType.ProfileTypeID;
          break;
        default:
          throw new Exception("Illegal item type");
      }
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Create new copied profiles");
      stringBuilder1.AppendLine("------------------");
      stringBuilder1.AppendLine();
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          DbTransaction transaction = newConnection.BeginTransaction();
          string selectSql1 = "SELECT * FROM ConnectionItems WHERE ConnectionItemID = " + num1.ToString();
          DbCommandBuilder commandBuilder;
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection, transaction, out commandBuilder);
          Schema.ConnectionItemsDataTable connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
          dataAdapter1.Fill((DataTable) connectionItemsDataTable);
          if (connectionItemsDataTable.Count != 1 || connectionItemsDataTable[0].ItemType != itemType.ToString())
            throw new Exception("Illegal item ID. (Wrong type or not available.");
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append("SELECT * FROM ConnectionProfiles");
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            stringBuilder1.AppendLine(connectionProfileIDsList[index].ToString());
            if (index == 0)
              stringBuilder2.Append(" WHERE ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
            else
              stringBuilder2.Append(" OR ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
          }
          string selectSql2 = stringBuilder2.ToString();
          DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, transaction, out commandBuilder);
          Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
          dataAdapter2.Fill((DataTable) profilesDataTable);
          if (profilesDataTable.Count != connectionProfileIDsList.Count)
            throw new Exception("Illegal number of profiles found");
          stringBuilder2.Clear();
          stringBuilder2.Append("SELECT * FROM ConnectionProfileParameters");
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            stringBuilder1.AppendLine(connectionProfileIDsList[index].ToString());
            if (index == 0)
              stringBuilder2.Append(" WHERE ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
            else
              stringBuilder2.Append(" OR ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
          }
          string selectSql3 = stringBuilder2.ToString();
          DbDataAdapter dataAdapter3 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql3, newConnection, transaction, out DbCommandBuilder _);
          Schema.ConnectionProfileParametersDataTable parametersDataTable = new Schema.ConnectionProfileParametersDataTable();
          dataAdapter3.Fill((DataTable) parametersDataTable);
          IdContainer newIds = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("ConnectionProfiles", profilesDataTable.Count);
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            Schema.ConnectionProfilesRow connectionProfilesRow = profilesDataTable[index];
            Schema.ConnectionProfilesRow row1 = profilesDataTable.NewConnectionProfilesRow();
            row1.ConnectionProfileID = newIds.GetNextID();
            row1.DeviceModelID = connectionProfilesRow.DeviceModelID;
            row1.EquipmentModelID = connectionProfilesRow.EquipmentModelID;
            row1.ProfileTypeID = connectionProfilesRow.ProfileTypeID;
            row1.ConnectionSettingsID = connectionProfilesRow.ConnectionSettingsID;
            switch (itemType)
            {
              case ConnectionItemTypes.EquipmentModel:
                row1.EquipmentModelID = num1;
                break;
              case ConnectionItemTypes.DeviceModel:
                row1.DeviceModelID = num1;
                break;
              case ConnectionItemTypes.ProfileType:
                row1.ProfileTypeID = num1;
                break;
            }
            profilesDataTable.AddConnectionProfilesRow(row1);
            foreach (Schema.ConnectionProfileParametersRow profileParametersRow in (Schema.ConnectionProfileParametersRow[]) parametersDataTable.Select("ConnectionProfileID = " + connectionProfilesRow.ConnectionProfileID.ToString()))
            {
              Schema.ConnectionProfileParametersRow row2 = parametersDataTable.NewConnectionProfileParametersRow();
              row2.ConnectionProfileID = row1.ConnectionProfileID;
              row2.ConnectionProfileParameter = profileParametersRow.ConnectionProfileParameter;
              row2.ParameterOrder = profileParametersRow.ParameterOrder;
              if (!profileParametersRow.IsParameterValueNull())
                row2.ParameterValue = profileParametersRow.ParameterValue;
              parametersDataTable.AddConnectionProfileParametersRow(row2);
            }
          }
          int num2 = dataAdapter2.Update((DataTable) profilesDataTable);
          dataAdapter3.Update((DataTable) parametersDataTable);
          stringBuilder1.Append("Number of created profiles: " + num2.ToString());
          transaction.Commit();
          this.LoadAllConnectionTables();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on copy profiles");
        return (string) null;
      }
      return stringBuilder1.ToString();
    }

    internal void CreateProfilesForMBusConverters(int ProfileTypeID)
    {
      ProfileType profileType = this.CachedProfileTypes.FirstOrDefault<ProfileType>((System.Func<ProfileType, bool>) (x => x.ProfileTypeID == ProfileTypeID));
      if (profileType == null)
        throw new Exception("ProfileType error");
      SortedList<int, EquipmentModel> sortedList1 = new SortedList<int, EquipmentModel>();
      SortedList<int, DeviceModel> sortedList2 = new SortedList<int, DeviceModel>();
      foreach (ConnectionProfile connectionProfile in this.CachedPartiallyConnectionProfiles)
      {
        if (connectionProfile.EquipmentModel.EquipmentGroup.Name == "Converters" && connectionProfile.ProfileType.ProfileTypeID == ProfileTypeID)
        {
          if (!sortedList1.ContainsKey(connectionProfile.EquipmentModel.EquipmentModelID))
            sortedList1.Add(connectionProfile.EquipmentModel.EquipmentModelID, connectionProfile.EquipmentModel);
          if (!sortedList2.ContainsKey(connectionProfile.DeviceModel.DeviceModelID))
            sortedList2.Add(connectionProfile.DeviceModel.DeviceModelID, connectionProfile.DeviceModel);
        }
      }
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          string selectSql = "SELECT * FROM ConnectionProfiles";
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out DbCommandBuilder _);
          Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
          StringBuilder stringBuilder = new StringBuilder();
          foreach (DeviceModel deviceModel in (IEnumerable<DeviceModel>) sortedList2.Values)
          {
            DeviceModel theModel = deviceModel;
            List<Schema.ConnectionProfilesRow> connectionProfilesRowList = new List<Schema.ConnectionProfilesRow>();
            int? nullable = new int?();
            foreach (EquipmentModel equipmentModel in (IEnumerable<EquipmentModel>) sortedList1.Values)
            {
              EquipmentModel theConverter = equipmentModel;
              ConnectionProfile connectionProfile = this.CachedPartiallyConnectionProfiles.SingleOrDefault<ConnectionProfile>((System.Func<ConnectionProfile, bool>) (p => p.DeviceModel.DeviceModelID == theModel.ID && p.EquipmentModel.EquipmentModelID == theConverter.EquipmentModelID && p.ProfileType.ProfileTypeID == ProfileTypeID));
              if (connectionProfile == null)
              {
                Schema.ConnectionProfilesRow connectionProfilesRow = profilesDataTable.NewConnectionProfilesRow();
                connectionProfilesRow.ConnectionProfileID = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("ConnectionProfiles");
                connectionProfilesRow.DeviceModelID = theModel.DeviceModelID;
                connectionProfilesRow.ProfileTypeID = ProfileTypeID;
                connectionProfilesRow.EquipmentModelID = theConverter.EquipmentModelID;
                connectionProfilesRowList.Add(connectionProfilesRow);
                stringBuilder.AppendLine(theModel.Name + ";" + theConverter.Name + ";" + profileType.Name);
              }
              else
                nullable = new int?(connectionProfile.ConnectionSettings.ConnectionSettingsID);
            }
            if (connectionProfilesRowList.Count > 0)
            {
              stringBuilder.AppendLine("   -> ConnectionSettingsID = " + nullable.Value.ToString());
              foreach (Schema.ConnectionProfilesRow row in connectionProfilesRowList)
              {
                row.ConnectionSettingsID = nullable.Value;
                profilesDataTable.AddConnectionProfilesRow(row);
              }
            }
          }
          if (GMM_MessageBox.ShowMessage("Create profiles", stringBuilder.ToString(), MessageBoxButtons.OKCancel) != DialogResult.OK)
            return;
          dataAdapter.Update((DataTable) profilesDataTable);
          this.LoadAllConnectionTables();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on copy profiles");
      }
    }

    internal void CheckSettings()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (ConnectionSettings connectionSettings in (IEnumerable<ConnectionSettings>) this.CachedConnectionSettingsById.Values)
      {
        SortedList<string, string> changableParameters = connectionSettings.AllChangableParameters;
        SortedList<string, string> setupParameterList = connectionSettings.SetupParameterList;
        if (!setupParameterList.ContainsKey("BusMode"))
        {
          stringBuilder.AppendLine("BusMode missed in: " + connectionSettings.ConnectionSettingsID.ToString());
        }
        else
        {
          switch (setupParameterList["BusMode"])
          {
            case "MBusPointToPoint":
              if (!setupParameterList.ContainsKey("OnlySecondaryAddressing"))
              {
                stringBuilder.AppendLine("OnlySecondaryAddressing missed in: " + connectionSettings.ConnectionSettingsID.ToString());
                break;
              }
              if (setupParameterList["OnlySecondaryAddressing"] != "False")
                stringBuilder.AppendLine("OnlySecondaryAddressing wrong in: " + connectionSettings.ConnectionSettingsID.ToString());
              break;
            case "MBus":
              if (!setupParameterList.ContainsKey("OnlySecondaryAddressing"))
                stringBuilder.AppendLine("OnlySecondaryAddressing missed in: " + connectionSettings.ConnectionSettingsID.ToString());
              break;
          }
        }
        SortedList<string, string> sortedList1 = changableParameters;
        ParameterKey parameterKey = ParameterKey.PrimaryAddress;
        string key1 = parameterKey.ToString();
        if (sortedList1.ContainsKey(key1))
        {
          SortedList<string, string> sortedList2 = changableParameters;
          parameterKey = ParameterKey.SecondaryAddress;
          string key2 = parameterKey.ToString();
          if (sortedList2.ContainsKey(key2))
            stringBuilder.AppendLine("PrimaryAddress and SecondaryAddress marked as changable in: " + connectionSettings.ConnectionSettingsID.ToString());
        }
      }
      int num = (int) System.Windows.MessageBox.Show(stringBuilder.ToString());
    }

    internal string ChangeIDs(
      List<int> connectionProfileIDsList,
      string newIdString,
      ConnectionItemTypes itemType)
    {
      int result;
      if (!int.TryParse(newIdString, out result) || result < 0)
        throw new Exception("Illegal ID number");
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Change id's");
      stringBuilder1.AppendLine("------------------");
      stringBuilder1.AppendLine();
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append("SELECT * FROM ConnectionProfiles");
          for (int index = 0; index < connectionProfileIDsList.Count; ++index)
          {
            stringBuilder1.AppendLine(connectionProfileIDsList[index].ToString());
            if (index == 0)
              stringBuilder2.Append(" WHERE ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
            else
              stringBuilder2.Append(" OR ConnectionProfileID = " + connectionProfileIDsList[index].ToString());
          }
          string selectSql = stringBuilder2.ToString();
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out DbCommandBuilder _);
          Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
          dataAdapter.Fill((DataTable) profilesDataTable);
          if (profilesDataTable.Count != connectionProfileIDsList.Count)
            throw new Exception("Illegal number of profiles found");
          foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) profilesDataTable)
          {
            switch (itemType)
            {
              case ConnectionItemTypes.EquipmentModel:
                connectionProfilesRow.EquipmentModelID = result;
                break;
              case ConnectionItemTypes.DeviceModel:
                connectionProfilesRow.DeviceModelID = result;
                break;
              case ConnectionItemTypes.ProfileType:
                connectionProfilesRow.ProfileTypeID = result;
                break;
              default:
                if (itemType != ~ConnectionItemTypes.EquipmentModel)
                  throw new Exception("Illegal ID type");
                connectionProfilesRow.ConnectionSettingsID = result;
                break;
            }
          }
          int num = dataAdapter.Update((DataTable) profilesDataTable);
          stringBuilder1.Append("Number of changed profiles: " + num.ToString());
          this.LoadAllConnectionTables();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on copy profiles");
        return (string) null;
      }
      return stringBuilder1.ToString();
    }

    internal int CopyTagsToParameters()
    {
      int parameters = 0;
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionItems", newConnection);
        Schema.ConnectionItemsDataTable connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
        dataAdapter1.Fill((DataTable) connectionItemsDataTable);
        newConnection.Open();
        DbTransaction transaction = newConnection.BeginTransaction();
        foreach (Schema.ConnectionItemsRow connectionItemsRow in (TypedTableBase<Schema.ConnectionItemsRow>) connectionItemsDataTable)
        {
          if (!connectionItemsRow.IsTypeClassificationNull())
          {
            string[] strArray = connectionItemsRow.TypeClassification.Split(new char[1]
            {
              ';'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length >= 1)
            {
              DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionItemParameters WHERE ConnectionItemID = " + connectionItemsRow.ConnectionItemID.ToString(), newConnection, transaction, out DbCommandBuilder _);
              Schema.ConnectionItemParametersDataTable parametersDataTable = new Schema.ConnectionItemParametersDataTable();
              dataAdapter2.Fill((DataTable) parametersDataTable);
              SortedList<ConnectionProfileParameter, string> sortedList = new SortedList<ConnectionProfileParameter, string>();
              foreach (Schema.ConnectionItemParametersRow itemParametersRow in (TypedTableBase<Schema.ConnectionItemParametersRow>) parametersDataTable)
              {
                ConnectionProfileParameter connectionItemParameter = (ConnectionProfileParameter) itemParametersRow.ConnectionItemParameter;
                string str = (string) null;
                if (!itemParametersRow.IsParameterValueNull())
                  str = itemParametersRow.ParameterValue;
                sortedList.Add(connectionItemParameter, str);
              }
              int num = parameters;
              foreach (string str in strArray)
              {
                ConnectionProfileParameter key = (ConnectionProfileParameter) Enum.Parse(typeof (ConnectionProfileParameter), str);
                if (!sortedList.ContainsKey(key))
                {
                  sortedList.Add(key, (string) null);
                  ++parameters;
                }
              }
              if (!connectionItemsRow.IsManufacturerNull() && connectionItemsRow.Manufacturer.Length > 0)
              {
                if (sortedList.ContainsKey(ConnectionProfileParameter.Manufacturer))
                {
                  if (sortedList[ConnectionProfileParameter.Manufacturer] != connectionItemsRow.Manufacturer)
                  {
                    sortedList[ConnectionProfileParameter.Manufacturer] = connectionItemsRow.Manufacturer;
                    ++parameters;
                  }
                }
                else
                {
                  sortedList.Add(ConnectionProfileParameter.Manufacturer, connectionItemsRow.Manufacturer);
                  ++parameters;
                }
              }
              if (!connectionItemsRow.IsMediumNull() && connectionItemsRow.Medium.Length > 0)
              {
                if (sortedList.ContainsKey(ConnectionProfileParameter.Medium))
                {
                  if (sortedList[ConnectionProfileParameter.Medium] != connectionItemsRow.Medium)
                  {
                    sortedList[ConnectionProfileParameter.Medium] = connectionItemsRow.Medium;
                    ++parameters;
                  }
                }
                else
                {
                  sortedList.Add(ConnectionProfileParameter.Medium, connectionItemsRow.Medium);
                  ++parameters;
                }
              }
              if (!connectionItemsRow.IsGenerationNull() && connectionItemsRow.Generation.Length > 0)
              {
                if (sortedList.ContainsKey(ConnectionProfileParameter.Generation))
                {
                  if (sortedList[ConnectionProfileParameter.Generation] != connectionItemsRow.Generation)
                  {
                    sortedList[ConnectionProfileParameter.Generation] = connectionItemsRow.Generation;
                    ++parameters;
                  }
                }
                else
                {
                  sortedList.Add(ConnectionProfileParameter.Generation, connectionItemsRow.Generation);
                  ++parameters;
                }
              }
              if (num < parameters)
              {
                foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionItemParametersRow>) parametersDataTable)
                  dataRow.Delete();
                dataAdapter2.Update((DataTable) parametersDataTable);
                for (int index = 0; index < sortedList.Count; ++index)
                {
                  Schema.ConnectionItemParametersRow row = parametersDataTable.NewConnectionItemParametersRow();
                  row.ConnectionItemID = connectionItemsRow.ConnectionItemID;
                  row.ConnectionItemParameter = (int) sortedList.Keys[index];
                  row.ParameterOrder = index;
                  if (sortedList.Values[index] != null)
                    row.ParameterValue = sortedList.Values[index];
                  parametersDataTable.AddConnectionItemParametersRow(row);
                }
                dataAdapter2.Update((DataTable) parametersDataTable);
              }
              dataAdapter2.Dispose();
            }
          }
        }
        transaction.Commit();
        newConnection.Close();
      }
      return parameters;
    }

    internal void CopyProfileToClipboard(int profileID)
    {
      System.Windows.Clipboard.SetDataObject((object) ("ProfileID:" + profileID.ToString()));
    }

    internal ConnectionProfile GetProfileFromClipboard()
    {
      string text = System.Windows.Clipboard.GetText();
      if (text == null || !text.StartsWith("ProfileID:"))
        throw new Exception("No profile in Clipboard");
      int profileID;
      if (!int.TryParse(text.Substring("ProfileID:".Length), out profileID))
        throw new Exception("Profile ID error");
      return this.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((System.Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == profileID));
    }

    internal string PastDeviceParametersFromClipboard(List<int> theIds)
    {
      return this.PastProfileItemParametersFromClipboard(theIds, (IConnectionDeviceItem) this.GetProfileFromClipboard().DeviceModel);
    }

    internal string PastEquipmentParametersFromClipboard(List<int> theIds)
    {
      return this.PastProfileItemParametersFromClipboard(theIds, (IConnectionDeviceItem) this.GetProfileFromClipboard().EquipmentModel);
    }

    internal string PastTypeParametersFromClipboard(List<int> theIds)
    {
      return this.PastProfileItemParametersFromClipboard(theIds, (IConnectionDeviceItem) this.GetProfileFromClipboard().ProfileType);
    }

    internal string PastProfileParametersFromClipboard(List<int> profileIds)
    {
      ConnectionProfile profileFromClipboard = this.GetProfileFromClipboard();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        newConnection.Open();
        DbTransaction myTransaction = newConnection.BeginTransaction();
        foreach (int profileId in profileIds)
        {
          int profileID = profileId;
          if (profileID != profileFromClipboard.ConnectionProfileID)
            this.ChangeProfileParameters(this.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((System.Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == profileID)).ConnectionProfileID, profileFromClipboard.Parameters, newConnection, myTransaction);
        }
        myTransaction.Commit();
        newConnection.Close();
      }
      this.LoadAllConnectionTables();
      return "ok";
    }

    internal string PastProfileItemParametersFromClipboard(
      List<int> profileIds,
      IConnectionDeviceItem sourceItem)
    {
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        newConnection.Open();
        DbTransaction myTransaction = newConnection.BeginTransaction();
        HashSet<int> intSet = new HashSet<int>();
        foreach (int profileId in profileIds)
        {
          int profileID = profileId;
          ConnectionProfile connectionProfile = this.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((System.Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == profileID));
          IConnectionDeviceItem connectionDeviceItem;
          switch (sourceItem)
          {
            case DeviceModel _:
              connectionDeviceItem = (IConnectionDeviceItem) connectionProfile.DeviceModel;
              break;
            case EquipmentModel _:
              connectionDeviceItem = (IConnectionDeviceItem) connectionProfile.EquipmentModel;
              break;
            case ProfileType _:
              connectionDeviceItem = (IConnectionDeviceItem) connectionProfile.ProfileType;
              break;
            default:
              throw new Exception("Illegal item type");
          }
          if (connectionDeviceItem.ID != sourceItem.ID && !intSet.Contains(connectionDeviceItem.ID))
          {
            intSet.Add(connectionDeviceItem.ID);
            this.ChangeItemParameters(connectionDeviceItem.ID, sourceItem.Parameters, newConnection, myTransaction);
          }
        }
        myTransaction.Commit();
        newConnection.Close();
      }
      this.LoadAllConnectionTables();
      return "ok";
    }

    private void ChangeItemParameters(
      int connectionItemID,
      SortedList<ConnectionProfileParameter, string> theParameters,
      DbConnection myConnection,
      DbTransaction myTransaction)
    {
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionItemParameters WHERE ConnectionItemID = " + connectionItemID.ToString(), myConnection, myTransaction, out DbCommandBuilder _);
      Schema.ConnectionItemParametersDataTable parametersDataTable = new Schema.ConnectionItemParametersDataTable();
      dataAdapter.Fill((DataTable) parametersDataTable);
      foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionItemParametersRow>) parametersDataTable)
        dataRow.Delete();
      dataAdapter.Update((DataTable) parametersDataTable);
      if (theParameters != null)
      {
        for (int index = 0; index < theParameters.Count; ++index)
        {
          if (theParameters.Keys[index] != ConnectionProfileParameter.None)
          {
            Schema.ConnectionItemParametersRow row = parametersDataTable.NewConnectionItemParametersRow();
            row.ConnectionItemID = connectionItemID;
            row.ConnectionItemParameter = (int) theParameters.Keys[index];
            row.ParameterOrder = index;
            if (theParameters.Values[index] != null)
              row.ParameterValue = theParameters.Values[index];
            parametersDataTable.AddConnectionItemParametersRow(row);
          }
        }
        dataAdapter.Update((DataTable) parametersDataTable);
      }
      dataAdapter.Dispose();
    }

    private void ChangeProfileParameters(
      int profileID,
      SortedList<ConnectionProfileParameter, string> theParameters,
      DbConnection myConnection,
      DbTransaction myTransaction)
    {
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionProfileParameters WHERE ConnectionProfileID = " + profileID.ToString(), myConnection, myTransaction, out DbCommandBuilder _);
      Schema.ConnectionProfileParametersDataTable parametersDataTable = new Schema.ConnectionProfileParametersDataTable();
      dataAdapter.Fill((DataTable) parametersDataTable);
      foreach (DataRow dataRow in (TypedTableBase<Schema.ConnectionProfileParametersRow>) parametersDataTable)
        dataRow.Delete();
      dataAdapter.Update((DataTable) parametersDataTable);
      if (theParameters != null)
      {
        for (int index = 0; index < theParameters.Count; ++index)
        {
          if (theParameters.Keys[index] != ConnectionProfileParameter.None)
          {
            Schema.ConnectionProfileParametersRow row = parametersDataTable.NewConnectionProfileParametersRow();
            row.ConnectionProfileID = profileID;
            row.ConnectionProfileParameter = (int) theParameters.Keys[index];
            row.ParameterOrder = index;
            if (theParameters.Values[index] != null)
              row.ParameterValue = theParameters.Values[index];
            parametersDataTable.AddConnectionProfileParametersRow(row);
          }
        }
        dataAdapter.Update((DataTable) parametersDataTable);
      }
      dataAdapter.Dispose();
    }

    internal void DeleteCommonSettingsValues(string parameterName, List<int> settingIds)
    {
      int key = this.ChangableParameterByID.Keys[this.ChangableParameterByID.IndexOfValue(this.ChangableParameterByName[parameterName])];
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SELECT * FROM ConnectionSettings");
      stringBuilder.Append(" WHERE ParameterID = " + key.ToString());
      for (int index = 0; index < settingIds.Count; ++index)
      {
        if (index == 0)
          stringBuilder.Append(" AND (ConnectionSettingsID = " + settingIds[index].ToString());
        else
          stringBuilder.Append(" OR ConnectionSettingsID = " + settingIds[index].ToString());
      }
      stringBuilder.Append(")");
      DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
      Schema.ConnectionSettingsDataTable settingsDataTable = new Schema.ConnectionSettingsDataTable();
      dataAdapter.Fill((DataTable) settingsDataTable);
      foreach (int settingId in settingIds)
      {
        Schema.ConnectionSettingsRow[] connectionSettingsRowArray = (Schema.ConnectionSettingsRow[]) settingsDataTable.Select("ConnectionSettingsID = " + settingId.ToString());
        if (connectionSettingsRowArray.Length == 1)
          connectionSettingsRowArray[0].Delete();
        else if (connectionSettingsRowArray.Length > 1)
          throw new Exception("Illegal settings count");
      }
      dataAdapter.Update((DataTable) settingsDataTable);
    }

    internal void DeleteConnectionItems(List<int> itemIds)
    {
      if (itemIds == null || itemIds.Count == 0)
        return;
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("DELETE FROM ConnectionItems");
      int itemId;
      for (int index = 0; index < itemIds.Count; ++index)
      {
        if (index == 0)
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          itemId = itemIds[index];
          string str = " WHERE (ConnectionItemID = " + itemId.ToString();
          stringBuilder2.Append(str);
        }
        else
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          itemId = itemIds[index];
          string str = " OR ConnectionItemID = " + itemId.ToString();
          stringBuilder3.Append(str);
        }
      }
      stringBuilder1.Append(")");
      DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = stringBuilder1.ToString();
        command.ExecuteNonQuery();
      }
    }

    internal void DeleteSettings(List<int> settingIds)
    {
      if (settingIds == null || settingIds.Count == 0)
        return;
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("DELETE FROM ConnectionSettings");
      int settingId;
      for (int index = 0; index < settingIds.Count; ++index)
      {
        if (index == 0)
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          settingId = settingIds[index];
          string str = " WHERE (ConnectionSettingsID = " + settingId.ToString();
          stringBuilder2.Append(str);
        }
        else
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          settingId = settingIds[index];
          string str = " OR ConnectionSettingsID = " + settingId.ToString();
          stringBuilder3.Append(str);
        }
      }
      stringBuilder1.Append(")");
      DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = stringBuilder1.ToString();
        command.ExecuteNonQuery();
      }
    }

    private string GetUniqueConnectionItemName(string baseName)
    {
      baseName = baseName.Trim();
      while (((Schema.ConnectionItemsRow[]) this.connectionItemsDataTable.Select("Name = '" + baseName + "'")).Length != 0)
        baseName += "_";
      return baseName;
    }

    internal void ChangeConnectionProfile(ConnectionListRow listRow)
    {
      Schema.ConnectionProfilesRow[] connectionProfilesRowArray = (Schema.ConnectionProfilesRow[]) this.connectionProfilesDataTable.Select("ConnectionProfileID=" + listRow.ID.ToString());
      connectionProfilesRowArray[0].DeviceModelID = listRow.DeviceModelID;
      connectionProfilesRowArray[0].EquipmentModelID = listRow.EquipmentModelID;
      connectionProfilesRowArray[0].ProfileTypeID = listRow.ProfileTypeID;
      connectionProfilesRowArray[0].ConnectionSettingsID = listRow.SettingsId;
      this.ConnectionProfilesAdapter.Update((DataTable) this.connectionProfilesDataTable);
      if (listRow.DeviceGroupID != this.GetGroupID(listRow.DeviceModelID))
        this.ChangeGroup(listRow.DeviceModelID, listRow.DeviceGroupID);
      if (listRow.EquipmentGroupID != this.GetGroupID(listRow.EquipmentModelID))
        this.ChangeGroup(listRow.EquipmentModelID, listRow.EquipmentGroupID);
      if (listRow.ProfileTypeGroupID != this.GetGroupID(listRow.ProfileTypeID))
        this.ChangeGroup(listRow.ProfileTypeID, listRow.ProfileTypeGroupID);
      this.LoadAllConnectionTables();
    }

    private bool ShowNeadAdditionalProfileErrorMessage(string partName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("The current " + partName + " is only used in current profile!");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("By changing this profile to a new " + partName + " the old");
      stringBuilder.AppendLine(partName + " will lose all connections to profiles");
      stringBuilder.AppendLine("and will be lost for future editing!");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Please create first a second profile before you");
      stringBuilder.AppendLine("create a new " + partName + " and connect it to the profile.");
      return this.ShowErrorMessage(stringBuilder.ToString());
    }

    private bool ShowErrorMessage(string message)
    {
      int num = (int) GMM_MessageBox.ShowMessage("Readout configuration", message, true);
      return false;
    }
  }
}
