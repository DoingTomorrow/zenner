// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.ViewArchiveLocationViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO.Archive;
using MSS.Localisation;
using Ninject;
using System;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class ViewArchiveLocationViewModel : ViewArchiveEntityViewModel
  {
    private ArchiveLocationDTO _Entity { get; }

    [Inject]
    public ViewArchiveLocationViewModel(ArchiveLocationDTO entity)
    {
      this._Entity = entity;
      this.ArchiveViewEntityDialogTitle = CultureResources.GetValue("MSS_Client_Archiving_ViewLocation");
      this.InitializeData(this._Entity);
    }

    private void InitializeData(ArchiveLocationDTO entity)
    {
      this.CityTextValue = entity.City;
      this.StreetTextValue = entity.Street;
      this.ZipCodeValue = entity.ZipCode;
      this.BuildingNumberValue = entity.BuildingNr;
      this.DescriptionValue = entity.Description;
      this.DueDateValue = entity.DueDate;
      this.GenerationValue = entity.Generation.ToString();
      this.Scale = entity.Scale.ToString();
      this.ScenarioCode = entity.ScenarioCode;
    }

    public string CityTextValue { get; set; }

    public string StreetTextValue { get; set; }

    public string DescriptionValue { get; set; }

    public string ZipCodeValue { get; set; }

    public string BuildingNumberValue { get; set; }

    public DateTime DueDateValue { get; set; }

    public string GenerationValue { get; set; }

    public string Scale { get; set; }

    public string ScenarioCode { get; set; }
  }
}
