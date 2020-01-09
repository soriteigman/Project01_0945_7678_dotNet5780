using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
        public enum VacationType { Hotel, Bungalow, Tent, Hut, LogCabin, BeachHouse };
    //public enum VacationSubArea{};

        public enum num { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven, Twelve, Thirteen, Fourteen };

        public enum VacationArea {All, DeadSea, Eilat, Jerusalem, North, South, Center };
        public enum DeadSea { All, EnGedi, DeadSea }
        public enum Eilat { All, Eilat }
        public enum Jerusalem {All, Jerusalem , BeitShemesh, Beitar, }
        public enum North {All, Tiberias, Ẕefat, RishonLeẔiyon, Afula, Akko , BetSheʾan , Caesarea, Haifa, Karmiʾel , Nahariya, Nazareth, QiryatShemona, }
        public enum South {All, Ashdod, Ashqelon, Beersheba , Meron , Netanya, Dimona, Arad, H̱adera , KefarSava }
        public enum Center {All, BneiBrak, TelAviv, RamatGan , PetaẖTikva ,BatYam, GivatHayim, Herzliya, H̱olon, Lod, Ramla , Reẖovot }
        public enum All
        {
                Afula, Akko, Arad, Ashdod, Ashqelon, BatYam, Beersheba, BetSheʾan, BetSheʿarim, BneiBrak, Caesarea, Dimona,
            Eilat, EnGedi, GivatHayim, H̱adera, Haifa, Herzliya, H̱olon, Jerusalem, Karmiʾel, KefarSava, Lod, Meron, Nahariya, Nazareth, Netanya,
            PetaẖTikva, QiryatShemona, RamatGan, Ramla, Reẖovot, RishonLeẔiyon, TelAviv, Tiberias, Ẕefat
        };

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

        public enum StarRating { dontCare, one_star, two_star, three_star, four_star, five_star  }

       
}
