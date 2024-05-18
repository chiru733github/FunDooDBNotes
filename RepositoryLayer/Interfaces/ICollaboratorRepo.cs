using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepo
    {
        CollaboratorEntity AddCollaborator(int UserId, int NotesId, string Email);
        bool RemoveCollaboratorByNotesId(int UserId, int NotesId, string Email);

    }
}
