// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ModelError
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  [Serializable]
  public class ModelError
  {
    public ModelError(Exception exception)
      : this(exception, (string) null)
    {
    }

    public ModelError(Exception exception, string errorMessage)
      : this(errorMessage)
    {
      this.Exception = exception != null ? exception : throw new ArgumentNullException(nameof (exception));
    }

    public ModelError(string errorMessage) => this.ErrorMessage = errorMessage ?? string.Empty;

    public Exception Exception { get; private set; }

    public string ErrorMessage { get; private set; }
  }
}
