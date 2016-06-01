using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Expressions
{
    public class EntryPoint
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Test1>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono));
            BenchmarkRunner.Run<Test2>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono));
        }
    }

    public class TestClassA
    {
        public string S { get; set; }

        public decimal Y { get; set; }

        public TestClassB[] ArrayB { get; set; }
    }

    public class TestClassB
    {
        public string S { get; set; }

        public int? X { get; set; }

        public decimal Y { get; set; }

        public TestClassC C { get; set; }
    }

    public class TestClassC
    {
        public TestClassD[] ArrayD { get; set; }
    }

    public class TestClassD
    {
        public int X { get; set; }
    }
}