using ApiCrud.Data;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

using System.Runtime.InteropServices;

namespace ApiCrud.Produtos
{
    public static class ProdutosRotas
    {
        public static void AddRotasProdutos(this WebApplication app)
        {
            var rotasProdutos = app.MapGroup("produtos");



            //Rota para Cadastro.
            rotasProdutos.MapPost("",
                async (AddProdutoRequest request, AppDbContext context) =>
            {
                var validarProduto = await context.Produtos
                .AnyAsync(produto => produto.Nome == request.nome);
                Console.WriteLine($"Valor da variavel:{validarProduto}");
                if (validarProduto)
                    return Results.Conflict("Já existe");

                var novoProduto = new Produto(request.nome);
                await context.Produtos.AddAsync(novoProduto);
                await context.SaveChangesAsync();

                var produtoRetorno = new ProdutoDto(novoProduto.Id, novoProduto.Nome);

                return Results.Ok(produtoRetorno);

            });


            //Rota para consultar todos os produtos
            rotasProdutos.MapGet("", async (AppDbContext context) => {
                var produtos = await context
                .Produtos                
                .Where(produto => produto.Ativo)
                .Select(produto => new ProdutoDto(produto.Id, produto.Nome))
                .ToListAsync();

                return produtos;

            });

            
            //Rota consultar um produto
            
            rotasProdutos.MapGet("{id:guid}",
                async (Guid id, AppDbContext context) =>
                {
                    var produto = await context
                    .Produtos
                    .Where(produto => produto.Ativo)
                    .SingleOrDefaultAsync(produto =>   produto.Id == id);
                    
                    if (produto == null)
                        return Results.NotFound();
                    
                   
                    return Results.Ok(produto);
                });
           
            //Rota Atualização
            rotasProdutos.MapPut("{id:guid}",
                async (Guid id,UpdateProdutoRequest request, AppDbContext context) =>
            {
                var produto = await context.Produtos
                .SingleOrDefaultAsync(produto => produto.Id == id);

                if (produto == null)
                    return Results.NotFound();

                produto.AtualizarNome(request.Nome);
                await context.SaveChangesAsync();
                return Results.Ok(new  ProdutoDto(produto.Id, produto.Nome));
             });


            rotasProdutos.MapDelete("{id}",
                async (Guid id, AppDbContext context) =>
                {
                    var produto = await context.Produtos
                    .SingleOrDefaultAsync(produto => produto.Id == id);
                    if (produto == null)
                        return Results.NotFound();


                    produto.Desativar();
                    await context.SaveChangesAsync();
                    return Results.Ok();
                });
        }


    }
}
