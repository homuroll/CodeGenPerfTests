using System;
using System.Linq;
using System.Reflection;
using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Regex
{
    public class EntryPoint
    {
        private static string[] ReadTestData()
        {
            // the benchmark data is from http://lh3lh3.users.sourceforge.net/reb.shtml
            const string resourceName = "Regex.Data.howto";
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if(resourceStream == null)
                throw new InvalidOperationException($"Resource '{resourceName}' is not found");
            var content = new byte[resourceStream.Length];
            if(resourceStream.Read(content, 0, content.Length) != content.Length)
                throw new InvalidOperationException($"Unable to read data from resource '{resourceName}'");
            var result = new UTF8Encoding(false).GetString(content).Split('\r', '\n').ToArray();
            var random = new Random(314159);
            for(int i = 1; i < result.Length; ++i)
            {
                var j = random.Next(i);
                var temp = result[i];
                result[i] = result[j];
                result[j] = temp;
            }
            return result.ToArray();
        }

        [Benchmark]
        public void NotCompiled()
        {
            test.Run_RegexNotCompiled(lines);
        }

        [Benchmark]
        public void Compiled()
        {
            test.Run_RegexCompiled(lines);
        }

        [Benchmark]
        public void Automaton()
        {
            test.Run_Automaton(lines);
        }

        [Benchmark]
        public void OptimizedAutomaton()
        {
            test.Run_OptimizedAutomaton(lines);
        }

        [Benchmark(Baseline = true)]
        public void InlinedAutomaton()
        {
            test.Run_InlinedAutomaton(lines);
        }

        public static void Main()
        {
            BenchmarkRunner.Run<EntryPoint>(
                ManualConfig.Create(DefaultConfig.Instance)
                            .With(Job.LegacyJitX86)
                            .With(Job.LegacyJitX64)
                            .With(Job.RyuJitX64)
                            .With(Job.Mono)
                );
        }

        [Setup]
        public void Setup()
        {
            switch(type)
            {
            case "number":
                test = new Test_Number();
                break;
            case "uri":
                test = new Test_Uri();
                break;
            case "email":
                test = new Test_Email();
                break;
            default:
                throw new InvalidOperationException();
            }
        }

        private static readonly string[] lines = ReadTestData();

        private IRunner test;

        [Params("number", "uri", "email")]
        public string type;
    }
}