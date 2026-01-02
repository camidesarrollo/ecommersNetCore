using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Domain.Entities;
using Ecommers.Web.Filters;
using Ecommers.Web.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.Controllers
{
    [Route("Gestion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class ProductsController(
        IProducts ProductsService,
        IMasterAttributes MasterAttributesService,
        ICategorias CategoriasService,
        IImageStorage imageStorage,
        IMapper mapper,
        ILogger<ProductsController> logger) : BaseController
    {
        private readonly IProducts _Productservice = ProductsService;
        private readonly IMasterAttributes _MasterAttributeService = MasterAttributesService;
        private readonly ICategorias _CategoriasService = CategoriasService;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ProductsController> _logger = logger;
        // -------------------------------------------------------------------
        // GET: Vista principal
        // -------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Web/Views/Products/Index.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public async Task<IActionResult> CrearAsync()
        {
            var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();
            var Categorias = await _CategoriasService.GetAllActiveAsync();
            var ProductViewModel = new ProductsCreateViewModel
            {
                MasterAttributes = MaestroAtributes,
                Categories = Categorias
            };  
            return View("~/Web/Views/Products/Create.cshtml", ProductViewModel);
        }

    }
}
