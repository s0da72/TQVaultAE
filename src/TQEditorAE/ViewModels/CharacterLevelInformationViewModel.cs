using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Mvvm;
using TQEditorAE.Events;
using TQEditorAE.Models;
using TQVaultData;

namespace TQEditorAE.ViewModels
{
	public class CharacterLevelInformationViewModel : BindableBase
	{
		IEventAggregator _eventAggregator;
		ISettings _settings;
		ITQDal _dal;

		public CharacterLevelInformationViewModel(ISettings settings, IEventAggregator eventAggregator, ITQDal dal)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
			_dal = dal;

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
			_eventAggregator.GetEvent<CharacterSelectedEvent>().Subscribe(CharacterSelected);
			_eventAggregator.GetEvent<PreCommitEvent>().Subscribe(PreCommit);

			_diffcultiesUnlocked = new List<DifficultyInfo>();
			setupDifficulty();
		}

		PlayerInfo _selectedPlayerInfo;
		CharacterInfo _selectedCharacterInfo;

		public string AttributeGroupBoxName { get => _settings.GetPropertyFromResource("AttributeGroupBoxName"); }

		public string LevelGroupBoxName { get => _settings.GetPropertyFromResource("LevelGroupBoxName"); }


		protected void PreCommit(string name)
		{
			doPlayerInfoUpdate();
			RaisePropertyChanged(string.Empty);
		}

		protected void CharacterSelected(CharacterInfo player)
		{
			if (player == null) return;

			if (_selectedPlayerInfo != null && _selectedPlayerInfo != player.PlayerInfo)
			{
				doPlayerInfoUpdate();
			}

			_selectedPlayerInfo = player.PlayerInfo;
			_selectedCharacterInfo = player;

			_strength = player.PlayerInfo.BaseStrength;
			_dexterity = player.PlayerInfo.BaseDexterity;
			_intelligence = player.PlayerInfo.BaseIntelligence;
			_health = player.PlayerInfo.BaseHealth;
			_mana = player.PlayerInfo.BaseMana;
			_attributePoints = player.PlayerInfo.AttributesPoints;
			_skillPoints = player.PlayerInfo.SkillPoints;
			_xp = player.PlayerInfo.CurrentXP;
			_level = player.PlayerInfo.CurrentLevel;

			if (player.PlayerInfo.DifficultyUnlocked < _diffcultiesUnlocked.Count)
			{
				DifficultySelected = _diffcultiesUnlocked[player.PlayerInfo.DifficultyUnlocked];
			}

			EnableDifficulty = player.PlayerInfo.HasBeenInGame > 0;

			RaisePropertyChanged(string.Empty);
		}

		protected void LanguageSelected(string name)
		{
			RaisePropertyChanged(string.Empty);
		}

		private void doPlayerInfoUpdate()
		{
			if (_selectedPlayerInfo != null)
			{
				var newPlayerInfo = CheckForUpdatedPlayerInfo();

				if (_selectedCharacterInfo != null && newPlayerInfo != null)
				{
					if (_dal.CommitPlayerInfo(_selectedCharacterInfo.Name, newPlayerInfo))
					{
						_selectedCharacterInfo.Level = newPlayerInfo.CurrentLevel;
					}
				}
			}
		}

