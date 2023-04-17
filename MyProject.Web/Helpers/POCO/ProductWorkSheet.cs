using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Web.Helpers.POCO
{
	public class ProductWorkSheet
	{
		[Required]
		public string SKU { get; set; }
		public string Slug { get; set; }
		public string Thumbnail { get; set; }
		public string Name { get; set; }

		public string ShortDescription { get; set; }

		public string LongDescription { get; set; }

		public decimal RegularPrice { get; set; }
		public decimal? SalePrice { get; set; }
		public DateTime? SalePriceFrom { get; set; }
		public DateTime? SalePriceTo { get; set; }
		public int Stock { get; set; }
		public int Threshold { get; set; }
		public string Type { get; set; }
		public float Weight { get; set; }
		public float Length { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public string IsSoldIndividually { get; set; }
		public string AllowMultipleOrder { get; set; }
		public string StockStatus { get; set; }
		public string IsFeatured { get; set; }
		public string IsManageStock { get; set; }
		public string Brand { get; set; }
		public string ProductSports { get; set; }
		public string ProductTags { get; set; }
		public string Attributes { get; set; }
		public string Images { get; set; }
		public string VariantSKU { get; set; }
		public string VariantThumbnail { get; set; }
		public decimal VariantRegularPrice { get; set; }
		public decimal? VariantSalePrice { get; set; }
		public DateTime? VariantSalePriceFrom { get; set; }
		public DateTime? VariantSalePriceTo { get; set; }
		public int VariantStock { get; set; }
		public int VariantThreshold { get; set; }
		public string VariantStockStatus { get; set; }
		public float VariantWeight { get; set; }
		public float VariantLength { get; set; }
		public float VariantWidth { get; set; }
		public float VariantHeight { get; set; }
		public string VariantDescription { get; set; }
		public string VariantIsManageStock { get; set; }
		public string VariantSoldIndividually { get; set; }
		public string VariantAttributes { get; set; }
		public string VariantImages { get; set; }
	}
}