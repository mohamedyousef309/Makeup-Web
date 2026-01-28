using Application_Layer.CQRS.Attributes.Commands.addAttribute;
using Application_Layer.CQRS.Attributes.Commands.AddAttributesWithValues;
using Application_Layer.CQRS.Attributes.Quries.GetAttributes;
using Application_Layer.CQRS.Attributes.Quries.GetAttributeWithValueByid;
using Domain_Layer.DTOs.Attribute;
using Domain_Layer.ViewModels.AttributesViewModle;
using MediatR;
using Microsoft.AspNetCore.Mvc;
//using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Makeup_Web.Controllers
{
    public class AttributesController : BaseController
    {
        private readonly IMediator mediator;

        public AttributesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllAttributes(int pageNumber = 1, int pageSize = 10) 
        {
            var query = new GetAttributesQuery(pageNumber, pageSize);

            var response = await mediator.Send(query);

            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessage = response.Message;
                return View("Error"); 
            }

            return View(response.Data);

        }
        [HttpGet]
        public async Task<IActionResult> GetAttributeByid(int id) 
        {
           var GetAttributeByidResult = await mediator.Send(new GetAttributeWithValueByidQuery(id));
            if (!GetAttributeByidResult.IsSuccess)
            {
                ViewBag.ErrorMessage = GetAttributeByidResult.Message;
                return View("Error");
            }
            return View(GetAttributeByidResult.Data);
        }
        public IActionResult AddAttributesWithValues() 
        {
            return View(new AddAttributesWithValuesViewModel());
        }

        [HttpPost]

        public async Task<IActionResult> AddAttributesWithValues(AddAttributesWithValuesViewModel Modle) 
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = string.Join("<br/>", validationErrors);
                return View(Modle);
            }

            var AddAttributesWithValuesResult = await mediator.Send(
                new addAttributeCommand(Modle.AttributeName, Modle.Values));

            if (!AddAttributesWithValuesResult.IsSuccess)
            {
                TempData["ErrorMessage"] = AddAttributesWithValuesResult?.Message ?? "error while AddAttributesWithValues";
                return View(Modle);
            }

            TempData["SuccessMessage"] = AddAttributesWithValuesResult.Message ?? "Attribute added successfully";
            return RedirectToAction("GetAllAttributes"); 

        }

    }
}

