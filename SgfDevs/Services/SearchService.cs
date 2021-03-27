using Examine;
using Examine.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using SgfDevs.Extensions;
using Lucene.Net.Analysis;
using static Umbraco.Core.Constants;

namespace SgfDevs.Services
{
    public class SearchService : ISearchService
    {

        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public SearchService(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        public IEnumerable<IPublishedContent> GetPageOfContentSearchResults(string searchTerm)
        {
            var pageOfResults = GetPageOfSearchResults(searchTerm);

            var items = new List<IPublishedContent>();
            if (pageOfResults != null && pageOfResults.Any())
            {
                foreach (var item in pageOfResults)
                {
                    var page = _umbracoContextAccessor.UmbracoContext.Content.GetById(int.Parse(item.Id));
                    if (page != null)
                    {
                        items.Add(page);
                    }
                }
            }
            return items;
        }

        public IEnumerable<ISearchResult> GetPageOfSearchResults(string searchTerm)
        {
            string[] terms = !string.IsNullOrEmpty(searchTerm) && searchTerm.Contains(" ")
            ? searchTerm.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            : !string.IsNullOrWhiteSpace(searchTerm) ? new string[] { searchTerm } : null;

            IEnumerable<ISearchResult> results = new List<ISearchResult>();

            if (terms != null && terms.Any())
            {
                terms = terms.Where(x => !StopAnalyzer.ENGLISH_STOP_WORDS_SET.Contains(x.ToLower()) &&
                    x.Length > 2).ToArray();
            }

            if (ExamineManager.Instance.TryGetIndex(UmbracoIndexes.ExternalIndexName, out var contentIndex))
            {

                var contentSearcher = contentIndex.GetSearcher();
                var contentQuery = contentSearcher.CreateQuery("content").ManagedQuery(searchTerm);
                //var contentQuery = contentCriteria.GroupedNot(new string[] { "umbracoNaviHide" },
                //    new string[] { "1" });

                //if (terms != null && terms.Any())
                //{
                //    contentQuery.And(q => q
                //    .GroupedOr(new[] { "nodeName" }, terms.Boost(12))
                //    .Or()
                //    .GroupedOr(new[] { "description" }, terms.Boost(10))
                //    .Or()
                //    .GroupedOr(new[] { "aboutText" }, terms.Boost(9))
                //    .Or()
                //    .GroupedOr(new[] { "pageTitle" }, terms.Boost(8))
                //    .Or()
                //    .GroupedOr(new[] { "bodyText" }, terms.Boost(6))
                //    .Or()
                //    .GroupedOr(new[] { "seoMetaDescription" }, terms.Boost(4))
                //    .Or()
                //    .GroupedOr(new[] { "keywords" }, terms.Boost(2))
                //    .Or()
                //    .GroupedOr(new[] { "nodeName", "pageTitle", "bodyText", "seoMetaDescription",
                //        "keywords" }, terms.Fuzzy()), BooleanOperation.Or);
                //}

                results = contentQuery.Execute();

            }
            // no... just... no... but IDK more about creating my own combined index with members and content... IDK

            if (ExamineManager.Instance.TryGetIndex("MembersIndex", out var memberIndex))
            {

                var memberSearcher = memberIndex.GetSearcher();
                var memberQuery = memberSearcher.CreateQuery("content").ManagedQuery(searchTerm);
                //    //var memberQuery = criteria.GroupedNot(new string[] { "umbracoNaviHide" },
                //    //    new string[] { "1" });

                //    if (terms != null && terms.Any())
                //    {
                //        memberQuery.And(q => q
                //        .GroupedOr(new[] { "nodeName" }, terms.Boost(12))
                //        .Or()
                //        .GroupedOr(new[] { "skills" }, terms.Boost(10))
                //        .Or()
                //        .GroupedOr(new[] { "jobTitle" }, terms.Boost(9))
                //        .Or()
                //        .GroupedOr(new[] { "aboutText" }, terms.Boost(8))
                //        .Or()
                //        .GroupedOr(new[] { "email" }, terms.Boost(6))
                //        .Or()
                //        .GroupedOr(new[] { "city" }, terms.Boost(4))
                //        .Or()
                //        .GroupedOr(new[] { "nodeName", "jobTitle", "aboutText", "skills",
                //            "city" }, terms.Fuzzy()), BooleanOperation.Or);
                //    }

                results.Concat(memberQuery.Execute());

            }
            return results;

        }
    }
}