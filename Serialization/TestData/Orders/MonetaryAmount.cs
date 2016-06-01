using System.Runtime.Serialization;

using ProtoBuf;

namespace Serialization.TestData.Orders
{
    [DataContract]
    [ProtoContract]
    public class MonetaryAmount
    {
        [DataMember]
        [ProtoMember(1)]
        public MonetaryAmountGroup MonetaryAmountGroup { get; set; }
    }
}