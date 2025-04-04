// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MinomatV4
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class MinomatV4 : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (MinomatV4));
    private SCGiConnection connection;
    private readonly DateTime TIMEPOINT0 = new DateTime(2001, 1, 1);
    private MeasurementSet measurementSet;
    private List<byte> notHandledMeasurementData;

    public MinomatV4()
      : this(new SCGiConnection())
    {
    }

    public MinomatV4(SCGiConnection connection)
      : this(connection, "01AA02AA03AA")
    {
    }

    public MinomatV4(SCGiConnection connection, string psw)
    {
      this.connection = connection;
      this.Authentication = new SCGiHeaderEx(true, SCGiSequenceHeaderType.Authentication, Util.HexStringToByteArray(psw));
      this.MaxAttempt = 3;
      this.notHandledMeasurementData = new List<byte>();
      this.CancelCurrentMethod = false;
    }

    public SCGiConnection Connection => this.connection;

    public int MaxAttempt { get; set; }

    public bool CancelCurrentMethod { get; set; }

    public SCGiHeaderEx Authentication { get; set; }

    public event EventHandler<MinomatV4Parameter> OnMinomatV4ParameterReceived;

    public event EventHandler<MeasurementData> OnMeasurementDataReceived;

    public event EventHandler<MinomatV4.StateEventArgs> OnMessage;

    public event EventHandlerEx<Exception> OnError;

    public void Dispose()
    {
      MinomatV4.logger.Debug("Dispose MinomatV4 handler");
      if (this.connection == null)
        return;
      this.connection.Dispose();
    }

    private SCGiPacket SendAndReceiveOnlyOnePacket(
      SCGiHeader header,
      byte[] payload,
      string callerInfo)
    {
      return this.SendAndReceiveOnlyOnePacket(header, new List<byte>((IEnumerable<byte>) payload), callerInfo);
    }

    private SCGiPacket SendAndReceiveOnlyOnePacket(
      SCGiHeader header,
      List<byte> payload,
      string callerInfo)
    {
      SCGiFrame frame = this.SendAndReceiveFrame(header, payload, callerInfo);
      if (frame == null)
        return (SCGiPacket) null;
      if (frame.Count != 1)
      {
        string message = string.Format("Wrong count of responce SCGi packets! Actual: {0}, Expected: 1", (object) frame.Count);
        MinomatV4.logger.Error(message);
        throw new Exception(message);
      }
      return frame[0];
    }

    private SCGiFrame SendAndReceiveFrame(SCGiHeader header, List<byte> payload, string callerInfo)
    {
      header.SourceAddress = this.connection.SourceAddress;
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV4))
        throw new AccessDeniedException("No permission for MinomatV4!");
      if (header == null)
        throw new ArgumentNullException("SCGi header can not be null!");
      if (payload == null)
        throw new ArgumentNullException("SCGi payload can not be null!");
      if (!this.connection.Open())
        return (SCGiFrame) null;
      SCGiPacket packet = new SCGiPacket(header, payload.ToArray());
      int maxAttempt = this.MaxAttempt;
      SCGiFrame frame = (SCGiFrame) null;
      Exception innerException = (Exception) null;
      do
      {
        --maxAttempt;
        if (this.CancelCurrentMethod)
        {
          string message = "User has canceled execution of method.";
          MinomatV4.logger.Info(message);
          if (this.OnMessage != null)
            this.OnMessage((object) this, new MinomatV4.StateEventArgs()
            {
              Message = message,
              ProgressValue = 0
            });
          return (SCGiFrame) null;
        }
        ZR_ClassLibMessages.ClearErrors();
        if (!this.connection.Write(packet))
          return (SCGiFrame) null;
        ZR_ClassLibMessages.ClearErrors();
        try
        {
          frame = header.MessageClass != SCGiMessageType.SCGI_1_9 ? this.connection.ReadSCGi2_0() : this.connection.ReadSCGi1_9();
        }
        catch (SCGiError ex)
        {
          if (this.OnError != null)
            this.OnError((object) this, new Exception(string.Format("{0}, Attempt: {1} {2}", (object) callerInfo, (object) (this.MaxAttempt - maxAttempt), (object) ex.Message), (Exception) ex));
          if (ex.Type == SCGiErrorType.Communication || ex.Type == SCGiErrorType.InvalidResponce || ex.Type == SCGiErrorType.InvalidSCGiPacket || ex.Type == SCGiErrorType.UnknownErrorOccured || ex.Type == SCGiErrorType.UnknownMessageType || ex.Type == SCGiErrorType.UnknownResponce || ex.Type == SCGiErrorType.WrongCRC || ex.Type == SCGiErrorType.ParseError)
          {
            innerException = (Exception) ex;
            MinomatV4.logger.Error<int, string>(callerInfo + " failed! Attempt: {0} {1}", this.MaxAttempt - maxAttempt, ex.Message);
            if (this.OnMessage != null)
              this.OnMessage((object) this, new MinomatV4.StateEventArgs()
              {
                Message = "Error occurred! Attempt: " + (this.MaxAttempt - maxAttempt).ToString()
              });
            ++packet.Header.Sequencenumber;
          }
          else
          {
            MinomatV4.logger.Error(callerInfo + " Error occurred! Break off. Reason: " + ex.Message);
            throw ex;
          }
        }
        catch (Exception ex)
        {
          if (this.OnError != null)
            this.OnError((object) this, new Exception(string.Format("{0}, Attempt: {1} {2}", (object) callerInfo, (object) (this.MaxAttempt - maxAttempt), (object) ex.Message), ex));
          innerException = ex;
          MinomatV4.logger.Error<int, string>(callerInfo + " failed! Attempt: {0} {1}", this.MaxAttempt - maxAttempt, ZR_ClassLibMessages.GetLastErrorInfo().LastErrorDescription);
          if (this.OnMessage != null)
            this.OnMessage((object) this, new MinomatV4.StateEventArgs()
            {
              Message = "Error occurred! Attempt: " + (this.MaxAttempt - maxAttempt).ToString()
            });
        }
      }
      while (frame == null && maxAttempt >= 1);
      if (frame == null || frame.Count == 0)
      {
        if (this.OnError != null && innerException != null)
          this.OnError((object) this, new Exception(callerInfo + " failed! " + innerException.Message, innerException));
        return (SCGiFrame) null;
      }
      this.CheckResponse(frame[0]);
      return frame;
    }

    private bool SendPacket(SCGiHeader header, List<byte> payload)
    {
      if (header == null)
        throw new ArgumentNullException("SCGi header can not be null!");
      if (payload == null)
        throw new ArgumentNullException("SCGi payload can not be null!");
      if (!this.connection.Open())
        return false;
      SCGiPacket packet = new SCGiPacket(header, payload.ToArray());
      ZR_ClassLibMessages.ClearErrors();
      return this.connection.Write(packet);
    }

    private void CheckResponse(SCGiPacket firstPacket)
    {
      if (firstPacket == null)
        throw new ArgumentNullException("Parameter 'firstPacket' can not be null!");
      if (!firstPacket.IsFirstPacketOfFrame || firstPacket.Header.MessageClass == SCGiMessageType.SCGI_1_9 || firstPacket.Header.MessageClass == SCGiMessageType.GSM)
        return;
      if (firstPacket.Payload.Length >= 2)
      {
        if (firstPacket.Payload[0] == byte.MaxValue && firstPacket.Payload[1] == (byte) 254)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.AuthenticationFailed, "Authentication failed! Buffer: " + Util.ByteArrayToHexString(firstPacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        if (firstPacket.Payload[0] == byte.MaxValue && firstPacket.Payload[1] == (byte) 252)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownMessageType, "Unknown message type! Buffer: " + Util.ByteArrayToHexString(firstPacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        if (firstPacket.Payload[0] == byte.MaxValue && firstPacket.Payload[1] == byte.MaxValue)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.SystemTimeIsNotConfigured, "System time is not configured! Buffer: " + Util.ByteArrayToHexString(firstPacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        if (firstPacket.Payload[0] == byte.MaxValue && firstPacket.Payload[1] == (byte) 0)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.AuthenticationFailed, "Authentication failed! Buffer: " + Util.ByteArrayToHexString(firstPacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        if (firstPacket.Payload[0] == (byte) 241 && firstPacket.Payload[1] == (byte) 0)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownMessageType, "Unknown SCGi message class! Buffer: " + Util.ByteArrayToHexString(firstPacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
      }
      else
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, "Wrong command ID of SCGi responce! Buffer: " + Util.ByteArrayToHexString(firstPacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
    }

    private static List<byte> CreatePayload(SCGiCommand cmd)
    {
      return MinomatV4.CreatePayload(cmd, (byte[]) null);
    }

    private List<byte> CreatePayload(SCGiCommand cmd, short content)
    {
      return MinomatV4.CreatePayload(cmd, BitConverter.GetBytes(content));
    }

    private List<byte> CreatePayload(SCGiCommand cmd, uint content)
    {
      return MinomatV4.CreatePayload(cmd, BitConverter.GetBytes(content));
    }

    private static List<byte> CreatePayload(SCGiCommand cmd, byte[] content)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV4))
        throw new Exception("No permission for MinomatV4!");
      List<byte> payload = new List<byte>((IEnumerable<byte>) SCGiCommandManager.GetBytes(cmd));
      if (content != null)
        payload.AddRange((IEnumerable<byte>) content);
      return payload;
    }

    public MeasurementSet GetMeasurementData(
      string id,
      string type,
      string start,
      string end,
      bool useBulkQuery)
    {
      if (string.IsNullOrEmpty(type) || !Enum.IsDefined(typeof (MeasurementDataType), (object) type))
        throw new ArgumentException("Invalid measurement data type of the mess unit!");
      DateTime start1;
      if (!Util.TryParseToDateTime(start, out start1))
        throw new ArgumentException("Invalid start date!");
      DateTime end1;
      if (!Util.TryParseToDateTime(end, out end1))
        throw new ArgumentException("Invalid end date!");
      MeasurementDataType type1 = (MeasurementDataType) Enum.Parse(typeof (MeasurementDataType), type, true);
      uint maxValue;
      if (string.IsNullOrEmpty(id))
      {
        maxValue = uint.MaxValue;
      }
      else
      {
        if (id.Contains(","))
        {
          List<uint> ids = new List<uint>();
          string[] strArray = id.Split(',');
          if (strArray == null)
            throw new ArgumentException("Invalid list of mess unit id's!");
          foreach (string str in strArray)
          {
            string strValue = str.Trim();
            if (!string.IsNullOrEmpty(strValue))
            {
              uint num;
              if (!Util.TryParseToUInt32(strValue, out num))
                throw new ArgumentException("Invalid list of mess unit id's!");
              ids.Add(num);
            }
          }
          return this.GetMeasurementData(ids, type1, start1, end1, useBulkQuery);
        }
        if (!Util.TryParseToUInt32(id, out maxValue))
          throw new ArgumentException("Invalid id of mess unit!");
      }
      return this.GetMeasurementData(maxValue, type1, start1, end1, useBulkQuery);
    }

    public MeasurementSet GetMeasurementData(
      MeasurementDataType type,
      DateTime start,
      DateTime end,
      bool useBulkQuery)
    {
      return this.GetMeasurementData(uint.MaxValue, type, start, end, useBulkQuery);
    }

    public MeasurementSet GetMeasurementData(
      uint id,
      MeasurementDataType type,
      DateTime start,
      DateTime end,
      bool useBulkQuery)
    {
      return this.GetMeasurementData(new List<uint>() { id }, type, start, end, useBulkQuery);
    }

    public MeasurementSet GetMeasurementData(
      List<uint> ids,
      MeasurementDataType type,
      DateTime start,
      DateTime end,
      bool useBulkQuery)
    {
      if (ids == null)
        throw new ArgumentNullException("Input parameter 'ids' can not be null!");
      if (ids.Count == 0)
        ids.Add(uint.MaxValue);
      if (type != MeasurementDataType.Quarter)
      {
        start = new DateTime(start.Year, start.Month, start.Day);
        end = new DateTime(end.Year, end.Month, end.Day);
      }
      DateTime? nullable = new DateTime?();
      SortedList<DateTime, DateTime> sortedList = (SortedList<DateTime, DateTime>) null;
      switch (type)
      {
        case MeasurementDataType.DueDate:
        case MeasurementDataType.MonthAndHalfMonth:
          sortedList = this.SpitTime(start, end, 547);
          break;
        case MeasurementDataType.Day:
        case MeasurementDataType.Quarter:
        case MeasurementDataType.Timediff:
          sortedList = this.SpitTime(start, end, 31);
          break;
      }
      MeasurementSet measurementData1 = new MeasurementSet();
      if (ids[0] == uint.MaxValue && !useBulkQuery)
      {
        string message = "Read list of registered mess units.";
        MinomatV4.logger.Info(message);
        if (this.OnMessage != null)
          this.OnMessage((object) this, new MinomatV4.StateEventArgs()
          {
            Message = message
          });
        ids = this.GetRegisteredMessUnits();
        if (ids == null)
          return measurementData1;
      }
      if (this.OnMessage != null)
        this.OnMessage((object) this, new MinomatV4.StateEventArgs()
        {
          Message = "Read measurement values..."
        });
      int num = 1;
      DateTime dateTimeNow = SystemValues.DateTimeNow;
      MinomatV4.StateEventArgs e = new MinomatV4.StateEventArgs();
      foreach (uint id in ids)
      {
        if (this.CancelCurrentMethod)
        {
          string message = "User has canceled execution of method.";
          MinomatV4.logger.Info(message);
          if (this.OnMessage != null)
            this.OnMessage((object) this, new MinomatV4.StateEventArgs()
            {
              Message = message,
              ProgressValue = 0
            });
          return measurementData1;
        }
        if (num > 1 && ids.Count > 1)
        {
          double totalMinutes = (SystemValues.DateTimeNow - dateTimeNow).TotalMinutes;
          string message = string.Format("Read {0} values {1} of {2}. Remained {3} minutes", (object) type, (object) num, (object) ids.Count, (object) Math.Round(totalMinutes / (double) num * (double) (ids.Count - num), 1));
          MinomatV4.logger.Info(message);
          if (this.OnMessage != null)
          {
            int int32 = Convert.ToInt32((double) num / (double) ids.Count * 100.0);
            e.ProgressValue = int32;
            e.Message = message;
            this.OnMessage((object) this, e);
          }
        }
        ++num;
        for (int index = sortedList.Count - 1; index >= 0; --index)
        {
          if (this.CancelCurrentMethod)
          {
            string message = "User has canceled execution of method.";
            MinomatV4.logger.Info(message);
            if (this.OnMessage != null)
              this.OnMessage((object) this, new MinomatV4.StateEventArgs()
              {
                Message = message
              });
            return measurementData1;
          }
          DateTime key = sortedList.Keys[index];
          DateTime end1 = sortedList.Values[index];
          if (!nullable.HasValue || !(nullable.Value == end1))
          {
            try
            {
              MeasurementSet measurementData2 = this.GetMeasurementData(id, type, key, end1);
              if (measurementData2 != null && measurementData2.Count > 0)
                measurementData1.Add(measurementData2);
            }
            catch (SCGiError ex)
            {
              if (ex.Type != SCGiErrorType.Communication)
              {
                if (ex.Type != SCGiErrorType.InvalidTimeInterval)
                  throw ex;
                nullable = new DateTime?(end1);
                break;
              }
              break;
            }
          }
          else
            break;
        }
      }
      return measurementData1;
    }

    private SortedList<DateTime, DateTime> SpitTime(DateTime start, DateTime end, int days)
    {
      SortedList<DateTime, DateTime> sortedList = new SortedList<DateTime, DateTime>();
      DateTime key = start;
      DateTime dateTime = start.AddDays((double) days);
      DateTime now = DateTime.Now;
      while (dateTime < end)
      {
        sortedList.Add(key, dateTime);
        key = dateTime.AddDays(1.0);
        dateTime = key.AddDays((double) days);
        if (!(key < end) || !(key < now))
          goto label_4;
      }
      sortedList.Add(key, end);
label_4:
      return sortedList;
    }

    public MeasurementSet GetMeasurementData(
      uint id,
      MeasurementDataType type,
      DateTime start,
      DateTime end)
    {
      string str = string.Format("GetMeasurementData( ID: {0}, Type: {1}, Start: {2:d}, End: {3:d} )", (object) id, (object) type, (object) start, (object) end);
      MinomatV4.logger.Info(str);
      this.measurementSet = new MeasurementSet();
      this.notHandledMeasurementData.Clear();
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MeasurementData);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(id));
      payload.Add((byte) type);
      payload.Add((byte) 0);
      payload.Add(Convert.ToByte(start.ToString("yy")));
      payload.Add(Convert.ToByte(start.Month));
      if (type == MeasurementDataType.MonthAndHalfMonth)
        payload.Add((byte) 0);
      else
        payload.Add(Convert.ToByte(start.Day));
      if (type == MeasurementDataType.Quarter)
      {
        byte quarter = this.ConvertToQuarter(start.Minute);
        payload.Add(quarter);
      }
      else
        payload.Add(byte.MaxValue);
      payload.Add(Convert.ToByte(end.ToString("yy")));
      payload.Add(Convert.ToByte(end.Month));
      if (type == MeasurementDataType.MonthAndHalfMonth)
        payload.Add((byte) 0);
      else
        payload.Add(Convert.ToByte(end.Day));
      if (type == MeasurementDataType.Quarter)
      {
        byte quarter = this.ConvertToQuarter(start.Minute);
        payload.Add(quarter);
      }
      else
        payload.Add(byte.MaxValue);
      int timeOffsetPerBlock = this.connection.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      try
      {
        this.connection.OnPacketReceived += new EventHandler<SCGiPacket>(this.Connection_OnPacketReceived);
        this.SendAndReceiveFrame(header, payload, str);
      }
      catch (SCGiError ex)
      {
        if (ex.Type != SCGiErrorType.InvalidResponce)
          throw ex;
        this.connection.ClearCom();
        MinomatV4.logger.Fatal("Can not read ID: {0}, Type: {1}, Start: {2:d}, End: {3:d} Reason: {4}", new object[5]
        {
          (object) id,
          (object) type,
          (object) start,
          (object) end,
          (object) ex.Message
        });
      }
      finally
      {
        this.connection.OnPacketReceived -= new EventHandler<SCGiPacket>(this.Connection_OnPacketReceived);
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
      return this.measurementSet;
    }

    private byte ConvertToQuarter(int minute)
    {
      if (minute >= 0 && minute < 15)
        return 0;
      if (minute >= 15 && minute < 30)
        return 1;
      return minute >= 30 && minute < 45 ? (byte) 2 : (byte) 3;
    }

    private void Connection_OnPacketReceived(object sender, SCGiPacket e)
    {
      if (e.IsFirstPacketOfFrame)
      {
        this.CheckResponse(e);
        if (e.Payload.Length == 4 && e.Payload[2] == (byte) 0)
        {
          switch (e.Payload[3])
          {
            case 1:
              throw new SCGiError(SCGiErrorType.UnknownMessUnit, "Unknown mess unit! Error number: 1");
            case 2:
              this.measurementSet = new MeasurementSet();
              return;
            case 3:
              throw new SCGiError(SCGiErrorType.ItIsNotPossableCurrently, "The Minomat is currently in a phase in which the measurement data retrieval is not possible! Error number: 3");
            case 4:
              throw new SCGiError(SCGiErrorType.InvalidTimeInterval, "Invalid time interval! Error number: 4");
            default:
              throw new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.MeasurementData)) + " (GetMeasurementData) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(e.Payload));
          }
        }
      }
      if (e.IsFirstPacketOfFrame && this.notHandledMeasurementData.Count > 0)
        this.notHandledMeasurementData.Clear();
      byte[] numArray1 = e.Payload;
      int offset = e.IsFirstPacketOfFrame ? 2 : 0;
      if (this.notHandledMeasurementData.Count > 0)
      {
        this.notHandledMeasurementData.AddRange((IEnumerable<byte>) numArray1);
        numArray1 = this.notHandledMeasurementData.ToArray();
        this.notHandledMeasurementData.Clear();
      }
      while (numArray1.Length >= 12 + offset)
      {
        MeasurementDataHeader header = MeasurementDataHeader.Parse(numArray1, offset);
        offset += 12;
        int num = numArray1.Length - offset;
        if (num >= header.CountOfExpectedBytes)
        {
          try
          {
            MeasurementData measurementData = MeasurementData.Parse(header, numArray1, ref offset);
            if (measurementData != null)
            {
              this.measurementSet.Add(measurementData);
              if (this.OnMeasurementDataReceived != null)
                this.OnMeasurementDataReceived((object) this, measurementData);
            }
          }
          catch (SCGiError ex)
          {
            this.connection.ClearCom();
            this.notHandledMeasurementData.Clear();
            throw ex;
          }
        }
        else if (num >= 0)
        {
          byte[] numArray2 = new byte[num + 12];
          Buffer.BlockCopy((Array) numArray1, offset - 12, (Array) numArray2, 0, numArray2.Length);
          this.notHandledMeasurementData.AddRange((IEnumerable<byte>) numArray2);
          return;
        }
        if (offset >= numArray1.Length)
          return;
      }
      byte[] numArray3 = new byte[numArray1.Length - offset];
      Buffer.BlockCopy((Array) numArray1, offset, (Array) numArray3, 0, numArray3.Length);
      this.notHandledMeasurementData.AddRange((IEnumerable<byte>) numArray3);
    }

    public MessUnitMetadata GetMessUnitMetadata(object id)
    {
      return id != null ? this.GetMessUnitMetadata(id.ToString()) : throw new ArgumentNullException("ID can not be null!");
    }

    public MessUnitMetadata GetMessUnitMetadata(string id)
    {
      uint id1;
      if (!Util.TryParseToUInt32(id, out id1))
        throw new ArgumentException("Invalid id of mess unit!");
      return this.GetMessUnitMetadata(id1);
    }

    public MessUnitMetadata GetMessUnitMetadata(uint id)
    {
      string str = string.Format("GetMessUnitMetadata( {0} )", (object) id);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MessUnitMetadata);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(id));
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Minol, this.Authentication), payload, str);
      if (onlyOnePacket == null || onlyOnePacket.Payload.Length == 2 && onlyOnePacket.Payload[0] == (byte) 0 && onlyOnePacket.Payload[1] == (byte) 0 || onlyOnePacket.Payload.Length == 4 && onlyOnePacket.Payload[0] == (byte) 0 && onlyOnePacket.Payload[1] == (byte) 0)
        return (MessUnitMetadata) null;
      if (onlyOnePacket.Payload.Length == 2 || onlyOnePacket.Payload.Length == 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.MessUnitMetadata)) + " (GetMessUnitMetadata(" + id.ToString() + ") received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return MessUnitMetadata.Parse(onlyOnePacket.Payload);
    }

    public List<uint> GetRegisteredMessUnits()
    {
      string str = "GetRegisteredMessUnits()";
      MinomatV4.logger.Info(str);
      int timeOffsetPerBlock = this.connection.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.RegisteredMessUnits);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiFrame scGiFrame = (SCGiFrame) null;
      try
      {
        scGiFrame = this.SendAndReceiveFrame(header, payload, str);
        if (scGiFrame == null || scGiFrame.Count == 0)
          return (List<uint>) null;
      }
      finally
      {
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
      List<uint> registeredMessUnits = new List<uint>();
      for (int index = 0; index < scGiFrame.Count; ++index)
      {
        SCGiPacket scGiPacket = scGiFrame[index];
        int num = index == 0 ? 2 : 0;
        if ((scGiPacket.Payload.Length - num) % 4 != 0)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: length mod 4, SCGi header: {1}, Payload buffer: {2}", (object) scGiPacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(scGiPacket.Payload)));
          MinomatV4.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        for (int startIndex = num; startIndex < scGiPacket.Payload.Length; startIndex += 4)
        {
          uint uint32 = BitConverter.ToUInt32(scGiPacket.Payload, startIndex);
          registeredMessUnits.Add(uint32);
        }
      }
      MinomatV4.logger.Info("RegisteredMessUnits = {0}", registeredMessUnits.Count);
      return registeredMessUnits;
    }

    public MessUnit GetInfoOfRegisteredMessUnit(object id)
    {
      return id != null ? this.GetInfoOfRegisteredMessUnit(id.ToString()) : throw new ArgumentNullException("ID can not be null!");
    }

    public MessUnit GetInfoOfRegisteredMessUnit(string id)
    {
      uint id1;
      if (!Util.TryParseToUInt32(id, out id1))
        throw new ArgumentException("Invalid serialnumber!");
      return this.GetInfoOfRegisteredMessUnit(id1);
    }

    public MessUnit GetInfoOfRegisteredMessUnit(uint id)
    {
      string str = string.Format("GetInfoOfRegisteredMessUnit( ID: {0} )", (object) id);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.InfoOfRegisteredMessUnit);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(id));
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return (MessUnit) null;
      if (onlyOnePacket.Payload.Length != 6)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return MessUnit.Parse(id, onlyOnePacket.Payload);
    }

    public uint? RegisterMessUnit(object id, object category, object type, object protocol)
    {
      if (id == null)
        throw new ArgumentNullException("Input parameter 'id' can not be null!");
      if (category == null)
        throw new ArgumentNullException("Input parameter 'category' can not be null!");
      if (type == null)
        throw new ArgumentNullException("Input parameter 'type' can not be null!");
      if (protocol == null)
        throw new ArgumentNullException("Input parameter 'protocol' can not be null!");
      return this.RegisterMessUnit(id.ToString(), category.ToString(), type.ToString(), protocol.ToString());
    }

    public uint? RegisterMessUnit(string id, string category, string type, string protocol)
    {
      uint id1;
      if (!Util.TryParseToUInt32(id, out id1))
        throw new ArgumentException("Invalid id of mess unit!");
      if (string.IsNullOrEmpty(category) || !Enum.IsDefined(typeof (MeasurementCategory), (object) category))
        throw new ArgumentException("Invalid measurement category of the mess unit!");
      if (string.IsNullOrEmpty(type) || !Enum.IsDefined(typeof (MeasurementValueType), (object) type))
        throw new ArgumentException("Invalid measurement type of the mess unit!");
      if (string.IsNullOrEmpty(protocol) || !Enum.IsDefined(typeof (RadioProtocol), (object) protocol))
        throw new ArgumentException("Invalid radio protocol of the mess unit!");
      MeasurementCategory category1 = (MeasurementCategory) Enum.Parse(typeof (MeasurementCategory), category, true);
      MeasurementValueType type1 = (MeasurementValueType) Enum.Parse(typeof (MeasurementValueType), type, true);
      RadioProtocol protocol1 = (RadioProtocol) Enum.Parse(typeof (RadioProtocol), protocol, true);
      return this.RegisterMessUnit(id1, category1, type1, protocol1);
    }

    public uint? RegisterMessUnit(
      uint id,
      MeasurementCategory category,
      MeasurementValueType type,
      RadioProtocol protocol)
    {
      return this.RegisterMessUnit(new MessUnit()
      {
        ID = id,
        Category = category,
        Type = type,
        Protocol = protocol
      });
    }

    public uint? RegisterMessUnit(object messunit)
    {
      if (messunit == null)
        throw new ArgumentNullException("Input paramenet 'messunit' can not be null!");
      return messunit is MessUnit ? this.RegisterMessUnit(messunit as MessUnit) : throw new ArgumentException("Wrong type of parameter 'messunit'. Expected type: MessUnit");
    }

    public uint? RegisterMessUnit(MessUnit messunit)
    {
      string str = messunit != null ? string.Format("RegisterMessUnit( MessUnit: {0} )", (object) messunit) : throw new ArgumentNullException("Mess unit can not be null!");
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.RegisterMessUnit);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(messunit.ID));
      payload.Add((byte) messunit.Category);
      payload.Add((byte) messunit.Type);
      payload.Add((byte) messunit.Protocol);
      payload.Add((byte) 0);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new uint?();
      if (onlyOnePacket.Payload.Length < 2)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: > 2, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, string.Format("Unknown responce received! SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      short int16 = BitConverter.ToInt16(onlyOnePacket.Payload, 2);
      if (int16 > (short) 0)
        return new uint?((uint) BitConverter.ToUInt16(onlyOnePacket.Payload, 2));
      if (int16 == (short) 0)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.CanNotRegisterMessUnit, "Can not register the mess unit! " + messunit?.ToString());
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (int16 == (short) -1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.MeterAlreadyExists, "The meter already exists!");
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (int16 == (short) -2)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.MaxNumberOfRegisteredMeterReached, "Maximum number of meter units are reached (max. 300)!");
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (int16 == (short) -3)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.NoMemorySpaceInEepromAvailable, "No memory space available (EEPROM)!");
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (int16 == (short) -5)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidScenario, "Invalid scenario configured!");
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      SCGiError scGiError1 = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.RegisterMessUnit)) + " (RegisterMessUnit) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
      MinomatV4.logger.Error<SCGiError>(scGiError1);
      throw scGiError1;
    }

    public bool StartTestReception(string action, string mode)
    {
      if (string.IsNullOrEmpty(action) || !Enum.IsDefined(typeof (StartTestReceptionAction), (object) action))
        throw new ArgumentException("Invalid StartTestReceptionAction argument!");
      return !string.IsNullOrEmpty(mode) && Enum.IsDefined(typeof (RadioProtocol), (object) mode) ? this.StartTestReception((StartTestReceptionAction) Enum.Parse(typeof (StartTestReceptionAction), action, true), (RadioProtocol) Enum.Parse(typeof (RadioProtocol), mode, true)) : throw new ArgumentException("Invalid RadioProtocol argument!");
    }

    public bool StartTestReception(StartTestReceptionSettings settings)
    {
      return this.StartTestReception(settings.Action, settings.Protocol);
    }

    public bool StartTestReception(object action, object mode)
    {
      if (action == null)
        throw new ArgumentNullException("Input parameter 'action' can not be null!");
      return mode != null ? this.StartTestReception(action.ToString(), mode.ToString()) : throw new ArgumentNullException("Input parameter 'mode' can not be null!");
    }

    public bool StartTestReception(StartTestReceptionAction action, RadioProtocol mode)
    {
      string str = string.Format("StartTestReception( Action: {0}, Mode: {1} )", (object) action, (object) mode);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.StartTestReception);
      payload.Add((byte) action);
      payload.Add((byte) mode);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      switch (onlyOnePacket.Payload[2])
      {
        case 0:
          SCGiError scGiError1 = new SCGiError(SCGiErrorType.DeviceIsNotInDeploymentPhase, "The device is not in the deployment phase!");
          MinomatV4.logger.Error<SCGiError>(scGiError1);
          throw scGiError1;
        case 1:
          return true;
        case byte.MaxValue:
          SCGiError scGiError2 = action != StartTestReceptionAction.Start ? new SCGiError(SCGiErrorType.TestReceptionWasNotStarted, "The test reception was not started!") : new SCGiError(SCGiErrorType.TestReceptionAlreadyRunningOrWasExitWithoutCommit, "The test reception is already running or was already exit without commit!");
          MinomatV4.logger.Error<SCGiError>(scGiError2);
          throw scGiError2;
        default:
          SCGiError scGiError3 = new SCGiError(SCGiErrorType.UnknownResponce, string.Format("Unexpected responce received! PAYLOAD: {0}", (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
          MinomatV4.logger.Error<SCGiError>(scGiError3);
          throw scGiError3;
      }
    }

    public TestReceptionResult GetTestReceptionResult()
    {
      string str = "GetTestReceptionResult()";
      MinomatV4.logger.Info(str);
      SCGiFrame frame = this.SendAndReceiveFrame(new SCGiHeader(SCGiMessageType.Minol, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.TestReceptionResult), str);
      return frame == null ? (TestReceptionResult) null : TestReceptionResult.Parse(frame);
    }

    public bool DeleteMessUnit(object id)
    {
      return id != null ? this.DeleteMessUnit(id.ToString()) : throw new ArgumentNullException("ID can not be null!");
    }

    public bool DeleteMessUnit(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("Invalid input parameter 'id'!");
      uint id1;
      if (!Util.TryParseToUInt32(id, out id1))
        throw new ArgumentException("Invalid input parameter 'id'!");
      return this.DeleteMessUnit(id1);
    }

    public bool DeleteMessUnit(uint id)
    {
      string str = string.Format("DeleteMessUnit( ID: {0} )", (object) id);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.DeleteMessUnit);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(id));
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return onlyOnePacket.Payload[2] == (byte) 1;
    }

    public RadioChannel GetRadioChannel()
    {
      string str = "GetRadioChannel()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Minol, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.RadioChannel), str);
      return onlyOnePacket == null ? (RadioChannel) null : RadioChannel.Parse(onlyOnePacket.Payload);
    }

    public bool SetRadioChannel(object newRadioChannelId)
    {
      if (newRadioChannelId == null)
        throw new ArgumentNullException("Input parameter 'newRadioChannelId' can not be null!");
      return newRadioChannelId is short id || Util.TryParseToInt16(newRadioChannelId.ToString(), out id) ? this.SetRadioChannel(new RadioChannel(id)) : throw new ArgumentNullException("Can not parse input parameter 'newRadioChannelId' to Int16!");
    }

    public bool SetRadioChannel(string newRadioChannelId)
    {
      if (string.IsNullOrEmpty(newRadioChannelId))
        throw new ArgumentNullException("Input parameter 'newRadioChannelId' can not be null!");
      short id;
      if (!Util.TryParseToInt16(newRadioChannelId, out id))
        throw new ArgumentNullException("Input parameter 'newRadioChannelId' is not valid Int16 object!");
      return this.SetRadioChannel(new RadioChannel(id));
    }

    public bool SetRadioChannel(short newRadioChannelId)
    {
      return this.SetRadioChannel(new RadioChannel(newRadioChannelId));
    }

    public bool SetRadioChannel(RadioChannel newRadioChannel)
    {
      if (newRadioChannel == null || !newRadioChannel.ID.HasValue)
        return false;
      string str = string.Format("SetRadioChannel( RadioChannel: {0} )", (object) newRadioChannel.ID);
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Minol, this.Authentication), this.CreatePayload(SCGiCommand.RadioChannel, newRadioChannel.ID.Value), str);
      if (onlyOnePacket == null)
        return false;
      RadioChannel radioChannel = RadioChannel.Parse(onlyOnePacket.Payload);
      newRadioChannel.Error = radioChannel.Error;
      return newRadioChannel.Error == RadioChannelError.None;
    }

    public byte[] GetPhaseDetailsBuffer()
    {
      string str = "GetPhaseDetailsBuffer()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.PhaseDetailsBuffer);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return (byte[]) null;
      if (onlyOnePacket.Payload.Length < 2)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: >=2, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return (byte[]) null;
      }
      if (onlyOnePacket.Payload.Length == 2)
        return new byte[0];
      byte[] dst = new byte[onlyOnePacket.Payload.Length - 2];
      Buffer.BlockCopy((Array) onlyOnePacket.Payload, 2, (Array) dst, 0, dst.Length);
      return dst;
    }

    public bool SetPhaseDetailsBuffer(object newPhasesDetailsBuffer)
    {
      return newPhasesDetailsBuffer != null ? this.SetPhaseDetailsBuffer(!(newPhasesDetailsBuffer is byte[]) ? Util.HexStringToByteArray(newPhasesDetailsBuffer.ToString()) : (byte[]) newPhasesDetailsBuffer) : throw new ArgumentNullException("Input parameter 'newPhasesDetailsBuffer' can not be null!");
    }

    public bool SetPhaseDetailsBuffer(string newPhasesDetailsBuffer)
    {
      return this.SetPhaseDetailsBuffer(Util.HexStringToByteArray(newPhasesDetailsBuffer));
    }

    public bool SetPhaseDetailsBuffer(byte[] newPhasesDetails)
    {
      if (newPhasesDetails == null)
        return false;
      string str = string.Format("SetPhaseDetailsBuffer( {0} )", (object) Util.ByteArrayToHexString(newPhasesDetails));
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.PhaseDetailsBuffer, newPhasesDetails);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length == 4)
        return BitConverter.ToUInt16(onlyOnePacket.Payload, 2) == (ushort) 1;
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return false;
    }

    public bool StartNetworkSetup(object mode)
    {
      return mode != null ? this.StartNetworkSetup(mode.ToString()) : throw new ArgumentNullException("Input paramenet 'mode' can not be null!");
    }

    public bool StartNetworkSetup(string mode)
    {
      return !string.IsNullOrEmpty(mode) && Enum.IsDefined(typeof (NetworkSetupMode), (object) mode) ? this.StartNetworkSetup((NetworkSetupMode) Enum.Parse(typeof (NetworkSetupMode), mode, true)) : throw new ArgumentException("Invalid NetworkSetupMode parameter!");
    }

    public bool StartNetworkSetup(NetworkSetupMode mode)
    {
      string str = string.Format("StartNetworkSetup( Mode: {0} )", (object) mode);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.StartNetworkSetup);
      payload.Add((byte) mode);
      payload.Add((byte) 0);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      switch (onlyOnePacket.Payload[2])
      {
        case 0:
          SCGiError scGiError1 = new SCGiError(SCGiErrorType.WrongPhaseOrWrongDeviceType, "Wrong phase or wrong device type! Error number: 0");
          MinomatV4.logger.Error<SCGiError>(scGiError1);
          throw scGiError1;
        case 1:
          return true;
        case 253:
          SCGiError scGiError2 = new SCGiError(SCGiErrorType.InvalidNetworkState, "Invalid network state! Error number: -3");
          MinomatV4.logger.Error<SCGiError>(scGiError2);
          throw scGiError2;
        case 254:
          SCGiError scGiError3 = new SCGiError(SCGiErrorType.WrongDeviceType, "Wrong device type (standalone)! Error number: -2");
          MinomatV4.logger.Error<SCGiError>(scGiError3);
          throw scGiError3;
        case byte.MaxValue:
          SCGiError scGiError4 = new SCGiError(SCGiErrorType.NoSlaveRegistered, "No slaves registered! Error number: -1");
          MinomatV4.logger.Error<SCGiError>(scGiError4);
          throw scGiError4;
        default:
          SCGiError scGiError5 = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.StartNetworkSetup)) + " (StartNetworkSetup) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError5);
          throw scGiError5;
      }
    }

    public ResetConfigurationState GetResetConfigurationState()
    {
      EepromBlock eeprom = this.GetEeprom((ushort) 0, (ushort) 2840, (ushort) 2);
      if (eeprom == null || eeprom.Count != 2)
        return ResetConfigurationState.Unknown;
      ushort uint16 = BitConverter.ToUInt16(eeprom.ToArray(), 0);
      if (!Enum.IsDefined(typeof (ResetConfigurationState), (object) uint16))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Unknown state of ResetConfig received! Value: 0x{0}", (object) uint16.ToString("X4")));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (ResetConfigurationState) Enum.ToObject(typeof (ResetConfigurationState), uint16);
    }

    public bool ResetConfiguration()
    {
      string str = "ResetConfiguration()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ResetConfiguration);
      payload.AddRange((IEnumerable<byte>) new byte[2]
      {
        (byte) 211,
        (byte) 145
      });
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      switch (onlyOnePacket.Payload[2])
      {
        case 240:
          SCGiError scGiError1 = new SCGiError(SCGiErrorType.AuthenticationFailed, "Wrong password by reset configuration!");
          MinomatV4.logger.Error<SCGiError>(scGiError1);
          throw scGiError1;
        case byte.MaxValue:
          return true;
        default:
          SCGiError scGiError2 = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.ResetConfiguration)) + " (ResetConfiguration) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError2);
          throw scGiError2;
      }
    }

    public bool DeregisterSlave(object minolID)
    {
      return minolID != null ? this.DeregisterSlave(minolID.ToString()) : throw new ArgumentNullException("minolID can not be null!");
    }

    public bool DeregisterSlave(string minolID)
    {
      uint minolID1;
      if (!Util.TryParseToUInt32(minolID, out minolID1))
        throw new ArgumentException("Invalid input parameter 'minolID'!");
      return this.DeregisterSlave(minolID1);
    }

    public bool DeregisterSlave(uint minolID)
    {
      MinomatV4.logger.Info("DeregisterSlave( ID: {0} )", minolID);
      return this.RegisterSlave((ushort) 0, minolID);
    }

    public bool RegisterSlave(object slaveNodeID, object minolID)
    {
      if (slaveNodeID == null)
        throw new ArgumentNullException("Input parameter 'slaveNodeID' can not be null!");
      return minolID != null ? this.RegisterSlave(slaveNodeID.ToString(), minolID.ToString()) : throw new ArgumentNullException("Input parameter 'minolID' can not be null!");
    }

    public bool RegisterSlave(string slaveNodeID, string minolID)
    {
      ushort slaveID;
      if (!Util.TryParseToUInt16(slaveNodeID, out slaveID))
        throw new ArgumentException("Invalid Slave ID!");
      uint minolID1;
      if (!Util.TryParseToUInt32(minolID, out minolID1))
        throw new ArgumentException("Invalid Minol ID!");
      return this.RegisterSlave(slaveID, minolID1);
    }

    public bool RegisterSlave(Slave slave)
    {
      if (slave == null)
        throw new ArgumentNullException("Input parameter 'slave' can not be null!");
      return this.RegisterSlave(slave.SlaveNodeID, slave.MinolID);
    }

    public bool RegisterSlave(ushort slaveID, uint minolID)
    {
      string str = string.Format("RegisterSlave( SlaveID: {0}, MinolID: {1} )", (object) slaveID, (object) minolID);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.RegisterOrDeregisterSlave);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(slaveID));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(minolID));
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 6)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      byte num = onlyOnePacket.Payload[2];
      switch (num)
      {
        case 250:
          SCGiError scGiError1 = new SCGiError(SCGiErrorType.NoResourceAvailable, "No resource available! Errornumber: -6");
          MinomatV4.logger.Error<SCGiError>(scGiError1);
          throw scGiError1;
        case 251:
          SCGiError scGiError2 = new SCGiError(SCGiErrorType.IsNotMaster, "It is not a master Minomat! Errornumber: -5");
          MinomatV4.logger.Error<SCGiError>(scGiError2);
          throw scGiError2;
        case 252:
          SCGiError scGiError3 = new SCGiError(SCGiErrorType.InvalidMinolID, "Invalid Minol ID! Errornumber: -4");
          MinomatV4.logger.Error<SCGiError>(scGiError3);
          throw scGiError3;
        case 253:
          SCGiError scGiError4 = new SCGiError(SCGiErrorType.MaxNumberOfSlavesReached, "Max number of registered slaves reached! Errornumber: -3");
          MinomatV4.logger.Error<SCGiError>(scGiError4);
          throw scGiError4;
        case 254:
          SCGiError scGiError5 = new SCGiError(SCGiErrorType.NotUsed, "Today not used! Errornumber: -2");
          MinomatV4.logger.Error<SCGiError>(scGiError5);
          throw scGiError5;
        case byte.MaxValue:
          SCGiError scGiError6 = new SCGiError(SCGiErrorType.SlaveAlreadyRegistered, "Slave already registered! Errornumber: -1");
          MinomatV4.logger.Error<SCGiError>(scGiError6);
          throw scGiError6;
        default:
          MinomatV4.logger.Debug("Count of registered slaves: " + num.ToString());
          return num >= (byte) 0;
      }
    }

    public TableOfSlaves GetRegisteredSlaves(string slaveType)
    {
      return !string.IsNullOrEmpty(slaveType) && Enum.IsDefined(typeof (SlaveType), (object) slaveType) ? this.GetRegisteredSlaves((SlaveType) Enum.Parse(typeof (SlaveType), slaveType, true)) : throw new ArgumentException("Invalid filter argument!");
    }

    public TableOfSlaves GetRegisteredSlaves(object type)
    {
      return type != null ? this.GetRegisteredSlaves(type.ToString()) : throw new ArgumentNullException("Input parameter 'type' can not be null!");
    }

    public TableOfSlaves GetRegisteredSlaves(SlaveType type)
    {
      string str = string.Format("GetRegisteredSlaves( Type: {0} )", (object) type);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.RegisteredSlaves);
      payload.Add((byte) type);
      payload.Add((byte) 0);
      SCGiFrame frame = this.SendAndReceiveFrame(new SCGiHeader(SCGiMessageType.Minol, this.Authentication), payload, str);
      if (frame == null || frame.Count == 0)
        return (TableOfSlaves) null;
      TableOfSlaves registeredSlaves = new TableOfSlaves();
      for (int index = 0; index < frame.Count; ++index)
      {
        List<Slave> collection = (List<Slave>) null;
        if (index == 0)
        {
          if (frame[index].Payload.Length >= 8)
          {
            byte[] numArray = new byte[frame[index].Payload.Length - 2];
            Buffer.BlockCopy((Array) frame[index].Payload, 2, (Array) numArray, 0, numArray.Length);
            collection = Slave.Parse(numArray);
          }
          else if (frame[index].Payload.Length != 2)
          {
            if (frame[index].Payload.Length != 4)
            {
              SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, string.Format("Unknown responce! Buffer: {0}", (object) Util.ByteArrayToHexString(frame[index].Payload)));
              MinomatV4.logger.Error<SCGiError>(scGiError);
              throw scGiError;
            }
            switch (frame[index].Payload[2])
            {
              case 254:
                SCGiError scGiError1 = new SCGiError(SCGiErrorType.IsNotMaster, "It is not a master Minomat! Error number: -2");
                MinomatV4.logger.Error<SCGiError>(scGiError1);
                throw scGiError1;
              case byte.MaxValue:
                SCGiError scGiError2 = new SCGiError(SCGiErrorType.NoResourceAvailable, "No resource available! Error number: -1");
                MinomatV4.logger.Error<SCGiError>(scGiError2);
                throw scGiError2;
              default:
                SCGiError scGiError3 = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.RegisteredSlaves)) + " (GetRegisteredSlaves) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(frame[index].Payload));
                MinomatV4.logger.Error<SCGiError>(scGiError3);
                throw scGiError3;
            }
          }
        }
        else
          collection = Slave.Parse(frame[index].Payload);
        if (collection != null)
          registeredSlaves.AddRange((IEnumerable<Slave>) collection);
      }
      return registeredSlaves;
    }

    public uint? GetMinolId()
    {
      string str = "GetMinolId()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MinolId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new uint?();
      if (onlyOnePacket.Payload.Length == 6)
        return new uint?(BitConverter.ToUInt32(onlyOnePacket.Payload, 2));
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new uint?();
    }

    public bool SetMinolId(object id)
    {
      if (id == null)
        throw new ArgumentNullException("Input parameter 'id' can not be null!");
      return id is uint id1 || Util.TryParseToUInt32(id.ToString(), out id1) ? this.SetMinolId(id1) : throw new ArgumentException("Can not parse input paramter 'id' to UInt32!");
    }

    public bool SetMinolId(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentNullException("Input parameter 'id' can not be null!");
      uint id1;
      if (!Util.TryParseToUInt32(id, out id1))
        throw new ArgumentException("Can not parse input paramter 'id' to UInt32!");
      return this.SetMinolId(id1);
    }

    public bool SetMinolId(uint id)
    {
      string str = string.Format("SetMinolId( ID: {0})", (object) id);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.MinolId, id);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length == 6)
        return (int) BitConverter.ToUInt32(onlyOnePacket.Payload, 2) == (int) id;
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return false;
    }

    public NetworkOptimizationState ForceNetworkOptimisation()
    {
      string str = "ForceNetworkOptimisation()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ForceNetworkOptimization);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return NetworkOptimizationState.Unknown;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      byte num = onlyOnePacket.Payload[2];
      if (!Enum.IsDefined(typeof (NetworkOptimizationState), (object) num))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Unknown state of NetworkOptimisation detected! State: 0x{0}, Payload buffer: {1}", (object) num.ToString("X2"), (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (NetworkOptimizationState) Enum.ToObject(typeof (NetworkOptimizationState), num);
    }

    public PhaseDetails GetPhaseDetails()
    {
      string str = "GetPhaseDetails()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Minol, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.PhaseDetails), str);
      return onlyOnePacket == null ? (PhaseDetails) null : PhaseDetails.Parse(onlyOnePacket.Payload);
    }

    public bool SwitchToNetworkModel()
    {
      string str = "SwitchToNetworkModel()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.SwitchToNetworkModel);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      switch (onlyOnePacket.Payload[2])
      {
        case 0:
          SCGiError scGiError1 = new SCGiError(SCGiErrorType.SavingError, "Can not save! Buffer: " + Util.ByteArrayToHexString(onlyOnePacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError1);
          throw scGiError1;
        case 1:
          return true;
        case 254:
          SCGiError scGiError2 = new SCGiError(SCGiErrorType.NoSwitchPossible, "No switch possible! Buffer: " + Util.ByteArrayToHexString(onlyOnePacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError2);
          throw scGiError2;
        case byte.MaxValue:
          SCGiError scGiError3 = new SCGiError(SCGiErrorType.NoRestorePossible, "No restore possible! Buffer: " + Util.ByteArrayToHexString(onlyOnePacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError3);
          throw scGiError3;
        default:
          SCGiError scGiError4 = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.SwitchToNetworkModel)) + " (SwitchToNetworkModel) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
          MinomatV4.logger.Error<SCGiError>(scGiError4);
          throw scGiError4;
      }
    }

    public bool StartNetworkOptimization()
    {
      string str = "StartNetworkOptimization()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.StartNetworkOptimization);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (onlyOnePacket.Payload[2] == (byte) 1)
        return true;
      throw new Exception("Can not start the network optimization.");
    }

    public ushort? GetMessUnitNumberMax()
    {
      string str = "GetMessUnitNumberMax()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MessUnitNumberMax);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length == 4)
        return new ushort?(BitConverter.ToUInt16(onlyOnePacket.Payload, 2));
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new ushort?();
    }

    public ushort? GetMessUnitNumberNotConfiguredMax()
    {
      string str = "GetMessUnitNumberNotConfiguredMax()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MaxMessUnitNumberNotConfigured);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length == 4)
        return new ushort?(BitConverter.ToUInt16(onlyOnePacket.Payload, 2));
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new ushort?();
    }

    public bool SetMessUnitNumberNotConfiguredMax(object messUnitNumberNotExplicitConfigureMax)
    {
      if (messUnitNumberNotExplicitConfigureMax == null)
        throw new ArgumentNullException("Imput parameter 'messUnitNumberNotExplicitConfigureMax' can not be null!");
      return messUnitNumberNotExplicitConfigureMax is ushort newMessUnitNumberNotExplicitConfigureMax || Util.TryParseToUInt16(messUnitNumberNotExplicitConfigureMax.ToString(), out newMessUnitNumberNotExplicitConfigureMax) ? this.SetMessUnitNumberNotConfiguredMax(newMessUnitNumberNotExplicitConfigureMax) : throw new ArgumentException("Can not parse input parameter 'messUnitNumberNotExplicitConfigureMax' to UInt16!");
    }

    public bool SetMessUnitNumberNotConfiguredMax(string messUnitNumberNotExplicitConfigureMax)
    {
      if (string.IsNullOrEmpty(messUnitNumberNotExplicitConfigureMax))
        throw new ArgumentNullException("Imput parameter 'messUnitNumberNotExplicitConfigureMax' can not be null!");
      ushort newMessUnitNumberNotExplicitConfigureMax;
      if (!Util.TryParseToUInt16(messUnitNumberNotExplicitConfigureMax, out newMessUnitNumberNotExplicitConfigureMax))
        throw new ArgumentException("Can not parse input parameter 'messUnitNumberNotExplicitConfigureMax' to UInt16!");
      return this.SetMessUnitNumberNotConfiguredMax(newMessUnitNumberNotExplicitConfigureMax);
    }

    public bool SetMessUnitNumberNotConfiguredMax(ushort newMessUnitNumberNotExplicitConfigureMax)
    {
      string str = string.Format("SetMessUnitNumberNotExplicitConfigureMax( {0} )", (object) newMessUnitNumberNotExplicitConfigureMax);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.MaxMessUnitNumberNotConfigured, (uint) newMessUnitNumberNotExplicitConfigureMax);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length == 4)
        return (int) BitConverter.ToUInt16(onlyOnePacket.Payload, 2) == (int) newMessUnitNumberNotExplicitConfigureMax;
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return false;
    }

    public Scenario? GetScenario()
    {
      string str = "GetScenario()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.Scenario);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new Scenario?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new Scenario?();
      }
      if (Enum.IsDefined(typeof (Scenario), (object) onlyOnePacket.Payload[2]))
        return new Scenario?((Scenario) Enum.ToObject(typeof (Scenario), onlyOnePacket.Payload[2]));
      MinomatV4.logger.Error<byte, string>("Unknown scenario! 0x{0:X2}, PAYLOAD: {1}", onlyOnePacket.Payload[2], Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new Scenario?();
    }

    public bool SetScenario(object scenario)
    {
      return scenario != null ? this.SetScenario(scenario.ToString()) : throw new ArgumentNullException("Input parameter 'scenario' can not be null!");
    }

    public bool SetScenario(string scenario)
    {
      return !string.IsNullOrEmpty(scenario) && Enum.IsDefined(typeof (Scenario), (object) scenario) ? this.SetScenario((Scenario) Enum.Parse(typeof (Scenario), scenario, true)) : throw new ArgumentException("Invalid Scenario parameter!");
    }

    public bool SetScenario(Scenario scenario)
    {
      string str = string.Format("SetScenario( {0} )", (object) scenario);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.Scenario);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      payload.Add((byte) scenario);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      if (onlyOnePacket.Payload[2] == byte.MaxValue)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidScenario, "Invalid scenario! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (onlyOnePacket.Payload[2] == (byte) 250)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.NotChangeable, "Can not change scenario! Wrong phase. Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (Scenario), (object) onlyOnePacket.Payload[2]))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, "Unknown scenario received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      Scenario scenario1 = (Scenario) Enum.ToObject(typeof (Scenario), onlyOnePacket.Payload[2]);
      if (scenario1 != scenario)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.CanNotChangeScenario, "Can't change current scenario! Current scanerio is: " + scenario1.ToString());
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return true;
    }

    public bool SetScenario_1Byte(object scenario)
    {
      return scenario != null ? this.SetScenario_1Byte(scenario.ToString()) : throw new ArgumentNullException("Input parameter 'scenario' can not be null!");
    }

    public bool SetScenario_1Byte(string scenario)
    {
      return !string.IsNullOrEmpty(scenario) && Enum.IsDefined(typeof (Scenario), (object) scenario) ? this.SetScenario_1Byte((Scenario) Enum.Parse(typeof (Scenario), scenario, true)) : throw new ArgumentException("Invalid Scenario parameter!");
    }

    public bool SetScenario_1Byte(Scenario scenario)
    {
      string str = string.Format("SetScenario_1Byte( {0} )", (object) scenario);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.Scenario);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      payload.Add((byte) scenario);
      payload.Add((byte) 0);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      if (onlyOnePacket.Payload[2] == byte.MaxValue)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidScenario, "Invalid scenario! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (onlyOnePacket.Payload[2] == (byte) 250)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.NotChangeable, "Can not change scenario! Wrong phase. Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (!Enum.IsDefined(typeof (Scenario), (object) onlyOnePacket.Payload[2]))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, "Unknown scenario received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      Scenario scenario1 = (Scenario) Enum.ToObject(typeof (Scenario), onlyOnePacket.Payload[2]);
      if (scenario1 != scenario)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.CanNotChangeScenario, "Can't change current scenario! Current scanerio is: " + scenario1.ToString());
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return true;
    }

    public FlashBlock GetFlash(object flash)
    {
      if (flash == null)
        throw new ArgumentNullException("Input parameter 'flash' can not be null!");
      return flash is FlashBlock ? this.GetFlash(flash as FlashBlock) : throw new ArgumentException("Wrong type of parameter 'flash'. Expected type: FlashBlock");
    }

    public FlashBlock GetFlash(FlashBlock flash)
    {
      if (flash == null)
        throw new ArgumentNullException("Input parameter 'flash' can not be null!");
      if (flash.ExpectedSize == (ushort) 0)
        throw new ArgumentException("Invalid input parameter! 'size' can not be 0.");
      if ((int) flash.Offset > (int) flash.ExpectedSize)
        throw new ArgumentOutOfRangeException("Invalid input parameter! 'offset' can not be greater as 'size'.");
      return this.GetFlash(flash.ChipNumber, flash.PageNumber, flash.Offset, flash.ExpectedSize);
    }

    public FlashBlock GetFlash(ushort chipNumber, ushort pageNumber)
    {
      return this.GetFlash(chipNumber, pageNumber, (ushort) 0, (ushort) 4096);
    }

    public FlashBlock GetFlash(object chipNumber, object pageNumber, object offset, object size)
    {
      if (chipNumber == null)
        throw new ArgumentNullException("Input parameter 'chipNumber' can not be null!");
      if (pageNumber == null)
        throw new ArgumentNullException("Input parameter 'pageNumber' can not be null!");
      if (offset == null)
        throw new ArgumentNullException("Input parameter 'offset' can not be null!");
      if (size == null)
        throw new ArgumentNullException("Input parameter 'size' can not be null!");
      return this.GetFlash(chipNumber.ToString(), pageNumber.ToString(), offset.ToString(), size.ToString());
    }

    public FlashBlock GetFlash(string chipNumber, string pageNumber, string offset, string size)
    {
      if (string.IsNullOrEmpty(chipNumber))
        throw new ArgumentNullException("Input parameter 'chipNumber' can not be null!");
      if (string.IsNullOrEmpty(pageNumber))
        throw new ArgumentNullException("Input parameter 'pageNumber' can not be null!");
      if (string.IsNullOrEmpty(offset))
        throw new ArgumentNullException("Input parameter 'offset' can not be null!");
      if (string.IsNullOrEmpty(size))
        throw new ArgumentNullException("Input parameter 'size' can not be null!");
      ushort chipNumber1;
      if (!Util.TryParseToUInt16(chipNumber, out chipNumber1))
        throw new ArgumentException("Can not parse input paramnetert 'chipNumber' to UInt16!");
      ushort pageNumber1;
      if (!Util.TryParseToUInt16(pageNumber, out pageNumber1))
        throw new ArgumentException("Can not parse input paramnetert 'pageNumber' to UInt16!");
      ushort offset1;
      if (!Util.TryParseToUInt16(offset, out offset1))
        throw new ArgumentException("Can not parse input paramnetert 'offset' to UInt16!");
      ushort size1;
      if (!Util.TryParseToUInt16(size, out size1))
        throw new ArgumentException("Can not parse input paramnetert 'size' to UInt16!");
      return this.GetFlash(chipNumber1, pageNumber1, offset1, size1);
    }

    public FlashBlock GetFlash(ushort chipNumber, ushort pageNumber, ushort offset, ushort size)
    {
      string str = string.Format("GetFlash( Chip: {0}, Page: {1}, Offset: {2}, Size: {3} )", (object) chipNumber, (object) pageNumber, (object) offset, (object) size);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.Flash);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(chipNumber));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(pageNumber));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(offset));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(size));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) 0));
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      int timeOffsetPerBlock = this.connection.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      SCGiFrame scGiFrame = (SCGiFrame) null;
      try
      {
        scGiFrame = this.SendAndReceiveFrame(header, payload, str);
        if (scGiFrame == null || scGiFrame.Count == 0 || scGiFrame[0].Payload.Length == 12)
          return (FlashBlock) null;
      }
      finally
      {
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
      if (scGiFrame[0].Payload.Length < 10)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Invalid responce received! Payload: " + Util.ByteArrayToHexString(scGiFrame[0].Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      FlashBlock flash = new FlashBlock();
      flash.ChipNumber = BitConverter.ToUInt16(scGiFrame[0].Payload, 2);
      flash.PageNumber = BitConverter.ToUInt16(scGiFrame[0].Payload, 4);
      flash.Offset = BitConverter.ToUInt16(scGiFrame[0].Payload, 6);
      ushort uint16 = BitConverter.ToUInt16(scGiFrame[0].Payload, 8);
      for (int index = 0; index < scGiFrame.Count; ++index)
      {
        if (index == 0)
        {
          byte[] numArray = new byte[scGiFrame[index].Payload.Length - 12];
          Buffer.BlockCopy((Array) scGiFrame[index].Payload, 12, (Array) numArray, 0, numArray.Length);
          flash.AddRange((IEnumerable<byte>) numArray);
        }
        else
          flash.AddRange((IEnumerable<byte>) scGiFrame[index].Payload);
      }
      if ((int) uint16 > flash.Count)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong size of FLASH block! Expected: {0},  Actual: {1}, FLASH: {2}", (object) uint16, (object) flash.Count, (object) Util.ByteArrayToHexString(flash.ToArray())));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return flash;
    }

    public EepromBlock GetEeprom(object eeprom)
    {
      if (eeprom == null)
        throw new ArgumentNullException("Input parameter 'eeprom' can not be null!");
      return eeprom is EepromBlock ? this.GetEeprom(eeprom as EepromBlock) : throw new ArgumentException("Wrong type of parameter 'eeprom'. Expected type: EepromBlock");
    }

    public EepromBlock GetEeprom(EepromBlock eeprom)
    {
      if (eeprom == null)
        throw new ArgumentNullException("Input parameter 'eeprom' can not be null!");
      if (eeprom.ExpectedSize == (ushort) 0)
        throw new ArgumentException("Invalid input parameter! 'size' can not be 0.");
      if ((int) eeprom.Offset > (int) eeprom.ExpectedSize)
        throw new ArgumentOutOfRangeException("Invalid input parameter! 'offset' can not be greater as 'size'.");
      return this.GetEeprom(eeprom.ChipNumber, eeprom.Offset, eeprom.ExpectedSize);
    }

    public EepromBlock GetEeprom(object chipNumber, object offset, object size)
    {
      if (chipNumber == null)
        throw new ArgumentNullException("Input parameter 'chipNumber' can not be null!");
      if (offset == null)
        throw new ArgumentNullException("Input parameter 'offset' can not be null!");
      if (size == null)
        throw new ArgumentNullException("Input parameter 'size' can not be null!");
      return this.GetEeprom(chipNumber.ToString(), offset.ToString(), size.ToString());
    }

    public EepromBlock GetEeprom(string chipNumber, string offset, string size)
    {
      if (chipNumber == null)
        throw new ArgumentNullException("Input parameter 'chipNumber' can not be null!");
      if (offset == null)
        throw new ArgumentNullException("Input parameter 'offset' can not be null!");
      if (size == null)
        throw new ArgumentNullException("Input parameter 'size' can not be null!");
      ushort chipNumber1;
      if (!Util.TryParseToUInt16(chipNumber, out chipNumber1))
        throw new ArgumentException("Can not parse input paramnetert 'chipNumber' to UInt16!");
      ushort offset1;
      if (!Util.TryParseToUInt16(offset, out offset1))
        throw new ArgumentException("Can not parse input paramnetert 'offset' to UInt16!");
      ushort size1;
      if (!Util.TryParseToUInt16(size, out size1))
        throw new ArgumentException("Can not parse input paramnetert 'size' to UInt16!");
      return this.GetEeprom(chipNumber1, offset1, size1);
    }

    public EepromBlock GetEeprom(ushort chipNumber, ushort offset, ushort size)
    {
      string str = string.Format("GetEeprom( Chip: {0}, Offset: {1}, Size: {2} )", (object) chipNumber, (object) offset, (object) size);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.Eeprom);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(chipNumber));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(offset));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(size));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) 0));
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      int timeOffsetPerBlock = this.connection.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      SCGiFrame scGiFrame = (SCGiFrame) null;
      try
      {
        scGiFrame = this.SendAndReceiveFrame(header, payload, str);
        if (scGiFrame == null || scGiFrame.Count == 0 || scGiFrame[0].Payload.Length == 10)
          return (EepromBlock) null;
      }
      finally
      {
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
      EepromBlock eeprom = new EepromBlock();
      eeprom.ChipNumber = BitConverter.ToUInt16(scGiFrame[0].Payload, 2);
      eeprom.Offset = BitConverter.ToUInt16(scGiFrame[0].Payload, 4);
      ushort uint16 = BitConverter.ToUInt16(scGiFrame[0].Payload, 6);
      for (int index = 0; index < scGiFrame.Count; ++index)
      {
        if (index == 0)
        {
          byte[] numArray = new byte[scGiFrame[index].Payload.Length - 10];
          Buffer.BlockCopy((Array) scGiFrame[index].Payload, 10, (Array) numArray, 0, numArray.Length);
          eeprom.AddRange((IEnumerable<byte>) numArray);
        }
        else
          eeprom.AddRange((IEnumerable<byte>) scGiFrame[index].Payload);
      }
      if ((int) uint16 != eeprom.Count)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong size of EEPROM block! Expected: {0},  Actual: {1}, EEPROM: {2}", (object) uint16, (object) eeprom.Count, (object) Util.ByteArrayToHexString(eeprom.ToArray())));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return eeprom;
    }

    public bool SetLED(object red, object yellow, object green1, object green2)
    {
      if (red == null)
        throw new ArgumentNullException("Input parameter 'red' can not be null!");
      if (yellow == null)
        throw new ArgumentNullException("Input parameter 'yellow' can not be null!");
      if (green1 == null)
        throw new ArgumentNullException("Input parameter 'green1' can not be null!");
      if (green2 == null)
        throw new ArgumentNullException("Input parameter 'green2' can not be null!");
      return this.SetLED(Convert.ToBoolean(red), Convert.ToBoolean(yellow), Convert.ToBoolean(green1), Convert.ToBoolean(green2));
    }

    public bool SetLED(bool red, bool yellow, bool green1, bool green2)
    {
      LED led = LED.None;
      if (red)
        led |= LED.D1_Red;
      if (yellow)
        led |= LED.D2_Yellow;
      if (green1)
        led |= LED.D3_Green;
      if (green2)
        led |= LED.D4_Green;
      return this.SetLED(led);
    }

    public bool SetLED(LED led)
    {
      string str = string.Format("SetLED( LED: {0})", (object) led);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.LED);
      payload.Add((byte) led);
      payload.Add((byte) 0);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Minol, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length == 4)
        return (LED) onlyOnePacket.Payload[2] == led;
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return false;
    }

    public string GetUserappName()
    {
      string str = "GetUserappName()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Firmware, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.UserappName), str);
      return onlyOnePacket == null ? (string) null : Encoding.ASCII.GetString(onlyOnePacket.Payload, 2, onlyOnePacket.Payload.Length - 2).TrimEnd(new char[1]);
    }

    public string GetFirmwareVersion()
    {
      string str = "GetFirmwareVersion()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Firmware, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.FirmwareVersion), str);
      return onlyOnePacket == null ? (string) null : Encoding.ASCII.GetString(onlyOnePacket.Payload, 2, onlyOnePacket.Payload.Length - 2).TrimEnd(new char[1]);
    }

    public string GetFirmwareBuildTime()
    {
      string str = "GetFirmwareBuildTime()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Firmware, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.FirmwareBuildTime), str);
      return onlyOnePacket == null ? (string) null : Encoding.ASCII.GetString(onlyOnePacket.Payload, 2, onlyOnePacket.Payload.Length - 2).TrimEnd(new char[1]);
    }

    public string GetUserappBuildTime()
    {
      string str = "GetUserappBuildTime()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.Firmware, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.UserappBuildTime), str);
      return onlyOnePacket == null ? (string) null : Encoding.ASCII.GetString(onlyOnePacket.Payload, 2, onlyOnePacket.Payload.Length - 2).TrimEnd(new char[1]);
    }

    public ushort? GetNodeId()
    {
      string str = "GetNodeId()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.NodeId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new ushort?();
      }
      return new ushort?(BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0));
    }

    public bool SetNodeId(object newNodeId)
    {
      if (newNodeId == null)
        throw new ArgumentNullException("Input parameter 'newNodeId' can not be null!");
      return newNodeId is ushort newNodeId1 || Util.TryParseToUInt16(newNodeId.ToString(), out newNodeId1) ? this.SetNodeId(newNodeId1) : throw new ArgumentException("Can not parse input paramnetert 'newNodeID' to UInt16!");
    }

    public bool SetNodeId(string newNodeId)
    {
      if (newNodeId == null)
        throw new ArgumentNullException("Input parameter 'newNodeId' can not be null!");
      ushort newNodeId1;
      if (!Util.TryParseToUInt16(newNodeId, out newNodeId1))
        throw new ArgumentException("Can not parse input paramnetert 'newNodeID' to UInt16!");
      return this.SetNodeId(newNodeId1);
    }

    public bool SetNodeId(ushort newNodeId)
    {
      string str = string.Format("SetNodeId( {0} )", (object) newNodeId);
      MinomatV4.logger.Info(str);
      if (newNodeId == (ushort) 0 || newNodeId == ushort.MaxValue)
        return false;
      List<byte> payload = this.CreatePayload(SCGiCommand.NodeId, (uint) newNodeId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      return (int) BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0) == (int) newNodeId;
    }

    public ushort? GetNetworkId()
    {
      string str = "GetNetworkId()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.NetworkId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new ushort?();
      }
      return new ushort?(BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0));
    }

    public bool SetNetworkId(object newNetworkId)
    {
      if (newNetworkId == null)
        throw new ArgumentNullException("Input parameter 'newNetworkId' can not be null!");
      return newNetworkId is ushort newNetworkId1 || Util.TryParseToUInt16(newNetworkId.ToString(), out newNetworkId1) ? this.SetNetworkId(newNetworkId1) : throw new ArgumentException("Can not parse input paramnetert 'newNetworkId' to UInt16!");
    }

    public bool SetNetworkId(string newNetworkId)
    {
      if (newNetworkId == null)
        throw new ArgumentNullException("Input parameter 'newNetworkId' can not be null!");
      ushort newNetworkId1;
      if (!Util.TryParseToUInt16(newNetworkId, out newNetworkId1))
        throw new ArgumentException("Can not parse input paramnetert 'newNetworkId' to UInt16!");
      return this.SetNetworkId(newNetworkId1);
    }

    public bool SetNetworkId(ushort newNetworkId)
    {
      string str = string.Format("SetNetworkId( {0} )", (object) newNetworkId);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.NetworkId, (uint) newNetworkId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      return (int) BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0) == (int) newNetworkId;
    }

    public uint? GetErrorFlags()
    {
      string str = "GetErrorFlags()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ErrorFlags);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new uint?();
      if (onlyOnePacket.Payload.Length == 6)
        return new uint?(BitConverter.ToUInt32(onlyOnePacket.Payload, 2));
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new uint?();
    }

    public ushort? GetTransmissionPower()
    {
      string str = "GetTransmissionPower()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.TransmissionPower);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length == 4)
        return new ushort?(BitConverter.ToUInt16(onlyOnePacket.Payload, 2));
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new ushort?();
    }

    public bool SetTransmissionPower(object newTransmissionPower)
    {
      if (newTransmissionPower == null)
        throw new ArgumentNullException("Input parameter 'newTransmissionPower' can not be null!");
      return newTransmissionPower is ushort newTransmissionPower1 || Util.TryParseToUInt16(newTransmissionPower.ToString(), out newTransmissionPower1) ? this.SetTransmissionPower(newTransmissionPower1) : throw new ArgumentException("Can not parse input paramnetert 'newTransmissionPower' to UInt16!");
    }

    public bool SetTransmissionPower(string newTransmissionPower)
    {
      if (newTransmissionPower == null)
        throw new ArgumentNullException("Input parameter 'newTransmissionPower' can not be null!");
      ushort newTransmissionPower1;
      if (!Util.TryParseToUInt16(newTransmissionPower, out newTransmissionPower1))
        throw new ArgumentException("Can not parse input parameter 'newTransmissionPower' to UInt16!");
      return this.SetTransmissionPower(newTransmissionPower1);
    }

    public bool SetTransmissionPower(ushort newTransmissionPower)
    {
      string str = string.Format("SetTransmissionPower( {0} )", (object) newTransmissionPower);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.TransmissionPower, (uint) newTransmissionPower);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      return (int) BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0) == (int) newTransmissionPower;
    }

    public ushort? GetTransceiverChannelId()
    {
      string str = "GetTransceiverChannelId()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.TransceiverChannelId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new ushort?();
      }
      return new ushort?(BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0));
    }

    public bool SetTransceiverChannelId(object newTransceiverChannelId)
    {
      if (newTransceiverChannelId == null)
        throw new ArgumentNullException("Input parameter 'newTransceiverChannelId' can not be null!");
      return newTransceiverChannelId is ushort newTransceiverChannelId1 || Util.TryParseToUInt16(newTransceiverChannelId.ToString(), out newTransceiverChannelId1) ? this.SetTransceiverChannelId(newTransceiverChannelId1) : throw new ArgumentException("Can not parse input paramnetert 'newTransceiverChannelId' to UInt16!");
    }

    public bool SetTransceiverChannelId(string newTransceiverChannelId)
    {
      if (newTransceiverChannelId == null)
        throw new ArgumentNullException("Input parameter 'newTransceiverChannelId' can not be null!");
      ushort newTransceiverChannelId1;
      if (!Util.TryParseToUInt16(newTransceiverChannelId, out newTransceiverChannelId1))
        throw new ArgumentException("Can not parse input paramnetert 'newTransceiverChannelId' to UInt16!");
      return this.SetTransceiverChannelId(newTransceiverChannelId1);
    }

    public bool SetTransceiverChannelId(ushort newTransceiverChannelId)
    {
      string str = string.Format("SetTransceiverChannelId( {0} )", (object) newTransceiverChannelId);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.TransceiverChannelId, (uint) newTransceiverChannelId);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      return (int) BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0) == (int) newTransceiverChannelId;
    }

    public MultiChannelSettings GetMultiChannelSettings()
    {
      string str = "GetMultiChannelSettings()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MultiChannelSettings);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return (MultiChannelSettings) null;
      if (onlyOnePacket.Payload.Length == 6)
        return MultiChannelSettings.Parse(onlyOnePacket.Payload);
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return (MultiChannelSettings) null;
    }

    public bool SetMultiChannelSettings(
      object MFChannel0,
      object MFChannel1,
      object MFChannel2,
      object MFChannel3)
    {
      if (MFChannel0 == null)
        throw new ArgumentNullException("Input parameter 'MFChannel0' can not be null!");
      if (MFChannel1 == null)
        throw new ArgumentNullException("Input parameter 'MFChannel1' can not be null!");
      if (MFChannel2 == null)
        throw new ArgumentNullException("Input parameter 'MFChannel2' can not be null!");
      if (MFChannel3 == null)
        throw new ArgumentNullException("Input parameter 'MFChannel3' can not be null!");
      return this.SetMultiChannelSettings(MFChannel0.ToString(), MFChannel1.ToString(), MFChannel2.ToString(), MFChannel3.ToString());
    }

    public bool SetMultiChannelSettings(
      string MFChannel0,
      string MFChannel1,
      string MFChannel2,
      string MFChannel3)
    {
      if (string.IsNullOrEmpty(MFChannel0))
        throw new ArgumentNullException("Input parameter 'MFChannel0' can not be null!");
      if (string.IsNullOrEmpty(MFChannel1))
        throw new ArgumentNullException("Input parameter 'MFChannel1' can not be null!");
      if (string.IsNullOrEmpty(MFChannel2))
        throw new ArgumentNullException("Input parameter 'MFChannel2' can not be null!");
      if (string.IsNullOrEmpty(MFChannel3))
        throw new ArgumentNullException("Input parameter 'MFChannel3' can not be null!");
      byte MFChannel0_1;
      if (!Util.TryParseToByte(MFChannel0, out MFChannel0_1))
        throw new ArgumentException("Can not parse input paramnetert 'MFChannel0' to Byte!");
      byte MFChannel1_1;
      if (!Util.TryParseToByte(MFChannel1, out MFChannel1_1))
        throw new ArgumentException("Can not parse input paramnetert 'MFChannel1' to Byte!");
      byte MFChannel2_1;
      if (!Util.TryParseToByte(MFChannel2, out MFChannel2_1))
        throw new ArgumentException("Can not parse input paramnetert 'MFChannel2' to Byte!");
      byte MFChannel3_1;
      if (!Util.TryParseToByte(MFChannel3, out MFChannel3_1))
        throw new ArgumentException("Can not parse input paramnetert 'MFChannel3' to Byte!");
      return this.SetMultiChannelSettings(MFChannel0_1, MFChannel1_1, MFChannel2_1, MFChannel3_1);
    }

    public bool SetMultiChannelSettings(
      byte MFChannel0,
      byte MFChannel1,
      byte MFChannel2,
      byte MFChannel3)
    {
      return this.SetMultiChannelSettings(new MultiChannelSettings()
      {
        MFChannel0 = MFChannel0,
        MFChannel1 = MFChannel1,
        MFChannel2 = MFChannel2,
        MFChannel3 = MFChannel3
      });
    }

    public bool SetMultiChannelSettings(MultiChannelSettings newMultiChannelSettings)
    {
      string str = newMultiChannelSettings != null ? string.Format("SetMultiChannelSettings( {0} )", (object) newMultiChannelSettings) : throw new ArgumentNullException("MultiChannelSettings can not be null!");
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MultiChannelSettings);
      payload.Add(newMultiChannelSettings.MFChannel0);
      payload.Add(newMultiChannelSettings.MFChannel1);
      payload.Add(newMultiChannelSettings.MFChannel2);
      payload.Add(newMultiChannelSettings.MFChannel3);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 6)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      MultiChannelSettings multiChannelSettings = MultiChannelSettings.Parse(onlyOnePacket.Payload);
      return (int) multiChannelSettings.MFChannel0 == (int) newMultiChannelSettings.MFChannel0 && (int) multiChannelSettings.MFChannel1 == (int) newMultiChannelSettings.MFChannel1 && (int) multiChannelSettings.MFChannel2 == (int) newMultiChannelSettings.MFChannel2 && (int) multiChannelSettings.MFChannel3 == (int) newMultiChannelSettings.MFChannel3;
    }

    public ushort? GetTransceiverFrequencyOffset()
    {
      string str = "GetTransceiverFrequencyOffset()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.TransceiverFrequencyOffset);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new ushort?();
      }
      return new ushort?(BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0));
    }

    public bool SetTransceiverFrequencyOffset(object newTransceiverFrequencyOffset)
    {
      return newTransceiverFrequencyOffset != null ? this.SetTransceiverFrequencyOffset(newTransceiverFrequencyOffset.ToString()) : throw new ArgumentNullException("Input parameter 'newTransceiverFrequencyOffset' can not be null!");
    }

    public bool SetTransceiverFrequencyOffset(string newTransceiverFrequencyOffset)
    {
      if (newTransceiverFrequencyOffset == null)
        throw new ArgumentNullException("Input parameter 'newTransceiverFrequencyOffset' can not be null!");
      ushort newTransceiverFrequencyOffset1;
      if (!Util.TryParseToUInt16(newTransceiverFrequencyOffset, out newTransceiverFrequencyOffset1))
        throw new ArgumentException("Can not parse input paramnetert 'newTransceiverFrequencyOffset' to UInt16!");
      return this.SetTransceiverFrequencyOffset(newTransceiverFrequencyOffset1);
    }

    public bool SetTransceiverFrequencyOffset(ushort newTransceiverFrequencyOffset)
    {
      string str = string.Format("SetTransceiverFrequencyOffset( {0} )", (object) newTransceiverFrequencyOffset);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.TransceiverFrequencyOffset, (uint) newTransceiverFrequencyOffset);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      return (int) BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0) == (int) newTransceiverFrequencyOffset;
    }

    public DateTime? GetSystemTime()
    {
      string str = "GetSystemTime()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.SystemTime);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new DateTime?();
      if (onlyOnePacket.Payload.Length != 8)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 8, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new DateTime?();
      }
      uint uint32 = BitConverter.ToUInt32(onlyOnePacket.Payload, 2);
      ushort uint16 = BitConverter.ToUInt16(onlyOnePacket.Payload, 6);
      DateTime dateTime = this.TIMEPOINT0;
      if (uint32 > 0U)
        dateTime = dateTime.AddSeconds((double) uint32);
      if (uint16 > (ushort) 0)
        dateTime = dateTime.AddMilliseconds((double) uint16);
      return new DateTime?(dateTime);
    }

    public bool SetSystemTime(object newSystemTime)
    {
      if (newSystemTime == null)
        throw new ArgumentNullException("Input parameter 'newSystemTime' can not be null!");
      return newSystemTime is DateTime newSystemTime1 || Util.TryParseToDateTime(newSystemTime.ToString(), out newSystemTime1) ? this.SetSystemTime(newSystemTime1) : throw new ArgumentException("Can not parse input paramnetert 'newSystemTime' to DateTime!");
    }

    public bool SetSystemTime(string newSystemTime)
    {
      if (newSystemTime == null)
        throw new ArgumentNullException("Input parameter 'newSystemTime' can not be null!");
      DateTime newSystemTime1;
      if (!Util.TryParseToDateTime(newSystemTime, out newSystemTime1))
        throw new ArgumentException("Can not parse input paramnetert 'newSystemTime' to DateTime!");
      return this.SetSystemTime(newSystemTime1);
    }

    public bool SetSystemTime(DateTime newSystemTime)
    {
      string str = string.Format("SetSystemTime( {0} )", (object) newSystemTime);
      MinomatV4.logger.Info(str);
      if (newSystemTime < this.TIMEPOINT0)
      {
        MinomatV4.logger.Error("Wrong input parameter systenTime! System time can not be less as " + this.TIMEPOINT0.ToString());
        return false;
      }
      TimeSpan timeSpan = newSystemTime - this.TIMEPOINT0;
      uint uint32_1 = Convert.ToUInt32(timeSpan.TotalSeconds);
      uint uint16_1 = (uint) Convert.ToUInt16(timeSpan.Milliseconds);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.SystemTime);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(uint32_1));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(uint16_1));
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 8)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 8, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      uint uint32_2 = BitConverter.ToUInt32(new byte[4]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3],
        onlyOnePacket.Payload[4],
        onlyOnePacket.Payload[5]
      }, 0);
      ushort uint16_2 = BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[6],
        onlyOnePacket.Payload[7]
      }, 0);
      DateTime dateTime = this.TIMEPOINT0;
      if (uint32_2 > 0U)
        dateTime = dateTime.AddSeconds((double) uint32_1);
      if (uint16_2 > (ushort) 0)
        dateTime = dateTime.AddMilliseconds((double) uint16_1);
      return (dateTime - newSystemTime).TotalSeconds < 60.0;
    }

    public ushort? GetTemperatureOffset()
    {
      string str = "GetTemperatureOffset()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.TemperatureOffset);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new ushort?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return new ushort?();
      }
      return new ushort?(BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0));
    }

    public bool SetTemperatureOffset(object newTemperatureOffset)
    {
      return newTemperatureOffset != null ? this.SetTemperatureOffset(newTemperatureOffset.ToString()) : throw new ArgumentNullException("Input parameter 'newTemperatureOffset' can not be null!");
    }

    public bool SetTemperatureOffset(string newTemperatureOffset)
    {
      if (newTemperatureOffset == null)
        throw new ArgumentNullException("Input parameter 'newTemperatureOffset' can not be null!");
      ushort newTemperatureOffset1;
      if (!Util.TryParseToUInt16(newTemperatureOffset, out newTemperatureOffset1))
        throw new ArgumentException("Can not parse input paramnetert 'newTemperatureOffset' to UInt16!");
      return this.SetTemperatureOffset(newTemperatureOffset1);
    }

    public bool SetTemperatureOffset(ushort newTemperatureOffset)
    {
      string str = string.Format("SetTemperatureOffset( {0} )", (object) newTemperatureOffset);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.TemperatureOffset, (uint) newTemperatureOffset);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.Firmware, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
        return false;
      }
      return (int) BitConverter.ToUInt16(new byte[2]
      {
        onlyOnePacket.Payload[2],
        onlyOnePacket.Payload[3]
      }, 0) == (int) newTemperatureOffset;
    }

    public bool RestartMinomat()
    {
      MinomatV4.logger.Info("RestartMinomat()");
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.RestartMinomat);
      payload.AddRange((IEnumerable<byte>) new byte[4]
      {
        (byte) 211,
        (byte) 145,
        (byte) 211,
        (byte) 145
      });
      return this.SendPacket(new SCGiHeader(SCGiMessageType.Firmware, this.Authentication), payload);
    }

    public RoutingTable GetRoutingTable()
    {
      string str = "GetRoutingTable()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.RoutingTable);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.LPSR, this.Authentication);
      int timeOffsetPerBlock = this.connection.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      SCGiFrame scGiFrame = (SCGiFrame) null;
      try
      {
        scGiFrame = this.SendAndReceiveFrame(header, payload, str);
        if (scGiFrame == null || scGiFrame.Count == 0)
          return (RoutingTable) null;
      }
      finally
      {
        this.connection.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
      RoutingTable routingTable = new RoutingTable();
      routingTable.StateOfLPRS = StateDataOfLPRS.Parse(scGiFrame[0].Payload);
      for (int index = 1; index < scGiFrame.Count; ++index)
      {
        RoutingRow routingRow = RoutingRow.Parse(scGiFrame[index].Payload);
        if (routingRow != null)
          routingTable.Add(routingRow);
      }
      return routingTable;
    }

    public bool SendSMS(string phoneNumber, string message)
    {
      if (phoneNumber == null)
        throw new ArgumentNullException("Input parameter 'phoneNumber' can not be null!");
      if (phoneNumber.Length > (int) byte.MaxValue)
        throw new ArgumentNullException("Phone number can not be > Byte.MaxValue!");
      if (message == null)
        throw new ArgumentNullException("Input parameter 'message' can not be null!");
      if (message.Length > 160)
        throw new ArgumentNullException("SMS mesage can not be > 160!");
      MinomatV4.logger.Info<string, string>("SendSMS( Phone: {0}, Message: {1} )", phoneNumber, message);
      List<byte> collection1 = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(phoneNumber));
      List<byte> collection2 = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(message));
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.SendSMS);
      payload.Add((byte) phoneNumber.Length);
      payload.AddRange((IEnumerable<byte>) collection1);
      payload.AddRange((IEnumerable<byte>) collection2);
      return this.SendPacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload);
    }

    public uint? GetMasterMinolIDOfModem()
    {
      string str = "GetMasterMinolIDOfModem()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.MasterMinolID);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new uint?();
      if (onlyOnePacket.Payload.Length == 6)
        return new uint?(BitConverter.ToUInt32(onlyOnePacket.Payload, 2));
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return new uint?();
    }

    public bool SetMasterMinolIDOfModem(object id)
    {
      if (id == null)
        throw new ArgumentNullException("Input parameter 'id' can not be null!");
      return id is uint id1 || Util.TryParseToUInt32(id.ToString(), out id1) ? this.SetMasterMinolIDOfModem(id1) : throw new ArgumentException("Can not parse input parameter 'id' to UInt32!");
    }

    public bool SetMasterMinolIDOfModem(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentNullException("Input parameter 'id' can not be null!");
      uint id1;
      if (!Util.TryParseToUInt32(id, out id1))
        throw new ArgumentException("Can not parse input parameter 'id' to UInt32!");
      return this.SetMasterMinolIDOfModem(id1);
    }

    public bool SetMasterMinolIDOfModem(uint id)
    {
      string str = string.Format("SetMasterMinolIDOfModem(ID: {0})", (object) id);
      MinomatV4.logger.Info(str);
      List<byte> payload = this.CreatePayload(SCGiCommand.MasterMinolID, id);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length == 6)
        return (int) BitConverter.ToUInt32(onlyOnePacket.Payload, 2) == (int) id;
      MinomatV4.logger.Error<int, SCGiHeader, string>("Wrong length of SCGi payload! Actual: {0}, Expected: 6, SCGi header: {1}, Payload buffer: {2}", onlyOnePacket.Payload.Length, header, Util.ByteArrayToHexString(onlyOnePacket.Payload));
      return false;
    }

    public bool StartHttpConnection()
    {
      string str = "StartHttpConnection()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.StartHttpConnection);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length < 1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: >= 2, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (int) onlyOnePacket.Payload[0] == (int) payload[0];
    }

    public bool ActivateModemAT_Mode()
    {
      string str = "ActivateModemAT_Mode()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ActivateModemAT_Mode);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length < 1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: >= 2, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (int) onlyOnePacket.Payload[0] == (int) payload[0];
    }

    public bool StartGSMTestReception()
    {
      string str = "StartGSMTestReception()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.StartGSMTestReception);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length < 1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: >= 2, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (int) onlyOnePacket.Payload[0] == (int) payload[0];
    }

    public AppInitialSettings GetAppInitialSettings()
    {
      string str = "GetAppInitialSettings()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.AppInitialSettings), str);
      if (onlyOnePacket == null)
        return (AppInitialSettings) null;
      if (onlyOnePacket.Payload.Length == 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.MissingMD5Parameter, "Missing MD5 parameter");
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return AppInitialSettings.Parse(onlyOnePacket.Payload);
    }

    public bool SetAppInitialSettings(
      object challenge,
      object gsmId,
      object sapConfigNumber,
      object MD5)
    {
      if (challenge == null)
        throw new ArgumentNullException("Input parameter 'challenge' can not be null!");
      if (gsmId == null)
        throw new ArgumentNullException("Input parameter 'gsmId' can not be null!");
      if (sapConfigNumber == null)
        throw new ArgumentNullException("Input parameter 'sapConfigNumber' can not be null!");
      string MD5_1 = string.Empty;
      if (MD5 != null)
        MD5_1 = !(MD5 is byte[]) ? MD5.ToString() : Util.ByteArrayToHexString((byte[]) MD5);
      return this.SetAppInitialSettings(challenge.ToString(), gsmId.ToString(), sapConfigNumber.ToString(), MD5_1);
    }

    public bool SetAppInitialSettings(
      string challenge,
      string gsmId,
      string sapConfigNumber,
      string MD5)
    {
      if (string.IsNullOrEmpty(challenge))
        throw new ArgumentNullException("Input parameter 'challenge' can not be null!");
      if (string.IsNullOrEmpty(gsmId))
        throw new ArgumentNullException("Input parameter 'gsmId' can not be null!");
      if (string.IsNullOrEmpty(sapConfigNumber))
        throw new ArgumentNullException("Input parameter 'sapConfigNumber' can not be null!");
      uint challenge1;
      if (!Util.TryParseToUInt32(challenge, out challenge1))
        throw new ArgumentException("Can not parse input paramnetert 'challenge' to UInt32!");
      uint gsmId1;
      if (!Util.TryParseToUInt32(gsmId, out gsmId1))
        throw new ArgumentException("Can not parse input paramnetert 'gsmId' to UInt32!");
      uint sapConfigNumber1;
      if (!Util.TryParseToUInt32(sapConfigNumber, out sapConfigNumber1))
        throw new ArgumentException("Can not parse input paramnetert 'sapConfigNumber' to UInt32!");
      byte[] MD5_1 = (byte[]) null;
      if (!string.IsNullOrEmpty(MD5))
        MD5_1 = Util.HexStringToByteArray(MD5);
      return this.SetAppInitialSettings(challenge1, gsmId1, sapConfigNumber1, MD5_1);
    }

    public bool SetAppInitialSettings(
      uint challenge,
      uint gsmId,
      uint sapConfigNumber,
      byte[] MD5)
    {
      return this.SetAppInitialSettings(new AppInitialSettings()
      {
        Challenge = challenge,
        GsmId = gsmId,
        SapConfigNumber = sapConfigNumber,
        MD5 = MD5
      });
    }

    public bool SetAppInitialSettings(AppInitialSettings appInitialSettings)
    {
      string str = appInitialSettings != null ? string.Format("SetAppInitialSettings( {0} )", (object) appInitialSettings) : throw new ArgumentNullException("AppInitialSettings can not be null!");
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.AppInitialSettings);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(appInitialSettings.Challenge));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(appInitialSettings.GsmId));
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(appInitialSettings.SapConfigNumber));
      if (appInitialSettings.MD5 != null)
        payload.AddRange((IEnumerable<byte>) appInitialSettings.MD5);
      payload.Add((byte) 0);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length == 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.MissingMD5Parameter, "Missing MD5 parameter");
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      AppInitialSettings appInitialSettings1 = AppInitialSettings.Parse(onlyOnePacket.Payload);
      return appInitialSettings1 != null && (int) appInitialSettings1.Challenge == (int) appInitialSettings.Challenge && (int) appInitialSettings1.GsmId == (int) appInitialSettings.GsmId && (int) appInitialSettings1.SapConfigNumber == (int) appInitialSettings.SapConfigNumber;
    }

    public GsmState GetGsmState()
    {
      string str = "GetGsmState()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.GsmState);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return (GsmState) null;
      if (onlyOnePacket.Payload.Length < 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: 4, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return GsmState.Parse(onlyOnePacket.Payload);
    }

    public HttpState GetHttpState()
    {
      string str = "GetHttpState()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.HttpState);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return (HttpState) null;
      if (onlyOnePacket.Payload.Length < 5)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: > 5, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return HttpState.Parse(onlyOnePacket.Payload);
    }

    public byte? GetGsmLinkQuality()
    {
      string str = "GetGsmLinkQuality()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.GsmLinkQuality);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return new byte?();
      if (onlyOnePacket.Payload.Length < 1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: >=1, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return new byte?(onlyOnePacket.Payload[1]);
    }

    public GSMTestReceptionState GetGSMTestReceptionState()
    {
      string str = "GetGSMTestReceptionState()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.GSMTestReceptionState);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return GSMTestReceptionState.Failed;
      if (onlyOnePacket.Payload.Length < 1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: >=1, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (GSMTestReceptionState) Enum.ToObject(typeof (GSMTestReceptionState), onlyOnePacket.Payload[1]);
    }

    public string GetSimPin()
    {
      string str = "GetSimPin()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.SimPin), str);
      if (onlyOnePacket == null)
        return (string) null;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() : string.Empty;
    }

    public bool SetSimPin(object newSimPim)
    {
      return newSimPim != null ? this.SetSimPin(newSimPim.ToString()) : throw new ArgumentNullException("Input parameter 'newSimPim' can not be null!");
    }

    public bool SetSimPin(string newSimPim)
    {
      if (newSimPim == null)
        throw new ArgumentNullException("SIM PIN can not be null!");
      string str = newSimPim.Length <= 12 ? string.Format("SetSimPin( {0} )", (object) newSimPim) : throw new ArgumentNullException("SIM PIN length can not be > 12!");
      MinomatV4.logger.Info(str);
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newSimPim));
      collection.Add((byte) 0);
      if (newSimPim.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.SimPin);
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() == newSimPim : newSimPim == string.Empty;
    }

    public string GetAPN()
    {
      string str = "GetAPN()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.APN), str);
      return onlyOnePacket == null ? (string) null : EndPoint.Parse(onlyOnePacket.Payload, 1).Servername;
    }

    public bool SetAPN(object newAPN)
    {
      return newAPN != null ? this.SetAPN(newAPN.ToString()) : throw new ArgumentNullException("Input parameter 'newAPN' can not be null!");
    }

    public bool SetAPN(string newAPN)
    {
      if (newAPN == null)
        throw new ArgumentNullException("APN can not be null!");
      string str = newAPN.Length <= 50 ? string.Format("SetAPN( {0} )", (object) newAPN) : throw new ArgumentNullException("Servername length can not be > 50!");
      MinomatV4.logger.Info(str);
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newAPN));
      collection.Add((byte) 0);
      if (newAPN.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.APN);
      payload.Add((byte) 0);
      payload.Add((byte) 0);
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      return onlyOnePacket != null && EndPoint.Parse(onlyOnePacket.Payload, 1).Servername == newAPN;
    }

    public string GetGPRSUserName()
    {
      string str = "GetGPRSUserName()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.GPRSUserName), str);
      if (onlyOnePacket == null)
        return (string) null;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() : string.Empty;
    }

    public bool SetGPRSUserName(object newGPRSUserName)
    {
      return newGPRSUserName != null ? this.SetGPRSUserName(newGPRSUserName.ToString()) : throw new ArgumentNullException("Input parameter 'newGPRSUserName' can not be null!");
    }

    public bool SetGPRSUserName(string newGPRSUserName)
    {
      if (newGPRSUserName == null)
        throw new ArgumentNullException("GPRS User Name can not be null!");
      string str = newGPRSUserName.Length <= 52 ? string.Format("SetGPRSUserName( {0} )", (object) newGPRSUserName) : throw new ArgumentNullException("GPRS User Name length can not be > 52!");
      MinomatV4.logger.Info(str);
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newGPRSUserName));
      collection.Add((byte) 0);
      if (newGPRSUserName.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.GPRSUserName);
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() == newGPRSUserName : newGPRSUserName == string.Empty;
    }

    public string GetGPRSPassword()
    {
      string str = "GetGPRSPassword()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.GPRSPassword), str);
      if (onlyOnePacket == null)
        return (string) null;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() : string.Empty;
    }

    public bool SetGPRSPassword(object newGPRSPassword)
    {
      return newGPRSPassword != null ? this.SetGPRSPassword(newGPRSPassword.ToString()) : throw new ArgumentNullException("Input parameter 'newGPRSPassword' can not be null!");
    }

    public bool SetGPRSPassword(string newGPRSPassword)
    {
      if (newGPRSPassword == null)
        throw new ArgumentNullException("GPRS Password can not be null!");
      string str = newGPRSPassword.Length <= 22 ? string.Format("SetGPRSPassword( {0} )", (object) newGPRSPassword) : throw new ArgumentNullException("GPRS Password length can not be > 22!");
      MinomatV4.logger.Info(str);
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newGPRSPassword));
      collection.Add((byte) 0);
      if (newGPRSPassword.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.GPRSPassword);
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() == newGPRSPassword : newGPRSPassword == string.Empty;
    }

    public EndPoint GetTcpConfiguration()
    {
      string str = "GetTcpConfiguration()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.TcpConfiguration), str);
      return onlyOnePacket == null ? (EndPoint) null : EndPoint.Parse(onlyOnePacket.Payload, 1);
    }

    public bool SetTcpConfiguration(object port, object servername)
    {
      if (port == null)
        throw new ArgumentNullException("Input parameter 'port' can not be null!");
      return servername != null ? this.SetTcpConfiguration(port.ToString(), servername.ToString()) : throw new ArgumentNullException("Input parameter 'servername' can not be null!");
    }

    public bool SetTcpConfiguration(string port, string servername)
    {
      if (port == null)
        throw new ArgumentNullException("Input parameter 'port' can not be null!");
      if (servername == null)
        throw new ArgumentNullException("Input parameter 'servername' can not be null!");
      ushort port1;
      if (!Util.TryParseToUInt16(port, out port1))
        throw new ArgumentException("Can not parse input paramnetert 'port' to UInt16!");
      return this.SetTcpConfiguration(port1, servername);
    }

    public bool SetTcpConfiguration(ushort port, string servername)
    {
      return this.SetTcpConfiguration(new EndPoint()
      {
        Port = port,
        Servername = servername
      });
    }

    public bool SetTcpConfiguration(EndPoint newServer)
    {
      string str = newServer != null ? string.Format("SetTcpConfiguration( {0} )", (object) newServer) : throw new ArgumentNullException("Input parameter 'newServer' can not be null!");
      MinomatV4.logger.Info(str);
      if (newServer.Servername == null)
        throw new ArgumentNullException("Servername can not be null!");
      if (newServer.Servername.Length > 64)
        throw new ArgumentNullException("Servername length can not be > 64!");
      if (newServer.Port < (ushort) 0)
        throw new ArgumentNullException("Port can not be < 0!");
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newServer.Servername));
      collection.Add((byte) 0);
      if (newServer.Servername.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.TcpConfiguration);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(newServer.Port));
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      EndPoint endPoint = EndPoint.Parse(onlyOnePacket.Payload, 1);
      return endPoint != null && (int) newServer.Port == (int) endPoint.Port && newServer.Servername == endPoint.Servername;
    }

    public EndPoint GetHttpServer()
    {
      string str = "GetHttpServer()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.HttpServer), str);
      return onlyOnePacket == null ? (EndPoint) null : EndPoint.Parse(onlyOnePacket.Payload, 1);
    }

    public bool SetHttpServer(object port, object servername)
    {
      if (port == null)
        throw new ArgumentNullException("Input parameter 'port' can not be null!");
      return servername != null ? this.SetHttpServer(port.ToString(), servername.ToString()) : throw new ArgumentNullException("Input parameter 'servername' can not be null!");
    }

    public bool SetHttpServer(string port, string servername)
    {
      if (port == null)
        throw new ArgumentNullException("Input parameter 'port' can not be null!");
      if (servername == null)
        throw new ArgumentNullException("Input parameter 'servername' can not be null!");
      ushort port1;
      if (!Util.TryParseToUInt16(port, out port1))
        throw new ArgumentException("Can not parse input paramnetert 'port' to UInt16!");
      return this.SetHttpServer(port1, servername);
    }

    public bool SetHttpServer(ushort port, string servername)
    {
      return this.SetHttpServer(new EndPoint()
      {
        Port = port,
        Servername = servername
      });
    }

    public bool SetHttpServer(EndPoint newHttpServer)
    {
      string str = newHttpServer != null ? string.Format("SetHttpServer( {0} )", (object) newHttpServer) : throw new ArgumentNullException("HttpServer can not be null!");
      MinomatV4.logger.Info(str);
      if (newHttpServer.Servername == null)
        throw new ArgumentNullException("Servername can not be null!");
      if (newHttpServer.Servername.Length > 50)
        throw new ArgumentNullException("Servername length can not be > 50!");
      if (newHttpServer.Port < (ushort) 0)
        throw new ArgumentNullException("Port can not be < 0!");
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newHttpServer.Servername));
      collection.Add((byte) 0);
      if (newHttpServer.Servername.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.HttpServer);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(newHttpServer.Port));
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      EndPoint endPoint = EndPoint.Parse(onlyOnePacket.Payload, 1);
      return endPoint != null && (int) newHttpServer.Port == (int) endPoint.Port && newHttpServer.Servername == endPoint.Servername;
    }

    public string GetHttpResourceName()
    {
      string str = "GetHttpResourceName()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.HttpResourceName), str);
      if (onlyOnePacket == null)
        return (string) null;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() : string.Empty;
    }

    public bool SetHttpResourceName(object newHttpResourceName)
    {
      return newHttpResourceName != null ? this.SetHttpResourceName(newHttpResourceName.ToString()) : throw new ArgumentNullException("Input parameter 'newHttpResourceName' can not be null!");
    }

    public bool SetHttpResourceName(string newHttpResourceName)
    {
      if (newHttpResourceName == null)
        throw new ArgumentNullException("HttpResourceName can not be null!");
      string str = newHttpResourceName.Length <= 120 ? string.Format("SetHttpResourceName( {0} )", (object) newHttpResourceName) : throw new ArgumentNullException("Servername length can not be > 120!");
      MinomatV4.logger.Info(str);
      List<byte> collection = new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(newHttpResourceName));
      collection.Add((byte) 0);
      if (newHttpResourceName.Length == 0)
        collection.Add((byte) 0);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.HttpResourceName);
      payload.AddRange((IEnumerable<byte>) collection);
      SCGiHeader header = new SCGiHeader(SCGiMessageType.GSM, this.Authentication);
      header.DestinationAddress = SCGiAddress.Modem;
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(header, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length < 1)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, string.Format("Wrong length of SCGi payload! Actual: {0}, Expected: >= 1, SCGi header: {1}, Payload buffer: {2}", (object) onlyOnePacket.Payload.Length, (object) header, (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() == newHttpResourceName;
    }

    public DateTime? GetModemDueDate()
    {
      string str = "GetModemDueDate()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemDueDate), str);
      if (onlyOnePacket == null)
        return new DateTime?();
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, "Unknown response received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      byte num1 = onlyOnePacket.Payload[1];
      byte num2 = onlyOnePacket.Payload[2];
      int day = (int) num2 & 31;
      int month = (int) num1 & 15;
      int num3 = ((int) num2 & 224) >> 5 | ((int) num1 & 240) >> 1;
      if (day == 0)
        day = 1;
      if (month == 0)
        month = 1;
      return new DateTime?(new DateTime(num3 != 0 ? 2000 + num3 : 2000, month, day));
    }

    public bool SetModemDueDate(object newDueDate)
    {
      if (newDueDate == null || newDueDate is string && newDueDate.ToString().Trim() == string.Empty)
        return this.SetModemDueDate(DateTime.MinValue);
      return newDueDate is DateTime newDueDate1 || Util.TryParseToDateTime(newDueDate.ToString(), out newDueDate1) ? this.SetModemDueDate(newDueDate1) : throw new ArgumentException("Can not parse input paramnetert 'newDueDate' to DateTime!");
    }

    public bool SetModemDueDate(string newDueDate)
    {
      if (string.IsNullOrEmpty(newDueDate))
        return this.SetModemDueDate(DateTime.MinValue);
      DateTime newDueDate1;
      if (!Util.TryParseToDateTime(newDueDate, out newDueDate1))
        throw new ArgumentException("Can not parse input paramnetert 'newDueDate' to DateTime!");
      return this.SetModemDueDate(newDueDate1);
    }

    public bool SetModemDueDate(DateTime newDueDate)
    {
      int num1 = 0;
      string str = string.Format("SetModemDueDate( {0} )", newDueDate == DateTime.MinValue ? (object) "0,0,0" : (object) newDueDate.ToShortDateString());
      MinomatV4.logger.Info(str);
      if (newDueDate != DateTime.MinValue)
      {
        DateTime dateTime = new DateTime(2000, 1, 1);
        if (newDueDate < dateTime)
        {
          MinomatV4.logger.Error("Wrong input parameter due date! Due date can not be less as 1/1/2000");
          return false;
        }
        int num2 = newDueDate.Year - 2000;
        int month = newDueDate.Month;
        num1 = newDueDate.Day | month << 8 | (num2 & 120) << 9 | (num2 & 7) << 5;
      }
      byte[] bytes = BitConverter.GetBytes((ushort) num1);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ModemDueDate, new byte[2]
      {
        bytes[1],
        bytes[0]
      });
      payload.Add((byte) 0);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 4)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, "Unknown response received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (int) onlyOnePacket.Payload[0] == (int) payload[0] && (int) onlyOnePacket.Payload[1] == (int) payload[1] && (int) onlyOnePacket.Payload[2] == (int) payload[2] && (int) onlyOnePacket.Payload[3] == (int) payload[3];
    }

    public bool SetActionTimepoint(object action, object type, object timepoint)
    {
      if (action == null)
        throw new ArgumentException("Input parameter 'action' can not be null!");
      if (type == null)
        throw new ArgumentException("Input parameter 'type' can not be null!");
      if (timepoint == null)
        throw new ArgumentException("Input parameter 'timepoint' can not be null!");
      return this.SetActionTimepoint(action.ToString(), type.ToString(), timepoint.ToString());
    }

    public bool SetActionTimepoint(string action, string type, string timepoint)
    {
      if (string.IsNullOrEmpty(action) || !Enum.IsDefined(typeof (ActionMode), (object) action))
        throw new ArgumentException("Invalid ActionMode argument!");
      if (string.IsNullOrEmpty(type) || !Enum.IsDefined(typeof (ActionTimepointType), (object) type))
        throw new ArgumentException("Invalid ActionTimepointType argument!");
      DateTime timepoint1;
      if (string.IsNullOrEmpty(timepoint) || !Util.TryParseToDateTime(timepoint, out timepoint1))
        throw new ArgumentException("Invalid Timepoint argument!");
      return this.SetActionTimepoint((ActionMode) Enum.Parse(typeof (ActionMode), action, true), (ActionTimepointType) Enum.Parse(typeof (ActionTimepointType), type, true), timepoint1);
    }

    public bool SetActionTimepoint(ActionMode action, ActionTimepointType type, DateTime timepoint)
    {
      return this.SetActionTimepoint(new ActionTimepoint()
      {
        Action = action,
        TimepointType = type,
        Timepoint = timepoint
      });
    }

    public bool SetActionTimepoint(ActionTimepoint actionTimepoint)
    {
      string str = actionTimepoint != null ? string.Format("SetActionTimepoint( {0} )", (object) actionTimepoint) : throw new ArgumentNullException("ActionTimepoint can not be null!");
      MinomatV4.logger.Info(str);
      if (actionTimepoint.Timepoint < this.TIMEPOINT0)
        throw new ArgumentNullException("Action time point can not less as 2001/1/1!");
      uint uint32_1 = Convert.ToUInt32((actionTimepoint.Timepoint - this.TIMEPOINT0).TotalSeconds);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ActionTimepoint);
      payload.Add((byte) actionTimepoint.Action);
      payload.Add((byte) actionTimepoint.TimepointType);
      payload.AddRange((IEnumerable<byte>) BitConverter.GetBytes(uint32_1));
      payload.Add((byte) 0);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null || onlyOnePacket.Payload.Length != 8)
        return false;
      if (!Enum.IsDefined(typeof (ActionMode), (object) onlyOnePacket.Payload[1]))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse response of the ActionTimepoint! Unknown action. Value: {0}, Payload: {1}", (object) onlyOnePacket.Payload[1], (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      ActionMode actionMode = (ActionMode) Enum.ToObject(typeof (ActionMode), onlyOnePacket.Payload[1]);
      if (!Enum.IsDefined(typeof (ActionTimepointType), (object) onlyOnePacket.Payload[2]))
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, string.Format("Can not parse response of the ActionTimepoint! Unknown type of time point. Value: {0}, Payload: {1}", (object) onlyOnePacket.Payload[2], (object) Util.ByteArrayToHexString(onlyOnePacket.Payload)));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      ActionTimepointType actionTimepointType = (ActionTimepointType) Enum.ToObject(typeof (ActionTimepointType), onlyOnePacket.Payload[2]);
      uint uint32_2 = BitConverter.ToUInt32(onlyOnePacket.Payload, 3);
      return actionTimepoint.Action == actionMode && actionTimepoint.TimepointType == actionTimepointType && (int) uint32_1 == (int) uint32_2;
    }

    public string GetModemBuildDate()
    {
      string str = "GetModemBuildDate()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemBuildDate), str);
      if (onlyOnePacket == null)
        return (string) null;
      return onlyOnePacket.Payload.Length > 1 ? Encoding.ASCII.GetString(onlyOnePacket.Payload, 1, onlyOnePacket.Payload.Length - 1).TrimEnd(new char[1]).Trim() : string.Empty;
    }

    public bool ModemUpdateImageClear()
    {
      MinomatV4.logger.Info("ModemUpdateImageClear()");
      return this.SendPacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemUpdateImageClear));
    }

    public bool ModemUpdate(string url)
    {
      url = !string.IsNullOrEmpty(url) ? url.Trim() : throw new ArgumentNullException("Input parameter 'url' can not be null!");
      MinomatV4.logger.Info("ModemUpdate( {0} )", url);
      return this.SendPacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemUpdate, new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(url))
      {
        (byte) 0
      }.ToArray()));
    }

    public bool ModemReboot()
    {
      string str = "ModemReboot()";
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ModemReboot);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 2)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, "Unknown response received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return (int) onlyOnePacket.Payload[0] == (int) payload[0];
    }

    public ModemCounter GetModemCounter(string type)
    {
      return !string.IsNullOrEmpty(type) && Enum.IsDefined(typeof (ModemCounterType), (object) type) ? this.GetModemCounter((ModemCounterType) Enum.Parse(typeof (ModemCounterType), type, true)) : throw new ArgumentException("Invalid ModemCounterType argument!");
    }

    public ModemCounter GetModemCounter(ModemCounterType type)
    {
      string str = string.Format("GetModemCounter( {0} )", (object) type);
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemCounter, new byte[1]
      {
        (byte) type
      }), str);
      if (onlyOnePacket == null)
        return (ModemCounter) null;
      if (onlyOnePacket.Payload.Length != 10)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, "Unknown response received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      return ModemCounter.Parse(onlyOnePacket.Payload);
    }

    public ModemUpdateTiming GetModemUpdateTiming()
    {
      string str = "GetModemUpdateTiming()";
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemUpdateTiming), str);
      return onlyOnePacket == null ? (ModemUpdateTiming) null : ModemUpdateTiming.Parse(onlyOnePacket.Payload);
    }

    public bool ResetModemCounter(string type)
    {
      return !string.IsNullOrEmpty(type) && Enum.IsDefined(typeof (ModemCounterType), (object) type) ? this.ResetModemCounter((ModemCounterType) Enum.Parse(typeof (ModemCounterType), type, true)) : throw new ArgumentException("Invalid ModemCounterType argument!");
    }

    public bool ResetModemCounter(ModemCounterType type)
    {
      string str = string.Format("ResetModemCounter( {0} )", (object) type);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.ModemCounter, new byte[1]
      {
        (byte) type
      });
      payload.Add(byte.MaxValue);
      payload.Add((byte) 0);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (onlyOnePacket == null)
        return false;
      if (onlyOnePacket.Payload.Length != 10)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.UnknownResponce, "Unknown response received! Payload: 0x" + Util.ByteArrayToHexString(onlyOnePacket.Payload));
        MinomatV4.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      ModemCounter modemCounter = ModemCounter.Parse(onlyOnePacket.Payload);
      return modemCounter != null && modemCounter.Type == type && modemCounter.Value == (ushort) 0;
    }

    public bool ModemUpdateTest(string url)
    {
      url = url != null ? url.Trim() : throw new ArgumentNullException("Input parameter 'url' can not be null!");
      string str = !string.IsNullOrEmpty(url) ? string.Format("ModemUpdateTest( {0} )", (object) url) : throw new ArgumentNullException("Input parameter 'url' can not be empty!");
      MinomatV4.logger.Info(str);
      SCGiPacket onlyOnePacket = this.SendAndReceiveOnlyOnePacket(new SCGiHeader(SCGiMessageType.GSM, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, MinomatV4.CreatePayload(SCGiCommand.ModemUpdateTest, new List<byte>((IEnumerable<byte>) Encoding.ASCII.GetBytes(url))
      {
        (byte) 0
      }.ToArray()), str);
      return onlyOnePacket != null && onlyOnePacket.Payload != null && onlyOnePacket.Payload.Length == 2 && onlyOnePacket.Payload[0] == (byte) 236;
    }

    public string GetConfigurationString()
    {
      string str = "GetMinomatConfigurationString()";
      MinomatV4.logger.Info(str);
      SCGiFrame frame = this.SendAndReceiveFrame(new SCGiHeader(SCGiMessageType.SCGI_1_9, this.Authentication), MinomatV4.CreatePayload(SCGiCommand.ConfigurationString), str);
      if (frame == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (SCGiPacket scGiPacket in (List<SCGiPacket>) frame)
        stringBuilder.Append(Encoding.ASCII.GetString(scGiPacket.Payload, 0, scGiPacket.Payload.Length).TrimEnd(new char[1]));
      return stringBuilder.ToString();
    }

    public MinomatV4Settings ReadSettings()
    {
      MinomatV4.logger.Info("ReadSettings()");
      MinomatV4Settings minomatV4Settings = new MinomatV4Settings();
      minomatV4Settings.Add(SCGiCommand.UserappName, this.ExecuteCMD(SCGiCommand.UserappName));
      bool flag = string.IsNullOrEmpty(minomatV4Settings[SCGiCommand.UserappName].ErrorText) && minomatV4Settings[SCGiCommand.UserappName].Value != null && minomatV4Settings[SCGiCommand.UserappName].Value.ToString().Contains("Master");
      minomatV4Settings.Add(SCGiCommand.FirmwareVersion, this.ExecuteCMD(SCGiCommand.FirmwareVersion));
      minomatV4Settings.Add(SCGiCommand.FirmwareBuildTime, this.ExecuteCMD(SCGiCommand.FirmwareBuildTime));
      minomatV4Settings.Add(SCGiCommand.UserappBuildTime, this.ExecuteCMD(SCGiCommand.UserappBuildTime));
      minomatV4Settings.Add(SCGiCommand.NodeId, this.ExecuteCMD(SCGiCommand.NodeId));
      minomatV4Settings.Add(SCGiCommand.NetworkId, this.ExecuteCMD(SCGiCommand.NetworkId));
      minomatV4Settings.Add(SCGiCommand.ErrorFlags, this.ExecuteCMD(SCGiCommand.ErrorFlags));
      minomatV4Settings.Add(SCGiCommand.TransmissionPower, this.ExecuteCMD(SCGiCommand.TransmissionPower));
      minomatV4Settings.Add(SCGiCommand.TransceiverChannelId, this.ExecuteCMD(SCGiCommand.TransceiverChannelId));
      minomatV4Settings.Add(SCGiCommand.MultiChannelSettings, this.ExecuteCMD(SCGiCommand.MultiChannelSettings));
      minomatV4Settings.Add(SCGiCommand.TransceiverFrequencyOffset, this.ExecuteCMD(SCGiCommand.TransceiverFrequencyOffset));
      minomatV4Settings.Add(SCGiCommand.SystemTime, this.ExecuteCMD(SCGiCommand.SystemTime));
      minomatV4Settings.Add(SCGiCommand.TemperatureOffset, this.ExecuteCMD(SCGiCommand.TemperatureOffset));
      minomatV4Settings.Add(SCGiCommand.ResetConfigurationState, this.ExecuteCMD(SCGiCommand.ResetConfigurationState));
      minomatV4Settings.Add(SCGiCommand.RegisteredMessUnits, this.ExecuteCMD(SCGiCommand.RegisteredMessUnits));
      minomatV4Settings.Add(SCGiCommand.TestReceptionResult, this.ExecuteCMD(SCGiCommand.TestReceptionResult));
      minomatV4Settings.Add(SCGiCommand.RadioChannel, this.ExecuteCMD(SCGiCommand.RadioChannel));
      minomatV4Settings.Add(SCGiCommand.PhaseDetailsBuffer, this.ExecuteCMD(SCGiCommand.PhaseDetailsBuffer));
      if (flag)
      {
        minomatV4Settings.Add(SCGiCommand.RegisteredSlavesIntegrated, this.ExecuteCMD(SCGiCommand.RegisteredSlavesIntegrated));
        minomatV4Settings.Add(SCGiCommand.RegisteredSlavesNotIntegrated, this.ExecuteCMD(SCGiCommand.RegisteredSlavesNotIntegrated));
      }
      minomatV4Settings.Add(SCGiCommand.MinolId, this.ExecuteCMD(SCGiCommand.MinolId));
      minomatV4Settings.Add(SCGiCommand.PhaseDetails, this.ExecuteCMD(SCGiCommand.PhaseDetails));
      minomatV4Settings.Add(SCGiCommand.MessUnitNumberMax, this.ExecuteCMD(SCGiCommand.MessUnitNumberMax));
      minomatV4Settings.Add(SCGiCommand.MaxMessUnitNumberNotConfigured, this.ExecuteCMD(SCGiCommand.MaxMessUnitNumberNotConfigured));
      minomatV4Settings.Add(SCGiCommand.Scenario, this.ExecuteCMD(SCGiCommand.Scenario));
      minomatV4Settings.Add(SCGiCommand.RoutingTable, this.ExecuteCMD(SCGiCommand.RoutingTable));
      if (flag)
      {
        minomatV4Settings.Add(SCGiCommand.MasterMinolID, this.ExecuteCMD(SCGiCommand.MasterMinolID));
        minomatV4Settings.Add(SCGiCommand.GsmState, this.ExecuteCMD(SCGiCommand.GsmState));
        minomatV4Settings.Add(SCGiCommand.HttpState, this.ExecuteCMD(SCGiCommand.HttpState));
        minomatV4Settings.Add(SCGiCommand.GSMTestReceptionState, this.ExecuteCMD(SCGiCommand.GSMTestReceptionState));
        minomatV4Settings.Add(SCGiCommand.AppInitialSettings, this.ExecuteCMD(SCGiCommand.AppInitialSettings));
        minomatV4Settings.Add(SCGiCommand.SimPin, this.ExecuteCMD(SCGiCommand.SimPin));
        minomatV4Settings.Add(SCGiCommand.APN, this.ExecuteCMD(SCGiCommand.APN));
        minomatV4Settings.Add(SCGiCommand.GPRSUserName, this.ExecuteCMD(SCGiCommand.GPRSUserName));
        minomatV4Settings.Add(SCGiCommand.GPRSPassword, this.ExecuteCMD(SCGiCommand.GPRSPassword));
        minomatV4Settings.Add(SCGiCommand.HttpServer, this.ExecuteCMD(SCGiCommand.HttpServer));
        minomatV4Settings.Add(SCGiCommand.HttpResourceName, this.ExecuteCMD(SCGiCommand.HttpResourceName));
        minomatV4Settings.Add(SCGiCommand.ModemBuildDate, this.ExecuteCMD(SCGiCommand.ModemBuildDate));
      }
      return minomatV4Settings;
    }

    private MinomatV4Parameter ExecuteCMD(SCGiCommand cmd) => this.ExecuteCMD(cmd, (object) null);

    private MinomatV4Parameter ExecuteCMD(SCGiCommand cmd, object obj)
    {
      DateTime now = DateTime.Now;
      string str = string.Empty;
      object obj1 = (object) null;
      try
      {
        switch (cmd)
        {
          case SCGiCommand.RegisteredSlavesIntegrated:
            obj1 = (object) this.GetRegisteredSlaves(SlaveType.Integrated);
            break;
          case SCGiCommand.RegisteredSlavesNotIntegrated:
            obj1 = (object) this.GetRegisteredSlaves(SlaveType.NotIntegrated);
            break;
          case SCGiCommand.ResetConfigurationState:
            obj1 = (object) this.GetResetConfigurationState();
            break;
          case SCGiCommand.MinolId:
            obj1 = (object) this.GetMinolId();
            break;
          case SCGiCommand.NodeId:
            obj1 = (object) this.GetNodeId();
            break;
          case SCGiCommand.NetworkId:
            obj1 = (object) this.GetNetworkId();
            break;
          case SCGiCommand.SystemTime:
            obj1 = (object) this.GetSystemTime();
            break;
          case SCGiCommand.RadioChannel:
            obj1 = (object) this.GetRadioChannel();
            break;
          case SCGiCommand.TransceiverChannelId:
            obj1 = (object) this.GetTransceiverChannelId();
            break;
          case SCGiCommand.RoutingTable:
            obj1 = (object) this.GetRoutingTable();
            break;
          case SCGiCommand.FirmwareVersion:
            obj1 = (object) this.GetFirmwareVersion();
            break;
          case SCGiCommand.UserappName:
            obj1 = (object) this.GetUserappName();
            break;
          case SCGiCommand.FirmwareBuildTime:
            obj1 = (object) this.GetFirmwareBuildTime();
            break;
          case SCGiCommand.UserappBuildTime:
            obj1 = (object) this.GetUserappBuildTime();
            break;
          case SCGiCommand.ErrorFlags:
            obj1 = (object) this.GetErrorFlags();
            break;
          case SCGiCommand.TransmissionPower:
            obj1 = (object) this.GetTransmissionPower();
            break;
          case SCGiCommand.MultiChannelSettings:
            obj1 = (object) this.GetMultiChannelSettings();
            break;
          case SCGiCommand.TransceiverFrequencyOffset:
            obj1 = (object) this.GetTransceiverFrequencyOffset();
            break;
          case SCGiCommand.TemperatureOffset:
            obj1 = (object) this.GetTemperatureOffset();
            break;
          case SCGiCommand.PhaseDetailsBuffer:
            obj1 = (object) this.GetPhaseDetailsBuffer();
            break;
          case SCGiCommand.PhaseDetails:
            obj1 = (object) this.GetPhaseDetails();
            break;
          case SCGiCommand.MessUnitNumberMax:
            obj1 = (object) this.GetMessUnitNumberMax();
            break;
          case SCGiCommand.MaxMessUnitNumberNotConfigured:
            obj1 = (object) this.GetMessUnitNumberNotConfiguredMax();
            break;
          case SCGiCommand.Scenario:
            obj1 = (object) this.GetScenario();
            break;
          case SCGiCommand.TestReceptionResult:
            obj1 = (object) this.GetTestReceptionResult();
            break;
          case SCGiCommand.RegisteredMessUnits:
            obj1 = (object) this.GetRegisteredMessUnits();
            break;
          case SCGiCommand.SimPin:
            obj1 = (object) this.GetSimPin();
            break;
          case SCGiCommand.MasterMinolID:
            obj1 = (object) this.GetMasterMinolIDOfModem();
            break;
          case SCGiCommand.APN:
            obj1 = (object) this.GetAPN();
            break;
          case SCGiCommand.GPRSUserName:
            obj1 = (object) this.GetGPRSUserName();
            break;
          case SCGiCommand.GPRSPassword:
            obj1 = (object) this.GetGPRSPassword();
            break;
          case SCGiCommand.HttpServer:
            obj1 = (object) this.GetHttpServer();
            break;
          case SCGiCommand.HttpResourceName:
            obj1 = (object) this.GetHttpResourceName();
            break;
          case SCGiCommand.GSMTestReceptionState:
            obj1 = (object) this.GetGSMTestReceptionState();
            break;
          case SCGiCommand.AppInitialSettings:
            obj1 = (object) this.GetAppInitialSettings();
            break;
          case SCGiCommand.HttpState:
            obj1 = (object) this.GetHttpState();
            break;
          case SCGiCommand.GsmState:
            obj1 = (object) this.GetGsmState();
            break;
          case SCGiCommand.ModemBuildDate:
            obj1 = (object) this.GetModemBuildDate();
            break;
          default:
            throw new NotImplementedException(cmd.ToString() + " is not implemented!");
        }
      }
      catch (Exception ex)
      {
        str = ex.Message;
      }
      TimeSpan timeSpan = DateTime.Now - now;
      MinomatV4Parameter e = new MinomatV4Parameter()
      {
        Name = cmd,
        Elapsed = timeSpan,
        Value = obj1,
        ErrorText = str
      };
      if (this.OnMinomatV4ParameterReceived != null)
        this.OnMinomatV4ParameterReceived((object) this, e);
      return e;
    }

    public ComServerFile GetComServerFile(string type)
    {
      return !string.IsNullOrEmpty(type) && Enum.IsDefined(typeof (ComServerFileType), (object) type) ? this.GetComServerFile((ComServerFileType) Enum.Parse(typeof (ComServerFileType), type, true)) : throw new ArgumentException("Invalid input parameter 'type'!");
    }

    public ComServerFile GetComServerFile(ComServerFileType type)
    {
      MinomatV4.logger.Info<ComServerFileType>("GetComServerFile( {0} )", type);
      switch (type)
      {
        case ComServerFileType.Name:
          return this.GetComServerFile("/proc/sys/kernel/hostname", true);
        case ComServerFileType.SystemTime:
          return this.GetComServerFile("date", true);
        case ComServerFileType.Network:
          return this.GetComServerFile("ifconfig", true);
        case ComServerFileType.NetworkState:
          return this.GetComServerFile("netstat -a", true);
        case ComServerFileType.NetworkInterfaces:
          return this.GetComServerFile("/etc/network/interfaces", true);
        case ComServerFileType.MeterVPN:
          return this.GetComServerFile("/etc/openvpn/metervpn.ovpn", true);
        case ComServerFileType.ModemChatGPRS:
          return this.GetComServerFile("/etc/ppp/chat/gprs", true);
        case ComServerFileType.ModemPeersGPRS:
          return this.GetComServerFile("/etc/ppp/peers/gprs", true);
        case ComServerFileType.ModemResolv:
          return this.GetComServerFile("/etc/ppp/resolv.conf", true);
        case ComServerFileType.WhoIsLogged:
          return this.GetComServerFile("who", true);
        case ComServerFileType.Certificate:
          return this.GetComServerFile("/etc/openvpn/comserver.p12", false);
        case ComServerFileType.TaskManager:
          return this.GetComServerFile("ps -elf", true);
        case ComServerFileType.Route:
          return this.GetComServerFile("route", true);
        case ComServerFileType.Ser2net:
          return this.GetComServerFile("/etc/ser2net.conf", true);
        case ComServerFileType.DiskFree:
          return this.GetComServerFile("df -h", true);
        case ComServerFileType.Reboot:
          return this.GetComServerFile("reboot", true);
        case ComServerFileType.Test:
          return new ComServerFile();
        default:
          return (ComServerFile) null;
      }
    }

    private bool SetComServerFile(string target, string source)
    {
      if (string.IsNullOrEmpty(target))
        throw new ArgumentNullException("Target can not be empty!");
      if (string.IsNullOrEmpty(source))
        throw new ArgumentNullException("Source can not be empty!");
      if (!File.Exists(source))
        throw new ArgumentException("Source file doesn't exist!: Path: " + source);
      MinomatV4.logger.Info<string, string>("SetComServerFile( Target: {0}, Source: {1} )", target, source);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.SetComServerFile);
      payload.AddRange((IEnumerable<byte>) Util.StringToByteArray(target));
      SCGiPacket packet = new SCGiPacket(new SCGiHeader(SCGiMessageType.ComServer, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem,
        IsSequence = true
      }, payload.ToArray());
      packet.CalculateCRC();
      if (!this.connection.Write(packet))
        return false;
      packet.InitCRC = packet.CalculateCRC().Value;
      packet.Header.ExtendedHeaderList.Clear();
      packet.Header.SequenceHeaderType = SCGiSequenceHeaderType.None;
      using (BinaryReader binaryReader = new BinaryReader((Stream) File.Open(source, FileMode.Open)))
      {
        int length = (int) binaryReader.BaseStream.Length;
        byte count = length < 199 ? (byte) length : (byte) 199;
        while (length > 0)
        {
          Thread.Sleep(50);
          byte[] collection = binaryReader.ReadBytes((int) count);
          payload.Clear();
          payload.Add(count);
          payload.AddRange((IEnumerable<byte>) collection);
          packet.SetPayload(payload.ToArray());
          length -= (int) count;
          if (length < (int) count)
            count = (byte) length;
          if (length == 0)
            packet.Header.IsSequence = false;
          if (!this.connection.Write(packet))
            return false;
          packet.InitCRC = packet.CalculateCRC().Value;
        }
      }
      return true;
    }

    private ComServerFile GetComServerFile(string cmd, bool isASCII)
    {
      string str = string.Format("GetComServerFile( {0}, IsASCII {1} )", (object) cmd, (object) isASCII);
      MinomatV4.logger.Info(str);
      List<byte> payload = MinomatV4.CreatePayload(SCGiCommand.GetComServerFile);
      payload.AddRange((IEnumerable<byte>) Util.StringToByteArray(cmd));
      SCGiFrame frame = this.SendAndReceiveFrame(new SCGiHeader(SCGiMessageType.ComServer, this.Authentication)
      {
        DestinationAddress = SCGiAddress.Modem
      }, payload, str);
      if (frame == null || frame.Count == 0 || frame[0].Payload.Length < 2)
        return (ComServerFile) null;
      byte[] bytes = BitConverter.GetBytes((ushort) 64);
      if ((int) bytes[1] == (int) frame[0].Payload[0] && (int) bytes[0] == (int) frame[0].Payload[1])
        return ComServerFile.Parse(frame, isASCII);
      MinomatV4.logger.Error("Received wrong command!");
      return (ComServerFile) null;
    }

    public static byte[] RemoveSCGiFrame(byte[] stuffedBuffer) => SCGiFrame.GetData(stuffedBuffer);

    public byte[] GetRoutingTableViaFlash()
    {
      return this.GetFlash((ushort) 0, (ushort) 20, (ushort) 0, (ushort) 527)?.ToArray();
    }

    public static void AddValues(
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      Dictionary<MeasurementDataType, MeasurementData> dataSet,
      ValueIdent.ValueIdPart_MeterType meterType,
      List<long> filter,
      ValueIdent.ValueIdPart_StorageInterval storageInterval,
      MeasurementDataType valueType)
    {
      if (!dataSet.ContainsKey(valueType) || filter != null && !ValueIdent.Contains(filter, storageInterval))
        return;
      switch (meterType)
      {
        case ValueIdent.ValueIdPart_MeterType.Water:
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1M, ValueIdent.ValueIdPart_PhysicalQuantity.Pulse);
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1000M, ValueIdent.ValueIdPart_PhysicalQuantity.Volume);
          break;
        case ValueIdent.ValueIdPart_MeterType.HeatCostAllocator:
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1M, ValueIdent.ValueIdPart_PhysicalQuantity.HCA);
          break;
        case ValueIdent.ValueIdPart_MeterType.Thermometer:
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1M, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature);
          break;
        case ValueIdent.ValueIdPart_MeterType.Hygrometer:
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1M, ValueIdent.ValueIdPart_PhysicalQuantity.Percent);
          break;
        case ValueIdent.ValueIdPart_MeterType.SmokeDetector:
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1M, ValueIdent.ValueIdPart_PhysicalQuantity.StatusNumber);
          break;
        default:
          MinomatV4.AddValues(ValueList, dataSet, meterType, storageInterval, valueType, 1M, ValueIdent.ValueIdPart_PhysicalQuantity.Pulse);
          break;
      }
    }

    private static void AddValues(
      SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      Dictionary<MeasurementDataType, MeasurementData> dataSet,
      ValueIdent.ValueIdPart_MeterType meterType,
      ValueIdent.ValueIdPart_StorageInterval storageInterval,
      MeasurementDataType valueType,
      Decimal scalar,
      ValueIdent.ValueIdPart_PhysicalQuantity physicalQuantity)
    {
      long valueIdForValueEnum = ValueIdent.GetValueIdForValueEnum(physicalQuantity, meterType, ValueIdent.ValueIdPart_Calculation.Accumulated, ValueIdent.ValueIdPart_CalculationStart.MeterProduction, storageInterval, ValueIdent.ValueIdPart_Creation.Meter, (object) ValueIdent.ValueIdPart_Index.Any);
      SortedList<DateTime, ReadingValue> sortedList = new SortedList<DateTime, ReadingValue>();
      foreach (KeyValuePair<DateTime, Decimal?> keyValuePair in dataSet[valueType].Data)
      {
        if (keyValuePair.Value.HasValue)
          sortedList.Add(keyValuePair.Key, new ReadingValue()
          {
            state = ReadingValueState.ok,
            value = Convert.ToDouble(keyValuePair.Value.Value / scalar)
          });
      }
      ValueList.Add(valueIdForValueEnum, sortedList);
    }

    public sealed class StateEventArgs : EventArgs
    {
      public int ProgressValue;
      public string Message;
      public Exception Exception;
    }
  }
}
