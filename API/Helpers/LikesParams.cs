namespace API.Helpers
{
    public class LikesParams : PaginationParams
    {
        public string predicate { get; set; }
        public int UserId { get; set; }
        
        
        
    }
}