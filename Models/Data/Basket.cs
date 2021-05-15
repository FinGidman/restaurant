using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Models.Data
{
    public class Basket
    {
        private List<Item> basketCollection = new List<Item>();
        public void AddItem(Dish dish)
        {
            Item item = basketCollection
                .Where(a => a.Dish.Id == dish.Id)
                .FirstOrDefault();
            if (item == null)
            {
                basketCollection.Add(new Item { Dish = dish, Count = 1 });
            }
            else
            {
                item.Count++;
            }
        }

        public void RemoveItem(int id)
        {
            Item item = basketCollection
               .Where(a => a.Dish.Id == id)
               .FirstOrDefault();
            if (item != null)
            {
                if (item.Count == 1)
                {
                    basketCollection.Remove(item);
                }
                else
                {
                    item.Count--;
                }
            }
        }

        public void RemoveLine(Dish dish)
        {
            basketCollection.RemoveAll(l => l.Dish.Id == dish.Id);
        }

        public decimal ComputeTotalValue()
        {
            return basketCollection.Sum(a => a.Dish.Price *a.Count);
        }

        public void Clear()
        {
            basketCollection.Clear();
        }

        public string GetDishes()
        {
            return JsonConvert.SerializeObject(basketCollection);
        }

        public IEnumerable<Item> Items
        {
            get { return basketCollection; }
        }
    }

    public class Item
    {
        public Dish Dish { get; set; }
        public int Count { get; set; }
    }
}
