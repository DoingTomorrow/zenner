// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.NoteWithTranslation
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Meters;
using MSS.DTO;
using MSS.Localisation;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class NoteWithTranslation : DTOBase
  {
    private Note _note;

    public Note Note
    {
      get => this._note;
      set
      {
        this._note = value;
        this.OnPropertyChanged(nameof (Note));
      }
    }

    public string Translation
    {
      get
      {
        return this.Note == null || this.Note.NoteType == null || string.IsNullOrEmpty(this.Note.NoteType.Description) ? string.Empty : CultureResources.GetValue("MSS_NoteType_" + this.Note.NoteType.Description);
      }
    }
  }
}
