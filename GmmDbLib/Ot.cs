// Decompiled with JetBrains decompiler
// Type: GmmDbLib.Ot
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace GmmDbLib
{
  public class Ot
  {
    private static Logger logger = LogManager.GetLogger("OnlineTranslator");
    public static bool ShowMessageNumber = true;
    private const string defaultNamespace = "default";
    private const string fileChacheFolder = "Languages";
    private static SortedList<Tg, SortedList<string, string>> TranslatedDataCache;
    private static SortedList<string, string> BaseMessagesCache;
    private static string CurrentLanguage = string.Empty;
    private static Logger OnlineTranslatorLogger = LogManager.GetLogger("OnlineTranslator");

    public static string CachePath
    {
      get
      {
        return Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ZENNER"), "GMM"), "Cache");
      }
    }

    public static string Gtt(Tg translationGroup, string textKey, string defaultText)
    {
      if (DbBasis.PrimaryDB == null || string.IsNullOrEmpty(textKey))
        return defaultText;
      if (Ot.OnLanguageTranslatedMessage != null)
        Ot.OnLanguageTranslatedMessage((object) null, new TranslationListText(translationGroup, textKey, defaultText));
      string languageTextBase = Ot.GetLanguageTextBase(translationGroup, textKey, defaultText);
      if (Ot.logger.IsDebugEnabled)
        Ot.logger.Debug("Text: '" + textKey + "' translated to: " + languageTextBase);
      return languageTextBase;
    }

    public static string Gtt(Tg translationGroup, string defaultText)
    {
      string textKey = Ot.KeyFromDefaultText(defaultText);
      if (Ot.logger.IsDebugEnabled)
        Ot.logger.Debug("Generated translator key: '" + textKey + "' translated from: " + defaultText);
      return Ot.Gtt(translationGroup, textKey, defaultText);
    }

    public static string Gtm(Tg translationGroup, string textKey, string defaultText)
    {
      if (DbBasis.PrimaryDB == null)
        return defaultText;
      if (Ot.OnLanguageTranslatedMessage != null)
        Ot.OnLanguageTranslatedMessage((object) null, new TranslationListText(translationGroup, textKey, defaultText, true));
      string messageText = Ot.GetLanguageTextBase(translationGroup, textKey, defaultText);
      if (!Ot.ShowMessageNumber)
        messageText = Ot.GetMessageTextWithoutNumber(messageText);
      if (Ot.logger.IsDebugEnabled)
        Ot.logger.Debug("Message: '" + textKey + "' translated to: " + messageText);
      return messageText;
    }

    public static string Gtm(Tg translationGroup, string defaultText)
    {
      return Ot.Gtm(translationGroup, Ot.KeyFromDefaultText(defaultText), defaultText);
    }

    public static string GetBaseMessage(Exception exceptionTree)
    {
      return Ot.GetBaseMessage(Ot.GetMessageNumbersTree(exceptionTree));
    }

    public static event EventHandler<TranslationListText> OnLanguageTranslatedMessage;

    public static bool UseLanguageFileCache { get; set; }

    public static Ot.LanguageTableTypes LanguageTableType { get; private set; }

    static Ot()
    {
      Ot.TranslatedDataCache = new SortedList<Tg, SortedList<string, string>>();
      Ot.LanguageTableType = Ot.LanguageTableTypes.Unknown;
      Ot.UseLanguageFileCache = false;
    }

    private static string GetLanguageTextBase(Tg translationGroup, string key, string defaultText)
    {
      int index1 = Ot.TranslatedDataCache.IndexOfKey(translationGroup);
      if (index1 >= 0)
      {
        SortedList<string, string> sortedList = Ot.TranslatedDataCache.Values[index1];
        int index2 = sortedList.IndexOfKey(key);
        return index2 >= 0 ? sortedList.Values[index2] : defaultText;
      }
      if (Ot.GarantLanguageNamespaceCached(translationGroup))
      {
        int index3 = Ot.TranslatedDataCache.IndexOfKey(translationGroup);
        if (index3 >= 0)
        {
          SortedList<string, string> sortedList = Ot.TranslatedDataCache.Values[index3];
          int index4 = sortedList.IndexOfKey(key);
          if (index4 >= 0)
            return sortedList.Values[index4];
        }
      }
      return defaultText;
    }

    private static string GetBaseMessage(string messageNumbersTree)
    {
      if (Ot.BaseMessagesCache == null)
      {
        Ot.BaseMessagesCache = new SortedList<string, string>();
        try
        {
          using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
          {
            DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM OnlineTranslationBaseMassages", newConnection);
            BaseTables.OnlineTranslationBaseMassagesDataTable massagesDataTable = new BaseTables.OnlineTranslationBaseMassagesDataTable();
            dataAdapter.Fill((DataTable) massagesDataTable);
            foreach (BaseTables.OnlineTranslationBaseMassagesRow translationBaseMassagesRow in (TypedTableBase<BaseTables.OnlineTranslationBaseMassagesRow>) massagesDataTable)
              Ot.BaseMessagesCache.Add(translationBaseMassagesRow.MessageTree, Ot.Gtm(Tg.BaseErrors, translationBaseMassagesRow.MessageTree, translationBaseMassagesRow.DefaultText));
          }
        }
        catch
        {
        }
      }
      int index = Ot.BaseMessagesCache.IndexOfKey(messageNumbersTree);
      return index >= 0 ? Ot.BaseMessagesCache.Values[index] : (string) null;
    }

    private static bool GarantLanguageNamespaceCached(Tg translationGroup)
    {
      lock (Ot.TranslatedDataCache)
      {
        if (!Ot.GarantTranslatorInitialised())
          return false;
        if (Ot.TranslatedDataCache.ContainsKey(translationGroup))
          return true;
        SortedList<string, string> textList = Ot.GetLanguageFromFileCache(translationGroup);
        if (textList == null)
        {
          textList = Ot.LoadOnlineTranslation(Ot.CurrentLanguage, translationGroup);
          if (textList == null)
            textList = new SortedList<string, string>();
          else
            Ot.WriteLanguageToFileCache(translationGroup, textList);
        }
        Ot.TranslatedDataCache.Add(translationGroup, textList);
        return true;
      }
    }

    private static bool IfLanguageChangeAllowed()
    {
      return Ot.LanguageTableType == Ot.LanguageTableTypes.UniversalNamespaceType;
    }

    public static bool GarantTranslatorInitialised()
    {
      if (Ot.LanguageTableType == Ot.LanguageTableTypes.InitialisationError)
        return false;
      if (Ot.LanguageTableType != 0)
        return true;
      try
      {
        Ot.CurrentLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          try
          {
            DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM OnlineTranslations WHERE 1=0", newConnection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            Ot.LanguageTableType = !dataTable.Columns.Contains("TranslationGroup") ? Ot.LanguageTableTypes.UniversalType : Ot.LanguageTableTypes.UniversalNamespaceType;
          }
          catch (Exception ex)
          {
            Ot.OnlineTranslatorLogger.Fatal(ex.Message);
            Ot.LanguageTableType = Ot.LanguageTableTypes.OnlyEnglishType;
          }
        }
      }
      catch (Exception ex)
      {
        Ot.OnlineTranslatorLogger.Fatal(ex.Message);
        Ot.LanguageTableType = Ot.LanguageTableTypes.InitialisationError;
        return false;
      }
      return true;
    }

    private static SortedList<string, string> LoadOnlineTranslation(
      string language,
      Tg translationGroup)
    {
      SortedList<string, string> sortedList = new SortedList<string, string>();
      string lower = language.ToLower();
      DateTime now = DateTime.Now;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          if (Ot.LanguageTableType != Ot.LanguageTableTypes.OnlyEnglishType)
          {
            DbCommand command1 = newConnection.CreateCommand();
            DbCommand command2 = newConnection.CreateCommand();
            if (Ot.LanguageTableType == Ot.LanguageTableTypes.UniversalNamespaceType)
            {
              string str1 = ((int) translationGroup).ToString();
              command1.CommandText = "SELECT TextKey,LanguageText,MessageNumber FROM OnlineTranslations WHERE LanguageCode = '" + lower + "' AND TranslationGroup = " + str1 + " ORDER BY TextKey ASC";
              command2.CommandText = "SELECT TextKey,LanguageText,MessageNumber FROM OnlineTranslations WHERE LanguageCode = 'en' AND TranslationGroup = " + str1 + " ORDER BY TextKey ASC";
              DbDataReader dbDataReader1 = command1.ExecuteReader();
              while (dbDataReader1.Read())
              {
                string key = dbDataReader1["TextKey"].ToString();
                string str2 = dbDataReader1["LanguageText"].ToString();
                if (dbDataReader1["MessageNumber"] != DBNull.Value)
                  str2 = "@" + ((int) dbDataReader1["MessageNumber"]).ToString() + " " + str2;
                sortedList.Add(key, str2);
              }
              dbDataReader1.Close();
              DbDataReader dbDataReader2 = command2.ExecuteReader();
              while (dbDataReader2.Read())
              {
                string key = dbDataReader2["TextKey"].ToString();
                string str3 = dbDataReader2["LanguageText"].ToString();
                if (dbDataReader2["MessageNumber"] != DBNull.Value)
                  str3 = "@" + ((int) dbDataReader2["MessageNumber"]).ToString() + " " + str3;
                if (!sortedList.ContainsKey(key))
                  sortedList.Add(key, str3);
              }
              dbDataReader2.Close();
            }
            else
            {
              command1.CommandText = "SELECT TextKey,LanguageText FROM OnlineTranslations WHERE LanguageCode = '" + lower + "' ORDER BY TextKey ASC";
              command2.CommandText = "SELECT TextKey,LanguageText FROM OnlineTranslations WHERE LanguageCode = 'en' ORDER BY TextKey ASC";
              DbDataReader dbDataReader3 = command1.ExecuteReader();
              while (dbDataReader3.Read())
              {
                string key = dbDataReader3["TextKey"].ToString();
                string str = dbDataReader3["LanguageText"].ToString();
                sortedList.Add(key, str);
              }
              dbDataReader3.Close();
              DbDataReader dbDataReader4 = command2.ExecuteReader();
              while (dbDataReader4.Read())
              {
                string key = dbDataReader4["TextKey"].ToString();
                string str = dbDataReader4["LanguageText"].ToString();
                if (!sortedList.ContainsKey(key))
                  sortedList.Add(key, str);
              }
              dbDataReader4.Close();
            }
          }
          else
          {
            string name1 = "TextKey";
            string name2 = "TextEN";
            string name3 = "Text" + language.ToUpper();
            string str4 = name3;
            if (str4 != name2)
              str4 = name2 + "," + name3;
            DbCommand command = newConnection.CreateCommand();
            command.CommandText = "SELECT " + name1 + ", " + str4 + " FROM OnlineTranslation ORDER BY TextKey ASC;";
            DbDataReader dbDataReader = command.ExecuteReader();
            while (dbDataReader.Read())
            {
              string key = dbDataReader[name1].ToString();
              string str5 = dbDataReader[name3].ToString();
              if (str5 == "")
              {
                str5 = dbDataReader[name2].ToString();
                if (str5 == "")
                  str5 = key;
              }
              sortedList.Add(key, str5);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error on load online translation language", ex.InnerException);
      }
      if (Ot.OnlineTranslatorLogger.IsTraceEnabled)
        Ot.OnlineTranslatorLogger.Trace("Translation group '" + translationGroup.ToString() + "' loaded from database. Load time[ms]:" + DateTime.Now.Subtract(now).TotalMilliseconds.ToString("N0") + " Number of translations:" + sortedList.Count.ToString());
      return sortedList;
    }

    public static bool AddOrChangeOnlineTranslation(
      string language,
      Tg translationGroup,
      string key,
      string newLanguageText,
      bool isMessage)
    {
      return Ot.AddOrChangeOnlineTranslation(language, translationGroup, key, newLanguageText, isMessage, out int? _);
    }

    public static bool AddOrChangeOnlineTranslation(
      string language,
      Tg translationGroup,
      string key,
      string newLanguageText,
      bool isMessage,
      out int? messageNumber)
    {
      bool flag = false;
      messageNumber = new int?();
      if (!Ot.IfLanguageChangeAllowed())
        throw new Exception("AddOrChangeOnlineTranslation not allowed.");
      if (DbBasis.PrimaryDB == null || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(newLanguageText))
        throw new Exception("Change online translation: Parameter error.");
      try
      {
        string str = ((int) translationGroup).ToString();
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM OnlineTranslations WHERE TranslationGroup = " + str + " AND TextKey = '" + key + "'", newConnection, out DbCommandBuilder _);
          BaseTables.OnlineTranslationsDataTable translationsDataTable = new BaseTables.OnlineTranslationsDataTable();
          dataAdapter.Fill((DataTable) translationsDataTable);
          BaseTables.OnlineTranslationsRow onlineTranslationsRow1 = (BaseTables.OnlineTranslationsRow) null;
          foreach (BaseTables.OnlineTranslationsRow onlineTranslationsRow2 in (TypedTableBase<BaseTables.OnlineTranslationsRow>) translationsDataTable)
          {
            if (onlineTranslationsRow2.LanguageCode == language)
              onlineTranslationsRow1 = onlineTranslationsRow2;
            if (!onlineTranslationsRow2.IsMessageNumberNull())
            {
              if (!messageNumber.HasValue)
              {
                messageNumber = new int?(onlineTranslationsRow2.MessageNumber);
              }
              else
              {
                int? nullable = messageNumber;
                int messageNumber1 = onlineTranslationsRow2.MessageNumber;
                if (!(nullable.GetValueOrDefault() == messageNumber1 & nullable.HasValue))
                  throw new Exception("Different message number on languages: " + onlineTranslationsRow2.LanguageText);
              }
            }
          }
          if (onlineTranslationsRow1 != null)
          {
            if (onlineTranslationsRow1.LanguageText != newLanguageText)
            {
              onlineTranslationsRow1.LanguageText = newLanguageText;
              flag = true;
            }
          }
          else
          {
            BaseTables.OnlineTranslationsRow row = translationsDataTable.NewOnlineTranslationsRow();
            row.TranslationGroup = (int) translationGroup;
            row.LanguageCode = language;
            row.TextKey = key;
            row.LanguageText = newLanguageText;
            translationsDataTable.AddOnlineTranslationsRow(row);
            flag = true;
          }
          if (flag)
          {
            if (isMessage && !messageNumber.HasValue)
              messageNumber = new int?(Ot.GetNextMessageNumber(newConnection));
            if (messageNumber.HasValue)
            {
              foreach (BaseTables.OnlineTranslationsRow onlineTranslationsRow3 in (TypedTableBase<BaseTables.OnlineTranslationsRow>) translationsDataTable)
              {
                onlineTranslationsRow3.MessageNumber = messageNumber.Value;
                int num;
                if (!onlineTranslationsRow3.IsMessageNumberNull())
                {
                  int messageNumber2 = onlineTranslationsRow3.MessageNumber;
                  int? nullable = messageNumber;
                  int valueOrDefault = nullable.GetValueOrDefault();
                  num = !(messageNumber2 == valueOrDefault & nullable.HasValue) ? 1 : 0;
                }
                else
                  num = 1;
                if (num != 0)
                  flag = true;
              }
            }
            dataAdapter.Update((DataTable) translationsDataTable);
            Ot.CleareCache();
            if (language == Ot.CurrentLanguage)
            {
              int index1 = Ot.TranslatedDataCache.IndexOfKey(translationGroup);
              if (index1 >= 0)
              {
                SortedList<string, string> sortedList = Ot.TranslatedDataCache.Values[index1];
                int index2 = sortedList.IndexOfKey(key);
                if (index2 >= 0)
                  sortedList.RemoveAt(index2);
                sortedList.Add(key, Ot.IncludeMessageNumberInLanguageText(newLanguageText, messageNumber));
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Change online translation error.", ex);
      }
      return flag;
    }

    private static bool DeleteOnlineTranslation(string language, Tg translationGroup, string key)
    {
      bool flag = false;
      if (!Ot.IfLanguageChangeAllowed())
        throw new Exception("AddOrChangeOnlineTranslation not allowed.");
      if (DbBasis.PrimaryDB == null || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(key))
        throw new Exception("Change online translation: Parameter error.");
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM OnlineTranslations WHERE TranslationGroup = " + ((int) translationGroup).ToString() + " AND TextKey = '" + key + "' AND LanguageCode = '" + language + "'", newConnection, out DbCommandBuilder _);
          BaseTables.OnlineTranslationsDataTable translationsDataTable = new BaseTables.OnlineTranslationsDataTable();
          dataAdapter.Fill((DataTable) translationsDataTable);
          if (translationsDataTable.Count == 1)
          {
            translationsDataTable[0].Delete();
            dataAdapter.Update((DataTable) translationsDataTable);
            flag = true;
            if (language == Ot.CurrentLanguage)
            {
              int index1 = Ot.TranslatedDataCache.IndexOfKey(translationGroup);
              if (index1 >= 0)
              {
                SortedList<string, string> sortedList = Ot.TranslatedDataCache.Values[index1];
                int index2 = sortedList.IndexOfKey(key);
                if (index2 >= 0)
                  sortedList.RemoveAt(index2);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Change online translation error.", ex);
      }
      return flag;
    }

    public static int GetNextMessageNumber(DbConnection dbConnection)
    {
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT MAX(MessageNumber) AS MaxMessageNumber FROM OnlineTranslations", dbConnection);
      DataTable dataTable = new DataTable();
      dataAdapter.Fill(dataTable);
      if (dataTable.Rows.Count != 1)
        throw new Exception("MessageNumber generation error");
      string s = dataTable.Rows[0]["MaxMessageNumber"].ToString();
      return string.IsNullOrEmpty(s) ? 1 : int.Parse(s) + 1;
    }

    private static void WriteLanguageToFileCache(
      Tg translationGroup,
      SortedList<string, string> textList)
    {
      if (!Ot.UseLanguageFileCache)
        return;
      string str = Path.Combine(Ot.CachePath, "Languages");
      Directory.CreateDirectory(str);
      string path = Path.Combine(str, Ot.CurrentLanguage + "_" + translationGroup.ToString() + ".cache");
      if (File.Exists(path) || textList.Count == 0)
        return;
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        foreach (KeyValuePair<string, string> text in textList)
        {
          streamWriter.WriteLine(text.Key);
          streamWriter.Write((char) text.Value.Length);
          streamWriter.Write(text.Value);
        }
        streamWriter.Close();
      }
    }

    private static SortedList<string, string> GetLanguageFromFileCache(Tg translationGroup)
    {
      if (!Ot.UseLanguageFileCache)
        return (SortedList<string, string>) null;
      string path = Path.Combine(Ot.CachePath, "Languages", Ot.CurrentLanguage + "_" + translationGroup.ToString() + ".cache");
      if (!File.Exists(path))
        return (SortedList<string, string>) null;
      DateTime now = DateTime.Now;
      SortedList<string, string> languageFromFileCache = new SortedList<string, string>();
      using (StreamReader streamReader = new StreamReader(path))
      {
        StringBuilder stringBuilder = new StringBuilder();
        char[] buffer = new char[70000];
        while (true)
        {
          string key = streamReader.ReadLine();
          if (!string.IsNullOrEmpty(key))
          {
            int num = streamReader.Read();
            streamReader.Read(buffer, 0, num);
            stringBuilder.Clear();
            stringBuilder.Append(buffer, 0, num);
            languageFromFileCache.Add(key, stringBuilder.ToString());
          }
          else
            break;
        }
        streamReader.Close();
      }
      if (Ot.OnlineTranslatorLogger.IsTraceEnabled)
        Ot.OnlineTranslatorLogger.Trace("Translation Group '" + translationGroup.ToString() + "' loaded from file. Load time[ms]:" + DateTime.Now.Subtract(now).TotalMilliseconds.ToString("N0") + " Number of translations:" + languageFromFileCache.Count.ToString());
      return languageFromFileCache;
    }

    private static void CleareCache()
    {
      if (!Directory.Exists(Ot.CachePath))
        return;
      Directory.Delete(Ot.CachePath, true);
      Directory.CreateDirectory(Ot.CachePath);
    }

    public static int? GetMessageNumberFromLanguageText(string messageText)
    {
      if (messageText[0] != '@')
        return new int?();
      int num = messageText.IndexOf(' ');
      if (num < 2)
        return new int?();
      int result;
      return !int.TryParse(messageText.Substring(1, num - 1), out result) ? new int?() : new int?(result);
    }

    public static string IncludeMessageNumberInLanguageText(string languageText, int? messageNumber)
    {
      int num1;
      if (messageNumber.HasValue)
      {
        int? nullable = messageNumber;
        int num2 = 0;
        num1 = nullable.GetValueOrDefault() > num2 & nullable.HasValue ? 1 : 0;
      }
      else
        num1 = 0;
      return num1 != 0 ? "@" + messageNumber.ToString() + " " + languageText : languageText;
    }

    public static string GetMessageTextWithoutNumber(string messageText)
    {
      if (messageText[0] != '@')
        return messageText;
      int num = messageText.IndexOf(' ');
      return num < 2 ? messageText : messageText.Substring(num + 1);
    }

    public static string KeyFromDefaultText(string defaultText)
    {
      StringBuilder stringBuilder = new StringBuilder(defaultText);
      for (int index = 0; index < stringBuilder.Length; ++index)
      {
        if (stringBuilder[index] == ' ' || stringBuilder[index] == ':' || stringBuilder[index] == '#')
        {
          stringBuilder.Remove(index, 1);
          if (stringBuilder.Length > index)
            stringBuilder[index] = char.ToUpper(stringBuilder[index]);
        }
      }
      if (stringBuilder.Length > 42)
      {
        stringBuilder.Remove(20, stringBuilder.Length - 40);
        stringBuilder.Insert(20, "..");
      }
      return stringBuilder.ToString();
    }

    public static string GetMessageNumbersTree(Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      Exception exception = ex;
      while (true)
      {
        int? fromLanguageText = Ot.GetMessageNumberFromLanguageText(exception.Message);
        if (fromLanguageText.HasValue)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(fromLanguageText.ToString());
          if (exception.InnerException != null)
            exception = exception.InnerException;
          else
            break;
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    [Obsolete("Use Gtt function")]
    public static string GetTranslatedLanguageText(TranslatorKey key)
    {
      switch (key)
      {
        case TranslatorKey.MeterInstallerMissingSerialnumber:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "MissingSerialnumber");
        case TranslatorKey.MeterInstallerMissingReadoutSettings:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "MissingReadoutSettings");
        case TranslatorKey.MeterInstallerInvalidSubNodes:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "InvalidSubNodes");
        case TranslatorKey.MeterInstallerMeterAlreadyExists:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "MeterAlreadyExists");
        case TranslatorKey.MeterInstallerMissingNodeName:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "MissingNodeName");
        case TranslatorKey.MeterInstallerBaseNodeAlreadyExist:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "BaseNodeAlreadyExist");
        case TranslatorKey.MeterInstallerDeleteNodeQuestion:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "DeleteNodeQuestion");
        case TranslatorKey.MeterInstallerCanNotDeleteNode:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "CanNotDeleteNode");
        case TranslatorKey.MeterInstallerSystemmanagerLightLimitReached:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "SystemmanagerLightLimitReached");
        case TranslatorKey.MeterInstallerWrongNodeNameLength:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "WrongNodeNameLength");
        case TranslatorKey.MeterInstallerDemoLimitReached:
          return Ot.GetTranslatedLanguageText("MeterInstaller", "DemoLimitReached");
        case TranslatorKey.GMMDatabaseUpdateQuestion:
          return Ot.GetTranslatedLanguageText("GMM", "DatabaseUpdateQuestion");
        default:
          throw new NotImplementedException("Unknown key: " + key.ToString());
      }
    }

    [Obsolete("Use Gtt function")]
    public static string GetTranslatedLanguageText(
      string GmmModule,
      string TextKey,
      string defaultText)
    {
      return Ot.Gtt(Tg.Common, GmmModule + TextKey, defaultText);
    }

    [Obsolete("Use Gtt function")]
    public static string GetTranslatedLanguageText(string GmmModule, string TextKey)
    {
      string str = GmmModule + TextKey;
      return Ot.Gtt(Tg.Common, str, str);
    }

    [Obsolete("Use Gtt function")]
    public static string GetTranslatedLanguageText(string key) => Ot.Gtt(Tg.Common, key, key);

    public static bool Delete(string module, string key)
    {
      return Ot.DeleteOnlineTranslation(Ot.CurrentLanguage, Tg.Common, module + key);
    }

    [Obsolete("Use Gtt function")]
    public static string TextToKey(string text)
    {
      string key = text.Replace(" ", "");
      if (key.Length > 22)
        key = key.Substring(0, 10) + ".." + key.Substring(key.Length - 10);
      return key;
    }

    [Obsolete("Use KeyFromDefaultText function")]
    public static string GetKey(string moduleName, string textValue)
    {
      Ot.GarantLanguageNamespaceCached(Tg.Common);
      int index = Ot.TranslatedDataCache.IndexOfKey(Tg.Common);
      if (index >= 0)
      {
        foreach (KeyValuePair<string, string> keyValuePair in Ot.TranslatedDataCache.Values[index])
        {
          if (keyValuePair.Key.StartsWith(moduleName) && keyValuePair.Value == textValue)
            return keyValuePair.Key.Replace(moduleName, string.Empty);
        }
      }
      return (string) null;
    }

    public enum LanguageTableTypes
    {
      Unknown,
      UniversalNamespaceType,
      UniversalType,
      OnlyEnglishType,
      InitialisationError,
    }
  }
}
