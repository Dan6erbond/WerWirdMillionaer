using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class CategoryGameSpecification : ISpecification<CategoryGame>
    {
        private readonly int? _categoryId;
        private readonly int? _gameId;
        
        public CategoryGameSpecification(int? categoryId = null, int? gameId = null)
        {
            _categoryId = categoryId;
            _gameId = gameId;
        }

        public bool Specificied(CategoryGame item)
        {
            if (_categoryId != null && _categoryId != item.CategoryId)
            {
                return false;
            }
            
            if (_gameId != null && _gameId != item.GameId)
            {
                return false;
            }

            return true;
        }
    }
}