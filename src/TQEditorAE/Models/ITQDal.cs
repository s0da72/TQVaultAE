using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQEditorAE.ViewModels;

using TQVaultData;

namespace TQEditorAE.Models
{
	public interface ITQDal
	{
		IList<CharacterInfo> Characters { get; }
	}
}
