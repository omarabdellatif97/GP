
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GP_API.Utils
{
    public class CaseFileUrlMapper : ICaseFileUrlMapper
    {

        //public string UrlTemplatePattern { get => @$"{UrlTemplatePattern}/[0-9]+"; }
        public string RoutePattern { get => $@"{actionRouteString}/[0-9]+"; }
        //public string TemplatePattern { get => $@"{templateString}/[0-9]+"; }

        public string DownloadActionUrl
        {
            set
            {
                actionRouteString = value;
                actionRouteStringRegex = new Regex(RoutePattern);
            }
            get => actionRouteString;
        }

        public string TemplateString
        {
            set
            {
                templateString = value;
                //templateRegex = new Regex(TemplatePattern);
            }
            get => templateString;
        }


        protected Regex actionRouteStringRegex;
        //protected Regex templateRegex;
        protected string actionRouteString;
        protected string templateString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_actionRouteString">_actionRouteString is the url of the action like https://localhost:44371/api/file/download </param>
        /// <param name="_templateString"> template string is the string that is used in database</param>
        public CaseFileUrlMapper(string _actionRouteString)
        {

            //ValidateStringParameter(_templateString);
            ValidateStringParameter(_actionRouteString);
            this.DownloadActionUrl = _actionRouteString;
            //this.TemplateString = _templateString;
            this.TemplateString = "FileURL-d61b8182e027-FileURL";
        }

        //public CaseFileUrlMapper()
        //{
        //    this.ActionRouteString = "https://localhost:44371/api/file/download";
        //    this.TemplateString = @"FileURL-d61b8182e027-FileURL";
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description">the description of the case file you want to extact id after actionRoute</param>
        /// <returns></returns>
        public List<int> ExtractIds(string description)
        {
            ValidateStringParameter(description);
            var result = ExtractFullUrls(description);
            List<int> ids = result?.Select(item =>
            {
                string idString = item.ToString().Replace(@$"{actionRouteString}/", "");
                int id = Convert.ToInt32(idString);
                return id;
            }).ToList() ?? new List<int>();


            return ids;
        }

        public List<string> ExtractFullUrls(string description)
        {

            ValidateStringParameter(description);
            return actionRouteStringRegex.Matches(description).Select(u => u.Value).ToList();
        }

        public string GenerateTemplate(string description)
        {

            ValidateStringParameter(description);
            return description.Replace(actionRouteString, templateString);
        }

        public string GenerateDescription(string template)
        {

            ValidateStringParameter(template);
            return template.Replace(TemplateString, actionRouteString);
        }


        private void ValidateStringParameter(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentException("not valid string parameter to CaseFileUrlMapper Methods");

        }

    }
}
