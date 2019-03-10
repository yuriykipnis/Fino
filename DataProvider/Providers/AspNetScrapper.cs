using System;

namespace DataProvider.Providers
{
    public abstract class AspNetScrapper : HttpScrapper
    {
        protected const string ViewState = "__VIEWSTATE";
        protected const string ViewStateGenerator = "__VIEWSTATEGENERATOR";
        protected const string EventValidation = "__EVENTVALIDATION";

        protected static string ExtractAspEntity(string data, string entity)
        {
            const string valueDelimiter = "value=\"";
            const string hiddenDelimiter = "|";
            try
            {
                int viewStateStartPosition, viewStateEndPosition;
                int viewStateNamePosition = data.IndexOf(entity, StringComparison.Ordinal);
                if (viewStateNamePosition == -1)
                {
                    return string.Empty;
                }

                var viewStateValuePosition = data.IndexOf(valueDelimiter, viewStateNamePosition, StringComparison.Ordinal);
                if (viewStateValuePosition == -1)
                {
                    viewStateValuePosition = data.IndexOf(hiddenDelimiter, viewStateNamePosition, StringComparison.Ordinal);
                    viewStateStartPosition = viewStateValuePosition + hiddenDelimiter.Length;
                    viewStateEndPosition = data.IndexOf(hiddenDelimiter, viewStateStartPosition, StringComparison.Ordinal);
                    return data.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition);
                }

                viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
                viewStateEndPosition = data.IndexOf("\"", viewStateStartPosition, StringComparison.Ordinal);
                return data.Substring(viewStateStartPosition, viewStateEndPosition - viewStateStartPosition);
            }
            catch (Exception exp)
            {
               throw new AggregateException($"Error while trying to extract asp entity: {entity} in {data}", exp);
            }
        }
       
    }
}
