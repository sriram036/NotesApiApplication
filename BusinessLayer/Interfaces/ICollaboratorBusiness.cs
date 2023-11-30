using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorBusiness
    {
        CollaboratorEntity AddCollaborator(string EmailId, int NoteId, int UserId);

        List<string> GetCollaborators(int NoteId, int UserId);

        bool DeleteCollaborator(int CollaboratorId, int NoteId, int UserId);
    }
}