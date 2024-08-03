using Refit;
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
        public int Max_Weight_Kg { get; set; }
        public int Max_Distance_Km { get; set; }

        public override string ToString()
        {
            return $"Типа транспорта {Id} - {Title} - {Slug}\n" +
                $"Максимальный вес: {Max_Weight_Kg} кг\n" +
                $"Максимальное расстояние: {Max_Distance_Km} км \n";

            
        }
    }

}
