namespace latihanwebesemka.Models
{
    public class PackageTransaction
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public Guid PackageId { get; set; }
        public double Price { get; set; }
        public double AvaibleUnit { get; set; }
    }

    public class PackageTransactionCreate
    {
        public string UserEmail { get; set; }
        public Guid PackageId { get; set; }
    }

    public class PackageTransactionsShow
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public virtual User User { get; set; }
        public Guid PackageId { get; set; }
        public virtual Package Package { get; set; }
        public double Price { get; set; }
        public double AvaibleUnit { get; set; }
    }
}
