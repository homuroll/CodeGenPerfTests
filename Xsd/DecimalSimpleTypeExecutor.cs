using JetBrains.Annotations;

namespace Xsd
{
    public static class DecimalSimpleTypeExecutor
    {
        public static unsafe bool Check([NotNull] string value, out int totalDigits, out int fractionDigits)
        {
            totalDigits = 0;
            fractionDigits = 0;
            fixed(char* c = value)
            {
                var end = c + value.Length;
                var p = c;
                while(p < end && char.IsWhiteSpace(*p))
                    ++p;
                if(p >= end)
                    return true;
                if(*p == '-' || *p == '+')
                    ++p;
                if(p >= end || (!char.IsDigit(*p) && *p != '.'))
                    return false;
                var hasDigit = char.IsDigit(*p);
                while(p < end && *p == '0')
                    ++p;
                while(p < end && char.IsDigit(*p))
                {
                    ++p;
                    ++totalDigits;
                }
                if(p >= end)
                    return hasDigit;
                if(*p == '.')
                {
                    ++p;
                    if(p >= end)
                        return hasDigit;
                    end = c + value.Length - 1;
                    while(char.IsWhiteSpace(*end))
                        --end;
                    while(*end == '0')
                        --end;
                    ++end;
                    if(!char.IsDigit(*p))
                        return p >= end && hasDigit;
                    while(p < end && char.IsDigit(*p))
                    {
                        ++p;
                        ++totalDigits;
                        ++fractionDigits;
                    }
                    return p >= end;
                }
                while(p < end && char.IsWhiteSpace(*p))
                    ++p;
                return p >= end;
            }
        }
    }
}