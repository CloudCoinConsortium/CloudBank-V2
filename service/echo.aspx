<%@ Page Language="C#" Debug="true"  Async="true"%>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System" %>
<%@ Import namespace="System.Data.SqlClient" %>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Threading.Tasks" %>

<script language="c#" runat="server">
    /*
      echo.aspx
      Version 10/5/2017
      Created by Navraj Singh
      Get's the bank's welcome information
      Usage: https://cloudcoin.global/bank/echo.aspx
      This software is for use by CloudCoin Consortium Clients only. 
      All rights reserved.  
      
      SAMPLE RESPONSE READY
      {
        "bank_server":"CloudCoin.co",
        "status":"ready",
        "message":"The RAIDA is ready for counterfeit detection.",
        "time":"2016-49-21 7:49:PM"
        }
        
       SAMPLE RESPONSE IF No READY 
        {
            "bank_server":"CloudCoin.co",
            "status":"fail",
            "message":"Not enough RAIDA servers can be contacted to import new coins.",
            "time":"2016-49-21 7:49:PM"
       }
      */
    
	      //Usage: http://bank.CloudCoin.Global/service/echo?account=CloudCoin@Protonmail.com&pk=640322f6d30c45328914b441ac0f4e5b

    //Response.Write("here");

	
    public void Page_Load(object sender, EventArgs e)
    {
		ServiceResponse serviceResponse = new ServiceResponse();
        serviceResponse.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
        serviceResponse.status = "fail";
        serviceResponse.time = DateTime.UtcNow.ToString("o");
		serviceResponse.version = "2.0";

		
		//Check to see if the Account matches the password:
	
		string path = Request.QueryString["pk"];
		string account = Request.QueryString["account"];
		//Response.Write(account);
         //   Response.End();
        if (path == null || account == null)
        {
            serviceResponse.message = "Request Error: Private key or Account not specified";
            var json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }

		string PasswordFolder = WebConfigurationManager.AppSettings["PasswordFolder"];
		string text = System.IO.File.ReadAllText( Server.MapPath("..") + @"\accounts\" + PasswordFolder + @"\" + account + ".txt");
		
		
        if ( text != path )
        {
            serviceResponse.status = "fail";
            serviceResponse.message = "Private key not correct";
            var serialjson = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(serialjson);
            Response.End();
        }
		
	//End of checking passwords
			

        RegisterAsyncTask(new PageAsyncTask(EchoRaida));
    }//End Page Load
	
	public async Task EchoRaida( )
        {
			RAIDA raida = new RAIDA();
			raida = RAIDA.GetInstance();
            var echos = raida.GetEchoTasks();


            await Task.WhenAll(echos.AsParallel().Select(async task => await task()));
			
				ServiceResponse serviceResponse = new ServiceResponse();
				serviceResponse.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
				serviceResponse.status = "ready";
				serviceResponse.time = DateTime.UtcNow.ToString("o");
				serviceResponse.readyCount = raida.ReadyCount;
				serviceResponse.notReadyCount = raida.NotReadyCount;
				serviceResponse.version = "2.0";

			if (raida.ReadyCount > 20){
			//send yay
				serviceResponse.status = "ready";
				serviceResponse.message = "The RAIDA is ready for counterfeit detection.";
			}else{
			//send nay
				serviceResponse.status = "fail";
				serviceResponse.message = "Not enough RAIDA servers can be contacted to import new coins.";
			}
			
			var json = new JavaScriptSerializer().Serialize(serviceResponse);
				Response.Write(json);
				Response.End();  
        }
	
	
	public class ServiceResponse
    {
		public string bank_server;
        public string status;
        public string version;
        public string message;
        public string time;
		public int readyCount;
		public int notReadyCount;

	}//End Service Response class
	
  
</script>