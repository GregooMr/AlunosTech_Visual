using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlunosTech.Visual.Data;
using AlunosTech.Visual.Models;

namespace AlunosTech.Visual.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        ViewBag.Cursos = await _context.Cursos.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Entrar(string email, string senha)
    {
        var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.Email == email && a.Senha == senha);
        if (aluno == null)
        {
            TempData["Erro"] = "E-mail ou senha inválidos! (Teste com: carlos@alunostech.com / 123)";
            return RedirectToAction(nameof(Login));
        }

        HttpContext.Session.SetInt32("AlunoId", aluno.Id);
        HttpContext.Session.SetString("AlunoNome", aluno.Nome);
        HttpContext.Session.SetString("AlunoRA", aluno.RA);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarEMatricular(string nome, string email, string senha, int cursoId)
    {
        if (await _context.Alunos.AnyAsync(a => a.Email == email))
        {
            TempData["Erro"] = "Este e-mail já está cadastrado no sistema!";
            return RedirectToAction(nameof(Login));
        }

        var novoAluno = new Aluno
        {
            Nome = nome,
            Email = email,
            Senha = senha,
            RA = "2026" + new Random().Next(100, 999)
        };

        _context.Alunos.Add(novoAluno);
        await _context.SaveChangesAsync();

        _context.Matriculas.Add(new Matricula { AlunoId = novoAluno.Id, CursoId = cursoId, ProgressoPercentual = 0 });
        await _context.SaveChangesAsync();

        HttpContext.Session.SetInt32("AlunoId", novoAluno.Id);
        HttpContext.Session.SetString("AlunoNome", novoAluno.Nome);
        HttpContext.Session.SetString("AlunoRA", novoAluno.RA);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        var alunoId = HttpContext.Session.GetInt32("AlunoId");
        if (alunoId == null) return RedirectToAction(nameof(Login));

        var matriculas = await _context.Matriculas
            .Include(m => m.Curso)
            .Where(m => m.AlunoId == alunoId)
            .ToListAsync();

        ViewBag.TodosCursos = await _context.Cursos.ToListAsync();
        ViewBag.AlunoNome = HttpContext.Session.GetString("AlunoNome");
        ViewBag.AlunoRA = HttpContext.Session.GetString("AlunoRA");

        return View(matriculas);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarCurso(int cursoId)
    {
        var alunoId = HttpContext.Session.GetInt32("AlunoId");
        if (alunoId == null) return RedirectToAction(nameof(Login));

        bool jaMatriculado = await _context.Matriculas.AnyAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
        if (jaMatriculado)
        {
            TempData["Aviso"] = "Você já está matriculado nesta disciplina!";
        }
        else
        {
            _context.Matriculas.Add(new Matricula { AlunoId = alunoId.Value, CursoId = cursoId, ProgressoPercentual = 0 });
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Matrícula realizada com sucesso!";
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> SalaDeAula(int id)
    {
        var alunoId = HttpContext.Session.GetInt32("AlunoId");
        if (alunoId == null) return RedirectToAction(nameof(Login));

        var matricula = await _context.Matriculas
            .Include(m => m.Curso)
            .FirstOrDefaultAsync(m => m.Id == id && m.AlunoId == alunoId);

        if (matricula == null) return RedirectToAction(nameof(Index));

        ViewBag.AlunoNome = HttpContext.Session.GetString("AlunoNome");
        return View(matricula);
    }

    [HttpPost]
    public async Task<IActionResult> ConcluirAtividade(int matriculaId, int incremento)
    {
        var matricula = await _context.Matriculas.FindAsync(matriculaId);
        if (matricula != null)
        {
            matricula.ProgressoPercentual = Math.Min(100, matricula.ProgressoPercentual + incremento);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(SalaDeAula), new { id = matriculaId });
    }

    public IActionResult Bibliotecas()
    {
        ViewBag.AlunoNome = HttpContext.Session.GetString("AlunoNome");
        return View();
    }

    public IActionResult MuralDoAluno()
    {
        ViewBag.AlunoNome = HttpContext.Session.GetString("AlunoNome");
        return View();
    }

    public IActionResult Tutoriais()
    {
        ViewBag.AlunoNome = HttpContext.Session.GetString("AlunoNome");
        return View();
    }

    public IActionResult Sair()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}