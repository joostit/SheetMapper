using Microsoft.AspNetCore.Http;
using SheetMapperApi.Utils;

namespace SheetMapperApiTests
{
    public class UploadValidatorTests
    {
        

        [Test]
        public void ValidFilePassesValidation()
        {
            IFormFile validFile = CreateUploadedFile(2000, "test.xlsx");
            UploadValidator validator = Create5kXlsxValidator();

            TestDelegate act = () => validator.Validate(validFile);

            Assert.DoesNotThrow(act);
        }


        [Test]
        public void TooLargeFileThrowsException()
        {
            IFormFile largeFile = CreateUploadedFile(10 * 1024, "test.xlsx");
            UploadValidator validator = Create5kXlsxValidator();

            TestDelegate act = () => validator.Validate(largeFile);

            Assert.Throws<InvalidDataException>(act);
        }


        [Test]
        public void WrongExtensionThrowsException()
        {
            IFormFile wrongFile = CreateUploadedFile(2000, "document.docx");
            UploadValidator validator = Create5kXlsxValidator();

            TestDelegate act = () => validator.Validate(wrongFile);

            Assert.Throws<InvalidDataException>(act);
        }


        [Test]
        public void TooLargeWrongExtensionFileThrowsException()
        {
            IFormFile badFile = CreateUploadedFile(10 * 1024, "document.docx");
            UploadValidator validator = Create5kXlsxValidator();

            TestDelegate act = () => validator.Validate(badFile);

            Assert.Throws<InvalidDataException>(act);
        }


        private UploadValidator Create5kXlsxValidator()
        {
            return new UploadValidator([".xlsx"], 5 * 1024);
        }


        private IFormFile CreateUploadedFile(int size, string name)
        {
            byte[] fileData = new byte[size];
            MemoryStream stream = new MemoryStream();
            stream.Write(fileData, 0, fileData.Length);
            IFormFile uploadFile = new FormFile(stream, 0, stream.Length, name, name);
            return uploadFile;
        }

    }
}