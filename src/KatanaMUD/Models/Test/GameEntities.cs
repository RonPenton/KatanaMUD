using System;

namespace KatanaMUD.Models.Test
{
    public class GameEntities : Spam.EntityContext
    {
        GameEntities(string connectionString)
            : base(connectionString)
        { }
    }
}