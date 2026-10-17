namespace ChemiseLab.Dto.Delivery
{
    public class DistrictsList_Dto
    {
        public int IdDistrict { get; set; }
        public string Ville { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ArabicName { get; set; }
        public string? Delai { get; set; }
    }
}
