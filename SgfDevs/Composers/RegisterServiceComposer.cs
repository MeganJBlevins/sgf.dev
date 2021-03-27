using SgfDevs.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace SgfDevs.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class RegisterSErviceComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<ISearchService, SearchService>(Lifetime.Request);
        }
    }
}