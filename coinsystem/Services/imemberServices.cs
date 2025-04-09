using System.Collections.Generic;
using coinsystem.Models;

namespace coinsystem.Services

{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<Member> GetMemberByIdAsync(int id);
        Task<Member> AddMemberAsync(Member member);
        Task<Member> UpdateMemberAsync(Member member);
        Task DeleteMemberAsync(Member member);
        
        Task<Member> GetMemberByTell(string tell);
        
        Task<Member> AddCredit(string tell, double credit);
        
        Task<Member> SubCredit(int id, double credit);
    }
}