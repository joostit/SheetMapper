namespace SheetMapperApi.Services
{
    public interface ISheetMapService
    {

        /// <summary>
        /// Maps a three-sheet demo excel file to POCOs using different paradigms
        /// </summary>
        /// <param name="excelSheetFile">The XSLX file as a stream of bytes</param>
        Task MapSheetToObjects(Stream excelSheetFile);

    }
}
