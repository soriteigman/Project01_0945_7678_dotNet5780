﻿using System;
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
            XElement GuestRequestKey = new XElement("GuestRequestKey", GuestRequestToConvert.GuestRequestKey);
            XElement PrivateName = new XElement("PrivateName", GuestRequestToConvert.PrivateName);
            XElement FamilyName = new XElement("FamilyName", GuestRequestToConvert.FamilyName);
            XElement MailAddress = new XElement("MailAddress", GuestRequestToConvert.MailAddress);
            XElement Status = new XElement("Status", GuestRequestToConvert.Status);
            XElement YearReg = new XElement("YearReg", GuestRequestToConvert.RegistrationDate.Year);
            XElement MonthReg = new XElement("MonthReg", GuestRequestToConvert.RegistrationDate.Month);
            XElement DayReg = new XElement("DayReg", GuestRequestToConvert.RegistrationDate.Day);
            XElement YearEntry = new XElement("YearEntry", GuestRequestToConvert.EntryDate.Year);
            XElement MonthEntry = new XElement("MonthEntry", GuestRequestToConvert.EntryDate.Month);
            XElement DayEntry = new XElement("DayEntry", GuestRequestToConvert.EntryDate.Day);
            XElement YearRelease = new XElement("YearRelease", GuestRequestToConvert.ReleaseDate.Year);
            XElement MonthRelease = new XElement("MonthRelease", GuestRequestToConvert.ReleaseDate.Month);
            XElement DayRelease = new XElement("DayRelease", GuestRequestToConvert.ReleaseDate.Day);
            XElement RegistrationDate = new XElement("RegistrationDate", YearReg, MonthReg, DayReg);
            XElement EntryDate = new XElement("EntryDate", YearEntry, MonthEntry, DayEntry);
            XElement ReleaseDate = new XElement("ReleaseDate", YearRelease, MonthRelease, DayRelease);
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
                LoadConfigs();
                if (!GRexist(newGR.GuestRequestKey))
                {
                    newGR.GuestRequestKey = Configuration.GuestRequest_s++;
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
                     where gr.Element("GuestRequestKey").Value == updateGR.GuestRequestKey.ToString()
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

        public IEnumerable<GuestRequest> ListOfCustomers()//gets all of the requests saved in the system
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

        public IEnumerable<GuestRequest> GetAllRequests(Func<GuestRequest, bool> condition=null)//gets all requests that fit the condition
        {
            try
            {
                LoadGuestRequests();
                List<GuestRequest> guestRequests = new List<GuestRequest>(from gr in GuestRequestRoot.Elements()
                                                        select new GuestRequest()
                                                        {
                                                            GuestRequestKey = Convert.ToInt32(gr.Element("GuestRequestKey").Value),
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
                List<GuestRequest> tmp = (from gr in guestRequests
                                          where condition(gr)
                                    select gr).ToList();
                return tmp;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }



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
        /// Converting a XElement HostingUnit from the file to BE.HostingUnit
        /// </summary>
        /// <param name="toConvert"> the XElement HostingUnit to convert</param>
        /// <returns> the converted BE.HostingUnit </returns>
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
            };
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a HostingUnit to XElement to save in file 
        /// </summary>
        /// <param name="HostingUnitToConvert"> the HostingUnit to convert </param>
        /// <returns> the converted XElement HostingUnit </returns>
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
                LoadConfigs();
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
                    throw new KeyNotFoundException("HostingUnit to find doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        
        public IEnumerable<HostingUnit> ListOfHostingUnits()//gets all hosting units
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

        public IEnumerable<HostingUnit> GetAllUnits(Func<HostingUnit, bool> condition)//gets all units that match the condition
        {
            try
            {
                LoadHostingUnits();
                List<HostingUnit> HUs = new List<HostingUnit>((from hu in HostingUnitRoot.Elements()
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
                                                                Beds = int.Parse(hu.Element("Beds").Value),
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

                List<HostingUnit> tmp = (from hu in HUs
                                     where condition(hu)
                                     select hu).ToList();
                return tmp;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //Orders---------------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thet is given 
        /// if an exception has been thrown throw FileLoadException
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
                throw new FileLoadException("Cannot start the project check your Order Xml files");
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
                            where int.Parse(ord.Element("OrderKey").Value) == key
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
        /// Converting a Order from the file to BE.Order
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
                SaveConfigs();
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
                    throw new KeyNotFoundException("Order to find doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public IEnumerable<Order> ListOfOrders()//gets all the orders saved in the system
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

        public IEnumerable<Order> GetAllOrders(Func<Order, bool> condition=null)//gets all orders that fit the condition
        {
            try
            {
                LoadOrder();
                List<Order> ORs = new List<Order>((from or in OrderRoot.Elements()
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
                List<Order> tmp = (from o in ORs
                                  where condition(o)
                                  select o).ToList();
                return tmp;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //configs------------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thet is given 
        /// if an exception has been thrown throw FileLoadException
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

        public object GetSpecificConfig(string key)
        {
            try
            {
                LoadConfigs();
                if (ConfigExists(key))
                {
                    return (from cnfg in ConfigRoot.Elements()
                            where cnfg.Name.LocalName == key
                            select cnfg.Value).FirstOrDefault();
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

        public IEnumerable<BankBranch> ListOfBanks()
        {
            throw new NotImplementedException();
        }

        //-----------------------------------------------------------------------------



        /// <summary>



    }

}
        
    
