using AbakTools.Core.Domain.Product;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using PsProductEntity = Bukimedia.PrestaSharp.Entities.product;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService
    {
        private void SynchronizeProductImages(ProductEntity product, PsProductEntity psProduct)
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

                    foreach (var image in product.Images.Where(x=>x.IsDeleted == false))
                    {
                        SynchronizeImage(product, psProduct, image);
                    }

                    SaveOrUpdatePsProduct(psProduct, product);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Synchronize product Id: {product.Id} error.{Environment.NewLine}{ex}");
            }
        }

        private void SynchronizeImage(ProductEntity localProduct, PsProductEntity psProduct, ImageEntity localImage)
        {
            if(localImage.WebId.HasValue && psProduct.associations.images.All(x=>x.id != localImage.WebId))
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

        private void DeleteImage(ImageEntity localImage, PsProductEntity psProduct)
        {
            if (psProduct.associations.images.Any(x => x.id == localImage.WebId))
            {
                DeletePsImage(psProduct.id.Value, localImage.WebId.Value);
            }
            localImage.Synchronize = Framework.SynchronizeType.Synchronized;
            localImage.IsDeleted = true;
        }

        private void UpdateImage(ImageEntity localImage, PsProductEntity psProduct)
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

        private int AddImage(ImageEntity localImage, PsProductEntity psProduct)
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

        private void UpdatePsImage(ImageEntity localImage, PsProductEntity psProduct)
        {
            DeletePsImage((long)psProduct.id, localImage.WebId.Value);
            localImage.WebId = AddImage(localImage, psProduct);
        }
    }
}
