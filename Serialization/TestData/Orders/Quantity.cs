using System.Runtime.Serialization;

using ProtoBuf;

namespace Serialization.TestData.Orders
{
    [DataContract]
    [ProtoContract]
    public class Quantity
    {
        [DataMember]
        [ProtoMember(1)]
        public QuantityDetails QuantityDetails { get; set; }
    }
}