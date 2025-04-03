// Decompiled with JetBrains decompiler
// Type: HandlerLib.ILoggerManager
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public interface ILoggerManager
  {
    Task<ObservableCollection<LoggerListItem>> ReadLoggerListAsync(
      ProgressHandler progress,
      CancellationToken cancelToken);

    Task<IEnumerable> ReadLoggerDataAsync(
      string LoggerName,
      ProgressHandler progress,
      CancellationToken cancelToken);
  }
}
