using System.Collections.Generic;
using System.Linq;
using LibraryWinForms.Models;

namespace LibraryWinForms.Repositories
{
    public class LibraryRepository
    {
        private List<LibraryItem> items = new List<LibraryItem>();

        public void Add(LibraryItem item)
        {
            items.Add(item);
        }

        public void Remove(int id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item != null)
                items.Remove(item);
        }

        public List<LibraryItem> GetAll()
        {
            return items;
        }
    }
}