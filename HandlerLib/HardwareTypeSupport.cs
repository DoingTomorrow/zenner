// Decompiled with JetBrains decompiler
// Type: HandlerLib.HardwareTypeSupport
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace HandlerLib
{
  public class HardwareTypeSupport
  {
    internal const string LatestFirmwareVersionReleaseName = "Latest version";
    internal const string ReleasedFirmwareVersionReleaseName = "Released for production";
    private HardwareTypeTables.HardwareAndFirmwareInfoDataTable HardwareAndFirmwareInfoTable;
    private List<CompatibilityInfo> CompatibilityList;
    private static List<CompatibilityInfo> CachedCompatibilityInfo = (List<CompatibilityInfo>) null;
    private static string dummyForLock = "dfl";

    public HardwareTypeSupport(string[] hardwareNames)
    {
      if (hardwareNames == null || hardwareNames.Length == 0)
        throw new ArgumentException("hardwareNames not defined");
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT");
          stringBuilder.Append(" HardwareType.HardwareTypeID as HardwareTypeID");
          stringBuilder.Append(",HardwareType.HardwareName as HardwareName");
          stringBuilder.Append(",HardwareType.HardwareVersion as HardwareVersion");
          stringBuilder.Append(",HardwareType.HardwareResource as HardwareResource");
          stringBuilder.Append(",HardwareType.Description as Description");
          stringBuilder.Append(",HardwareType.Testinfo as Testinfo");
          stringBuilder.Append(",HardwareType.HardwareOptions as HardwareOptions");
          stringBuilder.Append(",ProgFiles.MapID as MapID");
          stringBuilder.Append(",ProgFiles.Options as Options");
          stringBuilder.Append(",ProgFiles.SourceInfo as SourceInfo");
          stringBuilder.Append(",ProgFiles.FirmwareVersion as FirmwareVersion");
          stringBuilder.Append(",ProgFiles.ReleasedName as ReleasedName");
          stringBuilder.Append(",ProgFiles.CompatibleOverwriteGroups as CompatibleOverwriteGroups");
          stringBuilder.Append(",ProgFiles.ReleaseComments as ReleaseComments");
          stringBuilder.Append(" FROM HardwareType,ProgFiles");
          stringBuilder.Append(" WHERE (HardwareType.HardwareVersion > 0)");
          stringBuilder.Append(" AND (HardwareType.FirmwareVersion > 0)");
          bool flag = true;
          foreach (string hardwareName in hardwareNames)
          {
            if (flag)
            {
              stringBuilder.Append(" AND (HardwareType.HardwareName = '" + hardwareName + "'");
              flag = false;
            }
            else
              stringBuilder.Append(" OR HardwareType.HardwareName = '" + hardwareName + "'");
          }
          stringBuilder.Append(")");
          stringBuilder.Append(" AND (HardwareType.MapID = ProgFiles.HardwareTypeMapID)");
          this.HardwareAndFirmwareInfoTable = new HardwareTypeTables.HardwareAndFirmwareInfoDataTable();
          DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection).Fill((DataTable) this.HardwareAndFirmwareInfoTable);
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read data from table HardwareType");
        throw ex;
      }
    }

    public bool IsMapCompatibility(uint mapId, uint compatibleMapId, string overwriteGroup)
    {
      if ((int) mapId == (int) compatibleMapId)
        return true;
      CompatibilityInfo compatibilityInfos = this.GetMapCompatibilityInfos(mapId, compatibleMapId);
      return compatibilityInfos != null && (compatibilityInfos.IsFullCompatible || compatibilityInfos.CompatibleGroupShortcuts.Contains(overwriteGroup));
    }

    public CompatibilityInfo GetMapCompatibilityInfos(uint mapId, uint compatibleMapId)
    {
      lock (this)
      {
        if (this.CompatibilityList == null)
          this.CreateCompatibilityInfo();
        return this.CompatibilityList.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x =>
        {
          if ((long) mapId == (long) x.MapID && (long) compatibleMapId == (long) x.CompatibleToMapID)
            return true;
          return (long) mapId == (long) x.CompatibleToMapID && (long) compatibleMapId == (long) x.MapID;
        }));
      }
    }

    private void CreateCompatibilityInfo()
    {
      List<CompatibilityInfo> compatibilityInfoList = new List<CompatibilityInfo>();
      foreach (HardwareTypeTables.HardwareAndFirmwareInfoRow andFirmwareInfoRow in (TypedTableBase<HardwareTypeTables.HardwareAndFirmwareInfoRow>) this.HardwareAndFirmwareInfoTable)
      {
        HardwareTypeTables.HardwareAndFirmwareInfoRow theRow = andFirmwareInfoRow;
        if (!theRow.IsCompatibleOverwriteGroupsNull())
        {
          string compatibleOverwriteGroups = theRow.CompatibleOverwriteGroups;
          char[] separator = new char[1]{ ';' };
          foreach (string str in compatibleOverwriteGroups.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            string[] strArray = str.Split(new char[1]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
            int compatibleMapID;
            if (strArray.Length < 1 || !int.TryParse(strArray[0], out compatibleMapID))
              throw new Exception("CompatibleMapId setup error on MapID:" + theRow.MapID.ToString());
            if (theRow.MapID == compatibleMapID)
              throw new Exception("CompatibleMapId setup error. Illegal reference to own MapID:" + theRow.MapID.ToString());
            if (compatibilityInfoList.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => x.MapID == theRow.MapID && x.CompatibleToMapID == compatibleMapID)) == null)
            {
              CompatibilityInfo compatibilityInfo = new CompatibilityInfo();
              compatibilityInfo.Firmware = (uint) theRow.FirmwareVersion;
              compatibilityInfo.MapID = theRow.MapID;
              compatibilityInfo.CompatibleToMapID = compatibleMapID;
              HardwareTypeTables.HardwareAndFirmwareInfoRow[] andFirmwareInfoRowArray = (HardwareTypeTables.HardwareAndFirmwareInfoRow[]) this.HardwareAndFirmwareInfoTable.Select("MapID = " + compatibleMapID.ToString());
              if (andFirmwareInfoRowArray.Length == 0)
                throw new Exception("CompatibleMapId setup error. Illegal reference to not existing compatibleMapID:" + compatibleMapID.ToString());
              if (andFirmwareInfoRowArray.Length != 0)
                compatibilityInfo.CompatibleToFirmware = (uint) andFirmwareInfoRowArray[0].FirmwareVersion;
              if (strArray.Length == 1)
                compatibilityInfo.IsFullCompatible = true;
              else if (strArray.Length == 2 && strArray[1].Trim() == "full")
              {
                compatibilityInfo.IsFullCompatible = true;
              }
              else
              {
                compatibilityInfo.CompatibleGroupShortcuts = new List<string>();
                for (int index = 1; index < strArray.Length; ++index)
                  compatibilityInfo.CompatibleGroupShortcuts.Add(strArray[index]);
              }
              compatibilityInfoList.Add(compatibilityInfo);
            }
          }
        }
      }
      HardwareTypeSupport.AddMultiLevelCompatibilities(compatibilityInfoList);
      this.CompatibilityList = compatibilityInfoList;
    }

    public HardwareTypeTables.HardwareAndFirmwareInfoRow GetHardwareAndFirmwareInfo(
      int hardwareTypeID)
    {
      return this.HardwareAndFirmwareInfoTable.FirstOrDefault<HardwareTypeTables.HardwareAndFirmwareInfoRow>((System.Func<HardwareTypeTables.HardwareAndFirmwareInfoRow, bool>) (x => x.HardwareTypeID == hardwareTypeID));
    }

    public HardwareTypeTables.HardwareAndFirmwareInfoRow GetHardwareAndFirmwareInfo(
      int hardwareVersion,
      int firmwareVersion)
    {
      return this.HardwareAndFirmwareInfoTable.FirstOrDefault<HardwareTypeTables.HardwareAndFirmwareInfoRow>((System.Func<HardwareTypeTables.HardwareAndFirmwareInfoRow, bool>) (x => x.HardwareVersion == hardwareVersion && x.FirmwareVersion == firmwareVersion));
    }

    public HardwareTypeTables.HardwareAndFirmwareInfoRow GetExactOrNewestMapCompatibleHardwareAndFirmwareInfo(
      int hardwareVersion,
      int firmwareVersion)
    {
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo = this.GetHardwareAndFirmwareInfo(hardwareVersion, firmwareVersion);
      if (hardwareAndFirmwareInfo != null)
        return hardwareAndFirmwareInfo;
      int compatibleFirmwareVersion = (int) ((long) firmwareVersion & 4294905855L);
      EnumerableRowCollection<HardwareTypeTables.HardwareAndFirmwareInfoRow> source = this.HardwareAndFirmwareInfoTable.Where<HardwareTypeTables.HardwareAndFirmwareInfoRow>((System.Func<HardwareTypeTables.HardwareAndFirmwareInfoRow, bool>) (x => x.HardwareVersion == hardwareVersion && ((long) x.FirmwareVersion & 4294905855L) == (long) compatibleFirmwareVersion));
      if (source == null)
        return (HardwareTypeTables.HardwareAndFirmwareInfoRow) null;
      List<HardwareTypeTables.HardwareAndFirmwareInfoRow> list = source.ToList<HardwareTypeTables.HardwareAndFirmwareInfoRow>();
      if (list.Count == 0)
        return (HardwareTypeTables.HardwareAndFirmwareInfoRow) null;
      list.Sort(new Comparison<HardwareTypeTables.HardwareAndFirmwareInfoRow>(this.CompareByVersion));
      return list[0];
    }

    public HardwareTypeTables.HardwareAndFirmwareInfoRow GetNewestFullCompatibleHardwareAndFirmwareInfo(
      HardwareTypeTables.HardwareAndFirmwareInfoRow baseInfo,
      int hardwareMask)
    {
      HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo = (HardwareTypeTables.HardwareAndFirmwareInfoRow) null;
      foreach (HardwareTypeTables.HardwareAndFirmwareInfoRow testRow in (TypedTableBase<HardwareTypeTables.HardwareAndFirmwareInfoRow>) this.HardwareAndFirmwareInfoTable)
      {
        if (testRow.HardwareVersion == hardwareMask && this.IsHardwareCompatible(baseInfo, testRow) && (hardwareAndFirmwareInfo == null || hardwareAndFirmwareInfo.FirmwareVersion < testRow.FirmwareVersion))
          hardwareAndFirmwareInfo = testRow;
      }
      return hardwareAndFirmwareInfo;
    }

    public bool IsHardwareCompatible(
      HardwareTypeTables.HardwareAndFirmwareInfoRow baseRow,
      HardwareTypeTables.HardwareAndFirmwareInfoRow testRow)
    {
      if (baseRow == testRow)
        return true;
      string str = baseRow.HardwareTypeID.ToString();
      while (!testRow.IsHardwareOptionsNull())
      {
        string parameter = ParameterService.GetParameter(testRow.HardwareOptions, "CompatibleHwTypeId", '=', ';');
        if (parameter.Length == 0)
          return false;
        if (parameter == str)
          return true;
        testRow = (HardwareTypeTables.HardwareAndFirmwareInfoRow) null;
        foreach (HardwareTypeTables.HardwareAndFirmwareInfoRow andFirmwareInfoRow in (TypedTableBase<HardwareTypeTables.HardwareAndFirmwareInfoRow>) this.HardwareAndFirmwareInfoTable)
        {
          if (andFirmwareInfoRow.HardwareTypeID.ToString() == parameter)
          {
            testRow = andFirmwareInfoRow;
            break;
          }
        }
        if (testRow == null)
          return false;
      }
      return false;
    }

    private int CompareByVersion(
      HardwareTypeTables.HardwareAndFirmwareInfoRow a,
      HardwareTypeTables.HardwareAndFirmwareInfoRow b)
    {
      return b.FirmwareVersion.CompareTo(a.FirmwareVersion);
    }

    public static List<FirmwareReleaseInfo> GetAllReleasedFirmwareReleaseInfosForSapNumber(
      string SAP_Number)
    {
      List<FirmwareReleaseInfo> infosForSapNumber = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" SELECT");
        stringBuilder.Append(" ProgFiles.MapID as MapID,");
        stringBuilder.Append(" ProgFiles.ProgFileName as ProgFileName,");
        stringBuilder.Append(" ProgFiles.FirmwareVersion as FirmwareVersion,");
        stringBuilder.Append(" ProgFiles.ReleasedName as ReleasedName,");
        stringBuilder.Append(" ProgFiles.ReleaseComments as ReleaseComments");
        stringBuilder.Append(" FROM HardwareType, ProgFiles, MeterInfo");
        stringBuilder.Append(" WHERE MeterInfo.PPSArtikelNr = '" + SAP_Number + "'");
        stringBuilder.Append(" AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID");
        stringBuilder.Append(" AND HardwareType.MapID = ProgFiles.HardwareTypeMapID");
        stringBuilder.Append(" AND NOT (ProgFiles.ReleasedName IS Null)");
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        dataAdapter.Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = (string) null;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          infosForSapNumber.Add(new FirmwareReleaseInfo()
          {
            MapID = progFilesRow.MapID,
            ProgFilName = progFilesRow.ProgFileName,
            ReleaseText = progFilesRow.ReleasedName,
            FirmwareVersion = progFilesRow.FirmwareVersion,
            FirmwareVersionString = str1,
            ReleaseDescription = str2
          });
        }
      }
      infosForSapNumber.Sort();
      return infosForSapNumber;
    }

    public static List<FirmwareReleaseInfo> GetAllReleasedFirmwareReleaseInfoForHardwareType(
      int HardwareTypeID)
    {
      List<FirmwareReleaseInfo> infoForHardwareType = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" SELECT");
        stringBuilder.Append(" ProgFiles.MapID as MapID,");
        stringBuilder.Append(" ProgFiles.ProgFileName as ProgFileName,");
        stringBuilder.Append(" ProgFiles.FirmwareVersion as FirmwareVersion,");
        stringBuilder.Append(" ProgFiles.ReleasedName as ReleasedName,");
        stringBuilder.Append(" ProgFiles.ReleaseComments as ReleaseComments");
        stringBuilder.Append(" FROM HardwareType, ProgFiles");
        stringBuilder.Append(" WHERE HardwareType.HardwareTypeID = " + HardwareTypeID.ToString());
        stringBuilder.Append(" AND HardwareType.MapID = ProgFiles.HardwareTypeMapID");
        stringBuilder.Append(" AND NOT (ReleasedName IS Null)");
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        dataAdapter.Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = (string) null;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          infoForHardwareType.Add(new FirmwareReleaseInfo()
          {
            MapID = progFilesRow.MapID,
            ProgFilName = progFilesRow.ProgFileName,
            ReleaseText = progFilesRow.ReleasedName,
            FirmwareVersion = progFilesRow.FirmwareVersion,
            FirmwareVersionString = str1,
            ReleaseDescription = str2
          });
        }
      }
      infoForHardwareType.Sort();
      return infoForHardwareType;
    }

    public static List<FirmwareReleaseInfo> GetAllReleasedFirmwareReleaseInfosForProgFileName(
      string FileName,
      string hardwareResource = null)
    {
      List<FirmwareReleaseInfo> infosForProgFileName = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" SELECT MapID,ProgFileName, FirmwareVersion, ReleasedName, ReleaseComments, Options FROM ProgFiles");
        stringBuilder.Append(" WHERE ProgFileName like '%" + FileName + "%'");
        stringBuilder.Append(" AND NOT (ReleasedName IS Null)");
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        dataAdapter.Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          if (hardwareResource != null)
          {
            if (!progFilesRow.IsOptionsNull())
            {
              List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(progFilesRow.Options);
              int index = keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "HardwareResource"));
              if (index >= 0)
              {
                if (((IEnumerable<string>) keyValuePairList[index].Value.Split(';')).FirstOrDefault<string>((System.Func<string, bool>) (x => x == hardwareResource)) == null)
                  continue;
              }
            }
            else
              continue;
          }
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = (string) null;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          FirmwareReleaseInfo firmwareReleaseInfo = new FirmwareReleaseInfo();
          firmwareReleaseInfo.MapID = progFilesRow.MapID;
          if (!progFilesRow.IsProgFileNameNull())
            firmwareReleaseInfo.ProgFilName = progFilesRow.ProgFileName;
          firmwareReleaseInfo.ReleaseText = progFilesRow.ReleasedName;
          firmwareReleaseInfo.FirmwareVersion = progFilesRow.FirmwareVersion;
          firmwareReleaseInfo.FirmwareVersionString = str1;
          firmwareReleaseInfo.ReleaseDescription = str2;
          infosForProgFileName.Add(firmwareReleaseInfo);
        }
      }
      infosForProgFileName.Sort();
      return infosForProgFileName;
    }

    public static List<FirmwareReleaseInfo> GetAllReleasedFirmwareReleaseInfosForHardwareName(
      string hardwareName,
      string hardwareResource = null)
    {
      List<FirmwareReleaseInfo> infosForHardwareName = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" SELECT MapID,ProgFileName, FirmwareVersion, ReleasedName, ReleaseComments, Options FROM ProgFiles");
        stringBuilder.Append(" WHERE HardwareName = '" + hardwareName + "'");
        stringBuilder.Append(" AND NOT (ReleasedName IS Null)");
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        dataAdapter.Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          if (hardwareResource != null)
          {
            if (!progFilesRow.IsOptionsNull())
            {
              List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(progFilesRow.Options);
              int index = keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "HardwareResource"));
              if (index >= 0)
              {
                if (((IEnumerable<string>) keyValuePairList[index].Value.Split(';')).FirstOrDefault<string>((System.Func<string, bool>) (x => x == hardwareResource)) == null)
                  continue;
              }
            }
            else
              continue;
          }
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = (string) null;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          FirmwareReleaseInfo firmwareReleaseInfo = new FirmwareReleaseInfo();
          firmwareReleaseInfo.MapID = progFilesRow.MapID;
          if (!progFilesRow.IsProgFileNameNull())
            firmwareReleaseInfo.ProgFilName = progFilesRow.ProgFileName;
          firmwareReleaseInfo.ReleaseText = progFilesRow.ReleasedName;
          firmwareReleaseInfo.FirmwareVersion = progFilesRow.FirmwareVersion;
          firmwareReleaseInfo.FirmwareVersionString = str1;
          firmwareReleaseInfo.ReleaseDescription = str2;
          infosForHardwareName.Add(firmwareReleaseInfo);
        }
      }
      infosForHardwareName.Sort();
      return infosForHardwareName;
    }

    public static List<FirmwareReleaseInfo> GetAllReleasedFirmwareReleaseInfosForHardwareNameAndSampleHardware(
      string hardwareName,
      string hardwareResource = null,
      bool sampleHardwareAllowed = false)
    {
      List<FirmwareReleaseInfo> andSampleHardware = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" SELECT MapID,ProgFileName, FirmwareVersion, ReleasedName, ReleaseComments, Options FROM ProgFiles");
        stringBuilder.Append(" WHERE HardwareName = '" + hardwareName + "'");
        stringBuilder.Append(" AND NOT (ReleasedName IS Null)");
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        dataAdapter.Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          if (hardwareResource != null)
          {
            if (!progFilesRow.IsOptionsNull())
            {
              List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(progFilesRow.Options);
              int index1 = keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "HardwareResource"));
              if (index1 >= 0)
              {
                KeyValuePair<string, string> keyValuePair = keyValuePairList[index1];
                if (((IEnumerable<string>) keyValuePair.Value.Split(',')).FirstOrDefault<string>((System.Func<string, bool>) (x => x == hardwareResource)) == null)
                {
                  if (sampleHardwareAllowed)
                  {
                    int index2 = keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "SampleHardware"));
                    if (index2 >= 0)
                    {
                      keyValuePair = keyValuePairList[index2];
                      if (((IEnumerable<string>) keyValuePair.Value.Split(',')).FirstOrDefault<string>((System.Func<string, bool>) (x => x == hardwareResource)) == null)
                        continue;
                    }
                    else
                      continue;
                  }
                  else
                    continue;
                }
              }
              else
                continue;
            }
            else
              continue;
          }
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = (string) null;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          FirmwareReleaseInfo firmwareReleaseInfo = new FirmwareReleaseInfo();
          firmwareReleaseInfo.MapID = progFilesRow.MapID;
          if (!progFilesRow.IsProgFileNameNull())
            firmwareReleaseInfo.ProgFilName = progFilesRow.ProgFileName;
          firmwareReleaseInfo.ReleaseText = progFilesRow.ReleasedName;
          firmwareReleaseInfo.FirmwareVersion = progFilesRow.FirmwareVersion;
          firmwareReleaseInfo.FirmwareVersionString = str1;
          firmwareReleaseInfo.ReleaseDescription = str2;
          andSampleHardware.Add(firmwareReleaseInfo);
        }
      }
      andSampleHardware.Sort();
      return andSampleHardware;
    }

    public static List<FirmwareReleaseInfo> GetBslFirmwareReleaseInfosForFirmwareVersion(
      uint firmwareVersion)
    {
      List<FirmwareReleaseInfo> forFirmwareVersion = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
        if (baseDbConnection == null)
          throw new ArgumentNullException("Primary database");
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        string empty = string.Empty;
        string selectSql = "SELECT p.MapID, p.ProgFileName, p.FirmwareVersion, p.ReleasedName, p.ReleaseComments FROM ProgFiles p WHERE p.hardwaretypemapid = 105 AND h.FirmwareDependencies like '%" + firmwareVersion.ToString().Trim() + "%' ORDER BY p.FirmwareVersion desc";
        baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = (string) null;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          forFirmwareVersion.Add(new FirmwareReleaseInfo()
          {
            MapID = progFilesRow.MapID,
            ProgFilName = progFilesRow.ProgFileName,
            ReleaseText = progFilesRow.ReleasedName,
            FirmwareVersion = progFilesRow.FirmwareVersion,
            FirmwareVersionString = str1,
            ReleaseDescription = str2
          });
        }
      }
      forFirmwareVersion.Sort();
      return forFirmwareVersion;
    }

    public static List<FirmwareReleaseInfo> GetFirmwareReleaseInfosForHardwareTypeID(
      uint HardwareTypeID)
    {
      List<FirmwareReleaseInfo> forHardwareTypeId = new List<FirmwareReleaseInfo>();
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
        if (baseDbConnection == null)
          throw new ArgumentNullException("Primary database");
        HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
        string empty = string.Empty;
        string selectSql = "SELECT MapID, ProgFileName, FirmwareVersion, ReleasedName, ReleaseComments  FROM ProgFiles  WHERE HardwareTypeMapID = " + HardwareTypeID.ToString().Trim() + " ORDER BY FirmwareVersion desc";
        baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) progFilesDataTable);
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
        {
          string str1 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
          string str2 = string.Empty;
          if (!progFilesRow.IsReleaseCommentsNull())
            str2 = progFilesRow.ReleaseComments;
          forHardwareTypeId.Add(new FirmwareReleaseInfo()
          {
            MapID = progFilesRow.MapID,
            FirmwareVersion = progFilesRow.FirmwareVersion,
            FirmwareVersionString = str1,
            ProgFilName = progFilesRow.ProgFileName,
            ReleaseDescription = progFilesRow.IsReleaseCommentsNull() ? string.Empty : str2,
            ReleaseText = progFilesRow.IsReleasedNameNull() ? string.Empty : progFilesRow.ReleasedName
          });
        }
      }
      forHardwareTypeId.Sort();
      return forHardwareTypeId;
    }

    public static FirmwareReleaseInfo GetReleasedFirmwareInfoForSapNumber(string SAP_Number)
    {
      return HardwareTypeSupport.GetReleasedFirmwareReleaseInfo(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfosForSapNumber(SAP_Number));
    }

    public static FirmwareReleaseInfo GetReleasedFirmwareInfoForHardwareType(int HardwareTypeID)
    {
      return HardwareTypeSupport.GetReleasedFirmwareReleaseInfo(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfoForHardwareType(HardwareTypeID));
    }

    public static FirmwareReleaseInfo GetReleasedFirmwareInfoForHardwareName(string hardwareName)
    {
      return HardwareTypeSupport.GetReleasedFirmwareReleaseInfo(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfosForHardwareName(hardwareName));
    }

    private static FirmwareReleaseInfo GetReleasedFirmwareReleaseInfo(
      List<FirmwareReleaseInfo> allReleased)
    {
      FirmwareReleaseInfo firmwareReleaseInfo = (FirmwareReleaseInfo) null;
      if (allReleased != null)
        firmwareReleaseInfo = allReleased.Count <= 1 ? allReleased[0] : throw new Exception("More then one released firmware version found.");
      return firmwareReleaseInfo;
    }

    public static FirmwareData GetReleasedFirmwareDataForSapNumber(string SAP_Number)
    {
      return HardwareTypeSupport.GetReleasedFirmwareData(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfosForSapNumber(SAP_Number));
    }

    public static FirmwareData GetReleasedFirmwareDataForHardwareType(int HardwareTypeID)
    {
      return HardwareTypeSupport.GetReleasedFirmwareData(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfoForHardwareType(HardwareTypeID));
    }

    public static FirmwareData GetReleasedFirmwareDataForHardwareName(string hardwareName)
    {
      return HardwareTypeSupport.GetReleasedFirmwareData(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfosForHardwareName(hardwareName));
    }

    private static FirmwareData GetReleasedFirmwareData(List<FirmwareReleaseInfo> allReleased)
    {
      FirmwareData releasedFirmwareData = (FirmwareData) null;
      FirmwareReleaseInfo firmwareReleaseInfo = HardwareTypeSupport.GetReleasedFirmwareReleaseInfo(allReleased);
      if (firmwareReleaseInfo != null)
        releasedFirmwareData = HardwareTypeSupport.GetFirmwareData((uint) firmwareReleaseInfo.MapID);
      return releasedFirmwareData;
    }

    public static FirmwareData GetFirmwareData(uint MapID)
    {
      FirmwareData firmwareData = new FirmwareData();
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          string selectSql = "SELECT * FROM ProgFiles WHERE MapID = " + MapID.ToString();
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out DbCommandBuilder _);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter.Fill((DataTable) progFilesDataTable);
          if (progFilesDataTable.Count <= 0)
            return (FirmwareData) null;
          if (!progFilesDataTable[0].IsProgFileNameNull())
            firmwareData.ProgFileName = progFilesDataTable[0].ProgFileName;
          firmwareData.Options = !progFilesDataTable[0].IsOptionsNull() ? DbUtil.KeyValueStringListToKeyValuePairList(progFilesDataTable[0].Options) : new List<KeyValuePair<string, string>>();
          if (progFilesDataTable[0].IsSourceInfoNull())
            firmwareData.SourceInfo = progFilesDataTable[0].SourceInfo;
          if (!progFilesDataTable[0].IsHexTextNull())
          {
            firmwareData.ProgrammerFileAsString = progFilesDataTable[0].HexText;
            firmwareData.Options.Add(new KeyValuePair<string, string>(nameof (MapID), MapID.ToString()));
            int index = firmwareData.Options.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "Compression"));
            if (index >= 0)
            {
              if (firmwareData.Options[index].Value != "ZIP")
                throw new Exception("Unsupported compression formate: " + firmwareData.Options[index].Value);
              firmwareData.ProgrammerFileAsString = ZipUnzip.GetStringFromZipedString(firmwareData.ProgrammerFileAsString);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("GetFirmwareData exception", ex);
      }
      return firmwareData;
    }

    [Obsolete]
    public static bool GetFirmwareProgramInformationsFromProgFilesTable(
      long MapID,
      out string FirmwareFileString,
      out string ProgFileName,
      out string ProgFileOptions,
      out string Fehlerstring)
    {
      FirmwareFileString = string.Empty;
      ProgFileName = string.Empty;
      ProgFileOptions = string.Empty;
      Fehlerstring = string.Empty;
      try
      {
        FirmwareData firmwareData = HardwareTypeSupport.GetFirmwareData((uint) MapID);
        FirmwareFileString = firmwareData.ProgrammerFileAsString;
        ProgFileName = firmwareData.ProgFileName;
        ProgFileOptions = DbUtil.KeyValuePairListToKeyValueStringList(firmwareData.Options);
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public static uint GetMapIDForHardwareTypeFromHardwareTypeTable(uint hardwareTypeID)
    {
      string empty = string.Empty;
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      if (baseDbConnection == null)
        throw new ArgumentNullException("Primary database");
      Schema.HardwareTypeDataTable hardwareTypeDataTable = new Schema.HardwareTypeDataTable();
      using (DbConnection newConnection = baseDbConnection.GetNewConnection())
      {
        string selectSql = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + hardwareTypeID.ToString();
        baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) hardwareTypeDataTable);
      }
      return hardwareTypeDataTable.Rows.Count == 0 ? 0U : (uint) hardwareTypeDataTable[0].MapID;
    }

    [Obsolete("Use FirmwareData.WriteToFile")]
    public static bool WriteFirmwareProgramIntoFile(
      string Dateiname,
      string FirmwareFileString,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      try
      {
        FileStream fileStream = new FileStream(Dateiname, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream, Encoding.ASCII);
        streamWriter.Write(FirmwareFileString);
        streamWriter.Close();
        fileStream.Close();
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        return false;
      }
    }

    public static FirmwareData GetNewestBSLFromProgFilesTable(
      uint actualFWVersion,
      uint newFWVersion)
    {
      FirmwareData fromProgFilesTable = new FirmwareData();
      int MapID = 0;
      string empty = string.Empty;
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      if (baseDbConnection == null)
        throw new ArgumentNullException("Primary database");
      HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
      using (DbConnection newConnection = baseDbConnection.GetNewConnection())
      {
        string selectSql = "SELECT * FROM Progfiles WHERE HardwareName like 'BootLoader' AND FirmwareDependencies like '%" + actualFWVersion.ToString("x8") + "%' " + (newFWVersion == 1U ? "" : "AND FirmwareDependencies like '%" + newFWVersion.ToString("x8") + "%' ") + "ORDER BY FirmwareVersion desc";
        baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) progFilesDataTable);
      }
      if (progFilesDataTable.Rows.Count == 0)
      {
        fromProgFilesTable.SourceInfo = "No Bootloader found for this firmware version!";
        return fromProgFilesTable;
      }
      ushort num = (ushort) (actualFWVersion & 4095U);
      if (progFilesDataTable.Count > 1)
        fromProgFilesTable.SourceInfo = "More than one Bootloader found for this firmware version!\nCheck your database, this should not happen !!!";
      if (!progFilesDataTable[0].IsHardwareTypeMapIDNull())
        MapID = progFilesDataTable[0].MapID;
      if (MapID <= 0)
      {
        fromProgFilesTable.SourceInfo = "No Bootloader found for this firmware version!";
        return fromProgFilesTable;
      }
      FirmwareData firmwareData = HardwareTypeSupport.GetFirmwareData((uint) MapID);
      firmwareData.Options.Add(new KeyValuePair<string, string>("MapID", MapID.ToString()));
      return firmwareData;
    }

    private static void FindCompatible(
      List<CompatibilityInfo> fullCompatibleList,
      List<CompatibilityInfo> mapCompatibleList,
      int MapID)
    {
      foreach (CompatibilityInfo compatibilityInfo in fullCompatibleList.FindAll((Predicate<CompatibilityInfo>) (x => x.CompatibleToMapID == MapID)))
      {
        CompatibilityInfo ci = compatibilityInfo;
        if (mapCompatibleList.FindIndex((Predicate<CompatibilityInfo>) (x => x.MapID == ci.MapID && x.CompatibleToMapID == ci.CompatibleToMapID)) < 0)
        {
          mapCompatibleList.Add(ci);
          HardwareTypeSupport.FindCompatible(fullCompatibleList, mapCompatibleList, ci.MapID);
        }
      }
    }

    public static CompatibilityInfo GetCompatibilityInfos(
      uint firmwareVersion,
      uint compatibleFirmwareVersion)
    {
      List<CompatibilityInfo> compatibilityInfos = HardwareTypeSupport.GetAllCompatibilityInfos();
      return compatibleFirmwareVersion >= firmwareVersion ? compatibilityInfos.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => (int) compatibleFirmwareVersion == (int) x.Firmware && (int) firmwareVersion == (int) x.CompatibleToFirmware)) ?? compatibilityInfos.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => (int) firmwareVersion == (int) x.Firmware && (int) compatibleFirmwareVersion == (int) x.CompatibleToFirmware)) : compatibilityInfos.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => (int) firmwareVersion == (int) x.Firmware && (int) compatibleFirmwareVersion == (int) x.CompatibleToFirmware)) ?? compatibilityInfos.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => (int) compatibleFirmwareVersion == (int) x.Firmware && (int) firmwareVersion == (int) x.CompatibleToFirmware));
    }

    public static void ClearCache()
    {
      lock (HardwareTypeSupport.dummyForLock)
        HardwareTypeSupport.CachedCompatibilityInfo = (List<CompatibilityInfo>) null;
    }

    public static List<CompatibilityInfo> GetAllCompatibilityInfos()
    {
      lock (HardwareTypeSupport.dummyForLock)
      {
        if (HardwareTypeSupport.CachedCompatibilityInfo != null)
          return HardwareTypeSupport.CachedCompatibilityInfo;
        List<CompatibilityInfo> compatibilityList = new List<CompatibilityInfo>();
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT HardwareTypeID, MapID, FirmwareVersion, HardwareName ");
          stringBuilder.Append(" FROM HardwareType");
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable = new HardwareTypeTables.HardwareTypeDataTable();
          dataAdapter1.Fill((DataTable) hardwareTypeDataTable);
          stringBuilder.Clear();
          stringBuilder.Append(" SELECT MapID, Options, FirmwareVersion, CompatibleOverwriteGroups, HardwareTypeMapID, HardwareName FROM ProgFiles");
          stringBuilder.Append(" WHERE NOT (CompatibleOverwriteGroups IS NULL or HardwareName IS NULL)");
          DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter2.Fill((DataTable) progFilesDataTable);
          foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
          {
            string compatibleOverwriteGroups = progFilesRow.CompatibleOverwriteGroups;
            char[] separator1 = new char[1]{ ';' };
            foreach (string str1 in compatibleOverwriteGroups.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
            {
              char[] separator2 = new char[1]{ ',' };
              string[] strArray = str1.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
              int result;
              if (strArray.Length < 1 || !int.TryParse(strArray[0], out result))
                throw new Exception("CompatibleMapId setup error on MapID:" + progFilesRow.MapID.ToString());
              string str2 = string.Empty;
              if (!progFilesRow.IsNull("HardwareName").Equals((object) DBNull.Value))
                str2 = " and HardwareName like '" + progFilesRow.HardwareName + "%' ";
              if (((HardwareTypeTables.HardwareTypeRow[]) hardwareTypeDataTable.Select("MapID = " + progFilesRow.HardwareTypeMapID.ToString() + str2)).Length != 0)
              {
                HardwareTypeTables.ProgFilesRow[] progFilesRowArray = (HardwareTypeTables.ProgFilesRow[]) null;
                HardwareTypeTables.HardwareTypeRow[] hardwareTypeRowArray = (HardwareTypeTables.HardwareTypeRow[]) hardwareTypeDataTable.Select("MapID = " + result.ToString() + str2);
                if (hardwareTypeRowArray.Length == 0)
                {
                  progFilesRowArray = (HardwareTypeTables.ProgFilesRow[]) progFilesDataTable.Select("MapID = " + result.ToString() + str2);
                  if (progFilesRowArray.Length == 0)
                    continue;
                }
                CompatibilityInfo compatibilityInfo = new CompatibilityInfo();
                compatibilityInfo.Firmware = (uint) progFilesRow.FirmwareVersion;
                compatibilityInfo.MapID = progFilesRow.MapID;
                compatibilityInfo.CompatibleToMapID = result;
                if (hardwareTypeRowArray.Length != 0)
                  compatibilityInfo.CompatibleToFirmware = (uint) hardwareTypeRowArray[0].FirmwareVersion;
                if (progFilesRowArray != null && progFilesRowArray.Length != 0)
                  compatibilityInfo.CompatibleToFirmware = (uint) progFilesRowArray[0].FirmwareVersion;
                if (strArray.Length == 1)
                  compatibilityInfo.IsFullCompatible = true;
                else if (strArray.Length == 2 && strArray[1].Trim().ToLower() == "full")
                {
                  compatibilityInfo.IsFullCompatible = true;
                }
                else
                {
                  compatibilityInfo.CompatibleGroupShortcuts = new List<string>();
                  for (int index = 1; index < strArray.Length; ++index)
                    compatibilityInfo.CompatibleGroupShortcuts.Add(strArray[index]);
                }
                compatibilityList.Add(compatibilityInfo);
              }
            }
          }
        }
        HardwareTypeSupport.AddMultiLevelCompatibilities(compatibilityList);
        HardwareTypeSupport.CachedCompatibilityInfo = compatibilityList;
        return compatibilityList;
      }
    }

    private static void AddMultiLevelCompatibilities(List<CompatibilityInfo> compatibilityList)
    {
      List<CompatibilityInfo> compatibilityInfoList1 = new List<CompatibilityInfo>();
      List<CompatibilityInfo> compatibilityInfoList2 = new List<CompatibilityInfo>();
      List<uint> uintList = new List<uint>();
      bool flag;
      do
      {
        uintList.Clear();
        flag = false;
        for (int index = 0; index < compatibilityList.Count; ++index)
        {
          CompatibilityInfo testInfo = compatibilityList[index];
          if (!uintList.Contains(testInfo.Firmware))
          {
            uintList.Add(testInfo.Firmware);
            List<CompatibilityInfo> all = compatibilityList.FindAll((Predicate<CompatibilityInfo>) (x => (int) x.Firmware == (int) testInfo.Firmware));
            compatibilityInfoList1.Clear();
            foreach (CompatibilityInfo compatibilityInfo1 in all)
            {
              CompatibilityInfo knownCompatibility = compatibilityInfo1;
              foreach (CompatibilityInfo compatibilityInfo2 in compatibilityList.FindAll((Predicate<CompatibilityInfo>) (x => (int) x.Firmware == (int) knownCompatibility.CompatibleToFirmware)))
              {
                CompatibilityInfo subObject = compatibilityInfo2;
                if ((int) subObject.CompatibleToFirmware != (int) testInfo.Firmware)
                {
                  CompatibilityInfo compatibilityInfo3 = all.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => (int) x.CompatibleToFirmware == (int) subObject.CompatibleToFirmware)) ?? compatibilityInfoList1.FirstOrDefault<CompatibilityInfo>((System.Func<CompatibilityInfo, bool>) (x => (int) x.CompatibleToFirmware == (int) subObject.CompatibleToFirmware));
                  if (compatibilityInfo3 == null)
                  {
                    CompatibilityInfo compatibilityInfo4 = new CompatibilityInfo();
                    compatibilityInfo4.MapID = knownCompatibility.MapID;
                    compatibilityInfo4.Firmware = knownCompatibility.Firmware;
                    compatibilityInfo4.CompatibleToMapID = subObject.CompatibleToMapID;
                    compatibilityInfo4.CompatibleToFirmware = subObject.CompatibleToFirmware;
                    if (knownCompatibility.IsFullCompatible && subObject.IsFullCompatible)
                      compatibilityInfo4.IsFullCompatible = true;
                    else if (knownCompatibility.IsFullCompatible)
                      compatibilityInfo4.CompatibleGroupShortcuts = subObject.CompatibleGroupShortcuts;
                    else if (subObject.IsFullCompatible)
                    {
                      compatibilityInfo4.CompatibleGroupShortcuts = knownCompatibility.CompatibleGroupShortcuts;
                    }
                    else
                    {
                      foreach (string compatibleGroupShortcut in knownCompatibility.CompatibleGroupShortcuts)
                      {
                        if (subObject.CompatibleGroupShortcuts.Contains(compatibleGroupShortcut))
                        {
                          if (compatibilityInfo4.CompatibleGroupShortcuts == null)
                            compatibilityInfo4.CompatibleGroupShortcuts = new List<string>();
                          compatibilityInfo4.CompatibleGroupShortcuts.Add(compatibleGroupShortcut);
                        }
                      }
                    }
                    if (compatibilityInfo4.IsFullCompatible || compatibilityInfo4.CompatibleGroupShortcuts != null)
                      compatibilityInfoList1.Add(compatibilityInfo4);
                  }
                  else if (!compatibilityInfo3.IsFullCompatible)
                  {
                    if (knownCompatibility.IsFullCompatible && subObject.IsFullCompatible)
                    {
                      compatibilityInfo3.IsFullCompatible = true;
                      flag = true;
                    }
                    else if (knownCompatibility.IsFullCompatible)
                    {
                      foreach (string compatibleGroupShortcut in subObject.CompatibleGroupShortcuts)
                      {
                        if (!compatibilityInfo3.CompatibleGroupShortcuts.Contains(compatibleGroupShortcut))
                        {
                          compatibilityInfo3.CompatibleGroupShortcuts.Add(compatibleGroupShortcut);
                          flag = true;
                        }
                      }
                    }
                    else if (subObject.IsFullCompatible)
                    {
                      foreach (string compatibleGroupShortcut in knownCompatibility.CompatibleGroupShortcuts)
                      {
                        if (!compatibilityInfo3.CompatibleGroupShortcuts.Contains(compatibleGroupShortcut))
                        {
                          compatibilityInfo3.CompatibleGroupShortcuts.Add(compatibleGroupShortcut);
                          flag = true;
                        }
                      }
                    }
                    else
                    {
                      foreach (string compatibleGroupShortcut in knownCompatibility.CompatibleGroupShortcuts)
                      {
                        if (subObject.CompatibleGroupShortcuts.Contains(compatibleGroupShortcut) && !compatibilityInfo3.CompatibleGroupShortcuts.Contains(compatibleGroupShortcut))
                        {
                          compatibilityInfo3.CompatibleGroupShortcuts.Add(compatibleGroupShortcut);
                          flag = true;
                        }
                      }
                    }
                  }
                }
              }
            }
            if (compatibilityInfoList1.Count > 0)
            {
              compatibilityList.AddRange((IEnumerable<CompatibilityInfo>) compatibilityInfoList1);
              flag = true;
            }
          }
        }
      }
      while (flag);
      compatibilityList.Sort();
    }

    public enum HardwareResources
    {
      IUWB,
      IUWR,
      IUWRR,
    }
  }
}
