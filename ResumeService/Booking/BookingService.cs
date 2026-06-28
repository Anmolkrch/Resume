using TastehubModel.ViewModel;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Tastehub.Core.EntityModel;

namespace Tastehub.Service.BookingService
{
    public class BookingService 
    {
        private OnBoadTaskEntities _Context = new OnBoadTaskEntities();
        #region Public_Methods

        public bool SaveBooking(BookingReqestViewModel bookingReqestViewModel)
        {
            bool status = false;

            BookingRequest booking = new BookingRequest();
            try
            {
                Mapper.Map(bookingReqestViewModel, booking); booking.IsActive = true;
                booking.CreatedDate = DateTime.Now;
                booking.CreatedBy = "101";
                booking.ModifiedBy = "101";
                booking.ModifiedDate = DateTime.Now;
                _Context.BookingRequests.Add(booking);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public List<BookingReqestViewModel> GetBookingList()
        {
            List<BookingReqestViewModel> bookingViewModel = new List<BookingReqestViewModel>();
            List<BookingRequest> booking = new List<BookingRequest>();
            booking = _Context.BookingRequests.ToList();
            Mapper.Map(booking, bookingViewModel);
            return bookingViewModel;
        }
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
        public List<ContactViewModel> GetContactRequestList()
        {
            List<ContactViewModel> contactViewModel = new List<ContactViewModel>();
            List<ContactRequest> contact = new List<ContactRequest>();
            contact = _Context.ContactRequests.ToList();
            Mapper.Map(contact, contactViewModel);
            return contactViewModel;
        }
        #endregion
    }
}
