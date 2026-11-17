-- Criação do Banco de Dados Legado da Alunos Tech
CREATE DATABASE AlunosTech_Legado;
GO

USE AlunosTech_Legado;
GO

-- Tabela de Alunos (Contém a falha proposital de senha em texto puro para análise crítica do PIM)
CREATE TABLE Alunos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Senha VARCHAR(255) NOT NULL,
    RA VARCHAR(50) NOT NULL
);
GO

-- Tabela de Cursos / Disciplinas
CREATE TABLE Cursos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(50) NOT NULL,
    Titulo VARCHAR(255) NOT NULL,
    Descricao TEXT,
    Professor VARCHAR(255) NOT NULL
);
GO

-- Tabela de Matrículas (Relacionamento entre Aluno e Curso com controle de progresso)
CREATE TABLE Matriculas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlunoId INT FOREIGN KEY REFERENCES Alunos(Id),
    CursoId INT FOREIGN KEY REFERENCES Cursos(Id),
    ProgressoPercentual INT DEFAULT 0
);
GO

-- Inserção de Dados Iniciais de Teste
INSERT INTO Alunos (Nome, Email, Senha, RA) 
VALUES ('Carlos Silva', 'carlos@alunostech.com', '123', '2026001');
GO

INSERT INTO Cursos (Codigo, Titulo, Descricao, Professor) VALUES 
('8125-60', 'Programação Aplicada em .NET', 'Desenvolvimento corporativo com C# e arquitetura modular.', 'Prof. Me. José Lorandi'),
('8130-60', 'Cloud Computing e DevOps', 'Infraestrutura em nuvem, Docker, Kubernetes e CI/CD.', 'Prof. Dra. Ana Costa'),
('8126-60', 'Programação de Banco de Dados', 'Procedures, Triggers, Transações e Otimização SQL.', 'Prof. Ricardo Mendes'),
('8127-60', 'Gerenciamento de Projetos Ágil', 'Scrum, Kanban, Sprints e Product Backlog.', 'Prof. Camila Rocha');
GO

INSERT INTO Matriculas (AlunoId, CursoId, ProgressoPercentual) VALUES 
(1, 1, 60),
(1, 2, 25);
GO