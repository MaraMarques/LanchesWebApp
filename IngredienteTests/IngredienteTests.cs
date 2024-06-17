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
    public class IngredientesControllerTests
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

        private List<Ingrediente> GetTestIngredientes()
        {
            return new List<Ingrediente>
            {
                new Ingrediente { IngredienteId = 1, Nome = "Ingrediente1", Descricao = "Descricao1", Quantidade = 10, PrecoUni = 1 },
                new Ingrediente { IngredienteId = 2, Nome = "Ingrediente2", Descricao = "Descricao2", Quantidade = 5, PrecoUni = 2 }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithAListOfIngredientes()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Ingredientes.AddRange(GetTestIngredientes());
            context.SaveChanges();

            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Ingrediente>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIngredienteNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Details(3); // ID not in the test data

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithIngrediente()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Ingredientes.AddRange(GetTestIngredientes());
            context.SaveChanges();

            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Ingrediente>(viewResult.ViewData.Model);
            Assert.Equal("Ingrediente1", model.Nome);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new IngredientesController(context);

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsRedirectAndAddsIngrediente_WhenModelIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new IngredientesController(context);
            var newIngrediente = new Ingrediente
            {
                Nome = "Ingrediente3",
                Descricao = "Descricao3",
                Quantidade = 15,
                PrecoUni = 3
            };

            // Act
            var result = await controller.Create(newIngrediente);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var ingrediente = context.Ingredientes.FirstOrDefault(i => i.Nome == "Ingrediente3");
            Assert.NotNull(ingrediente);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithIngrediente()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Ingredientes.AddRange(GetTestIngredientes());
            context.SaveChanges();

            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Ingrediente>(viewResult.ViewData.Model);
            Assert.Equal("Ingrediente1", model.Nome);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithIngrediente()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Ingredientes.AddRange(GetTestIngredientes());
            context.SaveChanges();

            var controller = new IngredientesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Ingrediente>(viewResult.ViewData.Model);
            Assert.Equal("Ingrediente1", model.Nome);
        }
    }
}
