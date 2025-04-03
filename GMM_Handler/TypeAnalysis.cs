// Decompiled with JetBrains decompiler
// Type: GMM_Handler.TypeAnalysis
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace GMM_Handler
{
  public class TypeAnalysis : Form
  {
    private ZR_HandlerFunctions MyHandler;
    private bool BreakLoop;
    private SortedList<ushort, SortedList<int, string>> UsedFunctions;
    private SortedList<int, string> MBusLists;
    private SortedList<int, SortedList<int, string>> UsedMBusLists;
    private DataTable FunctionList;
    private DataTable MBusListsTable;
    private DataTable MeterDetailsTable;
    private DataTable ExtendetMeterInfoTable;
    private DataTable ReducedExtendetMeterInfoTable;
    private static Logger logger = LogManager.GetLogger(nameof (TypeAnalysis));
    private TypeDetailWindow DetailWindow;
    private DataRow AnalyseRow;
    private IContainer components = (IContainer) null;
    private DataGridView dataGridView1;
    private Button buttonShowTypeList;
    private TextBox textBoxState;
    private Button buttonBreak;
    private Button buttonFunctionUsage;
    private Button buttonMBusListeTypes;
    private Button buttonReduceTypeList;
    private TextBox textBoxReduceTypeList;
    private Label label1;
    private Button buttonTypeListWithoutMeterDate;
    private TextBox textBoxFromMeterId;
    private Label label2;
    private Label label3;
    private TextBox textBoxToMeterInfoId;
    private Button buttonDetailWindow;
    private Label label4;
    private TextBox textBoxMaxNumberOfTypes;
    private Button buttonClearData;
    private Button buttonGenerateTypeInfos;
    private Button buttonAnalyseMeters;
    private TabControl tabControl1;
    private TabPage tabPageTable;
    private TabPage tabPageSettings;
    private Label label5;
    private Button buttonAnalyseAllMeters;
    private DateTimePicker dateTimePicker1;

    public TypeAnalysis(ZR_HandlerFunctions MyHandler)
    {
      this.InitializeComponent();
      this.MyHandler = MyHandler;
      this.dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
    }

    private void buttonBreak_Click(object sender, EventArgs e) => this.BreakLoop = true;

    private void buttonShowTypeList_Click(object sender, EventArgs e)
    {
      this.dataGridView1.DataSource = (object) null;
      if (!this.GeneratInfoLists())
        return;
      this.textBoxState.Text = "Generate meter overview";
      this.Refresh();
      this.ExtendetMeterInfoTable = this.MyHandler.MyDataBaseAccess.MeterInfoTable.Clone();
      DataColumnCollection columns1 = this.ExtendetMeterInfoTable.Columns;
      TypeAnalysis.ExtendedMeterInfoColum extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.First_prod;
      string columnName1 = extendedMeterInfoColum.ToString();
      System.Type type1 = typeof (DateTime);
      columns1.Add(columnName1, type1);
      DataColumnCollection columns2 = this.ExtendetMeterInfoTable.Columns;
      extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.Last_prod;
      string columnName2 = extendedMeterInfoColum.ToString();
      System.Type type2 = typeof (DateTime);
      columns2.Add(columnName2, type2);
      DataColumnCollection columns3 = this.ExtendetMeterInfoTable.Columns;
      extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.Meters;
      string columnName3 = extendedMeterInfoColum.ToString();
      System.Type type3 = typeof (int);
      columns3.Add(columnName3, type3);
      DataColumnCollection columns4 = this.ExtendetMeterInfoTable.Columns;
      extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.TypLoad;
      string columnName4 = extendedMeterInfoColum.ToString();
      System.Type type4 = typeof (string);
      columns4.Add(columnName4, type4);
      DataColumnCollection columns5 = this.ExtendetMeterInfoTable.Columns;
      extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.MeterLoad;
      string columnName5 = extendedMeterInfoColum.ToString();
      System.Type type5 = typeof (string);
      columns5.Add(columnName5, type5);
      DataColumnCollection columns6 = this.ExtendetMeterInfoTable.Columns;
      extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.MeterMBusLists;
      string columnName6 = extendedMeterInfoColum.ToString();
      System.Type type6 = typeof (string);
      columns6.Add(columnName6, type6);
      int count = this.MyHandler.MyDataBaseAccess.MeterInfoTable.Columns.Count;
      foreach (Schema.MeterInfoRow meterInfoRow in (TypedTableBase<Schema.MeterInfoRow>) this.MyHandler.MyDataBaseAccess.MeterInfoTable)
      {
        DataRow row = this.ExtendetMeterInfoTable.NewRow();
        for (int columnIndex = 0; columnIndex < count; ++columnIndex)
          row[columnIndex] = meterInfoRow[columnIndex];
        Schema.MeterRow[] meterRowArray = (Schema.MeterRow[]) this.MyHandler.MyDataBaseAccess.MeterTable.Select("MeterInfoID = " + meterInfoRow.MeterInfoID.ToString());
        DataRow dataRow1 = row;
        extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.Meters;
        string columnName7 = extendedMeterInfoColum.ToString();
        // ISSUE: variable of a boxed type
        __Boxed<int> length = (System.ValueType) meterRowArray.Length;
        dataRow1[columnName7] = (object) length;
        if (meterRowArray.Length != 0)
        {
          DataRow dataRow2 = row;
          extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.First_prod;
          string columnName8 = extendedMeterInfoColum.ToString();
          // ISSUE: variable of a boxed type
          __Boxed<DateTime> productionDate1 = (System.ValueType) meterRowArray[0].ProductionDate;
          dataRow2[columnName8] = (object) productionDate1;
          DataRow dataRow3 = row;
          extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.Last_prod;
          string columnName9 = extendedMeterInfoColum.ToString();
          // ISSUE: variable of a boxed type
          __Boxed<DateTime> productionDate2 = (System.ValueType) meterRowArray[meterRowArray.Length - 1].ProductionDate;
          dataRow3[columnName9] = (object) productionDate2;
        }
        this.ExtendetMeterInfoTable.Rows.Add(row);
      }
      this.dataGridView1.DataSource = (object) this.ExtendetMeterInfoTable;
    }

    private void dataGridView1_CellDoubleClick(object sender, EventArgs e)
    {
      this.dataGridView1.Rows[((DataGridViewCellEventArgs) e).RowIndex].Cells[0].Value.ToString();
    }

    private bool GeneratInfoLists()
    {
      this.ExtendetMeterInfoTable = (DataTable) null;
      this.textBoxState.Text = "Load type list";
      this.Refresh();
      if (!this.MyHandler.MyDataBaseAccess.LoadTypeList())
        return false;
      this.textBoxState.Text = "Load meter list";
      this.Refresh();
      if (!this.MyHandler.MyDataBaseAccess.LoadMeterList())
        return false;
      this.buttonGenerateTypeInfos.Enabled = true;
      return true;
    }

    private bool IsRangeDefined()
    {
      return this.textBoxFromMeterId.Text.Length != 0 && !(this.textBoxFromMeterId.Text == "0") || this.textBoxToMeterInfoId.Text.Length != 0 && !(this.textBoxToMeterInfoId.Text == "1000000000");
    }

    private bool GeneratTypeInfos()
    {
      this.dataGridView1.DataSource = (object) null;
      this.UsedFunctions = new SortedList<ushort, SortedList<int, string>>();
      this.MBusLists = new SortedList<int, string>();
      this.UsedMBusLists = new SortedList<int, SortedList<int, string>>();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      string str1 = this.MyHandler.MyDataBaseAccess.MeterInfoTable.Count.ToString();
      int num5 = 0;
      string str2 = string.Empty;
      this.BreakLoop = false;
      int result1;
      if (int.TryParse(this.textBoxMaxNumberOfTypes.Text, out result1))
      {
        if (result1 > this.MyHandler.MyDataBaseAccess.MeterInfoTable.Rows.Count)
          result1 = this.MyHandler.MyDataBaseAccess.MeterInfoTable.Rows.Count;
      }
      else
        result1 = this.MyHandler.MyDataBaseAccess.MeterInfoTable.Rows.Count;
      for (int index1 = 0; index1 < result1; ++index1)
      {
        Schema.MeterInfoRow meterInfoRow = this.MyHandler.MyDataBaseAccess.MeterInfoTable[index1];
        DataRow row = this.ExtendetMeterInfoTable.Rows[index1];
        ++num5;
        this.textBoxState.Text = num5.ToString() + "/" + str1 + "; Loaded:" + num1.ToString() + "; ErrorOn:" + num2.ToString() + "; Exception:" + num3.ToString();
        Application.DoEvents();
        if (!this.BreakLoop)
        {
          if (meterInfoRow.IsDescriptionNull())
          {
            ++num4;
          }
          else
          {
            str2 = meterInfoRow.Description;
            if (!this.MyHandler.MyDataBaseAccess.IsSerie2Type(meterInfoRow.MeterTypeID))
            {
              ++num4;
            }
            else
            {
              int result2 = 0;
              int result3 = 1000000000;
              if (!int.TryParse(this.textBoxFromMeterId.Text, out result2))
                result2 = 0;
              if (!int.TryParse(this.textBoxToMeterInfoId.Text, out result3))
                result3 = 1000000000;
              this.textBoxFromMeterId.Text = result2.ToString();
              this.textBoxToMeterInfoId.Text = result3.ToString();
              if (meterInfoRow.MeterInfoID >= result2 && meterInfoRow.MeterInfoID <= result3)
              {
                try
                {
                  TypeAnalysis.ExtendedMeterInfoColum extendedMeterInfoColum;
                  if (this.MyHandler.MyMeters.LoadType(meterInfoRow.MeterInfoID, true))
                  {
                    this.MyHandler.MyMeters.CopyMeter(ZR_HandlerFunctions.MeterObjects.Work);
                    string str3 = "true";
                    for (int index2 = 0; index2 < this.MyHandler.MyMeters.WorkMeter.MyFunctionTable.FunctionListByNumber.Count; ++index2)
                    {
                      ushort key = (ushort) this.MyHandler.MyMeters.WorkMeter.MyFunctionTable.FunctionListByNumber.GetKey(index2);
                      if (!this.UsedFunctions.ContainsKey(key))
                        this.UsedFunctions.Add(key, new SortedList<int, string>());
                      this.UsedFunctions[key].Add(meterInfoRow.MeterInfoID, meterInfoRow.Description);
                    }
                    ++num1;
                    StringBuilder stringBuilder1 = new StringBuilder(1000);
                    int key1 = 0;
                    string str4;
                    string str5;
                    try
                    {
                      stringBuilder1.AppendLine("ShortList:");
                      for (int index3 = 0; index3 < this.MyHandler.MyMeters.WorkMeter.MyMBusList.ShortListParameterNames.Count; ++index3)
                        stringBuilder1.AppendLine((string) this.MyHandler.MyMeters.WorkMeter.MyMBusList.ShortListParameterNames[index3]);
                      stringBuilder1.AppendLine("FullList:");
                      for (int index4 = 0; index4 < this.MyHandler.MyMeters.WorkMeter.MyMBusList.FullListParameterNames.Count; ++index4)
                        stringBuilder1.AppendLine((string) this.MyHandler.MyMeters.WorkMeter.MyMBusList.FullListParameterNames[index4]);
                      str4 = stringBuilder1.ToString();
                      for (int index5 = 0; index5 < str4.Length; ++index5)
                        key1 += (int) str4[index5] + index5;
                      str5 = str3 + ";MBusOk";
                    }
                    catch
                    {
                      key1 = -1;
                      str4 = "MBus list exception";
                      str5 = str3 + ";MBusException";
                    }
                    if (!this.UsedMBusLists.ContainsKey(key1))
                    {
                      this.MBusLists.Add(key1, str4);
                      this.UsedMBusLists.Add(key1, new SortedList<int, string>());
                    }
                    this.UsedMBusLists[key1].Add(meterInfoRow.MeterInfoID, meterInfoRow.Description);
                    row[TypeAnalysis.ExtendedMeterInfoColum.TypLoad.ToString()] = (object) str5;
                    bool flag1;
                    int num6;
                    Schema.MeterRow[] meterRowArray;
                    if (this.dateTimePicker1.Value > new DateTime(2000, 1, 1))
                    {
                      flag1 = false;
                      Schema.MeterDataTable meterTable = this.MyHandler.MyDataBaseAccess.MeterTable;
                      num6 = meterInfoRow.MeterInfoID;
                      string filterExpression = "MeterInfoID = " + num6.ToString() + " AND ApprovalDate >= " + this.dateTimePicker1.Value.ToString((IFormatProvider) DateTimeFormatInfo.InvariantInfo);
                      meterRowArray = (Schema.MeterRow[]) meterTable.Select(filterExpression);
                    }
                    else
                    {
                      flag1 = true;
                      Schema.MeterDataTable meterTable = this.MyHandler.MyDataBaseAccess.MeterTable;
                      num6 = meterInfoRow.MeterInfoID;
                      string filterExpression = "MeterInfoID = " + num6.ToString();
                      meterRowArray = (Schema.MeterRow[]) meterTable.Select(filterExpression);
                    }
                    if (meterInfoRow.PPSArtikelNr.StartsWith("HWTOTYPE"))
                    {
                      DataRow dataRow = row;
                      extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.MeterLoad;
                      string columnName = extendedMeterInfoColum.ToString();
                      dataRow[columnName] = (object) "Hardware test type: Meters not checked";
                    }
                    else if (meterRowArray.Length == 0)
                    {
                      row[TypeAnalysis.ExtendedMeterInfoColum.MeterLoad.ToString()] = !flag1 ? (object) ("Not produced since " + this.dateTimePicker1.Value.ToShortDateString()) : (object) "Not produced";
                    }
                    else
                    {
                      bool flag2 = false;
                      StringBuilder stringBuilder2 = new StringBuilder(500);
                      StringBuilder stringBuilder3 = new StringBuilder(500);
                      int num7 = 0;
                      int index6 = meterRowArray.Length - 1;
                      while (true)
                      {
                        Schema.MeterRow meterRow = meterRowArray[index6];
                        try
                        {
                          if (stringBuilder2.Length != 0)
                            stringBuilder2.Append(";");
                          StringBuilder stringBuilder4 = stringBuilder2;
                          num6 = meterRow.MeterID;
                          string str6 = "M:" + num6.ToString();
                          stringBuilder4.Append(str6);
                          stringBuilder2.Append(";P:" + meterRow.ProductionDate.ToShortDateString());
                          if (!meterRow.IsApprovalDateNull())
                            stringBuilder2.Append(";A:" + meterRow.ApprovalDate.ToShortDateString());
                          else
                            stringBuilder2.Append(";A:Missing");
                          if (this.MyHandler.MyMeters.LoadMeter(new ZR_MeterIdent(MeterBasis.DbMeter)
                          {
                            MeterID = meterRow.MeterID
                          }, DateTime.MaxValue))
                          {
                            stringBuilder2.Append(";D:" + this.MyHandler.MyMeters.DbMeter.DatabaseTime.ToShortDateString());
                            if (this.MyHandler.MyMeters.DbMeter.MyIdent.lFirmwareVersion != this.MyHandler.MyMeters.SavedMeter.MyIdent.lFirmwareVersion)
                            {
                              stringBuilder2.Append(";LoadExactFirmwareType");
                              if (this.MyHandler.MyMeters.LoadType(meterInfoRow.MeterInfoID, (int) this.MyHandler.MyMeters.DbMeter.MyIdent.lFirmwareVersion, true))
                                this.MyHandler.MyMeters.CopyMeter(ZR_HandlerFunctions.MeterObjects.Work);
                              else
                                stringBuilder2.Append(";--Load exact type error--");
                            }
                            stringBuilder1.Length = 0;
                            int key2 = 0;
                            try
                            {
                              stringBuilder1.AppendLine("ShortList:");
                              for (int index7 = 0; index7 < this.MyHandler.MyMeters.WorkMeter.MyMBusList.ShortListParameterNames.Count; ++index7)
                                stringBuilder1.AppendLine((string) this.MyHandler.MyMeters.WorkMeter.MyMBusList.ShortListParameterNames[index7]);
                              stringBuilder1.AppendLine("FullList:");
                              for (int index8 = 0; index8 < this.MyHandler.MyMeters.WorkMeter.MyMBusList.FullListParameterNames.Count; ++index8)
                                stringBuilder1.AppendLine((string) this.MyHandler.MyMeters.WorkMeter.MyMBusList.FullListParameterNames[index8]);
                              str4 = stringBuilder1.ToString();
                              for (int index9 = 0; index9 < str4.Length; ++index9)
                                key2 += (int) str4[index9] + index9;
                            }
                            catch
                            {
                              key2 = -1;
                              stringBuilder2.Append(";MBusException");
                            }
                            if (!this.UsedMBusLists.ContainsKey(key2))
                            {
                              this.MBusLists.Add(key2, str4);
                              this.UsedMBusLists.Add(key2, new SortedList<int, string>());
                            }
                            if (num7 != key2)
                            {
                              if (stringBuilder3.Length > 0)
                              {
                                if (stringBuilder3[0] != '@')
                                  stringBuilder3.Insert(0, '@');
                                stringBuilder3.Append(";" + key2.ToString());
                              }
                              else
                                stringBuilder3.Append(key2.ToString());
                              num7 = key2;
                            }
                            if (!DataChecker.IsEqualMap(this.MyHandler.MyMeters.WorkMeter, this.MyHandler.MyMeters.SavedMeter))
                            {
                              stringBuilder2.Append(";MapDiff");
                              flag2 = true;
                            }
                            if (!DataChecker.IsEqualAllPointers(this.MyHandler.MyMeters.WorkMeter, this.MyHandler.MyMeters.SavedMeter))
                            {
                              stringBuilder2.Append(";PointerDiff");
                              flag2 = true;
                            }
                            if (!DataChecker.IsEqualProtectedArea(this.MyHandler.MyMeters.WorkMeter, this.MyHandler.MyMeters.SavedMeter))
                            {
                              stringBuilder2.Append(";ProtAreaDiff");
                              flag2 = true;
                            }
                            if (!DataChecker.AreOverridesEqualToDatabase(this.MyHandler.MyMeters.WorkMeter))
                            {
                              stringBuilder2.Append(";OverridesChanged");
                              flag2 = true;
                            }
                            stringBuilder2.Append("|");
                          }
                          else
                          {
                            if (TypeAnalysis.logger.IsDebugEnabled)
                            {
                              TypeAnalysis.logger.Debug("     Load meter error (MeterInfoId): " + this.MyHandler.MyMeters.WorkMeter.MyIdent.MeterInfoID.ToString());
                              ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
                              if (errorAndClearError.LastErrorDescription != null && errorAndClearError.LastErrorDescription.Length > 0)
                                TypeAnalysis.logger.Debug("           ----->" + errorAndClearError.LastErrorDescription);
                            }
                            stringBuilder2.Append(";LoadErr|");
                            if (!this.MyHandler.MyDataBaseAccess.IsMeterInfoProperty(this.MyHandler.MyMeters.WorkMeter.MyIdent.MeterInfoID, DataBaseAccess.MeterInfoProperties.DatasetError, "LoadDevice"))
                              flag2 = true;
                          }
                        }
                        catch
                        {
                          stringBuilder2.Append(";Exception|");
                          flag2 = true;
                        }
                        DateTime dateTime1 = meterRow.ProductionDate.AddMonths(-6);
                        --index6;
                        DateTime dateTime2 = meterRow.ProductionDate;
                        for (; index6 >= 0; --index6)
                        {
                          if (!meterRowArray[index6].IsApprovalDateNull())
                            dateTime2 = meterRowArray[index6].ApprovalDate;
                          else if (!meterRowArray[index6].IsProductionDateNull())
                            dateTime2 = meterRowArray[index6].ProductionDate;
                          else
                            continue;
                          if (dateTime2 < dateTime1)
                            break;
                        }
                        if (index6 < 0)
                        {
                          if (dateTime2 != meterRow.ProductionDate)
                            index6 = 0;
                          else
                            break;
                        }
                      }
                      row[TypeAnalysis.ExtendedMeterInfoColum.MeterLoad.ToString()] = !flag2 ? (object) stringBuilder2.ToString() : (object) ("@@@|" + stringBuilder2.ToString());
                      row[TypeAnalysis.ExtendedMeterInfoColum.MeterMBusLists.ToString()] = (object) stringBuilder3.ToString();
                    }
                  }
                  else
                  {
                    if (TypeAnalysis.logger.IsDebugEnabled)
                    {
                      TypeAnalysis.logger.Debug("Load type error: " + meterInfoRow.MeterInfoID.ToString());
                      ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
                      if (errorAndClearError.LastErrorDescription != null && errorAndClearError.LastErrorDescription.Length > 0)
                        TypeAnalysis.logger.Debug("           ----->" + errorAndClearError.LastErrorDescription);
                    }
                    ++num2;
                    DataRow dataRow = row;
                    extendedMeterInfoColum = TypeAnalysis.ExtendedMeterInfoColum.TypLoad;
                    string columnName = extendedMeterInfoColum.ToString();
                    dataRow[columnName] = (object) "false";
                  }
                }
                catch
                {
                  ++num3;
                  row[TypeAnalysis.ExtendedMeterInfoColum.TypLoad.ToString()] = (object) "exception";
                }
              }
            }
          }
        }
        else
          break;
      }
      this.dataGridView1.DataSource = (object) this.ExtendetMeterInfoTable;
      return true;
    }

    private void buttonFunctionUsage_Click(object sender, EventArgs e)
    {
      this.FunctionList = new DataTable();
      this.FunctionList.Columns.Add("FunctionId", typeof (ushort));
      this.FunctionList.Columns.Add("FunctionName", typeof (string));
      this.FunctionList.Columns.Add("FunctionVersion", typeof (int));
      this.FunctionList.Columns.Add("Used on types", typeof (int));
      for (int index = 0; index < this.UsedFunctions.Count; ++index)
      {
        ushort key = this.UsedFunctions.Keys[index];
        Function fullLoadedFunction = (Function) this.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) key];
        DataRow row = this.FunctionList.NewRow();
        row[0] = (object) key;
        row[1] = (object) fullLoadedFunction.Name;
        row[2] = (object) fullLoadedFunction.Version;
        row[3] = (object) this.UsedFunctions.Values[index].Count;
        this.FunctionList.Rows.Add(row);
      }
      this.dataGridView1.DataSource = (object) this.FunctionList;
    }

    private void buttonMBusListeTypes_Click(object sender, EventArgs e)
    {
      this.MBusListsTable = new DataTable();
      this.MBusListsTable.Columns.Add("MBus list index", typeof (int));
      this.MBusListsTable.Columns.Add("Used on types", typeof (int));
      this.MBusListsTable.Columns.Add("The list", typeof (string));
      for (int index = 0; index < this.UsedMBusLists.Count; ++index)
      {
        int key = this.UsedMBusLists.Keys[index];
        DataRow row = this.MBusListsTable.NewRow();
        row[0] = (object) key;
        row[1] = (object) this.UsedMBusLists.Values[index].Count;
        row[2] = (object) this.MBusLists.Values[index].Replace(Environment.NewLine, ";");
        this.MBusListsTable.Rows.Add(row);
      }
      this.dataGridView1.DataSource = (object) this.MBusListsTable;
    }

    private void buttonReduceTypeList_Click(object sender, EventArgs e)
    {
      if (this.ExtendetMeterInfoTable == null)
        return;
      this.ReducedExtendetMeterInfoTable = this.ExtendetMeterInfoTable.Clone();
      foreach (DataRow dataRow in this.ExtendetMeterInfoTable.Select("Description LIKE '" + this.textBoxReduceTypeList.Text + "'"))
      {
        DataRow row = this.ReducedExtendetMeterInfoTable.NewRow();
        for (int columnIndex = 0; columnIndex < this.ReducedExtendetMeterInfoTable.Columns.Count; ++columnIndex)
          row[columnIndex] = dataRow[columnIndex];
        this.ReducedExtendetMeterInfoTable.Rows.Add(row);
      }
      this.dataGridView1.DataSource = (object) this.ReducedExtendetMeterInfoTable;
    }

    private void buttonTypeListWithoutMeterDate_Click(object sender, EventArgs e)
    {
      if (this.ExtendetMeterInfoTable == null)
        return;
      this.ReducedExtendetMeterInfoTable = this.ExtendetMeterInfoTable.Clone();
      StringBuilder stringBuilder = new StringBuilder();
      string columnName = TypeAnalysis.ExtendedMeterInfoColum.MeterLoad.ToString();
      foreach (DataRow row1 in (InternalDataCollectionBase) this.ExtendetMeterInfoTable.Rows)
      {
        DataRow row2 = this.ReducedExtendetMeterInfoTable.NewRow();
        for (int columnIndex = 0; columnIndex < this.ReducedExtendetMeterInfoTable.Columns.Count; ++columnIndex)
          row2[columnIndex] = row1[columnIndex];
        string str1 = row2[columnName].ToString();
        if (str1.Length == 0)
        {
          row2[columnName] = (object) "NoData";
        }
        else
        {
          stringBuilder.Length = 0;
          string str2 = str1;
          char[] chArray1 = new char[1]{ '|' };
          foreach (string str3 in str2.Split(chArray1))
          {
            char[] chArray2 = new char[1]{ ';' };
            string[] strArray = str3.Split(chArray2);
            for (int index = 0; index < strArray.Length; ++index)
            {
              if (strArray[index].Length <= 1 || strArray[index][1] != ':')
              {
                if (stringBuilder.Length > 0)
                  stringBuilder.Append(';');
                stringBuilder.Append(strArray[index]);
              }
            }
          }
          row2[columnName] = stringBuilder.Length != 0 ? (object) stringBuilder.ToString() : (object) "ok";
        }
        this.ReducedExtendetMeterInfoTable.Rows.Add(row2);
      }
      this.dataGridView1.DataSource = (object) this.ReducedExtendetMeterInfoTable;
    }

    private void buttonDetailWindow_Click(object sender, EventArgs e)
    {
      if (this.DetailWindow != null && !this.DetailWindow.IsDisposed)
        return;
      this.DetailWindow = new TypeDetailWindow();
      this.DetailWindow.Show();
    }

    private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
    {
      this.buttonDetailWindow.Enabled = false;
      this.buttonAnalyseMeters.Enabled = false;
      if (this.dataGridView1.CurrentRow == null || this.dataGridView1.CurrentRow.DataBoundItem == null || !(this.dataGridView1.CurrentRow.DataBoundItem is DataRowView))
        return;
      this.AnalyseRow = ((DataRowView) this.dataGridView1.CurrentRow.DataBoundItem).Row;
      this.buttonAnalyseMeters.Enabled = true;
      if (this.DetailWindow == null || this.DetailWindow.IsDisposed)
        this.buttonDetailWindow.Enabled = true;
      else
        this.DetailWindow.SetDataFromRow(this.AnalyseRow);
    }

    private void buttonClearData_Click(object sender, EventArgs e)
    {
      this.UsedFunctions = (SortedList<ushort, SortedList<int, string>>) null;
      this.buttonGenerateTypeInfos.Enabled = false;
      this.buttonMBusListeTypes.Enabled = false;
      this.buttonFunctionUsage.Enabled = false;
      this.buttonAnalyseMeters.Enabled = false;
    }

    private void buttonGenerateTypeInfos_Click(object sender, EventArgs e)
    {
      if (!this.GeneratTypeInfos())
        return;
      this.buttonMBusListeTypes.Enabled = true;
      this.buttonFunctionUsage.Enabled = true;
    }

    private void buttonAnalyseMeters_Click(object sender, EventArgs e)
    {
      this.dataGridView1.DataSource = (object) null;
      this.MeterDetailsTable = new DataTable();
      this.MeterDetailsTable.Columns.Add("SerialNumber", typeof (int));
      this.MeterDetailsTable.Columns.Add("OrderNumber", typeof (string));
      this.MeterDetailsTable.Columns.Add("ApprovalDate", typeof (string));
      this.MeterDetailsTable.Columns.Add("LoadError", typeof (bool));
      this.MeterDetailsTable.Columns.Add("EndDateError", typeof (bool));
      this.MeterDetailsTable.Columns.Add("EndDateInfo", typeof (string));
      Schema.MeterRow[] meterRowArray = (Schema.MeterRow[]) this.MyHandler.MyDataBaseAccess.MeterTable.Select("MeterInfoID = " + this.AnalyseRow["MeterInfoID"].ToString());
      this.BreakLoop = false;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < meterRowArray.Length && !this.BreakLoop; ++index)
      {
        this.textBoxState.Text = "Check meter " + index.ToString() + "/" + meterRowArray.Length.ToString() + "; LoadErrors:" + num1.ToString() + "; EndDateErrors:" + num2.ToString();
        Application.DoEvents();
        DataRow row = this.MeterDetailsTable.NewRow();
        row["SerialNumber"] = (object) meterRowArray[index].SerialNr;
        row["OrderNumber"] = (object) meterRowArray[index].OrderNr.ToString();
        row["ApprovalDate"] = (object) meterRowArray[index].ApprovalDate.ToString();
        row["LoadError"] = (object) true;
        if (this.MyHandler.MyMeters.LoadMeter(new ZR_MeterIdent(MeterBasis.DbMeter)
        {
          MeterID = meterRowArray[index].MeterID
        }, DateTime.MaxValue))
        {
          string Info;
          bool flag = DataChecker.IsEndTimeOk(this.MyHandler.MyMeters.DbMeter, out Info);
          if (!flag)
            ++num2;
          row["LoadError"] = (object) false;
          row["EndDateError"] = (object) !flag;
          row["EndDateInfo"] = (object) Info;
        }
        else
          ++num1;
        this.MeterDetailsTable.Rows.Add(row);
      }
      this.dataGridView1.DataSource = (object) this.MeterDetailsTable;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dataGridView1 = new DataGridView();
      this.buttonShowTypeList = new Button();
      this.textBoxState = new TextBox();
      this.buttonBreak = new Button();
      this.buttonFunctionUsage = new Button();
      this.buttonMBusListeTypes = new Button();
      this.buttonReduceTypeList = new Button();
      this.textBoxReduceTypeList = new TextBox();
      this.label1 = new Label();
      this.buttonTypeListWithoutMeterDate = new Button();
      this.textBoxFromMeterId = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.textBoxToMeterInfoId = new TextBox();
      this.buttonDetailWindow = new Button();
      this.label4 = new Label();
      this.textBoxMaxNumberOfTypes = new TextBox();
      this.buttonClearData = new Button();
      this.buttonGenerateTypeInfos = new Button();
      this.buttonAnalyseMeters = new Button();
      this.tabControl1 = new TabControl();
      this.tabPageTable = new TabPage();
      this.tabPageSettings = new TabPage();
      this.buttonAnalyseAllMeters = new Button();
      this.label5 = new Label();
      this.dateTimePicker1 = new DateTimePicker();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPageTable.SuspendLayout();
      this.tabPageSettings.SuspendLayout();
      this.SuspendLayout();
      this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Location = new Point(10, 7);
      this.dataGridView1.Margin = new Padding(4, 4, 4, 4);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowTemplate.Height = 24;
      this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = new Size(1153, 560);
      this.dataGridView1.TabIndex = 0;
      this.dataGridView1.CurrentCellChanged += new System.EventHandler(this.dataGridView1_CurrentCellChanged);
      this.buttonShowTypeList.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShowTypeList.Location = new Point(1060, 640);
      this.buttonShowTypeList.Margin = new Padding(4, 4, 4, 4);
      this.buttonShowTypeList.Name = "buttonShowTypeList";
      this.buttonShowTypeList.Size = new Size(118, 28);
      this.buttonShowTypeList.TabIndex = 1;
      this.buttonShowTypeList.Text = "Show type list";
      this.buttonShowTypeList.UseVisualStyleBackColor = true;
      this.buttonShowTypeList.Click += new System.EventHandler(this.buttonShowTypeList_Click);
      this.textBoxState.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxState.Location = new Point(3, 610);
      this.textBoxState.Margin = new Padding(4, 4, 4, 4);
      this.textBoxState.Name = "textBoxState";
      this.textBoxState.Size = new Size(1179, 22);
      this.textBoxState.TabIndex = 2;
      this.buttonBreak.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonBreak.Location = new Point(3, 640);
      this.buttonBreak.Margin = new Padding(4, 4, 4, 4);
      this.buttonBreak.Name = "buttonBreak";
      this.buttonBreak.Size = new Size(72, 28);
      this.buttonBreak.TabIndex = 1;
      this.buttonBreak.Text = "Break";
      this.buttonBreak.UseVisualStyleBackColor = true;
      this.buttonBreak.Click += new System.EventHandler(this.buttonBreak_Click);
      this.buttonFunctionUsage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonFunctionUsage.Enabled = false;
      this.buttonFunctionUsage.Location = new Point(773, 640);
      this.buttonFunctionUsage.Margin = new Padding(4, 4, 4, 4);
      this.buttonFunctionUsage.Name = "buttonFunctionUsage";
      this.buttonFunctionUsage.Size = new Size(126, 28);
      this.buttonFunctionUsage.TabIndex = 1;
      this.buttonFunctionUsage.Text = "Function usage";
      this.buttonFunctionUsage.UseVisualStyleBackColor = true;
      this.buttonFunctionUsage.Click += new System.EventHandler(this.buttonFunctionUsage_Click);
      this.buttonMBusListeTypes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonMBusListeTypes.Enabled = false;
      this.buttonMBusListeTypes.Location = new Point(640, 640);
      this.buttonMBusListeTypes.Margin = new Padding(4, 4, 4, 4);
      this.buttonMBusListeTypes.Name = "buttonMBusListeTypes";
      this.buttonMBusListeTypes.Size = new Size(125, 28);
      this.buttonMBusListeTypes.TabIndex = 1;
      this.buttonMBusListeTypes.Text = "MBus list types";
      this.buttonMBusListeTypes.UseVisualStyleBackColor = true;
      this.buttonMBusListeTypes.Click += new System.EventHandler(this.buttonMBusListeTypes_Click);
      this.buttonReduceTypeList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonReduceTypeList.Enabled = false;
      this.buttonReduceTypeList.Location = new Point(682, 539);
      this.buttonReduceTypeList.Margin = new Padding(4, 4, 4, 4);
      this.buttonReduceTypeList.Name = "buttonReduceTypeList";
      this.buttonReduceTypeList.Size = new Size(239, 28);
      this.buttonReduceTypeList.TabIndex = 1;
      this.buttonReduceTypeList.Text = "Reduce type list by like pattern";
      this.buttonReduceTypeList.UseVisualStyleBackColor = true;
      this.buttonReduceTypeList.Click += new System.EventHandler(this.buttonReduceTypeList_Click);
      this.textBoxReduceTypeList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxReduceTypeList.Location = new Point(196, 111);
      this.textBoxReduceTypeList.Margin = new Padding(4, 4, 4, 4);
      this.textBoxReduceTypeList.Name = "textBoxReduceTypeList";
      this.textBoxReduceTypeList.Size = new Size(723, 22);
      this.textBoxReduceTypeList.TabIndex = 3;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(15, 115);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(180, 17);
      this.label1.TabIndex = 4;
      this.label1.Text = "Like pattern on description:";
      this.buttonTypeListWithoutMeterDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonTypeListWithoutMeterDate.Enabled = false;
      this.buttonTypeListWithoutMeterDate.Location = new Point(929, 539);
      this.buttonTypeListWithoutMeterDate.Margin = new Padding(4, 4, 4, 4);
      this.buttonTypeListWithoutMeterDate.Name = "buttonTypeListWithoutMeterDate";
      this.buttonTypeListWithoutMeterDate.Size = new Size(239, 28);
      this.buttonTypeListWithoutMeterDate.TabIndex = 1;
      this.buttonTypeListWithoutMeterDate.Text = "Type list without meter date";
      this.buttonTypeListWithoutMeterDate.UseVisualStyleBackColor = true;
      this.buttonTypeListWithoutMeterDate.Click += new System.EventHandler(this.buttonTypeListWithoutMeterDate_Click);
      this.textBoxFromMeterId.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxFromMeterId.Location = new Point(196, 143);
      this.textBoxFromMeterId.Margin = new Padding(4, 4, 4, 4);
      this.textBoxFromMeterId.Name = "textBoxFromMeterId";
      this.textBoxFromMeterId.Size = new Size(132, 22);
      this.textBoxFromMeterId.TabIndex = 5;
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(29, 147);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(151, 17);
      this.label2.TabIndex = 4;
      this.label2.Text = "Work from MeterInfoId:";
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(49, 181);
      this.label3.Margin = new Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(139, 17);
      this.label3.TabIndex = 4;
      this.label3.Text = "Work to MeterInfoId::";
      this.textBoxToMeterInfoId.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxToMeterInfoId.Location = new Point(196, 176);
      this.textBoxToMeterInfoId.Margin = new Padding(4, 4, 4, 4);
      this.textBoxToMeterInfoId.Name = "textBoxToMeterInfoId";
      this.textBoxToMeterInfoId.Size = new Size(132, 22);
      this.textBoxToMeterInfoId.TabIndex = 5;
      this.buttonDetailWindow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonDetailWindow.Enabled = false;
      this.buttonDetailWindow.Location = new Point(186, 640);
      this.buttonDetailWindow.Margin = new Padding(4, 4, 4, 4);
      this.buttonDetailWindow.Name = "buttonDetailWindow";
      this.buttonDetailWindow.Size = new Size(106, 28);
      this.buttonDetailWindow.TabIndex = 1;
      this.buttonDetailWindow.Text = "Detail Window";
      this.buttonDetailWindow.UseVisualStyleBackColor = true;
      this.buttonDetailWindow.Click += new System.EventHandler(this.buttonDetailWindow_Click);
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(49, 213);
      this.label4.Margin = new Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(143, 17);
      this.label4.TabIndex = 4;
      this.label4.Text = "Max number of types:";
      this.textBoxMaxNumberOfTypes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxMaxNumberOfTypes.Location = new Point(196, 208);
      this.textBoxMaxNumberOfTypes.Margin = new Padding(4, 4, 4, 4);
      this.textBoxMaxNumberOfTypes.Name = "textBoxMaxNumberOfTypes";
      this.textBoxMaxNumberOfTypes.Size = new Size(132, 22);
      this.textBoxMaxNumberOfTypes.TabIndex = 5;
      this.buttonClearData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonClearData.Location = new Point(83, 640);
      this.buttonClearData.Margin = new Padding(4, 4, 4, 4);
      this.buttonClearData.Name = "buttonClearData";
      this.buttonClearData.Size = new Size(95, 28);
      this.buttonClearData.TabIndex = 1;
      this.buttonClearData.Text = "Clear data";
      this.buttonClearData.UseVisualStyleBackColor = true;
      this.buttonClearData.Click += new System.EventHandler(this.buttonClearData_Click);
      this.buttonGenerateTypeInfos.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonGenerateTypeInfos.Enabled = false;
      this.buttonGenerateTypeInfos.Location = new Point(907, 640);
      this.buttonGenerateTypeInfos.Margin = new Padding(4);
      this.buttonGenerateTypeInfos.Name = "buttonGenerateTypeInfos";
      this.buttonGenerateTypeInfos.Size = new Size(145, 28);
      this.buttonGenerateTypeInfos.TabIndex = 1;
      this.buttonGenerateTypeInfos.Text = "Generate type infos";
      this.buttonGenerateTypeInfos.UseVisualStyleBackColor = true;
      this.buttonGenerateTypeInfos.Click += new System.EventHandler(this.buttonGenerateTypeInfos_Click);
      this.buttonAnalyseMeters.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonAnalyseMeters.Enabled = false;
      this.buttonAnalyseMeters.Location = new Point(503, 640);
      this.buttonAnalyseMeters.Margin = new Padding(4);
      this.buttonAnalyseMeters.Name = "buttonAnalyseMeters";
      this.buttonAnalyseMeters.Size = new Size(129, 28);
      this.buttonAnalyseMeters.TabIndex = 1;
      this.buttonAnalyseMeters.Text = "Analyse Meters";
      this.buttonAnalyseMeters.UseVisualStyleBackColor = true;
      this.buttonAnalyseMeters.Click += new System.EventHandler(this.buttonAnalyseMeters_Click);
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.tabPageTable);
      this.tabControl1.Controls.Add((Control) this.tabPageSettings);
      this.tabControl1.Location = new Point(-1, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(1183, 603);
      this.tabControl1.TabIndex = 6;
      this.tabPageTable.Controls.Add((Control) this.dataGridView1);
      this.tabPageTable.Location = new Point(4, 25);
      this.tabPageTable.Name = "tabPageTable";
      this.tabPageTable.Padding = new Padding(3);
      this.tabPageTable.Size = new Size(1175, 574);
      this.tabPageTable.TabIndex = 0;
      this.tabPageTable.Text = "Table";
      this.tabPageTable.UseVisualStyleBackColor = true;
      this.tabPageSettings.Controls.Add((Control) this.dateTimePicker1);
      this.tabPageSettings.Controls.Add((Control) this.textBoxFromMeterId);
      this.tabPageSettings.Controls.Add((Control) this.textBoxMaxNumberOfTypes);
      this.tabPageSettings.Controls.Add((Control) this.buttonReduceTypeList);
      this.tabPageSettings.Controls.Add((Control) this.label4);
      this.tabPageSettings.Controls.Add((Control) this.buttonTypeListWithoutMeterDate);
      this.tabPageSettings.Controls.Add((Control) this.textBoxToMeterInfoId);
      this.tabPageSettings.Controls.Add((Control) this.label3);
      this.tabPageSettings.Controls.Add((Control) this.textBoxReduceTypeList);
      this.tabPageSettings.Controls.Add((Control) this.label5);
      this.tabPageSettings.Controls.Add((Control) this.label1);
      this.tabPageSettings.Controls.Add((Control) this.label2);
      this.tabPageSettings.Location = new Point(4, 25);
      this.tabPageSettings.Name = "tabPageSettings";
      this.tabPageSettings.Padding = new Padding(3);
      this.tabPageSettings.Size = new Size(1175, 574);
      this.tabPageSettings.TabIndex = 1;
      this.tabPageSettings.Text = "Settings";
      this.tabPageSettings.UseVisualStyleBackColor = true;
      this.buttonAnalyseAllMeters.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonAnalyseAllMeters.Enabled = false;
      this.buttonAnalyseAllMeters.Location = new Point(328, 640);
      this.buttonAnalyseAllMeters.Margin = new Padding(4);
      this.buttonAnalyseAllMeters.Name = "buttonAnalyseAllMeters";
      this.buttonAnalyseAllMeters.Size = new Size(167, 28);
      this.buttonAnalyseAllMeters.TabIndex = 1;
      this.buttonAnalyseAllMeters.Text = "Analyse all Meters";
      this.buttonAnalyseAllMeters.UseVisualStyleBackColor = true;
      this.buttonAnalyseAllMeters.Click += new System.EventHandler(this.buttonAnalyseMeters_Click);
      this.label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(114, 15);
      this.label5.Margin = new Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(74, 17);
      this.label5.TabIndex = 4;
      this.label5.Text = "Start date:";
      this.dateTimePicker1.Format = DateTimePickerFormat.Short;
      this.dateTimePicker1.Location = new Point(196, 15);
      this.dateTimePicker1.Name = "dateTimePicker1";
      this.dateTimePicker1.Size = new Size(132, 22);
      this.dateTimePicker1.TabIndex = 6;
      this.dateTimePicker1.Value = new DateTime(2000, 1, 1, 0, 0, 0, 0);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1185, 671);
      this.Controls.Add((Control) this.tabControl1);
      this.Controls.Add((Control) this.textBoxState);
      this.Controls.Add((Control) this.buttonAnalyseAllMeters);
      this.Controls.Add((Control) this.buttonAnalyseMeters);
      this.Controls.Add((Control) this.buttonDetailWindow);
      this.Controls.Add((Control) this.buttonMBusListeTypes);
      this.Controls.Add((Control) this.buttonClearData);
      this.Controls.Add((Control) this.buttonBreak);
      this.Controls.Add((Control) this.buttonFunctionUsage);
      this.Controls.Add((Control) this.buttonGenerateTypeInfos);
      this.Controls.Add((Control) this.buttonShowTypeList);
      this.Margin = new Padding(4, 4, 4, 4);
      this.Name = nameof (TypeAnalysis);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (TypeAnalysis);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPageTable.ResumeLayout(false);
      this.tabPageSettings.ResumeLayout(false);
      this.tabPageSettings.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum ExtendedMeterInfoColum
    {
      First_prod,
      Last_prod,
      Meters,
      TypLoad,
      MeterLoad,
      MeterMBusLists,
    }
  }
}
