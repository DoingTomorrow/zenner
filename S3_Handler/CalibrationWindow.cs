// Decompiled with JetBrains decompiler
// Type: S3_Handler.CalibrationWindow
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class CalibrationWindow : Form
  {
    private static float[,] adcTempRanges = new float[3, 2]
    {
      {
        1f,
        20f
      },
      {
        45f,
        80f
      },
      {
        90f,
        149f
      }
    };
    private static float[,] volTempRanges = new float[2, 2]
    {
      {
        1f,
        25f
      },
      {
        40f,
        60f
      }
    };
    private S3_HandlerFunctions MyFunctions;
    private bool communicationRuns = false;
    private float bathTempLow = 0.0f;
    private float bathTempMiddle = 0.0f;
    private float bathTempHight = 0.0f;
    private float flowTempLow;
    private float flowTempMiddle;
    private float volumeMeterTemperature = 0.0f;
    private float deltaVolumeTempAbsMax = 0.0f;
    private string baseStateText;
    private int loopReadOkCounter;
    private int loopReadErrCounter;
    private int contLoopReadOkCounter;
    private CalibrationWindow.CalibrationStateTemp calStateTemp;
    private TDC_Calibration.CalibrationStateVol calStateVolume;
    private TDC_Calibration tdcCalibration;
    internal static Logger TDC_ClaibrationLogger = LogManager.GetLogger("TDC_Claibration");
    private bool oldTimerEnabled;
    private const int numberOfTempPoints = 5;
    private List<float> flowTempList = new List<float>();
    private List<float> returnTempList = new List<float>();
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private GroupBox groupBoxTemperatureCalibration;
    private Button buttonTempMeasureHightAndCalibrate;
    private Button buttonTempMeasureMiddle;
    private Button buttonTempMeasureLow;
    private TextBox textBoxBathTemperatureHight;
    private TextBox textBoxBathTemperatureMiddle;
    private TextBox textBoxBathTemperatureLow;
    private Label label3;
    private Label label2;
    private Label label1;
    private GroupBox groupBoxVolumeCalibration;
    private Button buttonVolumeCalibrate;
    private Label lblQpErr;
    private TextBox txtBxQpErr;
    private Button buttonWrite;
    private Button buttonRead;
    private GroupBox groupBox1;
    private Label label5;
    private TextBox textBoxSerialNumber;
    private Button buttonSensorTemp;
    private TextBox textBoxSensorTemperatureFlow;
    private Label label6;
    private Button buttonIUF_HightTempZeroFlow;
    private Label labelIUF_HightTempZeroFlow;
    private TextBox textBoxSensorTemperatureReturn;
    private Label label7;
    private Timer timer1;
    private TextBox textBoxState;
    private GroupBox groupBox2;
    private Label label8;
    private TextBox textBoxTempValues;
    private CheckBox checkBoxDisableTempCheck;
    private Button buttonZeroCalibrationCheck;
    private GroupBox groupBoxFrequency;
    private Button buttonCalibrateRadioFrequency;
    private Label label10;
    private Button buttonCalibrateClockError;
    private TextBox textBoxRadioFrequencyError;
    private Label label9;
    private TextBox textBoxClockError;
    private Button buttonCalibrateRadioPower;
    private Label label11;
    private TextBox textBoxRadioPower;
    private Label lblQ;
    private Label lblQi;
    private TextBox txtBxQi;
    private TextBox txtBxQ;
    private Label lblQp;
    private TextBox txtBxQp;
    private Label lblQErr;
    private Label lblQiErr;
    private TextBox txtBxQiErr;
    private TextBox txtBxQErr;
    private RadioButton rBtnQiQQp;
    private RadioButton rBtnQi;
    private RadioButton rBtnQp;

    public CalibrationWindow(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.labelIUF_HightTempZeroFlow.Text = "Calibrate IUF zero flow [" + CalibrationWindow.volTempRanges[1, 0].ToString() + ".." + CalibrationWindow.volTempRanges[1, 1].ToString() + " °C]";
      if (this.MyFunctions.IsHandlerCompleteEnabled())
        this.checkBoxDisableTempCheck.Visible = true;
      this.MyFunctions.OnS3_HandlerMessage += new EventHandler<GMM_EventArgs>(this.WorkReceivedMessages);
      this.SetStartState();
    }

    private void WorkReceivedMessages(object sender, GMM_EventArgs TheMessage)
    {
      if (!(sender is TDC_Calibration))
        return;
      if (this.textBoxState.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.WorkReceivedMessages), sender, (object) TheMessage);
        }
        catch
        {
        }
      }
      else
      {
        if (TheMessage.TheMessageType != GMM_EventArgs.MessageType.MessageAndProgressPercentage)
          return;
        this.textBoxState.Text = "Progress: " + TheMessage.InfoNumber.ToString() + "%";
        if (!string.IsNullOrEmpty(TheMessage.InfoText))
        {
          TextBox textBoxState = this.textBoxState;
          textBoxState.Text = textBoxState.Text + " - " + TheMessage.InfoText;
        }
        this.Refresh();
      }
    }

    private void HoldFormEvents()
    {
      this.Enabled = false;
      this.oldTimerEnabled = this.timer1.Enabled;
      this.WaitTimerCommunicationStopps();
    }

    private void ResetFormEvents()
    {
      this.Enabled = this.oldTimerEnabled;
      this.Enabled = true;
    }

    private void buttonRead_Click(object sender, EventArgs e)
    {
      this.DisableTimer();
      this.HoldFormEvents();
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.Clear();
      this.SetStartState();
      this.ResetStateInfo("Reading ...");
      if (this.MyFunctions.ReadConnectedDevice())
      {
        this.SetStartState();
        this.ResetStateInfo("Read ok");
      }
      else
        this.ResetStateInfo("Read error");
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.ResetFormEvents();
      this.MyFunctions.SendMessageSwitchThread(21, GMM_EventArgs.MessageType.StatusChanged);
    }

    private void buttonWrite_Click(object sender, EventArgs e)
    {
      this.HoldFormEvents();
      this.DisableTimer();
      ZR_ClassLibMessages.ClearErrors();
      if (this.MyFunctions.WriteChangesToConnectedDevice())
      {
        this.SetStartState();
        this.ResetStateInfo("Write ok");
        this.MyFunctions.Clear();
      }
      else
        this.ResetStateInfo("Write error");
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.ResetFormEvents();
    }

    private bool SetStartState()
    {
      this.buttonZeroCalibrationCheck.Visible = false;
      this.ResetStateInfo("Stop");
      this.SetTempState(CalibrationWindow.CalibrationStateTemp.TempLow);
      this.labelIUF_HightTempZeroFlow.Visible = false;
      if (this.MyFunctions.MyMeters.WorkMeter == null || this.MyFunctions.MyMeters.ConnectedMeter == null)
      {
        this.buttonRead.Enabled = true;
        this.buttonWrite.Enabled = false;
        this.buttonSensorTemp.Enabled = false;
        this.buttonTempMeasureLow.Enabled = false;
        this.buttonTempMeasureMiddle.Enabled = false;
        this.buttonTempMeasureHightAndCalibrate.Enabled = false;
        this.buttonIUF_HightTempZeroFlow.Enabled = false;
        this.buttonVolumeCalibrate.Enabled = false;
      }
      else
      {
        if (this.MyFunctions.adc_Calibration == null)
          this.MyFunctions.adc_Calibration = new ADC_Calibration(this.MyFunctions);
        VolumeGraphCalibration.GarantObjectAvailable(this.MyFunctions);
        if (this.MyFunctions.MyMeters.WorkMeter.MyIdentification.GetVolumeMeterType() == ParameterService.HardwareMaskElements.Ultrasonic || this.MyFunctions.MyMeters.WorkMeter.MyIdentification.GetVolumeMeterType() == ParameterService.HardwareMaskElements.UltrasonicDirect)
        {
          this.buttonIUF_HightTempZeroFlow.Visible = true;
          this.labelIUF_HightTempZeroFlow.Visible = true;
          this.tdcCalibration = this.MyFunctions.volumeGraphCalibration as TDC_Calibration;
          this.calStateVolume = this.tdcCalibration.GetCalibrationState();
          if (this.calStateVolume == TDC_Calibration.CalibrationStateVol.zeroFlowCalibrated || this.calStateVolume == TDC_Calibration.CalibrationStateVol.complete)
            this.buttonVolumeCalibrate.Enabled = true;
          if (this.MyFunctions.IsHandlerCompleteEnabled())
            this.buttonZeroCalibrationCheck.Visible = true;
        }
        else
        {
          this.buttonVolumeCalibrate.Enabled = true;
          this.rBtnQp.Visible = false;
          this.lblQp.Visible = false;
          this.txtBxQp.Visible = false;
          this.lblQpErr.Visible = true;
          this.txtBxQpErr.Visible = true;
          this.rBtnQi.Visible = false;
          this.lblQi.Visible = false;
          this.txtBxQi.Visible = false;
          this.lblQiErr.Visible = false;
          this.txtBxQiErr.Visible = false;
          this.rBtnQiQQp.Visible = false;
          this.lblQ.Visible = false;
          this.txtBxQ.Visible = false;
          this.lblQErr.Visible = false;
          this.txtBxQErr.Visible = false;
        }
        this.buttonIUF_HightTempZeroFlow.Enabled = false;
        this.textBoxSerialNumber.Text = this.MyFunctions.MyMeters.WorkMeter.MyIdentification.FullSerialNumber;
        this.buttonRead.Enabled = true;
        this.buttonWrite.Enabled = true;
        this.buttonSensorTemp.Enabled = true;
        this.buttonTempMeasureLow.Enabled = false;
        this.buttonTempMeasureMiddle.Enabled = false;
        this.buttonTempMeasureHightAndCalibrate.Enabled = false;
        if (((uint) this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName["Device_Setup_2"].GetUshortValue() & 1U) > 0U)
          this.textBoxSensorTemperatureFlow.BackColor = Color.LightYellow;
        else
          this.textBoxSensorTemperatureReturn.BackColor = Color.LightYellow;
        IList<string> keys = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName.Keys;
        S3_ParameterNames s3ParameterNames = S3_ParameterNames.radioPower;
        string str = s3ParameterNames.ToString();
        if (keys.Contains(str))
        {
          SortedList<string, S3_Parameter> parameterByName = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.radioPower;
          string key = s3ParameterNames.ToString();
          this.textBoxRadioPower.Text = ((int) parameterByName[key].GetByteValue() - 18).ToString();
          this.buttonCalibrateRadioPower.Enabled = true;
        }
        else
          this.buttonCalibrateRadioPower.Enabled = false;
      }
      return true;
    }

    private void ResetStateInfo(string baseText)
    {
      this.baseStateText = baseText;
      this.loopReadOkCounter = 0;
      this.contLoopReadOkCounter = 0;
      this.loopReadErrCounter = 0;
      this.ShowStateInfo();
    }

    private void ShowStateInfo()
    {
      if (this.loopReadOkCounter == 0 && this.loopReadErrCounter == 0)
        this.textBoxState.Text = this.baseStateText;
      else
        this.textBoxState.Text = this.baseStateText + " (ok:" + this.loopReadOkCounter.ToString() + " err:" + this.loopReadErrCounter.ToString() + ")";
    }

    private void SetTempState(CalibrationWindow.CalibrationStateTemp calState)
    {
      this.buttonTempMeasureLow.Enabled = false;
      this.buttonTempMeasureMiddle.Enabled = false;
      this.buttonTempMeasureHightAndCalibrate.Enabled = false;
      switch (calState)
      {
        case CalibrationWindow.CalibrationStateTemp.TempLow:
          this.baseStateText = "Low temperature";
          this.groupBoxTemperatureCalibration.Enabled = true;
          this.textBoxBathTemperatureLow.BackColor = Color.White;
          this.textBoxBathTemperatureMiddle.BackColor = Color.White;
          this.textBoxBathTemperatureHight.BackColor = Color.White;
          this.textBoxBathTemperatureLow.Enabled = true;
          this.textBoxBathTemperatureMiddle.Enabled = false;
          this.textBoxBathTemperatureHight.Enabled = false;
          this.textBoxBathTemperatureLow.Clear();
          this.textBoxBathTemperatureMiddle.Clear();
          this.textBoxBathTemperatureHight.Clear();
          break;
        case CalibrationWindow.CalibrationStateTemp.TempMiddle:
          this.baseStateText = "Middle temperature";
          this.textBoxBathTemperatureLow.BackColor = Color.LightGreen;
          this.textBoxBathTemperatureLow.Enabled = false;
          this.textBoxBathTemperatureMiddle.Enabled = true;
          break;
        case CalibrationWindow.CalibrationStateTemp.TempHight:
          this.baseStateText = "High temperature";
          this.textBoxBathTemperatureMiddle.BackColor = Color.LightGreen;
          this.textBoxBathTemperatureMiddle.Enabled = false;
          this.textBoxBathTemperatureHight.Enabled = true;
          break;
        case CalibrationWindow.CalibrationStateTemp.Done:
          this.baseStateText = "Temperature done";
          this.textBoxBathTemperatureHight.BackColor = Color.LightGreen;
          this.textBoxBathTemperatureHight.Enabled = false;
          break;
      }
      this.calStateTemp = calState;
    }

    private void SetVolState(float volumeMeterTemp)
    {
      if (this.buttonIUF_HightTempZeroFlow.Visible)
      {
        switch (this.calStateVolume)
        {
          case TDC_Calibration.CalibrationStateVol.none:
            if (this.checkBoxDisableTempCheck.Checked || this.IsTempStableInRange(CalibrationWindow.volTempRanges[1, 0], CalibrationWindow.volTempRanges[1, 1]))
              this.buttonIUF_HightTempZeroFlow.Enabled = true;
            else
              this.buttonIUF_HightTempZeroFlow.Enabled = false;
            this.buttonVolumeCalibrate.Enabled = false;
            break;
          case TDC_Calibration.CalibrationStateVol.zeroFlowCalibrated:
            if (this.checkBoxDisableTempCheck.Checked || this.IsTempStableInRange(CalibrationWindow.volTempRanges[1, 0], CalibrationWindow.volTempRanges[1, 1]))
              this.buttonIUF_HightTempZeroFlow.Enabled = true;
            else
              this.buttonIUF_HightTempZeroFlow.Enabled = false;
            this.buttonVolumeCalibrate.Enabled = true;
            break;
        }
      }
      else
        this.buttonVolumeCalibrate.Enabled = true;
    }

    private bool IsTempStableInRange(float lowValue, float highValue)
    {
      return this.contLoopReadOkCounter >= 5 && (double) this.volumeMeterTemperature >= (double) lowValue && (double) this.volumeMeterTemperature <= (double) highValue && (double) this.deltaVolumeTempAbsMax <= 0.10000000149011612;
    }

    private void buttonSensorTemp_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      if (!this.timer1.Enabled)
      {
        this.timer1.Enabled = true;
        this.buttonSensorTemp.Text = "Break temperature reading";
        this.ResetStateInfo("Temperature reading started");
        this.MyFunctions.BreakRequest = false;
        this.ControlBox = false;
      }
      else
        this.DisableTimer();
      this.buttonTempMeasureLow.Enabled = false;
      this.buttonTempMeasureMiddle.Enabled = false;
      this.buttonTempMeasureHightAndCalibrate.Enabled = false;
      this.Enabled = true;
    }

    private void DisableTimer()
    {
      this.WaitTimerCommunicationStopps();
      this.MyFunctions.MyCommands.TestDone(272769346L);
      this.buttonSensorTemp.Text = "Run temperature reading";
      this.ResetStateInfo("Temperature reading stopped");
      this.MyFunctions.BreakRequest = true;
      this.buttonTempMeasureLow.Enabled = false;
      this.buttonTempMeasureMiddle.Enabled = false;
      this.buttonTempMeasureHightAndCalibrate.Enabled = false;
      this.buttonIUF_HightTempZeroFlow.Enabled = false;
      this.textBoxSensorTemperatureFlow.Clear();
      this.textBoxSensorTemperatureReturn.Clear();
      this.ControlBox = true;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      this.communicationRuns = true;
      if (!this.timer1.Enabled)
      {
        this.communicationRuns = false;
      }
      else
      {
        this.timer1.Enabled = false;
        float flowTemp;
        float returnTemp;
        if (this.MyFunctions.MyMeters.WorkMeter.MyParameters.GetRefreshedTemperatures(out flowTemp, out returnTemp))
        {
          this.baseStateText = "Temperature polling ok";
          ++this.loopReadOkCounter;
          ++this.contLoopReadOkCounter;
        }
        else
        {
          this.baseStateText = "Temperature polling error";
          ++this.loopReadErrCounter;
          this.contLoopReadOkCounter = 0;
        }
        this.ShowStateInfo();
        if (this.flowTempList.Count == 0)
        {
          for (int index = 0; index < 5; ++index)
          {
            this.flowTempList.Add(flowTemp);
            this.returnTempList.Add(returnTemp);
          }
        }
        else
        {
          this.flowTempList.RemoveAt(0);
          this.flowTempList.Add(flowTemp);
          this.returnTempList.RemoveAt(0);
          this.returnTempList.Add(returnTemp);
        }
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        for (int index = 0; index < 5; ++index)
        {
          float num5 = Math.Abs(this.flowTempList[index] - flowTemp);
          if ((double) num5 > (double) num1)
          {
            num1 = num5;
            num2 = flowTemp - this.flowTempList[index];
          }
          float num6 = Math.Abs(this.returnTempList[index] - returnTemp);
          if ((double) num6 > (double) num3)
          {
            num3 = num6;
            num4 = returnTemp - this.returnTempList[index];
          }
        }
        this.textBoxSensorTemperatureFlow.Text = flowTemp.ToString("000.00") + " / " + num2.ToString("000.00");
        this.textBoxSensorTemperatureReturn.Text = returnTemp.ToString("000.00") + " / " + num4.ToString("000.00");
        if (this.textBoxSensorTemperatureFlow.BackColor == Color.LightYellow)
        {
          this.volumeMeterTemperature = flowTemp;
          this.deltaVolumeTempAbsMax = num1;
        }
        else
        {
          this.volumeMeterTemperature = returnTemp;
          this.deltaVolumeTempAbsMax = num3;
        }
        if (this.contLoopReadOkCounter >= 5)
          this.textBoxTempValues.Text = "ok";
        else
          this.textBoxTempValues.Text = this.contLoopReadOkCounter.ToString();
        this.SetVolState(this.volumeMeterTemperature);
        if (this.contLoopReadOkCounter < 5 || (double) num1 > 0.10000000149011612 || (double) num3 > 0.10000000149011612)
        {
          this.buttonTempMeasureLow.Enabled = false;
          this.buttonTempMeasureMiddle.Enabled = false;
          this.buttonTempMeasureHightAndCalibrate.Enabled = false;
        }
        else if (this.calStateTemp == CalibrationWindow.CalibrationStateTemp.TempLow)
        {
          this.groupBoxVolumeCalibration.Enabled = true;
          this.flowTempLow = flowTemp;
          if (!float.TryParse(this.textBoxBathTemperatureLow.Text, out this.bathTempLow))
            this.bathTempLow = 0.0f;
          if ((double) this.bathTempLow < (double) CalibrationWindow.adcTempRanges[0, 0] || (double) this.bathTempLow > (double) CalibrationWindow.adcTempRanges[0, 1])
            this.bathTempLow = 0.0f;
          if ((double) Math.Abs(flowTemp - returnTemp) > 0.5)
            this.bathTempLow = 0.0f;
          if ((double) this.flowTempLow >= (double) CalibrationWindow.adcTempRanges[1, 0])
            this.bathTempLow = 0.0f;
          this.buttonTempMeasureLow.Enabled = (double) this.bathTempLow != 0.0;
        }
        else if (this.calStateTemp == CalibrationWindow.CalibrationStateTemp.TempMiddle)
        {
          this.groupBoxVolumeCalibration.Enabled = false;
          this.flowTempMiddle = flowTemp;
          if (!float.TryParse(this.textBoxBathTemperatureMiddle.Text, out this.bathTempMiddle))
            this.bathTempMiddle = 0.0f;
          if ((double) this.bathTempMiddle < (double) CalibrationWindow.adcTempRanges[1, 0] || (double) this.bathTempMiddle > (double) CalibrationWindow.adcTempRanges[1, 1])
            this.bathTempMiddle = 0.0f;
          if ((double) Math.Abs(flowTemp - returnTemp) > 0.5)
            this.bathTempMiddle = 0.0f;
          if ((double) flowTemp <= (double) this.flowTempLow + 20.0)
            this.bathTempMiddle = 0.0f;
          this.buttonTempMeasureMiddle.Enabled = (double) this.bathTempMiddle != 0.0;
        }
        else if (this.calStateTemp == CalibrationWindow.CalibrationStateTemp.TempHight)
        {
          this.groupBoxVolumeCalibration.Enabled = false;
          if (!float.TryParse(this.textBoxBathTemperatureHight.Text, out this.bathTempHight))
            this.bathTempHight = 0.0f;
          if ((double) this.bathTempHight < (double) CalibrationWindow.adcTempRanges[2, 0] || (double) this.bathTempHight > (double) CalibrationWindow.adcTempRanges[2, 1])
            this.bathTempHight = 0.0f;
          if ((double) Math.Abs(flowTemp - returnTemp) > 0.5)
            this.bathTempHight = 0.0f;
          if ((double) flowTemp <= (double) this.flowTempMiddle + 20.0)
            this.bathTempHight = 0.0f;
          this.buttonTempMeasureHightAndCalibrate.Enabled = (double) this.bathTempHight != 0.0;
        }
        else
          this.groupBoxVolumeCalibration.Enabled = true;
        this.communicationRuns = false;
        this.timer1.Enabled = true;
      }
    }

    private void buttonTempMeasureLow_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      this.groupBoxTemperatureCalibration.Enabled = false;
      this.WaitTimerCommunicationStopps();
      try
      {
        this.timer1.Enabled = false;
        if (this.MyFunctions.RunAdcCalibration(ADC_CalibrationSteps.StartCalibration, this.bathTempLow, this.bathTempLow, 5))
          this.SetTempState(CalibrationWindow.CalibrationStateTemp.TempMiddle);
        else
          this.baseStateText = "Low temp error";
      }
      catch
      {
        this.baseStateText = "Low temp error";
        this.ShowStateInfo();
      }
      finally
      {
        this.groupBoxTemperatureCalibration.Enabled = true;
        this.timer1.Enabled = true;
      }
      this.Enabled = true;
    }

    private void buttonTempMeasureMiddle_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      this.groupBoxTemperatureCalibration.Enabled = false;
      this.WaitTimerCommunicationStopps();
      try
      {
        if (this.MyFunctions.RunAdcCalibration(ADC_CalibrationSteps.SecondPoint, this.bathTempMiddle, this.bathTempMiddle, 5))
          this.SetTempState(CalibrationWindow.CalibrationStateTemp.TempHight);
        else
          this.baseStateText = "Middle temp error";
      }
      catch
      {
        this.baseStateText = "Middle temp error";
        this.ShowStateInfo();
      }
      this.groupBoxTemperatureCalibration.Enabled = true;
      this.timer1.Enabled = true;
      this.Enabled = true;
    }

    private void buttonTempMeasureHightAndCalibrate_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      this.groupBoxTemperatureCalibration.Enabled = false;
      this.WaitTimerCommunicationStopps();
      try
      {
        if (this.MyFunctions.RunAdcCalibration(ADC_CalibrationSteps.EndCalibration, this.bathTempHight, this.bathTempHight, 5))
          this.SetTempState(CalibrationWindow.CalibrationStateTemp.Done);
        else
          this.baseStateText = "High temp error";
      }
      catch
      {
        this.baseStateText = "Hight temp error";
        this.ShowStateInfo();
      }
      finally
      {
        this.groupBoxTemperatureCalibration.Enabled = true;
        this.timer1.Enabled = true;
      }
      this.Enabled = true;
    }

    private void textBoxBathTemperatureLow_TextChanged(object sender, EventArgs e)
    {
      this.textBoxBathTemperatureLow.BackColor = Color.White;
    }

    private void textBoxBathTemperatureMiddle_TextChanged(object sender, EventArgs e)
    {
      this.textBoxBathTemperatureMiddle.BackColor = Color.White;
    }

    private void textBoxBathTemperatureHight_TextChanged(object sender, EventArgs e)
    {
      this.textBoxBathTemperatureHight.BackColor = Color.White;
    }

    private void WaitTimerCommunicationStopps()
    {
      this.timer1.Enabled = false;
      DateTime dateTime = DateTime.Now.AddMilliseconds(1000.0);
      while (this.communicationRuns)
      {
        Application.DoEvents();
        if (!(DateTime.Now < dateTime))
          break;
      }
      Application.DoEvents();
      this.timer1.Enabled = false;
    }

    private void buttonIUF_ZeroFlowCalibration_Click(object sender, EventArgs e)
    {
      this.DisableTimer();
      this.Enabled = false;
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxState.Text = "Zero flow calibration high temperature";
      int numberOfLoops = 50;
      int mediaFilterRange = 3;
      TDC_Calibration.CalibrationInfo calibrationInfo;
      bool flag = this.tdcCalibration.CalibrateZeroFlow(numberOfLoops, mediaFilterRange, this.volumeMeterTemperature, out calibrationInfo);
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < numberOfLoops; ++index2)
          CalibrationWindow.TDC_ClaibrationLogger.Trace("CounterValuesUp[" + index1.ToString() + "," + index2.ToString() + "] = " + calibrationInfo.counterValuesUp[index1, index2].ToString());
      }
      for (int index3 = 0; index3 < 4; ++index3)
      {
        for (int index4 = 0; index4 < numberOfLoops; ++index4)
          CalibrationWindow.TDC_ClaibrationLogger.Trace("CounterValuesDown[" + index3.ToString() + "," + index4.ToString() + "] = " + calibrationInfo.counterValuesDown[index3, index4].ToString());
      }
      for (int index5 = 0; index5 < 4; ++index5)
      {
        for (int index6 = 0; index6 < numberOfLoops; ++index6)
          CalibrationWindow.TDC_ClaibrationLogger.Trace("diffValues[" + index5.ToString() + "," + index6.ToString() + "] = " + calibrationInfo.diffValues[index5, index6].ToString());
      }
      for (int index = 0; index < 4; ++index)
        CalibrationWindow.TDC_ClaibrationLogger.Trace("meandiffValues[" + index.ToString() + "] = " + calibrationInfo.meanDiffValues[index].ToString());
      if (flag)
        this.textBoxState.Text = "Done! Calibration=" + calibrationInfo.tdcZeroCalibrationValue.ToString() + "; Change=" + calibrationInfo.changeOfTdcZeroCalibrationValue.ToString();
      else
        this.textBoxState.Text = "calibration error";
      this.calStateVolume = this.tdcCalibration.GetCalibrationState();
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.Enabled = true;
    }

    private void buttonVolumeCalibrate_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      ZR_ClassLibMessages.ClearErrors();
      if (this.rBtnQp.Checked)
      {
        float result;
        if (!float.TryParse(this.txtBxQpErr.Text, out result))
          ZR_ClassLibMessages.AddErrorDescription("illegal error value");
        else
          this.MyFunctions.volumeGraphCalibration.AdjustVolumeFactor(result);
      }
      else if (this.rBtnQi.Checked)
      {
        float result1;
        float result2;
        float result3;
        if (float.TryParse(this.txtBxQiErr.Text, out result1) && float.TryParse(this.txtBxQi.Text, out result2) && float.TryParse(this.txtBxQ.Text, out result3))
          this.MyFunctions.volumeGraphCalibration.AdjustVolumeFactorQi(result2, result3, result1);
        else
          ZR_ClassLibMessages.AddErrorDescription("illegal error value");
      }
      else if (this.rBtnQiQQp.Checked)
      {
        float result4;
        float result5;
        if (float.TryParse(this.txtBxQi.Text, out result4) && float.TryParse(this.txtBxQiErr.Text, out result5))
        {
          float result6;
          float result7;
          if (float.TryParse(this.txtBxQ.Text, out result6) && float.TryParse(this.txtBxQErr.Text, out result7))
          {
            float result8;
            float result9;
            if (float.TryParse(this.txtBxQp.Text, out result8) && float.TryParse(this.txtBxQpErr.Text, out result9))
              this.MyFunctions.volumeGraphCalibration.AdjustVolumeCalibration(result4, result5, result6, result7, result8, result9);
            else
              ZR_ClassLibMessages.AddErrorDescription("illegal Qp value");
          }
          else
            ZR_ClassLibMessages.AddErrorDescription("illegal Q value");
        }
        else
          ZR_ClassLibMessages.AddErrorDescription("illegal Qi value");
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.Enabled = true;
    }

    private void ZeroCalibrationCheck_Click(object sender, EventArgs e)
    {
      int num = (int) new TDC_ZeroCalibrationCheck(this.MyFunctions).ShowDialog();
    }

    private void buttonCalibrateClockError_Click(object sender, EventArgs e)
    {
      try
      {
        double result;
        if (!double.TryParse(this.textBoxClockError.Text, out result))
        {
          int num = (int) GMM_MessageBox.ShowMessage("Clock calibration", "Illegal value");
          this.textBoxClockError.Text = "";
        }
        else
        {
          ZR_ClassLibMessages.ClearErrors();
          if (!this.MyFunctions.CalibrateClock(result))
          {
            ZR_ClassLibMessages.ShowAndClearErrors();
          }
          else
          {
            this.textBoxClockError.Text = "cal:" + result.ToString();
            this.buttonCalibrateClockError.Enabled = false;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Clock calibration", ex.ToString());
        this.textBoxClockError.Text = "";
      }
    }

    private void buttonCalibrateRadioFrequency_Click(object sender, EventArgs e)
    {
      try
      {
        double result;
        if (!double.TryParse(this.textBoxRadioFrequencyError.Text, out result))
        {
          int num = (int) GMM_MessageBox.ShowMessage("Frequency calibration", "Illegal value");
          this.textBoxRadioFrequencyError.Text = "";
        }
        else
        {
          ZR_ClassLibMessages.ClearErrors();
          if (!this.MyFunctions.CalibrateRadioFrequency(result))
          {
            ZR_ClassLibMessages.ShowAndClearErrors();
          }
          else
          {
            this.textBoxRadioFrequencyError.Text = "cal:" + result.ToString();
            this.textBoxRadioFrequencyError.Enabled = false;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Frequency calibration", ex.ToString());
        this.textBoxRadioFrequencyError.Text = "";
      }
    }

    private void buttonCalibrateRadioPower_Click(object sender, EventArgs e)
    {
      this.Enabled = false;
      ZR_ClassLibMessages.ClearErrors();
      int result;
      if (!int.TryParse(this.textBoxRadioPower.Text, out result) || result > 0 || result < -15)
        ZR_ClassLibMessages.AddErrorDescription("illegal radio power value");
      else
        this.MyFunctions.SetReducedRadioPowerBy_dBm(result);
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.Enabled = true;
    }

    private void rBtnQp_CheckedChanged(object sender, EventArgs e)
    {
      this.lblQp.Visible = false;
      this.txtBxQp.Visible = false;
      this.lblQpErr.Visible = true;
      this.txtBxQpErr.Visible = true;
      this.lblQi.Visible = false;
      this.txtBxQi.Visible = false;
      this.lblQiErr.Visible = false;
      this.txtBxQiErr.Visible = false;
      this.lblQ.Visible = false;
      this.txtBxQ.Visible = false;
      this.lblQErr.Visible = false;
      this.txtBxQErr.Visible = false;
    }

    private void rBtnQi_CheckedChanged(object sender, EventArgs e)
    {
      this.lblQp.Visible = false;
      this.txtBxQp.Visible = false;
      this.lblQpErr.Visible = false;
      this.txtBxQpErr.Visible = false;
      this.lblQi.Visible = true;
      this.txtBxQi.Visible = true;
      this.lblQiErr.Visible = true;
      this.txtBxQiErr.Visible = true;
      this.lblQ.Visible = true;
      this.txtBxQ.Visible = true;
      this.lblQErr.Visible = false;
      this.txtBxQErr.Visible = false;
    }

    private void rBtnQiQQp_CheckedChanged(object sender, EventArgs e)
    {
      this.lblQp.Visible = true;
      this.txtBxQp.Visible = true;
      this.lblQpErr.Visible = true;
      this.txtBxQpErr.Visible = true;
      this.lblQi.Visible = true;
      this.txtBxQi.Visible = true;
      this.lblQiErr.Visible = true;
      this.txtBxQiErr.Visible = true;
      this.lblQ.Visible = true;
      this.txtBxQ.Visible = true;
      this.lblQErr.Visible = true;
      this.txtBxQErr.Visible = true;
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
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBoxTemperatureCalibration = new GroupBox();
      this.buttonTempMeasureHightAndCalibrate = new Button();
      this.buttonTempMeasureMiddle = new Button();
      this.buttonTempMeasureLow = new Button();
      this.textBoxBathTemperatureHight = new TextBox();
      this.textBoxBathTemperatureMiddle = new TextBox();
      this.textBoxBathTemperatureLow = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.buttonSensorTemp = new Button();
      this.textBoxSensorTemperatureReturn = new TextBox();
      this.textBoxSensorTemperatureFlow = new TextBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.groupBoxVolumeCalibration = new GroupBox();
      this.lblQ = new Label();
      this.lblQi = new Label();
      this.txtBxQi = new TextBox();
      this.txtBxQ = new TextBox();
      this.lblQp = new Label();
      this.txtBxQp = new TextBox();
      this.lblQErr = new Label();
      this.lblQiErr = new Label();
      this.txtBxQiErr = new TextBox();
      this.txtBxQErr = new TextBox();
      this.rBtnQiQQp = new RadioButton();
      this.rBtnQi = new RadioButton();
      this.rBtnQp = new RadioButton();
      this.buttonZeroCalibrationCheck = new Button();
      this.buttonIUF_HightTempZeroFlow = new Button();
      this.buttonVolumeCalibrate = new Button();
      this.labelIUF_HightTempZeroFlow = new Label();
      this.lblQpErr = new Label();
      this.txtBxQpErr = new TextBox();
      this.buttonWrite = new Button();
      this.buttonRead = new Button();
      this.groupBox1 = new GroupBox();
      this.label5 = new Label();
      this.textBoxSerialNumber = new TextBox();
      this.timer1 = new Timer(this.components);
      this.textBoxState = new TextBox();
      this.groupBox2 = new GroupBox();
      this.checkBoxDisableTempCheck = new CheckBox();
      this.label8 = new Label();
      this.textBoxTempValues = new TextBox();
      this.groupBoxFrequency = new GroupBox();
      this.buttonCalibrateRadioPower = new Button();
      this.label11 = new Label();
      this.buttonCalibrateRadioFrequency = new Button();
      this.label10 = new Label();
      this.textBoxRadioPower = new TextBox();
      this.buttonCalibrateClockError = new Button();
      this.textBoxRadioFrequencyError = new TextBox();
      this.label9 = new Label();
      this.textBoxClockError = new TextBox();
      this.groupBoxTemperatureCalibration.SuspendLayout();
      this.groupBoxVolumeCalibration.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBoxFrequency.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(712, 41);
      this.zennerCoroprateDesign2.TabIndex = 17;
      this.groupBoxTemperatureCalibration.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.buttonTempMeasureHightAndCalibrate);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.buttonTempMeasureMiddle);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.buttonTempMeasureLow);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.textBoxBathTemperatureHight);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.textBoxBathTemperatureMiddle);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.textBoxBathTemperatureLow);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.label3);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.label2);
      this.groupBoxTemperatureCalibration.Controls.Add((Control) this.label1);
      this.groupBoxTemperatureCalibration.Location = new Point(13, 203);
      this.groupBoxTemperatureCalibration.Name = "groupBoxTemperatureCalibration";
      this.groupBoxTemperatureCalibration.Size = new Size(688, 136);
      this.groupBoxTemperatureCalibration.TabIndex = 18;
      this.groupBoxTemperatureCalibration.TabStop = false;
      this.groupBoxTemperatureCalibration.Text = "Temperature calibration";
      this.buttonTempMeasureHightAndCalibrate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTempMeasureHightAndCalibrate.Enabled = false;
      this.buttonTempMeasureHightAndCalibrate.Location = new Point(459, 92);
      this.buttonTempMeasureHightAndCalibrate.Name = "buttonTempMeasureHightAndCalibrate";
      this.buttonTempMeasureHightAndCalibrate.Size = new Size(194, 23);
      this.buttonTempMeasureHightAndCalibrate.TabIndex = 2;
      this.buttonTempMeasureHightAndCalibrate.Text = "Measure step 3 and calculate";
      this.buttonTempMeasureHightAndCalibrate.UseVisualStyleBackColor = true;
      this.buttonTempMeasureHightAndCalibrate.Click += new System.EventHandler(this.buttonTempMeasureHightAndCalibrate_Click);
      this.buttonTempMeasureMiddle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTempMeasureMiddle.Enabled = false;
      this.buttonTempMeasureMiddle.Location = new Point(459, 59);
      this.buttonTempMeasureMiddle.Name = "buttonTempMeasureMiddle";
      this.buttonTempMeasureMiddle.Size = new Size(194, 23);
      this.buttonTempMeasureMiddle.TabIndex = 2;
      this.buttonTempMeasureMiddle.Text = "Measure step 2";
      this.buttonTempMeasureMiddle.UseVisualStyleBackColor = true;
      this.buttonTempMeasureMiddle.Click += new System.EventHandler(this.buttonTempMeasureMiddle_Click);
      this.buttonTempMeasureLow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTempMeasureLow.Location = new Point(459, 25);
      this.buttonTempMeasureLow.Name = "buttonTempMeasureLow";
      this.buttonTempMeasureLow.Size = new Size(194, 23);
      this.buttonTempMeasureLow.TabIndex = 2;
      this.buttonTempMeasureLow.Text = "Measure step 1";
      this.buttonTempMeasureLow.UseVisualStyleBackColor = true;
      this.buttonTempMeasureLow.Click += new System.EventHandler(this.buttonTempMeasureLow_Click);
      this.textBoxBathTemperatureHight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxBathTemperatureHight.Location = new Point(353, 92);
      this.textBoxBathTemperatureHight.Name = "textBoxBathTemperatureHight";
      this.textBoxBathTemperatureHight.Size = new Size(100, 20);
      this.textBoxBathTemperatureHight.TabIndex = 1;
      this.textBoxBathTemperatureHight.TextChanged += new System.EventHandler(this.textBoxBathTemperatureHight_TextChanged);
      this.textBoxBathTemperatureMiddle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxBathTemperatureMiddle.Location = new Point(353, 59);
      this.textBoxBathTemperatureMiddle.Name = "textBoxBathTemperatureMiddle";
      this.textBoxBathTemperatureMiddle.Size = new Size(100, 20);
      this.textBoxBathTemperatureMiddle.TabIndex = 1;
      this.textBoxBathTemperatureMiddle.TextChanged += new System.EventHandler(this.textBoxBathTemperatureMiddle_TextChanged);
      this.textBoxBathTemperatureLow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxBathTemperatureLow.Location = new Point(353, 25);
      this.textBoxBathTemperatureLow.Name = "textBoxBathTemperatureLow";
      this.textBoxBathTemperatureLow.Size = new Size(100, 20);
      this.textBoxBathTemperatureLow.TabIndex = 1;
      this.textBoxBathTemperatureLow.TextChanged += new System.EventHandler(this.textBoxBathTemperatureLow_TextChanged);
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.Location = new Point(9, 92);
      this.label3.Name = "label3";
      this.label3.Size = new Size(338, 20);
      this.label3.TabIndex = 0;
      this.label3.Text = "Bath temperature hight [90..149°C]";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label2.Location = new Point(6, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(341, 20);
      this.label2.TabIndex = 0;
      this.label2.Text = "Bath temperature middle [45..80°C]";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(6, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(341, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Bath temperature low [1..20°C]";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.buttonSensorTemp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSensorTemp.Location = new Point(458, 21);
      this.buttonSensorTemp.Name = "buttonSensorTemp";
      this.buttonSensorTemp.Size = new Size(194, 23);
      this.buttonSensorTemp.TabIndex = 2;
      this.buttonSensorTemp.Text = "Run temperature reading";
      this.buttonSensorTemp.UseVisualStyleBackColor = true;
      this.buttonSensorTemp.Click += new System.EventHandler(this.buttonSensorTemp_Click);
      this.textBoxSensorTemperatureReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxSensorTemperatureReturn.Location = new Point(352, 47);
      this.textBoxSensorTemperatureReturn.Name = "textBoxSensorTemperatureReturn";
      this.textBoxSensorTemperatureReturn.ReadOnly = true;
      this.textBoxSensorTemperatureReturn.Size = new Size(100, 20);
      this.textBoxSensorTemperatureReturn.TabIndex = 1;
      this.textBoxSensorTemperatureFlow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxSensorTemperatureFlow.Location = new Point(352, 23);
      this.textBoxSensorTemperatureFlow.Name = "textBoxSensorTemperatureFlow";
      this.textBoxSensorTemperatureFlow.ReadOnly = true;
      this.textBoxSensorTemperatureFlow.Size = new Size(100, 20);
      this.textBoxSensorTemperatureFlow.TabIndex = 1;
      this.label7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label7.Location = new Point(9, 50);
      this.label7.Name = "label7";
      this.label7.RightToLeft = RightToLeft.No;
      this.label7.Size = new Size(337, 17);
      this.label7.TabIndex = 0;
      this.label7.Text = "[return / change °C]";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label6.Location = new Point(6, 23);
      this.label6.Name = "label6";
      this.label6.Size = new Size(340, 20);
      this.label6.TabIndex = 0;
      this.label6.Text = "Sensor temperature [flow / change °C]";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.groupBoxVolumeCalibration.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.lblQ);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.lblQi);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.txtBxQi);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.txtBxQ);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.lblQp);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.txtBxQp);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.lblQErr);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.lblQiErr);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.txtBxQiErr);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.txtBxQErr);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.rBtnQiQQp);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.rBtnQi);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.rBtnQp);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.buttonZeroCalibrationCheck);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.buttonIUF_HightTempZeroFlow);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.buttonVolumeCalibrate);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.labelIUF_HightTempZeroFlow);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.lblQpErr);
      this.groupBoxVolumeCalibration.Controls.Add((Control) this.txtBxQpErr);
      this.groupBoxVolumeCalibration.Location = new Point(12, 345);
      this.groupBoxVolumeCalibration.Name = "groupBoxVolumeCalibration";
      this.groupBoxVolumeCalibration.Size = new Size(688, 128);
      this.groupBoxVolumeCalibration.TabIndex = 18;
      this.groupBoxVolumeCalibration.TabStop = false;
      this.groupBoxVolumeCalibration.Text = "Volume calibration";
      this.lblQ.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblQ.Location = new Point(104, 71);
      this.lblQ.Name = "lblQ";
      this.lblQ.Size = new Size(62, 22);
      this.lblQ.TabIndex = 16;
      this.lblQ.Text = "Q [L/h]";
      this.lblQ.TextAlign = ContentAlignment.MiddleRight;
      this.lblQi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblQi.Location = new Point(104, 97);
      this.lblQi.Name = "lblQi";
      this.lblQi.Size = new Size(62, 22);
      this.lblQi.TabIndex = 15;
      this.lblQi.Text = "Qi [L/h]";
      this.lblQi.TextAlign = ContentAlignment.MiddleRight;
      this.txtBxQi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBxQi.Location = new Point(164, 98);
      this.txtBxQi.Name = "txtBxQi";
      this.txtBxQi.Size = new Size(100, 20);
      this.txtBxQi.TabIndex = 14;
      this.txtBxQ.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBxQ.Location = new Point(164, 72);
      this.txtBxQ.Name = "txtBxQ";
      this.txtBxQ.Size = new Size(100, 20);
      this.txtBxQ.TabIndex = 13;
      this.lblQp.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblQp.Location = new Point(101, 46);
      this.lblQp.Name = "lblQp";
      this.lblQp.Size = new Size(65, 22);
      this.lblQp.TabIndex = 11;
      this.lblQp.Text = "Qp [L/h]";
      this.lblQp.TextAlign = ContentAlignment.MiddleRight;
      this.txtBxQp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBxQp.Location = new Point(164, 47);
      this.txtBxQp.Name = "txtBxQp";
      this.txtBxQp.Size = new Size(100, 20);
      this.txtBxQp.TabIndex = 12;
      this.lblQErr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblQErr.Location = new Point(273, 71);
      this.lblQErr.Name = "lblQErr";
      this.lblQErr.Size = new Size(75, 22);
      this.lblQErr.TabIndex = 10;
      this.lblQErr.Text = "Q error [%]";
      this.lblQErr.TextAlign = ContentAlignment.MiddleRight;
      this.lblQiErr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblQiErr.Location = new Point(273, 97);
      this.lblQiErr.Name = "lblQiErr";
      this.lblQiErr.Size = new Size(75, 22);
      this.lblQiErr.TabIndex = 9;
      this.lblQiErr.Text = "Qi error [%]";
      this.lblQiErr.TextAlign = ContentAlignment.MiddleRight;
      this.txtBxQiErr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBxQiErr.Location = new Point(351, 98);
      this.txtBxQiErr.Name = "txtBxQiErr";
      this.txtBxQiErr.Size = new Size(100, 20);
      this.txtBxQiErr.TabIndex = 8;
      this.txtBxQErr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBxQErr.Location = new Point(351, 72);
      this.txtBxQErr.Name = "txtBxQErr";
      this.txtBxQErr.Size = new Size(100, 20);
      this.txtBxQErr.TabIndex = 7;
      this.rBtnQiQQp.AutoSize = true;
      this.rBtnQiQQp.Checked = true;
      this.rBtnQiQQp.Location = new Point(10, 100);
      this.rBtnQiQQp.Name = "rBtnQiQQp";
      this.rBtnQiQQp.Size = new Size(92, 17);
      this.rBtnQiQQp.TabIndex = 6;
      this.rBtnQiQQp.TabStop = true;
      this.rBtnQiQQp.Text = "Calib: Qp,Q,Qi";
      this.rBtnQiQQp.UseVisualStyleBackColor = true;
      this.rBtnQiQQp.CheckedChanged += new System.EventHandler(this.rBtnQiQQp_CheckedChanged);
      this.rBtnQi.AutoSize = true;
      this.rBtnQi.Location = new Point(10, 74);
      this.rBtnQi.Name = "rBtnQi";
      this.rBtnQi.Size = new Size(64, 17);
      this.rBtnQi.TabIndex = 5;
      this.rBtnQi.TabStop = true;
      this.rBtnQi.Text = "Calib: Qi";
      this.rBtnQi.UseVisualStyleBackColor = true;
      this.rBtnQi.CheckedChanged += new System.EventHandler(this.rBtnQi_CheckedChanged);
      this.rBtnQp.AutoSize = true;
      this.rBtnQp.Location = new Point(10, 49);
      this.rBtnQp.Name = "rBtnQp";
      this.rBtnQp.Size = new Size(68, 17);
      this.rBtnQp.TabIndex = 4;
      this.rBtnQp.TabStop = true;
      this.rBtnQp.Text = "Calib: Qp";
      this.rBtnQp.UseVisualStyleBackColor = true;
      this.rBtnQp.CheckedChanged += new System.EventHandler(this.rBtnQp_CheckedChanged);
      this.buttonZeroCalibrationCheck.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonZeroCalibrationCheck.Location = new Point(10, 19);
      this.buttonZeroCalibrationCheck.Name = "buttonZeroCalibrationCheck";
      this.buttonZeroCalibrationCheck.Size = new Size(235, 23);
      this.buttonZeroCalibrationCheck.TabIndex = 3;
      this.buttonZeroCalibrationCheck.Text = "Zero calibration check";
      this.buttonZeroCalibrationCheck.UseVisualStyleBackColor = true;
      this.buttonZeroCalibrationCheck.Visible = false;
      this.buttonZeroCalibrationCheck.Click += new System.EventHandler(this.ZeroCalibrationCheck_Click);
      this.buttonIUF_HightTempZeroFlow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonIUF_HightTempZeroFlow.Location = new Point(459, 19);
      this.buttonIUF_HightTempZeroFlow.Name = "buttonIUF_HightTempZeroFlow";
      this.buttonIUF_HightTempZeroFlow.Size = new Size(194, 23);
      this.buttonIUF_HightTempZeroFlow.TabIndex = 2;
      this.buttonIUF_HightTempZeroFlow.Text = "Start zero flow calibration";
      this.buttonIUF_HightTempZeroFlow.UseVisualStyleBackColor = true;
      this.buttonIUF_HightTempZeroFlow.Visible = false;
      this.buttonIUF_HightTempZeroFlow.Click += new System.EventHandler(this.buttonIUF_ZeroFlowCalibration_Click);
      this.buttonVolumeCalibrate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonVolumeCalibrate.Location = new Point(459, 46);
      this.buttonVolumeCalibrate.Name = "buttonVolumeCalibrate";
      this.buttonVolumeCalibrate.Size = new Size(194, 72);
      this.buttonVolumeCalibrate.TabIndex = 2;
      this.buttonVolumeCalibrate.Text = "Calibrate";
      this.buttonVolumeCalibrate.UseVisualStyleBackColor = true;
      this.buttonVolumeCalibrate.Click += new System.EventHandler(this.buttonVolumeCalibrate_Click);
      this.labelIUF_HightTempZeroFlow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.labelIUF_HightTempZeroFlow.Location = new Point(7, 19);
      this.labelIUF_HightTempZeroFlow.Name = "labelIUF_HightTempZeroFlow";
      this.labelIUF_HightTempZeroFlow.Size = new Size(444, 23);
      this.labelIUF_HightTempZeroFlow.TabIndex = 0;
      this.labelIUF_HightTempZeroFlow.Text = "Calibrate IUF zero flow [40..60 °C]";
      this.labelIUF_HightTempZeroFlow.TextAlign = ContentAlignment.MiddleRight;
      this.labelIUF_HightTempZeroFlow.Visible = false;
      this.lblQpErr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblQpErr.Location = new Point(270, 46);
      this.lblQpErr.Name = "lblQpErr";
      this.lblQpErr.Size = new Size(78, 22);
      this.lblQpErr.TabIndex = 0;
      this.lblQpErr.Text = "Qp error [%]";
      this.lblQpErr.TextAlign = ContentAlignment.MiddleRight;
      this.txtBxQpErr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBxQpErr.Location = new Point(351, 47);
      this.txtBxQpErr.Name = "txtBxQpErr";
      this.txtBxQpErr.Size = new Size(100, 20);
      this.txtBxQpErr.TabIndex = 1;
      this.buttonWrite.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonWrite.Location = new Point(580, 594);
      this.buttonWrite.Name = "buttonWrite";
      this.buttonWrite.Size = new Size(120, 23);
      this.buttonWrite.TabIndex = 19;
      this.buttonWrite.Text = "Write";
      this.buttonWrite.UseVisualStyleBackColor = true;
      this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
      this.buttonRead.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRead.Location = new Point(454, 594);
      this.buttonRead.Name = "buttonRead";
      this.buttonRead.Size = new Size(120, 23);
      this.buttonRead.TabIndex = 19;
      this.buttonRead.Text = "Read";
      this.buttonRead.UseVisualStyleBackColor = true;
      this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.textBoxSerialNumber);
      this.groupBox1.Location = new Point(12, 47);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(688, 50);
      this.groupBox1.TabIndex = 20;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Device identification";
      this.label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label5.Location = new Point(13, 19);
      this.label5.Name = "label5";
      this.label5.Size = new Size(440, 20);
      this.label5.TabIndex = 0;
      this.label5.Text = "Serial number";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.textBoxSerialNumber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxSerialNumber.Location = new Point(459, 19);
      this.textBoxSerialNumber.Name = "textBoxSerialNumber";
      this.textBoxSerialNumber.ReadOnly = true;
      this.textBoxSerialNumber.Size = new Size(194, 20);
      this.textBoxSerialNumber.TabIndex = 1;
      this.timer1.Interval = 2000;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      this.textBoxState.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxState.Location = new Point(13, 596);
      this.textBoxState.Name = "textBoxState";
      this.textBoxState.ReadOnly = true;
      this.textBoxState.Size = new Size(418, 20);
      this.textBoxState.TabIndex = 21;
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.checkBoxDisableTempCheck);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Controls.Add((Control) this.label8);
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.buttonSensorTemp);
      this.groupBox2.Controls.Add((Control) this.textBoxSensorTemperatureFlow);
      this.groupBox2.Controls.Add((Control) this.textBoxTempValues);
      this.groupBox2.Controls.Add((Control) this.textBoxSensorTemperatureReturn);
      this.groupBox2.Location = new Point(13, 103);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(687, 94);
      this.groupBox2.TabIndex = 22;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Temperature monitor";
      this.checkBoxDisableTempCheck.AutoSize = true;
      this.checkBoxDisableTempCheck.Location = new Point(333, 68);
      this.checkBoxDisableTempCheck.Name = "checkBoxDisableTempCheck";
      this.checkBoxDisableTempCheck.Size = new Size(218, 17);
      this.checkBoxDisableTempCheck.TabIndex = 3;
      this.checkBoxDisableTempCheck.Text = "Disable temperature check for developer";
      this.checkBoxDisableTempCheck.UseVisualStyleBackColor = true;
      this.checkBoxDisableTempCheck.Visible = false;
      this.label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label8.Location = new Point(458, 47);
      this.label8.Name = "label8";
      this.label8.Size = new Size(131, 20);
      this.label8.TabIndex = 0;
      this.label8.Text = "values";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.textBoxTempValues.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxTempValues.Location = new Point(595, 47);
      this.textBoxTempValues.Name = "textBoxTempValues";
      this.textBoxTempValues.ReadOnly = true;
      this.textBoxTempValues.Size = new Size(57, 20);
      this.textBoxTempValues.TabIndex = 1;
      this.groupBoxFrequency.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxFrequency.Controls.Add((Control) this.buttonCalibrateRadioPower);
      this.groupBoxFrequency.Controls.Add((Control) this.label11);
      this.groupBoxFrequency.Controls.Add((Control) this.buttonCalibrateRadioFrequency);
      this.groupBoxFrequency.Controls.Add((Control) this.label10);
      this.groupBoxFrequency.Controls.Add((Control) this.textBoxRadioPower);
      this.groupBoxFrequency.Controls.Add((Control) this.buttonCalibrateClockError);
      this.groupBoxFrequency.Controls.Add((Control) this.textBoxRadioFrequencyError);
      this.groupBoxFrequency.Controls.Add((Control) this.label9);
      this.groupBoxFrequency.Controls.Add((Control) this.textBoxClockError);
      this.groupBoxFrequency.Location = new Point(12, 479);
      this.groupBoxFrequency.Name = "groupBoxFrequency";
      this.groupBoxFrequency.Size = new Size(688, 108);
      this.groupBoxFrequency.TabIndex = 23;
      this.groupBoxFrequency.TabStop = false;
      this.groupBoxFrequency.Text = "Additional calibrations";
      this.buttonCalibrateRadioPower.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCalibrateRadioPower.Location = new Point(462, 77);
      this.buttonCalibrateRadioPower.Name = "buttonCalibrateRadioPower";
      this.buttonCalibrateRadioPower.Size = new Size(194, 23);
      this.buttonCalibrateRadioPower.TabIndex = 2;
      this.buttonCalibrateRadioPower.Text = "Calibrate";
      this.buttonCalibrateRadioPower.UseVisualStyleBackColor = true;
      this.buttonCalibrateRadioPower.Click += new System.EventHandler(this.buttonCalibrateRadioPower_Click);
      this.label11.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label11.Location = new Point(9, 77);
      this.label11.Name = "label11";
      this.label11.Size = new Size(341, 20);
      this.label11.TabIndex = 0;
      this.label11.Text = "Radio power reduction [dBm] 0..15";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.buttonCalibrateRadioFrequency.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCalibrateRadioFrequency.Location = new Point(462, 48);
      this.buttonCalibrateRadioFrequency.Name = "buttonCalibrateRadioFrequency";
      this.buttonCalibrateRadioFrequency.Size = new Size(194, 23);
      this.buttonCalibrateRadioFrequency.TabIndex = 2;
      this.buttonCalibrateRadioFrequency.Text = "Calibrate";
      this.buttonCalibrateRadioFrequency.UseVisualStyleBackColor = true;
      this.buttonCalibrateRadioFrequency.Click += new System.EventHandler(this.buttonCalibrateRadioFrequency_Click);
      this.label10.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label10.Location = new Point(9, 48);
      this.label10.Name = "label10";
      this.label10.Size = new Size(341, 20);
      this.label10.TabIndex = 0;
      this.label10.Text = "Radio frequency error [Hz]";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.textBoxRadioPower.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxRadioPower.Location = new Point(356, 77);
      this.textBoxRadioPower.Name = "textBoxRadioPower";
      this.textBoxRadioPower.Size = new Size(100, 20);
      this.textBoxRadioPower.TabIndex = 1;
      this.textBoxRadioPower.TextChanged += new System.EventHandler(this.textBoxBathTemperatureLow_TextChanged);
      this.buttonCalibrateClockError.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCalibrateClockError.Location = new Point(462, 19);
      this.buttonCalibrateClockError.Name = "buttonCalibrateClockError";
      this.buttonCalibrateClockError.Size = new Size(194, 23);
      this.buttonCalibrateClockError.TabIndex = 2;
      this.buttonCalibrateClockError.Text = "Calibrate";
      this.buttonCalibrateClockError.UseVisualStyleBackColor = true;
      this.buttonCalibrateClockError.Click += new System.EventHandler(this.buttonCalibrateClockError_Click);
      this.textBoxRadioFrequencyError.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxRadioFrequencyError.Location = new Point(356, 48);
      this.textBoxRadioFrequencyError.Name = "textBoxRadioFrequencyError";
      this.textBoxRadioFrequencyError.Size = new Size(100, 20);
      this.textBoxRadioFrequencyError.TabIndex = 1;
      this.textBoxRadioFrequencyError.TextChanged += new System.EventHandler(this.textBoxBathTemperatureLow_TextChanged);
      this.label9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label9.Location = new Point(9, 19);
      this.label9.Name = "label9";
      this.label9.Size = new Size(341, 20);
      this.label9.TabIndex = 0;
      this.label9.Text = "Clock error [ppm]";
      this.label9.TextAlign = ContentAlignment.MiddleRight;
      this.textBoxClockError.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxClockError.Location = new Point(356, 19);
      this.textBoxClockError.Name = "textBoxClockError";
      this.textBoxClockError.Size = new Size(100, 20);
      this.textBoxClockError.TabIndex = 1;
      this.textBoxClockError.TextChanged += new System.EventHandler(this.textBoxBathTemperatureLow_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(712, 629);
      this.Controls.Add((Control) this.groupBoxFrequency);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.textBoxState);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.buttonRead);
      this.Controls.Add((Control) this.buttonWrite);
      this.Controls.Add((Control) this.groupBoxVolumeCalibration);
      this.Controls.Add((Control) this.groupBoxTemperatureCalibration);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Name = nameof (CalibrationWindow);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calibration Window";
      this.groupBoxTemperatureCalibration.ResumeLayout(false);
      this.groupBoxTemperatureCalibration.PerformLayout();
      this.groupBoxVolumeCalibration.ResumeLayout(false);
      this.groupBoxVolumeCalibration.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBoxFrequency.ResumeLayout(false);
      this.groupBoxFrequency.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum CalibrationStateTemp
    {
      TempLow,
      TempMiddle,
      TempHight,
      Done,
    }
  }
}
