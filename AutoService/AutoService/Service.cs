//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoService
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            this.ClientService = new HashSet<ClientService>();
            this.ServicePhoto = new HashSet<ServicePhoto>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Cost { get; set; }
        public int DurationInSeconds { get; set; }
        public string Description { get; set; }
        public Nullable<double> Discount { get; set; }
        public string MainImagePath { get; set; }
    
        public float DiscountFloat {
            get {
                return Convert.ToSingle( Discount ?? 0 );
            }
        }

        public Uri ImageUri {
            get { 
                return new Uri(Path.Combine(Environment.CurrentDirectory, MainImagePath));
            }
        }

        public Boolean HasDiscount
        {
            get
            {
                return Discount > 0;
            }
        }

        public string CostString
        {
            get
            {
                return Cost.ToString("#.##");
            }
        }

        public string CostWithDiscount
        {
            get
            {
                return (Cost * Convert.ToDecimal(1 - Discount ?? 0)).ToString("#.##");
            }
        }

        public string CostTextDecoration
        {
            get
            {
                return HasDiscount ? "Strikethrough" : "None";
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientService> ClientService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServicePhoto> ServicePhoto { get; set; }
    }
}
