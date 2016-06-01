using JetBrains.Annotations;

namespace Xsd
{
    public enum ErrorLevel
    {
        None,
        Error,
        Warning
    }

    public abstract class SchemaAutomatonError
    {
        protected SchemaAutomatonError(ErrorLevel errorLevel)
        {
            ErrorLevel = errorLevel;
        }

        public ErrorLevel ErrorLevel { get; private set; }
    }

    public class SchemaAutomatonError0 : SchemaAutomatonError
    {
        public SchemaAutomatonError0([NotNull] string element, [NotNull] string[] expectedElements)
            : base(ErrorLevel.Error)
        {
            Element = element;
            ExpectedElements = expectedElements;
        }

        [NotNull]
        public string Element { get; private set; }

        [NotNull]
        public string[] ExpectedElements { get; private set; }

        public override string ToString()
        {
            return string.Format("Содержимое элемента '{0}' является неполным. Список ожидаемых элементов: '{1}'.",
                                 Element, string.Join(", ", ExpectedElements));
        }
    }

    public class SchemaAutomatonError1 : SchemaAutomatonError
    {
        public SchemaAutomatonError1([CanBeNull] string element, [NotNull] string invalidElement, [NotNull] string[] expectedElements)
            : base(ErrorLevel.Error)
        {
            Element = element;
            InvalidElement = invalidElement;
            ExpectedElements = expectedElements;
        }

        [CanBeNull]
        public string Element { get; private set; }

        [NotNull]
        public string InvalidElement { get; private set; }

        [NotNull]
        public string[] ExpectedElements { get; private set; }

        public override string ToString()
        {
            return string.Format("В элементе '{0}' содержится лишний дочерний элемент '{1}', отсутствует/не заполнен элемент '{2}' или нарушен их порядок. Список ожидаемых дочерних элементов: '{2}'.",
                                 Element, InvalidElement, string.Join(", ", ExpectedElements));
        }
    }

    public class SchemaAutomatonError2 : SchemaAutomatonError
    {
        public SchemaAutomatonError2([CanBeNull] string element, [NotNull] string invalidElement)
            : base(ErrorLevel.Error)
        {
            Element = element;
            InvalidElement = invalidElement;
        }

        [CanBeNull]
        public string Element { get; private set; }

        [NotNull]
        public string InvalidElement { get; private set; }

        public override string ToString()
        {
            return string.Format("В элементе '{0}' содержится лишний дочерний элемент '{1}'.", Element, InvalidElement);
        }
    }

    public class SchemaAutomatonError3 : SchemaAutomatonError
    {
        public SchemaAutomatonError3([NotNull] string attributeName)
            : base(ErrorLevel.Error)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; private set; }

