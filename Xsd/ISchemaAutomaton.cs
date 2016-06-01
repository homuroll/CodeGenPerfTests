using JetBrains.Annotations;

namespace Xsd
{
    public interface ISchemaAutomaton
    {
        void Reset();
        SchemaAutomatonError StartElement([NotNull] string name);
        SchemaAutomatonError EndElement();
        SchemaAutomatonError ReadAttribute([NotNull] string name, [CanBeNull] string value);
        SchemaAutomatonError ReadText([NotNull] string text);
    }
}