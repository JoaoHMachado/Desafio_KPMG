using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAppPeopleCRUD.ModelsEF;

public partial class PessoasContext : DbContext
{
    public PessoasContext()
    {
    }

    public PessoasContext(DbContextOptions<PessoasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dependente> Dependedentes { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Pessoas;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dependente>(entity =>
        {
            entity.HasKey(e => e.IdDependente).HasName("PK__Depended__8867D8375B2F450F");

            entity.Property(e => e.IdadeDependente)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NomeDependente)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPessoaNavigation).WithMany(p => p.Dependentes)
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dependede__IdPes__398D8EEE");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.IdPessoa).HasName("PK__Pessoa__7061465DB03A73C4");

            entity.ToTable("Pessoa");

            entity.Property(e => e.IdadePessoa)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NomePessoa)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
