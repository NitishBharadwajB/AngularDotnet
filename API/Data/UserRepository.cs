using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper ;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAllUserAsync()
        {
            return await _context.User
            .Include(x => x.Photos)
            .ToListAsync();
        }

        public async Task<MemberDto> GetMemberAsync(string userName)
        {
            return await _context.User.Where(x => x.UserName == userName).ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(Userparams userParams)
        {

            var query = _context.User.AsQueryable();

            query = query.Where(x => x.UserName != userParams.UserName);
            query = query.Where(x => x.Gender == userParams.Gender);

            var maxDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var minDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(x => x.DateOfBirth >= maxDob && x.DateOfBirth <= minDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking()
            ,userParams.PageNumber,userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.User
            .FindAsync(id);
        }

        public async Task<AppUser> GetUserByName(string userName)
        {
            return await _context.User
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> SaveAllChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}