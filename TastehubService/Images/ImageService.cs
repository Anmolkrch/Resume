using Tastehub.Core.EntityModel;
using TastehubModel.ViewModel;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TastehubService.ImageService
{
    
   public  class ImageService
    {
        OnBoadTaskEntities _Context = new OnBoadTaskEntities();

        public List<ImageViewModel> GetAllImages()
        {
            List<ImageViewModel> entities = new List<ImageViewModel>();

            var list = _Context.Images.Where(x => x.IsActive == true).ToList();

            Mapper.Map(list, entities);

            return entities.ToList();
        }
        public ImageCategory GetImageCategoryById(int Id)
        {
            ImageCategory entities = new ImageCategory();

            var list = _Context.ImageCategories.Where(x => x.CategoryID == Id).FirstOrDefault();

            Mapper.Map(list, entities);

            return entities;
        }

        public List<ImageCategory> GetCategory()
        {
            List<ImageCategory> entities = new List<ImageCategory>();

            var list = _Context.ImageCategories.Where(x => x.IsActive == true).ToList();

            Mapper.Map(list, entities);

            return entities;
        }
        public bool SaveImageCategory(ImageCategoryViewModel objCategory, long UserId)
        {
            bool status = false;
            try
            {
                var category = new ImageCategory();
                Mapper.Map(objCategory, category);

                category.IsActive = true;
                category.CreatedDate = DateTime.Now;
                category.ModifiedDate = DateTime.Now;
                category.CreatedBy = UserId;   // replace with actual user ID
                category.ModifiedBy = UserId;

                _Context.ImageCategories.Add(category);
                _Context.Configuration.ValidateOnSaveEnabled = true;
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool UpdateImageCategory(ImageCategoryViewModel objCategory, long UserId)
        {
            bool status = false;
            try
            {
                var category = _Context.ImageCategories
                    .FirstOrDefault(x => x.CategoryID == objCategory.CategoryID);

                if (category != null)
                {
                    category.CategoryName = objCategory.CategoryName;
                    category.Description = objCategory.Description;
                    category.ModifiedDate = DateTime.Now;
                    category.ModifiedBy = UserId;   // replace with actual user ID

                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }
        public bool SaveImage(ImageViewModel objImage, long UserId)
        {
            bool status = false;
            try
            {
                var image = new Image();
                Mapper.Map(objImage, image);

                image.IsActive = true;
                image.CreatedDate = DateTime.Now;
                image.ModifiedDate = DateTime.Now;
                image.CreatedBy = UserId;   // replace with actual user ID
                image.ModifiedBy = UserId;

                _Context.Images.Add(image);
                _Context.Configuration.ValidateOnSaveEnabled = true;
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool UpdateImage(ImageViewModel objImage, long UserId)
        {
            bool status = false;
            try
            {
                var image = _Context.Images
                    .FirstOrDefault(x => x.ImageID == objImage.ImageID);

                if (image != null)
                {
                    image.ImageName = objImage.ImageName;
                    image.PathPrimary = objImage.PathPrimary;
                    image.PathSecondary = objImage.PathSecondary;
                    image.CategoryID = objImage.CategoryID;
                    image.UploadedBy = objImage.UploadedBy;
                    image.ModifiedDate = DateTime.Now;
                    //image.ModifiedBy = UserId;   // replace with actual user ID

                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }
        public bool DeactivateImageCategory(int categoryId, long userId)
        {
            bool status = false;
            try
            {
                var category = _Context.ImageCategories
                    .FirstOrDefault(x => x.CategoryID == categoryId);

                if (category != null)
                {
                    category.IsActive = false;
                    category.ModifiedDate = DateTime.Now;
                    category.ModifiedBy = userId;

                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool DeleteImageCategory(int categoryId)
        {
            bool status = false;
            try
            {
                var category = _Context.ImageCategories
                    .FirstOrDefault(x => x.CategoryID == categoryId);

                if (category != null)
                {
                    _Context.ImageCategories.Remove(category);
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }
        public bool DeactivateImage(int imageId, long userId)
        {
            bool status = false;
            try
            {
                var image = _Context.Images
                    .FirstOrDefault(x => x.ImageID == imageId);

                if (image != null)
                {
                    image.IsActive = false;
                    image.ModifiedDate = DateTime.Now;
                    image.ModifiedBy = userId;

                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool DeleteImage(int imageId)
        {
            bool status = false;
            try
            {
                var image = _Context.Images
                    .FirstOrDefault(x => x.ImageID == imageId);

                if (image != null)
                {
                    _Context.Images.Remove(image);
                    _Context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

    }
}
