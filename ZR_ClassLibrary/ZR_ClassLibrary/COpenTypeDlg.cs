// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.COpenTypeDlg
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using GmmDbLib;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class COpenTypeDlg : Form
  {
    private Button btSuchen;
    private DataGrid dataGrid1;
    private Button btOk;
    private Button btCancel;
    private TextBox txtbx;
    public bool getTypeList = false;
    public ArrayList TypeList;
    private DataTable tov = new DataTable("TypeOverwriteParameter");
    private int pageNr;
    public string FirmwareVersion;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;
    private DataTable dtatblMeterInfo;
    private Button btExtendedSeek;
    private DataGridTableStyle dataGridTableStyle1;
    private DataGridTextBoxColumn dataGridColPPSArtikelNr;
    private DataGridTextBoxColumn dataGridColHardwareName;
    private DataGridTextBoxColumn dataGridColFirmwareVersion;
    private DataGridTextBoxColumn dataGridColMeterInfoID;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private DataGridTextBoxColumn dataGridColMTDescription;
    private Button SelectBaseTypeButton;
    private Panel panel1;
    private TextBox textBoxMeterType;
    private TextBox textBoxHardwareType;
    private TextBox textBoxMeterHardware;
    private Label label2;
    private Label label3;
    private Label label4;
    private DataGrid dataGridTypeOverWriteParameter;
    private DataGridTableStyle dataGridTableStyle2;
    private DataGridTextBoxColumn dataGridTextBoxParameterValue;
    private TextBox textBoxMeterinfoDescription;
    private Label label1;
    private DataGridTextBoxColumn dataGridTextBoxMeterHardwareID;
    private DataGridTextBoxColumn dataGridTextBoxMeterTypeID;
    private DataGridTextBoxColumn dataGridTextBoxHardwareTypeID;
    private DataGridTextBoxColumn dataGridTextBoxDefaultFunction;
    private Button buttonPrint;
    private PrintDialog printDialog;
    private PrintDocument printDocument;
    public int m_nMeterInfoID;
    public int m_nMeterTypeID;
    private Button buttonDeleteType;
    private string m_sPPSNr;

    public string FilterByPPSArtikelNr
    {
      set => this.txtbx.Text = value;
    }

    public string MeterTypeDescription { get; private set; }

    public COpenTypeDlg()
    {
      this.InitializeComponent();
      this.m_nMeterInfoID = int.MaxValue;
      this.m_sPPSNr = "";
      this.FirmwareVersion = "";
      this.initEnviromentOnUser();
    }

    public COpenTypeDlg(bool ingetTypeList)
    {
      this.InitializeComponent();
      this.Text = "Developer special Function extractor";
      this.getTypeList = ingetTypeList;
      if (this.getTypeList)
        this.TypeList = new ArrayList();
      this.m_nMeterInfoID = int.MaxValue;
      this.m_sPPSNr = "";
      this.FirmwareVersion = "";
      this.initEnviromentOnUser();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (COpenTypeDlg));
      this.txtbx = new TextBox();
      this.btSuchen = new Button();
      this.dataGrid1 = new DataGrid();
      this.dataGridTableStyle1 = new DataGridTableStyle();
      this.dataGridColPPSArtikelNr = new DataGridTextBoxColumn();
      this.dataGridColMTDescription = new DataGridTextBoxColumn();
      this.dataGridColMeterInfoID = new DataGridTextBoxColumn();
      this.dataGridColHardwareName = new DataGridTextBoxColumn();
      this.dataGridColFirmwareVersion = new DataGridTextBoxColumn();
      this.dataGridTextBoxMeterHardwareID = new DataGridTextBoxColumn();
      this.dataGridTextBoxMeterTypeID = new DataGridTextBoxColumn();
      this.dataGridTextBoxHardwareTypeID = new DataGridTextBoxColumn();
      this.dataGridTextBoxDefaultFunction = new DataGridTextBoxColumn();
      this.btOk = new Button();
      this.btCancel = new Button();
      this.dtatblMeterInfo = new DataTable();
      this.btExtendedSeek = new Button();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.SelectBaseTypeButton = new Button();
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.textBoxMeterinfoDescription = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.dataGridTypeOverWriteParameter = new DataGrid();
      this.dataGridTableStyle2 = new DataGridTableStyle();
      this.dataGridTextBoxParameterValue = new DataGridTextBoxColumn();
      this.textBoxMeterHardware = new TextBox();
      this.textBoxHardwareType = new TextBox();
      this.textBoxMeterType = new TextBox();
      this.buttonPrint = new Button();
      this.printDialog = new PrintDialog();
      this.printDocument = new PrintDocument();
      this.buttonDeleteType = new Button();
      this.dataGrid1.BeginInit();
      this.dtatblMeterInfo.BeginInit();
      this.panel1.SuspendLayout();
      this.dataGridTypeOverWriteParameter.BeginInit();
      this.SuspendLayout();
      this.txtbx.AcceptsReturn = true;
      componentResourceManager.ApplyResources((object) this.txtbx, "txtbx");
      this.txtbx.Name = "txtbx";
      this.txtbx.KeyDown += new KeyEventHandler(this.txtbx_KeyDown);
      componentResourceManager.ApplyResources((object) this.btSuchen, "btSuchen");
      this.btSuchen.Name = "btSuchen";
      this.btSuchen.Click += new System.EventHandler(this.btSuchen_Click);
      componentResourceManager.ApplyResources((object) this.dataGrid1, "dataGrid1");
      this.dataGrid1.HeaderForeColor = SystemColors.ControlText;
      this.dataGrid1.Name = "dataGrid1";
      this.dataGrid1.ReadOnly = true;
      this.dataGrid1.TableStyles.AddRange(new DataGridTableStyle[1]
      {
        this.dataGridTableStyle1
      });
      this.dataGrid1.DoubleClick += new System.EventHandler(this.dataGrid1_DoubleClick);
      this.dataGrid1.MouseUp += new MouseEventHandler(this.dtaGrdMenueViewItemMouseUp);
      this.dataGrid1.Navigate += new NavigateEventHandler(this.dataGrid1_Navigate);
      this.dataGridTableStyle1.DataGrid = this.dataGrid1;
      this.dataGridTableStyle1.GridColumnStyles.AddRange(new DataGridColumnStyle[9]
      {
        (DataGridColumnStyle) this.dataGridColPPSArtikelNr,
        (DataGridColumnStyle) this.dataGridColMTDescription,
        (DataGridColumnStyle) this.dataGridColMeterInfoID,
        (DataGridColumnStyle) this.dataGridColHardwareName,
        (DataGridColumnStyle) this.dataGridColFirmwareVersion,
        (DataGridColumnStyle) this.dataGridTextBoxMeterHardwareID,
        (DataGridColumnStyle) this.dataGridTextBoxMeterTypeID,
        (DataGridColumnStyle) this.dataGridTextBoxHardwareTypeID,
        (DataGridColumnStyle) this.dataGridTextBoxDefaultFunction
      });
      this.dataGridTableStyle1.HeaderForeColor = SystemColors.ControlText;
      this.dataGridColPPSArtikelNr.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridColPPSArtikelNr, "dataGridColPPSArtikelNr");
      this.dataGridColPPSArtikelNr.ReadOnly = true;
      this.dataGridColMTDescription.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridColMTDescription, "dataGridColMTDescription");
      this.dataGridColMTDescription.ReadOnly = true;
      this.dataGridColMeterInfoID.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridColMeterInfoID, "dataGridColMeterInfoID");
      this.dataGridColMeterInfoID.ReadOnly = true;
      this.dataGridColHardwareName.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridColHardwareName, "dataGridColHardwareName");
      this.dataGridColHardwareName.ReadOnly = true;
      this.dataGridColFirmwareVersion.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridColFirmwareVersion, "dataGridColFirmwareVersion");
      this.dataGridColFirmwareVersion.ReadOnly = true;
      this.dataGridTextBoxMeterHardwareID.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridTextBoxMeterHardwareID, "dataGridTextBoxMeterHardwareID");
      this.dataGridTextBoxMeterHardwareID.ReadOnly = true;
      this.dataGridTextBoxMeterTypeID.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridTextBoxMeterTypeID, "dataGridTextBoxMeterTypeID");
      this.dataGridTextBoxMeterTypeID.ReadOnly = true;
      this.dataGridTextBoxHardwareTypeID.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridTextBoxHardwareTypeID, "dataGridTextBoxHardwareTypeID");
      this.dataGridTextBoxHardwareTypeID.ReadOnly = true;
      this.dataGridTextBoxDefaultFunction.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridTextBoxDefaultFunction, "dataGridTextBoxDefaultFunction");
      this.dataGridTextBoxDefaultFunction.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.btOk, "btOk");
      this.btOk.DialogResult = DialogResult.OK;
      this.btOk.Name = "btOk";
      this.btOk.Click += new System.EventHandler(this.btOk_Click);
      componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
      this.btCancel.DialogResult = DialogResult.Cancel;
      this.btCancel.Name = "btCancel";
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      componentResourceManager.ApplyResources((object) this.btExtendedSeek, "btExtendedSeek");
      this.btExtendedSeek.Name = "btExtendedSeek";
      this.btExtendedSeek.Click += new System.EventHandler(this.btExtendedSeek_Click);
      componentResourceManager.ApplyResources((object) this.zennerCoroprateDesign1, "zennerCoroprateDesign1");
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      componentResourceManager.ApplyResources((object) this.SelectBaseTypeButton, "SelectBaseTypeButton");
      this.SelectBaseTypeButton.DialogResult = DialogResult.OK;
      this.SelectBaseTypeButton.Name = "SelectBaseTypeButton";
      this.SelectBaseTypeButton.Click += new System.EventHandler(this.SelectBaseTypeButton_Click);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.textBoxMeterinfoDescription);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.dataGridTypeOverWriteParameter);
      this.panel1.Controls.Add((Control) this.textBoxMeterHardware);
      this.panel1.Controls.Add((Control) this.textBoxHardwareType);
      this.panel1.Controls.Add((Control) this.textBoxMeterType);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.textBoxMeterinfoDescription, "textBoxMeterinfoDescription");
      this.textBoxMeterinfoDescription.Name = "textBoxMeterinfoDescription";
      this.textBoxMeterinfoDescription.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      this.dataGridTypeOverWriteParameter.AllowNavigation = false;
      componentResourceManager.ApplyResources((object) this.dataGridTypeOverWriteParameter, "dataGridTypeOverWriteParameter");
      this.dataGridTypeOverWriteParameter.ColumnHeadersVisible = false;
      this.dataGridTypeOverWriteParameter.HeaderForeColor = SystemColors.ControlText;
      this.dataGridTypeOverWriteParameter.Name = "dataGridTypeOverWriteParameter";
      this.dataGridTypeOverWriteParameter.PreferredColumnWidth = 150;
      this.dataGridTypeOverWriteParameter.ReadOnly = true;
      this.dataGridTypeOverWriteParameter.RowHeadersVisible = false;
      this.dataGridTypeOverWriteParameter.TableStyles.AddRange(new DataGridTableStyle[1]
      {
        this.dataGridTableStyle2
      });
      this.dataGridTableStyle2.ColumnHeadersVisible = false;
      this.dataGridTableStyle2.DataGrid = this.dataGridTypeOverWriteParameter;
      this.dataGridTableStyle2.GridColumnStyles.AddRange(new DataGridColumnStyle[1]
      {
        (DataGridColumnStyle) this.dataGridTextBoxParameterValue
      });
      this.dataGridTableStyle2.HeaderForeColor = SystemColors.ControlText;
      this.dataGridTableStyle2.MappingName = "TypeOverwriteParameters";
      componentResourceManager.ApplyResources((object) this.dataGridTableStyle2, "dataGridTableStyle2");
      this.dataGridTableStyle2.ReadOnly = true;
      this.dataGridTextBoxParameterValue.FormatInfo = (IFormatProvider) null;
      componentResourceManager.ApplyResources((object) this.dataGridTextBoxParameterValue, "dataGridTextBoxParameterValue");
      this.dataGridTextBoxParameterValue.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBoxMeterHardware, "textBoxMeterHardware");
      this.textBoxMeterHardware.Name = "textBoxMeterHardware";
      this.textBoxMeterHardware.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBoxHardwareType, "textBoxHardwareType");
      this.textBoxHardwareType.Name = "textBoxHardwareType";
      this.textBoxHardwareType.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBoxMeterType, "textBoxMeterType");
      this.textBoxMeterType.Name = "textBoxMeterType";
      this.textBoxMeterType.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.buttonPrint, "buttonPrint");
      this.buttonPrint.Name = "buttonPrint";
      this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
      this.printDialog.Document = this.printDocument;
      this.printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
      componentResourceManager.ApplyResources((object) this.buttonDeleteType, "buttonDeleteType");
      this.buttonDeleteType.Name = "buttonDeleteType";
      this.buttonDeleteType.Click += new System.EventHandler(this.buttonDeleteType_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.Controls.Add((Control) this.buttonDeleteType);
      this.Controls.Add((Control) this.buttonPrint);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.SelectBaseTypeButton);
      this.Controls.Add((Control) this.btOk);
      this.Controls.Add((Control) this.btCancel);
      this.Controls.Add((Control) this.btExtendedSeek);
      this.Controls.Add((Control) this.btSuchen);
      this.Controls.Add((Control) this.txtbx);
      this.Controls.Add((Control) this.dataGrid1);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Name = nameof (COpenTypeDlg);
      this.Load += new System.EventHandler(this.COpenTypeDlg_Load);
      this.dataGrid1.EndInit();
      this.dtatblMeterInfo.EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.dataGridTypeOverWriteParameter.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initEnviromentOnUser()
    {
      if (UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer))
      {
        this.buttonPrint.Visible = true;
        this.SelectBaseTypeButton.Visible = true;
        this.btExtendedSeek.Visible = true;
      }
      else
      {
        this.buttonPrint.Visible = false;
        this.SelectBaseTypeButton.Visible = false;
        this.btExtendedSeek.Visible = false;
      }
    }

    private void btSuchen_Click(object sender, EventArgs e) => this.SearchType();

    private void SearchType()
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        if (this.txtbx.Text.Length <= 0)
          return;
        string str = "SELECT  MeterInfo.PPSArtikelNr,HardwareType.FirmwareVersion,HardwareType.HardwareName,Meterinfo.Description as MeterInfoDescription,MeterType.Description as MeterTypeDescription,HardwareType.Description as HardwareTypeDescription,MeterHardware.Description as MeterHardwareDescription,MeterType.Typename,MeterInfo.HardwareTypeID,Meterinfo.MeterTypeID,MeterInfo.DefaultFunctionNr,MeterInfo.MeterHardwareID,MeterInfo.MeterInfoID FROM MeterType, HardwareType, MeterInfo, MeterHardware WHERE MeterInfo.PPSArtikelNr LIKE '" + this.txtbx.Text + "'";
        if (!UserRights.GlobalUserRights.CheckRight(UserRights.Rights.Developer))
          str += " AND MeterInfo.MeterInfoID >= 99000";
        if (this.FirmwareVersion != "")
          str = str + " AND HardwareType.FirmwareVersion = " + this.FirmwareVersion;
        string SqlCommand = str + " AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID AND MeterInfo.MeterHardwareID = MeterHardware.MeterHardwareID AND MeterInfo.MeterTypeID = MeterType.MeterTypeID ORDER BY MeterInfo.MeterInfoID DESC";
        this.dtatblMeterInfo.Clear();
        using (IDbConnection dbConnection = primaryDb.GetDbConnection())
        {
          dbConnection.Open();
          primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(this.dtatblMeterInfo);
          dbConnection.Close();
        }
        this.dataGrid1.DataSource = (object) this.dtatblMeterInfo;
        try
        {
          this.dataGrid1.Select(0);
          this.selectTheType();
        }
        catch
        {
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void dtaGrdMenueViewItemMouseUp(object sender, MouseEventArgs e)
    {
      this.selectTheType();
    }

    private void selectTheType()
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      try
      {
        if (this.dtatblMeterInfo.Rows.Count <= 0)
          return;
        DataRow inDtatblMeterInfo = this.getDataRowInDtatblMeterInfo();
        if (inDtatblMeterInfo != null)
        {
          this.m_nMeterInfoID = int.Parse(inDtatblMeterInfo["MeterInfoID"].ToString());
          this.m_nMeterTypeID = int.Parse(inDtatblMeterInfo["MeterTypeID"].ToString());
          this.m_sPPSNr = inDtatblMeterInfo["PPSArtikelNr"].ToString();
          this.textBoxHardwareType.Text = inDtatblMeterInfo["HardwareTypeDescription"].ToString();
          this.textBoxMeterHardware.Text = inDtatblMeterInfo["MeterHardwareDescription"].ToString();
          this.textBoxMeterType.Text = inDtatblMeterInfo["MeterTypeDescription"].ToString();
          this.MeterTypeDescription = this.textBoxMeterType.Text;
          this.textBoxMeterinfoDescription.Text = inDtatblMeterInfo["MeterInfoDescription"].ToString();
          this.tov.Clear();
          string SqlCommand = "Select ParameterValue from TypeOverwriteParameters where MeterInfoID = " + this.m_nMeterInfoID.ToString();
          using (IDbConnection dbConnection = primaryDb.GetDbConnection())
          {
            dbConnection.Open();
            primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(this.tov);
            dbConnection.Close();
          }
          this.dataGridTypeOverWriteParameter.DataSource = (object) this.tov;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void btCancel_Click(object sender, EventArgs e) => this.m_nMeterInfoID = int.MaxValue;

    private void btExtendedSeek_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Funnktion nicht mehr verfügbar!", "Extended seek", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void dataGrid1_DoubleClick(object sender, EventArgs e)
    {
      if (this.dtatblMeterInfo.Rows.Count > 0)
      {
        DataRow inDtatblMeterInfo = this.getDataRowInDtatblMeterInfo();
        this.m_nMeterInfoID = 5;
        if (inDtatblMeterInfo != null)
          this.m_nMeterInfoID = int.Parse(inDtatblMeterInfo["MeterInfoID"].ToString());
      }
      this.DialogResult = DialogResult.OK;
    }

    private DataRow getDataRowInDtatblMeterInfo()
    {
      return ((DataRowView) this.dataGrid1.BindingContext[(object) this.dtatblMeterInfo].Current).Row;
    }

    private void txtbx_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      sender = new object();
      EventArgs e1 = new EventArgs();
      this.btSuchen_Click(sender, e1);
    }

    internal int getBaseTypeMeterInfoID(
      int childMeterInfoID,
      out int baseTypeMeterInfoID,
      out string ErrorMsg)
    {
      return new BaseTypeCheck("", childMeterInfoID).getBaseTypeMeterInfoID(childMeterInfoID, out baseTypeMeterInfoID, out ErrorMsg);
    }

    private void SelectBaseTypeButton_Click(object sender, EventArgs e)
    {
      string ErrorMsg;
      if (this.getBaseTypeMeterInfoID(this.m_nMeterInfoID, out this.m_nMeterInfoID, out ErrorMsg) == 0)
      {
        if (MessageBox.Show("You want to open a BASETYPE!\r\nAttention! A manipulation on this type will change many other client-types!\r\nContinue?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.Close();
      }
      else
      {
        int num = (int) MessageBox.Show(ErrorMsg);
      }
    }

    private void dataGrid1_Navigate(object sender, NavigateEventArgs ne)
    {
    }

    private void buttonPrint_Click(object sender, EventArgs e)
    {
      this.printDocument.PrinterSettings.FromPage = 1;
      this.printDocument.PrinterSettings.ToPage = 1;
      if (this.printDialog.ShowDialog() != DialogResult.OK)
        return;
      this.printDocument.DocumentName = "MeterInfoType";
      this.pageNr = 1;
      this.printDocument.Print();
    }

    private void printDocument_PrintPage(object sender, PrintPageEventArgs ppea)
    {
      Graphics graphics1 = ppea.Graphics;
      Font fntTitle = new Font("Times New Roman", 16f);
      Font font1 = new Font("Times New Roman", 10f);
      Pen pen1 = new Pen(Color.Black);
      Pen pen2 = new Pen(Color.Black);
      bool flag = this.printMeterInfoTypeData(graphics1, fntTitle, ppea);
      Graphics graphics2 = graphics1;
      string s = "Seite " + this.pageNr.ToString();
      Font font2 = font1;
      Brush black1 = Brushes.Black;
      Rectangle pageBounds = ppea.PageBounds;
      double x1 = (double) (pageBounds.Width / 2 - 60);
      pageBounds = ppea.PageBounds;
      double y1 = (double) (pageBounds.Height - 100);
      graphics2.DrawString(s, font2, black1, (float) x1, (float) y1);
      graphics1.DrawString("ZENNER GmbH & Co.KGaA", font1, Brushes.Black, 10f, (float) (ppea.PageBounds.Height - 100));
      SizeF sizeF = graphics1.MeasureString("www.zenner.de", font1);
      Graphics graphics3 = graphics1;
      Font font3 = font1;
      Brush black2 = Brushes.Black;
      Rectangle rectangle = ppea.PageBounds;
      int width = rectangle.Width;
      rectangle = ppea.MarginBounds;
      int left = rectangle.Left;
      double x2 = (double) (width - left) - (double) sizeF.Width;
      rectangle = ppea.PageBounds;
      double y2 = (double) (rectangle.Height - 100);
      graphics3.DrawString("www.zenner.de", font3, black2, (float) x2, (float) y2);
      ++this.pageNr;
      ppea.HasMorePages = flag;
    }

    private bool printMeterInfoTypeData(Graphics grfx, Font fntTitle, PrintPageEventArgs ppea)
    {
      Font font1 = new Font("Times New Roman", 14f);
      Font font2 = new Font("Times New Roman", 8f);
      Point point1;
      ref Point local = ref point1;
      Rectangle rectangle = ppea.PageBounds;
      int width = rectangle.Width;
      rectangle = ppea.MarginBounds;
      int left = rectangle.Left;
      int x = width - left;
      rectangle = ppea.PageBounds;
      int height = rectangle.Height;
      rectangle = ppea.MarginBounds;
      int top = rectangle.Top;
      int y = height - top;
      local = new Point(x, y);
      string[] strArray1 = new string[50];
      Pen pen1 = new Pen(Color.Black);
      Pen pen2 = new Pen(Color.Black);
      string[] strArray2 = new string[10];
      int num1 = 20;
      int num2 = (int) font2.GetHeight() + 2;
      bool flag = false;
      Point point2 = new Point(30, 40);
      grfx.DrawString("MeterType", fntTitle, Brushes.Black, (float) point2.X, 27f - fntTitle.GetHeight());
      point2.Y += num2;
      string s1 = "PPS-Nr : " + this.m_sPPSNr;
      grfx.DrawString(s1, fntTitle, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2 * 2;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string s2 = "Datum : " + dateTime.ToString();
      grfx.DrawString(s2, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2 * 2;
      string s3 = "MeterInfoID : " + this.m_nMeterInfoID.ToString();
      grfx.DrawString(s3, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2 * 2;
      int baseTypeMeterInfoID;
      this.getBaseTypeMeterInfoID(this.m_nMeterInfoID, out baseTypeMeterInfoID, out string _);
      string s4 = "MeterInfo-BASE-ID: " + baseTypeMeterInfoID.ToString();
      grfx.DrawString(s4, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2 * 2;
      string s5 = "MeterInfo-Description : ";
      grfx.DrawString(s5, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2;
      string[] outstr1 = new string[50];
      int strCount;
      this.reformatString(this.textBoxMeterinfoDescription.Text, ref outstr1, out strCount, grfx, font2, grfx.DpiX - (float) point2.X - (float) num1);
      for (int index = 0; index < strCount; ++index)
      {
        grfx.DrawString(outstr1[index], font2, Brushes.Black, (float) (point2.X + num1), (float) point2.Y);
        point2.Y += num2;
      }
      point2.Y += num2 * 2;
      string s6 = "MeterType-Description : ";
      grfx.DrawString(s6, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2;
      string[] outstr2 = new string[50];
      this.reformatString(this.textBoxMeterType.Text, ref outstr2, out strCount, grfx, font2, grfx.DpiX - (float) point2.X - (float) num1);
      for (int index = 0; index < strCount; ++index)
      {
        grfx.DrawString(outstr2[index], font2, Brushes.Black, (float) (point2.X + num1), (float) point2.Y);
        point2.Y += num2;
      }
      point2.Y += num2 * 2;
      string s7 = "HardwareType-Description : ";
      grfx.DrawString(s7, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2;
      string[] outstr3 = new string[50];
      this.reformatString(this.textBoxHardwareType.Text, ref outstr3, out strCount, grfx, font2, grfx.DpiX - (float) point2.X - (float) num1);
      for (int index = 0; index < strCount; ++index)
      {
        grfx.DrawString(outstr3[index], font2, Brushes.Black, (float) (point2.X + num1), (float) point2.Y);
        point2.Y += num2;
      }
      point2.Y += num2 * 2;
      string s8 = "MeterHardware : ";
      grfx.DrawString(s8, font2, Brushes.Black, (float) point2.X, (float) point2.Y);
      point2.Y += num2;
      string[] outstr4 = new string[50];
      this.reformatString(this.textBoxMeterHardware.Text, ref outstr4, out strCount, grfx, font2, grfx.DpiX - (float) point2.X - (float) num1);
      for (int index = 0; index < strCount; ++index)
      {
        grfx.DrawString(outstr4[index], font2, Brushes.Black, (float) (point2.X + num1), (float) point2.Y);
        point2.Y += num2;
      }
      point2.Y += num2 * 4;
      string s9 = "Overwriteparameter:";
      grfx.DrawString(s9, font1, Brushes.Black, (float) point2.X, (float) point2.Y);
      foreach (DataRow row in (InternalDataCollectionBase) this.tov.Rows)
      {
        point2.Y += num2 * 2;
        grfx.DrawString(row["ParameterValue"].ToString(), font2, Brushes.Black, (float) (point2.X + num1), (float) point2.Y);
      }
      return flag;
    }

    private void reformatString(
      string strLine,
      ref string[] outstr,
      out int strCount,
      Graphics grfx,
      Font fntTitle,
      float fWidth)
    {
      SizeF sizeF1 = grfx.MeasureString(strLine, fntTitle);
      strCount = 0;
      if ((double) sizeF1.Width >= (double) fWidth)
      {
        string[] strArray1 = strLine.Split(' ');
        int index1 = 0;
        SizeF sizeF2;
        SizeF sizeF3;
        while (index1 < strArray1.GetLength(0) && strCount < outstr.GetLength(0))
        {
          string text = "";
          bool flag = false;
          while (index1 < strArray1.GetLength(0) && !flag)
          {
            sizeF2 = grfx.MeasureString(strArray1[index1] + " ", fntTitle);
            sizeF3 = grfx.MeasureString(text, fntTitle);
            if ((double) sizeF3.Width + (double) sizeF2.Width > (double) fWidth)
            {
              flag = true;
            }
            else
            {
              text = text + strArray1[index1] + " ";
              ++index1;
            }
          }
          outstr[strCount] = text;
          ++strCount;
        }
        foreach (string text1 in outstr)
        {
          sizeF3 = grfx.MeasureString(text1, fntTitle);
          if ((double) sizeF3.Width >= (double) fWidth)
          {
            string[] strArray2 = strLine.Split(';');
            int index2 = 0;
            while (index2 < strArray2.GetLength(0) && strCount < outstr.GetLength(0))
            {
              string text2 = "";
              bool flag = false;
              while (index2 < strArray2.GetLength(0) && !flag)
              {
                sizeF2 = grfx.MeasureString(strArray2[index2] + " ", fntTitle);
                sizeF3 = grfx.MeasureString(text2, fntTitle);
                if ((double) sizeF3.Width + (double) sizeF2.Width > (double) fWidth)
                {
                  flag = true;
                }
                else
                {
                  text2 = text2 + strArray2[index2] + " ";
                  ++index2;
                }
              }
              outstr[strCount] = text2;
              ++strCount;
            }
          }
        }
      }
      else
      {
        outstr[strCount] = strLine;
        ++strCount;
      }
    }

    private void btOk_Click(object sender, EventArgs e)
    {
      int row1 = 0;
      if (!this.getTypeList)
        return;
      this.TypeList.Clear();
      foreach (DataRow row2 in (InternalDataCollectionBase) this.dtatblMeterInfo.Rows)
      {
        if (this.dataGrid1.IsSelected(row1))
          this.TypeList.Add((object) int.Parse(row2["MeterInfoID"].ToString()));
        ++row1;
      }
    }

    private void COpenTypeDlg_Load(object sender, EventArgs e)
    {
      if (this.txtbx.Text.Trim().Length <= 0)
        return;
      this.SearchType();
    }

    private void buttonDeleteType_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Function not available!");
    }
  }
}
