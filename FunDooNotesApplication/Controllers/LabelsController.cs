using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;

namespace FunDooNotesApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelBusiness labelBusiness;

        public LabelsController(ILabelBusiness labelBusiness)
        {
            this.labelBusiness = labelBusiness;
        }

        [HttpPost]
        [Route("AddLabel")]
        public ActionResult AddLabel(string LabelName, int NoteId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            LabelEntity labelEntity = labelBusiness.AddLabel(LabelName, NoteId, UserId);

            if(labelEntity != null)
            {
                return Ok(new ResponseModel<LabelEntity> { IsSuccess = true, Message = "Label Addded", Data = labelEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label Not Added", Data = "UserId not Matched"});
            }
        }
    }
}
