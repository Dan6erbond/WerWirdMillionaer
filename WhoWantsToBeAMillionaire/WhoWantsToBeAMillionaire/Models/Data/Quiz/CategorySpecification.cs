using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class CategorySpecification : ISpecification<Category>
    {
        private readonly int? _categoryId;
        private readonly string _categoryName;

        public CategorySpecification(int? categoryId = null, string categoryName = null)
        {
            _categoryId = categoryId;
            _categoryName = categoryName;
        }

        public bool Specificied(Category item)
        {
            if (_categoryId != null && _categoryId != item.CategoryId)
            {
                return false;
            }
            
            if (_categoryName != null && _categoryName != item.Name)
            {
                return false;
            }

            return true;
        }
    }
}