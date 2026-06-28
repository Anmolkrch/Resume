using Tastehub.Core.EntityModel;
using TastehubModel.ViewModel;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TastehubService.MenuNamespace
{
    
   public  class MenuService
    {
        OnBoadTaskEntities _Context = new OnBoadTaskEntities();

        public List<MainMenuViewModel> GetAllMenu()
        {
            List<MainMenuViewModel> entities = new List<MainMenuViewModel>();
            
            var list = _Context.tblMenus.Where(x => x.IsActive == true).ToList();

            Mapper.Map(list, entities);

            return entities.ToList();
        }   
        public SubMainMenuViewModel GetSubMenuById(int Id)
        {
            SubMainMenuViewModel entities = new SubMainMenuViewModel();

            var list = _Context.tblSubMenus.Where(x => x.IsActive == true && x.Id ==Id).FirstOrDefault();

            Mapper.Map(list, entities);

            return entities;
        }
        
        public List<SubMainMenuViewModel> GetAllSubMenu()
        {
            List<SubMainMenuViewModel> entities = new List<SubMainMenuViewModel>();

            var list = _Context.tblSubMenus.Where(x => x.IsActive == true).ToList();

            Mapper.Map(list, entities);

            return entities;
        }

        public bool SaveSubMenuContent(SubMainMenuViewModel objSubMenu)
        {
            bool status = false;

            tblSubMenu tblsubmenu = new tblSubMenu();
            Mapper.Map(objSubMenu, tblsubmenu);

            tblsubmenu.IsActive = true;
            tblsubmenu.CreatedDate = DateTime.Now;
            tblsubmenu.ModifiedDate = DateTime.Now;
            tblsubmenu.CreatedBy = "101";
            tblsubmenu.ModifiedBy = "101";
            _Context.tblSubMenus.Add(tblsubmenu);
            _Context.Configuration.ValidateOnSaveEnabled = true;
            _Context.SaveChanges();
            status = true;

            return status;
            // for new users
        }

        public bool UpdateSubMenuContent(SubMainMenuViewModel objSubMenu)
        {
            bool status = false;
            try
            {
                var _objSubMenuDetails = _Context.tblSubMenus.Where(x=>x.Id==objSubMenu.Id).FirstOrDefault();
                if (_objSubMenuDetails != null)
                {
                    _objSubMenuDetails.MenuText = objSubMenu.MenuText;
                    _objSubMenuDetails.ModifiedDate = DateTime.Now;
                   
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

        public bool SaveGallery(GalleryViewModel objGallery)
        {
            bool status = false;

            tblGallery tblgallery = new tblGallery();
            Mapper.Map(objGallery, tblgallery);

            tblgallery.IsActive = true;
            tblgallery.CreatedDate = DateTime.Now;
            tblgallery.ModifiedDate = DateTime.Now;
            tblgallery.CreatedBy = "101";
            tblgallery.ModifiedBy = "101";
            _Context.tblGalleries.Add(tblgallery);
            _Context.Configuration.ValidateOnSaveEnabled = true;
            _Context.SaveChanges();
            status = true;

            return status;
        }

        public bool UpdateGallery(GalleryViewModel objgallery)
        {
            bool status = false;
            try
            {
                var _tblgallery = _Context.tblGalleries.Where(x => x.id == objgallery.id).FirstOrDefault();
                if (_tblgallery != null)
                {
                    _tblgallery.ModifiedDate = DateTime.Now;
                    _tblgallery.GalleryPath = objgallery.GalleryPath;
                    _tblgallery.GalleryName = objgallery.GalleryName;
                    _tblgallery.Title = objgallery.Title;
                    _tblgallery.TypeId = objgallery.TypeId;
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

        public List<GalleryViewModel> GetAllGallery()
        {
            List<GalleryViewModel> entities = new List<GalleryViewModel>();
            var list = _Context.GetGalleryItem(0).ToList();
            //var list = _Context.tblGalleries.Where(x => x.IsActive == true).ToList();

            Mapper.Map(list, entities);

            return entities.ToList();
        }

        public GalleryViewModel GetGalleryById(int Id)
        {
            GalleryViewModel entities = new GalleryViewModel();
            var list = _Context.GetGalleryItem(Id).FirstOrDefault();
           // var list = _Context.tblGalleries.Where(x => x.IsActive == true && x.id == Id).FirstOrDefault();

            Mapper.Map(list, entities);

            return entities;
        }

        public bool SaveAlbum(AlbumViewModel objAlbum)
        {
            bool status = false;

            mtbAlbum tblalbum = new mtbAlbum();
            Mapper.Map(objAlbum, tblalbum);

            tblalbum.IsActive = true;
            tblalbum.CreatedDate = DateTime.Now;
            tblalbum.ModifiedDate = DateTime.Now;
            tblalbum.CreatedBy = "101";
            tblalbum.ModifiedBy = "101";
            _Context.mtbAlbums.Add(tblalbum);
            _Context.Configuration.ValidateOnSaveEnabled = true;
            _Context.SaveChanges();
            status = true;

            return status;
        }

        public bool UpdateAlbum(AlbumViewModel objAlbum)
        {
            bool status = false;
            try
            {
                var _tblalbum = _Context.mtbAlbums.Where(x => x.id == objAlbum.id).FirstOrDefault();
                if (_tblalbum != null)
                {
                   
                    _tblalbum.ModifiedDate = DateTime.Now;
                    _tblalbum.AlbumImage = objAlbum.AlbumImage;
                    _tblalbum.AlbumName = objAlbum.AlbumName;
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

        public List<AlbumViewModel> GetAllAlbum()
        {
            List<AlbumViewModel> entities = new List<AlbumViewModel>();

            var list = _Context.mtbAlbums.Where(x => x.IsActive == true).ToList();

            Mapper.Map(list, entities);

            return entities.ToList();
        }

        public AlbumViewModel GetAlbumById(int Id)
        {
            AlbumViewModel entities = new AlbumViewModel();

            var list = _Context.mtbAlbums.Where(x => x.IsActive == true && x.id == Id).FirstOrDefault();

            Mapper.Map(list, entities);

            return entities;
        }
        public bool DeleteGalleryItemById(int Id,string LogId)
        {
            bool status = false;
            try
            {
                var _tblalbum = _Context.tblGalleries.Where(x => x.id == Id).FirstOrDefault();
                if (_tblalbum != null)
                {

                    _tblalbum.ModifiedDate = DateTime.Now;
                    _tblalbum.IsActive = !_tblalbum.IsActive;
                    _tblalbum.ModifiedBy = LogId;
                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                //Utility.LogWriter.LogWrite("Email Sending End", filePath);
            }
            return status;
        }
        public List<CityViewModel> GetCity(bool IsActive)
        {
            List<CityViewModel> cityViewModel = new List<CityViewModel>();
            try
            {
                var city = _Context.mtbCities.Where(x => x.IsActive == IsActive).ToList();
                return Mapper.Map(city, cityViewModel);
            }
            catch (Exception ex)
            {
            }
            return cityViewModel.ToList();
        }
        public List<PaymentViewModel> GetAllPayment()
        {
            List<PaymentViewModel> entities = new List<PaymentViewModel>();
            try
            {
                var list = _Context.GetPaidOrderDetail().ToList();
                Mapper.Map(list, entities);
            }
            catch (Exception ex)
            {
            }
            return entities.ToList();
        }
    }
}
