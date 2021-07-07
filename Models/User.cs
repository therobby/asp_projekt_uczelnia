using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APS.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // [BsonElement("Name")]
        [Required(ErrorMessage = "Wymagane")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public string Email { get; set; }

        public string Rola { get; set; }
        [StringLength(1000)]
        public string NumerTelefonu { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Płeć")]
        public string Plec { get; set; }

    }
}
