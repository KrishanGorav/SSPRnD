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
using SQLite;
using System.Drawing;
using System.Net;

namespace RentACar.UI.Modals
{
    [Serializable]
    class UserDetail
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int userid { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
        public string PostCode { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int UserDefaultVehicle { get; set; }
        public string token { get; set; }

        //user_id INT Primary Key IDENTITY,
        //username VARCHAR(255),
        //password VARCHAR(255),
        //email_address VARCHAR(255),
        //FirstName VARCHAR(255),			
        //LastName VARCHAR(255),			
        //mobile_number VARCHAR(25),
        //date_created Datetime,
        //      Address1            VARCHAR(255),			
        //Address2 VARCHAR(255),			
        //Address3 VARCHAR(255),			
        //City VARCHAR(255),			
        //Country VARCHAR(255)
    }

    [Serializable]
    class Vehicle
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int VehicleId { get; set; }
        public string RegNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VehicleType { get; set; }
  //      Registration_number VARCHAR(15),
		//Make VARCHAR(15),	
		//Model VARCHAR(15),
		//on_date DateTime,
  //      off_date                    DateTime,	
		//type Varchar(30)
    }

    [Serializable]
    class UserVehicle
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int userId { get; set; }
        public int VehicleId { get; set; }
        //User_id INT,
        //vehicle_id                  INT,
    }

    [Serializable]
    class Journey
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int JourneyId { get; set; }
        public int VehicleId { get; set; }
        public int userId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        //  Journey_id INT Primary Key  IDENTITY,
        //Vehicle_id INT,
        //      user_id         INT,
        //Startdate datetime,
        //      enddate         datetime,
    }

    [Serializable]
    class JourneyDetail
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int JourneyId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    [Serializable]
    class VehicleRent
    {
        [PrimaryKey, AutoIncrement]
        public int VehicleTransID { get; set; }
        public int VehicleId { get; set; }
        public string TransType { get; set; }
        public string VehicleType { get; set; }
        public string RegNo { get; set; }
        public string FuelLevel { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Mileage { get; set; }
        public string MarkDamageImagePath { get; set; }
        public string DamageDetail { get; set; }
        public string LooseItemDetail { get; set; }
        public int InspectionCondition { get; set; }
        public bool Interior { get; set; }
        public bool Roof { get; set; }
        public bool WindScreen { get; set; }
        public bool Bonet { get; set; }
        public bool Engine { get; set; }
        public bool FrontBumper { get; set; }
        public bool NSFWheel { get; set; }
        public bool NSDoor { get; set; }
        public bool NSRWheel { get; set; }
        public bool RearBumper { get; set; }
        public bool Taligate { get; set; }
        public bool Tools { get; set; }
        public bool OSRWheel { get; set; }
        public bool OSDoor { get; set; }
        public bool OSFWheel { get; set; }
        public bool Disc { get; set; }
        public bool SpareTyre { get; set; }
        public bool AlloyWheel { get; set; }
        public bool Oil { get; set; }
        public bool WasherFluid { get; set; }
        public bool Coolant { get; set; }
        public bool BrakeFluid { get; set; }
        public bool Tyres { get; set; }
        public int CleanedBy { get; set; }
        public int CheckedoutBy { get; set; }
        public int CheckedinBy { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime TimeIn { get; set; }
        public byte[] CustomerSignature { get; set; }
        public string CustomerSignatureData { get; set; }
        public byte[] DriverSignature { get; set; }
        public string DriverSignatureData { get; set; }
    }
    [Serializable]
    class Setting
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string SMTPServer { get; set; }
        public string username { get; set; }
        public string EmailPassword { get; set; }
        public int EmailProtNumber { get; set; }
        public Boolean EmailIsSecure { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailReplyTo { get; set; }
        public string SmsAPIUsername { get; set; }
        public string SmsAPIPassword { get; set; }
        public string SmsAPIUrl { get; set; }
        public string ServiceEndPoint { get; set; }
        public bool AutoSync { get; set; }
        public string ServiceToken { get; set; }
    }
    [Serializable]
    class SmsToSend
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string SmsTemplate { get; set; }
        public string SmsBody { get; set; }
        public DateTime DateTime { get; set; }
        public string userId { get; set; }
        public int JobId { get; set; }
    }
    [Serializable]
    class EmailToSend
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string EmailId { get; set; }
        public string EmailTemplate { get; set; }
        public string EmailBody { get; set; }
        public DateTime DateTime { get; set; }
        public string userId { get; set; }
        public int JobId { get; set; }
    }

    [Serializable]
    class SmsTemplate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int SmsId { get; set; }
        public string SmsTitle { get; set; }
        public string SmsBody { get; set; }
    }
    [Serializable]
    class EmailTemplate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int EmailId { get; set; }
        public string EmailTitle { get; set; }
        public string EmailBody { get; set; }
    }

    [Serializable]
    class VehicleMarkDamageDetails
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int VehicleTransID { get; set; }
        public int DamageImageId { get; set; }
        public string DamageType { get; set; }
        public int DamageLocationX { get; set; }
        public int DamageLocationY { get; set; }
        public int DamageNumber { get; set; }
        public string ImagePath { get; set; }
    }

    [Serializable]
    class VehicleMarkedDamageImage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int VehicleTransID { get; set; }
        public int DamageImageId { get; set; }
        public string MarkDamageImagePath { get; set; }
    }

    [Serializable]
    class RentRunningTrans : VehicleRent
    {
        public List<VehicleMarkDamageDetails> RentVehicleDamage { get; set; }
        public List<VehicleMarkedDamageImage> RentVehicleDamageImage { get; set; }
    }

    [Serializable]
    class VehicleImage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        //public int VehicleTransID { get; set; }
        public string ImageUrl { get; set; }
        public string ExpiryDate { get; set; }
        public string ViewPoint { get; set; }
    }

    [Serializable]
    class VehicleImageList
    {
        public List<VehicleImage> VehicleImages { get; set; }
    }

    /// <summary>
    /// Class used to communicate with API calls
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //[Serializable]
    public class GetAPIResult<T>
    {
        public HttpStatusCode HttpStatus { get; set; }
        public List<T> DataColl { get; set; }
        public T Data { get; set; }
        public string Da { get; set; }
        public string Headers { get; set; }
        public string Request { get; set; }
        public string Content { get; set; }
        public string KnownException { get; set; }
    }
}