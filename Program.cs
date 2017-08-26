using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Swift
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                throw new ArgumentException("Not enough arguments");
            }
            else if(args.Length > 1) 
            {
                throw new ArgumentException("Too many arguments");
            }

            
            
        }
    }
}
