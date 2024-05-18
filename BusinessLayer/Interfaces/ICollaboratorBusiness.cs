using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorBusiness
    {
        CollaboratorEntity AddCollaborator(int UserId, int NotesId, string Email);
        bool RemoveCollaboratorByNotesId(int UserId, int NotesId, string Email);

    }
}
