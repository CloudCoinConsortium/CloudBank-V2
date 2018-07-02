using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;


    public class CloudCoin
    {
        public string[] pan = new string[Config.NodeCount];
        public int hp;// HitPoints (1-25, One point for each server not failed)
        public String edHex;// Months from zero date that the coin will expire. 
        [JsonIgnore]
        public string folder;
        [JsonIgnore]
        public Response[] response = new Response[Config.NodeCount];
        [JsonIgnore]
        public String[] gradeStatus = new String[3];// What passed, what failed, what was undetected
        //Fields
        [JsonProperty("nn")]
        public int nn { get; set; }

        [JsonProperty("sn")]
        public int sn { get { return pSN; } set { pSN = value; denomination = getDenomination(); } }

        [JsonProperty("an")]
        public List<string> an { get; set; }

        [JsonProperty("ed")]
        public string ed { get; set; }

        [JsonProperty("pown")]
        public string pown { get; set; }

        [JsonProperty("aoid")]
        public List<string> aoid { get; set; }

        [JsonIgnore]
        public string pastPown = "uuuuuuuuuuuuuuuuuuuuuuuuu";//Used to see if there are any improvments in defracking

        [JsonIgnore]
        public bool IsPerfect { get; set; }
        [JsonIgnore]
        public bool IsCounterfeit { get; set; }
        [JsonIgnore]
        public bool IsGradable { get; set; }
        [JsonIgnore]
        public bool IsFracked { get; set; }
        [JsonIgnore]
        public int denomination { get; set; }
        public String DetectionResult;
        [JsonIgnore]
        public DetectionResult detectionResult { get; set; }
        [JsonIgnore]
        public DetectionStatus DetectResult { get; set; }
        public int PassCount { get { return passCount; } set { passCount = value; if (passCount >= Config.PassCount) DetectionResult = "Pass"; else DetectionResult = "Fail"; } }
        private int passCount = 0;
        private int failCount = 0;
        public int FailCount { get { return failCount; } set { failCount = value; if (passCount >= Config.PassCount) DetectionResult = "Pass"; else DetectionResult = "Fail"; } }

        public enum Folder { Suspect, Counterfeit, Fracked, Bank, Trash };

        [JsonIgnore]
        int pSN;
        //Constructors
        public CloudCoin()
        {
            an = new List<string>();
        }//end of constructor

        public CloudCoin(int nn, int sn, List<string> an, string ed, string pown, List<string> aoid)
        {
            this.nn = nn;
            this.sn = sn;
            this.an = an;
            this.ed = ed;
            this.pown = pown;
            this.aoid = aoid;

        }//end of constructor

        public CloudCoin(string fileName)
        {

        }//end of constructor
        public static CloudCoin FromCSV(string csvLine)
        {
            try
            {
                CloudCoin coin = new CloudCoin();
                string[] values = csvLine.Split(',');
                Debug.WriteLine(values[0]);
                coin.sn = Convert.ToInt32(values[0]);
                coin.nn = Convert.ToInt32(values[1]);
                coin.denomination = Convert.ToInt16(values[1]);
                coin.an = new List<string>();
                for (int i = 0; i < Config.NodeCount; i++)
                {
                    coin.an.Add(values[i + 3]);
                }

                return coin;

            }
            catch (Exception e)
            {

            }
            return null;
        }
        [JsonIgnore]
        public string FileName
        {
            get
            {
                return this.getDenomination() + ".CloudCoin." + nn + "." + sn + ".";
            }
        }

        public bool isDangerous()
        {
            //The coin is considered a threat if it has any of the patersns that would allow the last user to take control.
            //There are four of these patterns: One for each corner. 
            bool threat = false;
            //  Console.Out.WriteLine( cc.sn + " char count f =" + charCount(cc.pown, 'f'));
            if ((charCount(pown, 'f') + charCount(pown, 'n')) > 5)
            {
                string doublePown = pown + pown;//double it so we see patters that happen on the ends.
                Match UP_LEFT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]fp", RegexOptions.IgnoreCase);//String UP_LEFT = "ff***f";
                Match UP_RIGHT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]pf", RegexOptions.IgnoreCase);//String UP_RIGHT = "ff***pf";
                Match DOWN_LEFT = Regex.Match(doublePown, @"fp[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);//String DOWN_LEFT = "fp***ff";
                Match DOWN_RIGHT = Regex.Match(doublePown, @"pf[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "pf***ff";
                                                                                                            //Check if it has a weakness
                                                                                                            // if (UP_LEFT.Success) { Console.Out.WriteLine("up left match"); }//end
                                                                                                            //if (UP_RIGHT.Success) { Console.Out.WriteLine("up right match"); }//end
                                                                                                            // if (DOWN_LEFT.Success) { Console.Out.WriteLine("down left match"); }//end
                                                                                                            // if (DOWN_RIGHT.Success) { Console.Out.WriteLine("down right match"); }//end

                if (UP_LEFT.Success || UP_RIGHT.Success || DOWN_LEFT.Success || DOWN_RIGHT.Success)
                {
                    threat = true;
                }//end if coin contains threats.
            }
            return threat;
        }//end is threat

        public bool isCounterfeit()
        {
            //The coin is considered counterfeit if it has so many fails it cannot be fixed
            bool returnTruth = false;
            if ((charCount(pown, 'p') < 6 && (charCount(pown, 'f') > 13)))
            {
                returnTruth = true;
             //   Console.Out.WriteLine("isCounterfeit");
            }
            else
            {
               // Console.Out.WriteLine("Not isCounterfeit");
            }
            return returnTruth;
        }//end is counterfeit

        public int charCount(string pown, char character)
        {
            return pown.Count(x => x == character);
        }

        public string GetCSV()
        {
            string csv = this.sn + "," + this.nn + ",";
               

            for (int i = 0; i < Config.NodeCount; i++)
            {
                csv += an[i] + ",";
            }

            return csv.Substring(0, csv.Length - 1);
        }
        public bool isFracked()
        {
            //The coin is considered fracked if it has any fails
            bool returnTruth = false;
            if (charCount(pown, 'f') > 0 || charCount(pown, 'n') > 0)
            {
                returnTruth = true;
            }
            return returnTruth;
        }//end is fracked

        public bool isPerfect()
        {
            bool returnTruth = false;
            if (pown == "ppppppppppppppppppppppppp")
            {
                returnTruth = true;
            }
            return returnTruth;
        }
        public int getDenomination()
        {
            int nom = 0;
            if ((sn < 1))
            {
                nom = 0;
            }
            else if ((sn < 2097153))
            {
                nom = 1;
            }
            else if ((sn < 4194305))
            {
                nom = 5;
            }
            else if ((sn < 6291457))
            {
                nom = 25;
            }
            else if ((sn < 14680065))
            {
                nom = 100;
            }
            else if ((sn < 16777217))
            {
                nom = 250;
            }
            else
            {
                nom = '0';
            }

            return nom;
        }//end get denomination
        [JsonIgnore]
        public List<Task> detectTaskList = new List<Task>();
        public List<Task> GetDetectTasks()
        {
            var raida = RAIDA.GetInstance();

            CloudCoin cc = this;
            int i = 0;

            for (int j = 0; j < Config.NodeCount; j++)
            {
                Task t = Task.Factory.StartNew(() => raida.nodes[i].Detect(cc));
                detectTaskList.Add(t);
            }

            return detectTaskList;
        }
        public void GeneratePAN()
        {
            for (int i = 0; i < Config.NodeCount; i++)
            {
                pan[i] = this.generatePan();
            }
        }

        public bool isFixable()
        {
            //The coin is considered fixable if it has any of the patterns that would allow the new owner to fix fracked.
            //There are four of these patterns: One for each corner. 
            string origPown = pown;
            pown = pown.Replace('d', 'e').Replace('n', 'e').Replace('u', 'e');
            bool canFix = false;
            // Console.Out.WriteLine(cc.sn + " char count p =" + charCount(cc.pown, 'p'));
            if (charCount(pown, 'p') > 5)
            {
                string doublePown = pown + pown;//double it so we see patters that happen on the ends.
                Match UP_LEFT = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pf", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pf";
                Match UP_RIGHT = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]fp", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***fp";
                Match DOWN_LEFT = Regex.Match(doublePown, @"pf[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pf***pp";
                Match DOWN_RIGHT = Regex.Match(doublePown, @"fp[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "fp***pp";

                Match UP_LEFT_n = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pn", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pn";
                Match UP_RIGHT_n = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]np", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***np";
                Match DOWN_LEFT_n = Regex.Match(doublePown, @"pn[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pn***pp";
                Match DOWN_RIGHT_n = Regex.Match(doublePown, @"np[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "np***pp";

                Match UP_LEFT_e = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pe", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pe";
                Match UP_RIGHT_e = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]ep", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***ep";
                Match DOWN_LEFT_e = Regex.Match(doublePown, @"pe[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pe***pp";
                Match DOWN_RIGHT_e = Regex.Match(doublePown, @"ep[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "ep***pp";

                Match UP_LEFT_u = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pu", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pu";
                Match UP_RIGHT_u = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]up", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***up";
                Match DOWN_LEFT_u = Regex.Match(doublePown, @"pu[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pu***pp";
                Match DOWN_RIGHT_u = Regex.Match(doublePown, @"up[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "up***pp";

                if (UP_LEFT.Success || UP_RIGHT.Success || DOWN_LEFT.Success || DOWN_RIGHT.Success || UP_LEFT_n.Success || UP_RIGHT_n.Success || DOWN_LEFT_n.Success || DOWN_RIGHT_n.Success || UP_LEFT_e.Success || UP_RIGHT_e.Success || DOWN_LEFT_e.Success || DOWN_RIGHT_e.Success || UP_LEFT_u.Success || UP_RIGHT_u.Success || DOWN_LEFT_u.Success || DOWN_RIGHT_u.Success)
                {
                    canFix = true;
                    //Console.Out.WriteLine("isFixable");
                }
                else
                {
                    canFix = false;
                    //Console.Out.WriteLine("Not isFixable");
                }


                // if (UP_LEFT.Success) { Console.Out.WriteLine("canFix up left match"); }//end
                // if (UP_RIGHT.Success) { Console.Out.WriteLine("canFix up right match"); }//end
                // // if (DOWN_LEFT.Success) { Console.Out.WriteLine("canFix down left match"); }//end
                // if (DOWN_RIGHT.Success) { Console.Out.WriteLine("canFix down right match"); }//end
                //if (UP_LEFT_n.Success) { Console.Out.WriteLine("canFix_n up left match"); }//end
                // if (UP_RIGHT_n.Success) { Console.Out.WriteLine("canFix_n up right match"); }//end
                // if (DOWN_LEFT_n.Success) { Console.Out.WriteLine("canFix_n down left match"); }//end
                // if (DOWN_RIGHT_n.Success) { Console.Out.WriteLine("canFix_n down right match"); }//end
            }//end if more than five passed
            else
            {
                canFix = false;
//                Console.Out.WriteLine("Not isFixable");
            }
            pown = origPown;
            return canFix;
        }//end is fixable


        public void SetAnsToPans()
        {
            for (int i = 0; (i < Config.NodeCount); i++)
            {
                this.pan[i] = an[i];
            }// end for 25 ans
        }// end setAnsToPans

        public void SetAnsToPansIfPassed(bool partial = false)
        {
            // now set all ans that passed to the new pans
            char[] pownArray = pown.ToCharArray();

            for (int i = 0; (i < Config.NodeCount); i++)
            {
                if (pownArray[i] == 'p')//1 means pass
                {
                    an[i] = pan[i];
                }
                else if (pownArray[i] == 'u' && !(RAIDA.GetInstance().nodes[i].RAIDANodeStatus == NodeStatus.NotReady) && partial == false)//Timed out but there server echoed. So it probably changed the PAN just too slow of a response
                {
                    an[i] = pan[i];
                }
                else
                {
                    // Just keep the ans and do not change. Hopefully they are not fracked. 
                }
            }// for each guid in coin
        }// end set ans to pans if passed

        public void CalculateHP()
        {
            hp = Config.NodeCount;
            char[] pownArray = pown.ToCharArray();
            for (int i = 0; (i < Config.NodeCount); i++)
            {
                if (pownArray[i] == 'f')
                {
                    this.hp--;
                }
            }
        }//end calculate hp


        public bool setPastStatus(string status, int raida_id)
        {
            char[] pownArray = this.pown.ToCharArray();
            switch (status)
            {
                case "error": pownArray[raida_id] = 'e'; break;
                case "fail": pownArray[raida_id] = 'f'; break;
                case "pass": pownArray[raida_id] = 'p'; break;
                case "undetected": pownArray[raida_id] = 'u'; break;
                case "noresponse": pownArray[raida_id] = 'n'; break;
            }//end switch
            this.pown = new string(pownArray);
            return true;
        }//end set past status

        public void doPostProcessing()
        {
            setPastStatus();
            SetAnsToPansIfPassed();
            CalculateHP();
            CalcExpirationDate();
            //grade();
        }
        public bool setPastStatus()
        {
            char[] pownArray = pown.ToCharArray();
            for (int i = 0; i < Config.NodeCount; i++)
            {
                if (response[i] != null)
                {
                    string status = response[i].outcome;
                    switch (status)
                    {
                        case "error": pownArray[i] = 'e'; break;
                        case "fail": pownArray[i] = 'f'; break;
                        case "pass": pownArray[i] = 'p'; break;
                        case "undetected": pownArray[i] = 'u'; break;
                        case "noresponse": pownArray[i] = 'n'; break;
                    }//end switch
                }
                else
                {
                    pownArray[i] = 'u';
                };// should be pass, fail, error or undetected. 
            }//end for each detection agent


            this.pown = new string(pownArray);
            return true;
        }//end set past status

        public String generatePan()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] cryptoRandomBuffer = new byte[16];
                rng.GetBytes(cryptoRandomBuffer);

                Guid pan = new Guid(cryptoRandomBuffer);
                String rawpan = pan.ToString("N");
                String fullPan = "";
                switch (rawpan.Length)//Make sure the pan is 32 characters long. The odds of this happening are slim but it will happen.
                {
                    case 27: fullPan = ("00000" + rawpan); break;
                    case 28: fullPan = ("0000" + rawpan); break;
                    case 29: fullPan = ("000" + rawpan); break;
                    case 30: fullPan = ("00" + rawpan); break;
                    case 31: fullPan = ("0" + rawpan); break;
                    case 32: fullPan = rawpan; break;
                    case 33: fullPan = rawpan.Substring(0, rawpan.Length - 1); break;//trim one off end
                    case 34: fullPan = rawpan.Substring(0, rawpan.Length - 2); break;//trim one off end
                }

                return fullPan;
            }
        }

        public void recordPown()
        {
            //records the last pown so we can see if there are improvments
            pastPown = pown;
        }//end record pown

        public void SortToFolder()
        {
            //figures out which folder to put it in. 
            //pown = pown.Replace('d', 'e').Replace('n', 'e').Replace('u', 'e');
            //pown = pown.Replace('n','e');
            //pown = pown.Replace('u', 'e');
            if (isPerfect())
            {
                folder =  RAIDA.ActiveRAIDA.FS.BankFolder;
                //folder = Folder.Bank;
                return;
            }//if is perfect

            if (isCounterfeit())
            {
                folder = RAIDA.ActiveRAIDA.FS.CounterfeitFolder;
                //folder = Folder.Counterfeit;
                return;
            }//if is counterfeit

            //--------------------------------------
            /*Now look  at fracked coins*/

            if (isGradablePass())
            {
                if (!isFracked())
                {
                    folder = RAIDA.ActiveRAIDA.FS.BankFolder;
                    return;
                }
                else
                {
                    if (isDangerous())
                    {
                        if (isFixable())
                        {
                            recordPown();
                            folder = RAIDA.ActiveRAIDA.FS.DangerousFolder;
                            return;

                        }
                        else
                        {
                            folder = RAIDA.ActiveRAIDA.FS.CounterfeitFolder;
                            return;
                        }
                    }
                    else
                    {
                        if (!isFixable())
                        {
                            folder = RAIDA.ActiveRAIDA.FS.CounterfeitFolder;
                            return;
                        }
                        else
                        {
                            folder = RAIDA.ActiveRAIDA.FS.FrackedFolder;
                            return;
                        }
                    }
                }
            }
            else
            {
                if (noResponses())
                {
                    folder = RAIDA.ActiveRAIDA.FS.LostFolder;
                    //folder = Folder.Lost;
                    return;
                }//end no responses
                else
                {
                    folder = RAIDA.ActiveRAIDA.FS.SuspectFolder;
                    //folder = Folder.Lost;
                    return;
                }
            }
        }//end sort folder
        public bool noResponses()
        {
            //Does the coin have no-responses from the RIDA. This means the RAIDA may be using its PAN or AN
            //These must be fixed in a special way using both.  
            bool returnTruth = false;
            if (charCount(pown, 'n') > 0)
            {
                returnTruth = true;
            }
            return returnTruth;
        }//end is fracked


        public bool isGradablePass()
        {
            //The coin is considered ungradable if it does not get more than 19 RAIDA available
            bool returnTruth = false;
            if (charCount(pown, 'f') + charCount(pown, 'p') > 16 && isFixable())
            {
                returnTruth = true;
                //Console.Out.WriteLine("isGradable");
            }
            else
            {
                //Console.Out.WriteLine("Not isGradable");
            }
            return returnTruth;
        }//end is gradable pass


        public String[] grade()
        {
            int total = Config.NodeCount;

            int passed = response.Where(x => x.outcome == "pass").Count();
            int failed = response.Where(x => x.outcome == "fail").Count();
            int other = total - passed - failed;

            if (passed > Config.PassCount)
            {
                DetectResult = DetectionStatus.Passed;
            }
            else
            {
                DetectResult = DetectionStatus.Failed;
            }

            String passedDesc = "";
            String failedDesc = "";
            String otherDesc = "";

            // for each status
            // Calculate passed
            if (passed == 25)
            {
                passedDesc = "100% Passed!";
            }
            else if (passed > 17)
            {
                passedDesc = "Super Majority";
            }
            else if (passed > 13)
            {
                passedDesc = "Majority";
            }
            else if (passed == 0)
            {
                passedDesc = "None";
            }
            else if (passed < 5)
            {
                passedDesc = "Super Minority";
            }
            else
            {
                passedDesc = "Minority";
            }

            // Calculate failed
            if (failed == 25)
            {
                failedDesc = "100% Failed!";
            }
            else if (failed > 17)
            {
                failedDesc = "Super Majority";
            }
            else if (failed > 13)
            {
                failedDesc = "Majority";
            }
            else if (failed == 0)
            {
                failedDesc = "None";
            }
            else if (failed < 5)
            {
                failedDesc = "Super Minority";
            }
            else
            {
                failedDesc = "Minority";
            }

            // Calcualte Other RAIDA Servers did not help. 
            switch (other)
            {
                case 0:
                    otherDesc = "100% of RAIDA responded";
                    break;
                case 1:
                case 2:
                    otherDesc = "Two or less RAIDA errors";
                    break;
                case 3:
                case 4:
                    otherDesc = "Four or less RAIDA errors";
                    break;
                case 5:
                case 6:
                    otherDesc = "Six or less RAIDA errors";
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    otherDesc = "Between 7 and 12 RAIDA errors";
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    otherDesc = "RAIDA total failure";
                    break;
                default:
                    otherDesc = "FAILED TO EVALUATE RAIDA HEALTH";
                    break;
            }
            // end RAIDA other errors and unknowns
            // Coin will go to bank, counterfeit or fracked
            if (other > 12)
            {
                // not enough RAIDA to have a quorum
                folder = RAIDA.GetInstance().FS.SuspectFolder;
            }
            else if (failed > passed)
            {
                // failed out numbers passed with a quorum: Counterfeit
                folder = RAIDA.GetInstance().FS.CounterfeitFolder;
            }
            else if (failed > 0)
            {
                // The quorum majority said the coin passed but some disagreed: fracked. 
                folder = RAIDA.GetInstance().FS.FrackedFolder;
            }
            else
            {
                // No fails, all passes: bank
                folder = RAIDA.GetInstance().FS.BankFolder;

            }

            gradeStatus[0] = passedDesc;
            gradeStatus[1] = failedDesc;
            gradeStatus[2] = otherDesc;
            return this.gradeStatus;
        }// end gradeStatus

        public void CalcExpirationDate()
        {
            DateTime expirationDate = DateTime.Today.AddYears(Config.YEARSTILEXPIRE);
            ed = (expirationDate.Month + "-" + expirationDate.Year);
            //  Console.WriteLine("ed = " + cc.ed);
            DateTime zeroDate = new DateTime(2016, 08, 13);
            // DateTime zeroDate = DateTime.Parse("8/13/2016 8:33:21 AM");
            int monthsAfterZero = (int)(expirationDate.Subtract(zeroDate).Days / (365.25 / 12));
            //Turn positive and up to down to floor
            // Console.WriteLine("Months after zero = " + monthsAfterZero);
            this.edHex = monthsAfterZero.ToString("X2");
        }// end calc exp date

        public bool containsThreat()
        {
            bool threat = false;
            string doublePown = pown + pown;
            //There are four threat patterns that would allow attackers to seize other 
            //String UP_LEFT = "ff***f";
            //String UP_RIGHT = "ff***pf";
            //String DOWN_LEFT = "fp***ff";
            //String DOWN_RIGHT = "pf***ff";


            Match UP_LEFT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]fp", RegexOptions.IgnoreCase);
            Match UP_RIGHT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]pf", RegexOptions.IgnoreCase);
            Match DOWN_LEFT = Regex.Match(doublePown, @"fp[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);
            Match DOWN_RIGHT = Regex.Match(doublePown, @"pf[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);

            //Check if 
            if (UP_LEFT.Success || UP_RIGHT.Success || DOWN_LEFT.Success || DOWN_RIGHT.Success)
            {
                threat = true;
            }//end if coin contains threats.


            return threat;
        }//End Contains Threat
    }
    public enum DetectionStatus
    {
        Passed,
        Failed,
        Other
    }
    public struct DetectionResult
    {
        public DetectionStatus Result;
        public int PassCount;
        public int FailCount;
        public int OtherCount;

        public int Description;
    }

