// Decompiled with JetBrains decompiler
// Type: MVVM.Interfaces.IAsyncCommand
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using System.Threading.Tasks;
using System.Windows.Input;

#nullable disable
namespace MVVM.Interfaces
{
  public interface IAsyncCommand : ICommand
  {
    Task ExecuteAsync(object parameter);
  }
}
