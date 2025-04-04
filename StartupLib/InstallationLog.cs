// Decompiled with JetBrains decompiler
// Type: StartupLib.InstallationLog
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using PlugInLib;
using System;
using System.Data;
using System.Data.Common;
using ZR_ClassLibrary;

#nullable disable
namespace StartupLib
{
  public static class InstallationLog
  {
    private static bool beMonitored;
    private static string fingerPrintID;
    private static bool fingerPrintActive;
    private static UserInfo.PN_Source fingerPrintPNSource;
    private static string installationName;
    private static int installationID;
    private static int mainEquipmentID;

    public static bool BeMonitored => InstallationLog.beMonitored;

    public static string FingerPrintID => InstallationLog.fingerPrintID;

    public static bool FingerPrintActive => InstallationLog.fingerPrintActive;

    public static UserInfo.PN_Source FingerPrintPNSource => InstallationLog.fingerPrintPNSource;

    public static string InstallationName => InstallationLog.installationName;

    public static int InstallationID => InstallationLog.installationID;

    public static int MainEquipmentID => InstallationLog.mainEquipmentID;

    public static void StartupLog() => InstallationLog.RunLog();

    public static void RunLog()
    {
      try
      {
        InstallationLog.installationName = (string) null;
        InstallationLog.mainEquipmentID = 0;
        BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
        DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
        string machineName = Environment.MachineName;
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string str = Util.GMM_Version.ToString();
        string name = LicenseManager.CurrentLicense.Name;
        string customer = LicenseManager.CurrentLicense.Customer;
        DateTime licenseGenerationDate = LicenseManager.CurrentLicense.LicenseGenerationDate;
        int licenseGeneratorId = LicenseManager.CurrentLicense.LicenseGeneratorID;
        string selectSql1 = "SELECT * FROM Installations WHERE PcName ='" + machineName + "' AND InstallationPath ='" + baseDirectory + "'";
        DbDataAdapter dataAdapter1 = baseDbConnection.GetDataAdapter(selectSql1, newConnection, (DbTransaction) null, out DbCommandBuilder _);
        InstallationData.InstallationsDataTable installationsDataTable = new InstallationData.InstallationsDataTable();
        dataAdapter1.Fill((DataTable) installationsDataTable);
        if (installationsDataTable.Count > 1)
          throw new Exception("Installation log data base error. More then one installation with same propperties=" + Environment.NewLine + selectSql1);
        InstallationData.InstallationsRow row1;
        if (installationsDataTable.Count == 1)
        {
          row1 = installationsDataTable[0];
          InstallationLog.installationID = row1.InstallationId;
          if (!row1.IsInstallataionNameNull() && row1.InstallataionName.Length > 0)
          {
            string selectSql2 = "SELECT * FROM EQ_Equipment WHERE EquipmentID = EquipmentGroupID AND AccessName = '" + row1.InstallataionName + "'";
            DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection);
            InstallationData.EQ_EquipmentDataTable equipmentDataTable = new InstallationData.EQ_EquipmentDataTable();
            dataAdapter2.Fill((DataTable) equipmentDataTable);
            if (equipmentDataTable.Count == 1 && equipmentDataTable[0].IsOutOfOperationDateNull())
            {
              InstallationLog.installationName = equipmentDataTable[0].AccessName;
              InstallationLog.mainEquipmentID = equipmentDataTable[0].EquipmentID;
              InstallationLog.fingerPrintID = "0";
              InstallationLog.fingerPrintActive = false;
              InstallationLog.fingerPrintPNSource = UserInfo.PN_Source.None;
              try
              {
                string selectSql3 = "SELECT * from EQ_EquipmentValue,EQ_Equipment WHERE EQ_Equipment.EquipmentID = EQ_EquipmentValue.EquipmentID AND EQ_Equipment.EquipmentType = 'FingerPrintSensor' AND EQ_Equipment.EquipmentGroupID = " + equipmentDataTable[0].EquipmentID.ToString();
                DbDataAdapter dataAdapter3 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql3, newConnection);
                DataTable dataTable1 = new DataTable();
                dataAdapter3.Fill(dataTable1);
                if (dataTable1.Rows.Count >= 2)
                {
                  foreach (DataRow row2 in (InternalDataCollectionBase) dataTable1.Rows)
                  {
                    if (row2["ValueType"].ToString() == "SensorID")
                      InstallationLog.fingerPrintID = row2["Value"].ToString();
                    if (row2["ValueType"].ToString() == "SensorActive")
                      InstallationLog.fingerPrintActive = bool.Parse(row2["Value"].ToString());
                    if (row2["ValueType"].ToString() == "PNSource")
                      InstallationLog.fingerPrintPNSource = (UserInfo.PN_Source) Enum.Parse(typeof (UserInfo.PN_Source), row2["Value"].ToString());
                  }
                }
                int num = 0;
                string selectSql4 = "SELECT * from EQ_EquipmentValue,EQ_Equipment WHERE EQ_Equipment.EquipmentID = EQ_EquipmentValue.EquipmentID AND EQ_Equipment.EquipmentType = 'MonitoredInstallation' AND EQ_Equipment.EquipmentGroupID = " + equipmentDataTable[0].EquipmentID.ToString();
                DbDataAdapter dataAdapter4 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql4, newConnection);
                DataTable dataTable2 = new DataTable();
                dataAdapter4.Fill(dataTable2);
                if (dataTable2.Rows.Count >= 1)
                {
                  foreach (DataRow row3 in (InternalDataCollectionBase) dataTable2.Rows)
                  {
                    if (row3["ValueType"].ToString() == "InstallationID")
                      num = int.Parse(row3["Value"].ToString());
                  }
                }
                InstallationLog.beMonitored = InstallationLog.installationID == num;
              }
              catch
              {
              }
            }
          }
        }
        else
        {
          row1 = installationsDataTable.NewInstallationsRow();
          row1.InstallationId = baseDbConnection.GetNewId("Installations");
          row1.PcName = machineName;
          row1.InstallationPath = baseDirectory;
          installationsDataTable.AddInstallationsRow(row1);
          dataAdapter1.Update((DataTable) installationsDataTable);
        }
        string[] strArray1 = new string[5]
        {
          "SELECT * FROM InstallationChangeLog WHERE InstallationId =",
          row1.InstallationId.ToString(),
          " AND ChangeTime = (SELECT MAX(ChangeTime) FROM InstallationChangeLog WHERE InstallationId =",
          null,
          null
        };
        int installationId = row1.InstallationId;
        strArray1[3] = installationId.ToString();
        strArray1[4] = ")";
        string selectSql5 = string.Concat(strArray1);
        DbDataAdapter dataAdapter5 = baseDbConnection.GetDataAdapter(selectSql5, newConnection, (DbTransaction) null, out DbCommandBuilder _);
        InstallationData.InstallationChangeLogDataTable changeLogDataTable = new InstallationData.InstallationChangeLogDataTable();
        dataAdapter5.Fill((DataTable) changeLogDataTable);
        bool flag1 = true;
        InstallationData.InstallationChangeLogRow installationChangeLogRow1 = (InstallationData.InstallationChangeLogRow) null;
        if (changeLogDataTable.Count > 0)
        {
          installationChangeLogRow1 = changeLogDataTable[0];
          if (installationChangeLogRow1.IsMainEquipmentIDNull())
            installationChangeLogRow1.MainEquipmentID = 0;
          if (installationChangeLogRow1.SoftwareVersion == str && installationChangeLogRow1.LicenseName == name && installationChangeLogRow1.LicenseCustomer == customer && InstallationLog.IsDateTimeEqual(installationChangeLogRow1.LicenseGenerationTime, licenseGenerationDate) && installationChangeLogRow1.LicenseGeneratorID == licenseGeneratorId && installationChangeLogRow1.MainEquipmentID == InstallationLog.mainEquipmentID)
            flag1 = false;
        }
        DateTime now;
        if (flag1)
        {
          InstallationData.InstallationChangeLogRow row4 = changeLogDataTable.NewInstallationChangeLogRow();
          row4.InstallationId = row1.InstallationId;
          InstallationData.InstallationChangeLogRow installationChangeLogRow2 = row4;
          now = DateTime.Now;
          DateTime universalTime = now.ToUniversalTime();
          installationChangeLogRow2.ChangeTime = universalTime;
          row4.SoftwareVersion = str;
          row4.LicenseName = name;
          row4.LicenseCustomer = customer;
          row4.LicenseGenerationTime = licenseGenerationDate;
          row4.LicenseGeneratorID = licenseGeneratorId;
          row4.MainEquipmentID = InstallationLog.mainEquipmentID;
          row4.BasicState = installationChangeLogRow1 != null ? BasicTestbenchStates.NotTested.ToString() : BasicTestbenchStates.Undefined.ToString();
          changeLogDataTable.AddInstallationChangeLogRow(row4);
          dataAdapter5.Update((DataTable) changeLogDataTable);
        }
        if (UserManager.CurrentUser == null)
          return;
        string[] strArray2 = new string[5]
        {
          "SELECT * FROM InstallationUsers WHERE InstallationId =",
          null,
          null,
          null,
          null
        };
        installationId = row1.InstallationId;
        strArray2[1] = installationId.ToString();
        strArray2[2] = " AND ChangeTime = (SELECT MAX(ChangeTime) FROM InstallationUsers WHERE InstallationId =";
        installationId = row1.InstallationId;
        strArray2[3] = installationId.ToString();
        strArray2[4] = ")";
        string selectSql6 = string.Concat(strArray2);
        DbDataAdapter dataAdapter6 = baseDbConnection.GetDataAdapter(selectSql6, newConnection, (DbTransaction) null, out DbCommandBuilder _);
        InstallationData.InstallationUsersDataTable installationUsersDataTable = new InstallationData.InstallationUsersDataTable();
        dataAdapter6.Fill((DataTable) installationUsersDataTable);
        bool flag2 = true;
        if (installationUsersDataTable.Count > 0 && installationUsersDataTable[0].UserId == UserManager.CurrentUser.UserId)
          flag2 = false;
        if (flag2)
        {
          InstallationData.InstallationUsersRow row5 = installationUsersDataTable.NewInstallationUsersRow();
          row5.InstallationId = row1.InstallationId;
          InstallationData.InstallationUsersRow installationUsersRow = row5;
          now = DateTime.Now;
          DateTime universalTime = now.ToUniversalTime();
          installationUsersRow.ChangeTime = universalTime;
          row5.UserId = UserManager.CurrentUser.UserId;
          installationUsersDataTable.AddInstallationUsersRow(row5);
          dataAdapter6.Update((DataTable) installationUsersDataTable);
        }
      }
      catch
      {
      }
    }

    private static bool IsDateTimeEqual(DateTime dt1, DateTime dt2)
    {
      DateTime dateTime1 = dt1.AddSeconds(10.0);
      DateTime dateTime2 = dt1.AddSeconds(-10.0);
      return !(dt2 > dateTime1) && !(dt2 < dateTime2);
    }
  }
}
