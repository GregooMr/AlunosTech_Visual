using Microsoft.EntityFrameworkCore;
using AlunosTech.Visual.Models;

namespace AlunosTech.Visual.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
}