using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {

        public static List<Category> GetCategories()
        {
            var listCategories = new List<Category>();
            try
            {
                using (var context = new MyDBContext())
                {
                    listCategories = context.Categories.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listCategories;
        }

    }
}
