using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NotesBusiness : INotesBusiness
    {
        private readonly INotesRepo notesRepo;

        public NotesBusiness(INotesRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }

        public NotesEntity CreateNote(int UserId, NotesModel notesModel)
        {
            return notesRepo.CreateNote(UserId, notesModel);
        }

        public List<NotesEntity> GetNotes(int UserId)
        {
            return notesRepo.GetNotes(UserId);
        }

        public NotesEntity UpdateNote(int NotesId, NotesModel notesModel, int UserId)
        {
            return notesRepo.UpdateNote(NotesId, notesModel, UserId);
        }

        public bool DeleteNote(int NotesId, int UserId)
        {
            return notesRepo.DeleteNote(NotesId, UserId);
        }

        public bool PinNote(int NotesId, int UserId)
        {
            return notesRepo.PinNote(NotesId, UserId);
        }

        public bool ArchiveNote(int NotesId, int UserId)
        {
            return notesRepo.ArchiveNote(NotesId, UserId);
        }

        public bool TrashNote(int NotesId, int UserId)
        {
            return notesRepo.TrashNote(NotesId, UserId);
        }

        public bool AddColourInNote(int NotesId, string Colour, int UserId)
        {
            return notesRepo.AddColourInNote(NotesId, Colour, UserId);
        }

        public bool RestoreNote(int NotesId, int UserId)
        {
            return notesRepo.RestoreNote(NotesId, UserId);
        }
    }
}
