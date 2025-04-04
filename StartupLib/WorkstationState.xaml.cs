// Decompiled with JetBrains decompiler
// Type: StartupLib.WorkstationState
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using CommonWPF;
using GmmDbLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace StartupLib
{
  public partial class WorkstationState : Window, IComponentConnector
  {
    private static string infoText = "Insert change comment here!";
    private ZRDataAdapter InstallationChangeLogAdapter;
    private InstallationData.InstallationChangeLogDataTable InstallationChangeLogTable;
    private bool ButtonsEnabled = false;
    private List<TestbenchStateItem> changesList;
    private int installationID;
    internal TextBox TextBoxInstallationName;
    internal Button ButtonChangeInstallationName;
    internal DataGrid DataGridStateList;
    internal TextBlock TextBlockFullDescription;
    internal ComboBox ComboBoxStates;
    internal Button ButtonSaveNewState;
    internal TextBox TextBoxNewStateComment;
    private bool _contentLoaded;

    public static List<string> TestBenchStates { get; set; }

    static WorkstationState()
    {
      WorkstationState.TestBenchStates = new List<string>();
      foreach (string name in Enum.GetNames(typeof (BasicTestbenchStates)))
        WorkstationState.TestBenchStates.Add(name);
    }

    public WorkstationState()
    {
      this.InitializeComponent();
      InstallationLog.RunLog();
      this.installationID = InstallationLog.InstallationID;
      this.ButtonsEnabled = UserManager.CheckPermission("Administrator") || UserManager.CheckPermission("TestbenchManager");
      if (this.ButtonsEnabled)
        this.ButtonChangeInstallationName.IsEnabled = true;
      this.ComboBoxStates.ItemsSource = (IEnumerable) WorkstationState.TestBenchStates;
      this.TextBoxNewStateComment.Text = WorkstationState.infoText;
      this.LoadStateList();
    }

    private Task<List<string>> LoadPhoneNumberOrEmailAddressListOfEmployees(
      WorkstationState.SendingType Type,
      BasicTestbenchStates State,
      WorkstationState.EmployeeRole Role)
    {
      return Task.Run<List<string>>((Func<List<string>>) (() =>
      {
        string str1 = Type.ToString() + "%" + State.ToString() + "%";
        string str2 = Type.ToString() + "%EachChangedState%";
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection();
        IDbCommand DbCommand = DbBasis.PrimaryDB.DbCommand(dbConnection);
        string str3 = "SELECT EQ_EquipmentValue.EquipmentID,EQ_EquipmentValue.Value,EQ_Equipment.AccessName,EQ_Equipment.Name,EQ_Equipment.EquipmentType from EQ_EquipmentValue,EQ_Equipment where (EQ_EquipmentValue.ValueType like '" + str1 + "' OR EQ_EquipmentValue.ValueType LIKE '" + str2 + "') and (EQ_Equipment.EquipmentType = '" + Role.ToString() + "') and (EQ_EquipmentValue.EquipmentID = EQ_Equipment.EquipmentID) and (EQ_Equipment.EquipmentGroupID = " + InstallationLog.MainEquipmentID.ToString() + ") AND (EQ_Equipment.OutOfOperationDate >= @outOfOperationDate OR EQ_Equipment.OutOfOperationDate IS NULL)";
        DbCommand.CommandText = str3;
        IDbDataParameter parameter = DbCommand.CreateParameter();
        parameter.ParameterName = "@outOfOperationDate";
        IDbDataParameter dbDataParameter = parameter;
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.ToUniversalTime();
        string str4 = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        dbDataParameter.Value = (object) str4;
        parameter.DbType = DbType.DateTime;
        DbCommand.Parameters.Add((object) parameter);
        ZRDataAdapter zrDataAdapter1 = DbBasis.PrimaryDB.ZRDataAdapter(DbCommand);
        DataTable dataTable = new DataTable();
        zrDataAdapter1.Fill(dataTable);
        if (dataTable.Rows.Count >= 1)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            if (bool.Parse(row["Value"].ToString()))
              stringList1.Add(row["AccessName"].ToString());
          }
        }
        if (stringList1.Count >= 1)
        {
          string str5 = "UserId = " + stringList1[0];
          foreach (string str6 in stringList1)
            str5 = str5 + "or UserId=" + str6;
          ZRDataAdapter zrDataAdapter2 = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM SoftwareUsers WHERE " + str5, DbBasis.PrimaryDB.GetDbConnection());
          Schema.SoftwareUsersDataTable softwareUsersDataTable = new Schema.SoftwareUsersDataTable();
          zrDataAdapter2.Fill((DataTable) softwareUsersDataTable);
          if (softwareUsersDataTable.Rows.Count >= 1 && !softwareUsersDataTable[0].IsPasswordNull())
          {
            foreach (DataRow row in (InternalDataCollectionBase) softwareUsersDataTable.Rows)
            {
              if (Type == WorkstationState.SendingType.Email)
                stringList2.Add(row["EmailAddress"].ToString());
              else if (Type == WorkstationState.SendingType.SMS)
                stringList2.Add(row["PhoneNumber"].ToString());
            }
          }
        }
        return stringList2;
      }));
    }

    private Task<bool> IsTheInstallstionIDMonitored()
    {
      return Task.Run<bool>((Func<bool>) (() =>
      {
        List<string> stringList = new List<string>();
        IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection();
        IDbCommand DbCommand = DbBasis.PrimaryDB.DbCommand(dbConnection);
        string str1 = "SELECT EQ_EquipmentValue.Value from EQ_EquipmentValue,EQ_Equipment where (EQ_EquipmentValue.ValueType = 'InstallationID') and (EQ_Equipment.EquipmentType = 'MonitoredInstallation') and (EQ_EquipmentValue.EquipmentID = EQ_Equipment.EquipmentID) and (EQ_Equipment.EquipmentGroupID = " + InstallationLog.MainEquipmentID.ToString() + ") AND (EQ_Equipment.OutOfOperationDate >= @outOfOperationDate OR EQ_Equipment.OutOfOperationDate IS NULL)";
        DbCommand.CommandText = str1;
        IDbDataParameter parameter = DbCommand.CreateParameter();
        parameter.ParameterName = "@outOfOperationDate";
        IDbDataParameter dbDataParameter = parameter;
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.ToUniversalTime();
        string str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        dbDataParameter.Value = (object) str2;
        parameter.DbType = DbType.DateTime;
        DbCommand.Parameters.Add((object) parameter);
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter(DbCommand);
        DataTable dataTable = new DataTable();
        zrDataAdapter.Fill(dataTable);
        if (dataTable.Rows.Count >= 1)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            stringList.Add(row["Value"].ToString());
          if (stringList.Contains(this.installationID.ToString()))
            return true;
        }
        return false;
      }));
    }

    private Task<bool> WriteNotificationRecordIntoDatabase(
      BasicTestbenchStates State,
      bool SMSState,
      bool EmailState,
      DateTime dateTime)
    {
      return Task.Run<bool>((Func<bool>) (() =>
      {
        try
        {
          IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection();
          dbConnection.Open();
          IDbTransaction Transaction = dbConnection.BeginTransaction();
          ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter("SELECT * FROM NotificationRecord WHERE EquipmentID = " + InstallationLog.MainEquipmentID.ToString(), dbConnection);
          Schema.NotificationRecordDataTable MyDataTable = new Schema.NotificationRecordDataTable();
          zrDataAdapter.Fill((DataTable) MyDataTable, Transaction);
          Schema.NotificationRecordRow row = MyDataTable.NewNotificationRecordRow();
          row.EquipmentID = InstallationLog.MainEquipmentID;
          row.BasicState = State.ToString();
          row.SMSState = SMSState;
          row.EmailState = EmailState;
          row.DateTime = dateTime;
          MyDataTable.AddNotificationRecordRow(row);
          zrDataAdapter.Update((DataTable) MyDataTable, Transaction);
          Transaction.Commit();
          dbConnection.Close();
          return true;
        }
        catch
        {
          return false;
        }
      }));
    }

    private void LoadStateList()
    {
      try
      {
        this.TextBoxInstallationName.Text = InstallationLog.InstallationName;
        IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection();
        string SqlCommand1 = "SELECT * from Installations WHERE PcName ='" + Environment.MachineName + "' AND Installations.InstallationPath ='" + AppDomain.CurrentDomain.BaseDirectory + "'";
        ZRDataAdapter zrDataAdapter = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand1, dbConnection);
        InstallationData.InstallationsDataTable installationsDataTable = new InstallationData.InstallationsDataTable();
        zrDataAdapter.Fill((DataTable) installationsDataTable);
        string SqlCommand2 = "SELECT * FROM InstallationChangeLog WHERE InstallationId = " + installationsDataTable[0].InstallationId.ToString() + " ORDER BY ChangeTime DESC";
        this.InstallationChangeLogAdapter = DbBasis.PrimaryDB.ZRDataAdapter(SqlCommand2, dbConnection);
        this.InstallationChangeLogTable = new InstallationData.InstallationChangeLogDataTable();
        this.InstallationChangeLogAdapter.Fill((DataTable) this.InstallationChangeLogTable);
        this.changesList = new List<TestbenchStateItem>();
        string str = "";
        foreach (InstallationData.InstallationChangeLogRow installationChangeLogRow in (TypedTableBase<InstallationData.InstallationChangeLogRow>) this.InstallationChangeLogTable)
        {
          if (!installationChangeLogRow.IsBasicStateNull() && installationChangeLogRow.BasicState.Length != 0 && !(installationChangeLogRow.BasicState == str))
          {
            str = installationChangeLogRow.BasicState;
            this.changesList.Add(new TestbenchStateItem()
            {
              ChangeTime = installationChangeLogRow.ChangeTime,
              BasicState = installationChangeLogRow.BasicState,
              DbRow = installationChangeLogRow
            });
          }
        }
        this.DataGridStateList.ItemsSource = (IEnumerable) this.changesList;
        if (this.changesList.Count <= 0)
          return;
        this.DataGridStateList.SelectedIndex = 0;
      }
      catch (Exception ex)
      {
        this.TextBlockFullDescription.Text = ex.Message;
      }
    }

    private void SetInfoText(TestbenchStateItem theItem)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Workbench state: " + theItem.BasicState);
      StringBuilder stringBuilder2 = stringBuilder1;
      DateTime changeTime = theItem.ChangeTime;
      string longDateString = changeTime.ToLongDateString();
      changeTime = theItem.ChangeTime;
      string longTimeString = changeTime.ToLongTimeString();
      string str = "Change time: " + longDateString + " " + longTimeString;
      stringBuilder2.AppendLine(str);
      if (!theItem.DbRow.IsChangeInfoNull())
      {
        stringBuilder1.AppendLine();
        stringBuilder1.Append(theItem.DbRow.ChangeInfo);
      }
      this.TextBlockFullDescription.Text = stringBuilder1.ToString();
    }

    private async void ButtonSaveNewState_Click(object sender, RoutedEventArgs e)
    {
      BasicTestbenchStates basicState = (BasicTestbenchStates) Enum.Parse(typeof (BasicTestbenchStates), this.ComboBoxStates.SelectedValue.ToString());
      DateTime dateTime = DateTime.Now;
      if (this.ComboBoxStates.SelectedIndex < 0)
        return;
      try
      {
        InstallationData.InstallationChangeLogRow lastRow = this.InstallationChangeLogTable[this.InstallationChangeLogTable.Count - 1];
        InstallationData.InstallationChangeLogRow newRow = this.InstallationChangeLogTable.NewInstallationChangeLogRow();
        newRow.ItemArray = (object[]) lastRow.ItemArray.Clone();
        newRow.ChangeTime = dateTime;
        newRow.BasicState = this.ComboBoxStates.SelectedValue.ToString();
        string commentText = this.TextBoxNewStateComment.Text.Trim();
        StringBuilder fullComment = new StringBuilder();
        if (UserManager.CurrentUser != null)
          fullComment.AppendLine(UserManager.CurrentUser.Name);
        if (commentText != WorkstationState.infoText && commentText.Length > 0)
        {
          fullComment.AppendLine();
          fullComment.Append(commentText);
        }
        if (fullComment.Length > 0)
          newRow.ChangeInfo = fullComment.ToString();
        else
          newRow.SetChangeInfoNull();
        this.InstallationChangeLogTable.AddInstallationChangeLogRow(newRow);
        this.InstallationChangeLogAdapter.Update((DataTable) this.InstallationChangeLogTable);
        lastRow = (InstallationData.InstallationChangeLogRow) null;
        newRow = (InstallationData.InstallationChangeLogRow) null;
        commentText = (string) null;
        fullComment = (StringBuilder) null;
      }
      catch (Exception ex)
      {
        this.TextBlockFullDescription.Text = ex.Message;
      }
      this.LoadStateList();
      bool flag = await this.IsTheInstallstionIDMonitored();
      if (!flag)
        return;
      bool SMS_Res = false;
      bool Mail_Res = false;
      List<string> ResponsibleEmailAddressList = await this.LoadPhoneNumberOrEmailAddressListOfEmployees(WorkstationState.SendingType.Email, basicState, WorkstationState.EmployeeRole.ResponsibleEmployee);
      List<string> RelatedEmailAddressList = await this.LoadPhoneNumberOrEmailAddressListOfEmployees(WorkstationState.SendingType.Email, basicState, WorkstationState.EmployeeRole.RelatedEmployee);
      List<string> ResponsibleSMSAddressList = await this.LoadPhoneNumberOrEmailAddressListOfEmployees(WorkstationState.SendingType.SMS, basicState, WorkstationState.EmployeeRole.ResponsibleEmployee);
      List<string> RelatedSMSAddressList = await this.LoadPhoneNumberOrEmailAddressListOfEmployees(WorkstationState.SendingType.SMS, basicState, WorkstationState.EmployeeRole.RelatedEmployee);
      ResponsibleEmailAddressList.Remove("");
      RelatedEmailAddressList.Remove("");
      ResponsibleSMSAddressList.Remove("");
      RelatedSMSAddressList.Remove("");
      if (ResponsibleSMSAddressList.Count >= 1 || RelatedSMSAddressList.Count >= 1)
      {
        MessageSupport MS = new MessageSupport();
        foreach (string num in ResponsibleSMSAddressList)
          MS.Mobile.Add(num);
        foreach (string num in RelatedSMSAddressList)
          MS.Mobile.Add(num);
        MS.Content = InstallationLog.FingerPrintPNSource != UserInfo.PN_Source.ZFZ && InstallationLog.FingerPrintPNSource != UserInfo.PN_Source.ZSH ? "Name: " + InstallationLog.InstallationName + " State: " + basicState.ToString() : "测试台名称: " + InstallationLog.InstallationName + " 测试台状态: " + basicState.ToString();
        SMS_Res = await MS.SendMessageAndGetResponse();
        MS = (MessageSupport) null;
      }
      if (ResponsibleEmailAddressList.Count >= 1 || RelatedEmailAddressList.Count >= 1)
      {
        MailSupport Msu = new MailSupport();
        if (InstallationLog.FingerPrintPNSource == UserInfo.PN_Source.ZFZ || InstallationLog.FingerPrintPNSource == UserInfo.PN_Source.ZSH)
        {
          Msu.MailBody = "测试台名称: " + InstallationLog.InstallationName + " 测试台状态: " + basicState.ToString();
          Msu.MailSubject = "测试台状态通知";
        }
        else
        {
          Msu.MailBody = "Name: " + InstallationLog.InstallationName + " State: " + basicState.ToString();
          Msu.MailSubject = "Testbench State Notification";
        }
        if (ResponsibleEmailAddressList.Count >= 1)
        {
          foreach (string add in ResponsibleEmailAddressList)
            Msu.MailTo.Add(add);
          foreach (string add in RelatedEmailAddressList)
            Msu.MailCopy.Add(add);
          Mail_Res = await Msu.SendMail();
        }
        else
        {
          foreach (string add in RelatedEmailAddressList)
            Msu.MailTo.Add(add);
          Mail_Res = await Msu.SendMail();
        }
        Msu = (MailSupport) null;
      }
      int num1 = await this.WriteNotificationRecordIntoDatabase(basicState, SMS_Res, Mail_Res, dateTime) ? 1 : 0;
      ResponsibleEmailAddressList = (List<string>) null;
      RelatedEmailAddressList = (List<string>) null;
      ResponsibleSMSAddressList = (List<string>) null;
      RelatedSMSAddressList = (List<string>) null;
    }

    private void DataGridStateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.DataGridStateList.SelectedItem == null)
        return;
      this.SetInfoText((TestbenchStateItem) this.DataGridStateList.SelectedItem);
    }

    private void ComboBoxStates_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ButtonsEnabled)
        return;
      if (this.ComboBoxStates.SelectedItem != null)
      {
        string str = this.ComboBoxStates.SelectedItem.ToString();
        if (this.InstallationChangeLogTable.Count == 0 || this.InstallationChangeLogTable[0].BasicState != str)
        {
          this.ButtonSaveNewState.IsEnabled = true;
          return;
        }
      }
      this.ButtonSaveNewState.IsEnabled = false;
    }

    private void ButtonChangeInstallationName_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string newTestBenchName = ChangeTestBenchName.GetNewTestBenchName(this.TextBoxInstallationName.Text, Environment.MachineName);
        if (newTestBenchName == null || !ChangeTestBenchName.SetTestBenchName(newTestBenchName, Environment.MachineName, AppDomain.CurrentDomain.BaseDirectory))
          return;
        InstallationLog.RunLog();
        this.TextBoxInstallationName.Text = InstallationLog.InstallationName;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/StartupLib;component/workstationstate.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextBoxInstallationName = (TextBox) target;
          break;
        case 2:
          this.ButtonChangeInstallationName = (Button) target;
          this.ButtonChangeInstallationName.Click += new RoutedEventHandler(this.ButtonChangeInstallationName_Click);
          break;
        case 3:
          this.DataGridStateList = (DataGrid) target;
          this.DataGridStateList.SelectionChanged += new SelectionChangedEventHandler(this.DataGridStateList_SelectionChanged);
          break;
        case 4:
          this.TextBlockFullDescription = (TextBlock) target;
          break;
        case 5:
          this.ComboBoxStates = (ComboBox) target;
          this.ComboBoxStates.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxStates_SelectionChanged);
          break;
        case 6:
          this.ButtonSaveNewState = (Button) target;
          this.ButtonSaveNewState.Click += new RoutedEventHandler(this.ButtonSaveNewState_Click);
          break;
        case 7:
          this.TextBoxNewStateComment = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public enum EmployeeRole
    {
      RelatedEmployee,
      ResponsibleEmployee,
    }

    public enum SendingType
    {
      Email,
      SMS,
    }
  }
}
