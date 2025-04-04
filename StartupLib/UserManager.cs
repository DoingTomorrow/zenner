// Decompiled with JetBrains decompiler
// Type: StartupLib.UserManager
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using NLog;
using PlugInLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace StartupLib
{
  public static class UserManager
  {
    public static string Role_Developer = "Role\\Developer";
    public static string Role_Developer_Init = "|False|Enable all rights for a developer";
    public static string Right_ReadOnly = "Right\\ReadOnly";
    public static string Right_ReadOnly_Init = "|False|Write access to devices is blocked";
    public static string Right_DeleteMetersFromDatabase = "Right\\DeleteMetersFromDatabase";
    public static string Right_DeleteMetersFromDatabase_Init = "|False|The serial number reference of this meter is deleted.";
    private static Logger logger = LogManager.GetLogger(nameof (UserManager));
    public const string UserRoleString = "* UserRole * ";
    public static SortedList<string, UserInfo> AllUsersUpper;
    private static SortedList<string, RightInfo> EnabledLicenseRights;
    private static HashSet<string> EnabledRuntimePermissionsWithoutPath;
    private static HashSet<string> EnabledRuntimePermissionsWithPath;
    private static bool RuntimePermissionDeveloper;
    private static SortedList<int, UserInfo> UserInfoCache = new SortedList<int, UserInfo>();

    public static UserInfo CurrentUser { get; private set; }

    public static bool DeveloperChosenOnStart { get; set; }

    public static FullRightInfoList rightInfoList { get; private set; }

    public static UserInfo GetUser(int UserId)
    {
      int index = UserManager.UserInfoCache.IndexOfKey(UserId);
      if (index >= 0)
        return UserManager.UserInfoCache.Values[index];
      UserInfo userInfo1 = new UserInfo();
      UserInfo userInfo2 = (UserInfo) null;
      ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserId=" + UserId.ToString(), DbBasis.PrimaryDB.GetDbConnection());
      Schema.SoftwareUsersDataTable softwareUsersDataTable1 = new Schema.SoftwareUsersDataTable();
      zrDataAdapter1.Fill((DataTable) softwareUsersDataTable1);
      Schema.SoftwareUsersRow softwareUsersRow1 = softwareUsersDataTable1[0];
      userInfo1.UserId = softwareUsersRow1.UserId;
      userInfo1.Name = softwareUsersRow1.Name;
      userInfo1.RoleUserId = !softwareUsersRow1.IsUserRoleNull() ? softwareUsersRow1.UserRole : 0;
      userInfo1.Password = !softwareUsersRow1.IsPasswordNull() ? softwareUsersRow1.Password : "??";
      userInfo1.PersonalNumber = softwareUsersRow1.PersonalNumber;
      userInfo1.ControlKey = softwareUsersRow1.ControlKey;
      userInfo1.OnlyFinterprintLogin = !softwareUsersRow1.IsOnlyFinterprintLoginNull() && softwareUsersRow1.OnlyFinterprintLogin;
      userInfo1.PhoneNumber = !softwareUsersRow1.IsPhoneNumberNull() ? softwareUsersRow1.PhoneNumber : "";
      userInfo1.EmailAddress = !softwareUsersRow1.IsEmailAddressNull() ? softwareUsersRow1.EmailAddress : "";
      userInfo1.UserExtendedInfo = !softwareUsersRow1.IsUserExtendedInfoNull() ? softwareUsersRow1.UserExtendedInfo : "";
      userInfo1.PNSource = !softwareUsersRow1.IsPNSourceNull() ? (UserInfo.PN_Source) Enum.Parse(typeof (UserInfo.PN_Source), softwareUsersRow1.PNSource) : UserInfo.PN_Source.None;
      int num = userInfo1.UserId;
      if (userInfo1.RoleUserId > 0)
      {
        num = userInfo1.RoleUserId;
        userInfo2 = new UserInfo();
        userInfo2.UserId = userInfo1.RoleUserId;
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserId=" + userInfo2.UserId.ToString(), DbBasis.PrimaryDB.GetDbConnection());
        Schema.SoftwareUsersDataTable softwareUsersDataTable2 = new Schema.SoftwareUsersDataTable();
        zrDataAdapter2.Fill((DataTable) softwareUsersDataTable2);
        Schema.SoftwareUsersRow softwareUsersRow2 = softwareUsersDataTable2[0];
        userInfo2.UserId = softwareUsersRow2.UserId;
        userInfo2.Name = softwareUsersRow2.Name;
        userInfo2.RoleUserId = softwareUsersRow2.UserRole;
        userInfo2.Password = softwareUsersRow2.Password;
        userInfo2.PersonalNumber = softwareUsersRow2.PersonalNumber;
        userInfo2.ControlKey = softwareUsersRow2.ControlKey;
      }
      ZRDataAdapter zrDataAdapter3 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM UserPermissions WHERE UserId=" + num.ToString(), DbBasis.PrimaryDB.GetDbConnection());
      Schema.UserPermissionsDataTable permissionsDataTable = new Schema.UserPermissionsDataTable();
      zrDataAdapter3.Fill((DataTable) permissionsDataTable);
      userInfo1.Permissions = new List<PermissionInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) permissionsDataTable.Rows)
        userInfo1.Permissions.Add(new PermissionInfo()
        {
          PermissionName = row["PermissionName"].ToString(),
          PermissionValue = Convert.ToBoolean(row["PermissionValue"])
        });
      if (userInfo1.RoleUserId > 0)
      {
        userInfo2.Permissions = userInfo1.Permissions;
        if (userInfo2.ControlKey != UserManager.GenerateControlKey(userInfo2))
        {
          UserManager.logger.Fatal("DATABASE HAS ILLEGAL MODIFICATIONS! UserInfo: " + Environment.NewLine + userInfo2?.ToString());
          throw new AccessDeniedException("Database has illegal modifications! Please check user " + userInfo2?.ToString());
        }
      }
      if (userInfo1.Permissions.Count > 0 && userInfo1.ControlKey != UserManager.GenerateControlKey(userInfo1))
      {
        UserManager.logger.Fatal("DATABASE HAS ILLEGAL MODIFICATIONS! UserInfo: " + Environment.NewLine + userInfo1?.ToString());
        throw new AccessDeniedException("Database has illegal modifications! Please check user " + userInfo1?.ToString());
      }
      userInfo1.Permissions.RemoveAll((Predicate<PermissionInfo>) (item => item.PermissionName.Length < 2));
      UserManager.UserInfoCache.Add(UserId, userInfo1);
      return userInfo1;
    }

    internal static List<UserInfo> LoadAndGetUsers(string selectExtention)
    {
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      DbConnection newConnection = baseDbConnection.GetNewConnection();
      bool flag1 = UserManager.CheckPermission("Developer");
      bool flag2 = flag1 || UserManager.CheckPermission("Administrator");
      UserManager.AllUsersUpper = new SortedList<string, UserInfo>();
      SortedList<string, UserInfo> sortedList = new SortedList<string, UserInfo>();
      string selectSql1 = "SELECT * FROM SoftwareUsers";
      if (!string.IsNullOrEmpty(selectExtention))
        selectSql1 = selectSql1 + " " + selectExtention;
      DbDataAdapter dataAdapter1 = baseDbConnection.GetDataAdapter(selectSql1, newConnection);
      Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
      dataAdapter1.Fill((DataTable) softwareUsersDataTable);
      string selectSql2 = "SELECT * FROM UserPermissions WHERE (PermissionName = 'Role\\Developer') OR (PermissionName = 'Role\\Administrator') OR (PermissionName = 'Developer') OR (PermissionName = 'Administrator')";
      DbDataAdapter dataAdapter2 = baseDbConnection.GetDataAdapter(selectSql2, newConnection);
      Schema.UserPermissionsDataTable permissionsDataTable = new Schema.UserPermissionsDataTable();
      dataAdapter2.Fill((DataTable) permissionsDataTable);
      HashSet<int> intSet1 = new HashSet<int>();
      HashSet<int> intSet2 = new HashSet<int>();
      foreach (Schema.UserPermissionsRow userPermissionsRow in (TypedTableBase<Schema.UserPermissionsRow>) permissionsDataTable)
      {
        if (userPermissionsRow.PermissionValue)
        {
          if (userPermissionsRow.PermissionName.EndsWith("Developer") && !intSet1.Contains(userPermissionsRow.UserId))
            intSet1.Add(userPermissionsRow.UserId);
          if (userPermissionsRow.PermissionName.EndsWith("Administrator") && !intSet2.Contains(userPermissionsRow.UserId))
            intSet2.Add(userPermissionsRow.UserId);
        }
      }
      foreach (Schema.SoftwareUsersRow softwareUsersRow in (TypedTableBase<Schema.SoftwareUsersRow>) softwareUsersDataTable)
      {
        int num = softwareUsersRow.UserId;
        if (!softwareUsersRow.IsUserRoleNull() && softwareUsersRow.UserRole > 0)
          num = softwareUsersRow.UserRole;
        if ((flag1 || !intSet1.Contains(num)) && (flag2 || !intSet2.Contains(num)))
        {
          UserInfo userInfo = new UserInfo();
          userInfo.UserId = softwareUsersRow.UserId;
          userInfo.Name = softwareUsersRow.Name;
          userInfo.PersonalNumber = softwareUsersRow.PersonalNumber;
          if (!softwareUsersRow.IsUserRoleNull())
            userInfo.RoleUserId = softwareUsersRow.UserRole;
          if (!softwareUsersRow.IsPasswordNull())
            userInfo.Password = softwareUsersRow.Password;
          if (!softwareUsersRow.IsOnlyFinterprintLoginNull())
            userInfo.OnlyFinterprintLogin = softwareUsersRow.OnlyFinterprintLogin;
          if (!softwareUsersRow.IsPhoneNumberNull())
            userInfo.PhoneNumber = softwareUsersRow.PhoneNumber;
          if (!softwareUsersRow.IsEmailAddressNull())
            userInfo.EmailAddress = softwareUsersRow.EmailAddress;
          sortedList.Add(userInfo.Name, userInfo);
        }
      }
      List<UserInfo> users = new List<UserInfo>();
      foreach (UserInfo userInfo in (IEnumerable<UserInfo>) sortedList.Values)
      {
        UserManager.AllUsersUpper.Add(userInfo.Name.ToUpper(), userInfo);
        if (userInfo.Password == null || userInfo.Password != "Deleted")
          users.Add(userInfo);
      }
      return users;
    }

    internal static bool AddUser(UserInfo userInfo, int ClassicGmmUserId)
    {
      IDbConnection Connection = (IDbConnection) null;
      IDbTransaction Transaction = (IDbTransaction) null;
      try
      {
        Connection = DbBasis.PrimaryDB.GetDbConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE Name='" + userInfo.Name + "'", Connection);
        Schema.SoftwareUsersDataTable MyDataTable1 = new Schema.SoftwareUsersDataTable();
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        Schema.SoftwareUsersRow row1;
        if (MyDataTable1.Count > 0)
        {
          if (MyDataTable1[0].IsPasswordNull())
            row1 = MyDataTable1[0];
          else if (ClassicGmmUserId > 20 && ClassicGmmUserId != MyDataTable1[0].UserId)
          {
            row1 = MyDataTable1.NewSoftwareUsersRow();
            userInfo.Name = MyDataTable1[0].Name + " fc";
          }
          else
          {
            userInfo.ErrorMessageText = "User name exists!";
            return false;
          }
        }
        else
          row1 = MyDataTable1.NewSoftwareUsersRow();
        userInfo.UserId = ClassicGmmUserId >= 0 ? ClassicGmmUserId : (int) Datenbankverbindung.MainDBAccess.GetNewId("SoftwareUsers", "UserId");
        row1.UserId = userInfo.UserId;
        row1.Name = userInfo.Name;
        row1.PersonalNumber = userInfo.PersonalNumber;
        userInfo.Password = UserManager.ApplyHash(userInfo.Password);
        row1.Password = userInfo.Password;
        row1.UserRole = userInfo.RoleUserId;
        row1.ControlKey = UserManager.GenerateControlKey(userInfo);
        userInfo.ControlKey = row1.ControlKey;
        row1.OnlyFinterprintLogin = userInfo.OnlyFinterprintLogin;
        row1.PhoneNumber = userInfo.PhoneNumber;
        row1.EmailAddress = userInfo.EmailAddress;
        if (ClassicGmmUserId != -1 && userInfo.PNSource == UserInfo.PN_Source.None && !userInfo.Name.StartsWith("* UserRole * ") && DbBasis.PrimaryDB.BaseDbConnection.ConnectionInfo.DbType == MeterDbTypes.MSSQL)
        {
          userInfo.ErrorMessageText = "The PNSource must be set to actual value.";
          return false;
        }
        if (!userInfo.Name.StartsWith("* UserRole * "))
        {
          ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE PersonalNumber=" + userInfo.PersonalNumber.ToString() + " AND PNSource = '" + userInfo.PNSource.ToString() + "'", Connection);
          Schema.SoftwareUsersDataTable MyDataTable2 = new Schema.SoftwareUsersDataTable();
          try
          {
            zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
          }
          catch
          {
          }
          if (MyDataTable2.Rows.Count == 0 || MyDataTable2.Rows.Count == 1 && MyDataTable2.Rows[0]["UserId"].ToString() == userInfo.UserId.ToString())
          {
            row1.PNSource = userInfo.PNSource.ToString();
          }
          else
          {
            userInfo.ErrorMessageText = "The personal number is repeated in the database.";
            return false;
          }
        }
        else
          row1.PNSource = userInfo.PNSource.ToString();
        row1.UserExtendedInfo = userInfo.UserExtendedInfo;
        if (MyDataTable1.Count == 0)
          MyDataTable1.AddSoftwareUsersRow(row1);
        zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction);
        if (userInfo.Permissions != null && userInfo.Permissions.Count > 0)
        {
          Datenbankverbindung.MainDBAccess.setIDCachSize((long) userInfo.Permissions.Count);
          ZRDataAdapter zrDataAdapter3 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM UserPermissions WHERE UserId=0", Connection);
          Schema.UserPermissionsDataTable MyDataTable3 = new Schema.UserPermissionsDataTable();
          zrDataAdapter3.Fill((DataTable) MyDataTable3, Transaction);
          foreach (PermissionInfo permission in userInfo.Permissions)
          {
            permission.PermissionId = (int) Datenbankverbindung.MainDBAccess.GetNewId("UserPermissions", "PermissionId");
            Schema.UserPermissionsRow row2 = MyDataTable3.NewUserPermissionsRow();
            row2.PermissionId = permission.PermissionId;
            row2.UserId = userInfo.UserId;
            row2.PermissionName = permission.PermissionName;
            row2.PermissionValue = permission.PermissionValue;
            MyDataTable3.AddUserPermissionsRow(row2);
          }
          zrDataAdapter3.Update((DataTable) MyDataTable3, Transaction);
        }
        Transaction.Commit();
      }
      catch (Exception ex)
      {
        Transaction?.Rollback();
        userInfo.ErrorMessageText = "Exception: " + ex.ToString();
        return false;
      }
      finally
      {
        Connection?.Close();
        Datenbankverbindung.MainDBAccess.setIDCachSize(1L);
      }
      return true;
    }

    internal static bool EditUser(UserInfo userInfo)
    {
      IDbConnection Connection = (IDbConnection) null;
      IDbTransaction Transaction = (IDbTransaction) null;
      try
      {
        Datenbankverbindung.MainDBAccess.setIDCachSize((long) userInfo.Permissions.Count);
        Connection = DbBasis.PrimaryDB.GetDbConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserId=" + userInfo.UserId.ToString(), Connection);
        Schema.SoftwareUsersDataTable MyDataTable1 = new Schema.SoftwareUsersDataTable();
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        Schema.SoftwareUsersRow row1 = (Schema.SoftwareUsersRow) MyDataTable1.Rows[0];
        row1.Name = userInfo.Name;
        row1.PersonalNumber = userInfo.PersonalNumber;
        if (userInfo.Password != "")
          row1.Password = UserManager.ApplyHash(userInfo.Password);
        userInfo.Password = row1.Password;
        row1.UserRole = userInfo.RoleUserId;
        row1.PhoneNumber = userInfo.PhoneNumber;
        row1.EmailAddress = userInfo.EmailAddress;
        row1.OnlyFinterprintLogin = userInfo.OnlyFinterprintLogin;
        if (userInfo.PNSource == UserInfo.PN_Source.None && !userInfo.Name.StartsWith("* UserRole * ") && DbBasis.PrimaryDB.BaseDbConnection.ConnectionInfo.DbType == MeterDbTypes.MSSQL)
        {
          userInfo.ErrorMessageText = "The PNSource must be set to actual value.";
          return false;
        }
        if (!userInfo.Name.StartsWith("* UserRole * "))
        {
          ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE PersonalNumber=" + userInfo.PersonalNumber.ToString() + " AND PNSource = '" + userInfo.PNSource.ToString() + "'", Connection);
          Schema.SoftwareUsersDataTable MyDataTable2 = new Schema.SoftwareUsersDataTable();
          try
          {
            zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
          }
          catch
          {
          }
          if (MyDataTable2.Rows.Count == 0 || MyDataTable2.Rows.Count == 1 && MyDataTable2.Rows[0]["UserId"].ToString() == userInfo.UserId.ToString())
          {
            row1.PNSource = userInfo.PNSource.ToString();
          }
          else
          {
            userInfo.ErrorMessageText = "The personal number is repeated in the database.";
            return false;
          }
        }
        else
          row1.PNSource = userInfo.PNSource.ToString();
        row1.UserExtendedInfo = userInfo.UserExtendedInfo;
        row1.ControlKey = UserManager.GenerateControlKey(userInfo);
        zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction);
        ZRDataAdapter zrDataAdapter3 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM UserPermissions WHERE UserId=" + userInfo.UserId.ToString(), Connection);
        Schema.UserPermissionsDataTable permissionsDataTable = new Schema.UserPermissionsDataTable();
        zrDataAdapter3.Fill((DataTable) permissionsDataTable, Transaction);
        List<int> intList = new List<int>();
        List<Schema.UserPermissionsRow> userPermissionsRowList = new List<Schema.UserPermissionsRow>();
        foreach (PermissionInfo permission in userInfo.Permissions)
        {
          PermissionInfo permissionInfo = permission;
          Schema.UserPermissionsRow userPermissionsRow1 = permissionsDataTable.FirstOrDefault<Schema.UserPermissionsRow>((System.Func<Schema.UserPermissionsRow, bool>) (r => r.PermissionName == permissionInfo.PermissionName));
          if (userPermissionsRow1 != null)
          {
            intList.Add(userPermissionsRow1.PermissionId);
            if (userPermissionsRow1.UserId != userInfo.UserId)
              userPermissionsRow1.UserId = userInfo.UserId;
            if (userPermissionsRow1.PermissionName != permissionInfo.PermissionName)
              userPermissionsRow1.PermissionName = permissionInfo.PermissionName;
            if (userPermissionsRow1.PermissionValue != permissionInfo.PermissionValue)
              userPermissionsRow1.PermissionValue = permissionInfo.PermissionValue;
          }
          else
          {
            Schema.UserPermissionsRow userPermissionsRow2 = permissionsDataTable.NewUserPermissionsRow();
            userPermissionsRowList.Add(userPermissionsRow2);
            userPermissionsRow2.UserId = userInfo.UserId;
            userPermissionsRow2.PermissionName = permissionInfo.PermissionName;
            userPermissionsRow2.PermissionValue = permissionInfo.PermissionValue;
          }
        }
        if (userPermissionsRowList.Count > 0)
        {
          IdContainer newIds = DbBasis.PrimaryDB.BaseDbConnection.GetNewIds("UserPermissions", userPermissionsRowList.Count);
          foreach (Schema.UserPermissionsRow row2 in userPermissionsRowList)
          {
            row2.PermissionId = newIds.GetNextID();
            permissionsDataTable.AddUserPermissionsRow(row2);
            intList.Add(row2.PermissionId);
          }
        }
        foreach (Schema.UserPermissionsRow userPermissionsRow in (TypedTableBase<Schema.UserPermissionsRow>) permissionsDataTable)
        {
          if (!intList.Contains(userPermissionsRow.PermissionId))
            userPermissionsRow.Delete();
        }
        zrDataAdapter3.Update((DataTable) permissionsDataTable, Transaction);
        Transaction.Commit();
      }
      catch (Exception ex)
      {
        Transaction?.Rollback();
        userInfo.ErrorMessageText = "Exception: " + ex.ToString();
        return false;
      }
      finally
      {
        Connection?.Close();
        Datenbankverbindung.MainDBAccess.setIDCachSize(1L);
      }
      return true;
    }

    public static bool ChangUserPassword(string newPassword)
    {
      IDbConnection Connection = (IDbConnection) null;
      IDbTransaction Transaction = (IDbTransaction) null;
      try
      {
        Connection = DbBasis.PrimaryDB.GetDbConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserId = " + UserManager.CurrentUser.UserId.ToString(), Connection);
        Schema.SoftwareUsersDataTable MyDataTable1 = new Schema.SoftwareUsersDataTable();
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        Schema.SoftwareUsersRow row = (Schema.SoftwareUsersRow) MyDataTable1.Rows[0];
        UserInfo userInfo = new UserInfo();
        userInfo.Password = UserManager.ApplyHash(newPassword);
        userInfo.UserId = row.UserId;
        userInfo.Name = row.Name;
        userInfo.PersonalNumber = row.PersonalNumber;
        if (!row.IsLanguageSettingNull())
          userInfo.LanguageSetting = row.LanguageSetting;
        userInfo.RoleUserId = row.IsUserRoleNull() ? 0 : row.UserRole;
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM UserPermissions WHERE UserId=" + UserManager.CurrentUser.UserId.ToString(), Connection);
        Schema.UserPermissionsDataTable MyDataTable2 = new Schema.UserPermissionsDataTable();
        zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
        userInfo.Permissions = new List<PermissionInfo>();
        foreach (Schema.UserPermissionsRow userPermissionsRow in (TypedTableBase<Schema.UserPermissionsRow>) MyDataTable2)
          userInfo.Permissions.Add(new PermissionInfo()
          {
            PermissionId = userPermissionsRow.PermissionId,
            PermissionName = userPermissionsRow.PermissionName,
            PermissionValue = userPermissionsRow.PermissionValue
          });
        row.Password = userInfo.Password;
        row.ControlKey = UserManager.GenerateControlKey(userInfo);
        zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction);
        Transaction.Commit();
      }
      catch (Exception ex)
      {
        Transaction?.Rollback();
        return false;
      }
      finally
      {
        Connection?.Close();
      }
      return true;
    }

    internal static string LoadAllUsersFromClassig()
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection();
        dbConnection.Open();
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM GMM_User", dbConnection);
        Schema.GMM_UserDataTable gmmUserDataTable = new Schema.GMM_UserDataTable();
        zrDataAdapter.Fill((DataTable) gmmUserDataTable);
        SortedList<int, UserInfo> sortedList = new SortedList<int, UserInfo>();
        foreach (Schema.GMM_UserRow gmmUserRow in (TypedTableBase<Schema.GMM_UserRow>) gmmUserDataTable)
        {
          if (!sortedList.ContainsKey(gmmUserRow.UserPersonalNumber))
          {
            if (gmmUserRow.UserPersonalNumber >= 20)
            {
              try
              {
                UserInfo userInfo = new UserInfo();
                userInfo.Name = gmmUserRow.UserName;
                userInfo.UserId = gmmUserRow.UserPersonalNumber;
                userInfo.PersonalNumber = gmmUserRow.UserPersonalNumber;
                sortedList.Add(userInfo.UserId, userInfo);
              }
              catch
              {
              }
            }
          }
        }
        int num = 0;
        foreach (UserInfo userInfo in (IEnumerable<UserInfo>) sortedList.Values)
        {
          if (UserManager.AddUser(userInfo, userInfo.PersonalNumber))
            ++num;
          else
            stringBuilder.AppendLine("user " + userInfo.Name + "; Error: " + userInfo.ErrorMessageText);
        }
      }
      catch (Exception ex)
      {
        stringBuilder.AppendLine("Exception");
        stringBuilder.AppendLine(ex.ToString());
      }
      finally
      {
      }
      return stringBuilder.ToString();
    }

    public static bool GetUserNameByPersonalNumber(
      int personalNum,
      UserInfo.PN_Source pNSource,
      out string value)
    {
      IDbConnection Connection = (IDbConnection) null;
      IDbTransaction Transaction = (IDbTransaction) null;
      value = string.Empty;
      try
      {
        Connection = DbBasis.PrimaryDB.GetDbConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE PersonalNumber= " + personalNum.ToString() + " AND PNSource = '" + pNSource.ToString() + "'", Connection);
        Schema.SoftwareUsersDataTable MyDataTable = new Schema.SoftwareUsersDataTable();
        try
        {
          zrDataAdapter.Fill((DataTable) MyDataTable, Transaction);
        }
        catch
        {
        }
        if (MyDataTable.Rows.Count == 1)
        {
          value = MyDataTable.Rows[0]["Name"].ToString();
        }
        else
        {
          value = "The count is error! Count: " + MyDataTable.Rows.Count.ToString();
          return false;
        }
      }
      catch (Exception ex)
      {
        Transaction?.Rollback();
        throw new Exception(ex.Message);
      }
      finally
      {
        Connection?.Close();
        Datenbankverbindung.MainDBAccess.setIDCachSize(1L);
      }
      return true;
    }

    public static bool GetUserNameByUserID(int userID, out string value)
    {
      IDbConnection Connection = (IDbConnection) null;
      IDbTransaction Transaction = (IDbTransaction) null;
      value = string.Empty;
      try
      {
        Connection = DbBasis.PrimaryDB.GetDbConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserID = " + userID.ToString(), Connection);
        Schema.SoftwareUsersDataTable MyDataTable = new Schema.SoftwareUsersDataTable();
        try
        {
          zrDataAdapter.Fill((DataTable) MyDataTable, Transaction);
        }
        catch
        {
        }
        if (MyDataTable.Rows.Count == 1)
        {
          value = MyDataTable.Rows[0]["Name"].ToString();
        }
        else
        {
          value = "The count is error! Count: " + MyDataTable.Rows.Count.ToString();
          return false;
        }
      }
      catch (Exception ex)
      {
        Transaction?.Rollback();
        throw new Exception(ex.Message);
      }
      finally
      {
        Connection?.Close();
        Datenbankverbindung.MainDBAccess.setIDCachSize(1L);
      }
      return true;
    }

    internal static bool DeleteUser(int UserId)
    {
      IDbConnection Connection = (IDbConnection) null;
      IDbTransaction Transaction = (IDbTransaction) null;
      try
      {
        Connection = DbBasis.PrimaryDB.GetDbConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserId=" + UserId.ToString(), Connection);
        Schema.SoftwareUsersDataTable MyDataTable1 = new Schema.SoftwareUsersDataTable();
        zrDataAdapter1.Fill((DataTable) MyDataTable1, Transaction);
        if (MyDataTable1.Count != 1)
          throw new Exception("Delete not available user");
        if (MyDataTable1[0].Name.StartsWith("* UserRole * "))
        {
          List<string> usersForRole = UserManager.GetUsersForRole(MyDataTable1[0].UserId);
          if (usersForRole.Count > 0)
          {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Can not delete role. It is used by " + usersForRole.Count.ToString() + " users");
            foreach (string str in usersForRole)
              stringBuilder.AppendLine(str);
            throw new Exception(stringBuilder.ToString());
          }
          MyDataTable1[0].Delete();
        }
        else
        {
          MyDataTable1[0].Password = "Deleted";
          MyDataTable1[0].UserRole = 0;
          MyDataTable1[0].PNSource = UserInfo.PN_Source.None.ToString();
          MyDataTable1[0].UserExtendedInfo = "";
        }
        zrDataAdapter1.Update((DataTable) MyDataTable1, Transaction);
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM UserPermissions WHERE UserId=" + UserId.ToString(), Connection);
        Schema.UserPermissionsDataTable MyDataTable2 = new Schema.UserPermissionsDataTable();
        zrDataAdapter2.Fill((DataTable) MyDataTable2, Transaction);
        for (int index = 0; index < MyDataTable2.Rows.Count; ++index)
          MyDataTable2[index].Delete();
        zrDataAdapter2.Update((DataTable) MyDataTable2, Transaction);
        Transaction.Commit();
      }
      catch (Exception ex)
      {
        Transaction?.Rollback();
        throw new Exception(ex.Message);
      }
      finally
      {
        Connection?.Close();
        Datenbankverbindung.MainDBAccess.setIDCachSize(1L);
      }
      return true;
    }

    public static bool IsPasswordPoor(string password)
    {
      long[] numArray = new long[password.Length];
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      int index1 = 0;
      while (true)
      {
        if (index1 > 1)
        {
          if (flag1 || numArray[index1 - 2] != numArray[index1 - 1])
            flag1 = true;
          if (flag2 || numArray[index1 - 2] != numArray[index1 - 1] + 1L)
            flag2 = true;
          if (flag3 || numArray[index1 - 2] != numArray[index1 - 1] - 1L)
            flag3 = true;
          if (index1 > 2)
          {
            if (flag4 || numArray[index1 - 3] != numArray[index1 - 1])
              flag4 = true;
            if (index1 > 3)
            {
              if (flag5 || numArray[index1 - 4] != numArray[index1 - 1])
                flag5 = true;
              if (index1 > 4 && (flag6 || numArray[index1 - 5] != numArray[index1 - 1]))
                flag6 = true;
            }
          }
          if (index1 >= password.Length)
            break;
        }
        numArray[index1] = (long) Convert.ToUInt32(password[index1]);
        ++index1;
      }
      for (int index2 = 0; index2 < numArray.Length / 2 - 1; ++index2)
      {
        if (numArray[index2] != numArray[numArray.Length - 1 - index2])
        {
          flag7 = true;
          break;
        }
      }
      return !(flag2 & flag3 & flag1 & flag4 & flag5 & flag6 & flag7);
    }

    public static void LoadLicenseRights()
    {
      UserManager.EnabledLicenseRights = LicenseManager.CurrentLicense.GetPureEnabledRights();
    }

    public static bool IsUseUserLogin()
    {
      return UserManager.EnabledLicenseRights.ContainsKey("UseUserLogin");
    }

    public static void SetPermissionsFromLicense()
    {
      UserManager.rightInfoList = new FullRightInfoList();
      try
      {
        UserManager.rightInfoList.AddLicenseRights();
        UserManager.rightInfoList.AddScannedRights();
        UserManager.rightInfoList.FinalWorkAndSort();
        UserManager.rightInfoList.UseOnlyLicenseRights = true;
        UserManager.rightInfoList.SetEnabledRights();
        UserManager.CreateRightAccessLists();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage(nameof (SetPermissionsFromLicense), ex.Message, true);
      }
    }

    public static bool IsFingerPrintOnly(string Name)
    {
      try
      {
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE Name='" + Name + "'", DbBasis.PrimaryDB.GetDbConnection());
        Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
        zrDataAdapter.Fill((DataTable) softwareUsersDataTable);
        return softwareUsersDataTable.Rows.Count == 1 && bool.Parse(softwareUsersDataTable.Rows[0]["OnlyFinterprintLogin"].ToString());
      }
      catch
      {
        return false;
      }
    }

    public static bool SetCurrentUser(string Name, string Password, bool CheckPassword)
    {
      return UserManager.SetCurrentUser(Name, Password, CheckPassword, true);
    }

    public static bool SetCurrentUser(
      string Name,
      string Password,
      bool CheckPassword,
      bool saveLastUserInToConfigurationFile)
    {
      if (Name.StartsWith("* UserRole * "))
        return false;
      if (LicenseManager.CurrentLicense == null || LicenseManager.CurrentLicense.Rights == null)
        throw new ArgumentNullException("LicenseManager.CurrentLicense", "Can not set current user without a valid license file");
      ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE Name='" + Name + "'", DbBasis.PrimaryDB.GetDbConnection());
      Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
      zrDataAdapter1.Fill((DataTable) softwareUsersDataTable);
      if (softwareUsersDataTable.Rows.Count == 0 || softwareUsersDataTable[0].IsPasswordNull())
      {
        if (!UserManager.CreateNewUserFromOldUser(Name, Password))
          return false;
        ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE Name='" + Name + "'", DbBasis.PrimaryDB.GetDbConnection());
        softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
        zrDataAdapter2.Fill((DataTable) softwareUsersDataTable);
        if (softwareUsersDataTable.Rows.Count == 0)
          return false;
      }
      if (CheckPassword)
      {
        UserManager.ApplyHash(Password);
        string password = softwareUsersDataTable[0].Password;
        if (softwareUsersDataTable[0].Password != UserManager.ApplyHash(Password))
          return false;
      }
      if (UserManager.EnabledLicenseRights == null)
        UserManager.LoadLicenseRights();
      UserManager.rightInfoList = new FullRightInfoList();
      try
      {
        UserManager.rightInfoList.AddLicenseRights();
        UserManager.rightInfoList.AddScannedRights();
        UserManager.rightInfoList.AddCurrentUserRights(softwareUsersDataTable[0].UserId);
        UserManager.rightInfoList.FinalWorkAndSort();
        UserManager.CreateRightAccessLists();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Error on load user rights", ex.Message, true);
        return false;
      }
      UserManager.CurrentUser = UserManager.rightInfoList.CurrentUser;
      if (UserRights.GlobalUserRights == null)
        UserRights.GlobalUserRights = new UserRights();
      UserRights.GlobalUserRights.LoginName = UserManager.CurrentUser.Name;
      UserRights.GlobalUserRights.LoginPersonalNumber = UserManager.CurrentUser.PersonalNumber;
      if (saveLastUserInToConfigurationFile)
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("GMM", "LastUser", UserManager.CurrentUser.Name);
      return true;
    }

    public static void CreateRightAccessLists()
    {
      UserManager.rightInfoList.SetEnabledRights();
      UserManager.RuntimePermissionDeveloper = UserManager.rightInfoList.UserIsDeveloper;
      UserManager.EnabledRuntimePermissionsWithPath = new HashSet<string>();
      UserManager.EnabledRuntimePermissionsWithoutPath = new HashSet<string>();
      foreach (FullRightInfo rights in UserManager.rightInfoList.RightsList)
      {
        if (rights.Enabled)
        {
          UserManager.EnabledRuntimePermissionsWithPath.Add(rights.Right);
          UserManager.EnabledRuntimePermissionsWithoutPath.Add(rights.rightName);
        }
      }
    }

    public static bool CheckCurrentUserPassword(string Password)
    {
      ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserId = " + UserManager.CurrentUser.UserId.ToString(), DbBasis.PrimaryDB.GetDbConnection());
      Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
      zrDataAdapter.Fill((DataTable) softwareUsersDataTable);
      return softwareUsersDataTable.Rows.Count == 1 && !softwareUsersDataTable[0].IsPasswordNull() && !(UserManager.ApplyHash(Password) != softwareUsersDataTable[0].Password);
    }

    public static bool IsPermissionKnown(string permissionName)
    {
      string permSearch = "\\" + permissionName;
      return UserManager.CurrentUser.Permissions.Find((Predicate<PermissionInfo>) (item => item.PermissionName.EndsWith(permSearch))) != null || LicenseManager.CurrentLicense.Rights.Find((Predicate<RightInfo>) (item => item.Right.EndsWith(permSearch))) != null || LicenseManager.CurrentLicense.Plugins.Find((Predicate<PluginLicenseInfo>) (item => item.Plugin.EndsWith(permSearch))) != null;
    }

    public static bool HasUserDeveloperOption()
    {
      return !UserManager.rightInfoList.UserIsDeveloper && UserManager.rightInfoList.UserHasDeveloperOption;
    }

    internal static bool UserExists(string UserName)
    {
      ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE Name='" + UserName + "'", DbBasis.PrimaryDB.GetDbConnection());
      Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
      zrDataAdapter.Fill((DataTable) softwareUsersDataTable);
      return softwareUsersDataTable.Rows.Count > 0;
    }

    public static bool GarantFirstStartUser()
    {
      if (LicenseManager.CurrentLicense == null)
        throw new ArgumentNullException("LicenseManager.CurrentLicense", "Can not add the default 'Administrator' user to the table.");
      string str1 = "Administrator";
      string str2 = "start";
      ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE Name='" + str1 + "'", DbBasis.PrimaryDB.GetDbConnection());
      Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
      zrDataAdapter1.Fill((DataTable) softwareUsersDataTable);
      if (softwareUsersDataTable.Rows.Count != 0)
        return true;
      ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM GMM_User WHERE UserName='" + str1 + "'", DbBasis.PrimaryDB.GetDbConnection());
      Schema.GMM_UserDataTable gmmUserDataTable = new Schema.GMM_UserDataTable();
      zrDataAdapter2.Fill((DataTable) gmmUserDataTable);
      if (gmmUserDataTable.Rows.Count > 0)
        return true;
      UserInfo userInfo = new UserInfo();
      userInfo.Name = str1;
      userInfo.Password = str2;
      userInfo.PersonalNumber = 1;
      userInfo.Permissions = new List<PermissionInfo>();
      foreach (RightInfo right in LicenseManager.CurrentLicense.Rights)
        userInfo.Permissions.Add(new PermissionInfo()
        {
          PermissionName = right.Right,
          PermissionValue = right.Enable
        });
      UserManager.AddUser(userInfo, -1);
      return false;
    }

    public static List<string> GetUsersForRole(int roleUserId)
    {
      ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE UserRole =" + roleUserId.ToString(), DbBasis.PrimaryDB.GetDbConnection());
      Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
      zrDataAdapter.Fill((DataTable) softwareUsersDataTable);
      List<string> usersForRole = new List<string>();
      foreach (Schema.SoftwareUsersRow softwareUsersRow in (TypedTableBase<Schema.SoftwareUsersRow>) softwareUsersDataTable)
        usersForRole.Add(softwareUsersRow.Name);
      return usersForRole;
    }

    public static List<KeyValuePair<string, int>> GetUserRoles()
    {
      List<UserInfo> users = UserManager.LoadAndGetUsers("WHERE Name LIKE '* UserRole * %'");
      List<KeyValuePair<string, int>> userRoles = new List<KeyValuePair<string, int>>();
      foreach (UserInfo userInfo in users)
      {
        string key = userInfo.Name.Substring("* UserRole * ".Length);
        userRoles.Add(new KeyValuePair<string, int>(key, userInfo.UserId));
      }
      userRoles.Sort(new Comparison<KeyValuePair<string, int>>(UserManager.CompareKey));
      return userRoles;
    }

    public static int CompareKey(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
      return a.Key.CompareTo(b.Key);
    }

    public static bool IsNewLicenseModel() => LicenseManager.CurrentLicense != null;

    public static bool CheckPermission(UserRights.Rights right)
    {
      return UserManager.CheckPermission(right.ToString());
    }

    public static bool IsConfigParamVisible(OverrideID configParam)
    {
      return UserManager.CheckPermission("EnableAllConfigurationParameters") || UserManager.CheckPermission("ConfigParamVisible_" + configParam.ToString());
    }

    public static bool IsConfigParamEditable(OverrideID configParam)
    {
      return UserManager.CheckPermission("EnableAllConfigurationParameters") || UserManager.CheckPermission("ConfigParamEditable_" + configParam.ToString());
    }

    public static bool IsDeviceModelAllowed(string name)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(nameof (name));
      if (UserManager.CheckPermission("EnableAllTypeModels"))
        return true;
      return LicenseManager.CurrentLicense != null && LicenseManager.CurrentLicense.DeviceTypes != null && LicenseManager.CurrentLicense.DeviceTypes.Exists((Predicate<DeviceTypeInfo>) (x => x.DeviceType == name.Trim() && x.Enable));
    }

    public static bool CheckPermission(string permissionName)
    {
      if (string.IsNullOrEmpty(permissionName))
        throw new ArgumentNullException(nameof (permissionName), "Can not check the permissions!");
      if (UserManager.RuntimePermissionDeveloper)
        return !(permissionName == "Right\\ReadOnly");
      if (UserManager.EnabledRuntimePermissionsWithoutPath != null)
        return permissionName.Contains("\\") ? UserManager.EnabledRuntimePermissionsWithPath.Contains(permissionName) : UserManager.EnabledRuntimePermissionsWithoutPath.Contains(permissionName);
      permissionName = UserManager.NormalizePermissionName(permissionName);
      if (UserRights.GlobalUserRights != null)
      {
        if (permissionName == "Demo")
          return UserRights.GlobalUserRights.CheckRight();
        if (!Enum.IsDefined(typeof (UserRights.Rights), (object) permissionName))
          return true;
        bool pluginGmmFlag = UserRights.GlobalUserRights.PluginGMMFlag;
        UserRights.GlobalUserRights.PluginGMMFlag = false;
        bool flag = UserRights.GlobalUserRights.CheckRight((UserRights.Rights) Enum.Parse(typeof (UserRights.Rights), permissionName));
        UserRights.GlobalUserRights.PluginGMMFlag = pluginGmmFlag;
        return flag;
      }
      if (LicenseManager.CurrentLicense != null)
      {
        if (UserManager.EnabledLicenseRights == null)
          UserManager.EnabledLicenseRights = LicenseManager.CurrentLicense.GetPureEnabledRights();
        if (UserManager.EnabledLicenseRights.ContainsKey(permissionName) || UserManager.EnabledLicenseRights.ContainsKey("Developer") && !(permissionName == "ReadOnly"))
          return true;
      }
      return false;
    }

    public static string NormalizePermissionName(string fullPermissionName)
    {
      int num = fullPermissionName.LastIndexOf('\\');
      return num >= 0 ? fullPermissionName.Substring(num + 1) : fullPermissionName;
    }

    private static string GenerateControlKey(UserInfo userInfo)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(userInfo.UserId.ToString());
      stringBuilder.Append(userInfo.Name);
      stringBuilder.Append(userInfo.Password);
      stringBuilder.Append(userInfo.PersonalNumber.ToString());
      stringBuilder.Append(userInfo.LanguageSetting);
      if (userInfo.RoleUserId != 0)
        stringBuilder.Append(userInfo.RoleUserId.ToString());
      if (userInfo.RoleUserId == 0 && userInfo.Permissions != null)
      {
        foreach (PermissionInfo permissionInfo in (IEnumerable<PermissionInfo>) userInfo.Permissions.OrderBy<PermissionInfo, string>((System.Func<PermissionInfo, string>) (a => a.PermissionName)))
        {
          stringBuilder.Append(permissionInfo.PermissionName);
          stringBuilder.Append(permissionInfo.PermissionValue);
        }
      }
      return UserManager.ApplyHash(stringBuilder.ToString());
    }

    internal static bool CheckAutologin(string AutologinString, string User)
    {
      return AutologinString == UserManager.ApplyHash(true.ToString() + User) && UserManager.SetCurrentUser(User, "", false);
    }

    public static string GenerateAutologinString()
    {
      return UserManager.CurrentUser == null ? UserManager.ApplyHash(true.ToString() + "NoUser") : UserManager.ApplyHash(true.ToString() + UserManager.CurrentUser.Name);
    }

    private static string ApplyHash(string dataString)
    {
      return Encoding.Unicode.GetString(new SHA512Managed().ComputeHash(Encoding.Unicode.GetBytes(dataString)));
    }

    private static bool CreateNewUserFromOldUser(string UserName, string Password)
    {
      if (!UserRights.GlobalUserRights.VerifyUser(UserName, Password))
        return false;
      try
      {
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM GMM_User WHERE UserName='" + UserName + "'", DbBasis.PrimaryDB.GetDbConnection());
        Schema.GMM_UserDataTable gmmUserDataTable = new Schema.GMM_UserDataTable();
        zrDataAdapter.Fill((DataTable) gmmUserDataTable);
        if (gmmUserDataTable.Rows.Count < 1)
          return false;
        Schema.GMM_UserRow row = (Schema.GMM_UserRow) gmmUserDataTable.Rows[0];
        string str = row.UserPersonalNumber.ToString();
        string userRights = row.UserRights;
        UserInfo userInfo = new UserInfo()
        {
          Name = UserName,
          Password = Password,
          PersonalNumber = Convert.ToInt32(str),
          Permissions = UserManager.GetOldUserPermissions(userRights)
        };
        userInfo.UserId = userInfo.PersonalNumber;
        if (UserManager.AddUser(userInfo, userInfo.PersonalNumber))
        {
          UserManager.CurrentUser = userInfo;
          return true;
        }
        ZR_ClassLibMessages.AddErrorDescription(userInfo.ErrorMessageText);
        return false;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Exception");
        return false;
      }
    }

    private static List<PermissionInfo> GetOldUserPermissions(string sRights)
    {
      string[] strArray = sRights.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
      List<PermissionInfo> oldUserPermissions = new List<PermissionInfo>();
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = ((UserRights.Rights) int.Parse(strArray[index])).ToString();
        oldUserPermissions.Add(new PermissionInfo()
        {
          PermissionName = str,
          PermissionValue = true
        });
      }
      return oldUserPermissions;
    }

    internal static void Dispose()
    {
      UserManager.CurrentUser = (UserInfo) null;
      if (UserManager.EnabledLicenseRights != null)
        UserManager.EnabledLicenseRights.Clear();
      UserManager.EnabledLicenseRights = (SortedList<string, RightInfo>) null;
    }

    public static void Foo()
    {
      UserManager.CurrentUser = new UserInfo()
      {
        Permissions = new List<PermissionInfo>()
      };
      UserManager.CurrentUser.Permissions.Add(new PermissionInfo()
      {
        PermissionName = "Developer",
        PermissionValue = true
      });
      if (!AppDomain.CurrentDomain.FriendlyName.Contains("UnitTest"))
        return;
      UserManager.RuntimePermissionDeveloper = true;
    }

    public static bool CheckFingerPrintServerTime()
    {
      try
      {
        string str = "Data Source=msh-minolweb-03;Initial Catalog=eSI_ComtAtt;User ID=Sa;Password=Passw0rd;";
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = str;
        connection.Open();
        SqlCommand selectCommand = new SqlCommand("SELECT CONVERT(varchar(100), GETDATE(), 21)", connection);
        DataSet dataSet = new DataSet();
        new SqlDataAdapter(selectCommand).Fill(dataSet);
        connection.Close();
        return dataSet.Tables[0].Rows.Count == 1 && (DateTime.Now - Convert.ToDateTime(dataSet.Tables[0].Rows[0][0].ToString())).TotalSeconds <= 30.0;
      }
      catch
      {
        return false;
      }
    }

    public static int GetPNFromCometDatabase()
    {
      try
      {
        uint uint32 = Convert.ToUInt32(InstallationLog.FingerPrintID);
        string shortDateString = DateTime.Now.ToShortDateString();
        string str = "Data Source=msh-minolweb-03;Initial Catalog=Comet_eSI_V3;User ID=Sa;Password=Passw0rd;";
        SqlConnection connection = new SqlConnection();
        connection.ConnectionString = str;
        connection.Open();
        SqlCommand selectCommand = new SqlCommand("select top 1 E.EquCode,S.EmpName,S.EmpCode,A.RecDate,A.RecTime,A.LastDate from att_AtdRec A,Sys_Emp S,Equ_EquList E where E.EquCode=" + uint32.ToString() + " and E.EquID=A.EquID and A.EmpID=S.EmpID and A.RecDate = '" + shortDateString + "' order by A.LastDate desc", connection);
        DataSet dataSet = new DataSet();
        new SqlDataAdapter(selectCommand).Fill(dataSet);
        connection.Close();
        int fromCometDatabase;
        if (dataSet.Tables[0].Rows.Count == 1)
        {
          if ((DateTime.Now - Convert.ToDateTime(dataSet.Tables[0].Rows[0]["LastDate"].ToString())).TotalSeconds <= 90.0)
          {
            switch (InstallationLog.FingerPrintPNSource)
            {
              case UserInfo.PN_Source.ZSH:
                fromCometDatabase = Convert.ToInt32(dataSet.Tables[0].Rows[0]["EmpCode"].ToString().ToUpper().TrimStart('S'));
                break;
              case UserInfo.PN_Source.ZFZ:
                fromCometDatabase = Convert.ToInt32(dataSet.Tables[0].Rows[0]["EmpCode"].ToString().ToUpper().TrimStart('F'));
                break;
              default:
                fromCometDatabase = Convert.ToInt32(dataSet.Tables[0].Rows[0]["EmpCode"].ToString());
                break;
            }
          }
          else
            fromCometDatabase = 2;
        }
        else
          fromCometDatabase = 1;
        return fromCometDatabase;
      }
      catch
      {
        return 0;
      }
    }
  }
}
