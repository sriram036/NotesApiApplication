using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepo
    {
        CollaboratorEntity AddCollaborator(string EmailId, int NoteId, int UserId);

        List<string> GetCollaborators(int NoteId, int UserId);

        bool DeleteCollaborator(int CollaboratorId, int NoteId, int UserId);
    }
}