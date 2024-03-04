namespace NileCapitalCruisesBEAPI.DTOs.Step_GetCabins
{
    public static class CLS_PriceTaxes
    {
        public static double? TotalPriceAfterTax { get; set; }
        public static double? AmountService { get;  set; } 
        public static double? AmountVat { get;  set; } 
        public static double? AmountCityTax { get;  set; } 


        public static void CalculatePriceTaxes(double? netTotalPrice)
        {
            var prec_var_services = 12.0; // Declare as double
            var prec_var_vat = 14.0; // Declare as double
            var prec_var_city = 1.0; // Declare as double

            AmountService = netTotalPrice * (prec_var_services / 100.0); // Perform floating-point division
            AmountVat = (netTotalPrice + AmountService) * (prec_var_vat / 100.0); // Perform floating-point division
            AmountCityTax = netTotalPrice * (prec_var_city / 100.0); // Perform floating-point division
            TotalPriceAfterTax = netTotalPrice + AmountService + AmountVat + AmountCityTax;
        }
    }
}
