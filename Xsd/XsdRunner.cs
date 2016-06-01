using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Xsd
{
    public class XsdRunner
    {
        public XsdRunner(byte[] xmlContent, byte[] xsdContent, bool useSchema)
        {
            this.xmlContent = xmlContent;
            this.xsdContent = xsdContent;
            this.useSchema = useSchema;
        }

        public int Run()
        {
            int res = 0;
            using(var fileStream = new MemoryStream(xmlContent))
            {
                var reader = new StreamReader(fileStream, EntryPoint.encoding);
                XmlReader xmlReader;
                if(useSchema)
                {
                    var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema, IgnoreWhitespace = true};
                    settings.Schemas.Add(null, XmlReader.Create(new StreamReader(new MemoryStream(xsdContent))));
                    settings.ValidationEventHandler += (sender, args) => { ++res; };
                    settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                    xmlReader = XmlReader.Create(reader, settings);
                }
                else
                    xmlReader = XmlReader.Create(reader);

                while(xmlReader.Read())
                {
                    switch(xmlReader.NodeType)
                    {
                    case XmlNodeType.Element:
                        while(xmlReader.MoveToNextAttribute())
                        {
                            var attrValue = xmlReader.Value;
                            var attrName = xmlReader.Name;
                        }
                        xmlReader.MoveToElement();
                        break;
                    case XmlNodeType.Text:
                        var text = xmlReader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        break;
                    }
                }
            }
            return res;
        }

        private readonly byte[] xmlContent;
        private readonly byte[] xsdContent;
        private readonly bool useSchema;
    }
}