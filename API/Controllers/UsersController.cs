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
using System.Security.Claims;
using API.Extensions;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRep;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRep, IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _userRep = userRep;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromHeader]Userparams userparams)
        {

            var user = await _userRep.GetUserByName(User.GetUserName());
            userparams.UserName = user.UserName;

            if(string.IsNullOrEmpty(userparams.Gender)){
                userparams.Gender = user.Gender == "male" ? "female" : "male";
            }

            
            var users = await _userRep.GetMembersAsync(userparams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        // [HttpGet("{id}")]
        // public async  Task<ActionResult<AppUser>> GetUserByID(int id)
        // {
        //     return await _userRep.GetUserByIdAsync(id);
        // }

        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUserByUserName(string username)
        {
            return  await _userRep.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRep.GetUserByName(User.GetUserName());
            _mapper.Map(memberUpdateDto, user);
            _userRep.Update(user);

            if(await _userRep.SaveAllChanges()) return NoContent();

            return BadRequest("Update Failed");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRep.GetUserByName(User.GetUserName());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);

            if(await _userRep.SaveAllChanges()){
                //return _mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("GetUser", new {username = user.UserName} , _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem while Uploading Photos");
        } 

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRep.GetUserByName(User.GetUserName()); 

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo.IsMain) return BadRequest("This photo is already the main photo");

            var currentMainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMainPhoto != null) currentMainPhoto.IsMain = false;
            photo.IsMain = true;

            if(await _userRep.SaveAllChanges()) return NoContent();

            return BadRequest("Failed to set Main Photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRep.GetUserByName(User.GetUserName()); 

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("Could not delete the Main Photo");

            if(photo.PublicId != null){
                var result = await _photoService.DeletePhoto(photo.PublicId);
                if(result.Error != null){
                    return BadRequest(result.Error.Message);
                }
            }

            user.Photos.Remove(photo);

            if(await _userRep.SaveAllChanges()) return NoContent();

            return BadRequest("Failed to set Main Photo");
        }

    }
}