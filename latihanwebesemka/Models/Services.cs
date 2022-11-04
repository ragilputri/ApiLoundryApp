namespace latihanwebesemka.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public int Unit { get; set; }
        public double Price { get; set; }
        public int EstimationDuration { get; set; }
    }

    public class ServiceUpload
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public int EstimationDuration { get; set; }
    }


    public enum OptionCategoryEnum
    {
        Kiloan,
        Satuan,
        PerlengkapanBayi,
        Helm,
        Sepatu
    }

    public enum OptionUnitEnum
    {
        KG,
        Piece
    }

    public class ServiceQueryModel
    {
        public OptionCategoryEnum OptionCategory { get; set; }
        public OptionUnitEnum OptionUnit { get; set; }

    }

}
