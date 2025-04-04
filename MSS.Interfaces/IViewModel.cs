// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IViewModel
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using MSS.Core.Events;
using System;

#nullable disable
namespace MSS.Interfaces
{
  public interface IViewModel
  {
    event Action Disposed;

    event EventHandler RequestCancel;

    event EventHandler<RequestCloseEventArgs> RequestCloseDialog;
  }
}
