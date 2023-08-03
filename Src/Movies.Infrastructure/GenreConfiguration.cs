using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Core;

namespace Movies.Infrastructure
{
    public sealed class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
            builder.Property(d => d.description).HasMaxLength(1000);
            builder.Property(u => u.Code).IsRequired();
            builder.HasIndex(b => b.Code).IsUnique();

        }
    }
}
