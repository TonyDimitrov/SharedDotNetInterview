namespace DotNetInterview.Web.PdfGenerator
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public interface IHtmlToPdfConverter
    {
       byte[] Convert(string basePath, string htmlCode, FormatType formatType, OrientationType orientationType);
    }
}
