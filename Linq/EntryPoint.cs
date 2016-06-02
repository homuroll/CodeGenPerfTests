using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Linq
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<Test1>(
            //    ManualConfig.Create(DefaultConfig.Instance)
            //                .With(Job.LegacyJitX86)
            //                .With(Job.LegacyJitX64)
            //                .With(Job.RyuJitX64)
            //                .With(Job.Mono));
            BenchmarkRunner.Run<Test2>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono));
            //BenchmarkRunner.Run<Test3>(
            //    ManualConfig.Create(DefaultConfig.Instance)
            //                .With(Job.LegacyJitX86)
            //                .With(Job.LegacyJitX64)
            //                .With(Job.RyuJitX64)
            //                .With(Job.Mono));
        }
    }
}