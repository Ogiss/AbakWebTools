using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework.Domain;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Lib;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services.Implementations
{
    class ProductSynchronizeService : IProductSynchronizeService
    {
        private readonly ILogger _logger;
        private readonly IPrestaShopClient _prestaShopClient;
        private readonly IPsProductRepository _psProductRepository;
        private readonly IPsProductDiscountGroupRepository _psProductDiscountGroupRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductImageSynchronizeService _productImageSynchronizeService;
        private readonly IProductDiscountGroupSynchronizeService _productDiscountGroupSynchronizeService;

        public ProductSynchronizeService(
            ILogger<ProductSynchronizeService> logger,
            IPrestaShopClient prestaShopClient,
            IPsProductRepository psProductRepository,
            IPsProductDiscountGroupRepository psProductDiscountGroupRepository,
            IProductRepository productRepository,
            IProductImageSynchronizeService productImageSynchronizeService,
            IProductDiscountGroupSynchronizeService productDiscountGroupSynchronizeService)

        {
            _logger = logger;
            _prestaShopClient = prestaShopClient;
            _psProductRepository = psProductRepository;
            _psProductDiscountGroupRepository = psProductDiscountGroupRepository;
            _productRepository = productRepository;
            _productImageSynchronizeService = productImageSynchronizeService;
            _productDiscountGroupSynchronizeService = productDiscountGroupSynchronizeService;
        }

        public product Synchronize(ProductEntity product)
        {
            var psProduct = GetPsProduct(product);

            if (product.IsArchived)
            {
                Delete(product, psProduct);
            }
            else
            {
                if (psProduct == null)
                {
                    psProduct = Insert(product);
                }
                else
                {
                    psProduct = Update(product, psProduct);
                }
            }

            if (psProduct != null)
            {
                product.WebId = (int)psProduct.id;
            }

            product.MakeSynchronized();
            _productRepository.SaveOrUpdate(product);
            return psProduct;
        }

        public void AutomaticUpdate(ProductEntity product)
        {
            var psProduct = GetPsProduct(product);

            if (product.IsArchived)
            {
                Delete(product, psProduct);
            }
            else if (psProduct != null)
            {
                if (UpdatePrice(product, psProduct))
                {
                    _logger.LogInformation($"Automatic update PrestaShop product {product.Code}-{product.Name} (Id: {product.Id}, WebId: {psProduct.id})");
                    _psProductRepository.SaveOrUpdate(psProduct);
                }
            }
            else if(psProduct == null)
            {
                product.ClearWebIdentity();
            }

            _productRepository.SaveOrUpdate(product);
        }

        private product GetPsProduct(ProductEntity product)
        {
            return product.WebId.HasValue ? _psProductRepository.Get(product.WebId.Value) : null;
        }

        private product Insert(ProductEntity product)
        {
            _logger.LogInformation($"Insert product {product.Code} - {product.Name} (Id: {product.Id})");
            var psProduct = new product();
            return UpdateCore(product, psProduct);
        }

        private product Update(ProductEntity product, product psProduct)
        {
            _logger.LogInformation($"Update product {product.Code} - {product.Name} (Id: {product.Id}, WebId: {psProduct?.id})");
            return UpdateCore(product, psProduct);
        }

        private product UpdateCore(ProductEntity product, product psProduct)
        {
            if (product.NotWebAvailable)
            {
                DeactivatePsProduct(psProduct);
            }
            else
            {
                psProduct.id_tax_rules_group = product.Tax.WebId;
                psProduct.price = product.Price;
                psProduct.show_price = 1;
                //psProduct.id_unit = product.Unit?.WebId;

                var name = Functions.GetPrestaShopName(product.Name);

                _psProductRepository.SetLangValue(psProduct, x => x.name, name);
                _psProductRepository.SetLangValue(psProduct, x => x.name, name);
                _psProductRepository.SetLangValue(psProduct, x => x.link_rewrite, Functions.GetLinkRewrite(name));
                _psProductRepository.SetLangValue(psProduct, x => x.description_short, Functions.GetPrestaShopDescriptionShort(product.DescriptionShort));
                _psProductRepository.SetLangValue(psProduct, x => x.description, product.Description);

                UpdateProductCategories(product, psProduct);

                psProduct.active = (product.Active && !product.IsDeleted) ? 1 : 0;
                psProduct.state = 1;
                psProduct.available_for_order = 1;
                psProduct.position_in_category = 0;
                psProduct.minimal_quantity = product.MinimumOrderQuantity > 0 ? product.MinimumOrderQuantity : 1;
                psProduct.id_category_default = product.Categories.Where(x => x.WebId.HasValue && !x.IsDeleted &&
                    x.Synchronize != SynchronizeType.Deleted).Max(x => x.WebId);

                psProduct = _psProductRepository.SaveOrUpdate(psProduct);

                UpdateProductDiscountGroups(product, psProduct);
                UpdateProductAvailability(product, psProduct);
                psProduct = _productImageSynchronizeService.Synchronize(product, psProduct);
            }

            return psProduct;
        }

        private void Delete(ProductEntity product, product psProduct)
        {
            _logger.LogInformation($"Delete product {product.Code} - {product.Name} (Id: {product.Id}, WebId: {psProduct?.id})");
            DeactivatePsProduct(psProduct);
        }

        private void DeactivatePsProduct(product psProduct)
        {
            if (psProduct != null)
            {
                psProduct.active = 0;
            }
        }

        private void UpdateProductCategories(ProductEntity product, product psProduct)
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

        private void UpdateProductDiscountGroups(ProductEntity product, product psProduct)
        {
            var enovaProduct = _productRepository.GetEnovaProductAsync(product.EnovaGuid.Value).Result;
            if (enovaProduct != null)
            {
                _productDiscountGroupSynchronizeService.Synchronize(enovaProduct, psProduct);
            }
        }

        private void UpdateProductAvailability(ProductEntity product, product psProduct)
        {
            var quantity = product.IsAvailable ? 1_000_000 : 0;
            var stock = _prestaShopClient.GetStockForProduct((int)psProduct.id, 0);
            if (stock != null)
            {
                stock.quantity = quantity;
            }
            else
            {
                stock = new stock_available();
                stock.id_product = psProduct.id;
                stock.id_product_attribute = 0;
                stock.id_shop = _prestaShopClient.DefaultShopId;
                stock.quantity = quantity;
            }

            SaveOrUpdateStockAvailable(stock);
        }

        private stock_available SaveOrUpdateStockAvailable(Bukimedia.PrestaSharp.Entities.stock_available stock)
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

        private bool UpdatePrice(ProductEntity product, product psProduct)
        {
            if (psProduct.price != product.Price)
            {
                psProduct.price = product.Price;

                return true;
            }

            return false;
        }
    }
}
