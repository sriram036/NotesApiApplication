using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FunDooDBContext funDooDBContext;

        public NotesRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public NotesEntity CreateNote(int UserId, NotesModel notesModel)
        {
            NotesEntity notesEntity = new NotesEntity();
            notesEntity.Title = notesModel.Title;
            notesEntity.Description = notesModel.Description;
            notesEntity.Reminder = notesModel.Reminder;
            notesEntity.Colour = notesModel.Colour;
            notesEntity.Image = notesModel.Image;
            notesEntity.CreatedAt = DateTime.Now;
            notesEntity.UpdatedAt = DateTime.Now;
            notesEntity.UserId = UserId;
            funDooDBContext.Notes.Add(notesEntity);
            funDooDBContext.SaveChanges();
            return notesEntity;
        }

        public List<NotesEntity> GetNotes(int UserId)
        {
            List<NotesEntity> Notes = funDooDBContext.Notes.ToList().FindAll(user => user.UserId == UserId);
            if(Notes != null)
            {
                return Notes;
            }
            else
            {
                return null;
            }
        }

        public NotesEntity UpdateNote(int NotesId, NotesModel notesModel, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if(notesEntity.UserId == UserId)
            {
                notesEntity.Title = notesModel.Title;
                notesEntity.Description = notesModel.Description;
                notesEntity.Reminder = notesEntity.Reminder;
                notesEntity.Colour = notesModel.Colour;
                notesEntity.Image = notesModel.Image;
                notesEntity.UpdatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return notesEntity;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if( notesEntity.UserId == UserId && notesEntity.IsTrash == true)
            {
                funDooDBContext.Notes.Remove(notesEntity);
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PinNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if (notesEntity.UserId == UserId)
            {
                if (notesEntity.IsPin)
                {
                    notesEntity.IsPin = false;
                    funDooDBContext.SaveChanges();
                    return false;
                }
                else
                {
                    notesEntity.IsPin = true;
                    funDooDBContext.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ArchiveNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if (notesEntity.UserId == UserId)
            {
                if (notesEntity.IsArchive)
                {
                    notesEntity.IsArchive = false;
                    funDooDBContext.SaveChanges();
                    return false;
                }
                else
                {
                    notesEntity.IsArchive = true;
                    funDooDBContext.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool TrashNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if (notesEntity.UserId == UserId)
            {
                if (notesEntity.IsTrash)
                {
                    notesEntity.IsTrash = false;
                    funDooDBContext.SaveChanges();
                    return false;
                }
                else
                {
                    notesEntity.IsTrash = true;
                    funDooDBContext.SaveChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddColourInNote(int NotesId, string Colour, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if(notesEntity.UserId == UserId)
            {
                notesEntity.Colour = Colour;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RestoreNote(int NotesId, int UserId)
        {
            NotesEntity notesEntity = funDooDBContext.Notes.FirstOrDefault(note => note.NotesId == NotesId);
            if(notesEntity.IsTrash && notesEntity.UserId == UserId)
            {
                notesEntity.IsTrash = false;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                notesEntity.IsTrash = true;
                funDooDBContext.SaveChanges();
                return false;
            }
        }
    }
}
