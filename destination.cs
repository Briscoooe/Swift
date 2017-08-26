using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]
    public class Coordinate
    {
        [DataMember(Name="latitude")]
        public string Latitude { get; set; }

        [DataMember(Name="longitude")]
        public string Longitude { get; set; }
    }
}