using System;
using DroidTest.Lib.Entities;

using RestSharp;

namespace DroidTest.Lib.Entities
{
    [Serializable]
    public class User : IEntity
    {

        public User()
        {
            // empty
        }

        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

}