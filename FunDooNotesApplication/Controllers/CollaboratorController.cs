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
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBusiness collaboratorBusiness;

        public CollaboratorController(ICollaboratorBusiness collaboratorBusiness)
        {
            this.collaboratorBusiness = collaboratorBusiness;
        }

        [HttpPost]
        [Route("AddCollaborator")]
        public ActionResult AddCollaborator(string EmailId, int NoteId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            CollaboratorEntity collaboratorEntity = collaboratorBusiness.AddCollaborator(EmailId, NoteId, UserId);
            if(collaboratorEntity != null)
            {
                return Ok(new ResponseModel<CollaboratorEntity> { IsSuccess = true, Message = "Collaborator Added", Data = collaboratorEntity });
            }
            else
            {
                return Ok(new ResponseModel<string> { IsSuccess = false, Message = "Collaborator Not Added", Data = "UserId or NoteId not Matched" });
            }
        }

        [HttpPost]
        [Route("GetCollaborators")]
        public List<string> GetCollaborators(int NoteId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            List<string> Collaborators = collaboratorBusiness.GetCollaborators(NoteId, UserId);
            if(Collaborators is null)
            {
                return null;
            }
            else
            {
                return Collaborators;
            }
        }

        [HttpDelete]
        [Route("DeleteCollaborator")]
        public ActionResult DeleteCollaborator(int CollaboratorId, int NoteId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool Isdeleted = collaboratorBusiness.DeleteCollaborator(CollaboratorId, NoteId, UserId);

            if (Isdeleted)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Collaborator Deleted", Data = "NoteId Matched" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Collaborator not Deleted", Data = "NoteId not Matched" });
            }
        }
    }
}
