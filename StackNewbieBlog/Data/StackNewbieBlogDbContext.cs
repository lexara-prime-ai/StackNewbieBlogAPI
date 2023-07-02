using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackNewbieBlog.Models.Entities;

namespace StackNewbieBlog.Data
{
    public class StackNewbieBlogDbContext : DbContext
    {
        // CREATE CONSTRUCTOR
        public StackNewbieBlogDbContext(DbContextOptions options) : base(options)
        {

        }

        // DbSet
        public DbSet<Post> Posts { get; set; }
    }
}