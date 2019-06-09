using System;
using System.Collections.Generic;
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

		ISettings _settings;
		IEventAggregator _eventAggregator;
		ITQDal _dal;

		public CharacterInGameStatsViewModel(ISettings settings, IEventAggregator eventAggregator, ITQDal dal)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
			_dal = dal;
			_list = new List<CharacterStatsInfo>();

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
			_eventAggregator.GetEvent<CharacterSelectedEvent>().Subscribe(CharacterSelected);
		}

		protected void LanguageSelected(string name)
		{
			RaisePropertyChanged(string.Empty);
		}

		protected void CharacterSelected(CharacterInfo player)
		{
			if (player == null) return;
			if (player.PlayerInfo == null) return;

			var x = player.PlayerInfo.ToXElement<PlayerInfo>();

		}

		public IList<CharacterStatsInfo> List { get => _list; set => SetProperty( ref _list, value); }

		CharacterInfo _statsSelected;
		public CharacterInfo StatsSelected	{ get => _statsSelected;  set => SetProperty(ref _statsSelected, value); }

		public string NameLabel { get => _settings.GetPropertyFromResource("NameLabel"); }

		public string LevelLabel { get => _settings.GetPropertyFromResource("ValueLabel"); }


	}
}
