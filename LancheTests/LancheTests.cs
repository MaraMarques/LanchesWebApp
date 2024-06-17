using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanchesWebApp2.Controllers;
using LanchesWebApp2.Data;
using LanchesWebApp2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LanchesWebApp2.Tests
{
    public class LanchesControllerTests
    {
        private DbBitesContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DbBitesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DbBitesContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private List<Lanche> GetTestLanches()
        {
            return new List<Lanche>
            {
                new Lanche { LancheId = 1, Nome = "Lanche1", Descricao = "Descricao1", Preco = 10, IngredienteIds = new List<int> { 1 } },
                new Lanche { LancheId = 2, Nome = "Lanche2", Descricao = "Descricao2", Preco = 20, IngredienteIds = new List<int> { 2 } }
            };
        }

        private List<Ingrediente> GetTestIngredientes()
        {
            return new List<Ingrediente>
            {
                new Ingrediente { IngredienteId = 1, Nome = "Ingrediente1", Descricao = "Descricao1", Quantidade = 10, PrecoUni = 1 },
                new Ingrediente { IngredienteId = 2, Nome = "Ingrediente2", Descricao = "Descricao2", Quantidade = 5, PrecoUni = 2 }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithAListOfLanches()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Lanches.AddRange(GetTestLanches());
            context.SaveChanges();

            var controller = new LanchesController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Lanche>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new LanchesController(context);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenLancheNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new LanchesController(context);

            // Act
            var result = await controller.Details(3); // ID not in the test data

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithLanche()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Lanches.AddRange(GetTestLanches());
            context.SaveChanges();

            var controller = new LanchesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Lanche>(viewResult.ViewData.Model);
            Assert.Equal("Lanche1", model.Nome);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new LanchesController(context);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectAndAddsLanche_WhenModelIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Ingredientes.AddRange(GetTestIngredientes());
            context.SaveChanges();

            var controller = new LanchesController(context);
            var newLanche = new Lanche
            {
                Nome = "Lanche3",
                Descricao = "Descricao3",
                Preco = 30,
                IngredienteIds = new List<int> { 1, 2 }
            };

            // Act
            var result = await controller.Create(newLanche);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var lanche = context.Lanches.FirstOrDefault(l => l.Nome == "Lanche3");
            Assert.NotNull(lanche);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new LanchesController(context);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithLanche()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Lanches.AddRange(GetTestLanches());
            context.SaveChanges();

            var controller = new LanchesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Lanche>(viewResult.ViewData.Model);
            Assert.Equal("Lanche1", model.Nome);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new LanchesController(context);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithLanche()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Lanches.AddRange(GetTestLanches());
            context.LancheIngredientes.AddRange(new List<LancheIngrediente>
    {
        new LancheIngrediente { LancheId = 1, IngredienteId = 1 },
        new LancheIngrediente { LancheId = 2, IngredienteId = 2 }
    });
            context.Ingredientes.AddRange(GetTestIngredientes());
            context.SaveChanges();

            var controller = new LanchesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Lanche>(viewResult.ViewData.Model);
            Assert.Equal("Lanche1", model.Nome);
        }


        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex_AndRemovesLanche()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Lanches.AddRange(GetTestLanches());
            context.LancheIngredientes.AddRange(new List<LancheIngrediente>
            {
                new LancheIngrediente { LancheId = 1, IngredienteId = 1 },
                new LancheIngrediente { LancheId = 2, IngredienteId = 2 }
            });
            context.SaveChanges();

            var controller = new LanchesController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var lanche = context.Lanches.Find(1);
            Assert.Null(lanche);
        }
    }
}
