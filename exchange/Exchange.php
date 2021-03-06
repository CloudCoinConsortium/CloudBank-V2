<!DOCTYPE html>
<!-- saved from url=(0043)https://learn.cloudcoin.global/indexbk.aspx -->
<html class="no-js fa-events-icons-ready" lang="en" dir="ltr"><head><meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta http-equiv="x-ua-compatible" content="ie=edge"><meta name="viewport" content="width=device-width, initial-scale=1.0">

<title>Buy Cloud Currencies Educational Package</title>

<link rel="apple-touch-icon" sizes="180x180" href="https://cloudcoinconsortium.com/apple-touch-icon.png">
<link rel="icon" type="image/png" href="https://cloudcoinconsortium.com/favicon-32x32.png" sizes="32x32">
<link rel="icon" type="image/png" href="https://cloudcoinconsortium.com/favicon-16x16.png" sizes="16x16">
<link rel="manifest" href="https://cloudcoinconsortium.com/manifest.json">
<link rel="mask-icon" href="https://cloudcoinconsortium.com/safari-pinned-tab.svg" color="#5bbad5">

<meta name="theme-color" content="#ffffff">

<link rel="stylesheet" href="css/foundation.css">
<link rel="stylesheet" href="css/app.css">
<link href="css/css" rel="stylesheet" type="text/css">

<style type="text/css">
        .grandTotal {
	        text-align: center;
	        horizontal-align: center;
        }
        table {
            border-collapse: collapse;
            width: 100%;
        }

        th, td {
	        background-color: black;
            padding: 8px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }

        tr:hover {background-color:#8e8e8e;}

        #cta select option {
            color: black;
            background-color: purple;
        }

        select option {
            color: #111;
            background-color: #000;
        }
	</style>
		<link href="css/main.css" rel="stylesheet">
		<script src="css/b7a745f5ac.js.download" type=""></script>
		<link href="css/b7a745f5ac.css" media="all" rel="stylesheet"></head>
	<body>
	
	<?php
		$dir = "../accounts/00000000000000000000000000000000/Bank/";
		
		$Price=0.03;
		
		$numberOfOnes = 0;
		$numberOfFives = 0;
		$numberOfTwentyFives = 0;
		$numberOfOneHundreds = 0;
		$numberOfTwoHundredFifties = 0;

				
		$Ones = glob( $dir . "1.*.ForSale*.stack" );
		$Fives = glob( $dir . "5.*.ForSale*.stack" );
		$TwentyFives = glob( $dir . "25.*.ForSale*.stack" );
		$OneHundreds = glob( $dir . "100.*.ForSale*.stack" );
		$TwoHundredFifties = glob( $dir . "250.*.ForSale*.stack" );
		if ($Ones)  {   $numberOfOnes =   count($Ones);}
		if ($Fives)  {   $numberOfFives =   count($Fives);}
		if ($TwentFives)  {   $numberOfTwentyFives =   count($TwentFives);}
		if ($OneHundredss)  {   $numberOfOneHundredss =   count($OneHundreds);}
		if ($TwoHundredFifties)  {   $numberOfTwoHundredFifties =   count($TwoHundredFifties);}

		?>
	
	
