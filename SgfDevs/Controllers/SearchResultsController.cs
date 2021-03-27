using SgfDevs.Models;
using SgfDevs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace SgfDevs.Controllers
{
    public class SearchResultsController : RenderMvcController
    {
        private readonly ISearchService _searchService;

        public SearchResultsController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        // GET: SearchResults
        public ActionResult Index(ContentModel model, string query)
        {
            var searchPageModel = new SearchContentModel(model.Content);

            searchPageModel.Query = query;

            var searchResults = _searchService.GetPageOfContentSearchResults(query);

            searchPageModel.SearchResults = searchResults;

            return CurrentTemplate(searchPageModel);
        }
    }
}