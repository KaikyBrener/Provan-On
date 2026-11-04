# Provan-On

Sistema de provas online: API em ASP.NET Core (.NET 8) com MySQL e um front-end estático em HTML/JS.

## Estrutura

- `ApiProvaOnline/` — API REST (controllers de Administrativo, Aluno, Professor, Prova, Pergunta e respostas), com Swagger em `/swagger`.
- `Prova-On-Web/` — páginas HTML/JS do front-end (publicado na Vercel).
- `ApiProvaOnline/SQL TEXT BANCO` — script do banco.
- `Dockerfile` — imagem da API.

## Configuração

A string de conexão do MySQL **não fica no código**. Defina a variável de ambiente
`ConnectionStrings__Default` antes de subir a API:

```bash
# Linux/macOS
export ConnectionStrings__Default="Server=HOST;Port=3306;Database=ProvaOn;Uid=USUARIO;Pwd=SENHA"

# PowerShell
$env:ConnectionStrings__Default = "Server=HOST;Port=3306;Database=ProvaOn;Uid=USUARIO;Pwd=SENHA"
```

No Railway (ou outro serviço), cadastre a mesma variável nas configurações do serviço da API.
Sem a variável, a API falha com uma mensagem indicando o que falta.

## Como rodar

```bash
cd ApiProvaOnline/ApiProvaOnline
dotnet run
```

Swagger: `http://localhost:5150/swagger`.

Com Docker:

```bash
docker build -t provaon-api .
docker run -p 8080:8080 -e ConnectionStrings__Default="..." provaon-api
```

## Segurança

Nunca versione senhas, tokens ou strings de conexão. Se uma credencial já foi publicada, troque-a no provedor:
remover a linha do código não apaga o histórico do Git.