<form method="post" action="../service/GreenPayOrder.aspx" id="form1">
	<div class="row" style="max-width:2000px;background-color:#338FFF;">
		<div class="large-12 columns center">
			<h1 style=" font-size: 2.2vw; color:white; font-weight:  bold; text-shadow: 10px 10px 25px #333333; text-size-adjust:1005" class="white text-center">
				Cloud Currency Educational Package 
			</h1>
			<h2 style=" font-size: 1.2vw; color:white; font-weight:  bold; text-shadow: 10px 10px 25px #333333; text-size-adjust:1005" class="white text-center">
				Included in your purchase: EBook, Software, Videos and CloudCoins
			</h2>

		</div>
	 </div>
	<div class="inner">
	<div class="row center" style="max-width:2000px;  background-color:white">
		<!-- //column1 -->
		<div class="large-4 columns" style="padding:40px">
			<h3>eBook: </h3>
			<b>Beyond Bitcoin - The Future of Digital Currency</b>
			<img src="css/beyond_bitcoin.jpg" alt="Beyond Bitcoin BOOK">
			<br>
			<br>
			You will receive a link to the digital version of <i>Beyond Bitcoin: The Future of Digital Currency</i>. This eBook describes what money is, why digital money will dominate the future, the perfect money possible and, the utopian and dystopian potential of digital currencies, how money got started, and how we can protect our right to use money.
		</div>
		<!-- //column2 -->
		<div class="large-4 columns" style="padding:40px">
			<h3>Software: </h3>
			
			<b>CloudCoin Consumer Edition</b><br><br>
			<img src="css/consumeredition.jpg" alt="Consumer Edition">
			<br>
			<br>
			You will receive Consumers Edition Desktop software for Windows. (There are other programs for Mac, Android and Linux). Consumer's Edition
			manages your CloudCoins like a cash register. It Powns (password owns) your new CloudCoin 200 times faster than other software. It stores them safely in your bank folder until you exports them as jpeg files or text files.
		</div>
		<!-- //column3 -->
		<div class="large-4 columns" style="padding:40px">
			<h3>Videos:</h3>
			<b>CloudCoin University</b>
			<img src="css/ccu.webp">
			<br>
			<br>
			Includes links to private videos that can only be seen by link holder.
			Videos include:
				<ul>
				<li>CloudCoin file formats</li>
				<li>Storing CloudCoins</li>
				<li>Transfering CloudCoins</li>
				<li>Exchanging CloudCoins</li>
				<li>Accepting CloudCoins as Payment</li>
				<li>Using CloudCoins as Payment</li>
				</ul>
		</div>
	</div>
	</div>


	<section class="wrapper" id="cta" style="padding-top:20px;background-color:#000000">
	<div class="inner">
	<span id="LBLCurrentPrice" style="font-size:39px;">Current Price Per Coin: <?php echo $Price;?> </span>
	<table>
		<tbody>
		<tr>
			<td></td>
			<th>Packages <p id="demo"></p></th>
			<td>
				<select name="Package" id="Pselect" onchange="Packages()">
				<option value="0">Custom</option>
				<option value="1">Basic</option>
				<option value="2">Silver</option>
				<option value="3">Gold</option>
				<option value="4">Platinum</option>
				<option value="5">Platinum+</option>
				</select>
			</td>
			<td></td>
		</tr>
		
		<script>
		function Packages(){
			var O = document.getElementById("Pselect").value;
		switch(O){
			case 1:
					document.getElementById("DDLOnes").disabled = false;
					document.getElementById("DDLFives").disabled = true;
					document.getElementById("DDLTwentyFives").disabled = true;
					document.getElementById("DDLHundreds").disabled = true;
					document.getElementById("DDLTwoHundredFifties").disabled = true;
					document.getElementById("demo").innerHTML = "You selected: " + O;
			break;
			
			
			
		}
		}

	</script>
		
		
		
		<tr style="padding: 10px 10px 10px 10px;"><!--Begin Headers-->
			<th>CloudCoin</th>
			<th>Denomination</th>
			<th>Amount</th>
			<th>Stock Available</th>
		</tr><!-- end headers -->
		
		
		
		<tr>
			<td><img alt="1 CloudCoin" src="css/cc-1.jpg" width="200"></td>
			<th>1s</th>
			<td>
				<select name="DDLOnes" id="DDLOnes" onchange="totals()">
				<option value="0">0 | 0.00</option>
				<?php
					for ($i=1;$i<=$numberOfOnes;$i++){
						$total= $i * $Price;
						echo "<option value='$i'>$i | $total </option>";					
					}
				?>
				</select>
			</td>
			<td><span id="LBLOnesAvailable"><?php echo $numberOfOnes ?> in stock</span></td>
		</tr>
		
		
		<tr>
			<td><img alt="5 CloudCoin" src="css/cc-5.png" width="200"></td>
			<th>5s</th>
			<td>
				<select name="DDLFives" id="DDLFives" onchange="totals()">
				<option value="0">0 | 0.00</option>
				<?php
					for ($i=1;$i<=$numberOfFives;$i++){
						$total= $i * 5 * $Price;
						$value= $i * 5;
						echo "<option value='$value'>$i | $total </option>";					
					}
				?>
				</select>
			</td>
			<td><span id="LBLFivesAvailable"><?php echo $numberOfFives?> in stock</span></td>
		</tr>
		
		
		<tr>
			<td><img alt="25 CloudCoin" src="css/cc-25.png" width="200"></td>
			<th>25s</th>
			<td>
				<select name="DDLTwentyFives" id="DDLTwentyFives" onchange="totals()" >
				<option value="0">0 | 0.00</option>
				<?php
					for ($i=1;$i<=$numberOfTwentyFives;$i++){
						$total= $i * 25 *$Price;
						$value= $i * 25;
						echo "<option value='$value'>$i | $total </option>";					
					}
				?>
				</select>
			</td>
			<td><span id="LBLTwentyFivesAvailable"><?php echo $numberOfTwentyFives?> in stock</span></td>
		</tr>
		
	
		<tr>
			<td><img alt="100 CloudCoin" src="css/cc-100.png" width="200"></td>
			<th>100s</th>
			<td>
				<select name="DDLHundreds" id="DDLHundreds" onchange="totals()">
				<option value="0">0 | 0.00</option>
				<?php
					for ($i=1;$i<=$numberOfOneHundreds;$i++){
						$total= $i * 100 *$Price;
						$value= $i * 100;
						echo "<option value='$value'>$i | $total </option>";					
					}
				?>
				</select>
			</td>
			<td><span id="LBLHundredsAvailable"><?php echo $numberOfOneHundreds?> in stock</span></td>
		</tr>
		
		
		<tr>
			<td><img alt="250 CloudCoin" src="css/cc-250.png" width="200"></td>
			<th>250s</th>
			<td>
				<select name="DDLTwoHundredFifties" id="DDLTwoHundredFifties" onchange="totals()">
				<option value="0">0 | 0.00</option>
				<?php
					for ($i=1;$i<=$numberOfTwoHundredFifties;$i++){
						$total= $i * 250 * $Price;
						$value= $i * 250;
						echo "<option value='$value'>$i | $total </option>";					
					}
				?>
				</select>
			</td>
			<td><span id="LBLTwoHundredFiftiesAvailable"><?php echo $numberOfTwoHundredFifties?> in stock</span></td>
		</tr>
		
		
		<tr>
			<th colspan="4"><b colspan="4">Grand Total: <span id="grandtotal"></span></b> &nbsp; &nbsp;
			<span id="LBLWarning">Order must be at least $5.00 to process!</span><table border="0" cellpadding="0" cellspacing="2">
	<tbody>
		<tr>
		<td>
		<input type="submit" name="BTNSubmitGreenPay" value="" id="BTNSubmitGreenPay" style="background-image: url(images/PayNowButton.png); width: 157px; height: 55px;">
		</td>

		</tr>
		<tr>

		</tr>
		</tbody>
	</table>

	</div>

	</section>


