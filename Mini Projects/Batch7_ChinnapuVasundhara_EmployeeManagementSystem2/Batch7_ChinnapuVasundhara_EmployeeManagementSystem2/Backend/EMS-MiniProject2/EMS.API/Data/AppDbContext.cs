using EMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure Email is unique
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Ensure Username is unique
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Seed initial Admin and Viewer accounts (Passwords must be hashed in reality, 
            // but for seeding, we will generate the hashes. Admin password: 'admin123', Viewer: 'viewer123')
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = "Admin", CreatedAt = DateTime.UtcNow },
                new AppUser { Id = 2, Username = "viewer", PasswordHash = BCrypt.Net.BCrypt.HashPassword("viewer123"), Role = "Viewer", CreatedAt = DateTime.UtcNow }
            );

            // Seed Employees (Taking a few from your data.js as an example)
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Raja", LastName = "Chinnapu", Email = "rajak@gmail.com", Phone = "9376543210", Department = "Engineering", Designation = "Software Engineer", Salary = 580000, JoinDate = new DateTime(2021, 3, 15), Status = "Active" },
                new Employee { Id = 2, FirstName = "Roopa", LastName = "Chinnapu", Email = "roopam@gmail.com", Phone = "9523456780", Department = "Marketing", Designation = "Marketing Exec", Salary = 760000, JoinDate = new DateTime(2020, 7, 1), Status = "InActive" },
                new Employee { Id = 4, FirstName = "Vishnu", LastName = "Kasireddy", Email = "vishnu@gmail.com", Phone = "9676512340", Department = "HR", Designation = "HR Executive", Salary = 850000, JoinDate = new DateTime(2019, 11, 20), Status = "Active" },
                new Employee { Id = 5, FirstName = "Srinath", LastName = "Venkata", Email = "venkata@gmail.com", Phone = "9576511340", Department = "HR", Designation = "HR Executive", Salary = 950000, JoinDate = new DateTime(2019, 5, 12), Status = "InActive" },
                new Employee { Id = 6, FirstName = "keerthi", LastName = "Renigunta", Email = "rana@gmail.com", Phone = "8576512341", Department = "HR", Designation = "HR Executive", Salary = 750000, JoinDate = new DateTime(2019, 9, 22), Status = "InActive" },
                new Employee { Id = 7, FirstName = "Yamini", LastName = "Kanuma", Email = "keerati@gmail.com", Phone = "9076512330", Department = "Engineering", Designation = "Junior Developer", Salary = 650000, JoinDate = new DateTime(2022, 4, 10), Status = "Active" },
                new Employee { Id = 8, FirstName = "Kumar", LastName = "Maruthi", Email = "maruthi@gmail.com", Phone = "9116512342", Department = "Marketing", Designation = "Agent Operator", Salary = 850000, JoinDate = new DateTime(2022, 5, 20), Status = "InActive" },
                new Employee { Id = 9, FirstName = "Manish", LastName = "Thamatam", Email = "tamatam@gmail.com", Phone = "9644452340", Department = "HR", Designation = "HR Executive", Salary = 550000, JoinDate = new DateTime(2020, 10, 22), Status = "Active" },
                new Employee { Id = 10, FirstName = "Prakash", LastName = "Hamsa", Email = "hamsa@gmail.com", Phone = "9111512340", Department = "HR", Designation = "HR Executive", Salary = 850000, JoinDate = new DateTime(2022, 6, 21), Status = "Active" },
                new Employee { Id = 11, FirstName = "Suresh", LastName = "Katmurre", Email = "katmurre@gmail.com", Phone = "9871512380", Department = "Engineering", Designation = "Senior Developer", Salary = 800000, JoinDate = new DateTime(2020, 9, 23), Status = "InActive" },
                new Employee { Id = 12, FirstName = "Vijay", LastName = "Mavilla", Email = "mavilla@gmail.com", Phone = "9116512340", Department = "HR", Designation = "HR Executive", Salary = 750000, JoinDate = new DateTime(2021, 8, 15), Status = "Active" },
                new Employee { Id = 13, FirstName = "Naresh", LastName = "Thappeta", Email = "thappeta@gmail.com", Phone = "9676512340", Department = "HR", Designation = "HR Executive", Salary = 850000, JoinDate = new DateTime(2019, 11, 20), Status = "Active" },
                new Employee { Id = 14, FirstName = "Santhosh", LastName = "Penderi", Email = "penderi@gmail.com", Phone = "9676512340", Department = "HR", Designation = "HR Executive", Salary = 850000, JoinDate = new DateTime(2019, 11, 20), Status = "Active" },
                new Employee { Id = 15, FirstName = "Padhhu", LastName = "Jitta", Email = "jitta@gmail.com", Phone = "9676512340", Department = "HR", Designation = "HR Executive", Salary = 850000, JoinDate = new DateTime(2019, 11, 20), Status = "Active" }


           
            );
        }
    }
}