using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infinity.Utilities;
using System.Web.Script.Serialization;

namespace Infinity.Classes
{
    public class EmptyEnvelope
    {
        public String IpAddress { get; set; }
        public int Counter { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public int? AppVersion { get; set; }
        public String AdditionalInfo { get; set; }
        public String Credentials { get; set; }
        public String OsVersion { get; set; }
        public String ApiLevel { get; set; }
        public String Device { get; set; }
        public String Model { get; set; }
        public String Product { get; set; }
        public String DeviceIMEI { get; set; }
        public String MACAddress { get; set; }
        public String PhoneNo { get; set; }
        public SearchCriterias searchCriterias = null;//new SearchCriterias();
        public SortCriterias sortCriterias = null;
        public bool isTampered()
        {
            
            string data = IpAddress + "|||" + (OsVersion != null ? (OsVersion + "|||" + ApiLevel + "|||" + Device + "|||" + Model + "|||" + Product) + "|||" : "") +
                Counter + (Username != null ? Username + "|||" + Password + "|||" : "") +
                (AppVersion != null ? AppVersion + "|||" : "") + (AdditionalInfo != null ? AdditionalInfo + "|||" : "") +(DeviceIMEI != null ? DeviceIMEI+"|||":"")+
                (MACAddress != null?MACAddress+"|||":"")+(PhoneNo != null ? PhoneNo : "") +
                (searchCriterias == null ? "" : searchCriterias + "|||") + (sortCriterias == null ? "" : sortCriterias + "|||");
            return (Credentials != Encryption.sha256_hash(data));
        }

    }
    public class Envelope<T> : EmptyEnvelope
    {        
        public string EntityJson { get; set; }        

        private T _entity;
        //private bool isEntityNull = true;
        public T Entity
        {
            get
            {
                if(_entity == null)
                    _entity = new JavaScriptSerializer().Deserialize<T>(EntityJson);
                return _entity;
            }
            set
            {
                _entity = value;
            }
        }

        public  bool  isTampered()
        {
            string data = IpAddress + "|||" + (OsVersion != null ? (OsVersion + "|||" + ApiLevel + "|||" + Device + "|||" + Model + "|||" + Product) + "|||" : "") +
                Counter + (Username != null ? Username + "|||" + Password + "|||" : "") +
                ( AppVersion != null ? AppVersion + "|||" : "") + (AdditionalInfo != null ? AdditionalInfo + "|||" : "") +
                (DeviceIMEI != null ? DeviceIMEI + "|||" : "")+(MACAddress!=null?MACAddress+"|||":"")+(PhoneNo != null ? PhoneNo+"|||":"") +
                (searchCriterias == null ? "" : searchCriterias + "|||") + (sortCriterias == null ? "" : sortCriterias + "|||") + EntityJson;            
            return (Credentials != Encryption.sha256_hash(data));

        }
    }
}