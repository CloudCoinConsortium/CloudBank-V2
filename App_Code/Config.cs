using System;
using System.Collections.Generic;
using System.Text;

    public class Config
    {
        public static string TAG_IMPORT = "Import";
        public static string TAG_EXPORT = "Export";
        public static string TAG_BANK = "Bank";
        public static string TAG_LOST = "Lost";
        public static string TAG_IMPORTED = "Imported";
        public static string TAG_FRACKED = "Fracked";
        public static string TAG_TEMPLATES = "Templates";
        public static string TAG_COUNTERFEIT = "Counterfeit";
        public static string TAG_DETECTED = "Detected";
        public static string TAG_LANGUAGE = "Language";
        public static string TAG_PARTIAL = "Partial";
        public static string TAG_TRASH = "Trash";
        public static string TAG_SUSPECT = "Suspect";
        public static string TAG_PREDETECT = "Predetect";
        public static string URL_DIRECTORY = "http://michael.pravoslavnye.ru/";
        public static string TAG_REQUESTS = "Requests";
        public const int YEARSTILEXPIRE = 2;
        public static int milliSecondsToTimeOut = 20000;
        public static int MultiDetectLoad = 200;
        public static int NodeCount = 25;
        public static int PassCount = 16;
        public static int MinimumReadyCount = 16;

        public static int NetworkNumber = 1;

        public enum Folder { Suspect, Counterfeit, Fracked, Bank, Trash };

        public static string[] allowedExtensions = new[] { ".stack", ".jpeg", ".chest", ".bank", ".jpg",".celebrium",".celeb" };

        public static string TAG_DANGEROUS = "Dangerous";
        public static string TAG_LOGS = "Logs";
        public static string TAG_QR = "QrCodes";
        public static string TAG_BARCODE = "Barcodes";
        public static string TAG_CSV = "CSV";

        public static string URL_JPEG_Exists = "https://raida.tech/jpeg_exists.php?nn={0}&sn={1}";
        public static string URL_GET_TICKET = "get_ticket?nn={0}&sn={1}&an={2}&pan={3}&denomination={4}";
        public static string URL_GET_IMAGE = "https://raida.tech/get_template.php?nn={0}&sn={1}&fromserver1={2}&message1={3}";

    }
    public enum Formats
    {
        QRCode,
        BarCode,
        CSV
    }

