// Decompiled with JetBrains decompiler
// Type: DeviceCollector.ThreadStarter
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System.Threading;

#nullable disable
namespace DeviceCollector
{
  internal class ThreadStarter
  {
    internal static Thread CreateThread(
      RuntimeThread.Start StartFktPara,
      DeviceCollectorFunctions BaseClassRef)
    {
      return new Thread(new ThreadStart(new ThreadStarter.Args()
      {
        BaseClassRef = BaseClassRef,
        StartFkt = StartFktPara
      }.StarterStartFkt));
    }

    private class Args
    {
      public DeviceCollectorFunctions BaseClassRef;
      public RuntimeThread.Start StartFkt;

      public void StarterStartFkt() => this.StartFkt(ref this.BaseClassRef);
    }
  }
}
