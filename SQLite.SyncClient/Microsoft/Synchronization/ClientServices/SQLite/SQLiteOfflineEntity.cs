// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.SQLiteOfflineEntity
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  public class SQLiteOfflineEntity : IOfflineEntity, INotifyPropertyChanged
  {
    private OfflineEntityMetadata entityMetadata;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentNullException(nameof (propertyName));
      this.RaisePropertyChanged(propertyName);
    }

    protected void OnPropertyChanging(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentException(nameof (propertyName));
    }

    public OfflineEntityMetadata GetServiceMetadata() => this.ServiceMetadata;

    public void SetServiceMetadata(OfflineEntityMetadata value) => this.ServiceMetadata = value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DataMember]
    [Ignore]
    internal OfflineEntityMetadata ServiceMetadata
    {
      get => this.entityMetadata;
      set
      {
        if (value == this.entityMetadata)
          return;
        this.entityMetadata = value;
        this.RaisePropertyChanged("EntityMetadata");
      }
    }

    internal static PropertyInfo[] GetEntityPrimaryKeyProperties(Type t)
    {
      return ((IEnumerable<PropertyInfo>) SQLiteOfflineEntity.GetEntityProperties(t)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => ((IEnumerable<object>) p.GetCustomAttributes(typeof (PrimaryKeyAttribute), false)).Count<object>() != 0)).ToArray<PropertyInfo>();
    }

    internal static PropertyInfo[] GetEntityProperties(Type t)
    {
      return t.GetTypeInfo().DeclaredProperties.Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetMethod != (MethodInfo) null && p.SetMethod != (MethodInfo) null && p.DeclaringType == t)).ToArray<PropertyInfo>();
    }

    private void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
