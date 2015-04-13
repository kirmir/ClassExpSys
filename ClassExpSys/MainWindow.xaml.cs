using System;
using System.Windows;
using System.Windows.Data;

namespace ClassExpSys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Expert system.
        /// </summary>
        public ExpertSystem ClassExpertSystem;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassExpertSystem = new ExpertSystem();
                ClassExpertSystem.LoadDatabase("Database.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured during loading database file: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            //Set items sources for list boxes.
            attributesListBox.ItemsSource = ClassExpertSystem.Attributes;
            objectsListBox.ItemsSource = ClassExpertSystem.FilteredObjects;

            ClassExpertSystem.PropertyChanged += ClassExpertSystem_PropertyChanged;
        }

        void ClassExpertSystem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilteredObjects")
            {
                objectsListBox.ItemsSource = ClassExpertSystem.FilteredObjects;
            }
        }
    }
}
