using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAPIBank.DesignPatterns.StrategyPattern.Init;
using WebAPIBank.Models.Entities;

namespace WebAPIBank.Models.Context
{
    public class MyContext : DbContext
    {
        public MyContext() : base("MyConnection")
        {
            Database.SetInitializer(new MyInit());
        }

        public DbSet<CardInformation> Cards { get; set; }
    }
}