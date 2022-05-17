using System.Threading.Tasks;

namespace PhramacyApp.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}