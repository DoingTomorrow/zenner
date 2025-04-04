// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.Timestamper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Cache
{
  public sealed class Timestamper
  {
    private const int BinDigits = 12;
    public const short OneMs = 4096;
    private static object lockObject = new object();
    private static long baseDateMs = new DateTime(1970, 1, 1).Ticks / 10000L;
    private static short counter = 0;
    private static long time;

    public static long Next()
    {
      lock (Timestamper.lockObject)
      {
        long num = DateTime.Now.Ticks / 10000L - Timestamper.baseDateMs << 12;
        if (Timestamper.time < num)
        {
          Timestamper.time = num;
          Timestamper.counter = (short) 0;
        }
        else if (Timestamper.counter < (short) 4095)
          ++Timestamper.counter;
        return Timestamper.time + (long) Timestamper.counter;
      }
    }

    private Timestamper()
    {
    }
  }
}
