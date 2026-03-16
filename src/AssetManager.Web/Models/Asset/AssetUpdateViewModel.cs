using System.ComponentModel.DataAnnotations;

namespace AssetManager.Web.Models.Asset
{
    public class AssetUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string SerialNumber { get; set; }
        public int? AssignedUserId { get; set; } // Atanacak kullanıcı ID'si
    }
}
