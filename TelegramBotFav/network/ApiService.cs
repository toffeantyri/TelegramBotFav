﻿using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotFav.network
{
    internal interface ApiService
    {
        [Get("/transport-types/")]
        Task<TransportTypes[]> GetTransportTypes();
    }

    public class TransportTypes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int MaxWeightKg { get; set; }
        public int MaxDistanceKm { get; set; }
  
    }

}
