// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Providers.ConstantProvider`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

#nullable disable
namespace Ninject.Activation.Providers
{
  public class ConstantProvider<T> : Provider<T>
  {
    public T Value { get; private set; }

    public ConstantProvider(T value) => this.Value = value;

    protected override T CreateInstance(IContext context) => this.Value;
  }
}
