using System;

namespace HydroponicsServer.Models.Interfaces
{
    public interface IBaseEntity
    {
        DateTime CreatedOn { get; set; }

        DateTime LastUpdated { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
