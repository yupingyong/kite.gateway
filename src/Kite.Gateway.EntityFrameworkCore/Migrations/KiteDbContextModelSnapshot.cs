﻿// <auto-generated />
using System;
using Kite.Gateway.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace Kite.Gateway.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(KiteDbContext))]
    partial class KiteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.Sqlite)
                .HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.Administrator", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AdminName")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("NickName")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.AuthenticationConfigure", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Audience")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("CertificateFile")
                        .HasColumnType("TEXT");

                    b.Property<string>("CertificateFileName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("CertificatePassword")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("ClockSkew")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Issuer")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<bool>("RequireExpirationTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityKeyStr")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseSSL")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UseState")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ValidateAudience")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ValidateIssuer")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ValidateIssuerSigningKey")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ValidateLifetime")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AuthenticationConfigures");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.Cluster", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClusterName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadBalancingPolicy")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RouteId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceGovernanceName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("ServiceGovernanceType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.ClusterDestination", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ClusterDestinations");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.ClusterHealthCheck", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ClusterId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Interval")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Policy")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("Timeout")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ClusterHealthChecks");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.Middleware", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<int>("ExecWeight")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("Server")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<int>("SignalType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseState")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Middlewares");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AccessToken")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("NodeName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Server")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("RouteId")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("RouteMatchPath")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("RouteName")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseState")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.RouteTransform", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RouteId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransformsName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("TransformsValue")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RouteTransforms");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.ServiceGovernanceConfigure", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsulDatacenter")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsulServer")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("ConsulToken")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ServiceGovernanceConfigures");
                });

            modelBuilder.Entity("Kite.Gateway.Domain.Entities.Whitelist", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("FilterText")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("FilterType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestMethod")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RouteId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseState")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Whitelists");
                });
#pragma warning restore 612, 618
        }
    }
}
