// Decompiled with JetBrains decompiler
// Type: AForge.ThreadSafeRandom
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;

#nullable disable
namespace AForge
{
  public sealed class ThreadSafeRandom : Random
  {
    private object sync = new object();

    public ThreadSafeRandom()
    {
    }

    public ThreadSafeRandom(int seed)
      : base(seed)
    {
    }

    public override int Next()
    {
      lock (this.sync)
        return base.Next();
    }

    public override int Next(int maxValue)
    {
      lock (this.sync)
        return base.Next(maxValue);
    }

    public override int Next(int minValue, int maxValue)
    {
      lock (this.sync)
        return base.Next(minValue, maxValue);
    }

    public override void NextBytes(byte[] buffer)
    {
      lock (this.sync)
        base.NextBytes(buffer);
    }

    public override double NextDouble()
    {
      lock (this.sync)
        return base.NextDouble();
    }
  }
}
