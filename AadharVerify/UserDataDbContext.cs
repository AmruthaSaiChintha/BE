using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Permissions;
using AadharVerify.Models;

namespace AadharVerify.Models
{
    public class UserDataDbContext:DbContext

    {
        public UserDataDbContext(DbContextOptions<UserDataDbContext> options)
           : base(options)
        {
        }
        public DbSet<Data> Datas { get; set; }

        public DbSet<Users> UsersList { get;set; }

        public DbSet<Phone> Phonenumber{ get; set; }
        public DbSet<ImageProof> ImageProof { get; set; } = default!;

        public DbSet<Sample> Sample { get; set; } = default!;
    }
}
