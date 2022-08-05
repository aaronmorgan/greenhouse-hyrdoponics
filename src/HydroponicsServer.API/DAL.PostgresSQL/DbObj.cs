namespace DAL.PostgresSQL
{
    public class DbObj
    {
        public int Id { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }

        public DbObj()
        {
            DateTimeCreated = DateTime.UtcNow;
            DateTimeUpdated = DateTimeCreated;
        }
    }
}
