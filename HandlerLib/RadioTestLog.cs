// Decompiled with JetBrains decompiler
// Type: HandlerLib.RadioTestLog
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class RadioTestLog
  {
    private DataTable LogData;
    private int Order;

    public RadioTestLog()
    {
      this.LogData = new DataTable();
      this.LogData.Columns.Add(new DataColumn(RadioTestLog.LogColumnNames.Order.ToString())
      {
        DataType = typeof (int)
      });
      this.LogData.Columns.Add(new DataColumn(RadioTestLog.LogColumnNames.LogTime.ToString())
      {
        DataType = typeof (DateTime)
      });
      this.LogData.Columns.Add(new DataColumn(RadioTestLog.LogColumnNames.Frequency.ToString())
      {
        DataType = typeof (double)
      });
      this.LogData.Columns.Add(new DataColumn(RadioTestLog.LogColumnNames.Direction.ToString())
      {
        DataType = typeof (string)
      });
      this.LogData.Columns.Add(new DataColumn(RadioTestLog.LogColumnNames.ReceiveInfo.ToString())
      {
        DataType = typeof (string)
      });
      this.LogData.Columns.Add(new DataColumn(RadioTestLog.LogColumnNames.RSSI.ToString())
      {
        DataType = typeof (int)
      });
      this.Order = 0;
    }

    public void AddTest(
      double Frequency,
      RadioTestLog.RadioTestDirection direction,
      RadioTestLog.ReceiveInfo receiveInfo,
      int rssi)
    {
      DataRow row = this.LogData.NewRow();
      row[RadioTestLog.LogColumnNames.Order.ToString()] = (object) this.Order;
      row[RadioTestLog.LogColumnNames.LogTime.ToString()] = (object) DateTime.Now;
      row[RadioTestLog.LogColumnNames.Frequency.ToString()] = (object) Frequency;
      row[RadioTestLog.LogColumnNames.Direction.ToString()] = (object) direction.ToString();
      row[RadioTestLog.LogColumnNames.ReceiveInfo.ToString()] = (object) receiveInfo.ToString();
      if (receiveInfo == RadioTestLog.ReceiveInfo.ok)
        row[RadioTestLog.LogColumnNames.RSSI.ToString()] = (object) rssi;
      this.LogData.Rows.Add(row);
      ++this.Order;
    }

    public void ShowLog(string tableName)
    {
      ExcelConnect excelConnect = new ExcelConnect();
      excelConnect.AddTable(this.LogData, tableName, false, false);
      excelConnect.ShowWorkbook();
    }

    private enum LogColumnNames
    {
      Order,
      LogTime,
      Direction,
      Frequency,
      ReceiveInfo,
      RSSI,
    }

    public enum RadioTestDirection
    {
      DUT_To_MinoConnect,
      MinoConnect_To_DUT,
      DUT_To_IUWS,
      IUWS_To_DUT,
    }

    public enum ReceiveInfo
    {
      ok,
      timeout,
      error,
    }
  }
}
