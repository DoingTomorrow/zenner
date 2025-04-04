// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.LinqBinaryModelBinder
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Data.Linq;

#nullable disable
namespace System.Web.Mvc
{
  public class LinqBinaryModelBinder : ByteArrayModelBinder
  {
    public override object BindModel(
      ControllerContext controllerContext,
      ModelBindingContext bindingContext)
    {
      byte[] numArray = (byte[]) base.BindModel(controllerContext, bindingContext);
      return numArray == null ? (object) null : (object) new Binary(numArray);
    }
  }
}
