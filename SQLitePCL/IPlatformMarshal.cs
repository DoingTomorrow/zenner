// Decompiled with JetBrains decompiler
// Type: SQLitePCL.IPlatformMarshal
// Assembly: SQLitePCL, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 4D61F17D-4F76-4E73-B63C-94DC04208DE1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.dll

using System;

#nullable disable
namespace SQLitePCL
{
  public interface IPlatformMarshal
  {
    void CleanUpStringNativeUTF8(IntPtr nativeString);

    IntPtr MarshalStringManagedToNativeUTF8(string managedString);

    IntPtr MarshalStringManagedToNativeUTF8(string managedString, out int size);

    string MarshalStringNativeUTF8ToManaged(IntPtr nativeString);

    int GetNativeUTF8Size(IntPtr nativeString);

    Delegate ApplyNativeCallingConventionToFunction(FunctionNative function);

    Delegate ApplyNativeCallingConventionToAggregateStep(AggregateStepNative step);

    Delegate ApplyNativeCallingConventionToAggregateFinal(AggregateFinalNative final);

    IntPtr MarshalDelegateToNativeFunctionPointer(Delegate del);

    void Copy(IntPtr source, byte[] destination, int startIndex, int length);

    void Copy(byte[] source, IntPtr destination, int startIndex, int length);
  }
}
