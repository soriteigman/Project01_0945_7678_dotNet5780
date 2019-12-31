using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
        public enum VacationType { Hotel, Bungalow, Tent, Hut, LogCabin, BeachHouse };

        public enum VacationArea { North, South, East, West, Center };

        public enum Status { Active ,SentEmail, NoAnswer, NotAddressedYet, Closed , Booked};
    /*
     * sentEmail=email was sent, no answer yet
     * noAnswer= email sent but too many days passed so order was cancelled
     * notAddressedYet=order was created but no email was sent
     * closed= closed because no answer from guest
     * active= still dealing with order
     * booked= closed and booked
     
         */

        public enum Choices { DontCare, No, Yes};

        public enum StarRating { unrated, two_star, three_star, four_star, five_star }

        public enum VacationSubArea { Afula, Akko, Arad, Ashdod, Ashqelon, BatYam, Beersheba, BetSheʾan, BetSheʿarim, BneiBrak, Caesarea, Dimona,
            Eilat, EnGedi, GivatHayim, H̱adera, Haifa, Herzliya, H̱olon, Jerusalem, Karmiʾel, KefarSava, Lod, Meron, Nahariya, Nazareth, Netanya,
            PetaẖTikva, QiryatShemona, RamatGan, Ramla, Reẖovot, RishonLeẔiyon, TelAviv,Tiberias, Ẕefat };
   
}
