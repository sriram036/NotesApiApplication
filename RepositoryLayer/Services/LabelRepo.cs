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

        public bool AddLabel(string LabelName, int NoteId, int UserId)
        {
            if(funDooDBContext.Labels is null)
            {
                LabelEntity labelEntity = new LabelEntity();
                labelEntity.LabelName = LabelName;
                labelEntity.CreatedAt = DateTime.Now;
                labelEntity.UpdatedAt = DateTime.Now;
                labelEntity.NotesId = NoteId;
                labelEntity.UserId = UserId;
                funDooDBContext.Labels.Add(labelEntity);
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                List<LabelEntity> labels = funDooDBContext.Labels.ToList().FindAll(Note => Note.NotesId == NoteId && Note.UserId == UserId);
                if (labels is null)
                {
                    LabelEntity labelEntity = new LabelEntity();
                    labelEntity.LabelName = LabelName;
                    labelEntity.CreatedAt = DateTime.Now;
                    labelEntity.UpdatedAt = DateTime.Now;
                    labelEntity.NotesId = NoteId;
                    labelEntity.UserId = UserId;
                    funDooDBContext.Labels.Add(labelEntity);
                    funDooDBContext.SaveChanges();
                    return true;
                }
                else
                {
                    LabelEntity labelEntity = labels.Find(label => label.LabelName == LabelName && label.UserId == UserId);
                    if(labelEntity is null)
                    {
                        LabelEntity label = new LabelEntity();
                        label.LabelName = LabelName;
                        label.CreatedAt = DateTime.Now;
                        label.UpdatedAt = DateTime.Now;
                        label.NotesId = NoteId;
                        label.UserId = UserId;
                        funDooDBContext.Labels.Add(label);
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
