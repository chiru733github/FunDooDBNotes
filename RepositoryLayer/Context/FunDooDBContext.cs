using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Context
{
    public class FunDooDBContext :DbContext
    {
        public FunDooDBContext(DbContextOptions dbContext) : base(dbContext) { }
        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<NotesEntity> Notes { get; set; }
        public DbSet<LabelsEntity> Label { get; set; }
        public DbSet<CollaboratorEntity> Collaborators { get; set; }
    }
}