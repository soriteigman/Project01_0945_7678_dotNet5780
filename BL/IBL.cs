﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        IEnumerable<HostingUnit> GetAllUnits(string searchString, object key, StarRating?
                                         star, VacationArea? area, VacationType? type);
        IEnumerable<GuestRequest> GetAllReq(string searchString, StarRating?
                                         star, VacationArea? area, VacationType? type);
        IEnumerable<HostingUnit> listHU();
        IEnumerable<HostingUnit> searchHUbyOwner(int key);//filters from all orders based on parameters recieved
        IEnumerable<Order> GetAllOrders(BE.Status? status);
        IEnumerable<GuestRequest> listGR();
        void addreq(GuestRequest gr);
        void Updategr(GuestRequest gr);
        void AddHostingUnit(HostingUnit hu);

        void RemoveHostingUnit(HostingUnit hu);

        void UpdateHostingUnit(HostingUnit hu);
        void AddOrder(Order o);
        bool DateOK(DateTime start, DateTime end);
        void UpdateOrder(Order o); //status update

        Predicate<GuestRequest> BuildPredicate(HostingUnit hu);//based on a hosting unit builds a predicate to filter all guest requests

        bool DateLengthPermission(DateTime s, DateTime e);//checks if stay is at least one full day long

        void TotalCommissionCalculator(Creator c);//calculates total commission from all the bookings on the website

        Order CreateOrder(int HUkey, int GRkey);//create a order

        bool HExists(int id);//checks to see if any hosting units have a host with this ID

        HostingUnit SearchHUbyID_bl(int key);//finds hosting unit by its key

        bool IsValidEmail(string email);

        bool AvailabilityCheck(HostingUnit hu, GuestRequest gr);//checks if requested dates are available
        int CalculateDurationOfStay(GuestRequest gr);//returns duration of stay
        int CalculateComission(GuestRequest gr);//calculates comission
        void UpdateDiary(Order o);//after the status changes to closed, mark the days in the units diary

        void ChangeRequestStatus(Order o);//after order status changes to closed, also close to request status

        bool RemoveUnitCheck(HostingUnit hu);//checks to see if there are any active reservations for that unit before removing it

        //bool ChangeCollectionClearance(Host h);//
        HostingUnit SearchHUbyID(int key);

        void SendEmail(Order o);//sends email when order status changes to "sent mail"

        IEnumerable<HostingUnit> AvailableUnits(DateTime startDate, int numOfDays);//returns all available hosting units for the dates requested

        int NumOfDaysInBetweeen(DateTime startDate, DateTime endDate = default(DateTime));//remember if the end date is null change it to Configuration.today

        IEnumerable<Order> DaysPassedOnOrders(int numOfDays, Predicate<Order> conditions);//returns all orders that were sent a email/ created "numOfDays" ago

        IEnumerable<GuestRequest> AllRequestsThatMatch(Predicate<GuestRequest> conditions);//returns all requests that fullfill the conditions 

        int NumOfSent_GR_Orders(GuestRequest gr);//returns the num of orders that were sent for that guest request

        int NumOfSent_HU_Orders(HostingUnit hu, Predicate<Order> conditions);//returns the number of orders that were sent or booked for this hosting unit

        IEnumerable<IGrouping<VacationArea, GuestRequest>> Group_GR_ByArea();//groups the requests by area of choice
        IEnumerable<IGrouping<int, GuestRequest>> GroupByNumOfGuests();//groups by number of guests
        IEnumerable<IGrouping<int, HostingUnit>> GroupHUByHosts();//groups units by hosts
        IEnumerable<IGrouping<int, Host>> GroupByNumOfUnits();//groups by number of hosting units the hosts own
        IEnumerable<IGrouping<VacationArea, HostingUnit>> Group_HU_ByArea();//groups the units by area of choice


        IEnumerable<Order> GetsOpenOrders(); 
    }
}
