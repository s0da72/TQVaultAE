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
	public class BaseStatsViewModel : BindableBase
	{
		IEventAggregator _eventAggregator;
		ISettings _settings;
		public BaseStatsViewModel(ISettings settings, IEventAggregator eventAggregator)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
		}

		protected void LanguageSelected(string name)
		{
			RaisePropertyChanged(string.Empty);
		}

		public string StrengthLabel { get => _settings.GetPropertyFromResource("StrengthLabel"); }

		private int _strength = 50;
		public int Strength { get => _strength;
			set {
				var updateValue = _strength;
				if (value > _strength)
				{
					updateValue = value;
				}
				else
				{
				}
				_strength = updateValue;
			}
		}

		public string DexterityLabel { get => _settings.GetPropertyFromResource("DexterityLabel"); }

		public string IntelligenceLabel { get => _settings.GetPropertyFromResource("IntelligenceLabel"); }

		public string HealthLabel { get => _settings.GetPropertyFromResource("HealthLabel"); }

		public string ManaLabel { get => _settings.GetPropertyFromResource("ManaLabel"); }
	}
}
