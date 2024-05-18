using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Migrations;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBusiness icollaboratorBusiness;
        public CollaboratorController(ICollaboratorBusiness collaboratorBusiness, ILogger<CollaboratorController> logger)
        {
            this.icollaboratorBusiness = collaboratorBusiness;
        }
        [Authorize]
        [HttpPost]
        [Route("Add Collaborator")]
        public ActionResult AddCollaborator(int NoteId, string Email)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                CollaboratorEntity CollaboratorData = icollaboratorBusiness.AddCollaborator(UserId, NoteId, Email);
                if (CollaboratorData != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity> { IsSuccess = true, Message = "Successfull Added label", Data = CollaboratorData });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Neither NotesId nor UserId doesn't exists." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("RemoveCollaboratorByNotesId")]
        public ActionResult RemoveCollaboratorByNotesId(int NoteId,string Email)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool result = icollaboratorBusiness.RemoveCollaboratorByNotesId(UserId, NoteId, Email);
            if (result)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "success", Data = "Collaborator removed in given notesid" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "neither labelname nor notesId doesn't exists." });
            }
        }
    }
}
