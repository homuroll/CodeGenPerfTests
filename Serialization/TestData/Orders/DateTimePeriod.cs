using System.Runtime.Serialization;

using ProtoBuf;

namespace Serialization.TestData.Orders
{
    [DataContract]
    [ProtoContract]
    public class DateTimePeriod
    {
        [DataMember]
        [ProtoMember(1)]
        public DateTimePeriodGroup DateTimePeriodGroup { get; set; }
    }
}