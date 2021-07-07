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
    public class Lost
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public string Opis { get; set; }

        public string OstatniaLokalizacja { get; set; }

        public string DodanaPrzez { get; set; }

        public string Photo { get; set; }

        public string Plec { get; set; }
    }
}