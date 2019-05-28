﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TQVaultData
{

	/// <summary>
	/// Holds class tag information
	/// </summary>
	public class PlayerClass
	{

		static Dictionary<string, string> _classKey = new Dictionary<string, string> {
				{ "tagCClass01" , "Theurgist" },
				{ "tagCClass02" , "Wanderer" },
				{ "tagCClass03" , "Rogue" },
				{ "tagCClass04" , "Hunter" },
				{ "tagCClass05" , "Stormcaller" },
				{ "tagCClass06" , "Pyromancer" },
				{ "tagCClass07" , "Defender" },
				{ "tagCClass08" , "Warrior" },
				{ "tagCClass09" , "Spellbreaker" },
				{ "tagCClass10" , "Champion" },
				{ "tagCClass11" , "Assassin" },
				{ "tagCClass12" , "Slayer" },
				{ "tagCClass13" , "Thane" },
				{ "tagCClass14" , "Battlemage" },
				{ "tagCClass15" , "Conqueror" },
				{ "tagCClass16" , "Spellbinder" },
				{ "tagCClass17" , "Guardian" },
				{ "tagCClass18" , "Corsair" },
				{ "tagCClass19" , "Warden" },
				{ "tagCClass20" , "Paladin" },
				{ "tagCClass21" , "Juggernaut" },
				{ "tagCClass22" , "Conjurer" },
				{ "tagCClass23" , "Summoner" },
				{ "tagCClass24" , "Magician" },
				{ "tagCClass25" , "Avenger" },
				{ "tagCClass26" , "Elementalist" },
				{ "tagCClass27" , "Oracle" },
				{ "tagCClass28" , "Druid" },
				{ "tagCClass29" , "Sorcerer" },
				{ "tagCClass30" , "Sage" },
				{ "tagCClass31" , "Bone Charmer" },
				{ "tagCClass32" , "Ranger" },
				{ "tagCClass33" , "Brigand" },
				{ "tagCClass34" , "Warlock" },
				{ "tagCClass35" , "Illusionist" },
				{ "tagCClass36" , "Soothsayer" },
				{ "xtagCharacterClass01" , "Seer" },
				{ "xtagCharacterClass02" , "Harbinger" },
				{ "xtagCharacterClass03" , "Templar" },
				{ "xtagCharacterClass04" , "Evoker" },
				{ "xtagCharacterClass05" , "Prophet" },
				{ "xtagCharacterClass06" , "Haruspex" },
				{ "xtagCharacterClass07" , "Dreamkiller" },
				{ "xtagCharacterClass08" , "Ritualist" },
				{ "xtagCharacterClass09" , "Diviner" },
				{ "x2tag_class_rm_rm" , "Runemaster" },
				{ "x2tag_class_warfare_rm" , "Berserker" },
				{ "x2tag_class_defense_rm" , "Runesmith" },
				{ "x2tag_class_earth_rm" , "Stonespeaker" },
				{ "x2tag_class_storm_rm" , "Thunderer" },
				{ "x2tag_class_hunting_rm" , "Dragon Hunter" },
				{ "x2tag_class_stealth_rm" , "Trickster" },
				{ "x2tag_class_nature_rm" , "Skinchanger" },
				{ "x2tag_class_spirit_rm" , "Shaman" },
				{ "x2tag_class_dream_rm" , "Seidr Worker" },
		};


		public static string GetClassDisplayName(string classTagkey)
		{
			if (_classKey.ContainsKey(classTagkey))
			{
				return (_classKey[classTagkey]);
			}
			return ("Unknown");
		}

	}
}