		private PlayerInfo CheckForUpdatedPlayerInfo()
		{
			if (_selectedPlayerInfo == null) return null;
			var playerXml = _selectedPlayerInfo.ToXElement<PlayerInfo>();
			var newPlayerInfo = playerXml.FromXElement<PlayerInfo>();
			var oneChange = false;
			if (newPlayerInfo.BaseStrength != Strength)
			{
				newPlayerInfo.BaseStrength = Strength;
				oneChange = true;
			}
			if (newPlayerInfo.BaseDexterity != Dexterity)
			{
				newPlayerInfo.BaseDexterity = Dexterity;
				oneChange = true;
			}
			if (newPlayerInfo.BaseIntelligence != Intelligence)
			{
				newPlayerInfo.BaseIntelligence = Intelligence;
				oneChange = true;
			}
			if (newPlayerInfo.BaseHealth != Health)
			{
				newPlayerInfo.BaseHealth = Health;
				oneChange = true;
			}
			if (newPlayerInfo.BaseMana != Mana)
			{
				newPlayerInfo.BaseMana = Mana;
				oneChange = true;
			}
			if (newPlayerInfo.AttributesPoints != AttributePoints)
			{
				newPlayerInfo.AttributesPoints = AttributePoints;
				oneChange = true;
			}
			if (newPlayerInfo.SkillPoints != SkillPoints)
			{
				newPlayerInfo.SkillPoints = SkillPoints;
				oneChange = true;
			}
			if (newPlayerInfo.CurrentXP != XP)
			{
				newPlayerInfo.CurrentXP = XP;
				oneChange = true;
			}
			if (newPlayerInfo.CurrentLevel != Level)
			{
				newPlayerInfo.CurrentLevel = Level;
				oneChange = true;
			}
			if (DifficultySelected != null)
			{
				if (newPlayerInfo.DifficultyUnlocked != DifficultySelected.DifficultyId)
				{
					if (DifficultySelected.DifficultyId > newPlayerInfo.DifficultyUnlocked)
					{
						if (DifficultySelected.DifficultyId == 1)
						{
							if (newPlayerInfo.Money < 5000000)
							{
								newPlayerInfo.Money = 5000000;
							}
						} else if (DifficultySelected.DifficultyId > 1)
						{
							if (newPlayerInfo.Money < 7500000)
							{
								newPlayerInfo.Money = 7500000;
							}
						}

					}
					newPlayerInfo.DifficultyUnlocked = DifficultySelected.DifficultyId;
					oneChange = true;
				}
			}

			return (oneChange ? newPlayerInfo : null);
		}

		bool _enableDifficulty = false;
		public bool EnableDifficulty { get => _enableDifficulty; set=> SetProperty(ref _enableDifficulty, value); }

		private bool syncAttributePoints(int oldValue, int newValue)
		{
			if(oldValue < newValue)
			{
				if (AttributePoints > 0)
				{
					AttributePoints--;
					return true;
				}
				return false;
			}
			else if(oldValue > newValue)
			{
				AttributePoints++;
				return true;
			}
			return false;
		}

		public string StrengthLabel { get => _settings.GetPropertyFromResource("StrengthLabel"); }

		private int _strength = 50;
		public int Strength { get => _strength;
			set {
				var newValue = value;
				var oldValue = _strength;
				if(!syncAttributePoints(oldValue, newValue))
				{
					SetProperty(ref _strength, oldValue);
					return;
				}
				SetProperty(ref _strength, newValue);
			}
		}

		public string DexterityLabel { get => _settings.GetPropertyFromResource("DexterityLabel"); }
		private int _dexterity = 50;
		public int Dexterity
		{
			get => _dexterity;
			set
			{
				var newValue = value;
				var oldValue = _dexterity;
				if (!syncAttributePoints(oldValue, newValue))
				{
					SetProperty(ref _dexterity, oldValue);
					return;
				}
				SetProperty(ref _dexterity, newValue);
			}
		}


		public string IntelligenceLabel { get => _settings.GetPropertyFromResource("IntelligenceLabel"); }
		private int _intelligence = 50;
		public int Intelligence
		{
			get => _intelligence;
			set
			{
				var newValue = value;
				var oldValue = _intelligence;
				if (!syncAttributePoints(oldValue, newValue))
				{
					SetProperty(ref _intelligence, oldValue);
					return;
				}
				SetProperty(ref _intelligence, newValue);
			}
		}

		public string HealthLabel { get => _settings.GetPropertyFromResource("HealthLabel"); }
		private int _health = 300;
		public int Health
		{
			get => _health;
			set
			{
				var newValue = value;
				var oldValue = _health;
				if (!syncAttributePoints(oldValue, newValue))
				{
					SetProperty(ref _health, oldValue);
					return;
				}
				SetProperty(ref _health, newValue);
			}
		}

		public string ManaLabel { get => _settings.GetPropertyFromResource("ManaLabel"); }
		private int _mana = 300;
		public int Mana
		{
			get => _mana;
			set
			{
				var newValue = value;
				var oldValue = _mana;
				if (!syncAttributePoints(oldValue, newValue))
				{
					SetProperty(ref _mana, oldValue);
					return;
				}
				SetProperty(ref _mana, newValue);
			}
		}

