using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models;

namespace Portfolio.Entity
{
    public class DataContext : DbContext
    {
        //DataContext is your database
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        //The following represent your tables in the database
        //public DbSet<__Model Name__> __LocalObjectName__ { get; set; }
        public DbSet<Admin> Admins { get; set; }
        

    }
}
