using Tastehub.Core.EntityModel;
using TastehubModel.ViewModel;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static Utility.Enums;

namespace TastehubService.User
{
    public class HomeService
    {
        private OnBoadTaskEntities _Context = new OnBoadTaskEntities();
        #region Public_Methods
        public bool SaveContact(ContactViewModel contactViewModel)
        {
            bool status = false;

            ContactRequest contact = new ContactRequest();
            try
            {
                Mapper.Map(contactViewModel, contact);
                contact.IsActive = true;
                contact.CreatedDate = DateTime.Now;
                contact.CreatedBy = "101";
                contact.ModifiedBy = "101";
                contact.ModifiedDate = DateTime.Now;
                _Context.ContactRequests.Add(contact);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public bool AddItemInCart(int UserId, int ItemId, ref int count)
        {
            bool status = false;

            Tbl_Cart cart = new Tbl_Cart();
            try
            {
                cart.CartStatusId = (int)CartStatus.Addedtocart;
                cart.AddedOn = DateTime.Now;
                cart.ProductId = ItemId;
                cart.MemberId = UserId;
                cart.UpdatedOn = DateTime.Now;
                _Context.Tbl_Cart.Add(cart);
                _Context.SaveChanges();
                count = _Context.Tbl_Cart.
                    Where(x => x.MemberId == UserId && x.ProductId == 
                    ItemId && x.CartStatusId == (int)CartStatus.Addedtocart).Count();

                status = true;
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public List<CartViewModel> GetMyCart(int UserId)
        {
            List<CartViewModel> cartViewModels = new List<CartViewModel>();
            try
            {
                List<Tbl_Cart> product = new List<Tbl_Cart>();
                product = _Context.Tbl_Cart.Where(x => x.MemberId == UserId && x.CartStatusId ==
                (int)CartStatus.Addedtocart).Include("Tbl_Product").ToList();
                Mapper.Map(product, cartViewModels);
            }
            catch (Exception ex)
            {

            }
            return cartViewModels;
        }
        public List<UserDetailViewModel> GetMyAddress(long UserId)
        {
            List<UserDetailViewModel> userDetailViewModel = new List<UserDetailViewModel>();
            try
            {
                List<UserDetail> userDetail = new List<UserDetail>();
                userDetail = _Context.UserDetails.OrderBy(x => x.IsDefault).Where(x => x.UserId == UserId).ToList();
                Mapper.Map(userDetail, userDetailViewModel);
            }
            catch (Exception ex)
            {

            }
            return userDetailViewModel;
        }
        public bool PlaceOrder(long UserId)
        {
            bool status = false;

            Tbl_ShippingDetails shipping = new Tbl_ShippingDetails();
            try
            {
                List<Tbl_Cart> _objCart = _Context.Tbl_Cart.Include("Tbl_Product").Where(x => x.CartStatusId == (int)CartStatus.Addedtocart && x.MemberId== UserId).ToList();
                decimal TotalPrice = 0;
                if (_objCart != null)
                {
                   
                    foreach (var material in _objCart)
                    {
                        TotalPrice = TotalPrice+material.Tbl_Product.Price;
                    }
                    shipping.MemberId = (int)UserId;
                    shipping.AmountPaid = TotalPrice;
                    shipping.PaymentType = PaymentMode.PayOndelivery.ToString();
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.Tbl_ShippingDetails.Add(shipping);
                    _Context.SaveChanges();
                    _objCart.ForEach(z => { z.CartStatusId = (int)CartStatus.Purchasedtheitem;
                        z.UpdatedOn = DateTime.Now; z.ShippingDetailId = shipping.ShippingDetailId; });
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public List<CartViewModel> GetMyOrders(int UserId)
        {
            List<CartViewModel> cartViewModels = new List<CartViewModel>();
            try
            {
                List<Tbl_Cart> product = new List<Tbl_Cart>();
                product = _Context.Tbl_Cart.Where(x => x.MemberId == UserId && x.CartStatusId ==
                (int)CartStatus.Purchasedtheitem && x.ShippingDetailId != null).Include("Tbl_Product").ToList();
                Mapper.Map(product, cartViewModels);
            }
            catch (Exception ex)
            {

            }
            return cartViewModels;
        }
        public bool SaveMyAddress(long UserId, UserDetailViewModel userDetailViewModel)
        {
            bool status = false;
            try
            {
                    _Context.SaveUserDetails(userDetailViewModel.Id, userDetailViewModel.CurCountry,
                        1, userDetailViewModel.CurCity, userDetailViewModel.CurZipCode,
                        userDetailViewModel.CurAddress1, userDetailViewModel.CurAddress2, userDetailViewModel.ProfileUrl, UserId,
                        userDetailViewModel.IsDefault);
 
                status = true;
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public bool RemoveItemInCart(int UserId, int ItemId, ref int count)
        {
            bool status = false;

            Tbl_Cart cart = new Tbl_Cart();
            try
            {
               var item = _Context.Tbl_Cart.Where(x => x.ProductId == ItemId && x.MemberId==UserId).FirstOrDefault();
                if (item != null)
                {
                    _Context.Tbl_Cart.Remove(item);
                    _Context.SaveChanges();
                    count = _Context.Tbl_Cart.
                   Where(x => x.MemberId == UserId && x.ProductId ==
                   ItemId && x.CartStatusId == (int)CartStatus.Addedtocart).Count();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public List<CartViewModel> PlaceCartOrder(long UserId, ref bool result,
            ref decimal totalPrice,int AddressId, ref int shippingResponseId,string paymentType)
        {
            bool status = true;
            List<CartViewModel> cartList = new List<CartViewModel>();
            Tbl_ShippingDetails shipping = new Tbl_ShippingDetails();
            try
            {
                List<Tbl_Cart> _objCart = _Context.Tbl_Cart.Include("Tbl_Product").Where(x => x.CartStatusId == (int)CartStatus.Addedtocart && x.MemberId == UserId).ToList();
                decimal TotalPrice = 0;
                if (_objCart != null)
                {

                    foreach (var material in _objCart)
                    {
                        TotalPrice = TotalPrice + material.Tbl_Product.Price;
                    }
                    shipping.MemberId = (int)UserId;
                    shipping.AmountPaid = totalPrice= TotalPrice;
                    shipping.CreatedOn =  DateTime.Now;
                    shipping.CreatedBy = UserId;
                    shipping.IsActive = true;
                    shipping.IsDeleted = false;
                    shipping.AddressId = AddressId;
                    shipping.PaymentType = paymentType;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.Tbl_ShippingDetails.Add(shipping);
                    _Context.SaveChanges();
                    _objCart.ForEach(z => {
                        z.CartStatusId = (int)CartStatus.Purchasedtheitem;
                        z.UpdatedOn = DateTime.Now; z.ShippingDetailId = shipping.ShippingDetailId;
                    });
                    _Context.SaveChanges();
                    shippingResponseId=shipping.ShippingDetailId;
                    result = status;
                }
                Mapper.Map(_objCart, cartList);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return cartList;
        }

        public decimal GetOrderAmount(long UserId, List<CartSpiceModel> cartSpiceModel)
        {
            decimal TotalPrice = 0;
            try
            {
                List<Tbl_Cart> _objCart = _Context.Tbl_Cart.Include("Tbl_Product").
               Where(x => x.CartStatusId == (int)CartStatus.Addedtocart && x.MemberId == UserId).ToList();

                if (_objCart != null)
                {
                    foreach (var material in _objCart)
                    {
                        TotalPrice = TotalPrice + material.Tbl_Product.Price;
                        if (cartSpiceModel!=null && cartSpiceModel.Count > 0)
                        {
                            foreach (var cart in cartSpiceModel)
                            {
                                if (cart.CartId == material.CartId)
                                {
                                    _objCart.Where(x => x.CartId == material.CartId)
                                        .FirstOrDefault().Spices = cart.Spices;
                                }
                            }
                        }
                    }
                }
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return TotalPrice;
        }
        public bool PaymentDetails(long UserId, decimal totalPrice, int shippingResponseId, string response)
        {
            bool status = true;
            Tbl_PaymentDetails payment = new Tbl_PaymentDetails();
            try
            {
        
                payment.MemberId = (int)UserId;
                payment.AmountPaid = totalPrice;
                payment.CreatedOn = DateTime.Now;
                payment.CreatedBy = UserId;
                payment.IsActive = true;
                payment.IsDeleted = false;
                payment.ShippingDetailId = shippingResponseId;
                payment.Response = response;
                _Context.Configuration.ValidateOnSaveEnabled = false;
                _Context.Tbl_PaymentDetails.Add(payment);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        #endregion
    }
}
