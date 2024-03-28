﻿// <auto-generated />
using System;
using FitnessApp2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FitnessApp2.Migrations
{
    [DbContext(typeof(FAppDbContext))]
    partial class FAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.CourseGuest", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "GuestId");

                    b.HasIndex("GuestId");

                    b.ToTable("CourseGuests");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.CourseInstructor", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("InstructorId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "InstructorId");

                    b.HasIndex("InstructorId");

                    b.ToTable("CourseInstructors");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DetailId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<byte>("Hours")
                        .HasColumnType("tinyint");

                    b.Property<int?>("InstructorId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("SectionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DetailId");

                    b.HasIndex("InstructorId");

                    b.HasIndex("SectionId");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Instructor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte?>("ExperienceYears")
                        .HasColumnType("tinyint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.CourseGuest", b =>
                {
                    b.HasOne("FitnessApp2.Models.DbEntities.Course", "Course")
                        .WithMany("CourseGuests")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FitnessApp2.Models.DbEntities.Guest", "Guest")
                        .WithMany("CourseGuests")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.CourseInstructor", b =>
                {
                    b.HasOne("FitnessApp2.Models.DbEntities.Course", "Course")
                        .WithMany("CourseInstructors")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FitnessApp2.Models.DbEntities.Instructor", "Instructor")
                        .WithMany("CourseInstructors")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Guest", b =>
                {
                    b.HasOne("FitnessApp2.Models.DbEntities.Detail", "Detail")
                        .WithMany()
                        .HasForeignKey("DetailId");

                    b.HasOne("FitnessApp2.Models.DbEntities.Instructor", "Instructor")
                        .WithMany("Guests")
                        .HasForeignKey("InstructorId");

                    b.HasOne("FitnessApp2.Models.DbEntities.Section", "Section")
                        .WithMany("Guests")
                        .HasForeignKey("SectionId");

                    b.Navigation("Detail");

                    b.Navigation("Instructor");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Course", b =>
                {
                    b.Navigation("CourseGuests");

                    b.Navigation("CourseInstructors");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Guest", b =>
                {
                    b.Navigation("CourseGuests");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Instructor", b =>
                {
                    b.Navigation("CourseInstructors");

                    b.Navigation("Guests");
                });

            modelBuilder.Entity("FitnessApp2.Models.DbEntities.Section", b =>
                {
                    b.Navigation("Guests");
                });
#pragma warning restore 612, 618
        }
    }
}
