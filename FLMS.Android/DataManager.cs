using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RentACar.UI.Modals;
using System.IO;
using SQLite;

namespace RentACar.UI
{
    class DataManager
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "RentACar.db3");

        public long SaveUserDetailsToLocal(UserDetail userDetail)
        {

            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    return db.Insert(userDetail);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }

        public long UpdateUserDetailsToLocal(UserDetail userDetail)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    db.DeleteAll<UserDetail>();
                    return db.Insert(userDetail);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }

        //retrieve a specific user by querying against their first name
        public UserDetail GetUser()
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<UserDetail>().FirstOrDefault();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        //retrieve a list of all customers
        public IList<UserDetail> GetAllUsers()
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<UserDetail>().ToList();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        //save running vehicle transaction finally
        public long SaveRentRunningTransToLocal(RentRunningTrans rentRunningTrans)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    VehicleRent objVehicleRent = new VehicleRent();
                    objVehicleRent.VehicleId = rentRunningTrans.VehicleId;
                    objVehicleRent.VehicleType = rentRunningTrans.VehicleType;
                    objVehicleRent.TransType = rentRunningTrans.TransType;
                    objVehicleRent.RegNo = rentRunningTrans.RegNo;
                    objVehicleRent.Email = rentRunningTrans.Email;
                    objVehicleRent.Mobile = rentRunningTrans.Mobile;
                    objVehicleRent.Mileage = rentRunningTrans.Mileage;
                    objVehicleRent.FuelLevel = rentRunningTrans.FuelLevel;
                    objVehicleRent.AlloyWheel = rentRunningTrans.AlloyWheel;
                    objVehicleRent.Bonet = rentRunningTrans.Bonet;
                    objVehicleRent.BrakeFluid = rentRunningTrans.BrakeFluid;
                    objVehicleRent.Coolant = rentRunningTrans.Coolant;
                    objVehicleRent.Disc = rentRunningTrans.Disc;
                    objVehicleRent.Engine = rentRunningTrans.Engine;
                    objVehicleRent.FrontBumper = rentRunningTrans.FrontBumper;
                    objVehicleRent.Interior = rentRunningTrans.Interior;
                    objVehicleRent.NSDoor = rentRunningTrans.NSDoor;
                    objVehicleRent.NSFWheel = rentRunningTrans.NSFWheel;
                    objVehicleRent.NSRWheel = rentRunningTrans.NSRWheel;
                    objVehicleRent.Oil = rentRunningTrans.Oil;
                    objVehicleRent.OSDoor = rentRunningTrans.OSDoor;
                    objVehicleRent.OSFWheel = rentRunningTrans.OSFWheel;
                    objVehicleRent.OSRWheel = rentRunningTrans.OSRWheel;
                    objVehicleRent.RearBumper = rentRunningTrans.RearBumper;
                    objVehicleRent.Roof = rentRunningTrans.Roof;
                    objVehicleRent.SpareTyre = rentRunningTrans.SpareTyre;
                    objVehicleRent.Taligate = rentRunningTrans.Taligate;
                    objVehicleRent.Tools = rentRunningTrans.Tools;
                    objVehicleRent.Tyres = rentRunningTrans.Tyres;
                    objVehicleRent.WasherFluid = rentRunningTrans.WasherFluid;
                    objVehicleRent.WindScreen = rentRunningTrans.WindScreen;
                    objVehicleRent.DamageDetail = rentRunningTrans.DamageDetail;
                    objVehicleRent.LooseItemDetail = rentRunningTrans.LooseItemDetail;
                    objVehicleRent.InspectionCondition = rentRunningTrans.InspectionCondition;
                    objVehicleRent.CleanedBy = rentRunningTrans.CleanedBy;
                    objVehicleRent.CheckedoutBy = rentRunningTrans.CheckedoutBy;
                    objVehicleRent.DriverSignatureData = rentRunningTrans.DriverSignatureData;
                    objVehicleRent.CustomerSignatureData = rentRunningTrans.CustomerSignatureData;
                    // objVehicleRent.TimeOut

                    db.Insert(objVehicleRent);
                    //return objVehicleRent.VehicleTransID;
                    if (rentRunningTrans.RentVehicleDamage != null)
                    {
                        rentRunningTrans.RentVehicleDamage.All(c => { c.VehicleTransID = objVehicleRent.VehicleTransID; return true; });
                        db.InsertAll(rentRunningTrans.RentVehicleDamage);
                    }

                    if (objVehicleRent.VehicleTransID > 0)
                    {
                        this.UpdateVehicleStatus(objVehicleRent.VehicleId, objVehicleRent.TransType);
                        return objVehicleRent.VehicleTransID;
                    }
                    else
                    {
                        return -1;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }

        public long UpdateVehicleStatus(int VehicleId, string VehicleStatus)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    var objVehicle = db.Get<Vehicle>(VehicleId);
                    objVehicle.RentStatus = VehicleStatus;

                    return db.Update(objVehicle);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }

        //retrieve a list of all Vehicles as per action status passed 
        public IList<Vehicle> GetVehicles(string RequiredStatus)
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<Vehicle>().Where(a => a.RentStatus == RequiredStatus).ToList();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        //retrieve a vehicle details by querying against their rent status 
        public RentRunningTrans GetVehicleRentLastTransactionDetails(int VehicleId, string VehicleStatus)
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    RentRunningTrans objRentRunningTrans;
                    VehicleRent objVehicleRent = database.Table<VehicleRent>().Where(a => a.TransType == VehicleStatus && a.VehicleId == VehicleId).OrderByDescending(x => x.VehicleTransID).FirstOrDefault();
                    if (objVehicleRent != null)
                    {
                        objRentRunningTrans = new RentRunningTrans();
                        //objRentRunningTrans =(RentRunningTrans) objVehicleRent;
                        objRentRunningTrans.VehicleId = objVehicleRent.VehicleId;
                        objRentRunningTrans.VehicleType = objVehicleRent.VehicleType;
                        objRentRunningTrans.TransType = objVehicleRent.TransType;
                        objRentRunningTrans.RegNo = objVehicleRent.RegNo;
                        objRentRunningTrans.Email = objVehicleRent.Email;
                        objRentRunningTrans.Mobile = objVehicleRent.Mobile;
                        objRentRunningTrans.Mileage = objVehicleRent.Mileage;
                        objRentRunningTrans.FuelLevel = objVehicleRent.FuelLevel;
                        objRentRunningTrans.AlloyWheel = objVehicleRent.AlloyWheel;
                        objRentRunningTrans.Bonet = objVehicleRent.Bonet;
                        objRentRunningTrans.BrakeFluid = objVehicleRent.BrakeFluid;
                        objRentRunningTrans.Coolant = objVehicleRent.Coolant;
                        objRentRunningTrans.Disc = objVehicleRent.Disc;
                        objRentRunningTrans.Engine = objVehicleRent.Engine;
                        objRentRunningTrans.FrontBumper = objVehicleRent.FrontBumper;
                        objRentRunningTrans.Interior = objVehicleRent.Interior;
                        objRentRunningTrans.NSDoor = objVehicleRent.NSDoor;
                        objRentRunningTrans.NSFWheel = objVehicleRent.NSFWheel;
                        objRentRunningTrans.NSRWheel = objVehicleRent.NSRWheel;
                        objRentRunningTrans.Oil = objVehicleRent.Oil;
                        objRentRunningTrans.OSDoor = objVehicleRent.OSDoor;
                        objRentRunningTrans.OSFWheel = objVehicleRent.OSFWheel;
                        objRentRunningTrans.OSRWheel = objVehicleRent.OSRWheel;
                        objRentRunningTrans.RearBumper = objVehicleRent.RearBumper;
                        objRentRunningTrans.Roof = objVehicleRent.Roof;
                        objRentRunningTrans.SpareTyre = objVehicleRent.SpareTyre;
                        objRentRunningTrans.Taligate = objVehicleRent.Taligate;
                        objRentRunningTrans.Tools = objVehicleRent.Tools;
                        objRentRunningTrans.Tyres = objVehicleRent.Tyres;
                        objRentRunningTrans.WasherFluid = objVehicleRent.WasherFluid;
                        objRentRunningTrans.WindScreen = objVehicleRent.WindScreen;
                        objRentRunningTrans.DamageDetail = objVehicleRent.DamageDetail;
                        objRentRunningTrans.LooseItemDetail = objVehicleRent.LooseItemDetail;
                        objRentRunningTrans.InspectionCondition = objVehicleRent.InspectionCondition;
                        objRentRunningTrans.CleanedBy = objVehicleRent.CleanedBy;
                        objRentRunningTrans.CheckedoutBy = objVehicleRent.CheckedoutBy;
                        objRentRunningTrans.DriverSignatureData = objVehicleRent.DriverSignatureData;
                        objRentRunningTrans.CustomerSignatureData = objVehicleRent.CustomerSignatureData;

                        IList<VehicleMarkDamageDetails> objVehicleMarkDamageDetails = database.Table<VehicleMarkDamageDetails>().Where(a => a.VehicleTransID == objVehicleRent.VehicleTransID).ToList();
                        if (objVehicleMarkDamageDetails.Count > 0)
                        {
                            if (objRentRunningTrans.RentVehicleDamage == null)
                            {
                                objRentRunningTrans.RentVehicleDamage = new List<VehicleMarkDamageDetails>();

                            }
                            foreach (VehicleMarkDamageDetails obj in objVehicleMarkDamageDetails)
                            {
                                objRentRunningTrans.RentVehicleDamage.Add(obj);
                            }
                        }

                    }
                    else
                    {
                        objRentRunningTrans = null;
                    }

                    return objRentRunningTrans;
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        public int CreateUserTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("UserDetail").Any())
                    {
                        return db.CreateTable<UserDetail>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        #region "SmsToSend"
        public int createSmsToSendTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("SmsToSend").Any())
                    {
                        return db.CreateTable<SmsToSend>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       
        #endregion

        #region EmailToSend
        public int CreateEmailToSendTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("EmailToSend").Any())
                    {
                        return db.CreateTable<EmailToSend>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
       
        public long SaveEmailToLocal(EmailToSend email)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    //EmailToSend objSaveEmail = new EmailToSend();
                    //objSaveEmail.EmailId = email.EmailId;
                    //objSaveEmail.EmailTemplate = email.EmailTemplate;
                    //objSaveEmail.EmailBody = email.EmailBody;
                    //objSaveEmail.DateTime = email.DateTime;
                    db.Insert(email);
                    return email.Id;
                }

                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }
        #endregion

        #region "Sms template"
        public int CreateSmsTemplateTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("SmsTemplate").Any())
                    {
                        return db.CreateTable<SmsTemplate>();
                    }
                    else
                    {
                        return 1;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public SmsTemplate GetSmsTemplateByCode(string templateTitle)
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<SmsTemplate>().Where(a => a.SmsTitle == templateTitle
                    ).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        public IList<SmsTemplate> GetSmsTemplate()
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<SmsTemplate>().ToList();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }
        public long SaveSmsToLocal(SmsToSend sms)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    //SmsToSend objSaveSms = new SmsToSend();
                    //objSaveSms.MobileNumber = sms.MobileNumber;
                    //objSaveSms.SmsTemplate = sms.SmsTemplate;
                    //objSaveSms.SmsBody = sms.SmsBody;
                    //objSaveSms.DateTime = sms.DateTime;
                    db.Insert(sms);
                    return sms.Id;
                }

                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }
        #endregion

        #region "Setting"
        public int CreateSettingTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("Setting").Any())
                    {
                        return db.CreateTable<Setting>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public int AddDefaultSetting()
        {
            try
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    if (db.Table<Setting>().Count() == 0)
                    {
                        Setting setting;
                        List<Setting> oSetting = new List<Setting>();
                        setting = new Setting();
                        setting.SMTPServer = "smtp.gmail.com";
                        setting.UserName = "";
                        setting.EmailPassword = "";
                        setting.EmailProtNumber = Convert.ToInt32("587");
                        setting.EmailIsSecure = true;
                        setting.EmailSenderName = "";
                        setting.EmailReplyTo = "";
                        setting.SmsAPIUsername = "";
                        setting.SmsAPIPassword = "";
                        setting.SmsAPIUrl = "";
                        setting.ServiceEndPoint = "";
                        setting.AutoSync = true;
                        return db.InsertAll(oSetting);
                    }
                    else
                    {
                        return 1;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public long SaveSettingToLocal(Setting objSaveSetting)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    Setting objSetting = this.GetSetting();
                    objSetting.AutoSync = objSaveSetting.AutoSync;
                    objSetting.ServiceEndPoint = objSaveSetting.ServiceEndPoint;

                    return db.Update(objSetting);
                }

                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }

        public Setting GetSetting()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    return db.Table<Setting>().FirstOrDefault();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        //public long UpdateSettingToLocal(int Id, bool autoSync)
        //{
        //    using (var db = new SQLiteConnection(dbPath))
        //    {
        //        try
        //        {
        //            var objSetting = db.Get<Setting>(Id);
        //            objSetting.AutoSync = autoSync;
        //            return db.Update(objSetting);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //            //exception handling code to go here
        //        }
        //    }
        //}
        #endregion

        #region "Email template"
        public int CreateEmailTemplateTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("EmailTemplate").Any())
                    {
                        return db.CreateTable<EmailTemplate>();
                    }
                    else
                    {
                        return 1;
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public EmailTemplate GetEmailTemplateByCode(string templateTitle)
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<EmailTemplate>().Where(a => a.EmailTitle == templateTitle
                    ).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        public IList<EmailTemplate> GetEmailTemplate()
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<EmailTemplate>().ToList();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }
       
        #endregion

        public int CreateVehicleTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("Vehicle").Any())
                    {
                        return db.CreateTable<Vehicle>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        public int CreateVehicleRentTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("VehicleRent").Any())
                    {
                        return db.CreateTable<VehicleRent>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

        public int CreateVehicleMarkDamageDetailsTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("VehicleMarkDamageDetails").Any())
                    {
                        return db.CreateTable<VehicleMarkDamageDetails>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }
        public int CreateVehicleMarkedDamageImageTable()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    if (!db.GetTableInfo("VehicleMarkedDamageImage").Any())
                    {
                        return db.CreateTable<VehicleMarkedDamageImage>();
                    }
                    else
                    {
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }
        public int AddDefaultSmsTemplate()
        {
            try
            {
                using (var db = new SQLiteConnection(dbPath))
                    if (db.Table<SmsTemplate>().Count() == 0)
                    {
                        SmsTemplate smstemplate;
                        List<SmsTemplate> stemp = new List<SmsTemplate>();
                        smstemplate = new SmsTemplate();
                        smstemplate.SmsTitle = "Collected";
                        smstemplate.SmsBody = "Your Manufacturer Registration has been collected. Please contact Thrifty on 01482 888999 if you have any questions.";
                        stemp.Add(smstemplate);

                        smstemplate = new SmsTemplate();
                        smstemplate.SmsTitle = "Delivered Home";
                        smstemplate.SmsBody = "Your Manufacturer Registration has been delivered to your home address. Please contact Thrifty on 01482 888999 if you have any questions.";
                        stemp.Add(smstemplate);

                        smstemplate = new SmsTemplate();
                        smstemplate.SmsTitle = "Delivered Office";
                        smstemplate.SmsBody = "Your Manufacturer Registration has been delivered to your office address. Please contact Thrifty on 01482 888999 if you have any questions.";
                        stemp.Add(smstemplate);
                        return db.InsertAll(stemp);
                    }
                    else
                    {
                        return 1;
                    }
            }

            catch
            {
                throw;
            }
        }
        public int AddDefaultEmailTemplate()
        {
            try
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    if (db.Table<EmailTemplate>().Count() == 0)
                    {
                        EmailTemplate emailtemplate;
                        List<EmailTemplate> etemp = new List<EmailTemplate>();
                        emailtemplate = new EmailTemplate();
                        emailtemplate.EmailId = Convert.ToInt32("1");
                        emailtemplate.EmailTitle = "Car Rental Agreement";
                        emailtemplate.EmailBody = "Dear Customer, Please find attached your car rental agreement.  We will update you on the progress of the process once it has started.  In the meantime please contact our office on the number below if you need any further information.Regards,Thrifty Phone: 01482 888999";
                        etemp.Add(emailtemplate);
                        return db.InsertAll(etemp);
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            catch
            {
                throw;
            }
        }

        public int AddDefaultVehicles()
        {
            try
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    if (db.Table<Vehicle>().Count() == 0)
                    {
                        //insert default data for now
                        Vehicle vehicle;
                        List<Vehicle> oVehicles = new List<Vehicle>();
                        vehicle = new Vehicle();
                        vehicle.Make = "FORD";
                        vehicle.Model = "Fiesta";
                        vehicle.RegNumber = "XX60HJX";
                        vehicle.RentStatus = "IN";
                        vehicle.VehicleType = "Car";
                        oVehicles.Add(vehicle);

                        vehicle = new Vehicle();
                        vehicle.Make = "FORD";
                        vehicle.Model = "Fiesta";
                        vehicle.RegNumber = "KN60HJX";
                        vehicle.RentStatus = "IN";
                        vehicle.VehicleType = "Luton Van";
                        oVehicles.Add(vehicle);

                        vehicle = new Vehicle();
                        vehicle.Make = "FORD";
                        vehicle.Model = "Fiesta";
                        vehicle.RegNumber = "HL66JUI";
                        vehicle.RentStatus = "IN";
                        vehicle.VehicleType = "Standard Van";
                        oVehicles.Add(vehicle);

                        vehicle = new Vehicle();
                        vehicle.Make = "FORD";
                        vehicle.Model = "Fiesta";
                        vehicle.RegNumber = "KM12AKK";
                        vehicle.RentStatus = "IN";
                        vehicle.VehicleType = "Car";
                        oVehicles.Add(vehicle);

                        return db.InsertAll(oVehicles);
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Logout()
        {

            using (var db = new SQLiteConnection(dbPath))
            {
                try
                {
                    db.DeleteAll<UserDetail>();
                }
                catch (Exception ex)
                {
                    throw ex;
                    //exception handling code to go here
                }
            }
        }

        public IList<Vehicle> GetVehicleDetailsFromOnline()
        {
            using (var database = new SQLiteConnection(dbPath))
            {
                try
                {
                    return database.Table<Vehicle>().ToList();
                }
                catch (Exception ex)
                {
                    //exception handling code to go here
                    throw ex;
                }
            }
        }

    }
}