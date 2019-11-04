using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using InsightAPISample.WebApp.Models;
using System.Text;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using Microsoft.Extensions.Configuration;

namespace InsightAPISample.WebApp.Repositories
{
    public class MailChimpRepository
    {
        private const string ApiKey = "12e7281008569e61151a65f310e06476-us5";
                private MailChimpManager _mailChimpManager = new MailChimpManager(ApiKey);

        public async Task<string> AddSubscribers(string list, IList<Models.MailChimpMemberModel> members)
        {
            var mailChimpListCollection = await _mailChimpManager.Lists.GetAllAsync().ConfigureAwait(false);
            var listId = mailChimpListCollection.Where(x => x.Name == list).FirstOrDefault().Id;

            int successCount = 0;
            int errorCount = 0;

            foreach (var item in members)
            {
                var _member = new Member();
                _member.EmailAddress = item.EMailAddress;
                _member.StatusIfNew = Status.Subscribed;
                _member.MergeFields.Add("FNAME", item.FirstName);
                _member.MergeFields.Add("LNAME", item.LastName);

                try
                {
                    var memberresult = await _mailChimpManager.Members.AddOrUpdateAsync(listId, _member);
                    successCount++;
                }
                catch (Exception)
                {
                    errorCount++;
                }
            }

            return $"Adding Subscribers to MailChimp. Total Records: {members.Count}; Successfully added: {successCount}; Errors: {errorCount}";

        }
    }
}
