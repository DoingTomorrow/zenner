// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.CacheControllerBehavior
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

#nullable disable
namespace Microsoft.Synchronization.ClientServices
{
  public class CacheControllerBehavior
  {
    private bool locked;
    private readonly object lockObject = new object();
    private List<Type> _knownTypes;
    private ICredentials credentials;
    private SerializationFormat serFormat;
    private string scopeName;
    private readonly Dictionary<string, string> scopeParameters;

    internal CacheControllerBehavior()
    {
      this._knownTypes = new List<Type>();
      this.serFormat = SerializationFormat.ODataAtom;
      this.scopeParameters = new Dictionary<string, string>();
    }

    public ReadOnlyCollection<Type> KnownTypes
    {
      get => new ReadOnlyCollection<Type>((IList<Type>) this._knownTypes);
    }

    public Dictionary<string, string>.Enumerator ScopeParameters
    {
      get => this.scopeParameters.GetEnumerator();
    }

    public ICredentials Credentials
    {
      get => this.credentials;
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        lock (this.lockObject)
        {
          this.CheckLockState();
          this.credentials = value;
        }
      }
    }

    public SerializationFormat SerializationFormat
    {
      get => this.serFormat;
      set
      {
        lock (this.lockObject)
        {
          this.CheckLockState();
          this.serFormat = value;
        }
      }
    }

    public string ScopeName
    {
      get => this.scopeName;
      internal set => this.scopeName = value;
    }

    public void AddType<T>() where T : IOfflineEntity
    {
      lock (this.lockObject)
      {
        this.CheckLockState();
        this._knownTypes.Add(typeof (T));
      }
    }

    public void AddScopeParameters(string key, string value)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (string.IsNullOrEmpty(key))
        throw new ArgumentException("key cannot be empty", nameof (key));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      lock (this.lockObject)
      {
        this.CheckLockState();
        this.scopeParameters.Add(key, value);
      }
    }

    private void CheckLockState()
    {
      if (this.locked)
        throw new CacheControllerException("Cannot modify CacheControllerBehavior when sync is in progress.");
    }

    internal Dictionary<string, string> ScopeParametersInternal => this.scopeParameters;

    internal bool Locked
    {
      set
      {
        lock (this.lockObject)
          this.locked = value;
      }
    }
  }
}
