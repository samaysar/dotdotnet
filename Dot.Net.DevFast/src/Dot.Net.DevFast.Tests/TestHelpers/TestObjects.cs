using System;
using System.Threading;
using Dot.Net.DevFast.Collections;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.TestHelpers
{
    [JsonObject(MemberSerialization.OptOut)]
    public class TestObject
    {
        [JsonIgnore] private static readonly Random Ran = new Random();

        [JsonConstructor]
        public TestObject()
        {
            IntProp = Ran.Next();
            StrProp = Guid.NewGuid().ToString("N");
            BytesProp = new byte[8];
            Ran.NextBytes(BytesProp);
        }

        public string StrProp { get; set; }
        public int IntProp { get; set; }
        public byte[] BytesProp { get; set; }
    }

    public class TestAbstractBinaryHeap : AbstractBinaryHeap<int>
    {
        private readonly Func<int, int, bool> _comparer;

        public TestAbstractBinaryHeap(int initialCapacity,
            Func<int, int, bool> comparer) : base(initialCapacity)
        {
            _comparer = comparer;
        }

        protected override bool LeftPrecedes(int left, int right)
        {
            return _comparer(left, right);
        }
    }

    public class TestAbstractLimitHeap : AbstractLimitHeap<int>
    {
        public TestAbstractLimitHeap(int heapSize) : base(heapSize)
        {
        }

        protected override bool LeftPrecedes(int left, int right)
        {
            return left > right;
        }
    }
}