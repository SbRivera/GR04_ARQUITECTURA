
using UCONV_CLIMOV_REST.Controller;
using UCONV_CLIMOV_REST.Model;

namespace UCONV_CLIMOV_REST
{
    public partial class MainPage : ContentPage
    {
        private readonly UnitConvLoginController _loginController;

        public MainPage()
        {
            InitializeComponent();
            
            // Initialize HttpClient and pass it to UnitConvLoginController
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.244.140.58:44322/")
            };

            _loginController = new UnitConvLoginController(httpClient);
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Get the user input from the UI
            string enteredUsername = UsernameEntry.Text;
            string enteredPassword = PasswordEntry.Text;

            // Prepare the UserModel object with the user credentials
            UserModel user = new UserModel
            {
                userName = enteredUsername,
                password = enteredPassword
            };

            // Use the UnitConvLoginController to perform the login
            bool loginSuccessful = await _loginController.LoginAsync(user);

            if (loginSuccessful)
            {
                // Update UI for a successful login
                LoginStatusLabel.Text = "Login successful!";
                LoginStatusLabel.TextColor = Colors.Green;

                // Navigate to the UnitConverterView on successful login
                await Shell.Current.GoToAsync("///UnitConverterView");
            }
            else
            {
                // Update UI for a failed login
                LoginStatusLabel.Text = "Invalid username or password.";
                LoginStatusLabel.TextColor = Colors.Red;
            }
        }
    }

}
