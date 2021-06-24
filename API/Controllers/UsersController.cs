using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRep;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRep, IMapper mapper)
        {
            _mapper = mapper;
            _userRep = userRep;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRep.GetMembersAsync();

            return Ok(users);
        }

        // [HttpGet("{id}")]
        // public async  Task<ActionResult<AppUser>> GetUserByID(int id)
        // {
        //     return await _userRep.GetUserByIdAsync(id);
        // }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUserName(string username)
        {
            return  await _userRep.GetMemberAsync(username);
        }

    }
}