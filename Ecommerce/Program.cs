using Ecommerce.Data;
using Ecommerce.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registra os controllers (as classes que recebem as requisições HTTP)
            builder.Services.AddControllers();

            // Registra o AppDbContext e diz que o banco é SQLite,
            // usando a connection string do appsettings.json
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---------- Injeção de dependência ----------
            // Leia como: "quando alguém pedir a INTERFACE, entregue esta IMPLEMENTAÇÃO".
            // Scoped: um objeto por requisição HTTP. Os repositórios da mesma requisição
            // recebem o MESMO AppDbContext.

            // Repositórios (acesso a dados): para trocar de banco, só estas linhas mudam
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepositoryEf>();
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepositoryEf>();

            // Serviços (regras de negócio)
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IProdutoService, ProdutoService>();

            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
