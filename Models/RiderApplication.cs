namespace NextUses.Models
{
    public class RiderApplication
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public string RiderId { get; set; }
        public Users? Rider { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
    }
}
