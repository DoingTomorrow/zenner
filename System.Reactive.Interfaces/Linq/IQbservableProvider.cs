// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.IQbservableProvider
// Assembly: System.Reactive.Interfaces, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: F75E00F4-A403-435F-9B50-2B7670A5231F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Interfaces.dll

using System.Linq.Expressions;

#nullable disable
namespace System.Reactive.Linq
{
  public interface IQbservableProvider
  {
    IQbservable<TResult> CreateQuery<TResult>(Expression expression);
  }
}
