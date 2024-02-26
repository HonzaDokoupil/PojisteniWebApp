using PojisteniWebApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PojisteniWebApp.Classes
{
    /// <summary>
    /// třída na vytvařeni PDF souboru
    /// </summary>
    public class PDFGenerator
    {
        /// <summary>
        /// cesta kde budeme ukladat pdf soubory
        /// </summary>
        private string PDFFilePath = @".\wwwroot\pdf\";
        /// <summary>
        /// Metoda která vygeneruje PDF soubor
        /// </summary>
        /// <param name="info">info třída obsahující aktuální statistiku</param>
        public void GeneratePDF(Info info)
        {
            QuestPDF.Settings.License = LicenseType.Community;


            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Statistika")
                        .SemiBold().FontSize(30).FontColor(Colors.Orange.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Item().Text("Počet produktů:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.InsuranceTypesCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet pojištěnců:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.InsuredPersonsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet smluv:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.IndividualContractsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet pojistných udalostí:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.InsuranceEventsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet registrovaných uživatelů:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.UsersCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet pojištěnců bez smlouvy:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.PersonsWithNoContractsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet pojištěnců bez pojistných udalostí:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.PersonsWithNoEventCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet regitrovaných uživatelů kteří nejsou vedeni jako pojištěnci:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.UseresWithNoRole}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Počet pojištěneců bez založeného učtu:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.InsuredPersonsWithNoAccount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Pojištěnec s největší pojištěnou částkou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.PersonWithHighestInsuredValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Pojištěnec s nejmenší pojištěnou částkou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.PersonWithLowestInsuredValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Pojištěnec s nejvetší škodou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.PersonWithHighestDamageValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Pojištěnec s nejmenší škodou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.PersonWithLowestDamageValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Nejvíce smlouvaný produkt:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.MostContractedInsurenceType}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Nejméně smlouvaný produkt:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.LeastContractedInsurenceType}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Celková pojištěná suma:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.InsuredValueTotal.ToString("N")} Kč").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Celková suma všech škod:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.DamageValueTotal.ToString("N")} Kč").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                            x.Item().Text("Vytvořeno:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                            x.Item().AlignRight().Text($"{info.ReportDate}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                            x.Item().LineHorizontal(1, Unit.Mil);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page");
                            x.CurrentPageNumber();
                        });
                });


            }).GeneratePdf($"{PDFFilePath}Statistika.pdf");
        }
        /// <summary>
        /// Druha varianta metody na generaci PDF souboru
        /// </summary>
        /// <param name="report">Starší report uložený v databázi</param>
        public void GeneratePDF(Report report)
        {
            QuestPDF.Settings.License = LicenseType.Community;


            Document.Create(container =>
            {
                container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Statistika")
                    .SemiBold().FontSize(30).FontColor(Colors.Orange.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Item().Text("Počet produktů:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.InsuranceTypesCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet pojištěnců:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.InsuredPersonsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet smluv:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.IndividualContractsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet pojistných udalostí:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.InsuranceEventsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet registrovaných uživatelů:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.UsersCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet pojištěnců bez smlouvy:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.PersonsWithNoContractsCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet pojištěnců bez pojistných udalostí:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.PersonsWithNoEventCount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet regitrovaných uživatelů kteří nejsou vedeni jako pojištěnci:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.UseresWithNoRole}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Počet pojištěneců bez založeného učtu:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.InsuredPersonsWithNoAccount}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Pojištěnec s největší pojištěnou částkou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.PersonWithHighestInsuredValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Pojištěnec s nejmenší pojištěnou částkou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.PersonWithLowestInsuredValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Pojištěnec s nejvetší škodou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.PersonWithHighestDamageValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Pojištěnec s nejmenší škodou:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.PersonWithLowestDamageValueTotal}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Nejvíce smlouvaný produkt:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.MostContractedInsurenceType}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Nejméně smlouvaný produkt:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.LeastContractedInsurenceType}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Celková pojištěná suma:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.InsuredValueTotal.ToString("N")} Kč").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Celková suma všech škod:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.DamageValueTotal.ToString("N")} Kč").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                        x.Item().Text("Vytvořeno:").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).Bold().FontSize(12);
                        x.Item().AlignRight().Text($"{report.ReportDate}").FontFamily(Fonts.Calibri).Fallback(x => x.FontFamily("Segoe UI Emoji")).FontSize(12);
                        x.Item().LineHorizontal(1, Unit.Mil);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page");
                        x.CurrentPageNumber();
                    });
            });


        }).GeneratePdf($"{PDFFilePath}Statistika.pdf");
    }
    }   
}
