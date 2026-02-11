using Application_Layer.CQRS.Attributes.Commands.addAttribute;
using Application_Layer.CQRS.Attributes.Commands.AddAttributesWithValues;
using Application_Layer.CQRS.Attributes.Commands.AddValuesToAttribute;
using Application_Layer.CQRS.Attributes.Commands.deleteAttribute;
using Application_Layer.CQRS.Attributes.Commands.DeleteAttributeValue;
using Application_Layer.CQRS.Attributes.Commands.updateAttribute;
using Application_Layer.CQRS.Attributes.Commands.UpdateAttributeValue;
using Application_Layer.CQRS.Attributes.Quries.GetAttributes;
using Application_Layer.CQRS.Attributes.Quries.GetAttributesLookup;
using Application_Layer.CQRS.Attributes.Quries.GetAttributesWithValues;
using Application_Layer.CQRS.Attributes.Quries.GetAttributeWithValueByid;
using Domain_Layer.DTOs.Attribute;
using Domain_Layer.ViewModels.AttributesViewModle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
                return View();
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributesLookup()
        {
            var result = await mediator.Send(new GetAttributesLookupQuery());
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Json(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributeByidlookUpValue(int id)
        {
            var GetAttributeByidResult = await mediator.Send(new GetAttributeWithValueByidQuery(id));
            if (!GetAttributeByidResult.IsSuccess) return BadRequest(GetAttributeByidResult.Message);
            return Json(GetAttributeByidResult.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributesWithValues()
        {
            var GetAttributeByidResult = await mediator.Send(new GetAttributesWithValuesQuery());
            if (!GetAttributeByidResult.IsSuccess) return Json(new { success = false, message = GetAttributeByidResult.Message });

            return Json(GetAttributeByidResult.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAttributeByid(int id)
        {
            var GetAttributeByidResult = await mediator.Send(new GetAttributeWithValueByidQuery(id));
            if (!GetAttributeByidResult.IsSuccess)
            {
                ViewBag.ErrorMessage = GetAttributeByidResult.Message;
                return View();
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
                 
                return View(Modle);
            }

            var AddAttributesWithValuesResult = await mediator.Send(new addAttributeCommand(Modle.AttributeName, Modle.Values));

            if (!AddAttributesWithValuesResult.IsSuccess)
            {
                TempData["ErrorMessage"] = AddAttributesWithValuesResult?.Message ?? "error while AddAttributesWithValues";
                return View(Modle);
            }

            TempData["SuccessMessage"] = AddAttributesWithValuesResult.Message ?? "Attribute added successfully";
            return RedirectToAction("GetAllAttributes");
        }



        [HttpPost]
        public async Task<IActionResult> UpdateAttribute([FromBody] UpdateAttributeViewModel model)
        {

            var result = await mediator.Send(new updateAttributeCommand(new UpdateAttributeDto { Id = model.Id, AttributeName = model.AttributeName }));
            if (result.IsSuccess) return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAttribute(int id)
        {
            var result = await mediator.Send(new deleteAttributeCommand(id));
            if (result.IsSuccess) return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public async Task<IActionResult> AddValuesToAttribute([FromBody] AddValuesToAttributeViewModel model)
        {

            var result = await mediator.Send(new AddValuesToAttributeCommand(new AddValuesToAttributeDto { AttributeId = model.AttributeId, Values = model.NewValues }));
            if (result.IsSuccess) return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAttributeValue([FromBody] UpdateAttributeValueViewModel model)
        {
            var result = await mediator.Send(new UpdateAttributeValueCommand(new UpdateAttributeValueDto { Id = model.Id, AttributeId = model.AttributeId, Value = model.Value }));
            if (result.IsSuccess) return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAttributeValue(int id)
        {
            var result = await mediator.Send(new DeleteAttributeValueCommand(id));
            if (result.IsSuccess) return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }
    }
}