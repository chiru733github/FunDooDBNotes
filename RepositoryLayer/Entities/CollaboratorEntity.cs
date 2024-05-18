using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entities
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId { get; set; }
        public string Email { get; set; }
        [ForeignKey("CollaboratorUser")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserEntity CollaboratorUser { get; set; }

        [ForeignKey("CollaboratorNote")]
        public int NotesId { get; set; }

        [JsonIgnore]
        public virtual NotesEntity CollaboratorNote { get; set; }
    }
}
