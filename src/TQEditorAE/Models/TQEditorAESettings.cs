using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using TQEditorAE.Properties;

namespace TQEditorAE.Models
{
	public class TQEditorAESettings : ISettings
	{
		public string GetPropertyFromSettings(string key)
		{
			return ((string)Settings.Default[key]);
		}

		public string GetPropertyFromResource(string key)
		{
			return ((string)TQLangAE.Resource.ResourceManager.GetString(key));
		}

		public void SetPropertyToSettings(string key, string value)
		{
			if (Settings.Default[key] != null)
			{
				Settings.Default[key] = value;
			}

		}

		public void Save()
		{
			Settings.Default.Save();
		}
	}
}
