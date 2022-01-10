using System.Windows;

namespace TnCode.TnCodeApp
{
    /// <summary>
    /// Interaction logic for SplashView.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
        }

        private void Grid_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (GridMain.IsEnabled == false)
                this.Close();
        }
    }
}
