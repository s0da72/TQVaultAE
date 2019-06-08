using System;
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
	public class LevelingDisplayViewModel : BindableBase
	{
		IEventAggregator _eventAggregator;
		ISettings _settings;
		public LevelingDisplayViewModel(ISettings settings, IEventAggregator eventAggregator)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
		}

		protected void LanguageSelected(string name)
		{
			RaisePropertyChanged(string.Empty);
		}

		public string LevelLabel { get => _settings.GetPropertyFromResource("LevelLabel"); }

		private int _level = 1;
		public int Level { get => _level;
			set {
				_level = value;
			}
		}

	}
}
