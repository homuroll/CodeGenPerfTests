using System;
using System.Collections.Generic;

namespace SwitchStrings
{
    public class Runner
    {
        public Runner(int numberOfKeys, int seed)
        {
            random = new Random(seed);
            var keys = new string[numberOfKeys];
            var values = new int[numberOfKeys];
            for(int i = 0; i < numberOfKeys; ++i)
            {
                int len = random.Next(3, 10);
                var arr = new char[len];
                for(int j = 0; j < len; ++j)
                    arr[j] = (char)random.Next(33, 128);
                keys[i] = new string(arr);
                values[i] = i * 3;
            }

            testKeys = new string[numberOfKeys * 20];
            for(int i = 0; i < testKeys.Length / 2; ++i)
                testKeys[i] = keys[random.Next(numberOfKeys)];
            for(int i = testKeys.Length / 2; i < testKeys.Length; ++i)
            {
                int len = random.Next(3, 10);
                var arr = new char[len];
                for(int j = 0; j < len; ++j)
                    arr[j] = (char)random.Next(33, 128);
                testKeys[i] = new string(arr);
            }
            RandomShuffle(testKeys, random);

            var dict = new Dictionary<string, int>();
            for(int i = 0; i < numberOfKeys; ++i)
                dict.Add(keys[i], values[i]);
            dictionary = dict.TryGetValue;

            perfectHashtable = PerfectHashtableBuilder.Build(keys, values, FastHashEvaluator.Create(keys));

            perfectHashtableWithRoslynHash = PerfectHashtableBuilder.Build(keys, values, RoslynHashEvaluator.Create(keys).ComputeStringHash);

            trie = TrieBuilder.Build(keys, values);

            bunchOfIffs = BunchOfIfsBuilder.Build(keys, values);

            unrolledBSWithRoslynHash = UnrolledBinarySearchBuilder.Build(keys, values, RoslynHashEvaluator.Create(keys).ComputeStringHash);

            unrolledBSWithFastHash = UnrolledBinarySearchBuilder.Build(keys, values, FastHashEvaluator.Create(keys));
        }

        public int Run_Empty()
        {
            RandomShuffle(testKeys, random);
            return 0;
        }

        public int Run_Dictionary()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(dictionary(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_BunchOfIfs()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(bunchOfIffs(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_PerfectHashtable()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(perfectHashtable(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_PerfectHashtableWithRoslynHash()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(perfectHashtableWithRoslynHash(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_Trie()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(trie(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_UnrolledBSWithRoslynHash()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(unrolledBSWithRoslynHash(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_UnrolledBSWithFastHash()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(unrolledBSWithFastHash(key, out x))
                    sum += x;
            }
            return sum;
        }

        private static void RandomShuffle<T>(IList<T> list, Random random)
        {
            for(int i = 1; i < list.Count; ++i)
            {
                var j = random.Next(i);
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private readonly string[] testKeys;

        private readonly TryGetValueDelegate<int> dictionary;
        private readonly TryGetValueDelegate<int> perfectHashtable;
        private readonly TryGetValueDelegate<int> perfectHashtableWithRoslynHash;
        private readonly TryGetValueDelegate<int> trie;
        private readonly TryGetValueDelegate<int> bunchOfIffs;
        private readonly TryGetValueDelegate<int> unrolledBSWithRoslynHash;
        private readonly TryGetValueDelegate<int> unrolledBSWithFastHash;
        private readonly Random random;
    }
}