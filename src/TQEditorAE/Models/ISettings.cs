using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TQEditorAE.Models
{
	public interface ISettings
	{
		string GetPropertyFromSettings(string key);

		string GetPropertyFromResource(string key);

		void SetPropertyToSettings(string key, string value);

		void Save();

	}
}
