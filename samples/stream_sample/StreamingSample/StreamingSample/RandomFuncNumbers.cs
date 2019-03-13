using System;
using System.Threading.Tasks;

namespace StreamingSample
{
    public static class RandomFuncNumbers
    {
        public static async Task GenerateRandomIntegers(int total = 5)
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("============================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("Running Random INT Generator").ConfigureAwait(false);
            await Console.Out.WriteLineAsync("============================").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);
            var factory = RandomIntFactory(0, int.MaxValue / 2);
            int GeneratePositiveEvenRandomInteger() => GenerateNumberAndApply(factory, IsOdd, MultiplyByTwo, Identity);

            await Console.Out.WriteLineAsync($"Generating {total} EVEN Random Int withOUT Sugar:").ConfigureAwait(false);
            for (var i = 0; i < total; i++)
            {
                await Console.Out.WriteLineAsync($"{i+1}th Value is:{GeneratePositiveEvenRandomInteger()}").ConfigureAwait(false);
            }
            await Console.Out.WriteLineAsync().ConfigureAwait(false);


            Action<int, int> print = (val, i) => Console.WriteLine($"{i + 1}th Value is:{val}");
            await Console.Out.WriteLineAsync($"Generating {total} ODD Random Int WITH Sugar:").ConfigureAwait(false);
            var random = new Random();
            for (var i = 0; i < total; i++)
            {
                random.GenerateInt(0, int.MaxValue - 1)
                    .If(x => !IsOdd(x))
                    .Then(AddOne)
                    .And(v => print(v, i));
            }
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            Console.ReadLine();
        }

        private static int AddOne(int value)
        {
            return value + 1;
        }

        private static Func<int> RandomIntFactory(int min, int max)
        {
            var randomGenerator = new Random();
            return () => randomGenerator.Next(min, max);
        }

        private static bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

        private static int MultiplyByTwo(int value)
        {
            return value * 2;
        }

        private static int Identity(int value)
        {
            return value;
        }

        private static T GenerateNumberAndApply<T>(Func<T> factory,
            Func<T, bool> predicateFactory,
            Func<T, T> whenTrue,
            Func<T, T> whenFalse)
        {
            var value = factory();
            return predicateFactory(value) ? whenTrue(value) : whenFalse(value);
        }

        private static Func<int> GenerateInt(this Random value, int min, int max)
        {
            return () => value.Next(min, max);
        }

        private static Func<Func<int, int>, Func<int>> If(this Func<int> factory,
            Func<int, bool> predicateFunc)
        {
            //reusing all the function we defined above!
            return whenTrue => () => GenerateNumberAndApply(factory,
                predicateFunc,
                whenTrue,
                Identity);
        }

        private static Func<int> Then(this Func<Func<int, int>, Func<int>> conditionFunc,
            Func<int, int> whenTrue)
        {
            return conditionFunc(whenTrue);
        }

        private static void And(this Func<int> func, Action<int> apply)
        {
            apply(func());
        }
    }
}