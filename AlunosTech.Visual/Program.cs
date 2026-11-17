using Microsoft.EntityFrameworkCore;
using AlunosTech.Visual.Data;
using AlunosTech.Visual.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Necessário para manter o aluno logado

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Recria o banco legado caso a estrutura anterior não tenha a coluna Senha/RA
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    if (!db.Alunos.Any())
    {
        var aluno1 = new Aluno { Nome = "Carlos Silva", Email = "carlos@alunostech.com", Senha = "123", RA = "2026001" };
        db.Alunos.Add(aluno1);

        var c1 = new Curso { Codigo = "8125-60", Titulo = "Programação Aplicada em .NET", Descricao = "Desenvolvimento corporativo com C# e arquitetura modular.", Professor = "Prof. Me. José Lorandi" };
        var c2 = new Curso { Codigo = "8130-60", Titulo = "Cloud Computing e DevOps", Descricao = "Infraestrutura em nuvem, Docker, Kubernetes e CI/CD.", Professor = "Prof. Dra. Ana Costa" };
        var c3 = new Curso { Codigo = "8126-60", Titulo = "Programação de Banco de Dados", Descricao = "Procedures, Triggers, Transações e Otimização SQL.", Professor = "Prof. Ricardo Mendes" };
        var c4 = new Curso { Codigo = "8127-60", Titulo = "Gerenciamento de Projetos Ágil", Descricao = "Scrum, Kanban, Sprints e Product Backlog.", Professor = "Prof. Camila Rocha" };

        db.Cursos.AddRange(c1, c2, c3, c4);
        db.SaveChanges();

        db.Matriculas.AddRange(
            new Matricula { AlunoId = aluno1.Id, CursoId = c1.Id, ProgressoPercentual = 60 },
            new Matricula { AlunoId = aluno1.Id, CursoId = c2.Id, ProgressoPercentual = 25 }
        );
        db.SaveChanges();
    }
}

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();