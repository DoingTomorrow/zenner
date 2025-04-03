// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.TranslatorRecources
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections;
using System.Data;
using System.IO;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class TranslatorRecources
  {
    private ResXResourceWriter MyResourceWriter;
    internal string StandardResPath;
    internal string CompareResPath;
    internal string TranslateResPath;
    internal string RecourcePathRoot = "";
    internal string[] FullFileList;
    internal string[] RecourceNameList;
    internal string[] LanguageNameList;
    internal DataTable TextList;
    private ArrayList MyResource;

    internal bool SearchRecourcePath(string LastPath)
    {
      try
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = "Select the recource base folder";
        folderBrowserDialog.SelectedPath = this.RecourcePathRoot;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
          return false;
        this.RecourcePathRoot = Path.GetFullPath(folderBrowserDialog.SelectedPath);
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Translator", "Path error");
        return false;
      }
      ArrayList TheFiles = new ArrayList();
      this.GetFileList(this.RecourcePathRoot, TheFiles);
      TheFiles.Sort();
      ArrayList arrayList1 = new ArrayList();
      bool flag = false;
      string str1 = "";
      for (int index = 0; index < TheFiles.Count; ++index)
      {
        string[] strArray = Path.GetFileName(TheFiles[index].ToString()).Split('.');
        if (strArray[0] == str1)
        {
          if (flag)
          {
            arrayList1.Add(TheFiles[index - 1]);
            flag = false;
          }
          arrayList1.Add(TheFiles[index]);
        }
        else
        {
          flag = true;
          str1 = strArray[0];
        }
      }
      this.FullFileList = new string[arrayList1.Count];
      for (int index = 0; index < this.FullFileList.Length; ++index)
        this.FullFileList[index] = arrayList1[index].ToString();
      string str2 = "";
      arrayList1.Clear();
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < this.FullFileList.Length; ++index)
      {
        string fullFile = this.FullFileList[index];
        string[] strArray = Path.GetFileName(fullFile).Split('.');
        string str3;
        switch (strArray.Length)
        {
          case 2:
            str3 = strArray[1];
            break;
          case 3:
            str3 = strArray[1] + "." + strArray[2];
            break;
          default:
            continue;
        }
        string str4 = Path.Combine(this.GetRelevantPath(fullFile), strArray[0]);
        if (str2 != str4)
        {
          str2 = str4;
          foreach (string str5 in arrayList1)
          {
            if (str5.ToString() == str2)
            {
              int num = (int) MessageBox.Show("Doppelete Resourcen: " + fullFile);
              break;
            }
          }
          arrayList1.Add((object) str2);
          arrayList2.Add((object) str3);
        }
        else
          arrayList2[arrayList2.Count - 1] = (object) (arrayList2[arrayList2.Count - 1]?.ToString() + ";" + str3);
      }
      this.RecourceNameList = new string[arrayList1.Count];
      this.LanguageNameList = new string[this.RecourceNameList.Length];
      for (int index = 0; index < this.RecourceNameList.Length; ++index)
      {
        this.RecourceNameList[index] = arrayList1[index].ToString();
        this.LanguageNameList[index] = arrayList2[index].ToString();
      }
      return true;
    }

    private string GetRelevantPath(string PathAndFilename)
    {
      return Path.GetDirectoryName(PathAndFilename.Remove(0, this.RecourcePathRoot.Length + 1));
    }

    private string GetRelevantPathAndFileName(string PathAndFilename)
    {
      return PathAndFilename.Remove(0, this.RecourcePathRoot.Length + 1);
    }

    internal int GetResourceIndex(string FileName)
    {
      for (int resourceIndex = 0; resourceIndex < this.RecourceNameList.Length; ++resourceIndex)
      {
        if (this.RecourceNameList[resourceIndex] == FileName)
          return resourceIndex;
      }
      return -1;
    }

    private bool GetFileList(string RecourcePath, ArrayList TheFiles)
    {
      foreach (object file in Directory.GetFiles(RecourcePath, "*.resx"))
        TheFiles.Add(file);
      foreach (string directory in Directory.GetDirectories(RecourcePath))
        this.GetFileList(directory, TheFiles);
      return true;
    }

    internal bool LoadRecources(string Name, string SourceLanguage, string TranslateLanguage)
    {
      string str1 = Name + ".resx";
      for (int index1 = 0; index1 < this.FullFileList.Length; ++index1)
      {
        if (this.GetRelevantPathAndFileName(this.FullFileList[index1]) == str1)
        {
          this.StandardResPath = this.FullFileList[index1];
          ResXResourceReader resXresourceReader1 = new ResXResourceReader(this.StandardResPath);
          string str2 = Name + "." + SourceLanguage;
          this.CompareResPath = "";
          ResXResourceReader resXresourceReader2;
          if (str2 != str1)
          {
            for (int index2 = 0; index2 < this.FullFileList.Length; ++index2)
            {
              if (this.GetRelevantPathAndFileName(this.FullFileList[index2]) == str2)
              {
                this.CompareResPath = this.FullFileList[index2];
                resXresourceReader2 = new ResXResourceReader(this.CompareResPath);
                goto label_13;
              }
            }
            return false;
          }
          resXresourceReader2 = (ResXResourceReader) null;
label_13:
          string str3 = Name + "." + TranslateLanguage;
          for (int index3 = 0; index3 < this.FullFileList.Length; ++index3)
          {
            if (this.GetRelevantPathAndFileName(this.FullFileList[index3]) == str3)
            {
              this.TranslateResPath = this.FullFileList[index3];
              ResXResourceReader resXresourceReader3 = new ResXResourceReader(this.TranslateResPath);
              this.MyResource = new ArrayList();
              foreach (DictionaryEntry dictionaryEntry in resXresourceReader1)
              {
                string str4 = dictionaryEntry.Key.ToString();
                if (dictionaryEntry.Value != null)
                {
                  string str5 = dictionaryEntry.Value.ToString();
                  if (str5.Length != 0)
                  {
                    TranslatorRecources.RecourceItem recourceItem = new TranslatorRecources.RecourceItem();
                    recourceItem.Key = str4;
                    recourceItem.StandardValue = str5;
                    if (str4.EndsWith(".Text") || str4.EndsWith(".Caption") || str4.IndexOf('.') < 0)
                      recourceItem.TextItem = true;
                    this.MyResource.Add((object) recourceItem);
                  }
                }
              }
              resXresourceReader1.Close();
              if (resXresourceReader2 != null)
              {
                foreach (DictionaryEntry dictionaryEntry in resXresourceReader2)
                {
                  string str6 = dictionaryEntry.Key.ToString();
                  foreach (TranslatorRecources.RecourceItem recourceItem in this.MyResource)
                  {
                    if (recourceItem.Key == str6)
                    {
                      recourceItem.SourceValue = dictionaryEntry.Value.ToString();
                      break;
                    }
                  }
                }
                resXresourceReader2.Close();
              }
              foreach (DictionaryEntry dictionaryEntry in resXresourceReader3)
              {
                string str7 = dictionaryEntry.Key.ToString();
                foreach (TranslatorRecources.RecourceItem recourceItem in this.MyResource)
                {
                  if (recourceItem.Key == str7)
                  {
                    recourceItem.TranslateValue = dictionaryEntry.Value.ToString();
                    break;
                  }
                }
              }
              resXresourceReader3.Close();
              this.TextList = new DataTable("RecourceData");
              this.TextList.Columns.Add(new DataColumn()
              {
                ColumnName = "Key"
              });
              this.TextList.Columns.Add(new DataColumn()
              {
                ColumnName = "Compare Language"
              });
              this.TextList.Columns.Add(new DataColumn()
              {
                ColumnName = "Translate Language"
              });
              foreach (TranslatorRecources.RecourceItem recourceItem in this.MyResource)
              {
                if (recourceItem.TextItem)
                {
                  DataRow row = this.TextList.NewRow();
                  row[0] = (object) recourceItem.Key;
                  row[1] = !(recourceItem.SourceValue == "") ? (object) recourceItem.SourceValue : (object) recourceItem.StandardValue;
                  row[2] = (object) recourceItem.TranslateValue;
                  this.TextList.Rows.Add(row);
                }
              }
              return true;
            }
          }
          return false;
        }
      }
      return false;
    }

    internal bool SaveText(string IdentString, string TextToSave)
    {
      this.MyResourceWriter = new ResXResourceWriter(this.TranslateResPath);
      foreach (TranslatorRecources.RecourceItem recourceItem in this.MyResource)
      {
        if (recourceItem.Key == IdentString)
          recourceItem.TranslateValue = TextToSave;
        if (recourceItem.TranslateValue.Length > 0)
          this.MyResourceWriter.AddResource(recourceItem.Key, recourceItem.TranslateValue);
      }
      this.MyResourceWriter.Generate();
      this.MyResourceWriter.Close();
      foreach (DataRow row in (InternalDataCollectionBase) this.TextList.Rows)
      {
        if (row[0].ToString() == IdentString)
        {
          row[2] = (object) TextToSave;
          break;
        }
      }
      return true;
    }

    internal bool ExportResources()
    {
      WaitWindow waitWindow;
      try
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = "Select the base folder of the copy";
        folderBrowserDialog.SelectedPath = this.RecourcePathRoot;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
          return false;
        string fullPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
        waitWindow = new WaitWindow();
        waitWindow.labelWaitText.Text = "Copy Files";
        waitWindow.progressBarWait2.Visible = false;
        waitWindow.progressBarWait1.Minimum = 0;
        waitWindow.progressBarWait1.Maximum = this.FullFileList.Length;
        waitWindow.Show();
        waitWindow.BringToFront();
        for (int index = 0; index < this.FullFileList.Length; ++index)
        {
          waitWindow.progressBarWait1.Value = index;
          waitWindow.Refresh();
          string fullFile = this.FullFileList[index];
          string str = Path.Combine(fullPath, this.GetRelevantPathAndFileName(fullFile));
          Directory.CreateDirectory(Path.GetDirectoryName(str));
          File.Copy(fullFile, str, true);
          string fileName = Path.GetFileName(fullFile);
          if (Path.GetExtension(fileName.Substring(0, fileName.Length - 5)).Length == 0)
          {
            string destFileName = str.Insert(str.Length - 5, ".std");
            File.Copy(fullFile, destFileName, true);
          }
        }
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Translator", "Copy path error");
        return false;
      }
      waitWindow?.Close();
      return true;
    }

    private class RecourceItem
    {
      internal bool TextItem;
      internal string Key;
      internal string StandardValue;
      internal string SourceValue;
      internal string TranslateValue;

      public RecourceItem()
      {
        this.TextItem = false;
        this.SourceValue = "";
        this.TranslateValue = "";
      }
    }
  }
}
