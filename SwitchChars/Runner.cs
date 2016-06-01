using System;
using System.Collections.Generic;

namespace SwitchChars
{
    public class Runner
    {
        public Runner(int numberOfSegments, int numberOfKeysPerSegment, int seed)
        {
            random = new Random(seed);
            var totalNumberOfKeys = numberOfSegments * numberOfKeysPerSegment;
            var keys = new char[totalNumberOfKeys];
            var values = new int[totalNumberOfKeys];

            int segmentLength = 65535 / numberOfSegments;

            for(int i = 0; i < numberOfSegments; ++i)
            {
                var hashSet = new HashSet<char>();
                for(int j = 0; j < numberOfKeysPerSegment; ++j)
                {
                    while(true)
                    {
                        char c = (char)random.Next(1 + i * segmentLength, 1 + i * segmentLength + numberOfKeysPerSegment * 4);
                        if(hashSet.Contains(c)) continue;
                        hashSet.Add(c);
                        keys[i * numberOfKeysPerSegment + j] = c;
                        break;
                    }
                }
            }
            Array.Sort(keys);
            for(int i = 0; i < totalNumberOfKeys; ++i)
                values[i] = 3 * i;

            testKeys = new char[totalNumberOfKeys * 20];
            for(int i = 0; i < testKeys.Length / 2; ++i)
                testKeys[i] = keys[random.Next(totalNumberOfKeys)];
            for(int i = testKeys.Length / 2; i < testKeys.Length; ++i)
                testKeys[i] = (char)random.Next(1, 65536);
            RandomShuffle(testKeys, random);

            var dict = new Dictionary<char, int>();
            for(int i = 0; i < totalNumberOfKeys; ++i)
                dict.Add(keys[i], values[i]);
            dictionary = dict.TryGetValue;

            perfectHashtable = new PerfectHashtable<int>(keys, values).TryGetValue;

            bunchOfIfs = BunchOfIfsBuilder.Build(keys, values);

            unrolledBinarySearch = UnrolledBinarySearchBuilder.Build(keys, values);

            binarySearch = new BinarySearch(keys, values).TryGetValue;

            arrayOfSegments = ArrayOfSegmentsBuilder.Build(keys, values, numberOfSegments, numberOfKeysPerSegment);

            arrayOfSegmentsWithBS = ArrayOfSegmentsWithBinarySearchBuilder.Build(keys, values, numberOfSegments, numberOfKeysPerSegment);
        }

        private class BinarySearch
        {
            private readonly char[] keys;
            private readonly int[] values;

            public BinarySearch(char[] keys, int[] values)
            {
                this.keys = keys;
                this.values = values;
            }

            public bool TryGetValue(char key, out int value)
            {
                int l = 0, r = keys.Length - 1;
                while(l <= r)
                {
                    int m = (l + r) >> 1;
                    if(keys[m] == key)
                    {
                        value = values[m];
                        return true;
                    }
                    if(key < keys[m])
                        r = m - 1;
                    else l = m + 1;
                }
                value = 0;
                return false;
            }
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

        public int Run_BunchOfIfs()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(bunchOfIfs(key, out x))
                    sum += x;
            }
            return sum;
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

        public int Run_ArrayOfSegments()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(arrayOfSegments(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_ArrayOfSegmentsWithBS()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(arrayOfSegmentsWithBS(key, out x))
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

        public int Run_UnrolledBinarySearch()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach(var key in testKeys)
            {
                int x;
                if(unrolledBinarySearch(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_BinarySearch()
        {
            RandomShuffle(testKeys, random);
            int sum = 0;
            foreach (var key in testKeys)
            {
                int x;
                if (binarySearch(key, out x))
                    sum += x;
            }
            return sum;
        }

        public int Run_Empty()
        {
            RandomShuffle(testKeys, random);
            return 0;
        }

        private readonly char[] testKeys;

        private readonly TryGetValueDelegate<int> dictionary;
        private readonly TryGetValueDelegate<int> bunchOfIfs;
        private readonly TryGetValueDelegate<int> unrolledBinarySearch;
        private readonly TryGetValueDelegate<int> binarySearch;
        private readonly TryGetValueDelegate<int> arrayOfSegments;
        private readonly TryGetValueDelegate<int> arrayOfSegmentsWithBS;
        private readonly TryGetValueDelegate<int> perfectHashtable;
        private Random random;
    }
}