using System;
using DroidTest.Lib.Entities;

using RestSharp;

namespace DroidTest.Lib.Entities.Tradenet
{
    [Serializable]
    public class Tradenet : IEntity
    {

        public Tradenet()
        {
            // empty
        }

        public int id { get; set; }
        public string fullName { get; set; }
        public string shortName { get; set; }
        public string description { get; set; }
    }
}
