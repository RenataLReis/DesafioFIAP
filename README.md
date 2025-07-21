# 📚 Desafio FIAP - Secretaria Acadêmica

Este projeto é uma API RESTful desenvolvida em .NET 9 para gerenciar Alunos, Turmas e Matrículas, com autenticação via JWT, validações com FluentValidation e persistência em SQL Server.

---

## 🚀 Tecnologias Utilizadas

- [.NET 9](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [JWT (Json Web Token)](https://jwt.io/)
- [Swagger (OpenAPI)](https://swagger.io/)

---

## ⚙️ Requisitos

- [.NET SDK 9.0+](https://dotnet.microsoft.com/en-us/download)
- [SQL Server LocalDB](https://learn.microsoft.com/pt-br/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recomendado)

---

## 📦 Instalação

1. Clone o repositório:

```bash
git clone https://github.com/RenataLReis/DesafioFIAP.git
```

2. Crie um banco de dados local no SQL Server com o nome:

```
FIAP
```

3. Altere a `ConnectionString` no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=FIAP;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

4. Rode as migrations para criar o schema do banco:

```bash
dotnet ef database update --project Secretaria.Infrastructure --startup-project Secretaria.Api
```

> Obs: você também pode usar o Visual Studio > Package Manager Console.

---

## ▶️ Executando o Projeto

Execute a aplicação via terminal ou pelo Visual Studio (selecionando o projeto `Secretaria.Api` como projeto de inicialização):

```bash
dotnet run --project Secretaria.Api
```

---

## 📖 Acessando o Swagger

Caso seu Visual Studio não esteja configurado para abrir o browser automaticamente, após iniciar o projeto, abra o navegador em:

```
http://localhost:5246/swagger
```

---

## 🔐 Autenticação

- Todas as rotas protegidas exigem autenticação via JWT.
- Antes de usar os endpoints protegidos, você deve:

### 1. Cadastrar um administrador:

```
POST /auth/cadastrar-admin
```

```json
{
  "email": "admin@teste.com",
  "senha": "SenhaForte123!"
}
```

### 2. Gerar token:

```
POST /auth/gerar-token
```

```json
{
  "email": "admin@teste.com",
  "senha": "SenhaForte123!"
}
```

### 3. Inserir token no Swagger

- Clique em **Authorize**
- Digite apenas o token JWT (sem a palavra `Bearer`), pois o Swagger já a inclui automaticamente.

---

## 🥺 Testes

- Você pode testar todos os endpoints diretamente pelo Swagger.
- Os principais recursos disponíveis são:

### Aluno

- `POST /cadastrar-aluno`
- `PUT /atualizar-senha`
- `GET /alunos`

### Turma

- `POST /criar-turma`
- `GET /turmas/{id}`

### Matrícula

- `POST /matricular`
- `PUT /trancar-matricula`

---

## 📁 Estrutura de Pastas

```
/Secretaria.Api             -> Camada de apresentação
/Secretaria.Application     -> Casos de uso, DTOs, Validadores, Interfaces
/Secretaria.Domain          -> Entidades e contratos de domínio
/Secretaria.Infrastructure  -> Persistência de dados e Repositórios
```

---

## 👩‍💻 Autor(a)

Renata Reis  


