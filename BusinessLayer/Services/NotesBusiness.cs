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
    }
}
