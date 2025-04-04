// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Internal.FileAppenders;
using NLog.Layouts;
using NLog.Targets.FileArchiveModes;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Targets
{
  [Target("File")]
  public class FileTarget : TargetWithLayoutHeaderAndFooter, ICreateFileParameters
  {
    private const int InitializedFilesCleanupPeriod = 2;
    private const int InitializedFilesCounterMax = 25;
    private const int ArchiveAboveSizeDisabled = -1;
    private readonly Dictionary<string, DateTime> _initializedFiles = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private LineEndingMode _lineEndingMode = LineEndingMode.Default;
    private IFileAppenderFactory _appenderFactory;
    private FileAppenderCache _fileAppenderCache;
    private IFileArchiveMode _fileArchiveHelper;
    private Timer _autoClosingTimer;
    private int _initializedFilesCounter;
    private int _maxArchiveFiles;
    private FilePathLayout _fullFileName;
    private FilePathLayout _fullArchiveFileName;
    private FileArchivePeriod _archiveEvery;
    private long _archiveAboveSize;
    private bool _enableArchiveFileCompression;
    private DateTime? _previousLogEventTimestamp;
    private string _previousLogFileName;
    private bool? _concurrentWrites;
    private bool _keepFileOpen;
    private bool _cleanupFileName;
    private FilePathKind _fileNameKind;
    private FilePathKind _archiveFileKind;
    private string _archiveDateFormat;
    private ArchiveNumberingMode _archiveNumbering;
    private readonly ReusableStreamCreator _reusableFileWriteStream = new ReusableStreamCreator(4096);
    private readonly ReusableStreamCreator _reusableAsyncFileWriteStream = new ReusableStreamCreator(4096);
    private readonly ReusableBufferCreator _reusableEncodingBuffer = new ReusableBufferCreator(1024);
    private SortHelpers.KeySelector<AsyncLogEventInfo, string> _getFullFileNameDelegate;

    private IFileArchiveMode GetFileArchiveHelper(string archiveFilePattern)
    {
      return this._fileArchiveHelper ?? (this._fileArchiveHelper = FileArchiveModeFactory.CreateArchiveStyle(archiveFilePattern, this.ArchiveNumbering, this.GetArchiveDateFormatString(this.ArchiveDateFormat), this.ArchiveFileName != null, this.MaxArchiveFiles));
    }

    public FileTarget()
    {
      this.ArchiveNumbering = ArchiveNumberingMode.Sequence;
      this._maxArchiveFiles = 0;
      this.ConcurrentWriteAttemptDelay = 1;
      this.ArchiveEvery = FileArchivePeriod.None;
      this.ArchiveAboveSize = -1L;
      this.ConcurrentWriteAttempts = 10;
      this.ConcurrentWrites = true;
      this.Encoding = Encoding.Default;
      this.BufferSize = 32768;
      this.AutoFlush = true;
      this.FileAttributes = Win32FileAttributes.Normal;
      this.LineEnding = LineEndingMode.Default;
      this.EnableFileDelete = true;
      this.OpenFileCacheTimeout = -1;
      this.OpenFileCacheSize = 5;
      this.CreateDirs = true;
      this.ForceManaged = false;
      this.ArchiveDateFormat = string.Empty;
      this._fileAppenderCache = FileAppenderCache.Empty;
      this.CleanupFileName = true;
      this.WriteFooterOnArchivingOnly = false;
      this.OptimizeBufferReuse = this.GetType() == typeof (FileTarget);
    }

    static FileTarget()
    {
      FileTarget.FileCompressor = (IFileCompressor) new ZipArchiveFileCompressor();
    }

    public FileTarget(string name)
      : this()
    {
      this.Name = name;
    }

    [RequiredParameter]
    public Layout FileName
    {
      get => this._fullFileName == null ? (Layout) null : this._fullFileName.GetLayout();
      set
      {
        this._fullFileName = this.CreateFileNameLayout(value);
        this.ResetFileAppenders("FileName Changed");
      }
    }

    private FilePathLayout CreateFileNameLayout(Layout value)
    {
      return value == null ? (FilePathLayout) null : new FilePathLayout(value, this.CleanupFileName, this.FileNameKind);
    }

    [DefaultValue(true)]
    public bool CleanupFileName
    {
      get => this._cleanupFileName;
      set
      {
        if (this._cleanupFileName == value)
          return;
        this._cleanupFileName = value;
        this._fullFileName = this.CreateFileNameLayout(this.FileName);
        this._fullArchiveFileName = this.CreateFileNameLayout(this.ArchiveFileName);
        this.ResetFileAppenders("CleanupFileName Changed");
      }
    }

    [DefaultValue(FilePathKind.Unknown)]
    public FilePathKind FileNameKind
    {
      get => this._fileNameKind;
      set
      {
        if (this._fileNameKind == value)
          return;
        this._fileNameKind = value;
        this._fullFileName = this.CreateFileNameLayout(this.FileName);
        this.ResetFileAppenders("FileNameKind Changed");
      }
    }

    [DefaultValue(true)]
    [Advanced]
    public bool CreateDirs { get; set; }

    [DefaultValue(false)]
    public bool DeleteOldFileOnStartup { get; set; }

    [DefaultValue(false)]
    [Advanced]
    public bool ReplaceFileContentsOnEachWrite { get; set; }

    [DefaultValue(false)]
    public bool KeepFileOpen
    {
      get => this._keepFileOpen;
      set
      {
        if (this._keepFileOpen == value)
          return;
        this._keepFileOpen = value;
        this.ResetFileAppenders("KeepFileOpen Changed");
      }
    }

    [Obsolete("This option will be removed in NLog 5. Marked obsolete on NLog 4.5")]
    [DefaultValue(0)]
    public int maxLogFilenames { get; set; }

    [DefaultValue(true)]
    public bool EnableFileDelete { get; set; }

    [Advanced]
    public Win32FileAttributes FileAttributes { get; set; }

    bool ICreateFileParameters.CaptureLastWriteTime
    {
      get
      {
        return this.ArchiveNumbering == ArchiveNumberingMode.Date || this.ArchiveNumbering == ArchiveNumberingMode.DateAndSequence;
      }
    }

    bool ICreateFileParameters.IsArchivingEnabled => this.IsArchivingEnabled;

    [Advanced]
    public LineEndingMode LineEnding
    {
      get => this._lineEndingMode;
      set => this._lineEndingMode = value;
    }

    [DefaultValue(true)]
    public bool AutoFlush { get; set; }

    [DefaultValue(5)]
    [Advanced]
    public int OpenFileCacheSize { get; set; }

    [DefaultValue(-1)]
    [Advanced]
    public int OpenFileCacheTimeout { get; set; }

    [DefaultValue(32768)]
    public int BufferSize { get; set; }

    public Encoding Encoding { get; set; }

    [DefaultValue(false)]
    [Advanced]
    public bool DiscardAll { get; set; }

    [DefaultValue(true)]
    public bool ConcurrentWrites
    {
      get => this._concurrentWrites ?? true;
      set
      {
        bool? concurrentWrites = this._concurrentWrites;
        bool flag = value;
        if ((concurrentWrites.GetValueOrDefault() == flag ? (!concurrentWrites.HasValue ? 1 : 0) : 1) == 0)
          return;
        this._concurrentWrites = new bool?(value);
        this.ResetFileAppenders("ConcurrentWrites Changed");
      }
    }

    [DefaultValue(false)]
    public bool NetworkWrites { get; set; }

    [DefaultValue(false)]
    public bool WriteBom { get; set; }

    [DefaultValue(10)]
    [Advanced]
    public int ConcurrentWriteAttempts { get; set; }

    [DefaultValue(1)]
    [Advanced]
    public int ConcurrentWriteAttemptDelay { get; set; }

    [DefaultValue(false)]
    public bool ArchiveOldFileOnStartup { get; set; }

    [DefaultValue("")]
    public string ArchiveDateFormat
    {
      get => this._archiveDateFormat;
      set
      {
        if (!(this._archiveDateFormat != value))
          return;
        this._archiveDateFormat = value;
        this.ResetFileAppenders("ArchiveDateFormat Changed");
      }
    }

    public long ArchiveAboveSize
    {
      get => this._archiveAboveSize;
      set
      {
        if (this._archiveAboveSize == -1L != (value == -1L))
        {
          this._archiveAboveSize = value;
          this.ResetFileAppenders("ArchiveAboveSize Changed");
        }
        else
          this._archiveAboveSize = value;
      }
    }

    public FileArchivePeriod ArchiveEvery
    {
      get => this._archiveEvery;
      set
      {
        if (this._archiveEvery == value)
          return;
        this._archiveEvery = value;
        this.ResetFileAppenders("ArchiveEvery Changed");
      }
    }

    public FilePathKind ArchiveFileKind
    {
      get => this._archiveFileKind;
      set
      {
        if (this._archiveFileKind == value)
          return;
        this._archiveFileKind = value;
        this._fullArchiveFileName = this.CreateFileNameLayout(this.ArchiveFileName);
        this.ResetFileAppenders("ArchiveFileKind Changed");
      }
    }

    public Layout ArchiveFileName
    {
      get
      {
        return this._fullArchiveFileName == null ? (Layout) null : this._fullArchiveFileName.GetLayout();
      }
      set
      {
        this._fullArchiveFileName = this.CreateFileNameLayout(value);
        this.ResetFileAppenders("ArchiveFileName Changed");
      }
    }

    [DefaultValue(0)]
    public int MaxArchiveFiles
    {
      get => this._maxArchiveFiles;
      set
      {
        if (this._maxArchiveFiles == value)
          return;
        this._maxArchiveFiles = value;
        this.ResetFileAppenders("MaxArchiveFiles Changed");
      }
    }

    public ArchiveNumberingMode ArchiveNumbering
    {
      get => this._archiveNumbering;
      set
      {
        if (this._archiveNumbering == value)
          return;
        this._archiveNumbering = value;
        this.ResetFileAppenders("ArchiveNumbering Changed");
      }
    }

    public static IFileCompressor FileCompressor { get; set; }

    [DefaultValue(false)]
    public bool EnableArchiveFileCompression
    {
      get => this._enableArchiveFileCompression && FileTarget.FileCompressor != null;
      set
      {
        if (this._enableArchiveFileCompression == value)
          return;
        this._enableArchiveFileCompression = value;
        this.ResetFileAppenders("EnableArchiveFileCompression Changed");
      }
    }

    [DefaultValue(false)]
    public bool ForceManaged { get; set; }

    [DefaultValue(false)]
    public bool ForceMutexConcurrentWrites { get; set; }

    [DefaultValue(false)]
    public bool WriteFooterOnArchivingOnly { get; set; }

    protected internal string NewLineChars => this._lineEndingMode.NewLineCharacters;

    private void RefreshArchiveFilePatternToWatch(string fileName, LogEventInfo logEvent)
    {
      if (this._fileAppenderCache == null)
        return;
      this._fileAppenderCache.CheckCloseAppenders -= new EventHandler(this.AutoClosingTimerCallback);
      if (this.KeepFileOpen)
        this._fileAppenderCache.CheckCloseAppenders += new EventHandler(this.AutoClosingTimerCallback);
      if ((!this.IsArchivingEnabled || !this.ConcurrentWrites ? 0 : (this.KeepFileOpen ? 1 : 0)) != 0)
      {
        string archiveFileNamePattern = this.GetArchiveFileNamePattern(fileName, logEvent);
        string path2 = (!string.IsNullOrEmpty(archiveFileNamePattern) ? this.GetFileArchiveHelper(archiveFileNamePattern) : (IFileArchiveMode) null) != null ? this._fileArchiveHelper.GenerateFileNameMask(archiveFileNamePattern) : string.Empty;
        this._fileAppenderCache.ArchiveFilePatternToWatch = !string.IsNullOrEmpty(path2) ? Path.Combine(Path.GetDirectoryName(archiveFileNamePattern), path2) : string.Empty;
      }
      else
        this._fileAppenderCache.ArchiveFilePatternToWatch = (string) null;
    }

    public void CleanupInitializedFiles()
    {
      this.CleanupInitializedFiles(TimeSource.Current.Time.AddDays(-2.0));
    }

    public void CleanupInitializedFiles(DateTime cleanupThreshold)
    {
      if (InternalLogger.IsTraceEnabled)
        InternalLogger.Trace<string, DateTime>("FileTarget(Name={0}): Cleanup Initialized Files with cleanupThreshold {1}", this.Name, cleanupThreshold);
      List<string> stringList = (List<string>) null;
      foreach (KeyValuePair<string, DateTime> initializedFile in this._initializedFiles)
      {
        if (initializedFile.Value < cleanupThreshold)
        {
          if (stringList == null)
            stringList = new List<string>();
          stringList.Add(initializedFile.Key);
        }
      }
      if (stringList != null)
      {
        foreach (string fileName in stringList)
          this.FinalizeFile(fileName);
      }
      InternalLogger.Trace<string>("FileTarget(Name={0}): CleanupInitializedFiles Done", this.Name);
    }

    protected override void FlushAsync(AsyncContinuation asyncContinuation)
    {
      try
      {
        InternalLogger.Trace<string>("FileTarget(Name={0}): FlushAsync", this.Name);
        this._fileAppenderCache.FlushAppenders();
        asyncContinuation((Exception) null);
        InternalLogger.Trace<string>("FileTarget(Name={0}): FlushAsync Done", this.Name);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "FileTarget(Name={0}): Exception in FlushAsync", (object) this.Name);
        if (ex.MustBeRethrown())
          throw;
        else
          asyncContinuation(ex);
      }
    }

    private IFileAppenderFactory GetFileAppenderFactory()
    {
      if (this.DiscardAll)
        return NullAppender.TheFactory;
      if (!this.KeepFileOpen || this.NetworkWrites)
        return RetryingMultiProcessFileAppender.TheFactory;
      if (this.ConcurrentWrites)
      {
        if (!this.ForceMutexConcurrentWrites && PlatformDetector.IsDesktopWin32 && !PlatformDetector.IsMono)
          return WindowsMultiProcessFileAppender.TheFactory;
        return PlatformDetector.SupportsSharableMutex ? MutexMultiProcessFileAppender.TheFactory : RetryingMultiProcessFileAppender.TheFactory;
      }
      return this.IsArchivingEnabled ? CountingSingleProcessFileAppender.TheFactory : SingleProcessFileAppender.TheFactory;
    }

    private bool IsArchivingEnabled => this.ArchiveAboveSize != -1L || this.ArchiveEvery != 0;

    protected override void InitializeTarget()
    {
      base.InitializeTarget();
      this._appenderFactory = this.GetFileAppenderFactory();
      if (InternalLogger.IsTraceEnabled)
        InternalLogger.Trace<string, Type>("FileTarget(Name={0}): Using appenderFactory: {1}", this.Name, this._appenderFactory.GetType());
      this._fileAppenderCache = new FileAppenderCache(this.OpenFileCacheSize, this._appenderFactory, (ICreateFileParameters) this);
      if (this.OpenFileCacheSize <= 0 && !this.EnableFileDelete || this.OpenFileCacheTimeout <= 0)
        return;
      InternalLogger.Trace<string>("FileTarget(Name={0}): Start autoClosingTimer", this.Name);
      this._autoClosingTimer = new Timer((TimerCallback) (state => this.AutoClosingTimerCallback((object) this, EventArgs.Empty)), (object) null, this.OpenFileCacheTimeout * 1000, this.OpenFileCacheTimeout * 1000);
    }

    protected override void CloseTarget()
    {
      base.CloseTarget();
      foreach (string fileName in new List<string>((IEnumerable<string>) this._initializedFiles.Keys))
        this.FinalizeFile(fileName);
      this._fileArchiveHelper = (IFileArchiveMode) null;
      Timer autoClosingTimer = this._autoClosingTimer;
      if (autoClosingTimer != null)
      {
        InternalLogger.Trace<string>("FileTarget(Name={0}): Stop autoClosingTimer", this.Name);
        this._autoClosingTimer = (Timer) null;
        autoClosingTimer.WaitForDispose(TimeSpan.Zero);
      }
      this._fileAppenderCache.CloseAppenders("Dispose");
      this._fileAppenderCache.Dispose();
    }

    private void ResetFileAppenders(string reason)
    {
      this._fileArchiveHelper = (IFileArchiveMode) null;
      if (!this.IsInitialized)
        return;
      this._fileAppenderCache.CloseAppenders(reason);
      this._initializedFiles.Clear();
    }

    protected override void Write(LogEventInfo logEvent)
    {
      string fullFileName = this.GetFullFileName(logEvent);
      if (this.OptimizeBufferReuse)
      {
        using (ReusableObjectCreator<MemoryStream>.LockOject lockOject1 = this._reusableFileWriteStream.Allocate())
        {
          using (ReusableObjectCreator<StringBuilder>.LockOject lockOject2 = this.ReusableLayoutBuilder.Allocate())
          {
            using (ReusableObjectCreator<char[]>.LockOject lockOject3 = this._reusableEncodingBuffer.Allocate())
              this.RenderFormattedMessageToStream(logEvent, lockOject2.Result, lockOject3.Result, lockOject1.Result);
          }
          this.ProcessLogEvent(logEvent, fullFileName, new ArraySegment<byte>(lockOject1.Result.GetBuffer(), 0, (int) lockOject1.Result.Length));
        }
      }
      else
      {
        byte[] bytesToWrite = this.GetBytesToWrite(logEvent);
        this.ProcessLogEvent(logEvent, fullFileName, new ArraySegment<byte>(bytesToWrite));
      }
    }

    internal string GetFullFileName(LogEventInfo logEvent)
    {
      if (this._fullFileName == null)
        return (string) null;
      if (!this.OptimizeBufferReuse)
        return this._fullFileName.Render(logEvent);
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.ReusableLayoutBuilder.Allocate())
        return this._fullFileName.RenderWithBuilder(logEvent, lockOject.Result);
    }

    [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
    protected override void Write(AsyncLogEventInfo[] logEvents)
    {
      this.Write((IList<AsyncLogEventInfo>) logEvents);
    }

    protected override void Write(IList<AsyncLogEventInfo> logEvents)
    {
      if (this._getFullFileNameDelegate == null)
        this._getFullFileNameDelegate = (SortHelpers.KeySelector<AsyncLogEventInfo, string>) (c => this.GetFullFileName(c.LogEvent));
      SortHelpers.ReadOnlySingleBucketDictionary<string, IList<AsyncLogEventInfo>> bucketDictionary = logEvents.BucketSort<AsyncLogEventInfo, string>(this._getFullFileNameDelegate);
      using (ReusableObjectCreator<MemoryStream>.LockOject lockOject1 = !this.OptimizeBufferReuse || logEvents.Count > 1000 ? this._reusableAsyncFileWriteStream.None : this._reusableAsyncFileWriteStream.Allocate())
      {
        using (MemoryStream memoryStream = lockOject1.Result != null ? (MemoryStream) null : new MemoryStream())
        {
          MemoryStream ms = memoryStream ?? lockOject1.Result;
          foreach (KeyValuePair<string, IList<AsyncLogEventInfo>> keyValuePair in bucketDictionary)
          {
            string key = keyValuePair.Key;
            ms.SetLength(0L);
            ms.Position = 0L;
            LogEventInfo firstLogEvent = (LogEventInfo) null;
            int count = keyValuePair.Value.Count;
            using (ReusableObjectCreator<StringBuilder>.LockOject lockOject2 = this.OptimizeBufferReuse ? this.ReusableLayoutBuilder.Allocate() : this.ReusableLayoutBuilder.None)
            {
              using (ReusableObjectCreator<char[]>.LockOject lockOject3 = this.OptimizeBufferReuse ? this._reusableEncodingBuffer.Allocate() : this._reusableEncodingBuffer.None)
              {
                using (ReusableObjectCreator<MemoryStream>.LockOject lockOject4 = this.OptimizeBufferReuse ? this._reusableFileWriteStream.Allocate() : this._reusableFileWriteStream.None)
                {
                  for (int index = 0; index < count; ++index)
                  {
                    AsyncLogEventInfo asyncLogEventInfo = keyValuePair.Value[index];
                    if (firstLogEvent == null)
                      firstLogEvent = asyncLogEventInfo.LogEvent;
                    if (lockOject2.Result != null && lockOject4.Result != null)
                    {
                      lockOject4.Result.Position = 0L;
                      lockOject4.Result.SetLength(0L);
                      lockOject2.Result.ClearBuilder();
                      this.RenderFormattedMessageToStream(asyncLogEventInfo.LogEvent, lockOject2.Result, lockOject3.Result, lockOject4.Result);
                      ms.Write(lockOject4.Result.GetBuffer(), 0, (int) lockOject4.Result.Length);
                    }
                    else
                    {
                      byte[] bytesToWrite = this.GetBytesToWrite(asyncLogEventInfo.LogEvent);
                      if (ms.Capacity == 0)
                        ms.Capacity = this.GetMemoryStreamInitialSize(keyValuePair.Value.Count, bytesToWrite.Length);
                      ms.Write(bytesToWrite, 0, bytesToWrite.Length);
                    }
                  }
                }
              }
            }
            Exception lastException;
            this.FlushCurrentFileWrites(key, firstLogEvent, ms, out lastException);
            for (int index = 0; index < count; ++index)
              keyValuePair.Value[index].Continuation(lastException);
          }
        }
      }
    }

    private int GetMemoryStreamInitialSize(int eventsCount, int firstEventSize)
    {
      if (eventsCount > 10)
        return ((eventsCount + 1) * firstEventSize / 1024 + 1) * 1024;
      return eventsCount > 1 ? (1 + eventsCount) * firstEventSize : firstEventSize;
    }

    private void ProcessLogEvent(
      LogEventInfo logEvent,
      string fileName,
      ArraySegment<byte> bytesToWrite)
    {
      bool initializedNewFile = this.InitializeFile(fileName, logEvent);
      if (this.TryArchiveFile(fileName, logEvent, bytesToWrite.Count, initializedNewFile))
        initializedNewFile = this.InitializeFile(fileName, logEvent);
      this.WriteToFile(fileName, bytesToWrite, initializedNewFile);
      this._previousLogFileName = fileName;
      this._previousLogEventTimestamp = new DateTime?(logEvent.TimeStamp);
    }

    protected virtual string GetFormattedMessage(LogEventInfo logEvent)
    {
      return this.Layout.Render(logEvent);
    }

    protected virtual byte[] GetBytesToWrite(LogEventInfo logEvent)
    {
      string formattedMessage = this.GetFormattedMessage(logEvent);
      int byteCount1 = this.Encoding.GetByteCount(formattedMessage);
      int byteCount2 = this.Encoding.GetByteCount(this.NewLineChars);
      byte[] bytes = new byte[byteCount1 + byteCount2];
      this.Encoding.GetBytes(formattedMessage, 0, formattedMessage.Length, bytes, 0);
      this.Encoding.GetBytes(this.NewLineChars, 0, this.NewLineChars.Length, bytes, byteCount1);
      return this.TransformBytes(bytes);
    }

    protected virtual byte[] TransformBytes(byte[] value) => value;

    protected virtual void RenderFormattedMessageToStream(
      LogEventInfo logEvent,
      StringBuilder formatBuilder,
      char[] transformBuffer,
      MemoryStream streamTarget)
    {
      this.RenderFormattedMessage(logEvent, formatBuilder);
      formatBuilder.Append(this.NewLineChars);
      this.TransformBuilderToStream(logEvent, formatBuilder, transformBuffer, streamTarget);
    }

    protected virtual void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      this.Layout.RenderAppendBuilder(logEvent, target);
    }

    private void TransformBuilderToStream(
      LogEventInfo logEvent,
      StringBuilder builder,
      char[] transformBuffer,
      MemoryStream workStream)
    {
      builder.CopyToStream(workStream, this.Encoding, transformBuffer);
      this.TransformStream(logEvent, workStream);
    }

    protected virtual void TransformStream(LogEventInfo logEvent, MemoryStream stream)
    {
    }

    private void FlushCurrentFileWrites(
      string currentFileName,
      LogEventInfo firstLogEvent,
      MemoryStream ms,
      out Exception lastException)
    {
      lastException = (Exception) null;
      try
      {
        if (currentFileName == null)
          return;
        ArraySegment<byte> bytesToWrite = new ArraySegment<byte>(ms.GetBuffer(), 0, (int) ms.Length);
        this.ProcessLogEvent(firstLogEvent, currentFileName, bytesToWrite);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
        else
          lastException = ex;
      }
    }

    private void ArchiveFile(string fileName, string archiveFileName)
    {
      string directoryName = Path.GetDirectoryName(archiveFileName);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      if (string.Equals(fileName, archiveFileName, StringComparison.OrdinalIgnoreCase))
        InternalLogger.Info<string, string>("FileTarget(Name={0}): Archiving {1} skipped as ArchiveFileName equals FileName", this.Name, fileName);
      else if (this.EnableArchiveFileCompression)
      {
        InternalLogger.Info<string, string, string>("FileTarget(Name={0}): Archiving {1} to compressed {2}", this.Name, fileName, archiveFileName);
        FileTarget.FileCompressor.CompressFile(fileName, archiveFileName);
        this.DeleteAndWaitForFileDelete(fileName);
      }
      else
      {
        InternalLogger.Info<string, string, string>("FileTarget(Name={0}): Archiving {1} to {2}", this.Name, fileName, archiveFileName);
        if (File.Exists(archiveFileName))
        {
          InternalLogger.Info<string, string>("FileTarget(Name={0}): Already exists, append to {1}", this.Name, archiveFileName);
          using (FileStream input = File.Open(fileName, FileMode.Open))
          {
            using (FileStream output = File.Open(archiveFileName, FileMode.Append))
            {
              input.CopyAndSkipBom((Stream) output, this.Encoding);
              input.SetLength(0L);
              input.Close();
              output.Flush(true);
            }
          }
        }
        else
        {
          try
          {
            InternalLogger.Debug<string, string, string>("FileTarget(Name={0}): Move file from '{1}' to '{2}'", this.Name, fileName, archiveFileName);
            File.Move(fileName, archiveFileName);
          }
          catch (IOException ex)
          {
            if (this.KeepFileOpen && !this.ConcurrentWrites)
              throw;
            else if (!this.EnableFileDelete && this.KeepFileOpen)
              throw;
            else if (!PlatformDetector.SupportsSharableMutex)
            {
              throw;
            }
            else
            {
              object[] objArray = new object[3]
              {
                (object) this.Name,
                (object) fileName,
                (object) archiveFileName
              };
              InternalLogger.Warn((Exception) ex, "FileTarget(Name={0}): Archiving failed. Checking for retry move of {1} to {2}.", objArray);
              if (!File.Exists(fileName) || File.Exists(archiveFileName))
              {
                throw;
              }
              else
              {
                AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds(50.0));
                if (!File.Exists(fileName) || File.Exists(archiveFileName))
                {
                  throw;
                }
                else
                {
                  InternalLogger.Debug<string, string, string>("FileTarget(Name={0}): Archiving retrying move of {1} to {2}.", this.Name, fileName, archiveFileName);
                  File.Move(fileName, archiveFileName);
                }
              }
            }
          }
        }
      }
    }

    private bool DeleteOldArchiveFile(string fileName)
    {
      try
      {
        InternalLogger.Info<string, string>("FileTarget(Name={0}): Deleting old archive file: '{1}'.", this.Name, fileName);
        File.Delete(fileName);
        return true;
      }
      catch (DirectoryNotFoundException ex)
      {
        object[] objArray = new object[2]
        {
          (object) this.Name,
          (object) fileName
        };
        InternalLogger.Debug((Exception) ex, "FileTarget(Name={0}): Failed to delete old log file '{1}' as directory is missing.", objArray);
        return false;
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to delete old archive file: '{1}'.", (object) this.Name, (object) fileName);
        if (!ex.MustBeRethrown())
          return false;
        throw;
      }
    }

    private void DeleteAndWaitForFileDelete(string fileName)
    {
      try
      {
        InternalLogger.Trace<string, string>("FileTarget(Name={0}): Waiting for file delete of '{1}' for 12 sec", this.Name, fileName);
        DateTime creationTime = new FileInfo(fileName).CreationTime;
        if (!this.DeleteOldArchiveFile(fileName) || !File.Exists(fileName))
          return;
        for (int index = 0; index < 120; ++index)
        {
          AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds(100.0));
          FileInfo fileInfo = new FileInfo(fileName);
          if (!fileInfo.Exists || fileInfo.CreationTime != creationTime)
            return;
        }
        InternalLogger.Warn<string, string>("FileTarget(Name={0}): Timeout while deleting old archive file: '{1}'.", this.Name, fileName);
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to delete old archive file: '{1}'.", (object) this.Name, (object) fileName);
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    private string GetArchiveDateFormatString(string defaultFormat)
    {
      string dateFormatString = defaultFormat;
      if (string.IsNullOrEmpty(dateFormatString))
      {
        switch (this.ArchiveEvery)
        {
          case FileArchivePeriod.Year:
            dateFormatString = "yyyy";
            break;
          case FileArchivePeriod.Month:
            dateFormatString = "yyyyMM";
            break;
          case FileArchivePeriod.Hour:
            dateFormatString = "yyyyMMddHH";
            break;
          case FileArchivePeriod.Minute:
            dateFormatString = "yyyyMMddHHmm";
            break;
          default:
            dateFormatString = "yyyyMMdd";
            break;
        }
      }
      return dateFormatString;
    }

    private DateTime? GetArchiveDate(
      string fileName,
      LogEventInfo logEvent,
      bool initializedNewFile)
    {
      DateTime? creationTimeSource = this._fileAppenderCache.GetFileCreationTimeSource(fileName, true);
      DateTime? archiveDate = string.Equals(fileName, this._previousLogFileName, StringComparison.OrdinalIgnoreCase) ? this._previousLogEventTimestamp : new DateTime?();
      DateTime dateTime;
      if (!archiveDate.HasValue && !initializedNewFile && this._initializedFiles.TryGetValue(fileName, out dateTime))
        archiveDate = new DateTime?(dateTime);
      InternalLogger.Trace<string, DateTime?, DateTime?>("FileTarget(Name={0}): Calculating archive date. Last write time: {1}; Previous log event time: {2}", this.Name, creationTimeSource, archiveDate);
      if (!creationTimeSource.HasValue)
      {
        if (!archiveDate.HasValue)
          InternalLogger.Info<string, string>("FileTarget(Name={0}): Unable to acquire useful timestamp to archive file: {1}", this.Name, fileName);
        return archiveDate;
      }
      if (creationTimeSource.HasValue && archiveDate.HasValue)
      {
        if (archiveDate.Value > creationTimeSource.Value)
        {
          InternalLogger.Trace<string>("FileTarget(Name={0}): Using previous log event time (is more recent)", this.Name);
          return new DateTime?(archiveDate.Value);
        }
        if (this.PreviousLogOverlappedPeriod(logEvent, archiveDate.Value, creationTimeSource.Value))
        {
          InternalLogger.Trace<string>("FileTarget(Name={0}): Using previous log event time (previous log overlapped period)", this.Name);
          return new DateTime?(archiveDate.Value);
        }
      }
      InternalLogger.Trace<string>("FileTarget(Name={0}): Using last write time", this.Name);
      return new DateTime?(creationTimeSource.Value);
    }

    private bool PreviousLogOverlappedPeriod(
      LogEventInfo logEvent,
      DateTime previousLogEventTimestamp,
      DateTime lastFileWrite)
    {
      DateTime previousLogEventTimestamp1 = previousLogEventTimestamp;
      string dateFormatString = this.GetArchiveDateFormatString(string.Empty);
      string str1 = lastFileWrite.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
      string str2 = logEvent.TimeStamp.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
      if (str1 != str2)
        return false;
      DateTime dateTime;
      switch (this.ArchiveEvery)
      {
        case FileArchivePeriod.Year:
          dateTime = previousLogEventTimestamp1.AddYears(1);
          break;
        case FileArchivePeriod.Month:
          dateTime = previousLogEventTimestamp1.AddMonths(1);
          break;
        case FileArchivePeriod.Day:
          dateTime = previousLogEventTimestamp1.AddDays(1.0);
          break;
        case FileArchivePeriod.Hour:
          dateTime = previousLogEventTimestamp1.AddHours(1.0);
          break;
        case FileArchivePeriod.Minute:
          dateTime = previousLogEventTimestamp1.AddMinutes(1.0);
          break;
        case FileArchivePeriod.Sunday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Sunday);
          break;
        case FileArchivePeriod.Monday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Monday);
          break;
        case FileArchivePeriod.Tuesday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Tuesday);
          break;
        case FileArchivePeriod.Wednesday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Wednesday);
          break;
        case FileArchivePeriod.Thursday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Thursday);
          break;
        case FileArchivePeriod.Friday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Friday);
          break;
        case FileArchivePeriod.Saturday:
          dateTime = FileTarget.CalculateNextWeekday(previousLogEventTimestamp1, DayOfWeek.Saturday);
          break;
        default:
          return false;
      }
      string str3 = dateTime.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
      return str1 == str3;
    }

    public static DateTime CalculateNextWeekday(
      DateTime previousLogEventTimestamp,
      DayOfWeek dayOfWeek)
    {
      int dayOfWeek1 = (int) previousLogEventTimestamp.DayOfWeek;
      int num = (int) dayOfWeek;
      if (num <= dayOfWeek1)
        num += 7;
      return previousLogEventTimestamp.AddDays((double) (num - dayOfWeek1));
    }

    private void DoAutoArchive(string fileName, LogEventInfo eventInfo, bool initializedNewFile)
    {
      InternalLogger.Debug<string, string>("FileTarget(Name={0}): Do archive file: '{1}'", this.Name, fileName);
      FileInfo fileInfo = new FileInfo(fileName);
      if (!fileInfo.Exists)
      {
        this._fileAppenderCache.InvalidateAppender(fileName)?.Dispose();
        this._initializedFiles.Remove(fileName);
      }
      else
      {
        string archiveFileNamePattern = this.GetArchiveFileNamePattern(fileName, eventInfo);
        if (string.IsNullOrEmpty(archiveFileNamePattern))
        {
          InternalLogger.Warn<string>("FileTarget(Name={0}): Skip auto archive because archiveFilePattern is NULL", this.Name);
        }
        else
        {
          InternalLogger.Trace<string, string>("FileTarget(Name={0}): Archive pattern '{1}'", this.Name, archiveFileNamePattern);
          IFileArchiveMode fileArchiveHelper = this.GetFileArchiveHelper(archiveFileNamePattern);
          List<DateAndSequenceArchive> existingArchiveFiles = fileArchiveHelper.GetExistingArchiveFiles(archiveFileNamePattern);
          if (this.MaxArchiveFiles == 1)
          {
            InternalLogger.Trace<string>("FileTarget(Name={0}): MaxArchiveFiles = 1", this.Name);
            for (int index = existingArchiveFiles.Count - 1; index >= 0; --index)
            {
              DateAndSequenceArchive andSequenceArchive = existingArchiveFiles[index];
              if (!string.Equals(andSequenceArchive.FileName, fileInfo.FullName, StringComparison.OrdinalIgnoreCase))
              {
                this.DeleteOldArchiveFile(andSequenceArchive.FileName);
                existingArchiveFiles.RemoveAt(index);
              }
            }
            if (initializedNewFile && string.Equals(Path.GetDirectoryName(archiveFileNamePattern), fileInfo.DirectoryName, StringComparison.OrdinalIgnoreCase))
            {
              this._initializedFiles.Remove(fileName);
              this.DeleteOldArchiveFile(fileName);
              return;
            }
          }
          DateTime? archiveDate = this.GetArchiveDate(fileName, eventInfo, initializedNewFile);
          DateAndSequenceArchive archiveFileName = archiveDate.HasValue ? fileArchiveHelper.GenerateArchiveFileName(archiveFileNamePattern, archiveDate.Value, existingArchiveFiles) : (DateAndSequenceArchive) null;
          if (archiveFileName == null)
            return;
          if (initializedNewFile)
            this._initializedFiles.Remove(fileName);
          else
            this.FinalizeFile(fileName, true);
          if (string.Equals(Path.GetDirectoryName(archiveFileName.FileName), fileInfo.DirectoryName, StringComparison.OrdinalIgnoreCase))
          {
            for (int index = 0; index < existingArchiveFiles.Count; ++index)
            {
              if (string.Equals(existingArchiveFiles[index].FileName, fileInfo.FullName, StringComparison.OrdinalIgnoreCase))
              {
                existingArchiveFiles.RemoveAt(index);
                break;
              }
            }
          }
          existingArchiveFiles.Add(archiveFileName);
          foreach (DateAndSequenceArchive andSequenceArchive in fileArchiveHelper.CheckArchiveCleanup(archiveFileNamePattern, existingArchiveFiles, this.MaxArchiveFiles))
            this.DeleteOldArchiveFile(andSequenceArchive.FileName);
          this.ArchiveFile(fileInfo.FullName, archiveFileName.FileName);
        }
      }
    }

    private string GetArchiveFileNamePattern(string fileName, LogEventInfo eventInfo)
    {
      if (this._fullArchiveFileName != null)
        return this._fullArchiveFileName.Render(eventInfo);
      return this.EnableArchiveFileCompression ? Path.ChangeExtension(fileName, ".zip") : fileName;
    }

    private bool TryArchiveFile(
      string fileName,
      LogEventInfo ev,
      int upcomingWriteSize,
      bool initializedNewFile)
    {
      if (!this.IsArchivingEnabled)
        return false;
      string str = string.Empty;
      BaseFileAppender baseFileAppender1 = (BaseFileAppender) null;
      try
      {
        str = this.GetArchiveFileName(fileName, ev, upcomingWriteSize);
        if (!string.IsNullOrEmpty(str))
        {
          InternalLogger.Trace<string, string>("FileTarget(Name={0}): Archive attempt for file '{1}'", this.Name, str);
          baseFileAppender1 = this._fileAppenderCache.InvalidateAppender(fileName);
          if (fileName != str)
          {
            BaseFileAppender baseFileAppender2 = this._fileAppenderCache.InvalidateAppender(str);
            baseFileAppender1 = baseFileAppender1 ?? baseFileAppender2;
          }
          if (!string.IsNullOrEmpty(this._previousLogFileName) && this._previousLogFileName != str && this._previousLogFileName != fileName)
          {
            BaseFileAppender baseFileAppender3 = this._fileAppenderCache.InvalidateAppender(this._previousLogFileName);
            baseFileAppender1 = baseFileAppender1 ?? baseFileAppender3;
          }
          this._fileAppenderCache.InvalidateAppendersForArchivedFiles();
        }
        else
          this._fileAppenderCache.InvalidateAppendersForArchivedFiles();
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to check archive for file '{1}'.", (object) this.Name, (object) fileName);
        if (ex.MustBeRethrown())
          throw;
      }
      if (!string.IsNullOrEmpty(str))
      {
        try
        {
          try
          {
            if (baseFileAppender1 is BaseMutexFileAppender mutexFileAppender)
            {
              mutexFileAppender.ArchiveMutex?.WaitOne();
            }
            else
            {
              if (this.KeepFileOpen)
              {
                if (!this.ConcurrentWrites)
                  goto label_20;
              }
              InternalLogger.Info<string, string>("FileTarget(Name={0}): Archive mutex not available: {1}", this.Name, str);
            }
          }
          catch (AbandonedMutexException ex)
          {
          }
label_20:
          string archiveFileName = this.GetArchiveFileName(fileName, ev, upcomingWriteSize);
          if (string.IsNullOrEmpty(archiveFileName))
          {
            InternalLogger.Trace<string, string>("FileTarget(Name={0}): Archive already performed for file '{1}'", this.Name, str);
            if (str != fileName)
              this._initializedFiles.Remove(fileName);
            this._initializedFiles.Remove(str);
          }
          else
          {
            str = archiveFileName;
            this.DoAutoArchive(str, ev, initializedNewFile);
          }
          if (this._previousLogFileName == str)
          {
            this._previousLogFileName = (string) null;
            this._previousLogEventTimestamp = new DateTime?();
          }
          return true;
        }
        catch (Exception ex)
        {
          InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to archive file '{1}'.", (object) this.Name, (object) str);
          if (ex.MustBeRethrown())
            throw;
        }
        finally
        {
          if (baseFileAppender1 is BaseMutexFileAppender mutexFileAppender)
            mutexFileAppender.ArchiveMutex?.ReleaseMutex();
          baseFileAppender1?.Dispose();
        }
      }
      return false;
    }

    private string GetArchiveFileName(string fileName, LogEventInfo ev, int upcomingWriteSize)
    {
      return (fileName != null ? 1 : (this._previousLogFileName != null ? 1 : 0)) != 0 ? this.GetArchiveFileNameBasedOnFileSize(fileName, upcomingWriteSize) ?? this.GetArchiveFileNameBasedOnTime(fileName, ev) : (string) null;
    }

    private string GetPotentialFileForArchiving(string fileName)
    {
      return string.Equals(fileName, this._previousLogFileName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(this._previousLogFileName) || !string.IsNullOrEmpty(fileName) && this._fileAppenderCache.GetFileLength(fileName, true).HasValue ? fileName : this._previousLogFileName;
    }

    private string GetArchiveFileNameBasedOnFileSize(string fileName, int upcomingWriteSize)
    {
      if (this.ArchiveAboveSize == -1L)
        return (string) null;
      string fileForArchiving = this.GetPotentialFileForArchiving(fileName);
      if (fileForArchiving == null)
        return (string) null;
      long? fileLength = this._fileAppenderCache.GetFileLength(fileForArchiving, true);
      if (!fileLength.HasValue)
        return (string) null;
      if (fileForArchiving != fileName)
        upcomingWriteSize = 0;
      return fileLength.Value + (long) upcomingWriteSize > this.ArchiveAboveSize ? fileForArchiving : (string) null;
    }

    private string GetArchiveFileNameBasedOnTime(string fileName, LogEventInfo logEvent)
    {
      if (this.ArchiveEvery == FileArchivePeriod.None)
        return (string) null;
      fileName = this.GetPotentialFileForArchiving(fileName);
      if (fileName == null)
        return (string) null;
      DateTime? creationTimeSource = this._fileAppenderCache.GetFileCreationTimeSource(fileName, true);
      if (!creationTimeSource.HasValue)
        return (string) null;
      if (FileTarget.TruncateArchiveTime(creationTimeSource.Value, this.ArchiveEvery) != FileTarget.TruncateArchiveTime(logEvent.TimeStamp, this.ArchiveEvery))
      {
        string dateFormatString = this.GetArchiveDateFormatString(string.Empty);
        if (creationTimeSource.Value.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture) != logEvent.TimeStamp.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture))
          return fileName;
      }
      return (string) null;
    }

    private static DateTime TruncateArchiveTime(DateTime input, FileArchivePeriod resolution)
    {
      switch (resolution)
      {
        case FileArchivePeriod.Year:
          return new DateTime(input.Year, 1, 1, 0, 0, 0, 0, input.Kind);
        case FileArchivePeriod.Month:
          return new DateTime(input.Year, input.Month, 1, 0, 0, 0, input.Kind);
        case FileArchivePeriod.Day:
          return input.Date;
        case FileArchivePeriod.Hour:
          return input.AddTicks(-(input.Ticks % 36000000000L));
        case FileArchivePeriod.Minute:
          return input.AddTicks(-(input.Ticks % 600000000L));
        case FileArchivePeriod.Sunday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Sunday);
        case FileArchivePeriod.Monday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Monday);
        case FileArchivePeriod.Tuesday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Tuesday);
        case FileArchivePeriod.Wednesday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Wednesday);
        case FileArchivePeriod.Thursday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Thursday);
        case FileArchivePeriod.Friday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Friday);
        case FileArchivePeriod.Saturday:
          return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Saturday);
        default:
          return input;
      }
    }

    private void AutoClosingTimerCallback(object sender, EventArgs state)
    {
      try
      {
        lock (this.SyncRoot)
        {
          if (!this.IsInitialized)
            return;
          DateTime expireTime = this.OpenFileCacheTimeout > 0 ? DateTime.UtcNow.AddSeconds((double) -this.OpenFileCacheTimeout) : DateTime.MinValue;
          InternalLogger.Trace<string>("FileTarget(Name={0}): Stop CloseAppenders", this.Name);
          this._fileAppenderCache.CloseAppenders(expireTime);
        }
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "FileTarget(Name={0}): Exception in AutoClosingTimerCallback", (object) this.Name);
        if (!ex.MustBeRethrownImmediately())
          return;
        throw;
      }
    }

    private void WriteToFile(string fileName, ArraySegment<byte> bytes, bool initializedNewFile)
    {
      if (this.ReplaceFileContentsOnEachWrite)
      {
        this.ReplaceFileContent(fileName, bytes, true);
      }
      else
      {
        BaseFileAppender appender = this._fileAppenderCache.AllocateAppender(fileName);
        try
        {
          if (initializedNewFile)
            this.WriteHeaderAndBom(appender);
          appender.Write(bytes.Array, bytes.Offset, bytes.Count);
          if (!this.AutoFlush)
            return;
          appender.Flush();
        }
        catch (Exception ex)
        {
          object[] objArray = new object[2]
          {
            (object) this.Name,
            (object) fileName
          };
          InternalLogger.Error(ex, "FileTarget(Name={0}): Failed write to file '{1}'.", objArray);
          this._fileAppenderCache.InvalidateAppender(fileName)?.Dispose();
          throw;
        }
      }
    }

    private bool InitializeFile(string fileName, LogEventInfo logEvent)
    {
      bool flag = false;
      if (this._initializedFiles.Count != 0 && this._previousLogEventTimestamp.HasValue && this._previousLogFileName == fileName && logEvent.TimeStamp == this._previousLogEventTimestamp.Value)
        return false;
      DateTime timeStamp = logEvent.TimeStamp;
      DateTime dateTime;
      if (!this._initializedFiles.TryGetValue(fileName, out dateTime))
      {
        this.ProcessOnStartup(fileName, logEvent);
        ++this._initializedFilesCounter;
        if (this._initializedFilesCounter >= 25)
        {
          this._initializedFilesCounter = 0;
          this.CleanupInitializedFiles();
        }
        this._initializedFiles[fileName] = timeStamp;
        flag = true;
      }
      else if (dateTime != timeStamp)
        this._initializedFiles[fileName] = timeStamp;
      return flag;
    }

    private void FinalizeFile(string fileName, bool isArchiving = false)
    {
      InternalLogger.Trace<string, string, bool>("FileTarget(Name={0}): FinalizeFile '{1}, isArchiving: {2}'", this.Name, fileName, isArchiving);
      if (isArchiving || !this.WriteFooterOnArchivingOnly)
        this.WriteFooter(fileName);
      this._fileAppenderCache.InvalidateAppender(fileName)?.Dispose();
      this._initializedFiles.Remove(fileName);
    }

    private void WriteFooter(string fileName)
    {
      ArraySegment<byte> layoutBytes = this.GetLayoutBytes(this.Footer);
      if (layoutBytes.Count <= 0 || !File.Exists(fileName))
        return;
      this.WriteToFile(fileName, layoutBytes, false);
    }

    private void ProcessOnStartup(string fileName, LogEventInfo logEvent)
    {
      InternalLogger.Debug<string, string>("FileTarget(Name={0}): Process file '{1}' on startup", this.Name, fileName);
      this.RefreshArchiveFilePatternToWatch(fileName, logEvent);
      if (this.ArchiveOldFileOnStartup)
      {
        try
        {
          this.DoAutoArchive(fileName, logEvent, true);
        }
        catch (Exception ex)
        {
          InternalLogger.Warn(ex, "FileTarget(Name={0}): Unable to archive old log file '{1}'.", (object) this.Name, (object) fileName);
          if (ex.MustBeRethrown())
            throw;
        }
      }
      if (this.DeleteOldFileOnStartup)
        this.DeleteOldArchiveFile(fileName);
      string archiveFileNamePattern = this.GetArchiveFileNamePattern(fileName, logEvent);
      if (string.IsNullOrEmpty(archiveFileNamePattern) || !FileArchiveModeFactory.ShouldDeleteOldArchives(this.MaxArchiveFiles))
        return;
      IFileArchiveMode fileArchiveHelper = this.GetFileArchiveHelper(archiveFileNamePattern);
      if (!fileArchiveHelper.AttemptCleanupOnInitializeFile(archiveFileNamePattern, this.MaxArchiveFiles))
        return;
      List<DateAndSequenceArchive> existingArchiveFiles = fileArchiveHelper.GetExistingArchiveFiles(archiveFileNamePattern);
      foreach (DateAndSequenceArchive andSequenceArchive in fileArchiveHelper.CheckArchiveCleanup(archiveFileNamePattern, existingArchiveFiles, this.MaxArchiveFiles))
        this.DeleteOldArchiveFile(andSequenceArchive.FileName);
    }

    private void ReplaceFileContent(string fileName, ArraySegment<byte> bytes, bool firstAttempt)
    {
      try
      {
        using (FileStream fileStream = File.Create(fileName))
        {
          ArraySegment<byte> layoutBytes1 = this.GetLayoutBytes(this.Header);
          if (layoutBytes1.Count > 0)
            fileStream.Write(layoutBytes1.Array, layoutBytes1.Offset, layoutBytes1.Count);
          fileStream.Write(bytes.Array, bytes.Offset, bytes.Count);
          ArraySegment<byte> layoutBytes2 = this.GetLayoutBytes(this.Footer);
          if (layoutBytes2.Count <= 0)
            return;
          fileStream.Write(layoutBytes2.Array, layoutBytes2.Offset, layoutBytes2.Count);
        }
      }
      catch (DirectoryNotFoundException ex)
      {
        if (!this.CreateDirs || !firstAttempt)
        {
          throw;
        }
        else
        {
          Directory.CreateDirectory(Path.GetDirectoryName(fileName));
          this.ReplaceFileContent(fileName, bytes, false);
        }
      }
    }

    private void WriteHeaderAndBom(BaseFileAppender appender)
    {
      if (this.Header == null && !this.WriteBom)
        return;
      long? fileLength = appender.GetFileLength();
      if (fileLength.HasValue)
      {
        long? nullable = fileLength;
        long num = 0;
        if ((nullable.GetValueOrDefault() == num ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          return;
      }
      if (this.WriteBom)
      {
        InternalLogger.Trace<string, Encoding>("FileTarget(Name={0}): Write byte order mark from encoding={1}", this.Name, this.Encoding);
        byte[] preamble = this.Encoding.GetPreamble();
        if (preamble.Length != 0)
          appender.Write(preamble, 0, preamble.Length);
      }
      if (this.Header == null)
        return;
      InternalLogger.Trace<string>("FileTarget(Name={0}): Write header", this.Name);
      ArraySegment<byte> layoutBytes = this.GetLayoutBytes(this.Header);
      if (layoutBytes.Count <= 0)
        return;
      appender.Write(layoutBytes.Array, layoutBytes.Offset, layoutBytes.Count);
    }

    private ArraySegment<byte> GetLayoutBytes(Layout layout)
    {
      if (layout == null)
        return new ArraySegment<byte>();
      if (!this.OptimizeBufferReuse)
        return new ArraySegment<byte>(this.TransformBytes(this.Encoding.GetBytes(layout.Render(LogEventInfo.CreateNullEvent()) + this.NewLineChars)));
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject1 = this.ReusableLayoutBuilder.Allocate())
      {
        using (ReusableObjectCreator<char[]>.LockOject lockOject2 = this._reusableEncodingBuffer.Allocate())
        {
          LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
          layout.RenderAppendBuilder(nullEvent, lockOject1.Result);
          lockOject1.Result.Append(this.NewLineChars);
          using (MemoryStream workStream = new MemoryStream(lockOject1.Result.Length))
          {
            this.TransformBuilderToStream(nullEvent, lockOject1.Result, lockOject2.Result, workStream);
            return new ArraySegment<byte>(workStream.ToArray());
          }
        }
      }
    }
  }
}
