﻿using NakedObjects.Resources;
using System.Linq;
using NakedFunctions;

namespace AdventureWorksModel
{
    public static class CommonFactoryAndRepositoryFunctions
    {
        public static (T,object,string) SingleObjectWarnIfNoMatch<T>(IQueryable<T> query)
        {
            T result = default(T);
            string message = "";
            if (!query.Any())
            {
                message = ProgrammingModel.NoMatchSingular;
                
            } else
            {
                result = query.First();
            }
            return Result.ToDisplay(result, message);
        }

        /// <summary>
        ///     Returns a random instance from the set of all instance of type T
        /// </summary>
        public static T Random<T>(IQueryable<T> query, int random) where T : class
        {
            int random2 = random % query.Count();
           
            //The OrderBy(...) doesn't do anything, but is a necessary precursor to using .Skip
            //which in turn is needed because LINQ to Entities doesn't support .ElementAt(x)
            return query.OrderBy(n => "").Skip(random).FirstOrDefault();
        }
    }
}
