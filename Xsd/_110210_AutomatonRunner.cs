using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Xsd
{
    public class _110210_AutomatonRunner
    {
        public _110210_AutomatonRunner(byte[] xmlContent)
        {
            this.xmlContent = xmlContent;
            automaton = new _110201_Automaton(new int[0], new bool[16]);
        }

        public int Run()
        {
            int res = 0;
            automaton.Reset();
            using(var fileStream = new MemoryStream(xmlContent))
            {
                var reader = new StreamReader(fileStream, EntryPoint.encoding);
                var xmlReader = XmlReader.Create(reader);

                var stack = new Stack<string>();
                SchemaAutomatonError error;
                while(xmlReader.Read())
                {
                    switch(xmlReader.NodeType)
                    {
                    case XmlNodeType.Element:
                        stack.Push(xmlReader.Name);
                        error = automaton.StartElement(xmlReader.Name);
                        if(error != null)
                            ++res;
                        while(xmlReader.MoveToNextAttribute())
                        {
                            var attrValue = xmlReader.Value;
                            var attrName = xmlReader.Name;
                            error = automaton.ReadAttribute(attrName, attrValue);
                            if(error != null)
                                ++res;
                        }
                        xmlReader.MoveToElement();
                        if(xmlReader.IsEmptyElement)
                        {
                            error = automaton.EndElement();
                            if(error != null)
                                ++res;
                        }
                        break;
                    case XmlNodeType.Text:
                        var text = xmlReader.Value;
                        error = automaton.ReadText(text);
                        if(error != null)
                            ++res;
                        break;
                    case XmlNodeType.EndElement:
                        error = automaton.EndElement();
                        if(error != null)
                            ++res;
                        break;
                    }
                }
            }
            return res;
        }

        private readonly byte[] xmlContent;

        private readonly ISchemaAutomaton automaton;
    }
}