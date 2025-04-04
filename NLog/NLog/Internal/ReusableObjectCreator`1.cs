// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableObjectCreator`1
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Internal
{
  internal class ReusableObjectCreator<T> where T : class
  {
    protected T _reusableObject;
    private readonly Action<T> _clearObject;
    public readonly ReusableObjectCreator<T>.LockOject None;

    protected ReusableObjectCreator(T reusableObject, Action<T> clearObject)
    {
      this._reusableObject = reusableObject;
      this._clearObject = clearObject;
    }

    public ReusableObjectCreator<T>.LockOject Allocate()
    {
      return new ReusableObjectCreator<T>.LockOject(this);
    }

    public struct LockOject : IDisposable
    {
      public readonly T Result;
      private readonly ReusableObjectCreator<T> _owner;

      public LockOject(ReusableObjectCreator<T> owner)
      {
        this.Result = owner._reusableObject;
        owner._reusableObject = default (T);
        this._owner = owner;
      }

      public void Dispose()
      {
        if ((object) this.Result == null)
          return;
        this._owner._clearObject(this.Result);
        this._owner._reusableObject = this.Result;
      }
    }
  }
}
