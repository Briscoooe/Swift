using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]
    public class Coordinate
    {
        [DataMember(Name="latitude")]
        public double Latitude { get; set; }

        [DataMember(Name="longitude")]
        public double Longitude { get; set; }
    }
}