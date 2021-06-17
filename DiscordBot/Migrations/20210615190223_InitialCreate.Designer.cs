﻿// <auto-generated />
using System;
using DiscordBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DiscordBot.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210615190223_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("DiscordBot.Entity.ReminderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reminder")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReminderTimesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ReminderEntities");
                });

            modelBuilder.Entity("DiscordBot.Entity.UploadOnlyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ServerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("UploadOnlyEntities");
                });
#pragma warning restore 612, 618
        }
    }
}