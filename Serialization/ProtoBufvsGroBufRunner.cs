using System;
using System.Diagnostics;

using BenchmarkDotNet.Attributes;

using Serialization.TestData.Invoic;
using Serialization.TestData.Orders;

namespace Serialization
{
    public class ProtoBufvsGroBufRunner
    {
        public void Test()
        {
            var objects = Generate<Orders>(2, 60, 10, 5);
            var protobufRunner = new ProtoBufRunner<Orders>(objects);
            var objectsStringLeafs = Generate<Invoic>(8, 60, 10, 5);
            var protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(objectsStringLeafs);
            Console.WriteLine(protobufRunner.Serialize() + protobufRunnerStringLeafs.Serialize());
        }

        [Benchmark]
        public int GroBufSerialize()
        {
            return grobufRunner.Serialize() + grobufRunnerStringLeafs.Serialize();
        }

        [Benchmark]
        public object GroBufDeserialize()
        {
            return grobufRunner.Deserialize() ?? grobufRunnerStringLeafs.Deserialize();
        }

        [Benchmark]
        public int ProtoBufSerialize()
        {
            return protobufRunner.Serialize() + protobufRunnerStringLeafs.Serialize();
        }

        [Benchmark]
        public object ProtoBufDeserialize()
        {
            return protobufRunner.Deserialize() ?? protobufRunnerStringLeafs.Deserialize();
        }

        [Setup]
        public void Setup()
        {
            switch(mode)
            {
            case "small":
                {
                    var objects = Generate<Orders>(10, 30, 5, 2);
                    grobufRunner = new GroBufRunner<Orders>(objects);
                    protobufRunner = new ProtoBufRunner<Orders>(objects);
                    grobufRunnerStringLeafs = new GroBufRunner<Invoic>(new Invoic[0]);
                    protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(new Invoic[0]);
                    break;
                }
            case "big":
                {
                    var objects = Generate<Orders>(10, 60, 10, 5);
                    grobufRunner = new GroBufRunner<Orders>(objects);
                    protobufRunner = new ProtoBufRunner<Orders>(objects);
                    grobufRunnerStringLeafs = new GroBufRunner<Invoic>(new Invoic[0]);
                    protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(new Invoic[0]);
                    break;
                }
            case "small_strings":
                {
                    var objects = Generate<Invoic>(10, 30, 5, 2);
                    grobufRunner = new GroBufRunner<Orders>(new Orders[0]);
                    protobufRunner = new ProtoBufRunner<Orders>(new Orders[0]);

                    grobufRunnerStringLeafs = new GroBufRunner<Invoic>(objects);
                    protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(objects);
                    break;
                }
            case "big_strings":
                {
                    var objects = Generate<Invoic>(10, 60, 10, 5);
                    grobufRunner = new GroBufRunner<Orders>(new Orders[0]);
                    protobufRunner = new ProtoBufRunner<Orders>(new Orders[0]);
                    grobufRunnerStringLeafs = new GroBufRunner<Invoic>(objects);
                    protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(objects);
                    break;
                }
            case "small_mixed":
                {
                    var objects = Generate<Orders>(20, 30, 5, 2);
                    grobufRunner = new GroBufRunner<Orders>(objects);
                    protobufRunner = new ProtoBufRunner<Orders>(objects);
                    var objectsStringLeafs = Generate<Invoic>(80, 30, 5, 2);
                    grobufRunnerStringLeafs = new GroBufRunner<Invoic>(objectsStringLeafs);
                    protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(objectsStringLeafs);
                    break;
                }
            case "big_mixed":
                {
                    var objects = Generate<Orders>(2, 60, 10, 5);
                    grobufRunner = new GroBufRunner<Orders>(objects);
                    protobufRunner = new ProtoBufRunner<Orders>(objects);
                    var objectsStringLeafs = Generate<Invoic>(8, 60, 10, 5);
                    grobufRunnerStringLeafs = new GroBufRunner<Invoic>(objectsStringLeafs);
                    protobufRunnerStringLeafs = new ProtoBufRunner<Invoic>(objectsStringLeafs);
                    break;
                }
            }
        }

        private static T[] Generate<T>(int n, int fillRate, int stringsLength, int arraysSize)
        {
            var random = new Random(54717651);
            var objects = new T[n];
            objects[0] = TestHelpers.GenerateRandomTrash<T>(random, fillRate, stringsLength, arraysSize);
            for(int i = 1; i < objects.Length; ++i)
                objects[i] = TestHelpers.GenerateRandomTrash<T>(random, fillRate, stringsLength, arraysSize);
            return objects;
        }

        private IRunner grobufRunner;
        private IRunner grobufRunnerStringLeafs;
        private IRunner protobufRunner;
        private IRunner protobufRunnerStringLeafs;

        [Params("big_mixed")]
        public string mode;
    }
}