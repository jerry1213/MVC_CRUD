using Microsoft.AspNetCore.Mvc;
using Model.CommonModels;
using Omu.ValueInjecter;
using Platform.LogHelper;
using Platform.Utility;
using Service.BasicData;
using Service.Models.BasicData;
using YungChing_MVC.Models.BasicData;
using YungChing_MVC.Validations;

namespace YungChing_MVC.Controllers.BasicData
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly LogService<ProductController> _logService;
        private readonly IProductService _productService;
        private readonly CommonValidator _commonValidator;

        public ProductController(LogService<ProductController> logService,
            IProductService productService,
            CommonValidator commonValidator)

        {
            _logService = logService;
            _productService = productService;
            _commonValidator = commonValidator;
        }

        /// <summary>
        /// Call View
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有Product
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns>Data:所有Product資料</returns>
        [HttpGet(Name = nameof(QueryProductAll))]
        public async Task<IActionResult>QueryProductAll([FromQuery] ReqProductViewModel reqModel)
        {
            var serviceResult = await _productService.QueryProductAsync(Mapper.Map<ReqProductServiceModel>(reqModel));
            var apiRes = new ResponseModel<RespProductViewModel>()
            {
                IsOk = serviceResult.IsOk,
                //Data = serviceResult.Data?.Select(s => Mapper.Map<RespProductViewModel>(s)),
                Message = serviceResult.Message
            };

            if (apiRes.Data != null)
            {
                apiRes.Data = Mapper.Map<RespProductViewModel>(serviceResult.Data);
            }
            return Ok(apiRes);
        }

        /// <summary>
        /// 取得指定Id Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Data:指定Product資料</returns>
        [HttpGet("{productId}", Name = nameof(QueryProductById))]
        public async Task<IActionResult> QueryProductById(int productId)
        {
            var serviceResult = await _productService.QueryProductByIdAsync(productId);
            var apiRes = new ResponseModel<RespProductViewModel>()
            {
                IsOk = serviceResult.IsOk,
                Message = serviceResult.Message
            };
            if (serviceResult.Data != null)
            {
                apiRes.Data = Mapper.Map<RespProductViewModel>(serviceResult.Data);
            }
            return Ok(apiRes);
        }

        /// <summary>
        /// 新增BU
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns>Data:異動是否成功</returns>
        [HttpPost(Name = nameof(CreateProduct))]
        public async Task<IActionResult>CreateProduct(ReqCreateProductViewModel reqModel)
        {
            var (isValid, errorMessages) = _commonValidator.GetValidationResults(reqModel);
            var apiRes = new ResponseModel<bool>();
            if (!isValid)
            {
                apiRes.Message = errorMessages;
            }
            else
            {
                apiRes = await _productService.CreateProductAsync(Mapper.Map<ReqCreateProductServiceModel>(reqModel));
            }
            return Ok(apiRes);
        }
        /// <summary>
        /// 更新Product
        /// </summary>
        /// <param name="reqModel"></param>
        /// <returns>Data:異動是否成功</returns>
        [HttpPut(Name = nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct(ReqUpdateProductViewModel reqModel)
        {
            var (isValid, errorMessages) = _commonValidator.GetValidationResults(reqModel);
            var apiRes = new ResponseModel<bool>();
            if (!isValid)
            {
                apiRes.Message = errorMessages;
            }
            else
            {
                apiRes = await _productService.UpdateProductAsync(Mapper.Map<ReqUpdateProductServiceModel>(reqModel));
            }
            return Ok(apiRes);
        }

        /// <summary>
        /// 刪除指定Id Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Data:異動是否成功</returns>
        [HttpDelete("{productId}", Name = nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var serviceResult = await _productService.DeleteProductAsync(productId);
            return Ok(serviceResult);
        }

    }
}
