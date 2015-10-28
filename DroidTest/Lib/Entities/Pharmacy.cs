using System;
using System.Collections.Generic;
using PharmMerch.Lib.Entities;

using RestSharp;

namespace PharmMerch.Lib.Entities.Pharmacy
{
    [Serializable]
	public class Pharmacy: IEntity
	{
		public Pharmacy ()
		{
			// empty
		}

		public int id { get; set; }
		public string fullName { get; set; }
		public string shortName { get; set; }
		public string officialName { get; set; }
		public string address { get; set; }
		public string subway { get; set; }
		public string phone { get; set; }
		public string email { get; set; }
        public int territory { get; set; }
        public int tradenet { get; set; }
	}

    [Serializable]
    public class PharmacyRestData
    {
        public Method method { get; set; }
        public string resource { get; set; }
        public Pharmacy pharmacy { get; set; }
        public DataFormat dataFormat { get; set; }
    }

    [Serializable]
    public class PharmacyList
    {
        public List<Pharmacy> pharmacies { get; set; }
    }
}

