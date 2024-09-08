using System.Diagnostics;
using Ganss.Excel;
using Ganss.Excel.Exceptions;
using SheetMapperApi.Models;

namespace SheetMapperApi.Services.Implementations
{
    public class SheetMapService : ISheetMapService
    {
        private const string IndexColumnKey = "__indexes__";

        /// <summary>
        /// Maps a three-sheet demo excel file to POCOs using different paradigms
        /// </summary>
        /// <param name="excelSheetFile">The XSLX file as a stream of bytes</param>
        public async Task MapSheetToObjects(Stream excelSheetFile)
        {
            try
            {
                // Just pretend we're doing something time consuming here
                ExcelMapper mapper = await Task.Run(() =>
                    new ExcelMapper(excelSheetFile)
                );

                // Map the Excel workbook using different paradigms
                MapUsingDynamicTyping(mapper);
                MapUsingColumnNames(mapper);
                MapUsingCustomBinding(mapper);
            }
            catch (ExcelMapperConvertException emcEx)
            {
                throw new InvalidOperationException($"Could not process Excel file. Error message: {emcEx.Message}", emcEx);
            }
        }


        /// <summary>
        /// Maps the first sheet using dynamic mapping. 
        /// </summary>
        private void MapUsingDynamicTyping(ExcelMapper mapper)
        {
            // The first sheet is found at index zero, instead of one. Who would've thought. That's so not Excel...
            // This produces a collection of KeyValuePairs for each row, where the column header is the Key and the cell content is the Value.
            var dynamicSheetData = mapper.Fetch(0);

            // Coding the next part feels like in Team America when their supercomputer died and they lost their Intellisense
            // Forget about compile-time convenience and type-safety. Damn I love Pytho... C#
            Debug.WriteLine("");
            foreach (var row in dynamicSheetData)
            {
                // Skip the first column, as it contains metadata
                foreach(var column in row)
                {
                    if (column.Key != IndexColumnKey)
                    {
                        Debug.Write($"{column.Key} -> {column.Value}    ");      // Pretend do do something useful here
                    }
                }
                Debug.WriteLine("");
            }

            // ToDo: Look into an interesting bug or feature in ExcelMapper:
            // If the sheet contains an erroneous formula, it appears that the sheet cannot be loaded
        }


        /// <summary>
        /// Loads the Portfolio sheet data by mapping its column names to object properties. 
        /// </summary>
        private void MapUsingColumnNames(ExcelMapper mapper)
        {
            // The PortfolioShare class has its properies named like the Excel sheet column headers, so we don't need Attributes to bind the data
            var shares = mapper.Fetch<PortfolioShare>("Portfolio");     // Fetch by sheet by name instead of by index.

            // Pretend to do something with the data
            foreach (var share in shares)
            {
                Debug.WriteLine($"We own {share.Owned} shares of {share.Name}, priced €{share.Price} each.");
            }
        }


        /// <summary>
        /// Loads the restaurant sheet using customized mapping and a nested object
        /// </summary>
        private void MapUsingCustomBinding(ExcelMapper mapper)
        {
            // The model classes (Restaurant and Score) are nested and their properties don't match the Excel header names, so we create a custom mapping.
            mapper.AddMapping<Restaurant>("Restaurant name", r => r.Name);
            mapper.AddMapping<Restaurant>("Average price per meal", r => r.AvgPrice);
            mapper.AddMapping<Score>("Good reviews", s => s.Positive);
            mapper.AddMapping<Score>("Bad reviews", s => s.Negative);

            List<Restaurant> restaurants = mapper.Fetch<Restaurant>("Restaurants").ToList();

            // Just for fun we use a different approch to ForEach and pretend to do something with the data
            restaurants.ForEach(r =>
                Debug.WriteLine($"{r.Name} has a review ratio of {Math.Round(r.Score.Ratio, 1)}. The average price is €{r.AvgPrice} per meal")
            );
        }

    }
}
