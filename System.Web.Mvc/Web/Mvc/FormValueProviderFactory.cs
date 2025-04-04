// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FormValueProviderFactory
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Web.Helpers;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class FormValueProviderFactory : ValueProviderFactory
  {
    private readonly UnvalidatedRequestValuesAccessor _unvalidatedValuesAccessor;

    public FormValueProviderFactory()
      : this((UnvalidatedRequestValuesAccessor) null)
    {
    }

    internal FormValueProviderFactory(
      UnvalidatedRequestValuesAccessor unvalidatedValuesAccessor)
    {
      this._unvalidatedValuesAccessor = unvalidatedValuesAccessor ?? (UnvalidatedRequestValuesAccessor) (cc => (IUnvalidatedRequestValues) new UnvalidatedRequestValuesWrapper(cc.HttpContext.Request.Unvalidated()));
    }

    public override IValueProvider GetValueProvider(ControllerContext controllerContext)
    {
      return controllerContext != null ? (IValueProvider) new FormValueProvider(controllerContext, this._unvalidatedValuesAccessor(controllerContext)) : throw new ArgumentNullException(nameof (controllerContext));
    }
  }
}
