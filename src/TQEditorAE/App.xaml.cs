using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;
using Prism.Modularity;
using TQEditorAE.Views;
using TQEditorAE.ViewModels;
using TQEditorAE.Properties;
using TQEditorAE.Models;
using System.Runtime.InteropServices;

namespace TQEditorAE
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : PrismApplication
	{

		protected override Window CreateShell()
		{
			var language = TQLangAE.LanguageUtility.SetUILanguage(Settings.Default.DefaultLanguage);
			Settings.Default.DefaultLanguage = language;
			
			//Settings.Default.
			return Container.Resolve<MainWindow>();
		}

		public void OnInitialized(IContainerProvider containerProvider)
		{
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterDialog<EditorOptions, EditorOptionsViewModel>();
			containerRegistry.Register<ISettings, TQEditorAESettings>();
			containerRegistry.Register<ITQDal, TQDalWrapper>();
		}

		//protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
		//{
		//}

		//protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		//{
		//	moduleCatalog.AddModule<ModuleAModule>();
		//}
	}
}
