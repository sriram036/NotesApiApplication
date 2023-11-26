using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepo
    {
        NotesEntity CreateNote(int UserId, NotesModel notesModel);

        List<NotesEntity> GetNotes(int UserId);

        NotesEntity UpdateNote(int NotesId, NotesModel notesModel, int UserId);

        bool DeleteNote(int NotesId, int UserId);
    }
}