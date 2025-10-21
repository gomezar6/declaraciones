using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Models
{
    public partial class DeclaracionesContext : DbContext
    {
        private static bool isInitialized;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeclaracionesContext(IHttpContextAccessor accessor, DbContextOptions<DeclaracionesContext> options)
            : base(options)
        {
            if (!isInitialized) { InitializeAzureKeyVaultProvider(); isInitialized = true; }
            _httpContextAccessor = accessor;

            static void InitializeAzureKeyVaultProvider()
            {
                SqlColumnEncryptionAzureKeyVaultProvider azureKeyVaultProvider = new(new DefaultAzureCredential());

                Dictionary<string, SqlColumnEncryptionKeyStoreProvider> providers = new()
                {
                    { SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider }
                };
                SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
            }
        }

        public virtual DbSet<Alerta> Alertas { get; set; } = null!;
        public virtual DbSet<Auditoria> Auditoria { get; set; } = null!;
        public virtual DbSet<Ciudad> Ciudades { get; set; } = null!;
        public virtual DbSet<DeclaracionFuncionarioCargos> DeclaracionFuncionarioCargos { get; set; } = null!;
        public virtual DbSet<Declaracion> Declaraciones { get; set; } = null!;
        public virtual DbSet<Disclaimer> Disclaimers { get; set; } = null!;
        public virtual DbSet<EstadoDeclaracion> EstadoDeclaraciones { get; set; } = null!;
        public virtual DbSet<EstadoProceso> EstadoProcesos { get; set; } = null!;
        public virtual DbSet<Familiar> Familiares { get; set; } = null!;
        public virtual DbSet<Formulario> Formularios { get; set; } = null!;
        public virtual DbSet<Funcionario> Funcionarios { get; set; } = null!;
        public virtual DbSet<FuncionarioNacionalidad> FuncionarioNacionalidad { get; set; } = null!;
        public virtual DbSet<Pais> Paises { get; set; } = null!;
        public virtual DbSet<CatalogoCargos> CatalogoCargos { get; set; } = null!;
        public virtual DbSet<CatalogoAnios> CatalogoAnios { get; set; } = null!;
        public virtual DbSet<Parentesco> Parentescos { get; set; } = null!;
        public virtual DbSet<Participacion> Participaciones { get; set; } = null!;
        public virtual DbSet<Proceso> Procesos { get; set; } = null!;
        public virtual DbSet<ProcesoDisclaimerFormulario> ProcesoDisclaimerFormularios { get; set; } = null!;
        public virtual DbSet<ProcesosFuncionario> ProcesosFuncionarios { get; set; } = null!;
        public virtual DbSet<Sincronizacion> Sincronizaciones { get; set; } = null!;
        public virtual DbSet<SincronizacionDetalle> SincronizacionDetalles { get; set; } = null!;
        public virtual DbSet<TipoDeclaracion> TipoDeclaraciones { get; set; } = null!;
        public virtual DbSet<TipoParticipacion> TipoParticipaciones { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alerta>(entity =>
            {
                entity.HasKey(e => e.IdAlerta);

                entity.Property(e => e.Asunto)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.HasOne(d => d.IdProcesoNavigation)
                    .WithMany(p => p.Alerta)
                    .HasForeignKey(d => d.IdProceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Alertas_Procesos");
            });

            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.Evento });

                entity.Property(e => e.Descripcion)
                .HasColumnType("varchar")
                .HasMaxLength(8000)
                .IsUnicode(false);

                entity.Property(e => e.Evento)
                    .HasMaxLength(300)
                    .IsFixedLength();

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.Ip)
                    .HasMaxLength(20)
                    .HasColumnName("IP")
                    .IsFixedLength();

                entity.Property(e => e.Resultado).IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.IdProcesoNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdProceso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auditoria_Procesos");
            });

            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.HasKey(e => e.IdCiudad);

                entity.Property(e => e.IdCiudad).ValueGeneratedNever();

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor).HasMaxLength(100);

                entity.Property(e => e.NombreCiudad).HasMaxLength(100);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Ciudades)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ciudades_Paises");
            });

            modelBuilder.Entity<DeclaracionFuncionarioCargos>(entity =>
            {
                entity.HasKey(de => new { de.IdDeclaracion, de.IdFuncionario });


                entity.Property(e => e.Cargo)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");


                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.UnidadOrganizacional)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.IdFuncionarioNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdFuncionario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Declaración_Funcionario_Cargo_Funcionarios");
            });

            modelBuilder.Entity<Declaracion>(entity =>
            {
                entity.HasKey(e => e.IdDeclaracion);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaDeclaracion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.Cargo)
                .HasColumnType("nvarchar(max)");

                entity.Property(e => e.UnidadOrganizacional)
               .HasMaxLength(50)
               .IsFixedLength();

                entity.HasOne(d => d.IdCiudadNavigation)
                        .WithMany(p => p.Declaraciones)
                        .HasForeignKey(d => d.IdCiudad)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Declaraciones_Ciudad");

                entity.HasOne(d => d.IdFuncionarioNavigation)
                     .WithMany(p => p.Declaraciones)
                     .HasForeignKey(d => d.IdFuncionario)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_Declaraciones_Funcionario");

                entity.HasOne(d => d.IdEstadoDeclaracionNavigation)
                    .WithMany(p => p.Declaraciones)
                    .HasForeignKey(d => d.IdEstadoDeclaracion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Declaraciones_Estado_Declaracion");

                entity.HasOne(d => d.IdFormularioNavigation)
                    .WithMany(p => p.Declaraciones)
                    .HasForeignKey(d => d.IdFormulario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Declaraciones_Formularios");
            });

            modelBuilder.Entity<Disclaimer>(entity =>
            {
                entity.HasKey(e => e.IdDisclaimer);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.IdProcesoNavigation)
                    .WithMany(p => p.Disclaimers)
                    .HasForeignKey(d => d.IdProceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Disclaimers_Procesos");
            });

            modelBuilder.Entity<EstadoDeclaracion>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PK_Estado_Declaracion");

                entity.ToTable("EstadoDeclaracion");

                entity.Property(e => e.Creado).HasColumnType("datetime");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(N'SUSER_NAME')")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombreEstadoDeclaracion)
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            modelBuilder.Entity<EstadoProceso>(entity =>
            {
                entity.HasKey(e => e.IdEstadoProceso)
                    .HasName("PK_Estado_Procesos");

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombreEstadoProceso)
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Familiar>(entity =>
            {

                entity.HasKey(de => new { de.IdFamiliar, de.IdFuncionario });

                entity.Property(e => e.ApellidoFamiliar).HasMaxLength(100);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor).HasMaxLength(100);

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor).HasMaxLength(100);

                entity.Property(e => e.NombreFamiliar).HasMaxLength(100);

                entity.HasOne(d => d.IdFuncionarioNavigation)
                    .WithMany(p => p.Familiares)
                    .HasForeignKey(d => d.IdFuncionario)
                    .HasConstraintName("FK_Familiares_Funcionarios");

                entity.HasOne(d => d.IdParentescoNavigation)
                    .WithMany(p => p.Familiares)
                    .HasForeignKey(d => d.IdParentesco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Familiares_Parentesco");
            });

            modelBuilder.Entity<Formulario>(entity =>
            {
                entity.HasKey(e => e.IdFormulario);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor).HasMaxLength(100);

                entity.HasOne(d => d.IdProcesoNavigation)
                    .WithMany(p => p.Formularios)
                    .HasForeignKey(d => d.IdProceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Formularios_Procesos");

                entity.HasOne(d => d.IdTipoDeclaracionNavigation)
                    .WithMany(p => p.Formularios)
                    .HasForeignKey(d => d.IdTipoDeclaracion)
                    .HasConstraintName("FK_Formularios_TipoDeclaracion");
            });

            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasKey(e => e.IdFuncionario);

                entity.Property(e => e.Apellidos).HasMaxLength(100);

                entity.Property(e => e.Cargo).HasMaxLength(400);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.Cup).HasColumnName("CUP");

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EstadoAusencia).HasMaxLength(100);

                entity.Property(e => e.FechaFinAusencia).HasColumnType("date");

                entity.Property(e => e.FechaIngreso).HasColumnType("date");

                entity.Property(e => e.FechaInicioAusencia).HasColumnType("date");

                entity.Property(e => e.LugarTrabajo).HasMaxLength(50);

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor).HasMaxLength(100);

                entity.Property(e => e.Nombres).HasMaxLength(100);

                entity.Property(e => e.Siglas).HasMaxLength(50);

                entity.Property(e => e.UnidadOrganizacional).HasMaxLength(400);

                entity.Property(e => e.Vicepresidencia).HasMaxLength(400);
            });
            modelBuilder.Entity<FuncionarioNacionalidad>(entity =>
            {

                entity.HasKey(de => new {de.Id, de.IdFuncionario, de.Nacionalidad, de.IdDeclaracion });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();


                entity.HasOne(d => d.PaisNavigation)
                .WithMany(p => p.FuncionarioNacionalidadNavigation)
                .HasForeignKey(d => d.Nacionalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FuncionarioNacionalidad_Pais");
            });

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombrePais)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.PresenciaCaf).HasColumnName("PresenciaCAF");
            });

            modelBuilder.Entity<Parentesco>(entity =>
            {
                entity.HasKey(e => e.IdParentesco);

                entity.ToTable("Parentesco");

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombreParentesco)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Participacion>(entity =>
            {
                entity.HasKey(e => e.IdParticipacion);

                entity.Property(e => e.ApellidoFamiliar).HasMaxLength(100);

                entity.Property(e => e.Cargo)
                    .HasMaxLength(50)
                    .UseCollation("Latin1_General_BIN2");

                entity.Property(e => e.dMesAnioInicio)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.ntipoCargo)
                    .HasMaxLength(100);

                entity.Property(e => e.bOtro)
                    .HasMaxLength(2);

        

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor).HasMaxLength(100);

                entity.Property(e => e.NombreEmpresa)
                    .HasMaxLength(50)
                    .UseCollation("Latin1_General_BIN2");

                entity.Property(e => e.NombreFamiliar).HasMaxLength(100);

                entity.Property(e => e.PctAccionario).HasColumnType("numeric(5, 2)");

                entity.HasOne(d => d.IdDeclaracionNavigation)
                    .WithMany(p => p.Participaciones)
                    .HasForeignKey(d => d.IdDeclaracion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participaciones_Declaraciones");

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Participaciones)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participaciones_Paises");

                entity.HasOne(d => d.IdParentescoNavigation)
                    .WithMany(p => p.Participaciones)
                    .HasForeignKey(d => d.IdParentesco)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Participaciones_Parentesco");
            });


            modelBuilder.Entity<Proceso>(entity =>
            {
                entity.HasKey(e => e.IdProceso);

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombreProceso)
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.HasOne(d => d.IdEstadoProcesoNavigation)
                    .WithMany(p => p.Procesos)
                    .HasForeignKey(d => d.IdEstadoProceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Procesos_Estado_Procesos");
            });

            modelBuilder.Entity<ProcesoDisclaimerFormulario>(entity =>
            {


                
                entity.HasKey(de => new { de.Id,  de.IdFormulario, de.IdDisclaimer, de.IdProceso });
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.HasOne(d => d.IdDisclaimerNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDisclaimer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proceso_Disclaimer_Formularios_Disclaimers");

                entity.HasOne(d => d.IdProcesoNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdProceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proceso_Disclaimer_Formularios_Procesos");
            });

            modelBuilder.Entity<ProcesosFuncionario>(entity =>
            {
                entity.HasKey(e => new { e.IdProceso, e.IdFuncionario });

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor).HasMaxLength(100);

                entity.HasOne(d => d.IdFuncionarioNavigation)
                    .WithMany(p => p.ProcesosFuncionarios)
                    .HasForeignKey(d => d.IdFuncionario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesosFuncionarios_Funcionarios");

                entity.HasOne(d => d.IdProcesoNavigation)
                    .WithMany(p => p.ProcesosFuncionarios)
                    .HasForeignKey(d => d.IdProceso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcesosFuncionarios_Procesos");
            });

            modelBuilder.Entity<Sincronizacion>(entity =>
            {
                entity.HasKey(e => e.IdSincronizacion);

                entity.ToTable("Sincronizacion");

                entity.Property(e => e.Creado).HasColumnType("datetime");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.EstatusSincronizacion)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.TipoProceso)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            modelBuilder.Entity<SincronizacionDetalle>(entity =>
            {
                entity.HasKey(e => e.IdSincronizacionDetalle);

                entity.ToTable("SincronizacionDetalle");

                entity.HasOne(d => d.IdSincronizacionNavigation)
                    .WithMany(p => p.SincronizacionDetalles)
                    .HasForeignKey(d => d.IdSincronizacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SincronizacionDetalles_Sincronizacion");
            });

            modelBuilder.Entity<TipoDeclaracion>(entity =>
            {
                entity.HasKey(e => e.IdTipo)
                    .HasName("PK_Tipo_Declaracion");

                entity.ToTable("TipoDeclaracion");

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombreDeclaracion)
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TipoParticipacion>(entity =>
            {
                entity.HasKey(e => e.IdParticipacion)
                    .HasName("PK_Tipo_Participaciones");

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreadoPor)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(suser_name())")
                    .IsFixedLength();

                entity.Property(e => e.Eliminado).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado).HasColumnType("datetime");

                entity.Property(e => e.ModificadoPor)
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.Property(e => e.NombreParticipacion)
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public override int SaveChanges()
        {
            FixEntities();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            FixEntities();

            return base.SaveChangesAsync(cancellationToken);
        }

        public void FixEntities()
        {
            //https://dev.to/rickystam/ef-core-how-to-implement-basic-auditing-on-your-entities-1mbm
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Auditable && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified
                || e.State == EntityState.Deleted));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    ((Auditable)entityEntry.Entity).Creado = DateTime.UtcNow;
                    ((Auditable)entityEntry.Entity).CreadoPor = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "PDLA APP";
                }
                else if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    ((Auditable)entityEntry.Entity).Eliminado = true;
                    Entry((Auditable)entityEntry.Entity).Property(p => p.Creado).IsModified = false;
                    Entry((Auditable)entityEntry.Entity).Property(p => p.CreadoPor).IsModified = false;
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    Entry((Auditable)entityEntry.Entity).Property(p => p.Creado).IsModified = false;
                    Entry((Auditable)entityEntry.Entity).Property(p => p.CreadoPor).IsModified = false;
                }

                // In any case we always want to set the properties
                // ModifiedAt and ModifiedBy
                ((Auditable)entityEntry.Entity).Modificado = DateTime.UtcNow;
                ((Auditable)entityEntry.Entity).ModificadoPor = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "PDLA APP";
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
