using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TQEditorAE.Events;
using TQEditorAE.Models;

namespace TQEditorAE.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{

		public string Title { get => _settings.GetPropertyFromResource("EditorProgramTitle"); }

		ISettings _settings;
		IEventAggregator _eventAggregator;
		public MainWindowViewModel(ISettings settings, IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			_settings = settings;

			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageChanged);
			_eventAggregator.GetEvent<SettingsChangedEvent>().Subscribe(SettingsFileChanged);
		}

		public void SettingsFileChanged(string l)
		{
			var msg = _settings.GetPropertyFromResource("RestartMessage");
			MessageBox.Show(msg);
			System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
			Application.Current.Shutdown();
		}


		protected void LanguageChanged(string name)
		{
			RaisePropertyChanged(string.Empty);
		}

	}
}
