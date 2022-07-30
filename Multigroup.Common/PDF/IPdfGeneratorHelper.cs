using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Multigroup.Common.PDF
{
    public interface IPdfGeneratorHelper
    {
        string GenerateFilePdf(string templatePath, string TemplatePagePath, string pdfPath, string fileName, Hashtable formFields, List<string> htmlPageList);

        string GenerateTempPdf(string htmlPage, string pdfPath, string fileName);

        Task SaveFileContentsAsync(string filePath, Stream stream);

    }
}
