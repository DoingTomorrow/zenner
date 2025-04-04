// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ReflectedParameterDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  public class ReflectedParameterDescriptor : ParameterDescriptor
  {
    private readonly ActionDescriptor _actionDescriptor;
    private readonly ReflectedParameterBindingInfo _bindingInfo;

    public ReflectedParameterDescriptor(
      ParameterInfo parameterInfo,
      ActionDescriptor actionDescriptor)
    {
      if (parameterInfo == null)
        throw new ArgumentNullException(nameof (parameterInfo));
      if (actionDescriptor == null)
        throw new ArgumentNullException(nameof (actionDescriptor));
      this.ParameterInfo = parameterInfo;
      this._actionDescriptor = actionDescriptor;
      this._bindingInfo = new ReflectedParameterBindingInfo(parameterInfo);
    }

    public override ActionDescriptor ActionDescriptor => this._actionDescriptor;

    public override ParameterBindingInfo BindingInfo => (ParameterBindingInfo) this._bindingInfo;

    public override object DefaultValue
    {
      get
      {
        object obj;
        return ParameterInfoUtil.TryGetDefaultValue(this.ParameterInfo, out obj) ? obj : base.DefaultValue;
      }
    }

    public ParameterInfo ParameterInfo { get; private set; }

    public override string ParameterName => this.ParameterInfo.Name;

    public override Type ParameterType => this.ParameterInfo.ParameterType;

    public override object[] GetCustomAttributes(bool inherit)
    {
      return this.ParameterInfo.GetCustomAttributes(inherit);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return this.ParameterInfo.GetCustomAttributes(attributeType, inherit);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this.ParameterInfo.IsDefined(attributeType, inherit);
    }
  }
}
