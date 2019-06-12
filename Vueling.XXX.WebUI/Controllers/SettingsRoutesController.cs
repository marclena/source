using log4net;
using System.Web.Mvc;

namespace Vueling.XXX.WebUI.Controllers
{
    public class SettingsRoutesController : Vueling.Web.UI.Library.Controllers.VuelingController
    {
        protected  Vueling.Maestros.Settings.Routes.Contracts.ServiceLibrary.IRoutesSettingsServiceContract _RoutesSettingsServiceContract;

        public SettingsRoutesController(
            Vueling.Web.UI.Library.Configuration.IWebUILibraryConfiguration _WebUILibraryConfiguration,
            Vueling.Maestros.Settings.Routes.Contracts.ServiceLibrary.IRoutesSettingsServiceContract _IRoutesSettingsServiceContract)
            : base(_WebUILibraryConfiguration)
        {
            _RoutesSettingsServiceContract = _IRoutesSettingsServiceContract;
        }


        public ActionResult Markets()
        {
            var items = _RoutesSettingsServiceContract.GetVuelingMarkets();

            ViewBag.Message = string.Format("Retrieved {0} markets.", items.Count);

            return View("Index");
        }

        public ActionResult Stations()
        {
            var items = _RoutesSettingsServiceContract.GetVuelingStations();

            ViewBag.Message = string.Format("Retrieved {0} stations.", items.Count);

            return View("Index");
        }

    }
}
