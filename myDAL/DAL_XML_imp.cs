using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;

namespace DAL
{
    class DAL_XML_imp : IDal
    {

        //Singelton
        static DAL_XML_imp instance = new DAL_XML_imp();
        public static DAL_XML_imp Instance { get { return instance; } }

        //Flag for config update
        public volatile bool updated = false;

        //Roots and paths of the files
        XElement GuestRequestRoot;
        string GuestRequestPath;

        XElement HostingUnitRoot;
        string HostingUnitPath;

        XElement OrderRoot;
        string OrderPath;

        XElement ConfigRoot;
        string ConfigPath;

        //-----------------
        /// <summary>
        /// Ctor 
        /// activates the config change tread and creates files if they dont exist
        /// </summary>
        private DAL_XML_imp()
        {
            Thread configUpdateThread = new Thread(CheckIfUpdated);
            configUpdateThread.Start();
            try
            {
                //giving the paths to the strings (GuestRequestPath etc.)
                string str = Assembly.GetExecutingAssembly().Location;
                string localPath = Path.GetDirectoryName(str);
                for (int i = 0; i < 3; i++)
                    localPath = Path.GetDirectoryName(localPath);

                GuestRequestPath = localPath + @"\GuestRequestXml.xml";
                HostingUnitPath = localPath + @"\HostingUnitXml.xml";
                OrderPath = localPath + @"\OrderXml.xml";
                ConfigPath = localPath + @"\ConfigXml.xml";
                //creation of the files
                if (!File.Exists(GuestRequestPath))
                    CreateGuestRequestFile();

                if (!File.Exists(HostingUnitPath))
                    CreateHostingUnitFile();

                if (!File.Exists(OrderPath))
                    CreateOrderFile();

                if (!File.Exists(ConfigPath))
                    CreateConfigFile();

            }
            catch (FileLoadException a)//if couldn't create
            {
                throw a;
            }
        }

        static DAL_XML_imp() { }

