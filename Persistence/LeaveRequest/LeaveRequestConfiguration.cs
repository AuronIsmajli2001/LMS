using LMS.Domain.LeaveRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Persistence.LeaveRequests
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasKey(lr => lr.Id);
            builder.Property(lr => lr.RequestingEmployeeId).IsRequired();
            builder.Property(lr => lr.LeaveTypeId).IsRequired();
            builder.Property(lr => lr.StartDate).IsRequired();
            builder.Property(lr => lr.EndDate).IsRequired();
            builder.Property(lr => lr.Reason).HasMaxLength(500);
            builder.Property(lr => lr.Status).IsRequired();
            builder.Property(lr => lr.ApprovedBy).IsRequired();

            builder.HasOne(lr => lr.LeaveType)
                   .WithMany()
                   .HasForeignKey(lr => lr.LeaveTypeId);

            builder.HasOne(lr => lr.ApprovedBy)
                   .WithMany()
                   .HasForeignKey(lr => lr.ApprovedById);
        }
    }
}
