namespace Domain
{
    public class UserFollowing
    {
        public string OberverId { get; set; }
        public AppUser Oberver { get; set; }
        public string TargetId { get; set; }
        public AppUser Target { get; set; }
    }
}