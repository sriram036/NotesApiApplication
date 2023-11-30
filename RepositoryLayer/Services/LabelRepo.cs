using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepo : ILabelRepo
    {
        private readonly FunDooDBContext funDooDBContext;

        public LabelRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public int AddLabel(string LabelName, int NoteId, int UserId)
        {
            List<NotesEntity> notes = funDooDBContext.Notes.ToList().FindAll(User => User.UserId == UserId);
            if(notes.Any())
            {
                NotesEntity notesEntity = notes.Find(note => note.NotesId == NoteId);
                if (notesEntity != null)
                {
                    List<LabelEntity> labels = funDooDBContext.Labels.ToList().FindAll(label => label.NotesId == NoteId);
                    LabelEntity label = labels.Find(name => name.LabelName == LabelName);
                    if(label is null)
                    {
                        LabelEntity labelEntity = new LabelEntity();
                        labelEntity.LabelName = LabelName;
                        labelEntity.CreatedAt = DateTime.Now;
                        labelEntity.UpdatedAt = DateTime.Now;
                        labelEntity.NotesId = NoteId;
                        labelEntity.UserId = UserId;
                        funDooDBContext.Labels.Add(labelEntity);
                        funDooDBContext.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 4; 
                    }
                    
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 3;
            }
        }

        public List<LabelEntity> GetLabels(int UserId)
        {
            List<LabelEntity> labelEntities = funDooDBContext.Labels.ToList().FindAll(label => label.UserId == UserId);
            return labelEntities;
        }

        public LabelEntity UpdateLabel(int LabelId, int NoteId, int UserId, string LabelName)
        {
            LabelEntity labelEntity = funDooDBContext.Labels.Find(LabelId);
            if(labelEntity.NotesId  == NoteId && labelEntity.UserId == UserId)
            {
                labelEntity.LabelName = LabelName;
                labelEntity.UpdatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return labelEntity;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteLabel(int LabelId, int NoteId, int UserId)
        {
            LabelEntity labelEntity = funDooDBContext.Labels.Find(LabelId);
            if(labelEntity.NotesId == NoteId && labelEntity.UserId == UserId)
            {
                funDooDBContext.Labels.Remove(labelEntity);
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
