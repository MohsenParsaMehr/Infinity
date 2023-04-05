using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Infinity.Utilities
{
    public class ContentAnalyzer
    {
        InfinityEntities entities = new InfinityEntities();
        public ContentAnalyzer()
        {
            
        }
        public Penalty analyzeForForbiddenPhrases(bool partialComparsion = false, params string[] phrases)
        {
            foreach (string phrase in phrases)
            {
                ForbiddenPhrase forbiddenPhrase;
                if ((forbiddenPhrase = entities.ForbiddenPhrase.Where(p =>
                    (partialComparsion ? p.Phrase.Contains(phrase) : p.Phrase.Equals(phrase))).FirstOrDefault()) != null)
                {
                    return forbiddenPhrase.Penalty;
                }                
            }
            return null;
        }
        public bool hasContactInfoPattern(params string[] phrases)
        {
            foreach (string phrase in phrases)
            {
                if (!string.IsNullOrEmpty( phrase ) )
                {
                    if(phrase.Contains("09") && phrase.Length == 11){

                    }

                }
                
            }
           // throw new NotImplementedException();
            return false;
        }
        //public void penaltyExecuter(Penalties penalty)
        //{
        //    switch ( penalty.Title)
        //    {
        //        case PenaltyTypes.PERMANENT_INACTIVE:
        //    }
        //}
    }
    
    public enum AnalyzeResult
    {

    }
    public struct PenaltyTypes{
        public const string ADMIN_CHECKOUT = "ADMIN_CHECKOUT";
        public const string TEMPORARY_INACTIVATE = "TEMPORARY_INACTIVATE";
        public const string PERMANENT_INACTIVE = "PERMANENT_INACTIVE";        

    }
}