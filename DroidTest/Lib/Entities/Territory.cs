using System;
using DroidTest.Lib.Entities;

using RestSharp;

namespace DroidTest.Lib.Entities.Territory
{
    [Serializable]
    public class Territory : IEntity
    {

        public Territory()
        {
            // empty
        }

        public int id { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string baseCity { get; set; }
    }
}
