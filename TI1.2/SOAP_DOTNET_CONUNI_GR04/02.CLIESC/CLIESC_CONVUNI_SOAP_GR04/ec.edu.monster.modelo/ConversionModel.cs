using System.Collections.Generic;

namespace ec.edu.monster.modelo
{
    public static class ConversionModel
    {
        public static readonly Dictionary<ConversionCategory, List<ConversionType>> PorCategoria =
            new()
            {
                { ConversionCategory.Longitud, new() { ConversionType.CM_A_IN, ConversionType.IN_A_CM } },
                { ConversionCategory.Temperatura, new() { ConversionType.C_A_F, ConversionType.F_A_C } },
                { ConversionCategory.Masa, new() { ConversionType.KG_A_LB, ConversionType.LB_A_KG } }
            };
    }
}
