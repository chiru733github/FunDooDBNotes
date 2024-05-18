using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class LabelBusiness : ILabelBusiness
    {
        private readonly ILabelRepo labelRepo;
        public LabelBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }
        public LabelsEntity AddLabel(int UserId, int NotesId, string labelName)
        {
            return labelRepo.AddLabel(UserId, NotesId, labelName);
        }
        public List<LabelsEntity> GetByLabelname(int UserId, string labelname)
        {
            return labelRepo.GetByLabelname(UserId, labelname);
        }
        public bool RemoveLabelbyNotesID(int UserId, int NotesId, string labelname)
        {
            return labelRepo.RemoveLabelbyNotesID(UserId, NotesId, labelname);
        }

    }
}
