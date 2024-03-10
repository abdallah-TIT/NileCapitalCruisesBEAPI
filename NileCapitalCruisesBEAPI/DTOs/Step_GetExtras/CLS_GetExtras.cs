namespace NileCapitalCruisesBEAPI.DTOs.Step_GetExtras
{
    public class CLS_GetExtras
    {

        public int ExtraId { get; set; }
        public string? ExtraNameSys { get; set; }

        public string? ExtraDescription { get; set; }

        public string? ExtraPhoto { get; set; }


        public double? BasicExtraPrice { get; set; }
        public double? TotalExtraPrice { get; set; }
    }
}
