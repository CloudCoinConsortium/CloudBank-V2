<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GreenPayOrder.aspx.cs" Async="true" Inherits="CloudService.GreenPayOrder" %>

<!DOCTYPE html>
<link href="GPOrder.css" rel="stylesheet" />
<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.12/css/all.css" integrity="sha384-G0fIWCsCzJIMAVNQPfjH08cyYaUtMwjJwqiRKxxE/rx96Uroj1BtIQ6MLJuheaO9" crossorigin="anonymous">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div class="row">

        <div class="col-50">
            <div id="PurchaseInfo" class="container">
                <div id="PurchaseInfoHeader"><i style="float: left;padding: 6px;" class="fa fa-shopping-cart"></i><h2>Your Purchase Information</h2></div>
                <p><span class="PIText">Ones: </span><asp:Label ID="lblOnes" class=price runat="server" Text="1"></asp:Label></p>
                <p><span class="PIText">Fives: </span><asp:Label ID="lblFives" class=price runat="server" Text="1"></asp:Label></p>
                <p><span class="PIText">Twenty Fives: </span><asp:Label ID="lblTwentyFives" class=price runat="server" Text="1"></asp:Label></p>
                <p><span class="PIText">Hundreds: </span><asp:Label ID="lblHundreds" class=price runat="server" Text="1"></asp:Label></p>
                <p><span class="PIText">TwoHundredFifties: </span><asp:Label ID="lblTwoHundredFifties" class=price runat="server" Text="1"></asp:Label></p>
                <hr />
                <p><span class="PIText">Total Coins: </span><asp:Label ID="lblTotalCoins" style="color:black" class=price runat="server" Text="0"></asp:Label></p>
                <p><span class="totalText">Total Price: </span><asp:Label ID="lblTotalPrice" class="total" runat="server" Text="0"></asp:Label></p>
            </div>
            <div>
                <br />
            </div>
            <div id="PaymentInfo" class="container">
                <div id="PaymentInfoHeader"><h2>Your Payment Information:</h2></div>
                <div class="row">
                    <asp:ValidationSummary class="valtext" runat="server" headertext="There were errors on the page:" />
                </div>
                <div class="row">
                <div class="row">
                    <div class="col-50">
                    <label for="txtNameOnCheck"><i class="fa fa-user"></i> Name on check </label>
                    <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtNameOnCheck" errormessage="Name on check is required">*</asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtNameOnCheck" placeholder="John M. Doe" CssClass="inputtext" runat="server" Text=""></asp:TextBox>
                    <label for="txtEmailAddress"><i class="fa fa-envelope"></i> E-Mail Address </label>
                    <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtEmailAddress" errormessage="E-Mail Address is required">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" class="valtext" display="Dynamic" controltovalidate="txtEmailAddress" errormessage="E-mail incorrectly formatted" validationexpression="^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$" >*</asp:RegularExpressionValidator>
                    <asp:TextBox ID="txtEmailAddress" placeholder="email@address.com" CssClass="inputtext" runat="server" Text=""></asp:TextBox>
                    <label for="txtPhoneNumber"><i class="fa fa-phone"></i> Phone Number </label>
                    <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtPhoneNumber" errormessage="Phone Number is required">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" class="valtext" display="Dynamic" controltovalidate="txtPhoneNumber" errormessage="Phone Number incorrectly formatted, use xxx-xxx-xxxx" validationexpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}" >*</asp:RegularExpressionValidator>
                    <asp:TextBox ID="txtPhoneNumber" placeholder="999-999-9999" cssclass="inputtext" runat="server" Text=""></asp:TextBox>
                    <label for="txtAddress"><i class="fa fa-address-card"></i> Address on Check </label>
                    <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtAddress" errormessage="Address on check is required">*</asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtAddress" placeholder="123 Main Street" cssclass="inputtext" runat="server" Text=""></asp:TextBox>
                    <label for="txtCity"><i class="fa fa-building"></i> City on Check </label>
                    <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtCity" errormessage="City on check is required">*</asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtCity" placeholder="City" cssclass="inputtext" runat="server" Text=""></asp:TextBox>
                    <div class="row">
                        <div class="col-50" style="width: 40%">
                            <label for="DropDownListState"> State on Check </label>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="DropDownListState" errormessage="State on check is required">*</asp:RequiredFieldValidator>
                            <asp:DropDownList ID="DropDownListState" class="inputtext" runat="server">
	                            <asp:ListItem Value="AL">Alabama</asp:ListItem>
	                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
	                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
	                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
	                            <asp:ListItem Value="CA">California</asp:ListItem>
	                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
	                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
	                            <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
	                            <asp:ListItem Value="DE">Delaware</asp:ListItem>
	                            <asp:ListItem Value="FL">Florida</asp:ListItem>
	                            <asp:ListItem Value="GA">Georgia</asp:ListItem>
	                            <asp:ListItem Value="HI">Hawaii</asp:ListItem>
	                            <asp:ListItem Value="ID">Idaho</asp:ListItem>
	                            <asp:ListItem Value="IL">Illinois</asp:ListItem>
	                            <asp:ListItem Value="IN">Indiana</asp:ListItem>
	                            <asp:ListItem Value="IA">Iowa</asp:ListItem>
	                            <asp:ListItem Value="KS">Kansas</asp:ListItem>
	                            <asp:ListItem Value="KY">Kentucky</asp:ListItem>
	                            <asp:ListItem Value="LA">Louisiana</asp:ListItem>
	                            <asp:ListItem Value="ME">Maine</asp:ListItem>
	                            <asp:ListItem Value="MD">Maryland</asp:ListItem>
	                            <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
	                            <asp:ListItem Value="MI">Michigan</asp:ListItem>
	                            <asp:ListItem Value="MN">Minnesota</asp:ListItem>
	                            <asp:ListItem Value="MS">Mississippi</asp:ListItem>
	                            <asp:ListItem Value="MO">Missouri</asp:ListItem>
	                            <asp:ListItem Value="MT">Montana</asp:ListItem>
	                            <asp:ListItem Value="NE">Nebraska</asp:ListItem>
	                            <asp:ListItem Value="NV">Nevada</asp:ListItem>
	                            <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
	                            <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
	                            <asp:ListItem Value="NM">New Mexico</asp:ListItem>
	                            <asp:ListItem Value="NY">New York</asp:ListItem>
	                            <asp:ListItem Value="NC">North Carolina</asp:ListItem>
	                            <asp:ListItem Value="ND">North Dakota</asp:ListItem>
	                            <asp:ListItem Value="OH">Ohio</asp:ListItem>
	                            <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
	                            <asp:ListItem Value="OR">Oregon</asp:ListItem>
	                            <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
	                            <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
	                            <asp:ListItem Value="SC">South Carolina</asp:ListItem>
	                            <asp:ListItem Value="SD">South Dakota</asp:ListItem>
	                            <asp:ListItem Value="TN">Tennessee</asp:ListItem>
	                            <asp:ListItem Value="TX">Texas</asp:ListItem>
	                            <asp:ListItem Value="UT">Utah</asp:ListItem>
	                            <asp:ListItem Value="VT">Vermont</asp:ListItem>
	                            <asp:ListItem Value="VA">Virginia</asp:ListItem>
	                            <asp:ListItem Value="WA">Washington</asp:ListItem>
	                            <asp:ListItem Value="WV">West Virginia</asp:ListItem>
	                            <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
	                            <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                            </asp:DropDownList>                           
                        </div>
                        <div class="col-50" style="width: 38%">
                            <label for="txtZip"> Zip on Check </label>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtZip" errormessage="Zip Code on check is required">*</asp:RequiredFieldValidator>
                            <input placeholder="99999" class="inputtext" type="number" id="txtZip" runat="server" text="" />
                        </div>

                    </div>
                    </div>
                    <div class="col-50">
                        <label for="txtBankName"><i class="fa fa-university"></i> Bank Name on Check </label>
                        <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtBankName" errormessage="Bank Name on check is required">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtBankName" placeholder="Bank of America" cssclass="inputtext" runat="server" Text=""></asp:TextBox>
                        <label for="txtAccountNumber"><i class="fa fa-cog"></i> Account Number on Check </label>
                        <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtAccountNumber" errormessage="Account Number on check is required">*</asp:RequiredFieldValidator>
                        <input type="number" id="txtAccountNumber" placeholder="999999999" class="inputtext" runat="server" text="" />
                        <label for="txtRoutingNumber"><i class="fa fa-code-branch"> </i> Routing Number on Check </label>
                        <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtRoutingNumber" errormessage="RoutingNumber on check is required">*</asp:RequiredFieldValidator>
                        <input type="number" id="txtRoutingNumber" placeholder="999999999" class="inputtext" runat="server" text="" />
                        <label for="txtRoutingNumber"><i class="fa fa-money-check"> </i> Check Number </label>
                        <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtCheckNumber" errormessage="Check Number is required">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtCheckNumber" placeholder="99" cssclass="inputtext" runat="server" Text=""></asp:TextBox>
                    </div>
                </div>
            </div>

            </div>
            <div>
                <br />
            </div>
            <div id="SubmitPanel" class="container">
                <div id="Agreement">
                    <p>
                        I agree to pay the amount indicated above, using the checking account information listed. I understand this amount will be debited from the checking account using a check draft or electronic check debit. I understand that once I press the Submit Payment button below I will not be able to stop the payment from processing.  
                    </p>
                </div>
                <div id="CenteredButton">
                    <asp:Button ID="btnSubmit" class="submit" runat="server" Text="submit" OnClick="btnSubmit_Click1" />
                </div>
            </div>
        </div>
        </div>
    </form>
</body>
</html>
