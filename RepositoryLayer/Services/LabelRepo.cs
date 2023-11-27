using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
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

        public LabelEntity AddLabel(string LabelName, int NoteId, int UserId)
        {
            LabelEntity labelEntity = new LabelEntity();
            labelEntity.LabelName = LabelName;
            labelEntity.CreatedAt = DateTime.Now;
            labelEntity.UpdatedAt = DateTime.Now;
            labelEntity.NotesId = NoteId;
            labelEntity.UserId = UserId;
            funDooDBContext.Labels.Add(labelEntity);
            funDooDBContext.SaveChanges();
            return labelEntity;
        }
    }
}
