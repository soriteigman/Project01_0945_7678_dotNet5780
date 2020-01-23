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
            Thread configUpdateTread = new Thread(CheckIfUpdated);
            configUpdateTread.Start();
            try
            {
                //giving the pathes to the strings (testerPath etc.)
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

            XElement yearsOfExperience = new XElement("YearsOfExperience", GuestRequestToConvert.YearsOfExperience);
            XElement maxTestInWeek = new XElement("MaxTestInWeek", GuestRequestToConvert.MaxTestInWeek);
            XElement maxDistanceForTest = new XElement("MaxDistanceForTest", GuestRequestToConvert.MaxDistanceForTest);
            XElement phoneNumber = new XElement("PhoneNumber", GuestRequestToConvert.PhoneNumber);
            XElement gender = new XElement("Gender", GuestRequestToConvert.Gender.ToString());
            XElement vehicleType = new XElement("VehicleType", GuestRequestToConvert.VehicleType.ToString());
            XElement city = new XElement("City", GuestRequestToConvert.Address.City);
            XElement street = new XElement("Street", GuestRequestToConvert.Address.Street);
            XElement numOfBuilding = new XElement("NumOfBuilding", GuestRequestToConvert.Address.NumOfBuilding);
            
            XElement address = new XElement("Address", city, street, numOfBuilding);
            XElement ProfilePicturePath = new XElement("ProfilePicturePath", GuestRequestToConvert.ProfilePicturePath);
            XElement name = new XElement("Name", FamilyName, PrivateName);
            XElement tester = new XElement("Tester", GuestRequestKey, name, RegistrationDate, address, Status, yearsOfExperience,
                                            maxTestInWeek, maxDistanceForTest, ProfilePicturePath, phoneNumber, gender, vehicleType);
            return tester;
        }

        //-----------------------------------------------------------------------------

        public override void AddTester(Tester newTester)
        {
            try
            {
                LoadGuestRequests();
                if (!TesterExist(newTester.ID))
                {
                    GuestRequestRoot.Add(TesterToXElementTester(newTester));
                    SaveGuestRequests();
                }
                else
                    throw new DuplicateWaitObjectException("Tester already exist");
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

        public override void UpdateTester(Tester updatedTester)
        {
            try
            {
                LoadGuestRequests();
                if (TesterExist(updatedTester.ID))
                {
                    (from tester in GuestRequestRoot.Elements()
                     where tester.Element("ID").Value == updatedTester.ID
                     select tester
                     ).FirstOrDefault().Remove();
                    GuestRequestRoot.Add(TesterToXElementTester(updatedTester));
                    SaveGuestRequests();
                }
                else
                    throw new KeyNotFoundException("Tester to update doesn't exist");
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

        public override void DeleteTester(string testerID)
        {
            try
            {
                LoadSchedules();
                LoadGuestRequests();
                if (GRexist(testerID))
                {
                    (from tester in GuestRequestRoot.Elements()
                     where tester.Element("ID").Value == testerID
                     select tester
                     ).FirstOrDefault().Remove();
                    (from schd in scheduleRoot.Elements()
                     where schd.Element("value").Value == testerID
                     select schd).FirstOrDefault().Remove();
                    SaveSchedules();
                    SaveGuestRequests();
                }
                else
                    throw new KeyNotFoundException("Tester to delete doesn't exist");
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

        public override Tester GetTesterByID(string testerID)
        {
            try
            {
                LoadGuestRequests();
                if (GRexist(testerID))
                {
                    return (from tester in GuestRequestRoot.Elements()
                            where tester.Element("ID").Value == testerID
                            select new Tester(XElementGRToGR(tester))).FirstOrDefault();
                }
                else
                    throw new KeyNotFoundException("Tester doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override List<Tester> GetAllTesters()
        {
            try
            {
                LoadGuestRequests();
                return new List<Tester>(from tester in GuestRequestRoot.Elements()
                                        select new Tester(tester.Element("ID").Value)
                                        {
                                            LastName = tester.Element("Name").Element("LastName").Value,
                                            FirstName = tester.Element("Name").Element("FirstName").Value,
                                            BirthDay = new DateTime(int.Parse(tester.Element("BirthDay").Element("Year").Value),
                                                                    int.Parse(tester.Element("BirthDay").Element("Month").Value),
                                                                    int.Parse(tester.Element("BirthDay").Element("Day").Value)),
                                            Age = int.Parse(tester.Element("Age").Value),
                                            Gender = (Gender)Enum.Parse(typeof(Gender), tester.Element("Gender").Value),
                                            YearsOfExperience = int.Parse(tester.Element("YearsOfExperience").Value),
                                            MaxDistanceForTest = int.Parse(tester.Element("MaxDistanceForTest").Value),
                                            MaxTestInWeek = int.Parse(tester.Element("MaxTestInWeek").Value),
                                            PhoneNumber = tester.Element("PhoneNumber").Value,
                                            ProfilePicturePath = tester.Element("ProfilePicturePath").Value,
                                            VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), tester.Element("VehicleType").Value),
                                            Address = new Address(tester.Element("Address").Element("City").Value,
                                                                  tester.Element("Address").Element("Street").Value,
                                                                  int.Parse(tester.Element("Address").Element("NumOfBuilding").Value))
                                        }).ToList();
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override List<Tester> FilterTesters(Predicate<Tester> condition)
        {
            try
            {
                LoadGuestRequests();
                List<Tester> testers = new List<Tester>(from tester in GuestRequestRoot.Elements()
                                                        select new Tester(tester.Element("ID").Value)
                                                        {
                                                            LastName = tester.Element("Name").Element("LastName").Value,
                                                            FirstName = tester.Element("Name").Element("FirstName").Value,
                                                            BirthDay = new DateTime(int.Parse(tester.Element("BirthDay").Element("Year").Value),
                                                                                    int.Parse(tester.Element("BirthDay").Element("Month").Value),
                                                                                    int.Parse(tester.Element("BirthDay").Element("Day").Value)),
                                                            Age = int.Parse(tester.Element("Age").Value),
                                                            Gender = (Gender)Enum.Parse(typeof(Gender), tester.Element("Gender").Value),
                                                            YearsOfExperience = int.Parse(tester.Element("YearsOfExperience").Value),
                                                            MaxDistanceForTest = int.Parse(tester.Element("MaxDistanceForTest").Value),
                                                            MaxTestInWeek = int.Parse(tester.Element("MaxTestInWeek").Value),
                                                            PhoneNumber = tester.Element("PhoneNumber").Value,
                                                            ProfilePicturePath = tester.Element("ProfilePicturePath").Value,
                                                            VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), tester.Element("VehicleType").Value),
                                                            Address = new Address(tester.Element("Address").Element("City").Value,
                                                                                  tester.Element("Address").Element("Street").Value,
                                                                                  int.Parse(tester.Element("Address").Element("NumOfBuilding").Value))
                                                        }).ToList();
                List<Tester> tmp = (from tester in testers
                                    where condition(tester)
                                    select new Tester(tester)).ToList();
                return tmp;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //teainee------------------------------------------------------------------------

        /// <summary>
        /// Create the file in the path thet is given 
        /// if an exception has been thrown trrow FileLoadException
        /// </summary>
        private void CreateHostingUnitFile()
        {
            try
            {
                HostingUnitRoot = new XElement("Trainees");
                HostingUnitRoot.Save(HostingUnitPath);
            }
            catch
            {
                throw new FileLoadException("Can not start the project check your Trainees Xml files");
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// Load the file and throw FileLoadException if an exception has been thrown 
        /// </summary>
        private void LoadTrainees()
        {
            try
            {
                HostingUnitRoot = XElement.Load(HostingUnitPath);
            }
            catch
            {
                throw new FileLoadException("Trainees file uoload problem");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveTrainees()
        {
            try
            {
                HostingUnitRoot.Save(HostingUnitPath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save trainees in XML file");
            }
        }

        //-----------------------------------------------------------------------------

        public override bool TraineeExist(string TraineeID)
        {
            try
            {
                LoadTrainees();
                Trainee tmp = (from trainee in HostingUnitRoot.Elements()
                               where trainee.Element("ID").Value == TraineeID
                               select new Trainee(TraineeID)).FirstOrDefault();
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
        Trainee XElementTraineeToTrainee(XElement toConvert)
        {
            return new Trainee(toConvert.Element("ID").Value)
            {
                LastName = toConvert.Element("Name").Element("LastName").Value,
                FirstName = toConvert.Element("Name").Element("FirstName").Value,
                BirthDay = new DateTime(int.Parse(toConvert.Element("BirthDay").Element("Year").Value),
                                                        int.Parse(toConvert.Element("BirthDay").Element("Month").Value),
                                                        int.Parse(toConvert.Element("BirthDay").Element("Day").Value)),
                Age = int.Parse(toConvert.Element("Age").Value),
                Gender = (Gender)Enum.Parse(typeof(Gender), toConvert.Element("Gender").Value),
                PhoneNumber = toConvert.Element("PhoneNumber").Value,
                VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), toConvert.Element("VehicleType").Value),
                Address = new Address(toConvert.Element("Address").Element("City").Value,
                                                      toConvert.Element("Address").Element("Street").Value,
                                                      int.Parse(toConvert.Element("Address").Element("NumOfBuilding").Value)),
                GearOfTrainne = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), toConvert.Element("GearOfTrainne").Value),
                NameOfTeacher = toConvert.Element("NameOfTeacher").Value,
                ProfilePicturePath = toConvert.Element("ProfilePicturePath").Value,
                SchoolName = toConvert.Element("SchoolName").Value,
                NumOfLessons = int.Parse(toConvert.Element("NumOfLessons").Value)
            };
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a trainee to XElement to save in file 
        /// </summary>
        /// <param name="traineeToConvert"> the trainee to convert </param>
        /// <returns> the converted XElement trainee </returns>
        XElement TraineeToXElementTrainee(Trainee traineeToConvert)
        {
            XElement id = new XElement("ID", traineeToConvert.ID);
            XElement firstName = new XElement("FirstName", traineeToConvert.FirstName);
            XElement lastName = new XElement("LastName", traineeToConvert.LastName);
            XElement age = new XElement("Age", traineeToConvert.Age);
            XElement phoneNumber = new XElement("PhoneNumber", traineeToConvert.PhoneNumber);
            XElement nameOfTeacher = new XElement("NameOfTeacher", traineeToConvert.NameOfTeacher);
            XElement schoolName = new XElement("SchoolName", traineeToConvert.SchoolName);
            XElement numOfLessons = new XElement("NumOfLessons", traineeToConvert.NumOfLessons);
            XElement gender = new XElement("Gender", traineeToConvert.Gender.ToString());
            XElement vehicleType = new XElement("VehicleType", traineeToConvert.VehicleType.ToString());
            XElement gearOfTrainne = new XElement("GearOfTrainne", traineeToConvert.GearOfTrainne.ToString());
            XElement year = new XElement("Year", traineeToConvert.BirthDay.Year);
            XElement month = new XElement("Month", traineeToConvert.BirthDay.Month);
            XElement day = new XElement("Day", traineeToConvert.BirthDay.Day);
            XElement city = new XElement("City", traineeToConvert.Address.City);
            XElement street = new XElement("Street", traineeToConvert.Address.Street);
            XElement numOfBuilding = new XElement("NumOfBuilding", traineeToConvert.Address.NumOfBuilding);
            XElement address = new XElement("Address", city, street, numOfBuilding);
            XElement birthDay = new XElement("BirthDay", year, month, day);
            XElement ProfilePicturePath = new XElement("ProfilePicturePath", traineeToConvert.ProfilePicturePath);
            XElement name = new XElement("Name", firstName, lastName);
            XElement trainee = new XElement("Trainee", id, name, birthDay, address, age, phoneNumber, nameOfTeacher,
                                                       schoolName, numOfLessons, ProfilePicturePath, gender, vehicleType, gearOfTrainne);
            return trainee;
        }

        //-----------------------------------------------------------------------------

        public override void AddTrainee(Trainee newTrainee)
        {
            try
            {
                LoadTrainees();
                if (!TraineeExist(newTrainee.ID))
                {
                    HostingUnitRoot.Add(TraineeToXElementTrainee(newTrainee));
                    SaveTrainees();
                }
                else
                    throw new DuplicateWaitObjectException("Trainee already exist");
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

        public override void UpdateTrainee(Trainee updatedTrainee)
        {
            try
            {
                LoadTrainees();
                if (TraineeExist(updatedTrainee.ID))
                {
                    (from trainee in HostingUnitRoot.Elements()
                     where trainee.Element("ID").Value == updatedTrainee.ID
                     select trainee
                     ).FirstOrDefault().Remove();
                    HostingUnitRoot.Add(TraineeToXElementTrainee(updatedTrainee));
                    SaveTrainees();
                }
                else
                    throw new KeyNotFoundException("Trainee to update doesn't exist");
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

        public override void DeleteTrainee(string traineeID)
        {
            try
            {
                LoadTrainees();
                if (TraineeExist(traineeID))
                {
                    (from trainee in HostingUnitRoot.Elements()
                     where trainee.Element("ID").Value == traineeID
                     select trainee
                     ).FirstOrDefault().Remove();
                    SaveTrainees();
                }
                else
                    throw new KeyNotFoundException("Trainee to delete doesn't exist");
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

        public override Trainee GetTraineeByID(string traineeID)
        {
            try
            {
                LoadTrainees();
                if (TraineeExist(traineeID))
                {
                    return (from trainee in HostingUnitRoot.Elements()
                            where trainee.Element("ID").Value == traineeID
                            select new _DO.Trainee(XElementTraineeToTrainee(trainee))).FirstOrDefault();
                }
                else
                    throw new KeyNotFoundException("Trainee doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override string GetTraineeNameByID(string traineeID)
        {
            try
            {
                LoadTrainees();
                string tmp = (from trainee in HostingUnitRoot.Elements()
                              where trainee.Element("ID").Value == traineeID
                              select trainee.Element("Name").Element("FirstName").Value
                                     + " " + trainee.Element("Name").Element("LastName").Value).FirstOrDefault();
                if (tmp != null)
                    return tmp;
                else
                    throw new KeyNotFoundException("Trainee Doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override List<Trainee> GetAllTrainee()
        {
            try
            {
                LoadTrainees();
                return new List<Trainee>((from trainee in HostingUnitRoot.Elements()
                                          select new Trainee(trainee.Element("ID").Value)
                                          {
                                              LastName = trainee.Element("Name").Element("LastName").Value,
                                              FirstName = trainee.Element("Name").Element("FirstName").Value,
                                              BirthDay = new DateTime(int.Parse(trainee.Element("BirthDay").Element("Year").Value),
                                                                      int.Parse(trainee.Element("BirthDay").Element("Month").Value),
                                                                      int.Parse(trainee.Element("BirthDay").Element("Day").Value)),
                                              Age = int.Parse(trainee.Element("Age").Value),
                                              Gender = (Gender)Enum.Parse(typeof(Gender), trainee.Element("Gender").Value),
                                              PhoneNumber = trainee.Element("PhoneNumber").Value,
                                              VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), trainee.Element("VehicleType").Value),
                                              Address = new Address(trainee.Element("Address").Element("City").Value,
                                                                    trainee.Element("Address").Element("Street").Value,
                                                                    int.Parse(trainee.Element("Address").Element("NumOfBuilding").Value)),
                                              GearOfTrainne = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), trainee.Element("GearOfTrainne").Value),
                                              NameOfTeacher = trainee.Element("NameOfTeacher").Value,
                                              ProfilePicturePath = trainee.Element("ProfilePicturePath").Value,
                                              SchoolName = trainee.Element("SchoolName").Value,
                                              NumOfLessons = int.Parse(trainee.Element("NumOfLessons").Value)
                                          }).ToList());
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override List<Trainee> FilterTrainee(Predicate<Trainee> condition)
        {
            try
            {
                LoadTrainees();
                List<Trainee> trainees = new List<Trainee>((from trainee in HostingUnitRoot.Elements()
                                                            select new Trainee(trainee.Element("ID").Value)
                                                            {
                                                                LastName = trainee.Element("Name").Element("LastName").Value,
                                                                FirstName = trainee.Element("Name").Element("FirstName").Value,
                                                                BirthDay = new DateTime(int.Parse(trainee.Element("BirthDay").Element("Year").Value),
                                                                                        int.Parse(trainee.Element("BirthDay").Element("Month").Value),
                                                                                        int.Parse(trainee.Element("BirthDay").Element("Day").Value)),
                                                                Age = int.Parse(trainee.Element("Age").Value),
                                                                Gender = (Gender)Enum.Parse(typeof(Gender), trainee.Element("Gender").Value),
                                                                PhoneNumber = trainee.Element("PhoneNumber").Value,
                                                                VehicleType = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), trainee.Element("VehicleType").Value),
                                                                Address = new Address(trainee.Element("Address").Element("City").Value,
                                                                                      trainee.Element("Address").Element("Street").Value,
                                                                                      int.Parse(trainee.Element("Address").Element("NumOfBuilding").Value)),
                                                                GearOfTrainne = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), trainee.Element("GearOfTrainne").Value),
                                                                NameOfTeacher = trainee.Element("NameOfTeacher").Value,
                                                                SchoolName = trainee.Element("SchoolName").Value,
                                                                ProfilePicturePath = trainee.Element("ProfilePicturePath").Value,
                                                                NumOfLessons = int.Parse(trainee.Element("NumOfLessons").Value)
                                                            }).ToList());

                List<Trainee> tmp = (from trainee in trainees
                                     where condition(trainee)
                                     select new Trainee(trainee)).ToList();
                return tmp;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

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
                OrderRoot = new XElement("Tests");
                OrderRoot.Save(OrderPath);
            }
            catch
            {
                throw new FileLoadException("Can not start the project check your Test Xml files");
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// Load the file and throw FileLoadException if an exception has been thrown 
        /// </summary>
        void LoadTests()
        {
            try
            {
                OrderRoot = XElement.Load(OrderPath);
            }
            catch
            {

                throw new FileLoadException("Tests file load problem");
            }
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Save the changes in the file
        /// </summary>
        void SaveTests()
        {
            try
            {
                OrderRoot.Save(OrderPath);
            }
            catch
            {
                throw new FileNotFoundException("Unable to save tests in XML file");
            }
        }

        //-----------------------------------------------------------------------------

        public override bool TestExist(int testSerialNumber)
        {
            try
            {
                LoadTests();
                Test tmp = (from test in OrderRoot.Elements()
                            where int.Parse(test.Element("TestSerialNumber").Element("value").Value) == testSerialNumber
                            select new Test(XElementTestToTest(test))).FirstOrDefault();
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
        /// Converting a test from the file to _DO.Test
        /// </summary>
        /// <param name="toConvert"> the XElement test to convert</param>
        /// <returns> the converted _DO.Test </returns>
        Test XElementTestToTest(XElement toConvert)
        {
            return new Test()
            {
                TesterID = toConvert.Element("TesterID").Value,
                TraineeID = toConvert.Element("TraineeID").Value,
                DateAndHourOfTest = new DateTime(int.Parse(toConvert.Element("DateAndHourOfTest").Element("Year").Value),
                                                        int.Parse(toConvert.Element("DateAndHourOfTest").Element("Month").Value),
                                                        int.Parse(toConvert.Element("DateAndHourOfTest").Element("Day").Value),
                                                        int.Parse(toConvert.Element("DateAndHourOfTest").Element("Hour").Value),
                                                        0, 0),
                LocationOfTest = new Address(toConvert.Element("LocationOfTest").Element("City").Value,
                                                          toConvert.Element("LocationOfTest").Element("Street").Value,
                                                          int.Parse(toConvert.Element("LocationOfTest").Element("NumOfBuilding").Value)),
                StatusOfTest = (TestStatus)Enum.Parse(typeof(TestStatus), toConvert.Element("StatusOfTest").Value),
                GearType = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), toConvert.Element("GearType").Value),
                TypeOfCar = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), toConvert.Element("TypeOfCar").Value),
                GiveWay = bool.Parse(toConvert.Element("GiveWay").Value),
                SaveDistance = bool.Parse(toConvert.Element("SaveDistance").Value),
                MirrorUse = bool.Parse(toConvert.Element("MirrorUse").Value),
                ReverseParking = bool.Parse(toConvert.Element("ReverseParking").Value),
                Signals = bool.Parse(toConvert.Element("Signals").Value),
                StopSign = bool.Parse(toConvert.Element("StopSign").Value),
                RightOfWay = bool.Parse(toConvert.Element("RightOfWay").Value),
                ControledTheVehicle = bool.Parse(toConvert.Element("ControledTheVehicle").Value),
                Passed = bool.Parse(toConvert.Element("Passed").Value),
                Faild = bool.Parse(toConvert.Element("Faild").Value),
                ClipPath = toConvert.Element("ClipPath").Value,
                IsClipUpload = bool.Parse(toConvert.Element("IsClipUpload").Value),
                NoteOfTester = toConvert.Element("NoteOfTester").Value,
                CurrentTestSerialNumber = int.Parse(toConvert.Element("TestSerialNumber").Element("value").Value)
            };
        }

        //-----------------------------------------------------------------------------

        /// <summary>
        /// Converting a test to XElement test to save in file 
        /// </summary>
        /// <param name="testToConvert"> the test to convert </param>
        /// <returns> the converted XElement test </returns>
        XElement TestToXElementTest(Test testToConvert)
        {
            XElement serialValue = new XElement("value", testToConvert.CurrentTestSerialNumber);
            XElement serialNumber = new XElement("TestSerialNumber", serialValue);
            XElement testerID = new XElement("TesterID", testToConvert.TesterID);
            XElement traineeID = new XElement("TraineeID", testToConvert.TraineeID);
            XElement saveDistance = new XElement("SaveDistance", testToConvert.SaveDistance);
            XElement mirrorUse = new XElement("MirrorUse", testToConvert.MirrorUse);
            XElement reverseParking = new XElement("ReverseParking", testToConvert.ReverseParking);
            XElement signals = new XElement("Signals", testToConvert.Signals);
            XElement stopSign = new XElement("StopSign", testToConvert.StopSign);
            XElement rightOfWay = new XElement("RightOfWay", testToConvert.RightOfWay);
            XElement controledTheVehicle = new XElement("ControledTheVehicle", testToConvert.ControledTheVehicle);
            XElement passed = new XElement("Passed", testToConvert.Passed);
            XElement faild = new XElement("Faild", testToConvert.Faild);
            XElement GiveWay = new XElement("GiveWay", testToConvert.GiveWay);
            XElement ClipPath = new XElement("ClipPath", testToConvert.ClipPath);
            XElement IsClipUpload = new XElement("IsClipUpload", testToConvert.IsClipUpload);
            XElement noteOfTester = new XElement("NoteOfTester", testToConvert.NoteOfTester);
            XElement statusOfTest = new XElement("StatusOfTest", testToConvert.StatusOfTest.ToString());
            XElement GearType = new XElement("GearType", testToConvert.GearType.ToString());
            XElement TypeOfCar = new XElement("TypeOfCar", testToConvert.TypeOfCar.ToString());
            XElement city = new XElement("City", testToConvert.LocationOfTest.City);
            XElement street = new XElement("Street", testToConvert.LocationOfTest.Street);
            XElement numOfBuilding = new XElement("NumOfBuilding", testToConvert.LocationOfTest.NumOfBuilding);
            XElement locationOfTest = new XElement("LocationOfTest", city, street, numOfBuilding);
            XElement year = new XElement("Year", testToConvert.DateAndHourOfTest.Year);
            XElement month = new XElement("Month", testToConvert.DateAndHourOfTest.Month);
            XElement day = new XElement("Day", testToConvert.DateAndHourOfTest.Day);
            XElement hour = new XElement("Hour", testToConvert.DateAndHourOfTest.Hour);
            XElement dateAndHourOfTest = new XElement("DateAndHourOfTest", year, month, day, hour);
            XElement test = new XElement("Test", serialNumber, testerID, traineeID, dateAndHourOfTest,
                                         locationOfTest, saveDistance, mirrorUse, reverseParking, signals,
                                         stopSign, rightOfWay, controledTheVehicle, passed, faild, IsClipUpload, ClipPath, GiveWay, noteOfTester, GearType, TypeOfCar, statusOfTest);
            return test;
        }

        //-----------------------------------------------------------------------------

        public override int AddTest(Test newTest)
        {
            try
            {
                int serial;
                LoadTests();
                LoadConfigs();
                if (newTest.CurrentTestSerialNumber != 0)
                    throw new ArgumentException("Serial number most be 0");
                serial = int.Parse(ConfigRoot.Element("SerialNumber").Element("value").Value);
                OrderRoot.Add(TestToXElementTest(new Test(serial, newTest)));
                ConfigRoot.Element("SerialNumber").Element("value").Value = (serial + 1).ToString();
                SaveConfigs();
                SaveTests();
                return serial;
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

        public override void UpdateTest(Test updatedTest)
        {
            try
            {
                LoadTests();
                if (TestExist(updatedTest.CurrentTestSerialNumber))
                {
                    (from test in OrderRoot.Elements()
                     where test.Element("TestSerialNumber").Element("value").Value == updatedTest.CurrentTestSerialNumber.ToString()
                     select test).FirstOrDefault().Remove();
                    OrderRoot.Add(TestToXElementTest(updatedTest));
                    SaveTests();
                }
                else
                    throw new KeyNotFoundException("Test to update doesn't exist");
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

        public override void UpdateTestStatus(Test testToUpdate)
        {
            try
            {
                LoadTests();
                if (TestExist(testToUpdate.CurrentTestSerialNumber))
                {
                    (from test in OrderRoot.Elements()
                     where test.Element("TestSerialNumber").Element("value").Value == testToUpdate.CurrentTestSerialNumber.ToString()
                     select test).FirstOrDefault().Remove();
                    OrderRoot.Add(TestToXElementTest(testToUpdate));
                    SaveTests();
                }
                else
                    throw new KeyNotFoundException("Test to update doesn't exist");
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

        public override Test GetTestBySerialNumber(int serialNumber)
        {
            try
            {
                LoadTests();
                if (TestExist(serialNumber))
                {
                    return (from test in OrderRoot.Elements()
                            where test.Element("TestSerialNumber").Element("value").Value == serialNumber.ToString()
                            select new Test(XElementTestToTest(test))).FirstOrDefault();
                }
                else
                    throw new KeyNotFoundException("Test doesn't exist");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override List<Test> GetAllTests()
        {
            try
            {
                LoadTests();
                return new List<Test>((from test in OrderRoot.Elements()
                                       select new Test()
                                       {
                                           TesterID = test.Element("TesterID").Value,
                                           TraineeID = test.Element("TraineeID").Value,
                                           DateAndHourOfTest = new DateTime(int.Parse(test.Element("DateAndHourOfTest").Element("Year").Value),
                                                                      int.Parse(test.Element("DateAndHourOfTest").Element("Month").Value),
                                                                      int.Parse(test.Element("DateAndHourOfTest").Element("Day").Value),
                                                                      int.Parse(test.Element("DateAndHourOfTest").Element("Hour").Value),
                                                                      0, 0),
                                           LocationOfTest = new Address(test.Element("LocationOfTest").Element("City").Value,
                                                                        test.Element("LocationOfTest").Element("Street").Value,
                                                                        int.Parse(test.Element("LocationOfTest").Element("NumOfBuilding").Value)),
                                           StatusOfTest = (TestStatus)Enum.Parse(typeof(TestStatus), test.Element("StatusOfTest").Value),
                                           GearType = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), test.Element("GearType").Value),
                                           TypeOfCar = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), test.Element("TypeOfCar").Value),
                                           GiveWay = bool.Parse(test.Element("GiveWay").Value),
                                           SaveDistance = bool.Parse(test.Element("SaveDistance").Value),
                                           MirrorUse = bool.Parse(test.Element("MirrorUse").Value),
                                           ReverseParking = bool.Parse(test.Element("ReverseParking").Value),
                                           Signals = bool.Parse(test.Element("Signals").Value),
                                           StopSign = bool.Parse(test.Element("StopSign").Value),
                                           RightOfWay = bool.Parse(test.Element("RightOfWay").Value),
                                           ControledTheVehicle = bool.Parse(test.Element("ControledTheVehicle").Value),
                                           Passed = bool.Parse(test.Element("Passed").Value),
                                           Faild = bool.Parse(test.Element("Faild").Value),
                                           NoteOfTester = test.Element("NoteOfTester").Value,
                                           ClipPath = test.Element("ClipPath").Value,
                                           IsClipUpload = bool.Parse(test.Element("IsClipUpload").Value),
                                           CurrentTestSerialNumber = int.Parse(test.Element("TestSerialNumber").Element("value").Value)
                                       }).ToList());
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------

        public override List<Test> FilterTests(Predicate<Test> condition)
        {
            try
            {
                LoadTests();
                List<Test> tests = new List<Test>((from test in OrderRoot.Elements()
                                                   select new Test()
                                                   {
                                                       TesterID = test.Element("TesterID").Value,
                                                       TraineeID = test.Element("TraineeID").Value,
                                                       DateAndHourOfTest = new DateTime(int.Parse(test.Element("DateAndHourOfTest").Element("Year").Value),
                                                                                  int.Parse(test.Element("DateAndHourOfTest").Element("Month").Value),
                                                                                  int.Parse(test.Element("DateAndHourOfTest").Element("Day").Value),
                                                                                  int.Parse(test.Element("DateAndHourOfTest").Element("Hour").Value),
                                                                                  0, 0),
                                                       LocationOfTest = new Address(test.Element("LocationOfTest").Element("City").Value,
                                                                                    test.Element("LocationOfTest").Element("Street").Value,
                                                                                    int.Parse(test.Element("LocationOfTest").Element("NumOfBuilding").Value)),
                                                       StatusOfTest = (TestStatus)Enum.Parse(typeof(TestStatus), test.Element("StatusOfTest").Value),
                                                       GearType = (TypeOfGear)Enum.Parse(typeof(TypeOfGear), test.Element("GearType").Value),
                                                       TypeOfCar = (TypeOfVehicle)Enum.Parse(typeof(TypeOfVehicle), test.Element("TypeOfCar").Value),
                                                       GiveWay = bool.Parse(test.Element("GiveWay").Value),
                                                       SaveDistance = bool.Parse(test.Element("SaveDistance").Value),
                                                       MirrorUse = bool.Parse(test.Element("MirrorUse").Value),
                                                       ReverseParking = bool.Parse(test.Element("ReverseParking").Value),
                                                       Signals = bool.Parse(test.Element("Signals").Value),
                                                       StopSign = bool.Parse(test.Element("StopSign").Value),
                                                       RightOfWay = bool.Parse(test.Element("RightOfWay").Value),
                                                       ControledTheVehicle = bool.Parse(test.Element("ControledTheVehicle").Value),
                                                       Passed = bool.Parse(test.Element("Passed").Value),
                                                       Faild = bool.Parse(test.Element("Faild").Value),
                                                       NoteOfTester = test.Element("NoteOfTester").Value,
                                                       ClipPath = test.Element("ClipPath").Value,
                                                       IsClipUpload = bool.Parse(test.Element("IsClipUpload").Value),
                                                       CurrentTestSerialNumber = int.Parse(test.Element("TestSerialNumber").Element("value").Value)
                                                   }).ToList());
                List<Test> tmp = (from test in tests
                                  where condition(test)
                                  select new Test(test)).ToList();
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
        /// if an exception has been thrown trrow FileLoadException
        /// </summary>
        void CreateConfigFile()
        {
            try
            {
                ConfigRoot = new XElement("Configs");
                XElement readAble = new XElement("readAble", true);
                XElement writeAble = new XElement("writeAble", true);
                XElement notReadAble = new XElement("readAble", false);
                XElement notWriteable = new XElement("writeAble", false);
                XElement minTesterAgeValue = new XElement("value", 40);
                XElement minTraineeAgeValue = new XElement("value", 18);
                XElement minTimeRangeBetweenTestValue = new XElement("value", 7);
                XElement minlessonsValue = new XElement("value", 20);
                XElement maxTesterAgeValue = new XElement("value", 67);
                XElement serialValue = new XElement("value", 10000000);
                XElement serialNumber = new XElement("SerialNumber", serialValue, notReadAble, notWriteable);
                XElement minLessons = new XElement("minLessons", minlessonsValue, readAble, writeAble);
                XElement maxTesterAge = new XElement("maxTesterAge", maxTesterAgeValue, readAble, writeAble);
                XElement minTraineeAge = new XElement("minTraineeAge", minTraineeAgeValue, readAble, writeAble);
                XElement minTimeRangeBetweenTest = new XElement("minTimeRangeBetweenTest", minTimeRangeBetweenTestValue, readAble, writeAble);
                XElement minTesterAge = new XElement("minTesterAge", minTesterAgeValue, readAble, writeAble);
                ConfigRoot.Add(serialNumber, minLessons, maxTesterAge, minTesterAge, minTraineeAge, minTimeRangeBetweenTest);
                ConfigRoot.Save(ConfigPath);
            }
            catch
            {
                throw new FileLoadException("Can not start the project check your Config Xml files");
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
                Dictionary<string, object> confings = new Dictionary<string, object>();
                var objFoeDictionary = (from cnfg in ConfigRoot.Elements()
                                        where cnfg.Name.LocalName != "SerialNumber"
                                        where cnfg.Element("readAble").Value == "true"
                                        select new { Key = cnfg.Name.LocalName, Value = cnfg.Element("value").Value });
                foreach (var item in objFoeDictionary)
                {
                    confings.Add(item.Key, item.Value as object);
                }
                if (confings.Any())
                {
                    return new Dictionary<string, object>(confings);
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
        
    

