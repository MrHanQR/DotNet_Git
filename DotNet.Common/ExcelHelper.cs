using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace DotNet.Common
{
    /// <summary>
    /// NPOI实现Excel导入导出
    /// </summary>
    public class ExcelHelper
    {
        static HSSFWorkbook hssfworkbook;

        /// <summary>
        /// 导出到Excel文件
        /// 返回一个可供web下载的context流
        /// </summary>
        /// <param name="context">HttpContext上下文</param>
        /// <param name="excelName">导出成的Excel的文件名-如："人口信息表"</param>
        /// <param name="dt">数据源-DataTable</param>
        /// <param name="tableHead">表头每一列名称的集合</param>
        /// <param name="is03">是否是Office2003格式,True为03,False位07,03位xls,07以后为xlsx</param>
        /// <param name="sheetName">Sheet表名,不填默认为Sheet1</param>
        public static void OutPutExcel(HttpContext context, string excelName, DataTable dt, List<string> tableHead, bool is03, params string[] sheetName)
        {
            string filename = excelName;
            string defaultsheetName = "Sheet1";
            if (is03)//03
            {
                filename += ".xls";
                context.Response.ContentType = "application/vnd.ms-excel";
            }
            else
            {
                filename += ".xlsx";
                 context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            if (sheetName.Length > 0)//定义了表明
            {
                defaultsheetName = sheetName[0];
            }
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
            context.Response.Clear();
            InitializeWorkbook();
            GenerateData(defaultsheetName, dt, tableHead);
            GetExcelStream().WriteTo(context.Response.OutputStream);
            context.Response.End();
        }

        /// <summary>
        /// 导出成Excel文件
        /// </summary>
        /// <param name="dt">数据源-DataTable</param>
        /// <param name="tableHead">表头每一列名称的集合</param>
        /// <param name="strWhere">保存路径。如：C:\OutPutExcle\人口.xls</param>
        public static void OutPutExcel(DataTable dt,List<string> tableHead,string strWhere)
        {
            InitializeWorkbook();
            GenerateData("Sheet1", dt, tableHead);
            using (MemoryStream ms = GetExcelStream())
            {
                using (FileStream fs = new FileStream(strWhere, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        
        //*******************************************************************

        /// <summary>
        /// 从Excel导入到DataTable
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns>DataTable</returns>
        public static DataTable InPutExcel(string path)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            return ConvertToDataTable();
        }
        /// <summary>
        /// 从Excel导入到DataTable
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <returns>DataTable</returns>
        public static DataTable InPutExcel(FileStream fileStream)
        {
            hssfworkbook = new HSSFWorkbook(fileStream);
            return ConvertToDataTable();
        }
        /// <summary>
        /// 从Excel导入到DataTable
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="DBColumName">与Excel对应的数据库中每一列的名字</param>
        /// <returns>DataTable</returns>
        public static DataTable InPutExcel(string path, List<string> DBColumName)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            return ConvertToDataTable(DBColumName);
        }
        /// <summary>
        /// 从Excel导入到DataTable
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="DBColumName">与Excel对应的数据库中每一列的名字</param>
        /// <returns>DataTable</returns>
        public static DataTable InPutExcel(FileStream fileStream, List<string> DBColumName)
        {
            hssfworkbook = new HSSFWorkbook(fileStream);
            return ConvertToDataTable(DBColumName);
        }

        //*******************************************************************
       static DataTable ConvertToDataTable(List<string> DBColumName)
        {
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();
            for (int j = 0; j < 5; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                if (row.RowNum == 0)//第零行，创建数据库表的表头
                {
                    for (int i = 0; i < DBColumName.Count; i++)
                    {
                        dr[i] = DBColumName[i];
                    }
                }
                else
                {
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        ICell cell = row.GetCell(i);


                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

       static DataTable ConvertToDataTable()
        {
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();
            for (int j = 0; j < 5; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);


                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
       static MemoryStream GetExcelStream()
         {
             //Write the stream data of workbook to the root directory
             MemoryStream file = new MemoryStream();
             hssfworkbook.Write(file);
             return file;
         }

       static void GenerateData(string SheetName, DataTable dt, List<string> TableHead)
         {
             int rowNumber = 1;
             int i = 0;
             ISheet sheet1 = hssfworkbook.CreateSheet(SheetName);
             //创建表头
             IRow rowHead = sheet1.CreateRow(0);
             foreach (string headStr in TableHead)
             {
                 rowHead.CreateCell(i).SetCellValue(headStr);
                 i++;
             }
             //创建表内容
             foreach (DataRow item in dt.Rows)
             {
                 //创建一行
                 IRow row = sheet1.CreateRow(rowNumber);
                 //创建单元格
                 for (int j = 0; j < TableHead.Count; j++)
                 {
                     row.CreateCell(j).SetCellValue(item[j].ToString());
                 }
                 //row.CreateCell(1).SetCellValue(item.MingCheng);
                 rowNumber++;
             }
         }

       static void InitializeWorkbook()
         {
             hssfworkbook = new HSSFWorkbook();

             ////create a entry of DocumentSummaryInformation
             DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
             dsi.Company = "Mr_Han By NPOI";
             hssfworkbook.DocumentSummaryInformation = dsi;

             ////create a entry of SummaryInformation
             SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
             si.Subject = "NPOI 摘要";
             hssfworkbook.SummaryInformation = si;
         }
    }


}