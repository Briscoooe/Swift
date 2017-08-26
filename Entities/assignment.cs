using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]   
    public class Assignment
    {
        [DataMember(Name="droneId")]
        public long DroneId { get; set; }

        [DataMember(Name="packageId")]
        public long PackageId { get; set; }
    }
}