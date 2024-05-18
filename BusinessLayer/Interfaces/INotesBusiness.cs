using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Model_Layer.Models;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface INotesBusiness
    {
        NotesEntity CreateNote(int UserId, NotesModel notes);
        bool IsPin(int UserId, int NotesId);
        bool IsTrash(int UserId, int NotesId);
        bool IsArchive(int UserId, int NotesId);
        string AddColor(int UserId, int NotesId, string Color);
        bool AddImage(int UserId, int NotesId, IFormFile Image);
        string AddRemainder(int UserId, int NotesId, DateTime dateTime);
        string GetByUser(int UserId);
        Username_count CountNote(int UserId);
        List<NotesEntity> CheckOneOrmoreNote(int UserId);
    }
}
