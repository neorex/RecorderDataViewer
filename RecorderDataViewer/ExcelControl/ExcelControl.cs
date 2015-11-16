using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelControl
{
    public class ExcelControl
    {
        
        public ExcelControl()
        {

        }
        public async Task ExportToFile(string fileName)
        {
            DateTime start = DateTime.Now;
            using (SpreadsheetDocument xl = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                List<OpenXmlAttribute> oxa;
                OpenXmlWriter oxw;

                xl.AddWorkbookPart();
                WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                oxw = OpenXmlWriter.Create(wsp);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());

                for (int i = 1; i <= 1000000; ++i)
                {
                    oxa = new List<OpenXmlAttribute>();
                    // this is the row index
                    oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));

                    oxw.WriteStartElement(new Row(), oxa);

                    for (int j = 1; j <= 13; ++j)
                    {
                        oxa = new List<OpenXmlAttribute>();
                        // this is the data type ("t"), with CellValues.String ("str")
                        oxa.Add(new OpenXmlAttribute("t", null, "str"));

                        // it's suggested you also have the cell reference, but
                        // you'll have to calculate the correct cell reference yourself.
                        // Here's an example:
                        //oxa.Add(new OpenXmlAttribute("r", null, "A1"));

                        oxw.WriteStartElement(new Cell(), oxa);

                        oxw.WriteElement(new CellValue(string.Format("R{0}C{1}", i, j)));

                        // this is for Cell
                        oxw.WriteEndElement();
                    }

                    // this is for Row
                    oxw.WriteEndElement();
                    await Task.Delay(0);
                }

                // this is for SheetData
                oxw.WriteEndElement();
                // this is for Worksheet
                oxw.WriteEndElement();
                oxw.Close();

                oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                oxw.WriteStartElement(new Workbook());
                oxw.WriteStartElement(new Sheets());

                // you can use object initialisers like this only when the properties
                // are actual properties. SDK classes sometimes have property-like properties
                // but are actually classes. For example, the Cell class has the CellValue
                // "property" but is actually a child class internally.
                // If the properties correspond to actual XML attributes, then you're fine.
                oxw.WriteElement(new Sheet()
                {
                    Name = "Sheet1",
                    SheetId = 1,
                    Id = xl.WorkbookPart.GetIdOfPart(wsp)
                });

                // this is for Sheets
                oxw.WriteEndElement();
                // this is for Workbook
                oxw.WriteEndElement();
                oxw.Close();

                xl.Close();
            }
            Console.WriteLine("Elaped Time : {0}",DateTime.Now-start);
        }
        public void CreateSpreadsheetWorkbookRef(string filepath, DataSet ds)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "mySheet" };
            sheets.Append(sheet);

            // Get the sheetData cell table.
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Add a row to the cell table.
            Row row;
            row = new Row() { RowIndex = 1 };
            sheetData.Append(row);

            // In the new row, find the column location to insert a cell in A1.  
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, "A1", true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            // Add the cell to the cell table at A1.
            Cell newCell = new Cell() { CellReference = "A1" };
            row.InsertBefore(newCell, refCell);

            // Set the cell value to be a numeric value of 100.
            newCell.CellValue = new CellValue("100");
            newCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            // Close the document.
            spreadsheetDocument.Close();
        }
        public void CreateSpreadsheetWorkbook(string filepath, DataSet ds)
        {
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            #region data 추가부분
            
            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "mySheet" };
            sheets.Append(sheet);

            // Get the sheetData cell table.
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Add a row to the cell table.
            Row row;
            row = new Row() { RowIndex = 1 };
            sheetData.Append(row);

            // In the new row, find the column location to insert a cell in A1.  
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, "A1", true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            // Add the cell to the cell table at A1.
            Cell newCell = new Cell() { CellReference = "A1" };
            row.InsertBefore(newCell, refCell);

            // Set the cell value to be a numeric value of 100.
            newCell.CellValue = new CellValue("100");
            newCell.DataType = new EnumValue<CellValues>(CellValues.Number); 

            #endregion

            // Close the document.
            spreadsheetDocument.Close();
        }
        public void exportDocument(string FilePath, DataSet dataSet)
        {
            WorkbookPart wBookPart = null;
            using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(FilePath, SpreadsheetDocumentType.Workbook))
            {
                wBookPart = spreadsheetDoc.AddWorkbookPart();
                wBookPart.Workbook = new Workbook();
                uint sheetId = 1;
                spreadsheetDoc.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = spreadsheetDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                foreach (DataTable table in dataSet.Tables)
                {
                    WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "mySheet" + sheetId };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    wSheetPart.Worksheet = new Worksheet(sheetData);

                    Row headerRow = new Row();
                    foreach (DataColumn column in table.Columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dr in table.Rows)
                    {
                        Row row = new Row();
                        foreach (DataColumn column in table.Columns)
                        {
                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(dr[column].ToString());
                            row.AppendChild(cell);
                        }

                        sheetData.AppendChild(row);

                    }

                    sheetId++;
                }
            }
        }
        public async Task exportDocumentXMLwriter(string FilePath, DataSet dataSet)
        {
            WorkbookPart wBookPart = null;
            using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(FilePath, SpreadsheetDocumentType.Workbook))
            {
                wBookPart = spreadsheetDoc.AddWorkbookPart();
                wBookPart.Workbook = new Workbook();
                uint sheetId = 1;
                spreadsheetDoc.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = spreadsheetDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                foreach (DataTable table in dataSet.Tables)
                {
                    WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = "mySheet" + sheetId };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    wSheetPart.Worksheet = new Worksheet(sheetData);

                    Row headerRow = new Row();
                    foreach (DataColumn column in table.Columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dr in table.Rows)
                    {
                        OpenXmlWriter writer = OpenXmlWriter.Create(wSheetPart);
                        writer.WriteStartElement(new Worksheet());
                        writer.WriteStartElement(new SheetData());

                        SetRow(ref writer, table);

                        writer.WriteEndElement(); //end of SheetData
                        writer.WriteEndElement(); //end of worksheet
                        writer.Close();
                        await Task.Delay(0);
                    }
                    Console.WriteLine("SheetID: {0}",sheetId);
                    sheetId++;

                }
            }
            Console.WriteLine("Export Done");
        }
        public void exportDocumentXMLwriter(string FilePath, DataTable dataTable)
        {
            WorkbookPart wBookPart = null;
            using (SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(FilePath, SpreadsheetDocumentType.Workbook))
            {
                wBookPart = spreadsheetDoc.AddWorkbookPart();
                wBookPart.Workbook = new Workbook();
                uint sheetId = 1;
                spreadsheetDoc.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = spreadsheetDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>();


                    WorksheetPart wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = spreadsheetDoc.WorkbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = dataTable.TableName };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    wSheetPart.Worksheet = new Worksheet(sheetData);

                    Row headerRow = new Row();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        OpenXmlWriter writer = OpenXmlWriter.Create(wSheetPart);
                        writer.WriteStartElement(new Worksheet());
                        writer.WriteStartElement(new SheetData());

                        SetRow(ref writer, dataTable);

                        writer.WriteEndElement(); //end of SheetData
                        writer.WriteEndElement(); //end of worksheet
                        writer.Close();

                    }
            }
        }
            
        public void AddWorkbook(string sOutputPath, DataSet dataSet)
        {
            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(sOutputPath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                //create workbook part
                WorkbookPart wbp = spreadsheet.AddWorkbookPart();
                wbp.Workbook = new Workbook();
                Sheets sheets = wbp.Workbook.AppendChild<Sheets>(new Sheets());

                //create worksheet part, and add it to the sheets collection in workbook
                WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                int tableCount = dataSet.Tables.Count;
                UInt32Value sheetID = 1;
                for (int tableIndex = 0; tableIndex < tableCount; tableIndex++)
                {
                    SetSheet(spreadsheet, ref sheets, ref wsp, dataSet.Tables[tableIndex], sheetID++);
                }

                //Sheet sheet = new Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(wsp), SheetId = 1, Name = "Test" };
                //sheets.Append(sheet);

                //OpenXmlWriter writer = OpenXmlWriter.Create(wsp);
                //writer.WriteStartElement(new Worksheet());
                //writer.WriteStartElement(new SheetData());

                //SetRow(ref writer);

                //writer.WriteEndElement(); //end of SheetData
                //writer.WriteEndElement(); //end of worksheet
                //writer.Close();
            }
        }
        private void SetSheet(SpreadsheetDocument spreadsheet,ref Sheets sheets, ref WorksheetPart workSheetPart, System.Data.DataTable table, UInt32Value sheetID)
        {
            Sheet sheet = new Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(workSheetPart), SheetId = sheetID, Name = table.TableName };
            sheets.Append(sheet);

            OpenXmlWriter writer = OpenXmlWriter.Create(workSheetPart);
            writer.WriteStartElement(new Worksheet());
            writer.WriteStartElement(new SheetData());

            SetRow(ref writer, table);

            writer.WriteEndElement(); //end of SheetData
            writer.WriteEndElement(); //end of worksheet
            writer.Close();
        }
        private void SetRow(ref OpenXmlWriter writer, System.Data.DataTable table)
        {
            int rowCount = table.Rows.Count;
            int columnCount = table.Columns.Count;
            for (int row = 0; row < rowCount; row++)
            {
                writer.WriteStartElement(new Row());
                for (int column = 0; column < columnCount; column++)
                {
                    writer.WriteElement(new Cell { CellValue = new CellValue(table.Rows[row][column].ToString()), DataType = CellValues.String });
                }
                writer.WriteEndElement(); //end of Row
            }
        }
        public async Task ExportToFile(string fileName, DataSet ds)
        {
            DateTime start = DateTime.Now;
            using (SpreadsheetDocument xl = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                try
                {
                    List<OpenXmlAttribute> oxa;
                    OpenXmlWriter oxw;

                    xl.AddWorkbookPart();
                    WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                    List<OpenXmlWriter> listOXW = new List<OpenXmlWriter>();

                    oxw = OpenXmlWriter.Create(wsp);
                    oxw.WriteStartElement(new Worksheet());
                    oxw.WriteStartElement(new SheetData());

                    #region sheet 데이터 추가 부분
                    DataTable dt = ds.Tables[0];
                    int rowCount = dt.Rows.Count;
                    int columnCount = dt.Columns.Count;

                    //dt.Rows[0].cel
                    try
                    {
                        for (int i = 0; i < rowCount; ++i)
                        {
                            oxa = new List<OpenXmlAttribute>();
                            // this is the row index
                            oxa.Add(new OpenXmlAttribute("r", null, (i + 1).ToString()));

                            oxw.WriteStartElement(new Row(), oxa);
                            object[] row = dt.Rows[i].ItemArray;
                            for (int j = 0; j < columnCount; ++j)
                            {
                                oxa = new List<OpenXmlAttribute>();
                                // this is the data type ("t"), with CellValues.String ("str")
                                oxa.Add(new OpenXmlAttribute("t", null, "str"));

                                // it's suggested you also have the cell reference, but
                                // you'll have to calculate the correct cell reference yourself.
                                // Here's an example:
                                //oxa.Add(new OpenXmlAttribute("r", null, "A1"));

                                oxw.WriteStartElement(new Cell(), oxa);

                                oxw.WriteElement(new CellValue(row[j].ToString()));

                                // this is for Cell
                                oxw.WriteEndElement();
                            }

                            // this is for Row
                            oxw.WriteEndElement();
                            //await Task.Delay(0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                    #endregion

                    // this is for SheetData
                    oxw.WriteEndElement();
                    // this is for Worksheet
                    oxw.WriteEndElement();
                    listOXW.Add(oxw);
                    oxw.Close();

                    oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                    oxw.WriteStartElement(new Workbook());
                    oxw.WriteStartElement(new Sheets());

                    // you can use object initialisers like this only when the properties
                    // are actual properties. SDK classes sometimes have property-like properties
                    // but are actually classes. For example, the Cell class has the CellValue
                    // "property" but is actually a child class internally.
                    // If the properties correspond to actual XML attributes, then you're fine.
                    try
                    {
                        oxw.WriteElement(new Sheet()
                        {
                            Name = "Sheet1",
                            SheetId = 1,
                            Id = xl.WorkbookPart.GetIdOfPart(wsp)
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                    // this is for Sheets
                    oxw.WriteEndElement();
                    // this is for Workbook
                    oxw.WriteEndElement();
                    oxw.Close();

                    xl.Close();
                    await Task.Delay(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            Console.WriteLine("Elaped Time : {0}", DateTime.Now - start);
        }


        //new
        public void ExportDSToExcel(DataSet ds, string destination)
        {
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 1;

                foreach (DataTable table in ds.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                }
            }
        }
    }
}
