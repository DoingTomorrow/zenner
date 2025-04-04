// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Core.ExtendedPathFilter
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System;
using System.IO;

#nullable disable
namespace ICSharpCode.SharpZipLib.Core
{
  public class ExtendedPathFilter : PathFilter
  {
    private long minSize_;
    private long maxSize_ = long.MaxValue;
    private DateTime minDate_ = DateTime.MinValue;
    private DateTime maxDate_ = DateTime.MaxValue;

    public ExtendedPathFilter(string filter, long minSize, long maxSize)
      : base(filter)
    {
      this.MinSize = minSize;
      this.MaxSize = maxSize;
    }

    public ExtendedPathFilter(string filter, DateTime minDate, DateTime maxDate)
      : base(filter)
    {
      this.MinDate = minDate;
      this.MaxDate = maxDate;
    }

    public ExtendedPathFilter(
      string filter,
      long minSize,
      long maxSize,
      DateTime minDate,
      DateTime maxDate)
      : base(filter)
    {
      this.MinSize = minSize;
      this.MaxSize = maxSize;
      this.MinDate = minDate;
      this.MaxDate = maxDate;
    }

    public override bool IsMatch(string name)
    {
      bool flag = base.IsMatch(name);
      if (flag)
      {
        FileInfo fileInfo = new FileInfo(name);
        flag = this.MinSize <= fileInfo.Length && this.MaxSize >= fileInfo.Length && this.MinDate <= fileInfo.LastWriteTime && this.MaxDate >= fileInfo.LastWriteTime;
      }
      return flag;
    }

    public long MinSize
    {
      get => this.minSize_;
      set
      {
        this.minSize_ = value >= 0L && this.maxSize_ >= value ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public long MaxSize
    {
      get => this.maxSize_;
      set
      {
        this.maxSize_ = value >= 0L && this.minSize_ <= value ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public DateTime MinDate
    {
      get => this.minDate_;
      set
      {
        this.minDate_ = !(value > this.maxDate_) ? value : throw new ArgumentOutOfRangeException(nameof (value), "Exceeds MaxDate");
      }
    }

    public DateTime MaxDate
    {
      get => this.maxDate_;
      set
      {
        this.maxDate_ = !(this.minDate_ > value) ? value : throw new ArgumentOutOfRangeException(nameof (value), "Exceeds MinDate");
      }
    }
  }
}
