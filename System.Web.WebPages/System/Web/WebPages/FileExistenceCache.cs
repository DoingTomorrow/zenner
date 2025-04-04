// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.FileExistenceCache
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Web.Hosting;

#nullable disable
namespace System.Web.WebPages
{
  internal class FileExistenceCache
  {
    private const int TickPerMiliseconds = 10000;
    private readonly VirtualPathProvider _virtualPathProvider;
    private ConcurrentDictionary<string, bool> _cache;
    private long _creationTick;
    private int _ticksBeforeReset;

    public FileExistenceCache(VirtualPathProvider virtualPathProvider, int milliSecondsBeforeReset = 1000)
    {
      this._virtualPathProvider = virtualPathProvider;
      this._ticksBeforeReset = milliSecondsBeforeReset * 10000;
      this.Reset();
    }

    public VirtualPathProvider VirtualPathProvider => this._virtualPathProvider;

    public int MilliSecondsBeforeReset
    {
      get => this._ticksBeforeReset / 10000;
      internal set => this._ticksBeforeReset = value * 10000;
    }

    internal IDictionary<string, bool> CacheInternal => (IDictionary<string, bool>) this._cache;

    public bool TimeExceeded
    {
      get
      {
        return DateTime.UtcNow.Ticks - Interlocked.Read(ref this._creationTick) > (long) this._ticksBeforeReset;
      }
    }

    public void Reset()
    {
      this._cache = new ConcurrentDictionary<string, bool>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      Interlocked.Exchange(ref this._creationTick, DateTime.UtcNow.Ticks);
    }

    public bool FileExists(string virtualPath)
    {
      if (this.TimeExceeded)
        this.Reset();
      return this._cache.GetOrAdd(virtualPath, new Func<string, bool>(this._virtualPathProvider.FileExists));
    }
  }
}
