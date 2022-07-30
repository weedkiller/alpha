using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Multigroup.Services.Providers
{
    public class ConfigurationProvider
    {
        public static readonly string ActionPlanTemplatePath = ConfigurationManager.AppSettings["ActionPlanTemplatePath"];
        public static readonly string ActionPlanPageTemplatePath = ConfigurationManager.AppSettings["ActionPlanPageTemplatePath"];
        public static readonly string ActionPlanGeneratePath = ConfigurationManager.AppSettings["ActionPlanGeneratePath"];
        public static readonly string ActionPlanImagesPath = ConfigurationManager.AppSettings["ActionPlanImagesPath"];
        public static readonly string ActionPlanDocumentPath = ConfigurationManager.AppSettings["ActionPlanDocumentPath"];
        public static readonly string SpendingDocumentPath = ConfigurationManager.AppSettings["SpendingDocumentPath"];
        public static readonly string AttachmentPath = ConfigurationManager.AppSettings["AttachmentPath"];
        public static readonly string EquipmentModelIcon = ConfigurationManager.AppSettings["EquipmentModelIcon"];
        public static readonly string SampleDetailDocumentPath = ConfigurationManager.AppSettings["SampleDetailDocumentPath"];
        public static readonly string InspectionDocumentPath = ConfigurationManager.AppSettings["InspectionDocumentPath"];
        public static readonly string SampleDetailTemplatePath = ConfigurationManager.AppSettings["SampleDetailTemplatePath"];
        public static readonly string SampleDetailGeneratePath = ConfigurationManager.AppSettings["SampleDetailGeneratePath"];
        public static readonly string CustomPageName = ConfigurationManager.AppSettings["CustomPageName"];
        public static readonly string AnalysisFilePath = ConfigurationManager.AppSettings["AnalysisFilePath"];
        public static readonly string AttachmentActionPlanPath = ConfigurationManager.AppSettings["AttachmentActionPlanPath"];
        public static readonly string RoutineDataImagePath = ConfigurationManager.AppSettings["RoutineDataImagePath"];
        public static readonly string GeneralReportPath = ConfigurationManager.AppSettings["GeneralReportPath"];

        public static readonly string GeneralReportPathImgHead = ConfigurationManager.AppSettings["GeneralReportPathImgHead"];
        public static readonly string GeneralReportPathImgFooter = ConfigurationManager.AppSettings["GeneralReportPathImgFooter"];

        public static readonly string GeneralReportPathSaveTemp = ConfigurationManager.AppSettings["GeneralReportPathSaveTemp"];
        public static readonly string GeneralReportPathSaveEnd = ConfigurationManager.AppSettings["GeneralReportPathSaveEnd"];

        public static readonly string GeneralReportTemplateHtmlPag1 = ConfigurationManager.AppSettings["GeneralReportTemplateHtmlPag1"];
        public static readonly string GeneralReportTemplateHtmlPag3 = ConfigurationManager.AppSettings["GeneralReportTemplateHtmlPag3"];
        public static readonly string GeneralReportTemplateHtmlPag31 = ConfigurationManager.AppSettings["GeneralReportTemplateHtmlPag31"];

        public static readonly string GeneralReportTemplateHtmlPag2Body = ConfigurationManager.AppSettings["GeneralReportTemplateHtmlPag2Body"];
        public static readonly string GeneralReportTemplateHtmlPag2TrHead = ConfigurationManager.AppSettings["GeneralReportTemplateHtmlPag2TrHead"];
        public static readonly string GeneralReportTemplateHtmlPag2TrBody = ConfigurationManager.AppSettings["GeneralReportTemplateHtmlPag2TrBody"];

        public static readonly string GeneralReportPathImageCondition = ConfigurationManager.AppSettings["GeneralReportPathImageCondition"];
        public static readonly string RoutineDataImagePathDirectory = ConfigurationManager.AppSettings["RoutineDataImagePathDirectory"];

        public static readonly string RoutineDataTemplatePart1 = ConfigurationManager.AppSettings["RoutineDataTemplatePart1"];
        public static readonly string RoutineDataTemplate = ConfigurationManager.AppSettings["RoutineDataTemplate"];

        public static readonly string RoutineImageHeader = ConfigurationManager.AppSettings["RoutineImageHeader"];
        public static readonly string RoutineImageFooterGreen = ConfigurationManager.AppSettings["RoutineImageFooterGreen"];
        public static readonly string RoutineImageFooterRed = ConfigurationManager.AppSettings["RoutineImageFooterRed"];
        public static readonly string RoutineImageFooterYellow = ConfigurationManager.AppSettings["RoutineImageFooterYellow"];
    }
}
