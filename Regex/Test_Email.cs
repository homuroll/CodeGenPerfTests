using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Regex
{
    public class Test_Email : IRunner
    {
        public void Run_RegexNotCompiled(string[] lines)
        {
            var regex = regex_notCompiled;
            foreach(var line in lines)
            {
                var match = regex.Match(line);
                while(match.Success)
                    match = match.NextMatch();
            }
        }

        public void Run_RegexCompiled(string[] lines)
        {
            var regex = regex_compiled;
            foreach(var line in lines)
            {
                var match = regex.Match(line);
                while(match.Success)
                    match = match.NextMatch();
            }
        }

        public void Run_Automaton(string[] lines)
        {
            foreach(var line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    var match = simpleAutomaton.Match(line, i);
                    if(match.length == 0)
                        break;
                    i = match.start + match.length;
                }
            }
        }

        public void Run_OptimizedAutomaton(string[] lines)
        {
            foreach(var line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    var match = optimzedAutomaton.Match(line, i);
                    if(match.length == 0)
                        break;
                    i = match.start + match.length;
                }
            }
        }

        public void Run_InlinedAutomaton(string[] lines)
        {
            foreach(var line in lines)
            {
                int i = 0;
                while(i < line.Length)
                {
                    var match = Match(line, i);
                    if(match.length == 0)
                        break;
                    i = match.start + match.length;
                }
            }
        }

        private static bool[] BuildCharSet()
        {
            var table = new bool[256];
            for(char c = 'a'; c <= 'z'; ++c)
                table[c] = true;
            for(char c = 'A'; c <= 'Z'; ++c)
                table[c] = true;
            for(char c = '0'; c <= '9'; ++c)
                table[c] = true;
            foreach(char c in "-!#$%&'*+/=?^_`{|}~")
                table[c] = true;
            return table;
        }

        // This method can be generated automatically
        private static MatchResult Match(string str, int begPos)
        {
            int idx = begPos;
            int len = str.Length;
            int atPos;
            int endPos = -1;
            bool wasAccepted = false;
            char c;

            _0:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c < 256 && charSet[c]) goto _0_2;
            begPos = idx;
            goto _0;

            _0_2:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c < 256 && charSet[c]) goto _0_2;
            if(c == '@') goto _0_8;
            if(c == '.') goto _0_5;
            begPos = idx;
            goto _0;

            _0_5:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c < 256 && charSet[c]) goto _0_2_6;
            begPos = idx;
            goto _0;

            _0_2_6:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c < 256 && charSet[c]) goto _0_2;
            if(c == '@') goto _0_8;
            if(c == '.') goto _0_5;
            begPos = idx;
            goto _0;

            _0_8:
            if(idx >= len)
                return new MatchResult();
            atPos = idx - 1;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_11_18;
            if(c >= 'A' && c <= 'Z') goto _0_2_11_18;
            if(c >= '0' && c <= '9') goto _0_2_11;
            if(c < 256 && charSet[c])
            {
                begPos = idx - 1;
                goto _0_2;
            }
            begPos = idx;
            goto _0;

            _0_2_11_18:
            if(idx >= len)
                return new MatchResult();
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13_19;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13_19;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5_15;
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = idx - 2;
                goto _0_2;
            }
            begPos = idx;
            goto _0;

            _0_2_12_13_19:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            wasAccepted = true;
            endPos = idx;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13_20;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13_20;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5_15;
            return new MatchResult
                {
                    start = begPos,
                    length = endPos - begPos
                };

            _0_2_12_13_20:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            wasAccepted = true;
            endPos = idx;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13_21;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13_21;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5_15;
            return new MatchResult
                {
                    start = begPos,
                    length = endPos - begPos
                };

            _0_2_12_13_21:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            wasAccepted = true;
            endPos = idx;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5_15;
            return new MatchResult
                {
                    start = begPos,
                    length = endPos - begPos
                };

            _0_2_12:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = atPos + 1;
                goto _0_2;
            }
            begPos = idx;
            goto _0;

            _0_2_11:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5_15;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = atPos + 1;
                goto _0_2;
            }
            begPos = idx;
            goto _0;

            _0_2_12_13:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_12_13;
            if(c >= '0' && c <= '9') goto _0_2_12_13;
            if(c == '-') goto _0_2_12;
            if(c == '.') goto _0_5_15;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = atPos + 1;
                goto _0_2;
            }
            begPos = idx;
            goto _0;

            _0_5_15:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_11_18;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_11_18;
            if(c >= '0' && c <= '9') goto _0_2_6_11;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '-')
            {
                begPos = idx - 1;
                goto _0_2_6;
            }
            if(c < 256 && charSet[c])
            {
                begPos = idx - 1;
                goto _0_2;
            }
            begPos = idx;
            goto _0;

            _0_2_6_11_18:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13_19;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13_19;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5_15;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = idx - 2;
                goto _0_2_6;
            }
            begPos = idx;
            goto _0;

            _0_2_6_12_13_19:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            wasAccepted = true;
            endPos = idx;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13_20;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13_20;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5_15;
            return new MatchResult
                {
                    start = begPos,
                    length = endPos - begPos
                };

            _0_2_6_12_13_20:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            wasAccepted = true;
            endPos = idx;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13_21;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13_21;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5_15;
            return new MatchResult
                {
                    start = begPos,
                    length = endPos - begPos
                };

            _0_2_6_12_13_21:
            if(idx >= len)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = len - begPos
                    };
            }
            wasAccepted = true;
            endPos = idx;
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5_15;
            return new MatchResult
                {
                    start = begPos,
                    length = endPos - begPos
                };

            _0_2_6_12:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = atPos + 1;
                goto _0_2_6;
            }
            begPos = idx;
            goto _0;

            _0_2_6_11:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5_15;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = atPos + 1;
                goto _0_2_6;
            }
            begPos = idx;
            goto _0;

            _0_2_6_12_13:
            if(idx >= len)
            {
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            start = begPos,
                            length = endPos - begPos
                        };
                }
                return new MatchResult();
            }
            c = str[idx++];
            if(c >= 'a' && c <= 'z') goto _0_2_6_12_13;
            if(c >= 'A' && c <= 'Z') goto _0_2_6_12_13;
            if(c >= '0' && c <= '9') goto _0_2_6_12_13;
            if(c == '-') goto _0_2_6_12;
            if(c == '.') goto _0_5_15;
            if(wasAccepted)
            {
                return new MatchResult
                    {
                        start = begPos,
                        length = endPos - begPos
                    };
            }
            if(c == '@')
            {
                begPos = atPos + 1;
                goto _0_8;
            }
            if(c < 256 && charSet[c])
            {
                begPos = atPos + 1;
                goto _0_2_6;
            }
            begPos = idx;
            goto _0;
        }

        private readonly System.Text.RegularExpressions.Regex regex_notCompiled = new System.Text.RegularExpressions.Regex(@"[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+(\.[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+)*@([a-zA-Z0-9]([-a-zA-Z0-9]*[a-zA-Z0-9])?\.)*([a-zA-Z]{2,4})");
        private readonly System.Text.RegularExpressions.Regex regex_compiled = new System.Text.RegularExpressions.Regex(@"[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+(\.[-a-zA-Z0-9!#$%&'*+/=?^_`{|}~]+)*@([a-zA-Z0-9]([-a-zA-Z0-9]*[a-zA-Z0-9])?\.)*([a-zA-Z]{2,4})", RegexOptions.Compiled);

        private readonly SimpleAutomaton simpleAutomaton = new SimpleAutomaton();
        private readonly OptimizedAutomaton optimzedAutomaton = new OptimizedAutomaton();

        private static readonly bool[] charSet = BuildCharSet();

        public class SimpleAutomaton
        {
            public SimpleAutomaton()
            {
                var s_0 = new Node();
                var s_0_2 = new Node();
                var s_0_5 = new Node();
                var s_0_2_6 = new Node();
                var s_0_8 = new Node();
                var s_0_2_11_18 = new Node();
                var s_0_2_12_13_19 = new Node();
                var s_0_2_12_13_20 = new Node();
                var s_0_2_12_13_21 = new Node();
                var s_0_2_12 = new Node();
                var s_0_2_11 = new Node();
                var s_0_2_12_13 = new Node();
                var s_0_5_15 = new Node();
                var s_0_2_6_11_18 = new Node();
                var s_0_2_6_12_13_19 = new Node();
                var s_0_2_6_12_13_20 = new Node();
                var s_0_2_6_12_13_21 = new Node();
                var s_0_2_6_12 = new Node();
                var s_0_2_6_11 = new Node();
                var s_0_2_6_12_13 = new Node();

                var rollbackTo = new Jump(s_0, -1);

                AddJumpByS1(s_0, s_0_2, -1);
                s_0.rollbackTo = rollbackTo;

                AddJumpByS1(s_0_2, s_0_2, -1);
                s_0_2.jumps.Add('@', new Jump(s_0_8, -1));
                s_0_2.jumps.Add('.', new Jump(s_0_5, -1));
                s_0_2.rollbackTo = rollbackTo;

                AddJumpByS1(s_0_5, s_0_2_6, -1);
                s_0_5.rollbackTo = rollbackTo;

                AddJumpByS1(s_0_2_6, s_0_2_6, -1);
                s_0_2_6.jumps.Add('@', new Jump(s_0_8, -1));
                s_0_2_6.jumps.Add('.', new Jump(s_0_5, -1));
                s_0_2_6.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_8, s_0_2_11_18, -1);
                AddJumpByDigit(s_0_8, s_0_2_11, -1);
                AddJumpByS1mS2(s_0_8, s_0_2, 0);
                s_0_8.jumps.Add('-', new Jump(s_0_2, 0));
                s_0_8.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_11_18, s_0_2_12_13_19, -1);
                s_0_2_11_18.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_11_18.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_11_18, s_0_2_12_13, -1);
                s_0_2_11_18.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_11_18, s_0_2, 1);
                s_0_2_11_18.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_12_13_19, s_0_2_12_13_20, -1);
                s_0_2_12_13_19.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_12_13_19.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_12_13_19, s_0_2_12_13, -1);
                s_0_2_12_13_19.rollbackTo = rollbackTo;
                s_0_2_12_13_19.accepted = true;

                AddJumpByAlpha(s_0_2_12_13_20, s_0_2_12_13_21, -1);
                s_0_2_12_13_20.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_12_13_20.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_12_13_20, s_0_2_12_13, -1);
                s_0_2_12_13_20.rollbackTo = rollbackTo;
                s_0_2_12_13_20.accepted = true;

                AddJumpByAlpha(s_0_2_12_13_21, s_0_2_12_13, -1);
                s_0_2_12_13_21.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_12_13_21.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_12_13_21, s_0_2_12_13, -1);
                s_0_2_12_13_21.rollbackTo = rollbackTo;
                s_0_2_12_13_21.accepted = true;

                AddJumpByAlpha(s_0_2_12, s_0_2_12_13, -1);
                s_0_2_12.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_12.jumps.Add('.', new Jump(s_0_5, -1));
                AddJumpByDigit(s_0_2_12, s_0_2_12_13, -1);
                s_0_2_12.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_12, s_0_2, -2);
                s_0_2_12.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_11, s_0_2_12_13, -1);
                s_0_2_11.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_11.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_11, s_0_2_12_13, -1);
                s_0_2_11.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_11, s_0_2, -2);
                s_0_2_11.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_12_13, s_0_2_12_13, -1);
                s_0_2_12_13.jumps.Add('-', new Jump(s_0_2_12, -1));
                s_0_2_12_13.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_12_13, s_0_2_12_13, -1);
                s_0_2_12_13.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_12_13, s_0_2, -2);
                s_0_2_12_13.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_5_15, s_0_2_6_11_18, -1);
                AddJumpByDigit(s_0_5_15, s_0_2_6_11, -1);
                AddJumpByS1mS2(s_0_5_15, s_0_2_6, 0);
                s_0_5_15.jumps.Add('-', new Jump(s_0_2_6, 0));
                s_0_5_15.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_6_11_18, s_0_2_6_12_13_19, -1);
                s_0_2_6_11_18.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_11_18.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_6_11_18, s_0_2_6_12_13, -1);
                s_0_2_6_11_18.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_6_11_18, s_0_2_6, 1);
                s_0_2_6_11_18.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_6_12_13_19, s_0_2_6_12_13_20, -1);
                s_0_2_6_12_13_19.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_12_13_19.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_6_12_13_19, s_0_2_6_12_13, -1);
                s_0_2_6_12_13_19.rollbackTo = rollbackTo;
                s_0_2_6_12_13_19.accepted = true;

                AddJumpByAlpha(s_0_2_6_12_13_20, s_0_2_6_12_13_21, -1);
                s_0_2_6_12_13_20.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_12_13_20.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_6_12_13_20, s_0_2_6_12_13, -1);
                s_0_2_6_12_13_20.rollbackTo = rollbackTo;
                s_0_2_6_12_13_20.accepted = true;

                AddJumpByAlpha(s_0_2_6_12_13_21, s_0_2_6_12_13, -1);
                s_0_2_6_12_13_21.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_12_13_21.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_6_12_13_21, s_0_2_6_12_13, -1);
                s_0_2_6_12_13_21.rollbackTo = rollbackTo;
                s_0_2_6_12_13_21.accepted = true;

                AddJumpByAlpha(s_0_2_6_12, s_0_2_6_12_13, -1);
                s_0_2_6_12.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_12.jumps.Add('.', new Jump(s_0_5, -1));
                AddJumpByDigit(s_0_2_6_12, s_0_2_6_12_13, -1);
                s_0_2_6_12.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_6_12, s_0_2_6, -2);
                s_0_2_6_12.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_6_11, s_0_2_6_12_13, -1);
                s_0_2_6_11.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_11.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_6_11, s_0_2_6_12_13, -1);
                s_0_2_6_11.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_6_11, s_0_2_6, -2);
                s_0_2_6_11.rollbackTo = rollbackTo;

                AddJumpByAlpha(s_0_2_6_12_13, s_0_2_6_12_13, -1);
                s_0_2_6_12_13.jumps.Add('-', new Jump(s_0_2_6_12, -1));
                s_0_2_6_12_13.jumps.Add('.', new Jump(s_0_5_15, -1));
                AddJumpByDigit(s_0_2_6_12_13, s_0_2_6_12_13, -1);
                s_0_2_6_12_13.jumps.Add('@', new Jump(s_0_8, -2));
                AddJumpByS1mS2(s_0_2_6_12_13, s_0_2_6, -2);
                s_0_2_6_12_13.rollbackTo = rollbackTo;

                start = s_0;
            }

            private void AddJumpByS1(Node from, Node to, int diff)
            {
                AddJumpByAlpha(from, to, diff);
                AddJumpByDigit(from, to, diff);
                AddJumpByS1mS2(from, to, diff);
                from.jumps.Add('-', new Jump(to, diff));
            }

            private void AddJumpByS1mS2(Node from, Node to, int diff)
            {
                var s1ms2 = "!#$%&'*+/=?^_`{|}~";
                AddJumpBySet(from, to, s1ms2, diff);
            }

            private void AddJumpByAlpha(Node from, Node to, int diff)
            {
                var jump = new Jump(to, diff);
                for(char c = 'a'; c <= 'z'; ++c)
                    from.jumps.Add(c, jump);
                for(char c = 'A'; c <= 'Z'; ++c)
                    from.jumps.Add(c, jump);
            }

            private void AddJumpByDigit(Node from, Node to, int diff)
            {
                var jump = new Jump(to, diff);
                for(char c = '0'; c <= '9'; ++c)
                    from.jumps.Add(c, jump);
            }

            private void AddJumpBySet(Node from, Node to, string set, int diff)
            {
                var jump = new Jump(to, diff);
                foreach(char c in set)
                    from.jumps.Add(c, jump);
            }

            public MatchResult Match(string str, int begPos)
            {
                var node = start;
                int atPos = -1;
                int endPos = 0;
                bool wasAccepted = false;
                for(int i = begPos; i < str.Length; ++i)
                {
                    if(node.accepted)
                    {
                        wasAccepted = true;
                        endPos = i;
                    }
                    Jump jump;
                    if(!node.jumps.TryGetValue(str[i], out jump))
                    {
                        if(wasAccepted)
                        {
                            return new MatchResult
                                {
                                    length = endPos - begPos,
                                    start = begPos
                                };
                        }
                        jump = node.rollbackTo;
                        begPos = i + 1;
                    }
                    else if(jump.diff != -1)
                    {
                        if(wasAccepted)
                        {
                            return new MatchResult
                                {
                                    length = endPos - begPos,
                                    start = begPos
                                };
                        }
                    }
                    node = jump.node;
                    if(jump.diff >= 0)
                        begPos = i - jump.diff;
                    else if(jump.diff == -2)
                        begPos = atPos + 1;
                    if(str[i] == '@')
                        atPos = i;
                }
                if(node.accepted)
                {
                    wasAccepted = true;
                    endPos = str.Length;
                }
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            length = endPos - begPos,
                            start = begPos
                        };
                }
                return new MatchResult();
            }

            private readonly Node start;

            private class Jump
            {
                public Jump(Node node, int diff)
                {
                    this.node = node;
                    this.diff = diff;
                }

                public readonly Node node;
                public readonly int diff;
            }

            private class Node
            {
                public readonly Dictionary<char, Jump> jumps = new Dictionary<char, Jump>();
                public Jump rollbackTo;
                public bool accepted;
            }
        }

        public class OptimizedAutomaton
        {
            public OptimizedAutomaton()
            {
                var s_0 = new Node2_0();
                var s_0_2 = new Node2_1();
                var s_0_5 = new Node2_0();
                var s_0_2_6 = new Node2_1();
                var s_0_8 = new Node2_2();
                var s_0_2_11_18 = new Node2_3();
                var s_0_2_12_13_19 = new Node2_4();
                var s_0_2_12_13_20 = new Node2_4();
                var s_0_2_12_13_21 = new Node2_4();
                var s_0_2_12 = new Node2_3();
                var s_0_2_11 = new Node2_3();
                var s_0_2_12_13 = new Node2_3();
                var s_0_5_15 = new Node2_2();
                var s_0_2_6_11_18 = new Node2_3();
                var s_0_2_6_12_13_19 = new Node2_4();
                var s_0_2_6_12_13_20 = new Node2_4();
                var s_0_2_6_12_13_21 = new Node2_4();
                var s_0_2_6_12 = new Node2_3();
                var s_0_2_6_11 = new Node2_3();
                var s_0_2_6_12_13 = new Node2_3();

                var rollbackTo = new Jump2(s_0, -1);

                s_0.child = new Jump2(s_0_2, -1);
                s_0.rollbackTo = rollbackTo;

                s_0_2.child = new Jump2(s_0_2, -1);
                s_0_2.child_at = new Jump2(s_0_8, -1);
                s_0_2.child_dot = new Jump2(s_0_5, -1);
                s_0_2.rollbackTo = rollbackTo;

                s_0_5.child = new Jump2(s_0_2_6, -1);
                s_0_5.rollbackTo = rollbackTo;

                s_0_2_6.child = new Jump2(s_0_2_6, -1);
                s_0_2_6.child_at = new Jump2(s_0_8, -1);
                s_0_2_6.child_dot = new Jump2(s_0_5, -1);
                s_0_2_6.rollbackTo = rollbackTo;

                s_0_8.child_az = new Jump2(s_0_2_11_18, -1);
                s_0_8.child_digit = new Jump2(s_0_2_11, -1);
                s_0_8.child = new Jump2(s_0_2, 0);
                s_0_8.child_minus = new Jump2(s_0_2, 0);
                s_0_8.rollbackTo = rollbackTo;

                s_0_2_11_18.child_az = new Jump2(s_0_2_12_13_19, -1);
                s_0_2_11_18.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_11_18.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_11_18.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_11_18.child_at = new Jump2(s_0_8, -2);
                s_0_2_11_18.child = new Jump2(s_0_2, 1);
                s_0_2_11_18.rollbackTo = rollbackTo;

                s_0_2_12_13_19.child_az = new Jump2(s_0_2_12_13_20, -1);
                s_0_2_12_13_19.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_12_13_19.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_12_13_19.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_12_13_19.rollbackTo = rollbackTo;
                s_0_2_12_13_19.accepted = true;

                s_0_2_12_13_20.child_az = new Jump2(s_0_2_12_13_21, -1);
                s_0_2_12_13_20.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_12_13_20.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_12_13_20.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_12_13_20.rollbackTo = rollbackTo;
                s_0_2_12_13_20.accepted = true;

                s_0_2_12_13_21.child_az = new Jump2(s_0_2_12_13, -1);
                s_0_2_12_13_21.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_12_13_21.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_12_13_21.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_12_13_21.rollbackTo = rollbackTo;
                s_0_2_12_13_21.accepted = true;

                s_0_2_12.child_az = new Jump2(s_0_2_12_13, -1);
                s_0_2_12.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_12.child_dot = new Jump2(s_0_5, -1);
                s_0_2_12.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_12.child_at = new Jump2(s_0_8, -2);
                s_0_2_12.child = new Jump2(s_0_2, -2);
                s_0_2_12.rollbackTo = rollbackTo;

                s_0_2_11.child_az = new Jump2(s_0_2_12_13, -1);
                s_0_2_11.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_11.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_11.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_11.child_at = new Jump2(s_0_8, -2);
                s_0_2_11.child = new Jump2(s_0_2, -2);
                s_0_2_11.rollbackTo = rollbackTo;

                s_0_2_12_13.child_az = new Jump2(s_0_2_12_13, -1);
                s_0_2_12_13.child_minus = new Jump2(s_0_2_12, -1);
                s_0_2_12_13.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_12_13.child_digit = new Jump2(s_0_2_12_13, -1);
                s_0_2_12_13.child_at = new Jump2(s_0_8, -2);
                s_0_2_12_13.child = new Jump2(s_0_2, -2);
                s_0_2_12_13.rollbackTo = rollbackTo;

                s_0_5_15.child_az = new Jump2(s_0_2_6_11_18, -1);
                s_0_5_15.child_digit = new Jump2(s_0_2_6_11, -1);
                s_0_5_15.child = new Jump2(s_0_2_6, 0);
                s_0_5_15.child_minus = new Jump2(s_0_2_6, 0);
                s_0_5_15.rollbackTo = rollbackTo;

                s_0_2_6_11_18.child_az = new Jump2(s_0_2_6_12_13_19, -1);
                s_0_2_6_11_18.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_11_18.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_6_11_18.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_11_18.child_at = new Jump2(s_0_8, -2);
                s_0_2_6_11_18.child = new Jump2(s_0_2_6, 1);
                s_0_2_6_11_18.rollbackTo = rollbackTo;

                s_0_2_6_12_13_19.child_az = new Jump2(s_0_2_6_12_13_20, -1);
                s_0_2_6_12_13_19.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_12_13_19.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_6_12_13_19.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12_13_19.rollbackTo = rollbackTo;
                s_0_2_6_12_13_19.accepted = true;

                s_0_2_6_12_13_20.child_az = new Jump2(s_0_2_6_12_13_21, -1);
                s_0_2_6_12_13_20.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_12_13_20.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_6_12_13_20.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12_13_20.rollbackTo = rollbackTo;
                s_0_2_6_12_13_20.accepted = true;

                s_0_2_6_12_13_21.child_az = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12_13_21.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_12_13_21.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_6_12_13_21.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12_13_21.rollbackTo = rollbackTo;
                s_0_2_6_12_13_21.accepted = true;

                s_0_2_6_12.child_az = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_12.child_dot = new Jump2(s_0_5, -1);
                s_0_2_6_12.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12.child_at = new Jump2(s_0_8, -2);
                s_0_2_6_12.child = new Jump2(s_0_2_6, -2);
                s_0_2_6_12.rollbackTo = rollbackTo;

                s_0_2_6_11.child_az = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_11.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_11.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_6_11.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_11.child_at = new Jump2(s_0_8, -2);
                s_0_2_6_11.child = new Jump2(s_0_2_6, -2);
                s_0_2_6_11.rollbackTo = rollbackTo;

                s_0_2_6_12_13.child_az = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12_13.child_minus = new Jump2(s_0_2_6_12, -1);
                s_0_2_6_12_13.child_dot = new Jump2(s_0_5_15, -1);
                s_0_2_6_12_13.child_digit = new Jump2(s_0_2_6_12_13, -1);
                s_0_2_6_12_13.child_at = new Jump2(s_0_8, -2);
                s_0_2_6_12_13.child = new Jump2(s_0_2_6, -2);
                s_0_2_6_12_13.rollbackTo = rollbackTo;

                start = s_0;
            }

            public MatchResult Match(string str, int begPos)
            {
                var node = start;
                int atPos = -1;
                int endPos = 0;
                bool wasAccepted = false;
                for(int i = begPos; i < str.Length; ++i)
                {
                    if(node.accepted)
                    {
                        wasAccepted = true;
                        endPos = i;
                    }
                    var jump = node.Jump(str[i]);
                    if(jump == null)
                    {
                        if(wasAccepted)
                        {
                            return new MatchResult
                                {
                                    length = endPos - begPos,
                                    start = begPos
                                };
                        }
                        jump = node.rollbackTo;
                        begPos = i + 1;
                    }
                    else if(jump.diff != -1)
                    {
                        if(wasAccepted)
                        {
                            return new MatchResult
                                {
                                    length = endPos - begPos,
                                    start = begPos
                                };
                        }
                    }
                    node = jump.node;
                    if(jump.diff >= 0)
                        begPos = i - jump.diff;
                    else if(jump.diff == -2)
                        begPos = atPos + 1;
                    if(str[i] == '@')
                        atPos = i;
                }
                if(node.accepted)
                {
                    wasAccepted = true;
                    endPos = str.Length;
                }
                if(wasAccepted)
                {
                    return new MatchResult
                        {
                            length = endPos - begPos,
                            start = begPos
                        };
                }
                return new MatchResult();
            }

            private readonly Node2 start;

            private class Jump2
            {
                public Jump2(Node2 node, int diff)
                {
                    this.node = node;
                    this.diff = diff;
                }

                public readonly Node2 node;
                public readonly int diff;
            }

            private abstract class Node2
            {
                public abstract Jump2 Jump(char c);
                public Jump2 rollbackTo;
                public bool accepted;
            }

            private class Node2_0 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c < 256 && table[c])
                        return child;
                    return null;
                }

                private static readonly bool[] table = BuildCharSet();
                public Jump2 child;
            }

            private class Node2_1 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c == '.') return child_dot;
                    if(c == '@') return child_at;
                    if(c < 256 && table[c])
                        return child;
                    return null;
                }

                private static readonly bool[] table = BuildCharSet();
                public Jump2 child;
                public Jump2 child_at;
                public Jump2 child_dot;
            }

            private class Node2_2 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 'a' && c <= 'z') return child_az;
                    if(c >= 'A' && c <= 'Z') return child_az;
                    if(c >= '0' && c <= '9') return child_digit;
                    if(c == '-') return child_minus;
                    if(c < 256 && table[c])
                        return child;
                    return null;
                }

                private static readonly bool[] table = BuildCharSet();
                public Jump2 child;
                public Jump2 child_az;
                public Jump2 child_digit;
                public Jump2 child_minus;
            }

            private class Node2_3 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 'a' && c <= 'z') return child_az;
                    if(c >= 'A' && c <= 'Z') return child_az;
                    if(c >= '0' && c <= '9') return child_digit;
                    if(c == '-') return child_minus;
                    if(c == '@') return child_at;
                    if(c == '.') return child_dot;
                    if(c < 256 && table[c])
                        return child;
                    return null;
                }

                private static readonly bool[] table = BuildCharSet();
                public Jump2 child;
                public Jump2 child_az;
                public Jump2 child_digit;
                public Jump2 child_minus;
                public Jump2 child_at;
                public Jump2 child_dot;
            }

            private class Node2_4 : Node2
            {
                public override Jump2 Jump(char c)
                {
                    if(c >= 'a' && c <= 'z') return child_az;
                    if(c >= 'A' && c <= 'Z') return child_az;
                    if(c >= '0' && c <= '9') return child_digit;
                    if(c == '-') return child_minus;
                    if(c == '.') return child_dot;
                    return null;
                }

                public Jump2 child_az;
                public Jump2 child_digit;
                public Jump2 child_minus;
                public Jump2 child_dot;
            }
        }
    }
}