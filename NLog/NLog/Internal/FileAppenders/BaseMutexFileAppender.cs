// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.BaseMutexFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Internal.FileAppenders
{
  [SecuritySafeCritical]
  internal abstract class BaseMutexFileAppender : BaseFileAppender
  {
    protected BaseMutexFileAppender(string fileName, ICreateFileParameters createParameters)
      : base(fileName, createParameters)
    {
      if (!createParameters.IsArchivingEnabled)
        return;
      this.ArchiveMutex = this.CreateArchiveMutex();
    }

    public Mutex ArchiveMutex { get; private set; }

    private Mutex CreateArchiveMutex()
    {
      try
      {
        return this.CreateSharableMutex("FileArchiveLock");
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case SecurityException _:
          case NotSupportedException _:
          case NotImplementedException _:
            InternalLogger.Warn(ex, "Failed to create global archive mutex: {0}", (object) this.FileName);
            return new Mutex();
          default:
            InternalLogger.Error(ex, "Failed to create global archive mutex: {0}", (object) this.FileName);
            if (!ex.MustBeRethrown())
              return new Mutex();
            throw;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      this.ArchiveMutex?.Close();
    }

    protected Mutex CreateSharableMutex(string mutexNamePrefix)
    {
      if (!PlatformDetector.SupportsSharableMutex)
        return new Mutex();
      string mutexName = this.GetMutexName(mutexNamePrefix);
      MutexSecurity mutexSecurity = new MutexSecurity();
      SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier) null);
      mutexSecurity.AddAccessRule(new MutexAccessRule((IdentityReference) identity, MutexRights.FullControl, AccessControlType.Allow));
      bool createdNew;
      return new Mutex(false, mutexName, out createdNew, mutexSecurity);
    }

    private string GetMutexName(string mutexNamePrefix)
    {
      string s = Path.GetFullPath(this.FileName).ToLowerInvariant().Replace('\\', '_').Replace('/', '_');
      string mutexName = string.Format("Global\\NLog-File{0}-{1}", (object) mutexNamePrefix, (object) s);
      if (mutexName.Length <= 260)
        return mutexName;
      string base64String;
      using (MD5 md5 = MD5.Create())
        base64String = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(s)));
      string str = string.Format("Global\\NLog-File{0}-{1}", (object) mutexNamePrefix, (object) base64String);
      int startIndex = s.Length - (260 - str.Length);
      return str + s.Substring(startIndex);
    }
  }
}
