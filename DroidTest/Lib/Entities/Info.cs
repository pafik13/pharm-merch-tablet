using System;
using System.Collections.Generic;

using RestSharp;

namespace DroidTest.Lib.Entities
{
	[Serializable]
	public class Info : IEntity
	{
		public Info ()
		{
		}

		public int id { get; set; }
		public string name { get; set; }
	}

	[Serializable]
	public class InfoItem : IEntity
	{
		public InfoItem ()
		{
		}

		public int id { get; set; }
		public int infoID { get; set; }
		public List<DrugInfoItem> drugInfos { get; set; }
	}

	[Serializable]
	public class DrugInfoItem : IEntity
	{
		public DrugInfoItem ()
		{
		}

		public int id { get; set; }
		public int drugID { get; set; }
		public DateTime date { get; set; }
		public string value { get; set; }
	}

}

