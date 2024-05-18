using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class LabelRepo : ILabelRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        public LabelRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }
        public LabelsEntity AddLabel(int UserId, int NotesId, string labelName)
        {
            if (GetNotesById(UserId, NotesId))
            {
                var labelPresent = funDooDBContext.Label.Any(label => label.UserId==UserId && label.LabelName==labelName && label.NotesId==NotesId);
                if (!labelPresent)
                {
                    LabelsEntity labelData = new LabelsEntity();
                    labelData.LabelName = labelName;
                    labelData.UserId = UserId;
                    labelData.NotesId = NotesId;
                    funDooDBContext.Label.Add(labelData);
                    funDooDBContext.SaveChanges();
                    return labelData;
                }
                else
                {
                    throw new Exception("Already labelname is existed.");
                }
            }
            else
            {
                throw new Exception("Notes doesn't exist in given Userid.");
            }
        }
        public List<LabelsEntity> GetByLabelname(int UserId, string labelname) 
        {
            var result = from i in funDooDBContext.Label where i.LabelName == labelname && i.UserId == UserId select i;
            return result.ToList();
        }
        public bool RemoveLabelbyNotesID(int UserId, int NotesId,string labelname)
        {
            LabelsEntity? labelsEntity = funDooDBContext.Label.FirstOrDefault(label => label.UserId == UserId && 
                                            label.NotesId == NotesId && label.LabelName == labelname);
            if(labelsEntity != null)
            {
                funDooDBContext.Label.Remove(labelsEntity);
                funDooDBContext.SaveChanges();
                return true;
            }
            return false;
        }
        public bool GetNotesById(int UserId,int NotesId)
        {
            var notesEntity = funDooDBContext.Notes.Any(Notes => Notes.UserId == UserId && Notes.NotesId == NotesId);
            return notesEntity;
        }
    }
}
