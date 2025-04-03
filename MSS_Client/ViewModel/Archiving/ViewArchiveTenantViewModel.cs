// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.ViewArchiveTenantViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO.Archive;
using MSS.Localisation;
using Ninject;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class ViewArchiveTenantViewModel : ViewArchiveEntityViewModel
  {
    private string _floorNameValue = string.Empty;
    private string _floorNrValue = string.Empty;
    private string _apartmentNrValue = string.Empty;
    private string _descriptionValue = string.Empty;
    private string _customerTenantNo = string.Empty;
    private bool _isGroupValue;

    private ArchiveTenantDTO _Entity { get; }

    [Inject]
    public ViewArchiveTenantViewModel(ArchiveTenantDTO entity)
    {
      this._Entity = entity;
      this.ArchiveViewEntityDialogTitle = CultureResources.GetValue("MSS_Client_Archiving_ViewTenant");
      this.InitializeData(this._Entity);
    }

    private void InitializeData(ArchiveTenantDTO entity)
    {
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.TenantNr = entity.TenantNr;
      this.FloorNameValue = entity.FloorName;
      this.FloorNrValue = entity.FloorNr;
      this.ApartmentNrValue = entity.ApartmentNr;
      this.CustomerTenantNo = entity.CustomerTenantNo;
      this.IsGroup = entity.IsGroup;
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public int TenantNr { get; set; }

    public string FloorNameValue
    {
      get => this._floorNameValue;
      set => this._floorNameValue = value;
    }

    public string FloorNrValue
    {
      get => this._floorNrValue;
      set => this._floorNrValue = value;
    }

    public string ApartmentNrValue
    {
      get => this._apartmentNrValue;
      set => this._apartmentNrValue = value;
    }

    public string DescriptionValue
    {
      get => this._descriptionValue;
      set => this._descriptionValue = value;
    }

    public string CustomerTenantNo
    {
      get => this._customerTenantNo;
      set => this._customerTenantNo = value;
    }

    public bool IsGroup
    {
      get => this._isGroupValue;
      set => this._isGroupValue = value;
    }
  }
}
