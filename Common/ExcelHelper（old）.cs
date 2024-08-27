using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    public class ExcelHelper
    {
        //private static IWorkbook workbook = null;
        //private static ISheet sheet = null;

        /// <summary>
        /// 读取Excel模板文件，并返回文件头信息的行数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static int ReadExcelToDataTable(string path, ref DataTable dataTable)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            int index = 0;
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    // 实例化
                    if (path.IndexOf(".xlsx") > 0) // 2007
                        workbook = new XSSFWorkbook(fs);
                    else if (path.IndexOf(".xls") > 0) // 2003
                        workbook = new HSSFWorkbook(fs);
                    // 查询
                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                if (sheet != null)
                {
                    List<object> data = new List<object>();
                    List<ICell> cells;
                    while (true)
                    {
                        if (sheet.GetRow(index) == null) break;
                        data.Clear();
                        cells = sheet.GetRow(index).Cells;
                        for (int i = 0; i < cells.Count; i++)
                        {
                            switch (cells[i].CellType)
                            {
                                case CellType.Numeric:
                                    data.Add(cells[i].NumericCellValue);
                                    break;
                                case CellType.String:
                                    data.Add(cells[i].StringCellValue);
                                    break;
                                case CellType.Boolean:
                                    data.Add(cells[i].BooleanCellValue);
                                    break;
                                case CellType.Error:
                                    data.Add(cells[i].ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    {
                                        //cells[i].SetCellFormula(cells[i].CellFormula);
                                        //data.Add(cells[i].NumericCellValue.ToString());
                                        data.Add(cells[i].CellFormula);
                                    }
                                    break;
                            }
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            for (int i = 0; i < data.Count; i++) //添加列及列头
                            {
                                dataTable.Columns.Add(data[i].ToString());
                            }
                        }
                        else
                        {
                            dataTable.Rows.Add(data.ToArray());
                        }
                        index++;
                    }
                }
            }
            else
                MessageBox.Show(path + "文件不存在");

            return index - 1;
        }
        public static void WriteDataTableToExcel(DataTable dataTable, string path)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            // 实例化
            if (path.IndexOf(".xlsx") > 0) // 2007
                workbook = new XSSFWorkbook();
            else if (path.IndexOf(".xls") > 0) // 2003
                workbook = new HSSFWorkbook();
            // 创建工作表
            if (workbook != null)
                sheet = workbook.CreateSheet("Sheet1");
            else
                MessageBox.Show("请指定文件格式为.xlsx或.xls");
            /////////////////////
            try
            {
                if (sheet != null)
                {
                    double tValue;
                    object[] ItemArray;
                    DataRow[] dataRows = dataTable.Select();
                    for (int i = 0; i < dataRows.Length; i++)
                    {
                        row = sheet.CreateRow(i); // 这个参数是行号还是行的索引？
                        ItemArray = dataRows[i].ItemArray;
                        for (int j = 0; j < ItemArray.Length; j++)
                        {
                            if (ItemArray[j] != null)
                            {
                                switch (ItemArray[j].GetType().Name)
                                {
                                    case "Int64":
                                    case "long":
                                    case "float":
                                    case "Int32":
                                    case "Double":
                                        cell = row.CreateCell(j, CellType.Numeric);
                                        cell.SetCellValue((double)ItemArray[j]);
                                        break;
                                    case "String":
                                        if (double.TryParse(ItemArray[j].ToString(), out tValue))
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric);
                                            cell.SetCellValue(tValue);
                                        }
                                        else
                                        {
                                            cell = row.CreateCell(j, CellType.String);
                                            cell.SetCellValue((string)ItemArray[j]);
                                        }
                                        //cell = row.CreateCell(j, CellType.String);
                                        //cell.SetCellValue((string)ItemArray[j]);
                                        break;
                                }
                            }
                        }
                    }
                }
                if (File.Exists(path))
                {
                    if (path != null && path.Length > 0)
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write)) // 追加模式在这时为什么不起作用？
                        {
                            workbook.Write(fs);
                            fs.Close();
                            workbook.Close();
                        }
                    }
                }
                else
                {
                    if (path != null && path.Length > 0)
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            workbook.Write(fs);
                            fs.Close();
                            workbook.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("保存数据出错", ex);
            }
        }
        public static int ReadExcelToDataGridView(string path, ref DataGridView dataGridView)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            int index = 0;
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    // 实例化
                    if (path.IndexOf(".xlsx") > 0) // 2007
                        workbook = new XSSFWorkbook(fs);
                    else if (path.IndexOf(".xls") > 0) // 2003
                        workbook = new HSSFWorkbook(fs);
                    // 查询
                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                if (sheet != null)
                {
                    List<object> data = new List<object>();
                    List<ICell> cells;
                    while (true)
                    {
                        if (sheet.GetRow(index) == null) break;
                        data.Clear();
                        cells = sheet.GetRow(index).Cells;
                        for (int i = 0; i < cells.Count; i++)
                        {
                            switch (cells[i].CellType)
                            {
                                case CellType.Numeric:
                                    data.Add(cells[i].NumericCellValue); //.ToString()
                                    break;
                                case CellType.String:
                                    data.Add(cells[i].StringCellValue);
                                    break;
                                case CellType.Boolean:
                                    data.Add(cells[i].BooleanCellValue);
                                    break;
                                case CellType.Error:
                                    data.Add(cells[i].ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    {
                                        //cells[i].SetCellFormula(cells[i].CellFormula);
                                        //data.Add(cells[i].NumericCellValue.ToString());
                                        data.Add(cells[i].CellFormula);
                                    }
                                    break;
                            }
                        }
                        if (dataGridView.Columns.Count == 0)
                        {
                            for (int i = 0; i < data.Count; i++)
                            {
                                dataGridView.Columns.Add(data[i].ToString(), data[i].ToString()); //添加列及列头
                            }
                        }
                        else
                        {
                            int rowIndex = dataGridView.Rows.Add();
                            DataGridViewCellCollection ItemArray = dataGridView.Rows[rowIndex].Cells;
                            for (int i = 0; i < data.Count; i++)
                            {
                                ItemArray[i].Value = data[i];
                            }
                        }
                        index++;
                    }
                }
            }
            else
                MessageBox.Show(path + "文件不存在");

            return index - 1;
        }
        public static void WriteDataGridViewToExcel(DataGridView dataGridView, string path)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            // 实例化
            if (path.IndexOf(".xlsx") > 0) // 2007
                workbook = new XSSFWorkbook();
            else if (path.IndexOf(".xls") > 0) // 2003
                workbook = new HSSFWorkbook();
            // 创建工作表
            if (workbook != null)
                sheet = workbook.CreateSheet("Sheet1");
            else
                MessageBox.Show("请指定文件格式为.xlsx或.xls");
            /////////////////////
            try
            {
                if (sheet != null)
                {
                    DataGridViewCellCollection ItemArray;
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        row = sheet.CreateRow(i); // 这个参数是行号还是行的索引？
                        ItemArray = dataGridView.Rows[i].Cells;
                        for (int j = 0; j < ItemArray.Count; j++)
                        {
                            if (ItemArray[j].Value != null)
                            {
                                switch (ItemArray[j].Value.GetType().Name)
                                {
                                    case "Int64":
                                        if (ItemArray[j].Style.ForeColor == Color.Red)
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric); // 创建单元格时要指定单元类型，不然类型默认为：Numeric
                                            cell.SetCellValue((Int64)ItemArray[j].Value);
                                            cell.CellStyle.FillForegroundColor = 0; // 表示颜色的是哪个索引？
                                        }
                                        else
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric); // 创建单元格时要指定单元类型，不然类型默认为：Numeric
                                            cell.SetCellValue((Int64)ItemArray[j].Value);
                                        }
                                        break;
                                    case "Int32":
                                        if (ItemArray[j].Style.ForeColor == Color.Red)
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric); // 创建单元格时要指定单元类型，不然类型默认为：Numeric
                                            cell.SetCellValue((int)ItemArray[j].Value);
                                            cell.CellStyle.FillForegroundColor = 0; // 表示颜色的是哪个索引？
                                        }
                                        else
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric); // 创建单元格时要指定单元类型，不然类型默认为：Numeric
                                            cell.SetCellValue((int)ItemArray[j].Value);
                                        }
                                        break;
                                    case "float":
                                        if (ItemArray[j].Style.ForeColor == Color.Red)
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric); // 创建单元格时要指定单元类型，不然类型默认为：Numeric
                                            cell.SetCellValue((float)ItemArray[j].Value);
                                            cell.CellStyle.FillForegroundColor = 0; // 表示颜色的是哪个索引？
                                        }
                                        else
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric); // 创建单元格时要指定单元类型，不然类型默认为：Numeric
                                            cell.SetCellValue((float)ItemArray[j].Value);
                                        }
                                        break;
                                    case "Double":
                                        if (ItemArray[j].Style.ForeColor == Color.Red)
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric);
                                            cell.SetCellValue((double)ItemArray[j].Value);
                                            cell.CellStyle.FillForegroundColor = 0; // 表示颜色的是哪个索引？
                                        }
                                        else
                                        {
                                            cell = row.CreateCell(j, CellType.Numeric);
                                            cell.SetCellValue((double)ItemArray[j].Value);
                                        }
                                        break;
                                    case "Object":
                                    case "String":
                                        if (((string)ItemArray[j].Value).IndexOfAny(new char[] { '(', '-' }) > 0) // 如果字条串中包含'('，则表示该字符串为公式
                                        {
                                            cell = row.CreateCell(j, CellType.Formula);
                                            cell.SetCellFormula((string)ItemArray[j].Value);
                                        }
                                        else
                                        {
                                            cell = row.CreateCell(j, CellType.String);
                                            cell.SetCellValue((string)ItemArray[j].Value);
                                        }
                                        break;
                                }
                            }
                            cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        }
                    }
                }
                if (File.Exists(path))
                {
                    if (path != null && path.Length > 0)
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write)) // 追加模式在这时为什么不起作用？
                        {
                            workbook.Write(fs);
                            fs.Close();
                            workbook.Close();
                        }
                    }
                }
                else
                {
                    if (path != null && path.Length > 0)
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            workbook.Write(fs);
                            fs.Close();
                            workbook.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }



    }
}
