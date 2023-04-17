using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class BannerService : IBannerService
    {
        private readonly IBannersRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BannerService(IBannersRepository bannerRepository, IUnitOfWork unitOfWork)
        {
            this._bannerRepository = bannerRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Banner> GetBanners()
        {
            var banners = _bannerRepository.GetAll();
            return banners;
        }

        public IEnumerable<Banner> GetBannersByDevice(string device)
        {
            var banners = _bannerRepository.GetBannerByDevice(device);
            return banners;
        }
        public IEnumerable<Banner> GetBannersByContentType(string contentType)
        {
            var banners = _bannerRepository.GetBannerByContentType(contentType);
            return banners;
        }
        public IEnumerable<Banner> GetBannersByContentTypeAndDevice(string contentType, string device)
        {
            var banners = _bannerRepository.GetBannerByContentTypeAndDevice(contentType, device);
            return banners;
        }

        public IEnumerable<Banner> GetBannersByContentTypeAndDeviceAndType(string contentType, string device, string Type)
        {
            var banners = _bannerRepository.GetBannerByContentTypeAndDeviceAndType(contentType, device, Type);
            return banners;
        }

        public IEnumerable<Banner> GetBannersByTypeAndLang(string device, string lang = "en")
        {
            var banners = _bannerRepository.GetBannerByTypeAndLang(device, lang);
            return banners;
        }

        public Banner GetBanner(long id)
        {
            var bannner = _bannerRepository.GetById(id);
            return bannner;
        }

        public bool CreateBanner(Banner banner, ref string message)
        {
            try
            {
                _bannerRepository.Add(banner);

                if (SaveBanner())
                {
                    message = "Banner added successfully ...";
                    return true;

                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool UpdateBanner(ref Banner banner, ref string message)
        {
            try
            {
                _bannerRepository.Update(banner);
                if (SaveBanner())
                {
                    return true;
                }
                else
                {

                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool DeleteBanner(long id, bool softDelete = true)
        {
            try
            {
                Banner banner = _bannerRepository.GetById(id);
                if (softDelete)
                {

                    _bannerRepository.Delete(banner);
                    SaveBanner();
                    return true;
                }


                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool SaveBanner()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
    public interface IBannerService
    {
        IEnumerable<Banner> GetBanners();
        IEnumerable<Banner> GetBannersByDevice(string device);
        IEnumerable<Banner> GetBannersByContentType(string contentType);
        IEnumerable<Banner> GetBannersByContentTypeAndDevice(string contentType, string device);
        IEnumerable<Banner> GetBannersByContentTypeAndDeviceAndType(string contentType, string device, string Type);
        IEnumerable<Banner> GetBannersByTypeAndLang(string device, string lang = "en");
        Banner GetBanner(long id);
        bool CreateBanner(Banner Banner, ref string message);
        bool UpdateBanner(ref Banner banner, ref string message);
        bool DeleteBanner(long id, bool softDelete = true);
        bool SaveBanner();
    }
}
