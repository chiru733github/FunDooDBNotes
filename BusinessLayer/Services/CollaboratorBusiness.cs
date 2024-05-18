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
    public class CollaboratorBusiness : ICollaboratorBusiness
    {
        private readonly ICollaboratorRepo icollaboratorRepo;
        public CollaboratorBusiness(ICollaboratorRepo collaboratorRepo)
        {
            this.icollaboratorRepo= collaboratorRepo;
        }
        public CollaboratorEntity AddCollaborator(int UserId, int NotesId, string Email)
        {
            return icollaboratorRepo.AddCollaborator(UserId, NotesId, Email);
        }
        public bool RemoveCollaboratorByNotesId(int UserId, int NotesId, string Email)
        {
            return icollaboratorRepo.RemoveCollaboratorByNotesId(UserId, NotesId, Email);
        }
    }
}
