# Ecommerce API

API REST em **ASP.NET Core (.NET 10)** para gerenciar **categorias** e **produtos** de uma loja virtual, organizada em camadas (Controller → Service → Repository) com Entity Framework Core e SQLite.

## Conceitos aplicados

| Conceito | Onde está |
|----------|-----------|
| **Repository Pattern** | `Services/I*Repository.cs` (contratos) e `Data/*RepositoryEf.cs` (implementação) |
| **Camada de serviço** | `Services/*Service.cs` (regras de negócio) |
| **Injeção de dependência** | Construtores das classes + registros (`AddScoped`) no `Program.cs` |
| **Responsabilidade única (SRP)** | Cada classe tem uma função só: HTTP, regra, dados ou formato |
| **async / await** | Todos os métodos que acessam o banco retornam `Task` |
| **Entity Framework Core** | `Data/AppDbContext.cs` |
| **Relacionamento 1:N** | `Categoria` (1) → `Produto` (N) |
| **Migrations** | Pasta `Migrations/` |
| **DTOs** | Pasta `Dtos/` |

## Tecnologias

- ASP.NET Core / .NET 10
- Entity Framework Core + SQLite
- Swagger (Swashbuckle)

## Estrutura

```
Ecommerce/
├── Controllers/    # Recebem o HTTP e escolhem a resposta (200, 201, 400, 404...)
├── Services/       # Regras de negócio e contratos (interfaces)
├── Data/           # AppDbContext e repositórios com EF Core
├── Models/         # Entidades (viram tabelas)
├── Dtos/           # Objetos de entrada/saída da API + mapeamento
├── Migrations/     # Histórico de criação/alteração do banco
└── Program.cs      # Configuração e injeção de dependência
```

### Fluxo de uma requisição

```
HTTP ─▶ Controller ─▶ IService ─▶ IRepository ─▶ AppDbContext (EF Core) ─▶ SQLite
        (status HTTP)  (regras)    (contrato)
```

A resposta volta pelo mesmo caminho. Cada camada depende da seguinte por meio de uma **interface**, e o ASP.NET Core entrega a implementação (injeção de dependência).

## Modelo de dados

```
Categoria 1 ───< N Produto
```

- `Produto.CategoriaId` é a chave estrangeira (lado "N").
- Não é possível apagar uma categoria que ainda tenha produtos.

## Como executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git
- Ferramenta do EF Core (uma vez só): `dotnet tool install --global dotnet-ef`

### Passo a passo

```bash
# 1) Clonar
git clone <URL-DO-REPOSITORIO>

# 2) Entrar na pasta do projeto (a que contém o Ecommerce.csproj)
cd Ecommerce/Ecommerce

# 3) Criar o banco aplicando as migrations (gera o ecommerce.db)
dotnet ef database update

# 4) Rodar
dotnet run
```

No Visual Studio: abra o `Ecommerce.slnx`, rode `Update-Database` no *Console do Gerenciador de Pacotes* e pressione **F5**.

> O arquivo `ecommerce.db` não é versionado. É preciso rodar o `database update` na primeira vez; sem isso a API responde `no such table`.

Acesse o Swagger em:

- `https://localhost:7101/swagger`
- ou `http://localhost:5103/swagger`

## Endpoints

### Categorias — `/api/Categorias`

| Método | Endpoint | Descrição | Respostas |
|--------|----------|-----------|-----------|
| GET | `/api/Categorias` | Lista as categorias | 200 |
| GET | `/api/Categorias/{id}` | Busca uma categoria | 200, 404 |
| GET | `/api/Categorias/{id}/produtos` | Lista os produtos da categoria (1:N) | 200, 404 |
| POST | `/api/Categorias` | Cadastra uma categoria | 201, 400 |
| PUT | `/api/Categorias/{id}` | Atualiza uma categoria | 204, 400, 404 |
| DELETE | `/api/Categorias/{id}` | Remove uma categoria | 204, 400, 404 |

### Produtos — `/api/Produtos`

| Método | Endpoint | Descrição | Respostas |
|--------|----------|-----------|-----------|
| GET | `/api/Produtos` | Lista os produtos (filtro opcional `?categoriaId=`) | 200 |
| GET | `/api/Produtos/{id}` | Busca um produto | 200, 404 |
| POST | `/api/Produtos` | Cadastra um produto | 201, 400 |
| PUT | `/api/Produtos/{id}` | Atualiza um produto | 204, 400, 404 |
| DELETE | `/api/Produtos/{id}` | Remove um produto | 204, 404 |

## Exemplos

**Criar uma categoria**

```json
POST /api/Categorias
{
  "nome": "Eletrônicos",
  "descricao": "Celulares e acessórios"
}
```

**Criar um produto**

```json
POST /api/Produtos
{
  "nome": "Smartphone X",
  "descricao": "128 GB",
  "preco": 2499.90,
  "estoque": 15,
  "categoriaId": 1
}
```

Resposta (`201 Created`):

```json
{
  "id": 1,
  "nome": "Smartphone X",
  "descricao": "128 GB",
  "preco": 2499.9,
  "estoque": 15,
  "categoriaId": 1,
  "categoriaNome": "Eletrônicos"
}
```

## Regras de negócio

Ficam nos serviços. Quando uma regra é quebrada, o serviço lança `RegraDeNegocioException` e o controller responde `400 Bad Request` com a mensagem.

| Entidade | Regra |
|----------|-------|
| Categoria | O nome é único |
| Categoria | Não pode ser removida se tiver produtos |
| Produto | A categoria informada precisa existir |
| Produto | O nome não pode se repetir dentro da mesma categoria |

As validações de formato (`[Required]`, `[Range]`, `[StringLength]`) ficam nos DTOs e também resultam em `400`.

## Por que DTOs?

`Categoria` tem uma lista de `Produto` e cada `Produto` aponta de volta para `Categoria`. Devolver a entidade direto geraria um ciclo infinito no JSON. Por isso a API recebe `*Request` e devolve `*Response`, e o `Mapeamento.cs` faz a conversão (`ParaResponse()`).

## Migrations

```bash
# Criar uma nova migration após alterar um Model
dotnet ef migrations add NomeDaMigration

# Aplicar ao banco
dotnet ef database update
```
