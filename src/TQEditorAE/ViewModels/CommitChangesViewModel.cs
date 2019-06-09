using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TQEditorAE.Events;
using TQEditorAE.Models;

namespace TQEditorAE.ViewModels
{
	public class CommitChangesViewModel : BindableBase, IDialogAware
	{

		IEventAggregator _eventAggregator;
		ISettings _settings;
		ITQDal _dal;

		IList<CharacterInfo> _list;

		public CommitChangesViewModel(ISettings settings, IEventAggregator eventAggregator, ITQDal dal)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
			_dal = dal;
			_list = _dal.ModifiedCharacters;
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

		private string _title = "Commit Changes";
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
				try
				{
					_dal.SaveChangesToDisk();

					foreach (var chr in _list)
					{
						if (chr == null || chr.PlayerInfo == null) continue;
						chr.PlayerInfo.Modified = false;
					}
				}
				catch (Exception e)
				{
					result = false;
					MessageBox.Show("{0}",e.ToString());
					return;
				}
				result = true;
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

		bool _enableCommit = false;
		public bool EnableCommit { get => _enableCommit; set => SetProperty(ref _enableCommit, value); }

		public virtual void OnDialogOpened(IDialogParameters parameters)
		{
			if (_list == null || _list.Count <= 0){
				Message = _settings.GetPropertyFromResource("NoChangesDectected");
				EnableCommit = false;
				return;
			}
			Message = string.Format(_settings.GetPropertyFromResource("ChangesDectected"), _list.Count);
			EnableCommit = true;
		}

		public IList<CharacterInfo> List { get => _list; set => SetProperty(ref _list, value); }

		CharacterInfo _characterSelected;
		public CharacterInfo CharacterSelected
		{
			get => _characterSelected;
			set
			{

				var chrSelected = value;
				if (chrSelected == null) return;
				var palyerInfo = _dal.GetCharacter(chrSelected.Name);
				if (palyerInfo == null) return;
				SetProperty(ref _characterSelected, value);
				_characterSelected.PlayerInfo = palyerInfo;
				_eventAggregator.GetEvent<CharacterSelectedEvent>().Publish(_characterSelected);
			}
		}

		public string NameLabel { get => _settings.GetPropertyFromResource("NameLabel"); }

		public string LevelLabel { get => _settings.GetPropertyFromResource("LevelLabel"); }

		public string ClassLabel { get => _settings.GetPropertyFromResource("ClassLabel"); }

		public string CommitBtn { get => _settings.GetPropertyFromResource("CommitBtn"); }

		public string CancelBtn { get => _settings.GetPropertyFromResource("CancelBtn");	}

	}
}
