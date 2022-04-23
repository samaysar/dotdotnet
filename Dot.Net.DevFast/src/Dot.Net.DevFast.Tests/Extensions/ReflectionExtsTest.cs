using System;
using System.Threading.Tasks;
using Dot.Net.DevFast.Extensions;
using NUnit.Framework;

namespace Dot.Net.DevFast.Tests.Extensions
{
    [TestFixture]
    public class ReflectionExtsTest
    {
        private static int _normalCalled = 0;
        private static int _genericCalled = 0;

        public static Task NormalTask()
        {
            _normalCalled++;
            return Task.CompletedTask;
        }

        public static void NormalVoid()
        {
            _normalCalled++;
        }

        public static Task GenericTask<T>(T _)
        {
            _genericCalled++;
            return Task.CompletedTask;
        }

        public static void GenericVoid<T>(T _)
        {
            _genericCalled++;
        }

        public static Task<int> NormalTaskValue(int val)
        {
            _normalCalled++;
            return Task.FromResult(val);
        }

        public static Task<Derived> NormalTaskValueInheritance(Derived val)
        {
            _normalCalled++;
            return Task.FromResult(val);
        }

        public static Task<T> GenericTaskValue<T>(T val)
        {
            _genericCalled++;
            return Task.FromResult(val);
        }

        public static int NormalValue(int val)
        {
            _normalCalled++;
            return val;
        }

        public static Derived NormalValueInheritance(Derived val)
        {
            _normalCalled++;
            return val;
        }

        public static T GenericValue<T>(T val)
        {
            _genericCalled++;
            return val;
        }

        public class Base
        {
            public virtual int Value { get; } = 10;
        }

        public class Derived : Base
        {
            public override int Value { get; } = 11;
        }

        [Test]
        [Order(1)]
        public async Task InvokeNonValueMethodAsync_Works_Fine_For_Both_Normal_And_Generic()
        {
            _normalCalled = 0;
            _genericCalled = 0;
            await typeof(ReflectionExtsTest).InvokeNonValueMethodAsync(nameof(NormalTask)).ConfigureAwait(false);
            Assert.AreEqual(1, _normalCalled);

            await typeof(ReflectionExtsTest).InvokeNonValueMethodAsync(nameof(NormalVoid)).ConfigureAwait(false);
            Assert.AreEqual(2, _normalCalled);

            await typeof(ReflectionExtsTest)
                .InvokeNonValueMethodAsync(nameof(GenericTask), new[] { typeof(object) }, new[] { new object() })
                .ConfigureAwait(false);
            Assert.AreEqual(1, _genericCalled);

            await typeof(ReflectionExtsTest)
                .InvokeNonValueMethodAsync(nameof(GenericVoid), new[] { typeof(object) }, new[] { new object() })
                .ConfigureAwait(false);
            Assert.AreEqual(2, _genericCalled);

            await typeof(ReflectionExtsTest)
                .InvokeNonValueMethodAsync(nameof(GenericValue), new[] { typeof(Task) }, new object[] { Task.CompletedTask })
                .ConfigureAwait(false);
            Assert.AreEqual(3, _genericCalled);
        }

