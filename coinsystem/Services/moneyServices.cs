using coinsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace coinsystem.Services
{
    public class MoneyService : IMoneyService
    {
        private readonly AppDbContext _context;


        public MoneyService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Money>> GetAllMoneyAsync()
        {
            return await _context.Money.ToListAsync(); //
        }


        public async Task<Money> GetMoneyByIdAsync(int id)
        {
            return await _context.Money.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Money> GetMoneyByPriceAsync(double price)
        {
            return await _context.Money.FirstOrDefaultAsync(m => m.Price == price);
        }
        
        public async Task<Money> AddMoneyAsync(Money money)
        {
            _context.Money.Add(money);
            await _context.SaveChangesAsync();
            return money;
        }
        
        public async Task<Money> UpdateMoneyAsync(Money money)
        {
            _context.Money.Update(money);
            await _context.SaveChangesAsync();
            return money;
        }

        public async Task DeleteMoneyAsync(Money money)
        {
            _context.Money.Remove(money);
            await _context.SaveChangesAsync();
        }
        
        public async Task<Money> AddCoin(int id, int amount)
        {
            _context.Entry(await GetMoneyByIdAsync(id)).State = EntityState.Modified;
            var money = await _context.Money.FindAsync(id);
            if (money != null)
            {
                money.Amount += amount;
                await _context.SaveChangesAsync();
            }
            return money;
        }
        
        public async Task<Money> SubCoin(int id, int amount)
        {
            Console.WriteLine("USE THIS");
            Console.WriteLine(id);
            Console.WriteLine(amount);
            var money = await _context.Money.FindAsync(id);
            if (money != null && money.Amount >= amount)
            {
                Console.WriteLine("USE THIS CONTINUE");
                money.Amount -= amount;
                await _context.SaveChangesAsync();
            }
            return money;
        }
}
} 