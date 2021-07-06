using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
           _userRepository = userRepository;
           _likesRepository = likesRepository;
        }

        [HttpPost("{userName}")]

        public async Task<ActionResult> AddLike(string userName)
        {
            var sourceUserId = User.GetUserId();
            var likedUser = await _userRepository.GetUserByName(userName);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if(likedUser == null) return NotFound();

            if(sourceUser.UserName == userName) return BadRequest("You cannot like yourdwlf !!!! Atleast not in this app");

             var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);

            // TODO: Unlike option
             if(userLike != null) return BadRequest("User has already been liked");

             userLike = new UserLike
             {
                 SourceUserId = sourceUserId,
                 LikedUserId = likedUser.Id
             };

             sourceUser.LikedUsers.Add(userLike);

            //TODO: Change the savechanges to like Repository
             if(await _userRepository.SaveAllChanges()) return Ok();

             return BadRequest("Unable to like the user!!! Try Again later");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);

            return Ok(users);
        }
    }
}