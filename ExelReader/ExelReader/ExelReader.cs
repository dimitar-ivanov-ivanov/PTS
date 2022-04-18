using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExelReader
{
    public class ExelReader
    {
        private SharedStringTable table;
        private List<SheetData> Sheets { get; set; }
        public List<string> UpdatedWikis { get; private set; }
        public List<string> UploadedFiles { get; private set; }
        public Dictionary<int, double> Scores { get; private set; }
        public Dictionary<int, int> UpdatedWikisPerId { get; set; }
        public Dictionary<int, int> UploadedFilesPerId { get; set; }

        public ExelReader()
        {
            table = new SharedStringTable();
            Sheets = new List<SheetData>();
            UpdatedWikis = new List<string>();
            UploadedFiles = new List<string>();
            Scores = new Dictionary<int, double>();
            UpdatedWikisPerId = new Dictionary<int, int>();
            UploadedFilesPerId = new Dictionary<int, int>();
        }

        private void ExtractSheetsData(string path)
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                IEnumerable<WorksheetPart> worksheetParts = workbookPart.WorksheetParts;
                foreach (WorksheetPart worksheetPart in worksheetParts)
                {
                    if (worksheetPart.Worksheet.Elements<SheetData>().First() != null)
                    {
                        Sheets.Add(worksheetPart.Worksheet.Elements<SheetData>().First());
                    }
                }
                table = workbookPart.SharedStringTablePart.SharedStringTable;

            }
        }

        public void ReadLogs(string path)
        {
            Sheets=new List<SheetData>();
            UpdatedWikis = new List<string>();
            UploadedFiles = new List<string>();
            ExtractSheetsData(path);
            foreach (SheetData sheetData in Sheets)
            {
                ExtractLogsSheetData(sheetData);
            }
        }

        public void ReadScores(string path)
        {
            Sheets = new List<SheetData>();
            Scores = new Dictionary<int, double>();
            ExtractSheetsData(path);
            foreach (SheetData sheetData in Sheets)
            {
                ExtractScoresSheetData(sheetData);
            }
        }

        private void ExtractScoresSheetData(SheetData sheetData)
        {
            for (int i = 1; i < sheetData.Elements<Row>().Count(); i++)
            {
                Row currentRow = sheetData.Elements<Row>().ElementAt(i);
                if (currentRow.Elements<Cell>().Count() == 2)
                {
                    Cell id = currentRow.Elements<Cell>().ElementAt(0);
                    Cell score = currentRow.Elements<Cell>().ElementAt(1);
                    if (AreScoresValid(id) && AreScoresValid(score))
                    {
                        int idValue;
                        if (Int32.TryParse(id.InnerText, out idValue))
                        {
                            double scoreValue;
                            if (Double.TryParse(score.InnerText, out scoreValue))
                            {
                                Scores.Add(idValue, scoreValue);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid user id.");
                        }
                    }
                }
            }
        }

        private bool AreScoresValid(Cell currentCell)
        {
            if (currentCell.DataType != null)
            {
                if (currentCell.DataType == CellValues.SharedString)
                {
                    return false;
                }
            }
            return true;
        }

        private void ExtractLogsSheetData(SheetData sheetData)
        {
            foreach (Row currentRow in sheetData.Elements<Row>())
            {
                if (!currentRow.HasChildren)
                {
                    continue;
                }
                Cell currentCell = currentRow.Elements<Cell>().ElementAt(3);
                if (currentCell.DataType != null)
                {
                    if (currentCell.DataType == CellValues.SharedString)
                    {
                        string temp = ReadString(currentCell);
                        if (temp == "Wiki page updated")
                        {
                            Cell updatedWiki = currentRow.Elements<Cell>().ElementAt(4);
                            string content = ReadString(updatedWiki);
                            UpdatedWikis.Add(content);
                        }
                        if (temp == "A file has been uploaded.")
                        {
                            Cell uploadedFile = currentRow.Elements<Cell>().ElementAt(4);
                            string content = ReadString(uploadedFile);
                            UploadedFiles.Add(content);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Excel logs.");
                    }
                }
            }
        }
        private string ReadString(Cell currentCell)
        {
            string currentcellvalue;
            int id;
            if (Int32.TryParse(currentCell.InnerText, out id))
            {
                SharedStringItem item = table.Elements<SharedStringItem>().ElementAt(id);
                if (item.Text != null)
                {
                    currentcellvalue = item.Text.Text;
                    return currentcellvalue;
                }
                else if (item.InnerText != null)
                {
                    currentcellvalue = item.InnerText;
                    return currentcellvalue;
                }
            }
            return String.Empty;
        }
        public void GetUserId(List<string> itemToRead, List<int> userIds)
        {
            int resultId;
            foreach (string line in itemToRead)
            {
                int pFrom = line.IndexOf('\'') + 1;
                string result = line.Substring(pFrom, line.Length - pFrom);
                int pTo = result.IndexOf('\'');
                result = result.Substring(0, pTo);
                if (Int32.TryParse(result, out resultId))
                {
                    if (resultId >= 0)
                    {
                        userIds.Add(resultId);
                    }
                }
            }
        }
        public Dictionary<int, int> GetUserIdCount(List<string> itemToRead)
        {
            List<int> userIds = new List<int>();
            GetUserId(itemToRead, userIds);
            return userIds
                .GroupBy(i => i)
                .Select(x => new KeyValuePair<int,int>(x.Key, x.Count()))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void FillMissingUserIds()
        {
            foreach (int userId in UploadedFilesPerId.Keys)
            {
                if (!UpdatedWikisPerId.ContainsKey(userId))
                {
                    UpdatedWikisPerId.Add(userId, 0);
                }
            }

            foreach (int userId in UpdatedWikisPerId.Keys)
            {
                if (!UploadedFilesPerId.ContainsKey(userId))
                {
                    UploadedFilesPerId.Add(userId, 0);
                }
            }
        }
    }
}
