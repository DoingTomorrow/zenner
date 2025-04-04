// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.CustomModelBinderAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
  public abstract class CustomModelBinderAttribute : Attribute
  {
    internal const AttributeTargets ValidTargets = AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Parameter;

    public abstract IModelBinder GetBinder();
  }
}
