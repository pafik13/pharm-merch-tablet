using System;
using System.Collections.Generic;

namespace DroidTest.Lib.Entities
{
	public class DrugInfo: IEntity
	{
		public DrugInfo ()
		{
			attendaces = new List<Attendance> ();
		}

		public DrugInfo (int pharmacyID)
		{
			pharmacy = pharmacyID;
			attendaces = new List<Attendance> ();
		}

		public int id { get; set; }
		public int pharmacy { get; set; }
		public List<Attendance> attendaces { get; set; }

	}
}

