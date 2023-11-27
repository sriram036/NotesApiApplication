using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepo
    {
        LabelEntity AddLabel(string LabelName, int NoteId, int UserId);
    }
}