        public override string ToString()
        {
            return string.Format("Атрибут '{0}' является лишним.", AttributeName);
        }
    }

    public class SchemaAutomatonError4 : SchemaAutomatonError
    {
        public SchemaAutomatonError4([NotNull] string attributeName)
            : base(ErrorLevel.Error)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; private set; }

        public override string ToString()
        {
            return string.Format("Отсутствует обязательный атрибут '{0}'.", AttributeName);
        }
    }

    public class SchemaAutomatonError13 : SchemaAutomatonError
    {
        public SchemaAutomatonError13([NotNull] string nodeType, [NotNull] string name)
            : base(ErrorLevel.Error)
        {
            NodeType = nodeType;
            Name = name;
        }

        [NotNull]
        public string NodeType { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} '{1}' некорректный - Не заполнено значение.", NodeType, Name);
        }
    }

    public class SchemaAutomatonError11 : SchemaAutomatonError
    {
        public SchemaAutomatonError11([NotNull] string element, [NotNull] string invalidElement)
            : base(ErrorLevel.Error)
        {
            Element = element;
            InvalidElement = invalidElement;
        }

        [NotNull]
        public string Element { get; private set; }

        [NotNull]
        public string InvalidElement { get; private set; }

        public override string ToString()
        {
            return string.Format("Элемент '{0}' не может содержать элемент '{1}', поскольку элемент не может иметь содержимого.", Element, InvalidElement);
        }
    }

    public class SchemaAutomatonError12 : SchemaAutomatonError
    {
        public SchemaAutomatonError12([NotNull] string element, [NotNull] string invalidElement)
            : base(ErrorLevel.Error)
        {
            Element = element;
            InvalidElement = invalidElement;
        }

        [NotNull]
        public string Element { get; private set; }

        [NotNull]
        public string InvalidElement { get; private set; }

        public override string ToString()
        {
            return string.Format("Элемент '{0}' не может содержать элемент '{1}', поскольку элемент может содержать только текст.", Element, InvalidElement);
        }
    }

    public class SchemaAutomatonError10 : SchemaAutomatonError
    {
        public SchemaAutomatonError10([NotNull] string element)
            : base(ErrorLevel.Error)
        {
            Element = element;
        }

        [NotNull]
        public string Element { get; private set; }

        public override string ToString()
        {
            return string.Format("Элемент '{0}' не может содержать текста.", Element);
        }
    }

    public class SchemaAutomatonError5 : SchemaAutomatonError
    {
        public SchemaAutomatonError5([NotNull] string nodeType, [NotNull] string name, [NotNull] string value, [NotNull] string valueType)
            : base(ErrorLevel.Error)
        {
            NodeType = nodeType;
            Name = name;
            Value = value;
            ValueType = valueType;
        }

        [NotNull]
        public string NodeType { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [NotNull]
        public string Value { get; private set; }

        [NotNull]
        public string ValueType { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} '{1}' некорректный - Значение '{2}' не является {3}.", NodeType, Name, Value, ValueType);
        }
    }

    public class SchemaAutomatonError6 : SchemaAutomatonError
    {
        public SchemaAutomatonError6([NotNull] string nodeType, [NotNull] string name, [NotNull] string value, int length)
            : base(ErrorLevel.Error)
        {
            NodeType = nodeType;
            Name = name;
            Value = value;
            Length = length;
        }

        [NotNull]
        public string NodeType { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [NotNull]
        public string Value { get; private set; }

        public int Length { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} '{1}' некорректный - Длина значения '{2}' должна быть равна {3}.", NodeType, Name, Value, Length);
        }
    }

    public class SchemaAutomatonError7 : SchemaAutomatonError
    {
        public SchemaAutomatonError7([NotNull] string nodeType, [NotNull] string name, [NotNull] string value, int maxLength)
            : base(ErrorLevel.Error)
        {
            NodeType = nodeType;
            Name = name;
            Value = value;
            MaxLength = maxLength;
        }

        [NotNull]
        public string NodeType { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [NotNull]
        public string Value { get; private set; }

        public int MaxLength { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} '{1}' некорректный - Длина значения '{2}' должна быть не больше чем {3}.", NodeType, Name, Value, MaxLength);
        }
    }

    public class SchemaAutomatonError8 : SchemaAutomatonError
    {
        public SchemaAutomatonError8([NotNull] string nodeType, [NotNull] string name, [NotNull] string value, int minLength)
            : base(ErrorLevel.Error)
        {
            NodeType = nodeType;
            Name = name;
            Value = value;
            MinLength = minLength;
        }

        [NotNull]
        public string NodeType { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [NotNull]
        public string Value { get; private set; }

        public int MinLength { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} '{1}' некорректный - Длина значения '{2}' должна быть не меньше чем {3}.", NodeType, Name, Value, MinLength);
        }
    }

    public class SchemaAutomatonError14 : SchemaAutomatonError
    {
        public SchemaAutomatonError14([NotNull] string value, [NotNull] string tableName, ErrorLevel errorLevel)
            : base(errorLevel)
        {
            Value = value;
            TableName = tableName;
        }

        [NotNull]
        public string Value { get; private set; }

        [NotNull]
        public string TableName { get; private set; }

        public override string ToString()
        {
            return string.Format("Значение '{0}' не найдено в справочнике '{1}'.", Value, TableName);
        }
    }

    public class SchemaAutomatonError15 : SchemaAutomatonError
    {
        public SchemaAutomatonError15([NotNull] string value, ErrorLevel errorLevel)
            : base(errorLevel)
        {
            Value = value;
        }

        [NotNull]
        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("Код из справочника '{0}' на данный период отчетности недействителен.", Value);
        }
    }

    public class SchemaAutomatonError9 : SchemaAutomatonError
    {
        public SchemaAutomatonError9([NotNull] string nodeType, [NotNull] string name, [CanBeNull] string value, [NotNull] string restriction, [NotNull] object restrictionValue)
            : base(ErrorLevel.Error)
        {
            NodeType = nodeType;
            Name = name;
            Value = value;
            Restriction = restriction;
            RestrictionValue = restrictionValue;
        }

        [NotNull]
        public string NodeType { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [CanBeNull]
        public string Value { get; private set; }

        [NotNull]
        public string Restriction { get; private set; }

        [NotNull]
        public object RestrictionValue { get; private set; }

        private string GetError()
        {
            switch(Restriction)
            {
            case "enumeration":
                return string.Format("Значение должно быть из списка допустимых значений: {0}", string.Join(", ", (string[])RestrictionValue));
            case "pattern":
                return string.Format("Нарушен формат: {0}", RestrictionValue);
            case "totalDigits":
                return string.Format("Общее количество цифр числа должно быть не больше чем {0}", RestrictionValue);
            case "fractionDigits":
                return string.Format("Количество цифр дробной части числа должно быть не больше чем {0}", RestrictionValue);
            case "maxInclusive":
                return string.Format("Значение должно быть не больше чем {0}", RestrictionValue);
            case "maxExclusive":
                return string.Format("Значение должно быть строго меньше чем {0}", RestrictionValue);
            case "minInclusive":
                return string.Format("Значение должно быть не меньше чем {0}", RestrictionValue);
            case "minExclusive":
                return string.Format("Значение должно быть строго больше чем {0}", RestrictionValue);
            default:
                return Restriction;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}' некорректный - Значение '{2}' не проходит по следующему ограничению: '{3}'.", NodeType, Name, Value, GetError());
        }
    }
}