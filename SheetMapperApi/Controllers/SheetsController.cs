using Microsoft.AspNetCore.Mvc;
using SheetMapperApi.Services;
using SheetMapperApi.Utils;


namespace SheetMapperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SheetsController : ControllerBase
    {

        private const long maxExcelFileSizeBytes = 5 * 1024 * 1024;
        private readonly string[] xlsxExtensions = { ".xlsx" };

        private readonly UploadValidator excelUploadValidator;
        private readonly ISheetMapService sheetMapService;


        public SheetsController(ISheetMapService sheetMapService)
        {
            this.sheetMapService = sheetMapService;
            excelUploadValidator = new UploadValidator(xlsxExtensions, maxExcelFileSizeBytes);
        }


        /// <summary>
        /// Allows an XSLX file to be uploaded and processed
        /// </summary>
        /// <param name="file">An XSLX file. The demo file is located in the TestData directory in the solution root</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            //
            // Note that because we lack any form of authentication, we're extremely vulnerable to DOS attacks.
            // Don't try this at home. ... or at work.
            //

            try
            {
                excelUploadValidator.Validate(file);
                Stream fileStream = file.OpenReadStream();
                await sheetMapService.MapSheetToObjects(fileStream);
            }
            catch (Exception ex) when (ex is InvalidOperationException  
                                    || ex is InvalidDataException)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Excel sheet has been processed");
        }


    }
}
