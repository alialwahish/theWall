using Microsoft.EntityFrameworkCore;

    namespace Wall
    {
        public class MyContext : DbContext{
            public MyContext(DbContextOptions<MyContext> options) :base(options){ }

            public DbSet<User> users {get;set;}
            public DbSet<messages> messages {get;set;}
        }

    }