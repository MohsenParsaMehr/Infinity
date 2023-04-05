using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using Infinity.Classes;
using Infinity.Controllers;

namespace Infinity.Utilities
{
    public class Security
    {
        public static string analyzeRequest(EmptyEnvelope dataEnvelope,bool ignoreRemainingCheckOnFail = false,
            bool checkCredentials=true, bool checkBlackListHistory = true,bool checkBrouthForceAttack=true,
            bool checkServiceDenialAttack=true,bool checkTampering = true)
        {
            
            
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);
            string reasonPhrase = "";
                        
            // open envelope and decrypt and split credential info, if login with credential info succeeds, 
            // check for request quantity to not be a brouth force one, then check timestamp 
            // of request to inspect if not expired, then check for ip Address to ensure not backlisted
            if (checkBrouthForceAttack && dataEnvelope.Counter > Constants.MAX_SERVICE_REQUEST_COUNT)
            {
                reasonPhrase += Constants.ReasonPhrases.BROUTH_FORCE_ATTACK_DETECTED;
                if (ignoreRemainingCheckOnFail)
                    return reasonPhrase;
            }

            //string token = Encryption.decryptSymmetric(dataEnvelope.Token);
            //string[] tokenParts = token.Split(new string[]{"|||"},StringSplitOptions.None);
            List<BlackList> blackListRecords = new List<BlackList>();
            BlackList blackListInfo = new BlackList();
            blackListInfo.IpAddress = dataEnvelope.IpAddress;
            //blackListInfo.UserRef =
            Envelope <BlackList> blackListEnvelope = new Envelope<BlackList>();
            blackListEnvelope.IpAddress = dataEnvelope.IpAddress;
            blackListEnvelope.Username = dataEnvelope.Username;
            blackListEnvelope.Password = dataEnvelope.Password;

            blackListEnvelope.Counter = dataEnvelope.Counter;
            blackListEnvelope.EntityJson = new JavaScriptSerializer().Serialize(blackListInfo);

            //if(checkBlackListHistory && (blackListRecords = new BlackListsController().GetBlackLists(blackListEnvelope)) != null){  
            //    response.ReasonPhrase += "|"+Constants.ReasonPhrases.IP_OR_USER_BLACKLISTED;
            //    if (ignoreRemainingCheckOnFail)
            //        return response;
            //}      
            if(checkCredentials){
                Envelope<User> userEnvelope = new Envelope<User>();
                userEnvelope.Counter = 0;
                userEnvelope.IpAddress = dataEnvelope.IpAddress;
                userEnvelope.Entity = new User() { Username = dataEnvelope.Username, Password = dataEnvelope.Password };
                HttpResponseMessage loginResponse = new UsersController().Login(userEnvelope);
                if (loginResponse.StatusCode != HttpStatusCode.OK)
                {
                    reasonPhrase += "|" + Constants.ReasonPhrases.AUTHENTICATION_FAILD;
                    if (ignoreRemainingCheckOnFail)
                        return reasonPhrase;
                }
            }
            if (checkTampering && dataEnvelope.isTampered())
            {
                 reasonPhrase += "|"+Constants.ReasonPhrases.ENVELOPE_TAMPERED;
                    if (ignoreRemainingCheckOnFail)
                        return reasonPhrase;
            }
            //if(checkForbiddenPhrase){                
            //    // now analyze any texts to see if any bad word (including political word) used, if so reject based on severity accordingly                
            //    Penalties penalty;
            //    if ((penalty = new ContentAnalyzer().analyzeForForbiddenPhrases(false, userInputTexts)) != null)
            //    {
            //        response.ReasonPhrase += "|"+Constants.ReasonPhrases.CONTAIN_FORBIDDEN_PHRASE;
            //        if (ignoreRemainingCheckOnFail)
            //            return response;
            //    }
            //}
            
