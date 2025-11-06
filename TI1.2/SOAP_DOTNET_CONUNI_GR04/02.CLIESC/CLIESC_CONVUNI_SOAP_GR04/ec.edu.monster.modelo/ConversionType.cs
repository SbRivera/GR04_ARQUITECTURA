using System.ComponentModel;

namespace ec.edu.monster.modelo
{
    public enum ConversionType
    {
        // Longitud
        [Description("Centímetros → Pulgadas")] CM_A_IN,
        [Description("Pulgadas → Centímetros")] IN_A_CM,

        // Temperatura
        [Description("Celsius (°C) → Fahrenheit (°F)")] C_A_F,
        [Description("Fahrenheit (°F) → Celsius (°C)")] F_A_C,

        // Masa
        [Description("Kilogramos → Libras")] KG_A_LB,
        [Description("Libras → Kilogramos")] LB_A_KG
    }
}
