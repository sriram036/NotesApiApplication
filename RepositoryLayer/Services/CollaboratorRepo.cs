using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollaboratorRepo : ICollaboratorRepo
    {
        private readonly FunDooDBContext funDooDBContext;

        public CollaboratorRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public CollaboratorEntity AddCollaborator(string EmailId, int NoteId, int UserId)
        {
            List<NotesEntity> notes = funDooDBContext.Notes.ToList().FindAll(User => User.UserId == UserId);
            NotesEntity notesEntity = notes.Find(Note => Note.NotesId == NoteId);
            if (notesEntity is null)
            {
                return null;
            }
            else
            {
                CollaboratorEntity collaboratorEntity = new CollaboratorEntity();
                collaboratorEntity.Email = EmailId;
                collaboratorEntity.NoteId = NoteId;
                collaboratorEntity.UserId = UserId;
                funDooDBContext.Collaborators.Add(collaboratorEntity);
                funDooDBContext.SaveChanges();
                return collaboratorEntity;
            }
        }

        public List<string> GetCollaborators(int NoteId, int UserId)
        {
            List<string> Emails = funDooDBContext.Collaborators.ToList().FindAll(col => col.NoteId == NoteId && col.UserId == UserId).Select(email => email.Email).ToList();
            if(Emails is null)
            {
                return null;
            }
            else
            {
                return Emails;
            }
        }

        public bool DeleteCollaborator(int CollaboratorId, int NoteId, int UserId)
        {
            CollaboratorEntity collaboratorEntity = funDooDBContext.Collaborators.FirstOrDefault(user => user.NoteId == NoteId && user.UserId == UserId);
            if (collaboratorEntity is null)
            {
                return false;
            }
            else
            {
                funDooDBContext.Collaborators.Remove(collaboratorEntity);
                funDooDBContext.SaveChanges();
                return true;
            }
        }
    }
}
