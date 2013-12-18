﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace ShinePhoto.Models
{
    [Table(Name = "tbUser")]
    public class UserModel
    {
        [Column(IsPrimaryKey = true)]
        public string UserId { get; set; }
        [Column(CanBeNull = false)]
        public string UserName { get; set; }
        [Column(CanBeNull = false)]
        public string Password { get; set; }
        [Column(CanBeNull = false)]
        public string CreateDate { get; set; }
        [Column(CanBeNull = true)]
        public string ConfigInfo { get; set; }
    }
}