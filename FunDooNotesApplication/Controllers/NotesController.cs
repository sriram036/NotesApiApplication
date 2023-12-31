﻿using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
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

        [HttpPost]
        [Route("CreateNote")]
        public ActionResult CreateNote(NotesModel notesModel)
        {
            try
            {
                //int UserId = int.Parse(User.FindFirst("UserId").Value);
                int UserId = (int)HttpContext.Session.GetInt32("UserId");
                NotesEntity notesEntity = notesBusiness.CreateNote(UserId, notesModel);
                if (notesEntity == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note Not Created", Data = "User Not Found" });
                }
                else
                {
                    return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note Added Successfully", Data = notesEntity });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetNotesById")]
        public List<NotesEntity> GetNotes()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateNote")]
        public ActionResult UpdateNote(int NotesId, NotesModel notesModel)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);

                NotesEntity notesEntity = notesBusiness.UpdateNote(NotesId, notesModel, UserId);

                if (notesEntity == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note Not Updated", Data = "UserId is not matched" });
                }
                else
                {
                    return Ok(new ResponseModel<NotesEntity> { IsSuccess = true, Message = "Note Updated Successfully", Data = notesEntity });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteNote")]
        public ActionResult DeleteNote(int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool IsNoteDeleted = notesBusiness.DeleteNote(NotesId, UserId);

                if (IsNoteDeleted)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Note Deleted", Data = "UserId Matched" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Note not Deleted", Data = "UserId is not Matched" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("PinNote")]
        public ActionResult PinNote(int NotesId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                int IsPin = notesBusiness.PinNote(NotesId, UserId);
                if (IsPin == 2)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ArchiveNote")]
        public ActionResult ArchiveNote(int NotesId)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("TrashNote")]
        public ActionResult TrashNote(int NotesId)
        {
            try
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("AddColour")]
        public ActionResult AddColourInNote(int NotesId, string Colour)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool AddedColour = notesBusiness.AddColourInNote(NotesId, Colour, UserId);

                if (AddedColour)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Colour is Added", Data = Colour });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Colour is not Added", Data = "User Not Found" });
                }
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        [Authorize]
        [HttpPut]
        [Route("RestoreNote")]
        public ActionResult RestoreNote(int NotesId)
        {
            try
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("AddImage")]
        public ActionResult AddImage(int NoteId, IFormFile Image)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool IsImageAdded = notesBusiness.AddImage(NoteId, UserId, Image);
                if (IsImageAdded)
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Image Added Successfully", Data = "UserId Matched" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Image Not Added", Data = "UserId not Matched" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("AddReminder")]
        public ActionResult AddReminder(int NoteId, DateTime Reminder)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("UserId").Value);
                bool IsReminderAdded = notesBusiness.AddReminder(NoteId, UserId, Reminder);
                if (IsReminderAdded)
                {
                    return Ok(new ResponseModel<DateTime> { IsSuccess = true, Message = "Reminder Added", Data = Reminder });
                }
                else
                {
                    return BadRequest(new ResponseModel<DateTime> { IsSuccess = false, Message = "Reminder not Added", Data = Reminder });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
