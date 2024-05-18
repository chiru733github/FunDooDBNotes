using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.Models;
using RepositoryLayer.Entities;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBusiness _labelBusiness;
        public LabelController(ILabelBusiness labelBusiness)
        {
            _labelBusiness = labelBusiness;
        }
        [Authorize]
        [HttpPost]
        [Route("Add Label")]
        public ActionResult AddLabel(int NoteId, string Labelname)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                LabelsEntity labelEntity = _labelBusiness.AddLabel(UserId, NoteId, Labelname);
                if (labelEntity != null)
                {
                    return Ok(new ResponseModel<LabelsEntity> { IsSuccess = true, Message = "Successfull Added label", Data = labelEntity });
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
        [HttpGet("GetByLabelname")]
        public ActionResult GetByLabelname(string Labelname)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            List<LabelsEntity> list = _labelBusiness.GetByLabelname(UserId, Labelname);
            if (list != null)
            {
                return Ok(new ResponseModel<List<LabelsEntity>> { IsSuccess = true, Message = "Successfull Added label", Data = list });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "labelname doesn't exists." });
            }

        }
        [Authorize]
        [HttpPut("RemovelabelByNotesId")]
        public ActionResult RemovelabelByNotesId(int notesId,string labelname)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool result = _labelBusiness.RemoveLabelbyNotesID(UserId, notesId,labelname);
            if(result)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "success", Data = "labelname removed in given notesid" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "neither labelname nor notesId doesn't exists." });
            }
        }
    }
}