            //if (checkContactInfoEmbedding && new ContentAnalyzer().hasContactInfoPattern(userInputTexts))
            //{
            //    response.ReasonPhrase += "|"+Constants.ReasonPhrases.CONTACT_INFO_EMBEDED;
            //        if (ignoreRemainingCheckOnFail)
            //            return response;
            //}
            if (!ignoreRemainingCheckOnFail && !string.IsNullOrEmpty(reasonPhrase))
            {
                return reasonPhrase;
            }
            return null;
                
        }
        public static string analyzeRequest<T>(Envelope<T> dataEnvelope, bool ignoreRemainingCheckOnFail = true,
            bool checkCredentials = true, bool checkForbiddenPhrase = true, bool checkContactInfoEmbedding = true,
            bool checkBlackListHistory = true, bool checkBrouthForceAttack = true,
            bool checkServiceDenialAttack = true, bool checkTampering = true, params string[] userInputTexts)
        {

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound);
            string reasonPhrase = "";

            // open envelope and decrypt and split credential info, if login with credential info succeeds, 
            // check for request quantity to not be a brouth force one, then check timestamp 
            // of request to inspect if not expired, then check for ip Address to ensure not backlisted
            if (checkBrouthForceAttack && dataEnvelope.Counter > Constants.MAX_SERVICE_REQUEST_COUNT)
            {
                reasonPhrase += Constants.ReasonPhrases.BROUTH_FORCE_ATTACK_DETECTED;
                if (ignoreRemainingCheckOnFail)
                    return reasonPhrase;
            }

            //string token = Encryption.decryptSymmetric(dataEnvelope.Token);
            //string[] tokenParts = token.Split(new string[]{"|||"},StringSplitOptions.None);
            List<BlackList> blackListRecords = new List<BlackList>();
            BlackList blackListInfo = new BlackList();
            blackListInfo.IpAddress = dataEnvelope.IpAddress;
            //blackListInfo.UserRef =
            Envelope<BlackList> blackListEnvelope = new Envelope<BlackList>();
            blackListEnvelope.IpAddress = dataEnvelope.IpAddress;
            blackListEnvelope.Username = dataEnvelope.Username;
            blackListEnvelope.Password = dataEnvelope.Password;

            blackListEnvelope.Counter = dataEnvelope.Counter;
            blackListEnvelope.EntityJson = new JavaScriptSerializer().Serialize(blackListInfo);

            //if(checkBlackListHistory && (blackListRecords = new BlackListsController().GetBlackLists(blackListEnvelope)) != null){  
            //    response.ReasonPhrase += "|"+Constants.ReasonPhrases.IP_OR_USER_BLACKLISTED;
            //    if (ignoreRemainingCheckOnFail)
            //        return response;
            //}      
            if (checkCredentials)
            {
                Envelope<User> userEnvelope = new Envelope<User>();
                userEnvelope.Counter = 0;
                userEnvelope.IpAddress = dataEnvelope.IpAddress;
                userEnvelope.Entity = new User() { Username = dataEnvelope.Username, Password = dataEnvelope.Password };
                HttpResponseMessage loginResponse = new UsersController().Login(userEnvelope);
                if (loginResponse.StatusCode != HttpStatusCode.OK)
                {
                    reasonPhrase += "|" + Constants.ReasonPhrases.AUTHENTICATION_FAILD;
                    if (ignoreRemainingCheckOnFail)
                        return reasonPhrase;
                }
            }
            if (checkTampering && dataEnvelope.isTampered())
            {
                reasonPhrase += "|" + Constants.ReasonPhrases.ENVELOPE_TAMPERED;
                if (ignoreRemainingCheckOnFail)
                    return reasonPhrase;
            }
            if (checkForbiddenPhrase)
            {
                // now analyze any texts to see if any bad word (including political word) used, if so reject based on severity accordingly                
                Penalty penalty;
                if ((penalty = new ContentAnalyzer().analyzeForForbiddenPhrases(false, userInputTexts)) != null)
                {
                    reasonPhrase += "|" + Constants.ReasonPhrases.CONTAIN_FORBIDDEN_PHRASE;
                    if (ignoreRemainingCheckOnFail)
                        return reasonPhrase;
                }
            }

            if (checkContactInfoEmbedding && new ContentAnalyzer().hasContactInfoPattern(userInputTexts))
            {
                reasonPhrase += "|" + Constants.ReasonPhrases.CONTACT_INFO_EMBEDED;
                if (ignoreRemainingCheckOnFail)
                    return reasonPhrase;
            }
            if (!ignoreRemainingCheckOnFail && !string.IsNullOrEmpty(reasonPhrase))
            {
                return reasonPhrase;
            }
            return null;

        }

    }
}