using coinsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace coinsystem.Services
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Member.ToListAsync();
        }
        
        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _context.Member.FirstOrDefaultAsync(m => m.Id == id);
        }
        
        public async Task<Member> AddMemberAsync(Member member)
        {
            _context.Member.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }
        
        public async Task<Member> UpdateMemberAsync(Member member)
        {
            _context.Member.Update(member);
            await _context.SaveChangesAsync();
            return member;
        }
        
        public async Task DeleteMemberAsync(Member member)
        {
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
        }
        
        public async Task<Member> GetMemberByTell(string tell)
        {
            return await _context.Member.FirstOrDefaultAsync(m => m.Tell == tell);
        }
        
        public async Task<Member> AddCredit(string tell, double credit)
        {
            _context.Entry(await GetMemberByTell(tell)).State = EntityState.Modified;
            var member = await _context.Member.FindAsync(tell);
            if (member != null)
            {
                member.Credit += credit;
                await _context.SaveChangesAsync();
            }
            return member;
        }

        public async Task<Member> SubCredit(int id, double credit)
        {
            _context.Entry(await GetMemberByIdAsync(id)).State = EntityState.Modified;
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                member.Credit = credit;
                await _context.SaveChangesAsync();
            }

            return member;
        }
    }
}