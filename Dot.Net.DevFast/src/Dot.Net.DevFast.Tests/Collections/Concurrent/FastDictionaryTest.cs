using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dot.Net.DevFast.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Collections.Concurrent
{
    [TestFixture]
    public class FastDictionaryTest
    {
        [Test]
        public void Ctor_With_IEnumerable_Works_Well()
        {
            var coll = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1) };
            var instance = new FastDictionary<int, int>(coll);
            Assert.IsFalse(instance.IsReadOnly);
            Assert.IsFalse(instance.IsEmpty);
            Assert.IsTrue(((IReadOnlyCollection<KeyValuePair<int, int>>)instance).Count == 1);
            foreach (var pair in ((IEnumerable<KeyValuePair<int, int>>)coll))
            {
                Assert.AreEqual(pair.Key, pair.Value);
            }
            var arr = new KeyValuePair<int, int>[1];
            instance.CopyTo(arr, 0);
            Assert.IsTrue(arr[0].Key == arr[0].Value);
            Assert.IsTrue(arr[0].Key == 1);
            instance.Clear();
            Assert.IsTrue(instance.IsEmpty);
        }

        [Test]
        public void Different_Contains_Works_Well()
        {
            var coll = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1) };
            var instance = new FastDictionary<int, int>(coll);
            Assert.IsTrue(instance.Contains(new KeyValuePair<int, int>(1, 1)));
            Assert.IsFalse(instance.Contains(new KeyValuePair<int, int>(1, 2)));
            Assert.IsFalse(instance.ContainsKey(2));
            Assert.IsTrue(((IReadOnlyDictionary<int, int>)instance).ContainsKey(1));
            Assert.IsTrue(instance.ContainsKey(1));
        }

        [Test]
        public void Different_Remove_Works_Well()
        {
            var coll = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(1, 1) };
            var instance = new FastDictionary<int, int>(coll);
            Assert.IsTrue(instance.Remove(new KeyValuePair<int, int>(1, 1)));
            Assert.IsFalse(instance.Remove(new KeyValuePair<int, int>(1, 2)));
            Assert.IsFalse(instance.Remove(1));
            instance.Add(1, 1);
            Assert.IsTrue(instance.Remove(1));
            instance.Add(1, 1);
            Assert.IsTrue(instance.TryRemove(1, out var v) && v.Equals(1));
        }

        [Test]
        public void Different_Add_Works_Well()
        {
            var instance = new FastDictionary<int, int>();
            instance[1] = 1;
            Assert.AreEqual(1, instance[1]);
            Assert.AreEqual(1, instance.GetOrAdd(1, _ => 1));
            Assert.AreEqual(2, instance.GetOrAdd(2, _ => 2));
            Assert.AreEqual(3, instance.AddOrUpdate(2, 2, (_, __) => 3));
            Assert.AreEqual(2, instance.AddOrUpdate(3, 2, (_, __) => 3));
            Assert.IsFalse(instance.TryAdd(3, 2));
            Assert.IsTrue(instance.TryAdd(4, 2));
            instance.Add(new KeyValuePair<int, int>(5, 5));
            Assert.Throws<ArgumentException>(() => instance.Add(5, 5));
            Assert.IsTrue(instance.TryUpdate(5, 2, 5) && ((IReadOnlyDictionary<int, int>)instance).TryGetValue(5, out var vv) && vv == 2);
            Assert.IsFalse(instance.TryUpdate(5, 2, 5));
        }

        [Test]
        public void Enumeration_Works_Well()
        {
            var instance = new FastDictionary<int, int>(0, 2);
            instance[1] = 1;
            instance[2] = 1;
            instance[3] = 1;
            var pairs = instance.ToList();
            Assert.AreEqual(3, pairs.Count);
            Assert.IsTrue(1 <= pairs[0].Key && 3 >= pairs[0].Key);
            Assert.AreEqual(1, pairs[0].Value);
            var keys = instance.Keys;
            Assert.AreEqual(3, keys.Count);
            Assert.IsTrue(1 <= keys.First() && 3 >= keys.First());
            var values = instance.Values;
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual(1, values.First());

            var en = ((IEnumerable)instance).GetEnumerator();
            Assert.IsTrue(en.MoveNext());
            Assert.IsTrue(en.Current is KeyValuePair<int, int>);

            var en1 = ((IEnumerable)((IReadOnlyDictionary<int, int>)instance).Keys).GetEnumerator();
            Assert.IsTrue(en1.MoveNext());
            Assert.IsTrue(en1.Current is int);

            var en2 = ((IEnumerable)((IReadOnlyDictionary<int, int>)instance).Values).GetEnumerator();
            Assert.IsTrue(en2.MoveNext());
            Assert.IsTrue(en2.Current is int);

            instance.Clear(false);
        }
    }
}