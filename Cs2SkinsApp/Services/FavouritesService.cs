namespace Cs2SkinsApp.Services
{
    public class FavouritesService
    {
        private readonly HashSet<string> _favouriteIds = new();

        public IReadOnlyCollection<string> FavouriteIds => _favouriteIds;

        public bool IsFavourite(string id) => _favouriteIds.Contains(id);

        public void ToggleFavourite(string id)
        {
            if (!_favouriteIds.Add(id))
            {
                _favouriteIds.Remove(id);
            }
        }

        public void Clear()
        {
            _favouriteIds.Clear();
        }
    }
}
