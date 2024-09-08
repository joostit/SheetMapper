namespace SheetMapperApi.Utils
{
    /// <summary>
    /// Helper class for easy validation of uploaded files.
    /// </summary>
    public sealed class UploadValidator
    {

        private IEnumerable<string>? validExtensions;
        private long? maxFileSizeBytes = null;


        /// <summary>
        /// Instantiates a new UploadValidator with the specified parameters
        /// </summary>
        /// <param name="validExtensions">An optional list of valid extensions (Case insensitive)</param>
        /// <param name="maxFileSizeBytes">An optional maximum file size in bytes</param>
        public UploadValidator(IEnumerable<string>? validExtensions = null, long? maxFileSizeBytes = null)
        {
            this.validExtensions = validExtensions;
            this.maxFileSizeBytes = maxFileSizeBytes;
        }


        /// <summary>
        /// Validates the file upload. Throws an InvalidDataExceptions when the file is invalid
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException">Thrown when the file upload is not valid</exception>
        public void Validate(IFormFile fileUpload)
        {

            if(validExtensions != null)
            {
                ValidateFileExtension(fileUpload, validExtensions);
            }

            if(maxFileSizeBytes != null)
            {
                ValidateFileMaxFileSize(fileUpload, maxFileSizeBytes.Value);
            }
        }


        private void ValidateFileMaxFileSize(IFormFile fileUpload, long maxSizeBytes)
        {
            long size = fileUpload.Length;

            if(size > maxSizeBytes)
            {
                throw new InvalidDataException($"Maximum file size ({maxSizeBytes} bytes) exceeded. Uploaded file size: {fileUpload.Length} bytes");
            }
        }


        private void ValidateFileExtension(IFormFile fileUpload, IEnumerable<string> extensions)
        {
            string fileExtension = Path.GetExtension(fileUpload.FileName);

            if (!extensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase))
            {
                throw new InvalidDataException($"Unsuppported file extension: '{fileExtension}'");
            }
        }


    }
}
