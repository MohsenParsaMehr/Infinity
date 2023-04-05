using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infinity.Classes
{
    public class SearchCriterias
    {
        public String Criteria;
        public Boolean showNew;
        public Boolean showOnSale;
        public Boolean showInStock;
        public List<Entity> Brands = new List<Entity>();
        public int? PriceFrom;
        public int? PriceTo;
    }
}