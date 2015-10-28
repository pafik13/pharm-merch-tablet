using System;
using System.Collections.Generic;

namespace DroidTest.Lib.Entities
{
	public class Rout
	{
		public const int WORK_DAYS_COUNT = 5;
		public const int PHARMACIES_COUNT = 10;

		public DateTime	Date;
		public List<RoutItem> Items;

		public Rout ()
		{
			Items = new List<RoutItem> ();

			DayOfWeek dayOfWeek = DayOfWeek.Monday;
			for (int i = 0; i < WORK_DAYS_COUNT; i++) {
				Items.Add (new RoutItem(){DayOfWeek = dayOfWeek + i});
			}
		}
	}

	public class RoutItem
	{
		public DayOfWeek DayOfWeek;
		public int [] Pharmacies;

		public RoutItem()
		{
			Pharmacies = new int[Rout.PHARMACIES_COUNT];
		}
	}
}

