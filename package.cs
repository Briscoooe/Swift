using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]
    public class Package
    {
        [DataMember(Name="packageId", IsRequired = true)]
        public int Id { get; set; }
        
        [DataMember(Name="destination", IsRequired = false)]
        public Coordinate Destination { get; set; }

        [DataMember(Name="deadline", IsRequired = false)]
        public double Deadline { get; set; }
    }
}