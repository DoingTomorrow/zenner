// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndex
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Runtime.InteropServices;

#nullable disable
namespace System.Data.SQLite
{
  public sealed class SQLiteIndex
  {
    private SQLiteIndexInputs inputs;
    private SQLiteIndexOutputs outputs;

    internal SQLiteIndex(int nConstraint, int nOrderBy)
    {
      this.inputs = new SQLiteIndexInputs(nConstraint, nOrderBy);
      this.outputs = new SQLiteIndexOutputs(nConstraint);
    }

    private static void SizeOfNative(
      out int sizeOfInfoType,
      out int sizeOfConstraintType,
      out int sizeOfOrderByType,
      out int sizeOfConstraintUsageType)
    {
      sizeOfInfoType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_info));
      sizeOfConstraintType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint));
      sizeOfOrderByType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_orderby));
      sizeOfConstraintUsageType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint_usage));
    }

    private static IntPtr AllocateAndInitializeNative(int nConstraint, int nOrderBy)
    {
      IntPtr num1 = IntPtr.Zero;
      IntPtr num2 = IntPtr.Zero;
      IntPtr pMemory1 = IntPtr.Zero;
      IntPtr pMemory2 = IntPtr.Zero;
      IntPtr pMemory3 = IntPtr.Zero;
      try
      {
        int sizeOfInfoType;
        int sizeOfConstraintType;
        int sizeOfOrderByType;
        int sizeOfConstraintUsageType;
        SQLiteIndex.SizeOfNative(out sizeOfInfoType, out sizeOfConstraintType, out sizeOfOrderByType, out sizeOfConstraintUsageType);
        if (sizeOfInfoType > 0)
        {
          if (sizeOfConstraintType > 0)
          {
            if (sizeOfOrderByType > 0)
            {
              if (sizeOfConstraintUsageType > 0)
              {
                num2 = SQLiteMemory.Allocate(sizeOfInfoType);
                pMemory1 = SQLiteMemory.Allocate(sizeOfConstraintType * nConstraint);
                pMemory2 = SQLiteMemory.Allocate(sizeOfOrderByType * nOrderBy);
                pMemory3 = SQLiteMemory.Allocate(sizeOfConstraintUsageType * nConstraint);
                if (num2 != IntPtr.Zero)
                {
                  if (pMemory1 != IntPtr.Zero)
                  {
                    if (pMemory2 != IntPtr.Zero)
                    {
                      if (pMemory3 != IntPtr.Zero)
                      {
                        int offset1 = 0;
                        SQLiteMarshal.WriteInt32(num2, offset1, nConstraint);
                        int offset2 = SQLiteMarshal.NextOffsetOf(offset1, 4, IntPtr.Size);
                        SQLiteMarshal.WriteIntPtr(num2, offset2, pMemory1);
                        int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, 4);
                        SQLiteMarshal.WriteInt32(num2, offset3, nOrderBy);
                        int offset4 = SQLiteMarshal.NextOffsetOf(offset3, 4, IntPtr.Size);
                        SQLiteMarshal.WriteIntPtr(num2, offset4, pMemory2);
                        int offset5 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, IntPtr.Size);
                        SQLiteMarshal.WriteIntPtr(num2, offset5, pMemory3);
                        num1 = num2;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      finally
      {
        if (num1 == IntPtr.Zero)
        {
          if (pMemory3 != IntPtr.Zero)
          {
            SQLiteMemory.Free(pMemory3);
            IntPtr zero = IntPtr.Zero;
          }
          if (pMemory2 != IntPtr.Zero)
          {
            SQLiteMemory.Free(pMemory2);
            IntPtr zero = IntPtr.Zero;
          }
          if (pMemory1 != IntPtr.Zero)
          {
            SQLiteMemory.Free(pMemory1);
            IntPtr zero = IntPtr.Zero;
          }
          if (num2 != IntPtr.Zero)
          {
            SQLiteMemory.Free(num2);
            IntPtr zero = IntPtr.Zero;
          }
        }
      }
      return num1;
    }

    private static void FreeNative(IntPtr pIndex)
    {
      if (pIndex == IntPtr.Zero)
        return;
      int offset1 = SQLiteMarshal.NextOffsetOf(0, 4, IntPtr.Size);
      IntPtr pMemory1 = SQLiteMarshal.ReadIntPtr(pIndex, offset1);
      int offset2 = SQLiteMarshal.NextOffsetOf(SQLiteMarshal.NextOffsetOf(offset1, IntPtr.Size, 4), 4, IntPtr.Size);
      IntPtr pMemory2 = SQLiteMarshal.ReadIntPtr(pIndex, offset2);
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, IntPtr.Size);
      IntPtr pMemory3 = SQLiteMarshal.ReadIntPtr(pIndex, offset3);
      if (pMemory3 != IntPtr.Zero)
      {
        SQLiteMemory.Free(pMemory3);
        IntPtr zero = IntPtr.Zero;
      }
      if (pMemory2 != IntPtr.Zero)
      {
        SQLiteMemory.Free(pMemory2);
        IntPtr zero = IntPtr.Zero;
      }
      if (pMemory1 != IntPtr.Zero)
      {
        SQLiteMemory.Free(pMemory1);
        IntPtr zero = IntPtr.Zero;
      }
      if (!(pIndex != IntPtr.Zero))
        return;
      SQLiteMemory.Free(pIndex);
      pIndex = IntPtr.Zero;
    }

    internal static void FromIntPtr(IntPtr pIndex, bool includeOutput, ref SQLiteIndex index)
    {
      if (pIndex == IntPtr.Zero)
        return;
      int offset1 = 0;
      int nConstraint = SQLiteMarshal.ReadInt32(pIndex, offset1);
      int offset2 = SQLiteMarshal.NextOffsetOf(offset1, 4, IntPtr.Size);
      IntPtr pointer1 = SQLiteMarshal.ReadIntPtr(pIndex, offset2);
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, 4);
      int nOrderBy = SQLiteMarshal.ReadInt32(pIndex, offset3);
      int offset4 = SQLiteMarshal.NextOffsetOf(offset3, 4, IntPtr.Size);
      IntPtr pointer2 = SQLiteMarshal.ReadIntPtr(pIndex, offset4);
      IntPtr pointer3 = IntPtr.Zero;
      if (includeOutput)
      {
        offset4 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, IntPtr.Size);
        pointer3 = SQLiteMarshal.ReadIntPtr(pIndex, offset4);
      }
      index = new SQLiteIndex(nConstraint, nOrderBy);
      SQLiteIndexInputs inputs = index.Inputs;
      if (inputs == null)
        return;
      SQLiteIndexConstraint[] constraints = inputs.Constraints;
      if (constraints == null)
        return;
      SQLiteIndexOrderBy[] orderBys = inputs.OrderBys;
      if (orderBys == null)
        return;
      Type type1 = typeof (UnsafeNativeMethods.sqlite3_index_constraint);
      int num1 = Marshal.SizeOf(type1);
      for (int index1 = 0; index1 < nConstraint; ++index1)
      {
        UnsafeNativeMethods.sqlite3_index_constraint structure = (UnsafeNativeMethods.sqlite3_index_constraint) Marshal.PtrToStructure(SQLiteMarshal.IntPtrForOffset(pointer1, index1 * num1), type1);
        constraints[index1] = new SQLiteIndexConstraint(structure);
      }
      Type type2 = typeof (UnsafeNativeMethods.sqlite3_index_orderby);
      int num2 = Marshal.SizeOf(type2);
      for (int index2 = 0; index2 < nOrderBy; ++index2)
      {
        UnsafeNativeMethods.sqlite3_index_orderby structure = (UnsafeNativeMethods.sqlite3_index_orderby) Marshal.PtrToStructure(SQLiteMarshal.IntPtrForOffset(pointer2, index2 * num2), type2);
        orderBys[index2] = new SQLiteIndexOrderBy(structure);
      }
      if (!includeOutput)
        return;
      SQLiteIndexOutputs outputs = index.Outputs;
      if (outputs == null)
        return;
      SQLiteIndexConstraintUsage[] constraintUsages = outputs.ConstraintUsages;
      if (constraintUsages == null)
        return;
      Type type3 = typeof (UnsafeNativeMethods.sqlite3_index_constraint_usage);
      int num3 = Marshal.SizeOf(type3);
      for (int index3 = 0; index3 < nConstraint; ++index3)
      {
        UnsafeNativeMethods.sqlite3_index_constraint_usage structure = (UnsafeNativeMethods.sqlite3_index_constraint_usage) Marshal.PtrToStructure(SQLiteMarshal.IntPtrForOffset(pointer3, index3 * num3), type3);
        constraintUsages[index3] = new SQLiteIndexConstraintUsage(structure);
      }
      int offset5 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, 4);
      outputs.IndexNumber = SQLiteMarshal.ReadInt32(pIndex, offset5);
      int offset6 = SQLiteMarshal.NextOffsetOf(offset5, 4, IntPtr.Size);
      outputs.IndexString = SQLiteString.StringFromUtf8IntPtr(SQLiteMarshal.ReadIntPtr(pIndex, offset6));
      int offset7 = SQLiteMarshal.NextOffsetOf(offset6, IntPtr.Size, 4);
      outputs.NeedToFreeIndexString = SQLiteMarshal.ReadInt32(pIndex, offset7);
      int offset8 = SQLiteMarshal.NextOffsetOf(offset7, 4, 4);
      outputs.OrderByConsumed = SQLiteMarshal.ReadInt32(pIndex, offset8);
      int offset9 = SQLiteMarshal.NextOffsetOf(offset8, 4, 8);
      outputs.EstimatedCost = new double?(SQLiteMarshal.ReadDouble(pIndex, offset9));
      int offset10 = SQLiteMarshal.NextOffsetOf(offset9, 8, 8);
      if (outputs.CanUseEstimatedRows())
        outputs.EstimatedRows = new long?(SQLiteMarshal.ReadInt64(pIndex, offset10));
      int offset11 = SQLiteMarshal.NextOffsetOf(offset10, 8, 4);
      if (outputs.CanUseIndexFlags())
        outputs.IndexFlags = new SQLiteIndexFlags?((SQLiteIndexFlags) SQLiteMarshal.ReadInt32(pIndex, offset11));
      int offset12 = SQLiteMarshal.NextOffsetOf(offset11, 4, 8);
      if (!outputs.CanUseColumnsUsed())
        return;
      outputs.ColumnsUsed = new long?(SQLiteMarshal.ReadInt64(pIndex, offset12));
    }

    internal static void ToIntPtr(SQLiteIndex index, IntPtr pIndex, bool includeInput)
    {
      if (index == null)
        return;
      SQLiteIndexOutputs outputs = index.Outputs;
      if (outputs == null)
        return;
      SQLiteIndexConstraintUsage[] constraintUsages = outputs.ConstraintUsages;
      if (constraintUsages == null)
        return;
      SQLiteIndexConstraint[] liteIndexConstraintArray = (SQLiteIndexConstraint[]) null;
      SQLiteIndexOrderBy[] liteIndexOrderByArray = (SQLiteIndexOrderBy[]) null;
      if (includeInput)
      {
        SQLiteIndexInputs inputs = index.Inputs;
        if (inputs == null)
          return;
        liteIndexConstraintArray = inputs.Constraints;
        if (liteIndexConstraintArray == null)
          return;
        liteIndexOrderByArray = inputs.OrderBys;
        if (liteIndexOrderByArray == null)
          return;
      }
      if (pIndex == IntPtr.Zero)
        return;
      int offset1 = 0;
      int num1 = SQLiteMarshal.ReadInt32(pIndex, offset1);
      if (includeInput && num1 != liteIndexConstraintArray.Length || num1 != constraintUsages.Length)
        return;
      int offset2 = SQLiteMarshal.NextOffsetOf(offset1, 4, IntPtr.Size);
      if (includeInput)
      {
        IntPtr pointer = SQLiteMarshal.ReadIntPtr(pIndex, offset2);
        int num2 = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint));
        for (int index1 = 0; index1 < num1; ++index1)
          Marshal.StructureToPtr((object) new UnsafeNativeMethods.sqlite3_index_constraint(liteIndexConstraintArray[index1]), SQLiteMarshal.IntPtrForOffset(pointer, index1 * num2), false);
      }
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, 4);
      int num3 = includeInput ? SQLiteMarshal.ReadInt32(pIndex, offset3) : 0;
      if (includeInput && num3 != liteIndexOrderByArray.Length)
        return;
      int offset4 = SQLiteMarshal.NextOffsetOf(offset3, 4, IntPtr.Size);
      if (includeInput)
      {
        IntPtr pointer = SQLiteMarshal.ReadIntPtr(pIndex, offset4);
        int num4 = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_orderby));
        for (int index2 = 0; index2 < num3; ++index2)
          Marshal.StructureToPtr((object) new UnsafeNativeMethods.sqlite3_index_orderby(liteIndexOrderByArray[index2]), SQLiteMarshal.IntPtrForOffset(pointer, index2 * num4), false);
      }
      int offset5 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, IntPtr.Size);
      IntPtr pointer1 = SQLiteMarshal.ReadIntPtr(pIndex, offset5);
      int num5 = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint_usage));
      for (int index3 = 0; index3 < num1; ++index3)
        Marshal.StructureToPtr((object) new UnsafeNativeMethods.sqlite3_index_constraint_usage(constraintUsages[index3]), SQLiteMarshal.IntPtrForOffset(pointer1, index3 * num5), false);
      int offset6 = SQLiteMarshal.NextOffsetOf(offset5, IntPtr.Size, 4);
      SQLiteMarshal.WriteInt32(pIndex, offset6, outputs.IndexNumber);
      int offset7 = SQLiteMarshal.NextOffsetOf(offset6, 4, IntPtr.Size);
      SQLiteMarshal.WriteIntPtr(pIndex, offset7, SQLiteString.Utf8IntPtrFromString(outputs.IndexString));
      int offset8 = SQLiteMarshal.NextOffsetOf(offset7, IntPtr.Size, 4);
      int num6 = outputs.NeedToFreeIndexString != 0 ? outputs.NeedToFreeIndexString : 1;
      SQLiteMarshal.WriteInt32(pIndex, offset8, num6);
      int offset9 = SQLiteMarshal.NextOffsetOf(offset8, 4, 4);
      SQLiteMarshal.WriteInt32(pIndex, offset9, outputs.OrderByConsumed);
      int offset10 = SQLiteMarshal.NextOffsetOf(offset9, 4, 8);
      if (outputs.EstimatedCost.HasValue)
        SQLiteMarshal.WriteDouble(pIndex, offset10, outputs.EstimatedCost.GetValueOrDefault());
      int offset11 = SQLiteMarshal.NextOffsetOf(offset10, 8, 8);
      if (outputs.CanUseEstimatedRows() && outputs.EstimatedRows.HasValue)
        SQLiteMarshal.WriteInt64(pIndex, offset11, outputs.EstimatedRows.GetValueOrDefault());
      int offset12 = SQLiteMarshal.NextOffsetOf(offset11, 8, 4);
      if (outputs.CanUseIndexFlags() && outputs.IndexFlags.HasValue)
        SQLiteMarshal.WriteInt32(pIndex, offset12, (int) outputs.IndexFlags.GetValueOrDefault());
      int offset13 = SQLiteMarshal.NextOffsetOf(offset12, 4, 8);
      if (!outputs.CanUseColumnsUsed() || !outputs.ColumnsUsed.HasValue)
        return;
      SQLiteMarshal.WriteInt64(pIndex, offset13, outputs.ColumnsUsed.GetValueOrDefault());
    }

    public SQLiteIndexInputs Inputs => this.inputs;

    public SQLiteIndexOutputs Outputs => this.outputs;
  }
}
