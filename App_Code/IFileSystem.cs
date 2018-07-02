using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

using System.Drawing;

    public abstract class IFileSystem
    {
        public enum FileMoveOptions { Copy, Replace, Rename, Skip }
        public string RootPath { get; set; }
        public string ImportFolder { get; set; }
        public string ExportFolder { get; set; }
        public string BankFolder { get; set; }
        public string ImportedFolder { get; set; }
        public string LostFolder { get; set; }
        public string TrashFolder { get; set; }
        public string SuspectFolder { get; set; }
        public string DetectedFolder { get; set; }
        public string FrackedFolder { get; set; }
        public string TemplateFolder { get; set; }
        public string PartialFolder { get; set; }
        public string CounterfeitFolder { get; set; }
        public string LanguageFolder { get; set; }
        public string PreDetectFolder { get; set; }
        public string RequestsFolder { get; set; }
        public string DangerousFolder { get; set; }
        public string LogsFolder { get; set; }
        public string QRFolder { get; set; }
        public string BarCodeFolder { get; set; }
        public string CSVFolder { get; set; }

        //public abstract IFileSystem(string path);

        public static IEnumerable<CloudCoin> importCoins;
        public static IEnumerable<CloudCoin> exportCoins;
        public static IEnumerable<CloudCoin> importedCoins;
        public static IEnumerable<FileInfo> templateFiles;
        public static IEnumerable<CloudCoin> languageCoins;
        public static IEnumerable<CloudCoin> counterfeitCoins;
        public static IEnumerable<CloudCoin> partialCoins;
        public static IEnumerable<CloudCoin> frackedCoins;
        public static IEnumerable<CloudCoin> detectedCoins;
        public static IEnumerable<CloudCoin> suspectCoins;
        public static IEnumerable<CloudCoin> trashCoins;
        public static IEnumerable<CloudCoin> bankCoins;
        public static IEnumerable<CloudCoin> lostCoins;
        public static IEnumerable<CloudCoin> predetectCoins;
        public static IEnumerable<CloudCoin> dangerousCoins;

        public abstract bool CreateFolderStructure();

        public abstract void LoadFileSystem();

        public abstract void ClearCoins(string FolderName);

        public List<CloudCoin> LoadCoinsByFormat(string folder, Formats format)
        {
            List<CloudCoin> folderCoins = new List<CloudCoin>();

            if (format == Formats.BarCode)
            {
                var files = Directory
               .GetFiles(folder)
               .Where(file => Config.allowedExtensions.Any(file.ToLower().EndsWith))
               .ToList();

                string[] fnames = new string[files.Count()];
                for (int i = 0; i < files.Count(); i++)
                {
                    fnames[i] = Path.GetFileName(files.ElementAt(i));
                    string ext = Path.GetExtension(files.ElementAt(i));

                    try
                    {
                        var coin = readQRCode(files[i]);
                        folderCoins.Add(coin);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return folderCoins;
        }

            public List<CloudCoin> LoadFolderCoins(string folder)
        {
            List<CloudCoin> folderCoins = new List<CloudCoin>();


            // Get All the supported CloudCoin Files from the folder
            var files = Directory
                .GetFiles(folder)
                .Where(file => Config.allowedExtensions.Any(file.ToLower().EndsWith))
                .ToList();

            string[] fnames = new string[files.Count()];
            for (int i = 0; i < files.Count(); i++)
            {
                fnames[i] = Path.GetFileName(files.ElementAt(i));
                string ext = Path.GetExtension(files.ElementAt(i));
                if (ext == ".stack" || ext == ".celebrium" || ext == ".celeb")
                {
                   var coins = Utils.LoadJson(files[i]);
                    if (coins != null)
                        folderCoins.AddRange(coins);
                }
                if (ext == ".jpeg" || ext == ".jpg")
                {
                    try
                    {
                        var coin = importJPEG(files[i]);
                        folderCoins.Add(coin);
                    }
                    catch (Exception e)
                    {

                    }
                }
            };

            return folderCoins;
        }

        private CloudCoin readQRCode(String fileName)//Move one jpeg to suspect folder. 
        {
            CloudCoin coin = new CloudCoin();

            //IBarcodeReader reader = new BarcodeReader();
            // load a bitmap
            //var barcodeBitmap = (System.Drawing.Bitmap)Image.LoadFrom("C:\\sample-barcode-image.png");
            //// detect and decode the barcode inside the bitmap
            //var result = reader.Decode(barcodeBitmap);
            //// do something with the result
            //if (result != null)
            //{
                
            //}

            return coin;
        }
            private CloudCoin importJPEG(String fileName)//Move one jpeg to suspect folder. 
        {
            // bool isSuccessful = false;
            // Console.Out.WriteLine("Trying to load: " + this.fileUtils.importFolder + fileName );
            Debug.WriteLine("Trying to load: " + ImportFolder + fileName);
            try
            {
                //  Console.Out.WriteLine("Loading coin: " + fileUtils.importFolder + fileName);
                //CloudCoin tempCoin = this.fileUtils.loadOneCloudCoinFromJPEGFile( fileUtils.importFolder + fileName );

                /*Begin import from jpeg*/

                /* GET the first 455 bytes of he jpeg where the coin is located */
                String wholeString = "";
                byte[] jpegHeader = new byte[455];
                // Console.Out.WriteLine("Load file path " + fileUtils.importFolder + fileName);
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                try
                {
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(jpegHeader, sum, 455 - sum)) > 0)
                        sum += count;  // sum is a buffer offset for next reading
                }
                finally
                {
                    fileStream.Dispose();
                    //fileStream.Close();
                }
                wholeString = bytesToHexString(jpegHeader);

                CloudCoin tempCoin = parseJpeg(wholeString);
                // Console.Out.WriteLine("From FileUtils returnCC.fileName " + tempCoin.fileName);

                /*end import from jpeg file */



                //   Console.Out.WriteLine("Loaded coin filename: " + tempCoin.fileName);

                writeTo(SuspectFolder, tempCoin);
                return tempCoin;
            }
            catch (FileNotFoundException ex)
            {
                Console.Out.WriteLine("File not found: " + fileName + ex);
                //CoreLogger.Log("File not found: " + fileName + ex);
            }
            catch (IOException ioex)
            {
                Console.Out.WriteLine("IO Exception:" + fileName + ioex);
                //CoreLogger.Log("IO Exception:" + fileName + ioex);
            }// end try catch
            return null;
        }


        public CloudCoin LoadCoin(string fileName)
        {
            var coins = Utils.LoadJson(fileName);

            if (coins != null && coins.Length > 0)
                return coins[0];
            return null;
        }
        public IEnumerable<CloudCoin> LoadCoins(string fileName)
        {
            var coins = Utils.LoadJson(fileName);

            if (coins != null && coins.Length > 0)
                return coins;
            return null;
        }
        public List<FileInfo> LoadFiles(string folder)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            var files = Directory
                .GetFiles(folder)
                .ToList();
            foreach (var item in files)
            {
                fileInfos.Add(new FileInfo(item));
                Debug.WriteLine("Read File-" + item);
            }

            Debug.WriteLine("Total " + files.Count + " items read");

            return fileInfos;
        }

        public List<FileInfo> LoadFiles(string folder, string[] allowedExtensions)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            var files = Directory
                .GetFiles(folder)
                .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                .ToList();
            foreach (var item in files)
            {
                fileInfos.Add(new FileInfo(item));
                //Debug.WriteLine(item);
            }

            //Debug.WriteLine("Total " + files.Count + " items read");

            return fileInfos;
        }

        public abstract void ProcessCoins(IEnumerable<CloudCoin> coins);
        public abstract void DetectPreProcessing();


        public CloudCoin loadOneCloudCoinFromJsonFile(String loadFilePath)
        {

            CloudCoin returnCC = new CloudCoin();

            //Load file as JSON
            String incomeJson = this.importJSON(loadFilePath);
            //STRIP UNESSARY test
            int secondCurlyBracket = ordinalIndexOf(incomeJson, "{", 2) - 1;
            int firstCloseCurlyBracket = ordinalIndexOf(incomeJson, "}", 0) - secondCurlyBracket;
            // incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket);
            incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket + 1);
            // Console.Out.WriteLine(incomeJson);
            //Deserial JSON

            try
            {
                returnCC = JsonConvert.DeserializeObject<CloudCoin>(incomeJson);

            }
            catch (JsonReaderException)
            {
                Console.WriteLine("There was an error reading files in your bank.");
                Console.WriteLine("You may have the aoid memo bug that uses too many double quote marks.");
                Console.WriteLine("Your bank files are stored using and older version that did not use properly formed JSON.");
                Console.WriteLine("Would you like to upgrade these files to the newer standard?");
                Console.WriteLine("Your files will be edited.");
                Console.WriteLine("1 for yes, 2 for no.");


            }

            return returnCC;
        }//end load one CloudCoin from JSON

        public void MoveFile(string SourcePath, string TargetPath, FileMoveOptions options)
        {
            if (!File.Exists(TargetPath))
                File.Move(SourcePath, TargetPath);
            else
            {
                if (options == FileMoveOptions.Replace)
                {
                    File.Delete(TargetPath);
                    File.Move(SourcePath, TargetPath);
                }
                if (options == FileMoveOptions.Rename)
                {
                    string targetFileName = Path.GetFileNameWithoutExtension(SourcePath);
                    targetFileName += Utils.RandomString(8).ToLower() + ".stack";
                    string targetPath = Path.GetDirectoryName(TargetPath) + Path.DirectorySeparatorChar + targetFileName;
                    File.Move(SourcePath, targetPath);

                }
            }
        }

        public String importJSON(String jsonfile)
        {
            String jsonData = "";
            String line;

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.

                using (var sr = File.OpenText(jsonfile))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }//End if line is null
                        jsonData = (jsonData + line + "\n");
                    }//end while true
                }//end using
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file " + jsonfile + " could not be read:");
                Console.WriteLine(e.Message);
            }
            return jsonData;
        }//end importJSON

        // en d json test
        public String setJSON(CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "\t";
            String json = (tab + tab + "{ " + Environment.NewLine);// {
            json += tab + tab + quote + "nn" + quote + ":" + quote + cc.nn + quote + ", " + Environment.NewLine;// "nn":"1",
            json += tab + tab + quote + "sn" + quote + ":" + quote + cc.sn + quote + ", " + Environment.NewLine;// "sn":"367544",
            json += tab + tab + quote + "an" + quote + ": [" + quote;// "an": ["
            for (int i = 0; (i < 25); i++)
            {
                json += cc.an[i];// 8551995a45457754aaaa44
                if (i == 4 || i == 9 || i == 14 || i == 19)
                {
                    json += quote + "," + Environment.NewLine + tab + tab + tab + quote; //", 
                }
                else if (i == 24)
                {
                    // json += "\""; last one do nothing
                }
                else
                { // end if is line break
                    json += quote + ", " + quote;
                }

                // end else
            }// end for 25 ans

            json += quote + "]," + Environment.NewLine;//"],
            // End of ans
            //CoinUtils cu = new CoinUtils(cc);
            //cu.calcExpirationDate();
            cc.CalcExpirationDate();
            json += tab + tab + quote + "ed" + quote + ":" + quote + cc.ed + quote + "," + Environment.NewLine; // "ed":"9-2016",
            if (string.IsNullOrEmpty(cc.pown)) { cc.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu"; }//Set pown to unknow if it is not set. 
            json += tab + tab + quote + "pown" + quote + ":" + quote + cc.pown + quote + "," + Environment.NewLine;// "pown":"uuupppppffpppppfuuf",
            json += tab + tab + quote + "aoid" + quote + ": []" + Environment.NewLine;
            json += tab + tab + "}" + Environment.NewLine;
            // Keep expiration date when saving (not a truley accurate but good enought )
            return json;
        }
        // end get JSON

        public abstract void MoveImportedFiles();
        public void RemoveCoins(IEnumerable<CloudCoin> coins, string folder)
        {

            foreach (var coin in coins)
            {
                File.Delete(folder + (coin.FileName) + ".stack");

            }
        }

        public void RemoveCoins(IEnumerable<CloudCoin> coins, string folder, string extension)
        {

            foreach (var coin in coins)
            {
                File.Delete(folder + (coin.FileName) + extension);

            }
        }

        public void MoveCoins(IEnumerable<CloudCoin> coins, string sourceFolder, string targetFolder, bool replaceCoins = false)
        {
            var folderCoins = LoadFolderCoins(targetFolder);

            foreach (var coin in coins)
            {
                string fileName = (coin.FileName);
                int coinExists = (from x in folderCoins
                                  where x.sn == coin.sn
                                  select x).Count();
                if (coinExists > 0 && !replaceCoins)
                {
                    string suffix = Utils.RandomString(16);
                    fileName += suffix.ToLower();
                }
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    Stack stack = new Stack(coin);
                    using (StreamWriter sw = new StreamWriter(targetFolder + fileName + ".stack"))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, stack);
                    }
                    File.Delete(sourceFolder + (coin.FileName) + ".stack");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            }
        }

        public void MoveCoins(IEnumerable<CloudCoin> coins, string sourceFolder, string targetFolder, string extension, bool replaceCoins = false)
        {
            var folderCoins = LoadFolderCoins(targetFolder);

            foreach (var coin in coins)
            {
                string fileName = (coin.FileName);
                int coinExists = (from x in folderCoins
                                  where x.sn == coin.sn
                                  select x).Count();
                if (coinExists > 0 && !replaceCoins)
                {
                    string suffix = Utils.RandomString(16);
                    fileName += suffix.ToLower();
                }
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    Stack stack = new Stack(coin);
                    using (StreamWriter sw = new StreamWriter(targetFolder + fileName + extension))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, stack);
                    }
                    File.Delete(sourceFolder + (coin.FileName) + extension);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            }
        }

        public void WriteCoinsToFile(IEnumerable<CloudCoin> coins, string fileName, string extension = ".stack")
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            Stack stack = new Stack(coins.ToArray());
            using (StreamWriter sw = new StreamWriter(fileName + extension))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, stack);
            }
        }

        public void WriteCoin(CloudCoin coin, string folder)
        {
            var folderCoins = LoadFolderCoins(folder);
            string fileName = (coin.FileName);
            int coinExists = (from x in folderCoins
                              where x.sn == coin.sn
                              select x).Count();
            if (coinExists > 0)
            {
                string suffix = Utils.RandomString(16);
                fileName += suffix.ToLower();
            }
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            Stack stack = new Stack(coin);
            using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + fileName + ".stack"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, stack);
            }
        }

        public void WriteCoinToFile(CloudCoin coin, string filename)
        {


            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            Stack stack = new Stack(coin);
            using (StreamWriter sw = new StreamWriter(filename))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, stack);
            }
        }

        public void WriteCoin(IEnumerable<CloudCoin> coins, string folder, bool writeAll = false)
        {
            if (writeAll)
            {
                string fileName = Utils.RandomString(16) + ".stack";
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                Stack stack = new Stack(coins.ToArray());
                using (StreamWriter sw = new StreamWriter(folder + fileName + ".stack"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, stack);
                }
                return;
            }
            var folderCoins = LoadFolderCoins(folder);

            foreach (var coin in coins)
            {
                string fileName = coin.FileName;
                int coinExists = (from x in folderCoins
                                  where x.sn == coin.sn
                                  select x).Count();
                if (coinExists > 0)
                {
                    string suffix = Utils.RandomString(16);
                    fileName += suffix.ToLower();
                }
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                Stack stack = new Stack(coin);
                using (StreamWriter sw = new StreamWriter(folder + fileName + ".stack"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, stack);
                }

            }
        }

        public void WriteCoin(CloudCoin coin, string folder, string extension)
        {
            var folderCoins = LoadFolderCoins(folder);
            string fileName = (coin.FileName);
            int coinExists = (from x in folderCoins
                              where x.sn == coin.sn
                              select x).Count();
            if (coinExists > 0)
            {
                string suffix = Utils.RandomString(16);
                fileName += suffix.ToLower();
            }
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            Stack stack = new Stack(coin);
            using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + fileName + extension))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, stack);
            }
        }
        public int ordinalIndexOf(String str, String substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
                pos = str.IndexOf(substr, pos + 1);
            return pos;
        }

        public bool writeQrCode(CloudCoin cc, string tag)
        {/* 
            string fileName = ExportFolder + cc.FileName + "qr." + tag + ".jpg";
            cc.pan = null;
            //QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string json = JsonConvert.SerializeObject(cc);
            
            try
            {
                json.Replace("\\", "");
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(cc.GetCSV(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20);

                qrCodeImage.Save(fileName);

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            } */
			return true;
        }


        public bool writeBarCode(CloudCoin cc, string tag)
        {
            //string fileName = ExportFolder + cc.FileName + "barcode." + tag + ".jpg";
            //cc.pan = null;
            //QRCodeGenerator qrGenerator = new QRCodeGenerator();


            //try
            //{
            //    string json = JsonConvert.SerializeObject(cc);
            //    var barcode = new Barcode(json, Settings.Default);
            //    barcode.Canvas.SaveBmp(fileName);

            //    return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return false;
            //}
            return true;
        }

        public abstract bool WriteCoinToJpeg(CloudCoin cloudCoin, string TemplateFile,string OutputFile, string tag);

        public abstract bool WriteCoinToQRCode(CloudCoin cloudCoin, string OutputFile, string tag);

        public abstract bool WriteCoinToBARCode(CloudCoin cloudCoin, string OutputFile, string tag);

        public string GetCoinTemplate(CloudCoin cloudCoin)
        {
            int denomination = cloudCoin.denomination;
            string TemplatePath = "";
            switch (denomination)
            {
                case 1:
                    TemplatePath = this.TemplateFolder + "jpeg1.jpg";
                    break;
                case 5:
                    TemplatePath = this.TemplateFolder + "jpeg5.jpg";
                    break;
                case 25:
                    TemplatePath = this.TemplateFolder + "jpeg25.jpg";
                    break;
                case 100:
                    TemplatePath = this.TemplateFolder + "jpeg100.jpg";
                    break;
                case 250:
                    TemplatePath = this.TemplateFolder + "jpeg250.jpg";
                    break;

                default:
                    break;

            }
            return TemplatePath;
        }
        public bool writeJpeg(CloudCoin cc, string tag)
        {
           /*  // Console.Out.WriteLine("Writing jpeg " + cc.sn);

            //  CoinUtils cu = new CoinUtils(cc);

            bool fileSavedSuccessfully = true;

            /* BUILD THE CLOUDCOIN STRING */
          /*  String cloudCoinStr = "01C34A46494600010101006000601D05"; //THUMBNAIL HEADER BYTES
            for (int i = 0; (i < 25); i++)
            {
                cloudCoinStr = cloudCoinStr + cc.an[i];
            } // end for each an

            //cloudCoinStr += "204f42455920474f4420262044454645415420545952414e545320";// Hex for " OBEY GOD & DEFEAT TYRANTS "
            //cloudCoinStr += "20466f756e6465727320372d352d3137";// Founders 7-5-17
            cloudCoinStr += "4c6976652046726565204f7220446965";// Live Free or Die
            cloudCoinStr += "00000000000000000000000000";//Set to unknown so program does not export user data
                                                         // for (int i =0; i < 25; i++) {
                                                         //     switch () { }//end switch pown char
                                                         // }//end for each pown
            cloudCoinStr += "00"; // HC: Has comments. 00 = No
            cc.CalcExpirationDate();
            cloudCoinStr += cc.edHex; // 01;//Expiration date Sep 2016 (one month after zero month)
            cloudCoinStr += "01";//  cc.nn;//network number
            String hexSN = cc.sn.ToString("X6");
            String fullHexSN = "";
            switch (hexSN.Length)
            {
                case 1: fullHexSN = ("00000" + hexSN); break;
                case 2: fullHexSN = ("0000" + hexSN); break;
                case 3: fullHexSN = ("000" + hexSN); break;
                case 4: fullHexSN = ("00" + hexSN); break;
                case 5: fullHexSN = ("0" + hexSN); break;
                case 6: fullHexSN = hexSN; break;
            }
            cloudCoinStr = (cloudCoinStr + fullHexSN);
            /* BYTES THAT WILL GO FROM 04 to 454 (Inclusive)*/
            /*byte[] ccArray = this.hexStringToByteArray(cloudCoinStr);


            /* READ JPEG TEMPLATE*/
          /*  byte[] jpegBytes = null;
            switch (cc.getDenomination())
            {
                case 1: jpegBytes = readAllBytes(this.TemplateFolder + "jpeg1.jpg"); break;
                case 5: jpegBytes = readAllBytes(this.TemplateFolder + "jpeg5.jpg"); break;
                case 25: jpegBytes = readAllBytes(this.TemplateFolder + "jpeg25.jpg"); break;
                case 100: jpegBytes = readAllBytes(this.TemplateFolder + "jpeg100.jpg"); break;
                case 250: jpegBytes = readAllBytes(this.TemplateFolder + "jpeg250.jpg"); break;
            }// end switch


            /* WRITE THE SERIAL NUMBER ON THE JPEG */

          /*  //Bitmap bitmapimage;
            SKBitmap bitmapimage;
            //using (var ms = new MemoryStream(jpegBytes))
            {

                //bitmapimage = new Bitmap(ms);
                bitmapimage = SKBitmap.Decode(jpegBytes);
            }
            SKCanvas canvas = new SKCanvas(bitmapimage);
            //Graphics graphics = Graphics.FromImage(bitmapimage);
            //graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            SKPaint textPaint = new SKPaint()
            {
                IsAntialias = true,
                Color = SKColors.White,
                TextSize = 14,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };
            //PointF drawPointAddress = new PointF(30.0F, 25.0F);

            canvas.DrawText(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", 30, 40, textPaint);
            //graphics.DrawString(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", new Font("Arial", 10), Brushes.White, drawPointAddress);

            //ImageConverter converter = new ImageConverter();
            //byte[] snBytes = (byte[])converter.ConvertTo(bitmapimage, typeof(byte[]));
            SKImage image = SKImage.FromBitmap(bitmapimage);
            SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            byte[] snBytes = data.ToArray();

            List<byte> b1 = new List<byte>(snBytes);
            List<byte> b2 = new List<byte>(ccArray);
            b1.InsertRange(4, b2);

            if (tag == "random")
            {
                Random r = new Random();
                int rInt = r.Next(100000, 1000000); //for ints
                tag = rInt.ToString();
            }

            string fileName = ExportFolder + cc.FileName + tag + ".jpg";
            File.WriteAllBytes(fileName, b1.ToArray());
            Console.Out.WriteLine("Writing to " + fileName);
            //CoreLogger.Log("Writing to " + fileName);
            return fileSavedSuccessfully; 
		
			return true;
        }//end write JPEG

        public bool writeJpeg(CloudCoin cc, string tag,string filePath)
        {
            // Console.Out.WriteLine("Writing jpeg " + cc.sn);

            //  CoinUtils cu = new CoinUtils(cc);
            filePath = filePath.Replace("\\\\","\\");
            bool fileSavedSuccessfully = true;

            /* BUILD THE CLOUDCOIN STRING */
          /*  String cloudCoinStr = "01C34A46494600010101006000601D05"; //THUMBNAIL HEADER BYTES
            for (int i = 0; (i < 25); i++)
            {
                cloudCoinStr = cloudCoinStr + cc.an[i];
            } // end for each an

            //cloudCoinStr += "204f42455920474f4420262044454645415420545952414e545320";// Hex for " OBEY GOD & DEFEAT TYRANTS "
            //cloudCoinStr += "20466f756e6465727320372d352d3137";// Founders 7-5-17
            cloudCoinStr += "4c6976652046726565204f7220446965";// Live Free or Die
            cloudCoinStr += "00000000000000000000000000";//Set to unknown so program does not export user data
                                                         // for (int i =0; i < 25; i++) {
                                                         //     switch () { }//end switch pown char
                                                         // }//end for each pown
            cloudCoinStr += "00"; // HC: Has comments. 00 = No
            cc.CalcExpirationDate();
            cloudCoinStr += cc.edHex; // 01;//Expiration date Sep 2016 (one month after zero month)
            cloudCoinStr += "01";//  cc.nn;//network number
            String hexSN = cc.sn.ToString("X6");
            String fullHexSN = "";
            switch (hexSN.Length)
            {
                case 1: fullHexSN = ("00000" + hexSN); break;
                case 2: fullHexSN = ("0000" + hexSN); break;
                case 3: fullHexSN = ("000" + hexSN); break;
                case 4: fullHexSN = ("00" + hexSN); break;
                case 5: fullHexSN = ("0" + hexSN); break;
                case 6: fullHexSN = hexSN; break;
            }
            cloudCoinStr = (cloudCoinStr + fullHexSN);
            /* BYTES THAT WILL GO FROM 04 to 454 (Inclusive)*/
         /*   byte[] ccArray = this.hexStringToByteArray(cloudCoinStr);


            /* READ JPEG TEMPLATE*/
          /*  byte[] jpegBytes = null;

            //jpegBytes = readAllBytes(filePath);
            jpegBytes = File.ReadAllBytes(filePath);

            /* WRITE THE SERIAL NUMBER ON THE JPEG */

            //Bitmap bitmapimage;
          //  SKBitmap bitmapimage;
            //using (var ms = new MemoryStream(jpegBytes))
        /*    {

                //bitmapimage = new Bitmap(ms);
                bitmapimage = SKBitmap.Decode(jpegBytes);
            }
            SKCanvas canvas = new SKCanvas(bitmapimage);
            //Graphics graphics = Graphics.FromImage(bitmapimage);
            //graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            SKPaint textPaint = new SKPaint()
            {
                IsAntialias = true,
                Color = SKColors.White,
                TextSize = 14,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };
            //PointF drawPointAddress = new PointF(30.0F, 25.0F);

            canvas.DrawText(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", 30, 40, textPaint);
            //graphics.DrawString(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", new Font("Arial", 10), Brushes.White, drawPointAddress);

            //ImageConverter converter = new ImageConverter();
            //byte[] snBytes = (byte[])converter.ConvertTo(bitmapimage, typeof(byte[]));
            SKImage image = SKImage.FromBitmap(bitmapimage);
            SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            byte[] snBytes = data.ToArray();

            List<byte> b1 = new List<byte>(snBytes);
            List<byte> b2 = new List<byte>(ccArray);
            b1.InsertRange(4, b2);

            if (tag == "random")
            {
                Random r = new Random();
                int rInt = r.Next(100000, 1000000); //for ints
                tag = rInt.ToString();
            }

            string fileName = ExportFolder + cc.FileName  + ".jpg";
            File.WriteAllBytes(fileName, b1.ToArray());
            Console.Out.WriteLine("Writing to " + fileName);
            //CoreLogger.Log("Writing to " + fileName);
            return fileSavedSuccessfully; */
			return true;
        }//end write JPEG

        public bool writeJpeg(CloudCoin cc, string tag, string filePath,string targetPath)
        {return true;
        }//end write JPEG

        public bool writeJpeg(CloudCoin cc, string tag, string filePath, string targetPath,string printMessage){
        return true;
        }//end write JPEG
        public string bytesToHexString(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int length = data.Length;
            char[] hex = new char[length * 2];
            int num1 = 0;
            for (int index = 0; index < length * 2; index += 2)
            {
                byte num2 = data[num1++];
                hex[index] = GetHexValue(num2 / 0x10);
                hex[index + 1] = GetHexValue(num2 % 0x10);
            }
            return new string(hex);
        }//End NewConverted

        private char GetHexValue(int i)
        {
            if (i < 10)
            {
                return (char)(i + 0x30);
            }
            return (char)((i - 10) + 0x41);
        }//end GetHexValue

        /* Writes a JPEG To the Export Folder */

        /* OPEN FILE AND READ ALL CONTENTS AS BYTE ARRAY */
        public byte[] readAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                int fileLength = Convert.ToInt32(fs.Length);
                fs.Read(buffer, 0, fileLength);
            }
            return buffer;
        }//end read all bytes

        public bool writeTo(String folder, CloudCoin cc)
        {
            //CoinUtils cu = new CoinUtils(cc);
            const string quote = "\"";
            const string tab = "\t";
            String wholeJson = "{" + Environment.NewLine; //{
            bool alreadyExists = true;
            String json = this.setJSON(cc);
            if (!File.Exists(folder + cc.FileName + ".stack"))
            {
                wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
                wholeJson += json;
                wholeJson += Environment.NewLine + tab + "]" + Environment.NewLine + "}";
                File.WriteAllText(folder + cc.FileName + ".stack", wholeJson);
            }
            else
            {
                if (folder.Contains("Counterfeit") || folder.Contains("Trash"))
                {
                    //Let the program delete it
                    alreadyExists = false;
                    return alreadyExists;
                }
                else if (folder.Contains("Imported"))
                {
                    File.Delete(folder + cc.FileName + ".stack");
                    File.WriteAllText(folder + cc.FileName + ".stack", wholeJson);
                    alreadyExists = false;
                    return alreadyExists;
                }
                else
                {
                    Console.WriteLine(cc.FileName + ".stack" + " already exists in the folder " + folder);
                    //CoreLogger.Log(cu.fileName + ".stack" + " already exists in the folder " + folder);
                    return alreadyExists;

                }//end else

            }//File Exists
            File.WriteAllText(folder + cc.FileName + ".stack", wholeJson);
            alreadyExists = false;
            return alreadyExists;

        }//End Write To

        public void overWrite(String folder, CloudCoin cc)
        {
            //CoinUtils cu = new CoinUtils(cc);
            const string quote = "\"";
            const string tab = "\t";
            String wholeJson = "{" + Environment.NewLine; //{
            String json = this.setJSON(cc);

            wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
            wholeJson += json;
            wholeJson += Environment.NewLine + tab + "]" + Environment.NewLine + "}";

            File.WriteAllText(folder + cc.FileName + ".stack", wholeJson);
        }//End Overwrite

        public CloudCoin loadOneCloudCoinFromJPEGFile(String loadFilePath)
        {
            /* GET the first 455 bytes of he jpeg where the coin is located */
            String wholeString = "";
            byte[] jpegHeader = new byte[455];
            Console.Out.WriteLine("Load file path " + loadFilePath);
            using (FileStream fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = fileStream.Read(jpegHeader, sum, 455 - sum)) > 0)
                        sum += count;  // sum is a buffer offset for next reading
                }
                finally { }
            }
            wholeString = bytesToHexString(jpegHeader);
            CloudCoin returnCC = this.parseJpeg(wholeString);
            // Console.Out.WriteLine("From FileUtils returnCC.fileName " + returnCC.fileName);
            return returnCC;
        }//end load one CloudCoin from JSON

        public CloudCoin parseJpeg(String wholeString)
        {

            CloudCoin cc = new CloudCoin();
            int startAn = 40;
            for (int i = 0; i < 25; i++)
            {
                cc.an.Add(wholeString.Substring(startAn, 32));
                //cc.an[i] = wholeString.Substring(startAn, 32);
                // Console.Out.WriteLine(i +": " + cc.an[i]);
                startAn += 32;
            }

            // end for
            cc.aoid = null;
            // wholeString.substring( 840, 895 );
            //cc.hp = 25;
            // Integer.parseInt(wholeString.substring( 896, 896 ), 16);
            cc.ed = wholeString.Substring(898, 4);
            cc.nn = Convert.ToInt32(wholeString.Substring(902, 2), 16);
            cc.sn = Convert.ToInt32(wholeString.Substring(904, 6), 16);
            cc.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
            //  Console.Out.WriteLine("parseJpeg cc.fileName " + cc.fileName);
            return cc;
        }// end parse Jpeg

        // en d json test
        public byte[] hexStringToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }//End hex string to byte array





    }
