using System;
using DroidTest.Lib.Entities;

using RestSharp;

namespace DroidTest.Lib.Entities.Drug
{
    [Serializable]
    public class Drug : IEntity
    {

        public Drug()
        {
            // empty
        }

        public int id { get; set; }
        public string fullName { get; set; }
        public string officialName { get; set; }
        public string description { get; set; }
        public string barcode { get; set; }
        public string articul { get; set; }
        public string instruction { get; set; }
        public int reseller { get; set; }
    }
}
