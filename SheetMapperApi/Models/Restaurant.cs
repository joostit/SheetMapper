namespace SheetMapperApi.Models
{
    public class Restaurant
    {

        public string? Name { get; set; }

        public decimal AvgPrice { get; set; }

        public Score Score { get; private set; } = new Score();

    }
}
