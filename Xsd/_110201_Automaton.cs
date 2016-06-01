using System;
using System.Collections.Generic;
using System.Globalization;

namespace Xsd
{
    public class _110201_Automaton : ISchemaAutomaton
    {
        static _110201_Automaton()
        {
            map0 = new bool[256];
            for (char c = '1'; c <= '9'; c++)
                map0[c] = true;
            map1 = new bool[256];
            for (char c = '0'; c <= '9'; c++)
            {
                if (c != '4' && c != '5')
                    map1[c] = true;
            }
            map2 = new bool[256];
            for (char c = '0'; c <= '8'; c++)
                map2[c] = true;
        }

        public _110201_Automaton(int[] counters, bool[] existances)
        {
            this.counters = counters;
            this.existances = existances;
        }

        public void Reset()
        {
            state = 0;
            inFatalState = false;
            Array.Clear(counters, 0, counters.Length);
        }

        public SchemaAutomatonError StartElement(string name)
        {
            SchemaAutomatonError result = null;
            if (!inFatalState)
            {
                switch (state)
                {
                case 0:
                    {
                        if (name == "Файл")
                        {
                            state = 1;
                            depth = 1;
                            existances[0] = false;
                            existances[1] = false;
                            existances[2] = false;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "Файл" });
                        inFatalState = true;
                        {
                            state = -1;
                        }
                        path.Push(name);
                        return result;
                    }
                case 1:
                    {
                        if (name == "Документ")
                        {
                            state = 2;
                            depth = 2;
                            existances[3] = false;
                            existances[4] = false;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "Документ" });
                        inFatalState = true;
                        {
                            state = 20;
                            depth = 0;
                        }
                        path.Push(name);
                        return result;
                    }
                case 2:
                    {
                        if (name == "КнигаПрод")
                        {
                            state = 3;
                            depth = 3;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "КнигаПрод" });
                        inFatalState = true;
                        {
                            state = 19;
                            depth = 1;
                        }
                        path.Push(name);
                        return result;
                    }
                case 3:
                    {
                        if (name == "КнПродСтр")
                        {
                            state = 4;
                            depth = 4;
                            existances[5] = false;
                            existances[6] = false;
                            existances[7] = false;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "КнПродСтр" });
                        inFatalState = true;
                        {
                            state = 18;
                            depth = 2;
                        }
                        path.Push(name);
                        return result;
                    }
                case 4:
                    {
                        if (name == "КодВидОпер")
                        {
                            state = 5;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "КодВидОпер" });
                        inFatalState = true;
                        {
                            state = 17;
                            depth = 3;
                        }
                        path.Push(name);
                        return result;
                    }
                case 5:
                    {
                        result = new SchemaAutomatonError12(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 6;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 6:
                    {
                        if (name == "КодВидОпер")
                        {
                            state = 5;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        if (name == "ДокПдтвОпл")
                        {
                            state = 7;
                            depth = 5;
                            existances[8] = false;
                            existances[9] = false;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СвПокуп")
                        {
                            state = 9;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СвПос")
                        {
                            state = 13;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "КодВидОпер", "ДокПдтвОпл", "СвПокуп", "СвПос" });
                        inFatalState = true;
                        {
                            state = 17;
                            depth = 3;
                        }
                        path.Push(name);
                        return result;
                    }
                case 7:
                    {
                        result = new SchemaAutomatonError11(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 8;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 8:
                    {
                        if (name == "ДокПдтвОпл")
                        {
                            state = 7;
                            depth = 5;
                            existances[8] = false;
                            existances[9] = false;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СвПокуп")
                        {
                            state = 9;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СвПос")
                        {
                            state = 13;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "ДокПдтвОпл", "СвПокуп", "СвПос" });
                        inFatalState = true;
                        {
                            state = 17;
                            depth = 3;
                        }
                        path.Push(name);
                        return result;
                    }
                case 9:
                    {
                        if (name == "СведЮЛ")
                        {
                            state = 10;
                            depth = 6;
                            existances[10] = false;
                            existances[11] = false;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СведИП")
                        {
                            state = 23;
                            depth = 6;
                            existances[12] = false;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "СведЮЛ", "СведИП" });
                        inFatalState = true;
                        {
                            state = 12;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 10:
                    {
                        result = new SchemaAutomatonError11(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 11;
                            depth = 5;
                        }
                        path.Push(name);
                        return result;
                    }
                case 11:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 12;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 12:
                    {
                        if (name == "СвПокуп")
                        {
                            state = 9;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СвПос")
                        {
                            state = 13;
                            depth = 5;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "СвПокуп", "СвПос" });
                        inFatalState = true;
                        {
                            state = 17;
                            depth = 3;
                        }
                        path.Push(name);
                        return result;
                    }
                case 13:
                    {
                        if (name == "СведЮЛ")
                        {
                            state = 14;
                            depth = 6;
                            existances[13] = false;
                            existances[14] = false;
                            path.Push(name);
                            return result;
                        }
                        if (name == "СведИП")
                        {
                            state = 21;
                            depth = 6;
                            existances[15] = false;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "СведЮЛ", "СведИП" });
                        inFatalState = true;
                        {
                            state = 16;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 14:
                    {
                        result = new SchemaAutomatonError11(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 15;
                            depth = 5;
                        }
                        path.Push(name);
                        return result;
                    }
                case 15:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 16;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 16:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 17;
                            depth = 3;
                        }
                        path.Push(name);
                        return result;
                    }
                case 17:
                    {
                        if (name == "КнПродСтр")
                        {
                            state = 4;
                            depth = 4;
                            existances[5] = false;
                            existances[6] = false;
                            existances[7] = false;
                            path.Push(name);
                            return result;
                        }
                        result = new SchemaAutomatonError1(path.Count == 0 ? null : path.Peek(), name, new[] { "КнПродСтр" });
                        inFatalState = true;
                        {
                            state = 18;
                            depth = 2;
                        }
                        path.Push(name);
                        return result;
                    }
                case 18:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 19;
                            depth = 1;
                        }
                        path.Push(name);
                        return result;
                    }
                case 19:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 20;
                            depth = 0;
                        }
                        path.Push(name);
                        return result;
                    }
                case 20:
                    {
                        result = new SchemaAutomatonError11(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = -1;
                        }
                        path.Push(name);
                        return result;
                    }
                case 21:
                    {
                        result = new SchemaAutomatonError11(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 22;
                            depth = 5;
                        }
                        path.Push(name);
                        return result;
                    }
                case 22:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 16;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                case 23:
                    {
                        result = new SchemaAutomatonError11(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 24;
                            depth = 5;
                        }
                        path.Push(name);
                        return result;
                    }
                case 24:
                    {
                        result = new SchemaAutomatonError2(path.Count == 0 ? null : path.Peek(), name);
                        inFatalState = true;
                        {
                            state = 12;
                            depth = 4;
                        }
                        path.Push(name);
                        return result;
                    }
                }
            }
            path.Push(name);
            return result;
        }

        public SchemaAutomatonError EndElement()
        {
            SchemaAutomatonError result = null;
            var top = path.Pop();
            if (!inFatalState)
            {
                switch (state)
                {

                case 0:
                    {
                        result = new SchemaAutomatonError0(top, new[] { "Файл" });
                        {
                            state = -1;
                        }
                        goto zzz;
                    }
                case 1:
                    {
                        result = new SchemaAutomatonError0(top, new[] { "Документ" });
                        {
                            state = 20;
                            depth = 0;
                        }
                        goto zzz;
                    }
                case 2:
                    {
                        if (top == "Документ")
                        {
                            state = 19;
                            depth = 1;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "КнигаПрод", "Документ" });
                        {
                            state = 19;
                            depth = 1;
                        }
                        goto zzz;
                    }
                case 3:
                    {
                        result = new SchemaAutomatonError0(top, new[] { "КнПродСтр" });
                        {
                            state = 18;
                            depth = 2;
                        }
                        goto zzz;
                    }
                case 4:
                    {
                        result = new SchemaAutomatonError0(top, new[] { "КодВидОпер" });
                        {
                            state = 17;
                            depth = 3;
                        }
                        goto zzz;
                    }
                case 5:
                    {
                        if (top == "КодВидОпер")
                        {
                            state = 6;
                            depth = 4;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "КодВидОпер" });
                        {
                            state = 6;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 6:
                    {
                        if (top == "КнПродСтр")
                        {
                            state = 17;
                            depth = 3;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "КодВидОпер", "ДокПдтвОпл", "СвПокуп", "СвПос", "КнПродСтр" });
                        {
                            state = 17;
                            depth = 3;
                        }
                        goto zzz;
                    }
                case 7:
                    {
                        if (top == "ДокПдтвОпл")
                        {
                            state = 8;
                            depth = 4;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "ДокПдтвОпл" });
                        {
                            state = 8;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 8:
                    {
                        if (top == "КнПродСтр")
                        {
                            state = 17;
                            depth = 3;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "ДокПдтвОпл", "СвПокуп", "СвПос", "КнПродСтр" });
                        {
                            state = 17;
                            depth = 3;
                        }
                        goto zzz;
                    }
                case 9:
                    {
                        result = new SchemaAutomatonError0(top, new[] { "СведЮЛ", "СведИП" });
                        {
                            state = 12;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 10:
                    {
                        if (top == "СведЮЛ")
                        {
                            state = 11;
                            depth = 5;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СведЮЛ" });
                        {
                            state = 11;
                            depth = 5;
                        }
                        goto zzz;
                    }
                case 11:
                    {
                        if (top == "СвПокуп")
                        {
                            state = 12;
                            depth = 4;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СвПокуп" });
                        {
                            state = 12;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 12:
                    {
                        if (top == "КнПродСтр")
                        {
                            state = 17;
                            depth = 3;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СвПокуп", "СвПос", "КнПродСтр" });
                        {
                            state = 17;
                            depth = 3;
                        }
                        goto zzz;
                    }
                case 13:
                    {
                        result = new SchemaAutomatonError0(top, new[] { "СведЮЛ", "СведИП" });
                        {
                            state = 16;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 14:
                    {
                        if (top == "СведЮЛ")
                        {
                            state = 15;
                            depth = 5;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СведЮЛ" });
                        {
                            state = 15;
                            depth = 5;
                        }
                        goto zzz;
                    }
                case 15:
                    {
                        if (top == "СвПос")
                        {
                            state = 16;
                            depth = 4;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СвПос" });
                        {
                            state = 16;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 16:
                    {
                        if (top == "КнПродСтр")
                        {
                            state = 17;
                            depth = 3;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "КнПродСтр" });
                        {
                            state = 17;
                            depth = 3;
                        }
                        goto zzz;
                    }
                case 17:
                    {
                        if (top == "КнигаПрод")
                        {
                            state = 18;
                            depth = 2;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "КнПродСтр", "КнигаПрод" });
                        {
                            state = 18;
                            depth = 2;
                        }
                        goto zzz;
                    }
                case 18:
                    {
                        if (top == "Документ")
                        {
                            state = 19;
                            depth = 1;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "Документ" });
                        {
                            state = 19;
                            depth = 1;
                        }
                        goto zzz;
                    }
                case 19:
                    {
                        if (top == "Файл")
                        {
                            state = 20;
                            depth = 0;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "Файл" });
                        {
                            state = 20;
                            depth = 0;
                        }
                        goto zzz;
                    }
                case 20:
                    {
                        inFatalState = true;
                        {
                            state = -1;
                        }
                        goto zzz;
                    }
                case 21:
                    {
                        if (top == "СведИП")
                        {
                            state = 22;
                            depth = 5;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СведИП" });
                        {
                            state = 22;
                            depth = 5;
                        }
                        goto zzz;
                    }
                case 22:
                    {
                        if (top == "СвПос")
                        {
                            state = 16;
                            depth = 4;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СвПос" });
                        {
                            state = 16;
                            depth = 4;
                        }
                        goto zzz;
                    }
                case 23:
                    {
                        if (top == "СведИП")
                        {
                            state = 24;
                            depth = 5;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СведИП" });
                        {
                            state = 24;
                            depth = 5;
                        }
                        goto zzz;
                    }
                case 24:
                    {
                        if (top == "СвПокуп")
                        {
                            state = 12;
                            depth = 4;
                            goto zzz;
                        }
                        result = new SchemaAutomatonError0(top, new[] { "СвПокуп" });
                        {
                            state = 12;
                            depth = 4;
                        }
                        goto zzz;
                    }

                }
            }
            zzz:
            if (inFatalState && state >= 0 && path.Count == depth)
                inFatalState = false;
            return result;
        }

        private const NumberStyles decimalStyle = NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingWhite;

        public SchemaAutomatonError ReadAttribute(string name, string value)
        {
            if (inFatalState) return null;
            if (inFatalState) return null;
            switch (state)
            {
            case 1:
                {
                    if (name == "ИдФайл")
                    {
                        existances[0] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length < 1)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1);
                        if (value.Length > 100)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 100);
                        return null;
                    }
                    if (name == "ВерсПрог")
                    {
                        existances[1] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length < 1)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1);
                        if (value.Length > 40)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 40);
                        return null;
                    }
                    if (name == "ВерсФорм")
                    {
                        existances[2] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length < 1)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1);
                        if (value.Length > 5)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 5);
                        if (value == "5.04")
                            return null;
                        return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "enumeration", "5.04");
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 2:
                {
                    if (name == "Индекс")
                    {
                        existances[3] = true;
                        if (value == "0000090")
                            return null;
                        return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "enumeration", "0000090");
                        return null;
                    }
                    if (name == "НомКорр")
                    {
                        existances[4] = true;
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 3)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 3);
                        }
                        return null;
                    }
                    if (name == "ПризнСвед9")
                    {
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 1)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 1);
                        if (value == "0")
                            return null;
                        if (value == "1")
                            return null;
                        return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "enumeration", "[0; 1]");
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 3:
                {
                    if (name == "СтПродБезНДС18")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтПродБезНДС10")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтПродБезНДС0")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СумНДСВсКПр18")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СумНДСВсКПр10")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтПродОсвВсКПр")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 4:
                {
                    if (name == "НомерПор")
                    {
                        existances[5] = true;
                        decimal parsedDecimal = 0;
                        parsedDecimal = decimal.Parse(value, decimalStyle, CultureInfo.InvariantCulture);
                        if (parsedDecimal.CompareTo(1m) < 0)
                            return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "minInclusive", 1);
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 12)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 12);
                        }
                        return null;
                    }
                    if (name == "НомСчФПрод")
                    {
                        existances[6] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length < 1)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1);
                        if (value.Length > 1000)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1000);
                        return null;
                    }
                    if (name == "ДатаСчФПрод")
                    {
                        existances[7] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    if (name == "НомИспрСчФ")
                    {
                        decimal parsedDecimal = 0;
                        parsedDecimal = decimal.Parse(value, decimalStyle, CultureInfo.InvariantCulture);
                        if (parsedDecimal.CompareTo(1m) < 0)
                            return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "minInclusive", 1);
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 3)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 3);
                        }
                        return null;
                    }
                    if (name == "ДатаИспрСчФ")
                    {
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    if (name == "НомКСчФПрод")
                    {
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length < 1)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1);
                        if (value.Length > 256)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 256);
                        return null;
                    }
                    if (name == "ДатаКСчФПрод")
                    {
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    if (name == "НомИспрКСчФ")
                    {
                        decimal parsedDecimal = 0;
                        parsedDecimal = decimal.Parse(value, decimalStyle, CultureInfo.InvariantCulture);
                        if (parsedDecimal.CompareTo(1m) < 0)
                            return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "minInclusive", 1);
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 3)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 3);
                        }
                        return null;
                    }
                    if (name == "ДатаИспрКСчФ")
                    {
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    if (name == "ОКВ")
                    {
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 3)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 3);
                        return null;
                    }
                    if (name == "СтоимПродСФВ")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтоимПродСФ")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтоимПродСФ18")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтоимПродСФ10")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтоимПродСФ0")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СумНДССФ18")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СумНДССФ10")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    if (name == "СтоимПродОсв")
                    {
                        decimal parsedDecimal = 0;
                        {
                            int actualTotalDigits, actualFractionDigits;
                            DecimalSimpleTypeExecutor.Check(value, out actualTotalDigits, out actualFractionDigits);
                            if (actualTotalDigits > 19)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "totalDigits", 19);
                            if (actualFractionDigits > 2)
                                return new SchemaAutomatonError9("Атрибут", path.Peek(), value, "fractionDigits", 2);
                        }
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 7:
                {
                    if (name == "НомДокПдтвОпл")
                    {
                        existances[8] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length < 1)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 1);
                        if (value.Length > 256)
                            return new SchemaAutomatonError8("Атрибут", path.Peek(), value, 256);
                        return null;
                    }
                    if (name == "ДатаДокПдтвОпл")
                    {
                        existances[9] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 10:
                {
                    if (name == "ИННЮЛ")
                    {
                        existances[10] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    if (name == "КПП")
                    {
                        existances[11] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 9)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 9);
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 14:
                {
                    if (name == "ИННЮЛ")
                    {
                        existances[13] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 10)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 10);
                        return null;
                    }
                    if (name == "КПП")
                    {
                        existances[14] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 9)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 9);
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 21:
                {
                    if (name == "ИННФЛ")
                    {
                        existances[15] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 12)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 12);
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            case 23:
                {
                    if (name == "ИННФЛ")
                    {
                        existances[12] = true;
                        if (string.IsNullOrEmpty(value))
                            return new SchemaAutomatonError13("Атрибут", path.Peek());
                        if (value.Length != 12)
                            return new SchemaAutomatonError6("Атрибут", path.Peek(), value, 12);
                        return null;
                    }
                    return new SchemaAutomatonError3(name);
                }
            default:
                return new SchemaAutomatonError3(name);
            }
        }

        public SchemaAutomatonError ReadText(string text)
        {
            if (inFatalState) return null;
            if (state != 5) return null;
            if (string.IsNullOrEmpty(text))
                return new SchemaAutomatonError13("Элемент", path.Peek());
            if (text.Length != 2)
                return new SchemaAutomatonError6("Элемент", path.Peek(), text, 2);
            if (text[0] == '0')
            {
                if (map0[text[1]])
                    return null;
            }
            else if (text[0] == '1')
            {
                if (map1[text[1]])
                    return null;
            }
            else if (text[0] == '2')
            {
                if (map2[text[1]])
                    return null;
            }
            return new SchemaAutomatonError9("Элемент", path.Peek(), text, "enumeration", "long list of values");
        }

        private int state;
        private int depth;
        private bool inFatalState;
        private readonly Stack<string> path = new Stack<string>();

        private readonly int[] counters;
        private readonly bool[] existances;

        private static readonly bool[] map0;
        private static readonly bool[] map1;
        private static readonly bool[] map2;
    }
}