using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeeloewenCraft
{
    public partial class wndSettings : Window
    {
        //-- Constructor --//

        public wndSettings()
        {
            InitializeComponent();
            LoadSettings();
        }

        //-- Custom Methods --//
        private void SaveSettings()
        {
            //Check and save the auto stepup setting
            if (cbEnableAutoStepup.IsChecked == true)
            {
                Properties.Settings.Default.enableAutoStepup = true;
            }
            else
            {
                Properties.Settings.Default.enableAutoStepup = false;
            }

            //Check and save the save on world exit settings
            if(cbSaveWorldWhenClosing.IsChecked == true)
            {
                Properties.Settings.Default.saveWorldOnClose = true;
            }
            else
            {
                Properties.Settings.Default.saveWorldOnClose = false;
            }

            //Save the settings
            Properties.Settings.Default.Save();
        }

        private void LoadSettings()
        {
            //Check and load the auto stepup setting
            if(Properties.Settings.Default.enableAutoStepup == true)
            {
                cbEnableAutoStepup.IsChecked = true;
            }
            else
            {
                cbEnableAutoStepup.IsChecked = false;
            }

            //Check and load the save on world exit setting
            if(Properties.Settings.Default.saveWorldOnClose == true)
            {
                cbSaveWorldWhenClosing.IsChecked = true;
            }
            else
            {
                cbSaveWorldWhenClosing.IsChecked = false;
            }
        }

        //-- Event Handlers --//
        private void btnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            //Save the settings - WIP: Will get a rework later, currently resets when updating/reinstalling the app
            SaveSettings();
        }
    }
}
