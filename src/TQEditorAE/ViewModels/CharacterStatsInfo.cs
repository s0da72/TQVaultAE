using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using TQVaultData;

namespace TQEditorAE.ViewModels
{
	public class CharacterStatsInfo : BindableBase
	{

		string _name = "";
		public string Name { get => _name; set => SetProperty(ref _name, value); }

		string _value = "";
		public string Value { get => _value; set => SetProperty(ref _value, value); }

	}
}
