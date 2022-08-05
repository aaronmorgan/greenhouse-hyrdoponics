using System;

namespace HydroponicsServer.Models
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DeletedOn { get; set; }

        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
        }
    }
}
