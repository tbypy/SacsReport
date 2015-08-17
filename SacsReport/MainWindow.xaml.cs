using System.Windows;

namespace SacsReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowVM();
            InitializeComponent();
        }
    }
}
