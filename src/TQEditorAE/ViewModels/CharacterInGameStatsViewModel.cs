using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Prism.Events;
using Prism.Mvvm;
using TQEditorAE.Events;
using TQEditorAE.Models;
using TQVaultData;

namespace TQEditorAE.ViewModels
{
	public class CharacterInGameStatsViewModel : BindableBase
	{
		IList<CharacterStatsInfo> _list;

		private struct LabelData
		{
			public string Text;
			public int Handler;
			public CharacterStatsInfo keyPair;
		}

		Dictionary<string, LabelData> _labelKey = new Dictionary<string, LabelData>
		{
		};

		ISettings _settings;
		IEventAggregator _eventAggregator;
		ITQDal _dal;

		public CharacterInGameStatsViewModel(ISettings settings, IEventAggregator eventAggregator, ITQDal dal)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
			_dal = dal;
			_list = new List<CharacterStatsInfo>();
			LoadCharacterLabelFile(_settings.GetPropertyFromResource("CharacterInfoDisplay"));

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
			_eventAggregator.GetEvent<CharacterSelectedEvent>().Subscribe(CharacterSelected);
		}

		protected void LanguageSelected(string name)
		{
			LoadCharacterLabelFile(_settings.GetPropertyFromResource("CharacterInfoDisplay"));
			RaisePropertyChanged(string.Empty);
		}

		protected void CharacterSelected(CharacterInfo player)
		{
			if (player == null) return;
			if (player.PlayerInfo == null) return;

			//converts playerinfo to xml so it can be bound to Resource data file CharacterInfoDispaly.txt
			//Order of data displayed is controlled by order of the CharacterInfoDispaly.txt resource file.
			var playerXml = player.PlayerInfo.ToXElement<PlayerInfo>();

			foreach (var labelKey in _labelKey.Keys)
			{
				var elm = playerXml.Element(labelKey);
				if (elm != null)
				{
					var label = _labelKey[labelKey];
					var value = "";
					switch (label.Handler)
					{
						default:
							value = string.Format("{0}", elm.Value);
							break;
					}
					//label.Text should be language specific
					if (label.keyPair != null)
					{
						label.keyPair.Value = value;
					}
				}
			}

		}

		private void LoadCharacterLabelFile(string fileContents)
		{
			using (var sr = new StringReader(fileContents))
			{
				var data = sr.ReadLine();
				while (data != null)
				{
					var content = data.Split('=');
					if (content != null && content.Length > 1)
					{
						switch (content[0].ToUpper())
						{
							case "GREATESTDAMAGEINFLICTED":
							case "CLASS":
							case "DIFFICULTYUNLOCKED":
							case "PLAYERLEVEL":
							case "MAXLEVEL":
								//ignore for now
								break;
							default:
								if (!_labelKey.ContainsKey(content[0]))
								{
									var label = new LabelData() { Text = content[1], Handler = 0, keyPair = new CharacterStatsInfo { Name = content[1], Value = "" } };
									_labelKey.Add(
										content[0],
										label
									); ;
									_list.Add(label.keyPair);
								}
								else
								{
									var label = _labelKey[content[0]];
									label.Text = content[0];
									label.keyPair.Name = content[0];
								}
								break;
						}
					}
					data = sr.ReadLine();
				}
			}
		}


		public IList<CharacterStatsInfo> List { get => _list; set => SetProperty( ref _list, value); }

		CharacterInfo _statsSelected;
		public CharacterInfo StatsSelected	{ get => _statsSelected;  set => SetProperty(ref _statsSelected, value); }

		public string NameLabel { get => _settings.GetPropertyFromResource("NameLabel"); }

		public string ValueLabel { get => _settings.GetPropertyFromResource("ValueLabel"); }


	}
}
