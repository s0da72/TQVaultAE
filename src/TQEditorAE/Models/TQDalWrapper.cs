using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQEditorAE.ViewModels;
using TQVaultData;


namespace TQEditorAE.Models
{
	public class TQDalWrapper : ITQDal
	{

		Dictionary<string, PlayerCollection> _players = new Dictionary<string, PlayerCollection>();

		ISettings _settings;
		public TQDalWrapper(ISettings settings)
		{
			_settings = settings;
			if (Database.DB == null)
			{
				Database.DB = new Database();
				//Database.DB.AutoDetectLanguage = Settings.Default.AutoDetectLanguage;
				Database.DB.AutoDetectLanguage = false;
				Database.DB.TQLanguage = settings.GetPropertyFromSettings("DefaultLanguage");
				Database.DB.LoadDBFile();
			}

			UpdateLanguageData();
		}

		public void UpdateLanguageData()
		{
			Database.DB.TQLanguage = _settings.GetPropertyFromSettings("DefaultLanguage");
			var classnames = _settings.GetPropertyFromResource("CharacterClass");
			PlayerClass.LoadClassDataFile(classnames);

		}

		private bool LoadPlayer(string name)
		{
			if (_players.ContainsKey(name)) return true;
			var playerFile = TQData.GetPlayerFile(name);
			if (!File.Exists(playerFile)) return false;
			var newPlayer = new PlayerCollection(name, playerFile);
			newPlayer.IsImmortalThrone = true;
			try
			{
				newPlayer.LoadFile();
				_players.Add(name, newPlayer);
				return true;
			}
			catch
			{
			}
			return false;
		}

		public PlayerInfo GetCharacter(string key)
		{
			if(!LoadPlayer(key)) return null;
			return (_players[key].PlayerInfo);
		}

		public IList<CharacterInfo> Characters { get {
				var list = new List<CharacterInfo>();

				var names = TQData.GetCharacterList();
				foreach(var name in names)
				{
					if(!LoadPlayer(name))
					{
						continue;
					}
					var player = _players[name];
					if (player == null) continue;
					if (player.PlayerInfo == null) continue;

					list.Add(new CharacterInfo {Name = name, Level = player.PlayerInfo.CurrentLevel, ClassTag=player.PlayerInfo.Class});
				}

				return (list);
			}
		}

		public int AtrributePointsPerLevel { get => PlayerLevel.AtrributePointsPerLevel; }
		public int SkillPointsPerLevel { get => PlayerLevel.SkillPointsPerLevel; }

		public int GetLevelXP(int level)
		{
			return (PlayerLevel.GetLevelMinXP(level));
		}

		private bool SaveAllModifiedPlayers()
		{
			int numModified = 0;

			// Save each player as necessary
			foreach (KeyValuePair<string, PlayerCollection> kvp in _players)
			{
				var playerFile = TQData.GetPlayerFile(kvp.Key);
				if (!File.Exists(playerFile)) continue;
				var player = kvp.Value;

				if (player == null)
				{
					continue;
				}

				if (player.IsModified)
				{
					++numModified;
					var done = false;

					// backup the file
					while (!done)
					{
						try
						{
							TQData.BackupFile(player.PlayerName, playerFile);
							TQData.BackupStupidPlayerBackupFolder(playerFile);
							player.Save(playerFile);
							done = true;
						}
						catch (Exception e)
						{
						}
					}
				}
			}

			return numModified > 0;
		}

	}
}
