namespace API.Helpers
{
    public class Userparams : PaginationParams
    {
        public string UserName { get; set; }

        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        
        public string OrderBy { get; set; } = "lastActive";
        
        
    }
}