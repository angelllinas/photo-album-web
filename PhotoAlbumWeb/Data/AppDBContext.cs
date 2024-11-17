using Microsoft.EntityFrameworkCore;
using PhotoAlbumWeb.Models;

namespace PhotoAlbumWeb.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options) 
        {

        }

        public DbSet<PhotoBoard> PhotoBoard {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhotoBoard>(entity => {

                entity.HasKey(colum => colum.Id); //Primary key

                entity.Property(colum => colum.Id)
                .ValueGeneratedOnAdd();// Configure the Id colum to autoincrement

                entity.Property(colum => colum.Photo)
                .IsRequired();

                entity.Property(colum => colum.PhotoDescription)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(colum => colum.EventDate)
                .IsRequired()
                .HasColumnType("DATE");

                entity.Property(colum => colum.Location)
                .IsRequired()
                .HasMaxLength(255);

                entity.Property(colum => colum.EventDescription)
                .IsRequired()
                .HasColumnType("TEXT");
            });

            modelBuilder.Entity<PhotoBoard>().ToTable("photo_board");
        }
    }
}
