using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;

namespace {{cookiecutter.project_name}}.Helpers
{
    public class ExcelHelper
    {
        /// <summary>  
        /// 将DataTable数据导入到excel中  
        /// </summary>  
        /// <param name="data">要导入的数据</param>  
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>  
        /// <param name="sheetName">要导入的excel的sheet的名称</param>  
        /// <returns>导入数据行数(包含列名那一行)</returns>  
        public static int DataTableToExcel(DataTable data, string fileName, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            IWorkbook workbook = null;
            ISheet sheet = null;

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本  
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本  
                workbook = new HSSFWorkbook();

            if (workbook == null)
                return -1;

            sheet = workbook.CreateSheet(sheetName);

            if (isColumnWritten) //写入DataTable的列名  
            {
                IRow row = sheet.CreateRow(0);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                }
                count = 1;
            }
            else
            {
                count = 0;
            }

            for (i = 0; i < data.Rows.Count; ++i)
            {
                IRow row = sheet.CreateRow(count);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                }
                ++count;
            }
            workbook.Write(fs); //写入到excel  
            return count;
        }

        /// <summary>  
        /// 将excel中的数据导入到DataTable中  
        /// </summary>  
        /// <param name="sheetName">excel工作薄sheet的名称</param>  
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>  
        /// <returns>返回的DataTable</returns>  
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool firstRowHead = true)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本  
                workbook = new XSSFWorkbook(fs);
            else if (fileName.IndexOf(".xls") > 0) // 2003版本  
                workbook = new HSSFWorkbook(fs);

            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);
                if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet  
                {
                    sheet = workbook.GetSheetAt(0);
                }
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }
            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数  

                if (firstRowHead)
                {
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell == null) continue;

                        string cellValue = cell.StringCellValue;
                        if (cellValue == null) continue;

                        DataColumn column = new DataColumn(cellValue);
                        data.Columns.Add(column);
                    }
                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        DataColumn column = new DataColumn("F" + i);
                        data.Columns.Add(column);
                    }
                    startRow = sheet.FirstRowNum;
                }

                //最后一列的标号  
                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　  

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null  
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }
            }
            return data;
        }
    }
}