		private void setupDifficulty()
		{
			_diffcultiesUnlocked.Clear();
			_diffcultiesUnlocked.Add(new DifficultyInfo { DisplayName = _settings.GetPropertyFromResource("Difficulty0"), DifficultyId = 0 });
			_diffcultiesUnlocked.Add(new DifficultyInfo { DisplayName = _settings.GetPropertyFromResource("Difficulty1"), DifficultyId = 1 });
			_diffcultiesUnlocked.Add(new DifficultyInfo { DisplayName = _settings.GetPropertyFromResource("Difficulty2"), DifficultyId = 2 });
			DifficultySelected = _diffcultiesUnlocked[0];
		}

		IList<DifficultyInfo> _diffcultiesUnlocked;
		public IList<DifficultyInfo> DiffcultiesUnlocked { get => _diffcultiesUnlocked; set => SetProperty(ref _diffcultiesUnlocked, value); }

		DifficultyInfo _difficultySelected;
		public DifficultyInfo DifficultySelected { get => _difficultySelected; set => SetProperty(ref _difficultySelected, value); }

		public string LevelLabel { get => _settings.GetPropertyFromResource("LevelLabel"); }

		private int _level = 1;
		public int Level
		{
			get => _level;
			set
			{
				var newValue = value;
				var oldValue = _level;

				if ((AttributePoints <= 0 || SkillPoints <= 0)&&oldValue>newValue)
				{
					SetProperty(ref _level, oldValue);
					return;
				}

				updateAttributes(oldValue, newValue);

				updateSkillPoints(oldValue, newValue);

				updateXP(newValue);

				SetProperty(ref _level, newValue);
			}
		}

		private void updateAttributes(int oldLevel, int newLevel)
		{
			if (oldLevel < newLevel)
			{
				var newPoints = AttributePoints + _dal.AtrributePointsPerLevel;
				AttributePoints = newPoints;
			}
			else if (oldLevel > newLevel)
			{
				var newPoints = AttributePoints - _dal.AtrributePointsPerLevel;
				if (newPoints >= 0)
				{
					AttributePoints = newPoints;
				}
				else
				{
					AttributePoints = 0;
				}
			}
		}

		private void updateSkillPoints(int oldLevel, int newLevel)
		{
			if (oldLevel < newLevel)
			{
				var newPoints = SkillPoints + _dal.SkillPointsPerLevel;
				SkillPoints = newPoints;
			}
			else if (oldLevel > newLevel)
			{
				var newPoints = SkillPoints - _dal.SkillPointsPerLevel;
				if (newPoints >= 0)
				{
					SkillPoints = newPoints;
				}
				else
				{
					SkillPoints = 0;
				}
			}
		}

		private void updateXP(int level)
		{
			if (level > 1)
			{
				var xp = _dal.GetLevelXP(level);
				if (xp > 0)
				{
					XP = xp + 1;
				}
			}
			else
			{
				XP = 0;
			}
		}

		public string XPLabel { get => _settings.GetPropertyFromResource("XPLabel"); }

		private int _xp = 1;
		public int XP
		{
			get => _xp;
			set
			{
				SetProperty(ref _xp, value);
			}
		}

		bool _attributeEnabled = false;
		public bool AttributeEnabled { get => _attributeEnabled; set => SetProperty(ref _attributeEnabled, value); }

		bool _skillsEnabled = false;
		public bool SkillsEnabled { get => _skillsEnabled; set => SetProperty(ref _skillsEnabled, value); }

		public string AttributeLabel { get => _settings.GetPropertyFromResource("AttributeLabel"); }

		int _attributePoints;
		public int AttributePoints
		{
			get => _attributePoints;
			set
			{
				SetProperty(ref _attributePoints, value);
			}
		}

		public string SkillsLabel { get => _settings.GetPropertyFromResource("SkillsLabel"); }

		int _skillPoints;
		public int SkillPoints
		{
			get => _skillPoints;
			set
			{
				SetProperty(ref _skillPoints, value);
			}
		}

		public string DifficultyLabel { get => _settings.GetPropertyFromResource("DifficultyLabel"); }

	}
}
