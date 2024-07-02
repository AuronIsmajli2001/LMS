using Microsoft.EntityFrameworkCore;
using LMS.Application.Interfaces;
using LMS.Domain.Employees;
using Microsoft.Extensions.Configuration;
using LMS.Domain.Holidays;
using LMS.Domain.LeaveAllocations;
using LMS.Domain.LeaveRequests;
using LMS.Domain.LeaveTypes;
using LMS.Persistence.Holidays;

namespace LMS.Persistence.Db
{
    public class DatabaseService : DbContext, IDatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(DbContextOptions<DatabaseService> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        public void Save()
        {
            base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("EcommercePersistence"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.LeaveRequests)
                .WithOne(lr => lr.RequestingEmployee)
                .HasForeignKey(lr => lr.RequestingEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.LeaveAllocations)
                .WithOne(la => la.Employee)
                .HasForeignKey(la => la.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.ApprovedLeaveRequests)
                .WithOne(lr => lr.ApprovedBy)
                .HasForeignKey(lr => lr.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveType>()
                .HasMany(lt => lt.LeaveRequests)
                .WithOne(lr => lr.LeaveType)
                .HasForeignKey(lr => lr.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveType>()
                .HasMany(lt => lt.LeaveAllocations)
                .WithOne(la => la.LeaveType)
                .HasForeignKey(la => la.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.RequestingEmployee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.RequestingEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.ApprovedBy)
                .WithMany(e => e.ApprovedLeaveRequests)
                .HasForeignKey(lr => lr.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.LeaveType)
                .WithMany(lt => lt.LeaveRequests)
                .HasForeignKey(lr => lr.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveAllocation>()
                .HasOne(la => la.Employee)
                .WithMany(e => e.LeaveAllocations)
                .HasForeignKey(la => la.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveAllocation>()
                .HasOne(la => la.LeaveType)
                .WithMany(lt => lt.LeaveAllocations)
                .HasForeignKey(la => la.LeaveTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new HolidayConfiguration());
        }
    }
}
