using Frontend.Core;
using Frontend.Dto;
using Frontend.Entities;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace V2T.Frontend.Controllers
{
    public class V2TController : Controller
    {
        private readonly IV2TServices _services;
        private readonly IOptions<LanguajeDictionary> _languajeDictionary;

        public V2TController(IV2TServices services, IOptions<LanguajeDictionary> languajeDictionary)
        {
            _services = services;
            _languajeDictionary = languajeDictionary;
        }

        public ActionResult Index(AudioUploadViewModel model)
        {
            ViewBag.LanguajeList = _languajeDictionary.Value.Dictionary;
            try
            {
                if (ModelState.IsValid)
                {
                    return View(model);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAudio(AudioUploadViewModel model)
        {
            ViewBag.LanguajeList = _languajeDictionary.Value.Dictionary;
            try
            {
                if (ModelState.IsValid)
                {
                    var requestIdentifier = Guid.NewGuid();
                    await _services.SaveRequest(model, (int)StateType.Requested, requestIdentifier);
                    model.AudioText = await _services.ConvertAudio2TextAsyncGet(model);
                    await _services.SaveRequest(model, (int)StateType.Success, requestIdentifier);
                    return View(nameof(Index), model);
                }
                else
                {
                    return View(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return View(nameof(Index), model);
            }
        }
    }
}
