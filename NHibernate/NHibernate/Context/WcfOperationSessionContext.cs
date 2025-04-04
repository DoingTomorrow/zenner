// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.WcfOperationSessionContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections;
using System.ServiceModel;

#nullable disable
namespace NHibernate.Context
{
  public class WcfOperationSessionContext(ISessionFactoryImplementor factory) : 
    MapBasedSessionContext(factory)
  {
    private static WcfStateExtension WcfOperationState
    {
      get
      {
        WcfStateExtension wcfOperationState = OperationContext.Current.Extensions.Find<WcfStateExtension>();
        if (wcfOperationState == null)
        {
          wcfOperationState = new WcfStateExtension();
          OperationContext.Current.Extensions.Add((IExtension<OperationContext>) wcfOperationState);
        }
        return wcfOperationState;
      }
    }

    protected override IDictionary GetMap() => WcfOperationSessionContext.WcfOperationState.Map;

    protected override void SetMap(IDictionary value)
    {
      WcfOperationSessionContext.WcfOperationState.Map = value;
    }
  }
}
