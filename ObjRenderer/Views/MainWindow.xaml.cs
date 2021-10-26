using System.Windows;

namespace ObjRenderer.Views
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
        /// Handles the Click event of the exitItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void exitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the openItme control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void openItme_Click(object sender, RoutedEventArgs e)
        {
            var openFileWindow = new LoadFile();
            if (openFileWindow.ShowDialog() is true)
            {
                var loadedFile = ObjLoader.Instance.Load(openFileWindow.FilePath);
            }
        }
    }
}
