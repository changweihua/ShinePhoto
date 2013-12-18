using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data;
using System.Data.Linq.Mapping;

namespace ShinePhoto.Models
{
    [Database(Name = "ShinePhoto")]
    public class ShinePhotoDataContext : DataContext
    {
        public Table<UserModel> Users;

        public ShinePhotoDataContext(IDbConnection connection) : base(connection) {}

        public ShinePhotoDataContext(string connection) : base(connection) { }

    }
}
