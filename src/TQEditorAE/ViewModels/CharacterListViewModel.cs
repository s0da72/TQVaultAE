﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Mvvm;
using TQEditorAE.Events;
using TQEditorAE.Models;

namespace TQEditorAE.ViewModels
{
	public class CharacterListViewModel : BindableBase
	{
		IList<CharacterInfo> _list;

		ISettings _settings;
		IEventAggregator _eventAggregator;
		ITQDal _dal;

		public CharacterListViewModel(ISettings settings, IEventAggregator eventAggregator, ITQDal dal)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
			_dal = dal;
			_list = _dal.Characters;
			if (_list.Count > 0)
			{
				CharacterSelected = _list[0];
			}
			//_list = new List<CharacterInfo>();

			//_list.Add(new CharacterInfo {Name = "testhero1", Level=1, ClassName="unknown" });
			//_list.Add(new CharacterInfo { Name = "testhero2", Level = 17, ClassName = "battlemage" });
			//_list.Add(new CharacterInfo { Name = "xxxxxxxxxxxxxxxtesthero2", Level = 17, ClassName = "battlemage" });

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
		}

		protected void LanguageSelected(string name)
		{
			_list = _dal.Characters;
			RaisePropertyChanged(string.Empty);
		}

		public IList<CharacterInfo> List { get => _list; set => SetProperty( ref _list, value); }

		CharacterInfo _characterSelected;
		public CharacterInfo CharacterSelected { get => _characterSelected;
			set {

				var chrSelected = value;
				if (chrSelected == null) return;
				var palyerInfo = _dal.GetCharacter(chrSelected.Name);
				if (palyerInfo == null) return;
				SetProperty(ref _characterSelected, value);
				_eventAggregator.GetEvent<CharacterSelectedEvent>().Publish(palyerInfo);
			}
		}

		public string NameLabel { get => _settings.GetPropertyFromResource("NameLabel"); }

		public string LevelLabel { get => _settings.GetPropertyFromResource("LevelLabel"); }

		public string ClassLabel { get => _settings.GetPropertyFromResource("ClassLabel"); }

	}
}
