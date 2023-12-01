using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBusiness
    {
        bool AddLabel(string LabelName, int NoteId, int UserId);

        List<LabelEntity> GetLabels(int UserId);

        LabelEntity UpdateLabel(int LabelId, int NoteId, int UserId, string LabelName);

        bool DeleteLabel(int LabelId, int NoteId, int UserId);
    }
}