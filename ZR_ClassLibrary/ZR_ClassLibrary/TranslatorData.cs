// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.TranslatorData
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ZR_ClassLibrary
{
  public class TranslatorData
  {
    internal bool generatingExit = false;
    private string ErrorText;
    private WaitWindow MyWaitWindow;
    private EventTime MyEventTime;
    private long LastTicks;
    private DbBasis MyDB;
    private ZRDataAdapter MyDataAdapter;
    private int LastFieldIndex;
    private string[] FieldNames;
    private string[] TableNames;
    private int[] FieldSizes;

    public TranslatorData()
    {
      this.MyDB = DbBasis.PrimaryDB;
      this.ErrorText = "";
    }

    internal string getLastErrorMsg()
    {
      string errorText = this.ErrorText;
      this.ErrorText = "";
      return errorText;
    }

    private void addErrorText(string Error)
    {
      this.ErrorText += Error;
      this.ErrorText += Environment.NewLine;
    }

    public string[] GetAllTranslatedLanguageNames(string LanguageCode)
    {
      string[] translatedLanguageNames = (string[]) null;
      Schema.DBTranslatorDataTable translatorDataTable = new Schema.DBTranslatorDataTable();
      string SqlCommand = "select * from DBTranslator where TableName = '@BasicLanguageText' AND TableKey = '" + LanguageCode + "'";
      try
      {
        using (IDbConnection dbConnection = this.MyDB.GetDbConnection())
        {
          this.MyDataAdapter = this.MyDB.ZRDataAdapter(SqlCommand, dbConnection);
          this.MyDataAdapter.Fill((DataTable) translatorDataTable);
          if (translatorDataTable.Rows.Count != 1)
          {
            this.MyDataAdapter = this.MyDB.ZRDataAdapter("select * from DBTranslator where TableName = '@BasicLanguageText' AND TableKey = 'en'", dbConnection);
            this.MyDataAdapter.Fill((DataTable) translatorDataTable);
          }
          if (translatorDataTable.Rows.Count == 1)
          {
            ArrayList arrayList = new ArrayList();
            for (int index = 0; index < translatorDataTable.Columns.Count; ++index)
            {
              if (translatorDataTable.Columns[index].ColumnName.ToUpper().StartsWith("LANG_") && !translatorDataTable.Rows[0][index].ToString().StartsWith("!"))
                arrayList.Add((object) translatorDataTable.Rows[0][index].ToString());
            }
            translatedLanguageNames = new string[arrayList.Count];
            arrayList.Sort();
            for (int index = 0; index < arrayList.Count; ++index)
              translatedLanguageNames[index] = arrayList[index].ToString();
          }
          else
            goto label_18;
        }
      }
      catch
      {
        goto label_18;
      }
      return translatedLanguageNames;
label_18:
      int num = (int) GMM_MessageBox.ShowMessage("Global Meter Manager", "Language error on database" + Environment.NewLine, true);
      return new string[1]{ "No language available" };
    }

    public string GetLanguageColumnNameFromTranslatedLanguage(
      string LanguageCode,
      string LanguageName)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      bool flag = false;
      try
      {
        DataTable dataTable = new DataTable("DBTranslator");
        string SqlCommand = "select * from DBTranslator where TableName = '@BasicLanguageText' AND TableKey = '" + LanguageCode + "'";
        dbConnection.Open();
        flag = true;
        primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(dataTable);
        dbConnection.Close();
        flag = false;
        if (dataTable.Rows.Count != 1)
        {
          int num = (int) MessageBox.Show("Language error on database");
        }
        else
        {
          for (int index = 0; index < dataTable.Columns.Count; ++index)
          {
            string upper = dataTable.Columns[index].ColumnName.ToUpper();
            if (upper.StartsWith("LANG_") && LanguageName == dataTable.Rows[0][index].ToString())
              return upper;
          }
        }
        return string.Empty;
      }
      catch (Exception ex)
      {
        string text = ex.ToString();
        if (flag)
          dbConnection.Close();
        int num = (int) MessageBox.Show(text);
        return string.Empty;
      }
    }

    public string GetDatabaseLanguageCode()
    {
      string languageColumnName = this.GetDatabaseLanguageColumnName();
      return languageColumnName.Length != 7 ? "" : languageColumnName.Remove(0, 5).ToLower();
    }

    public string GetDatabaseLanguageColumnName()
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      bool flag = false;
      try
      {
        DataTable dataTable = new DataTable("DBTranslator");
        string SqlCommand = "select * from DBTranslator where TableName = '@BasicLanguageText' AND TableKey = 'actual'";
        dbConnection.Open();
        flag = true;
        primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(dataTable);
        dbConnection.Close();
        flag = false;
        if (dataTable.Rows.Count == 1)
          return dataTable.Rows[0]["FieldName"].ToString().ToUpper();
        int num = (int) MessageBox.Show("Language error on database");
        return string.Empty;
      }
      catch (Exception ex)
      {
        string text = ex.ToString();
        if (flag)
          dbConnection.Close();
        int num = (int) MessageBox.Show(text);
        return string.Empty;
      }
    }

    public string GetTranslatedLanguageText(string LanguageCode, string ColumnName)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      bool flag = false;
      try
      {
        DataTable dataTable = new DataTable("DBTranslator");
        string SqlCommand = "select * from DBTranslator where TableName = '@BasicLanguageText' AND TableKey = '" + LanguageCode + "'";
        dbConnection.Open();
        flag = true;
        primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(dataTable);
        dbConnection.Close();
        flag = false;
        if (dataTable.Rows.Count == 1)
          return dataTable.Rows[0][ColumnName].ToString();
        int num = (int) MessageBox.Show("Language error on database");
        return string.Empty;
      }
      catch (Exception ex)
      {
        string text = ex.ToString();
        if (flag)
          dbConnection.Close();
        int num = (int) MessageBox.Show(text);
        return string.Empty;
      }
    }

    public string GetDatabaseLanguageName()
    {
      string languageColumnName = this.GetDatabaseLanguageColumnName();
      string str = languageColumnName.Remove(0, 5);
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      bool flag = false;
      try
      {
        DataTable dataTable = new DataTable("DBTranslator");
        string SqlCommand = "select * from DBTranslator where TableName = '@BasicLanguageText' AND TableKey = '" + str + "'";
        dbConnection.Open();
        flag = true;
        primaryDb.ZRDataAdapter(SqlCommand, dbConnection).Fill(dataTable);
        dbConnection.Close();
        flag = false;
        if (dataTable.Rows.Count == 1)
          return dataTable.Rows[0][languageColumnName].ToString().ToUpper();
        int num = (int) MessageBox.Show("Language error on database");
        return string.Empty;
      }
      catch (Exception ex)
      {
        string text = ex.ToString();
        if (flag)
          dbConnection.Close();
        int num = (int) MessageBox.Show(text);
        return string.Empty;
      }
    }

    public bool translateDBToLanguage(
      string LanguageCode,
      string toLanguageFieldName,
      bool IncludeTextIds,
      bool showWaitWindow)
    {
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      primaryDb.DbCommand(dbConnection);
      bool flag = false;
      try
      {
        string str = "** Translating database language **" + Environment.NewLine + Environment.NewLine + "to '" + this.GetTranslatedLanguageText(LanguageCode, toLanguageFieldName) + "'" + Environment.NewLine + Environment.NewLine + "This may take several minutes. " + Environment.NewLine + "Do not interrupt!";
        if (showWaitWindow)
        {
          this.MyWaitWindow = new WaitWindow();
          this.MyWaitWindow.labelWaitText.Text = str;
          this.MyWaitWindow.StartPosition = FormStartPosition.CenterParent;
          this.MyWaitWindow.Show();
        }
        Application.DoEvents();
        this.MyEventTime = new EventTime();
        this.LastTicks = this.MyEventTime.GetTimeTicks();
        DataTable dataTable1 = new DataTable("TransTab");
        string SqlCommand1 = "select TextID," + toLanguageFieldName + ",LANG_en,LANG_de,FieldName,TableKey,TableName from DBTranslator WHERE LANG_de NOT LIKE '##=%' AND TableName NOT LIKE '@%'";
        dbConnection.Open();
        flag = true;
        primaryDb.ZRDataAdapter(SqlCommand1, dbConnection).Fill(dataTable1);
        string Fehlerstring;
        if (!this.transferlanguageText(dataTable1, toLanguageFieldName, IncludeTextIds, out Fehlerstring))
        {
          if (flag)
            dbConnection.Close();
          int num = (int) MessageBox.Show(Fehlerstring, "Translate", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        DataTable dataTable2 = new DataTable("TransTabDouble");
        string SqlCommand2 = "select TextID," + toLanguageFieldName + ",LANG_en,LANG_de,FieldName,TableKey,TableName from DBTranslator WHERE LANG_de LIKE '##=%' AND TableName NOT LIKE '@%'";
        primaryDb.ZRDataAdapter(SqlCommand2, dbConnection).Fill(dataTable2);
        if (!this.transferlanguageTextDouble(dataTable1, dataTable2, toLanguageFieldName, IncludeTextIds, out Fehlerstring))
        {
          if (flag)
            dbConnection.Close();
          int num = (int) MessageBox.Show(Fehlerstring, "Translate", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        Schema.DBTranslatorDataTable MyDataTable = new Schema.DBTranslatorDataTable();
        string SqlCommand3 = "Select * from  DBTranslator where TableKey = 'actual' AND TableName = '@BasicLanguageText'";
        ZRDataAdapter zrDataAdapter = primaryDb.ZRDataAdapter(SqlCommand3, dbConnection);
        zrDataAdapter.Fill((DataTable) MyDataTable);
        MyDataTable[0].FieldName = toLanguageFieldName;
        zrDataAdapter.Update((DataTable) MyDataTable);
        dbConnection.Close();
        flag = false;
        if (showWaitWindow)
        {
          this.MyWaitWindow.Close();
          this.MyWaitWindow = (WaitWindow) null;
        }
        return true;
      }
      catch (Exception ex)
      {
        string text = ex.ToString();
        if (flag)
          dbConnection.Close();
        int num = (int) MessageBox.Show(text, "Translate", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private bool transferlanguageText(
      DataTable SourceTab,
      string toLanguageName,
      bool IncludeTextIds,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      int num = 0;
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      IDbCommand DbCommand = primaryDb.DbCommand(dbConnection);
      bool flag = false;
      try
      {
        dbConnection.Open();
        flag = true;
        foreach (DataRow row in (InternalDataCollectionBase) SourceTab.Rows)
        {
          string str = this.GetFieldText(row, toLanguageName, IncludeTextIds);
          if (str.IndexOf("'") > 0)
            str = str.Replace("'", "''");
          StringBuilder stringBuilder = new StringBuilder(10000);
          stringBuilder.Append("select * from  ");
          stringBuilder.Append(row["TableName"].ToString());
          stringBuilder.Append(" WHERE ");
          stringBuilder.Append(row["TableKey"].ToString());
          DataTable dataTable = new DataTable();
          primaryDb.ZRDataAdapter(stringBuilder.ToString(), dbConnection).Fill(dataTable);
          if (dataTable.Rows.Count != 0)
          {
            dataTable.Rows[0][row["FieldName"].ToString()] = (object) str;
            DbCommand.CommandText = "select * from  " + row["TableName"].ToString();
            primaryDb.ZRDataAdapter(DbCommand).Update(dataTable);
            ++num;
          }
        }
        dbConnection.Close();
        flag = false;
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        if (flag)
          dbConnection.Close();
        return false;
      }
    }

    private string GetFieldText(DataRow TheRow, string LanguageName, bool IncludeTextIds)
    {
      string fieldText = TheRow[LanguageName].ToString();
      if (fieldText == "")
      {
        fieldText = TheRow["LANG_en"].ToString();
        if (fieldText == "")
        {
          TheRow["TableName"].ToString();
          TheRow["FieldName"].ToString();
          fieldText = TheRow["LANG_de"].ToString();
          if (fieldText == "")
            fieldText = "Text undefined";
        }
      }
      if (IncludeTextIds)
      {
        int maxFieldSize = this.GetMaxFieldSize(TheRow["TableName"].ToString(), TheRow["FieldName"].ToString());
        if (maxFieldSize > 0)
        {
          fieldText = "#" + TheRow["TextID"].ToString() + " " + fieldText;
          if (fieldText.Length > maxFieldSize)
            fieldText = fieldText.Substring(0, maxFieldSize);
        }
      }
      return fieldText;
    }

    private bool transferlanguageTextDouble(
      DataTable SourceTab,
      DataTable SourceDoubleTab,
      string toLanguageName,
      bool IncludeTextIds,
      out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      int num = 0;
      DbBasis primaryDb = DbBasis.PrimaryDB;
      IDbConnection dbConnection = primaryDb.GetDbConnection();
      IDbCommand DbCommand = primaryDb.DbCommand(dbConnection);
      bool flag = false;
      try
      {
        foreach (DataRow row in (InternalDataCollectionBase) SourceDoubleTab.Rows)
        {
          string str1 = row["LANG_de"].ToString().Remove(0, 3);
          DataRow[] dataRowArray = SourceTab.Select("TextID = " + str1);
          if (dataRowArray.Length == 1)
          {
            string str2 = this.GetFieldText(dataRowArray[0], toLanguageName, IncludeTextIds);
            if (str2.IndexOf("'") > 0)
              str2 = str2.Replace("'", "''");
            StringBuilder stringBuilder = new StringBuilder(10000);
            stringBuilder.Append("Select * from ");
            stringBuilder.Append(row["TableName"].ToString());
            stringBuilder.Append(" WHERE ");
            stringBuilder.Append(row["TableKey"].ToString());
            DataTable dataTable = new DataTable();
            primaryDb.ZRDataAdapter(stringBuilder.ToString(), dbConnection).Fill(dataTable);
            dataTable.Rows[0][row["FieldName"].ToString()] = (object) str2;
            DbCommand.CommandText = "select * from  " + row["TableName"].ToString();
            primaryDb.ZRDataAdapter(DbCommand).Update(dataTable);
            ++num;
          }
        }
        dbConnection.Close();
        flag = false;
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        if (flag)
          dbConnection.Close();
        return false;
      }
    }

    internal int GetMaxFieldSize(string TableName, string FieldName)
    {
      try
      {
        if (this.FieldNames == null)
        {
          DataTable dataTable = new DataTable("DBTranslatorStruct");
          using (IDbConnection dbConnection = this.MyDB.GetDbConnection())
          {
            this.MyDataAdapter = this.MyDB.ZRDataAdapter("Select * from DBTranslatorStruct", dbConnection);
            this.MyDataAdapter.Fill(dataTable);
          }
          this.FieldNames = new string[dataTable.Rows.Count];
          this.TableNames = new string[dataTable.Rows.Count];
          this.FieldSizes = new int[dataTable.Rows.Count];
          for (int index = 0; index < dataTable.Rows.Count; ++index)
          {
            this.FieldNames[index] = dataTable.Rows[index][nameof (FieldName)].ToString();
            this.TableNames[index] = dataTable.Rows[index][nameof (TableName)].ToString();
            this.FieldSizes[index] = int.Parse(dataTable.Rows[index]["FieldSize"].ToString());
            if (this.FieldNames[index] == FieldName && this.TableNames[index] == TableName)
              this.LastFieldIndex = index;
          }
          return this.FieldSizes[this.LastFieldIndex];
        }
        if (this.FieldNames[this.LastFieldIndex] == FieldName && this.TableNames[this.LastFieldIndex] == TableName)
          return this.FieldSizes[this.LastFieldIndex];
        for (int index = 0; index < this.FieldSizes.Length; ++index)
        {
          if (this.FieldNames[index] == FieldName && this.TableNames[index] == TableName)
          {
            this.LastFieldIndex = index;
            return this.FieldSizes[this.LastFieldIndex];
          }
        }
        return 0;
      }
      catch
      {
        return 0;
      }
    }

    public ArrayList GetTranslatorTextBlock(string BlockName)
    {
      try
      {
        string str = "LANG_" + Thread.CurrentThread.CurrentUICulture.ToString();
        int length;
        if ((length = str.IndexOf('-')) >= 0)
          str = str.Substring(0, length);
        string SqlCommand = "SELECT  FieldName," + str + " FROM DBTranslator WHERE TableName = '" + BlockName + "' ORDER BY TableKey";
        DataTable dataTable = new DataTable();
        using (IDbConnection dbConnection = this.MyDB.GetDbConnection())
        {
          this.MyDataAdapter = this.MyDB.ZRDataAdapter(SqlCommand, dbConnection);
          this.MyDataAdapter.Fill(dataTable);
        }
        ArrayList translatorTextBlock = new ArrayList();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          translatorTextBlock.Add((object) new TranslatorData.TranslatorTextItem()
          {
            TextInfo = row[0].ToString(),
            Text = row[1].ToString()
          });
        return translatorTextBlock;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error at GetTranslatorTextBlock" + Environment.NewLine + ex.ToString(), "Read database error!");
        return (ArrayList) null;
      }
    }

    public struct TranslatorTextItem
    {
      public string TextInfo;
      public string Text;
    }
  }
}
