using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBusiness
    {
        LabelEntity AddLabel(string LabelName, int NoteId, int UserId);
    }
}