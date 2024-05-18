using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class LabelsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }
        public string LabelName { get; set; }

        [ForeignKey("LabelsUser")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserEntity LabelsUser { get; set; }

        [ForeignKey("LabelsNote")]
        public int NotesId { get; set; }
        
        [JsonIgnore]
        public virtual NotesEntity LabelsNote { get; set; }
    }
}
