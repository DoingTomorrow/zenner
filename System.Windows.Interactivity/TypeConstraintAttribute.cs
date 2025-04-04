// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TypeConstraintAttribute
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class TypeConstraintAttribute : Attribute
  {
    public Type Constraint { get; private set; }

    public TypeConstraintAttribute(Type constraint) => this.Constraint = constraint;
  }
}
