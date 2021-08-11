namespace LiveClinic.Contracts
{
    public interface OrderItem
    {
        string DrugCode { get; set; }
        double Days { get; set; }
        double Quantity { get; set; }
    }
}