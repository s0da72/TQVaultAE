using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;

namespace TQEditorAE.ViewModels
{
	public class CharacterInfo : BindableBase
	{
		string _name = "";
		public string Name { get => _name; set => SetProperty(ref _name, value); }

		int _level = 0;
		public int Level { get => _level; set => SetProperty(ref _level, value); }

		string _className = "";
		public string ClassName { get => _className; set => SetProperty(ref _className, value); }

	}
}
