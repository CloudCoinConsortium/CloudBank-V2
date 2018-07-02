   <%@ Page Language="C#" Debug="true"  Async="true"%>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System" %>
<%@ Import Namespace="System.IO" %>
<%@ Import namespace="System.Data.SqlClient" %>
<%@ Import namespace="System.Web.Configuration" %>
<%@ Import namespace="System.Web.Script.Serialization" %>

<script language="c#" runat="server">

    //static string path = WebConfigurationManager.AppSettings["root"];
    //static FileUtils fileUtils = new FileUtils( Directory.GetCurrentDirectory()+ path+@"\");
    public class ServiceResponse
    {
        public string bank_server;
        public string status;
        public string message;
        public string time;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        serviceResponse.bank_server = WebConfigurationManager.AppSettings["thisServerName"];
        serviceResponse.status = "fail";
        serviceResponse.time = DateTime.Now.ToString();
        String json = new JavaScriptSerializer().Serialize(serviceResponse);
			//Check to see if the Account matches the password:
	
		string rn = Request.QueryString["rn"];
		string account = Request.QueryString["account"];
		
        if ( rn == null || account == null)
        {
            serviceResponse.message = "Request Error: Receipt Number or Account ID not specified";
			json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }

		string PasswordFolder = WebConfigurationManager.AppSettings["PasswordFolder"];
		
		
		//check if file exists
		if (System.IO.File.Exists( Server.MapPath("..") + @"\accounts\" + PasswordFolder + @"\" + account + @".txt")){
		
		string path = System.IO.File.ReadAllText( Server.MapPath("..") + @"\accounts\" + PasswordFolder + @"\" + account + @".txt");
		
	//End of checking passwords	
		
        if (path == null)
        {
            serviceResponse.message = "Request Error: Private key not specified";
            json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }

   

        FileUtils fileUtils = new FileUtils( Directory.GetCurrentDirectory()+ path+@"\");

        string id = Request["rn"];
        string FileLocation = AppDomain.CurrentDomain.BaseDirectory + @"\accounts\" + path + @"\Receipts\" + id + ".json";

        if(id == null)
        {
            serviceResponse.message = "Error: No Receipt Id in Request.";
            json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }else if(!File.Exists(FileLocation))
        {
            serviceResponse.message = "Error: File not Found " + AppDomain.CurrentDomain.BaseDirectory + @"\" + path+@"\Receipts\"+ id + ".json";
            json = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(json);
            Response.End();
        }
        else
        {
            json = "";
            using (StreamReader sr = File.OpenText(FileLocation))
            {

                while (!sr.EndOfStream)
                {
                    json += sr.ReadLine();
                }
            }
            Response.Write(json);
            Response.End();
        }
		}//if file doesnt exist  
		else{
		
			serviceResponse.status = "fail";
            serviceResponse.message = "Account ID not found";
            var serialjson = new JavaScriptSerializer().Serialize(serviceResponse);
            Response.Write(serialjson);
            Response.End();
		
		}
		
		
		
    }
</script>
