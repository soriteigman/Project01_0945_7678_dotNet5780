using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
        public enum VacationType { Hotel, Bungalow, Tent, Hut, LogCabin, BeachHouse };

        public enum VacationArea { North, South, East, West, Center };

        public enum Status { SentEmail, NoAnswer, NotAddressedYet, Closed, Active };

        public enum Choices { No, Yes , DontCare};

        public enum StarRating { two_star, three_star, four_star, five_star, unrated }

        public enum VacationSubArea { Afula, Akko, Arad, Ashdod, Ashqelon, BatYam, Beersheba, BetSheʾan, BetSheʿarim, BneiBrak, Caesarea, Dimona,
            Eilat, EnGedi, GivatHayim, H̱adera, Haifa, Herzliya, H̱olon, Jerusalem, Karmiʾel, KefarSava, Lod, Meron, Nahariya, Nazareth, Netanya,
            PetaẖTikva, QiryatShemona, RamatGan, Ramla, Reẖovot, RishonLeẔiyon, TelAviv,Tiberias, Ẕefat };
   
}
