using AbakTools.Core.Domain.Product;
using AbakTools.Core.Framework.Domain;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Bukimedia.PrestaSharp.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services.Implementations
{
    class ProductImageSynchronizeService : IProductImageSynchronizeService
    {
        private readonly ILogger _logger;
        private readonly IPsProductRepository _psProductRepository;
        private readonly IPrestaShopClient _prestaShopClient;

        public ProductImageSynchronizeService(
            ILogger<ProductImageSynchronizeService> logger,
            IPrestaShopClient prestaShopClient,
            IPsProductRepository psProductRepository)
            => (_logger, _prestaShopClient, _psProductRepository) = (logger, prestaShopClient, psProductRepository);

        public product Synchronize(ProductEntity product, product psProduct)
        {
            try
            {
                if (psProduct != null)
                {
                    foreach (var psImage in psProduct.associations.images.ToList())
                    {
                        if (!product.Images.Any(x => x.WebId.HasValue && x.WebId == psImage.id))
                        {
                            _prestaShopClient.ImageFactory.DeleteProductImage(psProduct.id.Value, psImage.id);
                            psProduct.associations.images.Remove(psImage);
                        }
                    }

                    foreach (var image in product.Images.Where(x => x.IsDeleted == false))
                    {
                        SynchronizeImage(product, psProduct, image);
                    }

                    psProduct = _psProductRepository.SaveOrUpdate(psProduct);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Synchronize product Id: {product.Id} error.{Environment.NewLine}{ex}");
            }

            return psProduct;
        }

        private void SynchronizeImage(ProductEntity localProduct, product psProduct, ImageEntity localImage)
        {
            if (localImage.WebId.HasValue && psProduct.associations.images.All(x => x.id != localImage.WebId))
            {
                localImage.WebId = null;
            }

            if (localImage.WebId.HasValue && localImage.WebId > 0)
            {
                if (localImage.IsArchived)
                {
                    DeleteImage(localImage, psProduct);
                }
                else if (localImage.IsSynchronizable)
                {
                    UpdateImage(localImage, psProduct);
                }
            }
            else if (localImage.IsSynchronizable)
            {
                localImage.WebId = AddImage(localImage, psProduct);
            }
        }

        private void DeleteImage(ImageEntity localImage, product psProduct)
        {
            if (psProduct.associations.images.Any(x => x.id == localImage.WebId))
            {
                DeletePsImage(psProduct.id.Value, localImage.WebId.Value);
            }
            localImage.Synchronize = SynchronizeType.Synchronized;
            localImage.IsDeleted = true;
        }

        private void UpdateImage(ImageEntity localImage, product psProduct)
        {
            try
            {
                var image = _prestaShopClient.ImageFactory.GetProductImage((long)psProduct.id, localImage.WebId.Value);
            }
            catch
            {
                UpdatePsImage(localImage, psProduct);
            }
        }

        private int AddImage(ImageEntity localImage, product psProduct)
        {
            return (int)_prestaShopClient.ImageFactory.AddProductImage((long)psProduct.id, localImage.ImageBytes);
        }

        private void DeletePsImage(long psProductId, int psImageId)
        {
            try
            {
                _prestaShopClient.ImageFactory.DeleteProductImage(psProductId, psImageId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Delete PS product image error ({psProductId},{psImageId}).{Environment.NewLine}{ex}");
            }
        }

        private void UpdatePsImage(ImageEntity localImage, product psProduct)
        {
            DeletePsImage((long)psProduct.id, localImage.WebId.Value);
            localImage.WebId = AddImage(localImage, psProduct);
        }

    }
}
