// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Reporting.CustomSerializationBinder
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace MSS.Business.Modules.Reporting
{
  public class CustomSerializationBinder : SerializationBinder
  {
    public override Type BindToType(string assemblyName, string typeName)
    {
      return assemblyName == "ClassBwF, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null" && typeName == "ClassBwF.ClassBwf" ? typeof (ClassBwF) : (Type) null;
    }
  }
}
