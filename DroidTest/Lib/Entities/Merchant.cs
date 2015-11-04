using System;
using DroidTest.Lib.Entities;

using RestSharp;

namespace DroidTest.Lib.Entities
{
    [Serializable]
    public class Merchant : IEntity
    {

        public Merchant()
        {
            // empty
        }

        public int id { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public int sex { get; set; }
        public string phone { get; set; }
        public string job_role { get; set; }
        public int manager { get; set; }
        public int user { get; set; }
        public int project { get; set; }
        public int territory { get; set; }
    }

    [Serializable]
    public class ManagerRestData
    {
        public Method method { get; set; }
        public string resource { get; set; }
        public Merchant merchant { get; set; }
        public DataFormat dataFormat { get; set; }
    }

}
