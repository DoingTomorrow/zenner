// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ERR8002_Form
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class ERR8002_Form : Form
  {
    private Datenliste TheDataList;
    private ZR_HandlerFunctions MyFunctions;
    private string BaseStatusText;
    private bool NichtReparieren;
    private bool NichtSpeichern;
    private DataSetLogData MyLogData;
    private string DataSetFileName;
    private string DataSetBackup1FileName;
    private string DataSetBackup2FileName;
    private string DataSetBackup3FileName;
    private string CSV_FileName;
    private StringBuilder CSV_Line;
    private char Seperator = '\t';
    private ZR_MeterIdent DeviceIdentity;
    private string Ableser;
    private string Liegenschaft;
    private int Nutzernummer;
    private int SerienNummer;
    private DateTime Stichtag;
    private DateTime AbweichenderStichtag;
    private bool ErrorFound;
    private bool ErrorResolved;
    private bool ManuellGespeichert;
    private int EnergieAktuell;
    private int EnergieStichtag;
    private int Energie100701;
    private int Energie100601;
    private int Energie100501;
    private int Energie100401;
    private int Energie100301;
    private int Energie100201;
    private int Energie100101;
    private int Energie091201;
    private int Energie091101;
    private int Energie091001;
    private int Energie090901;
    private int Energie090801;
    private int Energie090701;
    private int Energie090601;
    private int Energie090501;
    private int Energie090401;
    private int Energie090301;
    private int Energie090201;
    private int Energie090101;
    private DateTime Max_QmPerHour1_Month;
    private double Max_QmPerHour1;
    private DateTime Max_QmPerHour2_Month;
    private double Max_QmPerHour2;
    private DateTime Max_kW1_Month;
    private double Max_kW1;
    private DateTime Max_kW2_Month;
    private double Max_kW2;
    private IContainer components = (IContainer) null;
    private Button buttonLesen;
    private TextBox textBoxStatus;
    private TextBox textBoxCommunicationStatus;
    private TextBox textBoxAusleser;
    private Label label1;
    private TextBox textBoxLiegenschaft;
    private Label label2;
    private TextBox textBoxNutzernummer;
    private Label label3;
    private TextBox textBoxDeviceNumber;
    private Label label4;
    private TextBox textBoxEinbauort;
    private Label label5;
    private TextBox textBoxMessbereich;
    private Label label6;
    private TextBox textBoxUnit;
    private Label label7;
    private TextBox textBoxStichtag;
    private Label label8;
    private TextBox textBoxAbweichenderStichtag;
    private Label label9;
    private TextBox textBoxEinbaudatum;
    private Label label10;
    private TextBox textBoxAuftragsnummer;
    private Label label11;
    private TextBox textBoxEnergieAktuell;
    private Label label12;
    private TextBox textBoxEnergieAmStichtag;
    private Label label13;
    private TextBox textBoxKommentar;
    private Label label14;
    private Label label15;
    private Label label16;
    private Button buttonSpeichernOhneLesen;
    private Button buttonAblaufZumKommentar;
    private Button buttonDatenliste;
    private ToolTip toolTip1;
    private Button buttonTestlesen;
    private Button buttonOpenFolder;

    public ERR8002_Form(ZR_HandlerFunctions MyFunctions)
    {
      this.InitializeComponent();
      this.MyFunctions = MyFunctions;
      MyFunctions.SerBus.OnMessage += new EventHandler<GMM_EventArgs>(this.A_SerialBusMessage);
      this.MyLogData = new DataSetLogData();
      this.DataSetFileName = Path.Combine(SystemValues.LoggDataPath, "Serie2LogData.xml");
      this.DataSetBackup1FileName = Path.Combine(SystemValues.LoggDataPath, "Serie2LogDataBackup1.xml");
      this.DataSetBackup2FileName = Path.Combine(SystemValues.LoggDataPath, "Serie2LogDataBackup2.xml");
      this.DataSetBackup3FileName = Path.Combine(SystemValues.LoggDataPath, "Serie2LogDataBackup3.xml");
      if (File.Exists(this.DataSetFileName))
      {
        try
        {
          int num = (int) this.MyLogData.ReadXml(this.DataSetFileName);
        }
        catch (Exception ex)
        {
          int num = (int) GMM_MessageBox.ShowMessage("XLM File", "Fehler beim laden des Datenfiles" + ex.ToString());
        }
      }
      this.CSV_Line = new StringBuilder(5000);
    }

    private void A_SerialBusMessage(object sender, GMM_EventArgs TheMessage)
    {
      switch (TheMessage.TheMessageType)
      {
        case GMM_EventArgs.MessageType.StandardMessage:
          this.textBoxCommunicationStatus.Text = this.BaseStatusText + TheMessage.EventMessage + TheMessage.InfoNumber.ToString();
          break;
        case GMM_EventArgs.MessageType.PrimaryAddressMessage:
          this.textBoxCommunicationStatus.Text = this.BaseStatusText + TheMessage.InfoNumber.ToString();
          break;
        case GMM_EventArgs.MessageType.EndMessage:
          this.textBoxCommunicationStatus.Text = string.Empty;
          break;
      }
      this.Refresh();
    }

    private bool CheckInputData()
    {
      this.textBoxStatus.Clear();
      this.Refresh();
      this.Ableser = this.textBoxAusleser.Text.Trim();
      if (this.Ableser != this.textBoxAusleser.Text)
        this.textBoxAusleser.Text = this.Ableser;
      if (this.Ableser.Length < 2)
      {
        this.ShowErrorMessage("Ableser Name nicht gesetzt!");
        return false;
      }
      this.Liegenschaft = this.textBoxLiegenschaft.Text.Trim().Replace(" ", "_");
      if (!int.TryParse(this.textBoxNutzernummer.Text, out this.Nutzernummer) || this.Nutzernummer < 0 || this.Nutzernummer > 9999)
      {
        this.ShowErrorMessage("Nutzernummer nicht gültig.");
        return false;
      }
      if (this.textBoxAbweichenderStichtag.Text.Trim().Length > 0)
      {
        if (!DateTime.TryParse(this.textBoxAbweichenderStichtag.Text, out this.AbweichenderStichtag))
        {
          this.ShowErrorMessage("Abweichender Stichtag nicht gültig.");
          return false;
        }
      }
      else
        this.AbweichenderStichtag = DateTime.MinValue;
      this.Stichtag = DateTime.MinValue;
      this.EnergieStichtag = -1;
      this.EnergieAktuell = -1;
      this.Energie100701 = -1;
      this.Energie100601 = -1;
      this.Energie100501 = -1;
      this.Energie100401 = -1;
      this.Energie100301 = -1;
      this.Energie100201 = -1;
      this.Energie100101 = -1;
      this.Energie091201 = -1;
      this.Energie091101 = -1;
      this.Energie091001 = -1;
      this.Energie090901 = -1;
      this.Energie090801 = -1;
      this.Energie090701 = -1;
      this.Energie090601 = -1;
      this.Energie090501 = -1;
      this.Energie090401 = -1;
      this.Energie090301 = -1;
      this.Energie090201 = -1;
      this.Energie090101 = -1;
      this.Max_QmPerHour1_Month = DateTime.MinValue;
      this.Max_QmPerHour1 = -1.0;
      this.Max_QmPerHour2_Month = DateTime.MinValue;
      this.Max_QmPerHour2 = -1.0;
      this.Max_kW1_Month = DateTime.MinValue;
      this.Max_kW1 = -1.0;
      this.Max_kW2_Month = DateTime.MinValue;
      this.Max_kW2 = -1.0;
      this.ErrorResolved = false;
      return true;
    }

    private bool LoadDataFromMeter()
    {
      try
      {
        Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
        this.ShowMessageLine("Gerätenummer: " + this.DeviceIdentity.SerialNr);
        this.textBoxDeviceNumber.Text = this.DeviceIdentity.SerialNr;
        this.SerienNummer = int.Parse(this.DeviceIdentity.SerialNr);
        string versionString = ParameterService.GetVersionString(this.DeviceIdentity.lFirmwareVersion, 7);
        this.ShowMessageLine("Geräteversion: " + versionString);
        if (versionString != "1.5.0:C2")
        {
          this.ShowErrorMessage("Ungültiges Gerät für diese Funktionalität.");
          return false;
        }
        Parameter allParameter1 = (Parameter) workMeter.AllParameters[(object) "Sta_Status"];
        if (!workMeter.MyCommunication.ReadParameterValue(allParameter1, MemoryLocation.RAM))
        {
          this.ShowErrorMessage("Fehler beim lesen des Gerätestatus");
          return false;
        }
        this.ErrorFound = allParameter1.ValueCPU >= 32768L;
        Parameter allParameter2 = (Parameter) workMeter.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
        if (!workMeter.MyCommunication.ReadParameterValue(allParameter2, MemoryLocation.RAM))
        {
          this.ShowErrorMessage("Fehler beim lesen des Schreibschutz Zustands");
          return false;
        }
        bool flag = true;
        if (allParameter2.ValueCPU == (long) byte.MaxValue)
        {
          flag = false;
          this.ErrorFound = true;
        }
        if (this.ErrorFound)
          this.ShowMessageLine("Gerät ist im Fehlerzustand");
        else
          this.ShowMessageLine("Gerät ist nicht im Fehlerzustand");
        Parameter allParameter3 = (Parameter) workMeter.AllParameters[(object) "DefaultFunction.Waerme_EnergSum"];
        if (!workMeter.MyCommunication.ReadParameterValue(allParameter3, MemoryLocation.RAM))
        {
          this.ShowErrorMessage("Fehler beim lesen der aktuellen Energie");
          return false;
        }
        if (!flag)
        {
          allParameter3.ValueEprom = allParameter3.ValueCPU;
          allParameter3.UpdateByteList();
        }
        long valueCpu = allParameter3.ValueCPU;
        Parameter allParameter4 = (Parameter) workMeter.AllParameters[(object) "DefaultFunction.Energ_SumExpo"];
        long num1;
        this.EnergieAktuell = (int) (num1 = valueCpu >> (int) allParameter4.ValueEprom);
        this.textBoxEnergieAktuell.Text = this.EnergieAktuell.ToString();
        this.ShowMessageLine("Energie aktuell: " + this.textBoxEnergieAktuell.Text);
        IntervalAndLogger intervalAndLogger1 = (IntervalAndLogger) null;
        IntervalAndLogger intervalAndLogger2 = (IntervalAndLogger) null;
        IntervalAndLogger intervalAndLogger3 = (IntervalAndLogger) null;
        foreach (CodeBlock linkObj in workMeter.MyRuntimeCode.LinkObjList)
        {
          if (linkObj is IntervalAndLogger)
          {
            IntervalAndLogger intervalAndLogger4 = (IntervalAndLogger) linkObj;
            if (intervalAndLogger4.Type == LoggerTypes.FixedLogger)
            {
              if (intervalAndLogger4.FunctionNumber == 185)
              {
                intervalAndLogger1 = intervalAndLogger4;
                intervalAndLogger1.LetLoggerUnchanged = true;
              }
              else if (intervalAndLogger4.FunctionNumber == 148)
              {
                intervalAndLogger1 = intervalAndLogger4;
                intervalAndLogger1.LetLoggerUnchanged = true;
                if (!this.ErrorFound)
                  this.NichtReparieren = true;
              }
              else if (intervalAndLogger4.FunctionNumber == 254)
                intervalAndLogger2 = intervalAndLogger4;
              else if (intervalAndLogger4.FunctionNumber == 238)
              {
                intervalAndLogger3 = intervalAndLogger4;
                intervalAndLogger3.Type = LoggerTypes.FixedLoggerFuture;
              }
              else if (intervalAndLogger4.FunctionNumber == 180)
              {
                intervalAndLogger3 = intervalAndLogger4;
                intervalAndLogger3.Type = LoggerTypes.FixedLoggerFuture;
                if (!this.ErrorFound)
                  this.NichtReparieren = true;
              }
            }
          }
        }
        this.Stichtag = DateTime.MinValue;
        this.EnergieStichtag = -1;
        this.textBoxStichtag.Text = "";
        DateTime dateTime;
        if (intervalAndLogger3 != null)
        {
          int index1 = workMeter.AllParameters.IndexOfKey((object) "StichtagKomplettMitVorjahr.Stichtag_0T");
          if (index1 >= 0)
          {
            dateTime = ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) workMeter.AllParameters.GetByIndex(index1)).ValueEprom);
            dateTime = dateTime.AddYears(-1);
            this.Stichtag = dateTime.Date;
            this.textBoxStichtag.Text = this.Stichtag.ToShortDateString();
            this.ShowMessageLine("Stichtag: " + this.textBoxStichtag.Text);
            int index2 = workMeter.AllParameters.IndexOfKey((object) "StichtagKomplettMitVorjahr.WaermeEnergieAmStichtag");
            if (index2 >= 0)
            {
              this.EnergieStichtag = (int) ((Parameter) workMeter.AllParameters.GetByIndex(index2)).ValueEprom;
              this.ShowMessageLine("Energie am Stichtag: " + this.EnergieStichtag.ToString());
              this.textBoxEnergieAmStichtag.Text = this.EnergieStichtag.ToString();
            }
            int index3 = workMeter.AllParameters.IndexOfKey((object) "StichtagKomplettMitVorjahr.DueDateStorageTime");
            if (index3 >= 0)
            {
              dateTime = ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) workMeter.AllParameters.GetByIndex(index3)).ValueEprom);
              if (this.Stichtag != dateTime.Date)
              {
                this.ShowMessageLine("Stichtag wurde noch nicht gespeichert");
                this.EnergieStichtag = -1;
              }
            }
          }
        }
        else
        {
          this.NichtReparieren = true;
          this.ShowMessageLine("Stichtagsfunktion nicht gefunden");
        }
        if (intervalAndLogger1 == null)
        {
          this.ShowMessageLine("Monatswerte nicht vorhanden oder nicht unterstützt.");
          this.NichtReparieren = true;
          this.ShowMessageLine("Das Gerät ist nicht kritisch und wird nicht programmiert.");
        }
        else
        {
          int num2 = (int) (intervalAndLogger1.P_EndAddress.ValueEprom - intervalAndLogger1.P_StartAddress.ValueEprom);
          if (num2 > 500 || num2 < 0 || intervalAndLogger1.P_WriteAddress.ValueEprom > intervalAndLogger1.P_EndAddress.ValueEprom || intervalAndLogger1.P_WriteAddress.ValueEprom < intervalAndLogger1.P_StartAddress.ValueEprom)
          {
            this.NichtReparieren = true;
            this.ShowErrorMessage("Monatslogger beschädigt. Das Gerät kann nicht rapariert werden!");
            this.ShowMessageLine("Bitte die gespeicherten Daten auf Plausibilität prüfen!");
            this.textBoxKommentar.AppendText(Environment.NewLine);
            this.textBoxKommentar.AppendText("Monatslogger beschädigt! Das Gerät kann nicht rapariert werden!");
            this.textBoxKommentar.AppendText(Environment.NewLine);
            this.textBoxKommentar.AppendText("Bitte die gespeicherten Daten auf Plausibilität prüfen!");
            return true;
          }
          LoggerInfo loggerInfo = intervalAndLogger1.GetLoggerInfo(out int _);
          loggerInfo.ReadLogger(new DateTime(2008, 1, 1), new DateTime(2011, 1, 1));
          for (int index = 0; index < loggerInfo.LoggerData.Count; ++index)
          {
            dateTime = loggerInfo.LoggerData.Keys[index];
            DateTime date = dateTime.Date;
            double result;
            if (double.TryParse(loggerInfo.LoggerData.Values[index][0], NumberStyles.Any, (IFormatProvider) FixedFormates.TheFormates.NumberFormat, out result))
            {
              int num3 = (int) (result * 1000.0);
              if (date.Year == 2009 && date.Day == 1)
              {
                switch (date.Month)
                {
                  case 1:
                    this.Energie090101 = num3;
                    this.ShowMessageLine("Energie am 1.1.2009: " + num3.ToString());
                    break;
                  case 2:
                    this.Energie090201 = num3;
                    this.ShowMessageLine("Energie am 1.2.2009: " + num3.ToString());
                    break;
                  case 3:
                    this.Energie090301 = num3;
                    this.ShowMessageLine("Energie am 1.3.2009: " + num3.ToString());
                    break;
                  case 4:
                    this.Energie090401 = num3;
                    this.ShowMessageLine("Energie am 1.4.2009: " + num3.ToString());
                    break;
                  case 5:
                    this.Energie090501 = num3;
                    this.ShowMessageLine("Energie am 1.5.2009: " + num3.ToString());
                    break;
                  case 6:
                    this.Energie090601 = num3;
                    this.ShowMessageLine("Energie am 1.6.2009: " + num3.ToString());
                    break;
                  case 7:
                    this.Energie090701 = num3;
                    this.ShowMessageLine("Energie am 1.7.2009: " + num3.ToString());
                    break;
                  case 8:
                    this.Energie090801 = num3;
                    this.ShowMessageLine("Energie am 1.8.2009: " + num3.ToString());
                    break;
                  case 9:
                    this.Energie090901 = num3;
                    this.ShowMessageLine("Energie am 1.9.2009: " + num3.ToString());
                    break;
                  case 10:
                    this.Energie091001 = num3;
                    this.ShowMessageLine("Energie am 1.10.2009: " + num3.ToString());
                    break;
                  case 11:
                    this.Energie091101 = num3;
                    this.ShowMessageLine("Energie am 1.11.2009: " + num3.ToString());
                    break;
                  case 12:
                    this.Energie091201 = num3;
                    this.ShowMessageLine("Energie am 1.12.2009: " + num3.ToString());
                    break;
                }
              }
              else if (date.Year == 2010 && date.Day == 1)
              {
                switch (date.Month)
                {
                  case 1:
                    this.Energie100101 = num3;
                    this.ShowMessageLine("Energie am 1.1.2010: " + num3.ToString());
                    break;
                  case 2:
                    this.Energie100201 = num3;
                    this.ShowMessageLine("Energie am 1.2.2010: " + num3.ToString());
                    break;
                  case 3:
                    this.Energie100301 = num3;
                    this.ShowMessageLine("Energie am 1.3.2010: " + num3.ToString());
                    break;
                  case 4:
                    this.Energie100401 = num3;
                    this.ShowMessageLine("Energie am 1.4.2010: " + num3.ToString());
                    break;
                  case 5:
                    this.Energie100501 = num3;
                    this.ShowMessageLine("Energie am 1.5.2010: " + num3.ToString());
                    break;
                  case 6:
                    this.Energie100601 = num3;
                    this.ShowMessageLine("Energie am 1.6.2010: " + num3.ToString());
                    break;
                  case 7:
                    this.Energie100701 = num3;
                    this.ShowMessageLine("Energie am 1.7.2010: " + num3.ToString());
                    break;
                }
              }
            }
          }
        }
        if (intervalAndLogger2 == null)
        {
          this.ShowMessageLine("Spitzenwerte nicht vorhanden oder nicht unterstützt.");
        }
        else
        {
          int num4 = (int) (intervalAndLogger2.P_EndAddress.ValueEprom - intervalAndLogger2.P_StartAddress.ValueEprom);
          if (num4 > 500 || num4 < 0 || intervalAndLogger2.P_WriteAddress.ValueEprom > intervalAndLogger2.P_EndAddress.ValueEprom || intervalAndLogger2.P_WriteAddress.ValueEprom < intervalAndLogger2.P_StartAddress.ValueEprom)
          {
            this.NichtReparieren = true;
            this.ShowErrorMessage("Spitzenwerte beschädigt. Das Gerät kann nicht rapariert werden!");
            this.ShowMessageLine("Bitte die gespeicherten Daten auf Plausibilität prüfen!");
            this.textBoxKommentar.AppendText(Environment.NewLine);
            this.textBoxKommentar.AppendText("Spitzenwerte beschädigt! Das Gerät kann nicht rapariert werden!");
            this.textBoxKommentar.AppendText(Environment.NewLine);
            this.textBoxKommentar.AppendText("Bitte die gespeicherten Daten auf Plausibilität prüfen!");
            return true;
          }
          LoggerInfo loggerInfo = intervalAndLogger2.GetLoggerInfo(out int _);
          loggerInfo.ReadLogger(new DateTime(2008, 1, 1), new DateTime(2011, 1, 1));
          for (int index = 0; index < loggerInfo.LoggerData.Count; ++index)
          {
            dateTime = loggerInfo.LoggerData.Keys[index];
            DateTime date = dateTime.Date;
            string[] strArray = loggerInfo.LoggerData.Values[index];
            double result1;
            double result2;
            if (double.TryParse(strArray[1], NumberStyles.Any, (IFormatProvider) FixedFormates.TheFormates.NumberFormat, out result1) && double.TryParse(strArray[0], NumberStyles.Any, (IFormatProvider) FixedFormates.TheFormates.NumberFormat, out result2))
            {
              if (result2 > this.Max_kW1)
              {
                this.Max_kW2 = this.Max_kW1;
                this.Max_kW2_Month = this.Max_kW1_Month;
                this.Max_kW1 = result2;
                this.Max_kW1_Month = date;
              }
              else if (result2 > this.Max_kW2)
              {
                this.Max_kW2 = result2;
                this.Max_kW2_Month = date;
              }
              if (result1 > this.Max_QmPerHour1)
              {
                this.Max_QmPerHour2 = this.Max_QmPerHour1;
                this.Max_QmPerHour2_Month = this.Max_QmPerHour1_Month;
                this.Max_QmPerHour1 = result1;
                this.Max_QmPerHour1_Month = date;
              }
              else if (result1 > this.Max_QmPerHour2)
              {
                this.Max_QmPerHour2 = result1;
                this.Max_QmPerHour2_Month = date;
              }
            }
          }
          if (this.Max_QmPerHour1 >= 0.0)
            this.ShowMessageLine("Max_1 m\u00B3/h[" + this.Max_QmPerHour1_Month.ToShortDateString() + "]=" + this.Max_QmPerHour1.ToString());
          if (this.Max_kW1 >= 0.0)
            this.ShowMessageLine("Max_1 kW[" + this.Max_kW1_Month.ToShortDateString() + "]=" + this.Max_kW1.ToString());
          if (this.Max_QmPerHour2 >= 0.0)
            this.ShowMessageLine("Max_2 m\u00B3/h[" + this.Max_QmPerHour2_Month.ToShortDateString() + "]=" + this.Max_QmPerHour2.ToString());
          if (this.Max_kW2 >= 0.0)
            this.ShowMessageLine("Max_2 kW[" + this.Max_kW2_Month.ToShortDateString() + "]=" + this.Max_kW2.ToString());
        }
      }
      catch (Exception ex)
      {
        this.ShowErrorMessage("Fehler beim Auslesen der Daten aus dem Gerät");
        this.ShowMessageLine(ex.ToString());
        return false;
      }
      return true;
    }

    private void WriteData()
    {
      try
      {
        DataSetLogData.GMM_Serie2DeviceLogDataRow row = this.MyLogData.GMM_Serie2DeviceLogData.NewGMM_Serie2DeviceLogDataRow();
        row.ChangeDateTime = DateTime.Now;
        row.Ausleser = this.Ableser;
        row.Liegenschaft = this.Liegenschaft;
        row.Nutzer = this.Nutzernummer;
        row.DeviceNumber = this.SerienNummer;
        row.FehlerGefunden = this.ErrorFound;
        row.FehlerBeseitigt = this.ErrorResolved;
        row.Einbauort = this.textBoxEinbauort.Text.Trim();
        row.Einbaudatum = this.textBoxEinbaudatum.Text.Trim();
        row.Masseinheit = this.textBoxUnit.Text.Trim();
        row.Messbereich = this.textBoxMessbereich.Text.Trim();
        row.Stichtag = this.Stichtag;
        row.AbweichenderStichtag = this.AbweichenderStichtag;
        row.Kommentar = this.textBoxKommentar.Text.Trim();
        row.Montageauftrag = this.textBoxAuftragsnummer.Text.Trim();
        row.ManuellGespeichert = this.ManuellGespeichert;
        row.kWh_Aktuell = this.EnergieAktuell;
        row.kWh_Stichtag = this.EnergieStichtag;
        row.kWh_010109 = this.Energie090101;
        row.kWh_010209 = this.Energie090201;
        row.kWh_010309 = this.Energie090301;
        row.kWh_010409 = this.Energie090401;
        row.kWh_010509 = this.Energie090501;
        row.kWh_010609 = this.Energie090601;
        row.kWh_010709 = this.Energie090701;
        row.kWh_010809 = this.Energie090801;
        row.kWh_010909 = this.Energie090901;
        row.kWh_011009 = this.Energie091001;
        row.kWh_011109 = this.Energie091101;
        row.kWh_011209 = this.Energie091201;
        row.kWh_010110 = this.Energie100101;
        row.kWh_010210 = this.Energie100201;
        row.kWh_010310 = this.Energie100301;
        row.kWh_010410 = this.Energie100401;
        row.kWh_010510 = this.Energie100501;
        row.kWh_010610 = this.Energie100601;
        row.kWh_010710 = this.Energie100701;
        row.Max_kW1 = this.Max_kW1;
        row.Max_kW1_Month = this.Max_kW1_Month;
        row.Max_kW2 = this.Max_kW2;
        row.Max_kW2_Month = this.Max_kW2_Month;
        row.Max_QmPerHour1 = this.Max_QmPerHour1;
        row.Max_QmPerHour1_Month = this.Max_QmPerHour1_Month;
        row.Max_QmPerHour2 = this.Max_QmPerHour2;
        row.Max_QmPerHour2_Month = this.Max_QmPerHour2_Month;
        this.MyLogData.GMM_Serie2DeviceLogData.AddGMM_Serie2DeviceLogDataRow(row);
        this.ShowMessageLine("Schreibe Datei ...");
        this.Refresh();
        if (File.Exists(this.DataSetBackup3FileName))
          File.Delete(this.DataSetBackup3FileName);
        if (File.Exists(this.DataSetBackup2FileName))
          File.Move(this.DataSetBackup2FileName, this.DataSetBackup3FileName);
        if (File.Exists(this.DataSetBackup1FileName))
          File.Move(this.DataSetBackup1FileName, this.DataSetBackup2FileName);
        if (File.Exists(this.DataSetFileName))
          File.Move(this.DataSetFileName, this.DataSetBackup1FileName);
        this.MyLogData.WriteXml(this.DataSetFileName, XmlWriteMode.WriteSchema);
        try
        {
          string str1 = "WMZ_Ablesung_LG_" + this.Liegenschaft;
          if (this.Stichtag != DateTime.MinValue)
            str1 = str1 + "_Stichtag_" + this.Stichtag.ToShortDateString();
          this.CSV_FileName = Path.Combine(SystemValues.LoggDataPath, str1 + ".CSV");
          if (!File.Exists(this.CSV_FileName))
          {
            using (StreamWriter streamWriter = new StreamWriter(this.CSV_FileName, false, Encoding.Unicode))
            {
              this.CSV_Line.Length = 0;
              for (int index = 0; index < this.MyLogData.GMM_Serie2DeviceLogData.Columns.Count; ++index)
              {
                if (index > 0)
                  this.CSV_Line.Append(this.Seperator);
                this.CSV_Line.Append(this.MyLogData.GMM_Serie2DeviceLogData.Columns[index].ColumnName);
              }
              streamWriter.WriteLine(this.CSV_Line.ToString());
            }
          }
          using (StreamWriter streamWriter = new StreamWriter(this.CSV_FileName, true, Encoding.Unicode))
          {
            this.CSV_Line.Length = 0;
            for (int index = 0; index < this.MyLogData.GMM_Serie2DeviceLogData.Columns.Count; ++index)
            {
              if (index > 0)
                this.CSV_Line.Append(this.Seperator);
              object obj = row[index];
              string str2;
              if (obj.GetType() == typeof (string))
                str2 = obj.ToString().Replace("\t", "_").ToString().Replace(Environment.NewLine, "|");
              else if (obj.GetType() == typeof (DateTime))
              {
                string columnName = this.MyLogData.GMM_Serie2DeviceLogData.Columns[index].ColumnName;
                DateTime dateTime = (DateTime) obj;
                str2 = !(dateTime == DateTime.MinValue) ? (!(columnName == "ChangeDateTime") ? dateTime.ToShortDateString() : dateTime.ToString("dd.MM.yyyy HH:mm:ss")) : string.Empty;
              }
              else
                str2 = obj.ToString();
              this.CSV_Line.Append(str2);
            }
            streamWriter.WriteLine(this.CSV_Line.ToString());
          }
        }
        catch (Exception ex)
        {
          int num = (int) GMM_MessageBox.ShowMessage(this.CSV_FileName, "File error" + Environment.NewLine + ex.Message);
        }
        this.ShowMessageLine("Schreiben erfolgreich abgeschlossen.");
        this.ShowMessageLine("Vorhandene Datensätze: " + this.MyLogData.GMM_Serie2DeviceLogData.Rows.Count.ToString());
      }
      catch (Exception ex)
      {
        this.ShowErrorMessage("Die Daten konnten nicht in die Datei geschrieben werden");
        this.ShowMessageLine(ex.ToString());
      }
    }

    private void LesenRaparierenUndSchreiben()
    {
      this.Cursor = Cursors.WaitCursor;
      this.Enabled = false;
      this.Refresh();
      if (this.CheckInputData())
      {
        string AdditionalError = string.Empty;
        bool flag1 = false;
        this.MyFunctions.checksumErrorsAsWarning = true;
        string theFirmwareVersion;
        try
        {
          flag1 = this.MyFunctions.checkConnection(out theFirmwareVersion) == 0;
          if (flag1)
          {
            this.ShowMessageLine("Verbindung ok");
            this.ShowMessageLine("Starte auslesen ...");
            this.BaseStatusText = "Lese Addresse: ";
            flag1 = this.MyFunctions.ReadConnectedDevice(out this.DeviceIdentity);
            if (flag1)
              this.ShowMessageLine("Auslesen erfolgreich beendet");
            else
              this.ShowMessageLine("Lesefehler");
          }
          else
          {
            AdditionalError = ZR_ClassLibMessages.GetLastError() != ZR_ClassLibMessages.LastErrors.CommunicationError ? ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription : " ";
            this.MyFunctions.ClearConnectedReadAndWorkMeter();
          }
        }
        catch (Exception ex)
        {
          AdditionalError = ex.Message;
        }
        this.MyFunctions.checksumErrorsAsWarning = true;
        if (AdditionalError.Length > 0)
        {
          this.ShowHandlerMessage("Lesefehler", AdditionalError);
          this.ShowMessageLine("Wurde die Taste am Gerät kurz vor dem lesen gedrückt?");
          this.ShowMessageLine("War der Optokopf mit dem Kabel zur LCD aufgesetzt?");
        }
        else
          this.ShowHandlerMessage();
        if (flag1 && this.LoadDataFromMeter())
        {
          if (!this.NichtReparieren)
          {
            try
            {
              this.BaseStatusText = "Programmiere Addresse: ";
              this.ShowMessageLine("Starte programmieren ...");
              if (this.MyFunctions.progDevice(DateTime.Now))
              {
                this.ShowHandlerMessage("Programmierfehler");
                this.ShowMessageLine("Programmieren erfolgreich abgeschlossen");
                this.ErrorResolved = this.ErrorFound;
                this.BaseStatusText = "Testlesen: ";
                this.ShowMessageLine("");
                this.ShowMessageLine("Testlesen ob alles in Ordnung ist.");
                Thread.Sleep(1000);
                ZR_ClassLibMessages.ClearErrors();
                bool flag2 = this.MyFunctions.checkConnection(out theFirmwareVersion) == 0;
                if (flag2)
                  flag2 = this.MyFunctions.ReadConnectedDevice(out this.DeviceIdentity);
                ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo();
                if (flag2 && lastErrorInfo.LastWarnings.Length == 0)
                {
                  this.ShowMessageLine("Alles ok.");
                  this.ShowMessageLine("");
                  goto label_21;
                }
                else
                {
                  this.ShowErrorMessage("Das Gerät ist nicht vollständig in Ordnung!! Es muss ausgetauscht werden.");
                  goto label_21;
                }
              }
            }
            catch
            {
            }
            this.ShowErrorMessage("Schreibfehler");
            goto label_25;
          }
          else
            this.ShowMessageLine("!!! Das Gerät wird nicht programmiert !!!");
label_21:
          if (!this.NichtSpeichern)
            this.WriteData();
          this.MyFunctions.ClearConnectedReadAndWorkMeter();
        }
      }
label_25:
      this.textBoxCommunicationStatus.Clear();
      this.Cursor = Cursors.Default;
      this.Enabled = true;
      this.Refresh();
    }

    private void ShowHandlerMessage() => this.ShowHandlerMessage(string.Empty, string.Empty);

    private void ShowHandlerMessage(string StartMessage)
    {
      this.ShowHandlerMessage(StartMessage, string.Empty);
    }

    internal void ShowHandlerMessage(string StartMessage, string AdditionalError)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
      string lastWarnings = errorAndClearError.LastWarnings;
      string TheMessage = errorAndClearError.LastErrorDescription;
      if (TheMessage.Length > 0 || AdditionalError.Length > 0)
      {
        if (StartMessage.Trim().Length > 0)
          TheMessage = StartMessage + Environment.NewLine + Environment.NewLine + TheMessage;
        if (AdditionalError.Length > 0)
          TheMessage = TheMessage + Environment.NewLine + AdditionalError;
        this.ShowErrorMessage(TheMessage);
      }
      else if (lastWarnings.Length > 0)
        this.ShowDesignerMessages(lastWarnings);
    }

    internal void ShowErrorMessage(string TheMessage)
    {
      this.textBoxStatus.AppendText("Fehler:");
      this.textBoxStatus.AppendText(Environment.NewLine);
      this.textBoxStatus.AppendText(TheMessage);
      this.textBoxStatus.AppendText(Environment.NewLine);
      this.textBoxStatus.AppendText(Environment.NewLine);
    }

    internal void ShowDesignerMessages(string Warning)
    {
      this.textBoxStatus.AppendText("Warnungen und Informationen:");
      this.textBoxStatus.AppendText(Environment.NewLine);
      this.textBoxStatus.AppendText(Warning);
      this.textBoxStatus.AppendText(Environment.NewLine);
      this.textBoxStatus.AppendText(Environment.NewLine);
    }

    internal void ShowMessageLine(string TheMessage)
    {
      this.textBoxStatus.AppendText(TheMessage);
      this.textBoxStatus.AppendText(Environment.NewLine);
    }

    private void buttonLesen_Click(object sender, EventArgs e)
    {
      this.ManuellGespeichert = false;
      this.NichtReparieren = false;
      this.NichtSpeichern = false;
      this.LesenRaparierenUndSchreiben();
    }

    private void buttonTestlesen_Click(object sender, EventArgs e)
    {
      this.NichtReparieren = true;
      this.NichtSpeichern = true;
      this.LesenRaparierenUndSchreiben();
    }

    private void buttonSpeichernOhneLesen_Click(object sender, EventArgs e)
    {
      this.ManuellGespeichert = true;
      if (!this.CheckInputData())
        return;
      if (!int.TryParse(this.textBoxDeviceNumber.Text, out this.SerienNummer))
        this.ShowErrorMessage("Seriennummer nicht gültig.");
      else if (this.textBoxStichtag.Text.Trim().Length > 0 && !DateTime.TryParse(this.textBoxStichtag.Text, out this.Stichtag))
        this.ShowErrorMessage("Stichtag nicht gültig.");
      else if (this.textBoxEnergieAktuell.Text.Trim().Length > 0 && !int.TryParse(this.textBoxEnergieAktuell.Text, out this.EnergieAktuell))
        this.ShowErrorMessage("Aktuelle Energie nicht gültig.");
      else if (this.textBoxEnergieAmStichtag.Text.Trim().Length > 0 && !int.TryParse(this.textBoxEnergieAmStichtag.Text, out this.EnergieStichtag))
      {
        this.ShowErrorMessage("Aktuelle Energie nicht gültig.");
      }
      else
      {
        this.ErrorFound = true;
        this.ErrorResolved = false;
        this.WriteData();
      }
    }

    private void buttonAblaufZumKommentar_Click(object sender, EventArgs e)
    {
      this.textBoxKommentar.AppendText(Environment.NewLine);
      this.textBoxKommentar.AppendText("--- Ablauf ---");
      this.textBoxKommentar.AppendText(Environment.NewLine);
      this.textBoxKommentar.AppendText(this.textBoxStatus.Text);
    }

    private void buttonDatenliste_Click(object sender, EventArgs e)
    {
      if (this.TheDataList == null)
        this.TheDataList = new Datenliste((DataTable) this.MyLogData.GMM_Serie2DeviceLogData);
      int num = (int) this.TheDataList.ShowDialog();
    }

    private void buttonOpenFolder_Click(object sender, EventArgs e)
    {
      string directoryName = Path.GetDirectoryName(this.DataSetFileName);
      try
      {
        new Process()
        {
          StartInfo = new ProcessStartInfo("explorer.exe", directoryName)
        }.Start();
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Global Meter Manager", "Explorer start error", true);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.buttonLesen = new Button();
      this.textBoxStatus = new TextBox();
      this.textBoxCommunicationStatus = new TextBox();
      this.textBoxAusleser = new TextBox();
      this.label1 = new Label();
      this.textBoxLiegenschaft = new TextBox();
      this.label2 = new Label();
      this.textBoxNutzernummer = new TextBox();
      this.label3 = new Label();
      this.textBoxDeviceNumber = new TextBox();
      this.label4 = new Label();
      this.textBoxEinbauort = new TextBox();
      this.label5 = new Label();
      this.textBoxMessbereich = new TextBox();
      this.label6 = new Label();
      this.textBoxUnit = new TextBox();
      this.label7 = new Label();
      this.textBoxStichtag = new TextBox();
      this.label8 = new Label();
      this.textBoxAbweichenderStichtag = new TextBox();
      this.label9 = new Label();
      this.textBoxEinbaudatum = new TextBox();
      this.label10 = new Label();
      this.textBoxAuftragsnummer = new TextBox();
      this.label11 = new Label();
      this.textBoxEnergieAktuell = new TextBox();
      this.label12 = new Label();
      this.textBoxEnergieAmStichtag = new TextBox();
      this.label13 = new Label();
      this.textBoxKommentar = new TextBox();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.buttonSpeichernOhneLesen = new Button();
      this.buttonAblaufZumKommentar = new Button();
      this.buttonDatenliste = new Button();
      this.toolTip1 = new ToolTip(this.components);
      this.buttonTestlesen = new Button();
      this.buttonOpenFolder = new Button();
      this.SuspendLayout();
      this.buttonLesen.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonLesen.Location = new Point(544, 528);
      this.buttonLesen.Name = "buttonLesen";
      this.buttonLesen.Size = new Size(198, 23);
      this.buttonLesen.TabIndex = 0;
      this.buttonLesen.Text = "Lesen, korrigieren und speichern";
      this.toolTip1.SetToolTip((Control) this.buttonLesen, "Vollautomatischer Ablauf. Vor dem Start alle Felder ausfüllen!!");
      this.buttonLesen.UseVisualStyleBackColor = true;
      this.buttonLesen.Click += new System.EventHandler(this.buttonLesen_Click);
      this.textBoxStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxStatus.Location = new Point(14, 38);
      this.textBoxStatus.Multiline = true;
      this.textBoxStatus.Name = "textBoxStatus";
      this.textBoxStatus.Size = new Size(323, 449);
      this.textBoxStatus.TabIndex = 1;
      this.textBoxCommunicationStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxCommunicationStatus.Location = new Point(14, 531);
      this.textBoxCommunicationStatus.Name = "textBoxCommunicationStatus";
      this.textBoxCommunicationStatus.Size = new Size(322, 20);
      this.textBoxCommunicationStatus.TabIndex = 2;
      this.textBoxAusleser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxAusleser.Location = new Point(513, 12);
      this.textBoxAusleser.Name = "textBoxAusleser";
      this.textBoxAusleser.Size = new Size(229, 20);
      this.textBoxAusleser.TabIndex = 3;
      this.textBoxAusleser.Text = "?";
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(431, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Ableser Name";
      this.textBoxLiegenschaft.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxLiegenschaft.Location = new Point(513, 38);
      this.textBoxLiegenschaft.Name = "textBoxLiegenschaft";
      this.textBoxLiegenschaft.Size = new Size(229, 20);
      this.textBoxLiegenschaft.TabIndex = 3;
      this.textBoxLiegenschaft.Text = "PLZ_Musterstadt_Musterstr_HNR";
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(436, 41);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Liegenschaft";
      this.textBoxNutzernummer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxNutzernummer.Location = new Point(513, 64);
      this.textBoxNutzernummer.Name = "textBoxNutzernummer";
      this.textBoxNutzernummer.Size = new Size(229, 20);
      this.textBoxNutzernummer.TabIndex = 3;
      this.textBoxNutzernummer.Text = "-1";
      this.toolTip1.SetToolTip((Control) this.textBoxNutzernummer, "Eine Zahl zwischen 0 und 9999");
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(432, 67);
      this.label3.Name = "label3";
      this.label3.Size = new Size(75, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Nutzernummer";
      this.textBoxDeviceNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxDeviceNumber.Location = new Point(513, 90);
      this.textBoxDeviceNumber.Name = "textBoxDeviceNumber";
      this.textBoxDeviceNumber.Size = new Size(229, 20);
      this.textBoxDeviceNumber.TabIndex = 3;
      this.textBoxDeviceNumber.Text = "0";
      this.toolTip1.SetToolTip((Control) this.textBoxDeviceNumber, "Wird beim lesen automatisch  eingetragen.");
      this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(431, 93);
      this.label4.Name = "label4";
      this.label4.Size = new Size(76, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Gerätenummer";
      this.textBoxEinbauort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxEinbauort.Location = new Point(513, 116);
      this.textBoxEinbauort.Name = "textBoxEinbauort";
      this.textBoxEinbauort.Size = new Size(229, 20);
      this.textBoxEinbauort.TabIndex = 3;
      this.label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(455, 119);
      this.label5.Name = "label5";
      this.label5.Size = new Size(52, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Einbauort";
      this.textBoxMessbereich.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxMessbereich.Location = new Point(513, 142);
      this.textBoxMessbereich.Name = "textBoxMessbereich";
      this.textBoxMessbereich.Size = new Size(229, 20);
      this.textBoxMessbereich.TabIndex = 3;
      this.textBoxMessbereich.Text = "Heizung";
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(440, 145);
      this.label6.Name = "label6";
      this.label6.Size = new Size(67, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "Messbereich";
      this.textBoxUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxUnit.Location = new Point(513, 168);
      this.textBoxUnit.Name = "textBoxUnit";
      this.textBoxUnit.Size = new Size(229, 20);
      this.textBoxUnit.TabIndex = 3;
      this.textBoxUnit.Text = "kWh";
      this.label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(448, 171);
      this.label7.Name = "label7";
      this.label7.Size = new Size(59, 13);
      this.label7.TabIndex = 4;
      this.label7.Text = "Maßeinheit";
      this.textBoxStichtag.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxStichtag.Location = new Point(513, 194);
      this.textBoxStichtag.Name = "textBoxStichtag";
      this.textBoxStichtag.Size = new Size(229, 20);
      this.textBoxStichtag.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.textBoxStichtag, "Wird beim lesen automatisch  eingetragen. Immer mit Jahr eintragen.");
      this.label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(461, 197);
      this.label8.Name = "label8";
      this.label8.Size = new Size(46, 13);
      this.label8.TabIndex = 4;
      this.label8.Text = "Stichtag";
      this.textBoxAbweichenderStichtag.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxAbweichenderStichtag.Location = new Point(513, 220);
      this.textBoxAbweichenderStichtag.Name = "textBoxAbweichenderStichtag";
      this.textBoxAbweichenderStichtag.Size = new Size(229, 20);
      this.textBoxAbweichenderStichtag.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.textBoxAbweichenderStichtag, "Nur bei bekannten Abweichungen angeben. Immer mit Jahr z.B. 1.1.10");
      this.label9.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(390, 223);
      this.label9.Name = "label9";
      this.label9.Size = new Size(117, 13);
      this.label9.TabIndex = 4;
      this.label9.Text = "Abweichender Stichtag";
      this.textBoxEinbaudatum.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxEinbaudatum.Location = new Point(513, 246);
      this.textBoxEinbaudatum.Name = "textBoxEinbaudatum";
      this.textBoxEinbaudatum.Size = new Size(229, 20);
      this.textBoxEinbaudatum.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.textBoxEinbaudatum, "Immer mit Jahr angeben z.B. 25.7.9");
      this.label10.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(438, 249);
      this.label10.Name = "label10";
      this.label10.Size = new Size(69, 13);
      this.label10.TabIndex = 4;
      this.label10.Text = "Einbaudatum";
      this.textBoxAuftragsnummer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxAuftragsnummer.Location = new Point(513, 272);
      this.textBoxAuftragsnummer.Name = "textBoxAuftragsnummer";
      this.textBoxAuftragsnummer.Size = new Size(229, 20);
      this.textBoxAuftragsnummer.TabIndex = 3;
      this.textBoxAuftragsnummer.Text = "0";
      this.label11.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(359, 275);
      this.label11.Name = "label11";
      this.label11.Size = new Size(148, 13);
      this.label11.TabIndex = 4;
      this.label11.Text = "Montageauftragsnummer SAP";
      this.textBoxEnergieAktuell.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxEnergieAktuell.Location = new Point(513, 298);
      this.textBoxEnergieAktuell.Name = "textBoxEnergieAktuell";
      this.textBoxEnergieAktuell.Size = new Size(229, 20);
      this.textBoxEnergieAktuell.TabIndex = 3;
      this.textBoxEnergieAktuell.Text = "0";
      this.toolTip1.SetToolTip((Control) this.textBoxEnergieAktuell, "Wird beim lesen automatisch  eingetragen. -1 für Wert nicht bekannt.");
      this.label12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(400, 301);
      this.label12.Name = "label12";
      this.label12.Size = new Size(107, 13);
      this.label12.TabIndex = 4;
      this.label12.Text = "Aktueller Energiewert";
      this.textBoxEnergieAmStichtag.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxEnergieAmStichtag.Location = new Point(513, 324);
      this.textBoxEnergieAmStichtag.Name = "textBoxEnergieAmStichtag";
      this.textBoxEnergieAmStichtag.Size = new Size(229, 20);
      this.textBoxEnergieAmStichtag.TabIndex = 3;
      this.textBoxEnergieAmStichtag.Text = "0";
      this.toolTip1.SetToolTip((Control) this.textBoxEnergieAmStichtag, "Wird beim lesen automatisch  eingetragen. -1 für Wert nicht bekannt.");
      this.label13.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(405, 327);
      this.label13.Name = "label13";
      this.label13.Size = new Size(102, 13);
      this.label13.TabIndex = 4;
      this.label13.Text = "Energie am Stichtag";
      this.textBoxKommentar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxKommentar.Location = new Point(362, 377);
      this.textBoxKommentar.Multiline = true;
      this.textBoxKommentar.Name = "textBoxKommentar";
      this.textBoxKommentar.ScrollBars = ScrollBars.Both;
      this.textBoxKommentar.Size = new Size(380, 76);
      this.textBoxKommentar.TabIndex = 3;
      this.toolTip1.SetToolTip((Control) this.textBoxKommentar, "Freier Kommentar zu dem Zustand des Gerätes. Vor dem Auslesen ausfüllen!!");
      this.label14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(359, 353);
      this.label14.Name = "label14";
      this.label14.Size = new Size(60, 13);
      this.label14.TabIndex = 4;
      this.label14.Text = "Kommentar";
      this.label15.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(14, 509);
      this.label15.Name = "label15";
      this.label15.Size = new Size(60, 13);
      this.label15.TabIndex = 4;
      this.label15.Text = "Meldungen";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(14, 15);
      this.label16.Name = "label16";
      this.label16.Size = new Size(37, 13);
      this.label16.TabIndex = 4;
      this.label16.Text = "Ablauf";
      this.buttonSpeichernOhneLesen.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSpeichernOhneLesen.Location = new Point(544, 470);
      this.buttonSpeichernOhneLesen.Name = "buttonSpeichernOhneLesen";
      this.buttonSpeichernOhneLesen.Size = new Size(198, 23);
      this.buttonSpeichernOhneLesen.TabIndex = 0;
      this.buttonSpeichernOhneLesen.Text = "Ohne Gerätezugriff speichern";
      this.toolTip1.SetToolTip((Control) this.buttonSpeichernOhneLesen, "Die Daten, außer Ablauf und Meldungen, werden unverändert in die Ausgabedatei geschreiben.");
      this.buttonSpeichernOhneLesen.UseVisualStyleBackColor = true;
      this.buttonSpeichernOhneLesen.Click += new System.EventHandler(this.buttonSpeichernOhneLesen_Click);
      this.buttonAblaufZumKommentar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonAblaufZumKommentar.Location = new Point(362, 470);
      this.buttonAblaufZumKommentar.Name = "buttonAblaufZumKommentar";
      this.buttonAblaufZumKommentar.Size = new Size(166, 23);
      this.buttonAblaufZumKommentar.TabIndex = 0;
      this.buttonAblaufZumKommentar.Text = "Ablauf zum Kommentar";
      this.toolTip1.SetToolTip((Control) this.buttonAblaufZumKommentar, "Hängt die Daten aus dem Ablauffeld an den Kommentar an.");
      this.buttonAblaufZumKommentar.UseVisualStyleBackColor = true;
      this.buttonAblaufZumKommentar.Click += new System.EventHandler(this.buttonAblaufZumKommentar_Click);
      this.buttonDatenliste.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonDatenliste.Location = new Point(362, 499);
      this.buttonDatenliste.Name = "buttonDatenliste";
      this.buttonDatenliste.Size = new Size(166, 23);
      this.buttonDatenliste.TabIndex = 0;
      this.buttonDatenliste.Text = "Datenliste anzeigen";
      this.toolTip1.SetToolTip((Control) this.buttonDatenliste, "Zeigt alle schon gelesenen Daten als große Liste an.");
      this.buttonDatenliste.UseVisualStyleBackColor = true;
      this.buttonDatenliste.Click += new System.EventHandler(this.buttonDatenliste_Click);
      this.buttonTestlesen.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonTestlesen.Location = new Point(544, 499);
      this.buttonTestlesen.Name = "buttonTestlesen";
      this.buttonTestlesen.Size = new Size(198, 23);
      this.buttonTestlesen.TabIndex = 0;
      this.buttonTestlesen.Text = "Testlesen";
      this.toolTip1.SetToolTip((Control) this.buttonTestlesen, "Das Gerät wird nicht verändert und es werden keine Daten gespeichert");
      this.buttonTestlesen.UseVisualStyleBackColor = true;
      this.buttonTestlesen.Click += new System.EventHandler(this.buttonTestlesen_Click);
      this.buttonOpenFolder.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOpenFolder.Location = new Point(362, 528);
      this.buttonOpenFolder.Name = "buttonOpenFolder";
      this.buttonOpenFolder.Size = new Size(166, 23);
      this.buttonOpenFolder.TabIndex = 0;
      this.buttonOpenFolder.Text = "Datenordner öffnen";
      this.buttonOpenFolder.UseVisualStyleBackColor = true;
      this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(754, 563);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.label15);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxKommentar);
      this.Controls.Add((Control) this.textBoxEnergieAmStichtag);
      this.Controls.Add((Control) this.textBoxEnergieAktuell);
      this.Controls.Add((Control) this.textBoxAuftragsnummer);
      this.Controls.Add((Control) this.textBoxEinbaudatum);
      this.Controls.Add((Control) this.textBoxAbweichenderStichtag);
      this.Controls.Add((Control) this.textBoxStichtag);
      this.Controls.Add((Control) this.textBoxUnit);
      this.Controls.Add((Control) this.textBoxMessbereich);
      this.Controls.Add((Control) this.textBoxEinbauort);
      this.Controls.Add((Control) this.textBoxDeviceNumber);
      this.Controls.Add((Control) this.textBoxNutzernummer);
      this.Controls.Add((Control) this.textBoxLiegenschaft);
      this.Controls.Add((Control) this.textBoxAusleser);
      this.Controls.Add((Control) this.textBoxCommunicationStatus);
      this.Controls.Add((Control) this.textBoxStatus);
      this.Controls.Add((Control) this.buttonOpenFolder);
      this.Controls.Add((Control) this.buttonDatenliste);
      this.Controls.Add((Control) this.buttonAblaufZumKommentar);
      this.Controls.Add((Control) this.buttonSpeichernOhneLesen);
      this.Controls.Add((Control) this.buttonTestlesen);
      this.Controls.Add((Control) this.buttonLesen);
      this.Name = nameof (ERR8002_Form);
      this.Text = "Minocal nach ERR 8002 reanimieren";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
