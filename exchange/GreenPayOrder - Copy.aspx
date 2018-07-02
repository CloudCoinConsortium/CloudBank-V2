<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GreenPayOrder.aspx.cs" Inherits="CloudService.GreenPayOrder" %>

<!DOCTYPE html>
<link href="GPOrder.css" rel="stylesheet" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <div id="PurchaseInfo">
                <div id="PurchaseInfoHeader"><h2>Your Purchase Information:</h2></div>
                <table>
                    <tr>
                        <td>
                            <div class="PIText">Ones: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblOnes" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="PIText">Fives: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblFives" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="PIText">Twenty Fives: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblTwentyFives" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="PIText">Hundreds: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblHundreds" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="PIText">Two Hundred Fifties: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblTwoHundredFifties" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="PIText">Total Coins: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblTotalCoins" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="PIText">Total Price: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:Label ID="lblTotalPrice" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <br />
            </div>
            <div id="PaymentInfo">
                <div id="PaymentInfoHeader"><h2>Your Payment Information:</h2></div>
                <asp:ValidationSummary class="valtext" runat="server" headertext="There were errors on the page:" />
                <table>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtNameOnCheck" errormessage="Name on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">Name on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNameOnCheck" size="30" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtEmailAddress" errormessage="E-Mail Address is required">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" class="valtext" display="Dynamic" controltovalidate="txtEmailAddress" errormessage="E-mail incorrectly formatted" validationexpression="^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$" >*</asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <div class="PIText">Your E-Mail Address: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailAddress" size="40" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>    
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtPhoneNumber" errormessage="Phone Number is required">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" class="valtext" display="Dynamic" controltovalidate="txtPhoneNumber" errormessage="Phone Number incorrectly formatted, use xxx-xxx-xxxx" validationexpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}" >*</asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <div class="PIText">Your Phone Number: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhoneNumber" size="25" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtAddress" errormessage="Address on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">Address on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" size="45" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtCity" errormessage="City on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">City on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCity" size="22" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="DropDownListState" errormessage="State on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">State on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListState" runat="server">
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
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtZip" errormessage="Zip Code on check is required">*</asp:RequiredFieldValidator>
                            
                        </td>
                        <td>
                            <div class="PIText">Zip Code on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <input type="number" id="txtZip" runat="server" text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtBankName" errormessage="Bank Name on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">Bank Name on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtBankName" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtAccountNumber" errormessage="Account Number on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">Account Number on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <input type="number" id="txtAccountNumber" runat="server" text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtRoutingNumber" errormessage="RoutingNumber on check is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">Routing Number on check: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <input type="number" id="txtRoutingNumber" runat="server" text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="spacer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator runat="server" class="valtext" controltovalidate="txtCheckNumber" errormessage="Check Number is required">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <div class="PIText">Check Number: </div>
                        </td>
                        <td>
                            <br />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCheckNumber" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                </table>
                
            </div>
            <div>
                <br />
            </div>
            <div id="SubmitPanel">
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
    </form>
</body>
</html>
