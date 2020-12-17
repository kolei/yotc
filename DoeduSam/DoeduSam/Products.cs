//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoeduSam
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.AdditionalProducts = new HashSet<AdditionalProducts>();
            this.Images = new HashSet<Images>();
            this.ProductSales = new HashSet<ProductSales>();
        }
    
        public int Id { get; set; }

        public String ExistingProduct
        {
            get
            {
                if (Id == 0) return "Collapsed";
                return "Visible";
            }
        }

        public string Name { get; set; }
        public string Image { get; set; }
        public int ManufacturerId { get; set; }
        public int Active { get; set; }
        public int Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdditionalProducts> AdditionalProducts { get; set; }
        public virtual Manufacturers Manufacturers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Images> Images { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSales> ProductSales { get; set; }
    }
}
