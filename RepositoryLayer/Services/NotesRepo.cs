using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model_Layer.Models;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        public NotesRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }
        public NotesEntity CreateNote(int UserId, NotesModel notes)
        {
            NotesEntity notesEntity = new NotesEntity();
            notesEntity.Title = notes.Title;
            notesEntity.Description = notes.Description;
            notesEntity.Color = "Default color";
            notesEntity.Image = "URL";
            notesEntity.CreatedAt = DateTime.Now;
            notesEntity.UpdatedAt = DateTime.Now;
            notesEntity.UserId = UserId;
            funDooDBContext.Add(notesEntity);
            funDooDBContext.SaveChanges();
            return notesEntity;
        }
        public NotesEntity GetNoteById(int UserId, int NotesId)
        {
            var notesEntity = funDooDBContext.Notes.FirstOrDefault(Notes => Notes.UserId == UserId && Notes.NotesId == NotesId);
            if (notesEntity != null) return notesEntity;
            return null;
        }
        public bool IsPin(int UserId, int NotesId)
        {
            var notesEntity = GetNoteById(UserId, NotesId);
            if (notesEntity.IsArchive == false && notesEntity.IsTrash == false)
            {
                notesEntity.IsPin = !notesEntity.IsPin;
            }
            if (notesEntity.IsArchive == true)
            {
                notesEntity.IsPin = true;
                notesEntity.IsArchive = false;
            }
            funDooDBContext.SaveChanges();
            return notesEntity.IsPin;
        }
        public bool IsTrash(int UserId, int NotesId)
        {
            NotesEntity notesEntity = GetNoteById(UserId, NotesId);
            if (notesEntity.IsPin == true || notesEntity.IsArchive == true)
            {
                notesEntity.IsArchive = false;
                notesEntity.IsPin = false;
                notesEntity.IsTrash = !notesEntity.IsTrash;
            }
            else
            {
                notesEntity.IsTrash = !notesEntity.IsTrash;
            }
            funDooDBContext.SaveChanges();
            return notesEntity.IsTrash;
        }
        public bool IsArchive(int UserId, int NotesId)
        {
            NotesEntity notesEntity = GetNoteById(UserId, NotesId);
            if (notesEntity.IsTrash == false)
            {
                notesEntity.IsArchive = !notesEntity.IsArchive;
                notesEntity.IsPin = false;
            }
            funDooDBContext.SaveChanges();
            return notesEntity.IsArchive;
        }
        public string AddColor(int UserId, int NotesId, string color)
        {
            NotesEntity notesEntity = GetNoteById(UserId, NotesId);
            if (notesEntity != null)
            {
                List<string> colors = new List<string>() { "red", "blue", "green", "black", "brown", "yellow", "violet", "white", "orange", "grey" };
                if (colors.IndexOf(color.ToLower()) != -1)
                {
                    notesEntity.Color = color;
                    notesEntity.UpdatedAt = DateTime.Now;
                    funDooDBContext.SaveChanges();
                    return "Success";
                }
                return "Invalid color you have color like " + string.Join(", ", colors);
            }
            return "Notes doesn't Exists.";
        }
        public bool AddImage(int UserId,int NoteId,IFormFile imagePath)
        {
            try
            {
                NotesEntity findNote = GetNoteById(UserId,NoteId);
                if (findNote != null) {
                    Account account = new Account("dqmbhupyb", "259317145591123", "fnKuqvzDYkiRkvLbvlQZHh249ss");
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imagePath.FileName, imagePath.OpenReadStream())
                    };
                    var uploadImageRes = cloudinary.Upload(uploadParams);
                    findNote.Image = uploadImageRes.Url.ToString();
                    findNote.UpdatedAt = DateTime.Now;
                    funDooDBContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string AddRemainder(int UserId, int NotesId, DateTime dateTime)
        {
            NotesEntity notesEntity = GetNoteById(UserId, NotesId);
            if (notesEntity != null)
            {
                if (dateTime > DateTime.Now)
                {
                    notesEntity.Reminder = dateTime;
                    notesEntity.UpdatedAt = DateTime.Now;
                    funDooDBContext.SaveChanges();
                    return "Successfull added";
                }
                return "Failed by You entered Outdated time.";
            }
            return "Failed by Notes doesn't exists.";
        }
        public string GetByUser(int UserId)
        {
            var User = funDooDBContext.Users.FirstOrDefault(user => user.UserId == UserId);
            if (User!=null)
            {
                var getbyuser = from i in funDooDBContext.Notes
                                join j in funDooDBContext.Users on i.UserId equals j.UserId
                                where j.UserId == UserId
                                select new
                                {
                                    NoteId=i.NotesId,
                                    Titlename=i.Title,
                                };
                string join1 =User.FirstName+" "+User.LastName+" "+JsonConvert.SerializeObject(getbyuser);
                return join1;
            }
            return null;
        }

        public Username_count CountNote(int UserId)
        {
            var count = from i in funDooDBContext.Notes
                        group i by i.UserId into j
                        where j.Key==UserId
                        select j;
            var username = funDooDBContext.Users.FirstOrDefault(user => user.UserId == UserId);
            Username_count username_Count = new Username_count();
            username_Count.Username = username.FirstName + " " + username.LastName;
            username_Count.Count = count.Count();
            return username_Count;
        }
        //3) find the notes on the basis of title and description, if its a single note show single data
        //else if more than one note is found, show the list of notes
        public List<NotesEntity> CheckOneOrmoreNote(int UserId)
        {
            var notes = (List<NotesEntity>)(from i in funDooDBContext.Notes
                        where i.UserId == UserId select i);
            return notes;
        }
    }
}