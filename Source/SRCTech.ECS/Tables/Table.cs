using System.Buffers;
using SRCTech.Common.Lifetimes;

namespace SRCTech.ECS.Tables;

public class Table : IDisposable
{
    private List<TableColumn> _columns;
    private List<TableChunk> _chunks;
    
    public int ArrayCapacity { get; }

    public int Count { get; private set; }

    public int AddRow()
    {
        int slot = Count;
        (int quotient, int remainder) = Math.DivRem(slot, ArrayCapacity);

        TableChunk chunk;
        if (quotient >= _chunks.Count)
        {
            List<TableArray> arrays = new List<TableArray>();
            foreach (var column in _columns)
            {
                arrays.Add(column.PushArray());
            }

            chunk = new TableChunk(arrays);
            _chunks.Add(chunk);
        }
        else
        {
            chunk = _chunks[quotient];
        }

        Count += 1;
        return slot;
    }

    public int RemoveRow(int rowSlot)
    {
        int lastSlot = Count;
        if (rowSlot != lastSlot)
        {

        }

        return RemoveLastRow();
    }

    private int RemoveLastRow()
    {
        int slot = Count - 1;
        (int quotient, int remainder) = Math.DivRem(slot, ArrayCapacity);
        if (remainder == 0)
        {
            _chunks.RemoveAt(quotient);

            foreach (var column in _columns)
            {
                column.PopArray();
            }
        }

        Count -= 1;
        return slot;
    }
}

public sealed class TableChunk
{
    private List<TableArray> _arrays;

    public TableChunk(IEnumerable<TableArray> arrays)
    {
        _arrays = new List<TableArray>(arrays);
    }
}

public abstract class TableColumn : IDisposable
{
    public abstract void Dispose();

    public abstract TableArray PushArray();

    public abstract void PopArray();
}

public sealed class TableColumn<T> : TableColumn
{
    private ITableArrayAllocator<T> _arrayAllocator;
    private List<TableArray<T>> _arrays;

    public TableColumn(
        ITableArrayAllocator<T> arrayAllocator,
        List<TableArray<T>> arrays)
    {
        _arrayAllocator = arrayAllocator;
        _arrays = arrays;
    }

    public override void Dispose()
    {
        _arrays.DisposeAll();
    }

    public override TableArray PushArray()
    {
        TableArray<T> array = _arrayAllocator.AllocateArray();
        _arrays.Add(array);
        return array;
    }

    public override void PopArray()
    {
        _arrays.RemoveAt(_arrays.Count - 1);
    }
}

public abstract class TableArray : IDisposable
{
    public abstract void Dispose();

    public abstract Array GetArray();
}

public sealed class TableArray<T> : TableArray
{
    private ArrayPool<T>? _arrayPool;

    public TableArray(T[] array) : this(null, array)
    {
    }

    public TableArray(ArrayPool<T>? arrayPool, T[] array)
    {
        _arrayPool = arrayPool;
        Array = array;
    }

    public T[] Array { get; }

    public override void Dispose()
    {
        if (_arrayPool is not null)
        {
            _arrayPool.Return(Array);
            _arrayPool = null;
        }
    }

    public override Array GetArray()
    {
        return Array;
    }
}

public interface ITableArrayAllocator<T>
{
    public TableArray<T> AllocateArray();
}