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

			_diffcultiesUnlocked = new List<DifficultyInfo>();
			setupDifficulty();
		}

		public string AttributeGroupBoxName { get => _settings.GetPropertyFromResource("AttributeGroupBoxName"); }

		public string LevelGroupBoxName { get => _settings.GetPropertyFromResource("LevelGroupBoxName"); }

		protected void CharacterSelected(PlayerInfo player)
		{
			if (player == null) return;
			Strength = player.BaseStrength;
			Dexterity = player.BaseDexterity;
			Intelligence = player.BaseIntelligence;
			Health = player.BaseHealth;
			Mana = player.BaseMana;
			AttributePoints = player.AttributesPoints;
			SkillPoints = player.SkillPoints;
			XP = player.CurrentXP;
			Level = player.CurrentLevel;

			if (player.DifficultyUnlocked < _diffcultiesUnlocked.Count)
			{
				DifficultySelected = _diffcultiesUnlocked[player.DifficultyUnlocked];
			}

		}

		protected void LanguageSelected(string name)
		{
			RaisePropertyChanged(string.Empty);
		}

		public string StrengthLabel { get => _settings.GetPropertyFromResource("StrengthLabel"); }

		private int _strength = 50;
		public int Strength { get => _strength;
			set {
				var newValue = value;
				var oldValue = _strength;
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
				var updateValue = _dexterity;
				updateValue = value;
				SetProperty(ref _dexterity, updateValue);
			}
		}


		public string IntelligenceLabel { get => _settings.GetPropertyFromResource("IntelligenceLabel"); }
		private int _intelligence = 50;
		public int Intelligence
		{
			get => _intelligence;
			set
			{
				var updateValue = _intelligence;
				updateValue = value;
				SetProperty(ref _intelligence, updateValue);
			}
		}

		public string HealthLabel { get => _settings.GetPropertyFromResource("HealthLabel"); }
		private int _health = 300;
		public int Health
		{
			get => _health;
			set
			{
				var updateValue = _health;
				updateValue = value;
				SetProperty(ref _health, updateValue);
			}
		}

		public string ManaLabel { get => _settings.GetPropertyFromResource("ManaLabel"); }
		private int _mana = 50;
		public int Mana
		{
			get => _mana;
			set
			{
				var updateValue = _mana;
				updateValue = value;
				SetProperty(ref _mana, updateValue);
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
