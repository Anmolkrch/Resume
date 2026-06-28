using Tastehub.Core.EntityModel;
using TastehubModel.CartModel;
using TastehubModel.ViewModel;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using static Utility.Enums;

namespace TastehubService.Service.ItemService
{
    public class ItemService
    {
        private OnBoadTaskEntities _Context = new OnBoadTaskEntities();
        #region Public_Methods
        public List<ProductViewModel> GetItemList(int Id)
        {
            List<ProductViewModel> productViewModel = new List<ProductViewModel>();
            try
            {
                List<Tbl_Product> product = new List<Tbl_Product>();
                if (Id==0)
                {
                    product = _Context.Tbl_Product.Where(x => x.IsActive == true && x.IsDelete == false)
                                   .Include("Tbl_Category").ToList();
                }
                else{
                    product = _Context.Tbl_Product.Where(x => x.IsActive == true && x.IsDelete == false
                    && x.CategoryId==Id).Include("Tbl_Category").ToList();
                }
                Mapper.Map(product, productViewModel);
            }
            catch (Exception ex)
            {

            }
            return productViewModel;
        }
        public List<ProductViewModel> GetItemImage()
        {
            List<ProductViewModel> productViewModel = new List<ProductViewModel>();
            try
            {
                List<Tbl_Product> product = new List<Tbl_Product>();
                product = _Context.Tbl_Product.Where(x => x.IsActive == true && x.IsDelete == false).ToList();
                Mapper.Map(product, productViewModel);
            }
            catch (Exception ex)
            {

            }
            return productViewModel;
        }
        public ProductViewModel GetProductById(int Id)
        {
            ProductViewModel entities = new ProductViewModel();
            var list = _Context.Tbl_Product.Where(x=>x.ProductId==Id).FirstOrDefault();
            Mapper.Map(list, entities);
            return entities;
        }
        public bool SaveProduct(ProductViewModel objProduct)
        {
            bool status = false;
            try
            {
                Tbl_Product tblProduct = new Tbl_Product();
                Mapper.Map(objProduct, tblProduct);
                tblProduct.IsActive = true;
                tblProduct.CreatedDate = DateTime.Now;
                tblProduct.ModifiedDate = DateTime.Now;
                tblProduct.IsDelete = false;
                tblProduct.CategoryId = objProduct.CategoryId;
                tblProduct.IsFeatured = false;
                tblProduct.Tbl_Category = null;
                _Context.Tbl_Product.Add(tblProduct);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public bool UpdateProduct(ProductViewModel objProduct)
        {
            bool status = false;
            try
            {
                var _tblProduct = _Context.Tbl_Product.Where(x => x.ProductId == objProduct.ProductId).FirstOrDefault();
                if (_tblProduct != null)
                {
                    _tblProduct.ModifiedDate = DateTime.Now;
                    _tblProduct.ProductImage = objProduct.ProductImage;
                    _tblProduct.ProductName = objProduct.ProductName;
                    _tblProduct.Price = objProduct.Price;
                    _tblProduct.Description = objProduct.Description;
                    _tblProduct.CategoryId = objProduct.CategoryId;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public List<CategoryViewModel> GetCategoryList()
        {
            List<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>();
            try
            {
                List<Tbl_Category> category = new List<Tbl_Category>();
                category = _Context.Tbl_Category.Where(x=>x.IsActive== true).ToList();
                Mapper.Map(category, categoryViewModel);
            }
            catch (Exception ex)
            {

            }
            return categoryViewModel;
        }
        public List<CartViewModel> GetOrderList()
        {
            List<CartViewModel> cartViewModels = new List<CartViewModel>();
            try
            {
                List<Tbl_Cart> product = new List<Tbl_Cart>();
                product = _Context.Tbl_Cart.Where(x=>x.CartStatusId ==
                (int)CartStatus.Purchasedtheitem && x.ShippingDetailId != null).
                Include("Tbl_Product").OrderBy(x=>x.ShippingDetailId)
                .ToList();
               // var myQuery = from cart in _Context.Tbl_Cart.Where(x => x.CartStatusId ==
               //(int)CartStatus.Purchasedtheitem && x.ShippingDetailId != null)
               //   .Include(x => x.Tbl_Product)
               //               select new
               //               {
               //                   Product = cart,
               //                   Localization = cart.Tbl_Product
               //               };

                // var prods = myQuery.ToList();

                Mapper.Map(product, cartViewModels);
            }
            catch (Exception ex)
            {

            }
            return cartViewModels;
        }
        public List<PlacedOrderViewModel> GetAllOrderList()
        {
            List<PlacedOrderViewModel> placedOrder = new List<PlacedOrderViewModel>();
            try
            {
                var orderResul = _Context.GetPlacedOrders(0).ToList();
                Mapper.Map(orderResul, placedOrder);
            }
            catch(Exception ex)
            {

            }
            return placedOrder;
        }
        public List<OrderDetailViewModel> GetOrderDetail(int ShippingId)
        {
            List<OrderDetailViewModel> orderDetail = new List<OrderDetailViewModel>();
            try
            {
                var orderResul = _Context.GetOrdersByShippingId(ShippingId).ToList();
                Mapper.Map(orderResul, orderDetail);
            }
            catch (Exception ex)
            {

            }
            return orderDetail;
        }
        public bool DeleteProduct(int Id, string LogId, ref string file)
        {
            bool status = false;
            string filePath = string.Empty;
            try
            {
                var _tblProduct = _Context.Tbl_Product.Where(x => x.ProductId == Id).FirstOrDefault();
                if (_tblProduct != null)
                {

                    _tblProduct.ModifiedDate = DateTime.Now;
                    _tblProduct.IsActive = !_tblProduct.IsActive;
                    _tblProduct.IsDelete = true; filePath = _tblProduct.ProductImage;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            file = filePath;
            return status;
        }
        #endregion
    }
}
