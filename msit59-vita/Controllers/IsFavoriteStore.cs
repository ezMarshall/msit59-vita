using msit59_vita.Models;
using System.Diagnostics.Metrics;

namespace msit59_vita.Controllers
{
	public class IsFavoriteStore
	{
		private readonly VitaContext _context;
        public IsFavoriteStore(VitaContext context)
        {
			_context = context;
;
        }

		public bool FavoriteStore(int customerId, int storeId)
		{

			var favoriteStore = _context.Favorites.FirstOrDefault(x => x.CustomerId == customerId && x.StoreId == storeId);
			return favoriteStore != null;
			
		}
	}
}
