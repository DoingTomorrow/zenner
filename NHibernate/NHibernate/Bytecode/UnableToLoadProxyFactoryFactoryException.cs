// Decompiled with JetBrains decompiler
// Type: NHibernate.Bytecode.UnableToLoadProxyFactoryFactoryException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Bytecode
{
  [Serializable]
  public class UnableToLoadProxyFactoryFactoryException : HibernateByteCodeException
  {
    private readonly string typeName;

    public UnableToLoadProxyFactoryFactoryException(string typeName, Exception inner)
      : base("", inner)
    {
      this.typeName = typeName;
    }

    protected UnableToLoadProxyFactoryFactoryException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
    }

    public override string Message
    {
      get
      {
        return "Unable to load type '" + this.typeName + "' during configuration of proxy factory class.\r\nPossible causes are:\r\n- The NHibernate.Bytecode provider assembly was not deployed.\r\n- The typeName used to initialize the 'proxyfactory.factory_class' property of the session-factory section is not well formed.\r\n\r\nSolution:\r\nConfirm that your deployment folder contains one of the following assemblies:\r\nNHibernate.ByteCode.LinFu.dll\r\nNHibernate.ByteCode.Castle.dll";
      }
    }
  }
}
