namespace DecoranestBacknd.Ecommerce.Shared.DTO.Adminn
{
    public class AdminDashBoardDTO
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<MonthlyRevenueDTO> MonthlyRevenues { get; set; } = new List<MonthlyRevenueDTO>();
        public List<TopSellingDTO> TopSellingProducts { get; set; } = new List<TopSellingDTO>();

    }
}
