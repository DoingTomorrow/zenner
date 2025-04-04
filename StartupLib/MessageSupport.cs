// Decompiled with JetBrains decompiler
// Type: StartupLib.MessageSupport
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace StartupLib
{
  internal class MessageSupport
  {
    public List<string> Mobile;
    private readonly string Passcode = "123456";
    public string Content;
    private readonly string Sys = "GMM";
    private readonly string URL = "http://192.168.0.66:8001";
    private Hashtable MessageTable;

    public MessageSupport()
    {
      this.Mobile = new List<string>();
      this.Content = string.Empty;
      this.MessageTable = new Hashtable();
    }

    private Task<string> ConvertAllMobileToString()
    {
      return Task.Run<string>((Func<string>) (() =>
      {
        string str = string.Empty;
        try
        {
          for (int index = 0; index < this.Mobile.Count - 1; ++index)
            str = str + this.Mobile[index] + ",";
          return str + this.Mobile[this.Mobile.Count - 1];
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }));
    }

    private async Task<Hashtable> AddMessageContentToHashtable()
    {
      Hashtable messageTable;
      try
      {
        this.MessageTable.Clear();
        Hashtable hashtable = this.MessageTable;
        object key = (object) "mobile";
        string str = await this.ConvertAllMobileToString();
        hashtable.Add(key, (object) str);
        hashtable = (Hashtable) null;
        key = (object) null;
        str = (string) null;
        this.MessageTable.Add((object) "passcode", (object) this.Passcode);
        this.MessageTable.Add((object) "content", (object) this.Content);
        this.MessageTable.Add((object) "sys", (object) this.Sys);
        messageTable = this.MessageTable;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return messageTable;
    }

    private async Task<string> ConvertHashtableToString()
    {
      Hashtable hashtable = await this.AddMessageContentToHashtable();
      this.MessageTable = hashtable;
      hashtable = (Hashtable) null;
      string str;
      try
      {
        StringBuilder sb = new StringBuilder();
        foreach (string k in (IEnumerable) this.MessageTable.Keys)
        {
          if (sb.Length > 0)
            sb.Append("&");
          sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(this.MessageTable[(object) k].ToString()));
        }
        str = sb.ToString();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return str;
    }

    private Task<string> ReadResponse(WebResponse response)
    {
      return Task.Run<string>((Func<string>) (() =>
      {
        try
        {
          return new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }));
    }

    public async Task<bool> SendMessageAndGetResponse()
    {
      bool response;
      try
      {
        string str1 = this.URL;
        string str2 = await this.ConvertHashtableToString();
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(str1 + "/?" + str2);
        str1 = (string) null;
        str2 = (string) null;
        request.Method = "GET";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Credentials = CredentialCache.DefaultCredentials;
        request.Timeout = 5000;
        string Response = await this.ReadResponse(request.GetResponse());
        response = Response.ToLower().Contains("ok");
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return response;
    }
  }
}
