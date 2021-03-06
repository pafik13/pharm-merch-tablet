﻿//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using Android.OS;

using DroidTest.Lib.Entities;
using DroidTest.Lib.Entities.Manager;
using DroidTest.Lib.Entities.Project;
//using DroidTest.Lib.Entities.Drug;
using DroidTest.Lib.Entities.Territory;
using DroidTest.Lib.Entities.Pharmacy;

namespace DroidTest.Lib
{
    public class Common
    {

        static Common()
		{
		}

		public static bool CreateDirForPhotos(User user)
		{
			string storeLocation = Path.Combine(GetDirForPhotos(user),  @"photos.xml");
			new FileInfo(storeLocation).Directory.Create();
			return true;
		}

		public static string GetDirForPhotos(User user)
		{
			return Path.Combine(DatabaseFileDir, user.username, @"Photos");
		}

		/******  DRUG INFOS *****/
		public static List<DrugInfo> GetDrugInfos()
		{
			string storeLocation = Path.Combine(DatabaseFileDir,  @"drugInfos.xml");
			if (!File.Exists(storeLocation)) {
				return null;
			}

			var serializer = new XmlSerializer(typeof(List<DrugInfo> ));

			using (var stream = new FileStream(storeLocation, FileMode.Open))
			{
				return (List<DrugInfo> )serializer.Deserialize(stream);
			}
		}

		public static bool SetDrugInfos(List<DrugInfo>  drugInfos)
		{
			string storeLocation = Path.Combine(DatabaseFileDir, @"drugInfos.xml");
			new FileInfo(storeLocation).Directory.Create();
			var serializer = new XmlSerializer(typeof(List<DrugInfo>));
			using (var writer = new StreamWriter(storeLocation))
			{
				serializer.Serialize(writer, drugInfos);
			}

			return true;
		}

		/******  CURRENT USER *****/
		public static User GetCurrentUser()
		{
			string storeLocation = Path.Combine(DatabaseFileDir,  @"currentUser.xml");
			if (!File.Exists(storeLocation)) {
				return null;
			}

			var serializer = new XmlSerializer(typeof(User));

			using (var stream = new FileStream(storeLocation, FileMode.Open))
			{
				return (User)serializer.Deserialize(stream);
			}
		}

		public static bool SetCurrentUser(User user)
		{
			string storeLocation = Path.Combine(DatabaseFileDir, @"currentUser.xml");
			new FileInfo(storeLocation).Directory.Create();
			var serializer = new XmlSerializer(typeof(User));
			using (var writer = new StreamWriter(storeLocation))
			{
				serializer.Serialize(writer, user);
			}

			return true;
		}

        public static User GetUser(string username)
        {
			if (string.IsNullOrEmpty (username)) {
				return null;
			}

            string storeLocation = Path.Combine(DatabaseFileDir, username, @"user.xml");
            var serializer = new XmlSerializer(typeof(User));

            using (var stream = new FileStream(storeLocation, FileMode.Open))
            {
                return (User)serializer.Deserialize(stream);
            }
        }

        public static bool SetUser(string username, User user)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"user.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(User));
            using (var writer = new StreamWriter(storeLocation))
            {
                serializer.Serialize(writer, user);
            }

            return true;
        }

		public static Merchant GetMerchant (string username)
		{
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"merchant.xml");
		    var serializer = new XmlSerializer (typeof(Merchant));

			using (var stream = new FileStream (storeLocation, FileMode.Open)) {
                return (Merchant)serializer.Deserialize(stream);
			}
		}

        public static bool SetMerchant(string username, Merchant merchant)
		{
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"merchant.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(Merchant));
			using (var writer = new StreamWriter (storeLocation)) {
                serializer.Serialize(writer, merchant);
			}

            return true;
		}

        public static Manager GetManager(string username)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"manager.xml");
            var serializer = new XmlSerializer(typeof(Manager));

            using (var stream = new FileStream(storeLocation, FileMode.Open))
            {
                return (Manager)serializer.Deserialize(stream);
            }
        }

        public static bool SetManager(string username, Manager manager)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"manager.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(Manager));
            using (var writer = new StreamWriter(storeLocation))
            {
                serializer.Serialize(writer, manager);
            }

            return true;
        }

        public static Project GetProject(string username)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"project.xml");
            var serializer = new XmlSerializer(typeof(Project));

            using (var stream = new FileStream(storeLocation, FileMode.Open))
            {
                return (Project)serializer.Deserialize(stream);
            }
        }

        public static bool SetProject(string username, Project project)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"project.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(Project));
            using (var writer = new StreamWriter(storeLocation))
            {
                serializer.Serialize(writer, project);
            }

            return true;
        }

        public static List<Drug> GetDrugs(string username)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"drugs.xml");
            var serializer = new XmlSerializer(typeof(List<Drug>));

            using (var stream = new FileStream(storeLocation, FileMode.Open))
            {
                return (List<Drug>)serializer.Deserialize(stream);
            }
        }

        public static bool SetDrugs(string username, List<Drug> drugs)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"drugs.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(List<Drug>));
            using (var writer = new StreamWriter(storeLocation))
            {
                serializer.Serialize(writer, drugs);
            }

            return true;
        }

		public static List<Info> GetInfos(string username)
		{
			string storeLocation = Path.Combine(DatabaseFileDir, username, @"infos.xml");
			var serializer = new XmlSerializer(typeof(List<Info>));

			if (File.Exists (storeLocation)) {
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					return (List<Info>)serializer.Deserialize (stream);
				}
			}

			return new List<Info> ();
		}

		public static bool SetInfos(string username, List<Info> infos)
		{
			string storeLocation = Path.Combine(DatabaseFileDir, username, @"infos.xml");
			new FileInfo(storeLocation).Directory.Create();
			var serializer = new XmlSerializer(typeof(List<Info>));
			using (var writer = new StreamWriter(storeLocation))
			{
				serializer.Serialize(writer, infos);
			}

			return true;
		}

        public static Territory GetTerritory(string username)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"territory.xml");
            var serializer = new XmlSerializer(typeof(Territory));

            using (var stream = new FileStream(storeLocation, FileMode.Open))
            {
                return (Territory)serializer.Deserialize(stream);
            }
        }

        public static bool SetTerritory(string username, Territory territory)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"territory.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(Territory));
            using (var writer = new StreamWriter(storeLocation))
            {
                serializer.Serialize(writer, territory);
            }

            return true;
        }

        public static List<Pharmacy> GetPharmacies(string username)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"pharmacies.xml");
            var serializer = new XmlSerializer(typeof(List<Pharmacy>));

            using (var stream = new FileStream(storeLocation, FileMode.Open))
            {
                return (List<Pharmacy>)serializer.Deserialize(stream);
            }
        }

        public static bool SetPharmacies(string username, List<Pharmacy> pharmacies)
        {
            string storeLocation = Path.Combine(DatabaseFileDir, username, @"pharmacies.xml");
            new FileInfo(storeLocation).Directory.Create();
            var serializer = new XmlSerializer(typeof(List<Pharmacy>));
            using (var writer = new StreamWriter(storeLocation))
            {
                serializer.Serialize(writer, pharmacies);
            }

            return true;
        }

		public static string DatabaseFileDir {
            get {
				return Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, @"MyTempDir", @"PharmMerch");
            }
		}

    }
}
