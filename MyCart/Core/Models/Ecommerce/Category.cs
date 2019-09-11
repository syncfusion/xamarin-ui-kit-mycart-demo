using MyCart.Core.Interface;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyCart.Models.Ecommerce
{
    /// <summary>
    /// Model for category.
    /// </summary>
    [DataContract]
    public class Category : ICategory
    {
        private string icon;

        /// <summary>
        /// Gets or sets the property that has been bound with an image, which displays the category.
        /// </summary>
        [DataMember(Name = "icon")]
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a label in SfExpander header, which displays the main category.
        /// </summary>
        [DataMember(Name = "categoryname")]
        public string Name { get; set; }
    }
}