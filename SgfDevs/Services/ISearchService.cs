using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models.PublishedContent;

namespace SgfDevs.Services
{
    public interface ISearchService
    {
        IEnumerable<IPublishedContent> GetPageOfContentSearchResults(string searchTerm);

        IEnumerable<ISearchResult> GetPageOfSearchResults(string searchTerm);

    }
}