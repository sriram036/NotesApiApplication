using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBusiness : ILabelBusiness
    {
        private readonly ILabelRepo labelRepo;

        public LabelBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }

        public bool AddLabel(string LabelName, int NoteId, int UserId)
        {
            return labelRepo.AddLabel(LabelName, NoteId, UserId);
        }

        public List<LabelEntity> GetLabels(int UserId)
        {
            return labelRepo.GetLabels(UserId);
        }

        public LabelEntity UpdateLabel(int LabelId, int NoteId, int UserId, string LabelName)
        {
            return labelRepo.UpdateLabel(LabelId, NoteId, UserId, LabelName);
        }

        public bool DeleteLabel(int LabelId, int NoteId, int UserId)
        {
            return labelRepo.DeleteLabel(LabelId, NoteId, UserId);
        }
    }
}
