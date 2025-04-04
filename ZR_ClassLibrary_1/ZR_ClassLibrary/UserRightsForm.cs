// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.UserRightsForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ZR_ClassLibrary
{
  public class UserRightsForm : Form
  {
    private CheckedListBox checkedListBoxRights;
    private Button buttonQuit;
    private Label label3;
    private TextBox textBoxUserPassword;
    private Label label4;
    private TextBox textBoxUserPassword2;
    private Label label5;
    private Label label6;
    private Button buttonNewUser;
    private Label label7;
    private TextBox textBoxPersonalNumber;
    private Button buttonLoadUserData;
    private Button buttonDeleteUser;
    private ComboBox comboBoxUserName;
    private Button buttonChangeUserData;
    private Button buttonDeleteAllRights;
    private Button buttonSetAllRights;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button buttonChangeUserRights;
    private MainMenu mainMenu1;
    private MenuItem menuItem1;
    private MenuItem menuItem2;
    private Label label1;
    private TextBox textBoxUserIdRange;
    private Button buttonNewLicence;
    private Button buttonSetFactoryWorkerPermissions;
    private UserRights MyUserRights;

    public UserRightsForm(GMMConfig GmmKonfigGroupIn, UserRights MyUserRights)
    {
      this.InitializeComponent();
      this.MyUserRights = MyUserRights;
      int length = Util.GetNamesOfEnum(typeof (UserRights.Rights)).Length;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (UserRightsForm));
      this.checkedListBoxRights = new CheckedListBox();
      this.buttonQuit = new Button();
      this.label3 = new Label();
      this.textBoxUserPassword = new TextBox();
      this.label4 = new Label();
      this.textBoxUserPassword2 = new TextBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.buttonChangeUserData = new Button();
      this.buttonNewUser = new Button();
      this.label7 = new Label();
      this.textBoxPersonalNumber = new TextBox();
      this.buttonLoadUserData = new Button();
      this.buttonDeleteUser = new Button();
      this.comboBoxUserName = new ComboBox();
      this.buttonDeleteAllRights = new Button();
      this.buttonSetAllRights = new Button();
      this.buttonChangeUserRights = new Button();
      this.buttonNewLicence = new Button();
      this.mainMenu1 = new MainMenu();
      this.menuItem1 = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.label1 = new Label();
      this.textBoxUserIdRange = new TextBox();
      this.buttonSetFactoryWorkerPermissions = new Button();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.checkedListBoxRights, "checkedListBoxRights");
      this.checkedListBoxRights.CheckOnClick = true;
      this.checkedListBoxRights.Name = "checkedListBoxRights";
      this.checkedListBoxRights.Sorted = true;
      componentResourceManager.ApplyResources((object) this.buttonQuit, "buttonQuit");
      this.buttonQuit.Name = "buttonQuit";
      this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.textBoxUserPassword, "textBoxUserPassword");
      this.textBoxUserPassword.Name = "textBoxUserPassword";
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.textBoxUserPassword2, "textBoxUserPassword2");
      this.textBoxUserPassword2.Name = "textBoxUserPassword2";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.buttonChangeUserData, "buttonChangeUserData");
      this.buttonChangeUserData.Name = "buttonChangeUserData";
      this.buttonChangeUserData.Click += new System.EventHandler(this.buttonChangeUserData_Click);
      componentResourceManager.ApplyResources((object) this.buttonNewUser, "buttonNewUser");
      this.buttonNewUser.Name = "buttonNewUser";
      this.buttonNewUser.Click += new System.EventHandler(this.buttonNewUser_Click);
      componentResourceManager.ApplyResources((object) this.label7, "label7");
      this.label7.Name = "label7";
      componentResourceManager.ApplyResources((object) this.textBoxPersonalNumber, "textBoxPersonalNumber");
      this.textBoxPersonalNumber.Name = "textBoxPersonalNumber";
      componentResourceManager.ApplyResources((object) this.buttonLoadUserData, "buttonLoadUserData");
      this.buttonLoadUserData.Name = "buttonLoadUserData";
      this.buttonLoadUserData.Click += new System.EventHandler(this.buttonLoadUserData_Click);
      componentResourceManager.ApplyResources((object) this.buttonDeleteUser, "buttonDeleteUser");
      this.buttonDeleteUser.Name = "buttonDeleteUser";
      this.buttonDeleteUser.Click += new System.EventHandler(this.buttonDeleteUser_Click);
      componentResourceManager.ApplyResources((object) this.comboBoxUserName, "comboBoxUserName");
      this.comboBoxUserName.Name = "comboBoxUserName";
      this.comboBoxUserName.Sorted = true;
      this.comboBoxUserName.SelectedIndexChanged += new System.EventHandler(this.comboBoxUserName_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.buttonDeleteAllRights, "buttonDeleteAllRights");
      this.buttonDeleteAllRights.Name = "buttonDeleteAllRights";
      this.buttonDeleteAllRights.Click += new System.EventHandler(this.buttonDeleteAllRights_Click);
      componentResourceManager.ApplyResources((object) this.buttonSetAllRights, "buttonSetAllRights");
      this.buttonSetAllRights.Name = "buttonSetAllRights";
      this.buttonSetAllRights.Click += new System.EventHandler(this.buttonSetAllRights_Click);
      componentResourceManager.ApplyResources((object) this.buttonChangeUserRights, "buttonChangeUserRights");
      this.buttonChangeUserRights.Name = "buttonChangeUserRights";
      this.buttonChangeUserRights.Click += new System.EventHandler(this.buttonChangeUserRights_Click);
      componentResourceManager.ApplyResources((object) this.buttonNewLicence, "buttonNewLicence");
      this.buttonNewLicence.Name = "buttonNewLicence";
      this.buttonNewLicence.Click += new System.EventHandler(this.buttonNewLicence_Click);
      this.mainMenu1.MenuItems.AddRange(new MenuItem[1]
      {
        this.menuItem1
      });
      this.menuItem1.Index = 0;
      this.menuItem1.MenuItems.AddRange(new MenuItem[1]
      {
        this.menuItem2
      });
      componentResourceManager.ApplyResources((object) this.menuItem1, "menuItem1");
      this.menuItem2.Index = 0;
      componentResourceManager.ApplyResources((object) this.menuItem2, "menuItem2");
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.textBoxUserIdRange, "textBoxUserIdRange");
      this.textBoxUserIdRange.Name = "textBoxUserIdRange";
      componentResourceManager.ApplyResources((object) this.buttonSetFactoryWorkerPermissions, "buttonSetFactoryWorkerPermissions");
      this.buttonSetFactoryWorkerPermissions.Name = "buttonSetFactoryWorkerPermissions";
      this.buttonSetFactoryWorkerPermissions.Click += new System.EventHandler(this.buttonSetFactoryWorkerPermissions_Click);
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.buttonSetFactoryWorkerPermissions);
      this.Controls.Add((Control) this.comboBoxUserName);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.buttonQuit);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.textBoxUserPassword);
      this.Controls.Add((Control) this.textBoxUserPassword2);
      this.Controls.Add((Control) this.textBoxUserIdRange);
      this.Controls.Add((Control) this.textBoxPersonalNumber);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.checkedListBoxRights);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.buttonChangeUserData);
      this.Controls.Add((Control) this.buttonNewUser);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.buttonLoadUserData);
      this.Controls.Add((Control) this.buttonDeleteUser);
      this.Controls.Add((Control) this.buttonDeleteAllRights);
      this.Controls.Add((Control) this.buttonSetAllRights);
      this.Controls.Add((Control) this.buttonChangeUserRights);
      this.Controls.Add((Control) this.buttonNewLicence);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Menu = this.mainMenu1;
      this.Name = nameof (UserRightsForm);
      this.Activated += new System.EventHandler(this.UserRights_Activated);
      this.Load += new System.EventHandler(this.UserRights_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void UserRights_Load(object sender, EventArgs e)
    {
      this.MyUserRights.User_Adapter = (ZRDataAdapter) null;
      this.MyUserRights.GarantUserTableLoaded();
      this.textBoxUserPassword.Text = string.Empty;
      this.textBoxUserPassword2.Text = string.Empty;
      this.comboBoxUserName.Items.Clear();
      for (int index = 0; index < this.MyUserRights.Typed_GMM_UserTable.Rows.Count; ++index)
        this.comboBoxUserName.Items.Add((object) this.MyUserRights.Typed_GMM_UserTable[index].UserName);
      UserRights.Rights[] availableRights1 = this.MyUserRights.BaseRightsList[this.MyUserRights.PackageNumber].AvailableRights;
      this.buttonSetFactoryWorkerPermissions.Visible = this.MyUserRights.PackageNumber == 2 || this.MyUserRights.PackageNumber == 1;
      for (int index = 0; index < availableRights1.Length; ++index)
        this.checkedListBoxRights.Items.Add((object) availableRights1[index].ToString());
      UserRights.Rights[] availableRights2 = this.MyUserRights.OptionRightsList[this.MyUserRights.OptionNumber].AvailableRights;
      for (int index = 0; index < availableRights2.Length; ++index)
      {
        if (!this.MyUserRights.BasicRights[(int) availableRights2[index]])
          this.checkedListBoxRights.Items.Add((object) availableRights2[index].ToString());
      }
    }

    private bool LoadAllUser()
    {
      this.MyUserRights.LoadAllUser();
      this.MyUserRights.User_Adapter = (ZRDataAdapter) null;
      this.MyUserRights.GarantUserTableLoaded();
      this.textBoxUserPassword.Text = string.Empty;
      this.textBoxUserPassword2.Text = string.Empty;
      this.comboBoxUserName.Items.Clear();
      for (int index = 0; index < this.MyUserRights.Typed_GMM_UserTable.Rows.Count; ++index)
        this.comboBoxUserName.Items.Add((object) this.MyUserRights.Typed_GMM_UserTable[index].UserName);
      return true;
    }

    private void buttonLoadUserData_Click(object sender, EventArgs e) => this.LoadUserData();

    private void LoadUserData()
    {
      this.MyUserRights.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.MyUserRights.Typed_GMM_UserTable.Select("UserName = '" + this.comboBoxUserName.Text + "'");
      if (gmmUserRowArray.Length != 1)
      {
        int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("User not defined"));
      }
      else
      {
        this.textBoxPersonalNumber.Text = gmmUserRowArray[0].UserPersonalNumber.ToString();
        this.MyUserRights.TempRights = this.MyUserRights.GetRightsFromString(gmmUserRowArray[0].UserRights);
        for (int index1 = 0; index1 < this.checkedListBoxRights.Items.Count; ++index1)
        {
          string str = this.checkedListBoxRights.Items[index1].ToString();
          foreach (object index2 in Enum.GetValues(typeof (UserRights.Rights)))
          {
            if (str == index2.ToString())
            {
              if (this.MyUserRights.TempRights[(int) index2])
              {
                this.checkedListBoxRights.SetItemChecked(index1, true);
                break;
              }
              this.checkedListBoxRights.SetItemChecked(index1, false);
              break;
            }
          }
        }
      }
    }

    private bool GenerateUserInfo()
    {
      this.MyUserRights.TempRightsString = this.GetRightsStringFromListBox();
      this.MyUserRights.TempRights = this.MyUserRights.GetRightsFromString(this.MyUserRights.TempRightsString);
      this.MyUserRights.TempName = this.comboBoxUserName.Text.Trim();
      if (this.MyUserRights.TempName.Length < 7)
      {
        int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("The minimun name size is 7 characters"));
        return false;
      }
      string text = this.textBoxUserPassword.Text;
      if (text.Length < 5)
      {
        int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("The minimun password size is 5 characters"));
        return false;
      }
      if (text != this.textBoxUserPassword2.Text)
      {
        int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("The passwords are not equal."));
        return false;
      }
      long num1;
      try
      {
        num1 = (long) int.Parse(this.textBoxPersonalNumber.Text);
      }
      catch
      {
        int num2 = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("Illegal personal number."));
        return false;
      }
      if (this.MyUserRights.UserID_RangeMax >= 0 && (num1 < (long) this.MyUserRights.UserID_RangeMin || num1 > (long) this.MyUserRights.UserID_RangeMax))
      {
        int num3 = (int) GMM_MessageBox.ShowMessage("Global Meter Manager", this.MyUserRights.MyRes.GetString("Personal number out of range"), true);
        return false;
      }
      this.MyUserRights.TempPersonalNumber = num1.ToString();
      this.MyUserRights.TempKey = this.MyUserRights.GenerateKey(this.MyUserRights.TempName, this.MyUserRights.TempPersonalNumber, this.MyUserRights.TempRightsString, text);
      return true;
    }

    private string GetRightsStringFromListBox()
    {
      string empty = string.Empty;
      for (int index = 0; index < this.checkedListBoxRights.Items.Count; ++index)
      {
        if (this.checkedListBoxRights.GetItemChecked(index))
        {
          if (empty.Length > 0)
            empty += " ";
          foreach (object obj in Enum.GetValues(typeof (UserRights.Rights)))
          {
            if (this.checkedListBoxRights.Items[index].ToString() == obj.ToString())
            {
              empty += ((int) obj).ToString("d");
              break;
            }
          }
        }
      }
      return empty;
    }

    private void buttonNewUser_Click(object sender, EventArgs e)
    {
      if (!this.GenerateUserInfo() || !this.MyUserRights.WriteNewUser())
        return;
      int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("New user created"));
    }

    private void buttonDeleteUser_Click(object sender, EventArgs e)
    {
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.MyUserRights.Typed_GMM_UserTable.Select("UserName = '" + this.comboBoxUserName.Text + "'");
      if (gmmUserRowArray.Length != 1)
      {
        int num1 = (int) GMM_MessageBox.ShowMessage(this.MyUserRights.MyRes.GetString("GMM user rights"), this.MyUserRights.MyRes.GetString("User not found"));
      }
      else
      {
        gmmUserRowArray[0].Delete();
        this.MyUserRights.User_Adapter.Update((DataTable) this.MyUserRights.Typed_GMM_UserTable);
        this.LoadAllUser();
        int num2 = (int) GMM_MessageBox.ShowMessage(this.MyUserRights.MyRes.GetString("GMM user rights"), "User '" + this.comboBoxUserName.Text + "' deleted");
      }
    }

    private void buttonQuit_Click(object sender, EventArgs e) => this.Hide();

    private void buttonChangeUserData_Click(object sender, EventArgs e)
    {
      if (!this.GenerateUserInfo())
        return;
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.MyUserRights.Typed_GMM_UserTable.Select("UserName = '" + this.comboBoxUserName.Text + "'");
      if (gmmUserRowArray.Length == 1)
      {
        gmmUserRowArray[0].Delete();
        this.MyUserRights.User_Adapter.Update((DataTable) this.MyUserRights.Typed_GMM_UserTable);
      }
      if (this.MyUserRights.WriteNewUser())
      {
        int num1 = (int) GMM_MessageBox.ShowMessage(this.MyUserRights.MyRes.GetString("GMM user rights"), this.MyUserRights.MyRes.GetString("User data changed"));
      }
      else
      {
        int num2 = (int) GMM_MessageBox.ShowMessage(this.MyUserRights.MyRes.GetString("GMM user rights"), this.MyUserRights.MyRes.GetString("User data not changed"), true);
      }
    }

    private void buttonChangeUserRights_Click(object sender, EventArgs e)
    {
      this.textBoxUserPassword.Text = "ChangePass";
      this.textBoxUserPassword2.Text = "ChangePass";
      bool userInfo = this.GenerateUserInfo();
      this.textBoxUserPassword.Text = "";
      this.textBoxUserPassword2.Text = "";
      if (!userInfo || !this.MyUserRights.WriteChangeUser())
        return;
      int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("User permissions changed"));
    }

    private void buttonSetAllRights_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.checkedListBoxRights.Items.Count; ++index)
        this.checkedListBoxRights.SetItemChecked(index, true);
    }

    private void buttonSetFactoryWorkerPermissions_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.checkedListBoxRights.Items.Count; ++index)
      {
        UserRights.Rights rights = (UserRights.Rights) Enum.Parse(typeof (UserRights.Rights), this.checkedListBoxRights.GetItemText(this.checkedListBoxRights.Items[index]));
        int num;
        switch (rights)
        {
          case UserRights.Rights.Developer:
          case UserRights.Rights.Administrator:
          case UserRights.Rights.ChiefOfEnergyTestCenter:
          case UserRights.Rights.ChiefOfWaterTestCenter:
          case UserRights.Rights.EquipmentCalibration:
          case UserRights.Rights.EquipmentCreation:
            num = 1;
            break;
          default:
            num = rights == UserRights.Rights.Database ? 1 : 0;
            break;
        }
        if (num != 0)
          this.checkedListBoxRights.SetItemChecked(index, false);
        else
          this.checkedListBoxRights.SetItemChecked(index, true);
      }
    }

    private void buttonDeleteAllRights_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.checkedListBoxRights.Items.Count; ++index)
        this.checkedListBoxRights.SetItemChecked(index, false);
    }

    private void comboBoxUserName_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.LoadUserData();
      this.textBoxUserPassword.Text = "";
      this.textBoxUserPassword2.Text = "";
    }

    private void UserRights_Activated(object sender, EventArgs e)
    {
      this.Text = this.MyUserRights.MyRes.GetString("User Rights") + " ( Admin: " + this.MyUserRights.LoginName + " )";
    }

    public int GetUserKeyChecksum(string UserName)
    {
      this.MyUserRights.GarantUserTableLoaded();
      Schema.GMM_UserRow[] gmmUserRowArray = (Schema.GMM_UserRow[]) this.MyUserRights.Typed_GMM_UserTable.Select("UserName = '" + UserName + "'");
      if (gmmUserRowArray.Length != 1)
      {
        int num = (int) MessageBox.Show(this.MyUserRights.MyRes.GetString("User not defined"), nameof (GetUserKeyChecksum));
        return 0;
      }
      string userKey = gmmUserRowArray[0].UserKey;
      int userKeyChecksum = 0;
      int num1 = 0;
      for (int index = 0; index < userKey.Length; ++index)
      {
        num1 += 7;
        if (num1 > 25)
          num1 -= 25;
        userKeyChecksum += (int) userKey[index] << num1;
      }
      if (userKeyChecksum == 0)
        userKeyChecksum = 1267494;
      return userKeyChecksum;
    }

    private void buttonNewLicence_Click(object sender, EventArgs e)
    {
      this.MyUserRights.NewLicence();
    }
  }
}
