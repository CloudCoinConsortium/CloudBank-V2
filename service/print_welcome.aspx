<%@ Page Language="C#" Debug="false"  %>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System.Web.Script.Serialization" %>

<SCRIPT LANGUAGE="c#" RUNAT="server">

    /*
      print_welcome.aspx
      Version 6/22/2018
      Created by Sean Worthington
      Get's the bank's welcome information
      Usage: https://cloudcoin.global/bank/print_welcome
      This software is for use by CloudCoin Consortium Clients only. 
      All rights reserved.  
      */
	
	//Usage: http://192.168.1.4/service/print_welcome
	
    public void Page_Load(object sender, EventArgs e)
    {
        ServiceResponse response = new ServiceResponse();
		
        response.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
        response.status = "welcome";
        response.version = "2.0";
        response.time = DateTime.Now.ToString("o");
        response.message = "CloudCoin Bank. Used to Authenticate, Store and Payout CloudCoins." +
                "This Software is provided as is with all faults, defects and errors, and without warranty of any kind." +
                "Free from the CloudCoin Consortium.";

        var json = new JavaScriptSerializer().Serialize(response);
        Response.Write(json);
        Response.End();
    }//End Page Load
	
	
		  
    public class ServiceResponse
    {
        public string bank_server;
        public string status;
        public string version;
        public string message;
        public string time;
    }// end class Service Response


</SCRIPT>
