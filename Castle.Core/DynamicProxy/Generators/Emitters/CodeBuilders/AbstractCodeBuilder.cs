// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.CodeBuilders.AbstractCodeBuilder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.CodeBuilders
{
  public abstract class AbstractCodeBuilder
  {
    private bool isEmpty;
    private readonly ILGenerator generator;
    private readonly List<Statement> stmts;
    private readonly List<Reference> ilmarkers;

    protected AbstractCodeBuilder(ILGenerator generator)
    {
      this.generator = generator;
      this.stmts = new List<Statement>();
      this.ilmarkers = new List<Reference>();
      this.isEmpty = true;
    }

    public ILGenerator Generator => this.generator;

    public AbstractCodeBuilder AddStatement(Statement stmt)
    {
      this.SetNonEmpty();
      this.stmts.Add(stmt);
      return this;
    }

    public LocalReference DeclareLocal(Type type)
    {
      LocalReference localReference = new LocalReference(type);
      this.ilmarkers.Add((Reference) localReference);
      return localReference;
    }

    public void SetNonEmpty() => this.isEmpty = false;

    internal bool IsEmpty => this.isEmpty;

    internal void Generate(IMemberEmitter member, ILGenerator il)
    {
      foreach (Reference ilmarker in this.ilmarkers)
        ilmarker.Generate(il);
      foreach (Statement stmt in this.stmts)
        stmt.Emit(member, il);
    }
  }
}
