// Decompiled with JetBrains decompiler
// Type: System.Reactive.Unit
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Reactive
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct Unit : IEquatable<Unit>
  {
    private static readonly Unit _default;

    public bool Equals(Unit other) => true;

    public override bool Equals(object obj) => obj is Unit;

    public override int GetHashCode() => 0;

    public override string ToString() => "()";

    public static bool operator ==(Unit first, Unit second) => true;

    public static bool operator !=(Unit first, Unit second) => false;

    public static Unit Default => Unit._default;
  }
}
