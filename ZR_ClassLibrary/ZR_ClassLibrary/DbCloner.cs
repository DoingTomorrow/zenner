// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.DbCloner
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ZR_ClassLibrary
{
  public class DbCloner : Form
  {
    private bool CancelTimepointStart;
    private int StundeFuerStartUnterbrechung = 21;
    private int StundeFuerWeiterNachUnterbrechung = 1;
    private string SpecialTableIndex = string.Empty;
    private bool IsSpecialTable = false;
    private string TheStartLine = string.Empty;
    private static SortedList<string, SortedList<string, string>> TableGroups;
    private DbBasis SourceDB;
    private DbBasis TargetDB;
    private string OdbcConnectionString = "DSN=PostgreSQL35W";
    private List<DbCloner.SpecialTableClass> TheSpecialTables;
    private IContainer components = (IContainer) null;
    private Button cmdStartClone;
    private Label lblTables;
    private Label lblDatarows;
    private Label lblDuration;
    private Label lblMinutes;
    private TextBox txtLog;
    private CheckBox chkStructure;
    private CheckBox chkContent;
    private ListBox lstBoxTables;
    private ListBox listBoxSpecialGroup;
    private Button buttonWriteXSD;
    private TextBox textBoxWhereConditions;
    private Label label1;
    private CheckBox checkBoxVacuum;
    private GroupBox groupBox2;
    private ComboBox comboBoxSingleTableUpdate;
    private GroupBox groupBox3;
    private Label label2;
    private GroupBox groupBox5;
    private GroupBox groupBox4;
    private CheckBox checkBoxUpdateSingleTable;
    private ProgressBar progressBarCloner;
    private Label labelDatabaseName;
    private Label labelDatabaseLocation;
    private Label label3;
    private Label label4;
    private Button buttonStartSingleTableUpdate;
    private GroupBox groupBox1;
    private TextBox TableLog;
    private CheckBox checkBoxUseOdbcConnection;
    private DateTimePicker StartTimePicker;
    private Label labelTimepoint;
    private CheckBox checkBoxEnableStartTime;
    private Button buttonCancelTimePointStart;
    private Button buttonSetAll;
    private Button buttonClearAll;

    public DbCloner() => this.InitializeComponent();

    public bool Init()
    {
      CheckBox useOdbcConnection = this.checkBoxUseOdbcConnection;
      useOdbcConnection.Text = useOdbcConnection.Text + " (" + this.OdbcConnectionString + ")";
      this.TheSpecialTables = new List<DbCloner.SpecialTableClass>();
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("Meter", 3000, "MeterID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("PartList", 3000, "MeterID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("MeterData", 1000, "MeterID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("Test", 3000, "TestID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("TestEquipment", 3000, "TestID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("TestResult", 3000, "TestResultID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("TestResultExtended", 3000, "TestResultID"));
      this.TheSpecialTables.Add(new DbCloner.SpecialTableClass("TestbenchActualValues", 3000, "TestID"));
      this.textBoxWhereConditions.Text = string.Empty;
      DbCloner.TableGroups = new SortedList<string, SortedList<string, string>>();
      DbCloner.TableGroups.Add("BaseInfos", new SortedList<string, string>()
      {
        {
          "MeterInfo",
          "MeterInfoID < 50000"
        },
        {
          "MeterType",
          "MeterTypeID < 50000"
        },
        {
          "MTypeZelsius",
          "MeterTypeID < 50000"
        }
      });
      DbCloner.TableGroups.Add("Handler", new SortedList<string, string>()
      {
        {
          "ZRFunction",
          ""
        },
        {
          "ZRFunctionCompiled",
          ""
        },
        {
          "Menu",
          ""
        },
        {
          "DisplayCode",
          ""
        },
        {
          "RuntimeCode",
          ""
        },
        {
          "MBusCode",
          ""
        },
        {
          "Code",
          ""
        },
        {
          "ZRParameter",
          ""
        },
        {
          "Datalogger",
          ""
        },
        {
          "MinolDeviceData",
          ""
        },
        {
          "S3_FunctionParameter",
          ""
        },
        {
          "S3_Parameter",
          ""
        },
        {
          "OnlineTranslation",
          ""
        },
        {
          "EnumTranslation",
          ""
        }
      });
      DbCloner.TableGroups.Add("Hardware", new SortedList<string, string>()
      {
        {
          "BlockNames",
          ""
        },
        {
          "HardwareType",
          ""
        },
        {
          "MeterHardware",
          ""
        },
        {
          "LinkerTable",
          ""
        },
        {
          "MapBase",
          ""
        },
        {
          "MapDef",
          ""
        },
        {
          "IncludeDef",
          ""
        },
        {
          "ProgFiles",
          ""
        }
      });
      DbCloner.TableGroups.Add("SmokeDetector", new SortedList<string, string>()
      {
        {
          "MeterInfo",
          "MeterInfoID < 50000"
        },
        {
          "HardwareType",
          ""
        }
      });
      DbCloner.TableGroups.Add("ReadoutConfiguration", new SortedList<string, string>()
      {
        {
          "ConnectionItems",
          ""
        },
        {
          "ConnectionItemParameters",
          ""
        },
        {
          "ConnectionProfiles",
          ""
        },
        {
          "ConnectionProfileParameters",
          ""
        },
        {
          "ConnectionProfileFilters",
          ""
        },
        {
          "ConnectionSettings",
          ""
        },
        {
          "ChangeableParameters",
          ""
        },
        {
          "GmmImages",
          ""
        }
      });
      DataSet dataSet = (DataSet) new Schema();
      SortedList<string, string> sortedList = new SortedList<string, string>();
      foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
        sortedList.Add(table.TableName, (string) null);
      this.lstBoxTables.Items.Clear();
      foreach (object key in (IEnumerable<string>) sortedList.Keys)
        this.lstBoxTables.Items.Add(key);
      this.listBoxSpecialGroup.Items.Clear();
      for (int index = 0; index < DbCloner.TableGroups.Count; ++index)
        this.listBoxSpecialGroup.Items.Add((object) DbCloner.TableGroups.Keys[index]);
      try
      {
        if (Datenbankverbindung.MainDBAccess.GetDatabaseInfo("") == Datenbankverbindung.SecDBAccess.GetDatabaseInfo(""))
        {
          int num = (int) MessageBox.Show("The source and target database are the same. Please check you connection settings.");
          this.Close();
        }
        this.SourceDB = DbBasis.PrimaryDB;
        this.TargetDB = DbBasis.SecondaryDB;
      }
      catch
      {
        int num = (int) MessageBox.Show("Faild to connect to you database. Please check you connection settings.");
        return false;
      }
      this.labelDatabaseName.Text = "Database Name: " + this.TargetDB.GetDbConnection().Database;
      Schema.DatabaseIdentificationDataTable identificationDataTable = new Schema.DatabaseIdentificationDataTable();
      try
      {
        this.TargetDB.ZRDataAdapter("SELECT * FROM DatabaseIdentification", this.TargetDB.GetDbConnection()).Fill((DataTable) identificationDataTable);
      }
      catch
      {
      }
      if (identificationDataTable.Rows.Count == 0)
      {
        int num = (int) MessageBox.Show("There is no information about the target database available.");
        return false;
      }
      this.labelDatabaseLocation.Text = identificationDataTable.FindByInfoName("DatabaseLocationName").InfoData;
      this.labelDatabaseName.Text = identificationDataTable.FindByInfoName("DatabaseName").InfoData;
      return true;
    }

    private void cmdStartClone_Click(object sender, EventArgs e)
    {
      DateTime now1 = DateTime.Now;
      this.CancelTimepointStart = false;
      DateTime dateTime = this.StartTimePicker.Value;
      bool flag1 = this.checkBoxEnableStartTime.CheckState == CheckState.Checked;
      long num1 = 0;
      this.txtLog.Text = string.Empty;
      this.TableLog.Text = string.Empty;
      this.buttonWriteXSD.Enabled = false;
      this.cmdStartClone.Enabled = false;
      DateTime now2 = DateTime.Now;
      this.TheStartLine = "[" + now2.ToLongTimeString() + "] Process started.";
      this.AppendLogText(this.TheStartLine);
      this.AppendTableLogText(this.TheStartLine);
      bool flag2 = this.checkBoxUseOdbcConnection.CheckState == CheckState.Checked;
      if (flag1)
      {
        int num2 = 0;
        DateTime now3;
        do
        {
          now3 = DateTime.Now;
          ++num2;
          if (num2 == 10)
          {
            this.AppendLogText("Waiting for Starttime point! ActualTime = " + now3.ToLongTimeString());
            num2 = 0;
            this.Refresh();
          }
          Application.DoEvents();
          Thread.Sleep(100);
        }
        while (!(now3 >= dateTime) && !this.CancelTimepointStart);
      }
      if (!this.CancelTimepointStart)
      {
        this.Cursor = Cursors.WaitCursor;
        this.txtLog.ForeColor = Color.Black;
        Application.DoEvents();
        List<string> Tables = new List<string>();
        List<string> stringList1 = new List<string>();
        foreach (object selectedItem in this.lstBoxTables.SelectedItems)
        {
          Tables.Add(selectedItem.ToString());
          stringList1.Add(selectedItem.ToString());
        }
        List<string> stringList2 = new List<string>();
        for (int index = 0; index < this.textBoxWhereConditions.Lines.Length; ++index)
        {
          string[] strArray = this.textBoxWhereConditions.Lines[index].Split(';');
          Tables.Remove(strArray[0]);
          stringList2.Add(strArray[0]);
        }
        if (this.chkStructure.Checked)
        {
          if (Tables.Count > 0 && !this.TargetDB.CreateTableStructure(Tables))
          {
            this.txtLog.ForeColor = Color.Red;
            this.AppendLogText("[" + DateTime.Now.ToLongTimeString() + "] Cloning failed. Could not create Tables.");
            Application.DoEvents();
            goto label_120;
          }
          else
          {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Tables droped and new created:");
            foreach (string str in Tables)
              stringBuilder.AppendLine(" -> " + str);
            this.AppendLogText(stringBuilder.ToString());
            Application.DoEvents();
          }
        }
        DataSet dataSet = (DataSet) new Schema();
        using (IDbConnection dbConnection1 = this.SourceDB.GetDbConnection())
        {
          using (IDbConnection dbConnection2 = this.TargetDB.GetDbConnection())
          {
            dbConnection1.Open();
            dbConnection2.Open();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
            {
              if (stringList1.Contains(table.TableName))
              {
                bool flag3 = true;
                ZRDataAdapter zrDataAdapter = this.SourceDB.ZRDataAdapter("Select * from " + table.TableName + " where (1 = 2)", dbConnection1);
                DataTable dataTable = new DataTable();
                try
                {
                  zrDataAdapter.Fill(dataTable);
                }
                catch
                {
                  stringBuilder.Append("Tabelle: " + table.TableName + ": Konnte nicht geöffnet werden!" + Environment.NewLine);
                  flag3 = false;
                }
                if (flag3 && dataTable.Columns.Count != table.Columns.Count)
                {
                  stringBuilder.Append("Tabelle: " + table.TableName + ": Spaltenzahl stimmt nicht!" + Environment.NewLine);
                  flag3 = false;
                }
                if (flag3)
                {
                  for (int index = 0; index < table.Columns.Count; ++index)
                  {
                    if (table.Columns[index].ColumnName.Trim().ToUpper() != dataTable.Columns[index].ColumnName.Trim().ToUpper())
                    {
                      stringBuilder.Append("Tabelle: " + table.TableName + ": Spalten Reihenfolge stimmt nicht! (" + table.Columns[index].ColumnName.Trim() + ")" + Environment.NewLine);
                      flag3 = false;
                      break;
                    }
                  }
                }
                if (!flag3)
                {
                  if (Tables.Contains(table.TableName))
                  {
                    Tables.Remove(table.TableName);
                    break;
                  }
                  if (stringList2.Contains(table.TableName))
                  {
                    stringList2.Remove(table.TableName);
                    break;
                  }
                  break;
                }
              }
            }
            this.AppendTableLogText(stringBuilder.ToString());
            try
            {
              foreach (DbCloner.SpecialTableClass theSpecialTable in this.TheSpecialTables)
              {
                string tableName = theSpecialTable.TableName;
                if (Tables.Contains(tableName) & flag2)
                {
                  DateTime now4 = DateTime.Now;
                  this.AppendTableLogText("[" + now4.ToLongTimeString() + "] Start table (ODBC): " + tableName);
                  string format = "Delete from " + tableName;
                  string cmdText = "Select * from " + tableName;
                  string SqlCommand = "Select * from " + tableName + " where (1 = 2)";
                  using (OdbcConnection connection = new OdbcConnection(this.OdbcConnectionString))
                  {
                    IDbCommand dbCommand = this.TargetDB.DbCommand(dbConnection2);
                    dbCommand.CommandTimeout = 600;
                    dbCommand.CommandText = string.Format(format);
                    dbCommand.ExecuteNonQuery();
                    OdbcCommand odbcCommand = new OdbcCommand(cmdText, connection);
                    connection.Open();
                    OdbcDataReader odbcDataReader = odbcCommand.ExecuteReader();
                    ZRDataAdapter zrDataAdapter = this.TargetDB.ZRDataAdapter(SqlCommand, dbConnection2);
                    DataTable dataTable = new DataTable();
                    zrDataAdapter.Fill(dataTable);
                    int num3 = 0;
                    while (odbcDataReader.Read())
                    {
                      DataRow row = dataTable.NewRow();
                      for (int index = 0; index < odbcDataReader.FieldCount; ++index)
                        row[index] = odbcDataReader.GetValue(index);
                      dataTable.Rows.Add(row);
                      ++num3;
                      if (num3 % theSpecialTable.StepValue == 0)
                      {
                        zrDataAdapter.Update(dataTable);
                        string[] strArray = new string[7];
                        strArray[0] = "[";
                        now4 = DateTime.Now;
                        strArray[1] = now4.ToLongTimeString();
                        strArray[2] = "]";
                        strArray[3] = tableName;
                        strArray[4] = " -> ";
                        strArray[5] = num3.ToString();
                        strArray[6] = " rows";
                        this.AppendLogText(string.Concat(strArray));
                        Application.DoEvents();
                        this.Refresh();
                        dataTable.Clear();
                        this.UnterbrechnugWennNoetig();
                      }
                    }
                    zrDataAdapter.Update(dataTable);
                    odbcDataReader.Close();
                    string[] strArray1 = new string[7];
                    strArray1[0] = "[";
                    now4 = DateTime.Now;
                    strArray1[1] = now4.ToLongTimeString();
                    strArray1[2] = "] ";
                    strArray1[3] = tableName;
                    strArray1[4] = " Finished ";
                    strArray1[5] = num3.ToString();
                    strArray1[6] = " rows";
                    this.AppendLogText(string.Concat(strArray1));
                    string[] strArray2 = new string[7];
                    strArray2[0] = "[";
                    now4 = DateTime.Now;
                    strArray2[1] = now4.ToLongTimeString();
                    strArray2[2] = "] ";
                    strArray2[3] = tableName;
                    strArray2[4] = " Finished ";
                    strArray2[5] = num3.ToString();
                    strArray2[6] = " rows";
                    this.AppendTableLogText(string.Concat(strArray2));
                    this.Refresh();
                  }
                }
              }
            }
            catch (Exception ex)
            {
              this.AppendTableLogText(ex.ToString());
              goto label_120;
            }
            IDbCommand dbCommand1 = this.SourceDB.DbCommand(dbConnection1);
            dbCommand1.CommandTimeout = 600;
            if (this.chkContent.Checked)
            {
              this.progressBarCloner.Visible = true;
              this.progressBarCloner.Value = 0;
              this.progressBarCloner.Minimum = 0;
              this.progressBarCloner.Maximum = stringList2.Count + Tables.Count;
              foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
              {
                if (Tables.IndexOf(table.TableName) != -1 || stringList2.IndexOf(table.TableName) != -1)
                {
                  this.IsSpecialTable = false;
                  foreach (DbCloner.SpecialTableClass theSpecialTable in this.TheSpecialTables)
                  {
                    if (theSpecialTable.TableName.ToUpper().Trim() == table.TableName.ToUpper().Trim())
                    {
                      this.IsSpecialTable = true;
                      break;
                    }
                  }
                  if (!this.IsSpecialTable)
                  {
                    ++this.progressBarCloner.Value;
                    string str;
                    if (stringList2.IndexOf(table.TableName) != -1)
                    {
                      string[] strArray = this.textBoxWhereConditions.Lines[stringList2.IndexOf(table.TableName)].Split(';');
                      str = string.Format("SELECT * FROM {0} WHERE {1}", (object) table.TableName, (object) strArray[1]);
                      try
                      {
                        IDbCommand dbCommand2 = this.TargetDB.DbCommand(dbConnection2);
                        dbCommand2.CommandTimeout = 600;
                        dbCommand2.CommandText = string.Format("DELETE FROM {0} WHERE {1}", (object) table.TableName, (object) strArray[1]);
                        this.AppendLogText("Delete rows from table " + table.TableName + " conditions: " + strArray[1]);
                        dbCommand2.ExecuteNonQuery();
                      }
                      catch (Exception ex)
                      {
                        this.AppendLogText(ex.Message.ToString());
                      }
                    }
                    else
                    {
                      str = string.Format("SELECT * FROM {0}", (object) table.TableName);
                      if (!this.chkStructure.Checked)
                      {
                        try
                        {
                          IDbCommand dbCommand3 = this.TargetDB.DbCommand(dbConnection2);
                          dbCommand3.CommandTimeout = 600;
                          dbCommand3.CommandText = string.Format("DELETE FROM {0}", (object) table.TableName);
                          this.AppendLogText("Delete all rows from table " + table.TableName);
                          dbCommand3.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                          this.AppendLogText(ex.Message.ToString());
                        }
                      }
                    }
                    dbCommand1.CommandText = str;
                    try
                    {
                      DateTime now5 = DateTime.Now;
                      this.AppendTableLogText("[" + now5.ToLongTimeString() + "] Start Working on Table: " + table.TableName);
                      dbCommand1.CommandText = str;
                      IDataReader dataReader = dbCommand1.ExecuteReader();
                      if (dataReader.Read())
                      {
                        now5 = DateTime.Now;
                        this.AppendLogText("[" + now5.ToLongTimeString() + "] Working on Table: " + table.TableName);
                        Application.DoEvents();
                        ZRDataAdapter zrDataAdapter = this.TargetDB.ZRDataAdapter("Select * from " + table.TableName + " where (1 = 2)", dbConnection2);
                        DataTable dataTable = new DataTable();
                        zrDataAdapter.Fill(dataTable);
                        do
                        {
                          try
                          {
                            DataRow row = dataTable.NewRow();
                            for (int index = 0; index < dataReader.FieldCount; ++index)
                            {
                              dataReader.GetName(index);
                              dataReader.GetValue(index).GetType();
                              row[index] = dataReader.GetValue(index);
                              row[index].GetType();
                            }
                            dataTable.Rows.Add(row);
                            if (dataTable.Rows.Count % 1000 == 0)
                            {
                              zrDataAdapter.Update(dataTable);
                              this.AppendLogText("[" + DateTime.Now.ToLongTimeString() + "] " + dataTable.Rows.Count.ToString() + " rows on Table " + table.TableName + " copied.");
                              Application.DoEvents();
                            }
                          }
                          catch (Exception ex)
                          {
                            this.txtLog.ForeColor = Color.Red;
                            this.AppendLogText(ex.Message.ToString());
                            goto label_120;
                          }
                          ++num1;
                        }
                        while (dataReader.Read());
                        try
                        {
                          zrDataAdapter.Update(dataTable);
                          DateTime now6 = DateTime.Now;
                          this.AppendLogText("[" + now6.ToLongTimeString() + "] Table Finished " + num1.ToString() + " rows on Table " + table.TableName + ".");
                          this.AppendTableLogText("[" + now6.ToLongTimeString() + "] Table Finished " + num1.ToString() + " rows on Table " + table.TableName + ".");
                          Application.DoEvents();
                        }
                        catch (Exception ex)
                        {
                          this.txtLog.ForeColor = Color.Red;
                          this.AppendLogText(ex.Message.ToString());
                          break;
                        }
                      }
                      dataReader.Close();
                      this.UnterbrechnugWennNoetig();
                      if (!this.checkBoxVacuum.Checked)
                        ;
                    }
                    catch (Exception ex)
                    {
                      this.AppendLogText(ex.Message.ToString());
                      if (!ex.Message.Contains("-204"))
                      {
                        if (MessageBox.Show("Es trat folgender Fehler auf:" + Environment.NewLine + ex.Message, "Fehler beim Clonen", MessageBoxButtons.AbortRetryIgnore) == DialogResult.Abort)
                          break;
                      }
                    }
                    num1 = 0L;
                  }
                }
              }
            }
          }
        }
      }
label_120:
      this.progressBarCloner.Visible = false;
      this.progressBarCloner.Value = 0;
      DateTime now7 = DateTime.Now;
      this.AppendLogText("[" + now7.ToLongTimeString() + "] Process finished. ");
      TimeSpan timeSpan = now7 - now2;
      string[] strArray3 = new string[5]
      {
        "Cloning took ",
        null,
        null,
        null,
        null
      };
      int num4 = timeSpan.Hours;
      strArray3[1] = num4.ToString();
      strArray3[2] = " hours, ";
      num4 = timeSpan.Minutes;
      strArray3[3] = num4.ToString();
      strArray3[4] = " minutes";
      this.AppendLogText(string.Concat(strArray3));
      this.cmdStartClone.Enabled = true;
      this.buttonWriteXSD.Enabled = true;
      this.Cursor = Cursors.Default;
    }

    private void UnterbrechnugWennNoetig()
    {
      if (DateTime.Now.Hour < this.StundeFuerStartUnterbrechung)
        return;
      int num = 0;
      DateTime now;
      do
      {
        if (num == 0)
        {
          this.AppendLogText("[" + DateTime.Now.ToLongTimeString() + "] Unterbrechung ist aktiviert!");
          this.Refresh();
        }
        ++num;
        if (num > 100)
          num = 0;
        now = DateTime.Now;
        Application.DoEvents();
        Thread.Sleep(1000);
      }
      while (now.Hour != this.StundeFuerWeiterNachUnterbrechung);
    }

    private void AppendLogText(string TheLogText)
    {
      if (this.txtLog.TextLength + TheLogText.Length + this.TheStartLine.Length + 10 > this.txtLog.MaxLength)
        this.txtLog.Text = this.TheStartLine;
      if (this.txtLog.Text == string.Empty)
        this.txtLog.Text = TheLogText;
      else
        this.txtLog.AppendText(Environment.NewLine + TheLogText);
    }

    private void AppendTableLogText(string TheLogText)
    {
      if (this.TableLog.TextLength + TheLogText.Length > this.TableLog.MaxLength)
        this.TableLog.Text = string.Empty;
      if (this.TableLog.Text == string.Empty)
        this.TableLog.Text = TheLogText;
      else
        this.TableLog.AppendText(Environment.NewLine + TheLogText);
    }

    private void chkContent_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void buttonWriteXSD_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "xsd files (*.xsd)|*.xsd|All files (*.*)|*.*";
      saveFileDialog.FilterIndex = 1;
      saveFileDialog.RestoreDirectory = true;
      string fileName;
      if (saveFileDialog.ShowDialog() != DialogResult.OK || !((fileName = saveFileDialog.FileName) != ""))
        return;
      new Schema().WriteXmlSchema(fileName);
    }

    private void listBoxSpecialGroup_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.lstBoxTables.Items.Count; ++index)
        this.lstBoxTables.SetSelected(index, false);
      SortedList<string, string> tableGroup = DbCloner.TableGroups[this.listBoxSpecialGroup.Text];
      this.textBoxWhereConditions.Text = "";
      for (int index1 = 0; index1 < tableGroup.Count; ++index1)
      {
        if (tableGroup.Values[index1].ToString() != "")
        {
          TextBox boxWhereConditions = this.textBoxWhereConditions;
          boxWhereConditions.Text = boxWhereConditions.Text + tableGroup.Keys[index1].ToString() + ";" + tableGroup.Values[index1].ToString() + Environment.NewLine;
        }
        for (int index2 = 0; index2 < this.lstBoxTables.Items.Count; ++index2)
        {
          if (tableGroup.Keys[index1].ToString() == (string) this.lstBoxTables.Items[index2])
            this.lstBoxTables.SetSelected(index2, true);
        }
      }
    }

    private void checkBoxUpdateSingleTable_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxUpdateSingleTable.Checked)
      {
        this.chkContent.Checked = false;
        this.chkStructure.Checked = false;
        this.comboBoxSingleTableUpdate.Enabled = true;
        this.cmdStartClone.Enabled = false;
        if (this.comboBoxSingleTableUpdate.SelectedIndex != -1)
          this.buttonStartSingleTableUpdate.Enabled = true;
        else
          this.buttonStartSingleTableUpdate.Enabled = false;
      }
      else
      {
        this.chkStructure.Checked = true;
        this.comboBoxSingleTableUpdate.Enabled = false;
        this.cmdStartClone.Enabled = true;
        this.buttonStartSingleTableUpdate.Enabled = false;
      }
    }

    private void comboBoxSingleTableUpdate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.comboBoxSingleTableUpdate.SelectedIndex != -1)
        this.buttonStartSingleTableUpdate.Enabled = true;
      else
        this.buttonStartSingleTableUpdate.Enabled = false;
    }

    private void buttonStartSingleTableUpdate_Click(object sender, EventArgs e)
    {
      if (this.comboBoxSingleTableUpdate.SelectedItem == null)
        return;
      this.StartSingleTableUpdate(this.comboBoxSingleTableUpdate.SelectedItem.ToString());
    }

    internal void StartSingleTableUpdate(string TableName)
    {
      MeterDBAccess mainDbAccess = Datenbankverbindung.MainDBAccess;
      if (TableName == "ZRGlobalID")
      {
        Schema.ZRGlobalIDDataTable outTab = new Schema.ZRGlobalIDDataTable();
        Schema.ZRGlobalIDDataTable MyDataTable = new Schema.ZRGlobalIDDataTable();
        mainDbAccess.FillTable("Select * from zrglobalid", (DataTable) outTab, out string _);
        ZRDataAdapter zrDataAdapter = this.TargetDB.ZRDataAdapter("Select * from zrglobalid", this.TargetDB.GetDbConnection());
        zrDataAdapter.Fill((DataTable) MyDataTable);
        try
        {
          foreach (Schema.ZRGlobalIDRow row in (TypedTableBase<Schema.ZRGlobalIDRow>) outTab)
          {
            if (!(row.DatabaseLocationName != this.labelDatabaseLocation.Text))
            {
              Schema.ZRGlobalIDRow locationNameZrTableName = MyDataTable.FindByDatabaseLocationNameZRTableName(this.labelDatabaseLocation.Text, row.ZRTableName);
              if (locationNameZrTableName == null)
              {
                row.SetAdded();
                MyDataTable.ImportRow((DataRow) row);
              }
              else
              {
                locationNameZrTableName.ZRFirstNr = row.ZRFirstNr;
                locationNameZrTableName.ZRLastNr = row.ZRLastNr;
              }
            }
          }
          int num = (int) MessageBox.Show(zrDataAdapter.Update((DataTable) MyDataTable).ToString() + " rows have been updated");
        }
        catch
        {
          int num = (int) MessageBox.Show("There was an error while updating that table.");
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("There is no rule for this table implemented.");
      }
    }

    private void buttonCancelTimePointStart_Click(object sender, EventArgs e)
    {
      this.CancelTimepointStart = true;
    }

    private void lstBoxTables_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      for (int index = 0; index < this.lstBoxTables.Items.Count; ++index)
        this.lstBoxTables.SelectedIndex = index;
    }

    private void buttonClearAll_Click(object sender, EventArgs e)
    {
      this.lstBoxTables.SelectedItems.Clear();
      this.listBoxSpecialGroup.SelectedItems.Clear();
    }

    private void buttonSetAll_Click(object sender, EventArgs e)
    {
      this.lstBoxTables.SelectedItems.Clear();
      for (int index = 0; index < this.lstBoxTables.Items.Count; ++index)
        this.lstBoxTables.SelectedItems.Add(this.lstBoxTables.Items[index]);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DbCloner));
      this.cmdStartClone = new Button();
      this.lblTables = new Label();
      this.lblDatarows = new Label();
      this.lblDuration = new Label();
      this.lblMinutes = new Label();
      this.txtLog = new TextBox();
      this.chkStructure = new CheckBox();
      this.chkContent = new CheckBox();
      this.lstBoxTables = new ListBox();
      this.listBoxSpecialGroup = new ListBox();
      this.buttonWriteXSD = new Button();
      this.textBoxWhereConditions = new TextBox();
      this.label1 = new Label();
      this.checkBoxVacuum = new CheckBox();
      this.groupBox2 = new GroupBox();
      this.comboBoxSingleTableUpdate = new ComboBox();
      this.groupBox3 = new GroupBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.labelDatabaseName = new Label();
      this.labelDatabaseLocation = new Label();
      this.label2 = new Label();
      this.checkBoxUpdateSingleTable = new CheckBox();
      this.groupBox4 = new GroupBox();
      this.buttonCancelTimePointStart = new Button();
      this.StartTimePicker = new DateTimePicker();
      this.labelTimepoint = new Label();
      this.checkBoxEnableStartTime = new CheckBox();
      this.checkBoxUseOdbcConnection = new CheckBox();
      this.groupBox5 = new GroupBox();
      this.buttonSetAll = new Button();
      this.buttonClearAll = new Button();
      this.buttonStartSingleTableUpdate = new Button();
      this.progressBarCloner = new ProgressBar();
      this.groupBox1 = new GroupBox();
      this.TableLog = new TextBox();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.cmdStartClone.ForeColor = SystemColors.ControlText;
      this.cmdStartClone.Location = new Point(379, 420);
      this.cmdStartClone.Name = "cmdStartClone";
      this.cmdStartClone.Size = new Size(79, 22);
      this.cmdStartClone.TabIndex = 0;
      this.cmdStartClone.Text = "Start Cloning";
      this.cmdStartClone.UseVisualStyleBackColor = true;
      this.cmdStartClone.Click += new System.EventHandler(this.cmdStartClone_Click);
      this.lblTables.AutoSize = true;
      this.lblTables.Location = new Point(132, 384);
      this.lblTables.Name = "lblTables";
      this.lblTables.Size = new Size(0, 13);
      this.lblTables.TabIndex = 6;
      this.lblDatarows.AutoSize = true;
      this.lblDatarows.Location = new Point(409, 606);
      this.lblDatarows.Name = "lblDatarows";
      this.lblDatarows.Size = new Size(0, 13);
      this.lblDatarows.TabIndex = 9;
      this.lblDuration.AutoSize = true;
      this.lblDuration.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblDuration.Location = new Point(130, 810);
      this.lblDuration.Name = "lblDuration";
      this.lblDuration.Size = new Size(101, 20);
      this.lblDuration.TabIndex = 12;
      this.lblDuration.Text = "Cloning took.";
      this.lblDuration.Visible = false;
      this.lblMinutes.AutoSize = true;
      this.lblMinutes.Location = new Point(388, 619);
      this.lblMinutes.Name = "lblMinutes";
      this.lblMinutes.Size = new Size(55, 13);
      this.lblMinutes.TabIndex = 13;
      this.lblMinutes.Text = "0 minutes.";
      this.lblMinutes.Visible = false;
      this.txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLog.ForeColor = SystemColors.ControlText;
      this.txtLog.Location = new Point(6, 19);
      this.txtLog.Multiline = true;
      this.txtLog.Name = "txtLog";
      this.txtLog.ScrollBars = ScrollBars.Vertical;
      this.txtLog.Size = new Size(546, 274);
      this.txtLog.TabIndex = 14;
      this.chkStructure.AutoSize = true;
      this.chkStructure.Location = new Point(7, 19);
      this.chkStructure.Name = "chkStructure";
      this.chkStructure.Size = new Size(345, 17);
      this.chkStructure.TabIndex = 15;
      this.chkStructure.Text = "Table Structure ( Create  new tables. Only tables without conditions)";
      this.chkStructure.UseVisualStyleBackColor = true;
      this.chkContent.AutoSize = true;
      this.chkContent.Checked = true;
      this.chkContent.CheckState = CheckState.Checked;
      this.chkContent.Location = new Point(7, 36);
      this.chkContent.Name = "chkContent";
      this.chkContent.Size = new Size(205, 17);
      this.chkContent.TabIndex = 16;
      this.chkContent.Text = "Table Content (Delete and copy rows)";
      this.chkContent.UseVisualStyleBackColor = true;
      this.chkContent.CheckedChanged += new System.EventHandler(this.chkContent_CheckedChanged);
      this.lstBoxTables.FormattingEnabled = true;
      this.lstBoxTables.Location = new Point(7, 105);
      this.lstBoxTables.Name = "lstBoxTables";
      this.lstBoxTables.SelectionMode = SelectionMode.MultiSimple;
      this.lstBoxTables.Size = new Size(227, 121);
      this.lstBoxTables.TabIndex = 17;
      this.lstBoxTables.MouseDoubleClick += new MouseEventHandler(this.lstBoxTables_MouseDoubleClick);
      this.listBoxSpecialGroup.FormattingEnabled = true;
      this.listBoxSpecialGroup.Location = new Point(240, 105);
      this.listBoxSpecialGroup.Name = "listBoxSpecialGroup";
      this.listBoxSpecialGroup.Size = new Size(219, 121);
      this.listBoxSpecialGroup.TabIndex = 20;
      this.listBoxSpecialGroup.Click += new System.EventHandler(this.listBoxSpecialGroup_Click);
      this.buttonWriteXSD.ForeColor = SystemColors.ControlText;
      this.buttonWriteXSD.Location = new Point(35, 595);
      this.buttonWriteXSD.Name = "buttonWriteXSD";
      this.buttonWriteXSD.Size = new Size(170, 22);
      this.buttonWriteXSD.TabIndex = 22;
      this.buttonWriteXSD.Text = "Write Source Schema to File";
      this.buttonWriteXSD.UseVisualStyleBackColor = true;
      this.buttonWriteXSD.Click += new System.EventHandler(this.buttonWriteXSD_Click);
      this.textBoxWhereConditions.Location = new Point(6, 340);
      this.textBoxWhereConditions.Multiline = true;
      this.textBoxWhereConditions.Name = "textBoxWhereConditions";
      this.textBoxWhereConditions.Size = new Size(452, 77);
      this.textBoxWhereConditions.TabIndex = 23;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(6, 324);
      this.label1.Name = "label1";
      this.label1.Size = new Size(388, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Define special WHERE Conditions below (TABLENAME; WHERE CONDITION):";
      this.checkBoxVacuum.AutoSize = true;
      this.checkBoxVacuum.Location = new Point(7, 19);
      this.checkBoxVacuum.Name = "checkBoxVacuum";
      this.checkBoxVacuum.Size = new Size(95, 17);
      this.checkBoxVacuum.TabIndex = 26;
      this.checkBoxVacuum.Text = "Vaccum Table";
      this.checkBoxVacuum.UseVisualStyleBackColor = true;
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.txtLog);
      this.groupBox2.Location = new Point(499, 11);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(558, 299);
      this.groupBox2.TabIndex = 28;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Zeilen Status:";
      this.comboBoxSingleTableUpdate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxSingleTableUpdate.FormattingEnabled = true;
      this.comboBoxSingleTableUpdate.Items.AddRange(new object[1]
      {
        (object) "ZRGlobalID"
      });
      this.comboBoxSingleTableUpdate.Location = new Point(140, 448);
      this.comboBoxSingleTableUpdate.Name = "comboBoxSingleTableUpdate";
      this.comboBoxSingleTableUpdate.Size = new Size(233, 21);
      this.comboBoxSingleTableUpdate.TabIndex = 32;
      this.comboBoxSingleTableUpdate.SelectedIndexChanged += new System.EventHandler(this.comboBoxSingleTableUpdate_SelectedIndexChanged);
      this.groupBox3.Controls.Add((Control) this.label3);
      this.groupBox3.Controls.Add((Control) this.label4);
      this.groupBox3.Controls.Add((Control) this.labelDatabaseName);
      this.groupBox3.Controls.Add((Control) this.labelDatabaseLocation);
      this.groupBox3.Location = new Point(12, 12);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(469, 58);
      this.groupBox3.TabIndex = 29;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Database Info (Target DB):";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 35);
      this.label3.Name = "label3";
      this.label3.Size = new Size(87, 13);
      this.label3.TabIndex = 38;
      this.label3.Text = "Database Name:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 22);
      this.label4.Name = "label4";
      this.label4.Size = new Size(100, 13);
      this.label4.TabIndex = 37;
      this.label4.Text = "Database Location:";
      this.labelDatabaseName.AutoSize = true;
      this.labelDatabaseName.Location = new Point(142, 35);
      this.labelDatabaseName.Name = "labelDatabaseName";
      this.labelDatabaseName.Size = new Size(87, 13);
      this.labelDatabaseName.TabIndex = 36;
      this.labelDatabaseName.Text = "Database Name:";
      this.labelDatabaseLocation.AutoSize = true;
      this.labelDatabaseLocation.Location = new Point(142, 22);
      this.labelDatabaseLocation.Name = "labelDatabaseLocation";
      this.labelDatabaseLocation.Size = new Size(100, 13);
      this.labelDatabaseLocation.TabIndex = 35;
      this.labelDatabaseLocation.Text = "Database Location:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(236, 86);
      this.label2.Name = "label2";
      this.label2.Size = new Size(141, 13);
      this.label2.TabIndex = 34;
      this.label2.Text = "Choose a pre-defined group:";
      this.checkBoxUpdateSingleTable.AutoSize = true;
      this.checkBoxUpdateSingleTable.Location = new Point(6, 450);
      this.checkBoxUpdateSingleTable.Name = "checkBoxUpdateSingleTable";
      this.checkBoxUpdateSingleTable.Size = new Size(130, 17);
      this.checkBoxUpdateSingleTable.TabIndex = 35;
      this.checkBoxUpdateSingleTable.Text = "Update a single Table";
      this.checkBoxUpdateSingleTable.UseVisualStyleBackColor = true;
      this.checkBoxUpdateSingleTable.CheckedChanged += new System.EventHandler(this.checkBoxUpdateSingleTable_CheckedChanged);
      this.groupBox4.Controls.Add((Control) this.buttonCancelTimePointStart);
      this.groupBox4.Controls.Add((Control) this.StartTimePicker);
      this.groupBox4.Controls.Add((Control) this.labelTimepoint);
      this.groupBox4.Controls.Add((Control) this.checkBoxEnableStartTime);
      this.groupBox4.Controls.Add((Control) this.checkBoxUseOdbcConnection);
      this.groupBox4.Controls.Add((Control) this.checkBoxVacuum);
      this.groupBox4.Location = new Point(7, 232);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(452, 79);
      this.groupBox4.TabIndex = 36;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Options:";
      this.buttonCancelTimePointStart.AutoEllipsis = true;
      this.buttonCancelTimePointStart.Location = new Point(373, 48);
      this.buttonCancelTimePointStart.Name = "buttonCancelTimePointStart";
      this.buttonCancelTimePointStart.Size = new Size(75, 23);
      this.buttonCancelTimePointStart.TabIndex = 31;
      this.buttonCancelTimePointStart.Text = "Cancel";
      this.buttonCancelTimePointStart.UseVisualStyleBackColor = true;
      this.buttonCancelTimePointStart.Click += new System.EventHandler(this.buttonCancelTimePointStart_Click);
      this.StartTimePicker.CustomFormat = "dd.MM.yyyy HH.mm:ss";
      this.StartTimePicker.Format = DateTimePickerFormat.Custom;
      this.StartTimePicker.Location = new Point(223, 49);
      this.StartTimePicker.Name = "StartTimePicker";
      this.StartTimePicker.Size = new Size(143, 20);
      this.StartTimePicker.TabIndex = 30;
      this.labelTimepoint.AutoSize = true;
      this.labelTimepoint.Location = new Point(165, 53);
      this.labelTimepoint.Name = "labelTimepoint";
      this.labelTimepoint.Size = new Size(56, 13);
      this.labelTimepoint.TabIndex = 29;
      this.labelTimepoint.Text = "Timepoint:";
      this.checkBoxEnableStartTime.AutoSize = true;
      this.checkBoxEnableStartTime.Location = new Point(7, 49);
      this.checkBoxEnableStartTime.Name = "checkBoxEnableStartTime";
      this.checkBoxEnableStartTime.Size = new Size(117, 17);
      this.checkBoxEnableStartTime.TabIndex = 28;
      this.checkBoxEnableStartTime.Text = "Start after timepoint";
      this.checkBoxEnableStartTime.UseVisualStyleBackColor = true;
      this.checkBoxUseOdbcConnection.AutoSize = true;
      this.checkBoxUseOdbcConnection.Location = new Point(118, 19);
      this.checkBoxUseOdbcConnection.Name = "checkBoxUseOdbcConnection";
      this.checkBoxUseOdbcConnection.Size = new Size(150, 17);
      this.checkBoxUseOdbcConnection.TabIndex = 27;
      this.checkBoxUseOdbcConnection.Text = "Use ODBC for large tables";
      this.checkBoxUseOdbcConnection.UseVisualStyleBackColor = true;
      this.groupBox5.Controls.Add((Control) this.buttonSetAll);
      this.groupBox5.Controls.Add((Control) this.buttonClearAll);
      this.groupBox5.Controls.Add((Control) this.buttonStartSingleTableUpdate);
      this.groupBox5.Controls.Add((Control) this.checkBoxUpdateSingleTable);
      this.groupBox5.Controls.Add((Control) this.lblTables);
      this.groupBox5.Controls.Add((Control) this.progressBarCloner);
      this.groupBox5.Controls.Add((Control) this.chkContent);
      this.groupBox5.Controls.Add((Control) this.comboBoxSingleTableUpdate);
      this.groupBox5.Controls.Add((Control) this.groupBox4);
      this.groupBox5.Controls.Add((Control) this.cmdStartClone);
      this.groupBox5.Controls.Add((Control) this.chkStructure);
      this.groupBox5.Controls.Add((Control) this.label1);
      this.groupBox5.Controls.Add((Control) this.textBoxWhereConditions);
      this.groupBox5.Controls.Add((Control) this.label2);
      this.groupBox5.Controls.Add((Control) this.lstBoxTables);
      this.groupBox5.Controls.Add((Control) this.listBoxSpecialGroup);
      this.groupBox5.Location = new Point(12, 76);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(469, 497);
      this.groupBox5.TabIndex = 37;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Create Structure and Copy Data ";
      this.buttonSetAll.Location = new Point(125, 76);
      this.buttonSetAll.Name = "buttonSetAll";
      this.buttonSetAll.Size = new Size(109, 23);
      this.buttonSetAll.TabIndex = 39;
      this.buttonSetAll.Text = "Set all";
      this.buttonSetAll.UseVisualStyleBackColor = true;
      this.buttonSetAll.Click += new System.EventHandler(this.buttonSetAll_Click);
      this.buttonClearAll.Location = new Point(6, 76);
      this.buttonClearAll.Name = "buttonClearAll";
      this.buttonClearAll.Size = new Size(113, 23);
      this.buttonClearAll.TabIndex = 39;
      this.buttonClearAll.Text = "Clear all";
      this.buttonClearAll.UseVisualStyleBackColor = true;
      this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
      this.buttonStartSingleTableUpdate.ForeColor = SystemColors.ControlText;
      this.buttonStartSingleTableUpdate.Location = new Point(380, 448);
      this.buttonStartSingleTableUpdate.Name = "buttonStartSingleTableUpdate";
      this.buttonStartSingleTableUpdate.Size = new Size(79, 22);
      this.buttonStartSingleTableUpdate.TabIndex = 38;
      this.buttonStartSingleTableUpdate.Text = "Start Update";
      this.buttonStartSingleTableUpdate.UseVisualStyleBackColor = true;
      this.buttonStartSingleTableUpdate.Click += new System.EventHandler(this.buttonStartSingleTableUpdate_Click);
      this.progressBarCloner.Location = new Point(6, 419);
      this.progressBarCloner.Name = "progressBarCloner";
      this.progressBarCloner.Size = new Size(367, 23);
      this.progressBarCloner.TabIndex = 37;
      this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.TableLog);
      this.groupBox1.Location = new Point(499, 312);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(558, 261);
      this.groupBox1.TabIndex = 38;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Tabellen Status:";
      this.TableLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.TableLog.ForeColor = SystemColors.ControlText;
      this.TableLog.Location = new Point(6, 19);
      this.TableLog.Multiline = true;
      this.TableLog.Name = "TableLog";
      this.TableLog.ScrollBars = ScrollBars.Vertical;
      this.TableLog.Size = new Size(546, 229);
      this.TableLog.TabIndex = 14;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1069, 589);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.groupBox5);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.lblDatarows);
      this.Controls.Add((Control) this.lblDuration);
      this.Controls.Add((Control) this.lblMinutes);
      this.Controls.Add((Control) this.buttonWriteXSD);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (DbCloner);
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.Text = nameof (DbCloner);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public class SpecialTableClass
    {
      public string TableName;
      public int StepValue;
      public string Index;

      public SpecialTableClass(string TheTableName, int TheStepValue, string TheIndex)
      {
        this.TableName = TheTableName;
        this.StepValue = TheStepValue;
        this.Index = TheIndex;
      }
    }
  }
}
