using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infinity
{
    public class Constants
    {
        public struct RecordStatus
        {
            public const string ACTIVE = "Active";
            public const string INACTIVE = "Inactive";
            public const string UNCONFIRMED = "Unconfirmed";
            public const string IMAGE_UNCONFIRMED = "ImageUnconfirmed";
            public const string SUGGESTED = "Suggested";

        }
        public struct ReasonPhrases
        {
            public const string USER_NOT_ACTIVE = "Record Inactive";
            public const string CONTACT_INFO_EMBEDED = "#500";
            public const string CONTAIN_FORBIDDEN_PHRASE = "#501";
            public const string BROUTH_FORCE_ATTACK_DETECTED = "#502";
            public const string AUTHENTICATION_FAILD = "#503";
            public const string IP_OR_USER_BLACKLISTED = "#504";
            public const string SERVICE_DENIAL_ATTACK_DETECTED = "#505";
            public const string ENVELOPE_TAMPERED = "#506";
        }
        public const int MAX_IMAGE_SIZE = 2097152; // in bytes
        public const int MAX_SERVICE_REQUEST_COUNT = 20;
        
        public const string PARTS_CATEGORY_TITLE_IN_DB = "قطعات و لوازم";

        public const string USERNAME_ALREADY_TAKEN = "نام کاربری قبلاً گرفته شده است";
        
        public class Settings
        {
            
            public const int MAX_CONTACT_INFO_INCLUDE_ATTEMPT = 8;


        }
    }
    
}