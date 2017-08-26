using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]   
    public class Drone
    {
        [DataMember(Name="droneId")]
        public long Id { get; set; }

        [DataMember(Name="location")]
        public Coordinate Location { get; set; }

        [DataMember(Name="packages")]
        public Package[] Package { get; set; }
    }
}