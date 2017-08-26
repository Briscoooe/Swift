using System;
using System.Runtime.Serialization;

namespace Swift
{
    [DataContract]   
    public class Output
    {
        [DataMember(Name="assignments")]
        public Assignment[] Assignments { get;set; }

        [DataMember(Name="unassignedPackageIds")]
        public long[] UnassignedPackagesIds { get; set; }
    }
}