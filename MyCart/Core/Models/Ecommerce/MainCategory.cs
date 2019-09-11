using MyCart.Models.Ecommerce;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyCart.Core.Models.Ecommerce
{
    [DataContract]
    public class MainCategory : Category
    {
        /// <summary>
        /// Gets or sets the property that has been bound with a label in SfExpander content, which displays the sub category.
        /// </summary>
        [DataMember(Name = "subcategories")]
        public List<Category> SubCategories { get; set; }
    }
}