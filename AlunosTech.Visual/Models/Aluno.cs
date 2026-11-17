namespace AlunosTech.Visual.Models;

public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty; // Falha proposital do legado: sem hash!
    public string RA { get; set; } = string.Empty;
}