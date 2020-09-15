namespace Dot.Net.DevFast.Collections
{
    /// <summary>
    /// Abstract binary heap based limited element heap, i.e. k-heap on N elements.
    /// That means unlimited of elements (N) can be added, but, only limited number of elements (k) can be popped out.
    /// It will maintain the order on the added elements (before each pop) without consuming extra space.
    /// <example>
    /// When among long list of numbers, but we are interested to find only TOP/BOTTOM k
    /// elements, this abstract class is the right choice.
    /// </example>
    /// </summary>
    public abstract class AbstractLimitHeap
    {
        //todo: Add new member in heap interface... AddAll and PopAll
    }
}