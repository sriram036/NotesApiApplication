using Microsoft.AspNetCore.Http;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface INotesBusiness
    {
        NotesEntity CreateNote(int UserId, NotesModel notesModel);

        List<NotesEntity> GetNotes(int UserId);

        NotesEntity UpdateNote(int NotesId, NotesModel notesModel, int UserId);

        bool DeleteNote(int NotesId, int UserId);

        int PinNote(int NotesId, int UserId);

        int ArchiveNote(int NotesId, int UserId);

        int TrashNote(int NotesId, int UserId);

        bool AddColourInNote(int NotesId, string Colour, int UserId);

        int RestoreNote(int NotesId, int UserId);

        bool AddImage(int NoteId, int UserId, IFormFile Image);

        bool AddReminder(int NoteId, int UserId, DateTime Reminder);
    }
}