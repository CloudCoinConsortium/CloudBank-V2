<%@ Page Language="C#" Debug="true"  Async="true"%>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Data.SqlClient" %>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading.Tasks" %>
<%@ Import Namespace="System.IO" %>


<script language="c#" runat="server">
    class ServiceResponse
    {
        public string server;
        public string status;
        public string message;
        public string time;
    }//End Service Response class

    string CheckParameter(string param )
    {
        if (Request[param] != null)
            return Request[param];
        else
            return "";
    }

    private string Ones;
    private string Fives;
    private string TwentyFives;
    private string Hundreds;
    private string TwoHundredFifties;

    int iOnes;
    int iFives;
    int iTwentyFives;
    int iHundreds;
    int iTwoHundredFifties;

    int OnesToMark;
    int FivesToMark;
    int TwentyFivesToMark;
    int HundredsToMark;
    int TwoHundredFiftiesToMark;

    int OnesAvailableToMark;
    int FivesAvailableToMark;
    int TwentyFivesAvailableToMark;
    int HundredsAvailableToMark;
    int TwoHundredFiftiesAvailableToMark;

    private int BankOnes;
    private int BankFives;
    private int BankTwentyFives;
    private int BankHundreds;
    private int BankTwoHundredFifties;

    private int MarkedOnes;
    private int MarkedFives;
    private int MarkedTwentyFives;
    private int MarkedHundreds;
    private int MarkedTwoHundredFifties;


    public void Page_Load(object sender, EventArgs e)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        serviceResponse.server = WebConfigurationManager.AppSettings["thisServerName"];
        serviceResponse.time = DateTime.Now.ToString();

        string path = Page.Request["pk"];
        if (path == null)
        {
            serviceResponse.status = "fail";
            serviceResponse.message = "Request Error: Private key not specified";
            var json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }
		string account = Request["account"];
		string pk = Request["pk"];
		string PasswordFolder = WebConfigurationManager.AppSettings["PasswordFolder"];
		string text = System.IO.File.ReadAllText( Server.MapPath("..") + @"\accounts\" + PasswordFolder + @"\" + account + @".txt");
		
        if (pk != text )
        {
            serviceResponse.status = "fail";
            serviceResponse.message = "Private key not correct";
            var serialjson = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(serialjson);
            Response.End();
        }

        Ones = CheckParameter("ones");
        Fives = CheckParameter("fives");
        TwentyFives = CheckParameter("twentyfives");
        Hundreds = CheckParameter("hundreds");
        TwoHundredFifties = CheckParameter("twohundredfifties");

        if (Ones == "" && Fives == "" && TwentyFives == "" && Hundreds == "" && TwoHundredFifties == "")
        {
            serviceResponse.status = "fail";
            serviceResponse.message = "No coins to mark provided";
            var serialjson = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(serialjson);
            Response.End();
        }


        int x;
        if (int.TryParse(Ones, out x))
        {
            iOnes = x;
        }
        if (int.TryParse(Fives, out x))
        {
            iFives = x;
        }
        if (int.TryParse(TwentyFives, out x))
        {
            iTwentyFives = x;
        }
        if (int.TryParse(Hundreds, out x))
        {
            iHundreds = x;
        }
        if (int.TryParse(TwoHundredFifties, out x))
        {
            iTwoHundredFifties = x;
        }

        GetBankTotals();
        GetCoinsCurrentlyMarkedForSale();
        GetAvailableCoins();

        if (Ones != "")
        {
            if (iOnes < MarkedOnes)
            {
                UnMarkCoins("1", MarkedOnes - iOnes);
            }
            else
            {
                int toBeMarked = iOnes - MarkedOnes;
                if (toBeMarked > 0)
                {
                    if (toBeMarked <= OnesAvailableToMark)
                    {
                        MarkCoins("1", toBeMarked);
                    }
                    else
                    {
                        MarkCoins("1", OnesAvailableToMark);
                    }
                }
            }
        }

        if (Fives != "")
        {
            if (iFives < MarkedFives)
            {
                UnMarkCoins("5", MarkedFives - iFives);
            }
            else
            {
                int toBeMarked = iFives - MarkedFives;
                if (toBeMarked > 0)
                {
                    if (toBeMarked <= FivesAvailableToMark)
                    {
                        MarkCoins("5", toBeMarked);
                    }
                    else
                    {
                        MarkCoins("5", FivesAvailableToMark);
                    }
                }
            }
        }

        if (TwentyFives != "")
        {
            if (iTwentyFives < MarkedTwentyFives)
            {
                UnMarkCoins("25", MarkedTwentyFives - iTwentyFives);
            }
            else
            {
                int toBeMarked = iTwentyFives - MarkedTwentyFives;
                if (toBeMarked > 0)
                {
                    if (toBeMarked <= TwentyFivesAvailableToMark)
                    {
                        MarkCoins("25", toBeMarked);
                    }
                    else
                    {
                        MarkCoins("25", OnesAvailableToMark);
                    }
                }
            }
        }

        if (Hundreds != "")
        {
            if (iHundreds < MarkedHundreds)
            {
                UnMarkCoins("100", MarkedHundreds - iHundreds);
            }
            else
            {
                int toBeMarked = iHundreds - MarkedHundreds;
                if (toBeMarked > 0)
                {
                    if (toBeMarked <= HundredsAvailableToMark)
                    {
                        MarkCoins("100", toBeMarked);
                    }
                    else
                    {
                        MarkCoins("100", HundredsAvailableToMark);
                    }
                }
            }
        }

        if (TwoHundredFifties != "")
        {
            if (iTwoHundredFifties < MarkedTwoHundredFifties)
            {
                UnMarkCoins("250", MarkedTwoHundredFifties - iTwoHundredFifties);
            }
            else
            {
                int toBeMarked = iTwoHundredFifties - MarkedTwoHundredFifties;
                if (toBeMarked > 0)
                {
                    if (toBeMarked <= TwoHundredFiftiesAvailableToMark)
                    {
                        MarkCoins("250", toBeMarked);
                    }
                    else
                    {
                        MarkCoins("250", TwoHundredFiftiesAvailableToMark);
                    }
                }
            }
        }



        serviceResponse.status = "success";
        serviceResponse.message = "Coins Marked for Sale";
        var serialjson2 = new JavaScriptSerializer().Serialize(serviceResponse);
        Response.Write(serialjson2);
        Response.End();
    }

    void GetBankTotals()
    {
        string path = Request["pk"];
        FileUtils fileUtils = new FileUtils(HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\");
        Banker bank = new Banker(fileUtils);
        int[] bankTotals = bank.countCoins(fileUtils.BankFolder);
        BankOnes = bankTotals[1];
        BankFives = bankTotals[2];
        BankTwentyFives = bankTotals[3];
        BankHundreds = bankTotals[4];
        BankTwoHundredFifties = bankTotals[5];
    }

    void GetCoinsCurrentlyMarkedForSale()
    {
        string path = Request["pk"];
        string dir = HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\bank\";

        string filename;

        string[] files = Directory.GetFiles(dir);
        foreach (string file in files)
        {
            filename = Path.GetFileName(file);
            if (filename.Contains("forsale"))
            {
                int index = filename.IndexOf(".");
                string sub = filename.Substring(0, index);
                if (sub == "1")
                    MarkedOnes = MarkedOnes + 1;
                if (sub == "5")
                    MarkedFives = MarkedFives + 1;
                if (sub == "25")
                    MarkedTwentyFives = MarkedTwentyFives + 1;
                if (sub == "100")
                    MarkedHundreds = MarkedHundreds + 1;
                if (sub == "250")
                    MarkedTwoHundredFifties = MarkedTwoHundredFifties + 1;
            }
        }
    }

    void GetAvailableCoins()
    {
        OnesAvailableToMark = BankOnes - MarkedOnes;
        FivesAvailableToMark = BankFives - MarkedFives;
        TwentyFivesAvailableToMark = BankTwentyFives - MarkedTwentyFives;
        HundredsAvailableToMark = BankHundreds - MarkedHundreds;
        TwoHundredFiftiesAvailableToMark = BankTwoHundredFifties - MarkedTwoHundredFifties;
    }

    Boolean MarkCoins(string denomination, int number)
    {
        int x = number;

        string path = Request["pk"];
        string dir = HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\bank\";

        string filename;

        string[] files = Directory.GetFiles(dir);
        foreach (string file in files)
        {
            filename = Path.GetFileName(file);
            if (x > 0 && !filename.Contains("forsale"))
            {
                int index = filename.IndexOf(".");
                string sub = filename.Substring(0, index);
                if (sub == denomination)
                {
                    string newfilename = HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\bank\" + filename.Insert(filename.LastIndexOf("."),"forsale");
                    System.IO.File.Move(file, newfilename);
                    x = x - 1;
                }
            }
        }

        return true;
    }

    Boolean UnMarkCoins(string denomination, int number)
    {
        int x = number;

        string path =  Request["pk"];
        string dir = HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\bank\";

        string filename;

        string[] files = Directory.GetFiles(dir);
        foreach (string file in files)
        {
            filename = Path.GetFileName(file);
            if (filename.Contains("forsale") && x > 0)
            {
                int index = filename.IndexOf(".");
                string sub = filename.Substring(0, index);
                if (sub == denomination)
                {
                    string newfilename = HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\bank\" + filename.Replace("forsale", "");
                    System.IO.File.Move(file, newfilename);
                    x = x - 1;
                }
            }
        }

        return true;
    }


</script>
