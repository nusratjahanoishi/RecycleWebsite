namespace NextUses.Models.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalRiders { get; set; }

        public int TotalCategories { get; set; }

        public int ActiveProducts { get; set; }
        public int DeactiveProducts { get; set; }

        public int PendingOrders { get; set; }
        public int CompleteOrders { get; set; }
        public int RiderAssignedOrders { get; set; }
        public int RiderRejectedOrders { get; set; }
        public int CancelledOrders { get; set; }

        public int PendingRiderApply { get; set; }
        public int AcceptedRiderApply { get; set; }
        public int RejectedRiderApply { get; set; }
    }

}
