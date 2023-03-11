using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dot.Net.DevFast.Collections;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections
{
    [TestFixture]
    public class OneToManyDictionaryTest
    {
        [Test]
        public void Ctor_N_Properties_Are_Well_Defined()
        {
            var instance = new OneToManyDictionary<int, int> { { 1, 1 } };
            Assert.AreEqual(1, instance.Count);
            Assert.IsTrue(((IDictionary)instance).GetEnumerator().MoveNext());
            Assert.IsTrue(((IEnumerable)instance).GetEnumerator().MoveNext());
            Assert.NotNull(instance.SyncRoot);
            Assert.IsFalse(instance.IsSynchronized);
            Assert.IsFalse(instance.IsFixedSize);
            Assert.IsFalse(((ICollection<KeyValuePair<int, List<int>>>)instance).IsReadOnly);
            Assert.IsFalse(((IDictionary)instance).IsReadOnly);
            instance.Clear();
            Assert.AreEqual(0, instance.Count);
        }
    }
}