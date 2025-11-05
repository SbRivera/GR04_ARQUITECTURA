using UCONV_CLIMOV_REST.Controller;

namespace UCONV_CLIMOV_REST.View;

public partial class UnitConverterView : ContentPage
{
    private readonly UnitConverterController _controller;

    public UnitConverterView()
    {
        InitializeComponent();

        // Initialize the controller
        _controller = new UnitConverterController();
    }

    /// <summary>
    /// Handles the "Convert to Pounds" button click.
    /// </summary>
    private async void OnConvertToPoundsClicked(object sender, EventArgs e)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(KgInput.Text) || !double.TryParse(KgInput.Text, out var kg))
            {
                KgToLbResultLabel.Text = "Please enter a valid number.";
                return;
            }

            // Call the conversion API
            var result = await _controller.KgTolbConverter(kg.ToString());

            // Update the result label
            KgToLbResultLabel.Text = $"{kg} kilograms = {result:F2} pounds";
        }
        catch (Exception ex)
        {
            KgToLbResultLabel.Text = $"Error: {ex.Message}";
        }
    }

    /// <summary>
    /// Handles the "Convert to Kilograms" button click.
    /// </summary>
    private async void OnConvertToKgClicked(object sender, EventArgs e)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(LbInput.Text) || !double.TryParse(LbInput.Text, out var lb))
            {
                LbToKgResultLabel.Text = "Please enter a valid number.";
                return;
            }

            // Call the conversion API
            var result = await _controller.LbToKgConverter(lb.ToString());

            // Update the result label
            LbToKgResultLabel.Text = $"{lb} pounds = {result:F2} kilograms";
        }
        catch (Exception ex)
        {
            LbToKgResultLabel.Text = $"Error: {ex.Message}";
        }
    }
}
