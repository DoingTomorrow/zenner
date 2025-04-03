// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DbConnectionInfo
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace GmmDbLib
{
  public class DbConnectionInfo
  {
    public MeterDbTypes DbType;
    public DbInstances DbInstance;
    public string UrlOrPath;
    public string DatabaseName;
    public string UserName;
    public string Password;
    public string ConnectionString;

    public string SetupString
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("DbType=" + this.DbType.ToString());
        stringBuilder.Append(";DbInstance=" + this.DbInstance.ToString());
        stringBuilder.Append(";UrlOrPath=" + this.UrlOrPath);
        if (!string.IsNullOrEmpty(this.DatabaseName))
          stringBuilder.Append(";DatabaseName=" + this.DatabaseName);
        if (!string.IsNullOrEmpty(this.UserName))
          stringBuilder.Append(";UserName=" + this.UserName);
        if (!string.IsNullOrEmpty(this.Password))
          stringBuilder.Append(";Password=" + this.scrable(this.Password));
        return stringBuilder.ToString();
      }
      set
      {
        string str1 = value;
        char[] chArray1 = new char[1]{ ';' };
        foreach (string str2 in str1.Split(chArray1))
        {
          char[] chArray2 = new char[1]{ '=' };
          string[] strArray = str2.Split(chArray2);
          switch (strArray[0])
          {
            case "DbType":
              this.DbType = (MeterDbTypes) Enum.Parse(typeof (MeterDbTypes), strArray[1], true);
              break;
            case "DbInstance":
              this.DbInstance = (DbInstances) Enum.Parse(typeof (DbInstances), strArray[1], true);
              break;
            case "UrlOrPath":
              this.UrlOrPath = strArray[1];
              break;
            case "DatabaseName":
              this.DatabaseName = strArray[1];
              break;
            case "UserName":
              this.UserName = strArray[1];
              break;
            case "Password":
              this.Password = this.descrable(strArray[1]);
              break;
          }
        }
      }
    }

    public string scrable(string inpassword)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < inpassword.Length; ++index)
      {
        int num = (int) inpassword[index];
        num ^= 21046;
        stringBuilder.Append(num.ToString("x04"));
      }
      return stringBuilder.ToString();
    }

    private string descrable(string inpassword)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        for (int startIndex = 0; startIndex < inpassword.Length; startIndex += 4)
        {
          int num = int.Parse(inpassword.Substring(startIndex, 4), NumberStyles.HexNumber) ^ 21046;
          stringBuilder.Append((char) num);
        }
        return stringBuilder.ToString();
      }
      catch
      {
        return "";
      }
    }

    public DbConnectionInfo()
    {
      this.DbType = MeterDbTypes.Access;
      this.DbInstance = DbInstances.Primary;
      this.UrlOrPath = "";
      this.DatabaseName = "";
      this.UserName = "";
      this.Password = "";
    }

    public DbConnectionInfo(DbConnectionInfo srcConnectionInfo)
    {
      this.DbType = srcConnectionInfo.DbType;
      this.DbInstance = srcConnectionInfo.DbInstance;
      this.UrlOrPath = srcConnectionInfo.UrlOrPath;
      this.DatabaseName = srcConnectionInfo.DatabaseName;
      this.UserName = srcConnectionInfo.UserName;
      this.Password = srcConnectionInfo.Password;
    }
  }
}
