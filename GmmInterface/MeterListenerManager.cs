// Decompiled with JetBrains decompiler
// Type: ZENNER.MeterListenerManager
// Assembly: GmmInterface, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 25F1E48F-52B7-4A4F-B66A-62C91CCF5C52
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmInterface.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using MinomatListener;
using NLog;
using StartupLib;
using System;
using System.Globalization;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ZENNER
{
  public sealed class MeterListenerManager : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeterListenerManager));

    public Server Server { get; private set; }

    public bool StoreLoggerDataToDatabase { get; set; }

    public MeterListenerManager()
    {
      this.Server = new Server();
      this.Server.OnError += new EventHandler<Exception>(this.Server_OnError);
      this.Server.OnJobCompleted += new EventHandler<Job>(this.Server_OnJobCompleted);
      this.Server.OnJobStarted += new EventHandler<Job>(this.Server_OnJobStarted);
      this.Server.OnMinomatConnected += new EventHandler<MinomatDevice>(this.Server_OnMinomatConnected);
      this.Server.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.Server_ValueIdentSetReceived);
    }

    public event EventHandler<ValueIdentSet> ValueIdentSetReceived;

    public event EventHandler<Exception> OnError;

    public event EventHandler<Job> OnJobStarted;

    public event EventHandler<Job> OnJobCompleted;

    public event EventHandler<MinomatDevice> OnMinomatConnected;

    internal void AddJob(Job job) => this.Server.AddJob(job);

    internal void RemoveJob(Guid jobID) => this.Server.RemoveJob(jobID);

    internal void RemoveJob(Job job) => this.Server.RemoveJob(job);

    public void Dispose()
    {
      this.Server.OnError -= new EventHandler<Exception>(this.Server_OnError);
      this.Server.OnJobCompleted -= new EventHandler<Job>(this.Server_OnJobCompleted);
      this.Server.OnJobStarted -= new EventHandler<Job>(this.Server_OnJobStarted);
      this.Server.OnMinomatConnected -= new EventHandler<MinomatDevice>(this.Server_OnMinomatConnected);
      this.Server.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.Server_ValueIdentSetReceived);
      this.Server.Dispose();
      StartupManager.Dispose();
    }

    private void Server_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      if (this.ValueIdentSetReceived == null)
        return;
      this.ValueIdentSetReceived(sender, e);
    }

    private void Server_OnJobStarted(object sender, Job e)
    {
      if (this.OnJobStarted == null)
        return;
      this.OnJobStarted(sender, e);
    }

    private void Server_OnJobCompleted(object sender, Job e)
    {
      if (this.OnJobCompleted == null)
        return;
      this.OnJobCompleted(sender, e);
    }

    private void Server_OnError(object sender, Exception e)
    {
      if (this.OnError == null)
        return;
      this.OnError(sender, e);
    }

    private void Server_OnMinomatConnected(object sender, MinomatDevice e)
    {
      if (this.OnMinomatConnected == null)
        return;
      this.OnMinomatConnected(sender, e);
    }

    public static void AddMinomat(MinomatDevice minomatDevice)
    {
      if (minomatDevice == null)
        throw new ArgumentNullException(nameof (minomatDevice));
      if (!minomatDevice.GsmID.HasValue)
        throw new ArgumentNullException("GsmID can not be null!");
      if (!minomatDevice.ChallengeKey.HasValue)
        throw new ArgumentNullException("ChallengeKey can not be null!");
      if (!minomatDevice.SessionKey.HasValue)
        throw new ArgumentNullException("SessionKey can not be null!");
      if (MinomatList.AddMinomatList(DbBasis.PrimaryDB.BaseDbConnection, minomatDevice.GsmID.Value, minomatDevice.ChallengeKey.Value, minomatDevice.SessionKey.Value) == null)
        throw new Exception("Failed to add the Minomat Master device to the database!");
    }

    public static MinomatDevice GetMinomat(uint gsmID)
    {
      DriverTables.MinomatListRow minomatList = MinomatList.GetMinomatList(DbBasis.PrimaryDB.BaseDbConnection, gsmID);
      if (minomatList == null)
        return (MinomatDevice) null;
      MinomatDevice minomat = new MinomatDevice();
      minomat.GsmID = new uint?(gsmID);
      uint result1;
      if (!minomatList.IsMinolIDNull() && uint.TryParse(minomatList.MinolID, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result1))
        minomat.MinolID = new uint?(result1);
      uint result2;
      if (!minomatList.IsChallengeKeyNull() && uint.TryParse(minomatList.ChallengeKey, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result2))
        minomat.ChallengeKey = new uint?(result2);
      ulong result3;
      if (!minomatList.IsSessionKeyNull() && ulong.TryParse(minomatList.SessionKey, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result3))
        minomat.SessionKey = new ulong?(result3);
      uint result4;
      if (!minomatList.IsChallengeKeyOldNull() && uint.TryParse(minomatList.ChallengeKeyOld, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result4))
        minomat.ChallengeKeyOld = new uint?(result4);
      ulong result5;
      if (!minomatList.IsSessionKeyOldNull() && ulong.TryParse(minomatList.SessionKeyOld, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result5))
        minomat.SessionKeyOld = new ulong?(result5);
      uint result6;
      if (!minomatList.IsGsmIDEncodedNull() && uint.TryParse(minomatList.GsmIDEncoded, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result6))
        minomat.GsmIDEncoded = new uint?(result6);
      uint result7;
      if (!minomatList.IsChallengeKeyEncodedNull() && uint.TryParse(minomatList.ChallengeKeyEncoded, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result7))
        minomat.ChallengeKeyEncoded = new uint?(result7);
      uint result8;
      if (!minomatList.IsGsmIDEncodedOldNull() && uint.TryParse(minomatList.GsmIDEncodedOld, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result8))
        minomat.GsmIDEncodedOld = new uint?(result8);
      uint result9;
      if (!minomatList.IsChallengeKeyEncodedOldNull() && uint.TryParse(minomatList.ChallengeKeyEncodedOld, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result9))
        minomat.ChallengeKeyEncodedOld = new uint?(result9);
      return minomat;
    }

    public static bool DeleteMinomat(uint gsmID)
    {
      return MinomatList.DeleteMinomatList(DbBasis.PrimaryDB.BaseDbConnection, gsmID);
    }

    public static ulong? CreateRandomSessionKey() => new ulong?(Util.GetSecureRandomUInt64());

    public static uint? CreateRandomChallengeKey() => new uint?(Util.GetSecureRandomUInt32());
  }
}
