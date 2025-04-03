// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InterfaceProxyInstanceContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class InterfaceProxyInstanceContributor(
    Type targetType,
    string proxyGeneratorId,
    Type[] interfaces) : ProxyInstanceContributor(targetType, interfaces, proxyGeneratorId)
  {
    protected override Expression GetTargetReferenceExpression(ClassEmitter emitter)
    {
      return emitter.GetField("__target").ToExpression();
    }

    protected override void CustomizeGetObjectData(
      AbstractCodeBuilder codebuilder,
      ArgumentReference serializationInfo,
      ArgumentReference streamingContext,
      ClassEmitter emitter)
    {
      FieldReference field = emitter.GetField("__target");
      codebuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) serializationInfo, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__targetFieldType").ToExpression(),
        new ConstReference((object) field.Reference.FieldType.AssemblyQualifiedName).ToExpression()
      })));
      codebuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) serializationInfo, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__theInterface").ToExpression(),
        new ConstReference((object) this.targetType.AssemblyQualifiedName).ToExpression()
      })));
    }
  }
}
