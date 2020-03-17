using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class CategorySpecification : ISpecification<Category>
    {
        private readonly int? _categoryId;

        public CategorySpecification(int? categoryId = null)
        {
            _categoryId = categoryId;
        }

        public bool Specificied(Category item)
        {
            if (_categoryId != null && _categoryId != item.CategoryId)
            {
                return false;
            }

            return true;
        }
    }
}