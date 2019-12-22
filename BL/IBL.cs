using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    interface IBL
    {
        bool DateLengthPermission(DateTime start, DateTime end);//checks if stay is at least one full day long

        bool PermissionToCharge(GuestRequest gr);//checks if client gave permission for payment

        bool AvailabilityCheck(HostingUnit hu, GuestRequest gr);//checks if requested dates are available

        void FinalStatusChange(Order o);//after order status changes to closed cannot make further changes to the status

        void UpdateDiary(HostingUnit hu, Order o);//after the status changes to closed, mark the days in the units diary

        void ChangeRequestStatus(Order o, GuestRequest gr);//after order status changes to closed, also close to request status



    }
}
