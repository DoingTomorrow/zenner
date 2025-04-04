// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.IQueryService
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  [ServiceContract]
  public interface IQueryService
  {
    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/execute", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
    object[] ExecuteQuery(XElement xml);
  }
}
