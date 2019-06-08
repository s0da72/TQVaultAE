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

		Dictionary<string,PlayerCollection> _players = new Dictionary<string,PlayerCollection>();

		public TQDalWrapper(ISettings settings)
		{
			if (Database.DB == null)
			{
				Database.DB = new Database();
				//Database.DB.AutoDetectLanguage = Settings.Default.AutoDetectLanguage;
				Database.DB.AutoDetectLanguage = false;
				Database.DB.TQLanguage = settings.GetPropertyFromSettings("DefaultLanguage");
				Database.DB.LoadDBFile();
			}
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
				//newPlayer.LoadFile();
				_players.Add(name, newPlayer);
				return true;
			}
			catch
			{
			}
			return false;
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

					list.Add(new CharacterInfo {Name = name,Level=0,ClassName="unknown"});
				}

				return (list);
			}  }
	}
}
