using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollaboratorBusiness : ICollaboratorBusiness
    {
        private readonly ICollaboratorRepo collaboratorRepo;

        public CollaboratorBusiness(ICollaboratorRepo collaboratorRepo)
        {
            this.collaboratorRepo = collaboratorRepo;
        }

        public CollaboratorEntity AddCollaborator(string EmailId, int NoteId, int UserId)
        {
            return collaboratorRepo.AddCollaborator(EmailId, NoteId, UserId);
        }

        public List<string> GetCollaborators(int NoteId, int UserId)
        {
            return collaboratorRepo.GetCollaborators(NoteId, UserId);
        }

        public bool DeleteCollaborator(int CollaboratorId, int NoteId, int UserId)
        {
            return collaboratorRepo.DeleteCollaborator(CollaboratorId, NoteId, UserId);
        }
    }
}
