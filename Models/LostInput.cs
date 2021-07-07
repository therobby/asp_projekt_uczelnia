using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APS.Models
{
    public class LostInput
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Ostatnia Lokalizacja")]
        public string OstatniaLokalizacja { get; set; }
        public string DodanaPrzez { get; set; }

        [Required, FileExtensions(Extensions = "jpg, png",
             ErrorMessage = "Specify a jpg or png file.")]
        [Display(Name = "Zdjęcie")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Płeć")]
        public string Plec { get; set; }
    }
}