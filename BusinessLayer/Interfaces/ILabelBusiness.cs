using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBusiness
    {
        LabelsEntity AddLabel(int UserId, int NotesId, string labelName);
        List<LabelsEntity> GetByLabelname(int UserId, string labelname);
        bool RemoveLabelbyNotesID(int UserId, int NotesId, string labelname);

    }
}
