// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.LocalQueryMethodImplementationTypeAttribute
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.ComponentModel;

#nullable disable
namespace System.Reactive.Linq
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class LocalQueryMethodImplementationTypeAttribute : Attribute
  {
    private readonly Type _targetType;

    public LocalQueryMethodImplementationTypeAttribute(Type targetType)
    {
      this._targetType = targetType;
    }

    public Type TargetType => this._targetType;
  }
}
