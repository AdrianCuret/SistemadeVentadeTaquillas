﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VentadeTaquillas.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;

namespace VentadeTaquillas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Administrador>
    {

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Taquilla> Taquillas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }

        public DbSet<Asiento> Asientos { get; set; }

        public DbSet<Sala> Salas { get; set; }

        public DbSet<Cine> Cines { get; set; }

        public DbSet<Publicacion> Publicaciones { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Administrador>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Administrador");
                entityTypeBuilder.Property(u => u.Nombre).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Apellido).HasMaxLength(100);
                entityTypeBuilder.Property(u => u.Ciudad).HasMaxLength(100);

            });
        }


    }

        public class Administrador : IdentityUser
        {
            public Guid AdminId { get; set; }

            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Ciudad { get; set; }
        }


    public class Cliente
    {
        [Key]
        public int NumeroClienteId { get; set; }
        public Guid ClienteId { get; set; }

        public Guid PeliculaId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }

    }

        public class Taquilla
        {

        [Key]
        public int NumeroTaquillaId { get; set; }

        public Guid TaquillaId { get; set; }
        public Guid ClienteId { get; set; }
        [Required]
        public Guid AsientoId { get; set; }
        public Guid PeliculaId { get; set; }
        [Required]
        public Guid SalaId { get; set; }
        [Required]
        public Guid CineId { get; set; }
        }

    public class Sala
    {
        public Guid SalaId { get; set; }
        public string Nombre { get; set; }

        public Guid CineId { get; set; }

    }

    public class Asiento
    {
        public Guid AsientoId { get; set; }
        public int NumeroAsiento { get; set; }

        public string Estado { get; set; }

        public Guid SalaId { get; set; }

    }

    public class Cine
    {
        public Guid CineId { get; set; }
        public string NombreCine { get; set; }

    }

    public class Pelicula
    {
        public Guid PeliculaId { get; set; }
        public string NombrePeli { get; set; }
        public byte[] ImagenPeli { get; set; }


        public string Descripcion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime FechaPeli { get; set; }

        public int Valor { get; set; }
    }

    public class Publicacion
    {
        public Guid PublicacionId { get; set; }
        public string NombrePubliPeli { get; set; }

        public string Evento { get; set; }
        public byte[] ImagenPubliPeli { get; set; }


        public string Descripcion { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime FechaPeli { get; set; }

        public Guid PeliculaId { get; set; }
    }
}