        [Test]
        [Order(2)]
        public void InvokeNonValueMethodAsync_Throws_Error_When_Method_Returns_Type_Is_Neither_Void_Nor_NonGenericTask()
        {
            _normalCalled = 0;
            _genericCalled = 0;
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await typeof(ReflectionExtsTest)
                    .InvokeNonValueMethodAsync(nameof(NormalValue), null, new object[] { 5 }).ConfigureAwait(false));
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await typeof(ReflectionExtsTest)
                    .InvokeNonValueMethodAsync(nameof(NormalTaskValue), null, new object[] { 5 }).ConfigureAwait(false));
            Assert.AreEqual(0, _normalCalled);

            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeNonValueMethodAsync(nameof(GenericValue), new[] { typeof(object) }, new[] { new object() })
                .ConfigureAwait(false));
            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeNonValueMethodAsync(nameof(GenericTaskValue), new[] { typeof(object) }, new[] { new object() })
                .ConfigureAwait(false));
            Assert.AreEqual(0, _genericCalled);
        }

        [Test]
        [Order(2)]
        public void InvokeNonValueMethodAsync_Throws_Error_For_Unfound_Methods()
        {
            Assert.ThrowsAsync<MissingMethodException>(async () =>
                await typeof(ReflectionExtsTest)
                    .InvokeNonValueMethodAsync(nameof(NormalTask),
                        bindingFlags: System.Reflection.BindingFlags.Instance).ConfigureAwait(false));
        }

        [Test]
        [Order(2)]
        public void InvokeValueMethodAsync_Throws_Error_For_Unfound_Methods()
        {
            Assert.ThrowsAsync<MissingMethodException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<int>(nameof(NormalValue), bindingFlags: System.Reflection.BindingFlags.Instance)
                .ConfigureAwait(false));
        }

        [Test]
        [Order(3)]
        public async Task InvokeValueMethodAsync_Works_Fine_For_Both_Normal_And_Generic()
        {
            var value = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<int>(nameof(NormalValue), null, new object[] { 5 })
                .ConfigureAwait(false);
            Assert.AreEqual(5, value);

            value = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<int>(nameof(NormalTaskValue), null, new object[] { 5 })
                .ConfigureAwait(false);
            Assert.AreEqual(5, value);

            var value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(NormalValueInheritance), null, new object[] { new Derived() })
                .ConfigureAwait(false);
            Assert.AreEqual(11, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(NormalTaskValueInheritance), null, new object[] { new Derived() })
                .ConfigureAwait(false);
            Assert.AreEqual(11, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericValue), new[] { typeof(Derived) },
                    new object[] { new Derived() })
                .ConfigureAwait(false);
            Assert.AreEqual(11, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericTaskValue), new[] { typeof(Derived) },
                    new object[] { new Derived() })
                .ConfigureAwait(false);
            Assert.AreEqual(11, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericValue), new[] { typeof(Base) },
                    new object[] { new Derived() })
                .ConfigureAwait(false);
            Assert.AreEqual(11, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericTaskValue), new[] { typeof(Base) },
                    new object[] { new Derived() })
                .ConfigureAwait(false);
            Assert.AreEqual(11, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericValue), new[] { typeof(Base) },
                    new object[] { new Base() })
                .ConfigureAwait(false);
            Assert.AreEqual(10, value2.Value);

            value2 = await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericTaskValue), new[] { typeof(Base) },
                    new object[] { new Base() })
                .ConfigureAwait(false);
            Assert.AreEqual(10, value2.Value);
        }

        [Test]
        [Order(4)]
        public void InvokeValueMethodAsync_Throws_Error_When_Method_Returns_Type_Is_Neither_Given_Type_Nor_Its_Derived_Type_Nor_Task_Containing_TypeOrDerivedType()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await typeof(ReflectionExtsTest)
                    .InvokeValueMethodAsync<decimal>(nameof(NormalValue), null, new object[] { 5 }).ConfigureAwait(false));
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await typeof(ReflectionExtsTest)
                    .InvokeValueMethodAsync<decimal>(nameof(NormalTaskValue), null, new object[] { 5 }).ConfigureAwait(false));

            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericValue), new[] { typeof(object) }, new[] { new object() })
                .ConfigureAwait(false));
            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Base>(nameof(GenericTaskValue), new[] { typeof(object) }, new[] { new object() })
                .ConfigureAwait(false));

            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Derived>(nameof(GenericValue), new[] { typeof(Base) }, new object[] { new Derived() })
                .ConfigureAwait(false));
            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Derived>(nameof(GenericTaskValue), new[] { typeof(Base) }, new object[] { new Derived() })
                .ConfigureAwait(false));

            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Derived>(nameof(GenericValue), new[] { typeof(Base) }, new object[] { new Base() })
                .ConfigureAwait(false));
            Assert.ThrowsAsync<ArgumentException>(async () => await typeof(ReflectionExtsTest)
                .InvokeValueMethodAsync<Derived>(nameof(GenericTaskValue), new[] { typeof(Base) }, new object[] { new Base() })
                .ConfigureAwait(false));
        }
    }
}