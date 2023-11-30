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
            int labelEntity = labelBusiness.AddLabel(LabelName, NoteId, UserId);

            switch (labelEntity)
            {
                case 1:
                    {
                        return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Label Addded", Data = LabelName });
                    }break;
                case 2:
                    {
                        return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label Not Added", Data = "NoteId not Matched" });
                    }break;
                case 3:
                    {
                        return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label Not Added", Data = "UserId not Matched" });
                    }break;
                case 4:
                    {
                        return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Label Not Added", Data = "Label Already Available" });
                    }break;
                default:
                    {
                        return null;
                    }
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
