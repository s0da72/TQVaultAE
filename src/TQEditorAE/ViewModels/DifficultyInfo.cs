using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQEditorAE.ViewModels
{
	public class DifficultyInfo : BindableBase
	{
		public string DisplayName { get; set; }

		public int DifficultyId { get; set; }
	}
}
