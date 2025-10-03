namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class AdminUserDTO
    {
        public int User_Id { get; set; }   
        public string Name { get; set; }   
        public string Email { get; set; } 
        public string Role { get; set; }   
        public bool IsBlocked { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
