using System.Text;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using GreenPipes.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Model_Layer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusiness _NotesBusiness;
        private readonly IDistributedCache _cache;
        private readonly FunDooDBContext _funDooDBContext;
        public NotesController(INotesBusiness notesBusiness, IDistributedCache cache, FunDooDBContext _funDooDBContext)
        {
            _NotesBusiness = notesBusiness;
            _cache = cache;
            this._funDooDBContext = _funDooDBContext;
        }
        [Authorize]
        [HttpPost]
        [Route("CreateNote")]
        public ActionResult CreateNote(NotesModel model)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                NotesEntity notesEntity = _NotesBusiness.CreateNote(UserId, model);
                if (notesEntity != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note added successfully", Data = notesEntity });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note did not added", Data = "userId doesn't exists" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPost("TogglePin")]
        public IActionResult TogglePin(int noteid)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool pin = _NotesBusiness.IsPin(UserId, noteid);
            if (pin == true)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "TogglePinned Successsfully", Data = pin });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "TogglePin Failed", Data = pin });
            }
        }
        [Authorize]
        [HttpPost("ToggleArchive")]
        public IActionResult ToggleArchive(int noteid)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);

            bool pin = _NotesBusiness.IsArchive(UserId, noteid);
            if (pin == true)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Toggle Archive Successsfully", Data = pin });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Toggle Archive Failed", Data = pin });
            }
        }
        [Authorize]
        [HttpPost("Toggle Trash")]
        public IActionResult ToggleTrash(int noteid)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool pin = _NotesBusiness.IsTrash(UserId, noteid);
            if (pin == true)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Toggle Trash Successsfully", Data = pin });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Toggle Trash Failed", Data = pin });
            }
        }
        [Authorize]
        [HttpPost("AddColor")]
        public IActionResult AddColor(int NotesId, string Color)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            string notesEntity = _NotesBusiness.AddColor(UserId, NotesId, Color);
            if (notesEntity == "Success")
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Added color successfull", Data = notesEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding color Failed", Data = notesEntity });
            }
        }
        [Authorize]
        [HttpPost("AddImage")]
        public IActionResult AddImage(int NotesId, IFormFile Image)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool notesEntity = _NotesBusiness.AddImage(UserId, NotesId, Image);
            if (notesEntity)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Added Image successfull", Data = notesEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Adding Image Failed", Data = notesEntity });
            }
        }
        [Authorize]
        [HttpPost("AddRemainder")]
        public IActionResult AddRemainder(int NotesId, DateTime dateTime)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            string notesEntity = _NotesBusiness.AddRemainder(UserId, NotesId, dateTime);
            if (notesEntity == "Successfull added")
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Added Remainder successfull", Data = notesEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Adding Remainder Failed", Data = notesEntity });
            }
        }
        [HttpGet("GetByUserId")]
        public IActionResult GetByUserId(int UserId)
        {
            string GetByUser = _NotesBusiness.GetByUser(UserId);
            if(GetByUser != null)
            {
                return Ok(new ResponseModel<string> { IsSuccess=true,Message="get by user",Data = GetByUser});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess=false,Message="No data present in User table.",Data="Userid doesnot exist."});
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllUserWithRedis()
        {
            string cacheKey = "NotesList";
            string serializationUserList;
            var NotesList = new List<NotesEntity>();
            byte[] RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList = Encoding.UTF8.GetString(RedisUsersList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializationUserList);
            }
            else
            {
                NotesList = _funDooDBContext.Notes.ToList();
                serializationUserList = JsonConvert.SerializeObject(NotesList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList, options);
            }
            return Ok(NotesList);
        }
        [HttpGet("Username_Count")]
        public ActionResult CountNote(int UserId)
        {
            var result = _NotesBusiness.CountNote(UserId);
            if(result != null)
            {
                return Ok(new ResponseModel<Username_count> { IsSuccess=true,Message="success",Data=result});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data="User doesn't exist."});
            }
        }
        [HttpGet("CheckNotes")]
        public ActionResult CheckNotes(int UserId) 
        {
            var result = _NotesBusiness.CheckOneOrmoreNote(UserId);
            if (result.Count()==1)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "success", Data = "Single data" });
            }
            else if(result.Count()>1)
            {
                return Ok(new ResponseModel<List<NotesEntity>> { IsSuccess = false, Message = "Success", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "failed", Data = "User doesn't exist." });
            }
        }
    }
}
