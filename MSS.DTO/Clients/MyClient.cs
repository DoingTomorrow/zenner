// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Clients.MyClient
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace MSS.DTO.Clients
{
  public class MyClient : INotifyPropertyChanged
  {
    private int _idEnumStatus;
    private DateTime approvedOn = DateTime.Today;
    private string iconUrl = string.Empty;
    private string _userName = string.Empty;
    private Guid _userId = Guid.NewGuid();

    public virtual Guid Id { get; set; }

    public virtual string UniqueClientRequest { get; set; }

    public int idEnumStatus
    {
      get => this._idEnumStatus;
      set
      {
        this._idEnumStatus = value;
        this.OnPropertyChanged(nameof (idEnumStatus));
      }
    }

    public virtual DateTime ApprovedOn
    {
      get => this.approvedOn;
      set
      {
        this.approvedOn = value;
        this.OnPropertyChanged(nameof (ApprovedOn));
      }
    }

    public ObservableCollection<EnumObj> ObsStatus { get; set; }

    public virtual string IconUrl
    {
      get => this.iconUrl;
      set
      {
        this.iconUrl = value;
        this.OnPropertyChanged(nameof (IconUrl));
      }
    }

    public virtual string UserName
    {
      get => this._userName;
      set
      {
        this._userName = value;
        this.OnPropertyChanged(nameof (UserName));
      }
    }

    public virtual Guid UserId
    {
      get => this._userId;
      set
      {
        this._userId = value;
        this.OnPropertyChanged(nameof (UserId));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      this.VerifyPropertyName(propertyName);
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
      propertyChanged((object) this, e);
    }

    protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
      if (TypeDescriptor.GetProperties((object) this)[propertyName] != null)
        return;
      string message = "Invalid property name: " + propertyName;
      if (this.ThrowOnInvalidPropertyName)
        throw new Exception(message);
      Debug.Fail(message);
    }
  }
}
