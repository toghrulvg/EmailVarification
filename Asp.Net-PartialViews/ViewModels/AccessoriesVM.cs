using Asp.Net_PartialViews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net_PartialViews.ViewModels
{
    public class AccessoriesVM
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
