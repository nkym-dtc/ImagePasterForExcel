using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;

namespace ImagePasterForExcel.Model
{
    public class Excel
    {
       
        public void MakeExcelFile(DirectoryInfo rootDir,string savePath)
        {
            var imageDirs = new List<DirectoryInfo>();
            GetImageDirs(rootDir);


            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (var dir in imageDirs)
                {
                    PasteImages(wb, dir);
                }

                wb.SaveAs(savePath);

            }

            void GetImageDirs(DirectoryInfo dirInfo)
            {
                var imgdirs = dirInfo.GetDirectories();
                foreach (var subDir in imgdirs)
                {
                    if (subDir.GetDirectories().Count() > 0)
                    {
                        GetImageDirs(subDir);
                    }
                    else
                    {
                        imageDirs.Add(subDir);
                    }
                }
            }
        }

        public void PasteImages(XLWorkbook wb, DirectoryInfo dir)
        {
            int row = 2;
            int colum = 1;

            var images = dir.GetFiles();
            IXLWorksheet ws = GetSheetName(wb, dir.Name);

            foreach (var img in images)
            {
                IXLPicture image = ws.AddPicture(img.FullName);
                image.MoveTo(ws.Cell(row, colum));

                double rowHeight = ws.RowHeight * 20 / 15;
                double rowCount = image.Height / rowHeight;

                double columnWidth = ws.ColumnWidth * 7 + 5;
                double columCount = image.Width / columnWidth;

                ws.Cell(row + (int)Math.Ceiling(rowCount), colum).Value = img.Name;
                ws.Cell(row, colum + (int)Math.Ceiling(columCount)).Value = img.Name;

                row = row + (int)Math.Ceiling(rowCount) + 2;

            }

        }

        public IXLWorksheet GetSheetName(XLWorkbook wb, string name)
        {
            try
            {
                return wb.AddWorksheet(name);
            }
            catch
            {
                return GetSheetName(wb, $"{name}_sub");
            }
        }

    }
}

