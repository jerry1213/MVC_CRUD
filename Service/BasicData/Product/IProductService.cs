using Model.CommonModels;
using Service.Models.BasicData;

namespace Service.BasicData
{
    public interface IProductService
    {
        public Task<ResponseModel<IEnumerable<RespProductServiceModel>>> QueryProductAsync(ReqProductServiceModel reqModel);
        public Task<ResponseModel<RespProductServiceModel>> QueryProductByIdAsync(int productId);
        public Task<ResponseModel<bool>> CreateProductAsync(ReqCreateProductServiceModel reqModel);
        public Task<ResponseModel<bool>> UpdateProductAsync(ReqUpdateProductServiceModel reqModel);
        public Task<ResponseModel<bool>> DeleteProductAsync(int productId);
    }
}
