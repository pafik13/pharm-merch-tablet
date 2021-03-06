﻿using System;
using DroidTest.Lib.Entities;

namespace DroidTest.Lib.Entities.Pharmacy
{
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

	}
}

