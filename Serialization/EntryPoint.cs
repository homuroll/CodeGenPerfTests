using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using ProtoBuf;

using Serialization.Dynamic;
using Serialization.Static;

namespace Serialization
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
//            new EntryPoint().Test();
//            return;
            //new ProtoBufvsGroBufRunner().Test();
            //return;
//            BenchmarkRunner.Run<EntryPoint>(
//                ManualConfig.Create(DefaultConfig.Instance)
//                            .With(Job.LegacyJitX86)
//                            .With(Job.LegacyJitX64)
//                            .With(Job.RyuJitX64)
//                            .With(Job.Mono)
//                );
//            new ProtoBufvsGroBufRunner().Test();
//            return;
            BenchmarkRunner.Run<ProtoBufvsGroBufRunner>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono)
                );
            //var data = new StaticSerializer().Serialize(new Flat {Number = 1, Room = new Room {Area = 100}});
            //var flat = new DynamicSerializer().Deserialize<Flat>(data);
            //Console.WriteLine(flat.Number);
            //Console.WriteLine(flat.Room.Area);
        }

        public EntryPoint()
        {
            flats = new Flat[100];
            var random = new Random(123123);
            for(int i = 0; i < flats.Length; ++i)
            {
                flats[i] = new Flat();
                flats[i].Number = random.Next();
                if(random.Next(5) > 0)
                {
                    flats[i].Kitchen = new Room
                        {
                            NumberOfDoors = random.Next(),
                            NumberOfWindows = random.Next(),
                            Area = random.Next()
                        };
                }
                if(random.Next(5) > 0)
                {
                    flats[i].Room = new Room
                        {
                            NumberOfDoors = random.Next(),
                            NumberOfWindows = random.Next(),
                            Area = random.Next()
                        };
                }
            }
            staticRunner = new SerializerRunner(flats, new StaticSerializer());
            dynamicRunner = new SerializerRunner(flats, new DynamicSerializer());
            grobufRunner = new GroBufRunner<Flat>(flats);
            protobufRunner = new ProtoBufRunner<Flat>(flats);
        }

        public void Test()
        {
            for(int i = 0; i < 100000; ++i)
                grobufRunner.Serialize();
        }

        [Benchmark]
        public int StaticSerialize()
        {
            return staticRunner.Serialize();
        }

        [Benchmark]
        public int DynamicSerialize()
        {
            return dynamicRunner.Serialize();
        }

        [Benchmark]
        public object StaticDeserialize()
        {
            return staticRunner.Deserialize();
        }

        [Benchmark]
        public object DynamicDeserialize()
        {
            return dynamicRunner.Deserialize();
        }

        [Benchmark]
        public int GroBufSerialize()
        {
            return grobufRunner.Serialize();
        }

        [Benchmark]
        public object GroBufDeserialize()
        {
            return grobufRunner.Deserialize();
        }

        [Benchmark]
        public int ProtoBufSerialize()
        {
            return protobufRunner.Serialize();
        }

        [Benchmark]
        public object ProtoBufDeserialize()
        {
            return protobufRunner.Deserialize();
        }

        private Flat[] flats;
        private SerializerRunner staticRunner;
        private SerializerRunner dynamicRunner;
        private IRunner grobufRunner;
        private IRunner protobufRunner;
    }

    [ProtoContract]
    public class Flat
    {
        [ProtoMember(1)]
        public int Number;
        [ProtoMember(2)]
        public Room Kitchen;
        [ProtoMember(3)]
        public Room Room;
    }

    [ProtoContract]
    public class Room
    {
        [ProtoMember(1)]
        public int NumberOfWindows;
        [ProtoMember(2)]
        public int NumberOfDoors;
        [ProtoMember(3)]
        public int Area;
    }
}
