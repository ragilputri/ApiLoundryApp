namespace latihanwebesemka.Models
{
    public class Package
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public virtual ServiceUpload Service { get; set; }
        public double Total { get; set; }
        public double Price { get; set; }
    }
    public class PackageUpload
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; } 
        public double Total { get; set; }
        public double Price { get; set; }
    }
}
