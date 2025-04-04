// Decompiled with JetBrains decompiler
// Type: MBusLib.Utility.ValueEventHandler`1
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System;

#nullable disable
namespace MBusLib.Utility
{
  public delegate void ValueEventHandler<T>(object sender, T e) where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>;
}