        //GuestRequest-------------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thats given 
        /// if an exception has been thrown throw FileLoadException
        /// </summary>
        void CreateGuestRequestFile()
        {
            try
            {
                GuestRequestRoot = new XElement("GuestRequests");
                SaveGuestRequests();
            }
            catch
            {
                throw new FileLoadException("Cannot start the project, check your GuestRequest Xml files");
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// Load the file and throw FileLoadException if an exception has been thrown 
        /// </summary>
        void LoadGuestRequests()
        {
            try
            {
                GuestRequestRoot = XElement.Load(GuestRequestPath);
            }
            catch
            {
                throw new FileLoadException("GuestRequest file load error");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveGuestRequests()
        {
            try
            {
                GuestRequestRoot.Save(GuestRequestPath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save GuestRequests in XML file");
            }

        }

        //-----------------------------------------------------------------------------

        public bool GRexist(int key)
        {
            try
            {
                LoadGuestRequests();
                GuestRequest tmp = (from gr in GuestRequestRoot.Elements()
                                    where gr.Element("GuestRequestKey").Value == key.ToString()
                                    select (XElementGRToGR(gr))).FirstOrDefault();
                if (tmp == null)
                    return false;
                else
                    return true;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a GuestRequest from XML to BE.GuestRequest
        /// </summary>
        /// <param name="toConvert"> the XElement GuestRequest to convert</param>
        /// <returns> the converted BE.GuestRequest </returns>
        GuestRequest XElementGRToGR(XElement toConvert)
        {
            return new GuestRequest()
            {
                GuestRequestKey = Convert.ToInt32(toConvert.Element("GuestRequestKey").Value),
                PrivateName = toConvert.Element("Name").Element("FirstName").Value,
                FamilyName=toConvert.Element("Name").Element("FamilyName").Value,
                MailAddress = toConvert.Element("MailAddress").Value,
                Status = (Status)Enum.Parse(typeof(Status), toConvert.Element("Status").Value),
                RegistrationDate = new DateTime(int.Parse(toConvert.Element("RegistrationDate").Element("Year").Value),
                                                       int.Parse(toConvert.Element("RegistrationDate").Element("Month").Value),
                                                       int.Parse(toConvert.Element("RegistrationDate").Element("Day").Value)),
                EntryDate = new DateTime(int.Parse(toConvert.Element("EntryDate").Element("Year").Value),
                                                       int.Parse(toConvert.Element("EntryDate").Element("Month").Value),
                                                       int.Parse(toConvert.Element("EntryDate").Element("Day").Value)),
                ReleaseDate = new DateTime(int.Parse(toConvert.Element("ReleaseDate").Element("Year").Value),
                                                       int.Parse(toConvert.Element("ReleaseDate").Element("Month").Value),
                                                       int.Parse(toConvert.Element("ReleaseDate").Element("Day").Value)),
                Area = (VacationArea)Enum.Parse(typeof(VacationArea), toConvert.Element("Area").Value),
                Type = (VacationType)Enum.Parse(typeof(VacationType), toConvert.Element("Type").Value),
                Adults = Convert.ToInt32(toConvert.Element("Adults").Value),
                Children = Convert.ToInt32(toConvert.Element("Children").Value),
                Pool = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("Pool").Value),
                Jacuzzi = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("Jacuzzi").Value),
                Garden = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("Garden").Value),
                ChildrensAttractions = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("ChildrensAttractions").Value),
                FitnessCenter = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("FitnessCenter").Value),
                WiFi = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("WiFi").Value),
                Parking = (Choices)Enum.Parse(typeof(Choices), toConvert.Element("Parking").Value),
                Pet = toConvert.Element("Pet").Value=="true",
                Stars= (StarRating)Enum.Parse(typeof(StarRating), toConvert.Element("Stars").Value),
                SubArea = toConvert.Element("SubArea").Value,
            };

        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a GuestRequest to XElement GR to save in file
        /// </summary>
        /// <param name="GuestRequestToConvert"> the GuestRequest to convert </param>
        /// <returns> the converted XElement GuestRequest </returns>
        XElement GRToXElementGR(GuestRequest GuestRequestToConvert)
        {
            XElement GuestRequestKey = new XElement("ID", GuestRequestToConvert.GuestRequestKey);
            XElement PrivateName = new XElement("PrivateName", GuestRequestToConvert.PrivateName);
            XElement FamilyName = new XElement("FamilyName", GuestRequestToConvert.FamilyName);
            XElement MailAddress = new XElement("MailAddress", GuestRequestToConvert.MailAddress);
            XElement Status = new XElement("Status", GuestRequestToConvert.Status);
            XElement yearReg = new XElement("Year", GuestRequestToConvert.RegistrationDate.Year);
            XElement monthReg = new XElement("Month", GuestRequestToConvert.RegistrationDate.Month);
            XElement dayReg = new XElement("Day", GuestRequestToConvert.RegistrationDate.Day);
            XElement yearEntry = new XElement("Year", GuestRequestToConvert.EntryDate.Year);
            XElement monthEntry = new XElement("Month", GuestRequestToConvert.EntryDate.Month);
            XElement dayEntry = new XElement("Day", GuestRequestToConvert.EntryDate.Day);
            XElement yearRelease = new XElement("Year", GuestRequestToConvert.ReleaseDate.Year);
            XElement monthRelease = new XElement("Month", GuestRequestToConvert.ReleaseDate.Month);
            XElement dayRelease = new XElement("Day", GuestRequestToConvert.ReleaseDate.Day);
            XElement RegistrationDate = new XElement("RegistrationDate", yearReg, monthReg, dayReg);
            XElement EntryDate = new XElement("EntryDate", yearEntry, monthEntry, dayEntry);
            XElement ReleaseDate = new XElement("ReleaseDate", yearRelease, monthRelease, dayRelease);
            XElement Area = new XElement("Area", GuestRequestToConvert.Area.ToString());
            XElement Type = new XElement("Type", GuestRequestToConvert.Type.ToString());
            XElement Adults = new XElement("Adults", GuestRequestToConvert.Adults);
            XElement Children = new XElement("Children", GuestRequestToConvert.Children);
            XElement Pool = new XElement("Pool", GuestRequestToConvert.Pool.ToString());
            XElement Jacuzzi = new XElement("Jacuzzi", GuestRequestToConvert.Jacuzzi.ToString());
            XElement Garden = new XElement("Garden", GuestRequestToConvert.Garden.ToString());
            XElement ChildrensAttractions = new XElement("ChildrensAttractions", GuestRequestToConvert.ChildrensAttractions.ToString());
            XElement FitnessCenter = new XElement("FitnessCenter", GuestRequestToConvert.FitnessCenter.ToString());
            XElement WiFi = new XElement("WiFi", GuestRequestToConvert.WiFi.ToString());
            XElement Parking = new XElement("Parking", GuestRequestToConvert.Parking.ToString());
            XElement Pet = new XElement("Pet", GuestRequestToConvert.Pet);
            XElement Stars = new XElement("Stars", GuestRequestToConvert.Stars.ToString());
            XElement SubArea = new XElement("SubArea", GuestRequestToConvert.SubArea);
          
            XElement Name = new XElement("Name", FamilyName, PrivateName);
            XElement guestRequest = new XElement("GuestRequest", GuestRequestKey, Name, MailAddress, Status, RegistrationDate, EntryDate, ReleaseDate, Area, Type, Adults,
                                            Children, Pool, Jacuzzi, Garden, ChildrensAttractions, FitnessCenter, Stars, WiFi, Parking, Pet, SubArea);
            return guestRequest;
        }

        //-----------------------------------------------------------------------------

        public void AddGuestRequest(GuestRequest newGR)
        {
            try
            {
                LoadGuestRequests();
                if (!GRexist(newGR.GuestRequestKey))
                {
                    GuestRequestRoot.Add(GRToXElementGR(newGR));
                    SaveGuestRequests();
                }
                else
                    throw new DuplicateWaitObjectException("GuestRequest already exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public void UpdateGuestRequest(GuestRequest updateGR)
        {
            try
            {
                LoadGuestRequests();
                if (GRexist(updateGR.GuestRequestKey))
                {
                    (from gr in GuestRequestRoot.Elements()
                     where gr.Element("ID").Value == updateGR.GuestRequestKey.ToString()
                     select gr
                     ).FirstOrDefault().Remove();
                    GuestRequestRoot.Add(GRToXElementGR(updateGR));
                    SaveGuestRequests();
                }
                else
                    throw new KeyNotFoundException("GuestRequest to update doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        //public override void DeleteTester(string testerID)
        //{
        //    try
        //    {
        //        LoadSchedules();
        //        LoadGuestRequests();
        //        if (GRexist(testerID))
        //        {
        //            (from tester in GuestRequestRoot.Elements()
        //             where tester.Element("ID").Value == testerID
        //             select tester
        //             ).FirstOrDefault().Remove();
        //            (from schd in scheduleRoot.Elements()
        //             where schd.Element("value").Value == testerID
        //             select schd).FirstOrDefault().Remove();
        //            SaveSchedules();
        //            SaveGuestRequests();
        //        }
        //        else
        //            throw new KeyNotFoundException("Tester to delete doesn't exist");
        //    }
        //    catch (FileLoadException a)
        //    {
        //        throw a;
        //    }
        //    catch (FileNotFoundException a)
        //    {
        //        throw a;
        //    }
        //}

        //-----------------------------------------------------------------------------

        public GuestRequest searchGRbyID(int key)
        {
            try
            {
                LoadGuestRequests();
                if (GRexist(key))
                {
                    return (from gr in GuestRequestRoot.Elements()
                            where gr.Element("GuestRequestKey").Value == key.ToString()
                            select (XElementGRToGR(gr))).FirstOrDefault();
                }
                else
                    throw new KeyNotFoundException("GuestRequest doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public List<GuestRequest> ListOfCustomers()
        {
            List<GuestRequest> guests;
            try
            {
                LoadGuestRequests();
                guests=(from gr in GuestRequestRoot.Elements()
                                        select new GuestRequest()
                                        {
                                            GuestRequestKey=Convert.ToInt32(gr.Element("GuestRequestKey").Value),
                                            FamilyName = gr.Element("Name").Element("FamilyName").Value,
                                            PrivateName = gr.Element("Name").Element("PrivateName").Value,
                                            MailAddress = gr.Element("MailAddress").Value,
                                            Status = (Status)Enum.Parse(typeof(Status), gr.Element("Status").Value),
                                            RegistrationDate = new DateTime(int.Parse(gr.Element("RegistrationDate").Element("YearReg").Value),
                                                                    int.Parse(gr.Element("RegistrationDate").Element("MonthReg").Value),
                                                                    int.Parse(gr.Element("RegistrationDate").Element("DayReg").Value)),
                                            EntryDate = new DateTime(int.Parse(gr.Element("EntryDate").Element("YearEntry").Value),
                                                                    int.Parse(gr.Element("EntryDate").Element("MonthEntry").Value),
                                                                    int.Parse(gr.Element("EntryDate").Element("DayEntry").Value)),
                                            ReleaseDate = new DateTime(int.Parse(gr.Element("ReleaseDate").Element("YearRelease").Value),
                                                                    int.Parse(gr.Element("ReleaseDate").Element("MonthRelease").Value),
                                                                    int.Parse(gr.Element("ReleaseDate").Element("DayRelease").Value)),
                                            Area = (VacationArea)Enum.Parse(typeof(VacationArea), gr.Element("Area").Value),
                                            Type = (VacationType)Enum.Parse(typeof(VacationType), gr.Element("Type").Value),
                                            Adults = Convert.ToInt32(gr.Element("Adults").Value),
                                            Children = Convert.ToInt32(gr.Element("Children").Value),
                                            Pool = (Choices)Enum.Parse(typeof(Choices), gr.Element("Pool").Value),
                                            Jacuzzi = (Choices)Enum.Parse(typeof(Choices), gr.Element("Jacuzzi").Value),
                                            Garden = (Choices)Enum.Parse(typeof(Choices), gr.Element("Garden").Value),
                                            ChildrensAttractions = (Choices)Enum.Parse(typeof(Choices), gr.Element("ChildrensAttractions").Value),
                                            FitnessCenter = (Choices)Enum.Parse(typeof(Choices), gr.Element("FitnessCenter").Value),
                                            WiFi = (Choices)Enum.Parse(typeof(Choices), gr.Element("WiFi").Value),
                                            Parking = (Choices)Enum.Parse(typeof(Choices), gr.Element("Parking").Value),
                                            SubArea = gr.Element("SubArea").Value,
                                        }).ToList();
                return guests;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        //public List<Tester> FilterTesters(Predicate<Tester> condition)
        //{
        //    try
        //    {
        //        LoadGuestRequests();
        //        List<Tester> testers = new List<Tester>(from tester in GuestRequestRoot.Elements()
        //                                                select new Tester(tester.Element("ID").Value)
        //                                                {
        //                                                    LastName = tester.Element("Name").Element("LastName").Value,
        //                                                    FirstName = tester.Element("Name").Element("FirstName").Value,
        //                                                    BirthDay = new DateTime(int.Parse(tester.Element("BirthDay").Element("Year").Value),
        //                                                                            int.Parse(tester.Element("BirthDay").Element("Month").Value),
        //                                                                            int.Parse(tester.Element("BirthDay").Element("Day").Value)),
        //                                                    Age = int.Parse(tester.Element("Age").Value),
        //                                                    Gender = (Gender)Enum.Parse(typeof(Gender), tester.Element("Gender").Value),
        //                                                    YearsOfExperience = int.Parse(tester.Element("YearsOfExperience").Value),
        //                                                    MaxDistanceForTest = int.Parse(tester.Element("MaxDistanceForTest").Value),
        //                                                    MaxTestInWeek = int.Parse(tester.Element("MaxTestInWeek").Value),
        //                                                    PhoneNumber = tester.Element("PhoneNumber").Value,
        //                                                    ProfilePicturePath = tester.Element("ProfilePicturePath").Value,
        //                                                    VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), tester.Element("VehicleType").Value),
        //                                                    Address = new Address(tester.Element("Address").Element("City").Value,
        //                                                                          tester.Element("Address").Element("Street").Value,
        //                                                                          int.Parse(tester.Element("Address").Element("NumOfBuilding").Value))
        //                                                }).ToList();
        //        List<Tester> tmp = (from tester in testers
        //                            where condition(tester)
        //                            select new Tester(tester)).ToList();
        //        return tmp;
        //    }
        //    catch (FileLoadException a)
        //    {
        //        throw a;
        //    }
        //}

        //HostingUnit------------------------------------------------------------------------

        /// <summary>
        /// Create the file of the path that is given 
        /// if an exception has been thrown throw FileLoadException
        /// </summary>
        private void CreateHostingUnitFile()
        {
            try
            {
                HostingUnitRoot = new XElement("HostingUnits");
                HostingUnitRoot.Save(HostingUnitPath);
            }
            catch
            {
                throw new FileLoadException("Cannot start the project check your HostingUnits Xml files");
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// Load the file and throw FileLoadException if an exception has been thrown 
        /// </summary>
        private void LoadHostingUnits()
        {
            try
            {
                HostingUnitRoot = XElement.Load(HostingUnitPath);
            }
            catch
            {
                throw new FileLoadException("HostingUnits file load problem");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveHostingUnits()
        {
            try
            {
                HostingUnitRoot.Save(HostingUnitPath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save HostingUnits in XML file");
            }
        }

        //-----------------------------------------------------------------------------

        public bool HUexist(int key)
        {
            try
            {
                LoadHostingUnits();
                var tmp = (from hu in HostingUnitRoot.Elements()
                               where hu.Element("HostingUnitKey").Value == key.ToString()
                               select(XElementHUToHU(hu))).FirstOrDefault();
                if (tmp == null)
                    return false;
                else
                    return true;
            }
            catch (FileLoadException a)
            {

                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a trainee from the file to _DO.Trainee
        /// </summary>
        /// <param name="toConvert"> the XElement trainee to convert</param>
        /// <returns> the converted _DO.Trainee </returns>
        HostingUnit XElementHUToHU(XElement toConvert)
        {
            return new HostingUnit()//serializer?
            {
                HostingUnitKey= Convert.ToInt32(toConvert.Element("HostingUnitKey").Value),
                HostingUnitName = toConvert.Element("HostingUnitName").Value,
                //Diary=
                Owner=new Host()
                {
                    HostKey= Convert.ToInt32(toConvert.Element("HostKey").Value),
                    PrivateName= toConvert.Element("Name").Element("PrivateName").Value,
                    FamilyName = toConvert.Element("Name").Element("FamilyName").Value,
                    PhoneNumber = Convert.ToInt32(toConvert.Element("PhoneNumber").Value),
                    MailAddress = toConvert.Element("MailAddress").Value,
                    CollectionClearance=toConvert.Element("CollectionClearance").Value=="true",
                    BankBranchDetails=new BankBranch()
                    {
                        BankNumber = Convert.ToInt32(toConvert.Element("BankNumber").Value),
                        BankName = toConvert.Element("BankName").Value,
                        BranchNumber = Convert.ToInt32(toConvert.Element("BranchNumber").Value),
                        BranchAddress = toConvert.Element("BranchAddress").Value,
                        BranchCity = toConvert.Element("BranchCity").Value,
                    },
                    BankAccountNumber=Convert.ToInt32(toConvert.Element("BankAccountNumber").Value),
                },
                Area = (VacationArea)Enum.Parse(typeof(VacationArea), toConvert.Element("Area").Value),
                Type = (VacationType)Enum.Parse(typeof(VacationType), toConvert.Element("Type").Value),
                SubArea = toConvert.Element("SubArea").Value,
                Pet = toConvert.Element("Pet").Value == "true",
                WiFi = toConvert.Element("WiFi").Value == "true",
                Parking = toConvert.Element("Parking").Value == "true",
                Pool = toConvert.Element("Pool").Value == "true",
                Jacuzzi = toConvert.Element("Jacuzzi").Value == "true",
                Garden = toConvert.Element("Garden").Value == "true",
                ChildrensAttractions = toConvert.Element("ChildrensAttractions").Value == "true",
                FitnessCenter = toConvert.Element("FitnessCenter").Value == "true",
                Stars = (StarRating)Enum.Parse(typeof(StarRating), toConvert.Element("Stars").Value),
                Beds = Convert.ToInt32(toConvert.Element("Beds").Value),

                //Address = new Address(toConvert.Element("Address").Element("City").Value,
                //                                      toConvert.Element("Address").Element("Street").Value,
                //                                      int.Parse(toConvert.Element("Address").Element("NumOfBuilding").Value)),
            };
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a trainee to XElement to save in file 
        /// </summary>
        /// <param name="traineeToConvert"> the trainee to convert </param>
        /// <returns> the converted XElement trainee </returns>
        XElement HUToXElementHU(HostingUnit huToConvert)
        {
            XElement HostingUnitKey = new XElement("HostingUnitKey", huToConvert.HostingUnitKey);
            XElement HostingUnitName = new XElement("HostingUnitName", huToConvert.HostingUnitName);
            //XElement Diary=

            XElement BankNumber = new XElement("BankNumber", huToConvert.Owner.BankBranchDetails.BankNumber);
            XElement BankName = new XElement("BankName", huToConvert.Owner.BankBranchDetails.BankName);
            XElement BranchNumber = new XElement("BranchNumber", huToConvert.Owner.BankBranchDetails.BranchNumber);
            XElement BranchAddress = new XElement("BranchAddress", huToConvert.Owner.BankBranchDetails.BranchAddress);
            XElement BranchCity = new XElement("BranchCity", huToConvert.Owner.BankBranchDetails.BranchCity);

            XElement HostKey = new XElement("HostKey", huToConvert.Owner.HostKey);
            XElement PrivateName = new XElement("LastName", huToConvert.Owner.PrivateName);
            XElement FamilyName = new XElement("LastName", huToConvert.Owner.FamilyName);
            XElement PhoneNumber = new XElement("LastName", huToConvert.Owner.PhoneNumber);
            XElement MailAddress = new XElement("LastName", huToConvert.Owner.MailAddress);
            XElement CollectionClearance = new XElement("LastName", huToConvert.Owner.CollectionClearance);
            XElement BankBranchDetails = new XElement("BankBranchDetails", BankNumber, BankName, BranchNumber,
                BranchAddress, BranchCity);
            XElement BankAccountNumber = new XElement("LastName", huToConvert.Owner.BankAccountNumber);
            XElement Name = new XElement("Name", PrivateName, FamilyName);

            XElement Owner = new XElement("Owner", HostKey, Name, PhoneNumber, MailAddress, CollectionClearance,
                BankBranchDetails, BankAccountNumber);
            XElement Area = new XElement("Area", huToConvert.Area.ToString());
            XElement Type = new XElement("Type", huToConvert.Type.ToString());
            XElement SubArea = new XElement("SubArea", huToConvert.SubArea);
            XElement Pet = new XElement("Pet", huToConvert.Pet);
            XElement WiFi = new XElement("WiFi", huToConvert.WiFi);
            XElement Parking = new XElement("Parking", huToConvert.Parking);
            XElement Pool = new XElement("Pool", huToConvert.Pool);
            XElement Jacuzzi = new XElement("Jacuzzi", huToConvert.Jacuzzi);
            XElement Garden = new XElement("Garden", huToConvert.Garden);
            XElement ChildrensAttractions = new XElement("ChildrensAttractions", huToConvert.ChildrensAttractions);
            XElement FitnessCenter = new XElement("FitnessCenter", huToConvert.FitnessCenter);
            XElement Stars = new XElement("Stars", huToConvert.Stars.ToString());
            XElement Beds = new XElement("Beds", huToConvert.Beds);
      
            XElement hostingUnit = new XElement("HostingUnit", HostingUnitKey, HostingUnitName/*, Diary*/, Owner, Area, Type, SubArea, Pet,
                                                       WiFi, Parking, Pool, Jacuzzi, Garden, ChildrensAttractions, FitnessCenter, Stars, Beds);
            return hostingUnit;
        }

        //-----------------------------------------------------------------------------

        public void AddHostingUnit(HostingUnit newHU)
        {
            try
            {
                LoadHostingUnits();
                if (!HUexist(newHU.HostingUnitKey))
                {
                    HostingUnitRoot.Add(HUToXElementHU(newHU));
                    SaveHostingUnits();
                }
                else
                    throw new DuplicateWaitObjectException("HostingUnit already exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public void UpdateHostingUnit(HostingUnit updateHU)
        {
            try
            {
                LoadHostingUnits();
                if (HUexist(updateHU.HostingUnitKey))
                {
                    (from hu in HostingUnitRoot.Elements()
                     where hu.Element("HostingUnitKey").Value == updateHU.HostingUnitKey.ToString()
                     select hu
                     ).FirstOrDefault().Remove();
                    HostingUnitRoot.Add(HUToXElementHU(updateHU));
                    SaveHostingUnits();
                }
                else
                    throw new KeyNotFoundException("HostingUnit to update doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public void RemoveHostingUnit(HostingUnit hu)
        {
            try
            {
                LoadHostingUnits();
                if (HUexist(hu.HostingUnitKey))
                {
                    (from hostingUnit in HostingUnitRoot.Elements()
                     where hostingUnit.Element("HostingUnitKey").Value == hu.HostingUnitKey.ToString()
                     select hostingUnit
                     ).FirstOrDefault().Remove();
                    SaveHostingUnits();
                }
                else
                    throw new KeyNotFoundException("HostingUnit to remove doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public HostingUnit SearchHUbyID(int key)
        {
            try
            {
                LoadHostingUnits();
                if (HUexist(key))
                {
                    return (from hu in HostingUnitRoot.Elements()
                            where hu.Element("HostingUnitKey").Value == key.ToString()
                            select (XElementHUToHU(hu))).FirstOrDefault();
                }
                else
                    throw new KeyNotFoundException("HostingUnit doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        //public string GetTraineeNameByID(string traineeID)
        //{
        //    try
        //    {
        //        LoadHostingUnits();
        //        string tmp = (from trainee in HostingUnitRoot.Elements()
        //                      where trainee.Element("ID").Value == traineeID
        //                      select trainee.Element("Name").Element("FirstName").Value
        //                             + " " + trainee.Element("Name").Element("LastName").Value).FirstOrDefault();
        //        if (tmp != null)
        //            return tmp;
        //        else
        //            throw new KeyNotFoundException("Trainee Doesn't exist");
        //    }
        //    catch (FileLoadException a)
        //    {
        //        throw a;
        //    }
        //}

        //-----------------------------------------------------------------------------

        public List<HostingUnit> GetHostingUnits()
        {
            try
            {
                LoadHostingUnits();
                return new List<HostingUnit>((from hu in HostingUnitRoot.Elements()
                                              select new HostingUnit()
                                              {
                                                  HostingUnitKey = int.Parse(hu.Element("HostingUnitKey").Value),
                                                  Owner = new Host()
                                                  {
                                                      HostKey = int.Parse(hu.Element("HostKey").Value),
                                                      PrivateName = hu.Element("Name").Element("PrivateName").Value,
                                                      FamilyName = hu.Element("Name").Element("FamilyName").Value,
                                                      PhoneNumber = int.Parse(hu.Element("PhoneNumber").Value),
                                                      MailAddress = hu.Element("MailAddress").Value,
                                                      BankBranchDetails = new BankBranch()
                                                      {
                                                          BankNumber = int.Parse(hu.Element("BankNumber").Value),
                                                          BankName = hu.Element("BankName").Value,
                                                          BranchNumber = int.Parse(hu.Element("BranchNumber").Value),
                                                          BranchAddress = hu.Element("BranchAddress").Value,
                                                          BranchCity = hu.Element("BranchCity").Value,
                                                      },
                                                      BankAccountNumber = int.Parse(hu.Element("BankAccountNumber").Value),
                                                      CollectionClearance = hu.Element("CollectionClearance").Value == "true",
                                                  },

                                              HostingUnitName = hu.Element("HostingUnitName").Value,
                                              //diary
                                              Area = (VacationArea)Enum.Parse(typeof(VacationArea), hu.Element("Area").Value),
                                              SubArea = hu.Element("SubArea").Value,
                                              Type = (VacationType)Enum.Parse(typeof(VacationType), hu.Element("Type").Value),
                                              Beds=int.Parse(hu.Element("Beds").Value),
                                              Pet = hu.Element("Pet").Value == "true",
                                              WiFi = hu.Element("Pet").Value == "true",
                                              Parking = hu.Element("Pet").Value == "true",
                                              Pool = hu.Element("Pet").Value == "true",
                                              Jacuzzi = hu.Element("Pet").Value == "true",
                                              Garden = hu.Element("Pet").Value == "true",
                                              ChildrensAttractions = hu.Element("Pet").Value == "true",
                                              FitnessCenter = hu.Element("Pet").Value == "true",
                                              Stars = (StarRating)Enum.Parse(typeof(StarRating), hu.Element("Stars").Value),
                                          }).ToList());
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        //public override List<Trainee> FilterTrainee(Predicate<Trainee> condition)
        //{
        //    try
        //    {
        //        LoadHostingUnits();
        //        List<Trainee> trainees = new List<Trainee>((from trainee in HostingUnitRoot.Elements()
        //                                                    select new Trainee(trainee.Element("ID").Value)
        //                                                    {
        //                                                        LastName = trainee.Element("Name").Element("LastName").Value,
        //                                                        FirstName = trainee.Element("Name").Element("FirstName").Value,
        //                                                        BirthDay = new DateTime(int.Parse(trainee.Element("BirthDay").Element("Year").Value),
        //                                                                                int.Parse(trainee.Element("BirthDay").Element("Month").Value),
        //                                                                                int.Parse(trainee.Element("BirthDay").Element("Day").Value)),
        //                                                        Age = int.Parse(trainee.Element("Age").Value),
        //                                                        Gender = (Gender)Enum.Parse(typeof(Gender), trainee.Element("Gender").Value),
        //                                                        PhoneNumber = trainee.Element("PhoneNumber").Value,
        //                                                        VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), trainee.Element("VehicleType").Value),
        //                                                        Address = new Address(trainee.Element("Address").Element("City").Value,
        //                                                                              trainee.Element("Address").Element("Street").Value,
        //                                                                              int.Parse(trainee.Element("Address").Element("NumOfBuilding").Value)),
        //                                                        GearOfTrainne = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), trainee.Element("GearOfTrainne").Value),
        //                                                        NameOfTeacher = trainee.Element("NameOfTeacher").Value,
        //                                                        SchoolName = trainee.Element("SchoolName").Value,
        //                                                        ProfilePicturePath = trainee.Element("ProfilePicturePath").Value,
        //                                                        NumOfLessons = int.Parse(trainee.Element("NumOfLessons").Value)
        //                                                    }).ToList());

        //        List<Trainee> tmp = (from trainee in trainees
        //                             where condition(trainee)
        //                             select new Trainee(trainee)).ToList();
        //        return tmp;
        //    }
        //    catch (FileLoadException a)
        //    {
        //        throw a;
        //    }
        //}

        //test---------------------------------------------------------------------------
        //-------------------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thet is given 
        /// if an exception has been thrown trrow FileLoadException
        /// </summary>
        void CreateOrderFile()
        {
            try
            {
                OrderRoot = new XElement("Orders");
                OrderRoot.Save(OrderPath);
            }
            catch
            {
                throw new FileLoadException("Can not start the project check your Order Xml files");
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// Load the file and throw FileLoadException if an exception has been thrown 
        /// </summary>
        void LoadOrder()
        {
            try
            {
                OrderRoot = XElement.Load(OrderPath);
            }
            catch
            {

                throw new FileLoadException("Order file load problem");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveOrders()
        {
            try
            {
                OrderRoot.Save(OrderPath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save Orders in XML file");
            }
        }

        //-----------------------------------------------------------------------------

        public bool ORexist(int key)
        {
            try
            {
                LoadOrder();
                var tmp = (from ord in OrderRoot.Elements()
                            where int.Parse(ord.Element("orderKey").Value) == key
                            select ord).FirstOrDefault();
 
                if (tmp == null)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a Order from the file to BL.Order
        /// </summary>
        /// <param name="toConvert"> the XElement Order to convert</param>
        /// <returns> the converted Order </returns>
        Order XElementOrderToOrder(XElement toConvert)
        {
            return new Order()
            {
                HostingUnitKey = int.Parse(toConvert.Element("HostingUnitKey").Value),
                GuestRequestKey = int.Parse(toConvert.Element("GuestRequestKey").Value),
                OrderKey = int.Parse(toConvert.Element("OrderKey").Value),
                Status = (Status)Enum.Parse(typeof(Status), toConvert.Element("Status").Value),
                CreateDate = new DateTime(int.Parse(toConvert.Element("CreateDate").Element("CreateDateYear").Value),
                                                        int.Parse(toConvert.Element("CreateDate").Element("CreateDateMonth").Value),
                                                        int.Parse(toConvert.Element("CreateDate").Element("CreateDateDay").Value)
                                                        ),
                SentEmail = new DateTime(int.Parse(toConvert.Element("SentEmail").Element("SentEmailYear").Value),
                                                        int.Parse(toConvert.Element("SentEmail").Element("SentEmailMonth").Value),
                                                        int.Parse(toConvert.Element("SentEmail").Element("SentEmailDay").Value)
                                                        ),
            };
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a Order to XElement Order to save in file 
        /// </summary>
        /// <param name="OrderToConvert"> the Order to convert </param>
        /// <returns> the converted XElement Order </returns>
        XElement OrderToXElementOrder(Order OrderToConvert)
        {
            XElement OrderKey = new XElement("OrderKey", OrderToConvert.OrderKey);
            XElement GuestRequestKey = new XElement("GuestRequestKey", OrderToConvert.GuestRequestKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", OrderToConvert.HostingUnitKey);
            XElement CreateDateyear = new XElement("CreateDateYear", OrderToConvert.CreateDate.Year);
            XElement CreateDatemonth = new XElement("CreateDateMonth", OrderToConvert.CreateDate.Month);
            XElement CreateDateday = new XElement("CreateDateDay", OrderToConvert.CreateDate.Day);
            XElement CreateDate = new XElement("CreateDate", CreateDateyear, CreateDatemonth, CreateDateday);
            XElement SentEmailyear = new XElement("SentEmailYear", OrderToConvert.SentEmail.Year);
            XElement SentEmailmonth = new XElement("SentEmailMonth", OrderToConvert.SentEmail.Month);
            XElement SentEmailday = new XElement("SentEmailDay", OrderToConvert.SentEmail.Day);
            XElement SentEmail = new XElement("SentEmail", CreateDateyear, CreateDatemonth, CreateDateday);

            XElement Order = new XElement("Order", OrderKey, GuestRequestKey, HostingUnitKey, CreateDate, SentEmail);
            return Order;
        }

        //-----------------------------------------------------------------------------

        public void AddOrder(Order newOrder)
        {
            try
            {
                LoadOrder();
                LoadConfigs();
                if (ORexist(newOrder.OrderKey))
                    throw new DuplicateWaitObjectException("Order already exists");
                OrderRoot.Add(OrderToXElementOrder(newOrder));
                SaveOrders();
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public void UpdateOrder(Order updateOrder)
        {
            try
            {
                LoadOrder();
                if (ORexist(updateOrder.OrderKey))
                {
                    (from or in OrderRoot.Elements()
                     where or.Element("OrderKey").Value == updateOrder.OrderKey.ToString()
                     select or).FirstOrDefault().Remove();
                    OrderRoot.Add(OrderToXElementOrder(updateOrder));
                    SaveOrders();
                }
                else
                    throw new KeyNotFoundException("Order to update doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }

        }

        //-----------------------------------------------------------------------------

        //public override void UpdateTestStatus(Test testToUpdate)
        //{
        //    try
        //    {
        //        LoadOrder();
        //        if (TestExist(testToUpdate.CurrentTestSerialNumber))
        //        {
        //            (from test in OrderRoot.Elements()
        //             where test.Element("TestSerialNumber").Element("value").Value == testToUpdate.CurrentTestSerialNumber.ToString()
        //             select test).FirstOrDefault().Remove();
        //            OrderRoot.Add(TestToXElementTest(testToUpdate));
        //            SaveOrders();
        //        }
        //        else
        //            throw new KeyNotFoundException("Test to update doesn't exist");
        //    }
        //    catch (FileLoadException a)
        //    {
        //        throw a;
        //    }
        //    catch (FileNotFoundException a)
        //    {
        //        throw a;
        //    }
        //}

        //-----------------------------------------------------------------------------

        public Order SearchOrbyID(int key)
        {
            try
            {
                LoadOrder();
                if (ORexist(key))
                {
                    return (from or in OrderRoot.Elements()
                            where or.Element("OrderKey").Value == key.ToString()
                            select XElementOrderToOrder(or)).FirstOrDefault();
                }
                else
                    throw new KeyNotFoundException("Order doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public List<Order> GetAllOrders()
        {
            try
            {
                LoadOrder();
                return new List<Order>((from or in OrderRoot.Elements()
                                       select new Order()
                                       {
                                           OrderKey = Convert.ToInt32(or.Element("OrderKey").Value),
                                           HostingUnitKey = Convert.ToInt32(or.Element("HostingUnitKey").Value),
                                           GuestRequestKey = Convert.ToInt32(or.Element("GuestRequestKey").Value),
                                           CreateDate = new DateTime(int.Parse(or.Element("CreateDate").Element("CreateDateYear").Value),
                                                                      int.Parse(or.Element("CreateDate").Element("CreateDateMonth").Value),
                                                                      int.Parse(or.Element("CreateDate").Element("CreateDateDay").Value)),
                                           SentEmail = new DateTime(int.Parse(or.Element("SentEmail").Element("SentEmailYear").Value),
                                                                      int.Parse(or.Element("SentEmail").Element("SentEmailMonth").Value),
                                                                      int.Parse(or.Element("SentEmail").Element("SentEmailDay").Value)),
                                           Status = (Status)Enum.Parse(typeof(Status), or.Element("Status").Value),
                                       }).ToList());
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        //public override List<Test> FilterTests(Predicate<Test> condition)
        //{
        //    try
        //    {
        //        LoadOrder();
        //        List<Test> tests = new List<Test>((from test in OrderRoot.Elements()
        //                                           select new Test()
        //                                           {
        //                                               TesterID = test.Element("TesterID").Value,
        //                                               TraineeID = test.Element("TraineeID").Value,
        //                                               DateAndHourOfTest = new DateTime(int.Parse(test.Element("DateAndHourOfTest").Element("Year").Value),
        //                                                                          int.Parse(test.Element("DateAndHourOfTest").Element("Month").Value),
        //                                                                          int.Parse(test.Element("DateAndHourOfTest").Element("Day").Value),
        //                                                                          int.Parse(test.Element("DateAndHourOfTest").Element("Hour").Value),
        //                                                                          0, 0),
        //                                               LocationOfTest = new Address(test.Element("LocationOfTest").Element("City").Value,
        //                                                                            test.Element("LocationOfTest").Element("Street").Value,
        //                                                                            int.Parse(test.Element("LocationOfTest").Element("NumOfBuilding").Value)),
        //                                               StatusOfTest = (TestStatus)Enum.Parse(typeof(TestStatus), test.Element("StatusOfTest").Value),
        //                                               GearType = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), test.Element("GearType").Value),
        //                                               TypeOfCar = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), test.Element("TypeOfCar").Value),
        //                                               GiveWay = bool.Parse(test.Element("GiveWay").Value),
        //                                               SaveDistance = bool.Parse(test.Element("SaveDistance").Value),
        //                                               MirrorUse = bool.Parse(test.Element("MirrorUse").Value),
        //                                               ReverseParking = bool.Parse(test.Element("ReverseParking").Value),
        //                                               Signals = bool.Parse(test.Element("Signals").Value),
        //                                               StopSign = bool.Parse(test.Element("StopSign").Value),
        //                                               RightOfWay = bool.Parse(test.Element("RightOfWay").Value),
        //                                               ControledTheVehicle = bool.Parse(test.Element("ControledTheVehicle").Value),
        //                                               Passed = bool.Parse(test.Element("Passed").Value),
        //                                               Faild = bool.Parse(test.Element("Faild").Value),
        //                                               NoteOfTester = test.Element("NoteOfTester").Value,
        //                                               ClipPath = test.Element("ClipPath").Value,
        //                                               IsClipUpload = bool.Parse(test.Element("IsClipUpload").Value),
        //                                               CurrentTestSerialNumber = int.Parse(test.Element("TestSerialNumber").Element("value").Value)
        //                                           }).ToList());
        //        List<Test> tmp = (from test in tests
        //                          where condition(test)
        //                          select new Test(test)).ToList();
        //        return tmp;
        //    }
        //    catch (FileLoadException a)
        //    {
        //        throw a;
        //    }
        // }

        //configs------------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thet is given 
        /// if an exception has been thrown trrow FileLoadException
        /// </summary>
        void CreateConfigFile()
        {
            try
            {
                ConfigRoot = new XElement("Configs");

                XElement todayYear = new XElement("todayYear", DateTime.Today.Year);
                XElement todayMonth = new XElement("todayMonth", DateTime.Today.Month);
                XElement todayDay = new XElement("todayDay", DateTime.Today.Day);
                XElement today = new XElement("today", todayYear, todayMonth, todayDay);

                XElement commission = new XElement("value", 10);
                XElement ExpDay = new XElement("value", 10);
                XElement HostingUnitKey_s = new XElement("value", 10000000);
                XElement GuestRequestKey_s = new XElement("value", 10000000);
                XElement OrderKey_s = new XElement("value", 10000000);
                
                ConfigRoot.Add(HostingUnitKey_s, GuestRequestKey_s, OrderKey_s, ExpDay, commission, today);
                ConfigRoot.Save(ConfigPath);
            }
            catch
            {
                throw new FileLoadException("Cannot start the project check your Config Xml files");
            }
    }

    //----------------------------------------------------------------------------

    /// <summary>
    /// Load the file and throw FileLoadException if an exception has been thrown 
    /// </summary>
    void LoadConfigs()
        {
            try
            {
                ConfigRoot = XElement.Load(ConfigPath);

            }
            catch
            {
                throw new FileLoadException("Unable to open config file");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveConfigs()
        {
            try
            {
                ConfigRoot.Save(ConfigPath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save configs in Xml file");
            }
        }

        //-----------------------------------------------------------------------------

        public override Dictionary<string, object> GetConfig()
        {
            try
            {
                LoadConfigs();
                Dictionary<string, object> configs = new Dictionary<string, object>();
                var objFoeDictionary = (from cnfg in ConfigRoot.Elements()
                                        where cnfg.Name.LocalName != "SerialNumber"
                                        where cnfg.Element("readAble").Value == "true"
                                        select new { Key = cnfg.Name.LocalName, Value = cnfg.Element("value").Value });
                foreach (var item in objFoeDictionary)
                {
                    configs.Add(item.Key, item.Value as object);
                }
                if (configs.Any())
                {
                    return new Dictionary<string, object>(configs);
                }
                else
                    throw new NullReferenceException("There is no configuration to return");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override object GetSpecificConfig(string key)
        {
            try
            {
                LoadConfigs();
                if (ConfigExists(key))
                {
                    return (from cnfg in ConfigRoot.Elements()
                            where cnfg.Name.LocalName == key
                            where bool.Parse(cnfg.Element("readAble").Value) == true
                            select cnfg.Value).First();
                }
                else
                    throw new KeyNotFoundException("This config doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Checking if a config exists
        /// </summary>
        /// <param name="parm"> the key of the config</param>
        /// <returns> true if exists false if not </returns>
        bool ConfigExists(string parm)
        {
            var result = (from cnfg in ConfigRoot.Elements()
                          where cnfg.Name.LocalName == parm
                          select new XElement(cnfg)).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        //-----------------------------------------------------------------------------

        public override void ChangeConfig(string parm, object value)
        {
            try
            {
                LoadConfigs();
                if (ConfigExists(parm))
                {
                    foreach (var item in ConfigRoot.Elements())
                    {
                        if (item.Name.LocalName == parm)
                        {
                            if (bool.Parse(item.Element("writeAble").Value) == true)
                            {
                                item.Element("value").Value = value.ToString();
                                SaveConfigs();
                                updated = true;
                                break;
                            }
                            else
                                throw new AccessViolationException("Not alwod to change this config");
                        }
                    }
                }
                else
                    throw new KeyNotFoundException("This config doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Checking if the flag of change config is true 
        /// if it is, it will operate the configUpdate event
        /// if it is not, it will make the thread sleep for 1 second
        /// </summary>
        void CheckIfUpdated()
        {
            while (true)
            {
                if (updated)
                {
                    updated = false;
                    instance?.ConfigUpdated();
                }
                Thread.Sleep(1000);
            }
        }

        //schedules----------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thet is given 
        /// if an exception has been thrown trrow FileLoadException
        /// </summary>
        void CreateScheduleFile()
        {
            try
            {
                scheduleRoot = new XElement("Schedules");
                scheduleRoot.Save(schedulePath);
            }
            catch
            {
                throw new FileLoadException("Can not start the project check your Schedules Xml files");
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// Load the file and throw FileLoadException if an exception has been thrown 
        /// </summary>
        void LoadSchedules()
        {
            try
            {
                scheduleRoot = XElement.Load(schedulePath);
            }
            catch
            {
                throw new FileLoadException("Unable to open schedules fiile");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveSchedules()
        {
            try
            {
                scheduleRoot.Save(schedulePath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save schedules Xml file");
            }
        }

        XElement ScheduleToXElement(bool[,] schdule)
        {
            XElement week = new XElement("week");
            for (int i = 0; i < 5; i++)
            {
                XElement day = new XElement("day" + (i + 1));
                for (int j = 0; j < 6; j++)
                {
                    XElement hour = new XElement("hour" + (j + 9), schdule[j, i]);
                    day.Add(hour);
                }
                week.Add(day);
            }
            return week;
        }

        //-----------------------------------------------------------------------------

        public override void SetSchedule(string testerID, bool[,] testerSchedule)
        {
            try
            {
                LoadSchedules();
                XElement IDValue = new XElement("value", testerID);
                XElement ID = new XElement("ID", IDValue);
                XElement schedule = new XElement(ScheduleToXElement(testerSchedule));
                ID.Add(schedule);
                scheduleRoot.Add(ID);
                SaveSchedules();
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override void UpdateSchedule(string testerId, bool[,] schedhule)
        {
            try
            {
                LoadGuestRequests();
                LoadSchedules();
                if (GRexist(testerId))
                {
                    (from schd in scheduleRoot.Elements()
                     where schd.Element("value").Value == testerId
                     select schd).FirstOrDefault().Remove();
                    SaveSchedules();
                    SetSchedule(testerId, schedhule);
                }
                else
                    throw new KeyNotFoundException("Tester with this ID doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override bool[,] GetTesterScheduleByID(string testerID)
        {
            bool[,] testerSchedule = new bool[6, 5];
            try
            {
                LoadGuestRequests();
                LoadSchedules();
                if (GRexist(testerID))
                {
                    XElement schedule = (from schd in scheduleRoot.Elements()
                                         where schd.Element("value").Value == testerID
                                         select new XElement(schd.Element("week"))).FirstOrDefault();

                    if (schedule != null)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                testerSchedule[j, i] = bool.Parse(schedule.Element("day" + (i + 1)).Element("hour" + (j + 9)).Value);
                            }
                        }
                        return testerSchedule;
                    }
                    else
                        throw new KeyNotFoundException("Didn't find this tester's schedule in Xml file");
                }
                else
                    throw new KeyNotFoundException("Tester with this ID doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }
    }

}
        
    

