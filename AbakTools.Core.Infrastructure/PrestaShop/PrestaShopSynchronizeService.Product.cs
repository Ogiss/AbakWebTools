using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Domain.Tax;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Entities.AuxEntities;
using Bukimedia.PrestaSharp.Lib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService
    {
        private void SynchronizeProduct()
        {
            SynchronizeStampEntity synchronizeStamp = null;
            DateTime stampTo = DateTime.Now;

            using (var uow = unitOfWorkProvider.CreateReadOnly())
            {
                synchronizeStamp = synchronizeStampRepository.Get(SynchronizeCodes.Product, SynchronizeDirectionType.Export);
            }

            DateTime stampFrom = synchronizeStamp?.DateTimeStamp ?? DateTime.MinValue;

            IReadOnlyCollection<ProductEntity> products = null;

            using (var uow = unitOfWorkProvider.CreateReadOnly())
            {
                products = productRepository.GetAllReady();
            }

            if (products.Any())
            {
                logger.LogDebug("Starting synchronize products");

                stampTo = products.Max(x => x.ModificationDate);

                foreach (var product in products)
                {
                    ProcessProduct(product);
                }

                if (synchronizeStamp == null)
                {
                    synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Product, SynchronizeDirectionType.Export);
                }

                synchronizeStamp.DateTimeStamp = stampTo;

                using (var uow = unitOfWorkProvider.Create())
                {
                    synchronizeStampRepository.SaveOrUpdate(synchronizeStamp);
                    uow.Commit();
                }

                logger.LogDebug($"Synchronize products finished at {(DateTime.Now - stampTo).TotalSeconds} sec.");
            }
        }

        private void ProcessProduct(ProductEntity product)
        {
            try
            {
                using (var uow = unitOfWorkProvider.Create())
                {
                    product = productRepository.Get(product.Id);
                    Bukimedia.PrestaSharp.Entities.product psProduct = null;

                    try
                    {

                        psProduct = product.WebId.HasValue && product.WebId > 0 ? prestaShopClient.ProductFactory.Get(product.WebId.Value) : null;
                    }
                    catch (Bukimedia.PrestaSharp.PrestaSharpException ex)
                    {
                        switch (ex.ResponseHttpStatusCode)
                        {
                            case System.Net.HttpStatusCode.NotFound:
                                product.WebId = null;
                                break;

                            default:
                                throw;
                        }
                    }

                    if (product.Synchronize == Framework.SynchronizeType.Deleted || product.NotWebAvailable)
                    {
                        if (psProduct != null)
                        {
                            psProduct.active = 0;
                        }
                    }
                    else
                    {

                        if (psProduct == null && product.Synchronize != Framework.SynchronizeType.Deleted)
                        {
                            psProduct = new Bukimedia.PrestaSharp.Entities.product();
                        }

                        psProduct.id_tax_rules_group = GetTaxRuleGroupId(product.Tax);
                        psProduct.price = product.Price;

                        var name = Functions.GetPrestaShopName(product.Name);

                        prestaShopClient.SetLangValue(psProduct, x => x.name, name);
                        prestaShopClient.SetLangValue(psProduct, x => x.link_rewrite, Functions.GetLinkRewrite(name));
                        prestaShopClient.SetLangValue(psProduct, x => x.description_short, Functions.GetPrestaShopDescriptionShort(product.DescriptionShort));
                        prestaShopClient.SetLangValue(psProduct, x => x.description, product.Description);

                        product = productRepository.Get(product.Id);

                        foreach (var category in product.Categories)
                        {
                            if (category.WebId.HasValue && !category.IsDeleted && category.Synchronize != Framework.SynchronizeType.Deleted)
                            {
                                try
                                {
                                    var psCategory = prestaShopClient.CategoryFactory.Get(category.WebId.Value);
                                    if (psCategory != null)
                                    {
                                        psProduct.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(psCategory.id.Value));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex.ToString());
                                }
                            }
                        }

                        psProduct.active = (product.Active && !product.IsDeleted) ? 1 : 0;
                        psProduct.state = 1;
                        psProduct.available_for_order = 1;
                        psProduct.position_in_category = 0;
                        psProduct.minimal_quantity = product.MinimumOrderQuantity > 0 ? product.MinimumOrderQuantity : 1;
                        psProduct.id_category_default = product.Categories.Where(x => x.WebId.HasValue && !x.IsDeleted &&
                            x.Synchronize != Framework.SynchronizeType.Deleted).Max(x => x.WebId);
                    }

                    if (psProduct != null)
                    {

                        if (psProduct.id.HasValue && psProduct.id.Value > 0)
                        {
                            logger.LogInformation($"Update product id: {product.Id}, name: {product.Name}");
                            prestaShopClient.ProductFactory.Update(psProduct);
                        }
                        else
                        {
                            logger.LogInformation($"Add new product id: {product.Id}, name: {product.Name}");
                            psProduct = prestaShopClient.ProductFactory.Add(psProduct);
                        }

                        product.WebId = (int)psProduct.id;
                    }


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

                    if (product.Synchronize == Framework.SynchronizeType.Deleted)
                    {
                        product.IsDeleted = true;
                    }

                    product.Synchronize = Framework.SynchronizeType.Synchronized;
                    product.IsReady = false;

                    productRepository.SaveOrUpdate(product);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Synchronize product Id: {product.Id} error.{Environment.NewLine}{ex}");
            }
        }

        private void SynchronizeImage(ProductEntity localProduct, Bukimedia.PrestaSharp.Entities.product psProduct, ImageEntity localImage)
        {
            Bukimedia.PrestaSharp.Entities.image psImage = null;

            if (localImage.WebId.HasValue)
            {
                if ((localImage.IsDeleted || localImage.Synchronize == Framework.SynchronizeType.Deleted) && psProduct.associations.images.Any(x => x.id == localImage.WebId))
                {
                    prestaShopClient.ImageFactory.DeleteProductImage(psProduct.id.Value, localImage.WebId.Value);
                    localImage.Synchronize = Framework.SynchronizeType.Synchronized;
                    localImage.IsDeleted = true;
                }
            }
            else if (localImage.ImageBytes != null && localImage.ImageBytes.Length > 0
                && localImage.IsDeleted == false && localImage.Synchronize != Framework.SynchronizeType.Deleted)
            {
                localImage.WebId = (int)prestaShopClient.ImageFactory.AddProductImage((long)psProduct.id, localImage.ImageBytes);
            }
        }

        private long? GetTaxRuleGroupId(TaxEntity tax)
        {
            if (!tax.WebId.HasValue)
            {
                var psTax = prestaShopClient.GetTaxRuleGroupByRate(tax.Rate);

                if (psTax != null)
                {
                    tax.WebId = (int)psTax.id.Value;
                    taxRepository.SaveOrUpdate(tax);
                }

                return psTax?.id;
            }

            return tax.WebId;
        }

    }
}
