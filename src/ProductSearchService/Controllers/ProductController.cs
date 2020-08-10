using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSearchService.Models;
using ProductSearchService.Services;

namespace ProductSearchService.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productService.GetAsync(id);
            if (product != null)
                return Ok(product);
            return NotFound();
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> Get(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return BadRequest($"Parameter {nameof(ids)} is empty.");

            IEnumerable<int> productIds = null;
            try
            {
                productIds = ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => int.Parse(i.Trim()))
                    .ToArray();
            }
            catch 
            {
                return BadRequest($"Parameter {nameof(ids)} is not in a correct format.");
            }
            
            var product = await _productService.GetByIdsAsync(productIds);
            if (product != null)
                return Ok(product);
            return NotFound();
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string q, [FromQuery] int take = 100)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest($"Parameter {nameof(q)} is empty.");

            return Ok(await _productService.SearchAsync(q, take));
        }
    }
}