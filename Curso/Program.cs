using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // using var db = new Data.ApplicationContext();

            // // db.Database.Migrate();
            // var existe = db.Database.GetPendingMigrations().Any();
            // if(existe)
            // {
            //     // 
            // }

            // Console.WriteLine("Hello World!");

            // InserirDados();
            // InserirDadosEmMassa();
            // ConsultarDados();
            // CadastrarPedido();
            // ConsultarPedidoCarregamentoAdiantado();
            // AtualizarDados();
            RemoverDados();
        }

        private static void RemoverDados()
        {
            using var db = new Data.ApplicationContext();
            // var cliente  = db.Clientes.Find(2);

            var cliente = new Cliente
            {
                Id = 3,
            };

            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            // var cliente = db.Clientes.Find(1);
            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new
            {
                Nome = "Yuri Alberto",
                Telefone = "7966669999"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            db.SaveChanges();
            
        }
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db.Pedidos
                                .Include(p => p.Itens)
                                    .ThenInclude(p => p.Produto)
                                .ToList();

            Console.WriteLine($"Quantidade de pedidos: {pedidos.Count}");
        }

        private static void CadastrarPedido()
        {
            using var db =  new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }    
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db =  new Data.ApplicationContext();
            // var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();
            var consultaPorMetodo = db.Clientes.AsNoTracking().Where(p => p.Id > 0).ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                db.Clientes.Find(cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Andre Inacio",
                CEP = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "00999998888",
            };
            var cliente1 = new Cliente
            {
                Nome = "Teste 1",
                CEP = "99999000",
                Cidade = "Marajuara",
                Estado = "SE",
                Telefone = "00999998888",
            };
            var cliente2 = new Cliente
            {
                Nome = "Teste 2",
                CEP = "99999000",
                Cidade = "Pindamoiangaba",
                Estado = "SE",
                Telefone = "00999998888",
            };

            using var db =  new Data.ApplicationContext();
            db.AddRange(produto, cliente, cliente1, cliente2);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total de registros: {registros}");
        }


        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            // db.Produtos.Add(produto);
            // db.Set<Produto>().Add(produto);
            // db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de Registros: {registros}");
        }
    }
}
