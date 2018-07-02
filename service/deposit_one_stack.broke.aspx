<%@ Page Language="C#" Debug="true"  Async="true"%>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Data.SqlClient" %>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="System.Threading.Tasks" %>


<script language="c#" runat="server">
	int NetworkNumber = 1;


	//FileUtils fileUtils = new FileUtils(AppDomain.CurrentDomain.BaseDirectory);
    //static FileUtils fileUtils = FileUtils.GetInstance(AppDomain.CurrentDomain.BaseDirectory);

    public void Page_Load(object sender, EventArgs e)
    {
		
		ServiceResponse serviceResponse = new ServiceResponse();
        serviceResponse.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
        serviceResponse.status = "fail";
        serviceResponse.time = DateTime.UtcNow.ToString("o");
		serviceResponse.version = "2.0";
		var json = new JavaScriptSerializer().Serialize(serviceResponse);
		
		//Check to see if the Account matches the password:
	
		string account = Request["account"];
		
        if (account == null)
        {
            serviceResponse.message = "Request Error: Account not specified";
			json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }

		//Read what folder that account is located in
		string PasswordFolder = WebConfigurationManager.AppSettings["PasswordFolder"];
		string path = System.IO.File.ReadAllText( Server.MapPath("..") + @"\" + PasswordFolder + @"\" + account + @".txt");
		serviceResponse.account = account;
		
	//End of checking passwords

        FileUtils fileUtils = new FileUtils(HttpRuntime.AppDomainAppPath.ToString() + @"\" + path + @"\");
        //FileUtils fileUtils = FileUtils.GetInstance(@"H:\Banks\Preston\"+path+@"\");

		 
		//fileUtils.CreateFolderStructure();
		SetupRAIDA(path, fileUtils);
		FileUtils FS = new FileUtils(HttpRuntime.AppDomainAppPath.ToString() + @"\" + path + @"\");
        
		//Read Stack file from post in the form of a string
		string stack = Request.Form["stack"];
		//Write stack to imput folder
		System.IO.File.WriteAllText (Server.MapPath("..") + @"\" + path + @"\Import\" + @"new.stack", stack);
		 //Response.Write(Server.MapPath("..") + @"\" + path + @"\Import\" + @"new.stack");
	//	 Response.Write(Server.MapPath("..") + @"\" + path + @"\Import\" + @"new.stack");
		//Response.End();
       // Importer importer = new Importer(fileUtils);

		FS.LoadFileSystem();
		//RAIDA raida = RAIDA.GetInstance();
		//RAIDA.Instantiate();
		
        bool import = false;
        if (stack != null)
			await RAIDA.ProcessCoins(true);
           // import = importer.importJson(stack);
        if (!import)//Moves all CloudCoins from the Import folder into the Suspect folder. 
        {
           // RegisterAsyncTask(new PageAsyncTask(detect));
            ServiceResponse response = new ServiceResponse();
            response.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
            response.status = "Error";
            response.message = "The CloudCoin stack was either empty or the JSON was not valid.";
            response.time = DateTime.UtcNow.ToString("o");
            response.receipt = "";
            json = new JavaScriptSerializer().Serialize(response);
            Response.Write(json);
            Response.End();

        }
        else
        {
           // RegisterAsyncTask(new PageAsyncTask(detect));

        }//end if coins to import

        
    }//End Page Load

	
	
	
        public void SetupRAIDA(string path, FileUtils fileUtils)
        {
            RAIDA.FileSystem = new FileUtils(HttpRuntime.AppDomainAppPath.ToString() + @"\" + path + @"\");
            try
            {
                RAIDA.Instantiate();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            if (RAIDA.networks.Count == 0)
            {
                //updateLog("No Valid Network found.Quitting!!");
                Environment.Exit(1);
            }
            else
            {
              //  updateLog(RAIDA.networks.Count + " Networks found.");
			  RAIDA raida;
                raida = (from x in RAIDA.networks
                         where x.NetworkNumber == NetworkNumber
                         select x).FirstOrDefault();
                raida.FS = fileUtils;
                RAIDA.ActiveRAIDA = raida;
                if (raida == null)
                {
                   // updateLog("Selected Network Number not found. Quitting.");
                    Environment.Exit(0);
                }
                else
                {
                   // updateLog("Network Number set to " + NetworkNumber);
                }
            }
            //networks[0]
        }
	
	public class ServiceResponse
    {
        public string bank_server;
		public string account;
        public string status;
		public string message;
        public string receipt;
        public string time;
		public string version;
    }//End Service Response class

	
	/*
    private async Task detect()
    {
        string receiptFileName = await multi_detect();
        ServiceResponse response = new ServiceResponse();
        response.server = WebConfigurationManager.AppSettings["thisServerName"];
        response.receipt = receiptFileName;
        response.status = "importing";
        response.message = "The stack file has been imported and detection will begin automatically so long as they are not already in bank. Please check your reciept.";
        response.time = DateTime.Now.ToString("yyyy-MM-dd h:mm:tt");
     //   Grader grader = new Grader(fileUtils);
       // int[] detectionResults = grader.gradeAll(5000, 2000, receiptFileName);
        var json = new JavaScriptSerializer().Serialize(response);
        Response.Write(json);
        Response.End();
    }

    public static async Task<string> multi_detect() {

       MultiDetect multi_detector = new MultiDetect(fileUtils);
        string receiptFileName;
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] cryptoRandomBuffer = new byte[16];
            rng.GetBytes(cryptoRandomBuffer);

            Guid pan = new Guid(cryptoRandomBuffer);
            receiptFileName = pan.ToString("N");
        }

        //Calculate timeout
        int detectTime = 20000;
        if (RAIDA_Status.getLowest21() > detectTime)
        {
            detectTime = RAIDA_Status.getLowest21() + 200;
        }//Slow connection

        await multi_detector.detectMulti(detectTime, receiptFileName);
        return receiptFileName;
    }//end multi detect
*/
</script>