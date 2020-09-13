using AbakTools.Core.Domain.Product;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Cryptography;
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
                    foreach (var image in product.Images)
                    {
                        SynchronizeImage(product, psProduct, image);
                    }

                    if (psProduct.associations.images.Count != product.Images.Count)
                    {
                        foreach (var psImage in psProduct.associations.images)
                        {
                            if (!product.Images.Any(x => x.WebId.HasValue && x.WebId == psImage.id))
                            {
                                prestaShopClient.ImageFactory.DeleteProductImage(psProduct.id.Value, psImage.id);
                            }
                        }
                    }

                    SaveOrUpdatePsProduct(psProduct, product);
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Synchronize product Id: {product.Id} error.{Environment.NewLine}{ex}");
            }
        }

        private void SynchronizeImage(ProductEntity localProduct, PsProductEntity psProduct, ImageEntity localImage)
        {
            if (localImage.WebId.HasValue && localImage.WebId > 0)
            {
                if (localImage.IsArchived)
                {
                    DeleteImage(localImage, psProduct);
                }
                else if(localImage.IsSynchronizable)
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
                prestaShopClient.ImageFactory.DeleteProductImage(psProduct.id.Value, localImage.WebId.Value);
            }
            localImage.Synchronize = Framework.SynchronizeType.Synchronized;
            localImage.IsDeleted = true;
        }

        private void UpdateImage(ImageEntity localImage, PsProductEntity psProduct)
        {
            prestaShopClient.ImageFactory.UpdateProductImage((long)psProduct.id, localImage.WebId.Value, localImage.ImageBytes);
        }

        private int AddImage(ImageEntity localImage, PsProductEntity psProduct)
        {
            return (int)prestaShopClient.ImageFactory.AddProductImage((long)psProduct.id, localImage.ImageBytes);
        }
    }
}
