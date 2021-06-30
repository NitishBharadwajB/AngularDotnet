using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllChanges();
        Task<IEnumerable<AppUser>> GetAllUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByName(string userName);

        Task<PagedList<MemberDto>> GetMembersAsync(Userparams userParams);
        Task<MemberDto> GetMemberAsync(string userName);
    }
}