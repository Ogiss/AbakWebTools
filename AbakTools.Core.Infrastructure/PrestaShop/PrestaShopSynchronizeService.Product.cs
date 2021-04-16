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
using PsProduct = Bukimedia.PrestaSharp.Entities.product;


namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService
    {
        private void SynchronizeProduct()
        {
            SynchronizeStampEntity synchronizeStamp = null;
            DateTime stampTo = DateTime.Now;

            using (var uow = _unitOfWorkProvider.CreateReadOnly())
            {
                synchronizeStamp = _synchronizeStampRepository.Get(SynchronizeCodes.Product, SynchronizeDirectionType.Export);
            }

            DateTime stampFrom = synchronizeStamp?.DateTimeStamp ?? DateTime.MinValue;

            IReadOnlyCollection<ProductEntity> products = null;

            using (var uow = _unitOfWorkProvider.CreateReadOnly())
            {
                products = _productRepository.GetAllReady();
            }

            if (products.Any())
            {
                _logger.LogDebug("Starting synchronize products");

                foreach (var product in products)
                {
                    ProcessProduct(product);
                }

                if (synchronizeStamp == null)
                {
                    synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Product, SynchronizeDirectionType.Export);
                }

                synchronizeStamp.DateTimeStamp = products.Max(x => x.ModificationDate);

                using (var uow = _unitOfWorkProvider.Create())
                {
                    _synchronizeStampRepository.SaveOrUpdate(synchronizeStamp);
                    uow.Commit();
                }

                _logger.LogDebug($"Synchronize products finished at {(DateTime.Now - stampTo).TotalSeconds} sec.");
            }
        }

        private void ProcessProduct(ProductEntity product)
        {
            try
            {
                using (var uow = _unitOfWorkProvider.Create())
                {
                    product = _productRepository.Get(product.Id);
                    Bukimedia.PrestaSharp.Entities.product psProduct = GetPsProduct(product.WebId);

                    if (psProduct == null && product.WebId.HasValue)
                    {
                        product.WebId = null;
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

                        if (psProduct != null)
                        {

                            psProduct.id_tax_rules_group = GetTaxRuleGroupId(product.Tax);
                            psProduct.price = product.Price;
                            psProduct.show_price = 1;

                            var name = Functions.GetPrestaShopName(product.Name);

                            _prestaShopClient.SetLangValue(psProduct, x => x.name, name);
                            _prestaShopClient.SetLangValue(psProduct, x => x.link_rewrite, Functions.GetLinkRewrite(name));
                            _prestaShopClient.SetLangValue(psProduct, x => x.description_short, Functions.GetPrestaShopDescriptionShort(product.DescriptionShort));
                            _prestaShopClient.SetLangValue(psProduct, x => x.description, product.Description);

                            UpdateProductCategories(product, psProduct);

                            psProduct.active = (product.Active && !product.IsDeleted) ? 1 : 0;
                            psProduct.state = 1;
                            psProduct.available_for_order = 1;
                            psProduct.position_in_category = 0;
                            psProduct.minimal_quantity = product.MinimumOrderQuantity > 0 ? product.MinimumOrderQuantity : 1;
                            psProduct.id_category_default = product.Categories.Where(x => x.WebId.HasValue && !x.IsDeleted &&
                                x.Synchronize != Framework.SynchronizeType.Deleted).Max(x => x.WebId);

                            psProduct = SaveOrUpdatePsProduct(psProduct, product);

                            if (!product.IsArchived)
                            {
                                UpdateProductAvailability(product, psProduct);
                            }
                        }
                    }

                    SynchronizeProductImages(product, psProduct);

                    if (product.Synchronize == Framework.SynchronizeType.Deleted)
                    {
                        product.IsDeleted = true;
                    }

                    product.Synchronize = Framework.SynchronizeType.Synchronized;
                    product.IsReady = false;

                    _productRepository.SaveOrUpdate(product);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Synchronize product Id: {product.Id} error.{Environment.NewLine}{ex}");
            }
        }

        private void UpdateProductCategories(ProductEntity product, PsProduct psProduct)
        {
            psProduct.associations.categories.Clear();

            foreach (var category in product.Categories)
            {
                if (category.IsWebPublished)
                {
                    try
                    {
                        var psCategory = _prestaShopClient.CategoryFactory.Get(category.WebId.Value);
                        if (psCategory != null)
                        {
                            psProduct.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category(psCategory.id.Value));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
            }
        }

        private void UpdateProductAvailability(ProductEntity product, PsProduct psProduct)
        {
            var quantity = product.IsAvailable ? 1_000_000 : 0;
            var stock = _prestaShopClient.GetStockForProduct((int)psProduct.id, 0);
            if (stock != null)
            {
                stock.quantity = quantity;
            }
            else
            {
                stock = new Bukimedia.PrestaSharp.Entities.stock_available();
                stock.id_product = psProduct.id;
                stock.id_product_attribute = 0;
                stock.id_shop = _prestaShopClient.DefaultShopId;
                stock.quantity = quantity;
            }

            SaveOrUpdateStockAvailable(stock);
        }

        private Bukimedia.PrestaSharp.Entities.product GetPsProduct(int? id)
        {
            if (id.HasValue && id > 0)
            {
                try
                {
                    return _prestaShopClient.ProductFactory.Get(id.Value);
                }
                catch (Bukimedia.PrestaSharp.PrestaSharpException ex)
                {
                    switch (ex.ResponseHttpStatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            return null;

                        default:
                            throw;
                    }
                }
            }

            return null;
        }

        private long? GetTaxRuleGroupId(TaxEntity tax)
        {
            if (!tax.WebId.HasValue)
            {
                var psTax = _prestaShopClient.GetTaxRuleGroupByRate(tax.Rate);

                if (psTax != null)
                {
                    tax.WebId = (int)psTax.id.Value;
                    _taxRepository.SaveOrUpdate(tax);
                }

                return psTax?.id;
            }

            return tax.WebId;
        }

        private Bukimedia.PrestaSharp.Entities.product SaveOrUpdatePsProduct(Bukimedia.PrestaSharp.Entities.product psProduct, ProductEntity product)
        {
            if (psProduct.id.HasValue && psProduct.id.Value > 0)
            {
                _logger.LogInformation($"Update product id: {product.Id}, name: {product.Name}");
                _prestaShopClient.ProductFactory.Update(psProduct);
            }
            else
            {
                _logger.LogInformation($"Add new product id: {product.Id}, name: {product.Name}");
                psProduct = _prestaShopClient.ProductFactory.Add(psProduct);
            }

            product.WebId = (int)psProduct.id;

            return psProduct;
        }

        private Bukimedia.PrestaSharp.Entities.stock_available SaveOrUpdateStockAvailable(Bukimedia.PrestaSharp.Entities.stock_available stock)
        {
            if (stock.id.HasValue && stock.id > 0)
            {
                _prestaShopClient.StockAvailableFactory.Update(stock);
            }
            else
            {
                stock = _prestaShopClient.StockAvailableFactory.Add(stock);
            }

            return stock;
        }
    }
}
