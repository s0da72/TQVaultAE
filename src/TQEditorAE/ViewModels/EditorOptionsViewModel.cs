using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TQEditorAE.Events;
using TQEditorAE.Models;

namespace TQEditorAE.ViewModels
{
	public class EditorOptionsViewModel : BindableBase, IDialogAware
	{

		IEventAggregator _eventAggregator;
		ISettings _settings;
		ITQDal _dal;

		LanguageInfo _originalLanguage;
		public EditorOptionsViewModel(ISettings settings, IEventAggregator eventAggregator, ITQDal dal)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
			_dal = dal;
			var list = TQLangAE.LanguageUtility.CreateSupportedLanguageList(_settings.GetPropertyFromSettings("SupportedLanguages"));

			var defaultLanguage = _settings.GetPropertyFromSettings("DefaultLanguage");
			_languages = new List<LanguageInfo>();
			foreach(var language in list)
			{
				var langInfo = new LanguageInfo { Name = language.Key, DisplayName = language.Value };
				_languages.Add(langInfo);
				if (string.Equals(defaultLanguage, langInfo.Name, StringComparison.CurrentCultureIgnoreCase))
				{
					LanguageSelected = _originalLanguage = langInfo;
				}
			}
		}


		private DelegateCommand<string> _closeDialogCommand;
		public DelegateCommand<string> CloseDialogCommand =>
			_closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

		private string _message;
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		private string _title = "Options";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		IList<LanguageInfo> _languages;
		public IList<LanguageInfo> Languages { get => _languages; set => SetProperty(ref _languages, value); }

		LanguageInfo _languageSelected;
		public LanguageInfo LanguageSelected { get => _languageSelected; set => SetProperty(ref _languageSelected, value); }

		public event Action<IDialogResult> RequestClose;

		protected virtual void CloseDialog(string parameter)
		{
			bool? result = null;

			if (parameter?.ToLower() == "true")
			{
				var bOneChange = false;
				result = true;
				if (LanguageSelected != _originalLanguage)
				{
					var language = TQLangAE.LanguageUtility.SetUILanguage(LanguageSelected.Name);
					_settings.SetPropertyToSettings("DefaultLanguage", language);
					_dal.UpdateLanguageData();
					_eventAggregator.GetEvent<LanguageSelectedEvent>().Publish(LanguageSelected.Name);
					bOneChange = true;
				}

				if (bOneChange) {
					_settings.Save();
					_eventAggregator.GetEvent<SettingsChangedEvent>().Publish("");
				}
			}
			else if (parameter?.ToLower() == "false")
			{
				result = false;
			}

			RaiseRequestClose(new DialogResult(result));
		}


		public virtual void RaiseRequestClose(IDialogResult dialogResult)
		{
			RequestClose?.Invoke(dialogResult);
		}

		public virtual bool CanCloseDialog()
		{
			return true;
		}

		public virtual void OnDialogClosed()
		{

		}

		public virtual void OnDialogOpened(IDialogParameters parameters)
		{
			Message = parameters.GetValue<string>("message");
		}
	}
}
