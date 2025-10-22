namespace WS_ConUni_REST_DOTNET
{
    public class WS_ConUni
    {
        public double cm_to_inch(double cm)
        {
            return cm / 2.54;
        }

        public double inch_to_cm(double inch)
        {
            return inch * 2.54;
        }

        // Fahrenheit <-> Celsius
        public double fahrenheit_to_celsius(double f)
        {
            return (f - 32.0) * 5.0 / 9.0;
        }

        public double celsius_to_fahrenheit(double c)
        {
            return c * 9.0 / 5.0 + 32.0;
        }

        // Kilogramos <-> Libras
        public double kg_to_lb(double kg)
        {
            return kg * 2.20462262185;
        }

        public double lb_to_kg(double lb)
        {
            return lb / 2.20462262185;
        }
    }
}
