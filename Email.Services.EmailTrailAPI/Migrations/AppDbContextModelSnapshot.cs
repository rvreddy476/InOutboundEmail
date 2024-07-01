
using System;
using Email.Services.EmailTrailAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Email.Services.EmailTrailAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Email.Services.EmailTrailAPI.Models.EmailTrail", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Body")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int?>("ParentEmailId")
                    .HasColumnType("int");

                b.Property<string>("Recipient")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Sender")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("SentDate")
                    .HasColumnType("datetime2");

                b.Property<string>("Subject")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int?>("TicketDetailsId")
                    .HasColumnType("int");

                b.Property<string>("TicketId")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("Timestamp")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("ParentEmailId");

                b.HasIndex("TicketDetailsId");

                b.ToTable("EmailTrails");
            });

            modelBuilder.Entity("Email.Services.EmailTrailAPI.Models.TicketDetails", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("TicketId")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("TicketDetails");
            });

            modelBuilder.Entity("Email.Services.EmailTrailAPI.Models.EmailTrail", b =>
            {
                b.HasOne("Email.Services.EmailTrailAPI.Models.EmailTrail", "ParentEmail")
                    .WithMany("Replies")
                    .HasForeignKey("ParentEmailId");

                b.HasOne("Email.Services.EmailTrailAPI.Models.TicketDetails", null)
                    .WithMany("Emails")
                    .HasForeignKey("TicketDetailsId");

                b.Navigation("ParentEmail");
            });

            modelBuilder.Entity("Email.Services.EmailTrailAPI.Models.EmailTrail", b =>
            {
                b.Navigation("Replies");
            });

            modelBuilder.Entity("Email.Services.EmailTrailAPI.Models.TicketDetails", b =>
            {
                b.Navigation("Emails");
            });
#pragma warning restore 612, 618
        }
    }
}
