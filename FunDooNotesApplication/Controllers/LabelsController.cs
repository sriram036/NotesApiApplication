using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

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
            bool labelEntity = labelBusiness.AddLabel(LabelName, NoteId, UserId);
            if (labelEntity)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Label Added", Data = LabelName });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label not Added", Data = LabelName + " is Already Exist" });
            }
        }

        [HttpGet]
        [Route("GetLabels")]
        public List<LabelEntity> GetLabels()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            List<LabelEntity> Labels = labelBusiness.GetLabels(UserId);
            if(Labels != null)
            {
                return Labels;
            }
            else
            {
                return null;
            }
        }

        [HttpPut]
        [Route("UpdateLabel")]
        public ActionResult Updatelabel(int LabelId, int NoteId, string LabelName)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            LabelEntity labelEntity = labelBusiness.UpdateLabel(LabelId, NoteId, UserId, LabelName);
            if(labelEntity != null)
            {
                return Ok(new ResponseModel<LabelEntity> { IsSuccess = true, Message = "Label Updated Successfully", Data = labelEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label not Updated", Data = "UserId or NoteId not Matched" });
            }
        }

        [HttpDelete]
        [Route("DeleteLabel")]
        public ActionResult Deletelabel(int LabelId, int NoteId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool IsLabelDeleted = labelBusiness.DeleteLabel(LabelId, NoteId, UserId);
            if(IsLabelDeleted)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Label Deleted Successfully", Data = "NoteId Matched"});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label Not Deleted", Data = "NoteId not Matched" });
            }
        }
    }
}
