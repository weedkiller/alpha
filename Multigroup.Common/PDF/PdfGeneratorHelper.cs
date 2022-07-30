using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Common.PDF
{
    public class PdfGeneratorHelper : IPdfGeneratorHelper
    
    {
        public string GenerateFilePdf(string templatePath, string TemplatePagePath, string pdfPath, string fileName, Hashtable formFields, List<string> htmlPageList)
        {
            string tempFileName = pdfPath + "0_" + fileName;
            string reportFileName = pdfPath + fileName;
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            AcroFields pdfFormFields = null;
            FileStream destinationDocumentStream = null;
            PdfConcatenate pdfConcat = null;
            
            try
            {
                // Genero el formulario PDF con los datos del Análisis
                pdfReader = new PdfReader(templatePath);

                //using (MemoryStream ms = new MemoryStream())
                //using (FileStream file = new FileStream(tempFileName, FileMode.Create))
                //{
                //    byte[] bytes = new byte[file.Length];
                //    file.Read(bytes, 0, (int)file.Length);
                //    ms.Write(bytes, 0, (int)file.Length);
                //}

                using (pdfStamper = new PdfStamper(pdfReader, new FileStream(tempFileName, FileMode.Create)))
                {
                    //MemoryStream ms = FileManager.GetFileStream(file);

                    pdfFormFields = pdfStamper.AcroFields;

                    // set form pdfFormFields    
                    foreach (DictionaryEntry field in formFields)
                    {
                        pdfFormFields.SetField(field.Key.ToString(), (field.Value != null) ? field.Value.ToString() : "");
                    }

                    pdfStamper.FormFlattening = true;
                    // close the pdf
                    pdfStamper.Close();
                    pdfReader.Close();
                }

                List<string> tempDocuments = new List<string>();
                tempDocuments.Add(tempFileName);

                // Genero los PDF con los HTTML que vienen adjuntos
                for (int i = 0; i < htmlPageList.Count; i++)
                {
                    string htmlPage = htmlPageList[i];
                    string path = "";
                    if (htmlPage.Trim().Length > 0)
                    {
                        path = GenerateTempPdf(htmlPage, pdfPath, (i + 1).ToString() + "_" + fileName);
                    }
                    if (path.Trim().Length > 0)
                    {
                        tempDocuments.Add(path);
                    }
                }

                using (destinationDocumentStream = File.Create(reportFileName))
                {
                    if(destinationDocumentStream.CanRead)
                    {
                        // formo el nuevo documento
                        //destinationDocumentStream = new FileStream(reportFileName, FileMode.Create);
                        pdfConcat = new PdfConcatenate(destinationDocumentStream);

                        for (int i = 0; i < tempDocuments.Count; i++)
                        {
                            string path = tempDocuments[i];
                            var sourceDocumentStream = new FileStream(path, FileMode.Open);
                            var pdfReaderTemp = new PdfReader(sourceDocumentStream);

                            pdfReaderTemp.SelectPages("1-" + pdfReaderTemp.NumberOfPages);
                            pdfConcat.AddPages(pdfReaderTemp);
                            pdfReaderTemp.Close();

                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                        }

                        pdfConcat.Close();
                    }
                }

                return reportFileName;
            }
            catch (Exception ex)
            {
                if (pdfStamper != null)
                {
                    pdfStamper.Close();
                    pdfStamper.Dispose();
                }
                throw ex;
            }
            finally
            {
                pdfReader.Close();
                pdfStamper.Close();
                destinationDocumentStream.Close();
                pdfConcat.Close();
            }
        }

        public string GenerateTempPdf(string htmlPage, string pdfPath, string fileName)
        {
            try
            {
                string tempFileName = pdfPath + fileName;
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(tempFileName, FileMode.Create));

                document.Open();
                
                List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlPage), null);

                for (int k = 0; k < htmlarraylist.Count; k++)
                {
                    document.Add((IElement)htmlarraylist[k]);
                }

                Paragraph mypara = new Paragraph();
                document.Add(mypara);
                document.Close();

                return tempFileName;
            }
            catch (Exception ex)
            {
                // Retorno vacio para indicar que no se generó ningún documento
                return string.Empty;
            }
        }

        public async Task SaveFileContentsAsync(string filePath, Stream stream)
        {

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public async Task SaveFileContentsTask(string filePath, Stream stream)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public void SaveFileContents(string filePath, Stream stream)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}
