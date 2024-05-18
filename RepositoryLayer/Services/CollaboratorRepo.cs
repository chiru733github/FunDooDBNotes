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
    public class CollaboratorRepo : ICollaboratorRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        public CollaboratorRepo(FunDooDBContext DBContext)
        {
            this.funDooDBContext = DBContext;
        }
        public CollaboratorEntity AddCollaborator(int UserId, int NotesId, string Email)
        {
            if (GetNotesById(UserId, NotesId))
            {
                var CollaboratorPresent = funDooDBContext.Collaborators.Any(c => c.UserId == UserId && c.Email == Email && c.NotesId == NotesId);
                if (!CollaboratorPresent)
                {
                    CollaboratorEntity collaboratorData = new CollaboratorEntity();
                    collaboratorData.Email = Email;
                    collaboratorData.UserId = UserId;
                    collaboratorData.NotesId = NotesId;
                    funDooDBContext.Collaborators.Add(collaboratorData);
                    funDooDBContext.SaveChanges();
                    return collaboratorData;
                }
                else
                {
                    throw new Exception("Already Collaborator is existed.");
                }
            }
            else
            {
                throw new Exception("Notes doesn't exist in given Userid.");
            }
        }
        public bool RemoveCollaboratorByNotesId(int UserId, int NotesId,string Email)
        {
            CollaboratorEntity? CollaboratorData = (CollaboratorEntity?)(from i in funDooDBContext.Collaborators where i.Email == Email &&
                                                    i.UserId == UserId && i.NotesId == NotesId select i);
            if (CollaboratorData != null)
            {
                funDooDBContext.Collaborators.Remove(CollaboratorData);
                funDooDBContext.SaveChanges();
                return true;
            }
            return false;
        }
        public bool GetNotesById(int UserId, int NotesId)
        {
            var notesEntity = funDooDBContext.Notes.Any(Notes => Notes.UserId == UserId && Notes.NotesId == NotesId);
            return notesEntity;
        }
    }
}
