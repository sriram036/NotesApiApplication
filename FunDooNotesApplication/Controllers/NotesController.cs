using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;

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
        [HttpGet]
        [Route("GetNotesById")]
        public List<NotesEntity> GetNotes()
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            List<NotesEntity> Notes = notesBusiness.GetNotes(UserId);
            if (Notes != null)
            {
                return Notes;
            }
            else
            {
                return null;
            }
            
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

        [Authorize]
        [HttpPut]
        [Route("PinNote")]
        public ActionResult PinNote(int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            int IsPin = notesBusiness.PinNote(NotesId, UserId);
            if(IsPin == 2)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Data Changed", Data = "Note is Pinned" });
            }
            else if (IsPin == 1)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Changed", Data = "Note is UnPinned" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Not Changed", Data = "UserId Not Matched" });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ArchiveNote")]
        public ActionResult ArchiveNote(int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            int IsArchive = notesBusiness.ArchiveNote(NotesId, UserId);
            if (IsArchive == 2)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Data Changed", Data = "Note is Archived" });
            }
            else if (IsArchive == 1)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Changed", Data = "Note is UnArchived" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Not Changed", Data = "UserId Not Matched" });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("TrashNote")]
        public ActionResult TrashNote(int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            int IsTrash = notesBusiness.TrashNote(NotesId, UserId);
            if (IsTrash == 2)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Data Changed", Data = "Note is Trashed" });
            }
            else if (IsTrash == 1)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Changed", Data = "Note is UnTrashed" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Not Changed", Data = "UserId Not Matched" });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("AddColour")]
        public ActionResult AddColourInNote(int NotesId, string Colour)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            bool AddedColour = notesBusiness.AddColourInNote(NotesId, Colour, UserId);

            if(AddedColour)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Colour is Added", Data = Colour});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Colour is not Added", Data = "User Not Found"});
            }
        }

        [Authorize]
        [HttpPut]
        [Route("RestoreNote")]
        public ActionResult RestoreNote(int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            int IsRestored = notesBusiness.RestoreNote(NotesId, UserId);

            if (IsRestored == 1)
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Note is Restored", Data = "UserId Matched" });
            }
            else if (IsRestored == 2)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note is not Restored", Data = "Note is not in Trash" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Data Not Changed", Data = "UserId Not Matched" });
            }
        }
    }
}
