using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private bool _isAuthenticated = false;
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "MSAL WinUI 3 Packaged Desktop App Sample";
        }

        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            if (!_isAuthenticated)
            {
                this._isAuthenticated = true;
                signInOutButton.Content = "Sign Out";
            }
            else
            {
                this._isAuthenticated = false;
                signInOutButton.Content = "Sign In";
            }
        }
    }
}