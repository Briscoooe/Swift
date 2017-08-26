using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]
    public class Package
    {
        [DataMember(Name="packageId")]
        public int Id { get; set; }
        
        [DataMember(Name="destination")]
        public Coordinate Destination { get; set; }

        [DataMember(Name="deadline")]
        public double Deadline { get; set; }
    }
}