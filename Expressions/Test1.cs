using System;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;

using GrobExp.Compiler;

namespace Expressions
{
    public class Test1
    {
        public Test1()
        {
            sharp = Sharp;
            Expression<Func<TestClassA, string>> exp = o => o.ArrayB[0].S;
            compile = exp.Compile();
            grobexpCompile = LambdaCompiler.Compile(exp, CompilerOptions.None);
            a = new TestClassA
                {
                    ArrayB = new[]
                        {
                            new TestClassB
                                {
                                    C = new TestClassC
                                        {
                                            ArrayD = new[]
                                                {
                                                    new TestClassD {X = 5}
                                                }
                                        }
                                }
                        }
                };
        }

        private static string Sharp(TestClassA a)
        {
            return a.ArrayB[0].S;
        }

        [Benchmark(Baseline = true)]
        public string Run_Sharp()
        {
            return sharp(a);
        }

        [Benchmark]
        public string Compile()
        {
            return compile(a);
        }

        [Benchmark]
        public string GrobExpCompile()
        {
            return grobexpCompile(a);
        }

        private readonly Func<TestClassA, string> sharp;
        private readonly Func<TestClassA, string> compile;
        private readonly Func<TestClassA, string> grobexpCompile;

        private readonly TestClassA a;
    }
}