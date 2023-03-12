using Dot.Net.DevFast.Collections;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
            instance.Clear();
            Assert.AreEqual(0, instance.Count);

            instance = new OneToManyDictionary<int, int>(new List<Tuple<int, List<int>>>
                { 1.ToTuple(new List<int> { 1 }) });
            Assert.AreEqual(1, instance.Count);
            instance = new OneToManyDictionary<int, int>(new List<KeyValuePair<int, List<int>>>
                { 1.ToKvp(new List<int> { 1 }) });
            Assert.AreEqual(1, instance.Count);
        }

        [Test]
        public void Add_Works_And_Can_Create_Dups_In_The_List()
        {
            var instance = new OneToManyDictionary<int, int>();
            Assert.AreEqual(0, instance.Count);
            Assert.IsFalse(instance.TryGetValue(1, out _));
            foreach (var next in new[] { 1, 1 })
            {
                instance.Add(1, next);
            }
            Assert.AreEqual(1, instance.Count);
            Assert.IsTrue(instance.TryGetValue(1, out var l) && l.Count == 2 && l[0] == l[1] && l[0] == 1);
        }

        [Test]
        public void Contains_Works_Well_For_Both_Positive_N_Negative_Case()
        {
            var instance = new OneToManyDictionary<int, int>();
            foreach (var next in new[] { 1, 2 })
            {
                instance.Add(1, next);
            }

            Assert.IsTrue(instance.Contains(1, 1));
            Assert.IsTrue(instance.Contains(1, 2));
            Assert.IsFalse(instance.Contains(1, 0));
            Assert.IsFalse(instance.Contains(0, 1));
        }

        [Test]
        public void Remove_Works_Well_For_Both_Positive_N_Negative_Case()
        {
            var instance = new OneToManyDictionary<int, int>();
            foreach (var next in new[] { 1, 1, 2 })
            {
                instance.Add(1, next);
            }

            Assert.IsTrue(instance.Remove(1, 2));
            Assert.IsTrue(instance.Remove(1, 1));
            Assert.IsFalse(instance.Remove(1, 0));
            Assert.IsTrue(instance.ContainsKey(1));
            Assert.IsTrue(instance.Remove(1, 1));
            Assert.IsFalse(instance.ContainsKey(1));
        }
    }
}