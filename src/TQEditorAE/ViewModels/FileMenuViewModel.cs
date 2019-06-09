using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TQEditorAE.Events;
using TQEditorAE.Models;

namespace TQEditorAE.ViewModels
{
	public class FileMenuViewModel : BindableBase
	{
		

		private DelegateCommand<string> _openOptionsDialogCommand;
		public DelegateCommand<string> OpenOptionsDialogCommand =>
			_openOptionsDialogCommand ?? (_openOptionsDialogCommand = new DelegateCommand<string>(OpenOptionsDialog));


		private DelegateCommand<string> _commitChangesCommand;
		public DelegateCommand<string> CommitChangesCommand =>
			_commitChangesCommand ?? (_commitChangesCommand = new DelegateCommand<string>(CommitChanges));

		private IDialogService _dialogService;
		ISettings _settings;
		IEventAggregator _eventAggregator;

		public FileMenuViewModel(ISettings settings, IDialogService dialogService, IEventAggregator eventAggregator)
		{
			_dialogService = dialogService;
			_settings = settings;
			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<LanguageSelectedEvent>().Subscribe(LanguageSelected);
		}


		protected void LanguageSelected(string name)
		{
			RaisePropertyChanged(string.Empty);
		}


		public string FileMenuName { get => _settings.GetPropertyFromResource("FileMenuName"); }

		public string OptionsMenuName { get => _settings.GetPropertyFromResource("OptionsMenuName"); }

		public string ExitMenuName { get => _settings.GetPropertyFromResource("ExitMenuName"); }

		protected virtual void OpenOptionsDialog(string parameter)
		{
			ShowOptionsDialog();
		}

		private void ShowOptionsDialog()
		{
			var message = _settings.GetPropertyFromResource("OptionsMessage");
			//using the dialog service as-is
			_dialogService.ShowDialog("EditorOptions", new DialogParameters($"message={message}"), r =>
			{
				//if (!r.Result.HasValue)
				//	Title = "Result is null";
				//else if (r.Result == true)
				//	Title = "Result is True";
				//else if (r.Result == false)
				//	Title = "Result is False";
				//else
				//	Title = "What the hell did you do?";
			});
		}

		protected virtual void CommitChanges(string parameter)
		{
			ShowCommitDialog();
		}

		private void ShowCommitDialog()
		{
			_eventAggregator.GetEvent<PreCommitEvent>().Publish("");
			//using the dialog service as-is
			_dialogService.ShowDialog("CommitChanges", new DialogParameters($""), r =>
			{
			});

		}

		public string CommitAllChangesBtn { get => _settings.GetPropertyFromResource("CommitAllChangesBtn"); }

	}

}
