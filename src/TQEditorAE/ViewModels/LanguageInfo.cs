using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;

namespace TQEditorAE.ViewModels
{
	public class LanguageInfo : BindableBase
	{
		string _name = "";
		public string Name { get => _name; set => SetProperty(ref _name, value); }

		string _displayName = "";
		public string DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }

	}
}
