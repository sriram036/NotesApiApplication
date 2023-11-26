using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusiness notesBusiness;

        public NotesController(INotesBusiness notesBusiness)
        {
            this.notesBusiness = notesBusiness;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateNote")]
        public ActionResult CreateNote(NotesModel notesModel)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            NotesEntity notesEntity = notesBusiness.CreateNote(UserId, notesModel);
            if(notesEntity == null)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note Not Created", Data = "User Not Found" });
            }
            else
            {
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note Added Successfully", Data = notesEntity });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetNotesById")]
        public List<NotesEntity> GetNotes()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            return notesBusiness.GetNotes(UserId);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateNote")]
        public ActionResult UpdateNote(int NotesId, NotesModel notesModel)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);

            NotesEntity notesEntity = notesBusiness.UpdateNote(NotesId, notesModel, UserId);

            if(notesEntity == null)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note Not Updated", Data = "UserId is not matched" });
            }
            else
            {
                return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note Updated Successfully", Data = notesEntity});
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteNote")]
        public ActionResult DeleteNote(int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool IsNoteDeleted = notesBusiness.DeleteNote(NotesId, UserId);

            if(IsNoteDeleted)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Note Deleted", Data = "UserId Matched"});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note not Deleted", Data = "UserId is not Matched" });
            }
        }
    }
}
