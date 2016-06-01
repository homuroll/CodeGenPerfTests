using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;

using GrobExp.Compiler;

namespace Linq
{
    public class Test3
    {
        public Test3()
        {
            list = new List<Message>();
            for(int i = 0; i < 100000; ++i)
            {
                list.Add(new Message
                    {
                        SG7 = new[]
                            {
                                new SG7
                                    {
                                        Currencies = new Currencies
                                            {
                                                CurrencyDetails = new[] {new CurrencyDetails(), new CurrencyDetails(),}
                                            }
                                    },
                                new SG7
                                    {
                                        Currencies = new Currencies
                                            {
                                                CurrencyDetails = new[] {new CurrencyDetails(), new CurrencyDetails(),}
                                            }
                                    }
                            }
                    });
            }
            list.Add(new Message
                {
                    SG7 = new[]
                        {
                            new SG7
                                {
                                    Currencies = new Currencies
                                        {
                                            CurrencyDetails = new[]
                                                {
                                                    new CurrencyDetails
                                                        {
                                                            UsageCodeQualifier = "2",
                                                            TypeCodeQualifier = "4",
                                                            IdentificationCode = "zzz"
                                                        },
                                                }
                                        }
                                }
                        }
                });
            linq = x => (from message in x
                         from sg7 in message.SG7
                         from details in sg7.Currencies.CurrencyDetails
                         where details.UsageCodeQualifier == "2"
                               && details.TypeCodeQualifier == "4"
                         select details.IdentificationCode).FirstOrDefault();
            cyclesSharp = x =>
                {
                    string res = null;
                    for(int i = 0; i < x.Count; i++)
                    {
                        var sg7 = x[i].SG7;
                        for(int j = 0; j < sg7.Length; j++)
                        {
                            var currencyDetails = sg7[j].Currencies.CurrencyDetails;
                            for(int k = 0; k < currencyDetails.Length; k++)
                            {
                                var details = currencyDetails[k];
                                if(details.UsageCodeQualifier == "2" && details.TypeCodeQualifier == "4")
                                {
                                    res = details.IdentificationCode;
                                    goto _found;
                                }
                            }
                        }
                    }
                    _found:
                    return res;
                };
            Expression<Func<List<Message>, string>> exp = x => (from message in x
                                                                from sg7 in message.SG7
                                                                from details in sg7.Currencies.CurrencyDetails
                                                                where details.UsageCodeQualifier == "2"
                                                                      && details.TypeCodeQualifier == "4"
                                                                select details.IdentificationCode).FirstOrDefault();
            cyclesExpression = LambdaCompiler.Compile(exp.EliminateLinq(), CompilerOptions.None);
        }

        [Benchmark(Baseline = true)]
        public string Run_CyclesSharp()
        {
            return cyclesSharp(list);
        }

        [Benchmark]
        public string Run_CyclesExpression()
        {
            return cyclesExpression(list);
        }

        [Benchmark]
        public string Run_Linq()
        {
            return linq(list);
        }

        private readonly List<Message> list;
        private readonly Func<List<Message>, string> linq;
        private readonly Func<List<Message>, string> cyclesSharp;
        private readonly Func<List<Message>, string> cyclesExpression;

        private class Message
        {
            public SG7[] SG7 { get; set; }
        }

        private class SG7
        {
            public Currencies Currencies { get; set; }
        }

        private class Currencies
        {
            public CurrencyDetails[] CurrencyDetails { get; set; }
        }

        private class CurrencyDetails
        {
            public string UsageCodeQualifier { get; set; }
            public string TypeCodeQualifier { get; set; }
            public string IdentificationCode { get; set; }
        }
    }
}