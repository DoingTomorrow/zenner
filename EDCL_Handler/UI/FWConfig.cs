// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UI.FWConfig
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using GmmDbLib;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;

#nullable disable
namespace EDCL_Handler.UI
{
  internal class FWConfig
  {
    public int ConnectionProfileID { set; get; }

    public string DeviceType { set; get; }

    public int FW_MapID { set; get; }

    public int BL_MapID { set; get; }

    public string FW_Data { set; get; }

    public string BL_Data { set; get; }

    public string FW_FilePath { set; get; }

    public string BL_FilePath { set; get; }

    public string FW_FileName { set; get; }

    public string BL_FileName { set; get; }

    public FWConfig(string ConfigFilePath)
    {
      this.ConnectionProfileID = -1;
      this.DeviceType = string.Empty;
      this.FW_MapID = -1;
      this.BL_MapID = -1;
      this.FW_FilePath = string.Empty;
      this.BL_FilePath = string.Empty;
      this.FW_FileName = string.Empty;
      this.BL_FileName = string.Empty;
      this.FW_Data = string.Empty;
      this.BL_Data = string.Empty;
      int num = -1;
      foreach (string readAllLine in File.ReadAllLines(ConfigFilePath))
      {
        string source = readAllLine.Trim();
        if (!(source == string.Empty) && source[0] != ';' && source.Contains<char>('='))
        {
          string str1 = source.Split('=')[0];
          string str2 = source.Split('=')[1];
          switch (str1)
          {
            case nameof (ConnectionProfileID):
              num = int.Parse(str2);
              break;
            case nameof (DeviceType):
              switch (str2)
              {
                case "EDC_LoRa":
                  num = 271;
                  break;
                case "Micro_LoRa":
                  num = 272;
                  break;
                case "Micro_wM-Bus":
                  num = 599;
                  break;
              }
              break;
            case "Firmware_MapID":
              this.FW_MapID = int.Parse(str2);
              this.FW_FileName = this.GetFileNameFromMapID(this.FW_MapID);
              break;
            case "Firmware_FilePath":
              this.FW_FilePath = Path.Combine(Path.GetDirectoryName(ConfigFilePath), str2);
              this.FW_FileName = Path.GetFileName(this.FW_FilePath);
              break;
            case "BootLoader_MapID":
              this.BL_MapID = int.Parse(str2);
              this.BL_FileName = this.GetFileNameFromMapID(this.BL_MapID);
              break;
            case "BootLoader_FilePath":
              this.BL_FilePath = Path.Combine(Path.GetDirectoryName(ConfigFilePath), str2);
              this.BL_FileName = Path.GetFileName(this.BL_FilePath);
              break;
          }
        }
      }
      if (num == -1)
        throw new Exception("Missing ConnectionProfileID");
      if (this.BL_MapID == -1)
        this.BL_Data = this.BL_FilePath != string.Empty ? File.ReadAllText(this.BL_FilePath) : throw new Exception("Missing BootLoader");
      if (this.FW_MapID == -1)
        this.FW_Data = this.FW_FilePath != string.Empty ? File.ReadAllText(this.FW_FilePath) : throw new Exception("Missing Firmware");
      this.ConnectionProfileID = num;
    }

    private string GetFileNameFromMapID(int MapID)
    {
      DataTable table = this.QueryDatabase("SELECT ProgFileName FROM ProgFiles WHERE MapID=" + MapID.ToString()).Tables[0];
      if (table.Rows.Count == 0)
        throw new Exception("MapID:" + MapID.ToString() + " Not Found");
      return table.Rows[0]["ProgFileName"].ToString();
    }

    private DataSet QueryDatabase(string sqlstr)
    {
      BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
      DbConnection newConnection = baseDbConnection.GetNewConnection();
      newConnection.Open();
      DbDataAdapter dataAdapter = baseDbConnection.GetDataAdapter(sqlstr, newConnection);
      DataSet dataSet = new DataSet();
      dataAdapter.Fill(dataSet);
      newConnection.Close();
      return dataSet;
    }
  }
}
