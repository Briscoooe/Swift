using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]   
    public class Drone
    {
        [DataMember(Name="droneId", IsRequired = true)]
        public long Id { get; set; }

        [DataMember(Name="location", IsRequired = false)]
        public Coordinate Location { get; set; }

        [DataMember(Name="packages", IsRequired = true)]
        public Package[] Package { get; set; }
    }
}