<%@ Page Language="C#" Debug="true"  Async="true"%>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import namespace="System.Data.SqlClient" %>
<%@ Import namespace="System.Web.Configuration" %>

<%@ Import namespace="System.Web.Script.Serialization" %>

<script language="c#" runat="server">
 

      /*
      echo.aspx
      Version 6/22/2018
      Created by Sean Worthington
      Get's the bank's coin count
      Usage: https://cloudcoin.global/service/show_coins
      This software is for use by CloudCoin Consortium Clients only. 
      All rights reserved.  
      
      SAMPLE RESPONSE GOOD
      {
		"bank_server":"Bank.CloudCoin.Global",
		"status":"coins_shown",
		"message":"Coin totals returned.",
		"ones":3,
		"fives":0,
		"twentyfives":0,
		"hundreds":0,
		"twohundredfifties":1,
		"time":"2018-06-23T05:53:39.4155794Z",
		"version":"2.0"
	}
        
       SAMPLE RESPONSE IF BAD 
    {
		 "bank_server":"bank.cloudcoin.global",
		 "account":"CloudCoin@Protonmail.com",
		 "status":"fail",
		 "message":"Private key incorrect"
		 "ones":0,
		 "fives":0,
		 "twentyfives":0,
		 "hundreds":0,
		 "twohundredfifties":0,
		 "time":"2018-06-23T05:53:39.4155794Z",
		 "version":"2.0"
	}
      */


    public void Page_Load(object sender, EventArgs e)
    {
		
		ServiceResponse serviceResponse = new ServiceResponse();
        serviceResponse.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
        serviceResponse.status = "fail";
        serviceResponse.time = DateTime.UtcNow.ToString("o");
		serviceResponse.version = "2.0";
		var json = new JavaScriptSerializer().Serialize(serviceResponse);
		
		//Check to see if the Account matches the password:
	
		string path = Request.QueryString["pk"];
		string account = Request.QueryString["account"];
		
        if (path == null || account == null)
        {
            serviceResponse.message = "Request Error: Private key or Account not specified";
			json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }

		string PasswordFolder = WebConfigurationManager.AppSettings["PasswordFolder"];
		string text = System.IO.File.ReadAllText( Server.MapPath("..") + @"\accounts\" + PasswordFolder + @"\" + account + @".txt");
		serviceResponse.account = account;
		
        if ( text != path )
        {
            serviceResponse.status = "fail";
            serviceResponse.message = "Private key not correct";
            var serialjson = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(serialjson);
            Response.End();
        }
		
	//End of checking passwords

        FileUtils fileUtils = new FileUtils(HttpRuntime.AppDomainAppPath.ToString() + @"\accounts\" + path + @"\");
        //FileUtils fileUtils = FileUtils.GetInstance(@"H:\Banks\Preston\"+path+@"\");


        serviceResponse.status = "coins_shown";
        Banker bank = new Banker(fileUtils);
        int[] bankTotals = bank.countCoins(fileUtils.BankFolder);
        int[] frackedTotals = bank.countCoins(fileUtils.FrackedFolder);
        serviceResponse.ones = bankTotals[1] + frackedTotals[1];
        serviceResponse.fives = bankTotals[2] + frackedTotals[2];
        serviceResponse.twentyfives = bankTotals[3] + frackedTotals[3];
        serviceResponse.hundreds = bankTotals[4] + frackedTotals[4];
        serviceResponse.twohundredfifties = bankTotals[5] + frackedTotals[5];
		serviceResponse.message = "Coin totals returned.";
        json = new JavaScriptSerializer().Serialize(serviceResponse);
        Response.Write(json);
        Response.End();
    }

   public class ServiceResponse
    {
        public string bank_server;
		public string account;
        public string status;
        public string message;
        public int ones;
        public int fives;
        public int twentyfives;
        public int hundreds;
        public int twohundredfifties;
        public string time;
		public string version;
    }//End Service Response class


</script>