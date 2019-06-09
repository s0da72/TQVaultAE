using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQEditorAE.ViewModels;

using TQVaultData;

namespace TQEditorAE.Models
{
	public interface ITQDal
	{
		IList<CharacterInfo> Characters { get; }

		IList<CharacterInfo> ModifiedCharacters { get; }

		void UpdateLanguageData();

		PlayerInfo GetCharacter(string key);

		int AtrributePointsPerLevel { get; }

		int SkillPointsPerLevel { get; }

		int GetLevelXP(int level);

		bool CommitPlayerInfo(string name, PlayerInfo playerInfo);

		bool SaveChangesToDisk();
	}
}
