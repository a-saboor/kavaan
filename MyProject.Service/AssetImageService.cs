using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
    class AssetImageService :IAssetImageService
    {
		private readonly IAssetImageRepository _assetImageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AssetImageService(IAssetImageRepository assetImageRepository, IUnitOfWork unitOfWork)
		{
			this._assetImageRepository = assetImageRepository;
			this._unitOfWork = unitOfWork;
		}
		#region IassetImageService Members

		public IEnumerable<AssetImage> GetAssetImages()
		{
			var AssetImages= _assetImageRepository.GetAll();
			return AssetImages;
		}

		public IEnumerable<AssetImage> GetAssetImageByTournamentID(long tournamentID)
		{
			var assetImages = _assetImageRepository.GetAssetImages(tournamentID);
			return assetImages;
		}

		public AssetImage GetAssetImage(long id)
		{
			var assetimage = _assetImageRepository.GetById(id);
			return assetimage;
		}

		public bool CreateAssetImage(ref AssetImage assetimage, ref string message)
		{
			try
			{
				_assetImageRepository.Add(assetimage);
				if (SaveAssetImage())
				{

					message = "Asset gallery image added successfully ...";
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

		public bool UpdateTournamentImage(ref AssetImage assetimage, ref string message)
		{
			try
			{
				_assetImageRepository.Update(assetimage);
				if (SaveAssetImage())
				{
					message = "Asset gallery image updated successfully ...";
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

		public bool DeleteAssetImage(long id, ref string message, ref string filepath)
		{
			try
			{
				AssetImage assetImage = _assetImageRepository.GetById(id);
				filepath = assetImage.Image;
				_assetImageRepository.Delete(assetImage);
				if (SaveAssetImage())
				{
					message = "Asset gallery image deleted successfully ...";
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

		public bool SaveAssetImage()
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

		#endregion
	}
	public interface IAssetImageService
	{
		IEnumerable<AssetImage> GetAssetImages();
		IEnumerable<AssetImage> GetAssetImageByTournamentID(long tournamentID);
		AssetImage GetAssetImage(long id);
		bool CreateAssetImage(ref AssetImage assetimage, ref string message);
		bool UpdateTournamentImage(ref AssetImage assetimage, ref string message);
		bool DeleteAssetImage(long id, ref string message, ref string filepath);
		bool SaveAssetImage();
	}
}
