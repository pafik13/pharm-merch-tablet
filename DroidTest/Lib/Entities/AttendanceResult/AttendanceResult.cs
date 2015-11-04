using System;
using System.Collections.Generic;

using RestSharp;

namespace DroidTest.Lib.Entities
{
	[Serializable]
	public class AttendanceResult: IEntity
	{
		public AttendanceResult()
		{
		}

		public int id	{ get; set; }
		public int attendance { get; set; }
		public int info { get; set; }
		public int drug { get; set; }
		public string value { get; set; }


  		public static string InvertStringBool(string b)
  		{
  			if (b.Equals (@"Y")) {
  				return @"N";
  			} else {
  				return @"Y";
  			}
  		}

  		public static string StringBoolToRussian(string b)
  		{
  			if (b.Equals (@"Y")) {
  				return @"Да";
  			} else {
  				return @"Нет";
  			}
  		}
	}
 }
