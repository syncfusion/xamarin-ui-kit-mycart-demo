using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyCart.Core.Interface
{
    public interface ICategory
    {
        string Icon { get; set; }
        string Name { get; set; }
    }
}
