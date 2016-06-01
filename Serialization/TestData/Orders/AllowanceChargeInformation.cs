using System.Runtime.Serialization;

using ProtoBuf;

namespace Serialization.TestData.Orders
{
    [DataContract]
    [ProtoContract]
    public class AllowanceChargeInformation
    {
        [DataMember]
        [ProtoMember(1)]
        public uint AllowanceOrChargeIdentifier { get; set; }

        [DataMember]
        [ProtoMember(2)]
        public string AllowanceOrChargeIdentificationCode { get; set; }
    }
}