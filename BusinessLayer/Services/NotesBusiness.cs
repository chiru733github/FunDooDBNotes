using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Model_Layer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class NotesBusiness : INotesBusiness
    {
        private INotesRepo iNotesRepo;
        public NotesBusiness(INotesRepo notes) 
        {
            this.iNotesRepo = notes;
        }
        public NotesEntity CreateNote(int UserId, NotesModel notes)
        {
            return iNotesRepo.CreateNote(UserId, notes);
        }
        public bool IsPin(int UserId, int NotesId)
        {
            return iNotesRepo.IsPin(UserId, NotesId);
        }
        public bool IsTrash(int UserId, int NotesId)
        {
            return iNotesRepo.IsTrash(UserId, NotesId);
        }
        public bool IsArchive(int UserId, int NotesId)
        {
            return iNotesRepo.IsArchive(UserId, NotesId);
        }
        public string AddColor(int UserId, int NotesId, string Color)
        {
            return iNotesRepo.AddColor(UserId, NotesId, Color);
        }
        public bool AddImage(int UserId, int NotesId, IFormFile Image)
        {
            return iNotesRepo.AddImage(UserId, NotesId, Image);
        }
        public string AddRemainder(int UserId, int NotesId, DateTime dateTime)
        {
            return iNotesRepo.AddRemainder(UserId, NotesId, dateTime);
        }
        public string GetByUser(int UserId)
        {
            return iNotesRepo.GetByUser(UserId);
        }
        public Username_count CountNote(int UserId)
        {
            return iNotesRepo.CountNote(UserId);
        }
        public List<NotesEntity> CheckOneOrmoreNote(int UserId)
        {
            return iNotesRepo.CheckOneOrmoreNote(UserId);
        }

    }
}
