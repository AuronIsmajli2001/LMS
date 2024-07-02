
using LMS.Domain.Holidays;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Persistence.Holidays
{
    public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Date).IsRequired();
            builder.Property(h => h.Name).IsRequired().HasMaxLength(100);
        }
    }
}
