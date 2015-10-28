using System;
using DroidTest.Lib.Entities;

using RestSharp;

namespace DroidTest.Lib.Entities.Manager
{
    [Serializable]
    public class Manager : IEntity
    {

        public Manager()
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
        public int head { get; set; }
        public int user { get; set; }

    }

        [Serializable]
        public class ManagerRestData
        {
            public Method method { get; set; }
            public string resource { get; set; }
            public Manager manager { get; set; }
            public DataFormat dataFormat { get; set; }
        }

}
