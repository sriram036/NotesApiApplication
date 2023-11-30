using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepo
    {
        int AddLabel(string LabelName, int NoteId, int UserId);

        List<LabelEntity> GetLabels(int UserId);

        LabelEntity UpdateLabel(int LabelId, int NoteId, int UserId, string LabelName);

        bool DeleteLabel(int LabelId, int NoteId, int UserId);
    }
}