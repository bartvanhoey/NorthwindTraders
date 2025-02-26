﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Application.Products.Commands.CreateProduct;
using Northwind.Application.Products.Commands.DeleteProduct;
using Northwind.Application.Products.Commands.UpdateProduct;
using Northwind.Application.Products.Queries.GetAllProducts;
using Northwind.Application.Products.Queries.GetProduct;
using Microsoft.AspNetCore.Http;
using Northwind.Application.Products.Queries.GetProductsFile;

namespace Northwind.WebUI.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ProductsListViewModel>> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllProductsQuery()));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<FileResult> Download()
        {
            var vm = await Mediator.Send(new GetProductsFileQuery());

            return File(vm.Content, vm.ContentType, vm.FileName);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductQuery { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateProductCommand command)
        {
            var productId = await Mediator.Send(command);

            return Ok(productId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteProductCommand { Id = id });

            return NoContent();
        }
    }
}