using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Prism.Regions;

namespace TQEditorAE.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(IRegionManager regionManager)
		{
			InitializeComponent();
			if (regionManager == null)
			{
				throw new ArgumentNullException(nameof(regionManager));
			}
			regionManager.RegisterViewWithRegion("BaseStatsRegion", typeof(BaseStats));
			regionManager.RegisterViewWithRegion("CharacterListRegion", typeof(CharacterList));
			regionManager.RegisterViewWithRegion("FileMenuRegion", typeof(FileMenu));
			regionManager.RegisterViewWithRegion("LevelingDisplayRegion", typeof(LevelingDisplay));
			//regionManager.RegisterViewWithRegion("PersonDetailsRegion", typeof(PersonDetail));
		}
	}
}
