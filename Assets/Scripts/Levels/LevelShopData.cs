using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTK.Enemies;
using GMTK.Info;

namespace GMTK.Levels
{
	[CreateAssetMenu(fileName = "New shop", menuName = "Data/Shop", order = 3)]
	public class LevelShopData : ScriptableObject
	{
		public ClassInfo[] ObligatoryClasses;
		public ClassInfo[] AllClasses;
		public int ClassesPerShop = 5;

		public List<ClassInfo> GetRoll(List<ClassInfo> playerClasses)
		{
			List<ClassInfo> resp = new List<ClassInfo>();

			List<ClassInfo> obligatoryRoll = new List<ClassInfo>(ObligatoryClasses);
			foreach (ClassInfo character in playerClasses)
			{
				obligatoryRoll.Remove(character);
			}

			int currentClasses = 0;
			while (currentClasses < ClassesPerShop && obligatoryRoll.Count > 0)
			{
				currentClasses++;
				int rolledIndex = Random.Range(0, obligatoryRoll.Count);
				resp.Add(obligatoryRoll[rolledIndex]);
				obligatoryRoll.RemoveAt(rolledIndex);
			}

			List<ClassInfo> classesToRoll = new List<ClassInfo>(AllClasses);
			foreach (ClassInfo character in playerClasses)
			{
				classesToRoll.Remove(character);
			}

			while (currentClasses < ClassesPerShop && classesToRoll.Count > 0)
			{
				currentClasses++;
				int rolledIndex = Random.Range(0, classesToRoll.Count);
				resp.Add(classesToRoll[rolledIndex]);
				classesToRoll.RemoveAt(rolledIndex);
			}

			int n = resp.Count;
			while (n > 1)
			{
				n--;
				int k = Random.Range(0, n + 1);
				ClassInfo value = resp[k];
				resp[k] = resp[n];
				resp[n] = value;
			}

			return resp;
		}
	}
}