<footer id="footer">
	<div class="inner">
		<div class="content">
			<section>
				<h3>Extra Info</h3>
				<p>Secure Email Based in Switzerland. <a href="https://protonmail.com/" style="color:white;">Get Protonmail</a><br>
				Help Desk Available between 8am and 5pm PST.<br>
				</p>
			</section>
			
			<section>
				<h4>Contact Us</h4>
				<ul class="alt">
					<li><a href="mailto:CloudCoin.HelpDesk@protonmail.com" style="color:white;font-size:18px;"><u><span class="__cf_email__" style="">CloudCoin.HelpDesk@protonmail.com</span></u></a><br>
					<a style="font-size:18px;" href="tel:1-530-500-2646"><u>1-530-500-2646</u></a></li>
					<li  style="font-size:18px;">To verify this website is a trusted seller of CloudCoin, contact the Help Desk</li>
				</ul>
			</section>
			
			<section>
				<h4>Social Media</h4>
				<ul class="plain">
					<li><a href="https://twitter.com/CloudCoinGlobal"><i class="fa-twitter fa">  &nbsp;</i>Twitter</a></li>
					<li><a href="https://www.facebook.com/cloudcoinconsortium/"><i class="fa-facebook fa">  &nbsp;</i>Facebook</a></li>
					<li><a href="https://github.com/CloudCoinConsortium"><i class="fa fa-github">  &nbsp;</i>Git</a></li>
				</ul>
			</section>
		</div>
		
		<div class="copyright"><a href="http://cloudcoin.global/">CloudCoin.global</a> © 2018</div>
</div>

	</footer>
	<input name="pricepercoin" type="hidden" id="pricepercoin" value="<?php echo $Price?>">
</form>

<script type="text/javascript">
        function totals() {
            var totalCoins = 0;
            var totalPrice = 0;
            var pricePerCoin = parseFloat(pricepercoin.value);

            var ones = parseInt(DDLOnes.value);
            var fives = parseInt(DDLFives.value);
            var twentyfives = parseInt(DDLTwentyFives.value);
            var hundreds = parseInt(DDLHundreds.value);
            var twohundredfifties = parseInt(DDLTwoHundredFifties.value);

            totalCoins = ones + fives + twentyfives + hundreds + twohundredfifties;
            totalPrice = totalCoins * pricePerCoin;
            grandtotal.value = totalPrice;
            grandtotal.innerText = "$" + totalPrice.toFixed(2);
            if (totalPrice > 4.99) {
                LBLWarning.innerText = "";
            }
            else
            {
                LBLWarning.innerText = "Order must be at least $5.00 to process!";
            }
        }



				//<![CDATA[
			var theForm = document.forms['form1'];
				if (!theForm)
					{
					theForm = document.form1;
					}
			function __doPostBack(eventTarget, eventArgument)
				{
					if (!theForm.onsubmit || (theForm.onsubmit() != false)) 
					{
					theForm.__EVENTTARGET.value = eventTarget;
					theForm.__EVENTARGUMENT.value = eventArgument;
					theForm.submit();
					}
				}
		//]]>
			
    </script>

</body></html>